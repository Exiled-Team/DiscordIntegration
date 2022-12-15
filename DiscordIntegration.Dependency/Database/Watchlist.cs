namespace DiscordIntegration.Dependency.Database;

public class Watchlist
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Reason { get; set; }

    public Watchlist(string userId, string reason)
    {
        UserId = userId;
        Reason = reason;
    }
}