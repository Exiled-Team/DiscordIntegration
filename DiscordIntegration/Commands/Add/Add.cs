// -----------------------------------------------------------------------
// <copyright file="Add.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.Commands.Add
{
#pragma warning disable SA1600 // Elements should be documented
    using System;
    using CommandSystem;
    using static DiscordIntegration;

    internal sealed class Add : ParentCommand
    {
        private Add() => LoadGeneratedCommands();

        public static Add Instance { get; } = new Add();

        public override string Command { get; } = "add";

        public override string[] Aliases { get; } = new[] { "a" };

        public override string Description { get; } = string.Empty;

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(Role.Instance);
            RegisterCommand(User.Instance);
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = $"{Language.InvalidSubcommand} {Language.Available}: role, user";
            return false;
        }
    }
}
