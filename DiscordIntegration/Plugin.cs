using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Threading;
using EXILED;
using EXILED.Extensions;
using GameCore;
using MEC;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Log = EXILED.Log;

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
		
		public static bool Egg = false;
		public static string EggAddress = "";
		public bool OnlyFriendlyFire = true;
		public bool RoleSync = true;
		
		public override void OnEnable()
		{
			RefreshConfig();
			Timing.RunCoroutine(Methods.TickCounter(), Segment.Update, "ticks");
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
			
			Events.WarheadCancelledEvent += EventHandlers.OnWarheadCancelled;
			Events.WarheadDetonationEvent += EventHandlers.OnWarheadDetonation;
			Events.WarheadStartEvent += EventHandlers.OnWarheadStart;
			Events.WarheadKeycardAccessEvent += EventHandlers.OnWarheadAccess;
			Events.ElevatorInteractEvent += EventHandlers.OnElevatorInteraction;
			Events.LockerInteractEvent += EventHandlers.OnLockerInteraction;
			Events.TriggerTeslaEvent += EventHandlers.OnTriggerTesla;
			Events.GeneratorClosedEvent += EventHandlers.OnGenClosed;
			Events.GeneratorEjectedEvent += EventHandlers.OnGenEject;
			Events.GeneratorFinishedEvent += EventHandlers.OnGenFinish;
			Events.GeneratorInsertedEvent += EventHandlers.OnGenInsert;
			Events.GeneratorOpenedEvent += EventHandlers.OnGenOpen;
			Events.GeneratorUnlockEvent += EventHandlers.OnGenUnlock;
			Events.Scp106ContainEvent += EventHandlers.On106Contain;
			Events.Scp106CreatedPortalEvent += EventHandlers.On106CreatePortal;
			Events.ItemChangedEvent += EventHandlers.OnItemChanged;
			Events.Scp079ExpGainEvent += EventHandlers.On079GainExp;
			Events.Scp079LvlGainEvent += EventHandlers.On079GainLvl;
			Events.PlayerLeaveEvent += EventHandlers.OnPlayerLeave;
			Events.PlayerReloadEvent += EventHandlers.OnPlayerReload;
			Events.SetGroupEvent += EventHandlers.OnSetGroup;

			LoadTranslation();

			new Thread(ProcessSTT.Init).Start();
			Timing.RunCoroutine(HandleQueue.Handle(), "handle");
			Timing.RunCoroutine(UpdateStatus(), "update");
			Timing.RunCoroutine(Methods.UpdateServerStatus(), "updatechan");
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
			Events.WarheadCancelledEvent -= EventHandlers.OnWarheadCancelled;
			Events.WarheadDetonationEvent -= EventHandlers.OnWarheadDetonation;
			Events.WarheadStartEvent -= EventHandlers.OnWarheadStart;
			Events.WarheadKeycardAccessEvent -= EventHandlers.OnWarheadAccess;
			Events.ElevatorInteractEvent -= EventHandlers.OnElevatorInteraction;
			Events.LockerInteractEvent -= EventHandlers.OnLockerInteraction;
			Events.TriggerTeslaEvent -= EventHandlers.OnTriggerTesla;
			Events.GeneratorClosedEvent -= EventHandlers.OnGenClosed;
			Events.GeneratorEjectedEvent -= EventHandlers.OnGenEject;
			Events.GeneratorFinishedEvent -= EventHandlers.OnGenFinish;
			Events.GeneratorInsertedEvent -= EventHandlers.OnGenInsert;
			Events.GeneratorOpenedEvent -= EventHandlers.OnGenOpen;
			Events.GeneratorUnlockEvent -= EventHandlers.OnGenUnlock;
			Events.Scp106ContainEvent -= EventHandlers.On106Contain;
			Events.Scp106CreatedPortalEvent -= EventHandlers.On106CreatePortal;
			Events.ItemChangedEvent -= EventHandlers.OnItemChanged;
			Events.Scp079ExpGainEvent -= EventHandlers.On079GainExp;
			Events.Scp079LvlGainEvent -= EventHandlers.On079GainLvl;
			Events.PlayerLeaveEvent -= EventHandlers.OnPlayerLeave;
			Events.PlayerReloadEvent -= EventHandlers.OnPlayerReload;
			Events.SetGroupEvent -= EventHandlers.OnSetGroup;
			EventHandlers = null;
			Timing.KillCoroutines("handle");
			Timing.KillCoroutines("update");
			Timing.KillCoroutines("updatechan");
			Timing.KillCoroutines("ticks");
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

			Intercom = Config.GetBool("discord_intercom", true);
			WarheadAccess = Config.GetBool("discord_warhead_access", true);
			WarheadCancel = Config.GetBool("discord_warhead_cancel", true);
			WarheadDetonate = Config.GetBool("discord_warhead_detonate", true);
			WarheadStart = Config.GetBool("discord_warhead_start", true);
			Elevator = Config.GetBool("discord_interact_elevator");
			Locker = Config.GetBool("discord_interact_locker");
			TriggerTesla = Config.GetBool("discord_interaction_tesla");
			GenClose = Config.GetBool("discord_generator_closed", true);
			GenOpen = Config.GetBool("discord_generator_open", true);
			GenEject = Config.GetBool("discord_generator_eject", true);
			GenInsert = Config.GetBool("discord_generator_insert", true);
			GenFinish = Config.GetBool("discord_generator_finish", true);
			GenUnlock = Config.GetBool("discord_generator_unlock", true);
			Scp106Contain = Config.GetBool("discord_106_contain", true);
			Scp106Portal = Config.GetBool("discord_106_createportal", true);
			Scp079Exp = Config.GetBool("discord_079_expgain", true);
			Scp079Lvl = Config.GetBool("discord_079_lvlgain", true);
			PlayerLeave = Config.GetBool("discord_player_leave", true);
			PlayerReload = Config.GetBool("discord_player_reload");
			SetGroup = Config.GetBool("discord_setgroup", true);

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
				Log.Info("Invalid or corrupted translation file, creating backup and overwriting.");
				Log.Error(e.Message);

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
				Log.Info("Invalid or corrupted translation file, creating backup and overwriting.");
				Log.Error(e.Message);
				refreshTranslationFile = true;
			}

			if (refreshTranslationFile)
			{
				string json = JObject.FromObject(translation).ToString();

				Log.Info("Invalid or missing translation element detected fixing: " + string.Join(", ", propertyNames) + ".");

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
		public string UsedCommand = "used command";
		public string NoPlayersOnline = "No players online.";
		public string NoStaffOnline = "No staff online.";
		public string WaitingForPlayers = "Waiting for players...";
		public string RoundStarting = "Round starting";
		public string PlayersInRound = "players in round";
		public string RoundEnded = "Round ended";
		public string PlayersOnline = "players online";
		public string CheaterReportFiled = "Cheater report filed";
		public string Reported = "reported";
		[JsonProperty("for")]
		public string _For = "for";
		public string With = "with";
		public string Damaged = "damaged";
		public string Killed = "killed";
		public string ThrewAGrenade = "threw a grenade";
		public string UsedA = "used a";
		public string HasBenChangedToA = "has been changed to a";
		public string ChaosInsurgency = "Chaos Insurgency";
		public string NineTailedFox = "Nine-Tailed Fox";
		public string HasSpawnedWith = "has spawned with";
		public string Players = "players";
		public string HasJoinedTheGame = "has joined the game";
		public string HasBeenFreedBy = "has been freed by";
		public string HasBeenHandcuffedBy = "has been handcuffed by";
		public string WasBannedBy = "was banned by";
		public string HasStartedUsingTheIntercom = "has started using the intercom";
		public string HasPickedUp = "has picked up";
		public string HasDropped = "has dropped";
		public string DecontaminationHasBegun = "Deconamination has begun";
		public string HasRunClientConsoleCommand = "has run a client-console command";
		public string HasEnteredPocketDimension = "has entered the pocket dimension";
		public string HasEscapedPocketDimension = "has escaped the pocket dimension";
		public string HasTraveledThroughTheirPortal = "has traveled through their portal";
		public string HasTriggeredATeslaGate = "has triggered a tesla gate";
		public string Scp914HasProcessedTheFollowingPlayers = "SCP-914 has processed the following players";
		public string AndItems = "and items";
		public string HasClosedADoor = "has closed a door";
		public string HasOpenedADoor = "has opened a door";
		public string Scp914HasBeenActivated = "has activated SCP-914 on setting";
		public string Scp914Knobchange = "has changed the SCP-914 knob to";
		public string CancelledWarhead = "has cancelled the warhead";
		public string WarheadDetonated = "***The Alpha-warhead has detonated***";
		public string WarheadStarted = "Alpha-warhead countdown initiated, detonation in:";
		public string AccessedWarhead = "has accessed the Alpha-warhead detonation button cover";
		public string CalledElevator = "has called an elevator";
		public string UsedLocker = "has opened a locker";
		public string TriggeredTesla = "has triggered a tesla gate";
		public string GenClosed = "has closed a generator";
		public string GenOpened = "has opened a generator";
		public string GenEjected = "has ejected a tablet from a generator";
		public string GenFinished = "A generator has finished it's charge up";
		public string GenInserted = "has inserted a tablet into a generator";
		public string GenUnlocked = "has unlocked a generator door";
		public string WasContained = "has been contained by the Femur Breaker";
		public string CreatedPortal = "has created a portal";
		public string GainedExp = "has gained experience";
		public string GainedLevel = "has gained a level";
		public string LeftServer = "has left the server";
		public string Reloaded = "has reloaded their weapon";
		public string GroupSet = "has been assigned a group";
		public string ItemChanged = "changed the item in their hand";
	}
}
