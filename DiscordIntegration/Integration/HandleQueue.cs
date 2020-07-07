using System;
using System.Collections.Generic;
using Exiled.API.Features;
using MEC;

namespace DiscordIntegration_Plugin
{
	public class HandleQueue
	{
		public static ulong ChannelId;
		public static ulong GameLogChannelId = 1;
		public static ulong CommandLogChannelId = 2;
		
		public static void HandleQueuedItems()
		{
			while (ProcessSTT.dataQueue.TryDequeue(out SerializedData.SerializedData result))
			{
				string command = result.Data;
				Log.Debug($"STT: Received {result.Data} for {result.Port} at {result.Channel}", Plugin.Singleton.Config.Debug);
				
				
				if (result.Data == "ping")
				{
					Log.Debug("STT: Heartbeat received.", Plugin.Singleton.Config.Debug);
					ProcessSTT.SendData("ping", 0);
					return;
				}

				if (result.Data == "set gameid")
				{
					GameLogChannelId = result.Channel;
					Log.Debug($"STT: GameLogChannelId changed: {result.Channel}", Plugin.Singleton.Config.Debug);
					return;
				}

				if (result.Data == "set cmdid")
				{
					CommandLogChannelId = result.Channel;
					Log.Debug($"STT: CommandLogChannelId changed: {result.Channel}", Plugin.Singleton.Config.Debug);
					return;
				}

				if (result.Data.StartsWith("setgroup"))
				{
					string[] args = result.Data.Split(' ');
					Methods.SetSyncRole(args[2], args[1]);
				}

				ChannelId = result.Channel;

				try
				{
					command = command.Substring(1);
					GameCore.Console.singleton.TypeCommand($"/{command}", new BotSender(result.Name));
				}
				catch (Exception e)
				{
					Log.Error(e.ToString());
				}
			}
		}

		public static IEnumerator<float> Handle()
		{
			for (;;)
			{
				try
				{
					if (!ProcessSTT.dataQueue.IsEmpty)
						HandleQueuedItems();
				}
				catch (Exception e)
				{
					Log.Error($"STT: Error handling queue. {e}");
				}

				yield return Timing.WaitForSeconds(1f);
				
			}
		}
	}
	
	public class BotSender : CommandSender
	{
		public override void RaReply(string text, bool success, bool logToConsole, string overrideDisplay)
		{
			ProcessSTT.SendData($"{text}", HandleQueue.ChannelId);
		}

		public override void Print(string text)
		{
			ProcessSTT.SendData($"{text}", HandleQueue.ChannelId);
		}

		public string Name;
		public BotSender(string name) => Name = name;
		public override string SenderId => "SERVER CONSOLE";
		public override string Nickname => Name;
		public override ulong Permissions => ServerStatic.GetPermissionsHandler().FullPerm;
		public override byte KickPower => byte.MaxValue;
		public override bool FullPermissions => true;
	}
}