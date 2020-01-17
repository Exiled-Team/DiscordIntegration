using System;
using System.Collections.Generic;
using MEC;

namespace DiscordIntegration_Plugin
{
	public class HandleQueue
	{
		public static ulong channelid;
		public static ulong GameLogChannelId;
		public static ulong CommandLogChannelId;
		
		public static void HandleQueuedItems()
		{
			while (ProcessSTT.dataQueue.TryDequeue(out SerializedData.SerializedData result))
			{
				string command = result.Data;
				Plugin.Debug($"STT: Received {result.Data} for {result.Port}");
				if (result.Port != ServerConsole.Port) 
					return;
				
				if (result.Data == "ping")
				{
					Plugin.Debug("STT: Heartbeat received.");
					ProcessSTT.SendData("ping", 0);
					return;
				}

				if (result.Data == "set gameid")
				{
					GameLogChannelId = result.Channel;
					Plugin.Debug($"STT: GameLogChannelId changed: {result.Channel}");
					return;
				}

				if (result.Data == "set cmdid")
				{
					CommandLogChannelId = result.Channel;
					Plugin.Debug($"STT: CommandLogChannelId changed: {result.Channel}");
					return;
				}

				channelid = result.Channel;

				try
				{
					GameCore.Console.singleton.TypeCommand($"/{command}", new BotSender(result.Name));
				}
				catch (Exception e)
				{
					Plugin.Error(e.ToString());
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
					Plugin.Error($"STT: Error handling queue. {e}");
				}

				yield return Timing.WaitForSeconds(1f);
				
			}
		}
	}
	
	public class BotSender : CommandSender
	{
		public override void RaReply(string text, bool success, bool logToConsole, string overrideDisplay)
		{
			ProcessSTT.SendData($"{text}", HandleQueue.channelid);
		}

		public override void Print(string text)
		{
			ProcessSTT.SendData($"{text}", HandleQueue.channelid);
		}

		public string Name;
		public BotSender(string name) => Name = name;
		public override string SenderId => Name;
		public override string Nickname => Name;
		public override ulong Permissions => ServerStatic.GetPermissionsHandler().FullPerm;
		public override byte KickPower => byte.MaxValue;
		public override bool FullPermissions => true;
	}
}