using ProtoBuf;
using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Behavior.Subsystems.AutoPilot {

	[ProtoContract]
	public class AutoPilotProfileBak {

		//Profile
		[ProtoMember(1)]
		public string ProfileSubtypeId;

		//Speed Config
		[ProtoMember(2)]
		public float IdealMaxSpeed;

		[ProtoMember(3)]
		public float IdealMinSpeed;

		[ProtoMember(4)]
		public bool SlowDownOnWaypointApproach;

		[ProtoMember(5)]
		public double ExtraSlowDownDistance;

		[ProtoMember(6)]
		public float MaxSpeedTolerance;

		//Planet Config
		[ProtoMember(7)]
		public bool FlyLevelWithGravity;

		[ProtoMember(8)]
		public bool LevelWithGravityWhenIdle;

		[ProtoMember(9)]
		public double MaxPlanetPathCheckDistance;

		[ProtoMember(10)]
		public double IdealPlanetAltitude;

		[ProtoMember(11)]
		public double MinimumPlanetAltitude;

		[ProtoMember(12)]
		public double AltitudeTolerance;

		[ProtoMember(13)]
		public double WaypointTolerance;

		//Offset Space Config
		[ProtoMember(14)]
		public double OffsetSpaceMinDistFromTarget;

		[ProtoMember(15)]
		public double OffsetSpaceMaxDistFromTarget;

		//Offset Planet Config
		[ProtoMember(16)]
		public double OffsetPlanetMinDistFromTarget;

		[ProtoMember(17)]
		public double OffsetPlanetMaxDistFromTarget;

		[ProtoMember(18)]
		public double OffsetPlanetMinTargetAltitude;

		[ProtoMember(19)]
		public double OffsetPlanetMaxTargetAltitude;

		//Collision Config
		[ProtoMember(20)]
		public bool Unused;

		[ProtoMember(21)]
		public double CollisionEvasionWaypointDistance; //Make Space and Planet Variant - OR... Make This Based on Detection Type!

		[ProtoMember(22)]
		public double CollisionFallEvasionWaypointDistance;

		[ProtoMember(23)]
		public double CollisionEvasionResumeDistance;

		[ProtoMember(24)]
		public int CollisionEvasionResumeTime;

		[ProtoMember(25)]
		public bool CollisionEvasionWaypointCalculatedAwayFromEntity;

		[ProtoMember(26)]
		public double CollisionEvasionWaypointFromEntityMaxAngle;

		//Lead Config

		[ProtoMember(27)]
		public bool UseProjectileLeadPrediction;

		[ProtoMember(28)]
		public bool UseCollisionLeadPrediction;

		//Thrust Settings
		[ProtoMember(29)]
		public double AngleAllowedForForwardThrust;

		[ProtoMember(30)]
		public double MaxVelocityAngleForSpeedControl;

		//Strafe Settings
		[ProtoMember(31)]
		public bool AllowStrafing;

		[ProtoMember(32)]
		public int StrafeMinDurationMs;

		[ProtoMember(33)]
		public int StrafeMaxDurationMs;

		[ProtoMember(34)]
		public int StrafeMinCooldownMs;

		[ProtoMember(35)]
		public int StrafeMaxCooldownMs;

		[ProtoMember(36)]
		public double StrafeSpeedCutOff;

		[ProtoMember(37)]
		public double StrafeDistanceCutOff;

		[ProtoMember(38)]
		public double StrafeMinimumTargetDistance;

		[ProtoMember(39)]
		public double StrafeMinimumSafeAngleFromTarget;

		//Rotation Settings
		[ProtoMember(40)]
		public float RotationMultiplier;

		[ProtoMember(41)]
		public double DesiredAngleToTarget;

		[ProtoMember(42)]
		public bool DisableInertiaDampeners;

		[ProtoMember(43)]
		public bool ReverseOffsetDistAltAboveHeight;

		[ProtoMember(44)]
		public double ReverseOffsetHeight;

		[ProtoMember(45)]
		public double PadDistanceFromTarget;

		[ProtoMember(46)]
		public int BarrelRollMinDurationMs;

		[ProtoMember(47)]
		public int BarrelRollMaxDurationMs;

		[ProtoMember(48)]
		public int RamMinDurationMs;

		[ProtoMember(49)]
		public int RamMaxDurationMs;

		[ProtoMember(50)]
		public double EngageDistanceSpace;

		[ProtoMember(51)]
		public double EngageDistancePlanet;

		[ProtoMember(52)]
		public double DisengageDistanceSpace;

		[ProtoMember(53)]
		public double DisengageDistancePlanet;

		[ProtoMember(54)]
		public int WaypointWaitTimeTrigger;

		[ProtoMember(55)]
		public int WaypointAbandonTimeTrigger;

		[ProtoMember(56)]
		public double AttackRunDistanceSpace;

		[ProtoMember(57)]
		public double AttackRunDistancePlanet;

		[ProtoMember(58)]
		public double AttackRunBreakawayDistance;

		[ProtoMember(59)]
		public int OffsetRecalculationTime;

		[ProtoMember(60)]
		public bool AttackRunUseSafePlanetPathing;

		[ProtoMember(61)]
		public bool AttackRunUseCollisionEvasionSpace;

		[ProtoMember(62)]
		public bool AttackRunUseCollisionEvasionPlanet;

		[ProtoMember(63)]
		public bool AttackRunOverrideWithDistanceAndTimer;

		[ProtoMember(64)]
		public int AttackRunOverrideTimerTrigger;

		[ProtoMember(65)]
		public double AttackRunOverrideDistance;

		[ProtoMember(66)]
		public double DespawnCoordsMinDistance;

		[ProtoMember(67)]
		public double DespawnCoordsMaxDistance;

		[ProtoMember(68)]
		public double DespawnCoordsMinAltitude;

		[ProtoMember(69)]
		public double DespawnCoordsMaxAltitude;

		[ProtoMember(70)]
		public double MinAngleForLeveledDescent;

		[ProtoMember(71)]
		public double MaxAngleForLeveledAscent;

		[ProtoMember(72)]
		public bool LimitRotationSpeed;

		[ProtoMember(73)]
		public double MaxRotationMagnitude;

		[ProtoMember(74)]
		public double MinGravity;

		[ProtoMember(75)]
		public double MaxGravity;

		[ProtoMember(76)]
		public bool AvoidPlayerCollisions;

		[ProtoMember(77)]
		public bool UseSurfaceHoverThrustMode;

		[ProtoMember(78)]
		public double MaxVerticalSpeed;

		[ProtoMember(79)]
		public double HoverPathStepDistance;

		[ProtoMember(80)]
		public BoolEnum UseVelocityCollisionEvasion;

		[ProtoMember(81)]
		public bool UseVerticalRetreat;

		[ProtoMember(82)]
		public bool UseWaterPatrolMode;

		public AutoPilotProfileBak() {

			ProfileSubtypeId = "";

			DisableInertiaDampeners = false;
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

			PadDistanceFromTarget = 0;

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
			StrafeDistanceCutOff = 100;

			StrafeMinimumTargetDistance = 250;
			StrafeMinimumSafeAngleFromTarget = 25;

			RotationMultiplier = 1;

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

			AttackRunDistanceSpace = 75;
			AttackRunDistancePlanet = 100;
			AttackRunBreakawayDistance = 450;
			OffsetRecalculationTime = 30;
			AttackRunUseSafePlanetPathing = false;
			AttackRunUseCollisionEvasionSpace = false;
			AttackRunUseCollisionEvasionPlanet = false;
			AttackRunOverrideWithDistanceAndTimer = false;
			AttackRunOverrideTimerTrigger = 0;
			AttackRunOverrideDistance = 0;

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

			MaxVerticalSpeed = -1;

			UseVerticalRetreat = false;

			UseWaterPatrolMode = false;

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

			//WaypointAbandonTimeTrigger
			if (tag.Contains("[WaypointAbandonTimeTrigger:") == true) {

				TagParse.TagIntCheck(tag, ref this.WaypointAbandonTimeTrigger);

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
			if (tag.Contains("[MaxGravityVacuum:") == true) {

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

		}

	}

}
