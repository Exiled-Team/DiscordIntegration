using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.Webhook;
using Discord.WebSocket;

namespace DiscordIntegration_Bot
{
	public class ProcessSTT
	{
		private static ConcurrentDictionary<int, TcpClient> bag = new ConcurrentDictionary<int, TcpClient>();
		private static ConcurrentDictionary<int, int> heartbeats = new ConcurrentDictionary<int, int>();
		public static ulong GameChannelId;
		public static ulong CmdChannelId;
		private static Dictionary<ulong, string> _messages = new Dictionary<ulong, string>();

		public static void SendData(string data, int port, string name, ulong channel = 0)
		{
			try
			{
				BinaryFormatter formatter = new BinaryFormatter();
				SerializedData.SerializedData serializedData =
					new SerializedData.SerializedData { Data = data, Port = port, Channel = channel, Name = name };
				//Console.WriteLine($"Sending {serializedData.Data}");
				if (!bag.ContainsKey(port))
				{
					Console.WriteLine($"STT: Bag does not contain {port}");
					return;
				}

				if (bag[port] != null && bag[port].Connected)
					formatter.Serialize(bag[port].GetStream(), serializedData);
				else
				{
					Console.WriteLine($"Error - Bag {port} is null or not connected.");
					if (bag.TryRemove(port, out TcpClient client))
						client.Dispose();
				}
			}
			catch (IOException s)
			{
				Console.WriteLine($"STT: Socket exception, removing..");
				KeyValuePair<int, TcpClient> thingything = default;
				foreach (var thing in bag)
					if (thing.Key == port)
						thingything = thing;

				if (bag.TryRemove(thingything.Key, out TcpClient _client))
				{
					_client.Close();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		private static List<TcpListener> listener = new List<TcpListener>();
		public static void Init(Program program)
		{
			Bot.Client.SetStatusAsync(UserStatus.DoNotDisturb);
			TcpListener list = new TcpListener(Program.Config.EggMode ? IPAddress.Any : IPAddress.Loopback, Program.Config.Port);
			Program.Log($"STT started for {Program.Config.Port}");
			GameChannelId = Program.Config.GameLogChannelId;
			CmdChannelId = Program.Config.CommandLogChannelId;
			Program.Log("STT: Adding listener to list", true);
			listener.Add(list);
			Program.Log("STT: Starting listener.");
			list.Start();
			Program.Log("STT: Listener started.");
			ThreadPool.QueueUserWorkItem(ListenForConn, list);
			new Thread(DequeueMessages).Start();
		}

		public static async Task Heartbeat(int port)
		{
			await Task.Delay(10000);
			for (;;)
			{
				Console.WriteLine("STT: Starting Heartbeat");
				if (heartbeats[port] > 3)
				{
					Console.WriteLine($"STT: Removing {port} due to heartbeat timeout.");
					await Bot.Client.SetStatusAsync(UserStatus.DoNotDisturb);
					if (bag.TryRemove(port, out TcpClient client))
						client.Close();
					heartbeats.TryRemove(port, out int _);
					return;
				}

				heartbeats[port]++;

				Console.WriteLine($"STT: Sending heartbeat to: {port}");
				if (!bag[port].Connected)
				{
					Console.WriteLine($"STT: {port} is null, removing.");
					await Bot.Client.SetStatusAsync(UserStatus.DoNotDisturb);
					if (bag.TryRemove(port, out TcpClient client)) 
						client.Close();

					return;
				}
				SendData("ping", port, "bot", 0);
				await Task.Delay(10000);
			}
		}

		public static void ListenForConn(object token)
		{
			Program.Log($"STT: Listener started.", true);
			TcpListener listen = token as TcpListener;
				for (;;)
				{
					try
					{
						Program.Log($"STT: Attempting to start connection.");
						TcpClient thing = listen.AcceptTcpClient();
						ThreadPool.QueueUserWorkItem(ListenOn, thing);
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
					}
			}
		}

		public static async Task ReceiveData(SerializedData.SerializedData data, TcpClient client)
		{
			try
			{
				if (data == null)
				{
					Console.WriteLine("STT: Received data null");
					return;
				}
				
				Program.Log($"Receiving data: {data.Data} Channel: {data.Channel} for {data.Port}", true);
				if (data.Data.Contains("REQUEST_DATA PLAYER_LIST SILENT"))
					return;
				SocketGuild guild = Bot.Client.Guilds.FirstOrDefault();

				if (data.Data.StartsWith("checksync"))
				{
					string[] args = data.Data.Split(' ');
					Program.Log($"Checking rolesync for {args[1]}", true);
					SyncedUser user = Program.Users.FirstOrDefault(u => u.UserId == args[1]);
					if (user == null)
					{
						Program.Log($"Role sync for {args[1]} not found.", true);
						return;
					}

					foreach (SocketRole role in guild.GetUser(user.DiscordId).Roles)
					{
						if (Program.SyncedGroups.ContainsKey(role.Id))
							SendData($"setgroup {user.UserId} {Program.SyncedGroups[role.Id]}", Program.Config.Port, "RoleSync");
					}
				}
				if (data.Data == "ping")
				{
					if (!bag.ContainsKey(data.Port))
					{
						Console.WriteLine($"STT: Adding {data.Port}");
						bag.TryAdd(data.Port, client);
					}

					if (!bag[data.Port].Connected || bag[data.Port] == null)
					{
						Console.WriteLine($"STT: Bag {data.Port} not connected or null, removing.");
						if (bag.TryRemove(data.Port, out TcpClient cli))
						{
							cli?.Close();
						}
					}
					Console.WriteLine($"STT: Received heartbeat for: {data.Port}");
					if (!heartbeats.ContainsKey(data.Port))
					{
						Heartbeat(data.Port);
						heartbeats.TryAdd(data.Port, 0);
					}
					else
						heartbeats[data.Port]--;

					Program.Log($"Updating channelID's for plugin..{data.Port}", true);
					try
					{
						Program.Log($"GameChannelID: {GameChannelId}", true);
						SendData("set gameid", data.Port, "bot", GameChannelId);
						Program.Log($"CommandChannelID: {CmdChannelId}", true);
						SendData("set cmdid", data.Port, "bot", CmdChannelId);
					}
					catch (Exception e)
					{
						Program.Log(e.ToString());
					}

					return;
				}

				if (data.Data.StartsWith("channelstatus"))
				{
					Program.Log($"updating channel topic", true);
					string status = data.Data.Replace("channelstatus", "");
					SocketTextChannel chan1 = guild.GetTextChannel(GameChannelId);
					await chan1.ModifyAsync(x => x.Topic = status);
					SocketTextChannel chan2 = guild.GetTextChannel(CmdChannelId);
					await chan2.ModifyAsync(x => x.Topic = status);

					return;
				}

				if (data.Data.StartsWith("updateStatus"))
				{
					Program.Log($"updating status for bot", true);
					string status = data.Data.Replace("updateStatus ", "");
					if (status.StartsWith("0"))
						await Bot.Client.SetStatusAsync(UserStatus.Idle);
					else
						await Bot.Client.SetStatusAsync(UserStatus.Online);
					await Bot.Client.SetActivityAsync(new Game(status));
					return;
				}
				data.Data = data.Data.Substring(data.Data.IndexOf('#') + 1);

				
				
				Console.WriteLine("Getting guild.");
				Console.WriteLine("Getting channel");
				if (guild == null)
				{
					Console.WriteLine("Guild not found.");
					return;
				}

				SocketTextChannel chan = null;
				if (data.Channel == 1)
					chan = guild.GetTextChannel(GameChannelId);
				else if (data.Channel == 2)
					chan = guild.GetTextChannel(CmdChannelId);
				else
					chan = guild.GetTextChannel(data.Channel);
				
				if (chan == null)
				{
					await Program.Log(new LogMessage(LogSeverity.Critical, "recievedData", "Channel not found."));
					return;
				}

				if (chan.Id == Program.Config.GameLogChannelId || chan.Id == Program.Config.CommandLogChannelId)
				{
					Program.Log("Storing message.", true);
					lock (_messages)
					{
						if (!_messages.ContainsKey(chan.Id))
							_messages.Add(chan.Id, string.Empty);
						_messages[chan.Id] += $"[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {data.Data} {Environment.NewLine}";
					}
					return;
				}
				Console.WriteLine("Sending message.");
				await chan.SendMessageAsync($"[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {data.Data}");
				
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

		}

		private static void DequeueMessages()
		{
			for (;;)
			{
				Program.Log("For loop start", true);

				lock (_messages)
				{
					foreach (KeyValuePair<ulong, string> kvp in _messages)
					{
						Program.Log($"Sending messages.{_messages[kvp.Key]}", true);
						SocketTextChannel chan = Bot.Client.Guilds.FirstOrDefault()?.GetTextChannel(kvp.Key);
						if (chan == null)
						{
							Program.Log(new LogMessage(LogSeverity.Critical, "DequeueSend", "Channel not found!"));
							continue;
						}

						chan.SendMessageAsync(kvp.Value);
					}

					_messages.Clear();
				}

				Thread.Sleep(3000);
			}
		}

		public static void ListenOn(object token)
		{
			TcpClient client = token as TcpClient;
			try
			{
				BinaryFormatter formatter = new BinaryFormatter();
				for (;;)
				{
					SerializedData.SerializedData serializedData;
					if (!client.Connected)
					{
						Console.WriteLine($"Client not connected..");
						client.Close();
						continue;
					}
					
					serializedData = formatter.Deserialize(client.GetStream()) as SerializedData.SerializedData;
					ReceiveData(serializedData, client);
				}
			}
			catch (SerializationException s)
			{
				Console.WriteLine($"STT: Serialization exception, removing..");
				KeyValuePair<int, TcpClient> thingything = default;
				foreach (var thing in bag)
					if (thing.Value == client)
						thingything = thing;

				if (bag.TryRemove(thingything.Key, out TcpClient _client))
				{
					_client.Close();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}