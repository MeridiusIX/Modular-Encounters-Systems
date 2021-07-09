using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Weapons;
using SpaceEngineers.Game.ModAPI;
using ProtoBuf;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Utils;
using VRageMath;
using ModularEncountersSystems;
using ModularEncountersSystems.Behavior;
using ModularEncountersSystems.Behavior.Subsystems;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Behavior.Subsystems.Profiles;
using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Logging;

namespace ModularEncountersSystems.Behavior {

	public class Horsefly : IBehaviorSubClass {

		public int HorseflyWaypointWaitTimeTrigger {

			get {

				return _horseflyWaypointWaitTimeTrigger > 0 ? _horseflyWaypointWaitTimeTrigger : _behavior.AutoPilot.Data.WaypointWaitTimeTrigger;

			}

			set {

				_horseflyWaypointWaitTimeTrigger = value;

			}

		}

		public int HorseflyWaypointAbandonTimeTrigger {

			get {

				return _horseflyWaypointAbandonTimeTrigger > 0 ? _horseflyWaypointAbandonTimeTrigger : _behavior.AutoPilot.Data.WaypointAbandonTimeTrigger;

			}

			set {

				_horseflyWaypointAbandonTimeTrigger = value;

			}

		}

		private int _horseflyWaypointWaitTimeTrigger;
		private int _horseflyWaypointAbandonTimeTrigger;

		public byte Counter;
		public DateTime HorseflyWaypointWaitTime;
		public DateTime HorseflyWaypointAbandonTime;

		private IBehavior _behavior;

		public Horsefly(IBehavior behavior) {

			_behavior = behavior;
			_horseflyWaypointWaitTimeTrigger = -1;
			_horseflyWaypointAbandonTimeTrigger = -1;

			//HorseflyWaypointWaitTimeTrigger = 5;
			//HorseflyWaypointAbandonTimeTrigger = 30;

			Counter = 0;
			HorseflyWaypointWaitTime = MyAPIGateway.Session.GameDateTime;
			HorseflyWaypointAbandonTime = MyAPIGateway.Session.GameDateTime;

		}

		public void ProcessBehavior() {

			if (MES_SessionCore.IsServer == false) {

				return;

			}

			if (_behavior.Mode != BehaviorMode.Retreat && _behavior.Settings.DoRetreat == true) {

				_behavior.ChangeCoreBehaviorMode(BehaviorMode.Retreat);
				_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing, CheckEnum.Yes, CheckEnum.No);

			}

			//Init
			if(_behavior.Mode == BehaviorMode.Init) {

				if(!_behavior.AutoPilot.Targeting.HasTarget()) {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitingForTarget);

				} else {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					this.HorseflyWaypointWaitTime = MyAPIGateway.Session.GameDateTime;
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.OffsetWaypoint, CheckEnum.Yes, CheckEnum.No);

				}

			}

			//Waiting For Target
			if(_behavior.Mode == BehaviorMode.WaitingForTarget) {

				if(_behavior.AutoPilot.Targeting.HasTarget()) {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					this.HorseflyWaypointWaitTime = MyAPIGateway.Session.GameDateTime;
					_behavior.AutoPilot.OffsetWaypointGenerator(true);
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.OffsetWaypoint, CheckEnum.Yes, CheckEnum.No);

				} else if(_behavior.Despawn.NoTargetExpire == true) {

					_behavior.Despawn.Retreat();

				}

			}

			if(!_behavior.AutoPilot.Targeting.HasTarget() && _behavior.Mode != BehaviorMode.Retreat) {

				_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitingForTarget);

			}

			//Approach
			if(_behavior.Mode == BehaviorMode.ApproachTarget) {

				var timeSpan = MyAPIGateway.Session.GameDateTime - this.HorseflyWaypointAbandonTime;
				//Logger.Write("Distance To Waypoint: " + New_behavior.AutoPilot.DistanceToCurrentWaypoint.ToString(), BehaviorDebugEnum.General);

				if (_behavior.AutoPilot.ArrivedAtOffsetWaypoint()) {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitAtWaypoint);
					this.HorseflyWaypointWaitTime = MyAPIGateway.Session.GameDateTime;
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.None, CheckEnum.No, CheckEnum.Yes);
					_behavior.BehaviorTriggerA = true;

				} else if (timeSpan.TotalSeconds >= this.HorseflyWaypointAbandonTimeTrigger) {

					BehaviorLogger.Write("Horsefly Timeout, Getting New Offset", BehaviorDebugEnum.General);
					this.HorseflyWaypointAbandonTime = MyAPIGateway.Session.GameDateTime;
					_behavior.AutoPilot.OffsetWaypointGenerator(true);

				} else if (_behavior.AutoPilot.IsWaypointThroughVelocityCollision()) {

					BehaviorLogger.Write("Horsefly Velocity Through Collision, Getting New Offset", BehaviorDebugEnum.General);
					this.HorseflyWaypointAbandonTime = MyAPIGateway.Session.GameDateTime;
					_behavior.AutoPilot.OffsetWaypointGenerator(true);

				}

			}

			//WaitAtWaypoint
			if (_behavior.Mode == BehaviorMode.WaitAtWaypoint) {

				var timeSpan = MyAPIGateway.Session.GameDateTime - this.HorseflyWaypointWaitTime;

				if (timeSpan.TotalSeconds >= this.HorseflyWaypointWaitTimeTrigger) {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					this.HorseflyWaypointAbandonTime = MyAPIGateway.Session.GameDateTime;
					_behavior.AutoPilot.OffsetWaypointGenerator(true);
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.OffsetWaypoint, CheckEnum.Yes, CheckEnum.No);
					_behavior.BehaviorTriggerB = true;

				}

			}

			//Retreat
			if (_behavior.Mode == BehaviorMode.Retreat) {

				if (_behavior.Despawn.NearestPlayer?.Controller?.ControlledEntity?.Entity != null) {

					//Logger.AddMsg("DespawnCoordsCreated", true);
					_behavior.AutoPilot.SetInitialWaypoint(VectorHelper.GetDirectionAwayFromTarget(_behavior.RemoteControl.GetPosition(), _behavior.Despawn.NearestPlayer.GetPosition()) * 1000 + _behavior.RemoteControl.GetPosition());

				}

			}

		}

		public void SetDefaultTags() {

			_behavior.AutoPilot.Data = ProfileManager.GetAutopilotProfile("RAI-Generic-Autopilot-Horsefly");
			_behavior.Despawn.UseNoTargetTimer = true;

		}

		public void InitTags() {

			if (string.IsNullOrWhiteSpace(_behavior.RemoteControl?.CustomData) == false) {

				var descSplit = _behavior.RemoteControl.CustomData.Split('\n');

				foreach (var tag in descSplit) {

					//HorseflyWaypointWaitTimeTrigger
					if (tag.Contains("[HorseflyWaypointWaitTimeTrigger:") == true) {

						TagParse.TagIntCheck(tag, ref _horseflyWaypointWaitTimeTrigger);

					}

					//HorseflyWaypointAbandonTimeTrigger
					if (tag.Contains("[HorseflyWaypointAbandonTimeTrigger:") == true) {

						TagParse.TagIntCheck(tag, ref _horseflyWaypointAbandonTimeTrigger);

					}

				}

			}


		}

	}

}

