// -----------------------------------------------------------------------
// <copyright file="ConnectingEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.API.EventArgs.Network
{
    using System;
    using System.Net;

    /// <summary>
    /// Contains all informations before the network connects to a server.
    /// </summary>
    public class ConnectingEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectingEventArgs"/> class.
        /// </summary>
        /// <param name="ipEndPoint"><inheritdoc cref="IPEndPoint"/></param>
        public ConnectingEventArgs(IPEndPoint ipEndPoint) => IPEndPoint = ipEndPoint;

        /// <summary>
        /// Gets or sets the IP endpoint to connect with.
        /// </summary>
        public IPEndPoint IPEndPoint { get; set; }
    }
}
