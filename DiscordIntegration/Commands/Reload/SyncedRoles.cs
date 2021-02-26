// -----------------------------------------------------------------------
// <copyright file="SyncedRoles.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.Commands.Reload
{
    using System;
    using API.Commands;
    using CommandSystem;
    using Exiled.Permissions.Extensions;

    /// <summary>
    /// Reloads bot synced roles if connected.
    /// </summary>
    internal sealed class SyncedRoles : ICommand
    {
#pragma warning disable SA1600 // Elements should be documented
        private SyncedRoles()
        {
        }

        public static SyncedRoles Instance { get; } = new SyncedRoles();

        public string Command { get; } = "syncedroles";

        public string[] Aliases { get; } = new[] { "sr", "sync", "synced", "roles" };

        public string Description { get; } = DiscordIntegration.Language.ReloadSyncedRolesDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 0)
            {
                response = string.Format(DiscordIntegration.Language.InvalidParametersError, 0);
                return false;
            }

            if (!sender.CheckPermission("di.reload.syncedroles"))
            {
                response = string.Format(DiscordIntegration.Language.NotEnoughPermissions, "di.reload.syncedroles");
                return false;
            }

            if (!DiscordIntegration.Network.IsConnected)
            {
                response = DiscordIntegration.Language.BotIsNotConnectedError;
                return false;
            }

            DiscordIntegration.Instance.SyncedUsersCache.Clear();

            _ = DiscordIntegration.Network.SendAsync(new RemoteCommand("loadSyncedRoles", sender.GetCompatible()));

            response = DiscordIntegration.Language.ReloadSyncedRolesSuccess;
            return true;
        }
    }
}
