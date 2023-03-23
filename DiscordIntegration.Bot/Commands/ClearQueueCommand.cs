namespace DiscordIntegration.Bot.Commands;

using Discord;
using Discord.Interactions;

using DiscordIntegration.Bot.Services;

public class ClearQueueCommand : InteractionModuleBase<SocketInteractionContext>
{
    private readonly Bot bot;

    public ClearQueueCommand(Bot bot) => this.bot = bot;

    [SlashCommand($"clear", "Sends a command to the SCP server.")]
    public async Task Send([Summary("command", "The command to send.")] string command)
    {
        ErrorCodes canRunCommand = SlashCommandHandler.CanRunCommand((IGuildUser) Context.User, bot.ServerNumber, command);
        if (canRunCommand != ErrorCodes.None)
        {
            await RespondAsync(embed: await ErrorHandlingService.GetErrorEmbed(canRunCommand));
            return;
        }
        
        try
        {
            bot.Messages.Clear();
            await RespondAsync("Current message queue for ", ephemeral: true);
        }
        catch (Exception e)
        {
            Log.Error(bot.ServerNumber, nameof(Send), e);
            await RespondAsync(embed: await ErrorHandlingService.GetErrorEmbed(ErrorCodes.Unspecified, e.Message));
        }
    }
}