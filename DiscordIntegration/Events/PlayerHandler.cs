// -----------------------------------------------------------------------
// <copyright file="PlayerHandler.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.Events
{
    using System;
    using System.Linq;
    using API.Commands;
    using API.User;
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.Events.EventArgs;
    using Scp914;
    using Exiled.CustomItems.API;
    using Exiled.CustomItems.API.Features;
    using Exiled.CustomItems.API.Spawn;
    using static DiscordIntegration;

    /// <summary>
    /// Handles player-related events.
    /// </summary>
    internal sealed class PlayerHandler
    {
#pragma warning disable SA1600 // Elements should be documented
        public async void OnInsertingGeneratorTablet(InsertingGeneratorTabletEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerInsertingGeneratorTablet && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GeneratorInserted, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnOpeningGenerator(OpeningGeneratorEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerOpeningGenerator && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GeneratorOpened, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnUnlockingGenerator(UnlockingGeneratorEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerUnlockingGenerator && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GeneratorUnlocked, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
        }

        public async void OnContaining(ContainingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ContainingScp106 && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.Scp106WasContained, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnCreatingPortal(CreatingPortalEventArgs ev)
        {
            if (Instance.Config.EventsToLog.CreatingScp106Portal && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.Scp106CreatedPortal, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnChangingItem(ChangingItemEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ChangingPlayerItem && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.ItemChanged, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.OldItem.id, ev.NewItem.id))).ConfigureAwait(false);
        }

        public async void OnGainingExperience(GainingExperienceEventArgs ev)
        {
            if (Instance.Config.EventsToLog.GainingScp079Experience && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GainedExperience, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate(), ev.Amount, ev.GainType))).ConfigureAwait(false);
        }

        public async void OnGainingLevel(GainingLevelEventArgs ev)
        {
            if (Instance.Config.EventsToLog.GainingScp079Level && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
#pragma warning disable CS0618 // Type or member is obsolete
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GainedLevel, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate(), ev.OldLevel, ev.NewLevel))).ConfigureAwait(false);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public async void OnDestroying(DestroyingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerLeft && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.LeftServer, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnReloadingWeapon(ReloadingWeaponEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ReloadingPlayerWeapon && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.Reloaded, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.CurrentItem.id, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerActivatingWarheadPanel && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.AccessedWarhead, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnInteractingElevator(InteractingElevatorEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerInteractingElevator && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.CalledElevator, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnInteractingLocker(InteractingLockerEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerInteractingLocker && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.UsedLocker, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerTriggeringTesla && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasTriggeredATeslaGate, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnClosingGenerator(ClosingGeneratorEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerClosingGenerator && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GeneratorClosed, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnEjectingGeneratorTablet(EjectingGeneratorTabletEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerEjectingGeneratorTablet && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GeneratorEjected, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerInteractingDoor && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(ev.Door.NetworkTargetState ? Language.HasClosedADoor : Language.HasOpenedADoor, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate(), ev.Door.GetNametag()))).ConfigureAwait(false);
        }

        public async void OnActivatingScp914(ActivatingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ActivatingScp914 && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.Scp914HasBeenActivated, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate(), Scp914Machine.singleton.knobState))).ConfigureAwait(false);
        }

        public async void OnChangingScp914KnobSetting(ChangingKnobSettingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ChangingScp914KnobSetting && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.Scp914KnobSettingChanged, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate(), ev.KnobSetting))).ConfigureAwait(false);
        }

        public async void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerEnteringPocketDimension && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasEnteredPocketDimension, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnEscapingPocketDimension(EscapingPocketDimensionEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerEscapingPocketDimension && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasEscapedPocketDimension, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnTeleporting(TeleportingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.Scp106Teleporting && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.Scp106Teleported, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnInteractingTesla(InteractingTeslaEventArgs ev)
        {
            if (Instance.Config.EventsToLog.Scp079InteractingTesla && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasTriggeredATeslaGate, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnHurting(HurtingEventArgs ev)
        {
            // Verifico si el tipo tiene un medigun primero, y si no paso al log normal.
            if (Instance.Config.EventsToLog.HurtingPlayer &&
                ev.Attacker != null &&
                ev.Target != null &&
                (!ev.Attacker.DoNotTrack || !ev.Target.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack) &&
                (!Instance.Config.ShouldLogFriendlyFireDamageOnly || (Instance.Config.ShouldLogFriendlyFireDamageOnly && ev.Attacker.Side == ev.Target.Side && ev.Attacker != ev.Target)) && CustomItem.TryGet(ev.Attacker.CurrentItem, out CustomItem medigun) && medigun.CheckMediGun())
            {
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"<a:MediGun:841062245349720094> | **{ev.Attacker.Nickname}** (ID: {ev.Attacker.Id} - ROL: {ev.Attacker.Role.Translate()}) **esta curando** a **{ev.Target.Nickname}** (ID: {ev.Target.Id} ROL: {ev.Target.Role.Translate()} Con su arma que es {DamageTypes.FromIndex(ev.Tool).name} | Curacion hecha: {ev.Amount}")).ConfigureAwait(false);
            }
            else if (Instance.Config.EventsToLog.HurtingPlayer &&
               ev.Attacker != null &&
               ev.Target != null &&
               (!ev.Attacker.DoNotTrack || !ev.Target.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack) &&
               (!Instance.Config.ShouldLogFriendlyFireDamageOnly || (Instance.Config.ShouldLogFriendlyFireDamageOnly && ev.Attacker.Side == ev.Target.Side && ev.Attacker != ev.Target)))
            {
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasDamagedForWith, ev.Attacker.Nickname, Instance.Config.ShouldLogUserIds ? ev.Attacker.Id.ToString() : Language.Redacted, ev.Attacker.Role.Translate(), ev.Target.Nickname, ev.Target.Id.ToString(), ev.Target.Role.Translate(), ev.Amount, DamageTypes.FromIndex(ev.Tool).name))).ConfigureAwait(false);
            }
            else
            {
                return;
            }
        }

        public async void OnDying(DyingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerDying &&
                ev.Killer != null &&
                ev.Target != null &&
                (!ev.Killer.DoNotTrack || !ev.Target.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack) &&
                (!Instance.Config.ShouldLogFriendlyFireKillsOnly || (Instance.Config.ShouldLogFriendlyFireKillsOnly && ev.Killer.Side == ev.Target.Side && ev.Killer != ev.Target)))
            {
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasKilledWith, ev.Killer.Nickname, ev.Killer.Id.ToString(), ev.Killer.Role.Translate(), ev.Target.Nickname, ev.Target.Id.ToString(), ev.Target.Role.Translate(), DamageTypes.FromIndex(ev.HitInformation.Tool).name))).ConfigureAwait(false);
            }
        }

        public async void OnThrowingGrenade(ThrowingGrenadeEventArgs ev)
        {
            if (ev.Player != null && Instance.Config.EventsToLog.PlayerThrowingGrenade && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
            {
                string t = string.Empty;
                string emoji = string.Empty;
                switch (ev.Type)
                {
                    case GrenadeType.FragGrenade:
                        t = "Granada de Fragmentación";
                        emoji = "<:Grenade:816748774838763521> ";
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

                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(emoji + Language.ThrewAGrenade, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate(), t))).ConfigureAwait(false);
            }
        }

        public async void OnUsedMedicalItem(UsedMedicalItemEventArgs ev)
        {
            if (ev.Player != null && Instance.Config.EventsToLog.PlayerUsedMedicalItem && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.UsedMedicalItem, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate(), ev.Item))).ConfigureAwait(false);
        }

        public async void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player != null && Instance.Config.EventsToLog.ChangingPlayerRole && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.ChangedRole, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate(), ev.NewRole))).ConfigureAwait(false);
        }

        public async void OnVerified(VerifiedEventArgs ev)
        {
            if (Instance.Config.ShouldSyncRoles)
            {
                SyncedUser syncedUser = Instance.SyncedUsersCache.Where(tempSyncedUser => tempSyncedUser?.Id == ev.Player.UserId).FirstOrDefault();

                if (syncedUser == null)
                    await Network.SendAsync(new RemoteCommand("getGroupFromId", ev.Player.Id.ToString())).ConfigureAwait(false);
                else
                    syncedUser?.SetGroup();
            }

            if (Instance.Config.EventsToLog.PlayerJoined && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasJoinedTheGame, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, Instance.Config.ShouldLogIPAddresses ? ev.Player.IPAddress : Language.Redacted))).ConfigureAwait(false);
        }

        public async void OnRemovingHandcuffs(RemovingHandcuffsEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerRemovingHandcuffs && (!ev.Cuffer.DoNotTrack || !ev.Target.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasBeenFreedBy, ev.Target.Nickname, Instance.Config.ShouldLogUserIds ? ev.Target.Id.ToString() : Language.Redacted, ev.Target.Role.Translate(), ev.Cuffer.Nickname, Instance.Config.ShouldLogUserIds ? ev.Cuffer.Id.ToString() : Language.Redacted, ev.Cuffer.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnHandcuffing(HandcuffingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.HandcuffingPlayer && (!ev.Cuffer.DoNotTrack || !ev.Target.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasBeenHandcuffedBy, ev.Target.Nickname, Instance.Config.ShouldLogUserIds ? ev.Target.Id.ToString() : Language.Redacted, ev.Target.Role.Translate(), ev.Cuffer.Nickname, Instance.Config.ShouldLogUserIds ? ev.Cuffer.Id.ToString() : Language.Redacted, ev.Cuffer.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnKicked(KickedEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerBanned)
                await Network.SendAsync(new RemoteCommand("log", "kicks", string.Format(Language.WasKicked, ev.Target?.Nickname ?? Language.NotAuthenticated, ev.Target?.Id.ToString() ?? Language.NotAuthenticated, ev.Reason))).ConfigureAwait(false);
        }

        public async void OnBanned(BannedEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerBanned)
                await Network.SendAsync(new RemoteCommand("log", "bans", string.Format(Language.WasBannedBy, ev.Details.OriginalName, ev.Details.Id, ev.Details.Issuer, ev.Details.Reason, new DateTime(ev.Details.Expires)))).ConfigureAwait(false);
        }

        public async void OnIntercomSpeaking(IntercomSpeakingEventArgs ev)
        {
            if (ev.Player != null && Instance.Config.EventsToLog.PlayerIntercomSpeaking && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasStartedUsingTheIntercom, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerPickingupItem && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasPickedUp, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate(), ev.Pickup.ItemId))).ConfigureAwait(false);
        }

        public async void OnItemDropped(ItemDroppedEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerItemDropped && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasDropped, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate(), ev.Pickup.ItemId))).ConfigureAwait(false);
        }

        public async void OnChangingGroup(ChangingGroupEventArgs ev)
        {
            if (ev.Player != null && Instance.Config.EventsToLog.ChangingPlayerGroup && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GroupSet, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.Id.ToString() : Language.Redacted, ev.Player.Role.Translate(), ev.NewGroup?.BadgeText ?? Language.None, ev.NewGroup?.BadgeColor ?? Language.None))).ConfigureAwait(false);
        }
    }
}