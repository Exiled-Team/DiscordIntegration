namespace DiscordIntegration.Bot.ConfigObjects;

public class TcpServerConfig
{
    public ushort Port { get; set; } = 0;
    public string IpAddress { get; set; } = string.Empty;
}