// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration
{
    using System.ComponentModel;
    using API.Configs;
    using Exiled.API.Interfaces;

    /// <summary>
    /// Handles plugin configs.
    /// </summary>
    public sealed class Config : IConfig
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
        public Bot Bot { get; private set; } = new Bot();

        /// <summary>
        /// Gets events to log confings.
        /// </summary>
        [Description("Indicates events that should be logged or not")]
        public EventsToLog EventsToLog { get; private set; } = new EventsToLog();

        /// <summary>
        /// Gets a value indicating whether players' IP addresses should be logged or not.
        /// </summary>
        [Description("Indicates whether players' IP Addresses should be logged or not")]
        public bool ShouldLogIPAddresses { get; private set; } = true;

        /// <summary>
        /// Gets a value indicating whether player's user ids should be logged or not.
        /// </summary>
        [Description("Indicates whether players' user ids should be logged or not")]
        public bool ShouldLogUserIds { get; private set; } = true;

        /// <summary>
        /// Gets a value indicating whether players' with the "Do not track" enabled, should be logged or not.
        /// </summary>
        [Description("Indicates whether players' with the \"Do not track\" enabled, should be logged or not")]
        public bool ShouldRespectDoNotTrack { get; private set; } = true;

        /// <summary>
        /// Gets a value indicating whether only friendly fire for damage should be logged or not.
        /// </summary>
        [Description("Indicates whether only friendly fire for damage should be logged or not")]
        public bool ShouldLogFriendlyFireDamageOnly { get; private set; }

        /// <summary>
        /// Gets a value indicating whether only friendly fire for kills should be logged or not.
        /// </summary>
        [Description("Indicates whether only friendly fire for kills should be logged or not")]
        public bool ShouldLogFriendlyFireKillsOnly { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the plugin should or shouldn't log SCP-207 damage when the damage log is enabled.
        /// </summary>
        [Description("Indicates whether the plugin should or shouldn't log SCP-207 damage when the damage log is enabled")]
        public bool ShouldLogScp207Damage { get; private set; } = true;

        /// <summary>
        /// Gets the date format that will be used throughout the plugin.
        /// </summary>
        [Description("The date format that will be used throughout the plugin (es. dd/MM/yy HH:mm:ss or MM/dd/yy HH:mm:ss)")]
        public string DateFormat { get; private set; } = "dd/MM/yy HH:mm:ss";

        /// <summary>
        /// Gets a value indicating whether roles should be synced or not.
        /// </summary>
        [Description("Indicates whether the plugin should try and set player's roles when they join based on the Discord Bot's discord sync feature or not")]
        public bool ShouldSyncRoles { get; private set; } = true;

        /// <summary>
        /// Gets a value indicating whether the debug is enabled or not.
        /// </summary>
        [Description("Indicates whether the debug is enabled or not")]
        public bool IsDebugEnabled { get; private set; }

        /// <summary>
        /// Gets the plugin language.
        /// </summary>
        [Description("The plugin language")]
        public string Language { get; private set; } = "english";
    }
}