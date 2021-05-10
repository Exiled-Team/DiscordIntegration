// -----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration
{
    using System.Text.RegularExpressions;
    using API.Commands;
    using CommandSystem;
    using Exiled.API.Features;

    /// <summary>
    /// Useful Extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Checks if a user ID is valid.
        /// </summary>
        /// <param name="userId">The user ID to be checked.</param>
        /// <returns>Returns a value indicating whether the user ID is valid or not.</returns>
        public static bool IsValidUserId(this string userId) => Regex.IsMatch(userId, "^([0-9]{17})@(steam|patreon|northwood)|([0-9]{18})@(discord)$");

        /// <summary>
        /// Checks if a Discord ID is valid.
        /// </summary>
        /// <param name="discordId">The Discord ID to be checked.</param>
        /// <returns>Returns a value indicating whether the Discord ID is valid or not.</returns>
        public static bool IsValidDiscordId(this string discordId) => Regex.IsMatch(discordId, "^[0-9]{18}$");

        /// <summary>
        /// Checks if a Discord role ID is valid.
        /// </summary>
        /// <param name="discordRoleId">The Discord role ID to be checked.</param>
        /// <returns>Returns a value indicating whether the Discord ID is valid or not.</returns>
        public static bool IsValidDiscordRoleId(this string discordRoleId) => IsValidDiscordId(discordRoleId);

        /// <summary>
        /// Gets a compatible and JSON serializable <see cref="CommandSender"/>.
        /// </summary>
        /// <param name="sender">The <see cref="ICommandSender"/> to be checked.</param>
        /// <returns>Returns the compatible <see cref="CommandSender"/>.</returns>
        public static CommandSender GetCompatible(this ICommandSender sender) => ((CommandSender)sender).GetCompatible();

        /// <summary>
        /// Gets a compatible and JSON serializable <see cref="CommandSender"/>.
        /// </summary>
        /// <param name="sender">The sender to be checked.</param>
        /// <returns>Returns the compatible <see cref="CommandSender"/>.</returns>
        public static CommandSender GetCompatible(this CommandSender sender)
        {
            if (sender.GetType() != typeof(RemoteAdmin.PlayerCommandSender))
                return sender;

            return new PlayerCommandSender(sender.SenderId, sender.Nickname, sender.Permissions, sender.KickPower, sender.FullPermissions);
        }

#pragma warning disable SA1614 // Element parameter documentation should have text

#pragma warning disable SA1616 // Element return value documentation should have text
        /// <summary>
        /// Traduce los roles a Español.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string Translate(this RoleType type)
#pragma warning restore SA1616 // Element return value documentation should have text
#pragma warning restore SA1614 // Element parameter documentation should have text
        {
            switch (type)
            {
                case RoleType.None:
                    return "En Lobby";
                case RoleType.Scp173:
                    return "SCP-173";
                case RoleType.ClassD:
                    return "Clase-D";
                case RoleType.Spectator:
                    return "Espectador";
                case RoleType.Scp106:
                    return "SCP-106";
                case RoleType.NtfScientist:
                    return "Científico MTF";
                case RoleType.Scp049:
                    return "SCP-049";
                case RoleType.Scientist:
                    return "Científico";
                case RoleType.Scp079:
                    return "SCP-079";
                case RoleType.ChaosInsurgency:
                    return "Insurgente del Caos";
                case RoleType.Scp096:
                    return "SCP-096";
                case RoleType.Scp0492:
                    return "SCP-049-2";
                case RoleType.NtfLieutenant:
                    return "Teniente MTF";
                case RoleType.NtfCommander:
                    return "Comandante MTF";
                case RoleType.NtfCadet:
                    return "Cadete MTF";
                case RoleType.Tutorial:
                    return "Tutorial";
                case RoleType.FacilityGuard:
                    return "Guardia";
                case RoleType.Scp93953:
                    return "SCP-939-53";
                case RoleType.Scp93989:
                    return "SCP-939-89";
                default:
                    return "No tiene";
            }
        }

        /// <summary>
        /// Get if player have medigun on inventory.
        /// </summary>
        /// <param name="customItems"> The item to be checked.</param>
        /// <returns>bool.</returns>
        public static bool CheckMediGun(this Exiled.CustomItems.API.Features.CustomItem customItems)
        {
            if (customItems.Id == 5 && customItems.Name == "MG-119")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class Field
    {
        public string name;
        public string value;
        public bool inline;

        public Field(string name, string value, bool inline = false)
        {
            this.name = name;
            this.value = value;
            this.inline = inline;
        }
    }
}
