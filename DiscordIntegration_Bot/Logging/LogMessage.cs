using System;

namespace DiscordIntegration_Bot.Logging
{
    public class LogMessages
    {
        public LogMessages(LogType severity, string source, string message, Exception exception = null)
        {
            Severity = severity;
            Message = message;
            Source = source;
            Exception = exception;
        }
        public LogType Severity { get; }
        public string Message { get; }
        public string Source { get; }
        public Exception Exception { get; }
        public bool HasException => Exception != null;
    }
}