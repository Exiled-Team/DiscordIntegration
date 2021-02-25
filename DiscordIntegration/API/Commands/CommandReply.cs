// -----------------------------------------------------------------------
// <copyright file="CommandReply.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.API.Commands
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a command reply.
    /// </summary>
    public class CommandReply
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandReply"/> class.
        /// </summary>
        /// <param name="sender"><inheritdoc cref="Sender"/></param>
        /// <param name="response"><inheritdoc cref="Response"/></param>
        /// <param name="isSucceeded"><inheritdoc cref="IsSucceeded"/></param>
        [JsonConstructor]
        public CommandReply(CommandSender sender, string response, bool isSucceeded)
        {
            Sender = sender;
            Response = response;
            IsSucceeded = isSucceeded;
        }

        /// <summary>
        /// Gets the command sender.
        /// </summary>
        public CommandSender Sender { get; }

        /// <summary>
        /// Gets the command response.
        /// </summary>
        public string Response { get; }

        /// <summary>
        /// Gets a value indicating whether the command has succeeded or not.
        /// </summary>
        public bool IsSucceeded { get; }

        /// <summary>
        /// Answers to the command.
        /// </summary>
        public void Answer() => Sender?.RaReply(Response, IsSucceeded, true, string.Empty);
    }
}
