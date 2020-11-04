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
                ProcessSTT.SendData($":radioactive: **{Plugin.translation.WarheadDetonated}.**", HandleQueue.GameLogChannelId);
        }


        public void OnGenFinish(GeneratorActivatedEventArgs ev)
        {
            if (Plugin.Singleton.Config.GenFinish)
                ProcessSTT.SendData($":warning: {Plugin.translation.GenFinished}", HandleQueue.GameLogChannelId);
        }
        
        public void OnDecon(DecontaminatingEventArgs ev)
        {
            if (Plugin.Singleton.Config.Decon)
                ProcessSTT.SendData($":biohazard: **{Plugin.translation.DecontaminationHasBegun}.**", HandleQueue.GameLogChannelId);
        }
        
        public void OnWarheadStart(StartingEventArgs ev)
        {
            if (Plugin.Singleton.Config.WarheadStart)
                ProcessSTT.SendData($":radioactive: **{Plugin.translation.WarheadStarted} {Warhead.Controller.NetworktimeToDetonation} seconds.**", HandleQueue.GameLogChannelId);
        }
        
        public void OnWarheadCancelled(StoppingEventArgs ev)
        {
            if (Plugin.Singleton.Config.WarheadCancel)
                ProcessSTT.SendData($"**{ev.Player.Nickname} - ({ev.Player.Role}) {Plugin.translation.CancelledWarhead}.**", HandleQueue.GameLogChannelId);
        }
        
        public void OnScp194Upgrade(UpgradingItemsEventArgs ev)
        {
            if (Plugin.Singleton.Config.Scp914Upgrade)
            {
                
                string players = "";
                foreach (Player player in ev.Players) 
                    players += $"--- {player.Nickname} - {player.Role}\n";
                string items = "";
                foreach (Pickup item in ev.Items)
                    items += $"+ {item.ItemId}\n";
				
                ProcessSTT.SendData($"```diff\n- Resultados del SCP-914 -\n{players}- Objetos procesados:\n{items}```", HandleQueue.GameLogChannelId);
            }
        }
    }
}