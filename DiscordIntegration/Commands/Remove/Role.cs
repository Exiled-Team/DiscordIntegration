// -----------------------------------------------------------------------
// <copyright file="Role.cs" company="Exiled Team">
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
    /// Removes a role-group pair from the SyncedRole list.
    /// </summary>
    internal sealed class Role : ICommand
    {
#pragma warning disable SA1600 // Elements should be documented
        private Role()
        {
        }

        public static Role Instance { get; } = new Role();

        public string Command { get; } = "role";

        public string[] Aliases { get; } = new[] { "r" };

        public string Description { get; } = DiscordIntegration.Language.RemoveRoleCommandDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 1)
            {
                response = string.Format(DiscordIntegration.Language.InvalidParametersError, 1);
                return false;
            }

            if (!sender.CheckPermission("di.remove.role"))
            {
                response = string.Format(DiscordIntegration.Language.NotEnoughPermissions, "di.remove.role");
                return false;
            }

            if (!DiscordIntegration.Network.IsConnected)
            {
                response = DiscordIntegration.Language.BotIsNotConnectedError;
                return false;
            }

            if (!arguments.At(0).IsValidDiscordRoleId())
            {
                response = string.Format(DiscordIntegration.Language.InvalidDiscordRoleIdError, arguments.At(0));
                return false;
            }

            _ = DiscordIntegration.Network.SendAsync(new RemoteCommand("removeRole", arguments.At(0), sender.GetCompatible()));

            response = DiscordIntegration.Language.RemoveRoleCommandSuccess;
            return false;
        }
    }
}
