namespace DiscordIntegration.Bot.ConfigObjects;

using Dependency;

public class LogChannels
{
    public List<ulong>? Commands { get; set; } = new();
    public List<ulong>? GameEvents { get; set; } = new();
    public List<ulong>? Bans { get; set; } = new();
    public List<ulong>? Reports { get; set; } = new();
    public List<ulong>? StaffCopy { get; set; } = new();

    public IEnumerable<ulong> this[ChannelType result] => (result switch
    {
        ChannelType.Command => Commands,
        ChannelType.GameEvents => GameEvents,
        ChannelType.Bans => Bans,
        ChannelType.Reports => Reports,
        ChannelType.StaffCopy => StaffCopy,
        _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
    })!;
}