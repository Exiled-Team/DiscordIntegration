// -----------------------------------------------------------------------
// <copyright file="ClientCommandLogging.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.Patches
{
#pragma warning disable SA1118

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Emit;

    using Exiled.API.Features;

    using global::DiscordIntegration.Dependency;

    using HarmonyLib;

    using NorthwoodLib.Pools;

    using RemoteAdmin;

    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="RemoteAdmin.QueryProcessor.ProcessGameConsoleQuery"/> to add client command logging.
    /// </summary>
    [HarmonyPatch(typeof(QueryProcessor), nameof(QueryProcessor.ProcessGameConsoleQuery))]
    internal class ClientCommandLogging
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instruction)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instruction);
            const int index = 0;

            newInstructions.InsertRange(index, new CodeInstruction[]
            {
                new (OpCodes.Ldarg_1),
                new (OpCodes.Ldarg_0),
                new (OpCodes.Ldfld, Field(typeof(QueryProcessor), nameof(QueryProcessor._sender))),
                new (OpCodes.Call, Method(typeof(ClientCommandLogging), nameof(LogCommand))),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }

        private static void LogCommand(string query, CommandSender sender)
        {
            if (!DiscordIntegration.Instance.Config.EventsToLog.SendingConsoleCommands && !DiscordIntegration.Instance.Config.StaffOnlyEventsToLog.SendingConsoleCommands)
                return;

            string[] args = query.Trim().Split(QueryProcessor.SpaceArray, 512, StringSplitOptions.RemoveEmptyEntries);
            if (args[0].StartsWith("$"))
                return;

            Player player = sender is PlayerCommandSender playerCommandSender
                ? Player.Get(playerCommandSender)
                : Server.Host;
            if (player == null || (!string.IsNullOrEmpty(player.UserId) && DiscordIntegration.Instance.Config.TrustedAdmins.Contains(player.UserId)))
                return;
            if (DiscordIntegration.Instance.Config.EventsToLog.SendingConsoleCommands)
                _ = DiscordIntegration.Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.Command, string.Format(DiscordIntegration.Language.UsedCommand, sender.Nickname, sender.SenderId ?? DiscordIntegration.Language.DedicatedServer, player.Role, args[0], string.Join(" ", args.Where(a => a != args[0])))));
            if (DiscordIntegration.Instance.Config.StaffOnlyEventsToLog.SendingConsoleCommands)
                _ = DiscordIntegration.Network.SendAsync(new RemoteCommand(ActionType.Log, ChannelType.StaffCopy, string.Format(DiscordIntegration.Language.UsedCommand, sender.Nickname, sender.SenderId ?? DiscordIntegration.Language.DedicatedServer, player.Role, args[0], string.Join(" ", args.Where(a => a != args[0])))));
        }
    }
}