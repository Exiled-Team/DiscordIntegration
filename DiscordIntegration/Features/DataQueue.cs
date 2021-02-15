// -----------------------------------------------------------------------
// <copyright file="DataQueue.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.Features
{
    using System;
    using System.Collections.Generic;
    using Exiled.API.Features;
    using Features.Commands;
    using MEC;
    using Newtonsoft.Json;
    using static DiscordIntegration;

    /// <summary>
    /// Handles data received by the server.
    /// </summary>
    internal static class DataQueue
    {
        /// <summary>
        /// Handles the data queue.
        /// </summary>
        public static void Handle()
        {
            while (Network.DataQueue.TryDequeue(out string data))
            {
                RemoteCommand remoteCommand = JsonConvert.DeserializeObject<RemoteCommand>(data, DiscordIntegration.JsonSerializerSettings);

                Log.Debug($"[NET] {string.Format(DiscordIntegration.Language.HandlingQueueItem, remoteCommand.Action, remoteCommand.Parameters[0], Network.TcpClient?.Client?.RemoteEndPoint)}", Instance.Config.IsDebugEnabled);

                switch (remoteCommand.Action)
                {
                    case "executeCommand":
                        JsonConvert.DeserializeObject<GameCommand>(remoteCommand.Parameters[0].ToString())?.Execute();
                        break;
                    case "setGroupFromId":
                        SyncedUser syncedUser = JsonConvert.DeserializeObject<SyncedUser>(remoteCommand.Parameters[0].ToString(), DiscordIntegration.JsonSerializerSettings);

                        if (syncedUser == null)
                            break;

                        if (!Instance.SyncedUsersCache.Contains(syncedUser))
                            Instance.SyncedUsersCache.Add(syncedUser);

                        syncedUser?.SetGroup();
                        break;
                    case "commandReply":
                        JsonConvert.DeserializeObject<CommandReply>(remoteCommand.Parameters[0].ToString(), DiscordIntegration.JsonSerializerSettings)?.Answer();
                        break;
                }
            }
        }

        /// <summary>
        /// Updates the data queue through an infinite loop.
        /// </summary>
        /// <returns>Wait for the coroutine to finish.</returns>
        public static IEnumerator<float> Update()
        {
            while (true)
            {
                try
                {
                    if (!Network.DataQueue.IsEmpty)
                        Handle();
                }
                catch (Exception exception)
                {
                    Log.Error($"[NET] {string.Format(DiscordIntegration.Language.HandlingQueueError, exception)}");
                }

                yield return Timing.WaitForSeconds(1f);
            }
        }
    }
}