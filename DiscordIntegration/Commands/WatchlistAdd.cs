// -----------------------------------------------------------------------
// <copyright file="PlayerList.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

using DiscordIntegration.Dependency.Database;

namespace DiscordIntegration.Commands
{
    using System;
    using System.Text;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using NorthwoodLib.Pools;
    using static DiscordIntegration;

    /// <summary>
    /// Adds a user to the watchlist.
    /// </summary>
    internal sealed class WatchlistAdd : ICommand
    {
#pragma warning disable SA1600 // Elements should be documented
        private WatchlistAdd()
        {
        }

        public static WatchlistAdd Instance { get; } = new WatchlistAdd();

        public string Command { get; } = "watchadd";

        public string[] Aliases { get; } = new[] { "wla" };

        public string Description { get; } = Language.WatchlistAddDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("di.watchlistadd"))
            {
                response = string.Format(Language.NotEnoughPermissions, "di.watchlistadd");
                return false;
            }

            Player player = Player.Get(arguments.ElementAt(2));
            string reason = string.Empty;
            foreach (string s in arguments.Skip(2))
                reason += $"{s} ";
            reason = reason.TrimEnd(' ');
            DatabaseHandler.AddEntry(player.UserId, reason);
            response = $"{player.Nickname} added to watchlist for {reason}";
            return true;
        }
    }
}