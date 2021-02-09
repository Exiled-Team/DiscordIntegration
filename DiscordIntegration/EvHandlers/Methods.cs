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
                        $"channelstatus | <a:nyaAAAAAAAAAAA:788500757313486938> IP: {ServerConsole.Ip}:{ServerConsole.Port} | <a:popcat:796825671913046027> Jugadores: {cur}/{max}| <:079Agree:767172053718925373> SCPs vivos: {scpCount} | <:ClassDSadge:808671027051757640> Humanos vivos: {aliveCount} |",
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

}