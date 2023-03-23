using DiscordIntegration.Bot.Services;
using DiscordIntegration.Dependency.Database;

namespace DiscordIntegration.Bot.Commands;

using Discord;
using Discord.Interactions;

[Group("watchlist", "Commands for managing the watchlist.")]
public class WatchlistCommands: InteractionModuleBase<SocketInteractionContext>
{
    private readonly Bot bot;

    public WatchlistCommands(Bot bot) => this.bot = bot;
    
    [SlashCommand("add", "Adds a UserID to the watchlist.")]
    public async Task AddToWatchlist([Summary("UserId", "The user ID of the player to watch.")] string userId, [Summary("Reason", "The reason they should be watched.")] string reason)
    {
        ErrorCodes canRunCommand = SlashCommandHandler.CanRunCommand((IGuildUser) Context.User, bot.ServerNumber, "watchlist");
        if (canRunCommand != ErrorCodes.None)
        {
            await RespondAsync(embed: await ErrorHandlingService.GetErrorEmbed(canRunCommand), ephemeral: true);
            return;
        }

        if (DatabaseHandler.CheckWatchlist(userId, out string res))
        {
            await RespondAsync(
                embed: await EmbedBuilderService.CreateBasicEmbed("User already on Watchlist",
                    $"The userID {userId} is already on the watchlist for {reason}. If you wish to change the reason, please remove the user first then re-add them.",
                    Color.Orange), ephemeral: true);
            return;
        }
        
        DatabaseHandler.AddEntry(userId, reason);
        await RespondAsync(
            embed: await EmbedBuilderService.CreateBasicEmbed("User added to Watchlist",
                $"The userID {userId} has been added to the watchlist for {reason}", Color.Green), ephemeral: true);
    }

    [SlashCommand("remove", "Removes a UserID from the watchlist.")]
    public async Task RemoveFromWatchlist([Summary("UserID", "The user ID of the player to remove from the watchlist.")] string userId)
    {
        ErrorCodes canRunCommand =
            SlashCommandHandler.CanRunCommand((IGuildUser)Context.User, bot.ServerNumber, "watchlist");
        if (canRunCommand != ErrorCodes.None)
        {
            await RespondAsync(embed: await ErrorHandlingService.GetErrorEmbed(canRunCommand), ephemeral: true);
            return;
        }

        if (!DatabaseHandler.CheckWatchlist(userId, out string _))
        {
            await RespondAsync(embed: await ErrorHandlingService.GetErrorEmbed(ErrorCodes.NoRecordForUserFound), ephemeral: true);
            return;
        }
        
        DatabaseHandler.RemoveEntry(userId);
        await RespondAsync(
            embed: await EmbedBuilderService.CreateBasicEmbed("User removed from watchlist.",
                $"User {userId} has been removed from the watchlist.", Color.Green), ephemeral: true);
    }
}