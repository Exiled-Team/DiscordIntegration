using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Respawning;

namespace DiscordIntegration_Plugin
{
    public class ServerEvents
    {
        public Plugin plugin;
        public ServerEvents(Plugin plugin) => this.plugin = plugin;
        
        public void OnCommand(SendingRemoteAdminCommandEventArgs ev)
        {
            string Args = string.Join(" ", ev.Arguments);
            if (Plugin.Singleton.Config.RaCommands)
                ProcessSTT.SendData($":keyboard: **{ev.Sender.Nickname}** ID: **{ev.Sender.UserId}** {Plugin.translation.UsedCommand}: **{ev.Name} {Args}**", HandleQueue.CommandLogChannelId);
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
                        names += $"{player.Nickname} - {player.Id} ";
                    }
                }

                Log.Info($"Bool: {isStaff} Names: {names}");
                string response = isStaff ? names : $"{Plugin.translation.NoStaffOnline}";
                ev.CommandSender.RaReply($"{PlayerManager.players.Count}/{plugin.MaxPlayers} {response}", true, true, string.Empty);
            }
        }
        
        public void OnWaitingForPlayers()
        {
            if (Plugin.Singleton.Config.WaitingForPlayers)
                ProcessSTT.SendData($":hourglass: {Plugin.translation.WaitingForPlayers}", HandleQueue.GameLogChannelId);
        }

        public void OnRoundStart()
        {
            if (Plugin.Singleton.Config.RoundStart)
                ProcessSTT.SendData($":arrow_forward: {Plugin.translation.RoundStarting}: {Player.List.Count()} {Plugin.translation.PlayersInRound}.", HandleQueue.GameLogChannelId);
        }

        public void OnRoundEnd(RoundEndedEventArgs ev)
        {
            if (Plugin.Singleton.Config.RoundEnd)
                ProcessSTT.SendData($":stop_button: {Plugin.translation.RoundEnded}: {Player.List.Count()} {Plugin.translation.PlayersOnline}.", HandleQueue.GameLogChannelId);
        }

        public void OnCheaterReport(ReportingCheaterEventArgs ev)
        {
            if (Plugin.Singleton.Config.CheaterReport)
                ProcessSTT.SendData($":pirate_flag: **{Plugin.translation.CheaterReportFiled}: {ev.Reporter.Nickname} - ({ev.Reporter.Role}) {Plugin.translation.Reported} {ev.Reported.Nickname} - ({ev.Reported.Role}) {Plugin.translation._For} {ev.Reason}.**", HandleQueue.GameLogChannelId);
        }
        
        public void OnConsoleCommand(SendingConsoleCommandEventArgs ev)
        {
            string Argies = string.Join(" ", ev.Arguments);
            if (Plugin.Singleton.Config.ConsoleCommand)
                ProcessSTT.SendData($":joystick: {ev.Player.Nickname} - {ev.Player.UserId} - {ev.Player.Id} - ({ev.Player.Role}) {Plugin.translation.HasRunClientConsoleCommand}: {ev.Name} {Argies}", HandleQueue.CommandLogChannelId);
        }
        
        public void OnRespawn(RespawningTeamEventArgs ev)
        {
            if (Plugin.Singleton.Config.Respawn)
            {
                string msg = ev.NextKnownTeam == SpawnableTeamType.ChaosInsurgency ? $":spy: {Plugin.translation.ChaosInsurgency}" : $":cop: {Plugin.translation.NineTailedFox}";
                ProcessSTT.SendData($"{msg} {Plugin.translation.HasSpawnedWith} {ev.Players.Count} {Plugin.translation.Players}.", HandleQueue.GameLogChannelId);
            }
        }
    }
}