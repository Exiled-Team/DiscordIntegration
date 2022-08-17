namespace DiscordIntegration.Bot.ConfigObjects;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class LogChannel
{
    public ulong Id { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public LogType LogType { get; set; }
}