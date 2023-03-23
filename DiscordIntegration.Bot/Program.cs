namespace DiscordIntegration.Bot;

using System.Reflection;

using Newtonsoft.Json;
using Services;

public static class Program
{
    private static Config? _config;
    private static string KCfgFile = "DiscordIntegration-config.json";
    private static List<Bot> _bots = new();

    public static Config Config => _config ??= GetConfig();
    public static Random Rng { get; } = new();

    public static void Main(string[] args)
    {
        Log.Info(0, nameof(Main), $"Welcome to Discord Integration v{Assembly.GetExecutingAssembly().GetName().Version}!");
        if (args.Contains("--debug"))
            Config.Debug = true;
        
        foreach (KeyValuePair<ushort, string> botToken in Config.BotTokens)
            _bots.Add(new Bot(botToken.Key, botToken.Value));
        
        AppDomain.CurrentDomain.ProcessExit += (_, _) =>
        {
            foreach (Bot bot in _bots)
                bot.Destroy();

            if (Config.Debug)
            {
                Log.Warn(0, nameof(Main), "Shutting down..");
                Thread.Sleep(10000);
            }
        };

        KeepAlive().GetAwaiter().GetResult();
    }

    private static Config GetConfig()
    {
        if (File.Exists(KCfgFile))
            return JsonConvert.DeserializeObject<Config>(File.ReadAllText(KCfgFile))!;
        File.WriteAllText(KCfgFile, JsonConvert.SerializeObject(Config.Default, Formatting.Indented));
        return Config.Default;
    }

    private static async Task KeepAlive()
    {
        Log.Debug(0, nameof(KeepAlive), "Keeping alive bots.");
        await Task.Delay(-1);
    }
}
