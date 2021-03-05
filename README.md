# DiscordIntegration

A bot and server plugin to allow server logs to be sent to Discord channels, and for server commands to be run via the Discord bot.

## Minimum requirements
[Node.js](https://nodejs.org/en/) **14.16.0+**

[EXILED](https://github.com/Exiled-Team/EXILED/releases/latest) **2.1.30+**

## Installation
1. Extract `DiscordIntegration.dll` and its dependencies from `Plugin.tar.gz`, then extract `DiscordIntegration.Bot.tar.gz` in any folder you want.
2. Place `DiscordIntegration.dll` inside the EXILED `Plugins` folder like any other plugin and its dependencies in the `Plugins/dependencies` folder.

## How to install Node.js and npm

### Windows
1. Download Node.js from https://nodejs.org/en/.
2. Open the executable file you just downloaded and install it.
3. Check your Node.js and npm versions with PowerShell or command prompt with `node -v` and `npm -v` commands to check if you installed both of them correctly.

### Linux
1. Open this [link](https://nodejs.org/en/download/package-manager/).
2. Select your linux distro.
3. Follow instructions.

## How to install bot dependencies

### Windows

1. Open PowerShell or command prompt.
2. Run `cd path\to\bot` replacing `path\to\bot` with the path of where the extracted bot is located, make sure `discordIntegration.js` and `package.json` are in the same folder.
3. Run `npm install`.

### Linux

1. Run `cd path/to/bot` replacing `path/to/bot` with the path of where the extracted bot is located, make sure `discordIntegration.js` and `package.json` are in the same folder.
2. Run `npm install`.

## How to run the bot

Open the bot once to let it automatically generate config.yml and synced-roles.yml files.
Remember to always wrap configs with quotation marks, even if it's not necessary for strings.

### Windows

1. Open PowerShell or command prompt.
2. Run `cd path\to\bot` replacing `path\to\bot` with the path of where the extracted bot is located.
2. Run `node discordIntegration.js`.

### Linux

1. Run `cd path/to/bot` replacing `path/to/bot` with the path of where the extracted bot is located.
2. Run `node discordIntegration.js`.

## How to forcefully update Node.js and npm on linux

1. Run `npm install -g n`.
2. Run `n latest` to install the latest version or `n stable` to install the stable version.

## How configure the execution of game commands through Discord

1. Open your bot `config.yml` file.
2. Add to the `command` config, channel IDs in which commands are allowed to be executed.

```yaml
channels:
  command:
    - "channel-id-1"
    - "channel-id-2"
    - "channel-id-3"
```

3. Add role IDs and list every command they can exeute. You can use `.*` to permit to that role ID to use all game commands without restrictions.

```yaml
commands:
  "role-1":
    - "di"
    - "discordintegration"
  "role-2":
    - ".*"
 ```

4. **Never duplicate commands.** Higher roles on your Discord server will be able to use lower roles commands as well, based on the position of the roles.

## Available commands

| Command | Description | Arguments | Permission | Example |
| --- | --- | --- | --- | --- |
| di add role | Adds a role-group pair to the SyncedRole list. | **[RoleID] [Group name]** | di.add.role | **di add role 656673336402640902 helper** |
| di add user | Adds an userID-discordID pair to the SyncedRole list. | **[UserID] [DiscordID]** | di.add.user | **di add user 76561198023272004@steam 219862538844635136** |
| di remove role | Removes a role-group pair from the SyncedRole list. | **[RoleID]** | di.remove.role | **di remove role 656673336402640902** |
| di remove user | Removes an userID-discordID pair from the SyncedRole list. | **[UserID]** | di.remove.user | **di remove user 76561198023272004@steam** |
| di reload | Reloads bot syncedroles and configs if connected, reloads plugin language. | | di.reload | **di reload** |
| di reload configs | Reloads bot configs if connected. | | di.reload.configs | **di reload configs** |
| di reload language | Reloads plugin language. | | di.reload.language | **di reload language** |
| di reload syncedroles | Reloads bot syncedroles if connected. | | di.reload.syncedroles | **di reload syncedroles** |
| di playerlist | Gets the list of players in the server. | | di.playerlist | **di playerlist** |
| di stafflist | Gets the list of staffers in the server. | | di.stafflist | **di stafflist** |
