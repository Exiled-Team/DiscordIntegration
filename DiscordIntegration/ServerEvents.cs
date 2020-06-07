using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs;

namespace DiscordIntegration_Plugin
{
    public class ServerEvents
    {
        public Plugin plugin;
        public ServerEvents(Plugin plugin) => this.plugin = plugin;
        
        public void OnCommand(SendingRemoteAdminCommandEventArgs ev)
        {
            if (Plugin.Cfg.RaCommands)
                ProcessSTT.SendData($"{ev.Sender.Nickname} {Plugin.translation.UsedCommand}: {ev.Name}", HandleQueue.CommandLogChannelId);
            if (ev.Name.ToLower() == "list")
            {
                Log.Info("Getting List");
                ev.IsAllowed = false;
                string message = "";
                foreach (Player player in Player.List)
                    if (!player.IsHost)
                        message += $"{player.Nickname} - ({player.UserId})\n";
                if (string.IsNullOrEmpty(message))
                    message = $"{Plugin.translation.NoPlayersOnline}";
                ev.CommandSender.RaReply($"{message}", true, true, string.Empty);
            }
            else if (ev.Name.ToLower() == "stafflist")
            {
                Log.Info("Getting StaffList");
                ev.IsAllowed = false;
                Log.Info("Staff list");
                bool isStaff = false;
                string names = "";
                foreach (Player player in Player.List)
                {
                    if (player.ReferenceHub.serverRoles.RemoteAdmin)
                    {
                        isStaff = true;
                        names += $"{player.Nickname} ";
                    }
                }

                Log.Info($"Bool: {isStaff} Names: {names}");
                string response = isStaff ? names : $"{Plugin.translation.NoStaffOnline}";
                ev.CommandSender.RaReply($"{PlayerManager.players.Count}/{plugin.MaxPlayers} {response}", true, true, string.Empty);
            }
        }
        
        public void OnWaitingForPlayers()
        {
            if (Plugin.Cfg.WaitingForPlayers)
                ProcessSTT.SendData($"{Plugin.translation.WaitingForPlayers}", HandleQueue.GameLogChannelId);
        }

        public void OnRoundStart()
        {
            if (Plugin.Cfg.RoundStart)
                ProcessSTT.SendData($"{Plugin.translation.RoundStarting}: {Player.List.Count()} {Plugin.translation.PlayersInRound}.", HandleQueue.GameLogChannelId);
        }

        public void OnRoundEnd(RoundEndedEventArgs ev)
        {
            if (Plugin.Cfg.RoundEnd)
                ProcessSTT.SendData($"{Plugin.translation.RoundEnded}: {Player.List.Count()} {Plugin.translation.PlayersOnline}.", HandleQueue.GameLogChannelId);
        }

        public void OnCheaterReport(ReportingCheaterEventArgs ev)
        {
            if (Plugin.Cfg.CheaterReport)
                ProcessSTT.SendData($"**{Plugin.translation.CheaterReportFiled}: {ev.ReporterUserId} {Plugin.translation.Reported} {ev.ReportedUserId} {Plugin.translation._For} {ev.Reason}.**", HandleQueue.GameLogChannelId);
        }
        
        public void OnConsoleCommand(SendingConsoleCommandEventArgs ev)
        {
            if (Plugin.Cfg.ConsoleCommand)
                ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Role}) {Plugin.translation.HasRunClientConsoleCommand}: {ev.Name}", HandleQueue.CommandLogChannelId);
        }
        
        public void OnRespawn(RespawningTeamEventArgs ev)
        {
            if (Plugin.Cfg.Respawn)
            {
                string msg = ev.IsChaos ? $"{Plugin.translation.ChaosInsurgency}" : $"{Plugin.translation.NineTailedFox}";
                ProcessSTT.SendData($"{msg} {Plugin.translation.HasSpawnedWith} {ev.Players.Count} {Plugin.translation.Players}.", HandleQueue.GameLogChannelId);
            }
        }
    }
}