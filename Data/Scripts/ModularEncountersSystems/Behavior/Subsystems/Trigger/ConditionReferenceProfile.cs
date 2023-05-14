using ModularEncountersSystems.Behavior;
using ProtoBuf;
using ModularEncountersSystems.Helpers;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game.ModAPI;
using VRageMath;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.API;
using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Entities;

namespace ModularEncountersSystems.Behavior.Subsystems.Trigger {

	public class ConditionReferenceProfile
	{


		public bool UseConditions;
		public bool MatchAnyCondition;
		public string ProfileSubtypeId;

		public bool CheckAllLoadedModIDs;
		public List<long> AllModIDsToCheck;
		public bool CheckAnyLoadedModIDs;
		public List<long> AnyModIDsToCheck;

		public bool CheckTrueBooleans;
		public List<string> TrueBooleans;
		public bool AllowAnyTrueBoolean;
		public bool CheckCustomCounters;
		public List<string> CustomCounters;
		public List<int> CustomCountersTargets;
		public bool AllowAnyValidCounter;

		public bool CheckGridSpeed;
		public float MinGridSpeed;
		public float MaxGridSpeed;

		public bool CheckMESBlacklistedSpawnGroups;

		public List<string> SpawnGroupBlacklistContainsAll;
		public List<string> SpawnGroupBlacklistContainsAny;

		public bool UseRequiredFunctionalBlocks;
		public List<string> RequiredAllFunctionalBlockNames;
		public List<string> RequiredAnyFunctionalBlockNames;
		public List<string> RequiredNoneFunctionalBlockNames;

		public bool UseAccumulatedDamageWatcher;
		public float MinAccumulatedDamage;
		public float MaxAccumulatedDamage;

		public bool CheckTrueSandboxBooleans;
		public List<string> TrueSandboxBooleans;
		public bool AllowAnyTrueSandboxBoolean;
		public bool CheckCustomSandboxCounters;
		public List<string> CustomSandboxCounters;
		public List<int> CustomSandboxCountersTargets;
		public bool AllowAnyValidSandboxCounter;

		public bool CheckTargetAltitudeDifference;
		public double MinTargetAltitudeDifference;
		public double MaxTargetAltitudeDifference;

		public bool CheckTargetDistance;
		public double MinTargetDistance;
		public double MaxTargetDistance;

		public bool CheckTargetAngleFromForward;
		public double MinTargetAngle;
		public double MaxTargetAngle;

		public bool CheckIfTargetIsChasing;
		public double MinTargetChaseAngle;
		public double MaxTargetChaseAngle;

		public List<CounterCompareEnum> CounterCompareTypes;
		public List<CounterCompareEnum> SandboxCounterCompareTypes;

		public bool CheckIfGridNameMatches;
		public bool AllowPartialGridNameMatches;
		public List<string> GridNamesToCheck;

		public bool UnderwaterCheck;
		public bool IsUnderwater;
		public double MinDistanceUnderwater;
		public double MaxDistanceUnderwater;
		public bool TargetUnderwaterCheck;
		public bool TargetIsUnderwater;
		public double MinTargetDistanceUnderwater;
		public double MaxTargetDistanceUnderwater;

		public bool BehaviorSubclassCheck;
		public List<BehaviorSubclass> BehaviorSubclass;

		public bool BehaviorModeCheck;
		public List<BehaviorMode> CurrentBehaviorMode;

		public bool GravityCheck;
		public float MinGravity;
		public float MaxGravity;

		public bool AltitudeCheck;
		public double MinAltitude;
		public double MaxAltitude;

		public bool CheckHorizonAngle;
		public double MinHorizonAngle;
		public double MaxHorizonAngle;

		public bool CheckIfDamagerIsPlayer;
		public bool CheckIfDamagerIsNpc;
		public bool CheckIfTargetIsPlayerOwned;
		public bool CheckIfTargetIsNpcOwned;

		public bool IsTargetPlayer;
		public bool IsTargetGrid;

		public bool IsAttackerHostile;
		public bool IsAttackerNeutral;
		public bool IsAttackerFriendly;

		public bool CheckCommandGridValue;
		public float CommandGridValue;
		public CounterCompareEnum CheckCommandGridValueCompare;
		public bool CompareCommandGridValue;
		public CounterCompareEnum CompareCommandGridValueMode;
		public float CompareCommandGridValueSelfMultiplier;

		public bool CommandGravityCheck;
		public bool CommandGravityMatches;

		public bool UseFailCondition;

		public bool CheckForBlocksOfType;
		public List<string> BlocksOfType = new List<string>();

		public bool CheckForSpawnConditions;
		public List<string> RequiredSpawnConditions;

		public bool CheckForPlanetaryLane;
		public bool PlanetaryLanePassValue;

		public bool CheckPlayerReputation;
		public List<string> CheckReputationwithFaction;
		public List<int> MinPlayerReputation;
		public List<int> MaxPlayerReputation;
		public double MaxPlayerReputationDistanceCheck;
		public bool AllPlayersReputationMustMatch;

		public bool PlayerIdentityMatches;

		public bool CheckPlayerIdentitySandboxList;
		public bool PlayerIdentityInSandboxList;
		public string PlayerIdentitySandboxListId;

		public bool CheckSufficientUpwardThrust;

		public Dictionary<string, Action<string, object>> EditorReference;

		public ConditionReferenceProfile()
		{

			UseConditions = true;
			MatchAnyCondition = false;

			CheckAllLoadedModIDs = false;
			AllModIDsToCheck = new List<long>();

			CheckAnyLoadedModIDs = false;
			AnyModIDsToCheck = new List<long>();

			CheckTrueBooleans = false;
			TrueBooleans = new List<string>();
			AllowAnyTrueBoolean = false;

			CheckCustomCounters = false;
			CustomCounters = new List<string>();
			CustomCountersTargets = new List<int>();
			AllowAnyValidCounter = false;

			CheckTrueSandboxBooleans = false;
			TrueSandboxBooleans = new List<string>();
			AllowAnyTrueSandboxBoolean = false;

			CheckCustomSandboxCounters = false;
			CustomSandboxCounters = new List<string>();
			CustomSandboxCountersTargets = new List<int>();
			AllowAnyValidSandboxCounter = false;

			CheckGridSpeed = false;
			MinGridSpeed = -1;
			MaxGridSpeed = -1;

			CheckMESBlacklistedSpawnGroups = false;
			SpawnGroupBlacklistContainsAll = new List<string>();
			SpawnGroupBlacklistContainsAny = new List<string>();

			UseRequiredFunctionalBlocks = false;
			RequiredAllFunctionalBlockNames = new List<string>();
			RequiredAnyFunctionalBlockNames = new List<string>();
			RequiredNoneFunctionalBlockNames = new List<string>();

			UseAccumulatedDamageWatcher = false;
			MinAccumulatedDamage = -1;
			MaxAccumulatedDamage = -1;

			CheckTargetAltitudeDifference = false;
			MinTargetAltitudeDifference = 0;
			MaxTargetAltitudeDifference = 0;

			CheckTargetDistance = false;
			MinTargetDistance = -1;
			MaxTargetDistance = -1;

			CheckTargetAngleFromForward = false;
			MinTargetAngle = -1;
			MaxTargetAngle = -1;

			CheckIfTargetIsChasing = false;
			MinTargetChaseAngle = -1;
			MaxTargetChaseAngle = -1;

			CounterCompareTypes = new List<CounterCompareEnum>();
			SandboxCounterCompareTypes = new List<CounterCompareEnum>();

			CheckIfGridNameMatches = false;
			AllowPartialGridNameMatches = false;
			GridNamesToCheck = new List<string>();

			UnderwaterCheck = false;
			IsUnderwater = false;
			TargetUnderwaterCheck = false;
			TargetIsUnderwater = false;
			MinDistanceUnderwater = -1;
			MaxDistanceUnderwater = -1;
			MinTargetDistanceUnderwater = -1;
			MaxTargetDistanceUnderwater = -1;

			GravityCheck = false;
			MinGravity = -1000;
			MaxGravity = -1000;

			AltitudeCheck = false;
			MinAltitude = -1;
			MaxAltitude = -1;

			CheckHorizonAngle = false;
			MinHorizonAngle = -1;
			MaxHorizonAngle = -1;

			CheckIfDamagerIsPlayer = false;
			CheckIfDamagerIsNpc = false;

			CheckIfTargetIsPlayerOwned = false;
			CheckIfTargetIsNpcOwned = false;

			IsTargetPlayer = false;
			IsTargetGrid = false;

			IsAttackerHostile = false;
			IsAttackerNeutral = false;
			IsAttackerFriendly = false;

			BehaviorSubclassCheck = false;
			BehaviorSubclass = new List<BehaviorSubclass>();

			BehaviorModeCheck = false;
			CurrentBehaviorMode = new List<BehaviorMode>();

			CheckCommandGridValue = false;
			CommandGridValue = 0;
			CheckCommandGridValueCompare = CounterCompareEnum.GreaterOrEqual;

			CompareCommandGridValue = false;
			CompareCommandGridValueMode = CounterCompareEnum.GreaterOrEqual;
			CompareCommandGridValueSelfMultiplier = 1;

			CommandGravityCheck = false;
			CommandGravityMatches = false;

			UseFailCondition = false;

			CheckForSpawnConditions = false;
			RequiredSpawnConditions = new List<string>();

			CheckForPlanetaryLane = false;
			PlanetaryLanePassValue = true;


			CheckPlayerReputation = false;
			CheckReputationwithFaction = new List<string>();
			MinPlayerReputation = new List<int>();
			MaxPlayerReputation = new List<int>();
			MaxPlayerReputationDistanceCheck = 2500;
			AllPlayersReputationMustMatch = true;

			PlayerIdentityMatches = false;
			CheckPlayerIdentitySandboxList = false;
			PlayerIdentityInSandboxList = false;
			PlayerIdentitySandboxListId = "";

			CheckSufficientUpwardThrust = false;

			ProfileSubtypeId = "";

			EditorReference = new Dictionary<string, Action<string, object>> {

				{"UseConditions", (s, o) => TagParse.TagBoolCheck(s, ref UseConditions) },
				{"MatchAnyCondition", (s, o) => TagParse.TagBoolCheck(s, ref MatchAnyCondition) },
				{"CheckAllLoadedModIDs", (s, o) => TagParse.TagBoolCheck(s, ref CheckAllLoadedModIDs) },
				{"AllModIDsToCheck", (s, o) => TagParse.TagLongCheck(s, ref AllModIDsToCheck) },
				{"CheckAnyLoadedModIDs", (s, o) => TagParse.TagBoolCheck(s, ref CheckAnyLoadedModIDs) },
				{"AnyModIDsToCheck", (s, o) => TagParse.TagLongCheck(s, ref AnyModIDsToCheck) },
				{"CheckTrueBooleans", (s, o) => TagParse.TagBoolCheck(s, ref CheckTrueBooleans) },
				{"TrueBooleans", (s, o) => TagParse.TagStringListCheck(s, ref TrueBooleans) },
				{"AllowAnyTrueBoolean", (s, o) => TagParse.TagBoolCheck(s, ref AllowAnyTrueBoolean) },
				{"CheckCustomCounters", (s, o) => TagParse.TagBoolCheck(s, ref CheckCustomCounters) },
				{"CustomCounters", (s, o) => TagParse.TagStringListCheck(s, ref CustomCounters) },
				{"CustomCountersTargets", (s, o) => TagParse.TagIntListCheck(s, ref CustomCountersTargets) },
				{"AllowAnyValidCounter", (s, o) => TagParse.TagBoolCheck(s, ref AllowAnyValidCounter) },
				{"CheckGridSpeed", (s, o) => TagParse.TagBoolCheck(s, ref CheckGridSpeed) },
				{"MinGridSpeed", (s, o) => TagParse.TagFloatCheck(s, ref MinGridSpeed) },
				{"MaxGridSpeed", (s, o) => TagParse.TagFloatCheck(s, ref MaxGridSpeed) },
				{"CheckMESBlacklistedSpawnGroups", (s, o) => TagParse.TagBoolCheck(s, ref CheckMESBlacklistedSpawnGroups) },
				{"SpawnGroupBlacklistContainsAll", (s, o) => TagParse.TagStringListCheck(s, ref SpawnGroupBlacklistContainsAll) },
				{"SpawnGroupBlacklistContainsAny", (s, o) => TagParse.TagStringListCheck(s, ref SpawnGroupBlacklistContainsAny) },
				{"UseRequiredFunctionalBlocks", (s, o) => TagParse.TagBoolCheck(s, ref UseRequiredFunctionalBlocks) },
				{"RequiredAllFunctionalBlockNames", (s, o) => TagParse.TagStringListCheck(s, ref RequiredAllFunctionalBlockNames) },
				{"RequiredAnyFunctionalBlockNames", (s, o) => TagParse.TagStringListCheck(s, ref RequiredAnyFunctionalBlockNames) },
				{"RequiredNoneFunctionalBlockNames", (s, o) => TagParse.TagStringListCheck(s, ref RequiredNoneFunctionalBlockNames) },
				{"UseAccumulatedDamageWatcher", (s, o) => TagParse.TagBoolCheck(s, ref UseAccumulatedDamageWatcher) },
				{"MinAccumulatedDamage", (s, o) => TagParse.TagFloatCheck(s, ref MinAccumulatedDamage) },
				{"MaxAccumulatedDamage", (s, o) => TagParse.TagFloatCheck(s, ref MaxAccumulatedDamage) },
				{"CheckTrueSandboxBooleans", (s, o) => TagParse.TagBoolCheck(s, ref CheckTrueSandboxBooleans) },
				{"TrueSandboxBooleans", (s, o) => TagParse.TagStringListCheck(s, ref TrueSandboxBooleans) },
				{"AllowAnyTrueSandboxBoolean", (s, o) => TagParse.TagBoolCheck(s, ref AllowAnyTrueSandboxBoolean) },
				{"CheckCustomSandboxCounters", (s, o) => TagParse.TagBoolCheck(s, ref CheckCustomSandboxCounters) },
				{"CustomSandboxCounters", (s, o) => TagParse.TagStringListCheck(s, ref CustomSandboxCounters) },
				{"CustomSandboxCountersTargets", (s, o) => TagParse.TagIntListCheck(s, ref CustomSandboxCountersTargets) },
				{"AllowAnyValidSandboxCounter", (s, o) => TagParse.TagBoolCheck(s, ref AllowAnyValidSandboxCounter) },
				{"CheckTargetAltitudeDifference", (s, o) => TagParse.TagBoolCheck(s, ref CheckTargetAltitudeDifference) },
				{"MinTargetAltitudeDifference", (s, o) => TagParse.TagDoubleCheck(s, ref MinTargetAltitudeDifference) },
				{"MaxTargetAltitudeDifference", (s, o) => TagParse.TagDoubleCheck(s, ref MaxTargetAltitudeDifference) },
				{"CheckTargetDistance", (s, o) => TagParse.TagBoolCheck(s, ref CheckTargetDistance) },
				{"MinTargetDistance", (s, o) => TagParse.TagDoubleCheck(s, ref MinTargetDistance) },
				{"MaxTargetDistance", (s, o) => TagParse.TagDoubleCheck(s, ref MaxTargetDistance) },
				{"CheckTargetAngleFromForward", (s, o) => TagParse.TagBoolCheck(s, ref CheckTargetAngleFromForward) },
				{"MinTargetAngle", (s, o) => TagParse.TagDoubleCheck(s, ref MinTargetAngle) },
				{"MaxTargetAngle", (s, o) => TagParse.TagDoubleCheck(s, ref MaxTargetAngle) },
				{"CheckIfTargetIsChasing", (s, o) => TagParse.TagBoolCheck(s, ref CheckIfTargetIsChasing) },
				{"MinTargetChaseAngle", (s, o) => TagParse.TagDoubleCheck(s, ref MinTargetChaseAngle) },
				{"MaxTargetChaseAngle", (s, o) => TagParse.TagDoubleCheck(s, ref MaxTargetChaseAngle) },
				{"CounterCompareTypes", (s, o) => TagParse.TagCounterCompareEnumCheck(s, ref CounterCompareTypes) },
				{"SandboxCounterCompareTypes", (s, o) => TagParse.TagCounterCompareEnumCheck(s, ref SandboxCounterCompareTypes) },
				{"CheckIfGridNameMatches", (s, o) => TagParse.TagBoolCheck(s, ref CheckIfGridNameMatches) },
				{"AllowPartialGridNameMatches", (s, o) => TagParse.TagBoolCheck(s, ref AllowPartialGridNameMatches) },
				{"GridNamesToCheck", (s, o) => TagParse.TagStringListCheck(s, ref GridNamesToCheck) },
				{"UnderwaterCheck", (s, o) => TagParse.TagBoolCheck(s, ref UnderwaterCheck) },
				{"IsUnderwater", (s, o) => TagParse.TagBoolCheck(s, ref IsUnderwater) },
				{"MinDistanceUnderwater", (s, o) => TagParse.TagDoubleCheck(s, ref MinDistanceUnderwater) },
				{"MaxDistanceUnderwater", (s, o) => TagParse.TagDoubleCheck(s, ref MaxDistanceUnderwater) },
				{"TargetUnderwaterCheck", (s, o) => TagParse.TagBoolCheck(s, ref TargetUnderwaterCheck) },
				{"TargetIsUnderwater", (s, o) => TagParse.TagBoolCheck(s, ref TargetIsUnderwater) },
				{"MinTargetDistanceUnderwater", (s, o) => TagParse.TagDoubleCheck(s, ref MinTargetDistanceUnderwater) },
				{"MaxTargetDistanceUnderwater", (s, o) => TagParse.TagDoubleCheck(s, ref MaxTargetDistanceUnderwater) },
				{"BehaviorSubclassCheck", (s, o) => TagParse.TagBoolCheck(s, ref BehaviorSubclassCheck) },
				{"BehaviorSubclass", (s, o) => TagParse.TagBehaviorSubclassEnumCheck(s, ref BehaviorSubclass) },
				{"BehaviorModeCheck", (s, o) => TagParse.TagBoolCheck(s, ref BehaviorModeCheck) },
				{"CurrentBehaviorMode", (s, o) => TagParse.TagBehaviorModeEnumCheck(s, ref CurrentBehaviorMode) },
				{"GravityCheck", (s, o) => TagParse.TagBoolCheck(s, ref GravityCheck) },
				{"MinGravity", (s, o) => TagParse.TagFloatCheck(s, ref MinGravity) },
				{"MaxGravity", (s, o) => TagParse.TagFloatCheck(s, ref MaxGravity) },
				{"AltitudeCheck", (s, o) => TagParse.TagBoolCheck(s, ref AltitudeCheck) },
				{"MinAltitude", (s, o) => TagParse.TagDoubleCheck(s, ref MinAltitude) },
				{"MaxAltitude", (s, o) => TagParse.TagDoubleCheck(s, ref MaxAltitude) },
				{"CheckHorizonAngle", (s, o) => TagParse.TagBoolCheck(s, ref CheckHorizonAngle) },
				{"MinHorizonAngle", (s, o) => TagParse.TagDoubleCheck(s, ref MinHorizonAngle) },
				{"MaxHorizonAngle", (s, o) => TagParse.TagDoubleCheck(s, ref MaxHorizonAngle) },
				{"CheckIfDamagerIsPlayer", (s, o) => TagParse.TagBoolCheck(s, ref CheckIfDamagerIsPlayer) },
				{"CheckIfDamagerIsNpc", (s, o) => TagParse.TagBoolCheck(s, ref CheckIfDamagerIsNpc) },
				{"CheckIfTargetIsPlayerOwned", (s, o) => TagParse.TagBoolCheck(s, ref CheckIfTargetIsPlayerOwned) },
				{"CheckIfTargetIsNpcOwned", (s, o) => TagParse.TagBoolCheck(s, ref CheckIfTargetIsNpcOwned) },
				{"IsTargetGrid", (s, o) => TagParse.TagBoolCheck(s, ref IsTargetGrid) },
				{"IsTargetPlayer", (s, o) => TagParse.TagBoolCheck(s, ref IsTargetPlayer) },
				{"IsAttackerHostile", (s, o) => TagParse.TagBoolCheck(s, ref IsAttackerHostile) }, //
				{"IsAttackerNeutral", (s, o) => TagParse.TagBoolCheck(s, ref IsAttackerNeutral) },
				{"IsAttackerFriendly", (s, o) => TagParse.TagBoolCheck(s, ref IsAttackerFriendly) },
				{"CheckCommandGridValue", (s, o) => TagParse.TagBoolCheck(s, ref CheckCommandGridValue) },
				{"CommandGridValue", (s, o) => TagParse.TagFloatCheck(s, ref CommandGridValue) },
				{"CheckCommandGridValueCompare", (s, o) => TagParse.TagCounterCompareEnumCheck(s, ref CheckCommandGridValueCompare) },
				{"CompareCommandGridValue", (s, o) => TagParse.TagBoolCheck(s, ref CompareCommandGridValue) },
				{"CompareCommandGridValueMode", (s, o) => TagParse.TagCounterCompareEnumCheck(s, ref CompareCommandGridValueMode) },
				{"CompareCommandGridValueSelfMultiplier", (s, o) => TagParse.TagFloatCheck(s, ref CompareCommandGridValueSelfMultiplier) },
				{"CommandGravityCheck", (s, o) => TagParse.TagBoolCheck(s, ref CommandGravityCheck) },
				{"CommandGravityMatches", (s, o) => TagParse.TagBoolCheck(s, ref CommandGravityMatches) },
				{"UseFailCondition", (s, o) => TagParse.TagBoolCheck(s, ref UseFailCondition) },
				{"CheckForBlocksOfType", (s, o) => TagParse.TagBoolCheck(s, ref CheckForBlocksOfType) },
				{"BlocksOfType", (s, o) => TagParse.TagStringListCheck(s, ref BlocksOfType) },
				{"CheckForSpawnConditions", (s, o) => TagParse.TagBoolCheck(s, ref CheckForSpawnConditions) },
				{"RequiredSpawnConditions", (s, o) => TagParse.TagStringListCheck(s, ref RequiredSpawnConditions) },
				{"CheckForPlanetaryLane", (s, o) => TagParse.TagBoolCheck(s, ref CheckForPlanetaryLane) },//CheckForPlanetaryLane
				{"PlanetaryLanePassValue", (s, o) => TagParse.TagBoolCheck(s, ref PlanetaryLanePassValue) },


				{"CheckPlayerReputation", (s, o) => TagParse.TagBoolCheck(s, ref CheckPlayerReputation) },
				{"CheckReputationwithFaction", (s, o) => TagParse.TagStringListCheck(s, ref CheckReputationwithFaction) },
				{"MinPlayerReputation", (s, o) => TagParse.TagIntListCheck(s, ref MinPlayerReputation) },
				{"MaxPlayerReputation", (s, o) => TagParse.TagIntListCheck(s, ref MaxPlayerReputation) },
				{"MaxPlayerReputationDistanceCheck", (s, o) => TagParse.TagDoubleCheck(s, ref MaxPlayerReputationDistanceCheck) },
				{"AllPlayersReputationMustMatch", (s, o) => TagParse.TagBoolCheck(s, ref AllPlayersReputationMustMatch) },

				{"PlayerIdentityMatches", (s, o) => TagParse.TagBoolCheck(s, ref PlayerIdentityMatches) },//PlayerIdentityMatches
				{"CheckPlayerIdentitySandboxList", (s, o) => TagParse.TagBoolCheck(s, ref CheckPlayerIdentitySandboxList) },
				{"PlayerIdentityInSandboxList", (s, o) => TagParse.TagBoolCheck(s, ref PlayerIdentityInSandboxList) },
				{"PlayerIdentitySandboxListId", (s, o) => TagParse.TagStringCheck(s, ref PlayerIdentitySandboxListId) },

				{"CheckSufficientUpwardThrust", (s, o) => TagParse.TagBoolCheck(s, ref CheckSufficientUpwardThrust) },//CheckSufficientUpwardThrust
				
		};

		}


		public void EditValue(string receivedValue)
		{

			var processedTag = TagParse.ProcessTag(receivedValue);

			if (processedTag.Length < 2)
				return;

			Action<string, object> referenceMethod = null;

			if (!EditorReference.TryGetValue(processedTag[0], out referenceMethod))
				//TODO: Notes About Value Not Found
				return;

			referenceMethod?.Invoke(receivedValue, null);

		}

		public void InitTags(string customData)
		{

			if (string.IsNullOrWhiteSpace(customData) == false)
			{

				var descSplit = customData.Split('\n');

				foreach (var tag in descSplit)
				{

					EditValue(tag);

				}

			}

		}

	}

}


