using System;
using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using MEC;
using UnityEngine;

namespace DiscordIntegration_Plugin
{
	public class Methods
	{
		private readonly Plugin plugin;
		public Methods(Plugin plugin) => this.plugin = plugin;

		public static void CheckForSyncRole(ReferenceHub player)
		{
			Log.Info($"Checking rolesync for {player.characterClassManager.UserId}");
			ProcessSTT.SendData($"checksync {player.characterClassManager.UserId}", 119);
		}

		public static void SetSyncRole(string group, string steamId)
		{
			Log.Info($"Received setgroup for {steamId}");
			UserGroup userGroup = ServerStatic.PermissionsHandler.GetGroup(group);
			if (userGroup == null)
			{
				Log.Error($"Attempted to assign invalid user group {group} to {steamId}");
				return;
			}

			ReferenceHub player = Player.GetPlayer(steamId);
			if (player == null)
			{
				Log.Error($"Error assigning user group to {steamId}, player not found.");
				return;
			}
			
			Log.Info($"Assigning role: {userGroup} to {steamId}.");
			player.serverRoles.SetGroup(userGroup, false);
		}

		public static IEnumerator<float> UpdateServerStatus()
		{
			for (;;)
			{
				try
				{
					int max = GameCore.ConfigFile.ServerConfig.GetInt("max_players", 20);
					int cur = PlayerManager.players.Count;
					TimeSpan dur = TimeSpan.FromSeconds(EventPlugin.GetRoundDuration());
					int aliveCount = 0;
					int scpCount = 0;
					foreach (ReferenceHub hub in Player.GetHubs())
						if (hub.characterClassManager.IsHuman())
							aliveCount++;
						else if (hub.characterClassManager.IsAnyScp())
							scpCount++;
					string warhead = Map.IsNukeDetonated ? "Alpha-Warhead detonada." :
						Map.IsNukeInProgress ? "Alpha-Warhead en cuenta regresiva." :
						"Alpha-Warhead sin detonar.";
					ProcessSTT.SendData(
						$"channelstatus -=- Jugadores: {cur}/{max} -=- Tiempo: {Mathf.Floor((float)dur.TotalMinutes)}ms -=- Humanos vivos: {aliveCount} -=- SCPs libres: {scpCount} -=- {warhead} -=- IP: {ServerConsole.Ip}:{ServerConsole.Port} -=- TPS: {ResetTicks() / 10.0f} -=-",
						119);
				}
				catch (Exception e)
				{
					//ignored
				}

				yield return Timing.WaitForSeconds(10f);
			}
		}

		public static void UpdateIdleStatus()
		{
			for (;;)
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
			for (;;)
			{
				ticks++;
				yield return Timing.WaitForOneFrame;
			}
		}
	}
}