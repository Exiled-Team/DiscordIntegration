#nullable enable
using System;
using System.IO;
using LiteDB;

namespace DiscordIntegration.Dependency.Database;

public class DatabaseHandler
{
    private static string _connectionString =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DiscordIntegration.db");

    public static void AddEntry(string userId, string reason)
    {
        using LiteDatabase database = new(_connectionString);

        var collection = database.GetCollection<Watchlist>();

        Watchlist watch = new(userId, reason);

        collection.Insert(watch);
    }


    public static void RemoveEntry(string userId)
    {
        using LiteDatabase database = new(_connectionString);

        var collection = database.GetCollection<Watchlist>();

        Watchlist watch = collection.FindOne(x => x.UserId == userId);
        collection.Delete(watch.Id);
    }

    public static bool CheckWatchlist(string userId, out string reason)
    {
        using LiteDatabase database = new(_connectionString);

        var collection = database.GetCollection<Watchlist>();
        Watchlist? watch = collection.FindOne(x => x.UserId == userId);
        if (watch is not null)
        {
            reason = watch.Reason;
            return true;
        }

        reason = string.Empty;
        return false;
    }
}