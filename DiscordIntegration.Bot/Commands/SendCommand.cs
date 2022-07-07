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
    public async Task Send([Summary("server", "The server number to send the command to.")] ushort serverNum, [Summary("command", "The command to send.")] string command)
    {
        ErrorCodes canRunCommand = CanRunCommand((IGuildUser) Context.User, serverNum, command);
        if (canRunCommand != ErrorCodes.None)
        {
            await RespondAsync(embed: await ErrorHandlingService.GetErrorEmbed(canRunCommand));
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
            if (user.Hierarchy >= user.Guild.GetRole(commandList.Key)?.Position && commandList.Value.Contains(".*"))
                return ErrorCodes.None;
            if (!commandList.Value.Contains(command) && !commandList.Value.Any(command.StartsWith))
                return ErrorCodes.InvalidCommand;
            if (user.Hierarchy >= user.Guild.GetRole(commandList.Key)?.Position)
                return ErrorCodes.None;
        }

        return ErrorCodes.PermissionDenied;
    }
}
