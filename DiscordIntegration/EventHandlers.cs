using System;
using System.Collections.Generic;
using EXILED;
using GameCore;
using Grenades;
using MEC;
using UnityEngine;

namespace DiscordIntegration_Plugin
{
	public class EventHandlers
	{
		private readonly Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		
		public void OnCommand(ref RACommandEvent ev)
		{
			if (plugin.RaCommands)
				ProcessSTT.SendData($"{ev.Sender.Nickname} used command: {ev.Command}", HandleQueue.CommandLogChannelId);
			if (ev.Command.ToLower() == "list")
			{
				ev.Allow = false;
				string message = "";
				foreach (ReferenceHub hub in Plugin.GetHubs())
					message += $"{hub.nicknameSync.MyNick} ({hub.characterClassManager.UserId})";
				if (string.IsNullOrEmpty(message))
					message = "No players online.";
				ev.Sender.RAMessage(message);
			}
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

		public void OnCheaterReport(ref CheaterReportEvent ev)
		{
			if (plugin.CheaterReport)
				ProcessSTT.SendData($"**Cheater report filed: {ev.ReporterId} reported {ev.ReportedId} for {ev.Report}.**", HandleQueue.GameLogChannelId);
		}

		public void OnPlayerHurt(ref PlayerHurtEvent ev)
		{
			if (plugin.PlayerHurt)
			{
				try
				{
					if (ev.Attacker != null && ev.Attacker.characterClassManager != null && Plugin.GetTeam(ev.Player.characterClassManager.CurClass) == Plugin.GetTeam(ev.Attacker.characterClassManager.CurClass))
						ProcessSTT.SendData(
							$"**{ev.Attacker.nicknameSync.MyNick} - {ev.Attacker.characterClassManager.UserId} ({ev.Attacker.characterClassManager.CurClass}) damaged {ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) for {ev.Info.Amount} with {ev.Info.Tool}.**",
							HandleQueue.GameLogChannelId);
					else
					{
						ProcessSTT.SendData(
							$"{ev.Info.Attacker}  damaged {ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) for {ev.Info.Amount} with {ev.Info.Tool}.",
							HandleQueue.GameLogChannelId);
					}
				}
				catch (Exception e)
				{
					Plugin.Error($"Player Hurt error: {e}");
				}
			}
		}
		
		public void OnPlayerDeath(ref PlayerDeathEvent ev)
		{
			if (plugin.PlayerDeath)
			{
				try
				{
					if (ev.Killer != null && ev.Killer.characterClassManager != null && Plugin.GetTeam(ev.Player.characterClassManager.CurClass) == Plugin.GetTeam(ev.Killer.characterClassManager.CurClass))
						ProcessSTT.SendData(
							$"**{ev.Killer.nicknameSync.MyNick} - {ev.Killer.characterClassManager.UserId} ({ev.Killer.characterClassManager.CurClass}) killed {ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) with {ev.Info.Tool}.**",
							HandleQueue.GameLogChannelId);
					else
					{
						ProcessSTT.SendData(
							$"{ev.Info.Attacker} killed {ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) with {ev.Info.Tool}.",
							HandleQueue.GameLogChannelId);
					}
				}
				catch (Exception e)
				{
					Plugin.Error($"Player Hurt error: {e}");
				}
			}
		}

		public void OnGrenadeThrown(ref GrenadeThrownEvent ev)
		{
			if (plugin.GrenadeThrown)
			{
				if (ev.Player == null)
					return;
				ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) threw a grenade.", HandleQueue.GameLogChannelId);
			}
		}

		public void OnMedicalItem(MedicalItemEvent ev)
		{
			if (plugin.MedicalItem)
			{
				if (ev.Player == null)
					return;
				ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) user a {ev.Item}", HandleQueue.GameLogChannelId);
			}
		}

		public void OnSetClass(SetClassEvent ev)
		{
			if (plugin.SetClass)
			{
				if (ev.Player == null)
					return;
				ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} has been changed to a {ev.Role}.", HandleQueue.GameLogChannelId);
			}
		}

		public void OnRespawn(ref TeamRespawnEvent ev)
		{
			if (plugin.Respawn)
			{
				string msg;
				if (ev.IsChaos)
					msg = "Chaos Insurgency";
				else
					msg = "Nine-Tailed Fox";
				ProcessSTT.SendData($"{msg} has spawned with {ev.ToRespawn.Count} players.", HandleQueue.GameLogChannelId);
			}
		}

		public void OnPlayerJoin(PlayerJoinEvent ev)
		{
			if (plugin.PlayerJoin)
				if (ev.Player.nicknameSync.MyNick != "Dedicated Server")
					ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} has joined the game.", HandleQueue.GameLogChannelId);
		}
	}
}