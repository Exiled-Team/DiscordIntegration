const discord = require('discord.js');
const fs = require('fs');
const yaml = require('js-yaml');
const camelCaseKeys = require('camelcase-keys');
const snakeCaseKeys = require('snakecase-keys');
const sleep = require('util').promisify(setTimeout)

const configPath = './config.yml';
const syncedRolesPath = './synced-roles.yml';
const discordClient = new discord.Client();
const tcpServer = require('net').createServer();

let discordServer = null;
let config = null;
let syncedRoles = {
  roleToGroup: {},
  userIdToDiscordId: {}
}
let messagesQueue = {};
let sockets = [];

/**
 * Logs in the bot and starts a TCP server.
 */
discordClient.on('ready', async () => {
  await discordClient.user.setActivity('for connections.', {type: "LISTENING"});
  await discordClient.user.setStatus('dnd');

  this.handleMessagesQueue();

  console.log(`[DISCORD][INFO] Successfully logged in as ${discordClient.user.tag}.`)
  console.log(`[NET][INFO] Starting server at ${config.tcpServer.ipAddress}:${config.tcpServer.port}...`)

  tcpServer.listen(config.tcpServer.port, config.tcpServer.ipAddress);
  tcpServer.ref();

  discordClient.guilds.fetch(config.discordServer.id).then(result => discordServer = result)
    .catch(error => {
      console.error(`[DISCORD][ERROR] Invalid Discord server ID: ${error.message}`);
      process.exit(0);
  });
});

/**
 * Handles commands from Discord.
 */
discordClient.on('message', message => {
  if (message.author.bot || !message.content.startsWith(config.prefix) || !config.channels.commands.includes(message.channel.id))
    return;

  const command = message.content.substring(config.prefix.length, message.content.length).toLowerCase();

  if (command.length === 0 || !this.canExecuteCommand(message.member, command))
    return;

  if (config.isDebugEnabled)
    console.debug(`[DISCORD][DEBUG] ${message.author.tag} (${message.author.id}) executed a command: [${command}]`);

  sockets.forEach(socket => socket.write(JSON.stringify({action: 'executeCommand', parameters: {channelId: message.channel.id, content: command, user: {id: message.author.id, name: message.author.tag}}}) + '\0'));
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

      remoteCommand = JSON.parse(remoteCommand);

      if (typeof remoteCommand.action !== 'undefined' && remoteCommand.action in this) {
        const returnedValue = await this[remoteCommand.action](...remoteCommand.parameters);

        if (returnedValue)
          socket.write(returnedValue + '\0');
      } 
    });    
  });

  socket.on('error', error => {
    if (error.message.includes('ECONNRESET')) {
      console.info('[SOCKET][INFO] Server closed connection.');
      this.log('gameEvents', '```diff\n- Server closed connection.\n```', true);
    } else {
      console.error(`[SOCKET][ERROR] Server closed connection: ${error}.`);
      this.log('gameEvents', `\`\`\`diff\n - Server closed connection: ${error}.\n\`\`\``, true);
    }
  });

  socket.on('close', () => sockets.splice(sockets.indexOf(socket), 1));
});

/**
 * Check whether a command can be executed or not.
 * @param {any} member The user that executed the command
 * @param {any} command The command to be executed
 */
exports.canExecuteCommand = function(member, command) {
  if (!config.commands)
    return false;

  for (const roleId in config.commands) {
    if (member.roles.cache.has(roleId) && config.commands[roleId].some(tempCommand => command.startsWith(tempCommand)))
      return true;
  }

  return false;
}

/**
 * Loads bot configs.
 */
exports.loadConfigs = function () {
  let rawConfig;

  console.log('[BOT][INFO] Loading configs...');

  try {
    if (fs.existsSync(configPath)) {
      rawConfig = fs.readFileSync(configPath);
    } else {
      console.error('[BOT][ERROR] Config file wasn\'t found! Closing...');
      process.exit(0);
    }

    config = camelCaseKeys(yaml.load(rawConfig), { deep: true });
  } catch (exception) {
    console.error(`[BOT][ERROR] Error while loading configs: ${exception}`);
    process.exit(0);
  }

  console.log('[BOT][INFO] Configs loaded successfully.');
}

/**
 * Loads synced roles.
 */
exports.loadSyncedRoles = function () {
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
exports.saveSyncedRoles = function () {
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
 * @param {any} id The user id.
 */
exports.getGroupFromId = async function (id) {
  let obtainedId = id.substring(0, id.lastIndexOf('@'));

  if (obtainedId.length === 17) {
    if (id in syncedRoles.userIdToDiscordId)
      obtainedId = syncedRoles.userIdToDiscordId[id];
    else
      return;
  }

  const user = await discordServer.members.fetch(obtainedId).catch(console.error(`[BOT][ERROR] Cannot sync ${id} user ID, user not found!`));

  if (!user)
    return;

  let group;

  for (const role of user.roles.cache.array()) {
    group = syncedRoles.roleToGroup[role.id];

    if (group)
      break;
  };

  if (!group) {
    console.error(`[BOT][ERROR] Cannot find group of ${id} (${obtainedId}) in synced roles.`);
    return;
  }

  return JSON.stringify({action: 'setGroupFromId', parameters: {id: id, group: group}});
}

/**
 * Queues a message to in a specific Discord channel.
 * @param {any} channelId The channel id.
 * @param {any} content The content to be sent.
 */
exports.queueMessage = function (channelId, content, shouldLogTimestamp = true) {
  if (shouldLogTimestamp)
    content = `[${(new Date()).toLocaleTimeString()}] ${content}`;

  if (channelId in messagesQueue)
    messagesQueue[channelId] += '\n' + content;
  else
    messagesQueue[channelId] = content;
}

/**
 * Sends a message in a specific Discord channel.
 * @param {any} channelId
 * @param {any} content
 */
exports.sendMessage = function (channelId, content, shouldLogTimestamp = false) {
  if (shouldLogTimestamp)
    content = `[${(new Date()).toLocaleTimeString()}] ${content}`;

  const channel = discordServer.channels.cache.find(channel => channel.id === channelId);

  channel?.send(content, {split: true})
    .then(result => {

      if (config.isDebugEnabled)
        console.debug(`[DISCORD][DEBUG] "${result}" message has been sent in "${channel.name}" (${channel.id}).`);
    })
    .catch(error => console.error(`[DISCORD][ERROR] Cannot send message in "${channel.name}" (${channel.id}): ${error}`));
}

/**
 * Logs an event, command or ban in every configurated Discord channel.
 *
 * @param {any} content The content to be logged.
 */
exports.log = function (type, content, isInstant = false) {
  if (!config.channels[type])
    return;

  console.log(type, content);

  config.channels[type].forEach(channelId => isInstant ? this.sendMessage(channelId, content, true) : this.queueMessage(channelId, content));
}

/**
 * Changes the topic of a specific Discord channel.
 * 
 * @param {any} channelId The channel id
 * @param {any} newTopic The new topic to be set.
 */
exports.updateChannelTopic = function (channelId, newTopic) {
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
 * @param {any} newTopic The new topic to be set.
 */
exports.updateChannelsTopic = function (newTopic) {
  if (!config.channels.gameEvents)
    return;

  config.channels.gameEvents.forEach(channelId => this.updateChannelTopic(channelId, newTopic));
}

/**
 * Updates the bot activity.
 * @param {any} newActivity The new activity.
 */
exports.updateActivity = function (newActivity) {
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
 * @param {any} userId The user ID.
 * @param {any} discordId The discord ID.
 */
exports.addUser = function (userId, discordId, sender) {
  syncedRoles.userIdToDiscordId[userId] = discordId;

  this.saveSyncedRoles();

  return JSON.stringify({action: 'commandReply', parameters: {sender, response: `${userId}: ${discordId} user ID - Discord ID pair has been added to synced roles.`, isSucceeded: true}});
}

/**
 * Removes an userID-discordID pair from the SyncedRole list.
 * @param {any} userId The user ID.
 */
exports.removeUser = function (userId, sender) {
  if (!syncedRoles || !(userId in syncedRoles.userIdToDiscordId))
    return JSON.stringify({action: 'commandReply', parameters: {sender, response: `${userId} user ID wasn't present in synced roles!`, isSucceeded: false}});

  delete syncedRoles.userIdToDiscordId[userId];

  this.saveSyncedRoles();

  return JSON.stringify({action: 'commandReply', parameters: {sender, response: `${userId} user ID has been removed from synced roles.`, isSucceeded: true}});
}

/**
 * Adds a role-group pair to the SyncedRole list.
 * @param {any} roleId The role ID.
 * @param {any} groupName The group name.
 */
exports.addRole = function (roleId, group, sender) {
  if (!discordServer.roles.cache.has(roleId)) {
    return JSON.stringify({action: 'commandReply', parameters: {sender, response: `${roleId} role ID wasn't found!`, isSucceeded: false}});
  }

  syncedRoles.roleToGroup[roleId] = group;

  this.saveSyncedRoles();

  return JSON.stringify({action: 'commandReply', parameters: {sender, response: `${roleId}: ${group} role ID - group pair has been added to synced roles.`, isSucceeded: true}});
}

/**
 * Removes a role-group pair from the SyncedRole list.
 * @param {any} roleId The role ID.
 */
exports.removeRole = function (roleId, sender) {
  if (!syncedRoles || !(roleId in syncedRoles.roleToGroup))
    return JSON.stringify({action: 'commandReply', parameters: {sender, response: `${roleId} role ID wasn't present in synced roles!`, isSucceeded: false}});

  delete syncedRoles.roleToGroup[roleId];

  this.saveSyncedRoles();

  return JSON.stringify({action: 'commandReply', parameters: {sender, response: `${roleId} role ID has been removed from synced roles.`, isSucceeded: true}});
}

/**
 * Closes the bot.
 */
exports.close = async function () {
  await discordClient.user.setStatus('invisible');
  await discordClient.user.setActivity('');

  this.log('gameEvents', '```diff\n- Bot closed.\n```', true);

  sockets.forEach(socket => socket.destroy());

  tcpServer.close(() => {
    console.info('[NET][INFO] Server closed.');
    tcpServer.unref();
  });
}

/**
 * Handles queued messages sent from clients to the Discord server.
 */
exports.handleMessagesQueue = async function handleMessagesQueue() {
  for (; ;) {
    for (const channelId in messagesQueue)
      this.sendMessage(channelId, messagesQueue[channelId]);

    messagesQueue = {};

    await sleep(config.messagesDelay);
  }
}

this.loadConfigs();
this.loadSyncedRoles();

console.log('[DISCORD][INFO] Logging in...');

discordClient.login(config.token)
  .catch(error => {
    console.error(`[DISCORD][ERROR] Login has failed: ${error}`);
    process.exit(0);
  });

['exit', 'SIGINT', 'SIGTERM', 'SIGHUP', 'SIGUSR1', 'SIGUSR2'].forEach(event => {
  process.on(event, () => this.close());
});