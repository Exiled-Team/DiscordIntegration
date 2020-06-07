using System;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Scp914;

namespace DiscordIntegration_Plugin
{
    public class PlayerEvents
    {
        public Plugin plugin;
        public PlayerEvents(Plugin plugin) => this.plugin = plugin;
        
	    public void OnGenInsert(InsertingGeneratorTabletEventArgs ev)
		{
			if (Plugin.Cfg.GenInsert)
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} {Plugin.translation.GenInserted}.", HandleQueue.GameLogChannelId);
		}

		public void OnGenOpen(OpeningGeneratorEventArgs ev)
		{
			if (Plugin.Cfg.GenOpen)
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} {Plugin.translation.GenOpened}.", HandleQueue.GameLogChannelId);
		}

		public void OnGenUnlock(UnlockingGeneratorEventArgs ev)
		{
			if (Plugin.Cfg.GenUnlock)
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} {Plugin.translation.GenUnlocked}.", HandleQueue.GameLogChannelId);
		}

		public void On106Contain(ContainingEventArgs ev)
		{
			if (Plugin.Cfg.Scp106Contain)
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} {Plugin.translation.WasContained}.", HandleQueue.GameLogChannelId);
		}

		public void On106CreatePortal(CreatingPortalEventArgs ev)
		{
			if (Plugin.Cfg.Scp106Portal)
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} {Plugin.translation.CreatedPortal}.", HandleQueue.GameLogChannelId);
		}

		public void OnItemChanged(ChangingItemEventArgs ev)
		{
			if (Plugin.Cfg.ItemChanged)
				if (Plugin.Cfg.Scp106Portal)
					ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} {Plugin.translation.ItemChanged}: {ev.OldItem.id} -> {ev.NewItem.id}.", HandleQueue.GameLogChannelId);
		}

		public void On079GainExp(GainingExperienceEventArgs ev)
		{
			if (Plugin.Cfg.Scp079Exp)
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} {Plugin.translation.GainedExp}: {ev.Amount}, {ev.GainType}.", HandleQueue.GameLogChannelId);
		}

		public void On079GainLvl(GainingLevelEventArgs ev)
		{
			if (Plugin.Cfg.Scp079Lvl)
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} {Plugin.translation.GainedLevel} {ev.OldLevel} -> {ev.NewLevel}.", HandleQueue.GameLogChannelId);
		}

		public void OnPlayerLeave(LeftEventArgs ev)
		{
			if (Plugin.Cfg.PlayerLeave)
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} {Plugin.translation.LeftServer}", HandleQueue.GameLogChannelId);
		}

		public void OnPlayerReload(ReloadingWeaponEventArgs ev)
		{
			if (Plugin.Cfg.PlayerReload)
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} {Plugin.translation.Reloaded}: {ev.Player.CurrentItem.id}.", HandleQueue.GameLogChannelId);
		}

		public void OnWarheadAccess(ActivatingWarheadPanelEventArgs ev)
		{
			if (Plugin.Cfg.WarheadAccess)
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} {Plugin.translation.AccessedWarhead}.", HandleQueue.GameLogChannelId);
		}

		public void OnElevatorInteraction(InteractingElevatorEventArgs ev)
		{
			if (Plugin.Cfg.Elevator)
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} {Plugin.translation.CalledElevator}.", HandleQueue.GameLogChannelId);
		}

		public void OnLockerInteraction(InteractingLockerEventArgs ev)
		{
			if (Plugin.Cfg.Locker)
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} {Plugin.translation.UsedLocker}.", HandleQueue.GameLogChannelId);
		}

		public void OnTriggerTesla(TriggeringTeslaEventArgs ev)
		{
			if (Plugin.Cfg.TriggerTesla)
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} {Plugin.translation.TriggeredTesla}.", HandleQueue.GameLogChannelId);
		}

		public void OnGenClosed(ClosingGeneratorEventArgs ev)
		{
			if (Plugin.Cfg.GenClose)
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} {Plugin.translation.GenClosed}.", HandleQueue.GameLogChannelId);
		}

		public void OnGenEject(EjectingGeneratorTabletEventArgs ev)
		{
			if (Plugin.Cfg.GenEject)
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} {Plugin.translation.GenEjected}.", HandleQueue.GameLogChannelId);
		}
		
		public void OnDoorInteract(InteractingDoorEventArgs ev)
		{
			if (Plugin.Cfg.DoorInteract)
				ProcessSTT.SendData(ev.Door.NetworkisOpen
						? $"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Role}) {Plugin.translation.HasClosedADoor}: {ev.Door.DoorName}."
						: $"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Role}) {Plugin.translation.HasOpenedADoor}: {ev.Door.DoorName}.",
					HandleQueue.GameLogChannelId);
		}

		public void On914Activation(ActivatingEventArgs ev)
		{
			if (Plugin.Cfg.Scp914Activation)
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Role}) {Plugin.translation.Scp914HasBeenActivated} {Scp914Machine.singleton.knobState}.", HandleQueue.GameLogChannelId);
		}

		public void On914KnobChange(ChangingKnobSettingEventArgs ev)
		{
			if (Plugin.Cfg.Scp914KnobChange)
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Role}) {Plugin.translation.Scp914Knobchange} {ev.KnobSetting}.", HandleQueue.GameLogChannelId);
		}

		public void OnPocketEnter(EnteringPocketDimensionEventArgs ev)
		{
			if (Plugin.Cfg.PocketEnter)
				ProcessSTT.SendData(
					$"{ev.Player.Nickname} - {ev.Player.Role} ({ev.Player.Role}) {Plugin.translation.HasEnteredPocketDimension}.",
					HandleQueue.GameLogChannelId);
		}

		public void OnPocketEscape(EscapingPocketDimensionEventArgs ev)
		{
			if (Plugin.Cfg.PocketEscape)
				ProcessSTT.SendData(
					$"{ev.Player.Nickname} - {ev.Player.Role} ({ev.Player.Role}) {Plugin.translation.HasEscapedPocketDimension}.",
					HandleQueue.GameLogChannelId);		}

		public void On106Teleport(TeleportingEventArgs ev)
		{
			if (Plugin.Cfg.Scp106Tele)
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} {Plugin.translation.HasEscapedPocketDimension}.", HandleQueue.GameLogChannelId);
		}

		public void On079Tesla(InteractingTeslaEventArgs ev)
		{
			if (Plugin.Cfg.Scp079Tesla)
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Role}) {Plugin.translation.HasTriggeredATeslaGate}.", HandleQueue.GameLogChannelId);
		}

		public void OnPlayerHurt(HurtingEventArgs ev)
		{
			if (Plugin.Cfg.PlayerHurt)
			{
				try
				{
					if (ev.Attacker != null && ev.Target.Role.GetTeam() == ev.Attacker.Role.GetTeam() && ev.Target != ev.Attacker)
						ProcessSTT.SendData(
							$"**{ev.Attacker.Nickname} - {ev.Attacker.UserId} ({ev.Attacker.Role}) {Plugin.translation.Damaged} {ev.Target.Nickname} - {ev.Target.UserId} ({ev.Target.Role}) {Plugin.translation._For} {ev.Amount} {Plugin.translation.With} {DamageTypes.FromIndex(ev.Tool).name}.**",
							HandleQueue.GameLogChannelId);
					else if (!Plugin.Cfg.OnlyFriendlyFire)
					{
						ProcessSTT.SendData(
								$"{ev.HitInformations.Attacker}  {Plugin.translation.Damaged} {ev.Target.Nickname} - {ev.Target.UserId} ({ev.Target.Role}) {Plugin.translation._For} {ev.Amount} {Plugin.translation.With} {DamageTypes.FromIndex(ev.Tool).name}.",
								HandleQueue.GameLogChannelId);
					}
				}
				catch (Exception e)
				{
					Log.Error($"Player Hurt error: {e}");
				}
			}
		}
		
		public void OnPlayerDeath(DiedEventArgs ev)
		{
			if (Plugin.Cfg.PlayerDeath)
			{
				try
				{
					if (ev.Killer != null && ev.Target.Role.GetTeam() == ev.Killer.Role.GetTeam())
						ProcessSTT.SendData(
							$"**{ev.Killer.Nickname} - {ev.Killer.UserId} ({ev.Killer.Role}) {Plugin.translation.Killed} {ev.Target.Nickname} - {ev.Target.UserId} ({ev.Target.Role}) {Plugin.translation.With} {DamageTypes.FromIndex(ev.HitInformations.Tool).name}.**",
							HandleQueue.GameLogChannelId);
					else if (!Plugin.Cfg.OnlyFriendlyFire)
					{
						ProcessSTT.SendData(
							$"{ev.HitInformations.Attacker} {Plugin.translation.Killed} {ev.Target.Nickname} - {ev.Target.UserId} ({ev.Target.Role}) {Plugin.translation.With} {DamageTypes.FromIndex(ev.HitInformations.Tool).name}.",
							HandleQueue.GameLogChannelId);
					}
				}
				catch (Exception e)
				{
					Log.Error($"Player Hurt error: {e}");
				}
			}
		}

		public void OnGrenadeThrown(ThrowingGrenadeEventArgs ev)
		{
			if (Plugin.Cfg.GrenadeThrown)
			{
				if (ev.Player == null)
					return;
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Role}) {Plugin.translation.ThrewAGrenade}.", HandleQueue.GameLogChannelId);
			}
		}

		public void OnMedicalItem(UsingMedicalItemEventArgs ev)
		{
			if (Plugin.Cfg.MedicalItem)
			{
				if (ev.Player == null)
					return;
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Role}) {Plugin.translation.UsedA} {ev.Item}", HandleQueue.GameLogChannelId);
			}
		}

		public void OnSetClass(ChangingRoleEventArgs ev)
		{
			if (Plugin.Cfg.SetClass)
			{
				if (ev.Player == null)
					return;
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} {Plugin.translation.HasBenChangedToA} {ev.NewRole}.", HandleQueue.GameLogChannelId);
			}
		}

		public void OnPlayerJoin(JoinedEventArgs ev)
		{
			if (Plugin.Cfg.RoleSync)
				Methods.CheckForSyncRole(ev.Player);
			if (Plugin.Cfg.PlayerJoin)
				if (ev.Player.Nickname != "Dedicated Server")
					ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} {Plugin.translation.HasJoinedTheGame}.", HandleQueue.GameLogChannelId);
		}

		public void OnPlayerFreed(RemovingHandcuffsEventArgs ev)
		{
			if (Plugin.Cfg.Freed)
				ProcessSTT.SendData(
					$"{ev.Target.Nickname} - {ev.Target.UserId} ({ev.Target.Role}) {Plugin.translation.HasBeenFreedBy} {ev.Cuffer.Nickname} - {ev.Cuffer.UserId} ({ev.Cuffer.Role})",
						HandleQueue.GameLogChannelId);
		}

		public void OnPlayerHandcuffed(HandcuffingEventArgs ev)
		{
			if (Plugin.Cfg.Cuffed)
				ProcessSTT.SendData(
					$"{ev.Target.Nickname} - {ev.Target.UserId} ({ev.Target.Role}) {Plugin.translation.HasBeenHandcuffedBy} {ev.Cuffer.Nickname} - {ev.Cuffer.UserId} ({ev.Cuffer.Role})",
						HandleQueue.GameLogChannelId);
		}

		public void OnPlayerBanned(BannedEventArgs ev)
		{
			if (Plugin.Cfg.Banned)
				ProcessSTT.SendData($"{ev.Details.OriginalName} - {ev.Details.Id} {Plugin.translation.WasBannedBy} {ev.Details.Issuer} {Plugin.translation._For} {ev.Details.Reason}. {new DateTime(ev.Details.Expires)}", HandleQueue.CommandLogChannelId);
		}

		public void OnIntercomSpeak(IntercomSpeakingEventArgs ev)
		{
			if (Plugin.Cfg.Intercom)
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Role}) {Plugin.translation.HasStartedUsingTheIntercom}.", HandleQueue.GameLogChannelId);
		}

		public void OnPickupItem(PickingUpItemEventArgs ev)
		{
			if (Plugin.Cfg.PickupItem)
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Role}) {Plugin.translation.HasPickedUp} {ev.Pickup.ItemId}.", HandleQueue.GameLogChannelId);
		}

		public void OnDropItem(ItemDroppedEventArgs ev)
		{
			if (Plugin.Cfg.DropItem)
				ProcessSTT.SendData($"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Role}) {Plugin.translation.HasDropped} {ev.Pickup.ItemId}.", HandleQueue.GameLogChannelId);
		}

		public void OnSetGroup(ChangingGroupEventArgs ev)
		{
			try
			{
				if (Plugin.Cfg.SetGroup)
					ProcessSTT.SendData(
						$"{ev.Player.Nickname} - {ev.Player.UserId} {Plugin.translation.GroupSet}: {ev.NewGroup.BadgeText} ({ev.NewGroup.BadgeColor}).",
						HandleQueue.GameLogChannelId);
			}
			catch (Exception e)
			{
				Log.Error(e.ToString());
			}
		}
    }
}