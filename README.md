# DiscordIntegration

A bot and server plugin to allow server logs to be sent to Discord channels, and for server commands to be run via the Discord bot.

## Minimum requirements
[Node.js](https://nodejs.org/en/) **14.16.0+**
[EXILED](https://github.com/Exiled-Team/EXILED/releases/latest) **2.1.30+**

## Installation
1. Extract `DiscordIntegration.dll` and its dependencies from `DiscordIntegration.Bot.tar.gz`, then extract `Plugin.tar.gz` in any folder you want.
2. Place `DiscordIntegration.dll` inside the EXILED `Plugins` folder like any other plugin and its dependencies in the `Plugins/dependencies` folder.

## How to install Node.js and npm

# Windows
1. Download Node.js from https://nodejs.org/en/.
2. Open the executable file you just downloaded and install it.
3. Check your Node.js and npm versions with PowerShell or command prompt with the command `node -v` and `npm -v`, to check if you installed them correctly.

# Linux
1. Open this [link](https://nodejs.org/en/download/package-manager/).
2. Select your linux distro.
3. Follow instructions.

## How to install bot dependencies

# Windows

1. Open PowerShell or command prompt.
2. Run `cd path\to\bot` replacing `path\to\bot` with the path of where the extracted bot is located, make sure `discordIntegration.js` and `package.json` are in the same folder.
3. Run `npm install`.

# Linux

1. Run `cd path/to/bot` replacing `path/to/bot` with the path of where the extracted bot is located, make sure `discordIntegration.js` and `package.json` are in the same folder.
2. Run `npm install`.

## How to run the bot

# Windows

1. Open PowerShell or command prompt.
2. Run `cd path\to\bot` replacing `path\to\bot` with the path of where the extracted bot is located.
2. Run `node discordIntegration.js`.

# Linux

1. Run `cd path/to/bot` replacing `path/to/bot` with the path of where the extracted bot is located.
2. Run `node discordIntegration.js`.

## How to forcefully update Node.js and npm on linux

1. Run `npm install -g n`.
2. Run `n stable` to install the stable version.
3. Run `n latest` to install the latest version.
