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
        switch (CanRunCommand((IGuildUser) Context.User, serverNum, command))
        {
            case 0:
                break;
            case 1:
                await RespondAsync(embed: await ErrorHandlingService.GetErrorEmbed(ErrorCodes.InvalidCommand));
                return;
            case 2:
                await RespondAsync(embed: await ErrorHandlingService.GetErrorEmbed(ErrorCodes.PermissionDenied));
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

    public int CanRunCommand(IGuildUser user, ushort serverNum, string command)
    {
        if (!Program.Config.ValidCommands.ContainsKey(serverNum) || Program.Config.ValidCommands[serverNum].Count == 0)
            return 1;

        foreach (KeyValuePair<ulong, List<string>> commandList in Program.Config.ValidCommands[serverNum])
        {
            if (!commandList.Value.Contains(command) && !commandList.Value.Any(command.StartsWith))
                return 1;
            if (user.Hierarchy >= user.Guild.GetRole(commandList.Key)?.Position)
                return 0;
        }

        return 2;
    }
}