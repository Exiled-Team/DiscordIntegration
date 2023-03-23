// -----------------------------------------------------------------------
// <copyright file="ReceivingErrorEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.API.EventArgs.Network
{
    using System;

    /// <summary>
    /// Contains all informations after the network thrown an exception while receiving data.
    /// </summary>
    public class ReceivingErrorEventArgs : ErrorEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReceivingErrorEventArgs"/> class.
        /// </summary>
        /// <param name="exception"><inheritdoc cref="ErrorEventArgs.Exception"/></param>
        public ReceivingErrorEventArgs(Exception exception)
            : base(exception)
        {
        }
    }
}
