// -----------------------------------------------------------------------
// <copyright file="MapHandler.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.Events
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using API.Commands;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using NorthwoodLib.Pools;
    using static DiscordIntegration;

    /// <summary>
    /// Handles map-related events.
    /// </summary>
    internal sealed class MapHandler
    {
#pragma warning disable SA1600 // Elements should be documented
        public async void OnWarheadDetonated()
        {
            if (Instance.Config.EventsToLog.WarheadDetonated)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", Language.WarheadHasDetonated)).ConfigureAwait(false);
        }

        public async void OnGeneratorActivated(GeneratorActivatedEventArgs ev)
        {
            if (Instance.Config.EventsToLog.GeneratorActivated)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GeneratorFinished, ev.Generator.GetComponentInParent<Room>(), Map.ActivatedGenerators + 1))).ConfigureAwait(false);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = "Discard operator")]
        public async void OnDecontaminating(DecontaminatingEventArgs _)
        {
            if (Instance.Config.EventsToLog.Decontaminating)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", Language.DecontaminationHasBegun)).ConfigureAwait(false);
        }

        public async void OnStartingWarhead(StartingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.StartingWarhead)
            {
                object[] vars = ev.Player == null ?
                    new object[] { Warhead.DetonationTimer } :
                    new object[] { ev.Player.Nickname, ev.Player.Id, ev.Player.Role.Translate(), Warhead.DetonationTimer };

                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(ev.Player == null ? Language.WarheadStarted : Language.PlayerWarheadStarted, vars))).ConfigureAwait(false);
            }
        }

        public async void OnStoppingWarhead(StoppingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.StoppingWarhead)
            {
                object[] vars = ev.Player == null ?
                    Array.Empty<object>() :
                    new object[] { ev.Player.Nickname, ev.Player.Id, ev.Player.Role.Translate() };

                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(ev.Player == null ? Language.CanceledWarhead : Language.PlayerCanceledWarhead, vars))).ConfigureAwait(false);
            }

            if (Instance.Config.StaffOnlyEventsToLog.StoppingWarhead)
            {
                object[] vars = ev.Player == null
                    ? Array.Empty<object>()
                    : new object[] { ev.Player.Nickname, ev.Player.UserId, ev.Player.Role.Translate() };

                await Network.SendAsync(new RemoteCommand("log", "staffCopy", string.Format(ev.Player == null ? Language.CanceledWarhead : Language.PlayerCanceledWarhead, vars))).ConfigureAwait(false);
            }
        }

        public async void OnUpgradingItems(UpgradingItemEventArgs ev)
        {
            if (Instance.Config.EventsToLog.UpgradingScp914Items)
            {
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.Scp914ProcessedItem, ev.Item.Type)));
            }
        }
    }
}
