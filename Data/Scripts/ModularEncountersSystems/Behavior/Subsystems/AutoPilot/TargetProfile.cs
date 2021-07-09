using System;
using System.Collections.Generic;
using ProtoBuf;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Entities;

namespace ModularEncountersSystems.Behavior.Subsystems.AutoPilot {

	[ProtoContract]
	public class TargetProfile {

		[ProtoMember(1)]
		public bool UseCustomTargeting;

		[ProtoMember(2)]
		public int TimeUntilTargetAcquisition;

		[ProtoMember(3)]
		public bool UseTargetRefresh;

		[ProtoMember(4)]
		public int TimeUntilNextRefresh;

		[ProtoMember(5)]
		public bool UseTargetLastKnownPosition;

		[ProtoMember(6)]
		public int TimeUntilNextEvaluation;

		[ProtoMember(7)]
		public TargetTypeEnum Target;

		[ProtoMember(8)]
		public List<BlockTypeEnum> BlockTargets;

		[ProtoMember(9)]
		public TargetSortEnum GetTargetBy;

		[ProtoMember(10)]
		public double MaxDistance;

		[ProtoMember(11)]
		public List<TargetFilterEnum> MatchAllFilters;

		[ProtoMember(12)]
		public List<TargetFilterEnum> MatchAnyFilters;

		[ProtoMember(13)]
		public List<TargetFilterEnum> MatchNoneFilters;

		[ProtoMember(14)]
		public OwnerTypeEnum Owners;

		[ProtoMember(15)]
		public RelationTypeEnum Relations;

		[ProtoMember(16)]
		public List<string> FactionTargets;

		[ProtoMember(17)]
		public bool OnlyGetFromEntityOwner;

		[ProtoMember(18)]
		public bool GetFromMinorityGridOwners;

		[ProtoMember(19)]
		public bool PrioritizeSpecifiedFactions;

		[ProtoMember(20)]
		public CheckEnum IsStatic;

		[ProtoMember(21)]
		public double MinAltitude;

		[ProtoMember(22)]
		public double MaxAltitude;

		[ProtoMember(23)]
		public double NonBroadcastVisualRange;

		[ProtoMember(24)]
		public double MinGravity;

		[ProtoMember(25)]
		public double MaxGravity;

		[ProtoMember(26)]
		public double MinSpeed;

		[ProtoMember(27)]
		public double MaxSpeed;

		[ProtoMember(28)]
		public float MinTargetValue;

		[ProtoMember(29)]
		public float MaxTargetValue;

		[ProtoMember(30)]
		public string ProfileSubtypeId;

		[ProtoMember(31)]
		public bool PrioritizePlayerControlled;

		[ProtoMember(32)]
		public List<string> Names;

		[ProtoMember(33)]
		public bool UsePartialNameMatching;

		[ProtoMember(34)]
		public float MinMovementScore;

		[ProtoMember(35)]
		public float MaxMovementScore;

		[ProtoMember(36)]
		public double MaxLineOfSight;

		[ProtoMember(37)]
		public double MaxMovementDetectableDistance;

		[ProtoMember(38)]
		public bool BroadcastOnlyAntenna;

		[ProtoMember(39)]
		public double MinUnderWaterDepth;

		[ProtoMember(40)]
		public double MaxUnderWaterDepth;

		[ProtoMember(41)]
		public string PlayerKnownLocationFactionOverride;

		[ProtoIgnore]
		public bool BuiltUniqueFilterList;

		[ProtoIgnore]
		public List<TargetFilterEnum> AllUniqueFilters;
		public TargetProfile() {

			UseCustomTargeting = false;

			Target = TargetTypeEnum.None;
			BlockTargets = new List<BlockTypeEnum>();

			TimeUntilTargetAcquisition = 5;
			UseTargetRefresh = false;
			TimeUntilNextRefresh = 60;
			TimeUntilNextEvaluation = 2;

			MaxDistance = 12000;

			MatchAllFilters = new List<TargetFilterEnum>();
			MatchAnyFilters = new List<TargetFilterEnum>();
			MatchNoneFilters = new List<TargetFilterEnum>();
			GetTargetBy = TargetSortEnum.ClosestDistance;

			Owners = OwnerTypeEnum.None;
			Relations = RelationTypeEnum.None;
			FactionTargets = new List<string>();

			OnlyGetFromEntityOwner = false;
			GetFromMinorityGridOwners = false;
			PrioritizeSpecifiedFactions = false;

			IsStatic = CheckEnum.Ignore;

			MinAltitude = -100000;
			MaxAltitude = 100000;

			NonBroadcastVisualRange = 1500;

			MinGravity = 0;
			MaxGravity = 1.1;

			MinSpeed = 0;
			MaxSpeed = 110;

			MinTargetValue = 0;
			MaxTargetValue = 1;

			MinMovementScore = -1;
			MaxMovementScore = -1;

			MaxLineOfSight = -1;

			MaxMovementDetectableDistance = -1;

			PrioritizePlayerControlled = false;

			BroadcastOnlyAntenna = false;

			Names = new List<string>();
			UsePartialNameMatching = false;

			MinUnderWaterDepth = -1;
			MaxUnderWaterDepth = -1;

			PlayerKnownLocationFactionOverride = "";

			ProfileSubtypeId = "";
			BuiltUniqueFilterList = false;
			AllUniqueFilters = new List<TargetFilterEnum>();

		}

		public void InitTags(string customData) {

			if (string.IsNullOrWhiteSpace(customData) == false) {

				var descSplit = customData.Split('\n');

				foreach (var tag in descSplit) {

					//UseCustomTargeting
					if (tag.Contains("[UseCustomTargeting:") == true) {

						TagParse.TagBoolCheck(tag, ref UseCustomTargeting);

					}

					//TimeUntilTargetAcquisition
					if (tag.Contains("[TimeUntilTargetAcquisition:") == true) {

						TagParse.TagIntCheck(tag, ref TimeUntilTargetAcquisition);

					}

					//UseTargetRefresh
					if (tag.Contains("[UseTargetRefresh:") == true) {

						TagParse.TagBoolCheck(tag, ref UseTargetRefresh);

					}

					//TimeUntilNextRefresh
					if (tag.Contains("[TimeUntilNextRefresh:") == true) {

						TagParse.TagIntCheck(tag, ref TimeUntilNextRefresh);

					}

					//UseTargetLastKnownPosition
					if (tag.Contains("[UseTargetLastKnownPosition:") == true) {

						TagParse.TagBoolCheck(tag, ref UseTargetLastKnownPosition);

					}

					//TimeUntilNextEvaluation
					if (tag.Contains("[TimeUntilNextEvaluation:") == true) {

						TagParse.TagIntCheck(tag, ref TimeUntilNextEvaluation);

					}

					//Target
					if (tag.Contains("[Target:") == true) {

						TagParse.TagTargetTypeEnumCheck(tag, ref Target);

					}

					//BlockTargets
					if (tag.Contains("[BlockTargets:") == true) {

						TagParse.TagBlockTargetTypesCheck(tag, ref BlockTargets);

					}

					//GetTargetBy
					if (tag.Contains("[GetTargetBy:") == true) {

						TagParse.TagTargetSortEnumCheck(tag, ref GetTargetBy);

					}

					//MaxDistance
					if (tag.Contains("[MaxDistance:") == true) {

						TagParse.TagDoubleCheck(tag, ref MaxDistance);

					}

					//MatchAllFilters
					if (tag.Contains("[MatchAllFilters:") == true) {

						TagParse.TagTargetFilterEnumCheck(tag, ref MatchAllFilters);

					}

					//MatchAnyFilters
					if (tag.Contains("[MatchAnyFilters:") == true) {

						TagParse.TagTargetFilterEnumCheck(tag, ref MatchAnyFilters);

					}

					//MatchNoneFilters
					if (tag.Contains("[MatchNoneFilters:") == true) {

						TagParse.TagTargetFilterEnumCheck(tag, ref MatchNoneFilters);

					}

					//Owners
					if (tag.Contains("[Owners:") == true) {

						TagParse.TagTargetOwnerEnumCheck(tag, ref Owners);

					}

					//Relations
					if (tag.Contains("[Relations:") == true) {

						TagParse.TagTargetRelationEnumCheck(tag, ref Relations);

					}

					//FactionTargets
					if (tag.Contains("[FactionTargets:") == true) {

						TagParse.TagStringListCheck(tag, ref FactionTargets);

					}

					//OnlyGetFromEntityOwner
					if (tag.Contains("[OnlyGetFromEntityOwner:") == true) {

						TagParse.TagBoolCheck(tag, ref OnlyGetFromEntityOwner);

					}

					//GetFromMinorityGridOwners
					if (tag.Contains("[GetFromMinorityGridOwners:") == true) {

						TagParse.TagBoolCheck(tag, ref GetFromMinorityGridOwners);

					}

					//PrioritizeSpecifiedFactions
					if (tag.Contains("[PrioritizeSpecifiedFactions:") == true) {

						TagParse.TagBoolCheck(tag, ref PrioritizeSpecifiedFactions);

					}

					//IsStatic
					if (tag.Contains("[IsStatic:") == true) {

						TagParse.TagCheckEnumCheck(tag, ref IsStatic);

					}

					//MinAltitude
					if (tag.Contains("[MinAltitude:") == true) {

						TagParse.TagDoubleCheck(tag, ref MinAltitude);

					}

					//MaxAltitude
					if (tag.Contains("[MaxAltitude:") == true) {

						TagParse.TagDoubleCheck(tag, ref MaxAltitude);

					}

					//NonBroadcastVisualRange
					if (tag.Contains("[NonBroadcastVisualRange:") == true) {

						TagParse.TagDoubleCheck(tag, ref NonBroadcastVisualRange);

					}

					//MinGravity
					if (tag.Contains("[MinGravity:") == true) {

						TagParse.TagDoubleCheck(tag, ref MinGravity);

					}

					//MaxGravity
					if (tag.Contains("[MaxGravity:") == true) {

						TagParse.TagDoubleCheck(tag, ref MaxGravity);

					}

					//MinSpeed
					if (tag.Contains("[MinSpeed:") == true) {

						TagParse.TagDoubleCheck(tag, ref MinSpeed);

					}

					//MaxSpeed
					if (tag.Contains("[MaxSpeed:") == true) {

						TagParse.TagDoubleCheck(tag, ref MaxSpeed);

					}

					//MinTargetValue
					if (tag.Contains("[MinTargetValue:") == true) {

						TagParse.TagFloatCheck(tag, ref MinTargetValue);

					}

					//MaxTargetValue
					if (tag.Contains("[MaxTargetValue:") == true) {

						TagParse.TagFloatCheck(tag, ref MaxTargetValue);

					}

					//MinMovementScore
					if (tag.Contains("[MinMovementScore:") == true) {

						TagParse.TagFloatCheck(tag, ref MinMovementScore);

					}

					//MaxMovementScore
					if (tag.Contains("[MaxMovementScore:") == true) {

						TagParse.TagFloatCheck(tag, ref MaxMovementScore);

					}

					//PrioritizePlayerControlled
					if (tag.Contains("[PrioritizePlayerControlled:") == true) {

						TagParse.TagBoolCheck(tag, ref PrioritizePlayerControlled);

					}

					//Names
					if (tag.Contains("[Names:") == true) {

						TagParse.TagStringListCheck(tag, ref Names);

					}

					//UsePartialNameMatching
					if (tag.Contains("[UsePartialNameMatching:") == true) {

						TagParse.TagBoolCheck(tag, ref UsePartialNameMatching);

					}

					//MaxLineOfSight
					if (tag.Contains("[MaxLineOfSight:") == true) {

						TagParse.TagDoubleCheck(tag, ref MaxLineOfSight);

					}

					//MaxMovementDetectableDistance
					if (tag.Contains("[MaxMovementDetectableDistance:") == true) {

						TagParse.TagDoubleCheck(tag, ref MaxMovementDetectableDistance);

					}

					//BroadcastOnlyAntenna
					if (tag.Contains("[BroadcastOnlyAntenna:") == true) {

						TagParse.TagBoolCheck(tag, ref BroadcastOnlyAntenna);

					}

					//MinUnderWaterDepth
					if (tag.Contains("[MinUnderWaterDepth:") == true) {

						TagParse.TagDoubleCheck(tag, ref MinUnderWaterDepth);

					}

					//MaxUnderWaterDepth
					if (tag.Contains("[MaxUnderWaterDepth:") == true) {

						TagParse.TagDoubleCheck(tag, ref MaxUnderWaterDepth);

					}

					//PlayerKnownLocationFactionOverride
					if (tag.Contains("[PlayerKnownLocationFactionOverride:") == true) {

						TagParse.TagStringCheck(tag, ref PlayerKnownLocationFactionOverride);

					}

				}

			}

		}

	}

}
