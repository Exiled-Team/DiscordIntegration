using System.ComponentModel;
using Exiled.API.Interfaces;

namespace DiscordIntegration_Plugin
{
    public class Config : IConfig
    {
        public bool Debug { get; set; } = false;
        public bool RaCommands { get; set; } = true;
        public bool RoundStart { get; set; } = true;
        public bool RoundEnd { get; set; } = true;
        public bool WaitingForPlayers { get; set; } = true;
        public bool CheaterReport { get; set; } = true;
        public bool PlayerHurt { get; set; } = true;
        public bool PlayerDeath { get; set; } = true;
        public bool GrenadeThrown { get; set; } = true;
        public bool MedicalItem { get; set; } = true;
        public bool SetClass { get; set; } = true;
        public bool Respawn { get; set; } = true;
        public bool PlayerJoin { get; set; } = true;
        public bool DoorInteract { get; set; } = false;
        public bool Scp914Upgrade { get; set; } = true;
        public bool Scp079Tesla { get; set; } = true;
        public bool Scp106Tele { get; set; } = true;
        public bool PocketEnter { get; set; } = true;
        public bool PocketEscape { get; set; } = true;
        public bool ConsoleCommand { get; set; } = true;
        public bool Decon { get; set; } = true;
        public bool DropItem { get; set; } = false;
        public bool PickupItem { get; set; } = false;
        public bool Intercom { get; set; } = true;
        public bool Banned { get; set; } = true;
        public bool Cuffed { get; set; } = true;
        public bool Freed { get; set; } = true;
        public bool Scp914Activation { get; set; } = true;
        public bool Scp914KnobChange { get; set; } = true;
        public bool WarheadCancel { get; set; } = true;
        public bool WarheadDetonate { get; set; } = true;
        public bool WarheadStart { get; set; } = true;
        public bool WarheadAccess { get; set; } = true;
        public bool Elevator { get; set; } = true;
        public bool Locker { get; set; } = true;
        public bool TriggerTesla { get; set; } = true;
        public bool GenClose { get; set; } = true;
        public bool GenOpen { get; set; } = true;
        public bool GenInsert { get; set; } = true;
        public bool GenEject { get; set; } = true;
        public bool GenFinish { get; set; } = true;
        public bool GenUnlock { get; set; } = true;
        public bool Scp106Contain { get; set; } = true;
        public bool Scp106Portal { get; set; } = true;
        public bool ItemChanged { get; set; } = false;
        public bool Scp079Exp { get; set; } = true;
        public bool Scp079Lvl { get; set; } = true;
        public bool PlayerLeave { get; set; } = true;
        public bool PlayerReload { get; set; } = false;
        public bool SetGroup { get; set; } = true;
        public bool ShowIpAddresses { get; set; } = true;


        [Description("Wether or not it should use the EggAddress IP for connecting to the Discord Bot. Note that while this option exists, it's use it not supported, or recommended.")]
        public bool Egg { get; set; } = false;
        
        [Description("The IP address to connect to the bot, if EggMode is enabled.")]
        public string EggAddress { get; set; } = "";
        
        [Description("Only log friendly fire for damage.")]
        public bool OnlyFriendlyFire { get; set; } = true;
        
        [Description("Wether or not the plugin should try adn set player's roles when they join based on the Discord Bot's discord sync feature.")]
        public bool RoleSync { get; set; } = true;
        
        [Description("Wether or not the plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;
    }
}