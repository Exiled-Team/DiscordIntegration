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
using Newtonsoft.Json;
using Services;
using ActionType = Dependency.ActionType;
using ChannelType = Dependency.ChannelType;

public class Bot
{
    private DiscordSocketClient? client;
    private SocketGuild? guild;
    private string token;
    private Dictionary<ulong, string> messages = new();
    private int lastCount;

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
        Init().GetAwaiter().GetResult();
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
            Log.Error($"[{ServerNumber}] {nameof(Init)}", e);
            return;
        }

        Log.Debug($"[{ServerNumber}] {nameof(Init)}", "Setting up commands...");
        InteractionService = new(Client, new InteractionServiceConfig()
        {
            AutoServiceScopes = false,
        });
        CommandHandler = new(InteractionService, Client, this);

        Log.Debug($"[{ServerNumber}] {nameof(Init)}", "Setting up logging..");
        InteractionService.Log += Log.Send;
        Client.Log += Log.Send;

        Log.Debug($"[{ServerNumber}] {nameof(Init)}", "Registering commands..");
        await CommandHandler.InstallCommandsAsync();
        Client.Ready += async () =>
        {
            int slashCommands = (await InteractionService.RegisterCommandsToGuildAsync(Guild.Id)).Count;

            Log.Debug($"[{ServerNumber}] {nameof(Init)}", $"{slashCommands} slash commands registered.");
        };

        Log.Debug($"[{ServerNumber}] {nameof(Init)}", "Logging in..");
        await Client.LoginAsync(TokenType.Bot, token);
        await Client.StartAsync();

        Log.Debug($"[{ServerNumber}] {nameof(Init)}", "Login successful.");

        Server = new TcpServer(Program.Config.TcpServers[ServerNumber].IpAddress, Program.Config.TcpServers[ServerNumber].Port, TimeSpan.FromSeconds(5), this);
        _ = Server.Start();
        Server.ReceivedFull += OnReceived;
        await DequeueMessages();
        await Task.Delay(-1);
    }

    private async void OnReceived(object? sender, ReceivedFullEventArgs ev)
    {
        try
        {
            Log.Debug($"[{ServerNumber}]", $"Received data {ev.Data}");
            RemoteCommand command = JsonConvert.DeserializeObject<RemoteCommand>(ev.Data)!;
            Log.Debug($"[{ServerNumber}]", $"Received command {command.Action}.");

            switch (command.Action)
            {
                case ActionType.Log:
                    if (Enum.TryParse(command.Parameters[0].ToString(), true, out ChannelType type))
                    {
                        foreach (ulong channelId in Program.Config.Channels[ServerNumber].Logs[type])
                        {
                            if (!messages.ContainsKey(channelId))
                                messages.Add(channelId, string.Empty);
                            messages[channelId] += $"[{DateTime.Now}] {command.Parameters[1]}\n";
                        }
                    }

                    break;
                case ActionType.SendMessage:
                    if (ulong.TryParse(command.Parameters[0].ToString(), out ulong chanId))
                    {
                        string[] split = command.Parameters[1].ToString()!.Split("|");
                        await Guild.GetTextChannel(chanId).SendMessageAsync(embed: await EmbedBuilderService.CreateBasicEmbed(ServerNumber + split[0].TrimEnd('|'), split[1].TrimStart('|'), (bool)command.Parameters[2] ? Color.Green : Color.Red));
                    }

                    break;
                case ActionType.UpdateActivity:
                    if (int.TryParse(((string) command.Parameters[0]).AsSpan(0, 1), out int count))
                    {
                        switch (count)
                        {
                            case > 0 when Client.Status != UserStatus.Online:
                                await Client.SetStatusAsync(UserStatus.Online);
                                break;
                            case 0 when Client.Status != UserStatus.AFK:
                                await Client.SetStatusAsync(UserStatus.AFK);
                                break;
                        }
                    }

                    if (count != lastCount)
                        await Client.SetActivityAsync(new Game(command.Parameters[0].ToString()));
                    lastCount = count;

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
            Log.Error(nameof(OnReceived), e.Message);
            if (e.StackTrace is not null) 
                Log.Error(nameof(OnReceived), e.StackTrace);
        }
    }

    private async Task DequeueMessages()
    {
        for (;;)
        {
            List<KeyValuePair<ulong, string>> toSend = new();
            lock (messages)
            {
                foreach (KeyValuePair<ulong, string> message in messages)
                    toSend.Add(message);

                messages.Clear();
            }

            foreach (KeyValuePair<ulong, string> message in toSend)
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
                            msg += split[i];
                            i++;
                        }

                        _ = Guild.GetTextChannel(message.Key).SendMessageAsync(embed: await EmbedBuilderService.CreateBasicEmbed($"Server {ServerNumber} Logs", msg, Color.Green));
                        messages.Add(message.Key, message.Value.Replace(msg, string.Empty));
                    }
                    else
                        _ = Guild.GetTextChannel(message.Key).SendMessageAsync(embed: await EmbedBuilderService.CreateBasicEmbed($"Server {ServerNumber} Logs", message.Value, Color.Green));
                }
                catch (Exception e)
                {
                    Log.Error(nameof(DequeueMessages), $"{e.Message}\nThis is likely caused because {message.Key} is not a valid channel ID, or an invalid GuildID: {Program.Config.DiscordServerIds[ServerNumber]}. If the GuildID is correct, to avoid this error, disabling the logging of events targeting channels that you've purposefully set to an invalid channel ID.\nEnable debug mode to show the contents of the messages causing this error.");
                    Log.Debug(nameof(DequeueMessages), $"{e.Message}\n{message.Value}");
                }
            }

            await Task.Delay(Program.Config.MessageDelay);
        }
    }
}