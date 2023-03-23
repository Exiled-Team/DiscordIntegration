namespace DiscordIntegration.Bot.Services;

using System.Reflection;
using Discord;

public class EmbedBuilderService
{
    public static string Footer => $"Discord Integration | {Assembly.GetExecutingAssembly().GetName().Version} | - Joker119";

    public static async Task<Embed> CreateBasicEmbed(string title, string description, Color color) => await Task.Run(() => new EmbedBuilder().WithTitle(title).WithDescription(description).WithColor(color).WithCurrentTimestamp().WithFooter(Footer).Build());
}