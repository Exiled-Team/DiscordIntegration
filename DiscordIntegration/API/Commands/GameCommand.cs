// -----------------------------------------------------------------------
// <copyright file="GameCommand.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.API.Commands
{
    using API.User;
    using Newtonsoft.Json;
    using RemoteAdmin;

    /// <summary>
    /// Represents a command sent by a Discord user from the linked server.
    /// </summary>
    public class GameCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameCommand"/> class.
        /// </summary>
        /// <param name="channelId"><inheritdoc cref="ChannelId"/></param>
        /// <param name="content"><inheritdoc cref="Content"/></param>
        /// <param name="user"><inheritdoc cref="User"/></param>
        [JsonConstructor]
        public GameCommand(string channelId, string content, DiscordUser user)
        {
            ChannelId = channelId;
            Content = content;
            User = user;
            Sender = new BotCommandSender(channelId, user?.Id, user?.Name, user?.Command);
        }

        /// <summary>
        /// Gets the Discord channel ID.
        /// </summary>
        public string ChannelId { get; }

        /// <summary>
        /// Gets the command content.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Gets the Discord user which executed the command.
        /// </summary>
        public DiscordUser User { get; }

        /// <summary>
        /// Gets the command sender.
        /// </summary>
        [JsonIgnore]
        public CommandSender Sender { get; }

        /// <summary>
        /// Executes the command.
        /// </summary>
        public void Execute() => CommandProcessor.ProcessQuery(Content, Sender);
    }
}
