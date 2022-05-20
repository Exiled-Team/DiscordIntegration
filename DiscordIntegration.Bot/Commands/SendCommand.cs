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
            await bot.Server.TcpClient.GetStream()
                .WriteAsync(Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(new RemoteCommand(ActionType.ExecuteCommand, command))));
            
            await RespondAsync("Command sent.", ephemeral: true);
        }
        catch (Exception e)
        {
            Log.Error(nameof(Send), e);
        }
    }
}