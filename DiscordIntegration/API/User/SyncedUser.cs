// -----------------------------------------------------------------------
// <copyright file="SyncedUser.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.API.User
{
    using Exiled.API.Features;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a synced user from the Discord server and SCP: SL one.
    /// </summary>
    public class SyncedUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyncedUser"/> class.
        /// </summary>
        /// <param name="id"><inheritdoc cref="Id"/></param>
        /// <param name="group"><inheritdoc cref="Group"/></param>
        [JsonConstructor]
        public SyncedUser(string id, string group)
        {
            Id = id;
            Group = group;
        }

        /// <summary>
        /// Gets the user ID.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the user group.
        /// </summary>
        public string Group { get; }

        /// <summary>
        /// Sets the user group.
        /// </summary>
        public void SetGroup()
        {
            UserGroup userGroup = ServerStatic.PermissionsHandler.GetGroup(Group);

            if (userGroup == null)
            {
                Log.Error(string.Format(DiscordIntegration.Language.InvalidUserGroupError, Group, Id));
                return;
            }

            Player player = Player.Get(Id);

            if (player == null)
            {
                Log.Error(string.Format(DiscordIntegration.Language.AssigningUserGroupError, Id));
                return;
            }

            Log.Debug(string.Format(DiscordIntegration.Language.AssingingSyncedGroup, Id, Group));

            player.SetRank(userGroup.BadgeText, userGroup);
        }
    }
}
