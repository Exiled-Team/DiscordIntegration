namespace DiscordIntegration.Bot.Services;

using Discord;

public class Log
{
    public static Task Send(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    public static void Info(string source, object msg) => Send(new LogMessage(LogSeverity.Info,
        source, msg.ToString()));

    public static void Debug(string source, object msg)
    {
        if (Program.Config.Debug)
            Send(new LogMessage(LogSeverity.Debug, source, msg.ToString()));
    }
        
    public static void Error(string source, object msg) => Send(new LogMessage(LogSeverity.Error, source, msg.ToString()));

    public static void Warn(string source, object msg) =>
        Send(new LogMessage(LogSeverity.Warning, source, msg.ToString()));
}