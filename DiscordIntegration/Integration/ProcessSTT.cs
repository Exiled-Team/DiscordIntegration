using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

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
				Plugin.Debug($"STT: Starting INIT.");
				tcpClient?.Close();
				tcpClient = new TcpClient();
				while (!tcpClient.Connected)
				{
					Plugin.Debug($"STT: While loop start");
					Thread.Sleep(2000);
					try
					{
						tcpClient.Connect("127.0.0.1", ServerConsole.Port);
					}
					catch (SocketException)
					{
						tcpClient.Client.Disconnect(false);
					}
					catch (Exception e)
					{
						Plugin.Error($"STT: {e}");
					}
				}

				Thread thread = new Thread(ReceiveData);
				thread.Start();
				SendData("ping", 0);
				_locked = false;
			}
			catch (IOException)
			{
				_init = new Thread(Init);
				_locked = false;
				_init.Start();
			}
			catch (Exception e)
			{
				_locked = false;
				Plugin.Error(e.ToString());
			}
		}

		public static void SendData(string data, ulong channel)
		{
			try
			{
				if (!tcpClient.Connected)
					return;

				SerializedData.SerializedData serializedData = new SerializedData.SerializedData
				{
					Data = data, Port = ServerConsole.Port, Channel = channel
				};
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(tcpClient.GetStream(), serializedData);
				Plugin.Debug($"Sent {data}");
			}
			catch (IOException)
			{
				_init = new Thread(Init);
				_init.Start();
			}
			catch (Exception e)
			{
				Plugin.Error(e.ToString());
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
			catch (SerializationException)
			{
				_init = new Thread(Init);
				_init.Start();
			}
			catch (IOException)
			{
				_init = new Thread(Init);
				_init.Start();
			}
			catch (Exception e)
			{
				Plugin.Error(e.ToString());
			}
		}
	}
}