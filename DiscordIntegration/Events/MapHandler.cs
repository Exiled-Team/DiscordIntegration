// -----------------------------------------------------------------------
// <copyright file="MapHandler.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.Events
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using Features.Commands;
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
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $":radioactive: {Language.WarheadHasDetonated}")).ConfigureAwait(false);
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
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $":biohazard: **{Language.DecontaminationHasBegun}.**")).ConfigureAwait(false);
        }

        public async void OnStartingWarhead(StartingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.StartingWarhead)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $":radioactive: **{(ev.Player == null ? Language.WarheadStarted : string.Format(Language.PlayerWarheadStarted, ev.Player.Nickname, ev.Player.UserId))} {Warhead.Controller.NetworktimeToDetonation} seconds.**")).ConfigureAwait(false);
        }

        public async void OnStoppingWarhead(StoppingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.StoppingWarhead)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"***{ev.Player.Nickname} - {ev.Player.UserId} {Language.CancelledWarhead}.***")).ConfigureAwait(false);
        }

        public async void OnUpgradingItems(UpgradingItemsEventArgs ev)
        {
            if (Instance.Config.EventsToLog.UpgradingScp914Items)
            {
                StringBuilder players = StringBuilderPool.Shared.Rent();
                StringBuilder items = StringBuilderPool.Shared.Rent();

                foreach (Player player in ev.Players)
                    players.Append(player.Nickname).Append(" - ").Append(player.UserId).Append(" (").Append(player.Role).AppendLine();

                foreach (Pickup item in ev.Items)
                    items.Append(item.ItemId.ToString()).AppendLine();

                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{Language.Scp914HasProcessedTheFollowingPlayers}: {players} {Language.AndItems}: {items}."));

                StringBuilderPool.Shared.Return(players);
                StringBuilderPool.Shared.Return(items);
            }
        }
    }
}