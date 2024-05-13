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
using ModularEncountersSystems.Files;
using ModularEncountersSystems.Progression;

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

		public bool SwitchToBehavior; //OBSOLETE
		public string NewBehavior; //OBSOLETE
		public bool PreserveSettingsOnBehaviorSwitch; //OBSOLETE

		public bool RefreshTarget;

		public bool SwitchTargetProfile; //Obsolete
		public string NewTargetProfile; //Obsolete

		public bool TriggerTimerBlocks;
		public List<string> TimerBlockNames;

		public bool ChangeReputationWithPlayers;
		public double ReputationChangeRadius;
		public List<int> ReputationChangeAmount;
		public List<string> ReputationPlayerConditionIds;

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
		public int IncreaseSandboxCountersAmount;      
		public int DecreaseSandboxCountersAmount;      

		public List<string> ResetSandboxCounters;

		public bool ChangeAttackerReputation;
		public List<string> ChangeAttackerReputationFaction;
		public List<int> ChangeAttackerReputationAmount;
		public List<string> ReputationChangeFactions;
		public bool ReputationChangesForAllRadiusPlayerFactionMembers;
		public bool ReputationChangesForAllAttackPlayerFactionMembers;
		public int ReputationMinCap;
		public int ReputationMaxCap;

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

		public bool PreserveTriggersOnBehaviorSwitch; //OBSOLETE
		public bool PreserveTargetDataOnBehaviorSwitch; //OBSOLETE

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
		public List<string> ResetTriggerCooldownTags;

		public bool EnableTriggers;
		public List<string> EnableTriggerNames;
		public List<string> EnableTriggerTags;

		public bool DisableTriggers;
		public List<string> DisableTriggerNames;
		public List<string> DisableTriggerTags;

		public bool ManuallyActivateTrigger;
		public List<string> ManuallyActivatedTriggerNames;
		public List<string> ManuallyActivatedTriggerTags;

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

		public bool RazeBlocksOfType;
		public List<MyDefinitionId> RazeBlocksTypes;

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
		public bool ChangePlayerCreditsIncludeSavedPlayerIdentity;
		public List<string> ChangePlayerCreditsPlayerConditionIds;
		public bool ChangePlayerCreditsOverridePositionInPlayerCondition;


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

		public bool AddDatapadsToSeats;
		public List<string> DatapadNamesToAdd;
		public int DatapadCountToAdd;

		public bool ToggleBlocksOfType;
		public List<SerializableDefinitionId> BlockTypesToToggle;
		public List<SwitchEnum> BlockTypeToggles;

		public bool ClearAllWaypoints;

		public bool AddWaypoints;
		public List<string> WaypointsToAdd;

		public bool AddWaypointFromCommand;
		public bool RecalculateDespawnCoords;

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

		public bool ZoneToggleActive;
		public bool ZoneToggleActiveMode;

		public bool ZoneToggleActiveAtPosition;
		public bool ZoneToggleActiveAtPositionMode;

		public ModifierEnum ZoneRadiusChangeType;
		public double ZoneRadiusChangeAmount;

		public bool ZoneCustomCounterChange;
		public bool ZoneCustomCounterChangeUseKPL;
		public List<ModifierEnum> ZoneCustomCounterChangeType;
		public List<string> ZoneCustomCounterChangeName;
		public List<long> ZoneCustomCounterChangeAmount;

		public bool ZoneCustomBoolChange;
		public bool ZoneCustomBoolChangeUseKPL;
		public List<string> ZoneCustomBoolChangeName;
		public List<bool> ZoneCustomBoolChangeValue;

		public bool AddBotsToGrid;
		public int BotCount;
		public List<string> BotSpawnProfileNames;
		public bool OnlySpawnBotsInPressurizedRooms;

		public bool SetWeaponsToMinRange;
		public bool SetWeaponsToMaxRange;

		public bool ChangeBehaviorSubclass;
		public BehaviorSubclass NewBehaviorSubclass;

		public bool EnableHighestRangeAntennas;
		public bool DisableHighestRangeAntennas;

		public bool AssignEscortFromCommand; 

		public bool UseCurrentPositionAsPatrolReference;
		public bool ClearCustomPatrolReference;

		public bool SetGridToStatic;
		public bool SetGridToDynamic;

		public BoolEnum UseJetpackInhibitorEffect; 
		public BoolEnum UseDrillInhibitorEffect; 
		public BoolEnum UseNanobotInhibitorEffect; 
		public BoolEnum UseJumpInhibitorEffect; 
		public BoolEnum UsePlayerInhibitorEffect;

		public bool ChangeTurretTargetingParameters; //Implement
		public string TurretTargetType; //Implement
		public List<string> TurretTypesForTargetChanges; //Implement
		public List<string> TurretSubtypesForTargetChange; //Implement

		public bool JumpToTarget; 
		public bool JumpToJumpedEntity; 
		public bool JumpedEntityMustBeTarget;

		public bool JumpToWaypoint;
		public string JumpWaypoint;

		public bool SetGridCleanupExempt; 
		public int GridCleanupExemptDuration; 

		public bool PlaySoundAtPosition; 
		public string SoundAtPosition; 

		public bool SpawnPlanet; 
		public string PlanetName; 
		public float PlanetSize; 
		public bool PlanetIgnoreSafeLocation; //Implement //Doc
		public string PlanetWaypointProfile; 
		public bool TemporaryPlanet; 
		public int PlanetTimeLimit;

		public bool AddCustomDataToBlocks;
		public List<string> CustomDataBlockNames;
		public List<TextTemplate> CustomDataFiles;

		public bool ApplyContainerTypeToInventoryBlock;
		public List<string> ContainerTypeBlockNames;
		public List<string> ContainerTypeSubtypeIds;

		public string DebugMessage;

		public bool ApplyLcdChanges;
		public string LcdTextTemplateFile;
		public List<string> LcdBlockNames;
		public List<int> LcdTemplateIndexes;

		public bool AddResearchPoints;
		public int ResearchPointsAmount;

		public bool ApplyStoreProfiles;
		public bool ClearStoreContentsFirst;
		public List<string> StoreBlocks;
		public List<string> StoreProfiles;

		public bool ActivateEvent;
		public List<string> ActivateEventIds;
		public List<string> ActivateEventTags;

		public bool CreateSafeZone;
		public string SafeZoneProfile;
		public bool LinkSafeZoneToRemoteControl;
		public bool SafeZonePositionGridCenter;
		public bool SafeZonePositionTerrainSurface;
		public bool IgnoreOtherSafeZonesDuringCreation;

		public bool RemoveSafeZonesAtPosition;

		public bool SavePlayerIdentity;
		public bool RemovePlayerIdentity;

		public bool AddTagsToPlayers;
		public bool AddTagsIncludeSavedPlayerIdentity;
		public List<string> AddTagsPlayerConditionIds;
		public List<string> AddTags;
		public bool AddTagsOverridePositionInPlayerCondition;

		public bool RemoveTagsFromPlayers;
		public bool RemoveTagsIncludeSavedPlayerIdentity;
		public List<string> RemoveTagsPlayerConditionIds;
		public List<string> RemoveTags;
		public bool RemoveTagsOverridePositioninPlayerCondition;


		public bool PlayDialogueCue;
		public string DialogueCueId;


		public bool ResetCooldownTimeOfEvents;
		public List<string> ResetEventCooldownIds;
		public List<string> ResetEventCooldownTags;

		public bool ToggleEvents;
		public List<string> ToggleEventIds;
		public List<bool> ToggleEventIdModes;

		public List<string> ToggleEventTags;
		public List<bool> ToggleEventTagModes;

		public bool DisableAutopilot;
		public bool EnableAutopilot;

		public bool ResetThisStaticEncounter;

		public bool ChangeBlocksShareModeAll;
		public List<string> BlockNamesShareModeAll;

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

			SwitchToBehavior = false; //OBSOLETE
			NewBehavior = ""; //OBSOLETE
			PreserveSettingsOnBehaviorSwitch = false; //OBSOLETE
			PreserveTriggersOnBehaviorSwitch = false; //OBSOLETE
			PreserveTargetDataOnBehaviorSwitch = false; //OBSOLETE

			RefreshTarget = false;

			TriggerTimerBlocks = false;
			TimerBlockNames = new List<string>();

			ChangeReputationWithPlayers = false;
			ReputationChangeRadius = 0;
			ReputationChangeFactions = new List<string>();
			ReputationChangeAmount = new List<int>();
			ReputationChangesForAllRadiusPlayerFactionMembers = false;
			ReputationPlayerConditionIds = new List<string>();


			ChangeAttackerReputation = false;
			ChangeAttackerReputationFaction = new List<string>();
			ChangeAttackerReputationAmount = new List<int>();
			ReputationChangesForAllAttackPlayerFactionMembers = false;
			ReputationMinCap = -1500;
			ReputationMaxCap = 1500;

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

			
			IncreaseSandboxCountersAmount = 1;
			DecreaseSandboxCountersAmount = -1;


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
			ResetTriggerCooldownTags = new List<string>();

			ChangeInertiaDampeners = false;
			InertiaDampenersEnable = false;

			EnableTriggers = false;
			EnableTriggerNames = new List<string>();
			EnableTriggerTags = new List<string>();

			DisableTriggers = false;
			DisableTriggerNames = new List<string>();
			DisableTriggerTags = new List<string>();

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

			RazeBlocksOfType = false;
			RazeBlocksTypes = new List<MyDefinitionId>();

			ManuallyActivateTrigger = false;
			ManuallyActivatedTriggerNames = new List<string>();
			ManuallyActivatedTriggerTags = new List<string>();


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
			ChangePlayerCreditsIncludeSavedPlayerIdentity =true;
			ChangePlayerCreditsPlayerConditionIds = new List<string>();
			ChangePlayerCreditsOverridePositionInPlayerCondition =true;

			ChangeNpcFactionCredits = false;
			ChangeNpcFactionCreditsAmount = 0;
			ChangeNpcFactionCreditsTag = "";

			BuildProjectedBlocks = false;
			MaxProjectedBlocksToBuild = -1;

			ForceManualTriggerActivation = false;

			BroadcastCommandProfiles = false;
			CommandProfileIds = new List<string>();

			AddDatapadsToSeats = false;
			DatapadNamesToAdd = new List<string>();
			DatapadCountToAdd = 1;

			ToggleBlocksOfType = false;
			BlockTypesToToggle = new List<SerializableDefinitionId>();
			BlockTypeToggles = new List<SwitchEnum>();

			ClearAllWaypoints = false;

			AddWaypoints = false;
			WaypointsToAdd = new List<string>();

			AddWaypointFromCommand = false;
			RecalculateDespawnCoords = false;

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

			ZoneToggleActive = false;
			ZoneToggleActiveMode = false;

			ZoneToggleActiveAtPosition = false;
			ZoneToggleActiveAtPositionMode = false;

			ZoneRadiusChangeType = ModifierEnum.None;
			ZoneRadiusChangeAmount = 0;

			ZoneCustomCounterChange = false;
			ZoneCustomCounterChangeUseKPL = false;
			ZoneCustomCounterChangeType = new List<ModifierEnum>();
			ZoneCustomCounterChangeName = new List<string>();
			ZoneCustomCounterChangeAmount = new List<long>();

			ZoneCustomBoolChange = false;
			ZoneCustomBoolChangeUseKPL = false;
			ZoneCustomBoolChangeName = new List<string>();
			ZoneCustomBoolChangeValue = new List<bool>();

			AddBotsToGrid = false;
			BotCount = 1;
			BotSpawnProfileNames = new List<string>();
			OnlySpawnBotsInPressurizedRooms = false;

			ChangeBehaviorSubclass = false;
			NewBehaviorSubclass = BehaviorSubclass.None;

			SetWeaponsToMinRange = false;
			SetWeaponsToMaxRange = false;

			EnableHighestRangeAntennas = false;
			DisableHighestRangeAntennas = false;

			AssignEscortFromCommand = false;

			UseCurrentPositionAsPatrolReference = false;
			ClearCustomPatrolReference = false;

			SetGridToStatic = false;
			SetGridToDynamic = false;

			UseJetpackInhibitorEffect = BoolEnum.None;
			UseDrillInhibitorEffect = BoolEnum.None;
			UseNanobotInhibitorEffect = BoolEnum.None;
			UseJumpInhibitorEffect = BoolEnum.None;
			UsePlayerInhibitorEffect = BoolEnum.None;

			ChangeTurretTargetingParameters = false;
			TurretTargetType = "";
			TurretTypesForTargetChanges = new List<string>();
			TurretSubtypesForTargetChange = new List<string>();

			JumpToTarget = false;
			JumpToJumpedEntity = false;
			JumpedEntityMustBeTarget = false;

			JumpToWaypoint = false;
			JumpWaypoint = "";

			SetGridCleanupExempt = false;
			GridCleanupExemptDuration = 30;

			PlaySoundAtPosition = false;
			SoundAtPosition = "";

			SpawnPlanet = false;
			PlanetName = "";
			PlanetSize = 120000;
			PlanetIgnoreSafeLocation = false;
			PlanetWaypointProfile = "";
			TemporaryPlanet = false;
			PlanetTimeLimit = 10;

			AddCustomDataToBlocks = false;
			CustomDataBlockNames = new List<string>();
			CustomDataFiles = new List<TextTemplate>();

			ApplyContainerTypeToInventoryBlock = false;
			ContainerTypeBlockNames = new List<string>();
			ContainerTypeSubtypeIds = new List<string>();

			DebugMessage = "";

			ApplyLcdChanges = false;
			LcdTextTemplateFile = "";
			LcdBlockNames = new List<string>();
			LcdTemplateIndexes = new List<int>();

			AddResearchPoints = false;
			ResearchPointsAmount = 0;

			ApplyStoreProfiles = false;
			ClearStoreContentsFirst = false;
			StoreBlocks = new List<string>();
			StoreProfiles = new List<string>();

			ActivateEvent = false;
			ActivateEventIds = new List<string>();
			ActivateEventTags = new List<string>();


			CreateSafeZone = false;

			ProfileSubtypeId = "";
			SafeZoneProfile = "";
			LinkSafeZoneToRemoteControl = false;
			SafeZonePositionGridCenter = false;
			SafeZonePositionTerrainSurface = false;
			IgnoreOtherSafeZonesDuringCreation = false;

			RemoveSafeZonesAtPosition = false;

			SavePlayerIdentity = false;
			RemovePlayerIdentity = false;

			AddTagsToPlayers = false;
			AddTagsIncludeSavedPlayerIdentity = false;
			AddTagsPlayerConditionIds = new List<string>();
			AddTags = new List<string>();
			AddTagsOverridePositionInPlayerCondition = false;

			RemoveTagsFromPlayers = false;
			RemoveTagsIncludeSavedPlayerIdentity = false;
			RemoveTagsPlayerConditionIds = new List<string>();
			RemoveTags = new List<string>();
			RemoveTagsOverridePositioninPlayerCondition = false;

			PlayDialogueCue = false;
			DialogueCueId = "";


			ResetCooldownTimeOfEvents = false;
			ResetEventCooldownIds = new List<string>();
			ResetEventCooldownTags = new List<string>();

			ToggleEvents = false;

			ToggleEventIds = new List<string>();
			ToggleEventIdModes = new List<bool>();
			ToggleEventTags = new List<string>();
			ToggleEventTagModes = new List<bool>();

			
			DisableAutopilot = false;
			EnableAutopilot = false;

			ChangeBlocksShareModeAll = false;
			BlockNamesShareModeAll = new List<string>();

			ResetThisStaticEncounter = false;

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
				{"ReputationPlayerConditionIds", (s, o) => TagParse.TagStringListCheck(s, ref ReputationPlayerConditionIds) },
				{"ReputationChangeAmount", (s, o) => TagParse.TagIntListCheck(s, ref ReputationChangeAmount) },
				{"ActivateAssertiveAntennas", (s, o) => TagParse.TagBoolCheck(s, ref ActivateAssertiveAntennas) },
				{"ChangeAntennaOwnership", (s, o) => TagParse.TagBoolCheck(s, ref ChangeAntennaOwnership) },
				{"AntennaFactionOwner", (s, o) => TagParse.TagStringCheck(s, ref AntennaFactionOwner) },
				{"CreateKnownPlayerArea", (s, o) => TagParse.TagBoolCheck(s, ref CreateKnownPlayerArea) },
				{"KnownPlayerAreaRadius", (s, o) => TagParse.TagDoubleCheck(s, ref KnownPlayerAreaRadius) },
				{"KnownPlayerAreaTimer", (s, o) => TagParse.TagIntOrDayCheck(s, ref KnownPlayerAreaTimer) },
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
				{"IncreaseSandboxCountersAmount", (s, o) => TagParse.TagIntCheck(s, ref IncreaseSandboxCountersAmount) }, 
				{"DecreaseSandboxCountersAmount", (s, o) => TagParse.TagIntCheck(s, ref DecreaseSandboxCountersAmount) }, 
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
				{"ResetTriggerCooldownTags", (s, o) => TagParse.TagStringListCheck(s, ref ResetTriggerCooldownTags) },
				{"BroadcastGenericCommand", (s, o) => TagParse.TagBoolCheck(s, ref BroadcastGenericCommand) },
				{"BehaviorSpecificEventA", (s, o) => TagParse.TagBoolCheck(s, ref BehaviorSpecificEventA) },
				{"ChangeInertiaDampeners", (s, o) => TagParse.TagBoolCheck(s, ref ChangeInertiaDampeners) },
				{"InertiaDampenersEnable", (s, o) => TagParse.TagBoolCheck(s, ref InertiaDampenersEnable) },
				{"EnableTriggers", (s, o) => TagParse.TagBoolCheck(s, ref EnableTriggers) },
				{"EnableTriggerNames", (s, o) => TagParse.TagStringListCheck(s, ref EnableTriggerNames) },
				{"EnableTriggerTags", (s, o) => TagParse.TagStringListCheck(s, ref EnableTriggerTags) },
				{"DisableTriggers", (s, o) => TagParse.TagBoolCheck(s, ref DisableTriggers) },
				{"DisableTriggerNames", (s, o) => TagParse.TagStringListCheck(s, ref DisableTriggerNames) },
				{"DisableTriggerTags", (s, o) => TagParse.TagStringListCheck(s, ref DisableTriggerTags) },
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
				{"RazeBlocksOfType", (s, o) => TagParse.TagBoolCheck(s, ref RazeBlocksOfType) },
				{"RazeBlocksTypes", (s, o) => TagParse.TagMyDefIdCheck(s, ref RazeBlocksTypes) },
				{"ManuallyActivateTrigger", (s, o) => TagParse.TagBoolCheck(s, ref ManuallyActivateTrigger) },
				{"ManuallyActivatedTriggerNames", (s, o) => TagParse.TagStringListCheck(s, ref ManuallyActivatedTriggerNames) },
				{"ManuallyActivatedTriggerTags", (s, o) => TagParse.TagStringListCheck(s, ref ManuallyActivatedTriggerTags) },
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
				{"ChangePlayerCreditsIncludeSavedPlayerIdentity", (s, o) => TagParse.TagBoolCheck(s, ref ChangePlayerCreditsIncludeSavedPlayerIdentity) },
				{"ChangePlayerCreditsPlayerConditionIds", (s, o) => TagParse.TagStringListCheck(s, ref ChangePlayerCreditsPlayerConditionIds) },
				{"ChangePlayerCreditsOverridePositionInPlayerCondition", (s, o) => TagParse.TagBoolCheck(s, ref ChangePlayerCreditsOverridePositionInPlayerCondition) },
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
				{"ClearAllWaypoints", (s, o) => TagParse.TagBoolCheck(s, ref ClearAllWaypoints) },
				{"AddWaypoints", (s, o) => TagParse.TagBoolCheck(s, ref AddWaypoints) },
				{"WaypointsToAdd", (s, o) => TagParse.TagStringListCheck(s, ref WaypointsToAdd) },
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
				{"ZoneToggleActive", (s, o) => TagParse.TagBoolCheck(s, ref ZoneToggleActive) },
				{"ZoneToggleActiveMode", (s, o) => TagParse.TagBoolCheck(s, ref ZoneToggleActiveMode) },
				{"ZoneToggleActiveAtPosition", (s, o) => TagParse.TagBoolCheck(s, ref ZoneToggleActiveAtPosition) },
				{"ZoneToggleActiveAtPositionMode", (s, o) => TagParse.TagBoolCheck(s, ref ZoneToggleActiveAtPositionMode) },
				{"ZoneRadiusChangeType", (s, o) => TagParse.TagModifierEnumCheck(s, ref ZoneRadiusChangeType) },
				{"ZoneRadiusChangeAmount", (s, o) => TagParse.TagDoubleCheck(s, ref ZoneRadiusChangeAmount) },
				{"ZoneCustomCounterChange", (s, o) => TagParse.TagBoolCheck(s, ref ZoneCustomCounterChange) },
				{"ZoneCustomCounterChangeUseKPL", (s, o) => TagParse.TagBoolCheck(s, ref ZoneCustomCounterChangeUseKPL) },
				{"ZoneCustomCounterChangeType", (s, o) => TagParse.TagModifierEnumCheck(s, ref ZoneCustomCounterChangeType) },
				{"ZoneCustomCounterChangeName", (s, o) => TagParse.TagStringListCheck(s, ref ZoneCustomCounterChangeName) },
				{"ZoneCustomCounterChangeAmount", (s, o) => TagParse.TagLongCheck(s, ref ZoneCustomCounterChangeAmount) },
				{"ZoneCustomBoolChange", (s, o) => TagParse.TagBoolCheck(s, ref ZoneCustomBoolChange) },
				{"ZoneCustomBoolChangeUseKPL", (s, o) => TagParse.TagBoolCheck(s, ref ZoneCustomBoolChangeUseKPL) },
				{"ZoneCustomBoolChangeName", (s, o) => TagParse.TagStringListCheck(s, ref ZoneCustomBoolChangeName) },
				{"ZoneCustomBoolChangeValue", (s, o) => TagParse.TagBoolListCheck(s, ref ZoneCustomBoolChangeValue) },
				{"AddBotsToGrid", (s, o) => TagParse.TagBoolCheck(s, ref AddBotsToGrid) },
				{"BotCount", (s, o) => TagParse.TagIntCheck(s, ref BotCount) },
				{"BotSpawnProfileNames", (s, o) => TagParse.TagStringListCheck(s, ref BotSpawnProfileNames) },
				{"OnlySpawnBotsInPressurizedRooms", (s, o) => TagParse.TagBoolCheck(s, ref OnlySpawnBotsInPressurizedRooms) },
				{"ChangeBehaviorSubclass", (s, o) => TagParse.TagBoolCheck(s, ref ChangeBehaviorSubclass) },
				{"NewBehaviorSubclass", (s, o) => TagParse.TagBehaviorSubclassEnumCheck(s, ref NewBehaviorSubclass) },
				{"SetWeaponsToMinRange", (s, o) => TagParse.TagBoolCheck(s, ref SetWeaponsToMinRange) },
				{"SetWeaponsToMaxRange", (s, o) => TagParse.TagBoolCheck(s, ref SetWeaponsToMaxRange) },
				{"EnableHighestRangeAntennas", (s, o) => TagParse.TagBoolCheck(s, ref EnableHighestRangeAntennas) },
				{"DisableHighestRangeAntennas", (s, o) => TagParse.TagBoolCheck(s, ref DisableHighestRangeAntennas) },
				{"AssignEscortFromCommand", (s, o) => TagParse.TagBoolCheck(s, ref AssignEscortFromCommand) },
				{"UseCurrentPositionAsPatrolReference", (s, o) => TagParse.TagBoolCheck(s, ref UseCurrentPositionAsPatrolReference) },
				{"ClearCustomPatrolReference", (s, o) => TagParse.TagBoolCheck(s, ref ClearCustomPatrolReference) },
				{"SetGridToStatic", (s, o) => TagParse.TagBoolCheck(s, ref SetGridToStatic) },
				{"SetGridToDynamic", (s, o) => TagParse.TagBoolCheck(s, ref SetGridToDynamic) },
				{"UseJetpackInhibitorEffect", (s, o) => TagParse.TagBoolEnumCheck(s, ref UseJetpackInhibitorEffect) },
				{"UseDrillInhibitorEffect", (s, o) => TagParse.TagBoolEnumCheck(s, ref UseDrillInhibitorEffect) },
				{"UseNanobotInhibitorEffect", (s, o) => TagParse.TagBoolEnumCheck(s, ref UseNanobotInhibitorEffect) },
				{"UseJumpInhibitorEffect", (s, o) => TagParse.TagBoolEnumCheck(s, ref UseJumpInhibitorEffect) },
				{"UsePlayerInhibitorEffect", (s, o) => TagParse.TagBoolEnumCheck(s, ref UsePlayerInhibitorEffect) },
				{"ChangeTurretTargetingParameters", (s, o) => TagParse.TagBoolCheck(s, ref ChangeTurretTargetingParameters) },
				{"TurretTargetType", (s, o) => TagParse.TagStringCheck(s, ref TurretTargetType) },
				{"TurretTypesForTargetChanges", (s, o) => TagParse.TagStringListCheck(s, ref TurretTypesForTargetChanges) },
				{"TurretSubtypesForTargetChange", (s, o) => TagParse.TagStringListCheck(s, ref TurretSubtypesForTargetChange) },
				{"JumpToTarget", (s, o) => TagParse.TagBoolCheck(s, ref JumpToTarget) },
				{"JumpToJumpedEntity", (s, o) => TagParse.TagBoolCheck(s, ref JumpToJumpedEntity) },
				{"JumpedEntityMustBeTarget", (s, o) => TagParse.TagBoolCheck(s, ref JumpedEntityMustBeTarget) },
				{"JumpToWaypoint", (s, o) => TagParse.TagBoolCheck(s, ref JumpToWaypoint) },
				{"JumpWaypoint", (s, o) => TagParse.TagStringCheck(s, ref JumpWaypoint) },
				{"SetGridCleanupExempt", (s, o) => TagParse.TagBoolCheck(s, ref SetGridCleanupExempt) },
				{"GridCleanupExemptDuration", (s, o) => TagParse.TagIntCheck(s, ref GridCleanupExemptDuration) },
				{"PlaySoundAtPosition", (s, o) => TagParse.TagBoolCheck(s, ref PlaySoundAtPosition) },
				{"SoundAtPosition", (s, o) => TagParse.TagStringCheck(s, ref SoundAtPosition) },

				{"SpawnPlanet", (s, o) => TagParse.TagBoolCheck(s, ref SpawnPlanet) },
				{"PlanetName", (s, o) => TagParse.TagStringCheck(s, ref PlanetName) },
				{"PlanetSize", (s, o) => TagParse.TagFloatCheck(s, ref PlanetSize) },
				{"PlanetIgnoreSafeLocation", (s, o) => TagParse.TagBoolCheck(s, ref PlanetIgnoreSafeLocation) },
				{"PlanetWaypointProfile", (s, o) => TagParse.TagStringCheck(s, ref PlanetWaypointProfile) },
				{"TemporaryPlanet", (s, o) => TagParse.TagBoolCheck(s, ref TemporaryPlanet) },
				{"PlanetTimeLimit", (s, o) => TagParse.TagIntCheck(s, ref PlanetTimeLimit) },

				{"AddCustomDataToBlocks", (s, o) => TagParse.TagBoolCheck(s, ref AddCustomDataToBlocks) },
				{"CustomDataBlockNames", (s, o) => TagParse.TagStringListCheck(s, ref CustomDataBlockNames) },
				{"CustomDataFiles", (s, o) => TagParse.TagTextTemplateCheck(s, ref CustomDataFiles) },

				{"ApplyContainerTypeToInventoryBlock", (s, o) => TagParse.TagBoolCheck(s, ref ApplyContainerTypeToInventoryBlock) },
				{"ContainerTypeBlockNames", (s, o) => TagParse.TagStringListCheck(s, ref ContainerTypeBlockNames) },
				{"ContainerTypeSubtypeIds", (s, o) => TagParse.TagStringListCheck(s, ref ContainerTypeSubtypeIds) },

				{"DebugMessage", (s, o) => TagParse.TagStringCheck(s, ref DebugMessage) },

				{"ApplyLcdChanges", (s, o) => TagParse.TagBoolCheck(s, ref ApplyLcdChanges) },
				{"LcdTextTemplateFile", (s, o) => TagParse.TagStringCheck(s, ref LcdTextTemplateFile) },
				{"LcdBlockNames", (s, o) => TagParse.TagStringListCheck(s, ref LcdBlockNames) },
				{"LcdTemplateIndexes", (s, o) => TagParse.TagIntListCheck(s, true, ref LcdTemplateIndexes) },

				{"AddResearchPoints", (s, o) => TagParse.TagBoolCheck(s, ref AddResearchPoints) },
				{"ResearchPointsAmount", (s, o) => TagParse.TagIntCheck(s, ref ResearchPointsAmount) },

				{"ApplyStoreProfiles", (s, o) => TagParse.TagBoolCheck(s, ref ApplyStoreProfiles) },
				{"ClearStoreContentsFirst", (s, o) => TagParse.TagBoolCheck(s, ref ClearStoreContentsFirst) },
				{"StoreBlocks", (s, o) => TagParse.TagStringListCheck(s, ref StoreBlocks) },
				{"StoreProfiles", (s, o) => TagParse.TagStringListCheck(s, ref StoreProfiles) },

				{"ActivateEvent", (s, o) => TagParse.TagBoolCheck(s, ref ActivateEvent) },
				{"ActivateEventIds", (s, o) => TagParse.TagStringListCheck(s, ref ActivateEventIds) },
				{"ActivateEventTags", (s, o) => TagParse.TagStringListCheck(s, ref ActivateEventTags) },

				{"CreateSafeZone", (s, o) => TagParse.TagBoolCheck(s, ref CreateSafeZone) },
				{"SafeZoneProfile", (s, o) => TagParse.TagStringCheck(s, ref SafeZoneProfile) },
				{"LinkSafeZoneToRemoteControl", (s, o) => TagParse.TagBoolCheck(s, ref LinkSafeZoneToRemoteControl) },
				{"SafeZonePositionGridCenter", (s, o) => TagParse.TagBoolCheck(s, ref SafeZonePositionGridCenter) },
				{"SafeZonePositionTerrainSurface", (s, o) => TagParse.TagBoolCheck(s, ref SafeZonePositionTerrainSurface) },
				{"IgnoreOtherSafeZonesDuringCreation", (s, o) => TagParse.TagBoolCheck(s, ref IgnoreOtherSafeZonesDuringCreation) },

				{"SavePlayerIdentity", (s, o) => TagParse.TagBoolCheck(s, ref SavePlayerIdentity) },
				{"RemovePlayerIdentity", (s, o) => TagParse.TagBoolCheck(s, ref RemovePlayerIdentity) },

				{"AddTagstoPlayers", (s, o) => TagParse.TagBoolCheck(s, ref AddTagsToPlayers) },
				{"AddTagsToPlayers", (s, o) => TagParse.TagBoolCheck(s, ref AddTagsToPlayers) },
				{"AddTagsIncludeSavedPlayerIdentity", (s, o) => TagParse.TagBoolCheck(s, ref AddTagsIncludeSavedPlayerIdentity) },
				{"AddTagsPlayerConditionIds", (s, o) => TagParse.TagStringListCheck(s, ref AddTagsPlayerConditionIds) },
				{"AddTags", (s, o) => TagParse.TagStringListCheck(s, ref AddTags) },
				{"AddTagsOverridePositionInPlayerCondition", (s, o) => TagParse.TagBoolCheck(s, ref AddTagsOverridePositionInPlayerCondition) },


				{"RemoveTagsFromPlayers", (s, o) => TagParse.TagBoolCheck(s, ref RemoveTagsFromPlayers) },
				{"RemoveTagsIncludeSavedPlayerIdentity", (s, o) => TagParse.TagBoolCheck(s, ref RemoveTagsIncludeSavedPlayerIdentity) },
				{"RemoveTagsPlayerConditionIds", (s, o) => TagParse.TagStringListCheck(s, ref RemoveTagsPlayerConditionIds) },
				{"RemoveTags", (s, o) => TagParse.TagStringListCheck(s, ref RemoveTags) },
				{"RemoveTagsOverridePositioninPlayerCondition", (s, o) => TagParse.TagBoolCheck(s, ref RemoveTagsOverridePositioninPlayerCondition) },

				{"PlayDialogueCue", (s, o) => TagParse.TagBoolCheck(s, ref PlayDialogueCue) },

				{"DialogueCueId", (s, o) => TagParse.TagStringCheck(s, ref DialogueCueId) },

				{"ResetCooldownTimeOfEvents", (s, o) => TagParse.TagBoolCheck(s, ref ResetCooldownTimeOfEvents) },
				{"ResetEventCooldownIds", (s, o) => TagParse.TagStringListCheck(s, ref ResetEventCooldownIds) },
				{"ResetEventCooldownTags", (s, o) => TagParse.TagStringListCheck(s, ref ResetEventCooldownTags) },

				{"ToggleEvents", (s, o) => TagParse.TagBoolCheck(s, ref ToggleEvents) },

				{"ToggleEventIds", (s, o) => TagParse.TagStringListCheck(s, ref ToggleEventIds) },
				{"ToggleEventIdModes", (s, o) => TagParse.TagBoolListCheck(s, ref ToggleEventIdModes) },

				{"ToggleEventTags", (s, o) => TagParse.TagStringListCheck(s, ref ToggleEventTags) },
				{"ToggleEventTagModes", (s, o) => TagParse.TagBoolListCheck(s, ref ToggleEventTagModes) },

				{"ResetThisStaticEncounter", (s, o) => TagParse.TagBoolCheck(s, ref ResetThisStaticEncounter) },

				{"DisableAutopilot", (s, o) => TagParse.TagBoolCheck(s, ref DisableAutopilot) },
				{"EnableAutopilot", (s, o) => TagParse.TagBoolCheck(s, ref EnableAutopilot) },

				{"ChangeBlocksShareModeAll", (s, o) => TagParse.TagBoolCheck(s, ref ChangeBlocksShareModeAll) },
				{"BlockNamesShareModeAll", (s, o) => TagParse.TagStringListCheck(s, ref BlockNamesShareModeAll) },

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
