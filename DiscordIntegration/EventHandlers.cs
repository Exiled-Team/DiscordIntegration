using System;
using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using GameCore;
using Grenades;
using JetBrains.Annotations;
using MEC;
using Scp914;
using UnityEngine;
using UnityEngine.PostProcessing;
using Log = EXILED.Log;

namespace DiscordIntegration_Plugin {
	public class EventHandlers {
		private readonly Plugin plugin;
		public EventHandlers( Plugin plugin ) => this.plugin = plugin;
		private int maxPlayers = ConfigFile.ServerConfig.GetInt("max_players", 20);

		public void OnCommand( ref RACommandEvent ev ) {

			if( plugin.RaCommands )
				ProcessSTT.SendData($"{ev.Sender.Nickname} {Plugin.translation.UsedCommand}: {ev.Command}", HandleQueue.CommandLogChannelId);
			if( ev.Command.ToLower() == "list" ) {
				ev.Allow = false;
				string message = "";
				foreach( ReferenceHub hub in Player.GetHubs() )
					message +=
						$"`{hub.nicknameSync.MyNick}` - ({hub.characterClassManager.UserId})\n";
				if( string.IsNullOrEmpty(message) )
					message = $"{Plugin.translation.NoPlayersOnline}";
				ev.Sender.RAMessage(message);
			} else if( ev.Command.ToLower() == "stafflist" ) {
				ev.Allow = false;
				Log.Info("Staff list");
				bool isStaff = false;
				string names = "";
				foreach( GameObject o in PlayerManager.players ) {
					ReferenceHub rh = o.GetComponent<ReferenceHub>();

					if( rh.serverRoles.RemoteAdmin ) {
						isStaff = true;
						names += $"`{rh.nicknameSync.MyNick}` ";
					}
				}

				Log.Info($"Bool: {isStaff} Names: {names}");
				string response = isStaff ? names : $"{Plugin.translation.NoStaffOnline}";
				ev.Sender.RAMessage($"{PlayerManager.players.Count}/{maxPlayers} {response}");
			}
		}

		public void OnWaitingForPlayers() {
			if( plugin.WaitingForPlayers )
				ProcessSTT.SendData($"{Plugin.translation.WaitingForPlayers}", HandleQueue.GameLogChannelId);
		}

		public void OnRoundStart() {
			if( plugin.RoundStart )
				ProcessSTT.SendData($"{Plugin.translation.RoundStarting}: {Player.GetHubs().ToList().Count} {Plugin.translation.PlayersInRound}.", HandleQueue.GameLogChannelId);
		}

		public void OnRoundEnd() {
			if( plugin.RoundEnd )
				ProcessSTT.SendData($"{Plugin.translation.RoundEnded}: {Player.GetHubs().ToList().Count} {Plugin.translation.PlayersOnline}.", HandleQueue.GameLogChannelId);
		}

		public void OnCheaterReport( ref CheaterReportEvent ev ) {
			if( plugin.CheaterReport )
				ProcessSTT.SendData($"**{Plugin.translation.CheaterReportFiled}: {Plugin.translation.Reported} {Plugin.translation._For} {ev.Report}.**", HandleQueue.GameLogChannelId);
		}

		public void OnPlayerHurt( ref PlayerHurtEvent ev ) {
			if( plugin.PlayerHurt ) {
				try {
					if( ev.Attacker != null && ev.Attacker.characterClassManager != null && ev.Player.characterClassManager.CurClass.GetSide() == ev.Attacker.characterClassManager.CurClass.GetSide() && ev.Player != ev.Attacker )
						ProcessSTT.SendData(
							$"**`{ev.Attacker.nicknameSync.MyNick}` ({ev.Attacker.characterClassManager.CurClass}) {Plugin.translation.Damaged} `{ev.Player.nicknameSync.MyNick}` - ({ev.Player.characterClassManager.CurClass}) {Plugin.translation._For} {(int) ev.Info.Amount}hp {Plugin.translation.With} {DamageTypes.FromIndex(ev.Info.Tool).name}.**",
							HandleQueue.GameLogChannelId);
					else if( !plugin.OnlyFriendlyFire ) {
						ProcessSTT.SendData(
							$"`{ev.Info.Attacker}`  {Plugin.translation.Damaged} `{ev.Player.nicknameSync.MyNick}` ({ev.Player.characterClassManager.CurClass}) {Plugin.translation._For} {(int) ev.Info.Amount}hp {Plugin.translation.With} {DamageTypes.FromIndex(ev.Info.Tool).name}.",
							HandleQueue.GameLogChannelId);
					}
				} catch( Exception e ) {
					Log.Error($"Player Hurt error: {e}");
				}
			}
		}

		public void OnPlayerDeath( ref PlayerDeathEvent ev ) {
			if( plugin.PlayerDeath ) {
				try {
					if( ev.Killer != null && ev.Killer.characterClassManager != null && ev.Player.characterClassManager.CurClass.GetSide() == ev.Killer.characterClassManager.CurClass.GetSide() )
						ProcessSTT.SendData(
							$"**`{ev.Killer.nicknameSync.MyNick}` ({ev.Killer.characterClassManager.CurClass}) {Plugin.translation.Killed} `{ev.Player.nicknameSync.MyNick}` - ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.With} {DamageTypes.FromIndex(ev.Info.Tool).name}.**",
							HandleQueue.GameLogChannelId);
					else if( !plugin.OnlyFriendlyFire ) {
						ProcessSTT.SendData(
							$"`{ev.Info.Attacker}` {Plugin.translation.Killed} `{ev.Player.nicknameSync.MyNick}` ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.With} {DamageTypes.FromIndex(ev.Info.Tool).name}.",
							HandleQueue.GameLogChannelId);
					}
				} catch( Exception e ) {
					Log.Error($"Player Hurt error: {e}");
				}
			}
		}

		public void OnGrenadeThrown( ref GrenadeThrownEvent ev ) {
			if( plugin.GrenadeThrown ) {
				if( ev.Player == null )
					return;
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.ThrewAGrenade}.", HandleQueue.GameLogChannelId);
			}
		}

		public void OnMedicalItem( MedicalItemEvent ev ) {
			if( plugin.MedicalItem ) {
				if( ev.Player == null )
					return;
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.UsedA} {ev.Item}", HandleQueue.GameLogChannelId);
			}
		}

		public void OnSetClass( SetClassEvent ev ) {
			if( plugin.SetClass ) {
				if( ev.Player == null )
					return;
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` {Plugin.translation.HasBenChangedToA} {ev.Role}.", HandleQueue.GameLogChannelId);
			}
		}

		public void OnRespawn( ref TeamRespawnEvent ev ) {
			if( plugin.Respawn ) {
				string msg;
				if( ev.IsChaos )
					msg = $"{Plugin.translation.ChaosInsurgency}";
				else
					msg = $"{Plugin.translation.NineTailedFox}";
				ProcessSTT.SendData($"{msg} {Plugin.translation.HasSpawnedWith} {Plugin.translation.Players} ({ev.ToRespawn.Count}).", HandleQueue.GameLogChannelId);
			}
		}

		public void OnPlayerJoin( PlayerJoinEvent ev ) {
			if( plugin.RoleSync )
				Methods.CheckForSyncRole(ev.Player);
			if( plugin.PlayerJoin )
				if( ev.Player.nicknameSync.MyNick != "Dedicated Server" )
					ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` {Plugin.translation.HasJoinedTheGame}.", HandleQueue.GameLogChannelId);
		}

		public void OnPlayerFreed( ref HandcuffEvent ev ) {
			if( plugin.Freed )
				ProcessSTT.SendData(
					$"`{ev.Target.nicknameSync.MyNick}` ({ev.Target.characterClassManager.CurClass}) {Plugin.translation.HasBeenFreedBy} `{ev.Player.nicknameSync.MyNick}` - ({ev.Player.characterClassManager.CurClass})",
						HandleQueue.GameLogChannelId);
		}

		public void OnPlayerHandcuffed( ref HandcuffEvent ev ) {
			if( plugin.Cuffed )
				ProcessSTT.SendData(
					$"`{ev.Target.nicknameSync.MyNick}` ({ev.Target.characterClassManager.CurClass}) {Plugin.translation.HasBeenHandcuffedBy} `{ev.Player.nicknameSync.MyNick}` - ({ev.Player.characterClassManager.CurClass})",
						HandleQueue.GameLogChannelId);
		}

		public void OnPlayerBanned( PlayerBannedEvent ev ) {
			if( plugin.Banned )
				ProcessSTT.SendData($"`{ev.Details.OriginalName}` {Plugin.translation.WasBannedBy} `{ev.Details.Issuer}` {Plugin.translation._For} {ev.Details.Reason}. {new DateTime(ev.Details.Expires)}", HandleQueue.CommandLogChannelId);
		}

		public void OnIntercomSpeak( ref IntercomSpeakEvent ev ) {
			if( plugin.Intercom )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.HasStartedUsingTheIntercom}.", HandleQueue.GameLogChannelId);
		}

		public void OnPickupItem( ref PickupItemEvent ev ) {
			if( plugin.PickupItem )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.HasPickedUp} {ev.Item.ItemId}.", HandleQueue.GameLogChannelId);
		}

		public void OnDropItem( ref DropItemEvent ev ) {
			if( plugin.DropItem )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.HasDropped} {ev.Item.id}.", HandleQueue.GameLogChannelId);
		}

		public void OnDecon( ref DecontaminationEvent ev ) {
			if( plugin.Decon )
				ProcessSTT.SendData($"{Plugin.translation.HasDropped}.", HandleQueue.CommandLogChannelId);
		}

		public void OnConsoleCommand( ConsoleCommandEvent ev ) {
			if( plugin.ConsoleCommand )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` {ev.Player.characterClassManager.CurClass} ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.HasRunClientConsoleCommand}: {ev.Command}", HandleQueue.CommandLogChannelId);
		}

		public void OnPocketEnter( PocketDimEnterEvent ev ) {
			if( plugin.PocketEnter )
				ProcessSTT.SendData(
					$"`{ev.Player.nicknameSync.MyNick}` {ev.Player.characterClassManager.CurClass} ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.HasEnteredPocketDimension}.",
					HandleQueue.GameLogChannelId);
		}

		public void OnPocketEscape( PocketDimEscapedEvent ev ) {
			if( plugin.PocketEscape )
				ProcessSTT.SendData(
					$"`{ev.Player.nicknameSync.MyNick}` {ev.Player.characterClassManager.CurClass} ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.HasEscapedPocketDimension}.",
					HandleQueue.GameLogChannelId);
		}

		public void On106Teleport( Scp106TeleportEvent ev ) {
			if( plugin.Scp106Tele )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` {Plugin.translation.HasEscapedPocketDimension}.", HandleQueue.GameLogChannelId);
		}

		public void On079Tesla( ref Scp079TriggerTeslaEvent ev ) {
			if( plugin.Scp079Tesla )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.HasTriggeredATeslaGate}.", HandleQueue.GameLogChannelId);
		}

		public void OnScp194Upgrade( ref SCP914UpgradeEvent ev ) {
			if( plugin.Scp914Upgrade ) {
				string players = "";
				int i = 1;
				foreach( ReferenceHub hub in ev.Players ) {
					players += $"\n{i}. {hub.nicknameSync.MyNick} ({hub.characterClassManager.CurClass})";
					i++;
				}
				string items = "";
				int it = 1;
				foreach( Pickup item in ev.Items ) {
					items += $"\n{it}. {item.ItemId}";
					it++;
				}
				if( players == "" )
					players = "Ninguno.";
				if( items == "" )
					items = "Ninguno.";
				ProcessSTT.SendData($"{Plugin.translation.Scp914HasProcessedTheFollowingPlayers} \nJugadores procesados: \n```md \n{players}\n``` {Plugin.translation.AndItems}: \n```md \n{items}\n```", HandleQueue.GameLogChannelId);
			}
		}

		public void OnDoorInteract( ref DoorInteractionEvent ev ) {
			if( plugin.DoorInteract )
				ProcessSTT.SendData(ev.Door.NetworkisOpen
						? $"`{ev.Player.nicknameSync.MyNick}` ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.HasClosedADoor}: {ev.Door.DoorName}."
						: $"`{ev.Player.nicknameSync.MyNick}` ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.HasOpenedADoor}: {ev.Door.DoorName}.",
					HandleQueue.GameLogChannelId);
		}

		public void On914Activation( ref Scp914ActivationEvent ev ) {
			if( plugin.Scp914Activation )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.Scp914HasBeenActivated} {Scp914Machine.singleton.knobState}.", HandleQueue.GameLogChannelId);
		}

		public void On914KnobChange( ref Scp914KnobChangeEvent ev ) {
			if( plugin.Scp914KnobChange )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` ({ev.Player.characterClassManager.CurClass}) {Plugin.translation.Scp914Knobchange} {ev.KnobSetting}.", HandleQueue.GameLogChannelId);
		}

		public void OnWarheadCancelled( WarheadCancelEvent ev ) {
			if( plugin.WarheadCancel )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` {Plugin.translation.CancelledWarhead}.", HandleQueue.GameLogChannelId);
		}

		public void OnWarheadDetonation() {
			if( plugin.WarheadDetonate )
				ProcessSTT.SendData($"{Plugin.translation.WarheadDetonated}.", HandleQueue.GameLogChannelId);
		}

		public void OnWarheadStart( WarheadStartEvent ev ) {
			if( plugin.WarheadStart )
				ProcessSTT.SendData($"{Plugin.translation.WarheadStarted} {Map.AlphaWarheadController.NetworktimeToDetonation}.", HandleQueue.GameLogChannelId);
		}

		public void OnWarheadAccess( WarheadKeycardAccessEvent ev ) {
			if( plugin.WarheadAccess )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` {Plugin.translation.AccessedWarhead}.", HandleQueue.GameLogChannelId);
		}

		public void OnElevatorInteraction( ref ElevatorInteractionEvent ev ) {
			if( plugin.Elevator )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` {Plugin.translation.CalledElevator}.", HandleQueue.GameLogChannelId);
		}

		public void OnLockerInteraction( LockerInteractionEvent ev ) {
			if( plugin.Locker )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` {Plugin.translation.UsedLocker}.", HandleQueue.GameLogChannelId);
		}

		public void OnTriggerTesla( ref TriggerTeslaEvent ev ) {
			if( plugin.TriggerTesla )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` {Plugin.translation.TriggeredTesla}.", HandleQueue.GameLogChannelId);
		}

		public void OnGenClosed( ref GeneratorCloseEvent ev ) {
			if( plugin.GenClose )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` {Plugin.translation.GenClosed}.", HandleQueue.GameLogChannelId);
		}

		public void OnGenEject( ref GeneratorEjectTabletEvent ev ) {
			if( plugin.GenEject )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` {Plugin.translation.GenEjected}.", HandleQueue.GameLogChannelId);
		}

		public void OnGenFinish( ref GeneratorFinishEvent ev ) {
			if( plugin.GenFinish )
				ProcessSTT.SendData($"`{Plugin.translation.GenFinished}`", HandleQueue.GameLogChannelId);
		}

		public void OnGenInsert( ref GeneratorInsertTabletEvent ev ) {
			if( plugin.GenInsert )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` {Plugin.translation.GenInserted}.", HandleQueue.GameLogChannelId);
		}

		public void OnGenOpen( ref GeneratorOpenEvent ev ) {
			if( plugin.GenOpen )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` {Plugin.translation.GenOpened}.", HandleQueue.GameLogChannelId);
		}

		public void OnGenUnlock( ref GeneratorUnlockEvent ev ) {
			if( plugin.GenUnlock )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` {Plugin.translation.GenUnlocked}.", HandleQueue.GameLogChannelId);
		}

		public void On106Contain( Scp106ContainEvent ev ) {
			if( plugin.Scp106Contain )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` {Plugin.translation.WasContained}.", HandleQueue.GameLogChannelId);
		}

		public void On106CreatePortal( Scp106CreatedPortalEvent ev ) {
			if( plugin.Scp106Portal )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` {Plugin.translation.CreatedPortal}.", HandleQueue.GameLogChannelId);
		}

		public void OnItemChanged( ItemChangedEvent ev ) {
			if( plugin.ItemChanged )
				if( plugin.Scp106Portal )
					ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` {Plugin.translation.ItemChanged}: {ev.OldItem.id} -> {ev.NewItem.id}.", HandleQueue.GameLogChannelId);
		}

		public void On079GainExp( Scp079ExpGainEvent ev ) {
			if( plugin.Scp079Exp )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` {Plugin.translation.GainedExp}: {(int) ev.Amount}, {ev.GainType}.", HandleQueue.GameLogChannelId);
		}

		public void On079GainLvl( Scp079LvlGainEvent ev ) {
			if( plugin.Scp079Lvl )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` {Plugin.translation.GainedLevel} {ev.OldLvl} -> {ev.NewLvl}.", HandleQueue.GameLogChannelId);
		}

		public void OnPlayerLeave( PlayerLeaveEvent ev ) {
			if( plugin.PlayerLeave )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` {Plugin.translation.LeftServer}", HandleQueue.GameLogChannelId);
		}

		public void OnPlayerReload( ref PlayerReloadEvent ev ) {
			if( plugin.PlayerReload )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` {Plugin.translation.Reloaded}: {ev.Player.GetCurrentItem().id}.", HandleQueue.GameLogChannelId);
		}

		public void OnSetGroup( SetGroupEvent ev ) {
			if( plugin.SetGroup )
				ProcessSTT.SendData($"`{ev.Player.nicknameSync.MyNick}` {Plugin.translation.GroupSet}: {ev.Group.BadgeText} ({ev.Group.BadgeColor}).", HandleQueue.GameLogChannelId);
		}
	}
}