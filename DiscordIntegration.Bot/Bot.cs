namespace DiscordIntegration.Bot;

using System.Net;
using System.Net.Sockets;
using Commands;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Services;

public class Bot
{
    private DiscordSocketClient? client;
    private SocketGuild? guild;
    private string token;
    
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
        InteractionService = new(Client);
        CommandHandler = new(InteractionService, Client);

        Log.Debug($"[{Port}] {nameof(Init)}", "Setting up logging..");
        InteractionService.Log += Log.Send;
        Client.Log += Log.Send;

        Log.Debug($"[{Port}] {nameof(Init)}", "Registering commands..");
        Client.Ready += async () =>
        {
            int slashCommands = (await InteractionService.RegisterCommandsToGuildAsync(Guild.Id)).Count;

            Log.Debug($"[{Port}] {nameof(Init)}", $"{slashCommands} slash commands registered.");
        };

        Log.Debug($"[{Port}] {nameof(Init)}", "Logging in..");
        await Client.LoginAsync(TokenType.Bot, token);
        await Client.StartAsync();

        Log.Debug($"[{Port}] {nameof(Init)}", "Login successful.");

        Server = new TcpServer(this);
        await Server.StartTcpServer(CancellationToken.None);
        await Task.Delay(-1);
    }
}