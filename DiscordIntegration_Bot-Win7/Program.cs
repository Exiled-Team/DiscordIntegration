using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Newtonsoft.Json;
using System.Runtime.Serialization;

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

		public static void Main()
		{
			Log("Hello yes welcome to DiscordIntegration", true);
			new Program();
		}

		public Program()
		{
			string path = $"{Directory.GetCurrentDirectory()}/logs/{DateTime.UtcNow.Ticks}.txt";
			Log($"Creating log file: {path}", true);
			if (!Directory.Exists($"{Directory.GetCurrentDirectory()}/logs"))
				Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}/logs");
			if (!File.Exists(path))
				File.Create(path).Close();
			LogFile = path;
			Log("Initializing bot", true);
			_bot = new Bot(this);
		}

		public static Task Log(LogMessage msg)
		{
			Console.Write(msg.ToString() + Environment.NewLine);
			while (fileLocked)
				Thread.Sleep(1000);

			if (LogFile != null)
			{
				fileLocked = true;
				File.AppendAllText(LogFile, msg.ToString());
			}

			fileLocked = false;
			return Task.CompletedTask;
		}

		public static void Log(string message, bool debug = false)
		{
			if (!debug)
				Log(new LogMessage(LogSeverity.Info, "LOG", message));
			else if (Config.Debug)
				Log(new LogMessage(LogSeverity.Debug, "DEBUG", message));
		}
		
		public static void Error(string message) => Log(new LogMessage(LogSeverity.Debug, "ERROR", message));

		public static Config GetConfig()
		{
			if (File.Exists(kCfgFile))
				return JsonConvert.DeserializeObject<Config>(File.ReadAllText(kCfgFile));
			File.WriteAllText(kCfgFile, JsonConvert.SerializeObject(Config.Default));
			return Config.Default;
		}
	}
}