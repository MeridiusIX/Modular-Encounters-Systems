using ProtoBuf;
using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Helpers;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.ObjectBuilders;
using VRageMath;
using ModularEncountersSystems.Logging;

namespace ModularEncountersSystems.Behavior.Subsystems.Trigger {

	public class ActionReferenceProfile {

		public bool UseChatBroadcast;

		public bool BarrelRoll;
		public bool Strafe;
		public bool Ramming;

		public bool ChangeAutopilotSpeed;
		public float NewAutopilotSpeed;

		public bool SpawnEncounter;

		public bool SelfDestruct;
		public int SelfDestructTimerPadding;
		public int SelfDestructTimeBetweenBlasts;
		public bool StaggerWarheadDetonation;

		public bool Retreat;

		public bool BroadcastCurrentTarget;
		public bool BroadcastDamagerTarget;
		public string BroadcastSendCode;

		public bool SwitchToReceivedTarget;

		public bool SwitchToBehavior;
		public string NewBehavior;
		public bool PreserveSettingsOnBehaviorSwitch;

		public bool RefreshTarget;

		public bool SwitchTargetProfile; //Obsolete
		public string NewTargetProfile; //Obsolete

		public bool TriggerTimerBlocks;
		public List<string> TimerBlockNames;

		public bool ChangeReputationWithPlayers;
		public double ReputationChangeRadius;
		public List<int> ReputationChangeAmount;

		public bool ActivateAssertiveAntennas;
		public bool ChangeAntennaOwnership;
		public string AntennaFactionOwner;

		public bool CreateKnownPlayerArea;
		public double KnownPlayerAreaRadius;
		public int KnownPlayerAreaTimer;
		public int KnownPlayerAreaMaxSpawns;

		public bool DamageToolAttacker;
		public float DamageToolAttackerAmount;
		public string DamageToolAttackerParticle;
		public string DamageToolAttackerSound;

		public bool PlayParticleEffectAtRemote;
		public string ParticleEffectId;
		public Vector3D ParticleEffectOffset;
		public float ParticleEffectScale;
		public float ParticleEffectMaxTime;
		public Vector3D ParticleEffectColor;

		public List<string> SetBooleansTrue;
		public List<string> SetBooleansFalse;
		public List<string> IncreaseCounters;
		public List<string> DecreaseCounters;
		public List<string> ResetCounters;

		public List<string> SetSandboxBooleansTrue;
		public List<string> SetSandboxBooleansFalse;
		public List<string> IncreaseSandboxCounters;
		public List<string> DecreaseSandboxCounters;
		public List<string> ResetSandboxCounters;

		public bool ChangeAttackerReputation;
		public List<string> ChangeAttackerReputationFaction;
		public List<int> ChangeAttackerReputationAmount;
		public List<string> ReputationChangeFactions;
		public bool ReputationChangesForAllRadiusPlayerFactionMembers;
		public bool ReputationChangesForAllAttackPlayerFactionMembers;

		public string ProfileSubtypeId;

		public bool BroadcastGenericCommand;

		public bool BehaviorSpecificEventA;
		public bool BehaviorSpecificEventB;
		public bool BehaviorSpecificEventC;
		public bool BehaviorSpecificEventD;
		public bool BehaviorSpecificEventE;
		public bool BehaviorSpecificEventF;
		public bool BehaviorSpecificEventG;
		public bool BehaviorSpecificEventH;

		public bool TerminateBehavior;

		public bool ChangeTargetProfile;
		public string NewTargetProfileId; //Implement

		public bool PreserveTriggersOnBehaviorSwitch;
		public bool PreserveTargetDataOnBehaviorSwitch;

		public bool ChangeBlockNames;
		public List<string> ChangeBlockNamesFrom;
		public List<string> ChangeBlockNamesTo;

		public bool ChangeAntennaRanges;
		public List<string> AntennaNamesForRangeChange;
		public string AntennaRangeChangeType;
		public float AntennaRangeChangeAmount;

		public bool ForceDespawn;

		public bool ResetCooldownTimeOfTriggers;
		public List<string> ResetTriggerCooldownNames;

		public bool EnableTriggers;
		public List<string> EnableTriggerNames;

		public bool DisableTriggers;
		public List<string> DisableTriggerNames;

		public bool ManuallyActivateTrigger;
		public List<string> ManuallyActivatedTriggerNames;

		public bool ChangeInertiaDampeners;
		public bool InertiaDampenersEnable;

		public bool CreateWeatherAtPosition;
		public string WeatherSubtypeId;
		public double WeatherRadius;
		public int WeatherDuration;

		public bool ChangeRotationDirection;
		public Direction RotationDirection;

		public bool GenerateExplosion;
		public Vector3D ExplosionOffsetFromRemote;
		public int ExplosionDamage;
		public int ExplosionRange;
		public bool ExplosionIgnoresVoxels;

		public CheckEnum GridEditable;
		public CheckEnum SubGridsEditable;

		public CheckEnum GridDestructible;
		public CheckEnum SubGridsDestructible;

		public bool RecolorGrid;
		public bool RecolorSubGrids;
		public List<Vector3D> OldBlockColors;
		public List<Vector3D> NewBlockColors;
		public List<string> NewBlockSkins;

		public bool ChangeBlockOwnership;
		public List<string> OwnershipBlockNames;
		public List<string> OwnershipBlockFactions;

		public bool ChangeBlockDamageMultipliers;
		public List<string> DamageMultiplierBlockNames;
		public List<int> DamageMultiplierValues;

		public int KnownPlayerAreaMinThreatForAvoidingAbandonment;

		public bool RazeBlocksWithNames;
		public List<string> RazeBlocksNames;

		public bool SendCommandWithoutAntenna;
		public double SendCommandWithoutAntennaRadius;

		public bool SwitchToDamagerTarget;

		public bool RemoveKnownPlayerArea;
		public bool RemoveAllKnownPlayerAreas;

		public int Chance;

		public bool EnableBlocks;
		public List<string> EnableBlockNames;
		public List<SwitchEnum> EnableBlockStates;

		public bool ChangeAutopilotProfile;
		public AutoPilotDataMode AutopilotProfile;

		public bool CreateRandomLightning;
		public bool CreateLightningAtAttacker;
		public int LightningDamage;
		public int LightningExplosionRadius;
		public Vector3D LightningColor;
		public double LightningMinDistance;
		public double LightningMaxDistance;
		public bool CreateLightningAtTarget;

		public List<string> SetCounters;
		public List<string> SetSandboxCounters;
		public List<int> SetCountersValues;
		public List<int> SetSandboxCountersValues;

		public bool InheritLastAttackerFromCommand;

		public bool ChangePlayerCredits;
		public long ChangePlayerCreditsAmount;

		public bool ChangeNpcFactionCredits;
		public long ChangeNpcFactionCreditsAmount;
		public string ChangeNpcFactionCreditsTag;

		public bool BuildProjectedBlocks;
		public int MaxProjectedBlocksToBuild;

		public bool ForceManualTriggerActivation;

		public bool OverwriteAutopilotProfile;
		public AutoPilotDataMode OverwriteAutopilotMode;
		public string OverwriteAutopilotId;

		public bool SwitchTargetToDamager;

		public bool BroadcastCommandProfiles;
		public List<string> CommandProfileIds;

		public bool AddWaypointFromCommand;
		public bool RecalculateDespawnCoords;

		public bool AddDatapadsToSeats;
		public List<string> DatapadNamesToAdd;
		public int DatapadCountToAdd;

		public bool ToggleBlocksOfType;
		public List<SerializableDefinitionId> BlockTypesToToggle;
		public List<SwitchEnum> BlockTypeToggles;

		public bool CancelWaitingAtWaypoint;
		public bool SwitchToNextWaypoint;

		public bool HeavyYaw;

		public bool StopAllRotation;
		public bool StopAllThrust;
		public bool RandomGyroRotation;
		public bool RandomThrustDirection;

		public string ParentGridNameRequirement;

		public bool ChangeZoneAtPosition;
		public string ZoneName;

		public ModifierEnum ZoneRadiusChangeType;
		public double ZoneRadiusChangeAmount;

		public bool ZoneCustomCounterChange;
		public List<ModifierEnum> ZoneCustomCounterChangeType;
		public List<string> ZoneCustomCounterChangeName;
		public List<int> ZoneCustomCounterChangeAmount;

		public bool ZoneCustomBoolChange;
		public List<string> ZoneCustomBoolChangeName;
		public List<bool> ZoneCustomBoolChangeAmount;

		public bool CreateBots;
		public List<string> BotSpawnGroup;
		public List<string> BotConditionProfile;
		public List<string> BotWaypoint;

		public bool ChangeBehaviorSubclass;
		public BehaviorSubclass NewBehaviorSubclass;

		public Dictionary<string, Action<string, object>> EditorReference;

		public ActionReferenceProfile() {

			UseChatBroadcast = false;

			BarrelRoll = false;
			Strafe = false;

			ChangeAutopilotSpeed = false;
			NewAutopilotSpeed = 0;

			SpawnEncounter = false;

			SelfDestruct = false;
			StaggerWarheadDetonation = false;

			Retreat = false;

			BroadcastCurrentTarget = false;
			BroadcastDamagerTarget = false;
			BroadcastSendCode = "";
			SwitchToReceivedTarget = false;
			SwitchTargetToDamager = false;

			SwitchToBehavior = false;
			NewBehavior = "";
			PreserveSettingsOnBehaviorSwitch = false;
			PreserveTriggersOnBehaviorSwitch = false;
			PreserveTargetDataOnBehaviorSwitch = false;

			RefreshTarget = false;

			TriggerTimerBlocks = false;
			TimerBlockNames = new List<string>();

			ChangeReputationWithPlayers = false;
			ReputationChangeRadius = 0;
			ReputationChangeFactions = new List<string>();
			ReputationChangeAmount = new List<int>();
			ReputationChangesForAllRadiusPlayerFactionMembers = false;

			ChangeAttackerReputation = false;
			ChangeAttackerReputationFaction = new List<string>();
			ChangeAttackerReputationAmount = new List<int>();
			ReputationChangesForAllAttackPlayerFactionMembers = false;

			ActivateAssertiveAntennas = false;

			ChangeAntennaOwnership = false;
			AntennaFactionOwner = "Nobody";

			CreateKnownPlayerArea = false;
			KnownPlayerAreaRadius = 10000;
			KnownPlayerAreaTimer = 30;
			KnownPlayerAreaMaxSpawns = -1;

			DamageToolAttacker = false;
			DamageToolAttackerAmount = 90;
			DamageToolAttackerParticle = "";
			DamageToolAttackerSound = "";

			PlayParticleEffectAtRemote = false;
			ParticleEffectId = "";
			ParticleEffectOffset = Vector3D.Zero;
			ParticleEffectScale = 1;
			ParticleEffectMaxTime = -1;
			ParticleEffectColor = Vector3D.Zero;

			SetBooleansTrue = new List<string>();
			SetBooleansFalse = new List<string>();
			IncreaseCounters = new List<string>();
			DecreaseCounters = new List<string>();
			ResetCounters = new List<string>();
			SetCounters = new List<string>();
			SetCountersValues = new List<int>();

			SetSandboxBooleansTrue = new List<string>();
			SetSandboxBooleansFalse = new List<string>();
			IncreaseSandboxCounters = new List<string>();
			DecreaseSandboxCounters = new List<string>();
			ResetSandboxCounters = new List<string>();
			SetSandboxCounters = new List<string>();
			SetSandboxCountersValues = new List<int>();

			BroadcastGenericCommand = false;

			BehaviorSpecificEventA = false;
			BehaviorSpecificEventB = false;
			BehaviorSpecificEventC = false;
			BehaviorSpecificEventD = false;
			BehaviorSpecificEventE = false;
			BehaviorSpecificEventF = false;
			BehaviorSpecificEventG = false;
			BehaviorSpecificEventH = false;

			TerminateBehavior = false;

			ChangeTargetProfile = false;
			NewTargetProfileId = "";

			ChangeBlockNames = false;
			ChangeBlockNamesFrom = new List<string>();
			ChangeBlockNamesTo = new List<string>();

			ChangeAntennaRanges = false;
			AntennaNamesForRangeChange = new List<string>();
			AntennaRangeChangeType = "Set";
			AntennaRangeChangeAmount = 0;

			ForceDespawn = false;

			ResetCooldownTimeOfTriggers = false;
			ResetTriggerCooldownNames = new List<string>();

			ChangeInertiaDampeners = false;
			InertiaDampenersEnable = false;

			EnableTriggers = false;
			EnableTriggerNames = new List<string>();

			DisableTriggers = false;
			DisableTriggerNames = new List<string>();

			ChangeRotationDirection = false;
			RotationDirection = Direction.None;

			GenerateExplosion = false;
			ExplosionOffsetFromRemote = Vector3D.Zero;
			ExplosionDamage = 1;
			ExplosionRange = 1;
			ExplosionIgnoresVoxels = false;

			GridEditable = CheckEnum.Ignore;
			SubGridsEditable = CheckEnum.Ignore;
			GridDestructible = CheckEnum.Ignore;
			SubGridsDestructible = CheckEnum.Ignore;

			RecolorGrid = false;
			RecolorSubGrids = false;
			OldBlockColors = new List<Vector3D>();
			NewBlockColors = new List<Vector3D>();
			NewBlockSkins = new List<string>();

			ChangeBlockOwnership = false;
			OwnershipBlockNames = new List<string>();
			OwnershipBlockFactions = new List<string>();

			ChangeBlockDamageMultipliers = false;
			DamageMultiplierBlockNames = new List<string>();
			DamageMultiplierValues = new List<int>();

			KnownPlayerAreaMinThreatForAvoidingAbandonment = -1;

			RazeBlocksWithNames = false;
			RazeBlocksNames = new List<string>();

			ManuallyActivateTrigger = false;
			ManuallyActivatedTriggerNames = new List<string>();

			SendCommandWithoutAntenna = false;
			SendCommandWithoutAntennaRadius = -1;

			SwitchToDamagerTarget = false;

			RemoveKnownPlayerArea = false;
			RemoveAllKnownPlayerAreas = false;

			Chance = 100;

			EnableBlocks = false;
			EnableBlockNames = new List<string>();
			EnableBlockStates = new List<SwitchEnum>();

			ChangeAutopilotProfile = false;
			AutopilotProfile = AutoPilotDataMode.Primary;

			OverwriteAutopilotProfile = false;
			OverwriteAutopilotMode = AutoPilotDataMode.Primary;
			OverwriteAutopilotId = "";

			Ramming = false;

			CreateRandomLightning = false;
			CreateLightningAtAttacker = false;
			LightningDamage = 0;
			LightningExplosionRadius = 1;
			LightningColor = new Vector3D(100, 100, 100);
			LightningMinDistance = 100;
			LightningMaxDistance = 200;
			CreateLightningAtTarget = false;

			SelfDestructTimerPadding = 0;
			SelfDestructTimeBetweenBlasts = 1;

			InheritLastAttackerFromCommand = false;

			ChangePlayerCredits = false;
			ChangePlayerCreditsAmount = 0;

			ChangeNpcFactionCredits = false;
			ChangeNpcFactionCreditsAmount = 0;
			ChangeNpcFactionCreditsTag = "";

			BuildProjectedBlocks = false;
			MaxProjectedBlocksToBuild = -1;

			ForceManualTriggerActivation = false;

			BroadcastCommandProfiles = false;
			CommandProfileIds = new List<string>();

			AddWaypointFromCommand = false;

			RecalculateDespawnCoords = false;

			AddDatapadsToSeats = false;
			DatapadNamesToAdd = new List<string>();
			DatapadCountToAdd = 1;

			ToggleBlocksOfType = false;
			BlockTypesToToggle = new List<SerializableDefinitionId>();
			BlockTypeToggles = new List<SwitchEnum>();

			CancelWaitingAtWaypoint = false;
			SwitchToNextWaypoint = false;

			HeavyYaw = false;

			StopAllRotation = false;
			StopAllThrust = false;
			RandomGyroRotation = false;
			RandomThrustDirection = false;

			ParentGridNameRequirement = "";

			ChangeZoneAtPosition = false;
			ZoneName = "";

			ZoneRadiusChangeType = ModifierEnum.None;
			ZoneRadiusChangeAmount = 0;

			ZoneCustomCounterChange = false;
			ZoneCustomCounterChangeType = new List<ModifierEnum>();
			ZoneCustomCounterChangeName = new List<string>();
			ZoneCustomCounterChangeAmount = new List<int>();

			ZoneCustomBoolChange = false;
			ZoneCustomBoolChangeName = new List<string>();
			ZoneCustomBoolChangeAmount = new List<bool>();

			CreateBots = false;
			BotSpawnGroup = new List<string>();
			BotConditionProfile = new List<string>();
			BotWaypoint = new List<string>();

			ChangeBehaviorSubclass = false;
			NewBehaviorSubclass = BehaviorSubclass.None;

			ProfileSubtypeId = "";

			EditorReference = new Dictionary<string, Action<string, object>> {

				{"UseChatBroadcast", (s, o) => TagParse.TagBoolCheck(s, ref UseChatBroadcast) },
				{"BarrelRoll", (s, o) => TagParse.TagBoolCheck(s, ref BarrelRoll) },
				{"Strafe", (s, o) => TagParse.TagBoolCheck(s, ref Strafe) },
				{"ChangeAutopilotSpeed", (s, o) => TagParse.TagBoolCheck(s, ref ChangeAutopilotSpeed) },
				{"NewAutopilotSpeed", (s, o) => TagParse.TagFloatCheck(s, ref NewAutopilotSpeed) },
				{"SpawnEncounter", (s, o) => TagParse.TagBoolCheck(s, ref SpawnEncounter) },
				{"SelfDestruct", (s, o) => TagParse.TagBoolCheck(s, ref SelfDestruct) },
				{"Retreat", (s, o) => TagParse.TagBoolCheck(s, ref Retreat) },
				{"TerminateBehavior", (s, o) => TagParse.TagBoolCheck(s, ref TerminateBehavior) },
				{"BroadcastCurrentTarget", (s, o) => TagParse.TagBoolCheck(s, ref BroadcastCurrentTarget) },
				{"SwitchToReceivedTarget", (s, o) => TagParse.TagBoolCheck(s, ref SwitchToReceivedTarget) },
				{"SwitchTargetToDamager", (s, o) => TagParse.TagBoolCheck(s, ref SwitchTargetToDamager) },
				{"BroadcastDamagerTarget", (s, o) => TagParse.TagBoolCheck(s, ref BroadcastDamagerTarget) },
				{"BroadcastSendCode", (s, o) => TagParse.TagStringCheck(s, ref BroadcastSendCode) },
				{"SwitchToBehavior", (s, o) => TagParse.TagBoolCheck(s, ref SwitchToBehavior) },
				{"NewBehavior", (s, o) => TagParse.TagStringCheck(s, ref NewBehavior) },
				{"PreserveSettingsOnBehaviorSwitch", (s, o) => TagParse.TagBoolCheck(s, ref PreserveSettingsOnBehaviorSwitch) },
				{"PreserveTriggersOnBehaviorSwitch", (s, o) => TagParse.TagBoolCheck(s, ref PreserveTriggersOnBehaviorSwitch) },
				{"PreserveTargetDataOnBehaviorSwitch", (s, o) => TagParse.TagBoolCheck(s, ref PreserveTargetDataOnBehaviorSwitch) },
				{"RefreshTarget", (s, o) => TagParse.TagBoolCheck(s, ref RefreshTarget) },
				{"SwitchTargetProfile", (s, o) => TagParse.TagBoolCheck(s, ref SwitchTargetProfile) },
				{"NewTargetProfile", (s, o) => TagParse.TagStringCheck(s, ref NewTargetProfile) },
				{"TriggerTimerBlocks", (s, o) => TagParse.TagBoolCheck(s, ref TriggerTimerBlocks) },
				{"TimerBlockNames", (s, o) => TagParse.TagStringListCheck(s, ref TimerBlockNames) },
				{"ChangeReputationWithPlayers", (s, o) => TagParse.TagBoolCheck(s, ref ChangeReputationWithPlayers) },
				{"ReputationChangeRadius", (s, o) => TagParse.TagDoubleCheck(s, ref ReputationChangeRadius) },
				{"ReputationChangeFactions", (s, o) => TagParse.TagStringListCheck(s, ref ReputationChangeFactions) },
				{"ReputationChangeAmount", (s, o) => TagParse.TagIntListCheck(s, ref ReputationChangeAmount) },
				{"ActivateAssertiveAntennas", (s, o) => TagParse.TagBoolCheck(s, ref ActivateAssertiveAntennas) },
				{"ChangeAntennaOwnership", (s, o) => TagParse.TagBoolCheck(s, ref ChangeAntennaOwnership) },
				{"AntennaFactionOwner", (s, o) => TagParse.TagStringCheck(s, ref AntennaFactionOwner) },
				{"CreateKnownPlayerArea", (s, o) => TagParse.TagBoolCheck(s, ref CreateKnownPlayerArea) },
				{"KnownPlayerAreaRadius", (s, o) => TagParse.TagDoubleCheck(s, ref KnownPlayerAreaRadius) },
				{"KnownPlayerAreaTimer", (s, o) => TagParse.TagIntCheck(s, ref KnownPlayerAreaTimer) },
				{"KnownPlayerAreaMaxSpawns", (s, o) => TagParse.TagIntCheck(s, ref KnownPlayerAreaMaxSpawns) },
				{"KnownPlayerAreaMinThreatForAvoidingAbandonment", (s, o) => TagParse.TagIntCheck(s, ref KnownPlayerAreaMinThreatForAvoidingAbandonment) },
				{"DamageToolAttacker", (s, o) => TagParse.TagBoolCheck(s, ref DamageToolAttacker) },
				{"DamageToolAttackerAmount", (s, o) => TagParse.TagFloatCheck(s, ref DamageToolAttackerAmount) },
				{"DamageToolAttackerParticle", (s, o) => TagParse.TagStringCheck(s, ref DamageToolAttackerParticle) },
				{"DamageToolAttackerSound", (s, o) => TagParse.TagStringCheck(s, ref DamageToolAttackerSound) },
				{"PlayParticleEffectAtRemote", (s, o) => TagParse.TagBoolCheck(s, ref PlayParticleEffectAtRemote) },
				{"ParticleEffectId", (s, o) => TagParse.TagStringCheck(s, ref ParticleEffectId) },
				{"ParticleEffectOffset", (s, o) => TagParse.TagVector3DCheck(s, ref ParticleEffectOffset) },
				{"ParticleEffectScale", (s, o) => TagParse.TagFloatCheck(s, ref ParticleEffectScale) },
				{"ParticleEffectMaxTime", (s, o) => TagParse.TagFloatCheck(s, ref ParticleEffectMaxTime) },
				{"ParticleEffectColor", (s, o) => TagParse.TagVector3DCheck(s, ref ParticleEffectColor) },
				{"SetBooleansTrue", (s, o) => TagParse.TagStringListCheck(s, ref SetBooleansTrue) },
				{"SetBooleansFalse", (s, o) => TagParse.TagStringListCheck(s, ref SetBooleansFalse) },
				{"IncreaseCounters", (s, o) => TagParse.TagStringListCheck(s, ref IncreaseCounters) },
				{"DecreaseCounters", (s, o) => TagParse.TagStringListCheck(s, ref DecreaseCounters) },
				{"ResetCounters", (s, o) => TagParse.TagStringListCheck(s, ref ResetCounters) },
				{"SetSandboxBooleansTrue", (s, o) => TagParse.TagStringListCheck(s, ref SetSandboxBooleansTrue) },
				{"SetSandboxBooleansFalse", (s, o) => TagParse.TagStringListCheck(s, ref SetSandboxBooleansFalse) },
				{"IncreaseSandboxCounters", (s, o) => TagParse.TagStringListCheck(s, ref IncreaseSandboxCounters) },
				{"DecreaseSandboxCounters", (s, o) => TagParse.TagStringListCheck(s, ref DecreaseSandboxCounters) },
				{"ResetSandboxCounters", (s, o) => TagParse.TagStringListCheck(s, ref ResetSandboxCounters) },
				{"ChangeAttackerReputation", (s, o) => TagParse.TagBoolCheck(s, ref ChangeAttackerReputation) },
				{"ChangeAttackerReputationFaction", (s, o) => TagParse.TagStringListCheck(s, ref ChangeAttackerReputationFaction) },
				{"ChangeAttackerReputationAmount", (s, o) => TagParse.TagIntListCheck(s, ref ChangeAttackerReputationAmount) },
				{"ReputationChangesForAllAttackPlayerFactionMembers", (s, o) => TagParse.TagBoolCheck(s, ref ReputationChangesForAllAttackPlayerFactionMembers) },
				{"ChangeTargetProfile", (s, o) => TagParse.TagBoolCheck(s, ref ChangeTargetProfile) },
				{"NewTargetProfileId", (s, o) => TagParse.TagStringCheck(s, ref NewTargetProfileId) },
				{"ChangeBlockNames", (s, o) => TagParse.TagBoolCheck(s, ref ChangeBlockNames) },
				{"ChangeBlockNamesFrom", (s, o) => TagParse.TagStringListCheck(s, ref ChangeBlockNamesFrom) },
				{"ChangeBlockNamesTo", (s, o) => TagParse.TagStringListCheck(s, ref ChangeBlockNamesTo) },
				{"ChangeAntennaRanges", (s, o) => TagParse.TagBoolCheck(s, ref ChangeAntennaRanges) },
				{"AntennaNamesForRangeChange", (s, o) => TagParse.TagStringListCheck(s, ref AntennaNamesForRangeChange) },
				{"AntennaRangeChangeType", (s, o) => TagParse.TagStringCheck(s, ref AntennaRangeChangeType) },
				{"AntennaRangeChangeAmount", (s, o) => TagParse.TagFloatCheck(s, ref AntennaRangeChangeAmount) },
				{"ForceDespawn", (s, o) => TagParse.TagBoolCheck(s, ref ForceDespawn) },
				{"ResetCooldownTimeOfTriggers", (s, o) => TagParse.TagBoolCheck(s, ref ResetCooldownTimeOfTriggers) },
				{"ResetTriggerCooldownNames", (s, o) => TagParse.TagStringListCheck(s, ref ResetTriggerCooldownNames) },
				{"BroadcastGenericCommand", (s, o) => TagParse.TagBoolCheck(s, ref BroadcastGenericCommand) },
				{"BehaviorSpecificEventA", (s, o) => TagParse.TagBoolCheck(s, ref BehaviorSpecificEventA) },
				{"ChangeInertiaDampeners", (s, o) => TagParse.TagBoolCheck(s, ref ChangeInertiaDampeners) },
				{"InertiaDampenersEnable", (s, o) => TagParse.TagBoolCheck(s, ref InertiaDampenersEnable) },
				{"EnableTriggers", (s, o) => TagParse.TagBoolCheck(s, ref EnableTriggers) },
				{"EnableTriggerNames", (s, o) => TagParse.TagStringListCheck(s, ref EnableTriggerNames) },
				{"DisableTriggers", (s, o) => TagParse.TagBoolCheck(s, ref DisableTriggers) },
				{"DisableTriggerNames", (s, o) => TagParse.TagStringListCheck(s, ref DisableTriggerNames) },
				{"StaggerWarheadDetonation", (s, o) => TagParse.TagBoolCheck(s, ref StaggerWarheadDetonation) },
				{"ChangeRotationDirection", (s, o) => TagParse.TagBoolCheck(s, ref ChangeRotationDirection) },
				{"RotationDirection", (s, o) => TagParse.TagDirectionEnumCheck(s, ref RotationDirection) },
				{"GenerateExplosion", (s, o) => TagParse.TagBoolCheck(s, ref GenerateExplosion) },
				{"ExplosionOffsetFromRemote", (s, o) => TagParse.TagVector3DCheck(s, ref ExplosionOffsetFromRemote) },
				{"ExplosionRange", (s, o) => TagParse.TagIntCheck(s, ref ExplosionRange) },
				{"ExplosionDamage", (s, o) => TagParse.TagIntCheck(s, ref ExplosionDamage) },
				{"ExplosionIgnoresVoxels", (s, o) => TagParse.TagBoolCheck(s, ref ExplosionIgnoresVoxels) },
				{"GridEditable", (s, o) => TagParse.TagCheckEnumCheck(s, ref GridEditable) },
				{"SubGridsEditable", (s, o) => TagParse.TagCheckEnumCheck(s, ref SubGridsEditable) },
				{"GridDestructible", (s, o) => TagParse.TagCheckEnumCheck(s, ref GridDestructible) },
				{"SubGridsDestructible", (s, o) => TagParse.TagCheckEnumCheck(s, ref SubGridsDestructible) },
				{"RecolorGrid", (s, o) => TagParse.TagBoolCheck(s, ref RecolorGrid) },
				{"RecolorSubGrids", (s, o) => TagParse.TagBoolCheck(s, ref RecolorSubGrids) },
				{"OldBlockColors", (s, o) => TagParse.TagVector3DListCheck(s, ref OldBlockColors) },
				{"NewBlockColors", (s, o) => TagParse.TagVector3DListCheck(s, ref NewBlockColors) },
				{"NewBlockSkins", (s, o) => TagParse.TagStringListCheck(s, ref NewBlockSkins) },
				{"ChangeBlockOwnership", (s, o) => TagParse.TagBoolCheck(s, ref ChangeBlockOwnership) },
				{"OwnershipBlockNames", (s, o) => TagParse.TagStringListCheck(s, ref OwnershipBlockNames) },
				{"OwnershipBlockFactions", (s, o) => TagParse.TagStringListCheck(s, ref OwnershipBlockFactions) },
				{"ChangeBlockDamageMultipliers", (s, o) => TagParse.TagBoolCheck(s, ref ChangeBlockDamageMultipliers) },
				{"DamageMultiplierBlockNames", (s, o) => TagParse.TagStringListCheck(s, ref DamageMultiplierBlockNames) },
				{"DamageMultiplierValues", (s, o) => TagParse.TagIntListCheck(s, ref DamageMultiplierValues) },
				{"RazeBlocksWithNames", (s, o) => TagParse.TagBoolCheck(s, ref RazeBlocksWithNames) },
				{"RazeBlocksNames", (s, o) => TagParse.TagStringListCheck(s, ref RazeBlocksNames) },
				{"ManuallyActivateTrigger", (s, o) => TagParse.TagBoolCheck(s, ref ManuallyActivateTrigger) },
				{"ManuallyActivatedTriggerNames", (s, o) => TagParse.TagStringListCheck(s, ref ManuallyActivatedTriggerNames) },
				{"SendCommandWithoutAntenna", (s, o) => TagParse.TagBoolCheck(s, ref SendCommandWithoutAntenna) },
				{"SendCommandWithoutAntennaRadius", (s, o) => TagParse.TagDoubleCheck(s, ref SendCommandWithoutAntennaRadius) },
				{"RemoveKnownPlayerArea", (s, o) => TagParse.TagBoolCheck(s, ref RemoveKnownPlayerArea) },
				{"RemoveAllKnownPlayerAreas", (s, o) => TagParse.TagBoolCheck(s, ref RemoveAllKnownPlayerAreas) },
				{"Chance", (s, o) => TagParse.TagIntCheck(s, ref Chance) },
				{"EnableBlocks", (s, o) => TagParse.TagBoolCheck(s, ref EnableBlocks) },
				{"EnableBlockNames", (s, o) => TagParse.TagStringListCheck(s, ref EnableBlockNames) },
				{"EnableBlockStates", (s, o) => TagParse.TagSwitchEnumCheck(s, ref EnableBlockStates) },
				{"ChangeAutopilotProfile", (s, o) => TagParse.TagBoolCheck(s, ref ChangeAutopilotProfile) },
				{"AutopilotProfile", (s, o) => TagParse.TagAutoPilotProfileModeCheck(s, ref AutopilotProfile) },
				{"Ramming", (s, o) => TagParse.TagBoolCheck(s, ref Ramming) },
				{"CreateRandomLightning", (s, o) => TagParse.TagBoolCheck(s, ref CreateRandomLightning) },
				{"CreateLightningAtAttacker", (s, o) => TagParse.TagBoolCheck(s, ref CreateLightningAtAttacker) },
				{"LightningDamage", (s, o) => TagParse.TagIntCheck(s, ref LightningDamage) },
				{"LightningExplosionRadius", (s, o) => TagParse.TagIntCheck(s, ref LightningExplosionRadius) },
				{"LightningColor", (s, o) => TagParse.TagVector3DCheck(s, ref LightningColor) },
				{"LightningMinDistance", (s, o) => TagParse.TagDoubleCheck(s, ref LightningMinDistance) },
				{"LightningMaxDistance", (s, o) => TagParse.TagDoubleCheck(s, ref LightningMaxDistance) },
				{"CreateLightningAtTarget", (s, o) => TagParse.TagBoolCheck(s, ref CreateLightningAtTarget) },
				{"SelfDestructTimerPadding", (s, o) => TagParse.TagIntCheck(s, ref SelfDestructTimerPadding) },
				{"SelfDestructTimeBetweenBlasts", (s, o) => TagParse.TagIntCheck(s, ref SelfDestructTimeBetweenBlasts) },
				{"SetCounters", (s, o) => TagParse.TagStringListCheck(s, ref SetCounters) },
				{"SetSandboxCounters", (s, o) => TagParse.TagStringListCheck(s, ref SetSandboxCounters) },
				{"SetCountersValues", (s, o) => TagParse.TagIntListCheck(s, ref SetCountersValues) },
				{"SetSandboxCountersValues", (s, o) => TagParse.TagIntListCheck(s, ref SetSandboxCountersValues) },
				{"InheritLastAttackerFromCommand", (s, o) => TagParse.TagBoolCheck(s, ref InheritLastAttackerFromCommand) },
				{"ChangePlayerCredits", (s, o) => TagParse.TagBoolCheck(s, ref ChangePlayerCredits) },
				{"ChangePlayerCreditsAmount", (s, o) => TagParse.TagLongCheck(s, ref ChangePlayerCreditsAmount) },
				{"ChangeNpcFactionCredits", (s, o) => TagParse.TagBoolCheck(s, ref ChangeNpcFactionCredits) },
				{"ChangeNpcFactionCreditsAmount", (s, o) => TagParse.TagLongCheck(s, ref ChangeNpcFactionCreditsAmount) },
				{"ChangeNpcFactionCreditsTag", (s, o) => TagParse.TagStringCheck(s, ref ChangeNpcFactionCreditsTag) },
				{"BuildProjectedBlocks", (s, o) => TagParse.TagBoolCheck(s, ref BuildProjectedBlocks) },
				{"MaxProjectedBlocksToBuild", (s, o) => TagParse.TagIntCheck(s, ref MaxProjectedBlocksToBuild) },
				{"ForceManualTriggerActivation", (s, o) => TagParse.TagBoolCheck(s, ref ForceManualTriggerActivation) },
				{"OverwriteAutopilotProfile", (s, o) => TagParse.TagBoolCheck(s, ref OverwriteAutopilotProfile) },
				{"OverwriteAutopilotMode", (s, o) => TagParse.TagAutoPilotProfileModeCheck(s, ref OverwriteAutopilotMode) },
				{"OverwriteAutopilotId", (s, o) => TagParse.TagStringCheck(s, ref OverwriteAutopilotId) },
				{"BroadcastCommandProfiles", (s, o) => TagParse.TagBoolCheck(s, ref BroadcastCommandProfiles) },
				{"CommandProfileIds", (s, o) => TagParse.TagStringListCheck(s, ref CommandProfileIds) },
				{"AddWaypointFromCommand", (s, o) => TagParse.TagBoolCheck(s, ref AddWaypointFromCommand) },
				{"RecalculateDespawnCoords", (s, o) => TagParse.TagBoolCheck(s, ref RecalculateDespawnCoords) },
				{"AddDatapadsToSeats", (s, o) => TagParse.TagBoolCheck(s, ref AddDatapadsToSeats) },
				{"DatapadNamesToAdd", (s, o) => TagParse.TagStringListCheck(s, ref DatapadNamesToAdd) },
				{"DatapadCountToAdd", (s, o) => TagParse.TagIntCheck(s, ref DatapadCountToAdd) },
				{"ToggleBlocksOfType", (s, o) => TagParse.TagBoolCheck(s, ref ToggleBlocksOfType) },
				{"BlockTypesToToggle", (s, o) => TagParse.TagMyDefIdCheck(s, ref BlockTypesToToggle) },
				{"BlockTypeToggles", (s, o) => TagParse.TagSwitchEnumCheck(s, ref BlockTypeToggles) },
				{"CancelWaitingAtWaypoint", (s, o) => TagParse.TagBoolCheck(s, ref CancelWaitingAtWaypoint) },
				{"SwitchToNextWaypoint", (s, o) => TagParse.TagBoolCheck(s, ref SwitchToNextWaypoint) },
				{"HeavyYaw", (s, o) => TagParse.TagBoolCheck(s, ref HeavyYaw) },
				{"StopAllRotation", (s, o) => TagParse.TagBoolCheck(s, ref StopAllRotation) },
				{"StopAllThrust", (s, o) => TagParse.TagBoolCheck(s, ref StopAllThrust) },
				{"RandomGyroRotation", (s, o) => TagParse.TagBoolCheck(s, ref RandomGyroRotation) },
				{"RandomThrustDirection", (s, o) => TagParse.TagBoolCheck(s, ref RandomThrustDirection) },
				{"ParentGridNameRequirement", (s, o) => TagParse.TagStringCheck(s, ref ParentGridNameRequirement) },
				{"ChangeZoneAtPosition", (s, o) => TagParse.TagBoolCheck(s, ref ChangeZoneAtPosition) },
				{"ZoneName", (s, o) => TagParse.TagStringCheck(s, ref ZoneName) },
				{"ZoneRadiusChangeType", (s, o) => TagParse.TagModifierEnumCheck(s, ref ZoneRadiusChangeType) },
				{"ZoneRadiusChangeAmount", (s, o) => TagParse.TagDoubleCheck(s, ref ZoneRadiusChangeAmount) },
				{"ZoneCustomCounterChange", (s, o) => TagParse.TagBoolCheck(s, ref ZoneCustomCounterChange) },
				{"ZoneCustomCounterChangeType", (s, o) => TagParse.TagModifierEnumCheck(s, ref ZoneCustomCounterChangeType) },
				{"ZoneCustomCounterChangeName", (s, o) => TagParse.TagStringListCheck(s, ref ZoneCustomCounterChangeName) },
				{"ZoneCustomCounterChangeAmount", (s, o) => TagParse.TagIntListCheck(s, ref ZoneCustomCounterChangeAmount) },
				{"ZoneCustomBoolChange", (s, o) => TagParse.TagBoolCheck(s, ref ZoneCustomBoolChange) },
				{"ZoneCustomBoolChangeName", (s, o) => TagParse.TagStringListCheck(s, ref ZoneCustomBoolChangeName) },
				{"ZoneCustomBoolChangeAmount", (s, o) => TagParse.TagBoolListCheck(s, ref ZoneCustomBoolChangeAmount) },

			};

		}

		public void EditValue(string receivedValue) {

			var processedTag = TagParse.ProcessTag(receivedValue);

			if (processedTag.Length < 2)
				return;

			Action<string, object> referenceMethod = null;

			if (!EditorReference.TryGetValue(processedTag[0], out referenceMethod))
				//TODO: Notes About Value Not Found
				return;

			referenceMethod?.Invoke(receivedValue, null);

		}

		public void InitTags(string customData) {

			if (string.IsNullOrWhiteSpace(customData) == false) {

				var descSplit = customData.Split('\n');

				foreach (var tag in descSplit) {

					EditValue(tag);
					
				}

			}

		}

	}

}
