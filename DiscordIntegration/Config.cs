using Exiled.API.Interfaces;
using Exiled.Loader;

namespace DiscordIntegration_Plugin
{
    public class Config : IConfig
    {
        public void Reload()
        {
            RaCommands = PluginManager.YamlConfig.GetBool("discord_ra_commands", true);
			RoundStart = PluginManager.YamlConfig.GetBool("discord_round_start", true);
			RoundEnd = PluginManager.YamlConfig.GetBool("discord_round_end", true);
			WaitingForPlayers = PluginManager.YamlConfig.GetBool("discord_waiting_for_players", true);
			CheaterReport = PluginManager.YamlConfig.GetBool("discord_cheater_report", true);
			PlayerHurt = PluginManager.YamlConfig.GetBool("discord_player_hurt", true);
			PlayerDeath = PluginManager.YamlConfig.GetBool("discord_player_death", true);
			GrenadeThrown = PluginManager.YamlConfig.GetBool("discord_grenade_thrown", true);
			MedicalItem = PluginManager.YamlConfig.GetBool("discord_medical_item", true);
			SetClass = PluginManager.YamlConfig.GetBool("discord_set_class", true);
			Respawn = PluginManager.YamlConfig.GetBool("discord_respawn", true);
			PlayerJoin = PluginManager.YamlConfig.GetBool("discord_player_join", true);
			DoorInteract = PluginManager.YamlConfig.GetBool("discord_doorinteract", false);
			Scp914Upgrade = PluginManager.YamlConfig.GetBool("discord_914upgrade", true);
			Scp079Tesla = PluginManager.YamlConfig.GetBool("discord_079tesla", true);
			Scp106Tele = PluginManager.YamlConfig.GetBool("discord_106teleport", false);
			PocketEnter = PluginManager.YamlConfig.GetBool("discord_penter", true);
			PocketEscape = PluginManager.YamlConfig.GetBool("discord_pescape", true);
			ConsoleCommand = PluginManager.YamlConfig.GetBool("discord_player_console", true);
			Decon = PluginManager.YamlConfig.GetBool("discord_decon", true);
			DropItem = PluginManager.YamlConfig.GetBool("discord_item_drop", true);
			PickupItem = PluginManager.YamlConfig.GetBool("discord_item_pickup", true);
			Banned = PluginManager.YamlConfig.GetBool("discord_banned", true);
			Cuffed = PluginManager.YamlConfig.GetBool("discord_cuffed", true);
			Freed = PluginManager.YamlConfig.GetBool("discord_freed", true);
			Scp914Activation = PluginManager.YamlConfig.GetBool("discord_914_activation", true);
			Scp914KnobChange = PluginManager.YamlConfig.GetBool("discord_914_knob", true);

			Intercom = PluginManager.YamlConfig.GetBool("discord_intercom", true);
			WarheadAccess = PluginManager.YamlConfig.GetBool("discord_warhead_access", true);
			WarheadCancel = PluginManager.YamlConfig.GetBool("discord_warhead_cancel", true);
			WarheadDetonate = PluginManager.YamlConfig.GetBool("discord_warhead_detonate", true);
			WarheadStart = PluginManager.YamlConfig.GetBool("discord_warhead_start", true);
			Elevator = PluginManager.YamlConfig.GetBool("discord_interact_elevator");
			Locker = PluginManager.YamlConfig.GetBool("discord_interact_locker");
			TriggerTesla = PluginManager.YamlConfig.GetBool("discord_interaction_tesla");
			GenClose = PluginManager.YamlConfig.GetBool("discord_generator_closed", true);
			GenOpen = PluginManager.YamlConfig.GetBool("discord_generator_open", true);
			GenEject = PluginManager.YamlConfig.GetBool("discord_generator_eject", true);
			GenInsert = PluginManager.YamlConfig.GetBool("discord_generator_insert", true);
			GenFinish = PluginManager.YamlConfig.GetBool("discord_generator_finish", true);
			GenUnlock = PluginManager.YamlConfig.GetBool("discord_generator_unlock", true);
			Scp106Contain = PluginManager.YamlConfig.GetBool("discord_106_contain", true);
			Scp106Portal = PluginManager.YamlConfig.GetBool("discord_106_createportal", true);
			Scp079Exp = PluginManager.YamlConfig.GetBool("discord_079_expgain", true);
			Scp079Lvl = PluginManager.YamlConfig.GetBool("discord_079_lvlgain", true);
			PlayerLeave = PluginManager.YamlConfig.GetBool("discord_player_leave", true);
			PlayerReload = PluginManager.YamlConfig.GetBool("discord_player_reload");
			SetGroup = PluginManager.YamlConfig.GetBool("discord_setgroup", true);

			Egg = PluginManager.YamlConfig.GetBool("discord_egg_mode", false);
			EggAddress = PluginManager.YamlConfig.GetString("discord_ip_address", string.Empty);
			OnlyFriendlyFire = PluginManager.YamlConfig.GetBool("discord_only_ff", true);
			RoleSync = PluginManager.YamlConfig.GetBool("discord_rolesync", true);
			Debug = PluginManager.YamlConfig.GetBool("discord_debug", false);
        }


        public bool Debug;
        public bool RaCommands = true;
        public bool RoundStart = true;
        public bool RoundEnd = true;
        public bool WaitingForPlayers = true;
        public bool CheaterReport = true;
        public bool PlayerHurt = true;
        public bool PlayerDeath = true;
        public bool GrenadeThrown = true;
        public bool MedicalItem = true;
        public bool SetClass = true;
        public bool Respawn = true;
        public bool PlayerJoin = true;
        public bool DoorInteract = true;
        public bool Scp914Upgrade = true;
        public bool Scp079Tesla = true;
        public bool Scp106Tele = true;
        public bool PocketEnter = true;
        public bool PocketEscape = true;
        public bool ConsoleCommand = true;
        public bool Decon = true;
        public bool DropItem = true;
        public bool PickupItem = true;
        public bool Intercom = true;
        public bool Banned = true;
        public bool Cuffed = true;
        public bool Freed = true;
        public bool Scp914Activation = true;
        public bool Scp914KnobChange = true;
        public bool WarheadCancel;
        public bool WarheadDetonate;
        public bool WarheadStart;
        public bool WarheadAccess;
        public bool Elevator;
        public bool Locker;
        public bool TriggerTesla;
        public bool GenClose;
        public bool GenOpen;
        public bool GenInsert;
        public bool GenEject;
        public bool GenFinish;
        public bool GenUnlock;
        public bool Scp106Contain;
        public bool Scp106Portal;
        public bool ItemChanged;
        public bool Scp079Exp;
        public bool Scp079Lvl;
        public bool PlayerLeave;
        public bool PlayerReload;
        public bool SetGroup;

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


        public bool Egg = false;
        public string EggAddress = "";
        public bool OnlyFriendlyFire = true;
        public bool RoleSync = true;

        public bool IsEnabled { get; set; }
        public string Prefix { get; }
    }
}