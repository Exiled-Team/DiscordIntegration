using Exiled.API.Features;
using Exiled.Events.EventArgs;

namespace DiscordIntegration_Plugin
{
    public class MapEvents
    {
        public Plugin plugin;
        public MapEvents(Plugin plugin) => this.plugin = plugin;
        
        public void OnWarheadDetonation()
        {
            if (Plugin.Singleton.Config.WarheadDetonate)
                ProcessSTT.SendData($":radioactive: **{Plugin.Translation.WarheadDetonated}.**", HandleQueue.GameLogChannelId);
        }


        public void OnGenFinish(GeneratorActivatedEventArgs ev)
        {
            if (Plugin.Singleton.Config.GenFinish)
                ProcessSTT.SendData($"{Plugin.Translation.GenFinished}", HandleQueue.GameLogChannelId);
        }
        
        public void OnDecon(DecontaminatingEventArgs ev)
        {
            if (Plugin.Singleton.Config.Decon)
                ProcessSTT.SendData($":biohazard: **{Plugin.Translation.DecontaminationHasBegun}.**", HandleQueue.GameLogChannelId);
        }
        
        public void OnWarheadStart(StartingEventArgs ev)
        {
            if (Plugin.Singleton.Config.WarheadStart)
                ProcessSTT.SendData($":radioactive: **{Plugin.Translation.WarheadStarted} {Warhead.Controller.NetworktimeToDetonation} seconds.**", HandleQueue.GameLogChannelId);
        }
        
        public void OnWarheadCancelled(StoppingEventArgs ev)
        {
            if (Plugin.Singleton.Config.WarheadCancel)
                ProcessSTT.SendData($"***{ev.Player.Nickname} - {ev.Player.UserId} {Plugin.Translation.CancelledWarhead}.***", HandleQueue.GameLogChannelId);
        }
        
        public void OnScp194Upgrade(UpgradingItemsEventArgs ev)
        {
            if (Plugin.Singleton.Config.Scp914Upgrade)
            {
                string players = "";
                foreach (Player player in ev.Players) 
                    players += $"{player.Nickname} - {player.UserId} ({player.Role})\n";
                string items = "";
                foreach (Pickup item in ev.Items)
                    items += $"{item.ItemId}\n";
				
                ProcessSTT.SendData($"{Plugin.Translation.Scp914HasProcessedTheFollowingPlayers}: {players} {Plugin.Translation.AndItems}: {items}.", HandleQueue.GameLogChannelId);
            }
        }
    }
}