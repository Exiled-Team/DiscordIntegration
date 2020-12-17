using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordIntegration_Bot.Logging;

namespace DiscordIntegration_Bot
{
	public class Bot
	{
		private static DiscordSocketClient client;
		public static DiscordSocketClient Client => client ?? (client = new DiscordSocketClient());
		private readonly Program program;
		private static string _appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		private static string _exiled = Path.Combine(_appData, "EXILED");
		private static string _plugins = Path.Combine(_exiled, "Plugins");
		private static string _diPath = Path.Combine(_plugins, "Integration");
		private string roleSync = Path.Combine(_diPath, "Sync-Roles.txt");
		private string userSync = Path.Combine(_diPath, "Sync-Users.txt");

		/// <summary>
		///     Signals to a <see cref="CancellationToken"/>. <br/>
		///     <note>We'll use to end the bot if needed(used for most async stuff).</note>
		/// </summary>
		public CancellationTokenSource cts { get; set; }

		public Bot(Program program)
		{
			this.program = program;
			InitBot().GetAwaiter().GetResult();
		}

		private async Task InitBot()
		{
            Console.WriteLine("\t\t──────────── BOT ────────────");
            Logger.LogInfo("DiscordIntegration", "Starting the Bot"); //Notifying that the app is starting ...

            this.cts = new CancellationTokenSource();
            await Task.Delay(TimeSpan.FromSeconds(2));

			/*Loading the configuration file*/
			Logger.LogInfo("DiscordIntegration", "[1/4] Loading config file...");
            await Task.Delay(TimeSpan.FromSeconds(2));

            await ReloadConfig();

			/* Creating a Discord.Net Client */
			Logger.LogInfo("DiscordIntegration", $"[2/4] Creating client...");
            await Task.Delay(TimeSpan.FromSeconds(2));

            Client.Log += Program.Log;

			/* Register Commands */
			Logger.LogInfo("DiscordIntegration", $"[3/4] Loading commands...");
            await Task.Delay(TimeSpan.FromSeconds(2));
			Client.MessageReceived += OnMessageReceived;

			// Connect to discord service
			Logger.LogInfo("DiscordIntegration", $"[4/4] Connecting...");
            await Task.Delay(TimeSpan.FromSeconds(2));

			await Client.LoginAsync(TokenType.Bot, Program.Config.BotToken);
            await Client.StartAsync();

			Logger.LogInfo("DiscordIntegration", "Successfully connected!");
			new Thread((() => ProcessSTT.Init(program))).Start();
			await Task.Delay(-1);
		}

		public async Task OnMessageReceived(SocketMessage message)
		{
			CommandContext context = new CommandContext(Client, (IUserMessage)message);

			if (message.Content.StartsWith(Program.Config.BotPrefix) ||
			    message.Content.StartsWith(Client.CurrentUser.Mention))
			{
				try
				{
                    await HandleCommand(context);
				}
				catch (Exception e)
				{
					Logger.LogException("OnMessageReceived", e);
				}
			}
		}

		public async Task HandleCommand(ICommandContext context)
		{
			try
			{
				string[] args = context.Message.Content.Split(' ');
				IGuildUser user = (IGuildUser) context.Message.Author;
				if (context.Message.Content.StartsWith(context.Guild.EveryoneRole.Mention))
				{
					await context.Channel.SendMessageAsync("You cannot mention everyone in a command.");
					return;
				}

				args[0] = args[0].Replace(Program.Config.BotPrefix, "");

				switch (args[0].ToLower())
				{
					case "ping":
						await context.Channel.SendMessageAsync($"Pong!");
						return;
					case "addusr":
					{
						if (user.RoleIds.All(r => r != Program.Config.StaffRoleId))
						{
							await context.Channel.SendMessageAsync("Code 4: Permission Denied.");
							return;
						}

						if (args.Length != 3)
						{
							await context.Channel.SendMessageAsync("Code 3: Improper number of arguments.");
							return;
						}

						string id = args[1].Replace("<", "").Replace("@", "").Replace(">", "").Replace("!", "");
						if (!ulong.TryParse(id, out ulong userId))
						{
							await context.Channel.SendMessageAsync($"Code 2: invalid Discord user defined.");
							return;
						}

						if (!ulong.TryParse(args[2], out ulong steamId))
						{
							await context.Channel.SendMessageAsync("Code 5: Invalid steamID defined.");
							return;
						}

						File.AppendAllText(userSync, $"{userId}:{steamId}@steam\n");
						await context.Channel.SendMessageAsync("User successfully added to user sync file.");
						return;
					}
					case "addrole":
					{
						if (user.RoleIds.All(r => r != Program.Config.StaffRoleId))
						{
							await context.Channel.SendMessageAsync("Code 4: Permission Denied.");
							return;
						}

						if (args.Length != 3)
						{
							await context.Channel.SendMessageAsync($"Code 3: Improper number of arguments.");
							return;
						}

						string id = args[1].Replace("<", "").Replace("@", "").Replace(">", "").Replace("&", "")
							.Replace("!", "");
						if (!ulong.TryParse(id, out ulong roleId) || context.Guild.GetRole(roleId) == null)
						{
							await context.Channel.SendMessageAsync("Code 6: Unable to retrieve Discord Role.");
							return;
						}

						File.AppendAllText(roleSync, $"{id}:{args[2]}");
						await context.Channel.SendMessageAsync($"New role sync added successfully.");
						return;
					}
					case "resync":
					{
						await ReloadConfig();
						await context.Channel.SendMessageAsync("Role sync configs reloaded.");
						return;
					}
					case "delusr":
					{
						if (user.RoleIds.All(r => r != Program.Config.StaffRoleId))
						{
							await context.Channel.SendMessageAsync("Code 4: Permission Denied.");
							return;
						}

						if (args.Length != 2)
						{
							await context.Channel.SendMessageAsync("Code 3: Improper number of arguments.");
							return;
						}

						string[] readArray = File.ReadAllLines(userSync);
						List<string> toKeep = new List<string>();
						foreach (string usr in readArray)
						{
							string[] sync = usr.Split(':');
							if (sync[1] != $"{args[1]}@steam")
								toKeep.Add(usr);
						}

						File.WriteAllLines(userSync, toKeep);
						await context.Channel.SendMessageAsync("User sync successfully removed.");
						return;
					}
					case "delrole":
					{
						if (user.RoleIds.All(r => r != Program.Config.StaffRoleId))
						{
							await context.Channel.SendMessageAsync("Code 4: Permission Denied.");
							return;
						}

						if (args.Length != 2)
						{
							await context.Channel.SendMessageAsync($"Code 3: Improper number of arguments.");
							return;
						}

						string id = args[1].Replace("<", "").Replace("@", "").Replace(">", "").Replace("&", "")
							.Replace("!", "");
						if (!ulong.TryParse(id, out ulong roleId) || context.Guild.GetRole(roleId) == null)
						{
							await context.Channel.SendMessageAsync("Code 6: Unable to retrieve Discord Role.");
							return;
						}
						string[] readArray = File.ReadAllLines(roleSync);
						List<string> toKeep = new List<string>();
						foreach (string role in readArray)
						{
							string[] sync = role.Split(':');
							if (sync[0] != id)
								toKeep.Add(role);
						}

						File.WriteAllLines(roleSync, toKeep);
						await context.Channel.SendMessageAsync("Role sync successfully removed.");
						return;
					}
				}

				if (Program.Config.AllowedCommands.ContainsKey(args[0].ToLower()))
				{
					PermLevel lvl = PermLevel.PermLevel0;
					foreach (ulong id in user.RoleIds.Where(s =>
						s == Program.Config.PermLevel1Id || s == Program.Config.Permlevel2Id ||
						s == Program.Config.Permlevel3Id || s == Program.Config.Permlevel4Id))
					{
						if (GetPermlevel(id) > lvl)
							lvl = GetPermlevel(id);
					}

					if (lvl >= Program.Config.AllowedCommands[args[0].ToLower()])
						ProcessSTT.SendData(context.Message.Content, Program.Config.Port,
							context.Message.Author.Username, context.Channel.Id);
					else
						await context.Channel.SendMessageAsync("Permission denied.");
				}
			}
			catch (Exception e)
			{
				Program.Error($"Command failure: {e}");
			}
		}

		public async Task ReloadConfig()
		{
			if (!Directory.Exists(_diPath))
				Directory.CreateDirectory(_diPath);

			if (!File.Exists(roleSync))
				File.Create(roleSync).Close();
			if (!File.Exists(userSync))
				File.Create(userSync).Close();
			
			Program.SyncedGroups.Clear();
			Program.Users.Clear();

			foreach (string rs in File.ReadAllLines(roleSync))
			{
				string[] sync = rs.Split(':');
				if (!ulong.TryParse(sync[0], out ulong roleId))
				{
					Logger.LogError("DiscordIntegration", $"Invalid DiscordRole defined: {sync[0]}");
					continue;
				}
				
				Program.SyncedGroups.Add(roleId, sync[1]);
			}

			foreach (string us in File.ReadAllLines(userSync))
			{
				string[] sync = us.Split(':');
				if (!ulong.TryParse(sync[0], out ulong userId))
				{
					Logger.LogError("DiscordIntegration", $"Invalid Discord User ID defined: {sync[0]}");
					continue;
				}
				
				Program.Users.Add(new SyncedUser{DiscordId = userId, UserId = sync[1]});
			}
		}

		public PermLevel GetPermlevel(ulong id)
		{
			if (Program.Config.PermLevel1Id == id)
				return PermLevel.PermLevel1;
			if (Program.Config.Permlevel2Id == id)
				return PermLevel.PermLevel2;
			if (Program.Config.Permlevel3Id == id)
				return PermLevel.PermLevel3;
			if (Program.Config.Permlevel4Id == id)
				return PermLevel.PermLevel4;
			return PermLevel.PermLevel0;
		}
	}
}
