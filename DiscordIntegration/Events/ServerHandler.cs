// -----------------------------------------------------------------------
// <copyright file="ServerHandler.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.Events
{
    using API.Commands;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
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
                await Network.SendAsync(new RemoteCommand("log", "commands", string.Format(Language.UsedCommand, ev.CommandSender.Nickname, ev.CommandSender.SenderId ?? Language.DedicatedServer, ev.Sender.Role.Translate(), ev.Name, string.Join(" ", ev.Arguments)))).ConfigureAwait(false);
        }

        public async void OnSendingConsoleCommand(SendingConsoleCommandEventArgs ev)
        {
            if (Instance.Config.EventsToLog.SendingConsoleCommands)
                await Network.SendAsync(new RemoteCommand("log", "commands", string.Format(Language.HasRunClientConsoleCommand, ev.Player.Nickname, ev.Player.UserId ?? Language.DedicatedServer, ev.Player.Role.Translate(), ev.Name, string.Join(" ", ev.Arguments)))).ConfigureAwait(false);
        }

        public async void OnReportingCheater(ReportingCheaterEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ReportingCheater)
                await Network.SendAsync(new RemoteCommand("log", "commands", string.Format(Language.CheaterReportFilled, ev.Reporter.Nickname, ev.Reporter.Id.ToString(), ev.Reporter.Role.Translate(), ev.Reported.Nickname, ev.Reported.Id.ToString(), ev.Reported.Role.Translate(), ev.Reason))).ConfigureAwait(false);
        }

        public async void OnWaitingForPlayers()
        {
            if (Instance.Config.EventsToLog.WaitingForPlayers)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", Language.WaitingForPlayers)).ConfigureAwait(false);
        }

        public async void OnRoundStarted()
        {
            if (Instance.Config.EventsToLog.RoundStarted)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.RoundStarting, Player.Dictionary.Count))).ConfigureAwait(false);
        }

        public async void OnRoundEnded(RoundEndedEventArgs ev)
        {
            if (Instance.Config.EventsToLog.RoundEnded)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(Language.RoundEnded, ev.LeadingTeam, Player.Dictionary.Count, Instance.Slots))).ConfigureAwait(false);
        }

        public async void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            if (Instance.Config.EventsToLog.RespawningTeam)
                await Network.SendAsync(new RemoteCommand("log", "gameEvents", string.Format(ev.NextKnownTeam == SpawnableTeamType.ChaosInsurgency ? Language.ChaosInsurgencyHaveSpawned : Language.NineTailedFoxHaveSpawned, ev.Players.Count))).ConfigureAwait(false);
        }
    }
}