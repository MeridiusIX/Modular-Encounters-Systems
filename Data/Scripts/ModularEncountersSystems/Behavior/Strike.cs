using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using Sandbox.ModAPI;
using System;

namespace ModularEncountersSystems.Behavior {

	//Trigger Notes

	//BehaviorTriggerA - ApproachTarget initiated
	//BehaviorTriggerB - EngageTarget initiated

	//BehaviorActionA - Initiates Immediate ApproachTarget & Offset Recalculation
	//BehaviorActionB - Initiates Immediate EngageTarget Attempt (may revert to ApproachTarget if autopilot parameters are already at limit)

	public class Strike : IBehaviorSubClass {

		//Configurable

		public double StrikeBeginSpaceAttackRunDistance { get { return _behavior.AutoPilot?.Data != null ? _behavior.AutoPilot.Data.AttackRunDistanceSpace : _strikeBeginSpaceAttackRunDistance; } }
		public double StrikeBeginPlanetAttackRunDistance { get { return _behavior.AutoPilot?.Data != null ? _behavior.AutoPilot.Data.AttackRunDistancePlanet : _strikeBeginPlanetAttackRunDistance; } }
		public double StrikeBreakawayDistance { get { return _behavior.AutoPilot?.Data != null ? _behavior.AutoPilot.Data.AttackRunBreakawayDistance : _strikeBreakawayDistance; } }
		public int StrikeOffsetRecalculationTime { get { return _behavior.AutoPilot?.Data != null ? _behavior.AutoPilot.Data.OffsetRecalculationTime : _strikeOffsetRecalculationTime; } }
		public bool StrikeEngageUseSafePlanetPathing { get { return _behavior.AutoPilot?.Data != null ? _behavior.AutoPilot.Data.AttackRunUseSafePlanetPathing : _strikeEngageUseSafePlanetPathing; } }
		public bool StrikeEngageUseCollisionEvasionSpace { get { return _behavior.AutoPilot?.Data != null ? _behavior.AutoPilot.Data.AttackRunUseCollisionEvasionSpace : _strikeEngageUseCollisionEvasionSpace; } }
		public bool StrikeEngageUseCollisionEvasionPlanet { get { return _behavior.AutoPilot?.Data != null ? _behavior.AutoPilot.Data.AttackRunUseCollisionEvasionPlanet : _strikeEngageUseCollisionEvasionPlanet; } }

		public bool EngageOverrideWithDistanceAndTimer { get { return _behavior.AutoPilot?.Data != null ? _behavior.AutoPilot.Data.AttackRunOverrideWithDistanceAndTimer : _engageOverrideWithDistanceAndTimer; } }
		public int EngageOverrideTimerTrigger { get { return _behavior.AutoPilot?.Data != null ? _behavior.AutoPilot.Data.AttackRunOverrideTimerTrigger : _engageOverrideTimerTrigger; } }
		public double EngageOverrideDistance { get { return _behavior.AutoPilot?.Data != null ? _behavior.AutoPilot.Data.AttackRunOverrideDistance : _engageOverrideDistance; } }


		private double _strikeBeginSpaceAttackRunDistance;
		private double _strikeBeginPlanetAttackRunDistance;
		private double _strikeBreakawayDistance;
		private int _strikeOffsetRecalculationTime;
		private bool _strikeEngageUseSafePlanetPathing;
		private bool _strikeEngageUseCollisionEvasionSpace;
		private bool _strikeEngageUseCollisionEvasionPlanet;

		private bool _engageOverrideWithDistanceAndTimer;
		private int _engageOverrideTimerTrigger;
		private double _engageOverrideDistance;


		private bool _defaultCollisionSettings = false;

		public DateTime LastOffsetCalculation;
		public DateTime EngageOverrideTimer;
		public bool TargetIsHigh;

		public byte Counter;

		private IBehavior _behavior;
		public BehaviorSubclass SubClass { get { return _subClass; } set { _subClass = value; } }
		private BehaviorSubclass _subClass;

		public string DefaultWeaponProfile { get { return _defaultWeaponProfile; } }
		private string _defaultWeaponProfile;

		public Strike(IBehavior behavior) {

			_subClass = BehaviorSubclass.Strike;
			_behavior = behavior;

			_defaultWeaponProfile = "MES-Weapons-GenericStandard";

			_strikeBeginSpaceAttackRunDistance = 75;
			_strikeBeginPlanetAttackRunDistance = 100;
			_strikeBreakawayDistance = 450;
			_strikeOffsetRecalculationTime = 30;
			_strikeEngageUseSafePlanetPathing = true;
			_strikeEngageUseCollisionEvasionSpace = true;
			_strikeEngageUseCollisionEvasionPlanet = false;

			_engageOverrideWithDistanceAndTimer = true;
			_engageOverrideTimerTrigger = 20;
			_engageOverrideDistance = 1200;

			LastOffsetCalculation = MyAPIGateway.Session.GameDateTime;
			EngageOverrideTimer = MyAPIGateway.Session.GameDateTime;

			Counter = 0;

		}

		public void ProcessBehavior() {

			if (MES_SessionCore.IsServer == false) {

				return;

			}

			bool skipEngageCheck = false;

			if (_behavior.Mode != BehaviorMode.Retreat && _behavior.BehaviorSettings.DoRetreat == true) {

				ChangeCoreBehaviorMode(BehaviorMode.Retreat);
				_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing, CheckEnum.Yes, CheckEnum.No);

			}

			//Init
			if (_behavior.Mode == BehaviorMode.Init) {

				if (!_behavior.AutoPilot.Targeting.HasTarget()) {

					ChangeCoreBehaviorMode(BehaviorMode.WaitingForTarget);

				} else {

					EngageOverrideTimer = MyAPIGateway.Session.GameDateTime;
					ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					CreateAndMoveToOffset();
					skipEngageCheck = true;

				}

			}

			//Waiting For Target
			if (_behavior.Mode == BehaviorMode.WaitingForTarget) {

				if (_behavior.AutoPilot.CurrentMode != _behavior.AutoPilot.UserCustomMode) {

					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.None, CheckEnum.No, CheckEnum.Yes);

				}

				if (_behavior.AutoPilot.Targeting.HasTarget()) {

					EngageOverrideTimer = MyAPIGateway.Session.GameDateTime;
					ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					CreateAndMoveToOffset();
					skipEngageCheck = true;
					_behavior.BehaviorTriggerA = true;

				} else if (_behavior.Despawn.NoTargetExpire == true) {

					_behavior.Despawn.Retreat();

				}

			}

			if (!_behavior.AutoPilot.Targeting.HasTarget() && _behavior.Mode != BehaviorMode.Retreat) {

				ChangeCoreBehaviorMode(BehaviorMode.WaitingForTarget);

			}

			//Approach Target
			if (_behavior.Mode == BehaviorMode.ApproachTarget && !skipEngageCheck) {

				double distance = _behavior.AutoPilot.InGravity() ? this.StrikeBeginPlanetAttackRunDistance : this.StrikeBeginSpaceAttackRunDistance;
				bool engageOverride = false;

				if (EngageOverrideWithDistanceAndTimer) {

					if (_behavior.AutoPilot.DistanceToCurrentWaypoint < EngageOverrideDistance) {

						var time = MyAPIGateway.Session.GameDateTime - EngageOverrideTimer;

						if (time.TotalSeconds > EngageOverrideTimerTrigger) {

							engageOverride = true;

						}

					} else {

						EngageOverrideTimer = MyAPIGateway.Session.GameDateTime;

					}
				
				}

				if (_behavior.BehaviorActionB) {

					engageOverride = true;
					_behavior.BehaviorActionB = false;

				}

				if ((engageOverride || _behavior.AutoPilot.DistanceToCurrentWaypoint <= distance) && _behavior.AutoPilot.Targeting.Target.Distance(_behavior.RemoteControl.GetPosition()) > this.StrikeBreakawayDistance && !_behavior.AutoPilot.IsAvoidingCollision()) {

					ChangeCoreBehaviorMode(BehaviorMode.EngageTarget);
					EngageOverrideTimer = MyAPIGateway.Session.GameDateTime;
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | (StrikeEngageUseSafePlanetPathing ? NewAutoPilotMode.PlanetaryPathing : NewAutoPilotMode.None) | NewAutoPilotMode.WaypointFromTarget);
					skipEngageCheck = true;
					_behavior.BehaviorTriggerB = true;

				}

				if (skipEngageCheck == false) {

					var timeSpan = MyAPIGateway.Session.GameDateTime - LastOffsetCalculation;

					if (timeSpan.TotalSeconds >= StrikeOffsetRecalculationTime) {

						skipEngageCheck = true;
						_behavior.AutoPilot.DebugDataA = "Offset Expire, Recalc";
						CreateAndMoveToOffset();

					}


					if (_behavior.AutoPilot.Data.ReverseOffsetDistAltAboveHeight) {

						if (TargetIsHigh && _behavior.AutoPilot.Targeting.Target.CurrentAltitude() < _behavior.AutoPilot.Data.ReverseOffsetHeight) {

							TargetIsHigh = false;
							_behavior.AutoPilot.DebugDataA = "Target is Low";
							CreateAndMoveToOffset();

						} else if (!TargetIsHigh && _behavior.AutoPilot.Targeting.Target.CurrentAltitude() > _behavior.AutoPilot.Data.ReverseOffsetHeight) {

							TargetIsHigh = true;
							_behavior.AutoPilot.DebugDataA = "Target is High";
							CreateAndMoveToOffset();

						}

					}
					

				}

			}

			//Engage Target
			if (_behavior.Mode == BehaviorMode.EngageTarget && !skipEngageCheck) {

				bool timeUp = false;

				if (_behavior.AutoPilot.Data.AttackRunMaxTimeTrigger > 0) {

					var time = MyAPIGateway.Session.GameDateTime - EngageOverrideTimer;

					if (time.TotalSeconds > _behavior.AutoPilot.Data.AttackRunMaxTimeTrigger) {

						timeUp = true;

					}

				}

				BehaviorLogger.Write("Strike: " + StrikeBreakawayDistance.ToString() + " - " + _behavior.AutoPilot.DistanceToInitialWaypoint, BehaviorDebugEnum.General);
				if (timeUp || _behavior.BehaviorActionA || _behavior.AutoPilot.DistanceToInitialWaypoint <= StrikeBreakawayDistance || (_behavior.AutoPilot.Data.Unused && _behavior.AutoPilot.Collision.VelocityResult.CollisionImminent())) {

					EngageOverrideTimer = MyAPIGateway.Session.GameDateTime;
					ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					CreateAndMoveToOffset();
					_behavior.BehaviorTriggerA = true;
					_behavior.BehaviorActionA = false;

				}
			
			}

			//Retreat
			if (_behavior.Mode == BehaviorMode.Retreat) {

				if (_behavior.Despawn.NearestPlayer?.Player?.Controller?.ControlledEntity?.Entity != null) {

					_behavior.AutoPilot.SetInitialWaypoint(_behavior.Despawn.GetRetreatCoords());

				}

			}

		}

		public void ChangeCoreBehaviorMode(BehaviorMode newMode) {

			_behavior.ChangeCoreBehaviorMode(newMode);

			if (_defaultCollisionSettings == true) {

				if (_behavior.Mode == BehaviorMode.EngageTarget) {

					this._behavior.AutoPilot.Data.Unused = UseEngageCollisionEvasion();

				} else {

					this._behavior.AutoPilot.Data.Unused = true;

				}

			}

		}

		private bool UseEngageCollisionEvasion() {

			return _behavior.AutoPilot.InGravity() ? this.StrikeEngageUseCollisionEvasionPlanet : this.StrikeEngageUseCollisionEvasionSpace;
		
		}


		private void ChangeOffsetAction() {

			return;
			if(_behavior.Mode == BehaviorMode.ApproachTarget)
				_behavior.AutoPilot.ReverseOffsetDirection(70);

		}

		private void CreateAndMoveToOffset() {

			_behavior.AutoPilot.OffsetWaypointGenerator(true);
			LastOffsetCalculation = MyAPIGateway.Session.GameDateTime;
			_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.OffsetWaypoint, CheckEnum.Yes, CheckEnum.No);

		}

		public void SetDefaultTags() {

			//Behavior Specific Defaults
			_behavior.AutoPilot.Data = ProfileManager.GetAutopilotProfile("RAI-Generic-Autopilot-Strike");
			_behavior.Despawn.UseNoTargetTimer = true;

			if (_behavior.AutoPilot.Collision == null)
				_behavior.AutoPilot.Collision = new Subsystems.AutoPilot.CollisionSystem(_behavior.RemoteControl, _behavior.AutoPilot);

			_behavior.AutoPilot.Collision.CollisionTimeTrigger = 5;

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

					//StrikeBeginSpaceAttackRunDistance
					if (tag.Contains("[StrikeBeginSpaceAttackRunDistance:") == true) {

						TagParse.TagDoubleCheck(tag, ref this._strikeBeginSpaceAttackRunDistance);

					}

					//StrikeBeginPlanetAttackRunDistance
					if (tag.Contains("[StrikeBeginPlanetAttackRunDistance:") == true) {

						TagParse.TagDoubleCheck(tag, ref this._strikeBeginPlanetAttackRunDistance);

					}

					//StrikeBreakawayDistance
					if (tag.Contains("[StrikeBreakawayDistance:") == true) {

						TagParse.TagDoubleCheck(tag, ref this._strikeBreakawayDistance);

					}

					//StrikeOffsetRecalculationTime
					if (tag.Contains("[StrikeOffsetRecalculationTime:") == true) {

						TagParse.TagIntCheck(tag, ref this._strikeOffsetRecalculationTime);

					}

					//StrikeEngageUseSafePlanetPathing
					if (tag.Contains("[StrikeEngageUseSafePlanetPathing:") == true) {

						TagParse.TagBoolCheck(tag, ref _strikeEngageUseSafePlanetPathing);

					}

				}

			}

		}

	}

}

