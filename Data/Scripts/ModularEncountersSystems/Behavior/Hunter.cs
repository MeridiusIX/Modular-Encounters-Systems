using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using Sandbox.ModAPI;
using System;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Behavior {

	public class Hunter : IBehaviorSubClass{

		//Configurable
		public int TimeBetweenNewTargetChecks;
		public int LostTargetTimerTrigger;
		public double DistanceToCheckEngagableTarget;

		public bool EngageOnCameraDetection;
		public bool EngageOnWeaponActivation;
		public bool EngageOnTargetLineOfSight;

		public double CameraDetectionMaxRange;

		//Non-Config
		private DateTime _checkActiveTargetTimer;
		private DateTime _lostTargetTimer;

		private bool _inRange;
		private IBehavior _behavior;

		public BehaviorSubclass SubClass { get { return _subClass; } set { _subClass = value; } }
		private BehaviorSubclass _subClass;
		
		public Hunter(IBehavior behavior) {

			_subClass = BehaviorSubclass.Hunter;
			_behavior = behavior;

			TimeBetweenNewTargetChecks = 15;
			LostTargetTimerTrigger = 30;
			DistanceToCheckEngagableTarget = 1200;

			EngageOnCameraDetection = false;
			EngageOnWeaponActivation = false;
			EngageOnTargetLineOfSight = false;

			CameraDetectionMaxRange = 1800;

			_checkActiveTargetTimer = MyAPIGateway.Session.GameDateTime;
			_lostTargetTimer = MyAPIGateway.Session.GameDateTime;

			_inRange = false;

		}

		//A: Found Target (Approach)
		//B: Lost Target (Still Approach)
		//C: Lost Target (Go To Despawn)
		//D: Engage Target
		//E: Engage In Range
		//F: Engage Out Range


		public void ProcessBehavior() {

			if(MES_SessionCore.IsServer == false) {

				return;

			}

			//Logger.Write(Mode.ToString(), BehaviorDebugEnum.General);

			if (_behavior.Mode != BehaviorMode.Retreat && _behavior.Settings.DoRetreat == true){

				_behavior.ChangeCoreBehaviorMode(BehaviorMode.Retreat);
				_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing, CheckEnum.Yes, CheckEnum.No);

			}
			
			if(_behavior.Mode == BehaviorMode.Init) {

				if (_behavior.Settings.DespawnCoords == Vector3D.Zero) {

					if(_behavior.CurrentGrid.Npc != null && _behavior.CurrentGrid.Npc.EndCoords != Vector3D.Zero)
						_behavior.Settings.DespawnCoords = _behavior.CurrentGrid.Npc.EndCoords;

					if (_behavior.Settings.DespawnCoords == Vector3D.Zero)
						_behavior.Settings.DespawnCoords = _behavior.AutoPilot.CalculateDespawnCoords(_behavior.RemoteControl.GetPosition());

				}

				ReturnToDespawn();

			}

			if (_behavior.BehaviorActionA && _behavior.Mode != BehaviorMode.EngageTarget) {

				//Logger.Write("Hunter BehaviorActionA Triggered", BehaviorDebugEnum.General);

				_behavior.BehaviorActionA = false;

				if (_behavior.Settings.LastDamagerEntity != 0) {

					//Logger.Write("Damager Entity Id Valid" + _behavior.Settings.LastDamagerEntity.ToString(), BehaviorDebugEnum.General);

					IMyEntity tempEntity = null;

					if (MyAPIGateway.Entities.TryGetEntityById(_behavior.Settings.LastDamagerEntity, out tempEntity)) {

						//Logger.Write("Damager Entity Valid", BehaviorDebugEnum.General);

						var parentEnt = tempEntity.GetTopMostParent();

						if (parentEnt != null) {

							//Logger.Write("Damager Parent Entity Valid", BehaviorDebugEnum.General);
							var gridGroup = MyAPIGateway.GridGroups.GetGroup(_behavior.RemoteControl.SlimBlock.CubeGrid, GridLinkTypeEnum.Physical);
							bool isSameGridConstrust = false;

							foreach (var grid in gridGroup) {

								if (grid.EntityId == tempEntity.GetTopMostParent().EntityId) {

									//Logger.Write("Damager Parent Entity Was Same Grid", BehaviorDebugEnum.General);
									isSameGridConstrust = true;
									break;

								}

							}

							if (!isSameGridConstrust) {

								//Logger.Write("Damager Parent Entity Was External", BehaviorDebugEnum.General);
								_behavior.AutoPilot.Targeting.ForceTargetEntityId = parentEnt.EntityId;
								_behavior.AutoPilot.Targeting.ForceTargetEntity = parentEnt;
								_behavior.AutoPilot.Targeting.ForceRefresh = true;
								_behavior.AutoPilot.SetAutoPilotDataMode(AutoPilotDataMode.Secondary);
								_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget, CheckEnum.Yes, CheckEnum.No);
								_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
								BehaviorLogger.Write("Hunter Approaching Potential Target From Damage", BehaviorDebugEnum.BehaviorSpecific);
								return;

							}

						}

					}

				}

			}

			if (_behavior.Mode == BehaviorMode.ApproachWaypoint) {

				var time = MyAPIGateway.Session.GameDateTime - _checkActiveTargetTimer;

				if (time.TotalSeconds > TimeBetweenNewTargetChecks) {

					_checkActiveTargetTimer = MyAPIGateway.Session.GameDateTime;

					if (_behavior.AutoPilot.Targeting.HasTarget()) {

						_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
						_lostTargetTimer = MyAPIGateway.Session.GameDateTime;
						_behavior.BehaviorTriggerA = true;
						_behavior.AutoPilot.SetAutoPilotDataMode(AutoPilotDataMode.Secondary);
						_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget, CheckEnum.Yes, CheckEnum.No);
						BehaviorLogger.Write("Hunter Approaching Potential Target", BehaviorDebugEnum.BehaviorSpecific);

					}
					
				}

				if (!_behavior.BehaviorTriggerA) {

					if (Vector3D.Distance(_behavior.RemoteControl.GetPosition(), _behavior.Settings.DespawnCoords) <= MathTools.Hypotenuse(_behavior.AutoPilot.Data.WaypointTolerance, _behavior.AutoPilot.Data.WaypointTolerance)) {

						_behavior.Settings.DoDespawn = true;
					
					}
				
				}

			}

			if (_behavior.Mode == BehaviorMode.ApproachTarget) {

				if (!_behavior.AutoPilot.Targeting.HasTarget()) {

					_behavior.AutoPilot.SetInitialWaypoint(_behavior.AutoPilot.Targeting.TargetLastKnownCoords);
					var time = MyAPIGateway.Session.GameDateTime - _lostTargetTimer;

					if (time.TotalSeconds > LostTargetTimerTrigger) {

						BehaviorLogger.Write("Hunter Returning To Despawn", BehaviorDebugEnum.BehaviorSpecific);
						ReturnToDespawn();
						return;

					}

					return;

				}

				_lostTargetTimer = MyAPIGateway.Session.GameDateTime;
				bool engageTarget = false;
				var targetDist = Vector3D.Distance(_behavior.RemoteControl.GetPosition(), _behavior.AutoPilot.Targeting.TargetLastKnownCoords);

				//Check Turret
				if (EngageOnWeaponActivation == true) {

					if (_behavior.AutoPilot.Weapons.GetTurretTarget() != 0) {

						BehaviorLogger.Write("Hunter Turrets Detected Target", BehaviorDebugEnum.BehaviorSpecific);
						engageTarget = true;

					}
						


				}

				//Check Visual Range
				if (!engageTarget && EngageOnCameraDetection && targetDist < CameraDetectionMaxRange) {

					if (_behavior.Grid.RaycastGridCheck(_behavior.AutoPilot.Targeting.TargetLastKnownCoords)) {

						BehaviorLogger.Write("Hunter Raycast Target Success", BehaviorDebugEnum.BehaviorSpecific);

					}
						engageTarget = true;

				}

				//Check Collision Data
				if (!engageTarget && EngageOnTargetLineOfSight && _behavior.AutoPilot.Targeting.Data.MaxLineOfSight > 0 && _behavior.AutoPilot.Collision.TargetResult.HasTarget(_behavior.AutoPilot.Targeting.Data.MaxLineOfSight)) {

					if (_behavior.AutoPilot.Targeting.Target.GetParentEntity().EntityId == _behavior.AutoPilot.Collision.TargetResult.GetCollisionEntity().EntityId) {

						BehaviorLogger.Write("Hunter Has Line of Sight to Target", BehaviorDebugEnum.BehaviorSpecific);
						engageTarget = true;

					}
						

				}

				if (engageTarget) {

					BehaviorLogger.Write("Hunter Engaging Target", BehaviorDebugEnum.BehaviorSpecific);
					_behavior.BehaviorTriggerD = true;
					_behavior.ChangeCoreBehaviorMode(BehaviorMode.EngageTarget);

				}

			}

			//Engage
			if (_behavior.Mode == BehaviorMode.EngageTarget) {

				if (_behavior.AutoPilot.Targeting.HasTarget()) {

					var targetDist = Vector3D.Distance(_behavior.RemoteControl.GetPosition(), _behavior.AutoPilot.Targeting.TargetLastKnownCoords);

					if (!_inRange) {

						if (targetDist < (_behavior.AutoPilot.InGravity() ? _behavior.AutoPilot.Data.EngageDistancePlanet : _behavior.AutoPilot.Data.EngageDistanceSpace)) {

							BehaviorLogger.Write("Hunter Within Engage Range", BehaviorDebugEnum.BehaviorSpecific);
							_inRange = true;
							_behavior.BehaviorTriggerE = true;
							_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.Strafe | NewAutoPilotMode.WaypointFromTarget, CheckEnum.Yes, CheckEnum.No);

						}

					} else {

						if (targetDist > (_behavior.AutoPilot.InGravity() ? _behavior.AutoPilot.Data.DisengageDistancePlanet : _behavior.AutoPilot.Data.DisengageDistanceSpace)) {

							BehaviorLogger.Write("Hunter Outside Engage Range", BehaviorDebugEnum.BehaviorSpecific);
							_inRange = false;
							_behavior.BehaviorTriggerF = true;
							_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget, CheckEnum.Yes, CheckEnum.No);

						}

					}

				} else {

					BehaviorLogger.Write("Hunter Lost Target While Engaging", BehaviorDebugEnum.BehaviorSpecific);
					_behavior.BehaviorTriggerB = true;
					_behavior.BehaviorTriggerF = true;
					_inRange = false;
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget, CheckEnum.Yes, CheckEnum.No);
					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);

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

		public void ReturnToDespawn() {

			if(_behavior.Mode == BehaviorMode.ApproachTarget)
				_behavior.BehaviorTriggerC = true;

			_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachWaypoint);
			_behavior.AutoPilot.SetAutoPilotDataMode(AutoPilotDataMode.Primary);
			_behavior.AutoPilot.ActivateAutoPilot(_behavior.Settings.DespawnCoords, NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing, CheckEnum.Yes, CheckEnum.No);
			_checkActiveTargetTimer = MyAPIGateway.Session.GameDateTime;

		}

		public void SetDefaultTags() {


			_behavior.Despawn.UseNoTargetTimer = false;
			_behavior.AutoPilot.Weapons.UseStaticGuns = true;
			_behavior.AutoPilot.AssignAutoPilotDataMode("RAI-Generic-Autopilot-Hunter-A", AutoPilotDataMode.Primary);
			_behavior.AutoPilot.AssignAutoPilotDataMode("RAI-Generic-Autopilot-Hunter-B", AutoPilotDataMode.Secondary);

		}

		public void InitTags() {

			if(string.IsNullOrWhiteSpace(_behavior.RemoteControl?.CustomData) == false) {

				var descSplit = _behavior.RemoteControl.CustomData.Split('\n');

				foreach(var tag in descSplit) {

					//TimeBetweenNewTargetChecks
					if (tag.Contains("[TimeBetweenNewTargetChecks:")) {

						TagParse.TagIntCheck(tag, ref this.TimeBetweenNewTargetChecks);

					}

					//LostTargetTimerTrigger
					if (tag.Contains("[LostTargetTimerTrigger:")) {

						TagParse.TagIntCheck(tag, ref this.LostTargetTimerTrigger);

					}

					//DistanceToCheckEngagableTarget
					if (tag.Contains("[DistanceToCheckEngagableTarget:")) {

						TagParse.TagDoubleCheck(tag, ref this.DistanceToCheckEngagableTarget);

					}

					//EngageOnCameraDetection
					if (tag.Contains("[EngageOnCameraDetection:")) {

						TagParse.TagBoolCheck(tag, ref EngageOnCameraDetection);

					}

					//EngageOnWeaponActivation
					if (tag.Contains("[EngageOnWeaponActivation:")) {

						TagParse.TagBoolCheck(tag, ref EngageOnWeaponActivation);

					}

					//EngageOnTargetLineOfSight
					if (tag.Contains("[EngageOnTargetLineOfSight:")) {

						TagParse.TagBoolCheck(tag, ref EngageOnTargetLineOfSight);

					}

					//CameraDetectionMaxRange
					if (tag.Contains("[CameraDetectionMaxRange:")) {

						TagParse.TagDoubleCheck(tag, ref this.CameraDetectionMaxRange);

					}


				}

			}

		}

	}

}
	