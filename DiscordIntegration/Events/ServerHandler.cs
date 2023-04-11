// -----------------------------------------------------------------------
// <copyright file="ServerHandler.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;

namespace DiscordIntegration.Events
{
    using System.Diagnostics;

    using Dependency;
    using Exiled.API.Features;

    using global::DiscordIntegration.Patches;

    using Respawning;
    using static DiscordIntegration;

    /// <summary>
    /// Handles server-related events.
    /// </summary>
    internal sealed class ServerHandler
    {
        private Process? process;

        public Process Process => process ??= Process.GetCurrentProcess();
#pragma warning disable SA1600 // Elements should be documented

        public async void OnReportingCheater(ReportingCheaterEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ReportingCheater)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.Reports, string.Format(Language.CheaterReportFilled, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, ev.Target.Nickname, ev.Target.UserId, ev.Target.Role, ev.Reason))).ConfigureAwait(false);
        }

        public async void OnLocalReporting(LocalReportingEventArgs ev)
        {
            if (Instance.Config.EventsToLog.ReportingCheater)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.Reports, string.Format(Language.CheaterReportFilled, ev.Player.Nickname, ev.Player.UserId, ev.Player.Role, ev.Target.Nickname, ev.Target.UserId, ev.Target.Role, ev.Reason))).ConfigureAwait(false);
        }

        public async void OnWaitingForPlayers()
        {
            if (Instance.Config.EventsToLog.WaitingForPlayers)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, Language.WaitingForPlayers)).ConfigureAwait(false);
            if (Instance.Config.StaffOnlyEventsToLog.WaitingForPlayers)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, Language.WaitingForPlayers)).ConfigureAwait(false);

            Process.BeginErrorReadLine();
            Process.EnableRaisingEvents = true;
            Process.ErrorDataReceived += ErrorLoggingPatch.LogError;
        }

        public async void OnRoundStarted()
        {
            if (Instance.Config.EventsToLog.RoundStarted)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.RoundStarting, Player.Dictionary.Count))).ConfigureAwait(false);
        }

        public async void OnRoundEnded(RoundEndedEventArgs ev)
        {
            if (Instance.Config.EventsToLog.RoundEnded)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(Language.RoundEnded, ev.LeadingTeam, Player.Dictionary.Count, Instance.Slots))).ConfigureAwait(false);
            Process.ErrorDataReceived -= ErrorLoggingPatch.LogError;
        }

        public async void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            if (Instance.Config.EventsToLog.RespawningTeam)
                await Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.GameEvents, string.Format(ev.NextKnownTeam == SpawnableTeamType.ChaosInsurgency ? Language.ChaosInsurgencyHaveSpawned : Language.NineTailedFoxHaveSpawned, ev.Players.Count))).ConfigureAwait(false);
        }
    }
}