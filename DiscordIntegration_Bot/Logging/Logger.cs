using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordIntegration_Bot.Logging
{
    public static class Logger
    {
        public static List<Task> LogTasks { get; set; } = new List<Task>();
        private static object _lock = new object();
        public static void LogInfo(string source, string message) => LogToConsole(new LogMessages(LogType.Info, source, message));
        public static void LogWarning(string source, string message) => LogToConsole(new LogMessages(LogType.Warning, source, message));
        public static void LogError(string source, string message, Exception exception = null) => LogToConsole(new LogMessages(LogType.Error, source, message, exception));
        public static void LogDebug(string source, string message) => LogToConsole(new LogMessages(LogType.Debug, source, message));
        public static void LogCritical(string source, string message) => LogToConsole(new LogMessages(LogType.Critical, source, message));
        public static void LogVerbose(string source, string message) => LogToConsole(new LogMessages(LogType.Verbose, source, message));
        public static string LogFile;
        public static List<string> LogFiles = new List<string>();
        public static DateTime FileCreated;

        public static void LogException(string source, Exception exception, string message = null) => LogToConsole(new LogMessages(LogType.Error, source, message ?? "No extra information.", exception));
        public static void LogToConsole(LogMessages logMessage)
        {

            LogTasks.Add(Task.Run(() =>
            {
                lock (_lock)
                {
                    PrintSeverityPrefix(logMessage.Severity);
                    Console.WriteLine($" {logMessage.Source}: {logMessage.Message}");
                    Console.ResetColor();
                    if (logMessage.HasException)
                    {
                        Console.WriteLine();
                        Console.WriteLine(logMessage.Exception.ToString());
                    }
                    RegisterLog(logMessage);
                    LogTasks = LogTasks.Where(x => !x.IsCanceled && !x.IsCompleted && !x.IsFaulted).ToList();
                }
            }));


        }
        private static void PrintSeverityPrefix(LogType severity)
        {
            // Looks like '[Info]' but adds color to the inner text, and restore the old color
            Console.Write("[");
            var oldColor = Console.ForegroundColor;
            ConsoleColor severityColor;
            switch (severity)
            {
                case LogType.Error:
                    severityColor = ConsoleColor.Red;
                    break;
                case LogType.Critical:
                    severityColor = ConsoleColor.DarkRed;
                    break;
                case LogType.Verbose:
                    severityColor = ConsoleColor.Cyan;
                    break;
                case LogType.Debug:
                    severityColor = ConsoleColor.Green;
                    break;
                case LogType.Warning:
                    severityColor = ConsoleColor.DarkYellow;
                    break;
                case LogType.Info:
                    severityColor = ConsoleColor.DarkBlue;
                    break;
                default:
                    throw new NotImplementedException("That log type doesn't exist");
            }

            Console.ForegroundColor = severityColor;
            Console.Write(severity.ToString());
            Console.ForegroundColor = oldColor;
            Console.Write("]");
        }

        private static void RegisterLog(LogMessages message)
        {
            /*Create a record of everything that comes out of the console (using these methods)*/

            string path = $"{Directory.GetCurrentDirectory()}/logs/Log-{DateTime.UtcNow.ToString("yyyy-MM-dd")}.txt";

            if (!Directory.Exists($"{Directory.GetCurrentDirectory()}/logs"))
                Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}/logs");
            foreach (string file in Directory.GetFiles($"{Directory.GetCurrentDirectory()}/logs"))
                LogFiles.Add(file);
            if (!File.Exists(path))
                File.Create(path).Close();

            while (LogFiles.Count > 5)
            {
                string file = LogFiles[0];
                File.Delete(file);
                LogFiles.Remove(file);
            }

            LogFile = path;
            LogFiles.Add(path);
            FileCreated = DateTime.UtcNow;

            if ((FileCreated - DateTime.UtcNow).TotalDays > 1)
            {
                LogFile = $"{Directory.GetCurrentDirectory()}/logs/Log-{DateTime.UtcNow.ToString("yyyy-MM-dd")}.txt";
                FileCreated = DateTime.UtcNow;
                LogFiles.Add(LogFile);
            }


            string logText = $"{DateTime.Now.ToString("hh:mm:ss")} [{message.Severity}] {message.Source}: {message.Exception?.ToString() ?? message.Message}";
            File.AppendAllText(path, logText + "\n");
        }

    }
}