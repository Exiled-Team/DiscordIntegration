# DiscordIntegration

A bot and server plugin to allow server logs to be sent to Discord channels, and for server commands to be run via the Discord bot.

## Minimum requirements
[EXILED](https://github.com/Exiled-Team/EXILED/releases/latest) **5.2.0++**

## Installation
1. Extract `DiscordIntegration.dll` and its dependencies from `Plugin.tar.gz`.
2. Place `DiscordIntegration.dll` inside the EXILED `Plugins` folder like any other plugin and its dependencies in the `Plugins/dependencies` folder.

## How to create a Discord bot
1. Go to https://discord.com/developers/applications and create a new application.
2. Inside of the application page under "settings" click Bot & build the bot.
3. After creating the bot you can change the bots username and avatar if you wish.

Bot token is located on the Bot page under username, Do NOT share the token because people can control the bot if they get it.

**Note that before generating the link, check that you have applications.commands scope.**

## How to run the bot

Open the bot once to let it automatically generate config.yml and synced-roles.yml files.
Remember to always wrap configs with quotation marks, even if it's not necessary for strings.

### Windows

1. Double-click the .exe file.

### Linux

1. Run `cd path/to/bot` replacing `path/to/bot` with the path of where the extracted bot is located.
2. Run `./DiscordIntegration.Bot-Linux`.

## How configure the execution of game commands through Discord

1. Open your bot `config.yml` file.
2. Add to the `CommandChannel` config, channel IDs in which commands are allowed to be executed.

```json  "Channels": {
    "1": {
      "Logs": {
        "Commands": [
          940821568186109974
        ],
        "GameEvents": [
          940821568186109974
        ],
        "Bans": [
          940821568186109974
        ],
        "Reports": [
          940821568186109974
        ],
        "StaffCopy": [
          940821568186109974
        ]
      },
      "TopicInfo": [
        940821568186109974
      ],
      "CommandChannel": [
        940821568186109974
      ]
    }
  },
```

3. Add role IDs and list every command they can execute. You can use `.*` to permit to that role ID to use all game commands without restrictions.

```json
  "ValidCommands": {
    "1": {
      "953784342595915779": [
        "di"
      ]
    }
  },
 ```

4. **Never duplicate commands.** Higher roles on your Discord server will be able to use lower roles commands as well, based on the position of the roles.

## Available commands

| Command | Description | Arguments | Permission | Example |
| --- | --- | --- | --- | --- |
| di playerlist | Gets the list of players in the server. | | di.playerlist | **di playerlist** |
| di stafflist | Gets the list of staffers in the server. | | di.stafflist | **di stafflist** |
