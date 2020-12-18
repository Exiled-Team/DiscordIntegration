using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DiscordIntegration_Plugin
{
    public class Methods
    {
        private readonly Plugin plugin;
        public Methods(Plugin plugin) => this.plugin = plugin;

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
                    string warhead = Warhead.IsDetonated ? "<a:explosion:763586070020751370> La Warhead fue detonada." :
                        Warhead.IsInProgress ? "⏲️ Warhead ah iniciado su cuenta regresiva." :
                        "<:079Agree:767172053718925373> Warhead sin detonar.";
                    ProcessSTT.SendData(
                        $"channelstatus Jugadores Online: {cur}/{max}. ⌛ Duracion de la ronda: {Mathf.Floor((float)dur.TotalMinutes)} minutos. <:chiquito:701486171523514381> Humanos vivos: {aliveCount}. <:grenade:732065387511808090>  SCPs con vida: {scpCount}. {warhead} TPS del servidor: {ResetTicks() / 10.0f}",
                        119);
                }
                catch (Exception e)
                {
                    //ignored
                }

                yield return Timing.WaitForSeconds(170f);
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