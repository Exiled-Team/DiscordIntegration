// -----------------------------------------------------------------------
// <copyright file="TerminatedEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.API.EventArgs.Network
{
    using System.Threading.Tasks;

    /// <summary>
    /// Contains all informations after the network termination.
    /// </summary>
    public class TerminatedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TerminatedEventArgs"/> class.
        /// </summary>
        /// <param name="task"><inheritdoc cref="Task"/></param>
        public TerminatedEventArgs(Task task) => Task = task;

        /// <summary>
        /// Gets the terminated task.
        /// </summary>
        public Task Task { get; }
    }
}
