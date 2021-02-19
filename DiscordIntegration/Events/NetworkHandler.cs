// -----------------------------------------------------------------------
// <copyright file="NetworkHandler.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.Events
{
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
    using System;
    using System.Net;
    using API.Commands;
    using API.EventArgs.Network;
    using API.User;
    using Exiled.API.Features;
    using Newtonsoft.Json;
    using static DiscordIntegration;

    /// <summary>
    /// Handles network-related events.
    /// </summary>
    internal sealed class NetworkHandler
    {
        /// <inheritdoc cref="API.Network.OnReceivedFull(object, ReceivedFullEventArgs)"/>
        public void OnReceivedFull(object _, ReceivedFullEventArgs ev)
        {
            try
            {
                Log.Debug($"[NET] {string.Format(Language.ReceivedData, ev.Data, ev.Length)}", Instance.Config.IsDebugEnabled);

                RemoteCommand remoteCommand = JsonConvert.DeserializeObject<RemoteCommand>(ev.Data, Network.JsonSerializerSettings);

                Log.Debug($"[NET] {string.Format(Language.HandlingQueueItem, remoteCommand.Action, remoteCommand.Parameters[0], Network.TcpClient?.Client?.RemoteEndPoint)}", Instance.Config.IsDebugEnabled);

                switch (remoteCommand.Action)
                {
                    case "executeCommand":
                        JsonConvert.DeserializeObject<GameCommand>(remoteCommand.Parameters[0].ToString())?.Execute();
                        break;
                    case "setGroupFromId":
                        SyncedUser syncedUser = JsonConvert.DeserializeObject<SyncedUser>(remoteCommand.Parameters[0].ToString(), Network.JsonSerializerSettings);

                        if (syncedUser == null)
                            break;

                        if (!Instance.SyncedUsersCache.Contains(syncedUser))
                            Instance.SyncedUsersCache.Add(syncedUser);

                        syncedUser?.SetGroup();
                        break;
                    case "commandReply":
                        JsonConvert.DeserializeObject<CommandReply>(remoteCommand.Parameters[0].ToString(), Network.JsonSerializerSettings)?.Answer();
                        break;
                }
            }
            catch (Exception exception)
            {
                Log.Error($"[NET] {string.Format(Language.ReceivingDataError, Instance.Config.IsDebugEnabled ? exception.ToString() : exception.Message)}");
            }
        }

        /// <inheritdoc cref="API.Network.OnSendingError(object, SendingErrorEventArgs)"/>
        public void OnSendingError(object _, SendingErrorEventArgs ev)
        {
            Log.Error($"[NET] {string.Format(Language.SendingDataError, Instance.Config.IsDebugEnabled ? ev.Exception.ToString() : ev.Exception.Message)}");
        }

        /// <inheritdoc cref="API.Network.OnReceivingError(object, ReceivingErrorEventArgs)"/>
        public void OnReceivingError(object _, ReceivingErrorEventArgs ev)
        {
            Log.Error($"[NET] {string.Format(Language.ReceivingDataError, Instance.Config.IsDebugEnabled ? ev.Exception.ToString() : ev.Exception.Message)}");
        }

        /// <inheritdoc cref="API.Network.OnSent(object, SentEventArgs)"/>
        public void OnSent(object _, SentEventArgs ev) => Log.Debug(string.Format(Language.SentData, ev.Data, ev.Length), Instance.Config.IsDebugEnabled);

        /// <inheritdoc cref="API.Network.OnConnecting(object, ConnectingEventArgs)"/>
        public void OnConnecting(object _, ConnectingEventArgs ev)
        {
            if (!IPAddress.TryParse(Instance.Config?.Bot?.IPAddress, out IPAddress ipAddress))
            {
                Log.Error($"[NET] {string.Format(Language.InvalidIPAddress, Instance.Config?.Bot?.IPAddress)}");
                return;
            }

            ev.IPEndPoint = new IPEndPoint(ipAddress, Instance.Config?.Bot?.Port ?? 0);
            ev.ReconnectionInterval = TimeSpan.FromSeconds(Instance.Config.Bot.ReconnectionInterval);

            Log.Warn($"[NET] {string.Format(Language.ConnectingTo, Network.IPEndPoint.Address, Network.IPEndPoint.Port)}");
        }

        /// <inheritdoc cref="API.Network.OnConnected(object, System.EventArgs)"/>
        public async void OnConnected(object _, System.EventArgs ev)
        {
            Log.Info($"[NET] {string.Format(Language.SuccessfullyConnected, Network.IPEndPoint)}");

            await Network.SendAsync(new RemoteCommand("log", "gameEvents", Language.ServerConnected, true));
        }

        /// <inheritdoc cref="API.Network.OnConnectingError(object, ConnectingErrorEventArgs)"/>
        public void OnConnectingError(object _, ConnectingErrorEventArgs ev)
        {
            Log.Error($"[NET] {string.Format(Language.ConnectingError, Instance.Config.IsDebugEnabled ? ev.Exception.ToString() : ev.Exception.Message)}");
        }

        /// <inheritdoc cref="API.Network.OnConnectingError(object, ConnectingErrorEventArgs)"/>
        public void OnUpdatingConnectionError(object _, UpdatingConnectionErrorEventArgs ev)
        {
            Log.Error($"[NET] {string.Format(Language.UpdatingConnectionError, Instance.Config.IsDebugEnabled ? ev.Exception.ToString() : ev.Exception.Message)}");
        }

        /// <inheritdoc cref="API.Network.OnTerminated(object, TerminatedEventArgs)"/>
        public void OnTerminated(object _, TerminatedEventArgs ev)
        {
            if (ev.Task.IsFaulted)
                Log.Error($"[NET] {string.Format(Language.ServerHasBeenTerminatedWithErrors, Instance.Config.IsDebugEnabled ? ev.Task.Exception.ToString() : ev.Task.Exception.Message)}");
            else
                Log.Error($"[NET] {Language.ServerHasBeenTerminated}");
        }
    }
}
