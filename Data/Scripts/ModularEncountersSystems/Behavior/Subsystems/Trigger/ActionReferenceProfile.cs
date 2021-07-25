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

			ProfileSubtypeId = "";

			EditorReference = new Dictionary<string, Action<string, object>> {

				{"UseChatBroadcast", (s, o) => TagParse.TagBoolCheck(s, ref UseChatBroadcast) },

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

					//UseChatBroadcast
					if (tag.Contains("[UseChatBroadcast:") == true) {

						TagParse.TagBoolCheck(tag, ref UseChatBroadcast);

					}

					//BarrelRoll
					if (tag.Contains("[BarrelRoll:") == true) {

						 TagParse.TagBoolCheck(tag, ref BarrelRoll);

					}

					//Strafe
					if (tag.Contains("[Strafe:") == true) {

						 TagParse.TagBoolCheck(tag, ref Strafe);

					}

					//ChangeAutopilotSpeed
					if (tag.Contains("[ChangeAutopilotSpeed:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeAutopilotSpeed);

					}

					//NewAutopilotSpeed
					if (tag.Contains("[NewAutopilotSpeed:") == true) {

						 TagParse.TagFloatCheck(tag, ref NewAutopilotSpeed);

					}

					//SpawnEncounter
					if (tag.Contains("[SpawnEncounter:") == true) {

						 TagParse.TagBoolCheck(tag, ref SpawnEncounter);

					}

					//SelfDestruct
					if (tag.Contains("[SelfDestruct:") == true) {

						 TagParse.TagBoolCheck(tag, ref SelfDestruct);

					}

					//Retreat
					if (tag.Contains("[Retreat:") == true) {

						 TagParse.TagBoolCheck(tag, ref Retreat);

					}

					//TerminateBehavior
					if (tag.Contains("[TerminateBehavior:") == true) {

						 TagParse.TagBoolCheck(tag, ref TerminateBehavior);

					}

					//BroadcastCurrentTarget
					if (tag.Contains("[BroadcastCurrentTarget:") == true) {

						 TagParse.TagBoolCheck(tag, ref BroadcastCurrentTarget);

					}

					//SwitchToReceivedTarget
					if (tag.Contains("[SwitchToReceivedTarget:") == true) {

						 TagParse.TagBoolCheck(tag, ref SwitchToReceivedTarget);

					}

					//SwitchTargetToDamager
					if (tag.Contains("[SwitchTargetToDamager:") == true) {

						 TagParse.TagBoolCheck(tag, ref SwitchTargetToDamager);

					}

					//BroadcastDamagerTarget
					if (tag.Contains("[BroadcastDamagerTarget:") == true) {

						 TagParse.TagBoolCheck(tag, ref BroadcastDamagerTarget);

					}

					//BroadcastSendCode
					if (tag.Contains("[BroadcastSendCode:") == true) {

						 TagParse.TagStringCheck(tag, ref BroadcastSendCode);

					}

					//SwitchToBehavior
					if (tag.Contains("[SwitchToBehavior:") == true) {

						 TagParse.TagBoolCheck(tag, ref SwitchToBehavior);

					}

					//NewBehavior
					if (tag.Contains("[NewBehavior:") == true) {

						 TagParse.TagStringCheck(tag, ref NewBehavior);

					}

					//PreserveSettingsOnBehaviorSwitch
					if (tag.Contains("[PreserveSettingsOnBehaviorSwitch:") == true) {

						 TagParse.TagBoolCheck(tag, ref PreserveSettingsOnBehaviorSwitch);

					}

					//PreserveTriggersOnBehaviorSwitch
					if (tag.Contains("[PreserveTriggersOnBehaviorSwitch:") == true) {

						 TagParse.TagBoolCheck(tag, ref PreserveTriggersOnBehaviorSwitch);

					}

					//PreserveTargetDataOnBehaviorSwitch
					if (tag.Contains("[PreserveTargetDataOnBehaviorSwitch:") == true) {

						 TagParse.TagBoolCheck(tag, ref PreserveTargetDataOnBehaviorSwitch);

					}

					//RefreshTarget
					if (tag.Contains("[RefreshTarget:") == true) {

						 TagParse.TagBoolCheck(tag, ref RefreshTarget);

					}

					//SwitchTargetProfile
					if (tag.Contains("[SwitchTargetProfile:") == true) {

						 TagParse.TagBoolCheck(tag, ref SwitchTargetProfile);

					}

					//NewTargetProfile
					if (tag.Contains("[NewTargetProfile:") == true) {

						 TagParse.TagStringCheck(tag, ref NewTargetProfile);

					}

					//TriggerTimerBlocks
					if (tag.Contains("[TriggerTimerBlocks:") == true) {

						 TagParse.TagBoolCheck(tag, ref TriggerTimerBlocks);

					}

					//TimerBlockNames
					if (tag.Contains("[TimerBlockNames:") == true) {

						TagParse.TagStringListCheck(tag, ref TimerBlockNames);

					}

					//ChangeReputationWithPlayers
					if (tag.Contains("[ChangeReputationWithPlayers:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeReputationWithPlayers);

					}

					//ReputationChangeRadius
					if (tag.Contains("[ReputationChangeRadius:") == true) {

						 TagParse.TagDoubleCheck(tag, ref ReputationChangeRadius);

					}

					//ReputationChangeFactions
					if (tag.Contains("[ReputationChangeFactions:") == true) {

						TagParse.TagStringListCheck(tag, ref ReputationChangeFactions);

					}

					//ReputationChangeAmount
					if (tag.Contains("[ReputationChangeAmount:") == true) {

						TagParse.TagIntListCheck(tag, ref ReputationChangeAmount);

					}

					//ActivateAssertiveAntennas
					if (tag.Contains("[ActivateAssertiveAntennas:") == true) {

						 TagParse.TagBoolCheck(tag, ref ActivateAssertiveAntennas);

					}

					//ChangeAntennaOwnership
					if (tag.Contains("[ChangeAntennaOwnership:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeAntennaOwnership);

					}

					//AntennaFactionOwner
					if (tag.Contains("[AntennaFactionOwner:") == true) {

						 TagParse.TagStringCheck(tag, ref AntennaFactionOwner);

					}

					//CreateKnownPlayerArea
					if (tag.Contains("[CreateKnownPlayerArea:") == true) {

						 TagParse.TagBoolCheck(tag, ref CreateKnownPlayerArea);

					}

					//KnownPlayerAreaRadius
					if (tag.Contains("[KnownPlayerAreaRadius:") == true) {

						 TagParse.TagDoubleCheck(tag, ref KnownPlayerAreaRadius);

					}

					//KnownPlayerAreaTimer
					if (tag.Contains("[KnownPlayerAreaTimer:") == true) {

						 TagParse.TagIntCheck(tag, ref KnownPlayerAreaTimer);

					}

					//KnownPlayerAreaMaxSpawns
					if (tag.Contains("[KnownPlayerAreaMaxSpawns:") == true) {

						 TagParse.TagIntCheck(tag, ref KnownPlayerAreaMaxSpawns);

					}

					//KnownPlayerAreaMinThreatForAvoidingAbandonment
					if (tag.Contains("[KnownPlayerAreaMinThreatForAvoidingAbandonment:") == true) {

						 TagParse.TagIntCheck(tag, ref KnownPlayerAreaMinThreatForAvoidingAbandonment);

					}

					//DamageToolAttacker
					if (tag.Contains("[DamageToolAttacker:") == true) {

						 TagParse.TagBoolCheck(tag, ref DamageToolAttacker);

					}

					//DamageToolAttackerAmount
					if (tag.Contains("[DamageToolAttackerAmount:") == true) {

						 TagParse.TagFloatCheck(tag, ref DamageToolAttackerAmount);

					}

					//DamageToolAttackerParticle
					if (tag.Contains("[DamageToolAttackerParticle:") == true) {

						 TagParse.TagStringCheck(tag, ref DamageToolAttackerParticle);

					}

					//DamageToolAttackerSound
					if (tag.Contains("[DamageToolAttackerSound:") == true) {

						 TagParse.TagStringCheck(tag, ref DamageToolAttackerSound);

					}

					//PlayParticleEffectAtRemote
					if (tag.Contains("[PlayParticleEffectAtRemote:") == true) {

						 TagParse.TagBoolCheck(tag, ref PlayParticleEffectAtRemote);

					}

					//ParticleEffectId
					if (tag.Contains("[ParticleEffectId:") == true) {

						 TagParse.TagStringCheck(tag, ref ParticleEffectId);

					}

					//ParticleEffectOffset
					if (tag.Contains("[ParticleEffectOffset:") == true) {

						 TagParse.TagVector3DCheck(tag, ref ParticleEffectOffset);

					}

					//ParticleEffectScale
					if (tag.Contains("[ParticleEffectScale:") == true) {

						 TagParse.TagFloatCheck(tag, ref ParticleEffectScale);

					}

					//ParticleEffectMaxTime
					if (tag.Contains("[ParticleEffectMaxTime:") == true) {

						 TagParse.TagFloatCheck(tag, ref ParticleEffectMaxTime);

					}

					//ParticleEffectColor
					if (tag.Contains("[ParticleEffectColor:") == true) {

						 TagParse.TagVector3DCheck(tag, ref ParticleEffectColor);

					}

					//SetBooleansTrue
					if (tag.Contains("[SetBooleansTrue:") == true) {

						TagParse.TagStringListCheck(tag, ref SetBooleansTrue);

					}

					//SetBooleansFalse
					if (tag.Contains("[SetBooleansFalse:") == true) {

						TagParse.TagStringListCheck(tag, ref SetBooleansFalse);

					}

					//IncreaseCounters
					if (tag.Contains("[IncreaseCounters:") == true) {

						TagParse.TagStringListCheck(tag, ref IncreaseCounters);

					}

					//DecreaseCounters
					if (tag.Contains("[DecreaseCounters:") == true) {

						TagParse.TagStringListCheck(tag, ref DecreaseCounters);

					}

					//ResetCounters
					if (tag.Contains("[ResetCounters:") == true) {

						TagParse.TagStringListCheck(tag, ref ResetCounters);

					}

					//SetSandboxBooleansTrue
					if (tag.Contains("[SetSandboxBooleansTrue:") == true) {

						TagParse.TagStringListCheck(tag, ref SetSandboxBooleansTrue);

					}

					//SetSandboxBooleansFalse
					if (tag.Contains("[SetSandboxBooleansFalse:") == true) {

						TagParse.TagStringListCheck(tag, ref SetSandboxBooleansFalse);

					}

					//IncreaseSandboxCounters
					if (tag.Contains("[IncreaseSandboxCounters:") == true) {

						TagParse.TagStringListCheck(tag, ref IncreaseSandboxCounters);

					}

					//DecreaseSandboxCounters
					if (tag.Contains("[DecreaseSandboxCounters:") == true) {

						TagParse.TagStringListCheck(tag, ref DecreaseSandboxCounters);

					}

					//ResetSandboxCounters
					if (tag.Contains("[ResetSandboxCounters:") == true) {

						TagParse.TagStringListCheck(tag, ref ResetSandboxCounters);

					}

					//ChangeAttackerReputation
					if (tag.Contains("[ChangeAttackerReputation:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeAttackerReputation);

					}

					//ChangeAttackerReputationFaction
					if (tag.Contains("[ChangeAttackerReputationFaction:") == true) {

						TagParse.TagStringListCheck(tag, ref ChangeAttackerReputationFaction);

					}

					//ChangeAttackerReputationAmount
					if (tag.Contains("[ChangeAttackerReputationAmount:") == true) {

						TagParse.TagIntListCheck(tag, ref ChangeAttackerReputationAmount);

					}

					//ReputationChangesForAllAttackPlayerFactionMembers
					if (tag.Contains("[ReputationChangesForAllAttackPlayerFactionMembers:") == true) {

						 TagParse.TagBoolCheck(tag, ref ReputationChangesForAllAttackPlayerFactionMembers);

					}

					//ChangeTargetProfile
					if (tag.Contains("[ChangeTargetProfile:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeTargetProfile);

					}

					//NewTargetProfileId
					if (tag.Contains("[NewTargetProfileId:") == true) {

						 TagParse.TagStringCheck(tag, ref NewTargetProfileId);

					}

					//ChangeBlockNames
					if (tag.Contains("[ChangeBlockNames:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeBlockNames);

					}

					//ChangeBlockNamesFrom
					if (tag.Contains("[ChangeBlockNamesFrom:") == true) {

						TagParse.TagStringListCheck(tag, ref ChangeBlockNamesFrom);

					}

					//ChangeBlockNamesTo
					if (tag.Contains("[ChangeBlockNamesTo:") == true) {

						TagParse.TagStringListCheck(tag, ref ChangeBlockNamesTo);

					}

					//ChangeAntennaRanges
					if (tag.Contains("[ChangeAntennaRanges:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeAntennaRanges);

					}

					//AntennaNamesForRangeChange
					if (tag.Contains("[AntennaNamesForRangeChange:") == true) {

						TagParse.TagStringListCheck(tag, ref AntennaNamesForRangeChange);

					}

					//AntennaRangeChangeType
					if (tag.Contains("[AntennaRangeChangeType:") == true) {

						 TagParse.TagStringCheck(tag, ref AntennaRangeChangeType);

					}

					//AntennaRangeChangeAmount
					if (tag.Contains("[AntennaRangeChangeAmount:") == true) {

						 TagParse.TagFloatCheck(tag, ref AntennaRangeChangeAmount);

					}

					//ForceDespawn
					if (tag.Contains("[ForceDespawn:") == true) {

						 TagParse.TagBoolCheck(tag, ref ForceDespawn);

					}

					//ResetCooldownTimeOfTriggers
					if (tag.Contains("[ResetCooldownTimeOfTriggers:") == true) {

						 TagParse.TagBoolCheck(tag, ref ResetCooldownTimeOfTriggers);

					}

					//ResetTriggerCooldownNames
					if (tag.Contains("[ResetTriggerCooldownNames:") == true) {

						TagParse.TagStringListCheck(tag, ref ResetTriggerCooldownNames);

					}

					//BroadcastGenericCommand
					if (tag.Contains("[BroadcastGenericCommand:") == true) {

						 TagParse.TagBoolCheck(tag, ref BroadcastGenericCommand);

					}

					//BehaviorSpecificEventA
					if (tag.Contains("[BehaviorSpecificEventA:") == true) {

						 TagParse.TagBoolCheck(tag, ref BehaviorSpecificEventA);

					}

					//ChangeInertiaDampeners
					if (tag.Contains("[ChangeInertiaDampeners:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeInertiaDampeners);

					}

					//InertiaDampenersEnable
					if (tag.Contains("[InertiaDampenersEnable:") == true) {

						 TagParse.TagBoolCheck(tag, ref InertiaDampenersEnable);

					}

					//EnableTriggers
					if (tag.Contains("[EnableTriggers:") == true) {

						 TagParse.TagBoolCheck(tag, ref EnableTriggers);

					}

					//EnableTriggerNames
					if (tag.Contains("[EnableTriggerNames:") == true) {

						TagParse.TagStringListCheck(tag, ref EnableTriggerNames);

					}

					//DisableTriggers
					if (tag.Contains("[DisableTriggers:") == true) {

						 TagParse.TagBoolCheck(tag, ref DisableTriggers);

					}

					//DisableTriggerNames
					if (tag.Contains("[DisableTriggerNames:") == true) {

						TagParse.TagStringListCheck(tag, ref DisableTriggerNames);

					}

					//StaggerWarheadDetonation
					if (tag.Contains("[StaggerWarheadDetonation:") == true) {

						 TagParse.TagBoolCheck(tag, ref StaggerWarheadDetonation);

					}

					//ChangeRotationDirection
					if (tag.Contains("[ChangeRotationDirection:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeRotationDirection);

					}

					//RotationDirection
					if (tag.Contains("[RotationDirection:") == true) {

						 TagParse.TagDirectionEnumCheck(tag, ref RotationDirection);

					}

					//GenerateExplosion
					if (tag.Contains("[GenerateExplosion:") == true) {

						 TagParse.TagBoolCheck(tag, ref GenerateExplosion);

					}

					//ExplosionOffsetFromRemote
					if (tag.Contains("[ExplosionOffsetFromRemote:") == true) {

						 TagParse.TagVector3DCheck(tag, ref ExplosionOffsetFromRemote);

					}

					//ExplosionRange
					if (tag.Contains("[ExplosionRange:") == true) {

						 TagParse.TagIntCheck(tag, ref ExplosionRange);

					}

					//ExplosionDamage
					if (tag.Contains("[ExplosionDamage:") == true) {

						 TagParse.TagIntCheck(tag, ref ExplosionDamage);

					}

					//ExplosionIgnoresVoxels
					if (tag.Contains("[ExplosionIgnoresVoxels:") == true) {

						 TagParse.TagBoolCheck(tag, ref ExplosionIgnoresVoxels);

					}

					//GridEditable
					if (tag.Contains("[GridEditable:") == true) {

						 TagParse.TagCheckEnumCheck(tag, ref GridEditable);

					}

					//SubGridsEditable
					if (tag.Contains("[SubGridsEditable:") == true) {

						 TagParse.TagCheckEnumCheck(tag, ref SubGridsEditable);

					}

					//GridDestructible
					if (tag.Contains("[GridDestructible:") == true) {

						 TagParse.TagCheckEnumCheck(tag, ref GridDestructible);

					}

					//SubGridsDestructible
					if (tag.Contains("[SubGridsDestructible:") == true) {

						 TagParse.TagCheckEnumCheck(tag, ref SubGridsDestructible);

					}

					//RecolorGrid
					if (tag.Contains("[RecolorGrid:") == true) {

						 TagParse.TagBoolCheck(tag, ref RecolorGrid);

					}

					//RecolorSubGrids
					if (tag.Contains("[RecolorSubGrids:") == true) {

						 TagParse.TagBoolCheck(tag, ref RecolorSubGrids);

					}

					//OldBlockColors
					if (tag.Contains("[OldBlockColors:") == true) {

						TagParse.TagVector3DListCheck(tag, ref OldBlockColors);

					}

					//NewBlockColors
					if (tag.Contains("[NewBlockColors:") == true) {

						TagParse.TagVector3DListCheck(tag, ref NewBlockColors);

					}

					//NewBlockSkins
					if (tag.Contains("[NewBlockSkins:") == true) {

						TagParse.TagStringListCheck(tag, ref NewBlockSkins);

					}

					//ChangeBlockOwnership
					if (tag.Contains("[ChangeBlockOwnership:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeBlockOwnership);

					}

					//OwnershipBlockNames
					if (tag.Contains("[OwnershipBlockNames:") == true) {

						TagParse.TagStringListCheck(tag, ref OwnershipBlockNames);

					}

					//OwnershipBlockFactions
					if (tag.Contains("[OwnershipBlockFactions:") == true) {

						TagParse.TagStringListCheck(tag, ref OwnershipBlockFactions);

					}

					//ChangeBlockDamageMultipliers
					if (tag.Contains("[ChangeBlockDamageMultipliers:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeBlockDamageMultipliers);

					}

					//DamageMultiplierBlockNames
					if (tag.Contains("[DamageMultiplierBlockNames:") == true) {

						TagParse.TagStringListCheck(tag, ref DamageMultiplierBlockNames);

					}

					//DamageMultiplierValues
					if (tag.Contains("[DamageMultiplierValues:") == true) {

						TagParse.TagIntListCheck(tag, ref DamageMultiplierValues);

					}

					//RazeBlocksWithNames
					if (tag.Contains("[RazeBlocksWithNames:") == true) {

						 TagParse.TagBoolCheck(tag, ref RazeBlocksWithNames);

					}

					//RazeBlocksNames
					if (tag.Contains("[RazeBlocksNames:") == true) {

						TagParse.TagStringListCheck(tag, ref RazeBlocksNames);

					}

					//ManuallyActivateTrigger
					if (tag.Contains("[ManuallyActivateTrigger:") == true) {

						 TagParse.TagBoolCheck(tag, ref ManuallyActivateTrigger);

					}

					//ManuallyActivatedTriggerNames
					if (tag.Contains("[ManuallyActivatedTriggerNames:") == true) {

						TagParse.TagStringListCheck(tag, ref ManuallyActivatedTriggerNames);

					}

					//SendCommandWithoutAntenna
					if (tag.Contains("[SendCommandWithoutAntenna:") == true) {

						 TagParse.TagBoolCheck(tag, ref SendCommandWithoutAntenna);

					}

					//SendCommandWithoutAntennaRadius
					if (tag.Contains("[SendCommandWithoutAntennaRadius:") == true) {

						 TagParse.TagDoubleCheck(tag, ref SendCommandWithoutAntennaRadius);

					}

					//RemoveKnownPlayerArea
					if (tag.Contains("[RemoveKnownPlayerArea:") == true) {

						 TagParse.TagBoolCheck(tag, ref RemoveKnownPlayerArea);

					}

					//RemoveAllKnownPlayerAreas
					if (tag.Contains("[RemoveAllKnownPlayerAreas:") == true) {

						 TagParse.TagBoolCheck(tag, ref RemoveAllKnownPlayerAreas);

					}

					//Chance
					if (tag.Contains("[Chance:") == true) {

						 TagParse.TagIntCheck(tag, ref Chance);

					}

					//EnableBlocks
					if (tag.Contains("[EnableBlocks:") == true) {

						 TagParse.TagBoolCheck(tag, ref EnableBlocks);

					}

					//EnableBlockNames
					if (tag.Contains("[EnableBlockNames:") == true) {

						TagParse.TagStringListCheck(tag, ref EnableBlockNames);

					}

					//EnableBlockStates
					if (tag.Contains("[EnableBlockStates:") == true) {

						TagParse.TagSwitchEnumCheck(tag, ref EnableBlockStates);

					}

					//ChangeAutopilotProfile
					if (tag.Contains("[ChangeAutopilotProfile:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeAutopilotProfile);

					}

					//AutopilotProfile
					if (tag.Contains("[AutopilotProfile:") == true) {

						 TagParse.TagAutoPilotProfileModeCheck(tag, ref AutopilotProfile);

					}

					//Ramming
					if (tag.Contains("[Ramming:") == true) {

						 TagParse.TagBoolCheck(tag, ref Ramming);

					}

					//CreateRandomLightning
					if (tag.Contains("[CreateRandomLightning:") == true) {

						 TagParse.TagBoolCheck(tag, ref CreateRandomLightning);

					}

					//CreateLightningAtAttacker
					if (tag.Contains("[CreateLightningAtAttacker:") == true) {

						 TagParse.TagBoolCheck(tag, ref CreateLightningAtAttacker);

					}

					//LightningDamage
					if (tag.Contains("[LightningDamage:") == true) {

						 TagParse.TagIntCheck(tag, ref LightningDamage);

					}

					//LightningExplosionRadius
					if (tag.Contains("[LightningExplosionRadius:") == true) {

						 TagParse.TagIntCheck(tag, ref LightningExplosionRadius);

					}

					//LightningColor
					if (tag.Contains("[LightningColor:") == true) {

						 TagParse.TagVector3DCheck(tag, ref LightningColor);

					}

					//LightningMinDistance
					if (tag.Contains("[LightningMinDistance:") == true) {

						 TagParse.TagDoubleCheck(tag, ref LightningMinDistance);

					}

					//LightningMaxDistance
					if (tag.Contains("[LightningMaxDistance:") == true) {

						 TagParse.TagDoubleCheck(tag, ref LightningMaxDistance);

					}

					//CreateLightningAtTarget
					if (tag.Contains("[CreateLightningAtTarget:") == true) {

						 TagParse.TagBoolCheck(tag, ref CreateLightningAtTarget);

					}

					//SelfDestructTimerPadding
					if (tag.Contains("[SelfDestructTimerPadding:") == true) {

						 TagParse.TagIntCheck(tag, ref SelfDestructTimerPadding);

					}

					//SelfDestructTimeBetweenBlasts
					if (tag.Contains("[SelfDestructTimeBetweenBlasts:") == true) {

						 TagParse.TagIntCheck(tag, ref SelfDestructTimeBetweenBlasts);

					}

					//SetCounters
					if (tag.Contains("[SetCounters:") == true) {

						TagParse.TagStringListCheck(tag, ref SetCounters);

					}

					//SetSandboxCounters
					if (tag.Contains("[SetSandboxCounters:") == true) {

						TagParse.TagStringListCheck(tag, ref SetSandboxCounters);

					}

					//SetCountersValues
					if (tag.Contains("[SetCountersValues:") == true) {

						TagParse.TagIntListCheck(tag, ref SetCountersValues);

					}

					//SetSandboxCountersValues
					if (tag.Contains("[SetSandboxCountersValues:") == true) {

						TagParse.TagIntListCheck(tag, ref SetSandboxCountersValues);

					}

					//InheritLastAttackerFromCommand
					if (tag.Contains("[InheritLastAttackerFromCommand:") == true) {

						 TagParse.TagBoolCheck(tag, ref InheritLastAttackerFromCommand);

					}

					//ChangePlayerCredits
					if (tag.Contains("[ChangePlayerCredits:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangePlayerCredits);

					}

					//ChangePlayerCreditsAmount
					if (tag.Contains("[ChangePlayerCreditsAmount:") == true) {

						 TagParse.TagLongCheck(tag, ref ChangePlayerCreditsAmount);

					}

					//ChangeNpcFactionCredits
					if (tag.Contains("[ChangeNpcFactionCredits:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeNpcFactionCredits);

					}

					//ChangeNpcFactionCreditsAmount
					if (tag.Contains("[ChangeNpcFactionCreditsAmount:") == true) {

						 TagParse.TagLongCheck(tag, ref ChangeNpcFactionCreditsAmount);

					}

					//ChangeNpcFactionCreditsTag
					if (tag.Contains("[ChangeNpcFactionCreditsTag:") == true) {

						 TagParse.TagStringCheck(tag, ref ChangeNpcFactionCreditsTag);

					}

					//BuildProjectedBlocks
					if (tag.Contains("[BuildProjectedBlocks:") == true) {

						 TagParse.TagBoolCheck(tag, ref BuildProjectedBlocks);

					}

					//MaxProjectedBlocksToBuild
					if (tag.Contains("[MaxProjectedBlocksToBuild:") == true) {

						 TagParse.TagIntCheck(tag, ref MaxProjectedBlocksToBuild);

					}

					//ForceManualTriggerActivation
					if (tag.Contains("[ForceManualTriggerActivation:") == true) {

						 TagParse.TagBoolCheck(tag, ref ForceManualTriggerActivation);

					}

					//OverwriteAutopilotProfile
					if (tag.Contains("[OverwriteAutopilotProfile:") == true) {

						 TagParse.TagBoolCheck(tag, ref OverwriteAutopilotProfile);

					}

					//OverwriteAutopilotMode
					if (tag.Contains("[OverwriteAutopilotMode:") == true) {

						 TagParse.TagAutoPilotProfileModeCheck(tag, ref OverwriteAutopilotMode);

					}

					//OverwriteAutopilotId
					if (tag.Contains("[OverwriteAutopilotId:") == true) {

						 TagParse.TagStringCheck(tag, ref OverwriteAutopilotId);

					}

					//BroadcastCommandProfiles
					if (tag.Contains("[BroadcastCommandProfiles:") == true) {

						 TagParse.TagBoolCheck(tag, ref BroadcastCommandProfiles);

					}

					//CommandProfileIds
					if (tag.Contains("[CommandProfileIds:") == true) {

						TagParse.TagStringListCheck(tag, ref CommandProfileIds);

					}

					//AddWaypointFromCommand
					if (tag.Contains("[AddWaypointFromCommand:") == true) {

						 TagParse.TagBoolCheck(tag, ref AddWaypointFromCommand);

					}

					//RecalculateDespawnCoords
					if (tag.Contains("[RecalculateDespawnCoords:") == true) {

						 TagParse.TagBoolCheck(tag, ref RecalculateDespawnCoords);

					}

					//AddDatapadsToSeats
					if (tag.Contains("[AddDatapadsToSeats:") == true) {

						 TagParse.TagBoolCheck(tag, ref AddDatapadsToSeats);

					}

					//DatapadNamesToAdd
					if (tag.Contains("[DatapadNamesToAdd:") == true) {

						TagParse.TagStringListCheck(tag, ref DatapadNamesToAdd);

					}

					//DatapadCountToAdd
					if (tag.Contains("[DatapadCountToAdd:") == true) {

						 TagParse.TagIntCheck(tag, ref DatapadCountToAdd);

					}

					//ToggleBlocksOfType
					if (tag.Contains("[ToggleBlocksOfType:") == true) {

						 TagParse.TagBoolCheck(tag, ref ToggleBlocksOfType);

					}

					//BlockTypesToToggle
					if (tag.Contains("[BlockTypesToToggle:") == true) {

						TagParse.TagMyDefIdCheck(tag, ref BlockTypesToToggle);

					}

					//BlockTypeToggles
					if (tag.Contains("[BlockTypeToggles:") == true) {

						TagParse.TagSwitchEnumCheck(tag, ref BlockTypeToggles);

					}

					//CancelWaitingAtWaypoint
					if (tag.Contains("[CancelWaitingAtWaypoint:") == true) {

						 TagParse.TagBoolCheck(tag, ref CancelWaitingAtWaypoint);

					}

					//SwitchToNextWaypoint
					if (tag.Contains("[SwitchToNextWaypoint:") == true) {

						 TagParse.TagBoolCheck(tag, ref SwitchToNextWaypoint);

					}

					//HeavyYaw
					if (tag.Contains("[HeavyYaw:") == true) {

						 TagParse.TagBoolCheck(tag, ref HeavyYaw);

					}

					//StopAllRotation
					if (tag.Contains("[StopAllRotation:") == true) {

						 TagParse.TagBoolCheck(tag, ref StopAllRotation);

					}

					//StopAllThrust
					if (tag.Contains("[StopAllThrust:") == true) {

						 TagParse.TagBoolCheck(tag, ref StopAllThrust);

					}

					//RandomGyroRotation
					if (tag.Contains("[RandomGyroRotation:") == true) {

						 TagParse.TagBoolCheck(tag, ref RandomGyroRotation);

					}

					//RandomThrustDirection
					if (tag.Contains("[RandomThrustDirection:") == true) {

						 TagParse.TagBoolCheck(tag, ref RandomThrustDirection);

					}

					//ParentGridNameRequirement
					if (tag.Contains("ParentGridNameRequirement:") == true) {

						TagParse.TagStringCheck(tag, ref ParentGridNameRequirement);

					}

				}

			}

		}

	}

}
