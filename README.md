# DiscordIntegration

A bot and server plugin to allow server logs to be sent to Discord channels, and for server commands to be run via the Discord bot.

## Minimum requirements
[EXILED](https://github.com/Exiled-Team/EXILED/releases/latest) **6.0.0++**

## Installation
1. Extract `DiscordIntegration.dll` and its dependencies from `Plugin.tar.gz`.
2. Place `DiscordIntegration.dll` inside the EXILED `Plugins` folder like any other plugin and its dependencies in the `Plugins/dependencies` folder.
3. Download the `DiscordIntegration.Bot` (Linux) or `DiscordIntegration.Bot.exe` (Windows) file, and place it anywhere within the system running the SL server.

## How to create a Discord bot
1. Go to https://discord.com/developers/applications and create a new application.
2. Inside of the application page under "settings" click Bot & build the bot.
3. After creating the bot you can change the bots username and avatar if you wish.

Bot token is located on the Bot page under username, Do NOT share the token because people can control the bot if they get it.

## How to invite the bot to your discord
1. Go to "OAuth2" tab of the bot application on the above link.
2. Click the boxes labled "bot" and "application.commands".
3. Give the bot appropriate permissions (this is usually either Admin, or at the very least Send Messages).
4. Copy & paste the oauth2 URL link provided at the bottom of the page into a browser window.
5. The new page will ask you which server to invite the bot to, and once  you select the correct server, click Authorize.

**Note that before generating the link, check that you have applications.commands scope.**

## How to run the bot

Open the bot once to let it automatically generate config.json file.
Remember to always wrap configs with quotation marks, even if it's not necessary for strings.

Fill out the bot's config for the bot token, discord server id, and channel IDs, then start the bot.

### Windows

1. Double-click the .exe file.

### Linux

1. Run `cd path/to/bot` replacing `path/to/bot` with the path of where the extracted bot is located.
2. Run `./DiscordIntegration.Bot`.

## How configure the execution of game commands through Discord

1. Open your bot `config.json` file.

2. Add role IDs and list every command they can execute. You can use `.*` to permit to that role ID to use all game commands without restrictions.

```json
  "ValidCommands": {
    "1": {
      "953784342595915779": [
        "di"
      ]
    }
  },
 ```

3. **Never duplicate commands.** Higher roles on your Discord server will be able to use lower roles commands as well, based on the position of the roles.

## Available commands

| Command | Description | Arguments | Permission | Example |
| --- | --- | --- | --- | --- |
| di playerlist | Gets the list of players in the server. | | di.playerlist | **di playerlist** |
| di stafflist | Gets the list of staffers in the server. | | di.stafflist | **di stafflist** |
