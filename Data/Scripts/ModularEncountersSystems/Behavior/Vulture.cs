using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using Sandbox.ModAPI;
using System;
using VRageMath;

namespace ModularEncountersSystems.Behavior {

	public class Vulture : IBehaviorSubClass {

		private IBehavior _behavior;

		public BehaviorSubclass SubClass { get { return _subClass; } set { _subClass = value; } }
		private BehaviorSubclass _subClass;

		public string DefaultWeaponProfile { get { return _defaultWeaponProfile; } }
		private string _defaultWeaponProfile;

		internal DateTime _timeOutsideRadius;

		public Vulture(IBehavior behavior) {

			_subClass = BehaviorSubclass.Vulture;
			_behavior = behavior;

			_defaultWeaponProfile = "MES-Weapons-GenericStandard";

			_timeOutsideRadius = MyAPIGateway.Session.GameDateTime;


		}

		public void ProcessBehavior() {

			if (MES_SessionCore.IsServer == false) {

				return;

			}

			if (_behavior.Mode != BehaviorMode.Retreat && _behavior.BehaviorSettings.DoRetreat == true) {

				_behavior.ChangeCoreBehaviorMode(BehaviorMode.Retreat);
				_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing, CheckEnum.Yes, CheckEnum.No);

			}

			//Init
			if(_behavior.Mode == BehaviorMode.Init) {

				_behavior.AutoPilot.CircleTargetHandling(true);

				if(!_behavior.AutoPilot.Targeting.HasTarget()) {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitingForTarget);
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.CircleTarget, CheckEnum.Yes, CheckEnum.No);

				} else {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.CircleTarget, CheckEnum.Yes, CheckEnum.No);

				}

			}

			//Waiting For Target
			if (_behavior.Mode == BehaviorMode.WaitingForTarget) {

				if (_behavior.AutoPilot.Targeting.HasTarget()) {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					_behavior.AutoPilot.CircleTargetHandling(true);
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.CircleTarget, CheckEnum.Yes, CheckEnum.No);

				} else if (_behavior.Despawn.NoTargetExpire == true) {

					_behavior.Despawn.Retreat();

				} else {

					if (_behavior.AutoPilot.ArrivedAtCircleTargetWaypoint()) {

						_behavior.AutoPilot.CircleTargetHandling(false, true);
						_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.CircleTarget, CheckEnum.Yes, CheckEnum.No);
						_behavior.BehaviorTriggerA = true;

					} else if (_behavior.AutoPilot.IsWaypointThroughVelocityCollision()) {

						_behavior.AutoPilot.CircleTargetHandling(false, true);

					}

				}

			} else if(!_behavior.AutoPilot.Targeting.HasTarget() && _behavior.Mode != BehaviorMode.Retreat) {

				_behavior.AutoPilot.CircleTargetHandling(true);
				_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitingForTarget);
				_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.CircleTarget, CheckEnum.Yes, CheckEnum.No);

			}

			

			//Approach
			if(_behavior.Mode == BehaviorMode.ApproachTarget) {

				//Logger.Write("Distance To Waypoint: " + New_behavior.AutoPilot.DistanceToCurrentWaypoint.ToString(), BehaviorDebugEnum.General);

				//TODO: Add Timeout Thing If Outside Of Circle Radius For Too Long, Reset and Go To Nearest Direction

				if (_behavior.AutoPilot.ArrivedAtCircleTargetWaypoint()) {

					_behavior.AutoPilot.CircleTargetHandling(false, true);
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.CircleTarget, CheckEnum.Yes, CheckEnum.No);
					_behavior.BehaviorTriggerA = true;

				} else if (_behavior.AutoPilot.IsWaypointThroughVelocityCollision()) {

					_behavior.AutoPilot.CircleTargetHandling(false, true);

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

			_behavior.AutoPilot.Data = ProfileManager.GetAutopilotProfile("RAI-Generic-Autopilot-Vulture");
			_behavior.Despawn.UseNoTargetTimer = true;

			if (string.IsNullOrWhiteSpace(_behavior.BehaviorSettings.WeaponsSystemProfile)) {

				_behavior.BehaviorSettings.WeaponsSystemProfile = _defaultWeaponProfile;

			}

		}

		public override string ToString() {

			return "";

		}

		public void InitTags() {

			

		}

	}

}

