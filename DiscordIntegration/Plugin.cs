using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using EXILED;
using GameCore;
using MEC;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
		public bool Scp914Activation = true;
		public bool Scp914KnobChange = true;
		
		public static bool Egg = false;
		public static string EggAddress = "";
		public bool OnlyFriendlyFire = true;
		public bool RoleSync = true;
		
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
			Events.Scp914ActivationEvent += EventHandlers.On914Activation;
			Events.Scp914KnobChangeEvent += EventHandlers.On914KnobChange;

			LoadTranslation();

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
			Events.DoorInteractEvent -= EventHandlers.OnDoorInteract;
			Events.Scp914UpgradeEvent -= EventHandlers.OnScp194Upgrade;
			Events.Scp079TriggerTeslaEvent -= EventHandlers.On079Tesla;
			Events.Scp106TeleportEvent -= EventHandlers.On106Teleport;
			Events.PocketDimEscapedEvent -= EventHandlers.OnPocketEscape;
			Events.PocketDimEnterEvent -= EventHandlers.OnPocketEnter;
			Events.ConsoleCommandEvent -= EventHandlers.OnConsoleCommand;
			Events.DecontaminationEvent -= EventHandlers.OnDecon;
			Events.DropItemEvent -= EventHandlers.OnDropItem;
			Events.PickupItemEvent -= EventHandlers.OnPickupItem;
			Events.IntercomSpeakEvent -= EventHandlers.OnIntercomSpeak;
			Events.PlayerBannedEvent -= EventHandlers.OnPlayerBanned;
			Events.PlayerHandcuffedEvent -= EventHandlers.OnPlayerHandcuffed;
			Events.PlayerHandcuffFreedEvent -= EventHandlers.OnPlayerFreed;
			Events.Scp914ActivationEvent -= EventHandlers.On914Activation;
			Events.Scp914KnobChangeEvent -= EventHandlers.On914KnobChange;
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
			Scp914Activation = Config.GetBool("discord_914_activation", true);
			Scp914KnobChange = Config.GetBool("discord_914_knob", true);
			Egg = Config.GetBool("discord_egg_mode", false);
			EggAddress = Config.GetString("discord_ip_address", string.Empty);
			OnlyFriendlyFire = Config.GetBool("discord_only_ff", true);
			RoleSync = Config.GetBool("discord_rolesync", true);
		}

		public static Translation translation = new Translation();
		private bool refreshTranslationFile = false;
		private List<string> propertyNames = new List<string>();

		public void LoadTranslation()
		{
			string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			string pluginsPath = Path.Combine(appData, "Plugins");
			string configPath = Path.Combine(pluginsPath, "Integration");
			string translationFileName = Path.Combine(configPath, "translations.json");
			string translationBackupFileName = Path.Combine(configPath, "translations_backup.json");

			if (!Directory.Exists(configPath))
			{
				Directory.CreateDirectory(configPath);
			}

			if (!File.Exists(translationFileName))
			{
				string defaults = JObject.FromObject(translation).ToString();

				File.WriteAllText(translationFileName, defaults);
				return;
			}

			string fileText = File.ReadAllText(translationFileName);
			JObject o;

			try
			{
				o = JObject.Parse(fileText);
			}
			catch (Exception e)
			{
				Info("Invalid or corrupted translation file, creating backup and overwriting.");
				Error(e.Message);

				string json = JObject.FromObject(translation).ToString();

				File.Copy(translationFileName, translationBackupFileName, true);

				File.WriteAllText(translationFileName, json);
				return;
			}
			
			JsonSerializer j = new JsonSerializer();
			j.Error += Json_Error;

			try
			{
				translation = o.ToObject<Translation>(j);
			}
			catch (Exception e)
			{
				Info("Invalid or corrupted translation file, creating backup and overwriting.");
				Error(e.Message);
				refreshTranslationFile = true;
			}

			if (refreshTranslationFile)
			{
				string json = JObject.FromObject(translation).ToString();

				Info("Invalid or missing translation element detected fixing: " + string.Join(", ", propertyNames) + ".");

				File.Copy(translationFileName, translationBackupFileName, true);

				File.WriteAllText(translationFileName, json);
				return;
			}
		}

		private void Json_Error(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs e)
		{
			refreshTranslationFile = true;

			propertyNames.Add(e.ErrorContext.Member.ToString());

			e.ErrorContext.Handled = true;
		}
	}

	[JsonObject(ItemRequired = Required.Always)]
	public class Translation
	{
		public string usedCommand = "used command";
		public string noPlayersOnline = "No players online.";
		public string noStaffOnline = "No staff online.";
		public string waitingForPlayers = "Waiting for players...";
		public string roundStarting = "Round starting";
		public string playersInRound = "players in round";
		public string roundEnded = "Round ended";
		public string playersOnline = "players online";
		public string cheaterReportFiled = "Cheater report filed";
		public string reported = "reported";
		[JsonProperty("for")]
		public string _for = "for";
		public string with = "with";
		public string damaged = "damaged";
		public string killed = "killed";
		public string threwAGrenade = "threw a grenade";
		public string userA = "user a";
		public string hasBenChangedToA = "has been changed to a";
		public string chaosInsurgency = "Chaos Insurgency";
		public string nineTailedFox = "Nine-Tailed Fox";
		public string hasSpawnedWith = "has spawned with";
		public string players = "players";
		public string hasJoinedTheGame = "has joined the game";
		public string hasBeenFreedBy = "has been freed by";
		public string hasBeenHandcuffedBy = "has been handcuffed by";
		public string wasBannedBy = "was banned by";
		public string hasStartedUsingTheIntercom = "has started using the intercom";
		public string hasPickedUp = "has picked up";
		public string hasDropped = "has dropped";
		public string decontaminationHasBegun = "Deconamination has begun";
		public string hasRunClientConsoleCommand = "has run a client-console command";
		public string hasEnteredPocketDimension = "has entered the pocket dimension";
		public string hasEscapedPocketDimension = "has escaped the pocket dimension";
		public string hasTraveledThroughTheirPortal = "has traveled through their portal";
		public string hasTriggeredATeslaGate = "has triggered a tesla gate";
		public string scp914HasProcessedTheFollowingPlayers = "SCP-914 has processed the following players";
		public string andItems = "and items";
		public string hasClosedADoor = "has closed a door";
		public string hasOpenedADoor = "has opened a door";
		public string scp914HasBeenActivated = "has activated SCP-914 on setting";
		public string scp914knobchange = "has changed the SCP-914 knob to";
	}
}
