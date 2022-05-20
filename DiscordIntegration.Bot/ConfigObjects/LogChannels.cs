namespace DiscordIntegration.Bot.ConfigObjects;

public class LogChannels
{
    public List<ulong> Commands { get; set; } = new();
    public List<ulong> GameEvents { get; set; } = new();
    public List<ulong> Bans { get; set; } = new();
    public List<ulong> Reports { get; set; } = new();
    public List<ulong> StaffCopy { get; set; } = new();
}