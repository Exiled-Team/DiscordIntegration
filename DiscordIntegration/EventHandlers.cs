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
				ProcessSTT.SendData($"{ev.Sender.Nickname} {Plugin.translation.usedCommand}: {ev.Command}", HandleQueue.CommandLogChannelId);
			if (ev.Command.ToLower() == "list")
			{
				ev.Allow = false;
				string message = "";
				foreach (ReferenceHub hub in Plugin.GetHubs())
					message +=
						$"{hub.nicknameSync.MyNick} - ({hub.characterClassManager.UserId})\n";
				if (string.IsNullOrEmpty(message))
					message = $"{Plugin.translation.noPlayersOnline}";
				ev.Sender.RAMessage(message);
			}
			else if (ev.Command.ToLower() == "stafflist")
			{
				ev.Allow = false;
				Plugin.Info("Staff listen");
				bool isStaff = false;
				string names = "";
				foreach (GameObject o in PlayerManager.players)
				{
					ReferenceHub rh = o.GetComponent<ReferenceHub>();
					
					if (rh.serverRoles.RemoteAdmin)
					{
						isStaff = true;
						names += $"{rh.nicknameSync.MyNick} ";
					}
				}

				Plugin.Info($"Bool: {isStaff} Names: {names}");
				string response = isStaff ? names : $"{Plugin.translation.noStaffOnline}";
				ev.Sender.RAMessage($"{PlayerManager.players.Count}/25 {response}");
			}
		}

		public void OnWaitingForPlayers()
		{
			if (plugin.WaitingForPlayers)
				ProcessSTT.SendData($"{Plugin.translation.waitingForPlayers}", HandleQueue.GameLogChannelId);
		}

		public void OnRoundStart()
		{
			if (plugin.RoundStart)
				ProcessSTT.SendData($"{Plugin.translation.roundStarting}: {Plugin.GetHubs().Count} {Plugin.translation.playersInRound}.", HandleQueue.GameLogChannelId);
		}

		public void OnRoundEnd()
		{
			if (plugin.RoundEnd)
				ProcessSTT.SendData($"{Plugin.translation.roundEnded}: {Plugin.GetHubs().Count} {Plugin.translation.playersOnline}.", HandleQueue.GameLogChannelId);
		}

		public void OnCheaterReport(ref CheaterReportEvent ev)
		{
			if (plugin.CheaterReport)
				ProcessSTT.SendData($"**{Plugin.translation.cheaterReportFiled}: {ev.ReporterId} {Plugin.translation.reported} {ev.ReportedId} {Plugin.translation._for} {ev.Report}.**", HandleQueue.GameLogChannelId);
		}

		public void OnPlayerHurt(ref PlayerHurtEvent ev)
		{
			if (plugin.PlayerHurt)
			{
				try
				{
					if (ev.Attacker != null && ev.Attacker.characterClassManager != null && Plugin.GetTeam(ev.Player.characterClassManager.CurClass) == Plugin.GetTeam(ev.Attacker.characterClassManager.CurClass))
						ProcessSTT.SendData(
							$"**{ev.Attacker.nicknameSync.MyNick} - {ev.Attacker.characterClassManager.UserId} ({ev.Attacker.characterClassManager.CurClass}) {Plugin.translation.damaged} {ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) {Plugin.translation._for} {ev.Info.Amount} {Plugin.translation.with} {ev.Info.Tool}.**",
							HandleQueue.GameLogChannelId);
					else if (!plugin.OnlyFriendlyFire)
					{
						ProcessSTT.SendData(
							$"{ev.Info.Attacker}  {Plugin.translation.damaged} {ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) {Plugin.translation._for} {ev.Info.Amount} {Plugin.translation.with} {ev.Info.Tool}.",
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
							$"**{ev.Killer.nicknameSync.MyNick} - {ev.Killer.characterClassManager.UserId} ({ev.Killer.characterClassManager.CurClass}) {Plugin.translation.killed} {ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.with} {ev.Info.Tool}.**",
							HandleQueue.GameLogChannelId);
					else if (!plugin.OnlyFriendlyFire)
					{
						ProcessSTT.SendData(
							$"{ev.Info.Attacker} {Plugin.translation.killed} {ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.with} {ev.Info.Tool}.",
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
				ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.threwAGrenade}.", HandleQueue.GameLogChannelId);
			}
		}

		public void OnMedicalItem(MedicalItemEvent ev)
		{
			if (plugin.MedicalItem)
			{
				if (ev.Player == null)
					return;
				ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.userA} {ev.Item}", HandleQueue.GameLogChannelId);
			}
		}

		public void OnSetClass(SetClassEvent ev)
		{
			if (plugin.SetClass)
			{
				if (ev.Player == null)
					return;
				ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} {Plugin.translation.hasBenChangedToA} {ev.Role}.", HandleQueue.GameLogChannelId);
			}
		}

		public void OnRespawn(ref TeamRespawnEvent ev)
		{
			if (plugin.Respawn)
			{
				string msg;
				if (ev.IsChaos)
					msg = $"{Plugin.translation.chaosInsurgency}";
				else
					msg = $"{Plugin.translation.nineTailedFox}";
				ProcessSTT.SendData($"{msg} {Plugin.translation.hasSpawnedWith} {ev.ToRespawn.Count} {Plugin.translation.players}.", HandleQueue.GameLogChannelId);
			}
		}

		public void OnPlayerJoin(PlayerJoinEvent ev)
		{
			if (plugin.PlayerJoin)
				if (ev.Player.nicknameSync.MyNick != "Dedicated Server")
					ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} {Plugin.translation.hasJoinedTheGame}.", HandleQueue.GameLogChannelId);
		}
	}
}