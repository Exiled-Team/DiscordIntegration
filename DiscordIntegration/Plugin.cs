using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Exiled.API.Interfaces;
using Exiled.API.Features;
using GameCore;
using MEC;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Log = Exiled.API.Features.Log;
using Handlers = Exiled.Events.Handlers;

namespace DiscordIntegration_Plugin
{
	public class Plugin : Exiled.API.Features.Plugin<Config>
	{
		public MapEvents MapEvents;
		public ServerEvents ServerEvents;
		public PlayerEvents PlayerEvents;
		public static Plugin Singleton;
		public int MaxPlayers = ConfigFile.ServerConfig.GetInt("max_players", 20);

		public override void OnEnabled()
		{
			Singleton = this;
			Timing.RunCoroutine(Methods.TickCounter(), Segment.Update, "ticks");
			MapEvents = new MapEvents(this);
			ServerEvents = new ServerEvents(this);
			PlayerEvents = new PlayerEvents(this);

			Handlers.Map.Decontaminating += MapEvents.OnDecon;
			Handlers.Map.GeneratorActivated += MapEvents.OnGenFinish;
			Handlers.Warhead.Starting += MapEvents.OnWarheadStart;
			Handlers.Warhead.Stopping += MapEvents.OnWarheadCancelled;
			Handlers.Warhead.Detonated += MapEvents.OnWarheadDetonation;
			Handlers.Scp914.UpgradingItems += MapEvents.OnScp194Upgrade;

			Handlers.Server.SendingRemoteAdminCommand += ServerEvents.OnCommand;
			Handlers.Server.WaitingForPlayers += ServerEvents.OnWaitingForPlayers;
			Handlers.Server.SendingConsoleCommand += ServerEvents.OnConsoleCommand;
			Handlers.Server.RoundStarted += ServerEvents.OnRoundStart;
			Handlers.Server.RoundEnded += ServerEvents.OnRoundEnd;
			Handlers.Server.RespawningTeam += ServerEvents.OnRespawn;
			Handlers.Server.ReportingCheater += ServerEvents.OnCheaterReport;

			Handlers.Scp914.ChangingKnobSetting += PlayerEvents.On914KnobChange;
			Handlers.Player.MedicalItemUsed += PlayerEvents.OnMedicalItem;
			Handlers.Scp079.InteractingTesla += PlayerEvents.On079Tesla;
			Handlers.Player.PickingUpItem += PlayerEvents.OnPickupItem;
			Handlers.Player.InsertingGeneratorTablet += PlayerEvents.OnGenInsert;
			Handlers.Player.EjectingGeneratorTablet += PlayerEvents.OnGenEject;
			Handlers.Player.UnlockingGenerator += PlayerEvents.OnGenUnlock;
			Handlers.Player.OpeningGenerator += PlayerEvents.OnGenOpen;
			Handlers.Player.ClosingGenerator += PlayerEvents.OnGenClosed;
			Handlers.Scp079.GainingLevel += PlayerEvents.On079GainLvl;
			Handlers.Scp079.GainingExperience += PlayerEvents.On079GainExp;
			Handlers.Player.EscapingPocketDimension += PlayerEvents.OnPocketEscape;
			Handlers.Player.EnteringPocketDimension += PlayerEvents.OnPocketEnter;
			Handlers.Scp106.CreatingPortal += PlayerEvents.On106CreatePortal;
			Handlers.Player.ActivatingWarheadPanel += PlayerEvents.OnWarheadAccess;
			Handlers.Player.TriggeringTesla += PlayerEvents.OnTriggerTesla;
			Handlers.Map.ExplodingGrenade += PlayerEvents.OnGrenadeThrown;
			Handlers.Player.Hurting += PlayerEvents.OnPlayerHurt;
			Handlers.Player.Dying += PlayerEvents.OnPlayerDeath;
			//Handlers.Player.Banned += PlayerEvents.OnPlayerBanned;
			Handlers.Player.InteractingDoor += PlayerEvents.OnDoorInteract;
			Handlers.Player.InteractingElevator += PlayerEvents.OnElevatorInteraction;
			Handlers.Player.InteractingLocker += PlayerEvents.OnLockerInteraction;
			Handlers.Player.IntercomSpeaking += PlayerEvents.OnIntercomSpeak;
			Handlers.Player.Handcuffing += PlayerEvents.OnPlayerHandcuffed;
			Handlers.Player.RemovingHandcuffs += PlayerEvents.OnPlayerFreed;
			Handlers.Scp106.Teleporting += PlayerEvents.On106Teleport;
			Handlers.Player.ReloadingWeapon += PlayerEvents.OnPlayerReload;
			Handlers.Player.ItemDropped += PlayerEvents.OnDropItem;
			Handlers.Player.Joined += PlayerEvents.OnPlayerJoin;
			Handlers.Player.Left += PlayerEvents.OnPlayerLeave;
			Handlers.Player.ChangingRole += PlayerEvents.OnSetClass;
			//Handlers.Player.ChangingGroup += PlayerEvents.OnSetGroup;
			Handlers.Player.ChangingItem += PlayerEvents.OnItemChanged;
			Handlers.Scp914.Activating += PlayerEvents.On914Activation;
			Handlers.Scp106.Containing += PlayerEvents.On106Contain;

			LoadTranslation();

			new Thread(ProcessSTT.Init).Start();
			Timing.RunCoroutine(HandleQueue.Handle(), "handle");
			Timing.RunCoroutine(UpdateStatus(), "update");
			Timing.RunCoroutine(Methods.UpdateServerStatus(), "updatechan");
		}

		public override void OnDisabled()
		{
			Handlers.Map.Decontaminating -= MapEvents.OnDecon;
			Handlers.Map.GeneratorActivated -= MapEvents.OnGenFinish;
			Handlers.Warhead.Starting -= MapEvents.OnWarheadStart;
			Handlers.Warhead.Stopping -= MapEvents.OnWarheadCancelled;
			Handlers.Warhead.Detonated -= MapEvents.OnWarheadDetonation;
			Handlers.Scp914.UpgradingItems -= MapEvents.OnScp194Upgrade;

			Handlers.Server.SendingRemoteAdminCommand -= ServerEvents.OnCommand;
			Handlers.Server.WaitingForPlayers -= ServerEvents.OnWaitingForPlayers;
			Handlers.Server.SendingConsoleCommand -= ServerEvents.OnConsoleCommand;
			Handlers.Server.RoundStarted -= ServerEvents.OnRoundStart;
			Handlers.Server.RoundEnded -= ServerEvents.OnRoundEnd;
			Handlers.Server.RespawningTeam -= ServerEvents.OnRespawn;
			Handlers.Server.ReportingCheater -= ServerEvents.OnCheaterReport;

			Handlers.Scp914.ChangingKnobSetting -= PlayerEvents.On914KnobChange;
			Handlers.Player.MedicalItemUsed -= PlayerEvents.OnMedicalItem;
			Handlers.Scp079.InteractingTesla -= PlayerEvents.On079Tesla;
			Handlers.Player.PickingUpItem -= PlayerEvents.OnPickupItem;
			Handlers.Player.InsertingGeneratorTablet -= PlayerEvents.OnGenInsert;
			Handlers.Player.EjectingGeneratorTablet -= PlayerEvents.OnGenEject;
			Handlers.Player.UnlockingGenerator -= PlayerEvents.OnGenUnlock;
			Handlers.Player.OpeningGenerator -= PlayerEvents.OnGenOpen;
			Handlers.Player.ClosingGenerator -= PlayerEvents.OnGenClosed;
			Handlers.Scp079.GainingLevel -= PlayerEvents.On079GainLvl;
			Handlers.Scp079.GainingExperience -= PlayerEvents.On079GainExp;
			Handlers.Player.EscapingPocketDimension -= PlayerEvents.OnPocketEscape;
			Handlers.Player.EnteringPocketDimension -= PlayerEvents.OnPocketEnter;
			Handlers.Scp106.CreatingPortal -= PlayerEvents.On106CreatePortal;
			Handlers.Player.ActivatingWarheadPanel -= PlayerEvents.OnWarheadAccess;
			Handlers.Player.TriggeringTesla -= PlayerEvents.OnTriggerTesla;
			Handlers.Map.ExplodingGrenade -= PlayerEvents.OnGrenadeThrown;
			Handlers.Player.Hurting -= PlayerEvents.OnPlayerHurt;
			Handlers.Player.Dying -= PlayerEvents.OnPlayerDeath;
			//Handlers.Player.Banned -= PlayerEvents.OnPlayerBanned;
			Handlers.Player.InteractingDoor -= PlayerEvents.OnDoorInteract;
			Handlers.Player.InteractingElevator -= PlayerEvents.OnElevatorInteraction;
			Handlers.Player.InteractingLocker -= PlayerEvents.OnLockerInteraction;
			Handlers.Player.IntercomSpeaking -= PlayerEvents.OnIntercomSpeak;
			Handlers.Player.Handcuffing -= PlayerEvents.OnPlayerHandcuffed;
			Handlers.Player.RemovingHandcuffs -= PlayerEvents.OnPlayerFreed;
			Handlers.Scp106.Teleporting -= PlayerEvents.On106Teleport;
			Handlers.Player.ReloadingWeapon -= PlayerEvents.OnPlayerReload;
			Handlers.Player.ItemDropped -= PlayerEvents.OnDropItem;
			Handlers.Player.Joined -= PlayerEvents.OnPlayerJoin;
			Handlers.Player.Left -= PlayerEvents.OnPlayerLeave;
			Handlers.Player.ChangingRole -= PlayerEvents.OnSetClass;
			//Handlers.Player.ChangingGroup -= PlayerEvents.OnSetGroup;
			Handlers.Player.ChangingItem -= PlayerEvents.OnItemChanged;
			Handlers.Scp914.Activating -= PlayerEvents.On914Activation;
			Handlers.Scp106.Containing -= PlayerEvents.On106Contain;

			PlayerEvents = null;
			MapEvents = null;
			ServerEvents = null;

			Timing.KillCoroutines("handle");
			Timing.KillCoroutines("update");
			Timing.KillCoroutines("updatechan");
			Timing.KillCoroutines("ticks");
		}

		public IEnumerator<float> UpdateStatus()
		{
			for (; ; )
			{
				ProcessSTT.SendData($"updateStatus {PlayerManager.players.Count}/{ConfigFile.ServerConfig.GetInt("max_players")}", 0);
				yield return Timing.WaitForSeconds(5f);
			}
		}

		public override void OnReloaded()
		{
			OnDisabled();
			OnEnabled();

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
