using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using Sandbox.ModAPI;
using System;

namespace ModularEncountersSystems.Behavior {

	public class HorseFighter : IBehaviorSubClass{

		//Configurable
		public double HorseFighterEngageDistanceSpace;
		public double HorseFighterEngageDistancePlanet;

		public double HorseFighterDisengageDistanceSpace;
		public double HorseFighterDisengageDistancePlanet;

		public int HorseFighterWaypointWaitTimeTrigger;
		public int HorseFighterWaypointAbandonTimeTrigger;

		public int HorseFighterTimeApproaching { get { return _horseFighterTimeApproaching != 0 ? _horseFighterTimeApproaching : _behavior.AutoPilot.Data.TargetApproachTimer; } }
		public int HorseFighterTimeEngaging { get { return _horseFighterTimeEngaging != 0 ? _horseFighterTimeEngaging : _behavior.AutoPilot.Data.TargetEngageTimer; } }

		private int _horseFighterTimeApproaching;
		private int _horseFighterTimeEngaging;

		public DateTime HorseFighterWaypointWaitTime;
		public DateTime HorseFighterWaypointAbandonTime;
		public DateTime HorseFighterModeSwitchTime;

		public bool FighterMode;

		private IBehavior _behavior;

		public byte Counter;

		public BehaviorSubclass SubClass { get { return _subClass; } set { _subClass = value; } }
		private BehaviorSubclass _subClass;

		public string DefaultWeaponProfile { get { return _defaultWeaponProfile; } }
		private string _defaultWeaponProfile;

		public HorseFighter(IBehavior behavior){

			_subClass = BehaviorSubclass.HorseFighter;
			_behavior = behavior;

			_defaultWeaponProfile = "MES-Weapons-GenericStandard";

			HorseFighterEngageDistanceSpace = 400;
			HorseFighterEngageDistancePlanet = 600;

			HorseFighterDisengageDistanceSpace = 600;
			HorseFighterDisengageDistancePlanet = 600;

			HorseFighterWaypointWaitTimeTrigger = 5;
			HorseFighterWaypointAbandonTimeTrigger = 30;

			_horseFighterTimeApproaching = 0;
			_horseFighterTimeEngaging = 0;

			HorseFighterWaypointWaitTime = MyAPIGateway.Session.GameDateTime;
			HorseFighterWaypointAbandonTime = MyAPIGateway.Session.GameDateTime;
			HorseFighterModeSwitchTime = MyAPIGateway.Session.GameDateTime;

			FighterMode = false;

			Counter = 0;

		}

		public void ProcessBehavior() {

			if(MES_SessionCore.IsServer == false) {

				return;

			}

			//Logger.Write(_behavior.Mode.ToString(), BehaviorDebugEnum.General);

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

				if(_behavior.AutoPilot.CurrentMode != _behavior.AutoPilot.UserCustomMode) {

					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.None, CheckEnum.No, CheckEnum.Yes);

				}

				if(_behavior.AutoPilot.Targeting.HasTarget()) {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					_behavior.AutoPilot.OffsetWaypointGenerator(true);
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.OffsetWaypoint, CheckEnum.Yes, CheckEnum.No);
					_behavior.BehaviorTriggerA = true;

				} else if(_behavior.Despawn.NoTargetExpire == true){
					
					_behavior.Despawn.Retreat();
					
				}

			}

			if(!_behavior.AutoPilot.Targeting.HasTarget() && _behavior.Mode != BehaviorMode.Retreat && _behavior.Mode != BehaviorMode.WaitingForTarget) {


				_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitingForTarget);
				_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.None, CheckEnum.No, CheckEnum.Yes);

			}

			//TimerThing
			var modeTime = MyAPIGateway.Session.GameDateTime - HorseFighterModeSwitchTime;

			if (modeTime.TotalSeconds > (FighterMode ? HorseFighterTimeEngaging : HorseFighterTimeApproaching)) {

				HorseFighterModeSwitchTime = MyAPIGateway.Session.GameDateTime;
				FighterMode = FighterMode ? false : true;
				BehaviorLogger.Write("HorseFighter Using Fighter Mode: " + FighterMode.ToString(), BehaviorDebugEnum.General);

			}

			//Approach
			if (_behavior.Mode == BehaviorMode.ApproachTarget) {

				if (FighterMode && _behavior.AutoPilot.DistanceToTargetWaypoint < (_behavior.AutoPilot.InGravity() ? HorseFighterEngageDistancePlanet : HorseFighterEngageDistanceSpace)) {

					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.Strafe | NewAutoPilotMode.WaypointFromTarget);
					_behavior.ChangeCoreBehaviorMode(BehaviorMode.EngageTarget);
					_behavior.BehaviorTriggerC = true;

				} else {

					var timeSpan = MyAPIGateway.Session.GameDateTime - this.HorseFighterWaypointAbandonTime;

					if (_behavior.AutoPilot.ArrivedAtOffsetWaypoint()) {

						_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitAtWaypoint);
						this.HorseFighterWaypointWaitTime = MyAPIGateway.Session.GameDateTime;
						_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.None, CheckEnum.No, CheckEnum.Yes);
						_behavior.BehaviorTriggerB = true;

					} else if (timeSpan.TotalSeconds >= this.HorseFighterWaypointAbandonTimeTrigger) {

						this.HorseFighterWaypointAbandonTime = MyAPIGateway.Session.GameDateTime;
						_behavior.AutoPilot.OffsetWaypointGenerator(true);

					} else if (_behavior.AutoPilot.IsWaypointThroughVelocityCollision()) {

						this.HorseFighterWaypointAbandonTime = MyAPIGateway.Session.GameDateTime;
						_behavior.AutoPilot.OffsetWaypointGenerator(true);

					}

				}

			}

			//Engage
			if (_behavior.Mode == BehaviorMode.EngageTarget) {

				bool outRange = false;

				if (FighterMode) {

					outRange = _behavior.AutoPilot.DistanceToTargetWaypoint > (_behavior.AutoPilot.InGravity() ? HorseFighterDisengageDistancePlanet : HorseFighterDisengageDistanceSpace);

				} else {

					outRange = true;

				}

				if (outRange) {

					_behavior.AutoPilot.OffsetWaypointGenerator(true);
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.OffsetWaypoint, CheckEnum.Yes, CheckEnum.No);
					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					_behavior.BehaviorTriggerA = true;


				}

			}

			//WaitAtWaypoint
			if (_behavior.Mode == BehaviorMode.WaitAtWaypoint) {

				var timeSpan = MyAPIGateway.Session.GameDateTime - this.HorseFighterWaypointWaitTime;

				if (timeSpan.TotalSeconds >= this.HorseFighterWaypointWaitTimeTrigger) {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					this.HorseFighterWaypointAbandonTime = MyAPIGateway.Session.GameDateTime;
					_behavior.AutoPilot.OffsetWaypointGenerator(true);
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.OffsetWaypoint, CheckEnum.Yes, CheckEnum.No);
					_behavior.BehaviorTriggerA = true;

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

			//Behavior Specific Defaults
			_behavior.AutoPilot.Data = ProfileManager.GetAutopilotProfile("RAI-Generic-Autopilot-HorseFighter");
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

					//HorseFighterEngageDistanceSpace
					if (tag.Contains("[HorseFighterEngageDistanceSpace:") == true) {

						TagParse.TagDoubleCheck(tag, ref this.HorseFighterEngageDistanceSpace);

					}

					//HorseFighterEngageDistancePlanet
					if (tag.Contains("[HorseFighterEngageDistancePlanet:") == true) {

						TagParse.TagDoubleCheck(tag, ref this.HorseFighterEngageDistancePlanet);

					}

					//HorseFighterDisengageDistanceSpace
					if (tag.Contains("[HorseFighterDisengageDistanceSpace:") == true) {

						TagParse.TagDoubleCheck(tag, ref this.HorseFighterDisengageDistanceSpace);

					}

					//HorseFighterDisengageDistancePlanet
					if (tag.Contains("[HorseFighterDisengageDistancePlanet:") == true) {

						TagParse.TagDoubleCheck(tag, ref this.HorseFighterDisengageDistancePlanet);

					}

					//HorseFighterWaypointWaitTimeTrigger
					if (tag.Contains("[HorseFighterWaypointWaitTimeTrigger:") == true) {

						TagParse.TagIntCheck(tag, ref this.HorseFighterWaypointWaitTimeTrigger);

					}

					//HorseFighterWaypointAbandonTimeTrigger
					if (tag.Contains("[HorseFighterWaypointAbandonTimeTrigger:") == true) {

						TagParse.TagIntCheck(tag, ref this.HorseFighterWaypointAbandonTimeTrigger);

					}

					//HorseFighterTimeApproaching
					if (tag.Contains("[HorseFighterTimeApproaching:") == true) {

						TagParse.TagIntCheck(tag, ref this._horseFighterTimeApproaching);

					}

					//HorseFighterTimeEngaging
					if (tag.Contains("[HorseFighterTimeEngaging:") == true) {

						TagParse.TagIntCheck(tag, ref this._horseFighterTimeEngaging);

					}

				}
				
			}

		}

	}

}
	
