using DiscordIntegration.Dependency.Database;

namespace DiscordIntegration.Bot;

using System.Collections.Specialized;
using System.Net;
using System.Net.Sockets;
using API.EventArgs.Network;
using Commands;
using Dependency;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;

using DiscordIntegration.Bot.ConfigObjects;

using Newtonsoft.Json;
using Services;
using ActionType = Dependency.ActionType;
using ChannelType = Dependency.ChannelType;

public class Bot
{
    private DiscordSocketClient? client;
    private SocketGuild? guild;
    private string token;
    private int lastCount = -1;
    private int lastTotal = 0;

    internal readonly Dictionary<LogChannel, string> Messages = new();

    public ushort ServerNumber { get; }
    public DiscordSocketClient Client => client ??= new DiscordSocketClient();
    public SocketGuild Guild => guild ??= Client.GetGuild(Program.Config.DiscordServerIds[ServerNumber]);
    public InteractionService InteractionService { get; private set; } = null!;
    public SlashCommandHandler CommandHandler { get; private set; } = null!;
    public TcpServer Server { get; private set; } = null!;

    public Bot(ushort port, string token)
    {
        ServerNumber = port;
        this.token = token;
        Task.Run(Init);
    }

    public void Destroy() => Client.LogoutAsync();

    private async Task Init()
    {
        try
        {
            TokenUtils.ValidateToken(TokenType.Bot, token);
        }
        catch (Exception e)
        {
            Log.Error(ServerNumber, nameof(Init), e);
            return;
        }

        DatabaseHandler.Init();
        
        Log.Debug(ServerNumber, nameof(Init), "Setting up commands...");
        InteractionService = new(Client, new InteractionServiceConfig()
        {
            AutoServiceScopes = false,
        });
        CommandHandler = new(InteractionService, Client, this);

        Log.Debug(ServerNumber, nameof(Init), "Setting up logging..");
        InteractionService.Log += SendLog;
        Client.Log += SendLog;

        Log.Debug(ServerNumber, nameof(Init), "Registering commands..");
        await CommandHandler.InstallCommandsAsync();
        Client.Ready += async () =>
        {
            int slashCommands = (await InteractionService.RegisterCommandsToGuildAsync(Guild.Id)).Count;

            Log.Debug(ServerNumber, nameof(Init), $"{slashCommands} slash commands registered.");
        };

        Log.Debug(ServerNumber, nameof(Init), "Logging in..");
        await Client.LoginAsync(TokenType.Bot, token);
        await Client.StartAsync();

        Log.Debug(ServerNumber, nameof(Init), "Login successful.");

        Server = new TcpServer(Program.Config.TcpServers[ServerNumber].IpAddress, Program.Config.TcpServers[ServerNumber].Port, this);
        _ = Server.Start();
        Server.ReceivedFull += OnReceived;
        await DequeueMessages();
        await Task.Delay(-1);
    }

    private Task SendLog(LogMessage arg) => Log.Send(ServerNumber, arg);

    private async void OnReceived(object? sender, ReceivedFullEventArgs ev)
    {
        try
        {
            Log.Debug(ServerNumber, nameof(OnReceived), $"Received data {ev.Data}");
            RemoteCommand command = JsonConvert.DeserializeObject<RemoteCommand>(ev.Data)!;
            Log.Debug(ServerNumber, nameof(OnReceived), $"Received command {command.Action}.");

            switch (command.Action)
            {
                case ActionType.Log:
                    Log.Debug(ServerNumber, nameof(OnReceived), $"{Enum.TryParse(command.Parameters[0].ToString(), true, out ChannelType _)}");
                    if (Enum.TryParse(command.Parameters[0].ToString(), true, out ChannelType type))
                    {
                        foreach (LogChannel channel in Program.Config.Channels[ServerNumber].Logs[type])
                        {
                            Log.Debug(ServerNumber, nameof(OnReceived), "Adding message to queue..");
                            if (!Messages.ContainsKey(channel))
                                Messages.Add(channel, string.Empty);
                            Messages[channel] += $"[{DateTime.Now}] {command.Parameters[1]}\n";
                        }

                        break;
                    }

                    Log.Debug(ServerNumber, nameof(OnReceived), "Failed to add message to queue.");
                    break;
                case ActionType.SendMessage:
                    if (ulong.TryParse(command.Parameters[0].ToString(), out ulong chanId))
                    {
                        string[] split = command.Parameters[1].ToString()!.Split("|");
                        await Guild.GetTextChannel(chanId).SendMessageAsync(embed: await EmbedBuilderService.CreateBasicEmbed(ServerNumber + split[0].TrimEnd('|'), split[1].TrimStart('|'), (bool)command.Parameters[2] ? Color.Green : Color.Red));
                    }

                    break;
                case ActionType.UpdateActivity:
                    string commandMessage = string.Empty;
                    foreach (object obj in command.Parameters)
                        commandMessage += (string) obj + " ";
                    Log.Debug(ServerNumber, nameof(OnReceived), $"Updating activity status.. {commandMessage}");
                    try
                    {
                        string[] split = ((string)command.Parameters[0]).Split('/');
                        if (!int.TryParse(split[0], out int count))
                        {
                            Log.Error(ServerNumber, nameof(ActionType.UpdateActivity), $"Error parsing player count {split[0]}");
                            return;
                        }

                        if (!int.TryParse(split[1], out int total))
                        {
                            Log.Error(ServerNumber, nameof(ActionType.UpdateActivity), $"Error parsing player total {split[1]}");
                            return;
                        }
                        
                        switch (count)
                        {
                            case > 0 when Client.Status != UserStatus.Online:
                                await Client.SetStatusAsync(UserStatus.Online);
                                break;
                            case 0 when Client.Status != UserStatus.AFK:
                                await Client.SetStatusAsync(UserStatus.AFK);
                                break;
                        }
                        
                        Log.Debug(ServerNumber, nameof(OnReceived), $"Status message count: {count}");
                        Log.Debug(ServerNumber, nameof(OnReceived), $"Status message total: {total}");
                        if (count != lastCount || total != lastTotal)
                        {
                            lastCount = count;
                            if (total > 0)
                                lastTotal = total;
                            await Client.SetActivityAsync(new Game($"{lastCount}/{lastTotal}"));
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error(ServerNumber, nameof(OnReceived), "Error updating bot status. Enable debug for more information.");
                        Log.Debug(ServerNumber, nameof(OnReceived), e);
                    }

                    break;
                case ActionType.UpdateChannelActivity:
                    foreach (ulong channelId in Program.Config.Channels[ServerNumber].TopicInfo)
                    {
                        SocketTextChannel channel = Guild.GetTextChannel(channelId);
                        if (channel is not null) 
                            await channel.ModifyAsync(x => x.Topic = (string) command.Parameters[0]);
                    }

                    break;
            }
        }
        catch (Exception e)
        {
            Log.Error(ServerNumber, nameof(OnReceived), e.Message);
            if (e.StackTrace is not null) 
                Log.Error(ServerNumber, nameof(OnReceived), e.StackTrace);
        }
    }

    private async Task DequeueMessages()
    {
        for (;;)
        {
            Log.Debug(ServerNumber, nameof(DequeueMessages), "Dequeue loop");
            List<KeyValuePair<LogChannel, string>> toSend = new();
            lock (Messages)
            {
                foreach (KeyValuePair<LogChannel, string> message in Messages)
                    toSend.Add(message);

                Messages.Clear();
            }

            foreach (KeyValuePair<LogChannel, string> message in toSend)
            {
                try
                {
                    if (message.Value.Length > 1900)
                    {
                        string msg = string.Empty;
                        string[] split = message.Value.Split('\n');
                        int i = 0;
                        while (msg.Length < 1900)
                        {
                            msg += split[i] + "\n";
                            i++;
                        }

                        switch (message.Key.LogType)
                        {
                            case LogType.Embed:
                                _ = Guild.GetTextChannel(message.Key.Id).SendMessageAsync(embed: await EmbedBuilderService.CreateBasicEmbed($"Server {ServerNumber} Logs", msg, Color.Green));
                                break;
                            case LogType.Text:
                                _ = Guild.GetTextChannel(message.Key.Id).SendMessageAsync($"[{ServerNumber}]: {msg}");
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        Messages.Add(message.Key, message.Value.Substring(msg.Length));
                    }
                    else
                    {
                        Log.Debug(ServerNumber, nameof(DequeueMessages), $"Sending message to {message.Key.Id}: {message.Key.LogType} -- {message.Value}");
                        switch (message.Key.LogType)
                        {
                            case LogType.Embed:
                                await Guild.GetTextChannel(message.Key.Id).SendMessageAsync(embed: await EmbedBuilderService.CreateBasicEmbed($"Server {ServerNumber} Logs", message.Value, Color.Green));
                                break;
                            case LogType.Text:
                                await Guild.GetTextChannel(message.Key.Id).SendMessageAsync($"[{ServerNumber}]: {message.Value}");
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        Log.Debug(ServerNumber, nameof(DequeueMessages), "Message sent.");
                    }
                }
                catch (Exception e)
                {
                    Log.Error(ServerNumber, nameof(DequeueMessages), $"{e.Message}\nThis is likely caused because {message.Key.Id} is not a valid channel ID, or an invalid GuildID: {Program.Config.DiscordServerIds[ServerNumber]}. If the GuildID is correct, to avoid this error, disabling the logging of events targeting channels that you've purposefully set to an invalid channel ID.\nEnable debug mode to show the contents of the messages causing this error.");
                    Log.Debug(ServerNumber, nameof(DequeueMessages), $"{e.Message}\n{message.Value}");
                }
            }

            Log.Debug(ServerNumber, nameof(DequeueMessages), $"Waiting {Program.Config.MessageDelay} ms");
            await Task.Delay(Program.Config.MessageDelay);
        }
    }
}
