// -----------------------------------------------------------------------
// <copyright file="Language.cs" company="Exiled Team">
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
    /// Reloads plugin and bot language if connected.
    /// </summary>
    internal sealed class Language : ICommand
    {
#pragma warning disable SA1600 // Elements should be documented
        private Language()
        {
        }

        public static Language Instance { get; } = new Language();

        public string Command { get; } = "language";

        public string[] Aliases { get; } = new[] { "l", "lang" };

        public string Description { get; } = DiscordIntegration.Language.ReloadLanguageCommandDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 0)
            {
                response = string.Format(DiscordIntegration.Language.InvalidParametersError, 0);
                return false;
            }

            if (!sender.CheckPermission("di.reload.language"))
            {
                response = string.Format(DiscordIntegration.Language.NotEnoughPermissions, "di.reload.language");
                return false;
            }

            DiscordIntegration.Language.Save();
            DiscordIntegration.Language.Load();

            response = DiscordIntegration.Language.ReloadLanguageCommandSuccess;
            return true;
        }
    }
}
