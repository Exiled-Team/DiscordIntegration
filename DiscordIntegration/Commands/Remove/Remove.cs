// -----------------------------------------------------------------------
// <copyright file="Remove.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.Commands.Remove
{
#pragma warning disable SA1600 // Elements should be documented
    using System;
    using CommandSystem;
    using static DiscordIntegration;

    internal sealed class Remove : ParentCommand
    {
        private Remove() => LoadGeneratedCommands();

        public static Remove Instance { get; } = new Remove();

        public override string Command { get; } = "remove";

        public override string[] Aliases { get; } = new[] { "r" };

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
