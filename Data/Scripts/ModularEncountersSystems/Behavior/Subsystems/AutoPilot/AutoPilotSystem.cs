using ModularEncountersSystems.API;
using ModularEncountersSystems.Behavior.Subsystems.Trigger;
using ModularEncountersSystems.Behavior.Subsystems.Weapons;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using Ingame = Sandbox.ModAPI.Ingame;

namespace ModularEncountersSystems.Behavior.Subsystems.AutoPilot {

	public enum AutoPilotDataMode {
	
		Primary,
		Secondary,
		Tertiary
	
	}

	public enum AutoPilotType {

		None,
		CargoShip,
		Legacy,
		RivalAI,

	}

	[Flags]
	public enum NewAutoPilotMode {

		None = 0,
		RotateToWaypoint = 1 << 0,
		ThrustForward = 1 << 1,
		Strafe = 1 << 2,
		LevelWithGravity = 1 << 3,
		ThrustUpward = 1 << 4,
		BarrelRoll = 1 << 5,
		CollisionAvoidance = 1 << 6,
		PlanetaryPathing = 1 << 7,
		WaypointFromTarget = 1 << 8,
		Ram = 1 << 9,
		OffsetWaypoint = 1 << 10,
		RotateToTarget = 1 << 11,
		WaterNavigation = 1 << 12,
		HeavyYaw = 1 << 13,
		WaypointFromEscort = 1 << 14,
		CircleTarget = 1 << 15,
		RoverNavigation = 1 << 16,

	}

	public enum PathCheckResult {

		Ok,
		TerrainHigherThanNpc,
		TerrainHigherThanWaypoint


	}
	public partial class AutoPilotSystem {

		//Non-Configurable
		private IMyRemoteControl _remoteControl;
		private IBehavior _behavior;

		public AutoPilotState State { get { return _behavior.BehaviorSettings.State; }}

		public AutoPilotProfile Data {

			get {

				if (State.DataMode == AutoPilotDataMode.Primary) {

					bool settingBlank = string.IsNullOrWhiteSpace(State.PrimaryAutopilotId);
					bool profileBlank = string.IsNullOrWhiteSpace(State.PrimaryAutoPilot?.ProfileSubtypeId);

					if (settingBlank && !profileBlank) {

						State.PrimaryAutopilotId = State.PrimaryAutoPilot.ProfileSubtypeId;

					}

					if (State.PrimaryAutopilotId != State.PrimaryAutoPilot.ProfileSubtypeId)
						State.PrimaryAutoPilot = ProfileManager.GetAutopilotProfile(State.PrimaryAutopilotId);

					return State.PrimaryAutoPilot;

				}

				if (State.DataMode == AutoPilotDataMode.Secondary) {

					//TODO: Fix Immediately
					if (State.SecondaryAutopilotId != State.SecondaryAutoPilot?.ProfileSubtypeId)
						State.SecondaryAutoPilot = ProfileManager.GetAutopilotProfile(State.SecondaryAutopilotId);

					return State.SecondaryAutoPilot != null ? State.SecondaryAutoPilot : State.PrimaryAutoPilot;

				}

				if (State.DataMode == AutoPilotDataMode.Tertiary) {

					//TODO: Fix Immediately
					if (State.TertiaryAutopilotId != State.TertiaryAutoPilot?.ProfileSubtypeId)
						State.TertiaryAutoPilot = ProfileManager.GetAutopilotProfile(State.TertiaryAutopilotId);

					return State.TertiaryAutoPilot != null ? State.TertiaryAutoPilot : State.PrimaryAutoPilot;

				}

				return null;

			}

			set {

				State.PrimaryAutoPilot = value;

			}
		
		}

		//New AutoPilot
		public NewAutoPilotMode CurrentMode {

			get {

				if (State != null)
					return State.AutoPilotFlags;

				return NewAutoPilotMode.None;

			}

			set {

				if (State != null)
					State.AutoPilotFlags = value;

			}
		
		}

		public NewAutoPilotMode UserCustomMode { get { return Data.FlyLevelWithGravity ? NewAutoPilotMode.LevelWithGravity : NewAutoPilotMode.None; } }
		public NewAutoPilotMode UserCustomModeIdle { get { return Data.LevelWithGravityWhenIdle ? NewAutoPilotMode.LevelWithGravity : NewAutoPilotMode.None; } }

		public CollisionSystem Collision;
		//public RotationSystem Rotation;
		public TargetingSystem Targeting;
		//public ThrustSystem Thrust;
		public TriggerSystem Trigger;
		public WeaponSystem Weapons;

		public double MaxSpeed { get { return State.MaxSpeedOverride != -1 ? State.MaxSpeedOverride : Data.IdealMaxSpeed; } }

		public WaypointModificationEnum DirectWaypointType;
		public WaypointModificationEnum IndirectWaypointType;
		private Vector3D _myPosition; //
		private Vector3D _forwardDir;
		private Vector3D _previousWaypoint;
		private Vector3D _initialWaypoint { get { return State.InitialWaypoint; } set { State.InitialWaypoint = value; } } //A static waypoint or derived from last valid target position.
		private Vector3D _pendingWaypoint { get { return State.PendingWaypoint; } set { State.PendingWaypoint = value; } } //Gets calculated in parallel.
		private Vector3D _currentWaypoint { get { return State.CurrentWaypoint; } set { State.CurrentWaypoint = value;} } //Actual position being travelled to.


		private Vector3D _calculatedOffsetWaypoint;
		private Vector3D _calculatedCircleTargetWaypoint;
		private Vector3D _calculatedPlanetPathWaypoint;
		private Vector3D _calculatedWeaponPredictionWaypoint;
		private Vector3D _evadeWaypoint;
		private Vector3D _evadeFromWaypoint;
		private DateTime _evadeWaypointCreateTime;
		public double MyAltitude;
		public Vector3D UpDirectionFromPlanet;

		public Vector3D MyVelocity;

		//Special Modes
		private bool _applyBarrelRoll;
		private int _barrelRollDuration;
		private DateTime _barrelRollStart;

		private bool _applyHeavyYaw;
		private int _heavyYawDuration;
		private DateTime _heavyYawStart;

		private bool _applyRamming;
		private int _ramDuration;
		private DateTime _ramStart;

		//Offset Stuff
		private bool _offsetRequiresCalculation;
		private WaypointOffsetType _offsetType;
		private double _offsetDistanceFromTarget;
		private double _offsetAltitudeFromTarget;
		private bool _offsetAltitudeIsMinimum;
		private double _offsetDistance;
		private double _offsetAltitude;
		private Vector3D _offsetDirection;
		private MatrixD _offsetMatrix;
		private IMyEntity _offsetRelativeEntity;

		//Circle Target
		internal bool _circleTargetNextWaypoint;
		internal bool _circleTargetRequiresReset;
		internal List<Vector3D> _circleTargetDirections;
		internal int _circleTargetCurrentIndex;
		internal MatrixD _circleTargetMatrix;
		internal double _circleTargetDistance;
		internal double _circleTargetTotalDistanceDecrement;
		internal double _circleTargetAltitude;
		internal double _circleTargetTotalAltitudeDecrement;
		internal bool _circleTargetGravity;

		//Autopilot Correction
		private DateTime _lastAutoPilotCorrection;
		private bool _needsThrustersRetoggled;

		public double DistanceToInitialWaypoint;
		public double DistanceToCurrentWaypoint;
		public double DistanceToTargetWaypoint;
		public double DistanceToWaypointAtMyAltitude;
		public double AngleToInitialWaypoint;
		public double AngleToCurrentWaypoint;
		public double AngleToUpDirection;
		public double DistanceToOffsetAtMyAltitude;
		public double DistanceToCircleTargetAtMyAltitude;
		private bool _requiresClimbToIdealAltitude;
		private bool _requiresNavigationAroundCollision;

		//PlanetData - Self
		private bool _inGravityLastUpdate;
		public PlanetEntity CurrentPlanet;
		public WaterPathing WaterPath;
		public RoverPathing RoverPath;

		private Vector3D _upDirection;
		private double _gravityStrength;
		private double _surfaceDistance;
		public float AirDensity;

		public Action OnComplete; //After Autopilot is done everything, it starts a new task elsewhere.

		private const double PLANET_PATH_CHECK_DISTANCE = 1000;
		private const double PLANET_PATH_CHECK_INCREMENT = 50;

		//Debug
		public List<BoundingBoxD> DebugVoxelHits = new List<BoundingBoxD>();
		private string _debugThrustForwardMode;
		private string _debugThrustUpMode;
		private string _debugThrustSideMode;
		public string DebugDataA;
		public string DebugDataB;
		public string DebugDataC;
		private static HudAPIv2.HUDMessage _hudText;
		private bool ShownGps;

		public AutoPilotSystem(IMyRemoteControl remoteControl, IBehavior behavior) {

			_behavior = behavior;
			Data = new AutoPilotProfile();

			//CurrentMode = NewAutoPilotMode.None;

			DirectWaypointType = WaypointModificationEnum.None;
			IndirectWaypointType = WaypointModificationEnum.None;
			_myPosition = Vector3D.Zero;
			_forwardDir = Vector3D.Zero;
			_previousWaypoint = Vector3D.Zero;
			_initialWaypoint = Vector3D.Zero;
			_pendingWaypoint = Vector3D.Zero;
			_currentWaypoint = Vector3D.Zero;
			_evadeWaypoint = Vector3D.Zero;
			_evadeFromWaypoint = Vector3D.Zero;
			_evadeWaypointCreateTime = DateTime.Now;
			MyAltitude = 0;
			UpDirectionFromPlanet = Vector3D.Zero;

			_offsetRequiresCalculation = false;
			_offsetType = WaypointOffsetType.None;
			_offsetDistanceFromTarget = 0;
			_offsetAltitudeFromTarget = 0;
			_offsetAltitudeIsMinimum = false;
			_offsetDistance = 0;
			_offsetAltitude = 0;
			_offsetDirection = Vector3D.Zero;
			_offsetMatrix = MatrixD.Identity;
			_offsetRelativeEntity = null;

			_circleTargetNextWaypoint = false;
			_circleTargetRequiresReset = false;
			_circleTargetDirections = new List<Vector3D>();
			_circleTargetCurrentIndex = 0;
			_circleTargetMatrix = MatrixD.Identity;
			_circleTargetDistance = 0;
			_circleTargetTotalDistanceDecrement = 0;
			_circleTargetAltitude = 0;
			_circleTargetTotalAltitudeDecrement = 0;
			_circleTargetGravity = false;

		_lastAutoPilotCorrection = MyAPIGateway.Session.GameDateTime;
			_needsThrustersRetoggled = false;

			_requiresClimbToIdealAltitude = false;
			_requiresNavigationAroundCollision = false;

			CurrentPlanet = null;
			WaterPath = new WaterPathing(_behavior);
			RoverPath = new RoverPathing(_behavior);
			_upDirection = Vector3D.Zero;
			_gravityStrength = 0;
			_surfaceDistance = 0;
			AirDensity = 0;

			_debugThrustForwardMode = "";
			_debugThrustUpMode = "";
			_debugThrustSideMode = "";
			DebugDataA = "";
			DebugDataB = "";
			DebugDataC = "";

			//Internal - Rotation
			GyroProfiles = new List<GyroscopeProfile>();

			RotationToApply = Vector3.Zero;

			//Internal - Thrust
			ThrustProfiles = new List<ThrusterProfile>();
			_thrustToApply = new ThrustAction();
			Rnd = new Random();

			_referenceOrientation = new MyBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Up);

			BehaviorLogger.Write("Strafe Setup Start", BehaviorDebugEnum.BehaviorSetup);
			Strafing = false;
			CurrentStrafeDirections = Vector3I.Zero;
			CurrentAllowedStrafeDirections = Vector3I.Zero;
			ThisStrafeDuration = Rnd.Next(Data.StrafeMinDurationMs, Data.StrafeMaxDurationMs);
			ThisStrafeCooldown = Rnd.Next(Data.StrafeMinCooldownMs, Data.StrafeMaxCooldownMs);
			LastStrafeStartTime = MyAPIGateway.Session.GameDateTime;
			LastStrafeEndTime = MyAPIGateway.Session.GameDateTime;
			_strafeStartPosition = Vector3D.Zero;
			BehaviorLogger.Write("Strafe Setup End", BehaviorDebugEnum.BehaviorSetup);

			_collisionStrafeAdjusted = false;
			_minAngleDistanceStrafeAdjusted = false;
			_collisionStrafeDirection = Vector3D.Zero;

			//Post Constructor Setup
			_remoteControl = remoteControl;
			Targeting = new TargetingSystem(_remoteControl);
			ShownGps = false;	
		}

		public void SetupReferences(IBehavior behavior, StoredSettings settings, TriggerSystem trigger) {

			Targeting.SetupReferences(behavior);
			Trigger = trigger;

		}

		public void InitTags() {

			if (string.IsNullOrWhiteSpace(_remoteControl.CustomData) == false) {

				Collision = new CollisionSystem(_remoteControl, this);

				var descSplit = _remoteControl.CustomData.Split('\n');

				foreach (var tag in descSplit) {

					State.PrimaryAutoPilot.InitTag(tag);

					//AutopilotData
					if (tag.Contains("[AutopilotData:")) {

						string profileId = "";
						TagParse.TagStringCheck(tag, ref profileId);
						State.PrimaryAutoPilot = ProfileManager.GetAutopilotProfile(profileId);

						if (!string.IsNullOrWhiteSpace(State.PrimaryAutoPilot?.ProfileSubtypeId) && string.IsNullOrWhiteSpace(State.PrimaryAutopilotId)) {

							State.PrimaryAutopilotId = State.PrimaryAutoPilot.ProfileSubtypeId;
							BehaviorLogger.Write("Primary AutoPilot: " + State.PrimaryAutopilotId, BehaviorDebugEnum.BehaviorSetup);

						} else {

							ProfileManager.ReportProfileError(profileId, "Primary AutoPilot Data Could Not Be Loaded");
						
						}

					}

					//SecondaryAutopilotData
					if (tag.Contains("[SecondaryAutopilotData:")) {

						string profileId = "";
						TagParse.TagStringCheck(tag, ref profileId);
						State.SecondaryAutoPilot = ProfileManager.GetAutopilotProfile(profileId);

						if (!string.IsNullOrWhiteSpace(State.SecondaryAutoPilot?.ProfileSubtypeId) && string.IsNullOrWhiteSpace(State.SecondaryAutopilotId)) {

							State.SecondaryAutopilotId = State.SecondaryAutoPilot.ProfileSubtypeId;
							BehaviorLogger.Write("Secondary AutoPilot: " + State.SecondaryAutopilotId, BehaviorDebugEnum.BehaviorSetup);

						} else {

							ProfileManager.ReportProfileError(profileId, "Secondary AutoPilot Data Could Not Be Loaded From Grid: ");

						}

					}

					//TertiaryAutopilotData
					if (tag.Contains("[TertiaryAutopilotData:")) {

						string profileId = "";
						TagParse.TagStringCheck(tag, ref profileId);
						State.TertiaryAutoPilot = ProfileManager.GetAutopilotProfile(profileId);

						if (!string.IsNullOrWhiteSpace(State.TertiaryAutoPilot?.ProfileSubtypeId) && string.IsNullOrWhiteSpace(State?.TertiaryAutopilotId)) {

							State.TertiaryAutopilotId = State.TertiaryAutoPilot.ProfileSubtypeId;
							BehaviorLogger.Write("Tertiary AutoPilot: " + State.TertiaryAutopilotId, BehaviorDebugEnum.BehaviorSetup);

						} else {

							ProfileManager.ReportProfileError(profileId, "Tertiary AutoPilot Data Could Not Be Loaded");

						}


					}

					//MinimumSpeedForVelocityChecks
					if (tag.Contains("[MinimumSpeedForVelocityChecks:") == true) {

						TagParse.TagDoubleCheck(tag, ref this.Collision.MinimumSpeedForVelocityChecks);

					}

					//CollisionAsteroidUsesBoundingBoxForVelocity
					if (tag.Contains("[CollisionAsteroidUsesBoundingBoxForVelocity:") == true) {

						TagParse.TagBoolCheck(tag, ref Collision.CollisionAsteroidUsesBoundingBoxForVelocity);

					}

					//CollisionTimeTrigger
					if (tag.Contains("[CollisionTimeTrigger:") == true) {

						TagParse.TagIntCheck(tag, ref this.Collision.CollisionTimeTrigger);

					}

					//AllowedStrafingDirectionsSpace


					//AllowedStrafingDirectionsPlanet


				}

				if (_remoteControl != null && MyAPIGateway.Entities.Exist(_remoteControl?.SlimBlock?.CubeGrid)) {

					Targeting.InitTags();
					Weapons = new WeaponSystem(_remoteControl, _behavior);

					var blockList = new List<IMySlimBlock>();
					GridManager.GetBlocksFromGrid<IMyTerminalBlock>(_remoteControl.SlimBlock.CubeGrid, blockList, true);

					foreach (var block in blockList.Where(item => item.FatBlock as IMyThrust != null)) {

						this.ThrustProfiles.Add(new ThrusterProfile(block.FatBlock as IMyThrust, _remoteControl, _behavior, Data.UseSubgridThrust, Data.MaxSubgridThrustAngle));

					}

					foreach (var block in blockList.Where(item => item.FatBlock as IMyGyro != null && item.CubeGrid == _remoteControl.SlimBlock.CubeGrid)) {

						this.GyroProfiles.Add(new GyroscopeProfile(block.FatBlock as IMyGyro, _remoteControl, _behavior));

					}

					BehaviorLogger.Write("Total Thrusters: " + this.ThrustProfiles.Count.ToString(), BehaviorDebugEnum.BehaviorSetup);
					BehaviorLogger.Write("Total Gyros:     " + this.GyroProfiles.Count.ToString(), BehaviorDebugEnum.BehaviorSetup);

				}

			}

		}

		public void ThreadedAutoPilotCalculations() {

			_myPosition = _remoteControl.GetPosition();
			
			DirectWaypointType = WaypointModificationEnum.None;
			IndirectWaypointType = WaypointModificationEnum.None;

			if (_remoteControl?.SlimBlock?.CubeGrid?.Physics != null) {

				MyVelocity = _remoteControl.SlimBlock.CubeGrid.Physics.LinearVelocity;

			} else {

				MyVelocity = Vector3D.Zero;

			}

			if (State.CurrentAutoPilot == AutoPilotType.None && State.FirstRun) {

				_initialWaypoint = Vector3D.Zero;
				_currentWaypoint = Vector3D.Zero;
				return;

			}

			this.RefBlockMatrixRotation = GetReferenceMatrix(_remoteControl.WorldMatrix);
			_forwardDir = this.RefBlockMatrixRotation.Forward;

			_previousWaypoint = _currentWaypoint;

			if (CurrentMode.HasFlag(NewAutoPilotMode.WaypointFromTarget)) {

				if (Targeting.HasTarget()) {

					DirectWaypointType = WaypointModificationEnum.TargetIsInitialWaypoint;
					_initialWaypoint = Targeting.GetTargetCoords();

				} else {

					//BehaviorLogger.Write(" - Autopilot Has No Target", BehaviorDebugEnum.TargetEvaluation);

				}

			}

			try {

				//Escort
				if (CurrentMode.HasFlag(NewAutoPilotMode.WaypointFromEscort)) {

					if (_behavior.BehaviorSettings.ParentEscort != null && _behavior.BehaviorSettings.ParentEscort.Valid) {

						_initialWaypoint = _behavior.BehaviorSettings.ParentEscort.GetTransformedOffset(_initialWaypoint);
						IndirectWaypointType |= WaypointModificationEnum.EscortPathing;

					}

				}

				CalculateCurrentWaypoint();
				_currentWaypoint = _pendingWaypoint;
				this.DistanceToInitialWaypoint = Vector3D.Distance(_myPosition, _initialWaypoint);
				this.DistanceToCurrentWaypoint = Vector3D.Distance(_myPosition, _currentWaypoint);

				if(Targeting.HasTarget())
					DistanceToTargetWaypoint = Vector3D.Distance(_myPosition, Targeting.TargetLastKnownCoords);

				this.AngleToInitialWaypoint = VectorHelper.GetAngleBetweenDirections(_forwardDir, Vector3D.Normalize(_initialWaypoint - _myPosition));
				this.AngleToCurrentWaypoint = VectorHelper.GetAngleBetweenDirections(_forwardDir, Vector3D.Normalize(_currentWaypoint - _myPosition));
				this.DistanceToWaypointAtMyAltitude = VectorHelper.GetDistanceToTargetAtMyAltitude(_myPosition, _currentWaypoint, CurrentPlanet);
				this.DistanceToOffsetAtMyAltitude = VectorHelper.GetDistanceToTargetAtMyAltitude(_myPosition, _calculatedOffsetWaypoint, CurrentPlanet);
				this.DistanceToCircleTargetAtMyAltitude = VectorHelper.GetDistanceToTargetAtMyAltitude(_myPosition, _calculatedCircleTargetWaypoint, CurrentPlanet);
				this.MyAltitude = _surfaceDistance;
				this.UpDirectionFromPlanet = _upDirection;

				State.FirstRun = true;

			} catch (Exception exc) {

				BehaviorLogger.Write("Caught Exception While Calculating Autopilot Pathing", BehaviorDebugEnum.General);
				BehaviorLogger.Write(exc.ToString(), BehaviorDebugEnum.General);

			}

		}

		public void EngageAutoPilot() {

			if (State.CurrentAutoPilot == AutoPilotType.Legacy)
				UpdateLegacyAutoPilot();

			if (State.CurrentAutoPilot == AutoPilotType.RivalAI)
				ApplyAutopilot();

			/*
			if (_currentWaypoint == Vector3D.Zero)
				BehaviorLogger.Write("No Current Waypoint", BehaviorDebugEnum.Dev);
				*/

			//StartWeaponCalculations();

		}

		//Obsolete Since Vanilla Autopilot is Rewritten - Will Remove When Safe To
		private void UpdateLegacyAutoPilot() {

			if (_remoteControl.IsAutoPilotEnabled && Vector3D.Distance(_previousWaypoint, _currentWaypoint) < this.Data.WaypointTolerance) {

				return;

			}
				

			_remoteControl.SetAutoPilotEnabled(false); //
			_remoteControl.ClearWaypoints();

			if (MaxSpeed == 0)
				return;

			/*
			 
			//Obsolete Since Vanilla Autopilot is Rewritten
			 
			if (UseStuckMovementCorrection && _upDirection != Vector3D.Zero) {

				var timeSpan = MyAPIGateway.Session.GameDateTime - _lastAutoPilotCorrection;

				if (timeSpan.TotalSeconds > 5) {

					if (Collision.Velocity.Length() < this.MaxSpeedWhenStuck) {

						bool yawing = false;

						if (_remoteControl?.SlimBlock?.CubeGrid?.Physics != null) {

							var rotationAxis = Vector3D.Normalize(_remoteControl.SlimBlock.CubeGrid.Physics.AngularVelocity);
							var myUp = _remoteControl.WorldMatrix.Up;
							var myDown = _remoteControl.WorldMatrix.Down;
							yawing = VectorHelper.GetAngleBetweenDirections(myUp, rotationAxis) <= 1 || VectorHelper.GetAngleBetweenDirections(myDown, rotationAxis) <= 1;

						}

						if (VectorHelper.GetAngleBetweenDirections(_upDirection, Vector3D.Normalize(Collision.Velocity)) <= this.MaxUpAngleWhenStuck || yawing) {

							BehaviorLogger.Write("AutoPilot Stuck, Attempting Fix", BehaviorDebugEnum.General);
							_lastAutoPilotCorrection = MyAPIGateway.Session.GameDateTime;
							_needsThrustersRetoggled = true;
							_remoteControl.ControlThrusters = false;
							return;

						}

					}

				}

			}

			if (_needsThrustersRetoggled) {

				_needsThrustersRetoggled = false;
				_remoteControl.ControlThrusters = true;

			}
			*/
			_remoteControl.AddWaypoint(_currentWaypoint, "Current Waypoint Target");
			_remoteControl.FlightMode = Ingame.FlightMode.OneWay;
			//_remoteControl.SetCollisionAvoidance(this.UseVanillaCollisionAvoidance);
			_remoteControl.SetAutoPilotEnabled(true);

		}

		public void PrepareAutopilot() {

			bool hasWaypoint = _currentWaypoint != Vector3D.Zero;
			ProcessRotationParallel(hasWaypoint);
			ProcessThrustParallel(hasWaypoint);

		}

		public string GetAutopilotData() {

			var sb = new StringBuilder();

			if (_remoteControl == null) {

				sb.Append(" - Remote Control Null, Cannot Get Autopilot Details");
				sb.AppendLine();
				return sb.ToString();

			}

			sb.Append(" - Current Profile:                    ").Append(Data?.ProfileSubtypeId ?? "null").AppendLine();
			sb.Append(" - Speed:                              ").Append(Math.Round(MyVelocity.Length(), 4).ToString()).AppendLine();
			sb.Append(" - Altitude:                           ").Append(MyAltitude.ToString()).AppendLine();
			sb.Append(" - Position:                           ").Append(_remoteControl.GetPosition().ToString()).AppendLine();
			sb.Append(" - Initial Waypoint:                   ").Append(_initialWaypoint.ToString()).AppendLine();
			sb.Append(" - Current Waypoint:                   ").Append(_currentWaypoint.ToString()).AppendLine();
			sb.Append(" - Offset Waypoint:                    ").Append(_calculatedOffsetWaypoint.ToString()).AppendLine();
			sb.Append(" - Offset Distance:                    ").Append(_offsetDistance.ToString()).AppendLine();
			sb.Append(" - Offset Altitude:                    ").Append(_offsetAltitude.ToString()).AppendLine();
			sb.Append(" - Distance To Initial Waypoint:       ").Append(Vector3D.Distance(_initialWaypoint, _remoteControl.GetPosition())).AppendLine();
			sb.Append(" - Distance To Current Waypoint:       ").Append(Vector3D.Distance(_currentWaypoint, _remoteControl.GetPosition())).AppendLine();
			sb.Append(" - Distance From Initial To Current:   ").Append(Vector3D.Distance(_currentWaypoint, _initialWaypoint)).AppendLine();
			sb.Append(" - Angle To Initial Waypoint:          ").Append(VectorHelper.GetAngleBetweenDirections(Vector3D.Normalize(_remoteControl.WorldMatrix.Forward), Vector3D.Normalize(_initialWaypoint - _remoteControl.GetPosition()))).AppendLine();
			sb.Append(" - Angle To Current Waypoint:          ").Append(VectorHelper.GetAngleBetweenDirections(Vector3D.Normalize(_remoteControl.WorldMatrix.Forward), Vector3D.Normalize(_currentWaypoint - _remoteControl.GetPosition()))).AppendLine();
			sb.Append(" - Velocity Angle To Initial Waypoint: ").Append(VectorHelper.GetAngleBetweenDirections(Vector3D.Normalize(MyVelocity), Vector3D.Normalize(_initialWaypoint - _remoteControl.GetPosition()))).AppendLine();
			sb.Append(" - Velocity Angle To Current Waypoint: ").Append(VectorHelper.GetAngleBetweenDirections(Vector3D.Normalize(MyVelocity), Vector3D.Normalize(_currentWaypoint - _remoteControl.GetPosition()))).AppendLine();
			sb.AppendLine();

			sb.Append(" - Active Gyroscopes:                  ").Append(GyroProfiles.Count).AppendLine();
			sb.Append(" - Active Thrusters:                   ").Append(ThrustProfiles.Count.ToString()).AppendLine();

			sb.Append(" - Allowed Waypoint Types:             ").Append(DirectWaypointType.ToString()).AppendLine();
			sb.Append(" - Restricted Waypoint Types:          ").Append(IndirectWaypointType.ToString()).AppendLine();
			sb.AppendLine();

			sb.Append(" - Dampeners Enabled:                  ").Append(_remoteControl.DampenersOverride.ToString()).AppendLine();
			sb.Append(" - Forward Thrust Mode:                ").Append(_debugThrustForwardMode).AppendLine();
			sb.Append(" - Forward Thrust Max Angle:           ").Append(Data?.AngleAllowedForForwardThrust.ToString() ?? "null").AppendLine();
			sb.Append(" - Upward Thrust Mode:                 ").Append(_debugThrustUpMode).AppendLine();
			sb.Append(" - Side Thrust Mode:                   ").Append(_debugThrustSideMode).AppendLine();
			sb.AppendLine();

			sb.Append(" - ForwardDir:                         ").Append(_behavior?.BehaviorSettings?.RotationDirection.ToString() ?? "null").AppendLine();
			sb.Append(" - Pitch:   ").AppendLine();
			sb.Append("   - Angle:                            ").Append(Math.Round(PitchAngleDifference, 2)).AppendLine();
			sb.Append("   - Target Diff:                      ").Append(Math.Round(PitchTargetAngleResult, 2)).AppendLine();
			sb.Append("   - Gyro Rotation:                    ").Append(Math.Round(ActiveGyro?.RawValues.X ?? 0, 4)).AppendLine();
			sb.Append("   - Magnitude:                        ").Append(Math.Round(ExistingPitchMagnitude, 4)).AppendLine();
			sb.Append(" - Yaw: ").AppendLine();
			sb.Append("   - Angle:                            ").Append(Math.Round(YawAngleDifference, 2)).AppendLine();
			sb.Append("   - Target Diff:                      ").Append(Math.Round(YawTargetAngleResult, 2)).AppendLine();
			sb.Append("   - Gyro Rotation:                    ").Append(Math.Round(ActiveGyro?.RawValues.Y ?? 0, 4)).AppendLine();
			sb.Append("   - Magnitude:                        ").Append(Math.Round(ExistingYawMagnitude, 4)).AppendLine();
			sb.Append(" - Roll: ").AppendLine();
			sb.Append("   - Angle:                            ").Append(Math.Round(RollAngleDifference, 2)).AppendLine();
			sb.Append("   - Target Diff:                      ").Append(Math.Round(RollTargetAngleResult, 2)).AppendLine();
			sb.Append("   - Gyro Rotation:                    ").Append(Math.Round(ActiveGyro?.RawValues.Z ?? 0, 4)).AppendLine();
			sb.Append("   - Magnitude:                        ").Append(Math.Round(ExistingRollMagnitude, 4)).AppendLine();

			if (State != null) {

				sb.AppendLine();

				sb.Append("::: Autopilot State Data :::").AppendLine();
				sb.Append(" - Fly Level With Gravity:             ").Append(State.UseFlyLevelWithGravity).AppendLine();
				sb.Append(" - Fly Level With Gravity Idle:        ").Append(State.UseFlyLevelWithGravityIdle).AppendLine();

			}

			if (Data != null) {

				sb.AppendLine();

				sb.Append("::: Autopilot Profile Data :::").AppendLine();
				sb.Append(" - Fly Level With Gravity:             ").Append(Data.FlyLevelWithGravity).AppendLine();
				sb.Append(" - Fly Level With Gravity Idle:        ").Append(Data.LevelWithGravityWhenIdle).AppendLine();

			}

			return sb.ToString();
		
		}

		private void ApplyAutopilot() {

			if (State?.DisableAutopilot ?? false)
				return;

			ApplyGyroRotation();
			ApplyThrust();


			if (BehaviorLogger.ActiveDebug.HasFlag(BehaviorDebugEnum.Dev)) {

				try {

					var sbStats = new StringBuilder();
					sbStats.Append("Ship:          ").Append(_remoteControl.SlimBlock.CubeGrid.CustomName).AppendLine().AppendLine();
					sbStats.Append("Behavior Mode: ").Append(_behavior.Mode).AppendLine();
					sbStats.Append("Speed:         ").Append(Math.Round(MyVelocity.Length(), 4).ToString()).AppendLine();
					sbStats.Append("Profile:       ").Append(Data.ProfileSubtypeId).AppendLine();
					sbStats.Append("AP Modes:      ").AppendLine();
					sbStats.Append(CurrentMode.ToString().Replace(",", "\r\n")).AppendLine();
					//sbStats.Append("Allowed Waypoint Types: ").AppendLine();
					//sbStats.Append(DirectWaypointType.ToString()).AppendLine();
					//sbStats.Append("Restricted Waypoint Types: ").AppendLine();
					//sbStats.Append(IndirectWaypointType.ToString()).AppendLine();

					sbStats.AppendLine().Append("Dampeners Enabled: ").Append(_remoteControl.DampenersOverride.ToString()).AppendLine();
					sbStats.Append("Forward Thrust Mode: ").AppendLine();
					sbStats.Append(_debugThrustForwardMode).AppendLine().AppendLine();
					sbStats.Append("Upward Thrust Mode:  ").AppendLine();
					sbStats.Append(_debugThrustUpMode).AppendLine().AppendLine();
					sbStats.Append("Side Thrust Mode:  ").AppendLine();
					sbStats.Append(_debugThrustSideMode).AppendLine().AppendLine();
					sbStats.Append("Altitude:  ").AppendLine();
					sbStats.Append(MyAltitude.ToString()).AppendLine();


					sbStats.AppendLine();
					/*
					sbStats.Append("Pitch: ").AppendLine();
					sbStats.Append(" - Angle:         ").Append(Math.Round(PitchAngleDifference, 2)).AppendLine();
					sbStats.Append(" - Target Diff:   ").Append(Math.Round(PitchTargetAngleResult, 2)).AppendLine();
					sbStats.Append(" - Gyro Rotation: ").Append(Math.Round(ActiveGyro.RawValues.X, 4)).AppendLine();
					sbStats.Append(" - Magnitude:     ").Append(Math.Round(ExistingPitchMagnitude, 4)).AppendLine();
					*/
					sbStats.Append("Yaw: ").AppendLine();
					sbStats.Append(" - Actual Target Angle:   ").Append(Math.Round(AngleToCurrentWaypoint, 2)).AppendLine();
					sbStats.Append(" - Angle Difference:      ").Append(Math.Round(YawAngleDifference, 2)).AppendLine();
					sbStats.Append(" - Target Angle Result:   ").Append(Math.Round(YawTargetAngleResult, 2)).AppendLine();
					sbStats.Append(" - Gyro Rotation:         ").Append(Math.Round(ActiveGyro.RawValues.Y, 4)).AppendLine();
					sbStats.Append(" - Magnitude:             ").Append(Math.Round(ExistingYawMagnitude, 4)).AppendLine();
					sbStats.Append(" - Tgt A Left:            ").Append(Math.Round(DebugYawAngleLeft, 4)).AppendLine();
					sbStats.Append(" - Tgt A Right:           ").Append(Math.Round(DebugYawAngleRight, 4)).AppendLine();
					/*
					sbStats.Append("Roll: ").AppendLine();
					sbStats.Append(" - Angle:         ").Append(Math.Round(RollAngleDifference, 2)).AppendLine();
					sbStats.Append(" - Target Diff:   ").Append(Math.Round(RollTargetAngleResult, 2)).AppendLine();
					sbStats.Append(" - Gyro Rotation: ").Append(Math.Round(ActiveGyro.RawValues.Z, 4)).AppendLine();
					sbStats.Append(" - Magnitude:     ").Append(Math.Round(ExistingRollMagnitude, 4)).AppendLine();
					*/
					if (APIs.TextHud.Heartbeat) {

						if (_hudText == null) {

							_hudText = new HudAPIv2.HUDMessage(sbStats, new Vector2D(-0.75, 0.75));

						} else {

							_hudText.Message = sbStats;

						}

					}

				} catch (Exception e) {



				}

			}

		}

		public void ActivateAutoPilot(Vector3D initialWaypoint, NewAutoPilotMode mode, CheckEnum useUserMode = CheckEnum.Ignore, CheckEnum useUserModeIdle = CheckEnum.Ignore) {

			DeactivateAutoPilot();
			State.NormalAutopilotFlags = mode;
			CurrentMode = mode;

			if (useUserMode != CheckEnum.Ignore)
				State.UseFlyLevelWithGravity = (useUserMode == CheckEnum.Yes);

			if (useUserModeIdle != CheckEnum.Ignore)
				State.UseFlyLevelWithGravityIdle = (useUserModeIdle == CheckEnum.Yes);

			if (State.UseFlyLevelWithGravity && Data.FlyLevelWithGravity)
				CurrentMode |= NewAutoPilotMode.LevelWithGravity;

			if (State.UseFlyLevelWithGravityIdle && Data.LevelWithGravityWhenIdle)
				CurrentMode |= NewAutoPilotMode.LevelWithGravity;

			State.CurrentAutoPilot = AutoPilotType.RivalAI;
			_initialWaypoint = initialWaypoint;

		}

		public void DeactivateAutoPilot() {

			CurrentMode = NewAutoPilotMode.None;
			_remoteControl.SetAutoPilotEnabled(false);
			_requiresNavigationAroundCollision = false;
			_requiresClimbToIdealAltitude = false;
			StopAllRotation();
			StopAllThrust();
			ApplyAutopilot();

		}

		public Vector3D GetInitialCoords() {

			return _initialWaypoint;

		}

		public bool InGravity() {

			return (_gravityStrength > 0);

		}

		public void SetInitialWaypoint(Vector3D coords) {

			_initialWaypoint = coords;
			DistanceToInitialWaypoint = Vector3D.Distance(coords, _myPosition);

		}

		private void CalculateCurrentWaypoint() {

			if (_initialWaypoint == Vector3D.Zero && State.FirstRun)
				return;

			if (CurrentPlanet == null || CurrentPlanet.Closed || PlanetManager.InGravity(_myPosition))
				CurrentPlanet = PlanetManager.GetNearestPlanet(_myPosition);
			else
				CurrentPlanet = null;

			_pendingWaypoint = _initialWaypoint;

			if (CurrentPlanet != null && CurrentPlanet.Gravity != null && CurrentPlanet.Gravity.IsPositionInRange(_myPosition)) {

				
				_upDirection = CurrentPlanet.UpAtPosition(_myPosition);
				_gravityStrength = CurrentPlanet.Gravity.GetWorldGravity(_myPosition).Length();
				_surfaceDistance = Vector3D.Distance(CurrentPlanet.SurfaceCoordsAtPosition(_myPosition), _myPosition);
				AirDensity = CurrentPlanet.Planet.GetAirDensity(_myPosition);

				if (!_inGravityLastUpdate && (_offsetType == WaypointOffsetType.RandomOffsetFixed || _offsetType == WaypointOffsetType.RandomOffsetRelativeEntity)) {

					_offsetRequiresCalculation = true;
					_inGravityLastUpdate = true;

				}

			} else {

				_upDirection = Vector3D.Zero;
				_gravityStrength = 0;
				AirDensity = 0;

				if (_inGravityLastUpdate && (_offsetType == WaypointOffsetType.RandomOffsetFixed || _offsetType == WaypointOffsetType.RandomOffsetRelativeEntity)) {

					_offsetRequiresCalculation = true;
					_inGravityLastUpdate = false;

				}

			}

			//BehaviorLogger.Write("Autopilot: BarrelRoll and Ram", BehaviorDebugEnum.TempDebug);
			if (_applyBarrelRoll) {

				var rollTime = MyAPIGateway.Session.GameDateTime - _barrelRollStart;

				if (rollTime.TotalMilliseconds >= _barrelRollDuration) {

					BehaviorLogger.Write("Barrel Roll End", BehaviorDebugEnum.AutoPilot);
					_applyBarrelRoll = false;
					CurrentMode &= ~NewAutoPilotMode.BarrelRoll;

				} else {

					if (!CurrentMode.HasFlag(NewAutoPilotMode.BarrelRoll))
						CurrentMode |= NewAutoPilotMode.BarrelRoll;

				}

			}

			if (_applyHeavyYaw) {

				var rollTime = MyAPIGateway.Session.GameDateTime - _heavyYawStart;

				if (rollTime.TotalMilliseconds >= _heavyYawDuration) {

					BehaviorLogger.Write("Heavy Yaw End", BehaviorDebugEnum.AutoPilot);
					_applyHeavyYaw = false;
					CurrentMode &= ~NewAutoPilotMode.HeavyYaw;

				} else {

					if (!CurrentMode.HasFlag(NewAutoPilotMode.HeavyYaw))
						CurrentMode |= NewAutoPilotMode.HeavyYaw;

				}

			}

			if (_applyRamming) {

				var rollTime = MyAPIGateway.Session.GameDateTime - _ramStart;

				if (rollTime.TotalMilliseconds >= _ramDuration) {

					BehaviorLogger.Write("Ramming End", BehaviorDebugEnum.AutoPilot);
					_applyRamming = false;
					CurrentMode &= ~NewAutoPilotMode.Ram;
					StopAllThrust();

				} else {

					if (!CurrentMode.HasFlag(NewAutoPilotMode.Ram))
						CurrentMode |= NewAutoPilotMode.Ram;

				}

			}

			if (CurrentMode.HasFlag(NewAutoPilotMode.Strafe)) {

				return;

			}

			//BehaviorLogger.Write("Autopilot: Collision", BehaviorDebugEnum.TempDebug);
			//Collision
			if (this.Data.UseVelocityCollisionEvasion == BoolEnum.True && Collision.VelocityResult.CollisionImminent()) {

				if ((_gravityStrength <= 0 && Collision.VelocityResult.Type == CollisionType.Voxel) || Collision.VelocityResult.Type != CollisionType.Voxel) {

					if (!_requiresNavigationAroundCollision) {

						CalculateEvadeCoords();


					} else {

						var directionToOldCollision = Vector3D.Normalize(_evadeFromWaypoint - _myPosition);
						var directionToNewCollision = Vector3D.Normalize(Collision.VelocityResult.GetCollisionCoords() - _myPosition);
						if (VectorHelper.GetAngleBetweenDirections(directionToOldCollision, directionToNewCollision) > 45)
							CalculateEvadeCoords();

					}

				} else if (Collision.VelocityResult.Type == CollisionType.Voxel && _gravityStrength > 0 && VectorHelper.GetAngleBetweenDirections(Vector3D.Normalize(Collision.Velocity), _upDirection) < 15) {

					CalculateFallEvadeCoords();

				}

			} 

			if (_requiresNavigationAroundCollision) {

				//BehaviorLogger.Write("Collision Evasion Required", BehaviorDebugEnum.General);
				IndirectWaypointType |= WaypointModificationEnum.Collision;
				var timeDiff = MyAPIGateway.Session.GameDateTime - _evadeWaypointCreateTime;
				var evadeCoordsDistanceFromTarget = Vector3D.Distance(_evadeWaypoint, _evadeFromWaypoint);
				var myDistEvadeFromCoords = Vector3D.Distance(_myPosition, _evadeFromWaypoint);

				if (myDistEvadeFromCoords < evadeCoordsDistanceFromTarget && Vector3D.Distance(_myPosition, _evadeWaypoint) > this.Data.WaypointTolerance && timeDiff.TotalSeconds < this.Data.CollisionEvasionResumeTime) {

					_pendingWaypoint = _evadeWaypoint;
					return;
				
				}
				
				_requiresNavigationAroundCollision = false;


			}

			//BehaviorLogger.Write("Autopilot: Pad Distance", BehaviorDebugEnum.TempDebug);
			if (Data.PadDistanceFromTarget != 0 && Targeting.HasTarget()) {

				var dirFromTarget = Vector3D.Normalize(_myPosition - Targeting.TargetLastKnownCoords);
				var roughPaddedCoords = dirFromTarget * Data.PadDistanceFromTarget + Targeting.TargetLastKnownCoords;

				if (InGravity()) {

					var surfaceCoords = CurrentPlanet.SurfaceCoordsAtPosition(roughPaddedCoords);
					var distRoughToSurface = Vector3D.Distance(surfaceCoords, roughPaddedCoords);
					var distsurfaceToCore = Vector3D.Distance(surfaceCoords, CurrentPlanet.Center());
					var distroughToCore = Vector3D.Distance(surfaceCoords, CurrentPlanet.Center());

					if (distRoughToSurface < Data.MinimumPlanetAltitude || distsurfaceToCore > distroughToCore) {

						var upAtSurface = Vector3D.Normalize(surfaceCoords - CurrentPlanet.Center());
						roughPaddedCoords = upAtSurface * MathTools.ValueBetween(Data.MinimumPlanetAltitude, Data.IdealPlanetAltitude);

					}

				}

				IndirectWaypointType |= WaypointModificationEnum.TargetPadding;
				_pendingWaypoint = roughPaddedCoords;

			}




			//Bunker busting
			if (Data.TryToLevelWithTarget && Targeting.HasTarget() && InGravity() && Targeting.Target.IsStatic() && Targeting.Target.Distance(_myPosition) < 1200 && Targeting.Target.CurrentAltitude() < 0)
			{
				/*
				 
				WIP

				var targetCoords = Targeting.TargetLastKnownCoords;

				var distanceTargetCore = (Targeting.TargetLastKnownCoords - CurrentPlanet.Center()).Length(); 

				var surfaceTargetCoords = CurrentPlanet.SurfaceCoordsAtPosition(targetCoords);

				var upAtSurface = Vector3D.Normalize(surfaceTargetCoords - CurrentPlanet.Center());
				var distanceSurfaceTargetCore = (surfaceTargetCoords - CurrentPlanet.Center()).Length();

				Vector3D AxisA = new Vector3D();
				upAtSurface.CalculatePerpendicularVector(out AxisA);
				Vector3D AxisB = Vector3D.Cross(upAtSurface, AxisA);


				var stepSize = 25; // Define the step size for exploration
				var maxSteps = 8; // Define the maximum number of steps to explore
				var tolerance = 0.1; // Define a tolerance value for distance comparison

				var closestSurfacePoint = surfaceTargetCoords;
				//var closestDistance = Math.Abs((distanceSurfaceTargetCore - distanceTargetCore));
				double highestAltitude = 0;

				for (int i = -maxSteps; i <= maxSteps; i++)
				{
					for (int j = -maxSteps; j <= maxSteps; j++)
					{
						// Calculate the exploration point along AxisA and AxisB directions
						var explorationPoint = surfaceTargetCoords + i * stepSize * AxisA + j * stepSize * AxisB;
						var surfaceExplorationCoords = CurrentPlanet.SurfaceCoordsAtPosition(explorationPoint);

						// Calculate the distance from the exploration point to the planet's core
						var distToCore = (explorationPoint - CurrentPlanet.Center()).Length();
						var distSurfaceToCore = (surfaceExplorationCoords - CurrentPlanet.Center()).Length();

						double Altitude = 0;

						//Altitude
						if (distToCore> distSurfaceToCore)
                        {
							Altitude= (explorationPoint - surfaceExplorationCoords).Length();
							var diffDistance = distToCore - distanceTargetCore;
							Altitude = Altitude - diffDistance;
						}
						else
							Altitude = -(explorationPoint - surfaceExplorationCoords).Length();

						if (Altitude > highestAltitude)
						{
							highestAltitude = Altitude;
							closestSurfacePoint = surfaceExplorationCoords;

						}
					}

				}


				//closestSurfacePoint;
				var upAtClosestSurfacePoint = Vector3D.Normalize(closestSurfacePoint - CurrentPlanet.Center());

				if (highestAltitude < Data.MinimumPlanetAltitude)
					highestAltitude = Data.MinimumPlanetAltitude;

				closestSurfacePoint = closestSurfacePoint+ upAtClosestSurfacePoint * highestAltitude;


				if (!ShownGps)
				{
					MyVisualScriptLogicProvider.AddGPSForAll("Target", "", Targeting.TargetLastKnownCoords, Color.Red);
					MyVisualScriptLogicProvider.AddGPSForAll("Pos", "", closestSurfacePoint, Color.Beige);
					ShownGps = true;
				}

				IndirectWaypointType |= WaypointModificationEnum.TargetPadding;
				_pendingWaypoint = closestSurfacePoint;
				*/

			}




			//Offset
			//BehaviorLogger.Write("Autopilot: Offset", BehaviorDebugEnum.TempDebug);
			if (CurrentMode.HasFlag(NewAutoPilotMode.OffsetWaypoint))
				OffsetWaypointGenerator();

			//Circle Target
			if (CurrentMode.HasFlag(NewAutoPilotMode.CircleTarget))
				CircleTargetHandling();

			//BehaviorLogger.Write("Autopilot: Planet Pathing", BehaviorDebugEnum.TempDebug);
			//PlanetPathing
			if (InGravity() && CurrentMode.HasFlag(NewAutoPilotMode.PlanetaryPathing) && _gravityStrength > 0) {

				if (!Data.UseSurfaceHoverThrustMode)
					CalculateSafePlanetPathWaypoint(CurrentPlanet);
				else
					CalculateHoverPath(CurrentPlanet);

				CalculateAllowedGravity(CurrentPlanet);

				if (_initialWaypoint != _pendingWaypoint) {

					IndirectWaypointType |= WaypointModificationEnum.PlanetPathing;

				}

			}

			//WaterNavigation
			if (InGravity() && CurrentMode.HasFlag(NewAutoPilotMode.WaterNavigation) && _gravityStrength > 0) {

				if (APIs.WaterModApiLoaded && WaterPath != null) {

					_pendingWaypoint = WaterPath.GetPathCoords(_myPosition, Data.WaypointTolerance);

					if (_initialWaypoint != _pendingWaypoint) {

						IndirectWaypointType |= WaypointModificationEnum.WaterPathing;

					}

				}
			
			}

			//RoverNavigation
			if (InGravity() && CurrentMode.HasFlag(NewAutoPilotMode.RoverNavigation) && _gravityStrength > 0)
			{

				if (RoverPath != null)
				{
					
					_pendingWaypoint = RoverPath.GetPathCoords(_myPosition, 50);


					if (_pendingWaypoint == Vector3D.Zero)
						CurrentMode |= NewAutoPilotMode.OffsetWaypoint;
					else if (_initialWaypoint != _pendingWaypoint)
					{

						IndirectWaypointType |= WaypointModificationEnum.RoverPathing;

					}

				}

			}


			//BehaviorLogger.Write("Autopilot: Projectile Lead", BehaviorDebugEnum.TempDebug);
			if (Targeting.Target != null && _initialWaypoint == _pendingWaypoint && Targeting.Target.CurrentSpeed() > 0.1) {

				bool gotLead = false;

				if (Data.UseCollisionLeadPrediction && !gotLead) {

					gotLead = true;
					DirectWaypointType |= WaypointModificationEnum.CollisionLeading;
					_pendingWaypoint = VectorHelper.FirstOrderIntercept((Vector3)_myPosition, (Vector3)MyVelocity, 0, (Vector3)_pendingWaypoint, (Vector3)Targeting.Target.CurrentVelocity());
					//_pendingWaypoint = VectorHelper.FirstOrderIntercept((Vector3)_myPosition, (Vector3)MyVelocity, (float)MyVelocity.Length(), (Vector3)_pendingWaypoint, (Vector3)Targeting.Target.CurrentVelocity());
					_calculatedWeaponPredictionWaypoint = _pendingWaypoint;

				}

				if (Data.UseProjectileLeadPrediction && !gotLead) {

					gotLead = true;
					DirectWaypointType |= WaypointModificationEnum.WeaponLeading;
					//BehaviorLogger.Write("Weapon Lead, Target Velocity: " + Targeting.Target.TargetVelocity.ToString(), BehaviorDebugEnum.Weapon);
					//_pendingWaypoint = VectorHelper.FirstOrderIntercept(_myPosition, _myVelocity, Subsystems.Weapons.MostCommonAmmoSpeed(true), _pendingWaypoint, Targeting.Target.CurrentVelocity());
					double ammoAccel;
					double ammoInitVel;
					double ammoVel;

					Weapons.GetAmmoSpeedDetails(_behavior.BehaviorSettings.RotationDirection, out ammoVel, out ammoInitVel, out ammoAccel);

					if (ammoVel > 0) {

						_pendingWaypoint = VectorHelper.TrajectoryEstimation(
						Targeting.TargetLastKnownCoords,
						Targeting.Target.CurrentVelocity(),
						Targeting.Target.CurrentAcceleration(),
						Targeting.Target.MaxSpeed(),
						_myPosition,
						MyVelocity,
						ammoVel,
						ammoInitVel,
						ammoAccel
						);

						_calculatedWeaponPredictionWaypoint = _pendingWaypoint;

					}

				}

				if (!gotLead)
					_calculatedWeaponPredictionWaypoint = Vector3D.Zero;

			}

			//BehaviorLogger.Write("Autopilot: Calculation Done", BehaviorDebugEnum.TempDebug);

		}

		

		private void CalculateEvadeCoords() {

			if (!this.Data.CollisionEvasionWaypointCalculatedAwayFromEntity) {

				Collision.RunSecondaryCollisionChecks();
				var dirFromCollision = Vector3D.Normalize(_myPosition - Collision.VelocityResult.GetCollisionCoords());
				var evadeCoordList = new List<Vector3D>();

				if (FoundEvadeCoords(Collision.ForwardResult))
					evadeCoordList.Add(_evadeWaypoint);

				if (FoundEvadeCoords(Collision.UpResult))
					evadeCoordList.Add(_evadeWaypoint);

				if (FoundEvadeCoords(Collision.DownResult))
					evadeCoordList.Add(_evadeWaypoint);

				if (FoundEvadeCoords(Collision.LeftResult))
					evadeCoordList.Add(_evadeWaypoint);

				if (FoundEvadeCoords(Collision.RightResult))
					evadeCoordList.Add(_evadeWaypoint);

				if (FoundEvadeCoords(Collision.BackwardResult))
					evadeCoordList.Add(_evadeWaypoint);

				Vector3D closestToTarget = Vector3D.Zero;
				double closestDistanceToTarget = -1;

				foreach (var coords in evadeCoordList) {

					var distToTarget = Vector3D.Distance(coords, _pendingWaypoint);

					if (distToTarget < closestDistanceToTarget || closestDistanceToTarget == -1) {

						closestToTarget = coords;
						closestDistanceToTarget = distToTarget;

					}

				}

				if (closestDistanceToTarget != -1) {

					_evadeWaypoint = closestToTarget;
					return;

				}

				//If we got here, no evade coords could be calculated
				//GuessIllJustDie.jpg
				BehaviorLogger.Write("No Collision Coords Found: ", BehaviorDebugEnum.Collision);


			} else {

				if (Collision.VelocityResult.GetCollisionEntity()?.PositionComp != null) {

					GetEvadeCoordsAwayFromEntity(Collision.VelocityResult.GetCollisionEntity().PositionComp.WorldAABB.Center);

				}

			}
			
		}

		private void CalculateFallEvadeCoords() {

			_requiresNavigationAroundCollision = true;
			_evadeWaypointCreateTime = MyAPIGateway.Session.GameDateTime;
			_evadeFromWaypoint = Collision.VelocityResult.GetCollisionCoords();
			_evadeWaypoint = Vector3D.Normalize(Collision.Velocity * -1) * this.Data.CollisionFallEvasionWaypointDistance + _myPosition;

		}

		private bool FoundEvadeCoords(CollisionResult result) {

			if (result.Type == CollisionType.None || result.GetCollisionDistance() > this.Data.CollisionEvasionWaypointDistance) {

				_requiresNavigationAroundCollision = true;
				_evadeWaypointCreateTime = MyAPIGateway.Session.GameDateTime;
				_evadeFromWaypoint = Collision.VelocityResult.GetCollisionCoords();
				_evadeWaypoint = result.DirectionVector * this.Data.CollisionEvasionWaypointDistance + _myPosition;
				_evadeWaypoint = _evadeWaypoint + (VectorHelper.RandomPerpendicular(result.DirectionVector) * 10);
				return true;

			}

			return false;

		}

		private void GetEvadeCoordsAwayFromEntity(Vector3D entityCoords) {

			BehaviorLogger.Write("Get Collision Evasion Waypoint Away From Entity", BehaviorDebugEnum.Collision);
			_requiresNavigationAroundCollision = true;
			_evadeWaypointCreateTime = MyAPIGateway.Session.GameDateTime;
			_evadeFromWaypoint = entityCoords;
			_evadeWaypoint = (Vector3D.Normalize(_myPosition - entityCoords)) * this.Data.CollisionEvasionWaypointDistance + _myPosition;

		}

		public void OffsetWaypointGenerator(bool requestRefresh = false) {

			if (requestRefresh) {

				_offsetRequiresCalculation = true;
				return;

			}

			//Check if New Offset Direction Is Required
			if (_offsetRequiresCalculation) {

				_offsetRequiresCalculation = false;
				_offsetMatrix = MatrixD.Identity;

				if (InGravity()) {

					_offsetDirection = Vector3D.Normalize(MyUtils.GetRandomPerpendicularVector((Vector3)_upDirection));

					bool reverseDistAlt = false;

					if (Data.ReverseOffsetDistAltAboveHeight) {

						var surfaceAtWaypoint = CurrentPlanet.SurfaceCoordsAtPosition(_pendingWaypoint);
						reverseDistAlt = Vector3D.Distance(surfaceAtWaypoint, _pendingWaypoint) > Data.ReverseOffsetHeight;

					}

					if (reverseDistAlt) {

						_offsetAltitude = MathTools.RandomBetween(Data.OffsetPlanetMinDistFromTarget, Data.OffsetPlanetMaxDistFromTarget); 
						_offsetDistance = MathTools.RandomBetween(Data.OffsetPlanetMinTargetAltitude, Data.OffsetPlanetMaxTargetAltitude);

					} else {

						_offsetAltitude = MathTools.RandomBetween(Data.OffsetPlanetMinTargetAltitude, Data.OffsetPlanetMaxTargetAltitude);
						_offsetDistance = MathTools.RandomBetween(Data.OffsetPlanetMinDistFromTarget, Data.OffsetPlanetMaxDistFromTarget);

					}

				} else {

					var directionRand = VectorHelper.RandomDirection();
					var directionRandInv = directionRand * -1;
					var dirDist = Vector3D.Distance(_pendingWaypoint + directionRand, _myPosition);
					var dirDistInv = Vector3D.Distance(_pendingWaypoint + directionRandInv, _myPosition);
					_offsetDirection = dirDist < dirDistInv ? directionRand : directionRandInv;
					_offsetAltitude = 0;
					_offsetDistance = MathTools.RandomBetween(Data.OffsetSpaceMinDistFromTarget, Data.OffsetSpaceMaxDistFromTarget);

				}
			
			}

			//Update Position and Matrix
			if (Targeting.HasTarget() && CurrentMode.HasFlag(NewAutoPilotMode.WaypointFromTarget)) {

				_offsetMatrix = Targeting.Target.GetEntity().PositionComp.WorldMatrixRef;

			} else {

				if (_offsetMatrix == MatrixD.Identity) {

					if (InGravity()) {

						_offsetMatrix = MatrixD.CreateWorld(CurrentPlanet.SurfaceCoordsAtPosition(_initialWaypoint), Vector3D.CalculatePerpendicularVector(_upDirection), _upDirection);

					} else {

						_offsetMatrix = MatrixD.CreateWorld(_initialWaypoint, _remoteControl.WorldMatrix.Forward, _remoteControl.WorldMatrix.Up);

					}
				
				}
			
			}

			var translation = _offsetMatrix.Translation;

			//Get Offset Waypoint
			if (InGravity()) {

				var roughPerpendicularCoords = _offsetDistance * _offsetDirection + translation;
				var roughCoordsSurface = CurrentPlanet.SurfaceCoordsAtPosition(roughPerpendicularCoords);
				var worldCenter = CurrentPlanet.Center();
				var upAtRoughCoords = Vector3D.Normalize(roughPerpendicularCoords - worldCenter);
				var centerToRoughDist = Vector3D.Distance(worldCenter, roughPerpendicularCoords);
				var centerToSurfaceDist = Vector3D.Distance(worldCenter, roughCoordsSurface);
				var minToIdealPlanetAltitude = MathTools.ValueBetween(Data.MinimumPlanetAltitude, Data.IdealPlanetAltitude);
				var offsetAlt = _offsetAltitude > minToIdealPlanetAltitude ? _offsetAltitude : minToIdealPlanetAltitude;

				if ((centerToRoughDist - centerToSurfaceDist) < Data.MinimumPlanetAltitude) {

					_pendingWaypoint = upAtRoughCoords * offsetAlt + roughCoordsSurface;

				} else {

					var candidateWaypoint = upAtRoughCoords * _offsetAltitude + roughPerpendicularCoords;
					var centerToCandidateDist = Vector3D.Distance(worldCenter, candidateWaypoint);

					if ((centerToCandidateDist - centerToSurfaceDist) < Data.MinimumPlanetAltitude) {

						_pendingWaypoint = upAtRoughCoords * offsetAlt + roughCoordsSurface;

					} else {

						_pendingWaypoint = candidateWaypoint;

					}

				}
			
			} else {

				_pendingWaypoint = _offsetDirection * _offsetDistance + translation;

			}

			_calculatedOffsetWaypoint = _pendingWaypoint;
			IndirectWaypointType |= WaypointModificationEnum.Offset;

		}

		public void CircleTargetHandling(bool reset = false, bool nextWaypoint = false) {

			if (reset) {

				_circleTargetRequiresReset = true;
				return;
			
			}

			if (nextWaypoint) {

				_circleTargetNextWaypoint = true;
				return;
			
			}

			//Check Gravity
			if (InGravity() != _circleTargetGravity) {

				_circleTargetGravity = !_circleTargetGravity;
				_circleTargetRequiresReset = true;


			}

			//Check Matrix in Gravity
			if (_circleTargetGravity && !_circleTargetRequiresReset) {

				if (_circleTargetMatrix == MatrixD.Identity) {

					_circleTargetRequiresReset = true;

				} else {

					if (VectorHelper.GetAngleBetweenDirections(_circleTargetMatrix.Up, _upDirection) > Data.CircleTargetUpAngleLimit) {

						_circleTargetRequiresReset = true;

					}
				
				}
			
			}

			var targetCoords = Targeting.HasTarget() ? Targeting.Target.GetPosition() : _myPosition;
			
			//Reset
			if (_circleTargetRequiresReset) {

				//MyVisualScriptLogicProvider.ShowNotificationToAll("CT-Reset", 500);
				_circleTargetRequiresReset = false;
				_circleTargetMatrix = MatrixD.Identity;
				_circleTargetTotalDistanceDecrement = 0;
				_circleTargetTotalAltitudeDecrement = 0;
				RecalculateCircleTargetDistanceAndAltitude();

				if (_circleTargetGravity && CurrentPlanet?.Planet != null) {

					var upAtTarget = CurrentPlanet.UpAtPosition(targetCoords);
					_circleTargetMatrix = MatrixD.CreateWorld(targetCoords, Vector3D.CalculatePerpendicularVector(upAtTarget), upAtTarget);

				} else {

					_circleTargetMatrix = MatrixD.CreateWorld(targetCoords, Vector3D.Forward, Vector3D.Up);

				}

				_circleTargetDirections.Clear();
				VectorHelper.GenerateCircularDirections(_circleTargetMatrix, _circleTargetDirections);
				_circleTargetCurrentIndex = VectorHelper.GetClosestDirectionIndexFromList((targetCoords == _myPosition ? Vector3D.Forward : Vector3D.Normalize(_myPosition - targetCoords)), _circleTargetDirections, _circleTargetGravity ? 0 : 90);

			}

			//Prepare Next Waypoint
			if (_circleTargetNextWaypoint) {

				//MyVisualScriptLogicProvider.ShowNotificationToAll("CT-Next", 500);
				_circleTargetNextWaypoint = false;

				if (Data.CircleTargetRadiusConstriction)
					_circleTargetTotalDistanceDecrement += Data.CircleTargetRadiusConstrictionAmount;

				if (Data.CircleTargetAltitudeConstriction)
					_circleTargetTotalAltitudeDecrement += Data.CircleTargetAltitudeConstrictionAmount;

				if (Data.CircleTargetClockwise)
					_circleTargetCurrentIndex = MathTools.NextIndex(_circleTargetCurrentIndex, _circleTargetDirections.Count);
				else
					_circleTargetCurrentIndex = MathTools.PreviousIndex(_circleTargetCurrentIndex, _circleTargetDirections.Count);


			}

			//Generate Waypoint
			var posOrigin = _myPosition != targetCoords ? targetCoords : _circleTargetMatrix.Translation;
			var altitudeAdjustedOrigin = posOrigin;
			double distance = 0;

			if (CurrentPlanet?.Planet != null) {

				var up = CurrentPlanet.UpAtPosition(posOrigin);
				var altitude = MathHelper.Clamp(_circleTargetAltitude - _circleTargetTotalAltitudeDecrement, Data.OffsetPlanetMinTargetAltitude, Data.OffsetPlanetMaxTargetAltitude);
				distance = MathHelper.Clamp(_circleTargetDistance - _circleTargetTotalDistanceDecrement, Data.OffsetPlanetMinDistFromTarget, Data.OffsetPlanetMaxDistFromTarget);
				altitudeAdjustedOrigin = up * altitude + posOrigin;

			} else {

				distance = MathHelper.Clamp(_circleTargetDistance - _circleTargetTotalDistanceDecrement, Data.OffsetSpaceMinDistFromTarget, Data.OffsetSpaceMaxDistFromTarget);

			}

			_calculatedCircleTargetWaypoint = _circleTargetDirections[_circleTargetCurrentIndex] * distance + altitudeAdjustedOrigin;
			_pendingWaypoint = _calculatedCircleTargetWaypoint;
			IndirectWaypointType |= WaypointModificationEnum.CircleTarget;

		}

		private void RecalculateCircleTargetDistanceAndAltitude(bool decrement = false) {

			if (_circleTargetGravity && CurrentPlanet?.Planet != null) {

				bool reverseDistAlt = false;

				if (Data.ReverseOffsetDistAltAboveHeight) {

					var surfaceAtWaypoint = CurrentPlanet.SurfaceCoordsAtPosition(_pendingWaypoint);
					reverseDistAlt = Vector3D.Distance(surfaceAtWaypoint, _pendingWaypoint) > Data.ReverseOffsetHeight;

				}

				if (reverseDistAlt) {

					_circleTargetAltitude = Data.CircleTargetAltitudeConstriction ? Data.OffsetPlanetMaxDistFromTarget : MathTools.RandomBetween(Data.OffsetPlanetMinDistFromTarget, Data.OffsetPlanetMaxDistFromTarget);
					_circleTargetDistance = Data.CircleTargetRadiusConstriction ? Data.OffsetPlanetMaxTargetAltitude : MathTools.RandomBetween(Data.OffsetPlanetMinTargetAltitude, Data.OffsetPlanetMaxTargetAltitude);

				} else {

					_circleTargetAltitude = Data.CircleTargetAltitudeConstriction ? Data.OffsetPlanetMaxTargetAltitude : MathTools.RandomBetween(Data.OffsetPlanetMinTargetAltitude, Data.OffsetPlanetMaxTargetAltitude);
					_circleTargetDistance = Data.CircleTargetRadiusConstriction ? Data.OffsetPlanetMaxDistFromTarget : MathTools.RandomBetween(Data.OffsetPlanetMinDistFromTarget, Data.OffsetPlanetMaxDistFromTarget);

				}

			} else {

				_circleTargetAltitude = Data.CircleTargetRadiusConstriction ? Data.OffsetSpaceMaxDistFromTarget : MathTools.RandomBetween(Data.OffsetSpaceMinDistFromTarget, Data.OffsetSpaceMaxDistFromTarget);
				_circleTargetDistance = Data.CircleTargetRadiusConstriction ? Data.OffsetSpaceMaxDistFromTarget : MathTools.RandomBetween(Data.OffsetSpaceMinDistFromTarget, Data.OffsetSpaceMaxDistFromTarget);

			}

		}

		private void CalculateSafePlanetPathWaypoint(PlanetEntity planet) {

			Vector3D planetPosition = planet.Center();

			var angleBetweenWaypoint = VectorHelper.GetAngleBetweenDirections(_upDirection, Vector3D.Normalize(_pendingWaypoint - planetPosition));

			//Planet Circumnavigation Safety Stuff
			if (angleBetweenWaypoint > 45) {
			
				var directionFromTarget = Vector3D.Normalize(_myPosition - _pendingWaypoint);
				var lineFromTarget = directionFromTarget * (Vector3D.Distance(_pendingWaypoint, _myPosition) * 0.8) + _pendingWaypoint;
				var surfaceAtLineTermination = CurrentPlanet.SurfaceCoordsAtPosition(lineFromTarget);
				_pendingWaypoint = Vector3D.Normalize(surfaceAtLineTermination - planetPosition) * Data.IdealPlanetAltitude + surfaceAtLineTermination;

			}

			Vector3D directionToTarget = Vector3D.Normalize(_pendingWaypoint - _myPosition);
			double distanceToTarget = Vector3D.Distance(_pendingWaypoint, _myPosition);

			double requiredAltitude = _requiresClimbToIdealAltitude ? this.Data.IdealPlanetAltitude : this.Data.MinimumPlanetAltitude;
			

			Vector3D mySurfaceCoords = CurrentPlanet.SurfaceCoordsAtPosition(_myPosition);
			Vector3D waypointSurfaceCoords = CurrentPlanet.SurfaceCoordsAtPosition(_pendingWaypoint);

			double myAltitude = Vector3D.Distance(_myPosition, mySurfaceCoords);
			double waypointAltitude = Vector3D.Distance(_pendingWaypoint, waypointSurfaceCoords);

			double myCoreDistance = Vector3D.Distance(_myPosition, planetPosition);
			double waypointCoreDistance = Vector3D.Distance(_pendingWaypoint, planetPosition);

			List<Vector3D> stepsList = GetPlanetPathSteps(_myPosition, directionToTarget, distanceToTarget);

			Vector3D highestTerrainPoint = Vector3D.Zero;
			double highestTerrainCoreDistance = 0;

			foreach (Vector3D pathPoint in stepsList) {

				Vector3D surfacePathPoint = CurrentPlanet.SurfaceCoordsAtPosition(pathPoint);
				double surfaceCoreDistance = Vector3D.Distance(surfacePathPoint, planetPosition);

				if (surfaceCoreDistance >= highestTerrainCoreDistance) {

					highestTerrainPoint = surfacePathPoint;
					highestTerrainCoreDistance = surfaceCoreDistance;

				}

			}

			double myAltitudeDifferenceFromHighestTerrain = myCoreDistance - highestTerrainCoreDistance;
			double waypointAltitudeDifferenceFromHighestTerrain = waypointCoreDistance - highestTerrainCoreDistance;

			//Terrain Higher Than Me
			if (myAltitudeDifferenceFromHighestTerrain < this.Data.MinimumPlanetAltitude) {

				//BehaviorLogger.Write("Planet Pathing: Terrain Higher Than NPC", BehaviorDebugEnum.Dev);
				IndirectWaypointType |= WaypointModificationEnum.PlanetPathingAscend;
				_requiresClimbToIdealAltitude = true;
				_pendingWaypoint = GetCoordsAboveHighestTerrain(planetPosition, directionToTarget, highestTerrainCoreDistance);
				_calculatedPlanetPathWaypoint = _pendingWaypoint;
				return;

			}

			//Check if Climb is still required
			if (_requiresClimbToIdealAltitude) {

				if (CheckAltitudeTolerance(myAltitudeDifferenceFromHighestTerrain, this.Data.IdealPlanetAltitude, this.Data.AltitudeTolerance)) {

					_requiresClimbToIdealAltitude = false;

				} else {

					_pendingWaypoint = GetCoordsAboveHighestTerrain(planetPosition, directionToTarget, highestTerrainCoreDistance);
					_calculatedPlanetPathWaypoint = _pendingWaypoint;
					IndirectWaypointType |= WaypointModificationEnum.PlanetPathingAscend;
					return;

				}

			}

			//No Obstruction Case
			if (waypointAltitudeDifferenceFromHighestTerrain >= this.Data.MinimumPlanetAltitude) {

				BehaviorLogger.Write("Planet Pathing: No Obstruction", BehaviorDebugEnum.AutoPilot);
				_calculatedPlanetPathWaypoint = _pendingWaypoint;
				return;

			}

			//Terrain Higher Than NPC
			Vector3D waypointCoreDirection = Vector3D.Normalize(_pendingWaypoint - planetPosition);
			_pendingWaypoint = waypointCoreDirection * (highestTerrainCoreDistance + waypointAltitude) + planetPosition;
			_calculatedPlanetPathWaypoint = _pendingWaypoint;
			BehaviorLogger.Write("Planet Pathing: Terrain Higher Than Target " + waypointAltitudeDifferenceFromHighestTerrain.ToString(), BehaviorDebugEnum.AutoPilot); ;

		}

		private void CalculateRoverPath(PlanetEntity planet) {

			/*
			BehaviorLogger.Write("Calculating Hover Path", BehaviorDebugEnum.AutoPilot);
			var targetDir = Vector3D.Normalize(_pendingWaypoint - _myPosition);
			var core = planet.Center();

			//My Position
			var mySurface = CurrentPlanet.SurfaceCoordsAtPosition(_myPosition);
			var myAltitude = Vector3D.Distance(_myPosition, mySurface);
			var myCoreAltitude = Vector3D.Distance(_myPosition, core);

			//Step Ahead
			
			var stepAheadSurface = CurrentPlanet.SurfaceCoordsAtPosition(targetDir * Data.HoverPathStepDistance + _myPosition);
			var stepAheadCoords = _upDirection * Data.IdealPlanetAltitude + stepAheadSurface;
			var stepAheadUpAngle = VectorHelper.GetAngleBetweenDirections(_upDirection, Vector3D.Normalize(stepAheadCoords - _myPosition));
			var stepAheadCoreDistance = Vector3D.Distance(stepAheadCoords, core);

			//Cliff Check
			var cliffCheckSurface = CurrentPlanet.SurfaceCoordsAtPosition(targetDir * Data.HoverPathStepDistance + targetDir * (Data.HoverPathStepDistance * 4) + _myPosition);
			var cliffCheckCoords = _upDirection * Data.IdealPlanetAltitude + cliffCheckSurface;
			var cliffCheckUpAngle = VectorHelper.GetAngleBetweenDirections(_upDirection, Vector3D.Normalize(cliffCheckCoords - _myPosition));
			var cliffCheckCoreDistance = Vector3D.Distance(cliffCheckCoords, core);

			
			//Step Altitude Checks
			bool isStepHigher = stepAheadCoreDistance - myCoreAltitude > 0;
			bool isCliffHigher = cliffCheckCoreDistance - myCoreAltitude > 0;

			//Cliff Safety Check
			MyVisualScriptLogicProvider.ShowNotificationToAll($"Angle: {cliffCheckUpAngle}", 500, "green");
			
			if (cliffCheckUpAngle <= Data.HoverCliffAngle) {
				MyVisualScriptLogicProvider.ShowNotificationToAll("Cliff Safety Check", 500, "Red");

				CurrentMode |= NewAutoPilotMode.RoverNavigation;

				return;

			}

			if(!CurrentMode.HasFlag(NewAutoPilotMode.RoverNavigation))
				_pendingWaypoint = stepAheadCoords;
			*/
		}
		private void CalculateHoverPath(PlanetEntity planet)
		{

			BehaviorLogger.Write("Calculating Hover Path", BehaviorDebugEnum.AutoPilot);
			var targetDir = Vector3D.Normalize(_pendingWaypoint - _myPosition);
			var core = planet.Center();

			//My Position
			var mySurface = CurrentPlanet.SurfaceCoordsAtPosition(_myPosition);
			var myAltitude = Vector3D.Distance(_myPosition, mySurface);
			var myCoreAltitude = Vector3D.Distance(_myPosition, core);

			//Step Ahead

			var stepAheadSurface = CurrentPlanet.SurfaceCoordsAtPosition(targetDir * Data.HoverPathStepDistance + _myPosition);
			var stepAheadCoords = _upDirection * Data.IdealPlanetAltitude + stepAheadSurface;
			var stepAheadUpAngle = VectorHelper.GetAngleBetweenDirections(_upDirection, Vector3D.Normalize(stepAheadCoords - _myPosition));
			var stepAheadCoreDistance = Vector3D.Distance(stepAheadCoords, core);

			//Cliff Check
			var cliffCheckSurface = CurrentPlanet.SurfaceCoordsAtPosition(targetDir * Data.HoverPathStepDistance + targetDir * (Data.HoverPathStepDistance * 4) + _myPosition);
			var cliffCheckCoords = _upDirection * Data.IdealPlanetAltitude + cliffCheckSurface;
			var cliffCheckUpAngle = VectorHelper.GetAngleBetweenDirections(_upDirection, Vector3D.Normalize(cliffCheckCoords - _myPosition));
			var cliffCheckCoreDistance = Vector3D.Distance(cliffCheckCoords, core);

			//Step Altitude Checks
			bool isStepHigher = stepAheadCoreDistance - myCoreAltitude > 0;
			bool isCliffHigher = cliffCheckCoreDistance - myCoreAltitude > 0;

			//Cliff Safety Check
			if (isCliffHigher && cliffCheckUpAngle <= Data.HoverCliffAngle)
			{

				_pendingWaypoint = Vector3D.Normalize(stepAheadSurface - core) * cliffCheckCoreDistance + core;
				IndirectWaypointType |= WaypointModificationEnum.PlanetPathingAscend;
				return;

			}

			_pendingWaypoint = stepAheadCoords;

		}
		private void CalculateAllowedGravity(PlanetEntity planet) {

			if ((Data.MinGravity < 0 && Data.MaxGravity < 0) || Data.MaxGravity < Data.MinGravity)
				return;

			var gravity = planet?.Gravity;

			if (gravity == null)
				return;

			var minDistance = Data.MinGravity >= 0 ? MathTools.GravityToDistance(Data.MinGravity, planet.Planet.Generator.SurfaceGravity, planet.Planet.Generator.GravityFalloffPower, planet.Planet.MinimumRadius, planet.Planet.MaximumRadius) : -1;
			var maxDistance = Data.MaxGravity >= 0 ? MathTools.GravityToDistance(Data.MaxGravity, planet.Planet.Generator.SurfaceGravity, planet.Planet.Generator.GravityFalloffPower, planet.Planet.MinimumRadius, planet.Planet.MaximumRadius) : -1;
			var currentDistance = Vector3D.Distance(_pendingWaypoint, planet.Center());
			var up = Vector3D.Normalize(_pendingWaypoint - planet.Center());
			var surfaceCoords = CurrentPlanet.SurfaceCoordsAtPosition(_pendingWaypoint);
			var surfaceCoreDistance = Vector3D.Distance(surfaceCoords, planet.Center());
			var newCoords = Vector3D.Zero;

			//Adjust Minimum
			if (minDistance > 0 && currentDistance < minDistance) {

				newCoords = up * minDistance + planet.Center();

				if (Vector3D.Distance(newCoords, planet.Center()) - surfaceCoreDistance < Data.MinimumPlanetAltitude)
					newCoords = up * Data.MinimumPlanetAltitude + surfaceCoords;

				currentDistance = Vector3D.Distance(newCoords, planet.Center());

			}

			//Adjust Maximum
			if (maxDistance > 0 && currentDistance > maxDistance) {

				newCoords = up * maxDistance + planet.Center();

				if (Vector3D.Distance(newCoords, planet.Center()) - surfaceCoreDistance < Data.MinimumPlanetAltitude)
					newCoords = up * Data.MinimumPlanetAltitude + surfaceCoords;

				currentDistance = Vector3D.Distance(newCoords, planet.Center());

			}

			//Change Pending
			if (newCoords != Vector3D.Zero)
				_pendingWaypoint = newCoords;

		}

		private void SetMaxAltitude(MyPlanet planet) {

			if (!Data.UseSurfaceHoverThrustMode || planet == null || IndirectWaypointType.HasFlag(WaypointModificationEnum.PlanetPathingAscend))
				return;

			var surfaceCoords = CurrentPlanet.SurfaceCoordsAtPosition(_myPosition); ;
			var mySurfaceDistance = Vector3D.Distance(_myPosition, surfaceCoords);

			var targetDir = Vector3D.Normalize(_pendingWaypoint - _myPosition);
			var projectedCoords = targetDir * 100 + _myPosition;
			var projectedSurface = CurrentPlanet.SurfaceCoordsAtPosition(projectedCoords);
			var projectedSurfaceDist = Vector3D.Distance(projectedCoords, projectedSurface);

			if (mySurfaceDistance > Data.IdealPlanetAltitude + Data.AltitudeTolerance && projectedSurfaceDist > Data.IdealPlanetAltitude + Data.AltitudeTolerance)
				_pendingWaypoint = Vector3D.Normalize(_myPosition - surfaceCoords) * Data.IdealPlanetAltitude + surfaceCoords;

		}

		private Vector3D CalculateWaterPath() {

			if (CurrentPlanet == null)
				return _pendingWaypoint;

			bool isTargetOnOrAboveWater = false;
			Vector3D waterCoordsAtTarget = CurrentPlanet.SurfaceCoordsAtPosition(_pendingWaypoint);


			return _pendingWaypoint;

		}

		private Vector3D GetCoordsAboveHighestTerrain(Vector3D planetPosition, Vector3D directionToTarget, double highestTerrainDistanceFromCore) {

			//Get position 50m in direction of target
			var roughForwardStep = directionToTarget * 50 + _myPosition;

			var upDirectionFromStep = Vector3D.Normalize(roughForwardStep - planetPosition);
			return upDirectionFromStep * (highestTerrainDistanceFromCore + this.Data.IdealPlanetAltitude) + planetPosition;

		}

		public List<Vector3D> GetPlanetPathSteps(Vector3D startCoords, Vector3D directionToTarget, double distanceToTarget, bool overrideMaxDistance = false) {

			var distanceToUse = MathHelper.Clamp(distanceToTarget, 0, overrideMaxDistance ? distanceToTarget : this.Data.MaxPlanetPathCheckDistance);
			var result = new List<Vector3D>();
			double currentPathDistance = 0;

			while (currentPathDistance < distanceToUse) {

				if ((distanceToUse - currentPathDistance) < 50) {

					currentPathDistance = distanceToUse;

				} else {

					currentPathDistance += 50;

				}

				result.Add(directionToTarget * currentPathDistance + startCoords);

			}

			return result;

		}

		private bool CheckAltitudeTolerance(double currentCoreDistance, double targetCoreDistance, double tolerance) {

			if (currentCoreDistance < targetCoreDistance - tolerance || currentCoreDistance > targetCoreDistance + tolerance)
				return false;

			return true;

		}

		public void SetRandomOffset(IMyEntity entity = null, bool altitudeIsMinimum = false) {

			double distance = 0;
			double altitude = 0;

			if (_gravityStrength > 0) {

				distance = VectorHelper.RandomDistance(this.Data.OffsetPlanetMinDistFromTarget, this.Data.OffsetPlanetMaxDistFromTarget);
				altitude = VectorHelper.RandomDistance(this.Data.OffsetPlanetMinTargetAltitude, this.Data.OffsetPlanetMaxTargetAltitude);

			} else {

				distance = VectorHelper.RandomDistance(this.Data.OffsetSpaceMinDistFromTarget, this.Data.OffsetSpaceMaxDistFromTarget);

			}

			SetRandomOffset(distance, altitude, entity, altitudeIsMinimum);

		}

		public void SetRandomOffset(double distance, double altitude, IMyEntity entity, bool altitudeIsMinimum = false) {

			_offsetType = (entity == null) ? WaypointOffsetType.RandomOffsetFixed : WaypointOffsetType.RandomOffsetRelativeEntity;
			_offsetAltitudeIsMinimum = altitudeIsMinimum;
			_offsetRelativeEntity = entity;
			_offsetRequiresCalculation = true;
			_offsetDistanceFromTarget = distance;
			_offsetAltitudeFromTarget = altitude;

		}

		public void ReverseOffsetDirection(double minSafeAngle = 80) {

			if (InGravity()) {

				if (VectorHelper.GetAngleBetweenDirections(-_offsetDirection, _upDirection) < minSafeAngle)
					return;
			
			}

			_offsetDirection *= -1;


		}

		public bool ArrivedAtOffsetWaypoint() {

			if (InGravity() && MyAltitude < Data.IdealPlanetAltitude) {

				if (DistanceToWaypointAtMyAltitude == -1 || DistanceToOffsetAtMyAltitude == -1)
					return false;

				if (DistanceToWaypointAtMyAltitude < Data.WaypointTolerance && DistanceToOffsetAtMyAltitude < Data.WaypointTolerance) {

					BehaviorLogger.Write("Offset Compensation", BehaviorDebugEnum.AutoPilot);
					return true;

				}

				return false;

			}

			if (DistanceToCurrentWaypoint < Data.WaypointTolerance)
				return true;

			return false;

		}

		public bool ArrivedAtCircleTargetWaypoint() {

			if (Data.CircleTargetDriftExemption && _circleTargetDirections.Count > 0) {

				var targetCoords = Targeting.HasTarget() ? Targeting.Target.GetPosition() : _circleTargetMatrix.Translation;
				var dir = Vector3D.Normalize(_myPosition - targetCoords);
				var nextDir = dir;

				if(Data.CircleTargetClockwise)
					nextDir = _circleTargetDirections[MathTools.NextIndex(_circleTargetCurrentIndex, _circleTargetDirections.Count)];
				else
					nextDir = _circleTargetDirections[MathTools.PreviousIndex(_circleTargetCurrentIndex, _circleTargetDirections.Count)];

				var angle = VectorHelper.GetAngleBetweenDirections(dir, nextDir);

				//MyVisualScriptLogicProvider.ShowNotificationToAll(angle.ToString(), 500);

				if (VectorHelper.GetAngleBetweenDirections(dir, nextDir) <= 45) {

					//MyVisualScriptLogicProvider.ShowNotificationToAll("CT-DriftExempt", 500);
					return true;

				}
			
			}

			if (InGravity() && MyAltitude < Data.IdealPlanetAltitude) {

				if (DistanceToWaypointAtMyAltitude == -1 || DistanceToOffsetAtMyAltitude == -1)
					return false;

				if (DistanceToWaypointAtMyAltitude < Data.WaypointTolerance && DistanceToCircleTargetAtMyAltitude < Data.WaypointTolerance) {

					BehaviorLogger.Write("Offset Compensation", BehaviorDebugEnum.AutoPilot);
					return true;

				}

				return false;

			}

			if (DistanceToCurrentWaypoint < Data.WaypointTolerance)
				return true;

			return false;

		}
		public bool InvalidTarget() {

			return !Targeting.HasTarget();

		}

		public Vector3D GetCurrentWaypoint() {

			return _currentWaypoint;
		
		}

		public Vector3D GetPendingWaypoint() {

			return _pendingWaypoint;

		}

		public PlanetEntity GetCurrentPlanet() {

			return CurrentPlanet;
		
		}

		public Vector3D CalculateDespawnCoords(Vector3D coords) {

			var distance = MathTools.RandomBetween(Data.DespawnCoordsMinDistance, Data.DespawnCoordsMaxDistance);

			if (InGravity()) {

				BehaviorLogger.Write("Manually Created Despawn Coords in Gravity", BehaviorDebugEnum.BehaviorSpecific);
				var center = CurrentPlanet.Center();
				var up = Vector3D.Normalize(coords - center);
				var forward = MyUtils.GetRandomPerpendicularVector(ref up);
				var surfaceCoordsPos = CurrentPlanet.SurfaceCoordsAtPosition(coords);
				var surfaceMatrix = MatrixD.CreateWorld(surfaceCoordsPos, forward, up);
				var surfaceDistanceCore = Vector3D.DistanceSquared(surfaceCoordsPos, center);
				Vector3D direction = Vector3D.Zero;
				double distDifference = -1;

				var forwardDespawnCoreDistDifference = Math.Abs(surfaceDistanceCore - GetCoreDistanceFromPotentialDespawn(surfaceMatrix, surfaceMatrix.Forward, distance, center));
				
				if (distDifference == -1) {

					distDifference = forwardDespawnCoreDistDifference;
					direction = surfaceMatrix.Forward;

				}

				var backDespawnCoreDistDifference = Math.Abs(surfaceDistanceCore - GetCoreDistanceFromPotentialDespawn(surfaceMatrix, surfaceMatrix.Backward, distance, center));

				if (backDespawnCoreDistDifference < distDifference) {

					distDifference = forwardDespawnCoreDistDifference;
					direction = surfaceMatrix.Backward;

				}

				var leftDespawnCoreDistDifference = Math.Abs(surfaceDistanceCore - GetCoreDistanceFromPotentialDespawn(surfaceMatrix, surfaceMatrix.Left, distance, center));

				if (leftDespawnCoreDistDifference < distDifference) {

					distDifference = forwardDespawnCoreDistDifference;
					direction = surfaceMatrix.Left;

				}

				var rightDespawnCoreDistDifference = Math.Abs(surfaceDistanceCore - GetCoreDistanceFromPotentialDespawn(surfaceMatrix, surfaceMatrix.Right, distance, center));

				if (rightDespawnCoreDistDifference < distDifference) {

					distDifference = forwardDespawnCoreDistDifference;
					direction = surfaceMatrix.Right;

				}

				var finalRoughCoords = direction * distance + surfaceCoordsPos;
				var finalSurfaceCoords = CurrentPlanet.SurfaceCoordsAtPosition(finalRoughCoords);
				var finalUp = Vector3D.Normalize(finalSurfaceCoords - center);
				return finalUp * MathTools.RandomBetween(Data.DespawnCoordsMinAltitude, Data.DespawnCoordsMaxAltitude) + finalSurfaceCoords;

			} else {

				BehaviorLogger.Write("Manually Created Despawn Coords in Space", BehaviorDebugEnum.BehaviorSpecific);
				var randomDir = Vector3D.Normalize(MyUtils.GetRandomVector3D());
				Vector3D result = randomDir * distance + coords;

				if (SpaceDespawnInsideGravity(result)) {

					result = -randomDir * distance + coords;

					if(SpaceDespawnInsideGravity(result)){
					
						result = Vector3D.CalculatePerpendicularVector(randomDir) + coords;

					}

				}

				return result;
			
			}

		}

		private bool SpaceDespawnInsideGravity(Vector3D coords) {

			var planet = MyGamePruningStructure.GetClosestPlanet(coords);

			if (planet != null) {

				var gravityProvider = planet?.Components?.Get<MyGravityProviderComponent>();

				if (gravityProvider != null && gravityProvider.IsPositionInRange(coords))
					return true;

			}

			return false;

		}

		private double GetCoreDistanceFromPotentialDespawn(MatrixD matrix, Vector3D direction, double distance, Vector3D center) {

			var roughCoords = direction * distance + matrix.Translation;
			var surfaceCoords = CurrentPlanet.SurfaceCoordsAtPosition(roughCoords);
			return Vector3D.DistanceSquared(surfaceCoords, center);

		}

		public bool IsAvoidingCollision() {

			return _requiresNavigationAroundCollision;
		
		}

		public bool IsWaypointThroughVelocityCollision(int timeToCollision = -1, CollisionType type = CollisionType.None) {

			if (!Collision.VelocityResult.CollisionImminent(timeToCollision) || Collision.VelocityResult.Type == CollisionType.None)
				return false;

			if (Collision.VelocityResult.Type != type && type != CollisionType.None)
				return false;

			if (DistanceToCurrentWaypoint > Collision.VelocityResult.GetCollisionDistance() && VectorHelper.GetAngleBetweenDirections(Collision.VelocityResult.DirectionVector, Vector3D.Normalize(_currentWaypoint - _myPosition)) < 15)
				return true;

			return false;
		
		}

		public void SetAutoPilotDataMode(AutoPilotDataMode mode) {

			State.DataMode = mode;
			State.UseFlyLevelWithGravity = Data.FlyLevelWithGravity;
			State.UseFlyLevelWithGravityIdle = Data.LevelWithGravityWhenIdle;
			ActivateAutoPilot(_initialWaypoint, State.NormalAutopilotFlags);
		
		}

		public void AssignAutoPilotDataMode(string profileId, AutoPilotDataMode mode) {

			var autoPilotProfile = ProfileManager.GetAutopilotProfile(profileId);

			if (autoPilotProfile == null)
				return;

			if (mode == AutoPilotDataMode.Primary)
				State.PrimaryAutoPilot = autoPilotProfile;

			if (mode == AutoPilotDataMode.Secondary)
				State.SecondaryAutoPilot = autoPilotProfile;

			if (mode == AutoPilotDataMode.Tertiary)
				State.TertiaryAutoPilot = autoPilotProfile;

		}

		public void ActivateBarrelRoll() {

			BehaviorLogger.Write("Barrel Roll Start", BehaviorDebugEnum.AutoPilot);
			_applyBarrelRoll = true;
			_barrelRollStart = MyAPIGateway.Session.GameDateTime;
			_barrelRollDuration = MathTools.RandomBetween(Data.BarrelRollMinDurationMs, Data.BarrelRollMaxDurationMs);
		
		}

		public void ActivateHeavyYaw() {

			BehaviorLogger.Write("Heavy Yaw Start", BehaviorDebugEnum.AutoPilot);
			_applyHeavyYaw = true;
			_heavyYawStart = MyAPIGateway.Session.GameDateTime;
			_heavyYawDuration = MathTools.RandomBetween(Data.BarrelRollMinDurationMs, Data.BarrelRollMaxDurationMs);

		}

		public void ActivateRamming() {

			_applyRamming = true;
			this.Strafing = false;
			_ramStart = MyAPIGateway.Session.GameDateTime;
			_ramDuration = MathTools.RandomBetween(Data.RamMinDurationMs, Data.RamMaxDurationMs);
			BehaviorLogger.Write("Ramming Start. Dur: " + _ramDuration, BehaviorDebugEnum.AutoPilot);

		}

		public void DebugDrawingToWaypoints() {

			if (MyAPIGateway.Utilities.IsDedicated)
				return;

			Vector4 colorRed = ConvertColor(Color.Red);
			Vector4 colorOrange = ConvertColor(Color.Orange);
			Vector4 colorYellow = ConvertColor(Color.Yellow);
			Vector4 colorGreen = ConvertColor(Color.Green);
			Vector4 colorCyan = ConvertColor(Color.Cyan);
			Vector4 colorMajenta = ConvertColor(Color.Magenta);

			//MySimpleObjectDraw.DrawLine(_initialWaypoint, _offsetDirection * 5 + _initialWaypoint, MyStringId.GetOrCompute("WeaponLaser"), ref colorRed, 1.1f);



			if (_currentWaypoint != Vector3D.Zero) {

				MySimpleObjectDraw.DrawLine(_myPosition, _currentWaypoint, MyStringId.GetOrCompute("WeaponLaser"), ref colorGreen, 1.1f);
				WaterPath.DrawCurrentPath();
				RoverPath.DrawCurrentPath();
			}
			
			 if (_evadeWaypoint != Vector3D.Zero) {

				MySimpleObjectDraw.DrawLine(_myPosition + new Vector3D(1.5, 1.5, 0), _evadeWaypoint + new Vector3D(1.5, 1.5, 0), MyStringId.GetOrCompute("WeaponLaser"), ref colorRed, 1.1f);
				MySimpleObjectDraw.DrawLine(_evadeFromWaypoint + new Vector3D(-1.5, -1.5, 0), _evadeWaypoint + new Vector3D(-1.5, -1.5, 0), MyStringId.GetOrCompute("WeaponLaser"), ref colorOrange, 1.1f);

			}
			if (_calculatedOffsetWaypoint != Vector3D.Zero) {

				MySimpleObjectDraw.DrawLine(_myPosition + new Vector3D(1.5, 0, 1.5), _calculatedOffsetWaypoint + new Vector3D(1.5, 0, 1.5), MyStringId.GetOrCompute("WeaponLaser"), ref colorCyan, 1.1f);

			}

			if (_calculatedPlanetPathWaypoint != Vector3D.Zero) {

				MySimpleObjectDraw.DrawLine(_myPosition + new Vector3D(-1.5, 0, -1.5), _calculatedPlanetPathWaypoint + new Vector3D(-1.5, 0, -1.5), MyStringId.GetOrCompute("WeaponLaser"), ref colorMajenta, 1.1f);

			}

			if (_calculatedWeaponPredictionWaypoint != Vector3D.Zero) {

				MySimpleObjectDraw.DrawLine(_myPosition + new Vector3D(1.5, 1.5, 1.5), _calculatedWeaponPredictionWaypoint + new Vector3D(1.5, 1.5, 1.5), MyStringId.GetOrCompute("WeaponLaser"), ref colorYellow, 1.1f);

			}

			//Collisions
			
			if (Collision.ForwardResult.Type != CollisionType.None) {

				MySimpleObjectDraw.DrawLine(_myPosition, Collision.ForwardResult.GetCollisionCoords(), MyStringId.GetOrCompute("WeaponLaser"), ref colorRed, 1.1f);

			}

			if (Collision.BackwardResult.Type != CollisionType.None) {

				MySimpleObjectDraw.DrawLine(_myPosition, Collision.BackwardResult.GetCollisionCoords(), MyStringId.GetOrCompute("WeaponLaser"), ref colorRed, 1.1f);

			}

			if (Collision.LeftResult.Type != CollisionType.None) {

				MySimpleObjectDraw.DrawLine(_myPosition, Collision.LeftResult.GetCollisionCoords(), MyStringId.GetOrCompute("WeaponLaser"), ref colorRed, 1.1f);

			}

			if (Collision.RightResult.Type != CollisionType.None) {

				MySimpleObjectDraw.DrawLine(_myPosition, Collision.RightResult.GetCollisionCoords(), MyStringId.GetOrCompute("WeaponLaser"), ref colorRed, 1.1f);

			}

			if (Collision.UpResult.Type != CollisionType.None) {

				MySimpleObjectDraw.DrawLine(_myPosition, Collision.UpResult.GetCollisionCoords(), MyStringId.GetOrCompute("WeaponLaser"), ref colorRed, 1.1f);

			}

			if (Collision.DownResult.Type != CollisionType.None) {

				MySimpleObjectDraw.DrawLine(_myPosition, Collision.DownResult.GetCollisionCoords(), MyStringId.GetOrCompute("WeaponLaser"), ref colorRed, 1.1f);

			}
			
		}

		internal Vector4 ConvertColor(Color color) {

			return new Vector4(color.X / 20, color.Y / 20, color.Z / 20, 0.1f);

		}

	}

}
