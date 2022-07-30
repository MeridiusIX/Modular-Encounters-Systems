using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Helpers;
using Sandbox.ModAPI;
using System;
using VRageMath;

namespace ModularEncountersSystems.Behavior {

	public class Patrol : IBehaviorSubClass{

		DateTime WaitTime;
		IBehavior _behavior;
		public BehaviorSubclass SubClass { get { return _subClass; } set { _subClass = value; } }
		private BehaviorSubclass _subClass;
		private NewAutoPilotMode WaterNav { get { return (_behavior.AutoPilot.Data.UseWaterPatrolMode ? NewAutoPilotMode.WaterNavigation : NewAutoPilotMode.None); } }
		private DateTime _waypointWaitTime;
		private DateTime _waypointAbandonTime;

		public string DefaultWeaponProfile { get { return _defaultWeaponProfile; } }
		private string _defaultWeaponProfile;

		public Patrol(IBehavior behavior) {

			_subClass = BehaviorSubclass.Patrol;
			_behavior = behavior;
			_waypointWaitTime = MyAPIGateway.Session.GameDateTime;
			_waypointAbandonTime = MyAPIGateway.Session.GameDateTime;
			_defaultWeaponProfile = "MES-Weapons-GenericStandard";
			WaitTime = MyAPIGateway.Session.GameDateTime;

		}

		public void ProcessBehavior() {

			if(MES_SessionCore.IsServer == false) {

				return;

			}

			//Logger.Write(Mode.ToString(), BehaviorDebugEnum.General);

			if (_behavior.Mode != BehaviorMode.Retreat && _behavior.BehaviorSettings.DoRetreat == true){

				_behavior.ChangeCoreBehaviorMode(BehaviorMode.Retreat);
				_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.LevelWithGravity | NewAutoPilotMode.ThrustForward | WaterNav);

			}

			//Init
			if (_behavior.Mode == BehaviorMode.Init) {

				_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachWaypoint);
				_behavior.AutoPilot.OffsetWaypointGenerator(true);
				_behavior.AutoPilot.ActivateAutoPilot(PatrolCenterCoords(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.OffsetWaypoint | WaterNav, CheckEnum.Yes, CheckEnum.No);

			}

			//WaitAtWaypoint
			if (_behavior.Mode == BehaviorMode.WaitAtWaypoint) {

				var timeSpan = MyAPIGateway.Session.GameDateTime - this._waypointWaitTime;

				if (timeSpan.TotalSeconds >= _behavior.AutoPilot.Data.WaypointWaitTimeTrigger) {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachWaypoint);
					_waypointAbandonTime = MyAPIGateway.Session.GameDateTime;
					_behavior.AutoPilot.OffsetWaypointGenerator(true);
					_behavior.AutoPilot.ActivateAutoPilot(PatrolCenterCoords(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.OffsetWaypoint | WaterNav, CheckEnum.Yes, CheckEnum.No);
					_behavior.BehaviorTriggerB = true;
					return; //Allows Autopilot To Run A Cycle And Regenerate Coordinates So It Doesn't Immediately Wait At Coords;

				}

			}

			//Approach
			if (_behavior.Mode == BehaviorMode.ApproachWaypoint) {

				var timeSpan = MyAPIGateway.Session.GameDateTime - _waypointAbandonTime;
				//Logger.Write("Distance To Waypoint: " + New_behavior.AutoPilot.DistanceToCurrentWaypoint.ToString(), BehaviorDebugEnum.General);

				if (_behavior.AutoPilot.ArrivedAtOffsetWaypoint()) {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitAtWaypoint);
					_waypointWaitTime = MyAPIGateway.Session.GameDateTime;
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.None, CheckEnum.No, CheckEnum.Yes);
					_behavior.BehaviorTriggerA = true;

				} else if (timeSpan.TotalSeconds >= _behavior.AutoPilot.Data.WaypointAbandonTimeTrigger) {

					_waypointAbandonTime = MyAPIGateway.Session.GameDateTime;
					_behavior.AutoPilot.OffsetWaypointGenerator(true);

				} else if (_behavior.AutoPilot.IsWaypointThroughVelocityCollision()) {

					_waypointAbandonTime = MyAPIGateway.Session.GameDateTime;
					_behavior.AutoPilot.OffsetWaypointGenerator(true);

				}

			}

			//Retreat
			if (_behavior.Mode == BehaviorMode.Retreat) {

				if (_behavior.Despawn.NearestPlayer?.Player?.Controller?.ControlledEntity?.Entity != null) {

					//Logger.AddMsg("DespawnCoordsCreated", true);
					_behavior.AutoPilot.SetInitialWaypoint(VectorHelper.GetDirectionAwayFromTarget(_behavior.RemoteControl.GetPosition(), _behavior.Despawn.NearestPlayer.GetPosition()) * 1000 + _behavior.RemoteControl.GetPosition());

				}

			}

		}

		public Vector3D PatrolCenterCoords() {

			if (_behavior.BehaviorSettings.PatrolOverrideLocation != Vector3D.Zero)
				return _behavior.BehaviorSettings.PatrolOverrideLocation;

			if (_behavior.BehaviorSettings.StartCoords == Vector3D.Zero)
				_behavior.BehaviorSettings.StartCoords = _behavior?.CurrentGrid?.Npc.StartCoords ?? _behavior.RemoteControl.GetPosition();

			return _behavior?.BehaviorSettings?.StartCoords ?? Vector3D.Zero;

		}

		public void SetDefaultTags() {

			//Behavior Specific Defaults
			_behavior.AutoPilot.Data = ProfileManager.GetAutopilotProfile("RAI-Generic-Autopilot-Patrol");
			_behavior.Despawn.UseNoTargetTimer = false;
			
			if (string.IsNullOrWhiteSpace(_behavior.BehaviorSettings.WeaponsSystemProfile)) {

				_behavior.BehaviorSettings.WeaponsSystemProfile = _defaultWeaponProfile;
				//_behavior.BehaviorSettings.WeaponsSystemProfile

			}

		}

		public override string ToString() {

			return "";

		}

		public void InitTags() {

			return;

			if(string.IsNullOrWhiteSpace(_behavior.RemoteControl?.CustomData) == false) {

				var descSplit = _behavior.RemoteControl.CustomData.Split('\n');

				foreach(var tag in descSplit) {
					


				}
				
			}

		}

	}

}
	
