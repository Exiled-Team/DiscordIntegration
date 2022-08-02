namespace DiscordIntegration.Bot;

public enum ErrorCodes
{
    None,
    InternalCommandError,
    UnableToParseDuration,
    SpecifiedUserNotFound,
    PermissionDenied,
    InvalidChannelId,
    UnableToParseDate,
    InvalidCommand,
    DatabaseNotFound,
    FailedToParseTitle,
    FailedToParseColor,
    NoRecordForUserFound,
    InvalidMessageId,
    Unspecified,
    TriggerLengthExceedsLimit,
    UnableToParseId,
}