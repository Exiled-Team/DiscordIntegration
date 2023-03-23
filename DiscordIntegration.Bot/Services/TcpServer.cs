// -----------------------------------------------------------------------
// <copyright file="Network.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.Bot.Services
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Discord;

    using DiscordIntegration.API.EventArgs.Network;
    using DiscordIntegration.Dependency;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// A client that sends JSON serialized strings to a connected server.
    /// </summary>
    public class TcpServer : IDisposable
    {
        /// <summary>
        /// The reception buffer length.
        /// </summary>
        public const int ReceptionBuffer = 256;

        private bool isDisposed;
        private Bot bot;
        private TcpListener Listener = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="Network"/> class.
        /// </summary>
        /// <param name="ipAddress">The remote server IP address.</param>
        /// <param name="port">The remote server IP port.</param>
        public TcpServer(string ipAddress, ushort port, Bot bot)
            : this(new IPEndPoint(IPAddress.Parse(ipAddress), port), bot)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Network"/> class.
        /// </summary>
        /// <param name="ipEndPoint">The remote server IP address and port.</param>
        /// <param name="bot">The <see cref="Bot"/> instance.</param>
        public TcpServer(IPEndPoint? ipEndPoint, Bot bot)
        {
            IPEndPoint = ipEndPoint;
            this.bot = bot;
            if (ipEndPoint != null) 
                Listener = new TcpListener(ipEndPoint);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Network"/> class.
        /// </summary>
        ~TcpServer() => Dispose(false);

        /// <summary>
        /// Invoked after network received partial data.
        /// </summary>
        public event EventHandler<ReceivedPartialEventArgs> ReceivedPartial = null!;

        /// <summary>
        /// Invoked after network received full data.
        /// </summary>
        public event EventHandler<ReceivedFullEventArgs> ReceivedFull = null!;

        /// <summary>
        /// Invoked after the network thrown an exception while sending data.
        /// </summary>
        public event EventHandler<SendingErrorEventArgs> SendingError = null!;

        /// <summary>
        /// Invoked after the network thrown an exception while receiving data.
        /// </summary>
        public event EventHandler<ReceivingErrorEventArgs> ReceivingError = null!;

        /// <summary>
        /// Invoked after network sent data.
        /// </summary>
        public event EventHandler<SentEventArgs> Sent = null!;

        /// <summary>
        /// Invoked before the network connects to the server.
        /// </summary>
        public event EventHandler<ConnectingEventArgs> Connecting = null!;

        /// <summary>
        /// Invoked after successfully connecting to the server.
        /// </summary>
        public event EventHandler<ConnectingErrorEventArgs> ConnectingError = null!;

        /// <summary>
        /// Invoked after the network successfully connects to the server.
        /// </summary>
        public event EventHandler Connected = null!;

        /// <summary>
        /// Invoked after the network thrown an exception while updating the connection.
        /// </summary>
        public event EventHandler<UpdatingConnectionErrorEventArgs> UpdatingConnectionError = null!;

        /// <summary>
        /// Invoked after the network termination.
        /// </summary>
        public event EventHandler<TerminatedEventArgs> Terminated = null!;

        /// <summary>
        /// Gets the active <see cref="System.Net.Sockets.TcpClient"/> instance.
        /// </summary>
        public TcpClient? TcpClient { get; private set; } = null!;

        /// <summary>
        /// Gets the IP end point to connect with.
        /// </summary>
        public IPEndPoint? IPEndPoint { get; private set; } = null!;

        /// <summary>
        /// Gets a value indicating whether the <see cref="TcpClient"/> is connected or not.
        /// </summary>
        public bool IsConnected => TcpClient?.Connected ?? false;

        /// <summary>
        /// Gets the <see cref="Newtonsoft.Json.JsonSerializerSettings"/> instance.
        /// </summary>
        public JsonSerializerSettings JsonSerializerSettings { get; } = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            TypeNameHandling = TypeNameHandling.Objects,
        };

        /// <summary>
        /// Starts the <see cref="TcpClient"/>.
        /// </summary>
        /// <returns>Returns the <see cref="Network"/> <see cref="Task"/>.</returns>
        public async Task Start() => await Start(CancellationToken.None);

        /// <summary>
        /// Starts the <see cref="TcpClient"/>.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="Task"/> cancellation token.</param>
        /// <returns>Returns the <see cref="Network"/> <see cref="Task"/>.</returns>
        public async Task Start(CancellationToken cancellationToken)
        {
            if (isDisposed)
                throw new ObjectDisposedException(GetType().FullName);

            await bot.Client.SetStatusAsync(UserStatus.DoNotDisturb);
            await bot.Client.SetActivityAsync(new Game("Waiting for connection..."));
            await Update(cancellationToken).ContinueWith(task => OnTerminated(this, new TerminatedEventArgs(task)), cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Close the <see cref="TcpClient"/> and the <see cref="Network"/>.
        /// </summary>
        public void Close() => Dispose();

        /// <summary>
        /// Sends data async to the server.
        /// </summary>
        /// <typeparam name="T">he data type.</typeparam>
        /// <param name="data">The data to be sent.</param>
        /// <returns>Returns the <see cref="ValueTask"/>.</returns>
        public async ValueTask SendAsync<T>(T data) => await SendAsync(data, CancellationToken.None);

        /// <summary>
        /// Sends data async to the server.
        /// </summary>
        /// <typeparam name="T">The data type.</typeparam>
        /// <param name="data">The data to be sent.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the <see cref="ValueTask"/>.</returns>
        public async ValueTask SendAsync<T>(T data, CancellationToken cancellationToken)
        {
            try
            {
                if (!IsConnected)
                {
                    Log.Debug(bot.ServerNumber, nameof(SendAsync), "Sending aborted, not connected.");
                    return;
                }
                

                string serializedObject = JsonConvert.SerializeObject(data, JsonSerializerSettings);

                Log.Debug(bot.ServerNumber, nameof(SendAsync), $"Sending {serializedObject}");
                byte[] bytesToSend = Encoding.UTF8.GetBytes(serializedObject + '\0');

                await TcpClient?.GetStream().WriteAsync(bytesToSend, 0, bytesToSend.Length, cancellationToken)!;

                OnSent(this, new SentEventArgs(serializedObject, bytesToSend.Length));
            }
            catch (Exception exception) when (exception.GetType() != typeof(OperationCanceledException))
            {
                OnSendingError(this, new SendingErrorEventArgs(exception));
            }
        }

        /// <summary>
        /// Releases all resources.
        /// </summary>
        public void Dispose() => Dispose(true);

        /// <summary>
        /// Releases unmanaged resources and, optionally, managed ones.
        /// </summary>
        /// <param name="shouldDisposeAllResources">Indicates whether all resources should be disposed or only unmanaged ones.</param>
        protected virtual void Dispose(bool shouldDisposeAllResources)
        {
            if (shouldDisposeAllResources)
            {
                TcpClient?.Dispose();
                TcpClient = null;

                IPEndPoint = null;
            }

            isDisposed = true;

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Called after network received full data.
        /// </summary>
        /// <param name="sender">The sender instance.</param>
        /// <param name="ev">The <see cref="ReceivedFullEventArgs"/> instance.</param>
        protected virtual void OnReceivedFull(object sender, ReceivedFullEventArgs ev) => ReceivedFull?.Invoke(sender, ev);

        /// <summary>
        /// Called after network received partial data.
        /// </summary>
        /// <param name="sender">The sender instance.</param>
        /// <param name="ev">The <see cref="ReceivedPartialEventArgs"/> instance.</param>
        protected virtual void OnReceivedPartial(object sender, ReceivedPartialEventArgs ev) => ReceivedPartial?.Invoke(sender, ev);

        /// <summary>
        /// Called after the network thrown an exception while sending data.
        /// </summary>
        /// <param name="sender">The sender instance.</param>
        /// <param name="ev">The <see cref="SendingErrorEventArgs"/> instance.</param>
        protected virtual void OnSendingError(object sender, SendingErrorEventArgs ev) => SendingError?.Invoke(sender, ev);

        /// <summary>
        /// Called after the network thrown an exception while receiving data.
        /// </summary>
        /// <param name="sender">The sender instance.</param>
        /// <param name="ev">The <see cref="ReceivingErrorEventArgs"/> instance.</param>
        protected virtual void OnReceivingError(object sender, ReceivingErrorEventArgs ev) => ReceivingError?.Invoke(sender, ev);

        /// <summary>
        /// Called after network sent data.
        /// </summary>
        /// <param name="sender">The sender instance.</param>
        /// <param name="ev">The <see cref="SentEventArgs"/> instance.</param>
        protected virtual void OnSent(object sender, SentEventArgs ev) => Sent?.Invoke(sender, ev);

        /// <summary>
        /// Called before the network connects to the server.
        /// </summary>
        /// <param name="sender">The sender instance.</param>
        /// <param name="ev">The <see cref="ConnectingEventArgs"/> instance.</param>
        protected virtual void OnConnecting(object sender, ConnectingEventArgs ev) => Connecting?.Invoke(sender, ev);

        /// <summary>
        /// Called after the network thrown an exception while sending data.
        /// </summary>
        /// <param name="sender">The sender instance.</param>
        /// <param name="ev">The <see cref="ConnectingErrorEventArgs"/> instance.</param>
        protected virtual void OnConnectingError(object sender, ConnectingErrorEventArgs ev) => ConnectingError?.Invoke(sender, ev);

        /// <summary>
        /// Called after the network successfully connects to the server.
        /// </summary>
        /// <param name="sender">The sender instance.</param>
        /// <param name="ev">The <see cref="System.EventArgs"/> instance.</param>
        protected virtual void OnConnected(object sender, EventArgs ev) => Connected?.Invoke(sender, ev);

        /// <summary>
        /// Called after the network thrown an exception while updating the connection.
        /// </summary>
        /// <param name="sender">The sender instance.</param>
        /// <param name="ev">The <see cref="UpdatingConnectionErrorEventArgs"/> instance.</param>
        protected virtual void OnUpdatingConnectionError(object sender, UpdatingConnectionErrorEventArgs ev) => UpdatingConnectionError?.Invoke(sender, ev);

        /// <summary>
        /// Called after the network termination.
        /// </summary>
        /// <param name="sender">The sender instance.</param>
        /// <param name="ev">The <see cref="TerminatedEventArgs"/> instance.</param>
        protected virtual void OnTerminated(object sender, TerminatedEventArgs ev) => Terminated?.Invoke(sender, ev);

        private async Task ReceiveAsync(CancellationTokenSource cancellationToken)
        {
            StringBuilder totalReceivedData = new();
            byte[] buffer = new byte[ReceptionBuffer];

            if (TcpClient is null)
            {
                Log.Debug(bot.ServerNumber, nameof(ReceiveAsync), "Client is null, aborting.");
                return;
            }

            try
            {
                await SendAsync("heartbeat", cancellationToken.Token);
            }
            catch (Exception e)
            {
                Log.Error(bot.ServerNumber, nameof(ReceiveAsync), $"{TcpClient.Client.RemoteEndPoint} has disconnected.\n{e.Message}");
                TcpClient.Dispose();
                cancellationToken.Cancel();
                return;
            }

            while (true)
            {
                try
                {
                    read:
                    Task<int> readTask = TcpClient.GetStream().ReadAsync(buffer, 0, buffer.Length, cancellationToken.Token);
                    
                    await Task.WhenAny(readTask, Task.Delay(1000, cancellationToken.Token)).ConfigureAwait(false);

                    cancellationToken.Token.ThrowIfCancellationRequested();

                    int bytesRead = await readTask;
                    
                    try
                    {
                        await SendAsync("heartbeat", cancellationToken.Token);
                    }
                    catch (Exception e)
                    {
                        Log.Error(bot.ServerNumber, nameof(ReceiveAsync), $"{TcpClient.Client.RemoteEndPoint} has disconnected.\n{e.Message}");
                        cancellationToken.Cancel();
                        TcpClient.Dispose();
                        await bot.Client.SetStatusAsync(UserStatus.DoNotDisturb);
                        await bot.Client.SetActivityAsync(new Game("Waiting for connection..."));
                        break;
                    }
                    
                    if (bytesRead == 0)
                        goto read;

                    if (bytesRead > 0)
                    {
                        string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        if (receivedData.IndexOf('\0') != -1)
                        {
                            foreach (string splitData in receivedData.Split('\0'))
                            {
                                if (splitData is "\"heartbeat\"" or "heartbeat")
                                    continue;
                                if (totalReceivedData.Length > 0)
                                {
                                    try
                                    {
                                        _ = JsonConvert.DeserializeObject<RemoteCommand>(totalReceivedData + splitData)!;
                                    }
                                    catch (Exception)
                                    {
                                        Log.Error(bot.ServerNumber, nameof(ReceiveAsync), $"Received unusable data. Clearing buffer.");
                                        Log.Error(bot.ServerNumber, nameof(ReceiveAsync), receivedData);
                                        totalReceivedData.Clear();
                                        continue;
                                    }

                                    OnReceivedFull(this, new ReceivedFullEventArgs(totalReceivedData + splitData, bytesRead));

                                    totalReceivedData.Clear();
                                }
                                else if (!string.IsNullOrEmpty(splitData))
                                {
                                    try
                                    {
                                        _ = JsonConvert.DeserializeObject<RemoteCommand>(totalReceivedData + splitData)!;
                                    }
                                    catch (Exception)
                                    {
                                        totalReceivedData.Append(splitData);
                                        continue;
                                    }

                                    OnReceivedFull(this, new ReceivedFullEventArgs(splitData, bytesRead));

                                    totalReceivedData.Clear();
                                }
                            }
                        }
                        else
                        {
                            OnReceivedPartial(this, new ReceivedPartialEventArgs(receivedData, bytesRead));

                            totalReceivedData.Append(receivedData);
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error(bot.ServerNumber, nameof(ReceiveAsync), e.Message);
                    cancellationToken.Cancel();
                    TcpClient.Dispose();
                    await bot.Client.SetStatusAsync(UserStatus.DoNotDisturb);
                    await bot.Client.SetActivityAsync(new Game("Waiting for connection..."));
                    break;
                }
            }
        }

        private async Task Update(CancellationToken cancellationToken)
        {
            while (true)
            {
                try
                {
                    ConnectingEventArgs ev = new(IPEndPoint!.Address, (ushort)IPEndPoint.Port);

                    OnConnecting(this, ev);

                    IPEndPoint.Address = ev.IPAddress;
                    IPEndPoint.Port = ev.Port;

                    Listener.Start();
                    TcpClient = await Listener.AcceptTcpClientAsync(cancellationToken);

                    OnConnected(this, EventArgs.Empty);

                    await ReceiveAsync(new CancellationTokenSource());
                }
                catch (IOException ioException)
                {
                    OnReceivingError(this, new ReceivingErrorEventArgs(ioException));
                }
                catch (SocketException socketException) when (socketException.ErrorCode == 10061)
                {
                    OnConnectingError(this, new ConnectingErrorEventArgs(socketException));
                }
                catch (Exception exception) when (exception.GetType() != typeof(OperationCanceledException))
                {
                    OnUpdatingConnectionError(this, new UpdatingConnectionErrorEventArgs(exception));
                }

                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }
}