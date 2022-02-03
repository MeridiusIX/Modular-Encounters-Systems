using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Helpers;
using Sandbox.ModAPI;
using System;
using System.Text;

namespace ModularEncountersSystems.Behavior {

	public class Escort : IBehaviorSubClass{

		internal IBehavior _behavior;

		public BehaviorSubclass SubClass { get { return _subClass; } set { _subClass = value; } }
		private BehaviorSubclass _subClass;

		private NewAutoPilotMode WaterNav { get { return (_behavior.AutoPilot.Data.UseWaterPatrolMode ? NewAutoPilotMode.WaterNavigation : NewAutoPilotMode.None); } }

		private DateTime _waitForParent;

		public Escort(IBehavior behavior){

			_subClass = BehaviorSubclass.Escort;
			_behavior = behavior;

		}

		public void ProcessBehavior() {

			if(MES_SessionCore.IsServer == false) {

				return;

			}

			//Init
			if (_behavior.Mode == BehaviorMode.Init) {

				_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitingForTarget);
				_waitForParent = MyAPIGateway.Session.GameDateTime;

			}

			//WaitingForTarget
			if (_behavior.Mode == BehaviorMode.WaitingForTarget) {

				if (_behavior.BehaviorSettings.ParentEscort != null && _behavior.BehaviorSettings.ParentEscort.ValidationCheck()) {

					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.WaypointFromEscort | NewAutoPilotMode.PlanetaryPathing | WaterNav, CheckEnum.Yes, CheckEnum.No);
					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachWaypoint);
					_behavior.BehaviorTriggerA = true;

				} else {

					var timeSpan = MyAPIGateway.Session.GameDateTime - _waitForParent;

					if (timeSpan.TotalSeconds > _behavior.AutoPilot.Data.WaypointWaitTimeTrigger) {

						_behavior.ChangeCoreBehaviorMode(BehaviorMode.Retreat);
						_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | WaterNav, CheckEnum.Yes, CheckEnum.No);

					}

				}

			}

			//Approach
			if (_behavior.Mode == BehaviorMode.ApproachWaypoint) {

				if (_behavior.BehaviorSettings.ParentEscort == null || !_behavior.BehaviorSettings.ParentEscort.ValidationCheck()) {

					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.None, CheckEnum.No, CheckEnum.Yes);
					_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitingForTarget);
					_behavior.BehaviorTriggerB = true;
					_waitForParent = MyAPIGateway.Session.GameDateTime;

				} else {

					if (_behavior.AutoPilot.DistanceToInitialWaypoint < _behavior.AutoPilot.Data.EscortSpeedMatchMaxDistance) {

						var shipSpeed = _behavior.BehaviorSettings.ParentEscort.ParentBehavior.RemoteControl.GetShipSpeed();

						if (shipSpeed < _behavior.AutoPilot.Data.IdealMaxSpeed) {

							var lerpedDistance = MathTools.LerpToMultiplier(_behavior.AutoPilot.Data.EscortSpeedMatchMinDistance, _behavior.AutoPilot.Data.EscortSpeedMatchMaxDistance, _behavior.AutoPilot.DistanceToInitialWaypoint);
							var lerpedSpeed = MathTools.LerpToValue(shipSpeed, _behavior.AutoPilot.Data.IdealMaxSpeed, lerpedDistance);
							_behavior.AutoPilot.State.MaxSpeedOverride = shipSpeed;

						} else {

							if (_behavior.AutoPilot.State.MaxSpeedOverride != -1)
								_behavior.AutoPilot.State.MaxSpeedOverride = -1;

						}
						
					} else {

						if (_behavior.AutoPilot.State.MaxSpeedOverride != -1)
							_behavior.AutoPilot.State.MaxSpeedOverride = -1;


					}

				}

			}

			//Retreat
			if (_behavior.Mode == BehaviorMode.Retreat) {

				if (_behavior.Despawn.NearestPlayer?.Player?.Controller?.ControlledEntity?.Entity != null) {

					//BehaviorLogger.AddMsg("DespawnCoordsCreated", true);
					_behavior.AutoPilot.SetInitialWaypoint(_behavior.Despawn.GetRetreatCoords());

				}

			}

		}

		public void SetDefaultTags() {

			_behavior.AutoPilot.Data = ProfileManager.GetAutopilotProfile("RAI-Generic-Autopilot-Escort");
			_behavior.Despawn.UseNoTargetTimer = false;
			_behavior.AutoPilot.Data.DisableInertiaDampeners = false;

			if (string.IsNullOrWhiteSpace(_behavior.BehaviorSettings.WeaponsSystemProfile)) {

				_behavior.BehaviorSettings.WeaponsSystemProfile = "MES-Weapons-GenericStandard";

			}

		}

		public void InitTags() {

			return;

			/*
			if(string.IsNullOrWhiteSpace(_behavior.RemoteControl?.CustomData) == false) {

				var descSplit = _behavior.RemoteControl.CustomData.Split('\n');

				foreach(var tag in descSplit) {



				}

			}
			*/
		}

		public override string ToString() {

			var sb = new StringBuilder();
			sb.Append("::: Escort Behavior :::").AppendLine();
			sb.Append(" - Is Current Escort Existing:  ").Append(_behavior.BehaviorSettings.ParentEscort != null ? "True" : "False").AppendLine();
			sb.Append(" - Is Current Escort Valid:     ").Append((_behavior.BehaviorSettings.ParentEscort?.ValidationCheck() ?? false).ToString()).AppendLine();

			sb.AppendLine();
			return sb.ToString();

		}

	}

}
	
