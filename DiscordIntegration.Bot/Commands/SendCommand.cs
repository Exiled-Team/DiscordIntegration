namespace DiscordIntegration.Bot.Commands;

using Dependency;

using Discord;
using Discord.Interactions;
using Services;

using ActionType = DiscordIntegration.Dependency.ActionType;

public class SendCommand : InteractionModuleBase<SocketInteractionContext>
{
    private readonly Bot bot;

    public SendCommand(Bot bot) => this.bot = bot;

    [SlashCommand($"send", "Sends a command to the SCP server.")]
    public async Task Send([Summary("command", "The command to send.")] string command)
    {
        ErrorCodes errorCode = CanRunCommand((IGuildUser) Context.User, Config.Default.BotIds[Context.Client.CurrentUser.Id], command);
        
        if (errorCode != ErrorCodes.None)
        {
            await RespondAsync(embed: await ErrorHandlingService.GetErrorEmbed(errorCode));
            return;
        }

        try
        {
            Log.Debug(nameof(Send), $"Sending {command}");
            await bot.Server.SendAsync(new RemoteCommand(ActionType.ExecuteCommand, Context.Channel.Id, command, Context.User.Id, Context.User.Username));
            await RespondAsync("Command sent.", ephemeral: true);
        }
        catch (Exception e)
        {
            Log.Error(nameof(Send), e);
            await RespondAsync(embed: await ErrorHandlingService.GetErrorEmbed(ErrorCodes.Unspecified, e.Message));
        }
    }

    public ErrorCodes CanRunCommand(IGuildUser user, ushort serverNum, string command)
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