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
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using API.Commands;
    using API.EventArgs.Network;
    using API.User;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
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

                Log.Debug($"[NET] {string.Format(Language.HandlingRemoteCommand, remoteCommand.Action, remoteCommand.Parameters[0], Network.TcpClient?.Client?.RemoteEndPoint)}", Instance.Config.IsDebugEnabled);

                switch (remoteCommand.Action)
                {
                    case "executeCommand":
                        JsonConvert.DeserializeObject<GameCommand>(remoteCommand.Parameters[0].ToString())?.Execute();
                        break;
                    case "playerList":
                        string description = null;
                        IList<Field> fields = new List<Field>();
                        TimeSpan duration = Round.ElapsedTime;
                        string seconds = duration.Seconds < 10 ? $"0{duration.Seconds}" : duration.Seconds.ToString();
                        string minutes = duration.Minutes < 10 ? $"0{duration.Minutes}" : duration.Minutes.ToString();
                        var list = Player.List.OrderBy(pl => pl.Id);
                        if(list.Count > 25) {
                            list = list.Where((p, i) => i >= (list.Count - 25)); //saltear x cantidad en el comienzo
                            //cambiar description por lo q vos quieras
                        }
                        foreach (Player ply in list)
                        {
                            if (ply.RemoteAdminAccess)
                            {
                                fields.Add(new Field($"{ply.Id} - {ply.Nickname} - STAFF", ply.Role.Translate(), false));
                            }
                            else if (ply.CheckPermission("cerberus.viplist"))
                            {
                                fields.Add(new Field($"{ply.Id} - {ply.Nickname} - VIP", ply.Role.Translate(), false));
                            }
                            else if (ply.CheckPermission("cerberus.donadorlist"))
                            {
                                fields.Add(new Field($"{ply.Id} - {ply.Nickname} - DONADOR | Un capo el pibe", ply.Role.Translate(), false));
                            }
                            else
                            {
                                fields.Add(new Field($"{ply.Id} - {ply.Nickname}", ply.Role.Translate(), false));
                            }
                        }
                        

                        Network.SendAsync(new RemoteCommand(
                            "sendEmbed",
                            JsonConvert.DeserializeObject<GameCommand>(remoteCommand.Parameters[0].ToString())?.ChannelId,
                            $"-- Jugadores {Player.Dictionary.Count}/{Instance.Slots} -- Tiempo de la ronda {minutes}:{seconds}", description, fields));
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
                Log.Error($"[NET] {string.Format(Language.HandlingRemoteCommandError, Instance.Config.IsDebugEnabled ? exception.ToString() : exception.Message)}");
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

            ev.IPAddress = ipAddress;
            ev.Port = Instance.Config.Bot.Port;
            ev.ReconnectionInterval = TimeSpan.FromSeconds(Instance.Config.Bot.ReconnectionInterval);

            Log.Warn($"[NET] {string.Format(Language.ConnectingTo, ev.IPAddress, ev.Port)}");
        }

        /// <inheritdoc cref="API.Network.OnConnected(object, System.EventArgs)"/>
        public async void OnConnected(object _, System.EventArgs ev)
        {
            Log.Info($"[NET] {string.Format(Language.SuccessfullyConnected, Network.IPEndPoint?.Address, Network.IPEndPoint?.Port)}");

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
                Log.Warn($"[NET] {Language.ServerHasBeenTerminated}");
        }
    }
}
