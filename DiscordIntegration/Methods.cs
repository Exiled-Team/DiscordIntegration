namespace DiscordIntegration_Plugin
{
	public class Methods
	{
		private readonly Plugin plugin;
		public Methods(Plugin plugin) => this.plugin = plugin;

		public static void CheckForSyncRole(ReferenceHub player)
		{
			Plugin.Info($"Checking rolesync for {player.characterClassManager.UserId}");
			ProcessSTT.SendData($"checksync {player.characterClassManager.UserId}", 119);
		}

		public static void SetSyncRole(string group, string steamId)
		{
			Plugin.Info($"Received setgroup for {steamId}");
			UserGroup userGroup = ServerStatic.PermissionsHandler.GetGroup(group);
			if (userGroup == null)
			{
				Plugin.Error($"Attempted to assign invalid user group {group} to {steamId}");
				return;
			}

			ReferenceHub player = Plugin.GetPlayer(steamId);
			if (player == null)
			{
				Plugin.Error($"Error assigning user group to {steamId}, player not found.");
				return;
			}
			
			Plugin.Info($"Assigning role: {userGroup} to {steamId}.");
			player.serverRoles.SetGroup(userGroup, false);
		}
	}
}