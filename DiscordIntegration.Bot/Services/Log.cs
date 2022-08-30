namespace DiscordIntegration.Bot.Services;

using Discord;

using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

public class Log
{
    public static string DirectoryPath => Path.Combine(Environment.CurrentDirectory, "logs");

    public static Task Send(ushort port, LogMessage msg, bool skipLog = false)
    {
        Console.WriteLine($"{DateTime.Now.Date.ToString("MM/dd/yyyy")} {msg}");
        if (!Directory.Exists(DirectoryPath))
            Directory.CreateDirectory(DirectoryPath);

        string filePath = Path.Combine(DirectoryPath, port == 0 ? "Program.log" : $"{port}.log");
        File.AppendAllText(filePath, $"{DateTime.Now.Date.ToString("MM/dd/yyyy")} {msg}\n");

        if (!skipLog)
        {
            try
            {
                CheckFileSize(filePath);
            }
            catch (Exception e)
            {
                Error(0, nameof(Log), $"Error handling log file archival:\n{e}", true);
            }
        }

        return Task.CompletedTask;
    }

    public static void Info(ushort port, string source, object msg, bool skipLog = false) =>
        Send(port, new LogMessage(LogSeverity.Info, source, $"[INFO] {msg}"), skipLog);

    public static void Debug(ushort port, string source, object msg, bool skipLog = false)
    {
        if (Program.Config.Debug)
            Send(port, new LogMessage(LogSeverity.Debug, source, $"[DEBUG] {msg}"), skipLog);
    }
        
    public static void Error(ushort port, string source, object msg, bool skipLog = false) =>
        Send(port, new LogMessage(LogSeverity.Error, source, $"[ERROR] {msg}"), skipLog);

    public static void Warn(ushort port, string source, object msg, bool skipLog = false) =>
        Send(port, new LogMessage(LogSeverity.Warning, source, $"[WARN] {msg}"), skipLog);

    private static void CheckFileSize(string path)
    {
        FileInfo file = new(path);
        // 10485760 = 10 MB
        if (file.Length > 10485760)
        {
            string archivePath = Path.Combine(DirectoryPath, $"{file.Name}.tar.gz");
            if (File.Exists(archivePath))
                File.Delete(archivePath);
            
            using FileStream outStream = File.Create(archivePath);
            using GZipOutputStream gzoStream = new(outStream);
            
            TarArchive? archive = TarArchive.CreateOutputTarArchive(gzoStream);
            archive.RootPath = DirectoryPath.Replace('\\', '/').TrimEnd('/');

            TarEntry entry = TarEntry.CreateEntryFromFile(path);
            entry.Name = path.Replace(DirectoryPath, string.Empty);
            entry.Name = entry.Name.TrimStart('\\');
            
            archive.WriteEntry(entry, true);

            File.Delete(path);
        }
    }
}