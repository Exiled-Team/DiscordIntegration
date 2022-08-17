namespace DiscordIntegration.Bot.Commands;

using System.ComponentModel.Design;
using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Services;

public class SlashCommandHandler
{
    private readonly InteractionService service;
    private readonly DiscordSocketClient client;
    private readonly ServiceContainer serviceContainer;
    private readonly Bot bot;

    public SlashCommandHandler(InteractionService service, DiscordSocketClient client, Bot bot)
    {
        this.service = service;
        this.client = client;
        serviceContainer = new();
        serviceContainer.AddService(typeof(Bot), bot);
        this.bot = bot;
    }

    public async Task InstallCommandsAsync()
    {
        await service.AddModulesAsync(Assembly.GetEntryAssembly(), serviceContainer);
        client.InteractionCreated += HandleInteraction;
        service.SlashCommandExecuted += HandleSlashCommand;
        service.ContextCommandExecuted += HandleContextCommand;
        service.ComponentCommandExecuted += HandleComponentCommand;
    }

    private async Task HandleInteraction(SocketInteraction interaction)
    {
        try
        {
            SocketInteractionContext context = new(client, interaction);
            await service.ExecuteCommandAsync(context, serviceContainer);
            Log.Info(bot.ServerNumber, nameof(HandleInteraction), $"{interaction.User.Username} used an interaction.");
        }
        catch (Exception e)
        {
            Log.Error(bot.ServerNumber, nameof(HandleInteraction), e);
            if (interaction.Type == InteractionType.ApplicationCommand)
                await interaction.RespondAsync(
                    embed: await ErrorHandlingService.GetErrorEmbed(ErrorCodes.Unspecified, e.Message));
        }
    }

    private async Task HandleServiceError(IInteractionContext context, IResult result)
    {
        if (!result.IsSuccess)
        {
            if (result.Error == InteractionCommandError.UnknownCommand)
                return;

            await context.Interaction.RespondAsync(
                embed: await ErrorHandlingService.GetErrorEmbed(ErrorCodes.Unspecified, result.ErrorReason));
        }
    }

    private async Task HandleSlashCommand(SlashCommandInfo info, IInteractionContext context, IResult result) =>
        await HandleServiceError(context, result);

    private async Task HandleContextCommand(ContextCommandInfo info, IInteractionContext context, IResult result) =>
        await HandleServiceError(context, result);

    private async Task HandleComponentCommand(ComponentCommandInfo info, IInteractionContext context, IResult result) =>
        await HandleServiceError(context, result);

    public static ErrorCodes CanRunCommand(IGuildUser user, ushort serverNum, string command)
    {
        if (!Program.Config.ValidCommands.ContainsKey(serverNum) || Program.Config.ValidCommands[serverNum].Count == 0)
            return ErrorCodes.InvalidCommand;

        foreach (KeyValuePair<ulong, List<string>> commandList in Program.Config.ValidCommands[serverNum])
        {
            
            if (!commandList.Value.Contains(command) && !commandList.Value.Any(command.StartsWith) && !commandList.Value.Contains(".*"))
                return ErrorCodes.InvalidCommand;
            if (user.Hierarchy >= user.Guild.GetRole(commandList.Key)?.Position)
                return ErrorCodes.None;
        }

        return ErrorCodes.PermissionDenied;
    }
}