using Exiled.API.Interfaces;
using Exiled.Loader;

namespace DiscordIntegration_Plugin
{
    public class Config : IConfig
    {
        public bool Debug { get; private set; } = false;
        public bool RaCommands { get; private set; } = true;
        public bool RoundStart { get; private set; } = true;
        public bool RoundEnd { get; private set; } = true;
        public bool WaitingForPlayers { get; private set; } = true;
        public bool CheaterReport { get; private set; } = true;
        public bool PlayerHurt { get; private set; } = true;
        public bool PlayerDeath { get; private set; } = true;
        public bool GrenadeThrown { get; private set; } = true;
        public bool MedicalItem { get; private set; } = true;
        public bool SetClass { get; private set; } = true;
        public bool Respawn { get; private set; } = true;
        public bool PlayerJoin { get; private set; } = true;
        public bool DoorInteract { get; private set; } = false;
        public bool Scp914Upgrade { get; private set; } = true;
        public bool Scp079Tesla { get; private set; } = true;
        public bool Scp106Tele { get; private set; } = true;
        public bool PocketEnter { get; private set; } = true;
        public bool PocketEscape { get; private set; } = true;
        public bool ConsoleCommand { get; private set; } = true;
        public bool Decon { get; private set; } = true;
        public bool DropItem { get; private set; } = false;
        public bool PickupItem { get; private set; } = false;
        public bool Intercom { get; private set; } = true;
        public bool Banned { get; private set; } = true;
        public bool Cuffed { get; private set; } = true;
        public bool Freed { get; private set; } = true;
        public bool Scp914Activation { get; private set; } = true;
        public bool Scp914KnobChange { get; private set; } = true;
        public bool WarheadCancel { get; private set; } = true;
        public bool WarheadDetonate { get; private set; } = true;
        public bool WarheadStart { get; private set; } = true;
        public bool WarheadAccess { get; private set; } = true;
        public bool Elevator { get; private set; } = true;
        public bool Locker { get; private set; } = true;
        public bool TriggerTesla { get; private set; } = true;
        public bool GenClose { get; private set; } = true;
        public bool GenOpen { get; private set; } = true;
        public bool GenInsert { get; private set; } = true;
        public bool GenEject { get; private set; } = true;
        public bool GenFinish { get; private set; } = true;
        public bool GenUnlock { get; private set; } = true;
        public bool Scp106Contain { get; private set; } = true;
        public bool Scp106Portal { get; private set; } = true;
        public bool ItemChanged { get; private set; } = false;
        public bool Scp079Exp { get; private set; } = true;
        public bool Scp079Lvl { get; private set; } = true;
        public bool PlayerLeave { get; private set; } = true;
        public bool PlayerReload { get; private set; } = false;
        public bool SetGroup { get; private set; } = true;

        public bool AnnounceDecon;
        public bool PlacingBlood;
        public bool PlacingDecal;
        public bool AnnounceNtf;
        public bool AnnounceScpTerm;
        public bool PreAuth;
        public bool SpawnRagdoll;
        public bool Escape;
        public bool Kicked;
        public bool Enraging096;
        public bool Calming096;
        public bool FemurBreaker;
        public bool UsedMedicalItem;
        public bool RoundRestart;


        public bool Egg { get; private set; } = false;
        public string EggAddress { get; private set; } = "";
        public bool OnlyFriendlyFire { get; private set; } = true;
        public bool RoleSync { get; private set; } = true;

        public bool IsEnabled { get; set; }
        public string Prefix { get; } = "discord_";
    }
}