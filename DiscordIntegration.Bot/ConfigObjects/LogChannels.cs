namespace DiscordIntegration.Bot.ConfigObjects;

using Dependency;

public class LogChannels
{
    public List<LogChannel>? Commands { get; set; } = new();
    public List<LogChannel>? GameEvents { get; set; } = new();
    public List<LogChannel>? Bans { get; set; } = new();
    public List<LogChannel>? Reports { get; set; } = new();
    public List<LogChannel>? StaffCopy { get; set; } = new();
    public List<LogChannel>? Errors { get; set; } = new();
    public List<LogChannel>? Watchlist { get; set; } = new();

    public IEnumerable<LogChannel> this[ChannelType result]
    {
        get =>
            (result switch
            {
                ChannelType.Command => Commands,
                ChannelType.GameEvents => GameEvents,
                ChannelType.Bans => Bans,
                ChannelType.Reports => Reports,
                ChannelType.StaffCopy => StaffCopy,
                ChannelType.Errors => Errors,
                ChannelType.Watchlist => Watchlist,
                _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
            })!;
    }
}