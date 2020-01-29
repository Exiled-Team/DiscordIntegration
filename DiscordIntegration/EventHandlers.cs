using System;
using System.Collections.Generic;
using EXILED;
using GameCore;
using Grenades;
using MEC;
using UnityEngine;
using UnityEngine.PostProcessing;

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
					message +=
						$"{hub.nicknameSync.MyNick} - ({hub.characterClassManager.UserId})\n";
				if (string.IsNullOrEmpty(message))
					message = "No players online.";
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
				string response = isStaff ? names : "No staff online.";
				ev.Sender.RAMessage($"{PlayerManager.players.Count}/25 {response}");
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
					if (ev.Attacker != null && ev.Attacker.characterClassManager != null && Plugin.GetTeam(ev.Player.characterClassManager.CurClass) == Plugin.GetTeam(ev.Attacker.characterClassManager.CurClass) && ev.Player != ev.Attacker)
						ProcessSTT.SendData(
							$"**{ev.Attacker.nicknameSync.MyNick} - {ev.Attacker.characterClassManager.UserId} ({ev.Attacker.characterClassManager.CurClass}) damaged {ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) for {ev.Info.Amount} with {ev.Info.Tool}.**",
							HandleQueue.GameLogChannelId);
					else if (!plugin.OnlyFriendlyFire)
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
					else if (!plugin.OnlyFriendlyFire)
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
			if (plugin.RoleSync)
				Methods.CheckForSyncRole(ev.Player);
			if (plugin.PlayerJoin)
				if (ev.Player.nicknameSync.MyNick != "Dedicated Server")
					ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} has joined the game.", HandleQueue.GameLogChannelId);
		}

		public void OnPlayerFreed(ref HandcuffEvent ev)
		{
			if (plugin.Freed)
				ProcessSTT.SendData(
					$"{ev.Target.nicknameSync.MyNick} - {ev.Target.characterClassManager.UserId} ({ev.Target.characterClassManager.CurClass}) has been freed by {ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass})",
						HandleQueue.GameLogChannelId);
		}

		public void OnPlayerHandcuffed(ref HandcuffEvent ev)
		{
			if (plugin.Cuffed)
				ProcessSTT.SendData(
					$"{ev.Target.nicknameSync.MyNick} - {ev.Target.characterClassManager.UserId} ({ev.Target.characterClassManager.CurClass}) has been handcuffed by {ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass})",
						HandleQueue.GameLogChannelId);
		}

		public void OnPlayerBanned(PlayerBannedEvent ev)
		{
			if (plugin.Banned)
				ProcessSTT.SendData($"{ev.Details.OriginalName} - {ev.Details.Id} was banned by {ev.Details.Issuer} for {ev.Details.Reason}. {DateTime.Now.AddTicks(ev.Details.Expires)}", HandleQueue.CommandLogChannelId);
		}

		public void OnIntercomSpeak(ref IntercomSpeakEvent ev)
		{
			if (plugin.Intercom)
				ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) has started using the intercom.", HandleQueue.GameLogChannelId);
		}

		public void OnPickupItem(ref PickupItemEvent ev)
		{
			if (plugin.PickupItem)
				ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) has picked up {ev.Item.ItemId}.", HandleQueue.GameLogChannelId);
		}

		public void OnDropItem(ref DropItemEvent ev)
		{
			if (plugin.DropItem)
				ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) has dropped up {ev.Item.id}.", HandleQueue.GameLogChannelId);
		}

		public void OnDecon(ref DecontaminationEvent ev)
		{
			if (plugin.Decon)
				ProcessSTT.SendData($"Deconamination has begun.", HandleQueue.CommandLogChannelId);
		}

		public void OnConsoleCommand(ConsoleCommandEvent ev)
		{
			if (plugin.ConsoleCommand)
				ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.CurClass} ({ev.Player.characterClassManager.CurClass}) has run a client-console command: {ev.Command}", HandleQueue.CommandLogChannelId);
		}

		public void OnPocketEnter(PocketDimEnterEvent ev)
		{
			if (plugin.PocketEnter)
				ProcessSTT.SendData(
					$"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.CurClass} ({ev.Player.characterClassManager.CurClass}) has entered the pocket dimension.",
					HandleQueue.GameLogChannelId);
		}

		public void OnPocketEscape(PocketDimEscapedEvent ev)
		{
			if (plugin.PocketEscape)
				ProcessSTT.SendData(
					$"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.CurClass} ({ev.Player.characterClassManager.CurClass}) has escaped the pocket dimension.",
					HandleQueue.GameLogChannelId);		}

		public void On106Teleport(Scp106TeleportEvent ev)
		{
			if (plugin.Scp106Tele)
				ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} has traveled through their portal.", HandleQueue.GameLogChannelId);
		}

		public void On079Tesla(ref Scp079TriggerTeslaEvent ev)
		{
			if (plugin.Scp079Tesla)
				ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) has triggered a tesla gate.", HandleQueue.GameLogChannelId);
		}

		public void OnScp194Upgrade(ref SCP914UpgradeEvent ev)
		{
			if (plugin.Scp914Upgrade)
			{
				string players = "";
				foreach (ReferenceHub hub in ev.Players) 
					players += $"{hub.nicknameSync.MyNick} - {hub.characterClassManager.UserId} ({hub.characterClassManager.CurClass})\n";
				string items = "";
				foreach (Pickup item in ev.Items)
					items += $"{item.ItemId}\n";
				
				ProcessSTT.SendData($"SCP-914 has processed the following players: {players} and items: {items}.", HandleQueue.GameLogChannelId);
			}
		}

		public void OnDoorInteract(ref DoorInteractionEvent ev)
		{
			if (plugin.DoorInteract)
				ProcessSTT.SendData(ev.Door.NetworkisOpen
						? $"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) has closed a door: {ev.Door.DoorName}."
						: $"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) has opened a door: {ev.Door.DoorName}.",
					HandleQueue.GameLogChannelId);
		}
	}
}