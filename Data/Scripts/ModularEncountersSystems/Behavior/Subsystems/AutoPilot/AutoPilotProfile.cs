using ProtoBuf;
using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Behavior.Subsystems.AutoPilot {

	public class AutoPilotProfile {

		//Profile
		public string ProfileSubtypeId;

		//Speed Config
		public float IdealMaxSpeed;
		public float IdealMinSpeed;
		public bool SlowDownOnWaypointApproach;
		public double ExtraSlowDownDistance;
		public float MaxSpeedTolerance;

		//Planet Config
		public bool FlyLevelWithGravity;
		public bool LevelWithGravityWhenIdle;
		public double MaxPlanetPathCheckDistance;
		public double IdealPlanetAltitude;
		public double MinimumPlanetAltitude;
		public double AltitudeTolerance;
		public double WaypointTolerance;

		//Offset Space Config
		public double OffsetSpaceMinDistFromTarget;
		public double OffsetSpaceMaxDistFromTarget;

		//Offset Planet Config
		public double OffsetPlanetMinDistFromTarget;
		public double OffsetPlanetMaxDistFromTarget;
		public double OffsetPlanetMinTargetAltitude;
		public double OffsetPlanetMaxTargetAltitude;

		//Offset Misc Config
		public bool ReverseOffsetDistAltAboveHeight;
		public double ReverseOffsetHeight;
		public int WaypointWaitTimeTrigger;
		public int WaypointAbandonTimeTrigger;

		//Circle Target Config
		public bool CircleTargetClockwise;
		public bool CircleTargetRadiusConstriction;
		public double CircleTargetRadiusConstrictionAmount;
		public bool CircleTargetAltitudeConstriction;
		public double CircleTargetAltitudeConstrictionAmount;
		public bool CircleTargetDriftExemption;
		public double CircleTargetUpAngleLimit;
		public bool CircleTargetUseWaypointAbandon;

		//Collision Config
		public bool Unused;
		public double CollisionEvasionWaypointDistance; //Make Space and Planet Variant - OR... Make This Based on Detection Type!
		public double CollisionFallEvasionWaypointDistance;
		public double CollisionEvasionResumeDistance;
		public int CollisionEvasionResumeTime;
		public bool CollisionEvasionWaypointCalculatedAwayFromEntity;
		public double CollisionEvasionWaypointFromEntityMaxAngle;

		//Target Lead Config
		public bool UseProjectileLeadPrediction;
		public bool UseCollisionLeadPrediction;

		//Thrust Settings
		public double AngleAllowedForForwardThrust;
		public double MaxVelocityAngleForSpeedControl;
		public bool UseSubgridThrust;
		public double MaxSubgridThrustAngle;

		//Strafe Settings
		public bool AllowStrafing;
		public int StrafeMinDurationMs;
		public int StrafeMaxDurationMs;
		public int StrafeMinCooldownMs;
		public int StrafeMaxCooldownMs;
		public double StrafeSpeedCutOff;
		public double StrafeDistanceCutOff;
		public double StrafeMinimumTargetDistance;
		public double StrafeMinimumSafeAngleFromTarget;

		//Rotation Settings

		public float RotationMultiplier;
		public double DesiredAngleToTarget;
		public double RotationSlowdownAngle;
		public bool UseForcedRotationDampening;
		public float ForcedRotationDampeningAmount;
		public bool LimitRotationSpeed;
		public double MaxRotationMagnitude;
		public bool UseRotationBoostUp;
		public float RotationBoostUpAmount;

		//Dampener Control
		public bool DisableInertiaDampeners;
		public bool ForceDampenersEnabled;

		public double PadDistanceFromTarget;

		//Bunker
		public bool TryToLevelWithTarget;

		//Special Maneuvers
		public int BarrelRollMinDurationMs;
		public int BarrelRollMaxDurationMs;
		public int RamMinDurationMs;
		public int RamMaxDurationMs;

		//Engage Distances
		public double EngageDistanceSpace;
		public double EngageDistancePlanet;
		public double DisengageDistanceSpace;
		public double DisengageDistancePlanet;

		public int TargetApproachTimer;
		public int TargetEngageTimer;

		public int TimeBetweenNewTargetChecks;
		public int LostTargetTimerTrigger;
		public double DistanceToCheckEngagableTarget;

		public bool EngageOnCameraDetection;
		public bool EngageOnWeaponActivation;
		public bool EngageOnTargetLineOfSight;

		public double CameraDetectionMaxRange;

		public bool RotateTowardsTargetWhileAtPosition;

		//Attack Run
		public double AttackRunDistanceSpace;
		public double AttackRunDistancePlanet;
		public double AttackRunBreakawayDistance;
		public int OffsetRecalculationTime;
		public bool AttackRunUseSafePlanetPathing;
		public bool AttackRunUseCollisionEvasionSpace;
		public bool AttackRunUseCollisionEvasionPlanet;
		public bool AttackRunOverrideWithDistanceAndTimer;
		public int AttackRunOverrideTimerTrigger;
		public int AttackRunMaxTimeTrigger;
		public double AttackRunOverrideDistance;

		public double DespawnCoordsMinDistance;
		public double DespawnCoordsMaxDistance;
		public double DespawnCoordsMinAltitude;
		public double DespawnCoordsMaxAltitude;
		public double MinAngleForLeveledDescent;
		public double MaxAngleForLeveledAscent;

		public double MinGravity;
		public double MaxGravity;
		public bool AvoidPlayerCollisions;
		public bool UseSurfaceHoverThrustMode;
		public double MaxVerticalSpeed;
		public double HoverPathStepDistance;
		public double HoverCliffAngle;

		public double HoverUpAngle;

		public BoolEnum UseVelocityCollisionEvasion;
		public bool UseVerticalRetreat;
		public bool UseWaterPatrolMode;

		public bool EscortUsesRelativeDampening;
		public double EscortSpeedMatchMinDistance;
		public double EscortSpeedMatchMaxDistance;


		public AutoPilotProfile() {

			ProfileSubtypeId = "";

			DisableInertiaDampeners = false;
			ForceDampenersEnabled = false;
			IdealMaxSpeed = 100;
			IdealMinSpeed = 10;
			SlowDownOnWaypointApproach = false;
			ExtraSlowDownDistance = 25; 
			MaxSpeedTolerance = 15;

			FlyLevelWithGravity = false; 
			LevelWithGravityWhenIdle = false; 
			MaxPlanetPathCheckDistance = 1000;
			IdealPlanetAltitude = 200;
			MinimumPlanetAltitude = 110;
			AltitudeTolerance = 10;
			WaypointTolerance = 10;

			OffsetSpaceMinDistFromTarget = 100;
			OffsetSpaceMaxDistFromTarget = 200;

			OffsetPlanetMinDistFromTarget = 100;
			OffsetPlanetMaxDistFromTarget = 200;
			OffsetPlanetMinTargetAltitude = 100;
			OffsetPlanetMaxTargetAltitude = 200;

			ReverseOffsetDistAltAboveHeight = false;
			ReverseOffsetHeight = 1300;

			CircleTargetClockwise = true;
			CircleTargetRadiusConstriction = false;
			CircleTargetRadiusConstrictionAmount = 10;
			CircleTargetAltitudeConstriction = false;
			CircleTargetAltitudeConstrictionAmount = 10;
			CircleTargetUpAngleLimit = 20;
			CircleTargetDriftExemption = false;

			PadDistanceFromTarget = 0;

			TryToLevelWithTarget = false;
			Unused = true;
			CollisionEvasionWaypointDistance = 300;
			CollisionFallEvasionWaypointDistance = 75;
			CollisionEvasionResumeDistance = 25;
			CollisionEvasionResumeTime = 10;
			CollisionEvasionWaypointCalculatedAwayFromEntity = false;
			CollisionEvasionWaypointFromEntityMaxAngle = 15;

			UseProjectileLeadPrediction = true;
			UseCollisionLeadPrediction = false;

			AngleAllowedForForwardThrust = 35;
			MaxVelocityAngleForSpeedControl = 5;

			AllowStrafing = false;
			StrafeMinDurationMs = 750;
			StrafeMaxDurationMs = 1500;
			StrafeMinCooldownMs = 1000;
			StrafeMaxCooldownMs = 3000;
			StrafeSpeedCutOff = 100;
			StrafeDistanceCutOff = 175;

			StrafeMinimumTargetDistance = 250;
			StrafeMinimumSafeAngleFromTarget = 25;

			RotationMultiplier = 1;
			RotationSlowdownAngle = 70;
			UseForcedRotationDampening = false;
			ForcedRotationDampeningAmount = 0.15f;
			UseRotationBoostUp = false;
			RotationBoostUpAmount = 3.14f;

			BarrelRollMinDurationMs = 3000;
			BarrelRollMaxDurationMs = 5000;

			RamMinDurationMs = 7000;
			RamMaxDurationMs = 12000;

			EngageDistanceSpace = 500;
			EngageDistancePlanet = 500;
			DisengageDistanceSpace = 600;
			DisengageDistancePlanet = 600;

			WaypointWaitTimeTrigger = 5;
			WaypointAbandonTimeTrigger = 30;

			TargetApproachTimer = 30;
			TargetEngageTimer = 10;

			TimeBetweenNewTargetChecks = 15;
			LostTargetTimerTrigger = 30;
			DistanceToCheckEngagableTarget = 1200;

			EngageOnCameraDetection = false;
			EngageOnWeaponActivation = false;
			EngageOnTargetLineOfSight = false;

			CameraDetectionMaxRange = 1800;

			RotateTowardsTargetWhileAtPosition = false;

			AttackRunDistanceSpace = 75;
			AttackRunDistancePlanet = 100;
			AttackRunBreakawayDistance = 450;
			OffsetRecalculationTime = 30;
			AttackRunUseSafePlanetPathing = true;
			AttackRunUseCollisionEvasionSpace = true;
			AttackRunUseCollisionEvasionPlanet = false;
			AttackRunOverrideWithDistanceAndTimer = true;
			AttackRunOverrideTimerTrigger = 20;
			AttackRunMaxTimeTrigger = -1;
			AttackRunOverrideDistance = 1200;

			DespawnCoordsMinDistance = 8000;
			DespawnCoordsMaxDistance = 11000;

			DespawnCoordsMinAltitude = 1500;
			DespawnCoordsMaxAltitude = 2500;

			MinAngleForLeveledDescent = 0;
			MaxAngleForLeveledAscent = 180;

			LimitRotationSpeed = false;
			MaxRotationMagnitude = 6.28;

			MinGravity = -1;
			MaxGravity = -1;

			AvoidPlayerCollisions = true;

			UseSurfaceHoverThrustMode = false;
			HoverPathStepDistance = 50;
			HoverCliffAngle = 80;

			HoverUpAngle = 10;
			MaxVerticalSpeed = -1;

			UseVerticalRetreat = false;

			UseWaterPatrolMode = false;

			EscortUsesRelativeDampening = false;
			EscortSpeedMatchMinDistance = 25;
			EscortSpeedMatchMaxDistance = 150;

			UseSubgridThrust = false;
			MaxSubgridThrustAngle = 35;

			UseVelocityCollisionEvasion = BoolEnum.True;

		}

		public void MinMaxSanityChecks() {

			MathTools.MinMaxRangeSafety(ref OffsetSpaceMinDistFromTarget, ref OffsetSpaceMaxDistFromTarget);
			MathTools.MinMaxRangeSafety(ref OffsetPlanetMinDistFromTarget, ref OffsetPlanetMaxDistFromTarget);
			MathTools.MinMaxRangeSafety(ref OffsetPlanetMinTargetAltitude, ref OffsetPlanetMaxTargetAltitude);

			MathTools.MinMaxRangeSafety(ref StrafeMinDurationMs, ref StrafeMaxDurationMs);
			MathTools.MinMaxRangeSafety(ref StrafeMinCooldownMs, ref StrafeMaxCooldownMs);

			MathTools.MinMaxRangeSafety(ref BarrelRollMinDurationMs, ref BarrelRollMaxDurationMs);
			MathTools.MinMaxRangeSafety(ref RamMinDurationMs, ref RamMaxDurationMs);

		}

		public void InitTags(string tagData) {

			if (!string.IsNullOrWhiteSpace(tagData)) {

				var descSplit = tagData.Split('\n');

				foreach (var tag in descSplit) {

					InitTag(tag);

				}

			}
		
		}

		public void InitTag(string tag) {

			//DisableInertiaDampeners
			if (tag.Contains("[DisableInertiaDampeners:") == true) {

				TagParse.TagBoolCheck(tag, ref DisableInertiaDampeners);

			}

			//ForceDampenersEnabled
			if (tag.Contains("[ForceDampenersEnabled:") == true) {

				TagParse.TagBoolCheck(tag, ref ForceDampenersEnabled);

			}

			//IdealMaxSpeed
			if (tag.Contains("[IdealMaxSpeed:") == true) {

				TagParse.TagFloatCheck(tag, ref this.IdealMaxSpeed);

			}

			//IdealMinSpeed
			if (tag.Contains("[IdealMinSpeed:") == true) {

				TagParse.TagFloatCheck(tag, ref this.IdealMinSpeed);

			}

			//SlowDownOnWaypointApproach
			if (tag.Contains("[SlowDownOnWaypointApproach:") == true) {

				TagParse.TagBoolCheck(tag, ref SlowDownOnWaypointApproach);

			}

			//ExtraSlowDownDistance
			if (tag.Contains("[ExtraSlowDownDistance:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.ExtraSlowDownDistance);

			}

			//MaxSpeedTolerance
			if (tag.Contains("[MaxSpeedTolerance:")) {

				TagParse.TagFloatCheck(tag, ref this.MaxSpeedTolerance);

			}

			//FlyLevelWithGravity
			if (tag.Contains("[FlyLevelWithGravity:") == true) {

				TagParse.TagBoolCheck(tag, ref FlyLevelWithGravity);

			}

			//LevelWithGravityWhenIdle
			if (tag.Contains("[LevelWithGravityWhenIdle:") == true) {

				TagParse.TagBoolCheck(tag, ref LevelWithGravityWhenIdle);

			}

			//MaxPlanetPathCheckDistance
			if (tag.Contains("[MaxPlanetPathCheckDistance:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.MaxPlanetPathCheckDistance);

			}

			//IdealPlanetAltitude
			if (tag.Contains("[IdealPlanetAltitude:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.IdealPlanetAltitude);

			}

			//MinimumPlanetAltitude
			if (tag.Contains("[MinimumPlanetAltitude:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.MinimumPlanetAltitude);

			}

			//AltitudeTolerance
			if (tag.Contains("[AltitudeTolerance:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.AltitudeTolerance);

			}

			//WaypointTolerance
			if (tag.Contains("[WaypointTolerance:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.WaypointTolerance);

			}

			//OffsetSpaceMinDistFromTarget
			if (tag.Contains("[OffsetSpaceMinDistFromTarget:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.OffsetSpaceMinDistFromTarget);

			}

			//OffsetSpaceMaxDistFromTarget
			if (tag.Contains("[OffsetSpaceMaxDistFromTarget:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.OffsetSpaceMaxDistFromTarget);

			}

			//OffsetPlanetMinDistFromTarget
			if (tag.Contains("[OffsetPlanetMinDistFromTarget:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.OffsetPlanetMinDistFromTarget);

			}

			//OffsetPlanetMaxDistFromTarget
			if (tag.Contains("[OffsetPlanetMaxDistFromTarget:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.OffsetPlanetMaxDistFromTarget);

			}

			//OffsetPlanetMinTargetAltitude
			if (tag.Contains("[OffsetPlanetMinTargetAltitude:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.OffsetPlanetMinTargetAltitude);

			}

			//OffsetPlanetMaxTargetAltitude
			if (tag.Contains("[OffsetPlanetMaxTargetAltitude:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.OffsetPlanetMaxTargetAltitude);

			}

			//ReverseOffsetDistAltAboveHeight
			if (tag.Contains("[ReverseOffsetDistAltAboveHeight:") == true) {

				TagParse.TagBoolCheck(tag, ref ReverseOffsetDistAltAboveHeight);

			}

			//ReverseOffsetHeight
			if (tag.Contains("[ReverseOffsetHeight:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.ReverseOffsetHeight);

			}

			//CircleTargetClockwise
			if (tag.Contains("[CircleTargetClockwise:") == true) {

				TagParse.TagBoolCheck(tag, ref CircleTargetClockwise);

			}

			//CircleTargetRadiusConstriction
			if (tag.Contains("[CircleTargetRadiusConstriction:") == true) {

				TagParse.TagBoolCheck(tag, ref CircleTargetRadiusConstriction);

			}

			//CircleTargetRadiusConstrictionAmount
			if (tag.Contains("[CircleTargetRadiusConstrictionAmount:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.CircleTargetRadiusConstrictionAmount);

			}

			//CircleTargetAltitudeConstriction
			if (tag.Contains("[CircleTargetAltitudeConstriction:") == true) {

				TagParse.TagBoolCheck(tag, ref CircleTargetAltitudeConstriction);

			}

			//CircleTargetAltitudeConstrictionAmount
			if (tag.Contains("[CircleTargetAltitudeConstrictionAmount:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.CircleTargetAltitudeConstrictionAmount);

			}

			//CircleTargetUpAngleLimit
			if (tag.Contains("[CircleTargetUpAngleLimit:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.CircleTargetUpAngleLimit);

			}

			//CircleTargetDriftExemption
			if (tag.Contains("[CircleTargetDriftExemption:") == true) {

				TagParse.TagBoolCheck(tag, ref CircleTargetDriftExemption);

			}

			//Unused
			if (tag.Contains("[Unused:") == true) {

				TagParse.TagBoolCheck(tag, ref Unused);

			}

			//CollisionEvasionWaypointDistance
			if (tag.Contains("[CollisionEvasionWaypointDistance:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.CollisionEvasionWaypointDistance);

			}

			//CollisionFallEvasionWaypointDistance
			if (tag.Contains("[CollisionFallEvasionWaypointDistance:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.CollisionFallEvasionWaypointDistance);

			}

			//CollisionEvasionResumeDistance
			if (tag.Contains("[CollisionEvasionResumeDistance:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.CollisionEvasionResumeDistance);

			}

			//CollisionEvasionResumeTime
			if (tag.Contains("[CollisionEvasionResumeTime:") == true) {

				TagParse.TagIntCheck(tag, ref this.CollisionEvasionResumeTime);

			}

			//CollisionEvasionWaypointCalculatedAwayFromEntity
			if (tag.Contains("[CollisionEvasionWaypointCalculatedAwayFromEntity:") == true) {

				TagParse.TagBoolCheck(tag, ref CollisionEvasionWaypointCalculatedAwayFromEntity);

			}

			//CollisionEvasionWaypointFromEntityMaxAngle
			if (tag.Contains("[CollisionEvasionWaypointFromEntityMaxAngle:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.CollisionEvasionWaypointFromEntityMaxAngle);

			}

			//UseProjectileLeadPrediction
			if (tag.Contains("[UseProjectileLeadPrediction:") == true) {

				TagParse.TagBoolCheck(tag, ref UseProjectileLeadPrediction);

			}

			//UseCollisionLeadPrediction
			if (tag.Contains("[UseCollisionLeadPrediction:") == true) {

				TagParse.TagBoolCheck(tag, ref UseCollisionLeadPrediction);

			}

			////////////////////
			//Rotation and Thrust
			////////////////////

			//RotationMultiplier
			if (tag.Contains("[RotationMultiplier:") == true) {

				TagParse.TagFloatCheck(tag, ref this.RotationMultiplier);

			}

			//RotationSlowdownAngle
			if (tag.Contains("[RotationSlowdownAngle:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.RotationSlowdownAngle);

			}

			//UseForcedRotationDampening
			if (tag.Contains("[UseForcedRotationDampening:") == true) {

				TagParse.TagBoolCheck(tag, ref this.UseForcedRotationDampening);

			}

			//ForcedRotationDampeningAmount
			if (tag.Contains("[ForcedRotationDampeningAmount:") == true) {

				TagParse.TagFloatCheck(tag, ref this.ForcedRotationDampeningAmount);

			}

			//UseRotationBoostUp
			if (tag.Contains("[UseRotationBoostUp:") == true) {

				TagParse.TagBoolCheck(tag, ref this.UseRotationBoostUp);

			}

			//RotationBoostUpAmount
			if (tag.Contains("[RotationBoostUpAmount:") == true) {

				TagParse.TagFloatCheck(tag, ref this.RotationBoostUpAmount);

			}

			//AngleAllowedForForwardThrust
			if (tag.Contains("[AngleAllowedForForwardThrust:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.AngleAllowedForForwardThrust);

			}

			//MaxVelocityAngleForSpeedControl
			if (tag.Contains("[MaxVelocityAngleForSpeedControl:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.MaxVelocityAngleForSpeedControl);

			}

			//AllowStrafing
			if (tag.Contains("[AllowStrafing:") == true) {

				TagParse.TagBoolCheck(tag, ref AllowStrafing);

			}

			//StrafeMinDurationMs
			if (tag.Contains("[StrafeMinDurationMs:") == true) {

				TagParse.TagIntCheck(tag, ref this.StrafeMinDurationMs);

			}

			//StrafeMaxDurationMs
			if (tag.Contains("[StrafeMaxDurationMs:") == true) {

				TagParse.TagIntCheck(tag, ref this.StrafeMaxDurationMs);

			}

			//StrafeMinCooldownMs
			if (tag.Contains("[StrafeMinCooldownMs:") == true) {

				TagParse.TagIntCheck(tag, ref this.StrafeMinCooldownMs);

			}

			//StrafeMaxCooldownMs
			if (tag.Contains("[StrafeMaxCooldownMs:") == true) {

				TagParse.TagIntCheck(tag, ref this.StrafeMaxCooldownMs);

			}

			//StrafeSpeedCutOff
			if (tag.Contains("[StrafeSpeedCutOff:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.StrafeSpeedCutOff);

			}

			//StrafeDistanceCutOff
			if (tag.Contains("[StrafeDistanceCutOff:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.StrafeDistanceCutOff);

			}

			//StrafeMinimumTargetDistance
			if (tag.Contains("[StrafeMinimumTargetDistance:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.StrafeMinimumTargetDistance);

			}

			//StrafeMinimumSafeAngleFromTarget
			if (tag.Contains("[StrafeMinimumSafeAngleFromTarget:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.StrafeMinimumSafeAngleFromTarget);

			}

			////////////////////
			//The Rest
			////////////////////

			//PadDistanceFromTarget
			if (tag.Contains("[PadDistanceFromTarget:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.PadDistanceFromTarget);

			}
			//AccountForBunkers
			if (tag.Contains("[TryToLevelWithTarget:") == true)
			{

				TagParse.TagBoolCheck(tag, ref this.TryToLevelWithTarget);

			}
			


			//BarrelRollMinDurationMs
			if (tag.Contains("[BarrelRollMinDurationMs:") == true) {

				TagParse.TagIntCheck(tag, ref this.BarrelRollMinDurationMs);

			}

			//BarrelRollMaxDurationMs
			if (tag.Contains("[BarrelRollMaxDurationMs:") == true) {

				TagParse.TagIntCheck(tag, ref this.BarrelRollMaxDurationMs);

			}

			//RamMinDurationMs
			if (tag.Contains("[RamMinDurationMs:") == true) {

				TagParse.TagIntCheck(tag, ref this.RamMinDurationMs);

			}

			//RamMaxDurationMs
			if (tag.Contains("[RamMaxDurationMs:") == true) {

				TagParse.TagIntCheck(tag, ref this.RamMaxDurationMs);

			}

			//EngageDistanceSpace
			if (tag.Contains("[EngageDistanceSpace:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.EngageDistanceSpace);

			}

			//EngageDistancePlanet
			if (tag.Contains("[EngageDistancePlanet:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.EngageDistancePlanet);

			}

			//DisengageDistanceSpace
			if (tag.Contains("[DisengageDistanceSpace:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.DisengageDistanceSpace);

			}

			//DisengageDistancePlanet
			if (tag.Contains("[DisengageDistancePlanet:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.DisengageDistancePlanet);

			}

			//WaypointWaitTimeTrigger
			if (tag.Contains("[WaypointWaitTimeTrigger:") == true) {

				TagParse.TagIntCheck(tag, ref this.WaypointWaitTimeTrigger);

			}

			//TargetApproachTimer
			if (tag.Contains("[TargetApproachTimer:") == true) {

				TagParse.TagIntCheck(tag, ref this.TargetApproachTimer);

			}

			//TargetEngageTimer
			if (tag.Contains("[TargetEngageTimer:") == true) {

				TagParse.TagIntCheck(tag, ref this.TargetEngageTimer);

			}

			//TimeBetweenNewTargetChecks
			if (tag.Contains("[TimeBetweenNewTargetChecks:") == true) {

				TagParse.TagIntCheck(tag, ref this.TimeBetweenNewTargetChecks);

			}

			//LostTargetTimerTrigger
			if (tag.Contains("[LostTargetTimerTrigger:") == true) {

				TagParse.TagIntCheck(tag, ref this.LostTargetTimerTrigger);

			}

			//DistanceToCheckEngagableTarget
			if (tag.Contains("[DistanceToCheckEngagableTarget:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.DistanceToCheckEngagableTarget);

			}

			//EngageOnCameraDetection
			if (tag.Contains("[EngageOnCameraDetection:") == true) {

				TagParse.TagBoolCheck(tag, ref this.EngageOnCameraDetection);

			}

			//EngageOnWeaponActivation
			if (tag.Contains("[EngageOnWeaponActivation:") == true) {

				TagParse.TagBoolCheck(tag, ref this.EngageOnWeaponActivation);

			}

			//EngageOnTargetLineOfSight
			if (tag.Contains("[EngageOnTargetLineOfSight:") == true) {

				TagParse.TagBoolCheck(tag, ref this.EngageOnTargetLineOfSight);

			}

			//CameraDetectionMaxRange
			if (tag.Contains("[CameraDetectionMaxRange:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.CameraDetectionMaxRange);

			}

			//WaypointAbandonTimeTrigger
			if (tag.Contains("[WaypointAbandonTimeTrigger:") == true) {

				TagParse.TagIntCheck(tag, ref this.WaypointAbandonTimeTrigger);

			}

			//RotateTowardsTargetWhileAtPosition
			if (tag.Contains("[RotateTowardsTargetWhileAtPosition:") == true) {

				TagParse.TagBoolCheck(tag, ref this.RotateTowardsTargetWhileAtPosition);

			}

			//AttackRunDistanceSpace
			if (tag.Contains("[AttackRunDistanceSpace:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.AttackRunDistanceSpace);

			}

			//AttackRunDistancePlanet
			if (tag.Contains("[AttackRunDistancePlanet:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.AttackRunDistancePlanet);

			}

			//AttackRunBreakawayDistance
			if (tag.Contains("[AttackRunBreakawayDistance:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.AttackRunBreakawayDistance);

			}

			//OffsetRecalculationTime
			if (tag.Contains("[OffsetRecalculationTime:") == true) {

				TagParse.TagIntCheck(tag, ref this.OffsetRecalculationTime);

			}

			//AttackRunUseSafePlanetPathing
			if (tag.Contains("[AttackRunUseSafePlanetPathing:") == true) {

				TagParse.TagBoolCheck(tag, ref AttackRunUseSafePlanetPathing);

			}

			//AttackRunUseCollisionEvasionSpace
			if (tag.Contains("[AttackRunUseCollisionEvasionSpace:") == true) {

				TagParse.TagBoolCheck(tag, ref AttackRunUseCollisionEvasionSpace);

			}

			//AttackRunUseCollisionEvasionPlanet
			if (tag.Contains("[AttackRunUseCollisionEvasionPlanet:") == true) {

				TagParse.TagBoolCheck(tag, ref AttackRunUseCollisionEvasionPlanet);

			}

			//AttackRunOverrideWithDistanceAndTimer
			if (tag.Contains("[AttackRunOverrideWithDistanceAndTimer:") == true) {

				TagParse.TagBoolCheck(tag, ref AttackRunOverrideWithDistanceAndTimer);

			}

			//AttackRunOverrideTimerTrigger
			if (tag.Contains("[AttackRunOverrideTimerTrigger:") == true) {

				TagParse.TagIntCheck(tag, ref this.AttackRunOverrideTimerTrigger);

			}

			//AttackRunOverrideDistance
			if (tag.Contains("[AttackRunOverrideDistance:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.AttackRunOverrideDistance);

			}

			//DespawnCoordsMinDistance
			if (tag.Contains("[DespawnCoordsMinDistance:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.DespawnCoordsMinDistance);

			}

			//DespawnCoordsMaxDistance
			if (tag.Contains("[DespawnCoordsMaxDistance:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.DespawnCoordsMaxDistance);

			}

			//DespawnCoordsMinAltitude
			if (tag.Contains("[DespawnCoordsMinAltitude:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.DespawnCoordsMinAltitude);

			}

			//DespawnCoordsMaxAltitude
			if (tag.Contains("[DespawnCoordsMaxAltitude:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.DespawnCoordsMaxAltitude);

			}

			//MinAngleForLeveledDescent
			if (tag.Contains("[MinAngleForLeveledDescent:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.MinAngleForLeveledDescent);

			}

			//MaxAngleForLeveledAscent
			if (tag.Contains("[MaxAngleForLeveledAscent:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.MaxAngleForLeveledAscent);

			}

			//LimitRotationSpeed
			if (tag.Contains("[LimitRotationSpeed:") == true) {

				TagParse.TagBoolCheck(tag, ref LimitRotationSpeed);

			}

			//MaxRotationMagnitude
			if (tag.Contains("[MaxRotationMagnitude:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.MaxRotationMagnitude);

			}

			//MinGravity
			if (tag.Contains("[MinGravity:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.MinGravity);

			}

			//MaxGravity
			if (tag.Contains("[MaxGravity:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.MaxGravity);

			}

			//AvoidPlayerCollisions
			if (tag.Contains("[AvoidPlayerCollisions:") == true) {

				TagParse.TagBoolCheck(tag, ref AvoidPlayerCollisions);

			}

			//UseSurfaceHoverThrustMode
			if (tag.Contains("[UseSurfaceHoverThrustMode:") == true) {

				TagParse.TagBoolCheck(tag, ref UseSurfaceHoverThrustMode);

			}

			//MaxVerticalSpeed
			if (tag.Contains("[MaxVerticalSpeed:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.MaxVerticalSpeed);

			}

			//HoverPathStepDistance
			if (tag.Contains("[HoverPathStepDistance:") == true) {

				TagParse.TagDoubleCheck(tag, ref this.HoverPathStepDistance);

			}

			if (tag.Contains("[HoverCliffAngle:") == true)
			{

				TagParse.TagDoubleCheck(tag, ref this.HoverCliffAngle);

			}

			if (tag.Contains("[HoverUpAngle:") == true)
			{

				TagParse.TagDoubleCheck(tag, ref this.HoverUpAngle);

			}

			//UseVelocityCollisionEvasion
			if (tag.Contains("[UseVelocityCollisionEvasion:") == true) {

				TagParse.TagBoolEnumCheck(tag, ref UseVelocityCollisionEvasion);

			}

			//UseVerticalRetreat
			if (tag.Contains("[UseVerticalRetreat:") == true) {

				TagParse.TagBoolCheck(tag, ref UseVerticalRetreat);

			}

			//UseWaterPatrolMode
			if (tag.Contains("[UseWaterPatrolMode:") == true) {

				TagParse.TagBoolCheck(tag, ref UseWaterPatrolMode);

			}

			//UseSubgridThrust
			if (tag.Contains("[UseSubgridThrust:") == true) {

				TagParse.TagBoolCheck(tag, ref UseSubgridThrust);

			}

			//MaxSubgridThrustAngle
			if (tag.Contains("[MaxSubgridThrustAngle:") == true) {

				TagParse.TagDoubleCheck(tag, ref MaxSubgridThrustAngle);

			}

		}

	}

}
