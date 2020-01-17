using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
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
			TcpListener list = new TcpListener(IPAddress.Loopback, program.Config.Port);
			Console.WriteLine($"STT: Listener started for port {program.Config.Port}");
			GameChannelId = program.Config.GameLogChannelId;
			CmdChannelId = program.Config.CommandLogChannelId;
			listener.Add(list);
			list.Start();
			ThreadPool.QueueUserWorkItem(ListenForConn, list); 
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
			Console.WriteLine("STT: Listener started.");
			TcpListener listen = token as TcpListener;
				for (;;)
				{
					try
					{
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

				if (data.Data.Contains("REQUEST_DATA PLAYER_LIST SILENT"))
					return;

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
					SendData("set gameid", data.Port, "bot", GameChannelId);
					SendData("set cmdid",data.Port, "bot", CmdChannelId);
					return;
				}

				Console.WriteLine(data.Data);
				data.Data = data.Data.Substring(data.Data.IndexOf('#') + 1);
				if (data.Data.Contains("0/25"))
					return;

				Console.WriteLine("Getting guild.");
				SocketGuild guild = Bot.Client.Guilds.FirstOrDefault();
				Console.WriteLine("Getting channel");
				if (guild == null)
				{
					Console.WriteLine("Guild not found.");
					return;
				}
				SocketTextChannel chan = guild.GetTextChannel(data.Channel);
				Console.WriteLine("Sending message.");
				await chan.SendMessageAsync($"[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {data.Data}");
				
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
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
					new Thread(() => ReceiveData(serializedData, client)).Start();
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