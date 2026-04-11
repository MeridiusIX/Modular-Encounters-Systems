using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using Sandbox.ModAPI;
using System;

namespace ModularEncountersSystems.Behavior {

	public class HorseNautical : IBehaviorSubClass {

		public int HorseNauticalWaypointWaitTimeTrigger {

			get {

				return _horsenauticalWaypointWaitTimeTrigger > 0 ? _horsenauticalWaypointWaitTimeTrigger : _behavior.AutoPilot.Data.WaypointWaitTimeTrigger;

			}

			set {

				_horsenauticalWaypointWaitTimeTrigger = value;

			}

		}

		public int HorseNauticalWaypointAbandonTimeTrigger {

			get {

				return _horsenauticalWaypointAbandonTimeTrigger > 0 ? _horsenauticalWaypointAbandonTimeTrigger : _behavior.AutoPilot.Data.WaypointAbandonTimeTrigger;

			}

			set {

				_horsenauticalWaypointAbandonTimeTrigger = value;

			}

		}

		private int _horsenauticalWaypointWaitTimeTrigger;
		private int _horsenauticalWaypointAbandonTimeTrigger;

		public byte Counter;
		public DateTime HorseNauticalWaypointWaitTime;
		public DateTime HorseNauticalWaypointAbandonTime;

		private IBehavior _behavior;

		public BehaviorSubclass SubClass { get { return _subClass; } set { _subClass = value; } }
		private BehaviorSubclass _subClass;

		public string DefaultWeaponProfile { get { return _defaultWeaponProfile; } }
		private string _defaultWeaponProfile;

		public HorseNautical(IBehavior behavior) {

			_subClass = BehaviorSubclass.HorseNautical;
			_behavior = behavior;

			_defaultWeaponProfile = "MES-Weapons-GenericStandard";

			_horsenauticalWaypointWaitTimeTrigger = -1;
			_horsenauticalWaypointAbandonTimeTrigger = -1;

			//HorseNauticalWaypointWaitTimeTrigger = 5;
			//HorseNauticalWaypointAbandonTimeTrigger = 30;

			Counter = 0;
			HorseNauticalWaypointWaitTime = MyAPIGateway.Session.GameDateTime;
			HorseNauticalWaypointAbandonTime = MyAPIGateway.Session.GameDateTime;

		}

		public void ProcessBehavior() {

			if (MES_SessionCore.IsServer == false) {

				return;

			}

			if (_behavior.Mode != BehaviorMode.Retreat && _behavior.BehaviorSettings.DoRetreat == true) {

				_behavior.ChangeCoreBehaviorMode(BehaviorMode.Retreat);
				_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.LevelWithGravity | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.WaterNavigation, CheckEnum.Yes, CheckEnum.No);

			}

			//Init
			if(_behavior.Mode == BehaviorMode.Init) {

				if(!_behavior.AutoPilot.Targeting.HasTarget()) {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitingForTarget);

				} else {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					this.HorseNauticalWaypointWaitTime = MyAPIGateway.Session.GameDateTime;
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.LevelWithGravity | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.WaterNavigation | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.OffsetWaypoint, CheckEnum.Yes, CheckEnum.No);

				}

			}

			//Waiting For Target
			if(_behavior.Mode == BehaviorMode.WaitingForTarget) {

				if(_behavior.AutoPilot.Targeting.HasTarget()) {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					this.HorseNauticalWaypointWaitTime = MyAPIGateway.Session.GameDateTime;
					_behavior.AutoPilot.OffsetWaypointGenerator(true);
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.LevelWithGravity | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.WaterNavigation | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.OffsetWaypoint, CheckEnum.Yes, CheckEnum.No);

				} else if(_behavior.Despawn.NoTargetExpire == true) {

					_behavior.Despawn.Retreat();

				}

			}

			if(!_behavior.AutoPilot.Targeting.HasTarget() && _behavior.Mode != BehaviorMode.Retreat) {

				_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitingForTarget);

			}

			//Approach
			if(_behavior.Mode == BehaviorMode.ApproachTarget) {

				var timeSpan = MyAPIGateway.Session.GameDateTime - this.HorseNauticalWaypointAbandonTime;
				//Logger.Write("Distance To Waypoint: " + New_behavior.AutoPilot.DistanceToCurrentWaypoint.ToString(), BehaviorDebugEnum.General);

				if (_behavior.AutoPilot.ArrivedAtOffsetWaypoint()) {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitAtWaypoint);
					this.HorseNauticalWaypointWaitTime = MyAPIGateway.Session.GameDateTime;
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.LevelWithGravity, CheckEnum.No, CheckEnum.Yes);
					_behavior.BehaviorTriggerA = true;

				} else if (timeSpan.TotalSeconds >= this.HorseNauticalWaypointAbandonTimeTrigger) {

					BehaviorLogger.Write("HorseNautical Timeout, Getting New Offset", BehaviorDebugEnum.General);
					this.HorseNauticalWaypointAbandonTime = MyAPIGateway.Session.GameDateTime;
					_behavior.AutoPilot.OffsetWaypointGenerator(true);

				} else if (_behavior.AutoPilot.IsWaypointThroughVelocityCollision()) {

					BehaviorLogger.Write("HorseNautical Velocity Through Collision, Getting New Offset", BehaviorDebugEnum.General);
					this.HorseNauticalWaypointAbandonTime = MyAPIGateway.Session.GameDateTime;
					_behavior.AutoPilot.OffsetWaypointGenerator(true);

				}

			}

			//WaitAtWaypoint
			if (_behavior.Mode == BehaviorMode.WaitAtWaypoint) {

				var timeSpan = MyAPIGateway.Session.GameDateTime - this.HorseNauticalWaypointWaitTime;

				if (timeSpan.TotalSeconds >= this.HorseNauticalWaypointWaitTimeTrigger) {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					this.HorseNauticalWaypointAbandonTime = MyAPIGateway.Session.GameDateTime;
					_behavior.AutoPilot.OffsetWaypointGenerator(true);
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.LevelWithGravity | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.WaterNavigation | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.OffsetWaypoint, CheckEnum.Yes, CheckEnum.No);
					_behavior.BehaviorTriggerB = true;

				}

			}

			//Retreat
			if (_behavior.Mode == BehaviorMode.Retreat) {

				if (_behavior.Despawn.NearestPlayer?.Player?.Controller?.ControlledEntity?.Entity != null) {

					//Logger.AddMsg("DespawnCoordsCreated", true);
					_behavior.AutoPilot.SetInitialWaypoint(_behavior.Despawn.GetRetreatCoords());

				}

			}

		}

		public void SetDefaultTags() {

			_behavior.AutoPilot.Data = ProfileManager.GetAutopilotProfile("RAI-Generic-Autopilot-Nautical");
			_behavior.Despawn.UseNoTargetTimer = true;

			if (string.IsNullOrWhiteSpace(_behavior.BehaviorSettings.WeaponsSystemProfile)) {

				_behavior.BehaviorSettings.WeaponsSystemProfile = _defaultWeaponProfile;

			}

		}

		public override string ToString() {

			return "";

		}

		public void InitTags() {

			if (string.IsNullOrWhiteSpace(_behavior.RemoteControl?.CustomData) == false) {

				var descSplit = _behavior.RemoteControl.CustomData.Split('\n');

				foreach (var tag in descSplit) {

					//HorseNauticalWaypointWaitTimeTrigger
					if (tag.Contains("[HorseNauticalWaypointWaitTimeTrigger:") == true) {

						TagParse.TagIntCheck(tag, ref _horsenauticalWaypointWaitTimeTrigger);

					}

					//HorseNauticalWaypointAbandonTimeTrigger
					if (tag.Contains("[HorseNauticalWaypointAbandonTimeTrigger:") == true) {

						TagParse.TagIntCheck(tag, ref _horsenauticalWaypointAbandonTimeTrigger);

					}

				}

			}


		}

	}

}

