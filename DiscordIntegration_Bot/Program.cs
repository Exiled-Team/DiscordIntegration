using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Newtonsoft.Json;

namespace DiscordIntegration_Bot
{
	public class Program
	{
		private static string LogFile;
		public static Bot _bot;
		private const string kCfgFile = "IntegrationBotConfig.json";
		public Config Config => config ?? (config = GetConfig());
		private Config config;

		public static void Main()
		{
			new Program();
		}

		public Program()
		{
			string path = $"{Directory.GetCurrentDirectory()}/logs/{DateTime.UtcNow.Ticks}.txt";
			if (!Directory.Exists($"{Directory.GetCurrentDirectory()}/logs"))
				Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}/logs");
			if (!File.Exists(path))
				File.Create(path).Close();
			LogFile = path;
			_bot = new Bot(this);
		}

		public static Task Log(LogMessage msg)
		{
			Console.Write(msg.ToString());
			File.AppendAllText(LogFile, msg.ToString());
			return Task.CompletedTask;
		}

		public static Config GetConfig()
		{
			if (File.Exists(kCfgFile))
				return JsonConvert.DeserializeObject<Config>(File.ReadAllText(kCfgFile));
			File.WriteAllText(kCfgFile, JsonConvert.SerializeObject(Config.Default));
			return Config.Default;
		}
	}
}