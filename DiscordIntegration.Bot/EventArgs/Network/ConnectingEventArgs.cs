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
        /// <param name="ipAddress"><inheritdoc cref="IPAddress"/></param>
        /// <param name="port"><inheritdoc cref="Port"/></param>
        public ConnectingEventArgs(IPAddress ipAddress, ushort port)
        {
            IPAddress = ipAddress;
            Port = port;
        }

        /// <summary>
        /// Gets or sets the IP address.
        /// </summary>
        public IPAddress IPAddress { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        public ushort Port { get; set; }
    }
}
