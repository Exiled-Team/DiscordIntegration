// -----------------------------------------------------------------------
// <copyright file="DiscordIntegration.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using API;
    using API.Configs;
    using API.User;
    using Events;
    using Exiled.API.Features;
    using MEC;
    using Handlers = Exiled.Events.Handlers;
    using Version = System.Version;

    /// <summary>
    /// Link a Discord server with an SCP: SL server.
    /// </summary>
    public class DiscordIntegration : Plugin<Config>
    {
        private static readonly DiscordIntegration InstanceValue = new DiscordIntegration();

        private readonly List<CoroutineHandle> coroutines = new List<CoroutineHandle>();

        private MapHandler mapHandler;

        private ServerHandler serverHandler;

        private PlayerHandler playerHandler;

        private NetworkHandler networkHandler;

        private DiscordIntegration()
        {
        }

        /// <summary>
        /// Gets the plugin <see cref="Language"/> instance.
        /// </summary>
        public static Language Language { get; private set; }

        /// <summary>
        /// Gets the <see cref="API.Network"/> instance.
        /// </summary>
        public static Network Network { get; private set; }

        /// <summary>
        /// Gets the network <see cref="CancellationTokenSource"/> instance.
        /// </summary>
        public static CancellationTokenSource NetworkCancellationTokenSource { get; private set; }

        /// <summary>
        /// Gets the <see cref="DiscordIntegration"/> instance.
        /// </summary>
        public static DiscordIntegration Instance => InstanceValue;

        /// <summary>
        /// Gets a list of synced users from the Discord server and SCP: SL one.
        /// </summary>
        public HashSet<SyncedUser> SyncedUsersCache { get; } = new HashSet<SyncedUser>();

        /// <summary>
        /// Gets the server slots.
        /// </summary>
        public int Slots => CustomNetworkManager.slots;

        /// <summary>
        /// Gets the minimum version of Exiled to make the plugin work correctly.
        /// </summary>
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 30);

        /// <summary>
        /// Gets the ticks counter.
        /// </summary>
        public short Ticks { get; internal set; }

        /// <summary>
        /// Fired when the plugin is enabled.
        /// </summary>
        public override void OnEnabled()
        {
            Language = new Language();
            Network = new Network();

            NetworkCancellationTokenSource = new CancellationTokenSource();

            Language.Save();
            Language.Load();

            RegisterEvents();

            coroutines.Add(Timing.RunCoroutine(CountTicks(), Segment.Update));

            Bot.UpdateActivityCancellationTokenSource = new CancellationTokenSource();
            Bot.UpdateChannelsTopicCancellationTokenSource = new CancellationTokenSource();

            _ = Network.Start(NetworkCancellationTokenSource.Token);

            _ = Bot.UpdateActivity(Bot.UpdateActivityCancellationTokenSource.Token);
            _ = Bot.UpdateChannelsTopic(Bot.UpdateChannelsTopicCancellationTokenSource.Token);

            base.OnEnabled();
        }

        /// <summary>
        /// Fired when the plugin is disabled.
        /// </summary>
        public override void OnDisabled()
        {
            KillCoroutines();

            NetworkCancellationTokenSource.Cancel();
            NetworkCancellationTokenSource.Dispose();

            Network.Close();

            UnregisterEvents();

            Bot.UpdateActivityCancellationTokenSource.Cancel();
            Bot.UpdateActivityCancellationTokenSource.Dispose();

            Bot.UpdateChannelsTopicCancellationTokenSource.Cancel();
            Bot.UpdateChannelsTopicCancellationTokenSource.Dispose();

            Ticks = 0;

            SyncedUsersCache.Clear();

            Language = null;
            Network = null;

            base.OnDisabled();
        }

        private void RegisterEvents()
        {
            mapHandler = new MapHandler();
            serverHandler = new ServerHandler();
            playerHandler = new PlayerHandler();
            networkHandler = new NetworkHandler();

            Handlers.Map.Decontaminating += mapHandler.OnDecontaminating;
            Handlers.Map.GeneratorActivated += mapHandler.OnGeneratorActivated;
            Handlers.Warhead.Starting += mapHandler.OnStartingWarhead;
            Handlers.Warhead.Stopping += mapHandler.OnStoppingWarhead;
            Handlers.Warhead.Detonated += mapHandler.OnWarheadDetonated;
            Handlers.Scp914.UpgradingItems += mapHandler.OnUpgradingItems;

            Handlers.Server.SendingRemoteAdminCommand += serverHandler.OnSendingRemoteAdminCommand;
            Handlers.Server.WaitingForPlayers += serverHandler.OnWaitingForPlayers;
            Handlers.Server.SendingConsoleCommand += serverHandler.OnSendingConsoleCommand;
            Handlers.Server.RoundStarted += serverHandler.OnRoundStarted;
            Handlers.Server.RoundEnded += serverHandler.OnRoundEnded;
            Handlers.Server.RespawningTeam += serverHandler.OnRespawningTeam;
            Handlers.Server.ReportingCheater += serverHandler.OnReportingCheater;

            Handlers.Scp914.ChangingKnobSetting += playerHandler.OnChangingScp914KnobSetting;
            Handlers.Player.MedicalItemUsed += playerHandler.OnUsedMedicalItem;
            Handlers.Scp079.InteractingTesla += playerHandler.OnInteractingTesla;
            Handlers.Player.PickingUpItem += playerHandler.OnPickingUpItem;
            Handlers.Player.InsertingGeneratorTablet += playerHandler.OnInsertingGeneratorTablet;
            Handlers.Player.EjectingGeneratorTablet += playerHandler.OnEjectingGeneratorTablet;
            Handlers.Player.UnlockingGenerator += playerHandler.OnUnlockingGenerator;
            Handlers.Player.OpeningGenerator += playerHandler.OnOpeningGenerator;
            Handlers.Player.ClosingGenerator += playerHandler.OnClosingGenerator;
            Handlers.Scp079.GainingLevel += playerHandler.OnGainingLevel;
            Handlers.Scp079.GainingExperience += playerHandler.OnGainingExperience;
            Handlers.Player.EscapingPocketDimension += playerHandler.OnEscapingPocketDimension;
            Handlers.Player.EnteringPocketDimension += playerHandler.OnEnteringPocketDimension;
            Handlers.Scp106.CreatingPortal += playerHandler.OnCreatingPortal;
            Handlers.Player.ActivatingWarheadPanel += playerHandler.OnActivatingWarheadPanel;
            Handlers.Player.TriggeringTesla += playerHandler.OnTriggeringTesla;
            Handlers.Player.ThrowingGrenade += playerHandler.OnThrowingGrenade;
            Handlers.Player.Hurting += playerHandler.OnHurting;
            Handlers.Player.Dying += playerHandler.OnDying;
            Handlers.Player.Kicked += playerHandler.OnKicked;
            Handlers.Player.Banned += playerHandler.OnBanned;
            Handlers.Player.InteractingDoor += playerHandler.OnInteractingDoor;
            Handlers.Player.InteractingElevator += playerHandler.OnInteractingElevator;
            Handlers.Player.InteractingLocker += playerHandler.OnInteractingLocker;
            Handlers.Player.IntercomSpeaking += playerHandler.OnIntercomSpeaking;
            Handlers.Player.Handcuffing += playerHandler.OnHandcuffing;
            Handlers.Player.RemovingHandcuffs += playerHandler.OnRemovingHandcuffs;
            Handlers.Scp106.Teleporting += playerHandler.OnTeleporting;
            Handlers.Player.ReloadingWeapon += playerHandler.OnReloadingWeapon;
            Handlers.Player.ItemDropped += playerHandler.OnItemDropped;
            Handlers.Player.Verified += playerHandler.OnVerified;
            Handlers.Player.Destroying += playerHandler.OnDestroying;
            Handlers.Player.ChangingRole += playerHandler.OnChangingRole;
            Handlers.Player.ChangingGroup += playerHandler.OnChangingGroup;
            Handlers.Player.ChangingItem += playerHandler.OnChangingItem;
            Handlers.Scp914.Activating += playerHandler.OnActivatingScp914;
            Handlers.Scp106.Containing += playerHandler.OnContaining;

            Network.SendingError += networkHandler.OnSendingError;
            Network.ReceivingError += networkHandler.OnReceivingError;
            Network.UpdatingConnectionError += networkHandler.OnUpdatingConnectionError;
            Network.ConnectingError += networkHandler.OnConnectingError;
            Network.Connected += networkHandler.OnConnected;
            Network.Connecting += networkHandler.OnConnecting;
            Network.ReceivedFull += networkHandler.OnReceivedFull;
            Network.Sent += networkHandler.OnSent;
            Network.Terminated += networkHandler.OnTerminated;
        }

        private void UnregisterEvents()
        {
            Handlers.Map.Decontaminating -= mapHandler.OnDecontaminating;
            Handlers.Map.GeneratorActivated -= mapHandler.OnGeneratorActivated;
            Handlers.Warhead.Starting -= mapHandler.OnStartingWarhead;
            Handlers.Warhead.Stopping -= mapHandler.OnStoppingWarhead;
            Handlers.Warhead.Detonated -= mapHandler.OnWarheadDetonated;
            Handlers.Scp914.UpgradingItems -= mapHandler.OnUpgradingItems;

            Handlers.Server.SendingRemoteAdminCommand -= serverHandler.OnSendingRemoteAdminCommand;
            Handlers.Server.WaitingForPlayers -= serverHandler.OnWaitingForPlayers;
            Handlers.Server.SendingConsoleCommand -= serverHandler.OnSendingConsoleCommand;
            Handlers.Server.RoundStarted -= serverHandler.OnRoundStarted;
            Handlers.Server.RoundEnded -= serverHandler.OnRoundEnded;
            Handlers.Server.RespawningTeam -= serverHandler.OnRespawningTeam;
            Handlers.Server.ReportingCheater -= serverHandler.OnReportingCheater;

            Handlers.Scp914.ChangingKnobSetting -= playerHandler.OnChangingScp914KnobSetting;
            Handlers.Player.UsingMedicalItem -= playerHandler.OnUsedMedicalItem;
            Handlers.Scp079.InteractingTesla -= playerHandler.OnInteractingTesla;
            Handlers.Player.PickingUpItem -= playerHandler.OnPickingUpItem;
            Handlers.Player.InsertingGeneratorTablet -= playerHandler.OnInsertingGeneratorTablet;
            Handlers.Player.EjectingGeneratorTablet -= playerHandler.OnEjectingGeneratorTablet;
            Handlers.Player.UnlockingGenerator -= playerHandler.OnUnlockingGenerator;
            Handlers.Player.OpeningGenerator -= playerHandler.OnOpeningGenerator;
            Handlers.Player.ClosingGenerator -= playerHandler.OnClosingGenerator;
            Handlers.Scp079.GainingLevel -= playerHandler.OnGainingLevel;
            Handlers.Scp079.GainingExperience -= playerHandler.OnGainingExperience;
            Handlers.Player.EscapingPocketDimension -= playerHandler.OnEscapingPocketDimension;
            Handlers.Player.EnteringPocketDimension -= playerHandler.OnEnteringPocketDimension;
            Handlers.Scp106.CreatingPortal -= playerHandler.OnCreatingPortal;
            Handlers.Player.ActivatingWarheadPanel -= playerHandler.OnActivatingWarheadPanel;
            Handlers.Player.TriggeringTesla -= playerHandler.OnTriggeringTesla;
            Handlers.Player.ThrowingGrenade -= playerHandler.OnThrowingGrenade;
            Handlers.Player.Hurting -= playerHandler.OnHurting;
            Handlers.Player.Dying -= playerHandler.OnDying;
            Handlers.Player.Kicked -= playerHandler.OnKicked;
            Handlers.Player.Banned -= playerHandler.OnBanned;
            Handlers.Player.InteractingDoor -= playerHandler.OnInteractingDoor;
            Handlers.Player.InteractingElevator -= playerHandler.OnInteractingElevator;
            Handlers.Player.InteractingLocker -= playerHandler.OnInteractingLocker;
            Handlers.Player.IntercomSpeaking -= playerHandler.OnIntercomSpeaking;
            Handlers.Player.Handcuffing -= playerHandler.OnHandcuffing;
            Handlers.Player.RemovingHandcuffs -= playerHandler.OnRemovingHandcuffs;
            Handlers.Scp106.Teleporting -= playerHandler.OnTeleporting;
            Handlers.Player.ReloadingWeapon -= playerHandler.OnReloadingWeapon;
            Handlers.Player.ItemDropped -= playerHandler.OnItemDropped;
            Handlers.Player.Verified -= playerHandler.OnVerified;
            Handlers.Player.Destroying -= playerHandler.OnDestroying;
            Handlers.Player.ChangingRole -= playerHandler.OnChangingRole;
            Handlers.Player.ChangingGroup -= playerHandler.OnChangingGroup;
            Handlers.Player.ChangingItem -= playerHandler.OnChangingItem;
            Handlers.Scp914.Activating -= playerHandler.OnActivatingScp914;
            Handlers.Scp106.Containing -= playerHandler.OnContaining;

            Network.SendingError -= networkHandler.OnSendingError;
            Network.ReceivingError -= networkHandler.OnReceivingError;
            Network.UpdatingConnectionError -= networkHandler.OnUpdatingConnectionError;
            Network.ConnectingError -= networkHandler.OnConnectingError;
            Network.Connected -= networkHandler.OnConnected;
            Network.Connecting -= networkHandler.OnConnecting;
            Network.ReceivedFull -= networkHandler.OnReceivedFull;
            Network.Sent -= networkHandler.OnSent;
            Network.Terminated -= networkHandler.OnTerminated;

            playerHandler = null;
            mapHandler = null;
            serverHandler = null;
            networkHandler = null;
        }

        private void KillCoroutines()
        {
            Timing.KillCoroutines(coroutines.ToArray());

            coroutines.Clear();
        }

        private IEnumerator<float> CountTicks()
        {
            while (true)
            {
                Ticks++;

                yield return Timing.WaitForOneFrame;
            }
        }
    }
}
