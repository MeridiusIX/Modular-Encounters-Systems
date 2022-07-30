using Sandbox.ModAPI;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using System;
using VRageMath;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Core;

namespace ModularEncountersSystems.Behavior {

	public class Sniper : IBehaviorSubClass{

		public DateTime SniperWaypointWaitTime;
		public DateTime SniperWaypointAbandonTime;
		private IBehavior _behavior;
		public BehaviorSubclass SubClass { get { return _subClass; } set { _subClass = value; } }
		private BehaviorSubclass _subClass;

		public string DefaultWeaponProfile { get { return _defaultWeaponProfile; } }
		private string _defaultWeaponProfile;

		public Sniper(IBehavior behavior) : base() {

			_subClass = BehaviorSubclass.Sniper;
			_behavior = behavior;
			SniperWaypointWaitTime = MyAPIGateway.Session.GameDateTime;
			SniperWaypointAbandonTime = MyAPIGateway.Session.GameDateTime;

			_defaultWeaponProfile = "MES-Weapons-GenericStandard";

		}

		public void ProcessBehavior() {

			if(MES_SessionCore.IsServer == false) {

				return;

			}

			//BehaviorLogger.Write(_behavior.Mode.ToString(), BehaviorDebugEnum.General);

			if (_behavior.Mode != BehaviorMode.Retreat && _behavior.BehaviorSettings.DoRetreat == true){

				_behavior.ChangeCoreBehaviorMode(BehaviorMode.Retreat);
				_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing, CheckEnum.Yes, CheckEnum.No);

			}
			
			if(_behavior.Mode == BehaviorMode.Init) {

				if(!_behavior.AutoPilot.Targeting.HasTarget()) {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitingForTarget);

				} else {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					_behavior.AutoPilot.OffsetWaypointGenerator(true);
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.OffsetWaypoint, CheckEnum.Yes, CheckEnum.No);

				}

			}

			if(_behavior.Mode == BehaviorMode.WaitingForTarget) {

				if(_behavior.AutoPilot.CurrentMode != _behavior.AutoPilot.UserCustomModeIdle) {

					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.None, CheckEnum.No, CheckEnum.Yes);

				}

				if(_behavior.AutoPilot.Targeting.HasTarget()) {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					_behavior.AutoPilot.OffsetWaypointGenerator(true);
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.OffsetWaypoint, CheckEnum.Yes, CheckEnum.No);

				} else if(_behavior.Despawn.NoTargetExpire == true){
					
					_behavior.Despawn.Retreat();
					
				}

			}

			if(!_behavior.AutoPilot.Targeting.HasTarget() && _behavior.Mode != BehaviorMode.Retreat && _behavior.Mode != BehaviorMode.WaitingForTarget) {


				_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitingForTarget);
				_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.None, CheckEnum.No, CheckEnum.Yes);

			}

			//ApproachTarget
			if (_behavior.Mode == BehaviorMode.ApproachTarget) {

				var timeSpan = MyAPIGateway.Session.GameDateTime - this.SniperWaypointAbandonTime;
				//BehaviorLogger.Write("Distance To Waypoint: " + New_behavior.AutoPilot.DistanceToCurrentWaypoint.ToString(), BehaviorDebugEnum.General);

				if (_behavior.AutoPilot.ArrivedAtOffsetWaypoint()) {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.EngageTarget);
					this.SniperWaypointWaitTime = MyAPIGateway.Session.GameDateTime;
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.WaypointFromTarget);
					_behavior.BehaviorTriggerA = true;

				} else if (timeSpan.TotalSeconds >= _behavior.AutoPilot.Data.WaypointAbandonTimeTrigger) {

					BehaviorLogger.Write("Sniper Timeout, Getting New Offset", BehaviorDebugEnum.General);
					this.SniperWaypointAbandonTime = MyAPIGateway.Session.GameDateTime;
					_behavior.AutoPilot.OffsetWaypointGenerator(true);

				} else if (_behavior.AutoPilot.IsWaypointThroughVelocityCollision()) {

					BehaviorLogger.Write("Sniper Velocity Through Collision, Getting New Offset", BehaviorDebugEnum.General);
					this.SniperWaypointAbandonTime = MyAPIGateway.Session.GameDateTime;
					_behavior.AutoPilot.OffsetWaypointGenerator(true);

				}

			}

			//ApproachWaypoint
			if (_behavior.Mode == BehaviorMode.ApproachWaypoint) {

				var engageDistance = _behavior.AutoPilot.InGravity() ? _behavior.AutoPilot.Data.EngageDistancePlanet : _behavior.AutoPilot.Data.EngageDistanceSpace;
				var disengageDistance = _behavior.AutoPilot.InGravity() ? _behavior.AutoPilot.Data.DisengageDistancePlanet : _behavior.AutoPilot.Data.DisengageDistanceSpace;

				if (_behavior.AutoPilot.DistanceToTargetWaypoint < engageDistance) {

					var distanceDifference = engageDistance - _behavior.AutoPilot.DistanceToTargetWaypoint;
					var engageDifferenceHalved = (disengageDistance - engageDistance) / 2;
					var directionAwayFromTarget = Vector3D.Normalize(_behavior.RemoteControl.GetPosition() - _behavior.AutoPilot.Targeting.TargetLastKnownCoords);
					var fallbackCoords = directionAwayFromTarget * (distanceDifference + engageDifferenceHalved) + _behavior.RemoteControl.GetPosition();
					_behavior.AutoPilot.SetInitialWaypoint(fallbackCoords);

				} else {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.EngageTarget);
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.WaypointFromTarget);

				}

			}

			//Engage
			if (_behavior.Mode == BehaviorMode.EngageTarget) {

				var timeSpan = MyAPIGateway.Session.GameDateTime - this.SniperWaypointWaitTime;
				var engageDistance = _behavior.AutoPilot.InGravity() ? _behavior.AutoPilot.Data.EngageDistancePlanet : _behavior.AutoPilot.Data.EngageDistanceSpace;
				var disengageDistance = _behavior.AutoPilot.InGravity() ? _behavior.AutoPilot.Data.DisengageDistancePlanet : _behavior.AutoPilot.Data.DisengageDistanceSpace;

				if (timeSpan.TotalSeconds >= _behavior.AutoPilot.Data.WaypointWaitTimeTrigger || _behavior.AutoPilot.DistanceToTargetWaypoint > disengageDistance) {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					this.SniperWaypointAbandonTime = MyAPIGateway.Session.GameDateTime;
					_behavior.AutoPilot.OffsetWaypointGenerator(true);
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.OffsetWaypoint, CheckEnum.Yes, CheckEnum.No);

				}

				if (_behavior.AutoPilot.DistanceToTargetWaypoint < engageDistance) {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachWaypoint);
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing, CheckEnum.Yes, CheckEnum.No);

				}

			}

			//Retreat
			if (_behavior.Mode == BehaviorMode.Retreat) {

				if (_behavior.Despawn.NearestPlayer?.Player?.Controller?.ControlledEntity?.Entity != null) {

					_behavior.AutoPilot.SetInitialWaypoint(_behavior.Despawn.GetRetreatCoords());

				}

			}

		}

		public void SetDefaultTags() {

			BehaviorLogger.Write("Beginning Behavior Init For Sniper", BehaviorDebugEnum.General);

			//Behavior Specific Defaults
			_behavior.AutoPilot.Data = ProfileManager.GetAutopilotProfile("RAI-Generic-Autopilot-Sniper");
			_behavior.Despawn.UseNoTargetTimer = true;
			
			if (string.IsNullOrWhiteSpace(_behavior.BehaviorSettings.WeaponsSystemProfile)) {

				_behavior.BehaviorSettings.WeaponsSystemProfile = _defaultWeaponProfile;

			}

		}

		public override string ToString() {

			return "";

		}

		public void InitTags() {

			if(string.IsNullOrWhiteSpace(_behavior.RemoteControl?.CustomData) == false) {

				var descSplit = _behavior.RemoteControl.CustomData.Split('\n');

				foreach(var tag in descSplit) {
					
					

				}
				
			}

		}

	}

}
	
