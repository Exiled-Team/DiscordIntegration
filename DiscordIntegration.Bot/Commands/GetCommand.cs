namespace DiscordIntegration.Bot.Commands;

using Discord;
using Discord.Interactions;

public class GetCommand : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("get", "Shows which commands you can execute.")]
    public async Task ShowAvailableCommandsAsync([Summary("server", "The server number to get available commands.")] ushort serverNum)
    {
        if (!Program.Config.ValidCommands.ContainsKey(serverNum))
        {
            await RespondAsync("This server was not found.", ephemeral: true);
            return;
        }

        List<string> availableCommands = new();
        foreach (KeyValuePair<ulong, List<string>> commandList in Program.Config.ValidCommands[serverNum])
        foreach (string cmd in commandList.Value)
            if (SlashCommandHandler.CanRunCommand((IGuildUser)Context.User, serverNum, cmd) ==0)
                availableCommands.Add(cmd);

        if (!availableCommands.Any())
        {
            await RespondAsync("There are no available commands for you.", ephemeral: true);
            return;
        }
        
        await RespondAsync("You can use these commands:\n" + string.Join("\n", availableCommands), ephemeral: true);
    }
}