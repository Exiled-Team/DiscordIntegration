namespace DiscordIntegration_Plugin
{
	public static class Extensions
	{
		public static void RAMessage(this CommandSender sender, string message, bool success = true) =>
			sender.RaReply("Discord Integration#" + message, success, true, string.Empty);
	}
}