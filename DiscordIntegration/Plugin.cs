using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using EXILED;
using GameCore;
using MEC;
using Newtonsoft.Json.Linq;

namespace DiscordIntegration_Plugin
{
	public class Plugin : EXILED.Plugin
	{
		public EventHandlers EventHandlers;

		public bool RaCommands = true;
		public bool RoundStart = true;
		public bool RoundEnd = true;
		public bool WaitingForPlayers = false;
		public bool CheaterReport = false;
		public bool PlayerHurt = true;
		public bool PlayerDeath = true;
		public bool GrenadeThrown = true;
		public bool MedicalItem = true;
		public bool SetClass = false;
		public bool Respawn = true;
		public bool PlayerJoin = true;
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
			Egg = Config.GetBool("discord_egg_mode", false);
			EggAddress = Config.GetString("discord_ip_address", string.Empty);
			OnlyFriendlyFire = Config.GetBool("discord_only_ff", true);
		}

		public static Translation translation = new Translation();

		public void LoadTranslation()
		{
			string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			string pluginsPath = Path.Combine(appData, "Plugins");
			string configPath = Path.Combine(pluginsPath, "Integration");
			string translationFileName = Path.Combine(configPath, "translations.json");

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

			translation = JObject.Parse(fileText).ToObject<Translation>();
		}
		
	}

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
	}
}
