// -----------------------------------------------------------------------
// <copyright file="PlayerList.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using global::DiscordIntegration.API.Commands;
    using MEC;
    using Newtonsoft.Json;
    using NorthwoodLib.Pools;
    using RemoteAdmin;
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

        public string[] Aliases { get; } = new[] { "pli", "plys" };

        public string Description { get; } = Language.PlayerListCommandDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("di.playerlist"))
            {
                response = string.Format(Language.NotEnoughPermissions, "di.playerlist");
                return false;
            }

            if (sender is RemoteAdmin.PlayerCommandSender ply)
            {
                if (Player.Dictionary.Count == 0)
                {
                    response = $"<color=red>{Language.NoPlayersOnline}</color>";
                    return false;
                }
                else
                {
                    TimeSpan duration = Round.ElapsedTime;
                    var seconds = duration.Seconds < 10 ? $"0{duration.Seconds}" : duration.Seconds.ToString();
                    var minutes = duration.Minutes < 10 ? $"0{duration.Minutes}" : duration.Minutes.ToString();
                    int max = DiscordIntegration.Instance.Slots;
                    int cur = Player.Dictionary.Count;

                    var title = string.Format(Language.PlayerListEmbedTitle, cur, max, minutes, seconds);
                    string msg = $"\n<color=green>{title}</color>\n";
                    foreach (Player player in Player.List)
                    {
                        if (player.RemoteAdminAccess)
                        {
                            var yes = string.Format(Language.PlayerListTextperStaff, player.Id, player.Nickname, player.Role, player.GroupName);
                            msg += $"<color=red>{yes}</color>";
                        }
                        else
                        {
                            var yes = string.Format(Language.PlayerListTextperPlayer, player.Id, player.Nickname, player.Role);
                            msg += $"<color=green>{yes}</color>";
                        }
                    }

                    response = msg;
                    return true;
                }
            }
            else
            {
                TimeSpan duration = Round.ElapsedTime;
                string seconds = duration.Seconds < 10 ? $"0{duration.Seconds}" : duration.Seconds.ToString();
                string minutes = duration.Minutes < 10 ? $"0{duration.Minutes}" : duration.Minutes.ToString();
                string message = $"```{Language.PlayerListCodeBlock}\n";

                if (Player.Dictionary.Count == 0)
                {
                    message = Language.PlayerListNoPlayerOnline;
                    Timing.CallDelayed(1f, () =>
                    {
                        var channel = Events.NetworkHandler.channelId; // I do this to avoid that it takes too long and that when executing the command in another place it is sent to the previous one.
                        var title = string.Format(Language.PlayerListEmbedTitle, Player.Dictionary.Count, DiscordIntegration.Instance.Slots, minutes, seconds);
                        _ = Network.SendAsync(new RemoteCommand("sendEmbed", channel, title, message));
                    });
                   
                    response = Language.NoPlayersOnline;
                    return true;
                }
                else
                {
                    foreach (Player player in Player.List)
                    {
                        if (player.RemoteAdminAccess)
                        {
                            message += string.Format(Language.PlayerListTextperStaff, player.Id, player.Nickname, player.Role, player.GroupName);
                        }
                        else
                        {
                            message += string.Format(Language.PlayerListTextperPlayer, player.Id, player.Nickname, player.Role);
                        }
                    }
                }

                message += $"\n```";

                Timing.CallDelayed(1f, () =>
                {
                    var channel = Events.NetworkHandler.channelId; // I do this to avoid that it takes too long and that when executing the command in another place it is sent to the previous one.
                    IList<Field> fields = new List<Field>();
                    fields.Add(new Field(Language.PlayerListTitle, message, false));
                    var title = string.Format(Language.PlayerListEmbedTitle, Player.Dictionary.Count, DiscordIntegration.Instance.Slots, minutes, seconds);
                    _ = Network.SendAsync(new RemoteCommand("sendEmbed", channel, title, string.Empty, fields));


                });
                response = null;
                return true;
            }
        }
    }
}
