namespace DiscordIntegration_Plugin
{
	public class Methods
	{
		private readonly Plugin plugin;
		public Methods(Plugin plugin) => this.plugin = plugin;

		public void CheckForSyncRole(ReferenceHub player)
		{
			ProcessSTT.SendData($"checkrole {player.characterClassManager.UserId}", 119);
		}

		public static void SetSyncRole(string group, string steamId)
		{
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
			
			player.serverRoles.SetGroup(userGroup, false);
		}
	}
}