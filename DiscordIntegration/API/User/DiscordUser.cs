// -----------------------------------------------------------------------
// <copyright file="DiscordUser.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.API.User
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a Discord user.
    /// </summary>
    public class DiscordUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiscordUser"/> class.
        /// </summary>
        /// <param name="id"><inheritdoc cref="Id"/></param>
        /// <param name="name"><inheritdoc cref="Name"/></param>
        /// <param name="command"><inheritdoc cref="Command"/></param>
        [JsonConstructor]
        public DiscordUser(string id, string name, string command)
        {
            Id = id;
            Name = name;
            Command = command;
        }

        /// <summary>
        /// Gets the command used.
        /// </summary>
        public string Command { get; }

        /// <summary>
        /// Gets the Discord user ID.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the Discord username.
        /// </summary>
        public string Name { get; }
    }
}
