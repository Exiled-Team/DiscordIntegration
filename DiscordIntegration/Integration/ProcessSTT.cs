using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Exiled.API.Features;

namespace DiscordIntegration_Plugin
{
	public class ProcessSTT
	{
		private static TcpClient tcpClient; 
		public static readonly ConcurrentQueue<SerializedData.SerializedData> dataQueue = new ConcurrentQueue<SerializedData.SerializedData>();
		private static Thread _init;
		private static bool _locked;
		public static void Init()
		{
			if (_locked)
				return;
			_locked = true;
			Thread.Sleep(1000);
			try
			{
				Log.Info($"STT: Starting INIT for {ServerConsole.Port}");
				tcpClient?.Close();
				tcpClient = new TcpClient();
				while (!tcpClient.Connected)
				{
					Log.Debug($"STT: While loop start");
					Thread.Sleep(2000);
					try
					{
						if (Plugin.Cfg.Egg)
						{
							Log.Debug($"Starting connection for: '{Plugin.Cfg.EggAddress}' on port {ServerConsole.Port}");
							tcpClient.Connect(Plugin.Cfg.EggAddress, ServerConsole.Port);
						}
						else
						{
							Log.Debug($"Starting connection for: '127.0.0.1' on port {ServerConsole.Port}");
							tcpClient.Connect("127.0.0.1", ServerConsole.Port);
						}
					}
					catch (SocketException s)
					{
						tcpClient.Client.Disconnect(false);
						Log.Debug($"Socket Exception on connection: {s}");
					}
					catch (Exception e)
					{
						Log.Error($"STT: {e}");
					}
				}

				Thread thread = new Thread(ReceiveData);
				thread.Start();
				SendData("ping", 0);
				_locked = false;
			}
			catch (IOException io)
			{
				Log.Debug(io.ToString());
				_init = new Thread(Init);
				_locked = false;
				_init.Start();
			}
			catch (SocketException s)
			{
				Log.Debug(s.ToString());
				_init = new Thread(Init);
				_locked = false;
				_init.Start();
			}
			catch (Exception e)
			{
				_locked = false;
				Log.Error(e.ToString());
			}
		}

		public static void SendData(string data, ulong channel)
		{
			try
			{
				if (tcpClient == null || !tcpClient.Connected)
					return;

				SerializedData.SerializedData serializedData = new SerializedData.SerializedData
				{
					Data = data, Port = ServerConsole.Port, Channel = channel
				};
				if (Plugin.Cfg.Egg)
					serializedData = new SerializedData.SerializedData
					{
						Data = data, Port = ServerConsole.Port + 4130, Channel = channel
					};
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(tcpClient.GetStream(), serializedData);
				Log.Debug($"Sent {data}");
			}
			catch (IOException io)
			{
				Log.Debug(io.ToString());
				_init = new Thread(Init);
				_init.Start();
			}
			catch (Exception e)
			{
				Log.Error(e.ToString());
			}
		}

		public static void ReceiveData()
		{
			try
			{
				if (!tcpClient.Connected)
					throw new InvalidOperationException("Tcp Client not connected!");
				BinaryFormatter formatter = new BinaryFormatter();
				for (;;)
				{
					SerializedData.SerializedData deserialize =
						formatter.Deserialize(tcpClient.GetStream()) as SerializedData.SerializedData;
					if (deserialize == null)
						continue;
					dataQueue.Enqueue(deserialize);
				}
			}
			catch (SerializationException s)
			{
				Log.Debug(s.ToString());
				_init = new Thread(Init);
				_init.Start();
			}
			catch (IOException io)
			{
				Log.Debug(io.ToString());
				_init = new Thread(Init);
				_init.Start();
			}
			catch (Exception e)
			{
				Log.Error(e.ToString());
			}
		}
	}
}