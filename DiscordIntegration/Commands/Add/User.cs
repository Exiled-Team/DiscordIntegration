// -----------------------------------------------------------------------
// <copyright file="User.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.Commands.Add
{
    using System;
    using API.Commands;
    using CommandSystem;
    using Exiled.Permissions.Extensions;

    /// <summary>
    /// Adds an userID-discordID pair to the SyncedRole list.
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

        public string Description { get; } = DiscordIntegration.Language.AddUserCommandDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 2)
            {
                response = string.Format(DiscordIntegration.Language.InvalidParametersError, 2);
                return false;
            }

            if (!sender.CheckPermission("di.add.user"))
            {
                response = string.Format(DiscordIntegration.Language.NotEnoughPermissions, "di.add.user");
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

            if (!arguments.At(1).IsValidDiscordId())
            {
                response = string.Format(DiscordIntegration.Language.InvalidDiscordIdError, arguments.At(1));
                return false;
            }

            _ = DiscordIntegration.Network.SendAsync(new RemoteCommand("addUser", arguments.At(0), arguments.At(1), sender.GetCompatible()));

            response = DiscordIntegration.Language.AddUserCommandSuccess;
            return false;
        }
    }
}
