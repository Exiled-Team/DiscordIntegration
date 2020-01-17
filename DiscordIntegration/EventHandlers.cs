using System;
using System.Collections.Generic;
using Grenades;
using UnityEngine;

namespace DiscordIntegration_Plugin
{
	public class EventHandlers
	{
		private readonly Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		
		public void OnCommand(ref string query, ref CommandSender sender, ref bool allow)
		{
			if (plugin.RaCommands)
				ProcessSTT.SendData($"{sender.Nickname} used command: {query}", HandleQueue.CommandLogChannelId);
		}

		public void OnWaitingForPlayers()
		{
			if (plugin.WaitingForPlayers)
				ProcessSTT.SendData("Waiting for players...", HandleQueue.GameLogChannelId);
		}

		public void OnRoundStart()
		{
			if (plugin.RoundStart)
				ProcessSTT.SendData($"Round starting: {Plugin.GetHubs().Count} players in round.", HandleQueue.GameLogChannelId);
		}

		public void OnRoundEnd()
		{
			if (plugin.RoundEnd)
				ProcessSTT.SendData($"Round ended: {Plugin.GetHubs().Count} players online.", HandleQueue.GameLogChannelId);
		}

		public void OnCheaterReport(string reporterid, string reportedid, string reportedip, string reason, int serverid, ref bool allow)
		{
			if (plugin.CheaterReport)
				ProcessSTT.SendData($"**Cheater report filed: {reporterid} reported {reportedid} for {reason}.**", HandleQueue.GameLogChannelId);
		}

		public void OnPlayerHurt(PlayerStats stats, ref PlayerStats.HitInfo info, GameObject obj)
		{
			if (plugin.PlayerHurt)
			{
				ReferenceHub target = Plugin.GetPlayer(stats.gameObject);
				ReferenceHub attacker = Plugin.GetPlayer(info.Attacker);
				try
				{
					if (attacker != null && attacker.characterClassManager != null && Plugin.GetTeam(target.characterClassManager.CurClass) == Plugin.GetTeam(attacker.characterClassManager.CurClass))
						ProcessSTT.SendData(
							$"**{attacker.nicknameSync.MyNick} - {attacker.characterClassManager.UserId} ({attacker.characterClassManager.CurClass}) damaged {target.nicknameSync.MyNick} - {target.characterClassManager.UserId} ({target.characterClassManager.CurClass}) for {info.Amount} with {info.Tool}.**",
							HandleQueue.GameLogChannelId);
					else
					{
						ProcessSTT.SendData(
							$"{info.Attacker}  damaged {target.nicknameSync.MyNick} - {target.characterClassManager.UserId} ({target.characterClassManager.CurClass}) for {info.Amount} with {info.Tool}.",
							HandleQueue.GameLogChannelId);
					}
				}
				catch (Exception e)
				{
					Plugin.Error($"Player Hurt error: {e}");
				}
			}
		}
		
		public void OnPlayerDeath(PlayerStats stats, ref PlayerStats.HitInfo info, GameObject obj)
		{
			if (plugin.PlayerDeath)
			{
				ReferenceHub target = Plugin.GetPlayer(stats.gameObject);
				ReferenceHub attacker = Plugin.GetPlayer(info.Attacker);
				try
				{
					if (attacker != null && attacker.characterClassManager != null && Plugin.GetTeam(target.characterClassManager.CurClass) == Plugin.GetTeam(attacker.characterClassManager.CurClass))
						ProcessSTT.SendData(
							$"**{attacker.nicknameSync.MyNick} - {attacker.characterClassManager.UserId} ({attacker.characterClassManager.CurClass}) killed {target.nicknameSync.MyNick} - {target.characterClassManager.UserId} ({target.characterClassManager.CurClass}) with {info.Tool}.**",
							HandleQueue.GameLogChannelId);
					else
						ProcessSTT.SendData(
							$"{info.Attacker} killed {target.nicknameSync.MyNick} - {target.characterClassManager.UserId} ({target.characterClassManager.CurClass}) with {info.Tool}.",
							HandleQueue.GameLogChannelId);
				}
				catch (Exception e)
				{
					Plugin.Error($"Player Death error: {e}");
				}
			}
		}

		public void OnGrenadeThrown(ref GrenadeManager gm, ref int id, ref bool slow, ref double fuse, ref bool allow)
		{
			if (plugin.GrenadeThrown)
			{
				ReferenceHub hub = Plugin.GetPlayer(gm.gameObject);
				if (hub == null)
					return;
				ProcessSTT.SendData($"{hub.nicknameSync.MyNick} - {hub.characterClassManager.UserId} ({hub.characterClassManager.CurClass}) threw a grenade.", HandleQueue.GameLogChannelId);
			}
		}

		public void OnMedicalItem(GameObject obj, ItemType type)
		{
			if (plugin.MedicalItem)
			{
				ReferenceHub hub = Plugin.GetPlayer(obj);
				if (hub == null)
					return;
				ProcessSTT.SendData($"{hub.nicknameSync.MyNick} - {hub.characterClassManager.UserId} ({hub.characterClassManager.CurClass}) user a {type}", HandleQueue.GameLogChannelId);
			}
		}

		public void OnSetClass(CharacterClassManager ccm, RoleType id)
		{
			if (plugin.SetClass)
			{
				ReferenceHub hub = Plugin.GetPlayer(ccm.gameObject);
				if (hub == null)
					return;
				ProcessSTT.SendData($"{hub.nicknameSync.MyNick} - {hub.characterClassManager.UserId} has been changed to a {id}.", HandleQueue.GameLogChannelId);
			}
		}

		public void OnRespawn(ref bool ischaos, ref int maxrespawn, ref List<GameObject> torespawn)
		{
			if (plugin.Respawn)
			{
				string msg;
				if (ischaos)
					msg = "Chaos Insurgency";
				else
					msg = "Nine-Tailed Fox";
				ProcessSTT.SendData($"{msg} has spawned with {torespawn.Count} players.", HandleQueue.GameLogChannelId);
			}
		}

		public void OnPlayerJoin(ReferenceHub hub)
		{
			if (plugin.PlayerJoin)
				if (hub.nicknameSync.MyNick != "Dedicated Server")
					ProcessSTT.SendData($"{hub.nicknameSync.MyNick} - {hub.characterClassManager.UserId} has joined the game.", HandleQueue.GameLogChannelId);
		}
	}
}