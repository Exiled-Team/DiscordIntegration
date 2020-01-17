using System;
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
			if (program.Config.AllowedCommands.Contains(args[0].ToLower()))
				ProcessSTT.SendData(context.Message.Content, program.Config.Port, context.Message.Author.Username, context.Channel.Id);
		}
	}
}