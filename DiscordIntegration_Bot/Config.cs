using System.Collections.Generic;

namespace DiscordIntegration_Bot
{
	public class Config
	{
		public bool Debug { get; set; }
		public string BotPrefix { get; set; }
		public string BotToken { get; set; }
		public int Port { get; set; }
		public ulong PermLevel1Id { get; set; }
		public ulong Permlevel2Id { get; set; }
		public ulong Permlevel3Id { get; set; }
		public ulong Permlevel4Id { get; set; }
		public ulong CommandLogChannelId { get; set; }
		public ulong GameLogChannelId { get; set; }
		public Dictionary<string, PermLevel> AllowedCommands { get; set; }
		public ulong StaffRoleId { get; set; }
		public bool EggMode { get; set; }

		public static readonly Config Default = new Config
		{
			Debug = false,
			BotPrefix = "!",
			BotToken = "",
			Port = 0,
			PermLevel1Id = 0,
			Permlevel2Id = 0,
			Permlevel3Id = 0,
			Permlevel4Id = 0,
			CommandLogChannelId = 0,
			GameLogChannelId = 0,
			AllowedCommands = new Dictionary<string, PermLevel>() { {"list", PermLevel.PermLevel0} },
			StaffRoleId = 0,
			EggMode = false
		};
	}
}