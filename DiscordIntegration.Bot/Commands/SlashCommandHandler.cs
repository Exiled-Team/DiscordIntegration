namespace DiscordIntegration.Bot.Commands;

using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Services;

public class SlashCommandHandler
{
    private readonly InteractionService service;
    private readonly DiscordSocketClient client;

    public SlashCommandHandler(InteractionService service, DiscordSocketClient client)
    {
        this.service = service;
        this.client = client;
    }

    public async Task InstallCommandsAsync()
    {
        await service.AddModulesAsync(Assembly.GetEntryAssembly(), null);
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
            await service.ExecuteCommandAsync(context, null);
            Log.Info(nameof(HandleInteraction), $"{interaction.User.Username} used an interaction.");
        }
        catch (Exception e)
        {
            Log.Error(nameof(HandleInteraction), e);
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
}