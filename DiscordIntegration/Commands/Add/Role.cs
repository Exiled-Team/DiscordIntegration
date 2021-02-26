// -----------------------------------------------------------------------
// <copyright file="Role.cs" company="Exiled Team">
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
    using static DiscordIntegration;

    /// <summary>
    /// Adds a role-group pair to the SyncedRole list.
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

        public string Description { get; } = Language.AddRoleCommandDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 2)
            {
                response = string.Format(Language.InvalidParametersError, 2);
                return false;
            }

            if (!sender.CheckPermission("di.add.role"))
            {
                response = string.Format(Language.NotEnoughPermissions, "di.add.role");
                return false;
            }

            if (!Network.IsConnected)
            {
                response = Language.BotIsNotConnectedError;
                return false;
            }

            if (!arguments.At(0).IsValidDiscordRoleId())
            {
                response = string.Format(Language.InvalidDiscordRoleIdError, arguments.At(0));
                return false;
            }

            if (ServerStatic.PermissionsHandler.GetGroup(arguments.At(1)) == null)
            {
                response = string.Format(Language.InvalidGroupError, arguments.At(1));
                return false;
            }

            _ = Network.SendAsync(new RemoteCommand("addRole", arguments.At(0), arguments.At(1), sender.GetCompatible()));

            response = Language.AddRoleCommandSuccess;
            return false;
        }
    }
}
