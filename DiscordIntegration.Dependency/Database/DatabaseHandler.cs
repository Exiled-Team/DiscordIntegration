using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace DiscordIntegration.Dependency.Database;

public class DatabaseHandler
{
    private static string _connectionString =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DiscordIntegration.db");

    public static void Init()
    {
        using SqliteConnection conn = new(_connectionString);
        conn.Open();

        using (SqliteCommand cmd = conn.CreateCommand())
        {
            cmd.CommandText =
                "CREATE TABLE IF NOT EXISTS Watchlist(Id INTEGER PRIMARY KEY AUTOINCREMENT, UserId TEXT, Reason TEXT)";
            cmd.ExecuteNonQuery();
        }
        
        conn.Close();
    }

    public static void AddEntry(string userId, string reason)
    {
        using SqliteConnection conn = new(_connectionString);
        conn.Open();

        using (SqliteCommand cmd = conn.CreateCommand())
        {
            cmd.CommandText = "INSERT INTO Watchlist(UserId, Reason) VALUES(@id, @reason)";
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.Parameters.AddWithValue("@reason", reason);
            cmd.ExecuteNonQuery();
        }
        conn.Close();
    }


    public static void RemoveEntry(string userId)
    {
        using SqliteConnection conn = new(_connectionString);
        conn.Open();

        using (SqliteCommand cmd = conn.CreateCommand())
        {
            cmd.CommandText = "DELETE FROM Watchlist WHERE UserId=@id";
            cmd.Parameters.AddWithValue("@id", userId);

            cmd.ExecuteNonQuery();
        }
        
        conn.Close();
    }

    public static bool CheckWatchlist(string userId, out string reason)
    {
        reason = "Not in watchlist";
        using SqliteConnection conn = new(_connectionString);
        conn.Open();
        
        using (SqliteCommand cmd = conn.CreateCommand())
        {
            cmd.CommandText = "SELECT * FROM Watchlist WHERE UserId=@id";
            cmd.Parameters.AddWithValue("@id", userId);

            using (SqliteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    reason = reader.GetString(2);
                    return true;
                }
            }
        }
        conn.Close();
        return false;
    }
}