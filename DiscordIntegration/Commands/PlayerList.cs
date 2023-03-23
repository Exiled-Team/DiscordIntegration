// -----------------------------------------------------------------------
// <copyright file="PlayerList.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

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
    /// Gets the list of players in the server.
    /// </summary>
    internal sealed class PlayerList : ICommand
    {
#pragma warning disable SA1600 // Elements should be documented
        private PlayerList()
        {
        }

        public static PlayerList Instance { get; } = new PlayerList();

        public string Command { get; } = "playerlist";

        public string[] Aliases { get; } = new[] { "pli" };

        public string Description { get; } = Language.PlayerListCommandDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("di.playerlist"))
            {
                response = string.Format(Language.NotEnoughPermissions, "di.playerlist");
                return false;
            }

            StringBuilder message = StringBuilderPool.Shared.Rent();

            if (Player.Dictionary.Count == 0)
            {
                message.Append(Language.NoPlayersOnline);
            }
            else
            {
                foreach (Player player in Player.List)
                    message.Append(player.Nickname).Append(" - ").Append(player.UserId).AppendLine();
            }

            response = message.ToString();

            StringBuilderPool.Shared.Return(message);

            return true;
        }
    }
}
