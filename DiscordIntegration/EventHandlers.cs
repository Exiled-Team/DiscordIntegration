using System;
using System.Collections.Generic;
using EXILED;
using EXILED.Extensions;
using Grenades;
using MEC;
using Scp914;
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
				ProcessSTT.SendData($"{ev.Sender.Nickname} {Plugin.translation.usedCommand}: {ev.Command}", HandleQueue.CommandLogChannelId);
			if (ev.Command.ToLower() == "list")
			{
				ev.Allow = false;
				string message = "";
				foreach (ReferenceHub hub in Player.GetHubs())
					message +=
						$"{hub.nicknameSync.MyNick} - ({hub.characterClassManager.UserId})\n";
				if (string.IsNullOrEmpty(message))
					message = $"{Plugin.translation.noPlayersOnline}";
				ev.Sender.RAMessage(message);
			}
			else if (ev.Command.ToLower() == "stafflist")
			{
				ev.Allow = false;
				Log.Info("Staff listen");
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

				Log.Info($"Bool: {isStaff} Names: {names}");
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
					if (ev.Attacker != null && ev.Attacker.characterClassManager != null && Player.GetTeam(ev.Player.characterClassManager.CurClass) == Player.GetTeam(ev.Attacker.characterClassManager.CurClass) && ev.Player != ev.Attacker)
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
					Log.Error($"Player Hurt error: {e}");
				}
			}
		}
		
		public void OnPlayerDeath(ref PlayerDeathEvent ev)
		{
			if (plugin.PlayerDeath)
			{
				try
				{
					if (ev.Killer != null && ev.Killer.characterClassManager != null && Player.GetTeam(ev.Player.characterClassManager.CurClass) == Player.GetTeam(ev.Killer.characterClassManager.CurClass))
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
					Log.Error($"Player Hurt error: {e}");
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
			if (plugin.RoleSync)
				Methods.CheckForSyncRole(ev.Player);
			if (plugin.PlayerJoin)
				if (ev.Player.nicknameSync.MyNick != "Dedicated Server")
					ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} {Plugin.translation.hasJoinedTheGame}.", HandleQueue.GameLogChannelId);
		}

		public void OnPlayerLeave(PlayerLeaveEvent ev)
		{
			if (plugin.PlayerLeave)
				if (ev.Player.nicknameSync.MyNick != "Dedicated Server")
					ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} {Plugin.translation.hasLeftTheGame}.", HandleQueue.GameLogChannelId);
		}

		public void OnPlayerFreed(ref HandcuffEvent ev)
		{
			if (plugin.Freed)
				ProcessSTT.SendData(
					$"{ev.Target.nicknameSync.MyNick} - {ev.Target.characterClassManager.UserId} ({ev.Target.characterClassManager.CurClass}) {Plugin.translation.hasBeenFreedBy} {ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass})",
						HandleQueue.GameLogChannelId);
		}

		public void OnPlayerHandcuffed(ref HandcuffEvent ev)
		{
			if (plugin.Cuffed)
				ProcessSTT.SendData(
					$"{ev.Target.nicknameSync.MyNick} - {ev.Target.characterClassManager.UserId} ({ev.Target.characterClassManager.CurClass}) {Plugin.translation.hasBeenHandcuffedBy} {ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass})",
						HandleQueue.GameLogChannelId);
		}

		public void OnPlayerBanned(PlayerBannedEvent ev)
		{
			if (plugin.Banned)
				ProcessSTT.SendData($"{ev.Details.OriginalName} - {ev.Details.Id} {Plugin.translation.wasBannedBy} {ev.Details.Issuer} {Plugin.translation._for} {ev.Details.Reason}. {DateTime.Now.AddTicks(ev.Details.Expires)}", HandleQueue.CommandLogChannelId);
		}

		public void OnIntercomSpeak(ref IntercomSpeakEvent ev)
		{
			if (plugin.Intercom)
				ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.hasStartedUsingTheIntercom}.", HandleQueue.GameLogChannelId);
		}

		public void OnPickupItem(ref PickupItemEvent ev)
		{
			if (plugin.PickupItem)
				ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.hasPickedUp} {ev.Item.ItemId}.", HandleQueue.GameLogChannelId);
		}

		public void OnDropItem(ref DropItemEvent ev)
		{
			if (plugin.DropItem)
				ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.hasDropped} {ev.Item.id}.", HandleQueue.GameLogChannelId);
		}

		public void OnDecon(ref DecontaminationEvent ev)
		{
			if (plugin.Decon)
				ProcessSTT.SendData($"{Plugin.translation.hasDropped}.", HandleQueue.CommandLogChannelId);
		}

		public void OnConsoleCommand(ConsoleCommandEvent ev)
		{
			if (plugin.ConsoleCommand)
				ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.CurClass} ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.hasRunClientConsoleCommand}: {ev.Command}", HandleQueue.CommandLogChannelId);
		}

		public void OnPocketEnter(PocketDimEnterEvent ev)
		{
			if (plugin.PocketEnter)
				ProcessSTT.SendData(
					$"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.CurClass} ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.hasEnteredPocketDimension}.",
					HandleQueue.GameLogChannelId);
		}

		public void OnPocketEscape(PocketDimEscapedEvent ev)
		{
			if (plugin.PocketEscape)
				ProcessSTT.SendData(
					$"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.CurClass} ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.hasEscapedPocketDimension}.",
					HandleQueue.GameLogChannelId);		}

		public void On106Teleport(Scp106TeleportEvent ev)
		{
			if (plugin.Scp106Tele)
				ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} {Plugin.translation.hasEscapedPocketDimension}.", HandleQueue.GameLogChannelId);
		}

		public void On079Tesla(ref Scp079TriggerTeslaEvent ev)
		{
			if (plugin.Scp079Tesla)
				ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.hasTriggeredATeslaGate}.", HandleQueue.GameLogChannelId);
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
				
				ProcessSTT.SendData($"{Plugin.translation.scp914HasProcessedTheFollowingPlayers}: {players} {Plugin.translation.andItems}: {items}.", HandleQueue.GameLogChannelId);
			}
		}

		public void OnDoorInteract(ref DoorInteractionEvent ev)
		{
			if (plugin.DoorInteract)
				ProcessSTT.SendData(ev.Door.NetworkisOpen
						? $"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.hasClosedADoor}: {ev.Door.DoorName}."
						: $"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.hasOpenedADoor}: {ev.Door.DoorName}.",
					HandleQueue.GameLogChannelId);
		}

		public void On914Activation(ref Scp914ActivationEvent ev)
		{
			if (plugin.Scp914Activation)
				ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.scp914HasBeenActivated} {Scp914Machine.singleton.knobState}.", HandleQueue.GameLogChannelId);
		}

		public void On914KnobChange(ref Scp914KnobChangeEvent ev)
		{
			if (plugin.Scp914KnobChange)
				ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.scp914knobchange} {ev.KnobSetting}.", HandleQueue.GameLogChannelId);
		}

		public void OnPlayerEnterFemur (FemurEnterEvent ev)
		{
			if (plugin.EnterFemur)
				ProcessSTT.SendData($"{ev.Player.nicknameSync.MyNick} - {ev.Player.characterClassManager.UserId} ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.hasEnteredFemur}.", HandleQueue.GameLogChannelId);
		}
	}
}