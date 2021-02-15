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
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using Features;
    using Features.Commands;
    using Interactables.Interobjects.DoorUtils;
    using Scp914;
    using static DiscordIntegration;

    /// <summary>
    /// Handles player-related events.
    /// </summary>
    internal sealed class PlayerHandler
    {
#pragma warning disable SA1600 // Elements should be documented
        public async void OnInsertingGeneratorTablet(InsertingGeneratorTabletEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerInsertingGeneratorTablet)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{ev.Player.Nickname} - {ev.Player.UserId} {DiscordIntegration.Language.GeneratorInserted}.")).ConfigureAwait(false);
        }

        public async void OnOpeningGenerator(OpeningGeneratorEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerOpeningGenerator)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{ev.Player.Nickname} - {ev.Player.UserId} {DiscordIntegration.Language.GeneratorOpened}.")).ConfigureAwait(false);
        }

        public async void OnUnlockingGenerator(UnlockingGeneratorEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerUnlockingGenerator)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{ev.Player.Nickname} - {ev.Player.UserId} {DiscordIntegration.Language.GeneratorUnlocked}.")).ConfigureAwait(false);
        }

        public async void OnContaining(ContainingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ContainingScp106)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{ev.Player.Nickname} - {ev.Player.UserId} {DiscordIntegration.Language.WasContained}.")).ConfigureAwait(false);
        }

        public async void OnCreatingPortal(CreatingPortalEventArgs ev)
        {
            if (Instance.Config.EventsToLog.CreatingScp106Portal)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{ev.Player.Nickname} - {ev.Player.UserId} {DiscordIntegration.Language.CreatedPortal}.")).ConfigureAwait(false);
        }

        public async void OnChangingItem(ChangingItemEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ChangingPlayerItem)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{ev.Player.Nickname} - {ev.Player.UserId} {DiscordIntegration.Language.ItemChanged}: {ev.OldItem.id} -> {ev.NewItem.id}.")).ConfigureAwait(false);
        }

        public async void OnGainingExperience(GainingExperienceEventArgs ev)
        {
            if (Instance.Config.EventsToLog.GainingScp079Experience)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{ev.Player.Nickname} - {ev.Player.UserId} {DiscordIntegration.Language.GainedExperience}: {ev.Amount}, {ev.GainType}.")).ConfigureAwait(false);
        }

        public async void OnGainingLevel(GainingLevelEventArgs ev)
        {
            if (Instance.Config.EventsToLog.GainingScp079Level)
#pragma warning disable CS0618
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{ev.Player.Nickname} - {ev.Player.UserId} {DiscordIntegration.Language.GainedLevel} {ev.OldLevel} -> {ev.NewLevel}.")).ConfigureAwait(false);
#pragma warning restore CS0618
        }

        public async void OnDestroying(DestroyingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerLeft)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $":arrow_left: **{ev.Player.Nickname} - {ev.Player.UserId} {DiscordIntegration.Language.LeftServer}.**")).ConfigureAwait(false);
        }

        public async void OnReloadingWeapon(ReloadingWeaponEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ReloadingPlayerWeapon)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{ev.Player.Nickname} - {ev.Player.UserId} {DiscordIntegration.Language.Reloaded}: {ev.Player.CurrentItem.id}.")).ConfigureAwait(false);
        }

        public async void OnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerActivatingWarheadPanel)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{ev.Player.Nickname} - {ev.Player.UserId} {DiscordIntegration.Language.AccessedWarhead}.")).ConfigureAwait(false);
        }

        public async void OnInteractingElevator(InteractingElevatorEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerInteractingElevator)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{ev.Player.Nickname} - {ev.Player.UserId} {DiscordIntegration.Language.CalledElevator}.")).ConfigureAwait(false);
        }

        public async void OnInteractingLocker(InteractingLockerEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerInteractingLocker)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{ev.Player.Nickname} - {ev.Player.UserId} {DiscordIntegration.Language.UsedLocker}.")).ConfigureAwait(false);
        }

        public async void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerTriggeringTesla)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{ev.Player.Nickname} - {ev.Player.UserId} {DiscordIntegration.Language.TriggeredTesla}.")).ConfigureAwait(false);
        }

        public async void OnClosingGenerator(ClosingGeneratorEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerClosingGenerator)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{ev.Player.Nickname} - {ev.Player.UserId} {DiscordIntegration.Language.GeneratorClosed}.")).ConfigureAwait(false);
        }

        public async void OnEjectingGeneratorTablet(EjectingGeneratorTabletEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerEjectingGeneratorTablet)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{ev.Player.Nickname} - {ev.Player.UserId} {DiscordIntegration.Language.GeneratorEjected}.")).ConfigureAwait(false);
        }

        public async void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerInteractingDoor)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Role}) {(ev.Door.NetworkTargetState ? DiscordIntegration.Language.HasClosedADoor : DiscordIntegration.Language.HasOpenedADoor)}: {ev.Door.GetComponent<DoorNametagExtension>().GetName}.")).ConfigureAwait(false);
        }

        public async void OnActivatingScp914(ActivatingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ActivatingScp914)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Role}) {DiscordIntegration.Language.Scp914HasBeenActivated} {Scp914Machine.singleton.knobState}.")).ConfigureAwait(false);
        }

        public async void OnChangingScp914KnobSetting(ChangingKnobSettingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ChangingScp914KnobSetting)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Role}) {DiscordIntegration.Language.Scp914KnobSettingChanged} {ev.KnobSetting}.")).ConfigureAwait(false);
        }

        public async void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerEnteringPocketDimension)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $":door: {ev.Player.Nickname} - {ev.Player.Role} ({ev.Player.Role}) {DiscordIntegration.Language.HasEnteredPocketDimension}.")).ConfigureAwait(false);
        }

        public async void OnEscapingPocketDimension(EscapingPocketDimensionEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerEscapingPocketDimension)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $":high_brightness: {ev.Player.Nickname} - {ev.Player.Role} ({ev.Player.Role}) {DiscordIntegration.Language.HasEscapedPocketDimension}.")).ConfigureAwait(false);
        }

        public async void OnTeleporting(TeleportingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.Scp106Teleporting)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{ev.Player.Nickname} - {ev.Player.UserId} {DiscordIntegration.Language.HasEscapedPocketDimension}.")).ConfigureAwait(false);
        }

        public async void OnInteractingTesla(InteractingTeslaEventArgs ev)
        {
            if (Instance.Config.EventsToLog.Scp079InteractingTesla)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $":zap: {ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Role}) {DiscordIntegration.Language.HasTriggeredATeslaGate}.")).ConfigureAwait(false);
        }

        public async void OnHurting(HurtingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.HurtingPlayer)
            {
                try
                {
                    if (ev.Attacker != null && ev.Target.Role.GetTeam() == ev.Attacker.Role.GetTeam() && ev.Target != ev.Attacker)
                        await Network.SendAsync(new RemoteCommand("log", "gameEvents", $":crossed_swords: **{ev.Attacker.Nickname} - {ev.Attacker.UserId} ({ev.Attacker.Role}) {DiscordIntegration.Language.Damaged} {ev.Target.Nickname} - {ev.Target.UserId} ({ev.Target.Role}) {DiscordIntegration.Language.For} {ev.Amount} {DiscordIntegration.Language.With} {DamageTypes.FromIndex(ev.Tool).name}.**")).ConfigureAwait(false);
                    else if (!Instance.Config.ShouldLogFriendlyFireOnly)
                        await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{ev.HitInformations.Attacker} {DiscordIntegration.Language.Damaged} {ev.Target.Nickname} - {ev.Target.UserId} ({ev.Target.Role}) {DiscordIntegration.Language.For} {ev.Amount} {DiscordIntegration.Language.With} {DamageTypes.FromIndex(ev.Tool).name}.")).ConfigureAwait(false);
                }
                catch (Exception exception)
                {
                    Log.Error($"{typeof(PlayerHandler).FullName}.{nameof(OnHurting)} error: {exception}");
                }
            }
        }

        public async void OnDying(DyingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerDying)
            {
                try
                {
                    if (ev.Killer != null && ev.Target.Role.GetTeam() == ev.Killer.Role.GetTeam())
                        await Network.SendAsync(new RemoteCommand("log", "gameEvents", $":o: **{ev.Killer.Nickname} - {ev.Killer.UserId} ({ev.Killer.Role}) {DiscordIntegration.Language.Killed} {ev.Target.Nickname} - {ev.Target.UserId} ({ev.Target.Role}) {DiscordIntegration.Language.With} {DamageTypes.FromIndex(ev.HitInformation.Tool).name}.**")).ConfigureAwait(false);
                    else if (!Instance.Config.ShouldLogFriendlyFireOnly)
                        await Network.SendAsync(new RemoteCommand("log", "gameEvents", $":skull_crossbones: **{ev.Killer.Nickname} - {ev.Killer.UserId} ({ev.Killer.Role}) {DiscordIntegration.Language.Killed} {ev.Target.Nickname} - {ev.Target.UserId} ({ev.Target.Role}) {DiscordIntegration.Language.With} {DamageTypes.FromIndex(ev.HitInformation.Tool).name}.**")).ConfigureAwait(false);
                }
                catch (Exception exception)
                {
                    Log.Error($"{typeof(PlayerHandler).FullName}.{nameof(OnDying)} error: {exception}");
                }
            }
        }

        public async void OnThrowingGrenade(ThrowingGrenadeEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerThrowingGrenade)
            {
                if (ev.Player == null)
                    return;

                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $":bomb: {ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Role}) {DiscordIntegration.Language.ThrewAGrenade}.")).ConfigureAwait(false);
            }
        }

        public async void OnUsedMedicalItem(UsedMedicalItemEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerUsedMedicalItem)
            {
                if (ev.Player == null)
                    return;

                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $":medical_symbol: {ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Role}) {DiscordIntegration.Language.UsedA} {ev.Item}")).ConfigureAwait(false);
            }
        }

        public async void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ChangingPlayerRole)
            {
                if (ev.Player == null)
                    return;

                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $":mens: {ev.Player.Nickname} - {ev.Player.UserId} {DiscordIntegration.Language.HasBenChangedToA} {ev.NewRole}.")).ConfigureAwait(false);
            }
        }

        public async void OnVerified(VerifiedEventArgs ev)
        {
            if (Instance.Config.ShouldSyncRoles)
            {
                SyncedUser syncedUser = Instance.SyncedUsersCache.Where(tempSyncedUser => tempSyncedUser?.Id == ev.Player.UserId).FirstOrDefault();

                if (syncedUser == null)
                {
                    await Network.SendAsync(new RemoteCommand("getGroupFromId", ev.Player.UserId)).ConfigureAwait(false);
                    return;
                }

                syncedUser?.SetGroup();
            }

            if (Instance.Config.EventsToLog.PlayerJoined && !ev.Player.IsHost)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $":arrow_right: **{ev.Player.Nickname} - {ev.Player.UserId} {(!ev.Player.DoNotTrack && Instance.Config.ShouldLogIPAddresses ? $"({ev.Player.IPAddress})" : string.Empty)} {DiscordIntegration.Language.HasJoinedTheGame}.**")).ConfigureAwait(false);
        }

        public async void OnRemovingHandcuffs(RemovingHandcuffsEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerRemovingHandcuffs)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $":unlock: {ev.Target.Nickname} - {ev.Target.UserId} ({ev.Target.Role}) {DiscordIntegration.Language.HasBeenFreedBy} {ev.Cuffer.Nickname} - {ev.Cuffer.UserId} ({ev.Cuffer.Role})")).ConfigureAwait(false);
        }

        public async void OnHandcuffing(HandcuffingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.HandcuffingPlayer)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $":lock: {ev.Target.Nickname} - {ev.Target.UserId} ({ev.Target.Role}) {DiscordIntegration.Language.HasBeenHandcuffedBy} {ev.Cuffer.Nickname} - {ev.Cuffer.UserId} ({ev.Cuffer.Role})")).ConfigureAwait(false);
        }

        public async void OnKicked(KickedEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerBanned)
                await Network.SendAsync(new RemoteCommand("log", "kicks", $":no_entry: {ev.Target.Nickname} - {ev.Target.UserId} {DiscordIntegration.Language.WasKicked} {DiscordIntegration.Language.For} {ev.Reason}.")).ConfigureAwait(false);
        }

        public async void OnBanned(BannedEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerBanned)
                await Network.SendAsync(new RemoteCommand("log", "bans", $":no_entry: {ev.Details.OriginalName} - {ev.Details.Id} {DiscordIntegration.Language.WasBannedBy} {ev.Details.Issuer} {DiscordIntegration.Language.For} {ev.Details.Reason}. {new DateTime(ev.Details.Expires)}")).ConfigureAwait(false);
        }

        public async void OnIntercomSpeaking(IntercomSpeakingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerIntercomSpeaking)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $":loud_sound: {ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Role}) {DiscordIntegration.Language.HasStartedUsingTheIntercom}.")).ConfigureAwait(false);
        }

        public async void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerPickingupItem)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Role}) {DiscordIntegration.Language.HasPickedUp} {ev.Pickup.ItemId}.")).ConfigureAwait(false);
        }

        public async void OnItemDropped(ItemDroppedEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerItemDropped)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Role}) {DiscordIntegration.Language.HasDropped} {ev.Pickup.ItemId}.")).ConfigureAwait(false);
        }

        public async void OnChangingGroup(ChangingGroupEventArgs ev)
        {
            try
            {
                if (Instance.Config.EventsToLog.ChangingPlayerGroup)
                {
                    string groupMessage = ev.NewGroup == null ? DiscordIntegration.Language.None : $"{ev.NewGroup.BadgeText} ({ev.NewGroup.BadgeColor})";

                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{ev.Player.Nickname} - {ev.Player.UserId} {DiscordIntegration.Language.GroupSet}: **{groupMessage}**.")).ConfigureAwait(false);
                }
            }
            catch (Exception exception)
            {
                Log.Error($"{typeof(PlayerHandler).FullName}.{nameof(OnChangingGroup)} error: {exception}");
            }
        }
    }
}