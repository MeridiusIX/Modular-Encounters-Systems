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

namespace ModularEncountersSystems.Behavior{
	
	public class Nautical : IBehaviorSubClass{

		DateTime WaitTime;
		IBehavior _behavior;
		public Nautical(IBehavior behavior) {

			_behavior = behavior;
			WaitTime = MyAPIGateway.Session.GameDateTime;

		}

		public void ProcessBehavior() {

			if(MES_SessionCore.IsServer == false) {

				return;

			}

			//Logger.Write(Mode.ToString(), BehaviorDebugEnum.General);

			if (_behavior.Mode != BehaviorMode.Retreat && _behavior.Settings.DoRetreat == true){

				_behavior.ChangeCoreBehaviorMode(BehaviorMode.Retreat);
				_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.LevelWithGravity | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.WaterNavigation);

			}
			
			if(_behavior.Mode == BehaviorMode.Init) {

				if(!_behavior.AutoPilot.Targeting.HasTarget()) {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitingForTarget);
					_behavior.BehaviorTriggerD = true;

				} else {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.LevelWithGravity | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.WaterNavigation);

				}

			}

			if(_behavior.Mode == BehaviorMode.WaitingForTarget) {

				if(_behavior.AutoPilot.CurrentMode != _behavior.AutoPilot.UserCustomModeIdle) {

					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.LevelWithGravity);

				}

				if(_behavior.AutoPilot.Targeting.HasTarget()) {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.LevelWithGravity | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.WaterNavigation);

				} else if(_behavior.Despawn.NoTargetExpire == true){
					
					_behavior.Despawn.Retreat();
					_behavior.BehaviorTriggerD = true;

				}

			}

			if(!_behavior.AutoPilot.Targeting.HasTarget() && _behavior.Mode != BehaviorMode.Retreat && _behavior.Mode != BehaviorMode.WaitingForTarget) {


				_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitingForTarget);
				_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.LevelWithGravity);
				_behavior.BehaviorTriggerD = true;

			}

			//A - Stop All Movement
			if (_behavior.BehaviorActionA) {

				_behavior.BehaviorActionA = false;
				_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitAtWaypoint);
				_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.LevelWithGravity);
				WaitTime = MyAPIGateway.Session.GameDateTime;
				_behavior.BehaviorTriggerC = true;

			}

			//WaitAtWaypoint
			if (_behavior.Mode == BehaviorMode.WaitAtWaypoint) {

				var timespan = MyAPIGateway.Session.GameDateTime - WaitTime;

				if (timespan.TotalSeconds >= _behavior.AutoPilot.Data.WaypointWaitTimeTrigger) {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitingForTarget);
					_behavior.BehaviorTriggerD = true;

				}

			}

			//Approach
			if (_behavior.Mode == BehaviorMode.ApproachTarget) {

				bool inRange = false;

				if (!_behavior.AutoPilot.InGravity() && _behavior.AutoPilot.DistanceToTargetWaypoint < _behavior.AutoPilot.Data.EngageDistanceSpace)
					inRange = true;

				if(_behavior.AutoPilot.InGravity() && _behavior.AutoPilot.DistanceToTargetWaypoint < _behavior.AutoPilot.Data.EngageDistancePlanet)
					inRange = true;

				if (inRange) {

					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToTarget | NewAutoPilotMode.LevelWithGravity | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.WaterNavigation);
					_behavior.ChangeCoreBehaviorMode(BehaviorMode.EngageTarget);
					_behavior.BehaviorTriggerA = true;

				}

			}

			//Engage
			if (_behavior.Mode == BehaviorMode.EngageTarget) {

				bool outRange = false;

				if (!_behavior.AutoPilot.InGravity() && _behavior.AutoPilot.DistanceToTargetWaypoint > _behavior.AutoPilot.Data.DisengageDistanceSpace)
					outRange = true;

				if (_behavior.AutoPilot.InGravity() && _behavior.AutoPilot.DistanceToTargetWaypoint > _behavior.AutoPilot.Data.DisengageDistancePlanet)
					outRange = true;

				if (outRange) {

					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.LevelWithGravity | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.WaterNavigation);
					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
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

			//Behavior Specific Defaults
			_behavior.AutoPilot.Data = ProfileManager.GetAutopilotProfile("RAI-Generic-Autopilot-Nautical");
			_behavior.Despawn.UseNoTargetTimer = true;
			_behavior.AutoPilot.Weapons.UseStaticGuns = true;

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
	
