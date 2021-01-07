using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Respawning;
using System.IO;
using System.Linq;
using DiscordIntegration_Plugin.System;
using System;

namespace DiscordIntegration_Plugin.EvHandlers
{
    public class ServerEvents
    {
        public Plugin plugin;
        public ServerEvents(Plugin plugin) => this.plugin = plugin;

        public void OnCommand(SendingRemoteAdminCommandEventArgs ev)
        {
            string Args = string.Join(" ", ev.Arguments);
            if (Plugin.Singleton.Config.RaCommands)
                ProcessSTT.SendData($":keyboard: **{ev.Sender.Nickname}** |**ID:** {ev.Sender.UserId} |\nUsó el comando: ``{ev.Name} {Args}``", HandleQueue.CommandLogChannelId);
            if (ev.Name.ToLower() == "list")
            {
                Log.Info("Getting List");
                ev.IsAllowed = false;
                int max = GameCore.ConfigFile.ServerConfig.GetInt("max_players", 20);
                int cur = Player.List.Count();
                string message = $"```diff\n--- Jugadores conectados [{cur}/{max}] ---\n\n";
                foreach (Player player in Player.List.OrderBy(pl => pl.Id))
                    if (!player.IsHost)
                        message += $"+ {player.Nickname} | SteamID: {player.UserId} | IP: {player.IPAddress}\n";

                if (string.IsNullOrEmpty(message))
                    message = $"{Plugin.translation.NoPlayersOnline}";
                message += "```";
                ev.CommandSender.RaReply($"{message}", true, true, string.Empty);
            }
            else if (ev.Name.ToLower() == "stafflist")
            {
                Log.Info("Getting StaffList");
                ev.IsAllowed = false;
                Log.Info("Staff list");
                int max = GameCore.ConfigFile.ServerConfig.GetInt("max_players", 20);
                int cur = Player.List.Count();
                bool isStaff = false;
                string names = $"```diff\n--- Jugadores conectados [{cur}/{max}] ---\n\n";
                foreach (Player player in Player.List)
                {
                    if (player.ReferenceHub.serverRoles.RemoteAdmin)
                    {
                        isStaff = true;
                        names += $"- {player.Nickname} |  ID: {player.Id} | SteamID: {player.UserId} | IP: {player.IPAddress} \n";
                    }
                }

                Log.Info($"Bool: {isStaff} Names: {names}");
                string response = isStaff ? names : $"{Plugin.translation.NoStaffOnline}";
                response += $"\n```";
                ev.CommandSender.RaReply($"{PlayerManager.players.Count}/{plugin.MaxPlayers} {response}", true, true, string.Empty);


            }
            else if(ev.Name.ToLower() == "direstart")
            {
                Plugin.Singleton.OnDisabled();
                Plugin.Singleton.OnEnabled();
                Log.Info("Se reinicio el plugin de DiscordIntration con el comando");
                ev.CommandSender.RaReply($"Se reinicio el plugin de DiscordIntegration.", true, true, string.Empty);
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
          try
          {
                if (Plugin.Singleton.Config.RoundEnd)
                    ProcessSTT.SendData($":stop_button: {Plugin.translation.RoundEnded}: {Player.List.Count()} {Plugin.translation.PlayersOnline}.", HandleQueue.GameLogChannelId);

                Log.Debug("Antes RoundsPlayed " + Methods.RoundsPlayed);
                Methods.RoundsPlayed++;
                Log.Debug("Despues RoundsPlayed " + Methods.RoundsPlayed);

                Methods.UpdateStats(Methods.TKCount, Methods.RoundsPlayed, Methods.PlayerTotalDeaths, Methods.PlayerJoinCount);
            }
            catch(Exception e)
            {
                Log.Error($"OnRoundEnd: {e}");
          }
            

        }

     

        
        public void OnCheaterReport(ReportingCheaterEventArgs ev)
        {
            if (Plugin.Singleton.Config.CheaterReport)
                ProcessSTT.SendData($":pirate_flag: **{Plugin.translation.CheaterReportFiled}: {ev.Reporter.Nickname} - ({ev.Reporter.Role}) {Plugin.translation.Reported} {ev.Reported.Nickname} - ({ev.Reported.Role.Traduccion()}) {Plugin.translation._For} {ev.Reason}.**", HandleQueue.GameLogChannelId);
        }

        public void OnConsoleCommand(SendingConsoleCommandEventArgs ev)
        {
            string Argies = string.Join(" ", ev.Arguments);
            if (ev.Name == "zr") return;
                ProcessSTT.SendData($":joystick: **{ev.Player.Nickname}** |**ID:** {ev.Player.UserId} |\nUsó el comando: ``{ev.Name} {Argies}``", HandleQueue.CommandLogChannelId);
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