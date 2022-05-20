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
    using Dependency;
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using PlayerStatsSystem;
    using Scp914;
    using static DiscordIntegration;

    /// <summary>
    /// Handles player-related events.
    /// </summary>
    internal sealed class PlayerHandler
    {
#pragma warning disable SA1600 // Elements should be documented
        public async void OnInsertingGeneratorTablet(ActivatingGeneratorEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerInsertingGeneratorTablet && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GeneratorInserted, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerInsertingGeneratorTablet)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.GeneratorInserted, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        public async void OnOpeningGenerator(OpeningGeneratorEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerOpeningGenerator && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GeneratorOpened, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerOpeningGenerator)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.GeneratorOpened, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        public async void OnUnlockingGenerator(UnlockingGeneratorEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerUnlockingGenerator && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GeneratorUnlocked, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerUnlockingGenerator)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.GeneratorUnlocked, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        public async void OnContaining(ContainingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ContainingScp106 && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.Scp106WasContained, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.ContainingScp106)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.Scp106WasContained, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        public async void OnCreatingPortal(CreatingPortalEventArgs ev)
        {
            if (Instance.Config.EventsToLog.CreatingScp106Portal && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.Scp106CreatedPortal, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.CreatingScp106Portal)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.Scp106CreatedPortal, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        public async void OnChangingItem(ChangingItemEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ChangingPlayerItem && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.ItemChanged, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.CurrentItem.Type, ev.NewItem.Type))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.ChangingPlayerItem)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.ItemChanged, ev.Player.Nickname, ev.Player.UserId, ev.Player.CurrentItem.Type, ev.NewItem.Type))).ConfigureAwait(false);
        }

        public async void OnGainingExperience(GainingExperienceEventArgs ev)
        {
            if (Instance.Config.EventsToLog.GainingScp079Experience && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GainedExperience, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role, ev.Amount, ev.GainType))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.GainingScp079Experience)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.GainedExperience, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, ev.Amount, ev.GainType))).ConfigureAwait(false);
        }

        public async void OnGainingLevel(GainingLevelEventArgs ev)
        {
            if (Instance.Config.EventsToLog.GainingScp079Level && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
#pragma warning disable CS0618 // Type or member is obsolete
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GainedLevel, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role, ev.NewLevel - 1, ev.NewLevel))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.GainingScp079Level)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.GainedLevel, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, ev.NewLevel - 1, ev.NewLevel))).ConfigureAwait(false);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public async void OnDestroying(DestroyingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerLeft && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.LeftServer, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerLeft)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.LeftServer, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        public async void OnReloadingWeapon(ReloadingWeaponEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ReloadingPlayerWeapon && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.Reloaded, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.CurrentItem.Type, ev.Player.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.ReloadingPlayerWeapon)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.Reloaded, ev.Player.Nickname, ev.Player.UserId, ev.Player.CurrentItem.Type, ev.Player.Role))).ConfigureAwait(false);
        }

        public async void OnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerActivatingWarheadPanel && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.AccessedWarhead, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerActivatingWarheadPanel)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.AccessedWarhead, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        public async void OnInteractingElevator(InteractingElevatorEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerInteractingElevator && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.CalledElevator, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerInteractingElevator)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.CalledElevator, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        public async void OnInteractingLocker(InteractingLockerEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerInteractingLocker && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.UsedLocker, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerInteractingLocker)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.UsedLocker, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        public async void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerTriggeringTesla && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasTriggeredATeslaGate, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerTriggeringTesla)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasTriggeredATeslaGate, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        public async void OnClosingGenerator(ClosingGeneratorEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerClosingGenerator && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GeneratorClosed, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerClosingGenerator)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.GeneratorClosed, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        public async void OnEjectingGeneratorTablet(StoppingGeneratorEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerEjectingGeneratorTablet && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GeneratorEjected, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerEjectingGeneratorTablet)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.GeneratorEjected, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        public async void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerInteractingDoor && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(ev.Door.IsOpen ? Language.HasClosedADoor : Language.HasOpenedADoor, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role, ev.Door.Nametag))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerInteractingDoor)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(ev.Door.IsOpen ? Language.HasClosedADoor : Language.HasOpenedADoor, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, ev.Door.Nametag))).ConfigureAwait(false);
        }

        public async void OnActivatingScp914(ActivatingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ActivatingScp914 && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.Scp914HasBeenActivated, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role, Scp914.KnobStatus))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.ActivatingScp914)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.Scp914HasBeenActivated, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, Scp914.KnobStatus))).ConfigureAwait(false);
        }

        public async void OnChangingScp914KnobSetting(ChangingKnobSettingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ChangingScp914KnobSetting && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.Scp914KnobSettingChanged, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role, ev.KnobSetting))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.ChangingScp914KnobSetting)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.Scp914KnobSettingChanged, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, ev.KnobSetting))).ConfigureAwait(false);
        }

        public async void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerEnteringPocketDimension && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasEnteredPocketDimension, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerEnteringPocketDimension)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasEnteredPocketDimension, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        public async void OnEscapingPocketDimension(EscapingPocketDimensionEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerEscapingPocketDimension && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasEscapedPocketDimension, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerEscapingPocketDimension)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasEscapedPocketDimension, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        public async void OnTeleporting(TeleportingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.Scp106Teleporting && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.Scp106Teleported, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.Scp106Teleporting)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.Scp106Teleported, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        public async void OnInteractingTesla(InteractingTeslaEventArgs ev)
        {
            if (Instance.Config.EventsToLog.Scp079InteractingTesla && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasTriggeredATeslaGate, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.Scp079InteractingTesla)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasTriggeredATeslaGate, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        public async void OnHurting(HurtingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.HurtingPlayer && ev.Target != null && (ev.Attacker == null || !Instance.Config.ShouldLogFriendlyFireDamageOnly || ev.Attacker.Role.Side == ev.Target.Role.Side) && (!Instance.Config.ShouldRespectDoNotTrack || (ev.Attacker == null || (!ev.Attacker.DoNotTrack && !ev.Target.DoNotTrack))) && !Instance.Config.BlacklistedDamageTypes.Contains(ev.Handler.Type) && (!Instance.Config.OnlyLogPlayerDamage || ev.Attacker != null))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasDamagedForWith, ev.Attacker != null ? ev.Attacker.Nickname : "Server", Instance.Config.ShouldLogUserIds ? ev.Attacker != null ? ev.Attacker.UserId : string.Empty : Language.Redacted, ev.Attacker?.Role ?? RoleType.None, ev.Target.Nickname, Instance.Config.ShouldLogUserIds ? ev.Target.UserId : Language.Redacted, ev.Target.Role, ev.Amount, ev.Handler.Type))).ConfigureAwait(false);

            if (Instance.Config.StaffOnlyEventsToLog.HurtingPlayer && ev.Target != null && (ev.Attacker == null || !Instance.Config.ShouldLogFriendlyFireDamageOnly || ev.Attacker.Role.Side == ev.Target.Role.Side) && !Instance.Config.BlacklistedDamageTypes.Contains(ev.Handler.Type) && (!Instance.Config.OnlyLogPlayerDamage || ev.Attacker != null))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasDamagedForWith, ev.Attacker != null ? ev.Attacker.Nickname : "Server", Instance.Config.ShouldLogUserIds ? ev.Attacker != null ? ev.Attacker.UserId : string.Empty : Language.Redacted, ev.Attacker?.Role ?? RoleType.None, ev.Target.Nickname, Instance.Config.ShouldLogUserIds ? ev.Target.UserId : Language.Redacted, ev.Target.Role, ev.Amount, ev.Handler.Type))).ConfigureAwait(false);
        }

        public async void OnDying(DyingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerDying && ev.Target != null && (ev.Killer == null || !Instance.Config.ShouldLogFriendlyFireKillsOnly || ev.Killer.Role.Side == ev.Target.Role.Side) && (!Instance.Config.ShouldRespectDoNotTrack || (ev.Killer == null || (!ev.Killer.DoNotTrack && !ev.Target.DoNotTrack))))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasKilledWith, ev.Killer != null ? ev.Killer.Nickname : "Server", Instance.Config.ShouldLogUserIds ? ev.Killer != null ? ev.Killer.UserId : string.Empty : Language.Redacted, ev.Killer?.Role ?? RoleType.None, ev.Target.Nickname, Instance.Config.ShouldLogUserIds ? ev.Target.UserId : Language.Redacted, ev.Target.Role, ev.Handler.Type))).ConfigureAwait(false);

            if (Instance.Config.StaffOnlyEventsToLog.PlayerDying && ev.Target != null && (ev.Killer == null || !Instance.Config.ShouldLogFriendlyFireKillsOnly || ev.Killer.Role.Side == ev.Target.Role.Side))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasKilledWith, ev.Killer != null ? ev.Killer.Nickname : "Server", Instance.Config.ShouldLogUserIds ? ev.Killer != null ? ev.Killer.UserId : string.Empty : Language.Redacted, ev.Killer?.Role ?? RoleType.None, ev.Target.Nickname, Instance.Config.ShouldLogUserIds ? ev.Target.UserId : Language.Redacted, ev.Target.Role, ev.Handler.Type))).ConfigureAwait(false);
        }

        public async void OnThrowingGrenade(ThrowingItemEventArgs ev)
        {
            if (ev.Player != null && Instance.Config.EventsToLog.PlayerThrowingGrenade && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.ThrewAGrenade, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role, ev.Item.Type))).ConfigureAwait(false);
            if (ev.Player != null && Instance.Config.StaffOnlyEventsToLog.PlayerThrowingGrenade)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.ThrewAGrenade, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, ev.Item.Type))).ConfigureAwait(false);
        }

        public async void OnUsedMedicalItem(UsedItemEventArgs ev)
        {
            if (ev.Player != null && Instance.Config.EventsToLog.PlayerUsedMedicalItem && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.UsedMedicalItem, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role, ev.Item))).ConfigureAwait(false);
            if (ev.Player != null && Instance.Config.StaffOnlyEventsToLog.PlayerUsedMedicalItem)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.UsedMedicalItem, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, ev.Item))).ConfigureAwait(false);
        }

        public async void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player != null && Instance.Config.EventsToLog.ChangingPlayerRole && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.ChangedRole, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role, ev.NewRole))).ConfigureAwait(false);
            if (ev.Player != null && Instance.Config.StaffOnlyEventsToLog.ChangingPlayerRole)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.ChangedRole, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, ev.NewRole))).ConfigureAwait(false);
        }

        public async void OnVerified(VerifiedEventArgs ev)
        {
            if (Instance.Config.ShouldSyncRoles)
            {
                SyncedUser syncedUser = Instance.SyncedUsersCache.FirstOrDefault(tempSyncedUser => tempSyncedUser?.Id == ev.Player.UserId);

                if (syncedUser == null)
                    await Network.SendAsync(new RemoteCommand(ActionType.SetGroupFromId, ev.Player.UserId)).ConfigureAwait(false);
                else
                    syncedUser?.SetGroup();
            }

            if (Instance.Config.EventsToLog.PlayerJoined && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasJoinedTheGame, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, Instance.Config.ShouldLogIPAddresses ? ev.Player.IPAddress : Language.Redacted))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerJoined)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasJoinedTheGame, ev.Player.Nickname, ev.Player.UserId, ev.Player.IPAddress))).ConfigureAwait(false);
        }

        public async void OnRemovingHandcuffs(RemovingHandcuffsEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerRemovingHandcuffs && ((!ev.Cuffer.DoNotTrack && !ev.Target.DoNotTrack) || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasBeenFreedBy, ev.Target.Nickname, Instance.Config.ShouldLogUserIds ? ev.Target.UserId : Language.Redacted, ev.Target.Role, ev.Cuffer.Nickname, Instance.Config.ShouldLogUserIds ? ev.Cuffer.UserId : Language.Redacted, ev.Cuffer.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerRemovingHandcuffs)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasBeenFreedBy, ev.Target.Nickname, ev.Target.UserId, ev.Target.Role, ev.Cuffer.Nickname, ev.Cuffer.UserId, ev.Cuffer.Role))).ConfigureAwait(false);
        }

        public async void OnHandcuffing(HandcuffingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.HandcuffingPlayer && ((!ev.Cuffer.DoNotTrack && !ev.Target.DoNotTrack) || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasBeenHandcuffedBy, ev.Target.Nickname, Instance.Config.ShouldLogUserIds ? ev.Target.UserId : Language.Redacted, ev.Target.Role, ev.Cuffer.Nickname, Instance.Config.ShouldLogUserIds ? ev.Cuffer.UserId : Language.Redacted, ev.Cuffer.Role))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.HandcuffingPlayer)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasBeenHandcuffedBy, ev.Target.Nickname, ev.Target.UserId, ev.Target.Role, ev.Cuffer.Nickname, ev.Cuffer.UserId, ev.Cuffer.Role))).ConfigureAwait(false);
        }

        public async void OnKicked(KickedEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerBanned)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, "kicks", string.Format(Language.WasKicked, ev.Target?.Nickname ?? Language.NotAuthenticated, ev.Target?.UserId ?? Language.NotAuthenticated, ev.Reason))).ConfigureAwait(false);
        }

        public async void OnBanned(BannedEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerBanned)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, "bans", string.Format(Language.WasBannedBy, ev.Details.OriginalName, ev.Details.Id, ev.Details.Issuer, ev.Details.Reason, new DateTime(ev.Details.Expires).ToString(Instance.Config.DateFormat)))).ConfigureAwait(false);
        }

        public async void OnIntercomSpeaking(IntercomSpeakingEventArgs ev)
        {
            if (ev.Player != null && Instance.Config.EventsToLog.PlayerIntercomSpeaking && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasStartedUsingTheIntercom, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
            if (ev.Player != null && Instance.Config.StaffOnlyEventsToLog.PlayerIntercomSpeaking)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasStartedUsingTheIntercom, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        public async void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerPickingupItem && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasPickedUp, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role, ev.Pickup.Type))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerPickingupItem)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasPickedUp, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, ev.Pickup.Type))).ConfigureAwait(false);
        }

        public async void OnItemDropped(DroppingItemEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerItemDropped && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.HasDropped, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role, ev.Item.Type))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.PlayerItemDropped)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.HasDropped, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, ev.Item.Type))).ConfigureAwait(false);
        }

        public async void OnChangingGroup(ChangingGroupEventArgs ev)
        {
            if (ev.Player != null && Instance.Config.EventsToLog.ChangingPlayerGroup && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.GroupSet, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role, ev.NewGroup?.BadgeText ?? Language.None, ev.NewGroup?.BadgeColor ?? Language.None))).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.ChangingPlayerGroup)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(Language.GroupSet, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, ev.NewGroup?.BadgeText ?? Language.None, ev.NewGroup?.BadgeColor ?? Language.None))).ConfigureAwait(false);
        }
    }
}