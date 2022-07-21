namespace DiscordIntegration.Bot;

using ConfigObjects;

public class Config
{
    public Dictionary<ushort, string> BotTokens { get; set; } = new();
    public Dictionary<ulong, ushort> BotIds { get; set; } = new();
    public Dictionary<ushort, ChannelConfig> Channels { get; set; } = new();
    public Dictionary<ushort, Dictionary<ulong, List<string>>> ValidCommands { get; set; } = new();
    public Dictionary<ushort, ulong> DiscordServerIds { get; set; } = new();
    public Dictionary<ushort, TcpServerConfig> TcpServers { get; set; } = new();
    public int YourUtc;
    public int KeepAliveInterval { get; set; }
    public int MessageDelay { get; set; }
    public bool Debug { get; set; }

    public static Config Default => new()
    {
        BotTokens = new Dictionary<ushort, string>
        {
            {
                1,
                "bot-token-here"
            }
        },
        
        Channels = new Dictionary<ushort, ChannelConfig>
        {
            {
                1,
                new()
                {
                    TopicInfo = new List<ulong>
                    {
                        0
                    },
                    CommandChannel = new List<ulong>
                    {
                        0,
                    },
                    Logs = new LogChannels
                    {
                        Commands = new List<ulong>
                        {
                            0,
                        },
                        GameEvents = new List<ulong>
                        {
                            0,
                        },
                        Bans = new List<ulong>
                        {
                            0,
                        },
                        Reports = new List<ulong>
                        {
                            0,
                        },
                        StaffCopy = new List<ulong>
                        {
                            0,
                        },
                        Errors = new List<ulong>
                        {
                            0,
                        },
                    }
                }
            }
        },
        
        ValidCommands = new Dictionary<ushort, Dictionary<ulong, List<string>>>
        {
            {
                1, new Dictionary<ulong, List<string>>
                {
                    {
                        0, new List<string>
                        {
                            "di"
                        }
                    }
                }
            }
        },
        
        BotIds = new Dictionary<ulong, ushort>()
        {
            {1, 0}
        },
        
        DiscordServerIds = new Dictionary<ushort, ulong>
        {
            {
                1, 0
            }
        },
        
        TcpServers = new Dictionary<ushort, TcpServerConfig>
        {
            {
                1, new TcpServerConfig
                {
                    Port = 9000,
                    IpAddress = "127.0.0.1"
                }
            }
        },
        
        YourUtc = 3,
        KeepAliveInterval = 2000,
        MessageDelay = 1000,
        Debug = false,
    };
}