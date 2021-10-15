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
    using Exiled.API.Extensions;
    using Exiled.Permissions.Extensions;
    using NorthwoodLib.Pools;
    using static DiscordIntegration;
    using System.Linq;

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

        public string[] Aliases { get; } = new[] { "pli", "plys", "caca" };

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
                #region Bla bla bla

                int servermax = Server.MaxPlayerCount;
                int servercur = Server.PlayerCount;

                TimeSpan duration = Round.ElapsedTime;

                string seconds = duration.Seconds < 10 ? $"0{duration.Seconds}" : duration.Seconds.ToString();
                string minutes = duration.Minutes < 10 ? $"0{duration.Minutes}" : duration.Minutes.ToString();

                string textresponse = $"```diff\n-- Jugadores conectados [{servercur}/{servermax}] --\n-- Tiempo: {minutes}:{seconds} --\n\n";

                string textformat = "+- {0} - {1} - {2}\n";
                string texformatstaff = "-+ {0} - {1} - {2} - Staff\n";
                #endregion

                foreach (Player player in Player.List.OrderBy(p => p.Id))
                {
                    if (player.RemoteAdminAccess)
                    {
                        textresponse += string.Format(texformatstaff, player.Id, player.Nickname, player.Role.Translate());
                    }
                    else
                    {
                        textresponse += string.Format(textformat, player.Id, player.Nickname, player.Role.Translate());
                    }
                }

                textresponse += "\n```";

                message.Append(textresponse);
            }

            response = message.ToString();

            StringBuilderPool.Shared.Return(message);

            return true;
        }
    }
}
