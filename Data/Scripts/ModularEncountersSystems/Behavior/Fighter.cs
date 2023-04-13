using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using Sandbox.ModAPI;

namespace ModularEncountersSystems.Behavior {

	//BehaviorTriggerA - Engaging Target
	//BehaviorTriggerB - Had been engaging target, now approaching target.


	public class Fighter : IBehaviorSubClass{

		//Configurable
		public double FighterEngageDistanceSpace {

			get {

				return _fighterEngageDistanceSpace > 0 ? _fighterEngageDistanceSpace : _behavior.AutoPilot.Data.EngageDistanceSpace;

			}

			set {

				_fighterEngageDistanceSpace = value;

			}
		
		}

		public double FighterEngageDistancePlanet {

			get {

				return _fighterEngageDistancePlanet > 0 ? _fighterEngageDistancePlanet : _behavior.AutoPilot.Data.EngageDistancePlanet;

			}

			set {

				_fighterEngageDistancePlanet = value;

			}

		}

		public double FighterDisengageDistanceSpace {

			get {

				return _fighterDisengageDistanceSpace > 0 ? _fighterDisengageDistanceSpace : _behavior.AutoPilot.Data.DisengageDistanceSpace;

			}

			set {

				_fighterDisengageDistanceSpace = value;

			}

		}

		public double FighterDisengageDistancePlanet {

			get {

				return _fighterDisengageDistancePlanet > 0 ? _fighterDisengageDistancePlanet : _behavior.AutoPilot.Data.DisengageDistancePlanet;

			}

			set {

				_fighterDisengageDistancePlanet = value;

			}

		}

		private double _fighterEngageDistanceSpace;
		private double _fighterEngageDistancePlanet;

		private double _fighterDisengageDistanceSpace;
		private double _fighterDisengageDistancePlanet;

		internal CoreBehavior _behavior;

		public byte Counter;

		public BehaviorSubclass SubClass { get { return _subClass; } set { _subClass = value; } }
		private BehaviorSubclass _subClass;

		public string DefaultWeaponProfile { get { return _defaultWeaponProfile; } }
		private string _defaultWeaponProfile;

		public Fighter(CoreBehavior behavior){

			_subClass = BehaviorSubclass.Fighter;
			_behavior = behavior;

			_fighterEngageDistanceSpace = -1;
			_fighterEngageDistancePlanet = -1;

			_fighterDisengageDistanceSpace = -1;
			_fighterDisengageDistancePlanet = -1;

			_defaultWeaponProfile = "MES-Weapons-GenericStandard";

			Counter = 0;

		}

		public void ProcessBehavior() {

			if(MES_SessionCore.IsServer == false) {

				return;

			}

			if(_behavior.Mode != BehaviorMode.Retreat && _behavior.BehaviorSettings.DoRetreat == true){

				_behavior.ChangeCoreBehaviorMode(BehaviorMode.Retreat);
				_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing, CheckEnum.Yes, CheckEnum.No);

			}
			
			if(_behavior.Mode == BehaviorMode.Init) {

				if(!_behavior.AutoPilot.Targeting.HasTarget()) {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitingForTarget);

				} else {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget, CheckEnum.Yes, CheckEnum.No);

				}

			}

			if(_behavior.Mode == BehaviorMode.WaitingForTarget) {

				if(_behavior.AutoPilot.CurrentMode != _behavior.AutoPilot.UserCustomModeIdle) {

					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.None, CheckEnum.No, CheckEnum.Yes);

				}

				if(_behavior.AutoPilot.Targeting.HasTarget()) {

					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget, CheckEnum.Yes, CheckEnum.No);

				} else if(_behavior.Despawn.NoTargetExpire == true){

					_behavior.Despawn.Retreat();
					
				}

			}

			if(!_behavior.AutoPilot.Targeting.HasTarget() && _behavior.Mode != BehaviorMode.Retreat && _behavior.Mode != BehaviorMode.WaitingForTarget) {


				_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitingForTarget);
				_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.None, CheckEnum.No, CheckEnum.Yes);

			}

			//Approach
			if (_behavior.Mode == BehaviorMode.ApproachTarget) {

				bool inRange = false;

				if (!_behavior.AutoPilot.InGravity() && _behavior.AutoPilot.DistanceToTargetWaypoint < this.FighterEngageDistanceSpace)
					inRange = true;

				if(_behavior.AutoPilot.InGravity() && _behavior.AutoPilot.DistanceToTargetWaypoint < this.FighterEngageDistancePlanet)
					inRange = true;

				if (inRange) {

					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.Strafe | NewAutoPilotMode.WaypointFromTarget, CheckEnum.Yes, CheckEnum.No);
					_behavior.ChangeCoreBehaviorMode(BehaviorMode.EngageTarget);
					_behavior.BehaviorTriggerA = true;

				}

			}

			//Engage
			if (_behavior.Mode == BehaviorMode.EngageTarget) {

				bool outRange = false;

				if (!_behavior.AutoPilot.InGravity() && _behavior.AutoPilot.DistanceToTargetWaypoint > this.FighterDisengageDistanceSpace)
					outRange = true;

				if (_behavior.AutoPilot.InGravity() && _behavior.AutoPilot.DistanceToTargetWaypoint > this.FighterDisengageDistancePlanet)
					outRange = true;

				if (outRange) {

					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget, CheckEnum.Yes, CheckEnum.No);
					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					_behavior.BehaviorTriggerB = true;

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

			_behavior.AutoPilot.Data = ProfileManager.GetAutopilotProfile("RAI-Generic-Autopilot-Fighter");
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
					
					//FighterEngageDistanceSpace
					if(tag.Contains("[FighterEngageDistanceSpace:") == true) {

						TagParse.TagDoubleCheck(tag, ref _fighterEngageDistanceSpace);

					}	
			
					//FighterEngageDistancePlanet
					if(tag.Contains("[FighterEngageDistancePlanet:") == true) {

						TagParse.TagDoubleCheck(tag, ref _fighterEngageDistancePlanet);

					}

					//FighterDisengageDistanceSpace
					if (tag.Contains("[FighterDisengageDistanceSpace:") == true) {

						TagParse.TagDoubleCheck(tag, ref _fighterDisengageDistanceSpace);

					}

					//FighterDisengageDistancePlanet
					if (tag.Contains("[FighterDisengageDistancePlanet:") == true) {

						TagParse.TagDoubleCheck(tag, ref _fighterDisengageDistancePlanet);

					}

				}
				
			}

		}

	}

}
	
