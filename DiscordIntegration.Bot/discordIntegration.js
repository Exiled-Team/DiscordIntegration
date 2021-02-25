const discord = require('discord.js');
const fs = require('fs');
const yaml = require('js-yaml');
const camelCaseKeys = require('camelcase-keys');
const snakeCaseKeys = require('snakecase-keys');
const sleep = require('util').promisify(setTimeout);

const configPath = './config.yml';
const syncedRolesPath = './synced-roles.yml';
const discordClient = new discord.Client();
const tcpServer = require('net').createServer();

/**
 * @type {discord.Guild}
 * */
let discordServer = null;
let config = {
  token: '',
  prefix: '+',
  channels: {
    log: {
      commands: [
        'channel-id-1'
      ],
      gameEvents: [
        'channel-id-2'
      ],
      bans: [
        'channel-id-3'
      ],
    },
    topic: [
      'channel-id-4'
    ],
    command: [
      'channel-id-5'
    ],
  },
  commands: {
    'role-id-1': [ 'di', 'discordintegration' ]
  },
  discordServer: {
    id: ''
  },
  tcpServer: {
    port: 9000,
    ipAddress: '127.0.0.1'
  },
  keepAliveInterval: 2000,
  messagesDelay: 1000,
  isDebugEnabled: false
};
let syncedRoles = {
  roleToGroup: {},
  userIdToDiscordId: {}
};
let messagesQueue = {};
let sockets = [];
let remoteCommands = {
  "loadConfigs": loadConfigs,
  "loadSyncedRoles": loadSyncedRoles,
  "saveSyncedRoles": saveSyncedRoles,
  "getGroupFromId": getGroupFromId,
  "queueMessage": queueMessage,
  "sendMessage": sendMessage,
  "log": log,
  "updateChannelTopic": updateChannelTopic,
  "updateChannelsTopic": updateChannelsTopic,
  "updateActivity": updateActivity,
  "addUser": addUser,
  "removeUser": removeUser,
  "addRole": addRole,
  "removeRole": removeRole
};

/**
 * Logs in the bot and starts a TCP server.
 */
discordClient.on('ready', async () => {
  await discordClient.user.setActivity('for connections.', {type: "LISTENING"});
  await discordClient.user.setStatus('dnd');

  console.log(`[DISCORD][INFO] Successfully logged in as ${discordClient.user.tag}.`);
  console.log(`[NET][INFO] Starting server at ${config.tcpServer.ipAddress}:${config.tcpServer.port}...`);

  tcpServer.listen(config.tcpServer.port, config.tcpServer.ipAddress);
  tcpServer.ref();

  discordClient.guilds.fetch(config.discordServer.id)
    .then(result => discordServer = result)
    .catch(error => {
      console.error(`[DISCORD][ERROR] Invalid Discord server ID: ${error.message}`);
      process.exit(0);
    });

  await handleMessagesQueue();
});

/**
 * Handles commands from Discord.
 */
discordClient.on('message', message => {
  if (!config.channels.command || message.author.bot || !message.content.startsWith(config.prefix) || !config.channels.command.includes(message.channel.id))
    return;

  if (sockets.length === 0) {
    message.channel.send('Server is not connected.');
    return;
  }

  const command = message.content.substring(config.prefix.length, message.content.length);

  if (command.length === 0) {
    message.channel.send('Command cannot be empty.');
    return;
  }

  let commandInfo = canExecuteCommand(message.member, command.toLowerCase());

  if (!commandInfo.exists) {
    message.channel.send('Invalid command.');
    return;
  }
  else if (!commandInfo.hasRole) {
    message.channel.send('Permission denied.');
    return;
  }

  if (config.isDebugEnabled)
    console.debug(`[DISCORD][DEBUG] ${message.author.tag} (${message.author.id}) executed a command: [${command}]`);

  sockets.forEach(socket => socket.write(JSON.stringify({action: 'executeCommand', parameters: {channelId: message.channel.id, content: command, user: {id: message.author.id + '@discord', name: message.author.tag}}}) + '\0'));
});

/**
 * Handles Discord warns.
 */
discordClient.on('warn', warn => console.warn(`[DISCORD][WARN] ${warn}`));

/**
 * Handles Discord errors.
 */
discordClient.on('error', error => console.error(`[DISCORD][ERROR] ${error}`));

/**
 * Listens at a specified IPAddress:Port pair.
 */
tcpServer.on('listening', () => console.log(`[NET][INFO] Server successfully started at ${config.tcpServer.ipAddress}:${config.tcpServer.port}.`));

/**
 * Handles TCP server errors.
 */
tcpServer.on('error', error => {
  console.error(`[NET][ERROR] ${error === 'EADDRINUSE' ? `${config.address}:${config.port} is already in use!` : `${error}`}`);
  process.exit(0);
});

/**
 * Handles connections to the TCP server.
 */
tcpServer.on('connection', socket => {
  socket.setEncoding('UTF-8');
  socket.setKeepAlive(true, config.keepAliveDuration);

  sockets.push(socket);

  console.log(`[NET][INFO] Connection estabilished with ${socket.address().address}:${socket.address().port}.`);

  socket.on('data', (buffer) => {
    buffer.split('\0').forEach(async remoteCommand =>
    {
      if (!remoteCommand)
        return;

      try {
        remoteCommand = JSON.parse(remoteCommand);

        if (typeof remoteCommand.action !== 'undefined' && remoteCommand.action in remoteCommands) {
          const returnedValue = await remoteCommands[remoteCommand.action](...remoteCommand.parameters);

          if (returnedValue)
            socket.write(returnedValue + '\0');
        }

      } catch (exception) {
        console.error(`[NET][ERROR] An error has occurred while receiving data from ${socket?.address()?.address}:${socket?.address()?.port}: ${exception}`);
      }
    });    
  });

  socket.on('error', error => {
    if (error.message.includes('ECONNRESET')) {
      console.info('[SOCKET][INFO] Server closed connection.');
      log('gameEvents', '```diff\n- Server closed connection.\n```', true);
    } else {
      console.error(`[SOCKET][ERROR] Server closed connection: ${error}.`);
      log('gameEvents', `\`\`\`diff\n - Server closed connection: ${error}.\n\`\`\``, true);
    }
  });

  socket.on('close', () => sockets.splice(sockets.indexOf(socket), 1));
});

/**
 * Check whether a command can be executed or not.
 * @param {discord.GuildMember} member The user that executed the command
 * @param {string} command The command to be executed
 */
function canExecuteCommand(member, command) {
  const commandInfo = {
    hasRole: false,
    exists: false
  };

  if (!config.commands || !member)
    return commandInfo;

  for (const roleId in config.commands) {
    const tempHasRole = member.roles.cache.has(roleId);

    commandInfo.exists = config.commands[roleId].some(tempCommand => typeof command === 'string' && (command.startsWith(tempCommand.toLowerCase()) || (tempHasRole && tempCommand === '.*')));

    if (commandInfo.exists) {
      const role = discordServer.roles.cache.get(roleId)
      
      commandInfo.hasRole = role && member.roles.highest.position >= role.position;
      break;
    }
  }

  return commandInfo;
}

/**
 * Loads bot configs.
 */
function loadConfigs() {
  console.log('[BOT][INFO] Loading configs...');

  try {
    if (!fs.existsSync(configPath)) {
      console.error('[BOT][ERROR] Config file wasn\'t found! Generating...');

      fs.writeFileSync(configPath, yaml.dump(snakeCaseKeys(config)));

      console.error('[BOT][ERROR] Config generated! Closing...');
      process.exit(0);
    }

    const tempConfig = camelCaseKeys(yaml.load(fs.readFileSync(configPath)), {deep: true});

    if (tempConfig)
      config = tempConfig;
    else {
      console.error('[BOT][ERROR] Config file is empty! Closing...');
      process.exit(0);
    }
  } catch (exception) {
    console.error(`[BOT][ERROR] Error while loading configs: ${exception}`);
    process.exit(0);
  }

  console.log('[BOT][INFO] Configs loaded successfully.');
}

/**
 * Loads synced roles.
 */
function loadSyncedRoles() {
  console.log('[BOT][INFO] Loading synced roles...');

  try {
    if (!fs.existsSync(syncedRolesPath)) {
      console.error('[BOT][ERROR] Synced roles file wasn\'t found! Generating...');

      fs.writeFileSync(syncedRolesPath, yaml.dump(snakeCaseKeys(syncedRoles)));
    }

    const tempSyncedRoles = camelCaseKeys(yaml.load(fs.readFileSync(syncedRolesPath)), {deep: true});

    if (tempSyncedRoles)
      syncedRoles = tempSyncedRoles;

  } catch (exception) {
    console.error(`[BOT][ERROR] Error while loading synced roles: ${exception}`);
    return;
  }

  console.log('[BOT][INFO] Synced roles loaded successfully.');
}

/**
 * Saves synced roles.
 */
function saveSyncedRoles() {
  console.log('[BOT][INFO] Saving synced roles...');

  try {
    fs.writeFileSync(syncedRolesPath, yaml.dump(snakeCaseKeys(syncedRoles, {deep: false})));
  } catch (exception) {
    console.error(`[BOT][ERROR] Error while saving synced roles: ${exception}`);
  }

  console.log('[BOT][INFO] Synced roles saved successfully.');
}

/**
 * Gets the user's group from its user id.
 * 
 * @param {string} id The user id.
 */
async function getGroupFromId(id) {
  let obtainedId = id.substring(0, id.lastIndexOf('@'));

  if (obtainedId.length === 17) {
    if (id in syncedRoles.userIdToDiscordId)
      obtainedId = syncedRoles.userIdToDiscordId[id];
    else
      return;
  }

  let user;

  try {
    user = await discordServer.members.fetch(obtainedId);
  } catch (exception) {
    console.error(`[BOT][ERROR] Cannot sync ${id} (${obtainedId}) user ID, user not found! ${exception}`);
    return;
  }

  let group;

  for (const role of user.roles.cache.array()) {
    group = syncedRoles.roleToGroup[role.id];

    if (group)
      break;
  }

  if (!group) {
    console.error(`[BOT][ERROR] Cannot find group of ${id} (${obtainedId}) in synced roles.`);
    return;
  }

  return JSON.stringify({action: 'setGroupFromId', parameters: {id: id, group: group}});
}

/**
 * Queues a message to in a specific Discord channel.
 * @param {string} channelId The channel id.
 * @param {string} content The content to be sent.
 */
function queueMessage(channelId, content, shouldLogTimestamp = true) {
  if (shouldLogTimestamp)
    content = `[${(new Date()).toLocaleTimeString()}] ${content}`;

  if (channelId in messagesQueue)
    messagesQueue[channelId] += '\n' + content;
  else
    messagesQueue[channelId] = content;
}

/**
 * Sends a message in a specific Discord channel.
 * @param {string} channelId
 * @param {string} content
 */
function sendMessage(channelId, content, shouldLogTimestamp = false) {
  if (shouldLogTimestamp)
    content = `[${(new Date()).toLocaleTimeString()}] ${content}`;

  const channel = discordServer.channels.cache.find(channel => channel.id === channelId);

  channel?.send(content.replace(/(@here)|(@everyone)|(<@[0-9]*>)/gm, '`$&`'), {split: true})
    .then(result => {

      if (config.isDebugEnabled)
        console.debug(`[DISCORD][DEBUG] "${result}" message has been sent in "${channel.name}" (${channel.id}).`);
    })
    .catch(error => console.error(`[DISCORD][ERROR] Cannot send message in "${channel.name}" (${channel.id}): ${error}`));
}

/**
 * Logs an event, command or ban in every configurated Discord channel.
 *
 * @param {string} content The content to be logged.
 */
function log(type, content, isInstant = false) {
  if (!config.channels.log[type])
    return;

  config.channels.log[type].forEach(channelId => isInstant ? sendMessage(channelId, content, true) : queueMessage(channelId, content));
}

/**
 * Changes the topic of a specific Discord channel.
 * 
 * @param {string} channelId The channel id
 * @param {string} newTopic The new topic to be set.
 */
function updateChannelTopic(channelId, newTopic) {
  const channel = discordServer.channels.cache.find(channel => channel.id === channelId);

  channel?.setTopic(newTopic)
    .then(result => {
      if (config.isDebugEnabled)
        console.debug(`[DISCORD][DEBUG] Topic of "${result.name}" (${result.id}) has been changed to "${result.topic}".`);
    })
    .catch(error => console.error(`[DISCORD][ERROR] Cannot change "${channel.name}" (${channel.id}) topic: ${error}`));
}

/**
 * Changes the topic of specific Discord channels.
 *
 * @param {string} newTopic The new topic to be set.
 */
function updateChannelsTopic(newTopic) {
  if (!config.channels.topic)
    return;

  config.channels.topic.forEach(channelId => updateChannelTopic(channelId, newTopic));
}

/**
 * Updates the bot activity.
 * @param {string} newActivity The new activity.
 */
function updateActivity(newActivity) {
  discordClient.user.setActivity(newActivity)
    .then(async presence => {
      if (config.isDebugEnabled)
        console.debug(`[DISCORD][DEBUG] Bot activity has been set to "${presence.activities}".`);

      await discordClient.user.setStatus(newActivity.startsWith('0') ? 'idle' : 'online');
    })
    .catch(error => console.error(`[DISCORD][ERROR] Cannot set bot activity: ${error}`));
}

/**
 * Adds an userID-discordID pair to the SyncedRole list.
 * @param {string} userId The user ID.
 * @param {string} discordId The discord ID.
 * @param {object} sender The command sender.
 */
function addUser(userId, discordId, sender) {
  syncedRoles.userIdToDiscordId[userId] = discordId;

  saveSyncedRoles();

  return JSON.stringify({action: 'commandReply', parameters: {sender, response: `${userId}: ${discordId} user ID - Discord ID pair has been added to synced roles.`, isSucceeded: true}});
}

/**
 * Removes an userID-discordID pair from the SyncedRole list.
 * @param {string} userId The user ID.
 * @param {object} sender The command sender.
 */
function removeUser(userId, sender) {
  if (!syncedRoles || !(userId in syncedRoles.userIdToDiscordId))
    return JSON.stringify({action: 'commandReply', parameters: {sender, response: `${userId} user ID wasn't present in synced roles!`, isSucceeded: false}});

  delete syncedRoles.userIdToDiscordId[userId];

  saveSyncedRoles();

  return JSON.stringify({action: 'commandReply', parameters: {sender, response: `${userId} user ID has been removed from synced roles.`, isSucceeded: true}});
}

/**
 * Adds a role-group pair to the SyncedRole list.
 * @param {string} roleId The role ID.
 * @param {string} groupName The group name.
 * @param {object} sender The command sender.
 */
function addRole(roleId, group, sender) {
  if (!discordServer.roles.cache.has(roleId)) {
    return JSON.stringify({action: 'commandReply', parameters: {sender, response: `${roleId} role ID wasn't found!`, isSucceeded: false}});
  }

  syncedRoles.roleToGroup[roleId] = group;

  saveSyncedRoles();

  return JSON.stringify({action: 'commandReply', parameters: {sender, response: `${roleId}: ${group} role ID - group pair has been added to synced roles.`, isSucceeded: true}});
}

/**
 * Removes a role-group pair from the SyncedRole list.
 * @param {string} roleId The role ID.
 * @param {object} sender The command sender.
 */
function removeRole(roleId, sender) {
  if (!syncedRoles || !(roleId in syncedRoles.roleToGroup))
    return JSON.stringify({action: 'commandReply', parameters: {sender, response: `${roleId} role ID wasn't present in synced roles!`, isSucceeded: false}});

  delete syncedRoles.roleToGroup[roleId];

  saveSyncedRoles();

  return JSON.stringify({action: 'commandReply', parameters: {sender, response: `${roleId} role ID has been removed from synced roles.`, isSucceeded: true}});
}

/**
 * Closes the bot.
 */
async function close() {
  await discordClient.user.setStatus('invisible');
  await discordClient.user.setActivity('');

  log('gameEvents', '```diff\n- Bot closed.\n```', true);

  sockets.forEach(socket => socket.destroy());

  sockets = [];
  
  tcpServer.close(() => {
    console.info('[NET][INFO] Server closed.');
    tcpServer.unref();
  });
}

/**
 * Handles queued messages sent from clients to the Discord server.
 */
async function handleMessagesQueue() {
  for (; ;) {
    for (const channelId in messagesQueue)
      sendMessage(channelId, messagesQueue[channelId]);

    messagesQueue = {};

    await sleep(config.messagesDelay);
  }
}

loadConfigs();
loadSyncedRoles();

console.log('[DISCORD][INFO] Logging in...');

discordClient.login(config.token)
  .catch(error => {
    console.error(`[DISCORD][ERROR] Login has failed: ${error}`);
    process.exit(0);
  });

['exit', 'SIGINT', 'SIGTERM', 'SIGHUP', 'SIGUSR1', 'SIGUSR2'].forEach(event => {
  process.on(event, async () => await close());
});