using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordIntegration_Plugin
{
    public static class Translate
    {
        public static string Traduccion(this RoleType type)
        {
            switch (type)
            {
                case RoleType.None:
                    return "En Lobby";
                case RoleType.Scp173:
                    return "SCP-173";
                case RoleType.ClassD:
                    return "Class-D";
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
                    return "Ninguno";
            }
        }
    }
}