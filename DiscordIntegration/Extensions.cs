using EXILED.Extensions;

namespace DiscordIntegration_Plugin
{
	public static class Extensions
	{
		public static void RAMessage(this CommandSender sender, string message, bool success = true) =>
			sender.RaReply("Discord Integration#" + message, success, true, string.Empty);

		/// <summary>
		/// Gets the <see cref="ReferenceHub">player</see>'s <see cref="Side">side</see> they're currently in.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static Side GetSide( this RoleType type ) => type.GetTeam().GetSide();

		/// <summary>
		/// Gets the <see cref="ReferenceHub">player</see>'s <see cref="Side">side</see> they're currently in.
		/// </summary>
		/// <param name="team"></param>
		/// <returns></returns>
		public static Side GetSide( this Team team ) {
			switch( team ) {
				case Team.SCP:
				return Side.SCP;
				case Team.MTF:
				case Team.RSC:
				return Side.MTF;
				case Team.CHI:
				case Team.CDP:
				return Side.CHAOS;
				case Team.TUT:
				return Side.TUTORIAL;
				case Team.RIP:
				default:
				return Side.NONE;
			}
		}

		/// <summary>
		/// Gets the <see cref="ReferenceHub">player</see>'s <see cref="Side">side</see> they're currently in.
		/// </summary>
		/// <param name="hub"></param>
		/// <returns></returns>
		public static Side GetSide( this ReferenceHub hub ) => hub.GetTeam().GetSide();
	}

	public enum Side {
		TUTORIAL, SCP, MTF, CHAOS, NONE
	}
}