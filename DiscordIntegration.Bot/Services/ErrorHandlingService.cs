namespace DiscordIntegration.Bot.Services;

using Discord;

public static class ErrorHandlingService
{
    private static readonly Dictionary<ErrorCodes, string> ErrorDescriptions = new()
    {
        {
            ErrorCodes.PermissionDenied, "You are not permitted to run this command.\n{0}"
        },
        {
            ErrorCodes.UnableToParseDuration,
            "{0} is invalid.\nThe duration must be a whole number followed by a single modifier letter of 's', 'm', 'h', 'd', 'w', 'M' or 'y'"
        },
        {
            ErrorCodes.SpecifiedUserNotFound, "The specified user ({0}) is not in the server."
        },
        {
            ErrorCodes.InternalCommandError, "Joker broke something, ping him.\n{0}"
        },
        {
            ErrorCodes.InvalidChannelId, "No channel with the provided ID {0} exists."
        },
        {
            ErrorCodes.UnableToParseDate, "The date {0} is invalid."
        },
        {
            ErrorCodes.InvalidCommand, "That command is not enabled on this server."
        },
        {
            ErrorCodes.DatabaseNotFound, "Joker broke something, ping him."
        },
        {
            ErrorCodes.FailedToParseTitle,
            "No valid title was found. Titles for embeds must be encased in \"double-quotes\""
        },
        {
            ErrorCodes.FailedToParseColor, "{0} is not a valid HTML HEX color code."
        },
        {
            ErrorCodes.NoRecordForUserFound, "No database records for {0} were found."
        },
        {
            ErrorCodes.InvalidMessageId, "No message with ID {0} was found."
        },
        {
            ErrorCodes.Unspecified, "You probably used this wrong, but ping Joker anyways.\n{0}"
        },
        {
            ErrorCodes.TriggerLengthExceedsLimit, "Ping triggers are limited to {0} characters in length."
        }
    };

    private static string GetErrorMessage(ErrorCodes e, string extra = "") => $"Code {(int)e}: {e.ToString().SplitCamelCase()} {extra}".TrimEnd(' ');

    private static string GetErrorDescription(ErrorCodes e) => ErrorDescriptions.ContainsKey(e)
        ? ErrorDescriptions[e]
        : ErrorDescriptions[ErrorCodes.Unspecified];

    public static async Task<Embed> GetErrorEmbed(ErrorCodes errorCode, string extra = "") =>
        await EmbedBuilderService.CreateBasicEmbed(GetErrorMessage(errorCode),
            !string.IsNullOrEmpty(extra)
                ? string.Format(GetErrorDescription(errorCode), $"\"{extra}\"")
                : GetErrorDescription(errorCode).Replace("{0}", string.Empty) + "\n**Please report all bugs on Github.**", Color.Red);
}