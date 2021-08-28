// -----------------------------------------------------------------------
// <copyright file="Field.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration
{
    /// <summary>
    /// Fields for embed
    /// </summary>
    public class Field
    {
        private string name;
        private string value;
        private bool inline;

        /// <summary>
        /// Initializes a new instance of the <see cref="Field"/> class.
        /// </summary>
        /// <param name="name">Title of Field.</param>
        /// <param name="value"> Content of field.</param>
        /// <param name="inline">inline boolean.</param>
        public Field(string name, string value, bool inline = false)
        {
            this.name = name;
            this.value = value;
            this.inline = inline;
        }
    }
}
