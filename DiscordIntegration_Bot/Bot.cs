using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordIntegration_Bot
{
	public class Bot
	{
		private static DiscordSocketClient client;
		public static DiscordSocketClient Client => client ?? (client = new DiscordSocketClient());
		private readonly Program program;

		public Bot(Program program)
		{
			this.program = program;
			InitBot().GetAwaiter().GetResult();
		}

		private async Task InitBot()
		{
			Program.Log("Setting up bot..", true);
			Client.Log += Program.Log;
			Client.MessageReceived += OnMessageReceived;
			Program.Log("Logging into bot..", true);
			await Client.LoginAsync(TokenType.Bot, Program.Config.BotToken);
			await Client.StartAsync();
			Program.Log("Login successful, starting STT..");
			new Thread((() => ProcessSTT.Init(program))).Start();
			await Task.Delay(-1);
		}

		public async Task OnMessageReceived(SocketMessage message)
		{
			CommandContext context = new CommandContext(Client, (IUserMessage)message);
			if (context.Message.Author.IsBot)
				return;

			if (message.Content.StartsWith(Program.Config.BotPrefix) ||
			    message.Content.StartsWith(Client.CurrentUser.Mention))
			{
				try
				{
					HandleCommand(context);
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
			}
		}

		public async Task HandleCommand(ICommandContext context)
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
			}

			if (Program.Config.AllowedCommands.ContainsKey(args[0].ToLower()))
			{
				PermLevel lvl = PermLevel.PermLevel0;
				foreach (ulong id in user.RoleIds.Where(s => s == Program.Config.PermLevel1Id || s == Program.Config.Permlevel2Id || s == Program.Config.Permlevel3Id || s == Program.Config.Permlevel4Id))
				{
					if (GetPermlevel(id) > lvl)
						lvl = GetPermlevel(id);
				}

				if (lvl >= Program.Config.AllowedCommands[args[0].ToLower()])
					ProcessSTT.SendData(context.Message.Content, Program.Config.Port, context.Message.Author.Username,
						context.Channel.Id);
				else
					await context.Channel.SendMessageAsync("Permission denied.");
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