using System;
using System.Runtime.InteropServices.WindowsRuntime;
using CISpy;
using CISpy.API;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Scp914;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

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
				ProcessSTT.SendData($":chains:  {ev.Player.Nickname} {Plugin.translation.WasContained}.", HandleQueue.GameLogChannelId);
		}

		public void On106CreatePortal(CreatingPortalEventArgs ev)
		{
			if (Plugin.Singleton.Config.Scp106Portal)
				ProcessSTT.SendData($":nazar_amulet:  {ev.Player.Nickname} {Plugin.translation.CreatedPortal}.", HandleQueue.GameLogChannelId);
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
				ProcessSTT.SendData($":small_red_triangle: {ev.Player.Nickname} {Plugin.translation.GainedExp}: {ev.Amount}, {ev.GainType}.", HandleQueue.GameLogChannelId);
		}

		public void On079GainLvl(GainingLevelEventArgs ev)
		{
			if (Plugin.Singleton.Config.Scp079Lvl)
				ProcessSTT.SendData($"<:stonks:733310275415048222> {ev.Player.Nickname} {Plugin.translation.GainedLevel} {ev.OldLevel} -> {ev.NewLevel}.", HandleQueue.GameLogChannelId);
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
						? $"{ev.Player.Nickname} ({ev.Player.Role.Traduccion()}) {Plugin.translation.HasClosedADoor}: {ev.Door.DoorName}."
						: $"{ev.Player.Nickname} ({ev.Player.Role.Traduccion()}) {Plugin.translation.HasOpenedADoor}: {ev.Door.DoorName}.",
					HandleQueue.GameLogChannelId);
		}

		public void On914Activation(ActivatingEventArgs ev)
		{
			if (Plugin.Singleton.Config.Scp914Activation)
				ProcessSTT.SendData($":gear: {ev.Player.Nickname} - {ev.Player.Role.Traduccion()} {Plugin.translation.Scp914HasBeenActivated} {Scp914Machine.singleton.knobState}.", HandleQueue.GameLogChannelId);
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
				ProcessSTT.SendData($"{A} {ev.Player.Nickname} ({ev.Player.Role.Traduccion()}) {Plugin.translation.Scp914Knobchange} {ev.KnobSetting}.", HandleQueue.GameLogChannelId);
		}

		public void OnPocketEnter(EnteringPocketDimensionEventArgs ev)
		{
			if (Plugin.Singleton.Config.PocketEnter)
				ProcessSTT.SendData(
					$"<:argentinospordentro:772943936679182406> {ev.Player.Nickname} ({ev.Player.Role.Traduccion()}) {Plugin.translation.HasEnteredPocketDimension}.",
					HandleQueue.GameLogChannelId);
		}

		public void OnPocketEscape(EscapingPocketDimensionEventArgs ev)
		{
			if (Plugin.Singleton.Config.PocketEscape)
				ProcessSTT.SendData(
					$":door::man_running: {ev.Player.Nickname} ({ev.Player.Role.Traduccion()}) {Plugin.translation.HasEscapedPocketDimension}.",
					HandleQueue.GameLogChannelId);
		}

		public void On106Teleport(TeleportingEventArgs ev)
		{
			if (Plugin.Singleton.Config.Scp106Tele)
				ProcessSTT.SendData($":dagger: {ev.Player.Nickname} {Plugin.translation.HasEscapedPocketDimension}.", HandleQueue.GameLogChannelId);
		}

		public void On079Tesla(InteractingTeslaEventArgs ev)
		{
			if (Plugin.Singleton.Config.Scp079Tesla)
				ProcessSTT.SendData($":zap: {ev.Player.Nickname} ({ev.Player.Role.Traduccion()}) {Plugin.translation.HasTriggeredATeslaGate}.", HandleQueue.GameLogChannelId);
		}

		public void OnPlayerHurt(HurtingEventArgs ev)
		{

			if (Plugin.Singleton.Config.PlayerHurt)
			{
				try
				{
					if (ev.Attacker != null && ev.Attacker != ev.Target)
					{
						if (ev.Target.Side == ev.Attacker.Side)
						{
							if (IsSpy(ev.Attacker) || IsSpy(ev.Target))
								return;
							ProcessSTT.SendData($"<:Hitmark:777991200342147172> **{ev.Attacker.Nickname} - ID: {ev.Attacker.Id} - ({ev.Attacker.Role.Traduccion()})** {Plugin.translation.Damaged} **{ev.Target.Nickname} - ID: {ev.Target.Id} - ({ev.Target.Role.Traduccion()})** {Plugin.translation._For} {(int)ev.Amount}HP {Plugin.translation.With} {DamageTypes.FromIndex(ev.Tool).name}.",
							HandleQueue.GameLogChannelId);
						}
						else if (ev.Target.IsCuffed && ev.Attacker.Side != Side.Scp)
						{
							ProcessSTT.SendData($"<a:siren_blue:729921541625741344> **{ev.Attacker.Nickname} - ID: {ev.Attacker.Id} - ({ev.Attacker.Role.Traduccion()})** daño a **{ev.Target.Nickname} - ID: {ev.Target.Id} - ({ev.Target.Role.Traduccion()})** {Plugin.translation._For} {(int)ev.Amount}HP que esta arrestado, lo daño con {DamageTypes.FromIndex(ev.Tool).name}.",
									HandleQueue.GameLogChannelId);
						}
						


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
					if (ev.Killer != null && ev.Killer != ev.Target)
					{
						if (ev.Target.Side == ev.Killer.Side)
						{

							if (IsSpy(ev.Killer))
							{
								ProcessSTT.SendData($"<:Spy:777990138625261578> **{ev.Target.Nickname} - ID: {ev.Target.Id} ({ev.Target.Role.Traduccion()})** fue asesinado por un espia, con {DamageTypes.FromIndex(ev.HitInformation.Tool).name}. <:Spy:777990138625261578>",
								HandleQueue.GameLogChannelId);
								return;
							}
							else if (IsSpy(ev.Target))
							{
								ProcessSTT.SendData($"<:FBI:729918970446086225> **{ev.Killer.Nickname} - ID: {ev.Killer.Id} ({ev.Killer.Role.Traduccion()})** asesino a un espia que era **{ev.Target.Nickname}** - ID: {ev.Target.Id} fue asesinado con {DamageTypes.FromIndex(ev.HitInformation.Tool).name}. <:FBI:729918970446086225>  ",
								 HandleQueue.GameLogChannelId);
								return;
							}


							ProcessSTT.SendData($"<:RIP:777989450353475654>  **{ev.Killer.Nickname} - ID: {ev.Killer.Id} - ({ev.Killer.Role.Traduccion()})** {Plugin.translation.Killed} **{ev.Target.Nickname} - ID: {ev.Target.Id} ({ev.Target.Role.Traduccion()})** {Plugin.translation.With} {DamageTypes.FromIndex(ev.HitInformation.Tool).name}. <:RIP:777989450353475654> ",
							HandleQueue.GameLogChannelId);
						}
						else if (ev.Target.IsCuffed && ev.Killer.Side != Side.Scp)
						{
							ProcessSTT.SendData($"<:smallbrain:682243919278375016> ** {ev.Killer.Nickname} - ID: {ev.Killer.Id} - ({ev.Killer.Role.Traduccion()})** mato a  **{ev.Target.Nickname} - ID: {ev.Target.Id} - ({ev.Target.Role.Traduccion()})** que estaba arrestado, lo mato con {DamageTypes.FromIndex(ev.HitInformation.Tool).name}.",
							HandleQueue.GameLogChannelId);
						}


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
				switch (ev.Type)
				{
					case GrenadeType.FragGrenade:
						t = "Granada de Fragmentación";
						emoji = ":boom:";
						break;
					case GrenadeType.Flashbang:
						t = "Granada Cegadora";
						emoji = ":flashlight:";
						break;
					case GrenadeType.Scp018:
						t = "Bola (SCP-018)";
						emoji = ":red_circle:";
						break;
				}
				ProcessSTT.SendData($"{emoji} {ev.Player.Nickname} - ({ev.Player.Role.Traduccion()}) lanzó una {t}.", HandleQueue.GameLogChannelId);
			}
		}

		public void OnMedicalItem(UsedMedicalItemEventArgs ev)
		{
			if (Plugin.Singleton.Config.MedicalItem)
			{
				if (ev.Player == null)
					return;
				ProcessSTT.SendData($":medical_symbol: {ev.Player.Nickname} - ({ev.Player.Role.Traduccion()}) {Plugin.translation.UsedA} {ev.Item}", HandleQueue.GameLogChannelId);
			}
		}

		public void OnSetClass(ChangingRoleEventArgs ev)
		{
			if (Plugin.Singleton.Config.SetClass)
				try
				{
					if (ev.Player != null)
						ProcessSTT.SendData($":arrows_counterclockwise: {ev.Player.Nickname} - {Plugin.translation.HasBenChangedToA} {ev.NewRole}.", HandleQueue.GameLogChannelId);


				}
				catch (Exception e)
				{
					Log.Error($"Error en OnSetClass {e}");
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
					$":unlock: {ev.Target.Nickname} - ({ev.Target.Role.Traduccion()}) {Plugin.translation.HasBeenFreedBy} {ev.Cuffer.Nickname} - ({ev.Cuffer.Role.Traduccion()})",
						HandleQueue.GameLogChannelId);
		}

		public void OnPlayerHandcuffed(HandcuffingEventArgs ev)
		{
			if (Plugin.Singleton.Config.Cuffed)
				ProcessSTT.SendData(
					$":lock: {ev.Target.Nickname} - ({ev.Target.Role.Traduccion()}) {Plugin.translation.HasBeenHandcuffedBy} {ev.Cuffer.Nickname} - ({ev.Cuffer.Role.Traduccion()})",
						HandleQueue.GameLogChannelId);
		}

		public void OnIntercomSpeak(IntercomSpeakingEventArgs ev)
		{
			if (Plugin.Singleton.Config.Intercom)
				ProcessSTT.SendData($":loudspeaker: {ev.Player.Nickname} - ({ev.Player.Role.Traduccion()}) {Plugin.translation.HasStartedUsingTheIntercom}.", HandleQueue.GameLogChannelId);
		}

		public void OnPickupItem(PickingUpItemEventArgs ev)
		{
			if (Plugin.Singleton.Config.PickupItem)
				ProcessSTT.SendData($":outbox_tray: {ev.Player.Nickname} - ({ev.Player.Role.Traduccion()}) {Plugin.translation.HasPickedUp} {ev.Pickup.ItemId}.", HandleQueue.GameLogChannelId);
		}

		public void OnDropItem(ItemDroppedEventArgs ev)
		{
			if (Plugin.Singleton.Config.DropItem)
				ProcessSTT.SendData($":inbox_tray: {ev.Player.Nickname} - ({ev.Player.Role.Traduccion()}) {Plugin.translation.HasDropped} {ev.Pickup.ItemId}.", HandleQueue.GameLogChannelId);
		}

		/*public void OnSetGroup(ChangingGroupEventArgs ev)
		{
			try
			{
				if (Plugin.Singleton.Config.SetGroup && ev.NewGroup != null && ev.Player.Nickname != "Dedicated Server" && ev.NewGroup.BadgeText != null && ev.NewGroup.BadgeColor != null && ev.Player != null)
					ProcessSTT.SendData(
						$"<a:CerberusDance:742610186413277184> {ev.Player.Nickname} - {Plugin.translation.GroupSet}: **{ev.NewGroup.BadgeText} ({ev.NewGroup.BadgeColor})**.<a:CerberusDance:742610186413277184>",
						HandleQueue.GameLogChannelId);
				Log.Info("AAAAA pero de caca");
			}
			catch (Exception e)
			{
				Log.Error(e.ToString());
			}
		}*/

		public bool IsSpy(Player p)
		{
			try
			{

				return CISpy.EventHandlers.spies.ContainsKey(p);
			}
			catch (Exception)
			{
				Log.Error("No se encontró CiSpy");
				return false;
			}
		}
	}


}
