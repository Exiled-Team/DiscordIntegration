// -----------------------------------------------------------------------
// <copyright file="User.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.Commands.Remove
{
    using System;
    using API.Commands;
    using CommandSystem;
    using Exiled.Permissions.Extensions;

    /// <summary>
    /// Removes an userID-discordID pair from the SyncedRole list.
    /// </summary>
    internal sealed class User : ICommand
    {
#pragma warning disable SA1600 // Elements should be documented
        private User()
        {
        }

        public static User Instance { get; } = new User();

        public string Command { get; } = "user";

        public string[] Aliases { get; } = new[] { "u" };

        public string Description { get; } = DiscordIntegration.Language.RemoveUserCommandDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 1)
            {
                response = string.Format(DiscordIntegration.Language.InvalidParametersError, 1);
                return false;
            }

            if (!sender.CheckPermission("di.remove.user"))
            {
                response = string.Format(DiscordIntegration.Language.NotEnoughPermissions, "di.remove.user");
                return false;
            }

            if (!DiscordIntegration.Network.IsConnected)
            {
                response = DiscordIntegration.Language.BotIsNotConnectedError;
                return false;
            }

            if (!arguments.At(0).IsValidUserId())
            {
                response = string.Format(DiscordIntegration.Language.InvalidUserdIdError, arguments.At(0));
                return false;
            }

            DiscordIntegration.Instance.SyncedUsersCache.RemoveWhere(syncedUser => syncedUser.Id == arguments.At(0));

            _ = DiscordIntegration.Network.SendAsync(new RemoteCommand("removeUser", arguments.At(0), sender.GetCompatible()));

            response = DiscordIntegration.Language.RemoveUserCommandSuccess;
            return false;
        }
    }
}
