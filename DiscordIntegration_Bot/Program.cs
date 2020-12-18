using Discord;
using DiscordIntegration_Bot.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordIntegration_Bot
{
    public class Program
    {
        private static string LogFile;
        public static Bot _bot;
        private const string kCfgFile = "IntegrationBotConfig.json";
        public static Config Config = GetConfig();
        public static bool fileLocked = false;
        public static List<SyncedUser> Users = new List<SyncedUser>();
        public static Dictionary<ulong, string> SyncedGroups = new Dictionary<ulong, string>();
        public static List<string> LogFiles = new List<string>();
        public static DateTime FileCreated;

        public static void Main()
        {
            Logger.LogInfo("Joker", "Hello yes welcome to DiscordIntegration");
            new Program();
        }

        public Program()
        {
            string path = $"{Directory.GetCurrentDirectory()}/logs/Debug-{DateTime.UtcNow.ToString("yyyy-MM-dd")}.txt";
            Logger.LogInfo("Logs", $"Creating log file: {path}");
            if (!Directory.Exists($"{Directory.GetCurrentDirectory()}/logs"))
                Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}/logs");
            foreach (string file in Directory.GetFiles($"{Directory.GetCurrentDirectory()}/logs"))
                LogFiles.Add(file);
            if (!File.Exists(path))
                File.Create(path).Close();

            while (LogFiles.Count > 10)
            {
                string file = LogFiles[0];
                File.Delete(file);
                LogFiles.Remove(file);
            }

            LogFile = path;
            LogFiles.Add(path);
            FileCreated = DateTime.UtcNow;
            _bot = new Bot(this);
        }

        public static Task Log(LogMessage msg)
        {
            Console.Write(msg.ToString() + Environment.NewLine);
            while (fileLocked)
                Thread.Sleep(1000);

            if ((FileCreated - DateTime.UtcNow).TotalDays > 1)
            {
                LogFile = $"{Directory.GetCurrentDirectory()}/logs/Debug-{DateTime.UtcNow.ToString("yyyy-MM-dd")}.txt";
                FileCreated = DateTime.UtcNow;
                LogFiles.Add(LogFile);
            }

            if (LogFile != null)
            {
                fileLocked = true;
                File.AppendAllText(LogFile, msg.ToString());
            }

            fileLocked = false;
            while (LogFiles.Count > 10)
            {
                string file = LogFiles[0];
                File.Delete(file);
                LogFiles.Remove(file);
            }

            return Task.CompletedTask;
        }

        public static void Log(string message, bool debug = false)
        {
            if (!debug)
                Log(new LogMessage(LogSeverity.Info, "LOG", message));
            else if (Config.Debug)
                Log(new LogMessage(LogSeverity.Debug, "DEBUG", message));
        }

        public static void Error(string message) => Log(new LogMessage(LogSeverity.Error, "ERROR", message));

        public static Config GetConfig()
        {
            if (File.Exists(kCfgFile))
                return JsonConvert.DeserializeObject<Config>(File.ReadAllText(kCfgFile));
            File.WriteAllText(kCfgFile, JsonConvert.SerializeObject(Config.Default));
            return Config.Default;
        }
    }
}