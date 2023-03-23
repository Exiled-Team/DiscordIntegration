// -----------------------------------------------------------------------
// <copyright file="Main.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.Commands
{
#pragma warning disable SA1600 // Elements should be documented
    using System;
    using CommandSystem;
    using static DiscordIntegration;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal sealed class Main : ParentCommand
    {
        public Main() => LoadGeneratedCommands();

        public override string Command { get; } = "discordintegration";

        public override string[] Aliases { get; } = new[] { "di", "integration", "discord", "ds" };

        public override string Description { get; } = string.Empty;

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(PlayerList.Instance);
            RegisterCommand(StaffList.Instance);

            if (Instance.Config.UseWatchlist)
            {
                RegisterCommand(WatchlistAdd.Instance);
                RegisterCommand(WatchlistRemove.Instance);
            }
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = $"{Language.InvalidSubcommand} {Language.Available}: playerlist, stafflis" + $"{(Instance.Config.UseWatchlist ? "watchadd, watchrem" : string.Empty)}";
            return false;
        }
    }
}
