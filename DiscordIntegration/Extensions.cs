﻿// -----------------------------------------------------------------------
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

        /// <summary>
        /// Traduccion de los <see cref="RoleType"/> del juego.
        /// </summary>
        /// <param name="type"></param>
        /// <returns><see cref="string"/> traducida a español.</returns>
        public static string Translate(this RoleType type)
        {
            switch (type)
            {
                case RoleType.None:
                    return "Ninguno";
                case RoleType.Spectator:
                    return "Espectador";
                case RoleType.Tutorial:
                    return "Tutorial";
                #region SCPs
                case RoleType.Scp173:
                    return "SCP-173";
                case RoleType.Scp106:
                    return "SCP-106";
                case RoleType.Scp079:
                    return "SCP-079";
                case RoleType.Scp049:
                    return "SCP-049";
                case RoleType.Scp096:
                    return "SCP-096";
                case RoleType.Scp0492:
                    return "SCP-049-2";
                case RoleType.Scp93953:
                    return "SCP-939-53";
                case RoleType.Scp93989:
                    return "SCP-939-89";
                #endregion

                #region Humanos desarmados
                case RoleType.ClassD:
                    return "Clase-D";
                case RoleType.Scientist:
                    return "Científico";
                #endregion

                #region NTF/MTF
                case RoleType.NtfSpecialist:
                    return "Especialista de la NTF";
                case RoleType.NtfSergeant:
                    return "Sargento de la NTF";
                case RoleType.NtfCaptain:
                    return "Capitán de la NTF";
                case RoleType.NtfPrivate:
                    return "Soldado Raso de la NTF";
                case RoleType.FacilityGuard:
                    return "Guardia de las instalaciones";
                #endregion

                #region Chaos

                case RoleType.ChaosConscript:
                    return "Recluta de los Caos";
                case RoleType.ChaosRifleman:
                    return "Fusilero de los Caos";
                case RoleType.ChaosRepressor:
                    return "Represor de los Caos";
                case RoleType.ChaosMarauder:
                    return "Merodeador de los Caos";
                #endregion
                default:
                    return "None";
            }
        }


    }
}
