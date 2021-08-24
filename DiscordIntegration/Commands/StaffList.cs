// -----------------------------------------------------------------------
// <copyright file="StaffList.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using global::DiscordIntegration.API.Commands;
    using MEC;
    using NorthwoodLib.Pools;
    using static DiscordIntegration;

    /// <summary>
    /// Gets the list of staffers in the server.
    /// </summary>
    internal sealed class StaffList : ICommand
    {
#pragma warning disable SA1600 // Elements should be documented
        private StaffList()
        {
        }

        public static StaffList Instance { get; } = new StaffList();

        public string Command { get; } = "stafflist";

        public string[] Aliases { get; } = new[] { "sl", "slist" };

        public string Description { get; } = Language.StaffListCommandDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("di.stafflist"))
            {
                response = string.Format(Language.NotEnoughPermissions, "di.stafflist");
                return false;
            }

            Timing.WaitForSeconds(4);
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
                    var stafflist = Player.List.Where(p => p.RemoteAdminAccess);
                    if (stafflist.Count() >= 1)
                    {
                        foreach (Player player in stafflist)
                        {
                            var yes = string.Format(Language.PlayerListTextperStaff, player.Id, player.Nickname, player.Role, player.GroupName);
                            msg += $"<color=red>{yes}</color>\n";
                        }
                    }
                    else
                    {
                        msg += $"<color=red>{Language.NoStaffOnline}</color>";
                    }

                    response = msg;
                    return true;
                }
            }
            else
            {
                TimeSpan duration = Round.ElapsedTime;
                var seconds = duration.Seconds < 10 ? $"0{duration.Seconds}" : duration.Seconds.ToString();
                var minutes = duration.Minutes < 10 ? $"0{duration.Minutes}" : duration.Minutes.ToString();
                int max = DiscordIntegration.Instance.Slots;
                int cur = Player.Dictionary.Count;

                var title = string.Format(Language.PlayerListEmbedTitle, cur, max, minutes, seconds);
                string msg = $"```{Language.PlayerListCodeBlock}\n";
                var stafflist = Player.List.Where(p => p.RemoteAdminAccess);
                if (stafflist.Count() >= 1)
                {
                    foreach (Player player in stafflist)
                    {
                        var yes = string.Format(Language.PlayerListTextperStaff, player.Id, player.Nickname, player.Role, player.GroupName);
                        msg += yes;
                    }

                    msg += "\n```";
                }
                else
                {
                    msg = $"```diff\n-- {Language.NoStaffOnline} --\n```";
                }

                Timing.CallDelayed(1f, () =>
                {
                    var channel = Events.NetworkHandler.channelId; // I do this to avoid that it takes too long and that when executing the command in another place it is sent to the previous one.
                    IList<Field> fields = new List<Field>();
                    fields.Add(new Field(Language.PlayerListTitle, msg, false));
                    _ = Network.SendAsync(new RemoteCommand("sendEmbed", channel, title, string.Empty, fields));
                });
                response = null;
                return true;
            }
        }
    }
}
