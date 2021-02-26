// -----------------------------------------------------------------------
// <copyright file="Reload.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.Commands.Reload
{
    using System;
    using CommandSystem;
    using Exiled.Permissions.Extensions;

    /// <summary>
    /// Reloads bot configs.
    /// Reloads plugin and bot language if connected.
    /// Reloads bot synced roles if connected.
    /// </summary>
    internal sealed class Reload : ParentCommand
    {
#pragma warning disable SA1600 // Elements should be documented
        private Reload() => LoadGeneratedCommands();

        public static Reload Instance { get; } = new Reload();

        public override string Command { get; } = "reload";

        public override string[] Aliases { get; } = new[] { "rd", "rld" };

        public override string Description { get; } = $"{DiscordIntegration.Language.ReloadConfigsCommandDescription} {DiscordIntegration.Language.ReloadLanguageCommandDescription} {DiscordIntegration.Language.ReloadSyncedRolesDescription}";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(Configs.Instance);
            RegisterCommand(Language.Instance);
            RegisterCommand(SyncedRoles.Instance);
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 0)
            {
                response = string.Format(DiscordIntegration.Language.InvalidParametersError, 0);
                return false;
            }

            if (!sender.CheckPermission("di.reload"))
            {
                response = string.Format(DiscordIntegration.Language.NotEnoughPermissions, "di.reload");
                return false;
            }

            Configs.Instance.Execute(arguments, sender, out string configsResponse);
            Language.Instance.Execute(arguments, sender, out string languageResponse);
            SyncedRoles.Instance.Execute(arguments, sender, out string syncedRolesResponse);

            response = $"{configsResponse} {languageResponse} {syncedRolesResponse}";
            return false;
        }
    }
}
