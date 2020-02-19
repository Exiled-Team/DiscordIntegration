using EXILED.Extensions;
using Log = EXILED.Log;

namespace DiscordIntegration_Plugin
{
	public class Methods
	{
		private readonly Plugin plugin;
		public Methods(Plugin plugin) => this.plugin = plugin;

		public static void CheckForSyncRole(ReferenceHub player)
		{
			Log.Info($"Checking rolesync for {player.characterClassManager.UserId}");
			ProcessSTT.SendData($"checksync {player.characterClassManager.UserId}", 119);
		}

		public static void SetSyncRole(string group, string steamId)
		{
			Log.Info($"Received setgroup for {steamId}");
			UserGroup userGroup = ServerStatic.PermissionsHandler.GetGroup(group);
			if (userGroup == null)
			{
				Log.Error($"Attempted to assign invalid user group {group} to {steamId}");
				return;
			}

			ReferenceHub player = Player.GetPlayer(steamId);
			if (player == null)
			{
				Log.Error($"Error assigning user group to {steamId}, player not found.");
				return;
			}
			
			Log.Info($"Assigning role: {userGroup} to {steamId}.");
			player.serverRoles.SetGroup(userGroup, false);
		}
	}
}