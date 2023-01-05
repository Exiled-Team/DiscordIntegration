// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using API.Configs;
    using Exiled.API.Enums;
    using Exiled.API.Interfaces;

    /// <summary>
    /// Handles plugin configs.
    /// </summary>
    public class Config : IConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether the plugin is enabled or not.
        /// </summary>
        [Description("Indicates whether the plugin is enabled or not")]
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets bot-related configs.
        /// </summary>
        [Description("Bot-related configs")]
        public Bot Bot { get; set; } = new();

        /// <summary>
        /// Gets events to log confings.
        /// </summary>
        [Description("Indicates events that should be logged or not")]
        public EventsToLog EventsToLog { get; set; } = new();

        /// <summary>
        /// Gets events to log to the staff-only channel. These logs will always ignore DNT, and always show the player's UserID.
        /// </summary>
        [Description("Indicates events that should be logged to the staff-only channel. These logs always ignore DNT, and will always show the player's UserID.")]
        public EventsToLog StaffOnlyEventsToLog { get; set; } = new();

        /// <summary>
        /// Gets a value indicating whether players' IP addresses should be logged or not.
        /// </summary>
        [Description("Indicates whether players' IP Addresses should be logged or not")]
        public bool ShouldLogIPAddresses { get; set; } = true;

        /// <summary>
        /// Gets a value indicating whether player's user ids should be logged or not.
        /// </summary>
        [Description("Indicates whether players' user ids should be logged or not")]
        public bool ShouldLogUserIds { get; set; } = true;

        /// <summary>
        /// Gets a value indicating whether server errors should be logged.
        /// </summary>
        [Description("Indicates whether server errors should be logged or not.")]
        public bool LogErrors { get; set; } = true;

        /// <summary>
        /// Gets a value indicating whether players' with the "Do not track" enabled, should be logged or not.
        /// </summary>
        [Description("Indicates whether players' with the \"Do not track\" enabled, should be logged or not")]
        public bool ShouldRespectDoNotTrack { get; set; } = true;

        /// <summary>
        /// Gets a value indicating whether only friendly fire for damage should be logged or not.
        /// </summary>
        [Description("Indicates whether only friendly fire for damage should be logged or not")]
        public bool ShouldLogFriendlyFireDamageOnly { get; set; }

        /// <summary>
        /// Gets a value indicating whether only friendly fire for kills should be logged or not.
        /// </summary>
        [Description("Indicates whether only friendly fire for kills should be logged or not")]
        public bool ShouldLogFriendlyFireKillsOnly { get; set; }

        /// <summary>
        /// Gets a value indicating what damage types are not logged in hurting events.
        /// </summary>
        [Description("Indicates what damage types aren't allowed to be logged for hurting events. These filters will not apply to death logs.")]
        public List<DamageType> BlacklistedDamageTypes { get; set; } = new()
        {
            DamageType.Scp207,
            DamageType.PocketDimension,
        };

        /// <summary>
        /// Gets a value indicating whether only player-dealt damage is logged in hurting events.
        /// </summary>
        [Description("Indicates whether or not only player-dealt damage should be logged in hurting events.")]
        public bool OnlyLogPlayerDamage { get; set; }

        /// <summary>
        /// Gets the date format that will be used throughout the plugin.
        /// </summary>
        [Description("The date format that will be used throughout the plugin (es. dd/MM/yy HH:mm:ss or MM/dd/yy HH:mm:ss)")]
        public string DateFormat { get; set; } = "dd/MM/yy HH:mm:ss";

        /// <summary>
        /// Gets a value indicating whether the debug is enabled or not.
        /// </summary>
        [Description("Indicates whether the debug is enabled or not")]
        public bool Debug { get; set; }

        /// <summary>
        /// Gets a value indicating which admin userIDs are trusted.
        /// </summary>
        [Description("The list of trusted admins, whos command usage will not be logged.")]
        public List<string> TrustedAdmins { get; set; } = new();

        /// <summary>
        /// Gets the plugin language.
        /// </summary>
        [Description("The plugin language")]
        public string Language { get; set; } = "english";

        /// <summary>
        /// Gets a value indicating whether to use the player watchlist feature.
        /// </summary>
        [Description("Indicates whether the player watchlist feature can be used.")]
        public bool UseWatchlist { get; set; } = true;
    }
}