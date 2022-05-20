using System;

namespace DiscordIntegration.Bot.Services;

using System.Net;
using System.Net.Sockets;
using System.Text;
using Dependency;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ActionType = Dependency.ActionType;
using ChannelType = Dependency.ChannelType;

public class TcpServer
{
    private readonly Bot bot;

    public JsonSerializerSettings JsonSerializerSettings { get; } = new JsonSerializerSettings
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        TypeNameHandling = TypeNameHandling.Objects,
    };

    public Dictionary<ulong, List<string>> _messages { get; } = new();

    public TcpClient Client { get; private set; } = null!;
    public TcpListener Listener { get; private set; } = null!;

    public TcpServer(Bot bot) => this.bot = bot;

    public async Task StartTcpServer(CancellationToken cancellationToken)
    {
        await bot.Client.SetStatusAsync(UserStatus.DoNotDisturb);
        Listener =
            new(
                Program.Config.TcpServers[bot.Port].IpAddress == "127.0.0.1" ? IPAddress.Loopback :
                Program.Config.TcpServers[bot.Port].IpAddress == "0.0.0.0" ? IPAddress.Any :
                IPAddress.Parse(Program.Config.TcpServers[bot.Port].IpAddress),
                Program.Config.TcpServers[bot.Port].Port);

        Listener.Start();
        await ListenForConnection(cancellationToken);
        new Thread(DequeueMessages).Start();
    }

    private async Task ListenForConnection(CancellationToken cancellationToken)
    {
        for (;;)
        {
            Client = Listener.AcceptTcpClient();
            await ReceiveAsync(cancellationToken);
        }
    }

    private async Task ReceiveAsync(CancellationToken cancellationToken)
    {
        byte[] buffer = new byte[265];

        for (;;)
        {
            Task<int> readTask = Client.GetStream().ReadAsync(buffer, 0, buffer.Length, cancellationToken);

            await Task.WhenAny(readTask, Task.Delay(Timeout.Infinite, cancellationToken));

            cancellationToken.ThrowIfCancellationRequested();

            int bytesRead = await readTask;

            if (bytesRead > 0)
            {
                string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                RemoteCommand command = JsonConvert.DeserializeObject<RemoteCommand>(receivedData, JsonSerializerSettings)!;

                PreformAction(command);
            }
        }
    }

    private async Task PreformAction(RemoteCommand command)
    {
        List<ulong> channels = new();
        switch (command.Action)
        {
            case ActionType.Log:
                if (command.Parameters[0] is ChannelType type)
                {
                    switch (type)
                    {
                        case ChannelType.Command:
                            foreach (ulong channelId in Program.Config.Channels[bot.Port].Logs.Commands)
                                channels.Add(channelId);
                            break;
                        case ChannelType.GameEvents:
                            foreach (ulong channelId in Program.Config.Channels[bot.Port].Logs.GameEvents)
                                channels.Add(channelId);
                            break;
                        case ChannelType.Bans:
                            foreach (ulong channelId in Program.Config.Channels[bot.Port].Logs.Bans)
                                channels.Add(channelId);
                            break;
                        case ChannelType.Reports:
                            foreach (ulong channelId in Program.Config.Channels[bot.Port].Logs.Reports)
                                channels.Add(channelId);
                            break;
                        case ChannelType.StaffCopy:
                            foreach (ulong channelId in Program.Config.Channels[bot.Port].Logs.StaffCopy)
                                channels.Add(channelId);
                            break;
                    }

                    foreach (ulong channelId in channels)
                    {
                        if (!_messages.ContainsKey(channelId))
                            _messages.Add(channelId, new());
                        _messages[channelId].Add(string.Join(' ', command.Parameters.Skip(1)));
                    }
                }

                break;
            case ActionType.SendMessage:
                if (ulong.TryParse((string) command.Parameters[0], out ulong chanId))
                {
                    string message = $"[{DateTime.Now}] {string.Join(' ', command.Parameters.Skip(1))}";
                    bot.Guild.GetTextChannel(chanId).SendMessageAsync(message.Replace("@", "`$&`"));
                }

                break;
            case ActionType.UpdateActivity:
                string activity = (string)command.Parameters[0];
                if (int.TryParse(activity.AsSpan(0, 1), out int count) && count > 0)
                    await bot.Client.SetStatusAsync(UserStatus.Online);
                else
                    await bot.Client.SetStatusAsync(UserStatus.AFK);

                await bot.Client.SetActivityAsync(new Game((string) command.Parameters[0]));
                break;
            case ActionType.AddUser:
            case ActionType.AddRole:
            case ActionType.LoadConfig:
            case ActionType.LoadSyncedRole:
            case ActionType.RemoveRole:
            case ActionType.RemoveUser:
            case ActionType.UpdateChannelActivity:
                foreach (ulong channelId in Program.Config.Channels[bot.Port].TopicInfo)
                {
                    await bot.Guild.GetTextChannel(channelId)
                        .ModifyAsync(x => x.Topic = (string) command.Parameters[0]);
                }

                break;
            default:
                throw new NotImplementedException();
        }
    }

    public async Task SendCommand(RemoteCommand command)
    {
        switch (command.Action)
        {
            case ActionType.ExecuteCommand:
                string serializedObject = JsonConvert.SerializeObject(command, JsonSerializerSettings);
                byte[] bytesToSend = Encoding.UTF8.GetBytes(serializedObject + '\0');
                await Client.GetStream().WriteAsync(bytesToSend, CancellationToken.None);
                
                break;
            case ActionType.SetGroupFromId:
                break;
            case ActionType.CommandReply:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void DequeueMessages()
    {
        for (;;)
        {
            lock (_messages)
            {
                foreach (KeyValuePair<ulong, List<string>> kvp in _messages)
                {
                    SocketTextChannel channel = bot.Guild.GetTextChannel(kvp.Key);
                    if (channel is null)
                    {
                        Log.Warn($"[{bot.Port}] {nameof(DequeueMessages)}", $"Channel {kvp.Key} not found in guild  {bot.Guild.Name} ({bot.Guild.Id})");
                        continue;
                    }

                    foreach (string message in kvp.Value)
                        channel.SendMessageAsync(message);
                }

                _messages.Clear();
            }

            Thread.Sleep(Program.Config.MessageDelay);
        }
    }
}