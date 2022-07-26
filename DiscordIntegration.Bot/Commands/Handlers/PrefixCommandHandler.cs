using System.ComponentModel.Design;
using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordIntegration.Bot.Services;
using DiscordIntegration.Dependency;

namespace DiscordIntegration.Bot.Commands.Handlers;

public class PrefixCommandHandler
{
    private readonly InteractionService service;
    private readonly DiscordSocketClient client;
    private readonly ServiceContainer serviceContainer;
    private readonly Bot bot;
    
    public PrefixCommandHandler(InteractionService service, DiscordSocketClient client, Bot bot)
    {
        this.service = service;
        this.client = client;
        this.bot = bot;
        serviceContainer = new();
        serviceContainer.AddService(typeof(Bot), bot);
    }

    public async Task InstallCommandsAsync()
    {
        await service.AddModulesAsync(Assembly.GetEntryAssembly(), serviceContainer);
        client.MessageReceived += OnMessageReceived;
    }

    private async Task OnMessageReceived(SocketMessage socketMessage)
    {
        if (socketMessage is not SocketUserMessage message || message.Author.IsBot)
            return;

        int argPos = 0;
        
        if (!message.HasStringPrefix(Config.Default.BotPrefixes[Config.Default.BotIds[client.CurrentUser.Id]], ref argPos))
            return;

        string command = socketMessage.Content[argPos..];
        
        ErrorCodes errorCode = CanRunCommand((IGuildUser) socketMessage.Author, Config.Default.BotIds[client.CurrentUser.Id], command);
        
        if (errorCode != ErrorCodes.None)
        {
            await socketMessage.Channel.SendMessageAsync(embed: await ErrorHandlingService.GetErrorEmbed(errorCode));
            return;
        }
        
        try
        {
            Log.Debug(nameof(OnMessageReceived), $"Sending {command}");
            await bot.Server.SendAsync(new RemoteCommand(Dependency.ActionType.ExecuteCommand, socketMessage.Channel.Id, command, socketMessage.Id, socketMessage.Author.Username));
        }
        catch (Exception e)
        {
            Log.Error(nameof(OnMessageReceived), e);
            await socketMessage.Channel.SendMessageAsync(embed: await ErrorHandlingService.GetErrorEmbed(ErrorCodes.Unspecified, e.Message));
        }
    }
    
    private ErrorCodes CanRunCommand(IGuildUser user, ushort serverNum, string command)
    {
        if (!Program.Config.ValidCommands.ContainsKey(serverNum) || Program.Config.ValidCommands[serverNum].Count == 0)
            return ErrorCodes.InvalidCommand;

        foreach (KeyValuePair<ulong, List<string>> commandList in Program.Config.ValidCommands[serverNum])
        {
            if (!commandList.Value.Contains(command) && !commandList.Value.Any(command.StartsWith))
                return ErrorCodes.InvalidCommand;
            if (user.Hierarchy >= user.Guild.GetRole(commandList.Key)?.Position)
                return ErrorCodes.None;
        }

        return ErrorCodes.PermissionDenied;
    }
}