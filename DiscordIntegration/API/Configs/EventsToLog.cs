// -----------------------------------------------------------------------
// <copyright file="EventsToLog.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace DiscordIntegration.API.Configs
{
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable CS1591
    public sealed class EventsToLog
    {
        public bool SendingRemoteAdminCommands { get; private set; } = true;

        public bool SendingConsoleCommands { get; private set; } = true;

        public bool RoundStarted { get; private set; } = true;

        public bool RoundEnded { get; private set; } = true;

        public bool WaitingForPlayers { get; private set; } = true;

        public bool ReportingCheater { get; private set; } = true;

        public bool HurtingPlayer { get; private set; } = true;

        public bool PlayerDying { get; private set; } = true;

        public bool PlayerThrowingGrenade { get; private set; } = true;

        public bool PlayerUsedMedicalItem { get; private set; } = true;

        public bool PlayerEnteringPocketDimension { get; private set; } = true;

        public bool PlayerEscapingPocketDimension { get; private set; } = true;

        public bool PlayerItemDropped { get; private set; }

        public bool PlayerPickingupItem { get; private set; }

        public bool PlayerIntercomSpeaking { get; private set; } = true;

        public bool PlayerJoined { get; private set; } = true;

        public bool PlayerInteractingDoor { get; private set; }

        public bool PlayerRemovingHandcuffs { get; private set; } = true;

        public bool PlayerActivatingWarheadPanel { get; private set; } = true;

        public bool PlayerInteractingElevator { get; private set; } = true;

        public bool PlayerInteractingLocker { get; private set; } = true;

        public bool PlayerTriggeringTesla { get; private set; } = true;

        public bool PlayerClosingGenerator { get; private set; } = true;

        public bool PlayerOpeningGenerator { get; private set; } = true;

        public bool PlayerInsertingGeneratorTablet { get; private set; } = true;

        public bool PlayerEjectingGeneratorTablet { get; private set; } = true;

        public bool PlayerUnlockingGenerator { get; private set; } = true;

        public bool PlayerLeft { get; private set; } = true;

        public bool PlayerBanned { get; internal set; } = true;

        public bool ReloadingPlayerWeapon { get; private set; }

        public bool ChangingPlayerRole { get; private set; } = true;

        public bool RespawningTeam { get; private set; } = true;

        public bool HandcuffingPlayer { get; private set; } = true;

        public bool ChangingPlayerItem { get; private set; }

        public bool ChangingPlayerGroup { get; private set; } = true;

        public bool UpgradingScp914Items { get; private set; } = true;

        public bool ActivatingScp914 { get; private set; } = true;

        public bool ChangingScp914KnobSetting { get; private set; } = true;

        public bool Decontaminating { get; private set; } = true;

        public bool StoppingWarhead { get; private set; } = true;

        public bool StartingWarhead { get; private set; } = true;

        public bool WarheadDetonated { get; private set; } = true;

        public bool GeneratorActivated { get; private set; } = true;

        public bool Scp106Teleporting { get; private set; } = true;

        public bool ContainingScp106 { get; private set; } = true;

        public bool CreatingScp106Portal { get; private set; } = true;

        public bool GainingScp079Experience { get; private set; } = true;

        public bool Scp079InteractingTesla { get; private set; } = true;

        public bool GainingScp079Level { get; private set; } = true;
    }
}
