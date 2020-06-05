using Exiled.API.Features;
using Exiled.Events.Handlers.EventArgs;

namespace DiscordIntegration_Plugin
{
    public class MapEvents
    {
        public Plugin plugin;
        public MapEvents(Plugin plugin) => this.plugin = plugin;
        
        public void OnWarheadDetonation()
        {
            if (Plugin.Cfg.WarheadDetonate)
                ProcessSTT.SendData($"{Plugin.translation.WarheadDetonated}.", HandleQueue.GameLogChannelId);
        }


        public void OnGenFinish(GeneratorActivatedEventArgs ev)
        {
            if (Plugin.Cfg.GenFinish)
                ProcessSTT.SendData($"{Plugin.translation.GenFinished}", HandleQueue.GameLogChannelId);
        }
        
        public void OnDecon(DecontaminatingEventArgs ev)
        {
            if (Plugin.Cfg.Decon)
                ProcessSTT.SendData($"{Plugin.translation.HasDropped}.", HandleQueue.CommandLogChannelId);
        }
        
        public void OnWarheadStart(StartingWarheadEventArgs ev)
        {
            if (Plugin.Cfg.WarheadStart)
                ProcessSTT.SendData($"{Plugin.translation.WarheadStarted} {Warhead.Controller.NetworktimeToDetonation}.", HandleQueue.GameLogChannelId);
        }
        
        public void OnWarheadCancelled(StoppingWarheadEventArgs ev)
        {
            if (Plugin.Cfg.WarheadCancel)
                ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} {Plugin.translation.CancelledWarhead}.", HandleQueue.GameLogChannelId);
        }
        
        public void OnScp194Upgrade(UpgradingScp914ItemsEventArgs ev)
        {
            if (Plugin.Cfg.Scp914Upgrade)
            {
                string players = "";
                foreach (Player player in ev.Players) 
                    players += $"{player.Nickname} - {player.UserId} ({player.Role})\n";
                string items = "";
                foreach (Pickup item in ev.Items)
                    items += $"{item.ItemId}\n";
				
                ProcessSTT.SendData($"{Plugin.translation.Scp914HasProcessedTheFollowingPlayers}: {players} {Plugin.translation.AndItems}: {items}.", HandleQueue.GameLogChannelId);
            }
        }
    }
}