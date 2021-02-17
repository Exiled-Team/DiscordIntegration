// -----------------------------------------------------------------------
// <copyright file="Network.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Exiled.API.Features;
    using Features.Commands;
    using Newtonsoft.Json;
    using static DiscordIntegration;

    /// <summary>
    /// Handles the Network.
    /// </summary>
    public static class Network
    {
        /// <summary>
        /// The queue of received data from the server.
        /// </summary>
        public static readonly ConcurrentQueue<string> DataQueue = new ConcurrentQueue<string>();

        private static CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Gets the active <see cref="TcpClient"/> instance.
        /// </summary>
        public static TcpClient TcpClient { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="TcpClient"/> is connected or not.
        /// </summary>
        public static bool IsConnected => TcpClient?.Connected ?? false;

        /// <summary>
        /// Starts the <see cref="TcpClient"/>.
        /// </summary>
        /// <returns>Returns the <see cref="Task"/>.</returns>
        public static async Task Start()
        {
            if (!cancellationTokenSource?.IsCancellationRequested ?? false)
                return;

            cancellationTokenSource = new CancellationTokenSource();

            TcpClient = new TcpClient();

            await Update().ContinueWith(task => Log.Warn(Language.ServerHasBeenClosed)).ConfigureAwait(false);
        }

        /// <summary>
        /// Disconnectes the <see cref="TcpClient"/>.
        /// </summary>
        public static void Disconnect()
        {
            if (!cancellationTokenSource?.IsCancellationRequested ?? false)
                cancellationTokenSource.Cancel();

            TcpClient.Close();
        }

        /// <summary>
        /// Sends data async to the server.
        /// </summary>
        /// <typeparam name="T">The data type.</typeparam>
        /// <param name="data">The data to be sent.</param>
        /// <returns>Returns the <see cref="ValueTask"/>.</returns>
        public static async ValueTask SendAsync<T>(T data)
        {
            try
            {
                if (!IsConnected)
                    return;

                if (!(data is string serializedObject))
                    serializedObject = JsonConvert.SerializeObject(data, DiscordIntegration.JsonSerializerSettings);

                byte[] bytesToSend = Encoding.ASCII.GetBytes(serializedObject + '\0');

                await TcpClient.GetStream().WriteAsync(bytesToSend, 0, bytesToSend.Length, cancellationTokenSource.Token);

                Log.Debug(string.Format(Language.SentData, serializedObject, bytesToSend.Length), Instance.Config.IsDebugEnabled);
            }
            catch (Exception exception) when (exception.GetType() != typeof(OperationCanceledException))
            {
                Log.Error($"[NET] {string.Format(Language.SendingDataError, Instance.Config.IsDebugEnabled ? exception.ToString() : exception.Message)}");
            }
        }

        private static async Task ReceiveAsync()
        {
            StringBuilder allReceivedData = new StringBuilder();
            byte[] buffer = new byte[256];

            while (true)
            {
                Task<int> readTask = TcpClient.GetStream().ReadAsync(buffer, 0, buffer.Length, cancellationTokenSource.Token);

                await Task.WhenAny(readTask, Task.Delay(Timeout.Infinite, cancellationTokenSource.Token));

                cancellationTokenSource.Token.ThrowIfCancellationRequested();

                int bytesRead = await readTask;

                if (bytesRead > 0)
                {
                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    if (receivedData.IndexOf('\0') != -1)
                    {
                        foreach (var splittedData in receivedData.Split('\0'))
                        {
                            if (allReceivedData.Length > 0)
                            {
                                DataQueue.Enqueue(allReceivedData.ToString() + splittedData);

                                allReceivedData.Clear();
                            }
                            else if (!string.IsNullOrEmpty(splittedData))
                            {
                                DataQueue.Enqueue(splittedData);
                            }
                        }
                    }
                    else
                    {
                        allReceivedData.Append(receivedData);
                    }

                    Log.Debug($"[NET] {string.Format(Language.ReceivedData, receivedData, bytesRead)}", Instance.Config.IsDebugEnabled);
                }

                if (bytesRead == 0 && allReceivedData.Length > 1)
                    DataQueue.Enqueue(allReceivedData.ToString());
            }
        }

        private static async Task Update()
        {
            while (true)
            {
                try
                {
                    Log.Debug($"[NET] {string.Format(Language.TryingToConnect, Instance.Config.Bot.IPAddress, Instance.Config.Bot.Port)}", Instance.Config.IsDebugEnabled);

                    await TcpClient.ConnectAsync(Instance.Config.Bot.IPAddress, Instance.Config.Bot.Port);

                    if (IsConnected)
                    {
                        Log.Info($"[NET] {string.Format(Language.SuccessfullyConnected, TcpClient.Client.RemoteEndPoint)}");

                        await SendAsync(new RemoteCommand("log", "gameEvents", Language.ServerConnected, true));
                        await ReceiveAsync();
                    }
                }
                catch (IOException ioException)
                {
                    Log.Error($"[NET] {string.Format(Language.ReceivingDataError, Instance.Config.IsDebugEnabled ? ioException.ToString() : ioException.Message)}");

                    TcpClient = new TcpClient();
                }
                catch (SocketException socketException) when (socketException.ErrorCode == 10061)
                {
                    Log.Error($"[NET] {string.Format(Language.TryingToConnectError, Instance.Config.IsDebugEnabled ? socketException.ToString() : socketException.Message)}");
                }
                catch (Exception exception)
                {
                    Log.Error($"[NET] {string.Format(Language.GenericNetworkError, Instance.Config.IsDebugEnabled ? exception.ToString() : exception.Message)}");

                    TcpClient = new TcpClient();
                }

                cancellationTokenSource.Token.ThrowIfCancellationRequested();

                await Task.Delay(TimeSpan.FromSeconds(Instance.Config.Bot.ReconnectionInterval), cancellationTokenSource.Token);
            }
        }
    }
}