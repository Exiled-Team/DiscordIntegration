using Discord;
using Discord.WebSocket;
using DiscordIntegration_Bot.Logging;
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

namespace DiscordIntegration_Bot
{
    public class ProcessSTT
    {
        private static ConcurrentDictionary<int, TcpClient> bag = new ConcurrentDictionary<int, TcpClient>();
        private static ConcurrentDictionary<int, int> heartbeats = new ConcurrentDictionary<int, int>();
        public static ulong GameChannelId;
        public static ulong CmdChannelId;
        public static Config Config = Program.GetConfig();
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
                    Logger.LogError("SendData", $"STT: Bag does not contain {port}");
                    return;
                }

                if (bag[port] != null && bag[port].Connected)
                    formatter.Serialize(bag[port].GetStream(), serializedData);
                else
                {
                    Logger.LogError("SendData", $"Error - Bag {port} is null or not connected.");
                    if (bag.TryRemove(port, out TcpClient client))
                        client.Dispose();
                }
            }
            catch (IOException s)
            {
                Logger.LogException("SendData", s, $"STT: Socket exception, removing..");
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
                Logger.LogException("SendData", e);
            }
        }

        private static List<TcpListener> listener = new List<TcpListener>();
        public static void Init(Program program)
        {
            Bot.Client.SetStatusAsync(UserStatus.DoNotDisturb);
            TcpListener list = new TcpListener(Program.Config.EggMode ? IPAddress.Any : IPAddress.Loopback, Program.Config.Port);
            Logger.LogInfo("DiscordIntegration", $"STT started for {Program.Config.Port}");
            GameChannelId = Program.Config.GameLogChannelId;
            CmdChannelId = Program.Config.CommandLogChannelId;
            Logger.LogInfo("Bot", $"STT: Adding listener to list");
            listener.Add(list);
            Logger.LogInfo("Bot", "STT: Starting listener...");
            list.Start();
            Logger.LogInfo("Bot", "STT: Listener started.");
            ThreadPool.QueueUserWorkItem(ListenForConn, list);
            new Thread(DequeueMessages).Start();
        }

        public static async Task Heartbeat(int port)
        {
            await Task.Delay(10000);
            for (; ; )
            {
                Logger.LogInfo("Bot", "STT: Starting Heartbeat");
                if (heartbeats[port] > 3)
                {
                    Logger.LogInfo("DiscordIntegration", $"STT: Removing {port} due to heartbeat timeout.");
                    await Bot.Client.SetStatusAsync(UserStatus.DoNotDisturb);
                    if (bag.TryRemove(port, out TcpClient client))
                        client.Close();
                    heartbeats.TryRemove(port, out int _);
                    return;
                }

                heartbeats[port]++;

                Logger.LogInfo("Bot", $"STT: Sending heartbeat to: {port}");
                if (!bag[port].Connected)
                {
                    Logger.LogError("Bot", $"STT: {port} is null, removing.");
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
            Logger.LogInfo("Bot", "STT: Listener started.");

            TcpListener listen = token as TcpListener;
            for (; ; )
            {
                try
                {
                    Logger.LogInfo("DiscordIntegration", $"STT: Attempting to start connection.");
                    TcpClient thing = listen.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(ListenOn, thing);
                }
                catch (Exception e)
                {
                    Logger.LogException("ListenForConn", e);
                }
            }
        }

        public static async Task ReceiveData(SerializedData.SerializedData data, TcpClient client)
        {
            try
            {
                if (data == null)
                {
                    Logger.LogError("DiscordIntegration", "STT: Received data null");
                    return;
                }
                if (Config.Default.Debug == true)
                    Program.Log($"Receiving data: {data.Data} Channel: {data.Channel} for {data.Port}", true);
                if (data.Data.Contains("REQUEST_DATA PLAYER_LIST SILENT"))
                    return;
                //Guild Stuff
                // The last discord server where the bot entered
                SocketGuild guild = Bot.Client.Guilds.LastOrDefault();

                if (data.Data.StartsWith("checksync"))
                {
                    string[] args = data.Data.Split(' ');
                    Logger.LogDebug("Debug", $"Checking rolesync for {args[1]}");
                    Program.Log($"Checking rolesync for {args[1]}\n", false);
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
                        Logger.LogInfo("Bot", $"STT: Adding {data.Port}");
                        bag.TryAdd(data.Port, client);
                    }

                    if (!bag[data.Port].Connected || bag[data.Port] == null)
                    {
                        Logger.LogInfo("Bot", $"STT: Bag {data.Port} not connected or null, removing.");
                        if (bag.TryRemove(data.Port, out TcpClient cli))
                        {
                            cli?.Close();
                        }
                    }
                    Logger.LogInfo("Bot", $"STT: Received heartbeat for: {data.Port}");
                    if (!heartbeats.ContainsKey(data.Port))
                    {
                        Heartbeat(data.Port);
                        heartbeats.TryAdd(data.Port, 0);
                    }
                    else
                        heartbeats[data.Port]--;

                    if (Config.Debug)
                    {
                        Logger.LogDebug("DI", $"Updating channelID's for plugin..{data.Port}");
                    }
                    try
                    {
                        Program.Log($"GameChannelID: {GameChannelId}", true);
                        SendData("set gameid", data.Port, "bot", GameChannelId);
                        Program.Log($"CommandChannelID: {CmdChannelId}", true);
                        SendData("set cmdid", data.Port, "bot", CmdChannelId);
                    }
                    catch (Exception e)
                    {
                        Logger.LogException("ReceiveData", e);
                    }

                    return;
                }

                if (data.Data.StartsWith("channelstatus"))
                {

                    if (Config.Debug)
                    {
                        Program.Log($"updating channel topic", true);
                    }

                    /*string status = data.Data.Replace("channelstatus", "");
					SocketTextChannel chan1 = guild.GetTextChannel(GameChannelId);
					await chan1.ModifyAsync(x => x.Topic = status);
					SocketTextChannel chan2 = guild.GetTextChannel(CmdChannelId);
					await chan2.ModifyAsync(x => x.Topic = status);*/


                    return;
                }

                if (data.Data.StartsWith("updateStatus"))
                {
                    if (Config.Debug)
                    {
                        Logger.LogDebug("DI", $"updating status for bot");
                    }

                    string status = data.Data.Replace("updateStatus ", "");
                    if (status.StartsWith("0"))
                        await Bot.Client.SetStatusAsync(UserStatus.Idle);
                    else
                        await Bot.Client.SetStatusAsync(UserStatus.Online);
                    await Bot.Client.SetActivityAsync(new Game(status));
                    return;
                }
                data.Data = data.Data.Substring(data.Data.IndexOf('#') + 1);



                Logger.LogInfo("DiscordIntegration", "Getting guild.");
                Logger.LogInfo("DiscordIntegration", "Getting channel.");
                if (guild == null)
                {
                    Logger.LogError("Bot", "Guild not found.");
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
                    Logger.LogCritical("recievedData", "Channel not found.");
                    return;
                }

                if (chan.Id == Program.Config.GameLogChannelId || chan.Id == Program.Config.CommandLogChannelId)
                {
                    if (Config.Debug)
                    {
                        Logger.LogDebug("Bot", "Storing message.");
                    }
                    lock (_messages)
                    {
                        if (!_messages.ContainsKey(chan.Id))
                            _messages.Add(chan.Id, string.Empty);
                        _messages[chan.Id] += $"[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {data.Data} {Environment.NewLine}".Replace("@everyone", "@​everyone").Replace("@here", "@​here");
                    }
                    return;
                }
                Logger.LogDebug("Bot", "Sending message.");
                await chan.SendMessageAsync($"[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {data.Data}".Replace("@everyone", "@​everyone").Replace("@here", "@​here"));

            }
            catch (Exception e)
            {
                Logger.LogException("ReceiveData", e);
            }

        }

        private static void DequeueMessages()
        {
            for (; ; )
            {
                if (Config.Debug)
                {
                    Logger.LogDebug("Bot", "For loop start");
                }

                lock (_messages)
                {
                    foreach (KeyValuePair<ulong, string> kvp in _messages)
                    {
                        Program.Log($"Sending messages.{_messages[kvp.Key]}", true);
                        SocketTextChannel chan = Bot.Client.Guilds.LastOrDefault()?.GetTextChannel(kvp.Key);
                        if (chan == null)
                        {
                            Logger.LogCritical("DequeueSend", "Channel not found!");
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
                for (; ; )
                {
                    SerializedData.SerializedData serializedData;
                    if (!client.Connected)
                    {
                        Logger.LogError("Bot", $"Client not connected..");
                        client.Close();
                        continue;
                    }

                    serializedData = formatter.Deserialize(client.GetStream()) as SerializedData.SerializedData;
                    ReceiveData(serializedData, client);
                }
            }
            catch (SerializationException s)
            {
                Logger.LogError("Bot", $"STT: Serialization exception, removing..");
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
                Logger.LogException("SerializationException", e);
            }
        }
    }
}