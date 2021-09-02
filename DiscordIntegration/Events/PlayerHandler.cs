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
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
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
            if (Instance.Config.EventsToLog.PlayerInsertingGeneratorTablet)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GeneratorInserted, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GeneratorInserted, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.PlayerInsertingGeneratorTablet)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.GeneratorInserted, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnOpeningGenerator(OpeningGeneratorEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerOpeningGenerator)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GeneratorOpened, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GeneratorOpened, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.PlayerOpeningGenerator)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.GeneratorOpened, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnUnlockingGenerator(UnlockingGeneratorEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerUnlockingGenerator)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GeneratorUnlocked, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GeneratorUnlocked, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.PlayerUnlockingGenerator)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.GeneratorUnlocked, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnContaining(ContainingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ContainingScp106)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.Scp106WasContained, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.Scp106WasContained, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.ContainingScp106)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.Scp106WasContained, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnCreatingPortal(CreatingPortalEventArgs ev)
        {
            if (Instance.Config.EventsToLog.CreatingScp106Portal)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.Scp106CreatedPortal, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.Scp106CreatedPortal, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.CreatingScp106Portal)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.Scp106CreatedPortal, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnChangingItem(ChangingItemEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ChangingPlayerItem)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.ItemChanged, ev.Player.Nickname, Language.Redacted, ev.Player.CurrentItem.Type, ev.NewItem.Type))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.ItemChanged, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.CurrentItem.Type, ev.NewItem.Type))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.ChangingPlayerItem)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.ItemChanged, ev.Player.Nickname, ev.Player.UserId, ev.Player.CurrentItem.Type, ev.NewItem.Type))).ConfigureAwait(false);
        }

        public async void OnGainingExperience(GainingExperienceEventArgs ev)
        {
            if (Instance.Config.EventsToLog.GainingScp079Experience)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GainedExperience, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate(), ev.Amount, ev.GainType))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GainedExperience, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate(), ev.Amount, ev.GainType))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.GainingScp079Experience)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.GainedExperience, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate(), ev.Amount, ev.GainType))).ConfigureAwait(false);
        }

        public async void OnGainingLevel(GainingLevelEventArgs ev)
        {
            if (Instance.Config.EventsToLog.GainingScp079Level)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GainedLevel, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate(), ev.NewLevel - 1, ev.NewLevel))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GainedLevel, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate(), ev.NewLevel - 1, ev.NewLevel))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.GainingScp079Level)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.GainedLevel, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate(), ev.NewLevel - 1, ev.NewLevel))).ConfigureAwait(false);
        }

        public async void OnDestroying(DestroyingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerLeft)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.LeftServer, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.LeftServer, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.PlayerLeft)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.LeftServer, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnReloadingWeapon(ReloadingWeaponEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ReloadingPlayerWeapon)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.Reloaded, ev.Player.Nickname, Language.Redacted, ev.Player.CurrentItem.Type, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.Reloaded, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.CurrentItem.Type, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.ReloadingPlayerWeapon)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.Reloaded, ev.Player.Nickname, ev.Player.UserId, ev.Player.CurrentItem.Type, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerActivatingWarheadPanel)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.AccessedWarhead, ev.Player.Nickname, Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.AccessedWarhead, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.PlayerActivatingWarheadPanel)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.AccessedWarhead, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role))).ConfigureAwait(false);
        }

        public async void OnInteractingElevator(InteractingElevatorEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerInteractingElevator)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.CalledElevator, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.CalledElevator, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.PlayerInteractingElevator)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.CalledElevator, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnInteractingLocker(InteractingLockerEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerInteractingLocker)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.UsedLocker, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.UsedLocker, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.PlayerInteractingLocker)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.UsedLocker, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerTriggeringTesla)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasTriggeredATeslaGate, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasTriggeredATeslaGate, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.PlayerTriggeringTesla)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.HasTriggeredATeslaGate, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnClosingGenerator(ClosingGeneratorEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerClosingGenerator)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GeneratorClosed, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GeneratorClosed, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.PlayerClosingGenerator)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.GeneratorClosed, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnEjectingGeneratorTablet(StoppingGeneratorEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerEjectingGeneratorTablet)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GeneratorEjected, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GeneratorEjected, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.PlayerEjectingGeneratorTablet)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.GeneratorEjected, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerInteractingDoor)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(ev.Door.IsOpen ? Language.HasClosedADoor : Language.HasOpenedADoor, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate(), ev.Door.Nametag))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(ev.Door.IsOpen ? Language.HasClosedADoor : Language.HasOpenedADoor, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate(), ev.Door.Nametag))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.PlayerInteractingDoor)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(ev.Door.IsOpen ? Language.HasClosedADoor : Language.HasOpenedADoor, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate(), ev.Door.Nametag))).ConfigureAwait(false);
        }

        public async void OnActivatingScp914(ActivatingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ActivatingScp914)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.Scp914HasBeenActivated, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate(), Scp914.KnobStatus))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.Scp914HasBeenActivated, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate(), Scp914.KnobStatus))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.ActivatingScp914)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.Scp914HasBeenActivated, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate(), Scp914.KnobStatus))).ConfigureAwait(false);
        }

        public async void OnChangingScp914KnobSetting(ChangingKnobSettingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ChangingScp914KnobSetting)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.Scp914KnobSettingChanged, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate(), ev.KnobSetting))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.Scp914KnobSettingChanged, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate(), ev.KnobSetting))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.ChangingScp914KnobSetting)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.Scp914KnobSettingChanged, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate(), ev.KnobSetting))).ConfigureAwait(false);
        }

        public async void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerEnteringPocketDimension)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasEnteredPocketDimension, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasEnteredPocketDimension, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.PlayerEnteringPocketDimension)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.HasEnteredPocketDimension, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnEscapingPocketDimension(EscapingPocketDimensionEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerEscapingPocketDimension)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasEscapedPocketDimension, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasEscapedPocketDimension, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.PlayerEscapingPocketDimension)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.HasEscapedPocketDimension, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnTeleporting(TeleportingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.Scp106Teleporting)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.Scp106Teleported, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.Scp106Teleported, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.Scp106Teleporting)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.Scp106Teleported, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnInteractingTesla(InteractingTeslaEventArgs ev)
        {
            if (Instance.Config.EventsToLog.Scp079InteractingTesla)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasTriggeredATeslaGate, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasTriggeredATeslaGate, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.Scp079InteractingTesla)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.HasTriggeredATeslaGate, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnHurting(HurtingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.HurtingPlayer &&
                ev.Attacker != null &&
                ev.Target != null &&
                (!Instance.Config.ShouldLogFriendlyFireDamageOnly || (Instance.Config.ShouldLogFriendlyFireDamageOnly && ev.Attacker.Side == ev.Target.Side && ev.Attacker != ev.Target)) &&
                (ev.DamageType != DamageTypes.Scp207 || (ev.DamageType == DamageTypes.Scp207 && Instance.Config.ShouldLogScp207Damage)))
            {
                if ((ev.Target.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack) || (ev.Attacker.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack))
                {
                    // Instance.Config.ShouldLogUserIds ? ev.Attacker.UserId : Language.Redacted,
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasDamagedForWith, ev.Attacker.Nickname, ev.Attacker.DoNotTrack ? Language.Redacted : ev.Attacker.UserId , ev.Attacker.Role.Translate(), ev.Target.Nickname, ev.Target.DoNotTrack ? Language.Redacted : ev.Target.UserId, ev.Target.Role.Translate(), ev.Amount, ev.DamageType.Name))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Target.DoNotTrack || ev.Attacker.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasDamagedForWith, ev.Attacker.Nickname, Instance.Config.ShouldLogUserIds ? ev.Attacker.UserId : Language.Redacted, ev.Attacker.Role.Translate(), ev.Target.Nickname, Instance.Config.ShouldLogUserIds ? ev.Target.UserId : Language.Redacted, ev.Target.Role.Translate(), ev.Amount, ev.DamageType.Name))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.HurtingPlayer &&
                ev.Attacker != null &&
                ev.Target != null &&
                (!Instance.Config.ShouldLogFriendlyFireDamageOnly || (Instance.Config.ShouldLogFriendlyFireDamageOnly && ev.Attacker.Side == ev.Target.Side && ev.Attacker != ev.Target)) &&
                (ev.DamageType != DamageTypes.Scp207 || (ev.DamageType == DamageTypes.Scp207 && Instance.Config.ShouldLogScp207Damage)))
            {
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.HasDamagedForWith, ev.Attacker.Nickname, ev.Attacker.UserId, ev.Attacker.Role.Translate(), ev.Target.Nickname, ev.Target.UserId, ev.Target.Role.Translate(), ev.Amount, ev.DamageType.Name))).ConfigureAwait(false);
            }
        }

        public async void OnDying(DyingEventArgs ev)
        {

            if (Instance.Config.EventsToLog.PlayerDying &&
                ev.Killer != null &&
                ev.Target != null &&
                (!Instance.Config.ShouldLogFriendlyFireKillsOnly || (Instance.Config.ShouldLogFriendlyFireKillsOnly && ev.Killer.Side == ev.Target.Side && ev.Killer != ev.Target) || ev.Target.IsCuffed))
            {
                if ((ev.Killer.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack) || (ev.Target.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack))
                {
                    if (ev.Target.IsCuffed)
                    {
                        await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasKillCuffed, ev.Killer.Nickname, ev.Killer.DoNotTrack ? Language.Redacted : ev.Killer.UserId, ev.Killer.Role.Translate(), ev.Target.Nickname, ev.Target.DoNotTrack ? Language.Redacted : ev.Target.UserId, ev.Target.Role.Translate(), ev.HitInformation.Tool.Name))).ConfigureAwait(false);
                    }
                    else
                    {
                        await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasKilledWith, ev.Killer.Nickname, ev.Killer.DoNotTrack ? Language.Redacted : ev.Killer.UserId, ev.Killer.Role.Translate(), ev.Target.Nickname, ev.Target.DoNotTrack ? Language.Redacted : ev.Target.UserId, ev.Target.Role.Translate(), ev.HitInformation.Tool.Name))).ConfigureAwait(false);
                    }
                }
                else
                {
                    if (ev.Target.DoNotTrack || ev.Killer.DoNotTrack)
                        return;

                    if (ev.Target.IsCuffed)
                    {
                        await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasKillCuffed, ev.Killer.Nickname, Instance.Config.ShouldLogUserIds ? ev.Killer.UserId : Language.Redacted, ev.Killer.Role.Translate(), ev.Target.Nickname, Instance.Config.ShouldLogUserIds ? ev.Target.UserId : Language.Redacted, ev.Target.Role.Translate(), ev.HitInformation.Tool.Name))).ConfigureAwait(false);
                    }
                    else
                    {
                        await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasKilledWith, ev.Killer.Nickname, Instance.Config.ShouldLogUserIds ? ev.Killer.UserId : Language.Redacted, ev.Killer.Role.Translate(), ev.Target.Nickname, Instance.Config.ShouldLogUserIds ? ev.Target.UserId : Language.Redacted, ev.Target.Role.Translate(), ev.HitInformation.Tool.Name))).ConfigureAwait(false);
                    }
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.PlayerDying &&
                ev.Killer != null &&
                ev.Target != null &&
                (!Instance.Config.ShouldLogFriendlyFireKillsOnly || (Instance.Config.ShouldLogFriendlyFireKillsOnly && ev.Killer.Side == ev.Target.Side && ev.Killer != ev.Target)))
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.HasKilledWith, ev.Killer.Nickname, ev.Killer.UserId, ev.Killer.Role.Translate(), ev.Target.Nickname, ev.Target.UserId, ev.Target.Role.Translate(), ev.HitInformation.Tool.Name))).ConfigureAwait(false);
        }

        public async void OnThrowingGrenade(ThrowingItemEventArgs ev)
        {
            if (ev.Player != null && Instance.Config.EventsToLog.PlayerThrowingGrenade)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.ThrewAGrenade, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate(), ev.Item.Type))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.ThrewAGrenade, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate(), ev.Item.Type))).ConfigureAwait(false);
                }
            }

            if (ev.Player != null && Instance.Config.StaffOnlyEventsToLog.PlayerThrowingGrenade)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.ThrewAGrenade, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate(), ev.Item.Type))).ConfigureAwait(false);
        }

        public async void OnUsedMedicalItem(UsedItemEventArgs ev)
        {
            if (ev.Player != null && Instance.Config.EventsToLog.PlayerUsedMedicalItem)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.UsedMedicalItem, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate(), ev.Item.Type))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.UsedMedicalItem, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate(), ev.Item.Type))).ConfigureAwait(false);
                }
            }

            if (ev.Player != null && Instance.Config.StaffOnlyEventsToLog.PlayerUsedMedicalItem)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.UsedMedicalItem, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate(), ev.Item.Type))).ConfigureAwait(false);
        }

        public async void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player != null && Instance.Config.EventsToLog.ChangingPlayerRole)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.ChangedRole, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate(), ev.NewRole.Translate()))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.ChangedRole, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate(), ev.NewRole.Translate()))).ConfigureAwait(false);
                }
            }
                
            if (ev.Player != null && Instance.Config.StaffOnlyEventsToLog.ChangingPlayerRole)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.ChangedRole, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate(), ev.NewRole.Translate()))).ConfigureAwait(false);
        }

        public async void OnVerified(VerifiedEventArgs ev)
        {
            if (Instance.Config.ShouldSyncRoles)
            {
                SyncedUser syncedUser = Instance.SyncedUsersCache.FirstOrDefault(tempSyncedUser => tempSyncedUser?.Id == ev.Player.UserId);

                if (syncedUser == null)
                    await Network.SendAsync(new RemoteCommand("getGroupFromId", ev.Player.UserId)).ConfigureAwait(false);
                else
                    syncedUser?.SetGroup();
            }

            if (Instance.Config.EventsToLog.PlayerJoined)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasJoinedTheGame, ev.Player.Nickname, Language.Redacted, Instance.Config.ShouldLogIPAddresses ? ev.Player.IPAddress : Language.Redacted))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasJoinedTheGame, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, Instance.Config.ShouldLogIPAddresses ? ev.Player.IPAddress : Language.Redacted))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.PlayerJoined)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.HasJoinedTheGame, ev.Player.Nickname, ev.Player.UserId, ev.Player.IPAddress))).ConfigureAwait(false);
        }

        public async void OnRemovingHandcuffs(RemovingHandcuffsEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerRemovingHandcuffs)
            {
                if ((ev.Cuffer.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack) ||
                    (ev.Target.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack))
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasBeenFreedBy, ev.Target.Nickname, ev.Target.DoNotTrack ? Language.Redacted : ev.Target.UserId, ev.Target.Role.Translate(), ev.Cuffer.Nickname, ev.Cuffer.DoNotTrack ? Language.Redacted : ev.Cuffer.UserId, ev.Cuffer.Role.Translate()))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Target.DoNotTrack || ev.Cuffer.DoNotTrack)
                        return;

                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasBeenFreedBy, ev.Target.Nickname, Instance.Config.ShouldLogUserIds ? ev.Target.UserId : Language.Redacted, ev.Target.Role.Translate(), ev.Cuffer.Nickname, Instance.Config.ShouldLogUserIds ? ev.Cuffer.UserId : Language.Redacted, ev.Cuffer.Role.Translate()))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.PlayerRemovingHandcuffs)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.HasBeenFreedBy, ev.Target.Nickname, ev.Target.UserId, ev.Target.Role.Translate(), ev.Cuffer.Nickname, ev.Cuffer.UserId, ev.Cuffer.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnHandcuffing(HandcuffingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.HandcuffingPlayer)
            {
                if ((ev.Cuffer.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack) ||
                    (ev.Target.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack))
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasBeenHandcuffedBy, ev.Target.Nickname, ev.Target.DoNotTrack ? Language.Redacted : ev.Target.UserId, ev.Target.Role.Translate(), ev.Cuffer.Nickname, ev.Cuffer.DoNotTrack ? Language.Redacted : ev.Cuffer.UserId, ev.Cuffer.Role.Translate()))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Target.DoNotTrack || ev.Cuffer.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasBeenHandcuffedBy, ev.Target.Nickname, Instance.Config.ShouldLogUserIds ? ev.Target.UserId : Language.Redacted, ev.Target.Role.Translate(), ev.Cuffer.Nickname, Instance.Config.ShouldLogUserIds ? ev.Cuffer.UserId : Language.Redacted, ev.Cuffer.Role.Translate()))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.HandcuffingPlayer)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.HasBeenHandcuffedBy, ev.Target.Nickname, ev.Target.UserId, ev.Target.Role.Translate(), ev.Cuffer.Nickname, ev.Cuffer.UserId, ev.Cuffer.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnKicked(KickedEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerBanned)
                await Network.SendAsync(new RemoteCommand("log", "kicks", string.Format(Language.WasKicked, ev.Target?.Nickname ?? Language.NotAuthenticated, ev.Target?.UserId ?? Language.NotAuthenticated, ev.Reason))).ConfigureAwait(false);
        }

        public async void OnBanned(BannedEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerBanned)
                await Network.SendAsync(new RemoteCommand("log", "bans", string.Format(Language.WasBannedBy, ev.Details.OriginalName, ev.Details.Id, ev.Details.Issuer, ev.Details.Reason, new DateTime(ev.Details.Expires).ToString(Instance.Config.DateFormat)))).ConfigureAwait(false);
        }

        public async void OnIntercomSpeaking(IntercomSpeakingEventArgs ev)
        {
            if (ev.Player != null && Instance.Config.EventsToLog.PlayerIntercomSpeaking)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasStartedUsingTheIntercom, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasStartedUsingTheIntercom, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate()))).ConfigureAwait(false);
                }
            }

            if (ev.Player != null && Instance.Config.StaffOnlyEventsToLog.PlayerIntercomSpeaking)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.HasStartedUsingTheIntercom, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate()))).ConfigureAwait(false);
        }

        public async void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerPickingupItem)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasPickedUp, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate(), ev.Pickup.Type))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasPickedUp, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate(), ev.Pickup.Type))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.PlayerPickingupItem)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.HasPickedUp, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate(), ev.Pickup.Type))).ConfigureAwait(false);
        }

        public async void OnItemDropped(DroppingItemEventArgs ev)
        {
            if (Instance.Config.EventsToLog.PlayerItemDropped)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasDropped, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate(), ev.Item.Type))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.HasDropped, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate(), ev.Item.Type))).ConfigureAwait(false);
                }
            }

            if (Instance.Config.StaffOnlyEventsToLog.PlayerItemDropped)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.HasDropped, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate(), ev.Item.Type))).ConfigureAwait(false);
        }

        public async void OnChangingGroup(ChangingGroupEventArgs ev)
        {
            if (ev.Player != null && Instance.Config.EventsToLog.ChangingPlayerGroup)
            {
                if (ev.Player.DoNotTrack && Instance.Config.ShouldRespectDoNotTrack)
                {
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GroupSet, ev.Player.Nickname, Language.Redacted, ev.Player.Role.Translate(), ev.NewGroup?.BadgeText ?? Language.None, ev.NewGroup?.BadgeColor ?? Language.None))).ConfigureAwait(false);
                }
                else
                {
                    if (ev.Player.DoNotTrack)
                        return;
                    await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GroupSet, ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role.Translate(), ev.NewGroup?.BadgeText ?? Language.None, ev.NewGroup?.BadgeColor ?? Language.None))).ConfigureAwait(false);
                }
            }
            if (Instance.Config.StaffOnlyEventsToLog.ChangingPlayerGroup)
                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(Language.GroupSet, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate(), ev.NewGroup?.BadgeText ?? Language.None, ev.NewGroup?.BadgeColor ?? Language.None))).ConfigureAwait(false);
        }
    }
}