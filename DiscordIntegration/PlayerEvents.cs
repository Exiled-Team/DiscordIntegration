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
					$":eight_pointed_black_star: {ev.Player.Nickname} ({ev.Player.Role}) {Plugin.translation.HasEnteredPocketDimension}.",
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
				ProcessSTT.SendData($":dagger: {ev.Player.Nickname} {Plugin.translation.HasEscapedPocketDimension}.", HandleQueue.GameLogChannelId);
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
					if (ev.Attacker != null && ev.Attacker != ev.Target)
					{
						if (ev.Target.Side == ev.Attacker.Side)
						{
							if (IsSpy(ev.Attacker) || IsSpy(ev.Target))
								return;
							ProcessSTT.SendData($":crossed_swords: **{ev.Attacker.Nickname} - ID: {ev.Attacker.Id} - ({ev.Attacker.Role})** {Plugin.translation.Damaged} **{ev.Target.Nickname} - ID: {ev.Target.Id} - ({ev.Target.Role})** {Plugin.translation._For} {(int)ev.Amount}HP {Plugin.translation.With} {DamageTypes.FromIndex(ev.Tool).name}.",
							HandleQueue.GameLogChannelId);
						}
						else if (ev.Target.IsCuffed && ev.Attacker.Side != Side.Scp)
						{
							ProcessSTT.SendData($"<a:siren_blue:729921541625741344> **{ev.Attacker.Nickname} - ID: {ev.Attacker.Id} - ({ev.Attacker.Role})** daño a **{ev.Target.Nickname} - ID: {ev.Target.Id} - ({ev.Target.Role})** {Plugin.translation._For} {(int)ev.Amount}HP que esta arrestado, lo daño con {DamageTypes.FromIndex(ev.Tool).name}.",
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
								ProcessSTT.SendData($"<a:VenAqui:746307229505945687> **{ev.Target.Nickname} - ID: {ev.Target.Id} ({ev.Target.Role})** fue asesinado por un espia, con {DamageTypes.FromIndex(ev.HitInformation.Tool).name}. <a:VenAqui:746307229505945687>",
								HandleQueue.GameLogChannelId);
								return;
							}
							else if (IsSpy(ev.Target))
							{
								ProcessSTT.SendData($"<:FBI:729918970446086225> **{ev.Killer.Nickname} - ID: {ev.Killer.Id} ({ev.Killer.Role})** asesino a un espia, con {DamageTypes.FromIndex(ev.HitInformation.Tool).name}. <:FBI:729918970446086225>  ",
								 HandleQueue.GameLogChannelId);
								return;
							}

							ProcessSTT.SendData($"<a:jajajno:746302273386446879>  **{ev.Killer.Nickname} - ID: {ev.Killer.Id} - ({ev.Killer.Role})** {Plugin.translation.Killed} **{ev.Target.Nickname} - ID: {ev.Target.Id} ({ev.Target.Role})** {Plugin.translation.With} {DamageTypes.FromIndex(ev.HitInformation.Tool).name}. <a:jajajno:746302273386446879> ",
							HandleQueue.GameLogChannelId);
						}
						else if (ev.Target.IsCuffed && ev.Killer.Side != Side.Scp)
						{
							ProcessSTT.SendData($"<a:reeee:709898816131825694> ** {ev.Killer.Nickname} - ID: {ev.Killer.Id} - ({ev.Killer.Role})** mato a  **{ev.Target.Nickname} - ID: {ev.Target.Id} - ({ev.Target.Role})** que estaba arrestado, lo mato con {DamageTypes.FromIndex(ev.HitInformation.Tool).name}.",
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

		public void OnMedicalItem(UsedMedicalItemEventArgs ev)
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
				try
				{
					if (ev.Player != null)
						ProcessSTT.SendData($":arrows_counterclockwise: {ev.Player.Nickname} - {Plugin.translation.HasBenChangedToA} {ev.NewRole}.", HandleQueue.GameLogChannelId);

					/*else if (ev.Player != null && ev.NewRole.GetSide() == Side.ChaosInsurgency && IsSpy(ev.Player))
					{
						ProcessSTT.SendData($":mag_right: {ev.Player.Nickname} ID: {ev.Player.Id} SteamID: {ev.Player.UserId} ahora es un Spy.", HandleQueue.SpyLogID);
					}*/
					

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
				
		   ProcessSTT.SendData($":no_entry: **{ev.Details.OriginalName}** - ID: **{ev.Details.Id}** IP: **{ev.Player.IPAddress}** {Plugin.translation.WasBannedBy} **{ev.Details.Issuer}** {Plugin.translation._For} **{ev.Details.Reason}.** {new DateTime(ev.Details.Expires)}", HandleQueue.CommandLogChannelId);
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

		/*public void OnSetGroup(ChangingGroupEventArgs ev)
		{
			try
			{
				if (Plugin.Singleton.Config.SetGroup && ev.NewGroup != null && ev.Player.Nickname != "Dedicated Server" && ev.NewGroup.BadgeText != null && ev.NewGroup.BadgeColor != null && ev.Player != null)
					ProcessSTT.SendData(
						$"<a:CerberusDance:742610186413277184> {ev.Player.Nickname} - {Plugin.translation.GroupSet}: **{ev.NewGroup.BadgeText} ({ev.NewGroup.BadgeColor})**.<a:CerberusDance:742610186413277184>",
						HandleQueue.GameLogChannelId);
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
