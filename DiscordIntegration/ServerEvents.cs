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
                ProcessSTT.SendData($":keyboard: {ev.Sender.Nickname}({ev.Sender.UserId}) {Plugin.Translation.UsedCommand}: {ev.Name} {Args}", HandleQueue.CommandLogChannelId);
            if (ev.Name.ToLower() == "list")
            {
                Log.Info("Fetching players...");
                ev.IsAllowed = false;
                string message = "";
                foreach (Player player in Player.List)
                    if (!player.IsHost)
                        message += $"{player.Nickname} - ({player.UserId})\n";
                if (string.IsNullOrEmpty(message))
                    message = $"{Plugin.Translation.NoPlayersOnline}";
                ev.CommandSender.RaReply($"{message}", true, true, string.Empty);
            }
            else if (ev.Name.ToLower() == "stafflist")
            {
                Log.Info("Getting Staff...");
                ev.IsAllowed = false;
                Log.Info("Staff:");
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
                string response = isStaff ? names : $"{Plugin.Translation.NoStaffOnline}";
                ev.CommandSender.RaReply($"{PlayerManager.players.Count}/{plugin.MaxPlayers} {response}", true, true, string.Empty);
            }
        }
        
        public void OnWaitingForPlayers()
        {
            if (Plugin.Singleton.Config.WaitingForPlayers)
                ProcessSTT.SendData($":hourglass: {Plugin.Translation.WaitingForPlayers}", HandleQueue.GameLogChannelId);
        }

        public void OnRoundStart()
        {
            if (Plugin.Singleton.Config.RoundStart)
                ProcessSTT.SendData($":arrow_forward: {Plugin.Translation.RoundStarting}: {Player.List.Count()} {Plugin.Translation.PlayersInRound}.", HandleQueue.GameLogChannelId);
        }

        public void OnRoundEnd(RoundEndedEventArgs ev)
        {
            if (Plugin.Singleton.Config.RoundEnd)
                ProcessSTT.SendData($":stop_button: {Plugin.Translation.RoundEnded}: {Player.List.Count()} {Plugin.Translation.PlayersOnline}.", HandleQueue.GameLogChannelId);
        }

        public void OnCheaterReport(ReportingCheaterEventArgs ev)
        {
            if (Plugin.Singleton.Config.CheaterReport)
                ProcessSTT.SendData($"**{Plugin.Translation.CheaterReportFiled}: {ev.Reporter.UserId} {Plugin.Translation.Reported} {ev.Reported.UserId} {Plugin.Translation._For} {ev.Reason}.**", HandleQueue.GameLogChannelId);
        }
        
        public void OnConsoleCommand(SendingConsoleCommandEventArgs ev)
        {
            string Argies = string.Join(" ", ev.Arguments);
            if (Plugin.Singleton.Config.ConsoleCommand)
                ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Role}) {Plugin.Translation.HasRunClientConsoleCommand}: {ev.Name} {Argies}", HandleQueue.CommandLogChannelId);
        }
        
        public void OnRespawn(RespawningTeamEventArgs ev)
        {
            if (Plugin.Singleton.Config.Respawn)
            {
                string msg = ev.NextKnownTeam == SpawnableTeamType.ChaosInsurgency ? $":spy: {Plugin.Translation.ChaosInsurgency}" : $":cop: {Plugin.Translation.NineTailedFox}";
                ProcessSTT.SendData($"{msg} {Plugin.Translation.HasSpawnedWith} {ev.Players.Count} {Plugin.Translation.Players}.", HandleQueue.GameLogChannelId);
            }
        }
    }
}