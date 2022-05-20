namespace DiscordIntegration.Bot.Commands;

using System.Text;
using Dependency;
using Discord.Interactions;
using Newtonsoft.Json;
using Services;

public class SendCommand : InteractionModuleBase<SocketInteractionContext>
{
    private readonly Bot bot;

    public SendCommand(Bot bot) => this.bot = bot;

    [SlashCommand($"send", "Sends a command to the SCP server.")]
    public async Task Send([Summary("command", "The command to send.")] string command)
    {
        try
        {
            Log.Debug(nameof(Send), $"Sending {command}");
            string literal = @"{channelId: %chan, content: %command, user: {id: %userid@discord, name: %name}}";
            literal = literal.Replace("%chan", Context.Channel.Id.ToString()).Replace("%command", command).Replace("%userid", Context.User.Id.ToString()).Replace("%name", Context.User.Username);
            await bot.Server.SendAsync(new RemoteCommand(ActionType.ExecuteCommand, Context.Channel.Id, command, Context.User.Id, Context.User.Username));
            await RespondAsync("Command sent.", ephemeral: true);
        }
        catch (Exception e)
        {
            Log.Error(nameof(Send), e);
        }
    }
}