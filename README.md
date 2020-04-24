# DiscordIntegration

***Requires EXILED_Events to be installed***
A bot and server plugin to allow server logs to be sent to Discord channels, and for server commands to be run via the discord bot.

Installation:
1. Extract the Plugin.dll and the bot.zip archive from the initial archive.
2. Place the plugin dll inside the EXILED "Plugins" folder like any other plugin.
3. Place the included "SerializedData.dll" file in "Plugins/dependenciies".
4. Extract the included bot.zip archive, then edit the "IntegrationBotConfig.json" file.
5. In this file, you will need to enter your preferred Prefix, the token for the Discord bot to use, the ID of the channel you want RA commands to be logged to, and the channel to log everything else to. If you want to allow people to run commands via the bot, you need to add the command to the "AllowedCommands" list, and assign it a required permission level to be used, like so ``"AllowedCommands":{"list":0, "command":1, "anothercommand":3}`` etc.. Note that only the command name needs to be present, not it's arguments. IE: "bc" would allow people to run "bc 10 hello".

In the above example, anyone can run the command "list", people with permission level of 1 or higher can run "command", and people with permission level 3 or higher can run "anothercommand".

To setup permission levels, simply enter the ID of the role you want to receive a specific permission level in the appropriate config field for the desired permission level. Note, that currently each permission level supports a single RoleID only, if a user has more than one role granting them a permission level, the highest level will be used. A user with no roles that grant permission level, will have a permission level of 0.

```Bot Config```
Default values and explination:
Bot Prefix: ! - the prefix used before bot commands to tell the bot you want it to do something, alternatively, you can @ the bot instead.

BotToken: blank - the Discord bot login token used for the bot.

Port: The port number of the server you want the bot to connect to. This must be the same as the in-game port for that SCP server.

PermLevel1Id: 0 - the RoleID to which you want to assign level 1 permissions to.

PermLevel2Id: 0 - the RoleId to which you want to assign level 2 permissions to.

PermLevel3Id: 0 - the RoleId to which you want to assign level 3 permissions to.

PermLevel4Id: 0 - the RoleId to which you want to assign level 4 permissions to.

CommandLogChannelId: 0 - the ChannelId where the bot will log RA Command usage to.

GameLogChannelId: 0 - the ChannelId where the bot will log everything else that happens in-game to.

AllowedCommands: {} - the dictionary of "command":PermissionLevelRequired commands that you want the bot to be able to run on the SCP server.

```Plugin Config```
All values go in EXILED/ServerPort-config.yml and are specific to the server to which the config file belongs to.

discord_ra_commands: true - logs use of RA commands (even those run through the bot) to the CommandLogChannel.
discord_round_start: true - will log when a new round starts.
discord_waiting_for_players: true - will log when the server has generated a new map and is ready for new players to connect.
discord_cheater_report: true - will log whenever a cheater report is filed on the server.
discord_player_hurt: true - will log all instances of a player taking damage from any source. Friendly Fire damage will be in Bold.
discord_player_death: true - will log all instances of a player dieing from any source. Teamkills will be in Bold.
discord_grenade_thrown: true - will log when a player throws any type of grenade.
discord_medical_item: true - will log when a player uses a medical item.
discord_set_class: true - will log everytime a players in-game class/role is changed.
discord_respawn: true - will log when server respawns occur.
discord_player_join: true - will log when a player joins the server.

Unless otherwise stated, all logs are sent to the "GameLogChannel" defined by the bot's config.
To disable logging for any particular event, simply change it's value to false, and it will be ignored.
