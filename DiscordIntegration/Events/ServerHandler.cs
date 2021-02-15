// -----------------------------------------------------------------------
// <copyright file="ServerHandler.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.Events
{
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using Features.Commands;
    using Respawning;
    using static DiscordIntegration;

    /// <summary>
    /// Handles server-related events.
    /// </summary>
    internal sealed class ServerHandler
    {
#pragma warning disable SA1600 // Elements should be documented
        public async void OnSendingRemoteAdminCommand(SendingRemoteAdminCommandEventArgs ev)
        {
            if (Instance.Config.EventsToLog.SendingRemoteAdminCommands)
                await Network.SendAsync(new RemoteCommand("log", "commands", $":keyboard: {ev.Sender.Nickname}({ev.Sender.UserId}) {Language.UsedCommand}: {ev.Name} {string.Join(" ", ev.Arguments)}")).ConfigureAwait(false);
        }

        public async void OnSendingConsoleCommand(SendingConsoleCommandEventArgs ev)
        {
            if (Instance.Config.EventsToLog.SendingConsoleCommands)
                await Network.SendAsync(new RemoteCommand("log", "commands", $"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Role}) {Language.HasRunClientConsoleCommand}: {ev.Name} {string.Join(" ", ev.Arguments)}")).ConfigureAwait(false);
        }

        public async void OnReportingCheater(ReportingCheaterEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ReportingCheater)
                await Network.SendAsync(new RemoteCommand("log", "commands", $"**{Language.CheaterReportFilled}: {ev.Reporter.UserId} {Language.Reported} {ev.Reported.UserId} {Language.For} {ev.Reason}.**")).ConfigureAwait(false);
        }

        public async void OnWaitingForPlayers()
        {
            if (Instance.Config.EventsToLog.WaitingForPlayers)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $":hourglass: {Language.WaitingForPlayers}")).ConfigureAwait(false);
        }

        public async void OnRoundStarted()
        {
            if (Instance.Config.EventsToLog.RoundStarted)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $":arrow_forward: {Language.RoundStarting}: {Player.Dictionary.Count} {Language.PlayersInRound}.")).ConfigureAwait(false);
        }

        public async void OnRoundEnded(RoundEndedEventArgs ev)
        {
            if (Instance.Config.EventsToLog.RoundEnded)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $":stop_button: {Language.RoundEnded}: {ev.LeadingTeam} - {string.Format(Language.PlayersOnline, Player.Dictionary.Count, Instance.Slots)}.")).ConfigureAwait(false);
        }

        public async void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            if (Instance.Config.EventsToLog.RespawningTeam)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", $"{(ev.NextKnownTeam == SpawnableTeamType.ChaosInsurgency ? $":spy: {Language.ChaosInsurgency}" : $":cop: {Language.NineTailedFox}")} {Language.HasSpawnedWith} {ev.Players.Count} {Language.Players}.")).ConfigureAwait(false);
        }
    }
}