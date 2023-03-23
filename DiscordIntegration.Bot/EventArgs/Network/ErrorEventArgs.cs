// -----------------------------------------------------------------------
// <copyright file="ErrorEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.API.EventArgs.Network
{
    using System;

    /// <summary>
    /// Contains all informations after the network thrown an exception.
    /// </summary>
    public class ErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEventArgs"/> class.
        /// </summary>
        /// <param name="exception"><inheritdoc cref="Exception"/></param>
        public ErrorEventArgs(Exception exception) => Exception = exception;

        /// <summary>
        /// Gets the thrown exception.
        /// </summary>
        public Exception Exception { get; }
    }
}
