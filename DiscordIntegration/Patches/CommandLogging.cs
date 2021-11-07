// -----------------------------------------------------------------------
// <copyright file="CommandLogging.cs" company="Exiled Team">
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
    using System.Threading.Tasks;
    using Exiled.API.Features;
    using global::DiscordIntegration.API;
    using global::DiscordIntegration.API.Commands;
    using HarmonyLib;
    using NorthwoodLib.Pools;
    using RemoteAdmin;

    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="RemoteAdmin.CommandProcessor.ProcessQuery"/> for command logging.
    /// </summary>
    [HarmonyPatch(typeof(CommandProcessor), nameof(CommandProcessor.ProcessQuery))]
    internal class CommandLogging
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);
            const int index = 0;

            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Call, Method(typeof(CommandLogging), nameof(LogCommand))),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }

        private static void LogCommand(string query, CommandSender sender)
        {
            string[] args = query.Trim().Split(QueryProcessor.SpaceArray, 512, StringSplitOptions.RemoveEmptyEntries);
            if (args[0].ToUpperInvariant() == "REQUEST_DATA")
                return;

            Player player = sender is global::RemoteAdmin.PlayerCommandSender playerCommandSender
                ? Player.Get(playerCommandSender)
                : Server.Host;
#pragma warning disable SA1312
            if (player != null && !string.IsNullOrEmpty(player.UserId) && DiscordIntegration.Instance.Config.TrustedAdmins.Contains(player.UserId))
                return;
            ValueTask _ = DiscordIntegration.Network.SendAsync(new RemoteCommand("log", "commands", string.Format(DiscordIntegration.Language.UsedCommand, sender.Nickname, sender.SenderId ?? DiscordIntegration.Language.DedicatedServer, player.Role, args[0], string.Join(" ", args.Where(a => a != args[0])))));
#pragma warning restore
        }
    }
}