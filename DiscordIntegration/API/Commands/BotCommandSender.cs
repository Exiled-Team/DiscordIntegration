// -----------------------------------------------------------------------
// <copyright file="BotCommandSender.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.API.Commands
{
    using Newtonsoft.Json;
    using static DiscordIntegration;

    /// <summary>
    /// Represents the bot command sender.
    /// </summary>
    public class BotCommandSender : CommandSender
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BotCommandSender"/> class.
        /// </summary>
        /// <param name="channelId"><inheritdoc cref="ChannelId"/></param>
        /// <param name="senderId"><inheritdoc cref="SenderId"/></param>
        /// <param name="nickname"><inheritdoc cref="Nickname"/></param>
        [JsonConstructor]
        public BotCommandSender(string channelId, string senderId, string nickname)
        {
            ChannelId = channelId;
            SenderId = senderId;
            Nickname = nickname;
        }

        /// <summary>
        /// Gets the Discord channel ID to log the reply in.
        /// </summary>
        public string ChannelId { get; }

        /// <inheritdoc cref="CommandSender.SenderId"/>
        public override string SenderId { get; }

        /// <inheritdoc cref="CommandSender.Nickname"/>
        public override string Nickname { get; }

        /// <inheritdoc cref="CommandSender.Permissions"/>
        public override ulong Permissions { get; } = ServerStatic.GetPermissionsHandler().FullPerm;

        /// <inheritdoc cref="CommandSender.KickPower"/>
        public override byte KickPower { get; } = byte.MaxValue;

        /// <inheritdoc cref="CommandSender.FullPermissions"/>
        public override bool FullPermissions { get; } = true;

        /// <inheritdoc cref="CommandSender.RaReply"/>
        public override async void RaReply(string text, bool success, bool logToConsole, string overrideDisplay)
        {
            await Network.SendAsync(new RemoteCommand("sendMessage", ChannelId, text.Substring(text.IndexOf('#') + 1), true));
        }

        /// <inheritdoc cref="CommandSender.Print"/>
        public override async void Print(string text) => await Network.SendAsync(new RemoteCommand("sendMessage", ChannelId, text.Substring(text.IndexOf('#') + 1), true));
    }
}