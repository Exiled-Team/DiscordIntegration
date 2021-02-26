// -----------------------------------------------------------------------
// <copyright file="Configs.cs" company="Exiled Team">
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
    /// Reloads bot configs.
    /// </summary>
    internal sealed class Configs : ICommand
    {
#pragma warning disable SA1600 // Elements should be documented
        private Configs()
        {
        }

        public static Configs Instance { get; } = new Configs();

        public string Command { get; } = "configs";

        public string[] Aliases { get; } = new[] { "c", "cfgs" };

        public string Description { get; } = DiscordIntegration.Language.ReloadConfigsCommandDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 0)
            {
                response = string.Format(DiscordIntegration.Language.InvalidParametersError, 0);
                return false;
            }

            if (!sender.CheckPermission("di.reload.configs"))
            {
                response = string.Format(DiscordIntegration.Language.NotEnoughPermissions, "di.reload.configs");
                return false;
            }

            if (!DiscordIntegration.Network.IsConnected)
            {
                response = DiscordIntegration.Language.BotIsNotConnectedError;
                return false;
            }

            _ = DiscordIntegration.Network.SendAsync(new RemoteCommand("loadConfigs", sender.GetCompatible()));

            response = DiscordIntegration.Language.ReloadConfigsCommandSuccess;
            return true;
        }
    }
}
