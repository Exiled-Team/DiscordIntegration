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
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.GeneratorFinished, ev.Generator.CurRoom, Generator079.mainGenerator.totalVoltage + 1))).ConfigureAwait(false);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = "Discard operator")]
        public async void OnDecontaminating(DecontaminatingEventArgs _)
        {
            if (Instance.Config.EventsToLog.Decontaminating)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", Language.DecontaminationHasBegun)).ConfigureAwait(false);
        }

        public async void OnStartingWarhead(StartingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.StartingWarhead && (ev.Player == null || (ev.Player != null && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))))
            {
                object[] vars = ev.Player == null ?
                    new object[] { ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role, Warhead.DetonationTimer } :
                    new object[] { Warhead.DetonationTimer };

                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(ev.Player == null ? Language.WarheadStarted : Language.PlayerWarheadStarted, vars))).ConfigureAwait(false);
            }
        }

        public async void OnStoppingWarhead(StoppingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.StoppingWarhead && (ev.Player == null || (ev.Player != null && (!ev.Player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack))))
            {
                object[] vars = ev.Player == null ?
                    new object[] { ev.Player.Nickname, Instance.Config.ShouldLogUserIds ? ev.Player.UserId : Language.Redacted, ev.Player.Role } :
                    Array.Empty<object>();

                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(ev.Player == null ? Language.CanceledWarhead : Language.PlayerCanceledWarhead, vars))).ConfigureAwait(false);
            }
        }

        public async void OnUpgradingItems(UpgradingItemsEventArgs ev)
        {
            if (Instance.Config.EventsToLog.UpgradingScp914Items)
            {
                StringBuilder players = StringBuilderPool.Shared.Rent();
                StringBuilder items = StringBuilderPool.Shared.Rent();

                foreach (Player player in ev.Players)
                {
                    if (!player.DoNotTrack || !Instance.Config.ShouldRespectDoNotTrack)
                        players.Append(player.Nickname).Append(" (").Append(Instance.Config.ShouldLogUserIds ? player.UserId : Language.Redacted).Append(") [").Append(player.Role).Append(']').AppendLine();
                }

                foreach (Pickup item in ev.Items)
                    items.Append(item.ItemId.ToString()).AppendLine();

                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.Scp914HasProcessedTheFollowingPlayers, players, items)));

                StringBuilderPool.Shared.Return(players);
                StringBuilderPool.Shared.Return(items);
            }
        }
    }
}