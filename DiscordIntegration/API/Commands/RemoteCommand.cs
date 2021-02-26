// -----------------------------------------------------------------------
// <copyright file="RemoteCommand.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.API.Commands
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a remote command, sent to the server.
    /// </summary>
    public class RemoteCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteCommand"/> class.
        /// </summary>
        /// <param name="action"><inheritdoc cref="Action"/></param>
        /// <param name="parameters"><inheritdoc cref="Parameters"/></param>
        [JsonConstructor]
        public RemoteCommand(string action, object parameters)
        {
            Action = action;
            Parameters = new object[1] { parameters };
        }

        // A cosa serve questo costruttore sopra???

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteCommand"/> class.
        /// </summary>
        /// <param name="action"><inheritdoc cref="Action"/></param>
        /// <param name="parameters"><inheritdoc cref="Parameters"/></param>
        public RemoteCommand(string action, params object[] parameters)
        {
            Action = action;
            Parameters = parameters;
        }

        /// <summary>
        /// Gets the action.
        /// </summary>
        public string Action { get; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public object[] Parameters { get; }
    }
}
