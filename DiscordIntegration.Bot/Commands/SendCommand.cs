namespace DiscordIntegration.Bot.Commands;

using System.Text;
using Dependency;

using Discord;
using Discord.Interactions;
using Newtonsoft.Json;
using Services;

using ActionType = DiscordIntegration.Dependency.ActionType;

public class SendCommand : InteractionModuleBase<SocketInteractionContext>
{
    private readonly Bot bot;

    public SendCommand(Bot bot) => this.bot = bot;

    [SlashCommand($"send", "Sends a command to the SCP server.")]
    public async Task Send([Summary("command", "The command to send.")] string command)
    {
        ErrorCodes canRunCommand = SlashCommandHandler.CanRunCommand((IGuildUser) Context.User, bot.ServerNumber, command);
        if (canRunCommand != ErrorCodes.None)
        {
            await RespondAsync(embed: await ErrorHandlingService.GetErrorEmbed(canRunCommand), ephemeral: true);
            return;
        }
        
        try
        {
            Log.Debug(bot.ServerNumber, nameof(Send), $"Sending {command}");
            await bot.Server.SendAsync(new RemoteCommand(ActionType.ExecuteCommand, Context.Channel.Id, command, Context.User.Id, Context.User.Username));
            await RespondAsync("Command sent.", ephemeral: true);
        }
        catch (Exception e)
        {
            Log.Error(bot.ServerNumber, nameof(Send), e);
            await RespondAsync(embed: await ErrorHandlingService.GetErrorEmbed(ErrorCodes.Unspecified, e.Message));
        }
    }
}