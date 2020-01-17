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
			Client.Log += Program.Log;
			Client.MessageReceived += OnMessageReceived;
			await Client.LoginAsync(TokenType.Bot, program.Config.BotToken);
			await Client.StartAsync();
			new Thread((() => ProcessSTT.Init(program))).Start();
			await Task.Delay(-1);
		}

		public async Task OnMessageReceived(SocketMessage message)
		{
			CommandContext context = new CommandContext(Client, (IUserMessage)message);
			if (context.Message.Author.IsBot)
				return;

			if (message.Content.StartsWith(program.Config.BotPrefix) ||
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

			args[0] = args[0].Replace(program.Config.BotPrefix, "");

			switch (args[0].ToLower())
			{
				case "ping":
					await context.Channel.SendMessageAsync($"Pong!");
					return;
			}

			if (program.Config.AllowedCommands.ContainsKey(args[0].ToLower()))
			{
				PermLevel lvl = PermLevel.PermLevel0;
				foreach (ulong id in user.RoleIds.Where(s => s == program.Config.PermLevel1Id || s == program.Config.Permlevel2Id || s == program.Config.Permlevel3Id || s == program.Config.Permlevel4Id))
				{
					if (GetPermlevel(id) > lvl)
						lvl = GetPermlevel(id);
				}

				if (lvl >= program.Config.AllowedCommands[args[0].ToLower()])
					ProcessSTT.SendData(context.Message.Content, program.Config.Port, context.Message.Author.Username,
						context.Channel.Id);
				else
					await context.Channel.SendMessageAsync("Permission denied.");
			}
		}

		public PermLevel GetPermlevel(ulong id)
		{
			if (program.Config.PermLevel1Id == id)
				return PermLevel.PermLevel1;
			if (program.Config.Permlevel2Id == id)
				return PermLevel.PermLevel2;
			if (program.Config.Permlevel3Id == id)
				return PermLevel.PermLevel3;
			if (program.Config.Permlevel4Id == id)
				return PermLevel.PermLevel4;
			return PermLevel.PermLevel0;
		}
	}
}