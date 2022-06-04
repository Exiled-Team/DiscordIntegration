namespace DiscordIntegration.Bot;

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

    public ushort Port { get; }
    public DiscordSocketClient Client => client ??= new DiscordSocketClient();
    public SocketGuild Guild => guild ??= Client.GetGuild(Program.Config.DiscordServerIds[Port]);
    public InteractionService InteractionService { get; private set; } = null!;
    public SlashCommandHandler CommandHandler { get; private set; } = null!;
    public TcpServer Server { get; private set; } = null!;

    public Bot(ushort port, string token)
    {
        Port = port;
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
            Log.Error($"[{Port}] {nameof(Init)}", e);
            return;
        }

        Log.Debug($"[{Port}] {nameof(Init)}", "Setting up commands...");
        InteractionService = new(Client, new InteractionServiceConfig()
        {
            AutoServiceScopes = false,
        });
        CommandHandler = new(InteractionService, Client, this);

        Log.Debug($"[{Port}] {nameof(Init)}", "Setting up logging..");
        InteractionService.Log += Log.Send;
        Client.Log += Log.Send;

        Log.Debug($"[{Port}] {nameof(Init)}", "Registering commands..");
        await CommandHandler.InstallCommandsAsync();
        Client.Ready += async () =>
        {
            int slashCommands = (await InteractionService.RegisterCommandsToGuildAsync(Guild.Id)).Count;

            Log.Debug($"[{Port}] {nameof(Init)}", $"{slashCommands} slash commands registered.");
        };

        Log.Debug($"[{Port}] {nameof(Init)}", "Logging in..");
        await Client.LoginAsync(TokenType.Bot, token);
        await Client.StartAsync();

        Log.Debug($"[{Port}] {nameof(Init)}", "Login successful.");

        Server = new TcpServer(Program.Config.TcpServers[Port].IpAddress, Program.Config.TcpServers[Port].Port, TimeSpan.FromSeconds(5), this);
        _ = Server.Start();
        Server.ReceivedFull += OnReceived;
        await DequeueMessages();
        await Task.Delay(-1);
    }

    private async void OnReceived(object? sender, ReceivedFullEventArgs ev)
    {
        try
        {
            Log.Debug($"[{Port}]", $"Received data {ev.Data}");
            RemoteCommand command = JsonConvert.DeserializeObject<RemoteCommand>(ev.Data)!;
            Log.Debug($"[{Port}]", $"Received command {command.Action}.");

            switch (command.Action)
            {
                case ActionType.Log:
                    if (Enum.TryParse(command.Parameters[0].ToString(), true, out ChannelType type))
                    {
                        foreach (ulong channelId in Program.Config.Channels[Port].Logs[type])
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
                        await Guild.GetTextChannel(chanId).SendMessageAsync(embed: await EmbedBuilderService.CreateBasicEmbed(split[0].TrimEnd('|'), split[1].TrimStart('|'), (bool)command.Parameters[2] ? Color.Green : Color.Red));
                    }

                    break;
                case ActionType.UpdateActivity:
                    if (int.TryParse(((string) command.Parameters[0]).AsSpan(0, 1), out int count) && count > 0)
                        await Client.SetStatusAsync(UserStatus.Online);
                    else
                        await Client.SetStatusAsync(UserStatus.AFK);

                    await Client.SetActivityAsync(new Game(command.Parameters[0].ToString()));

                    break;
            }
        }
        catch (Exception e)
        {
            Log.Error(nameof(OnReceived), e.Message);
            
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
                if (message.Value.Length > 1900)
                {
                    string msg = string.Empty;
                    string[] split = message.Value.Split('\n');
                    while (msg.Length < 1900)
                        msg += split;
                    _ = Guild.GetTextChannel(message.Key).SendMessageAsync(embed: await EmbedBuilderService.CreateBasicEmbed("Server Logs", msg, Color.Green));
                    messages.Add(message.Key, message.Value.Replace(msg, string.Empty));
                }
                else
                    _ = Guild.GetTextChannel(message.Key).SendMessageAsync(embed: await EmbedBuilderService.CreateBasicEmbed("Server Logs", message.Value, Color.Green));
            }

            await Task.Delay(Program.Config.MessageDelay);
        }
    }
}