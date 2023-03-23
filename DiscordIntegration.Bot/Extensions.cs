namespace DiscordIntegration.Bot;

using System.Text.RegularExpressions;
using Discord.WebSocket;

public static class Extensions
{
    public static string SplitCamelCase(this string str)
    {
        return Regex.Replace( 
            Regex.Replace( 
                str, 
                @"(\P{Ll})(\P{Ll}\p{Ll})", 
                "$1 $2" 
            ), 
            @"(\p{Ll})(\P{Ll})", 
            "$1 $2" 
        );
    }
    
    public static string GetUsername(this SocketGuild guild, ulong userId) => !string.IsNullOrEmpty(guild.GetUser(userId)?.Username) ? guild.GetUser(userId).Username : $"Name unavailable. ({userId})";
}