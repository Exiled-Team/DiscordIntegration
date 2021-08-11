// -----------------------------------------------------------------------
// <copyright file="Language.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration
{
    using System;
    using System.IO;
    using Exiled.API.Features;
    using Newtonsoft.Json;
    using static DiscordIntegration;

    /// <summary>
    /// Represents the plugin language.
    /// </summary>
    [JsonObject(ItemRequired = Required.Always)]
    public sealed class Language
    {
        private readonly JsonSerializer jsonSerializer = new JsonSerializer();

        /// <summary>
        /// Initializes a new instance of the <see cref="Language"/> class.
        /// </summary>
        public Language()
        {
            jsonSerializer.Error += Error;
            jsonSerializer.Formatting = Formatting.Indented;
        }

        /// <summary>
        /// Gets the language folder.
        /// </summary>
        public static string Folder { get; } = Path.Combine(Paths.Plugins, Instance.Name, "Languages");

        /// <summary>
        /// Gets the language fullpath.
        /// </summary>
        public static string FullPath => Path.Combine(Folder, $"{Instance.Config.Language}.json");

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable CS1591
        public string UsedCommand { get; set; } = ":keyboard: {0} ({1}) [{2}] used command: {3} {4}";

        public string HasRunClientConsoleCommand { get; set; } = ":keyboard: {0} ({1}) [{2}] has run a client-console command: {3} {4}";

        public string NoPlayersOnline { get; set; } = "No players online.";

        public string NoStaffOnline { get; set; } = "No staff online.";

        public string WaitingForPlayers { get; set; } = $":hourglass: Waiting for players...";

        public string RoundStarting { get; set; } = ":arrow_forward: Round starting: {0} players in round.";

        public string RoundEnded { get; set; } = ":stop_button: Round ended: {0} - Players online {1}/{2}.";

        public string PlayersOnline { get; set; } = "Players online: {0}/{1}";

        public string RoundDuration { get; set; } = "Round duration: {0}";

        public string AliveHumans { get; set; } = "Alive humans: {0}";

        public string AliveScps { get; set; } = "Alive SCPs: {0}";

        public string CheaterReportFilled { get; set; } = ":incoming_envelope: **Cheater report filled: {0} ({1}) [{2}] reported {3} ({4}) [{5}] for {6}.**";

        public string HasDamagedForWith { get; set; } = ":crossed_swords: **{0} ({1}) [{2}] has damaged {3} ({4}) [{5}] for {6} with {7}.**";

        public string HasKilledWith { get; set; } = ":skull_crossbones: **{0} ({1}) [{2}] killed {3} ({4}) [{5}] with {6}.**";

        public string ThrewAGrenade { get; set; } = ":boom: {0} ({1}) [{2}] threw a {3}.";

        public string UsedMedicalItem { get; set; } = ":medical_symbol: {0} ({1}) [{2}] healed with {3}.";

        public string ChangedRole { get; set; } = ":mens: {0} ({1}) [{2}] has been changed to a {3}.";

        public string ChaosInsurgencyHaveSpawned { get; set; } = ":spy: Chaos Insurgency has spawned with {0} players.";

        public string NineTailedFoxHaveSpawned { get; set; } = ":cop: Nine-Tailed Fox has spawned with {0} players.";

        public string HasJoinedTheGame { get; set; } = ":arrow_right: **{0} ({1}) [{2}] has joined the game.**";

        public string HasBeenFreedBy { get; set; } = ":unlock: {0} ({1}) [{2}] has been freed by {3} ({4}) [{5}].";

        public string HasBeenHandcuffedBy { get; set; } = ":lock: {0} ({1}) [{2}] has been handcuffed by {3} ({4}) [{5}].";

        public string WasKicked { get; set; } = ":no_entry: {0} ({1}) was kicked for {2}.";

        public string WasBannedBy { get; set; } = ":no_entry: {0} ({1}) was banned by {2} for {3} expire {4}.";

        public string HasStartedUsingTheIntercom { get; set; } = ":loud_sound: {0} ({1}) [{2}] has started using the intercom.";

        public string HasPickedUp { get; set; } = "{0} ({1}) [{2}] has picked up **{3}**.";

        public string HasDropped { get; set; } = "{0} ({1}) [{2}] has dropped **{3}**.";

        public string DecontaminationHasBegun { get; set; } = ":biohazard: **Decontamination has begun.**";

        public string HasEnteredPocketDimension { get; set; } = ":door: {0} ({1}) [{2}] has entered the pocket dimension.";

        public string HasEscapedPocketDimension { get; set; } = ":high_brightness: {0} ({1}) [{2}] has escaped the pocket dimension.";

        public string HasTriggeredATeslaGate { get; set; } = ":zap: {0} ({1}) [{2}] has triggered a tesla gate.";

        public string Scp914HasProcessedTheFollowingPlayers { get; set; } = ":gear: SCP-914 has processed the following players:\n **{0}**\nand items:\n **{1}**";

        public string HasClosedADoor { get; set; } = ":door: {0} ({1}) [{2}] has closed {3} door.";

        public string HasOpenedADoor { get; set; } = ":door: {0} ({1}) [{2}] has opened {3} door.";

        public string Scp914HasBeenActivated { get; set; } = ":gear: {0} ({1}) [{2}] has activated SCP-914 on setting {3}.";

        public string Scp914KnobSettingChanged { get; set; } = ":gear: {0} ({1}) [{2}] has changed the SCP-914 knob to {3}.";

        public string PlayerCanceledWarhead { get; set; } = ":no_entry: **{0} ({1}) [{2}] canceled warhead detonation sequence.**";

        public string CanceledWarhead { get; set; } = ":no_entry: **Warhead detonation sequence canceled.**";

        public string WarheadHasDetonated { get; set; } = ":radioactive: **The Alpha-warhead has detonated.**";

        public string WarheadHasBeenDetonated { get; set; } = "Warhead has been detonated.";

        public string WarheadIsCountingToDetonation { get; set; } = "Warhead is counting down to detonation.";

        public string WarheadHasntBeenDetonated { get; set; } = "Warhead has not been detonated.";

        public string PlayerWarheadStarted { get; set; } = ":radioactive: **{0} ({1}) [{2}] started the alpha-warhead countdown, detonation in: {3}.**";

        public string WarheadStarted { get; set; } = ":radioactive: **Alpha-warhead countdown initiated, detonation in: {0}.**";

        public string AccessedWarhead { get; set; } = ":key: {0} ({1}) [{2}] has accessed the Alpha-warhead detonation button cover.";

        public string CalledElevator { get; set; } = ":elevator: {0} ({1}) [{2}] has called an elevator.";

        public string UsedLocker { get; set; } = "{0} ({1}) [{2}] has opened a locker.";

        public string GeneratorClosed { get; set; } = "{0} ({1}) [{2}] has closed a generator.";

        public string GeneratorOpened { get; set; } = "{0} ({1}) [{2}] has opened a generator.";

        public string GeneratorEjected { get; set; } = "{0} ({1}) [{2}] has ejected a tablet from a generator.";

        public string GeneratorFinished { get; set; } = "Generator in {0} has finished it's charge up, {1} generators have been activated.";

        public string GeneratorInserted { get; set; } = ":calling: {0} ({1}) [{2}] has inserted a tablet into a generator.";

        public string GeneratorUnlocked { get; set; } = ":unlock: {0} ({1}) [{2}] has unlocked a generator door.";

        public string Scp106WasContained { get; set; } = "{0} ({1}) [{2}] has been contained by the Femur Breaker.";

        public string Scp106CreatedPortal { get; set; } = "{0} ({1}) [{2}] has created a portal.";

        public string Scp106Teleported { get; set; } = "{0} ({1}) [{2}] has teleported to a portal.";

        public string GainedExperience { get; set; } = "{0} ({1}) [{2}] has gained {3} XP ({4}).";

        public string GainedLevel { get; set; } = "{0} ({1}) [{2}] has gained a level: {3} :arrow_right: {4}.";

        public string LeftServer { get; set; } = ":arrow_left: **{0} ({1}) [{2}] has left the server.**";

        public string Reloaded { get; set; } = ":arrows_counterclockwise: {0} ({1}) [{2}] has reloaded their {3} weapon.";

        public string GroupSet { get; set; } = "{0} ({1}) [{2}] has been assigned to the **{3} ({4})** group.";

        public string ItemChanged { get; set; } = "{0} ({1}) [{2}] changed the item in their hand: {3} :arrow_right: {4}.";

        public string InvalidSubcommand { get; set; } = "Invalid subcommand!";

        public string Available { get; set; } = "Available";

        public string BotIsNotConnectedError { get; set; } = "The bot is not connected!";

        public string PlayerListCommandDescription { get; set; } = "Gets the list of players in the server.";

        public string StaffListCommandDescription { get; set; } = "Gets the list of staffers in the server.";

        public string ReloadConfigsCommandDescription { get; set; } = "Reloads bot configs if connected.";

        public string ReloadConfigsCommandSuccess { get; set; } = "Bot configs reload request sent successfully.";

        public string NotEnoughPermissions { get; set; } = "You need \"{0}\" permission to run this command!";

        public string ServerConnected { get; set; } = "```diff\n+ Server connected.\n```";

        public string SendingDataError { get; set; } = "An error has occurred while sending data: {0}";

        public string ReceivingDataError { get; set; } = "An error has occurred while receiving data: {0}";

        public string ConnectingError { get; set; } = "An error has occurred while connecting: {0}";

        public string SuccessfullyConnected { get; set; } = "Successfully connected to {0}:{1}.";

        public string ReceivedData { get; set; } = "Received {0} ({1} bytes) from the server.";

        public string SentData { get; set; } = "Sent {0} ({1} bytes) to server.";

        public string ConnectingTo { get; set; } = "Connecting to {0}:{1}.";

        public string ReloadLanguageCommandDescription { get; set; } = "Reloads plugin language.";

        public string ReloadLanguageCommandSuccess { get; set; } = "Language reloaded successfully!";

        public string ReloadSyncedRolesSuccess { get; set; } = "Bot synced roles reload request sent successfully.";

        public string CouldNotUpdateChannelTopicError { get; set; } = "Error! Couldn't update channel topic: {0}";

        public string InvalidUserGroupError { get; set; } = "Attempted to assign invalid user group \"{0}\" to {1}.";

        public string AssigningUserGroupError { get; set; } = "Error assigning user group to {0}, player not found.";

        public string AssingingSyncedGroup { get; set; } = "Assigning synced group \"{0}\" to {1}.";

        public string HandlingRemoteCommand { get; set; } = "Handling remote command \"{0}\" with parameters: {1} from {2}.";

        public string HandlingRemoteCommandError { get; set; } = "An error has occurred while handling a remote command: {0}";

        public string None { get; set; } = "None";

        public string InvalidParametersError { get; set; } = "You've to insert {0} parameters!";

        public string AddUserCommandDescription { get; set; } = "Adds an userID-discordID pair to the SyncedRole list.";

        public string AddUserCommandSuccess { get; set; } = "User addition request has been sent successfully.";

        public string AddRoleCommandDescription { get; set; } = "Adds a role-group pair to the SyncedRole list.";

        public string AddRoleCommandSuccess { get; set; } = "Role addition request has been sent successfully.";

        public string RemoveUserCommandDescription { get; set; } = "Removes an userID-discordID pair from the SyncedRole list.";

        public string RemoveUserCommandSuccess { get; set; } = "User deletion request has been sent successfully.";

        public string RemoveRoleCommandDescription { get; set; } = "Removes a role-group pair from the SyncedRole list.";

        public string RemoveRoleCommandSuccess { get; set; } = "Role deletion request has been sent successfully.";

        public string ReloadSyncedRolesDescription { get; set; } = "Reloads bot synced roles if connected.";

        public string InvalidDiscordIdError { get; set; } = "{0} is not a valid Discord ID!";

        public string InvalidUserdIdError { get; set; } = "{0} is not a valid user ID!";

        public string InvalidDiscordRoleIdError { get; set; } = "{0} is not a valid Discord role ID!";

        public string InvalidGroupError { get; set; } = "{0} is not a valid group!";

        public string ServerHasBeenTerminated { get; set; } = "The server has been terminated.";

        public string ServerHasBeenTerminatedWithErrors { get; set; } = "The server has been terminated with errors: {0}";

        public string UpdatingConnectionError { get; set; } = "An error has occurred while updating the connection: {0}";

        public string InvalidIPAddress { get; set; } = "{0} is not a valid IP address!";

        public string Redacted { get; set; } = "████████";

        public string NotAuthenticated { get; set; } = "Not authenticated";

        public string DedicatedServer { get; set; } = "Dedicated server";

#pragma warning restore CS1591
#pragma warning restore SA1600 // Elements should be documented

        /// <summary>
        /// Loads the language.
        /// </summary>
        public void Load()
        {
            StreamReader streamReader = new StreamReader(FullPath);

            try
            {
                jsonSerializer.Populate(streamReader, this);
            }
            catch (Exception exception)
            {
                Log.Error($"Error! Failed to read {Instance.Config.Language} language, located at \"{FullPath}\": {exception}.\nDefault translation will be used.");
                return;
            }
            finally
            {
                streamReader?.Dispose();
            }
        }

        /// <summary>
        /// Saves the language.
        /// </summary>
        /// <param name="shouldOverwrite">Indicates a value whether the file should be overwritten or not.</param>
        public void Save(bool shouldOverwrite = false)
        {
            if (File.Exists(FullPath) && !shouldOverwrite)
                return;

            try
            {
                if (!Directory.Exists(Folder))
                    Directory.CreateDirectory(Folder);
            }
            catch (Exception exception)
            {
                Log.Error($"Error! Failed to create language directory at \"{Folder}\": {exception}.");
                return;
            }

            StreamWriter streamWriter = new StreamWriter(FullPath);

            try
            {
                jsonSerializer.Serialize(streamWriter, this);
            }
            catch (Exception exception)
            {
                Log.Error($"Error! Failed to create \"{Instance.Config.Language}\" language at \"{FullPath}\": {exception}.");
                return;
            }
            finally
            {
                streamWriter?.Dispose();
            }
        }

        private void Error(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs ev)
        {
            Log.Warn($"Translation not found for \"{ev.ErrorContext.Member}\" key, loading default one...");

            ev.ErrorContext.Handled = true;
        }
    }
}
