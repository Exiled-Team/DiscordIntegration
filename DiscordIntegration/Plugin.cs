using System.Collections.Generic;
using System.Threading;
using EXILED;
using GameCore;
using MEC;

namespace DiscordIntegration_Plugin
{
	public class Plugin : EXILED.Plugin
	{
		public EventHandlers EventHandlers;

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
		
		public static bool Egg = false;
		public static string EggAddress = "";
		public bool OnlyFriendlyFire = true;
		
		public override void OnEnable()
		{
			RefreshConfig();
			EventHandlers = new EventHandlers(this);
			Events.RemoteAdminCommandEvent += EventHandlers.OnCommand;
			Events.RoundStartEvent += EventHandlers.OnRoundStart;
			Events.RoundEndEvent += EventHandlers.OnRoundEnd;
			Events.WaitingForPlayersEvent += EventHandlers.OnWaitingForPlayers;
			Events.CheaterReportEvent += EventHandlers.OnCheaterReport;
			Events.PlayerHurtEvent += EventHandlers.OnPlayerHurt;
			Events.PlayerDeathEvent += EventHandlers.OnPlayerDeath;
			Events.GrenadeThrownEvent += EventHandlers.OnGrenadeThrown;
			Events.UseMedicalItemEvent += EventHandlers.OnMedicalItem;
			Events.SetClassEvent += EventHandlers.OnSetClass;
			Events.TeamRespawnEvent += EventHandlers.OnRespawn;
			Events.PlayerJoinEvent += EventHandlers.OnPlayerJoin;
			
			Events.DoorInteractEvent += EventHandlers.OnDoorInteract;
			Events.Scp914UpgradeEvent += EventHandlers.OnScp194Upgrade;
			Events.Scp079TriggerTeslaEvent += EventHandlers.On079Tesla;
			Events.Scp106TeleportEvent += EventHandlers.On106Teleport;
			Events.PocketDimEscapedEvent += EventHandlers.OnPocketEscape;
			Events.PocketDimEnterEvent += EventHandlers.OnPocketEnter;
			Events.ConsoleCommandEvent += EventHandlers.OnConsoleCommand;
			Events.DecontaminationEvent += EventHandlers.OnDecon;
			Events.DropItemEvent += EventHandlers.OnDropItem;
			Events.PickupItemEvent += EventHandlers.OnPickupItem;
			Events.IntercomSpeakEvent += EventHandlers.OnIntercomSpeak;
			Events.PlayerBannedEvent += EventHandlers.OnPlayerBanned;
			Events.PlayerHandcuffedEvent += EventHandlers.OnPlayerHandcuffed;
			Events.PlayerHandcuffFreedEvent += EventHandlers.OnPlayerFreed;

			new Thread(ProcessSTT.Init).Start();
			Timing.RunCoroutine(HandleQueue.Handle(), "handle");
			Timing.RunCoroutine(UpdateStatus(), "update");
		}

		public override void OnDisable()
		{
			Events.RemoteAdminCommandEvent -= EventHandlers.OnCommand;
			Events.RoundStartEvent -= EventHandlers.OnRoundStart;
			Events.RoundEndEvent -= EventHandlers.OnRoundEnd;
			Events.WaitingForPlayersEvent -= EventHandlers.OnWaitingForPlayers;
			Events.CheaterReportEvent -= EventHandlers.OnCheaterReport;
			Events.PlayerHurtEvent -= EventHandlers.OnPlayerHurt;
			Events.PlayerDeathEvent -= EventHandlers.OnPlayerDeath;
			Events.GrenadeThrownEvent -= EventHandlers.OnGrenadeThrown;
			Events.UseMedicalItemEvent -= EventHandlers.OnMedicalItem;
			Events.SetClassEvent -= EventHandlers.OnSetClass;
			Events.TeamRespawnEvent -= EventHandlers.OnRespawn;
			Events.PlayerJoinEvent -= EventHandlers.OnPlayerJoin;
			EventHandlers = null;
			Timing.KillCoroutines("handle");
			Timing.KillCoroutines("update");
		}
		
		public IEnumerator<float> UpdateStatus()
		{
			for (;;)
			{
				ProcessSTT.SendData($"updateStatus {PlayerManager.players.Count}/{ConfigFile.ServerConfig.GetInt("max_players")}", 0);
				yield return Timing.WaitForSeconds(5f);
			}
		}

		public override void OnReload()
		{
			
		}

		public override string getName { get; } = "Discord Integration";

		public void RefreshConfig()
		{
			RaCommands = Config.GetBool("discord_ra_commands", true);
			RoundStart = Config.GetBool("discord_round_start", true);
			RoundEnd = Config.GetBool("discord_round_end", true);
			WaitingForPlayers = Config.GetBool("discord_waiting_for_players", true);
			CheaterReport = Config.GetBool("discord_cheater_report", true);
			PlayerHurt = Config.GetBool("discord_player_hurt", true);
			PlayerDeath = Config.GetBool("discord_player_death", true);
			GrenadeThrown = Config.GetBool("discord_grenade_thrown", true);
			MedicalItem = Config.GetBool("discord_medical_item", true);
			SetClass = Config.GetBool("discord_set_class", true);
			Respawn = Config.GetBool("discord_respawn", true);
			PlayerJoin = Config.GetBool("discord_player_join", true);
			DoorInteract = Config.GetBool("discord_doorinteract", false);
			Scp914Upgrade = Config.GetBool("discord_914upgrade", true);
			Scp079Tesla = Config.GetBool("discord_079tesla", true);
			Scp106Tele = Config.GetBool("discord_106teleport", false);
			PocketEnter = Config.GetBool("discord_penter", true);
			PocketEscape = Config.GetBool("discord_pescape", true);
			ConsoleCommand = Config.GetBool("discord_player_console", true);
			Decon = Config.GetBool("discord_decon", true);
			DropItem = Config.GetBool("discord_item_drop", true);
			PickupItem = Config.GetBool("discord_item_pickup", true);
			Banned = Config.GetBool("discord_banned", true);
			Cuffed = Config.GetBool("discord_cuffed", true);
			Freed = Config.GetBool("discord_freed", true);
			Egg = Config.GetBool("discord_egg_mode", false);
			EggAddress = Config.GetString("discord_ip_address", string.Empty);
			OnlyFriendlyFire = Config.GetBool("discord_only_ff", true);
		}
		
	}
}