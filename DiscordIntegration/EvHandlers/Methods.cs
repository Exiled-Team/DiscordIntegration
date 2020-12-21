using Exiled.API.Features;
using MEC;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using UnityEngine;
using Ping = System.Net.NetworkInformation.Ping;
using DiscordIntegration_Plugin.System;
using Microsoft.CSharp.RuntimeBinder;


namespace DiscordIntegration_Plugin.EvHandlers
{
    public class Methods
    {
        private readonly Plugin plugin;
        public Methods(Plugin plugin) => this.plugin = plugin;
        private static bool refreshTranslationFile;
        private static List<string> propertyNames = new List<string>();

        public static string _configPath = Path.Combine(Paths.Configs, "DiscordIntegration");
        public static string _translationFileName = Path.Combine(_configPath, "translations.json");
        public static string _translationBackupFileName = Path.Combine(_configPath, "translations_backup.json");
        public static string _statsFileNamePath = Path.Combine(_configPath, "stats.json");

        public static Stats stats = new Stats();
        public static int RoundsPlayed { get; set; }
        public static int TKCount { get; set; }
        public static int PlayerTotalDeaths { get; set; }
        public static int PlayerJoinCount { get; set; }

        public static void LoadStats()
        {
            string configPath = Path.Combine(Paths.Configs, "DiscordIntegration");
            string statsFile = Path.Combine(configPath, "stats.json");
            string StatsBackupFile = Path.Combine(configPath, "stats_backup.json");


            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }

            if (!File.Exists(statsFile))
            {
                string defaults = JObject.FromObject(stats).ToString();

                File.WriteAllText(statsFile, defaults);
                return;
            }

            string fileText = File.ReadAllText(statsFile);
            JObject o;

            try
            {
                o = JObject.Parse(fileText);
            }
            catch (Exception e)
            {
                Log.Info("Invalid or corrupted translation file, creating backup and overwriting.");
                Log.Error(e.Message);

                string json = JObject.FromObject(stats).ToString();

                File.Copy(statsFile, StatsBackupFile, true);

                File.WriteAllText(statsFile, json);
                return;
            }

            JsonSerializer j = new JsonSerializer();
            j.Error += Json_Error;

            try
            {
                stats = o.ToObject<Stats>(j);
            }
            catch (Exception e)
            {
                Log.Info("Invalid or corrupted translation file, creating backup and overwriting.");
                Log.Error(e.Message);
                refreshTranslationFile = true;
            }

            if (refreshTranslationFile)
            {
                string json = JObject.FromObject(stats).ToString();

                Log.Info("Invalid or missing translation element detected fixing: " + string.Join(", ", propertyNames) + ".");

                File.Copy(statsFile, StatsBackupFile, true);

                File.WriteAllText(statsFile, json);
                return;
            }
        }
        private static void Json_Error(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs e)
        {
            refreshTranslationFile = true;

            propertyNames.Add(e.ErrorContext.Member.ToString());

            e.ErrorContext.Handled = true;
        }

        public static void UpdateStats(int tkAmount, int roundsPlayed, int totalDeaths, int totalplayerjoincount)
        {
            string json = File.ReadAllText(_statsFileNamePath);
            JObject jObject = JsonConvert.DeserializeObject(json) as JObject;

            jObject.SelectToken("Tk_Amount").Replace($"{tkAmount}");
            jObject.SelectToken("Played_Rounds").Replace($"{roundsPlayed}");
            jObject.SelectToken("Player_TotalDeaths").Replace($"{totalDeaths}");
            jObject.SelectToken("Player_JoinCount").Replace($"{totalplayerjoincount}");

            File.WriteAllText(_statsFileNamePath, 
                JsonConvert.SerializeObject(jObject, Formatting.Indented));
        }

        public static void CheckForSyncRole(Player player)
        {
            Log.Info($"Checking rolesync for {player.UserId}");
            ProcessSTT.SendData($"checksync {player.UserId}", 119);
        }

        public static void SetSyncRole(string group, string steamId)
        {
            Log.Info($"Received setgroup for {steamId}");
            UserGroup userGroup = ServerStatic.GetPermissionsHandler().GetGroup(group);
            if (userGroup == null)
            {
                Log.Error($"Attempted to assign invalid user group {group} to {steamId}");
                return;
            }

            Player player = Player.Get(steamId);
            if (player == null)
            {
                Log.Error($"Error assigning user group to {steamId}, player not found.");
                return;
            }

            Log.Info($"Assigning role: {userGroup} to {steamId}.");
            player.SetRank(userGroup.BadgeText, userGroup);
        }


        public static IEnumerator<float> UpdateServerStatus()
        {
            for (; ; )
            {
                try
                {
                    UpdateStats(TKCount, RoundsPlayed, PlayerTotalDeaths, PlayerJoinCount);

                    int max = GameCore.ConfigFile.ServerConfig.GetInt("max_players", 20);
                    int cur = Player.List.Count();
                    TimeSpan dur = TimeSpan.FromSeconds(Round.ElapsedTime.TotalSeconds);
                    int aliveCount = 0;
                    int scpCount = 0;
                    foreach (Player player in Player.List)
                        if (player.ReferenceHub.characterClassManager.IsHuman())
                            aliveCount++;
                        else if (player.ReferenceHub.characterClassManager.IsAnyScp())
                            scpCount++;
                    ProcessSTT.SendData(
                        $"channelstatus | <:079Agree:767172053718925373> Rondas Jugadas: {RoundsPlayed} | <:RIP:777989450353475654> Muertes Totales: {PlayerTotalDeaths} | <:argentinospordentro:772943936679182406> Contador de TK: {TKCount} | <a:SrLichtDespuesDeUnaActualizacion:746303286537093130> Contador de jugadores que se unieron: {PlayerJoinCount} | <a:nyaAAAAAAAAAAA:788500757313486938> IP: {ServerConsole.Ip}:{ServerConsole.Port} |",
                        119);

                    Log.Info("Actualizando channel topic");
                }
                catch (Exception e)
                {
                    Log.Error($"UpdateServerStatus {e}");
                }

                yield return Timing.WaitForSeconds(360f);
            }
        }

        public static void UpdateIdleStatus()
        {
            for (; ; )
            {

            }
        }

        private static short ResetTicks()
        {
            short t = ticks;
            ticks = 0;
            return t;
        }

        private static short ticks;
        internal static IEnumerator<float> TickCounter()
        {
            for (; ; )
            {
                ticks++;
                yield return Timing.WaitForOneFrame;
            }
        }
    }

    [JsonObject(ItemRequired = Required.Always)]
    public class Stats
    {
        public int Tk_Amount = 0;
        public int Player_TotalDeaths = 0;
        public int Played_Rounds = 0;
        public int Player_JoinCount = 0;


    }

}