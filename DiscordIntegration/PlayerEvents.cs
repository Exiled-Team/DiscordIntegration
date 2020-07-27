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
			if (Plugin.Singleton.Config.GenInsert)
				ProcessSTT.SendData($"{ev.Player.Nickname} {Plugin.translation.GenInserted}.", HandleQueue.GameLogChannelId);
		}

		public void OnGenOpen(OpeningGeneratorEventArgs ev)
		{
			if (Plugin.Singleton.Config.GenOpen)
				ProcessSTT.SendData($"{ev.Player.Nickname} {Plugin.translation.GenOpened}.", HandleQueue.GameLogChannelId);
		}

		public void OnGenUnlock(UnlockingGeneratorEventArgs ev)
		{
			if (Plugin.Singleton.Config.GenUnlock)
				ProcessSTT.SendData($"{ev.Player.Nickname} {Plugin.translation.GenUnlocked}.", HandleQueue.GameLogChannelId);
		}

		public void On106Contain(ContainingEventArgs ev)
		{
			if (Plugin.Singleton.Config.Scp106Contain)
				ProcessSTT.SendData($"{ev.Player.Nickname} {Plugin.translation.WasContained}.", HandleQueue.GameLogChannelId);
		}

		public void On106CreatePortal(CreatingPortalEventArgs ev)
		{
			if (Plugin.Singleton.Config.Scp106Portal)
				ProcessSTT.SendData($"{ev.Player.Nickname} {Plugin.translation.CreatedPortal}.", HandleQueue.GameLogChannelId);
		}

		public void OnItemChanged(ChangingItemEventArgs ev)
		{
			if (Plugin.Singleton.Config.ItemChanged)
				if (Plugin.Singleton.Config.Scp106Portal)
					ProcessSTT.SendData($"{ev.Player.Nickname} {Plugin.translation.ItemChanged}: {ev.OldItem.id} -> {ev.NewItem.id}.", HandleQueue.GameLogChannelId);
		}

		public void On079GainExp(GainingExperienceEventArgs ev)
		{
			if (Plugin.Singleton.Config.Scp079Exp)
				ProcessSTT.SendData($":Stonks: {ev.Player.Nickname} {Plugin.translation.GainedExp}: {ev.Amount}, {ev.GainType}.", HandleQueue.GameLogChannelId);
		}

		public void On079GainLvl(GainingLevelEventArgs ev)
		{
			if (Plugin.Singleton.Config.Scp079Lvl)
				ProcessSTT.SendData($":Stonks: {ev.Player.Nickname} {Plugin.translation.GainedLevel} {ev.OldLevel} -> {ev.NewLevel}.", HandleQueue.GameLogChannelId);
		}

		public void OnPlayerLeave(LeftEventArgs ev)
		{
			if (Plugin.Singleton.Config.PlayerLeave)
				ProcessSTT.SendData($":arrow_left: **{ev.Player.Nickname} {Plugin.translation.LeftServer}.**", HandleQueue.GameLogChannelId);
		}

		public void OnPlayerReload(ReloadingWeaponEventArgs ev)
		{
			if (Plugin.Singleton.Config.PlayerReload)
				ProcessSTT.SendData($"{ev.Player.Nickname} {Plugin.translation.Reloaded}: {ev.Player.CurrentItem.id}.", HandleQueue.GameLogChannelId);
		}

		public void OnWarheadAccess(ActivatingWarheadPanelEventArgs ev)
		{
			if (Plugin.Singleton.Config.WarheadAccess)
				ProcessSTT.SendData($"{ev.Player.Nickname} {Plugin.translation.AccessedWarhead}.", HandleQueue.GameLogChannelId);
		}

		public void OnElevatorInteraction(InteractingElevatorEventArgs ev)
		{
			if (Plugin.Singleton.Config.Elevator)
				ProcessSTT.SendData($"{ev.Player.Nickname} {Plugin.translation.CalledElevator}.", HandleQueue.GameLogChannelId);
		}

		public void OnLockerInteraction(InteractingLockerEventArgs ev)
		{
			if (Plugin.Singleton.Config.Locker)
				ProcessSTT.SendData($"{ev.Player.Nickname} {Plugin.translation.UsedLocker}.", HandleQueue.GameLogChannelId);
		}

		public void OnTriggerTesla(TriggeringTeslaEventArgs ev)
		{
			if (Plugin.Singleton.Config.TriggerTesla)
				ProcessSTT.SendData($":zap: {ev.Player.Nickname} {Plugin.translation.TriggeredTesla}.", HandleQueue.GameLogChannelId);
		}

		public void OnGenClosed(ClosingGeneratorEventArgs ev)
		{
			if (Plugin.Singleton.Config.GenClose)
				ProcessSTT.SendData($"{ev.Player.Nickname} {Plugin.translation.GenClosed}.", HandleQueue.GameLogChannelId);
		}

		public void OnGenEject(EjectingGeneratorTabletEventArgs ev)
		{
			if (Plugin.Singleton.Config.GenEject)
				ProcessSTT.SendData($"{ev.Player.Nickname} {Plugin.translation.GenEjected}.", HandleQueue.GameLogChannelId);
		}
		
		public void OnDoorInteract(InteractingDoorEventArgs ev)
		{
			if (Plugin.Singleton.Config.DoorInteract)
				ProcessSTT.SendData(ev.Door.NetworkisOpen
						? $"{ev.Player.Nickname} ({ev.Player.Role}) {Plugin.translation.HasClosedADoor}: {ev.Door.DoorName}."
						: $"{ev.Player.Nickname} ({ev.Player.Role}) {Plugin.translation.HasOpenedADoor}: {ev.Door.DoorName}.",
					HandleQueue.GameLogChannelId);
		}

		public void On914Activation(ActivatingEventArgs ev)
		{
			if (Plugin.Singleton.Config.Scp914Activation)
				ProcessSTT.SendData($":gear: {ev.Player.Nickname} - {ev.Player.Role} {Plugin.translation.Scp914HasBeenActivated} {Scp914Machine.singleton.knobState}.", HandleQueue.GameLogChannelId);
		}

		public void On914KnobChange(ChangingKnobSettingEventArgs ev)
		{
			string A = "";
			switch (ev.KnobSetting)
			{
				case Scp914Knob.Rough:
					A = ":clock1030:";
					break;
				case Scp914Knob.Coarse:
					A = ":clock1130:";
					break;
				case Scp914Knob.OneToOne:
					A = ":clock1230:";
					break;
				case Scp914Knob.Fine:
					A = ":clock1:";
					break;
				case Scp914Knob.VeryFine:
					A = ":clock130:";
					break;
				default:
					A = "se bugeo xd";
					break;
			}
			if (Plugin.Singleton.Config.Scp914KnobChange)
				ProcessSTT.SendData($"{A} {ev.Player.Nickname} ({ev.Player.Role}) {Plugin.translation.Scp914Knobchange} {ev.KnobSetting}.", HandleQueue.GameLogChannelId);
		}

		public void OnPocketEnter(EnteringPocketDimensionEventArgs ev)
		{
			if (Plugin.Singleton.Config.PocketEnter)
				ProcessSTT.SendData(
					$"{ev.Player.Nickname} ({ev.Player.Role}) {Plugin.translation.HasEnteredPocketDimension}.",
					HandleQueue.GameLogChannelId);
		}

		public void OnPocketEscape(EscapingPocketDimensionEventArgs ev)
		{
			if (Plugin.Singleton.Config.PocketEscape)
				ProcessSTT.SendData(
					$":door::man_running: {ev.Player.Nickname} ({ev.Player.Role}) {Plugin.translation.HasEscapedPocketDimension}.",
					HandleQueue.GameLogChannelId);		}

		public void On106Teleport(TeleportingEventArgs ev)
		{
			if (Plugin.Singleton.Config.Scp106Tele)
				ProcessSTT.SendData($"{ev.Player.Nickname} {Plugin.translation.HasEscapedPocketDimension}.", HandleQueue.GameLogChannelId);
		}

		public void On079Tesla(InteractingTeslaEventArgs ev)
		{
			if (Plugin.Singleton.Config.Scp079Tesla)
				ProcessSTT.SendData($":zap: {ev.Player.Nickname} ({ev.Player.Role}) {Plugin.translation.HasTriggeredATeslaGate}.", HandleQueue.GameLogChannelId);
		}

		public void OnPlayerHurt(HurtingEventArgs ev)
		{
			if (Plugin.Singleton.Config.PlayerHurt)
			{
				try
				{
					if (ev.Attacker != null && ev.Target.Side == ev.Attacker.Side && ev.Target != ev.Attacker)
						ProcessSTT.SendData(
							$":crossed_swords: **{ev.Attacker.Nickname} ({ev.Attacker.Role}) {Plugin.translation.Damaged} {ev.Target.Nickname} ({ev.Target.Role}) {Plugin.translation._For} {ev.Amount} {Plugin.translation.With} {DamageTypes.FromIndex(ev.Tool).name}.**",
							HandleQueue.GameLogChannelId);
					else if (!Plugin.Singleton.Config.OnlyFriendlyFire)
					{
						ProcessSTT.SendData(
								$"{ev.HitInformations.Attacker}  {Plugin.translation.Damaged} {ev.Target.Nickname} ({ev.Target.Role}) {Plugin.translation._For} {ev.Amount} {Plugin.translation.With} {DamageTypes.FromIndex(ev.Tool).name}.",
								HandleQueue.GameLogChannelId);
					}
				}
				catch (Exception e)
				{
					Log.Error($"Player Hurt error: {e}");
				}
			}
		}
		
		public void OnPlayerDeath(DyingEventArgs ev)
		{
			if (Plugin.Singleton.Config.PlayerDeath)
			{
				try
				{
					if (ev.Killer != null && ev.Target.Side == ev.Killer.Side)
						ProcessSTT.SendData(
							$":x: **{ev.Killer.Nickname} - ({ev.Killer.Role}) {Plugin.translation.Killed} {ev.Target.Nickname} - ({ev.Target.Role}) {Plugin.translation.With} {DamageTypes.FromIndex(ev.HitInformation.Tool).name}.**",
							HandleQueue.GameLogChannelId);
					else if (!Plugin.Singleton.Config.OnlyFriendlyFire)
					{
						ProcessSTT.SendData(
							$":skull_crossbones: **{ev.Killer.Nickname} - ({ev.Killer.Role}) {Plugin.translation.Killed} {ev.Target.Nickname} - ({ev.Target.Role}) {Plugin.translation.With} {DamageTypes.FromIndex(ev.HitInformation.Tool).name}.**",
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
			if (Plugin.Singleton.Config.GrenadeThrown)
			{
				if (ev.Player == null)
					return;
				string t = "", emoji = "";
				switch (ev.Id)
				{
					case 0:
						t = "Granada de Fragmentación";
						emoji = ":boom:";
						break;
					case 1:
						t = "Granada Cegadora";
						emoji = ":flashlight:";
						break;
					case 2:
						t = "Bola (SCP-018)";
						emoji = ":red_circle:";
						break;
				}
				ProcessSTT.SendData($"{emoji} {ev.Player.Nickname} - ({ev.Player.Role}) lanzó una {t}.", HandleQueue.GameLogChannelId);
			}
		}

		public void OnMedicalItem(UsingMedicalItemEventArgs ev)
		{
			if (Plugin.Singleton.Config.MedicalItem)
			{
				if (ev.Player == null)
					return;
				ProcessSTT.SendData($":medical_symbol: {ev.Player.Nickname} - ({ev.Player.Role}) {Plugin.translation.UsedA} {ev.Item}", HandleQueue.GameLogChannelId);
			}
		}

		public void OnSetClass(ChangingRoleEventArgs ev)
		{
			if (Plugin.Singleton.Config.SetClass)
			{
				if (ev.Player == null)
					return;
				ProcessSTT.SendData($":arrows_counterclockwise: {ev.Player.Nickname} - {Plugin.translation.HasBenChangedToA} {ev.NewRole}.", HandleQueue.GameLogChannelId);
			}
		}

		public void OnPlayerJoin(JoinedEventArgs ev)
		{
			if (Plugin.Singleton.Config.RoleSync)
				Methods.CheckForSyncRole(ev.Player);
			if (Plugin.Singleton.Config.PlayerJoin)
				if (ev.Player.Nickname != "Dedicated Server")
					ProcessSTT.SendData($":arrow_right: **{ev.Player.Nickname} {Plugin.translation.HasJoinedTheGame}.**", HandleQueue.GameLogChannelId);
		}

		public void OnPlayerFreed(RemovingHandcuffsEventArgs ev)
		{
			if (Plugin.Singleton.Config.Freed)
				ProcessSTT.SendData(
					$":unlock: {ev.Target.Nickname} - ({ev.Target.Role}) {Plugin.translation.HasBeenFreedBy} {ev.Cuffer.Nickname} - ({ev.Cuffer.Role})",
						HandleQueue.GameLogChannelId);
		}

		public void OnPlayerHandcuffed(HandcuffingEventArgs ev)
		{
			if (Plugin.Singleton.Config.Cuffed)
				ProcessSTT.SendData(
					$":lock: {ev.Target.Nickname} - ({ev.Target.Role}) {Plugin.translation.HasBeenHandcuffedBy} {ev.Cuffer.Nickname} - ({ev.Cuffer.Role})",
						HandleQueue.GameLogChannelId);
		}

		public void OnPlayerBanned(BannedEventArgs ev)
		{
			if (Plugin.Singleton.Config.Banned)
				ProcessSTT.SendData($":no_entry: {ev.Details.OriginalName} - {ev.Details.Id} {Plugin.translation.WasBannedBy} {ev.Details.Issuer} {Plugin.translation._For} {ev.Details.Reason}. {new DateTime(ev.Details.Expires)}", HandleQueue.CommandLogChannelId);
		}

		public void OnIntercomSpeak(IntercomSpeakingEventArgs ev)
		{
			if (Plugin.Singleton.Config.Intercom)
				ProcessSTT.SendData($":loudspeaker: {ev.Player.Nickname} - ({ev.Player.Role}) {Plugin.translation.HasStartedUsingTheIntercom}.", HandleQueue.GameLogChannelId);
		}

		public void OnPickupItem(PickingUpItemEventArgs ev)
		{
			if (Plugin.Singleton.Config.PickupItem)
				ProcessSTT.SendData($":outbox_tray: {ev.Player.Nickname} - ({ev.Player.Role}) {Plugin.translation.HasPickedUp} {ev.Pickup.ItemId}.", HandleQueue.GameLogChannelId);
		}

		public void OnDropItem(ItemDroppedEventArgs ev)
		{
			if (Plugin.Singleton.Config.DropItem)
				ProcessSTT.SendData($":inbox_tray: {ev.Player.Nickname} - ({ev.Player.Role}) {Plugin.translation.HasDropped} {ev.Pickup.ItemId}.", HandleQueue.GameLogChannelId);
		}

		public void OnSetGroup(ChangingGroupEventArgs ev)
		{
			try
			{
				if (Plugin.Singleton.Config.SetGroup)
					ProcessSTT.SendData(
						$":heart: {ev.Player.Nickname} - {Plugin.translation.GroupSet}: **{ev.NewGroup.BadgeText} ({ev.NewGroup.BadgeColor})**.",
						HandleQueue.GameLogChannelId);
			}
			catch (Exception e)
			{
				Log.Error(e.ToString());
			}
		}
    }
}
