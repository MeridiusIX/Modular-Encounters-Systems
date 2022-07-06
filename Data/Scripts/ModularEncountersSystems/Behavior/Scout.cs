using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using Sandbox.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Behavior {

	public class Scout : CoreBehavior, IBehavior{

		//Configurable
		public double MaxDistanceFromTarget;
		public bool RotateToTargetWithinRange;
		public IBehavior _behavior;

		public byte Counter;
		public BehaviorSubclass SubClass { get { return _subClass; } set { _subClass = value; } }
		private BehaviorSubclass _subClass;
		public Scout(IBehavior behavior) : base() {

			_behavior = behavior;
			_subClass = BehaviorSubclass.Scout;
			//_behaviorType = "Scout";
			MaxDistanceFromTarget = 1500;
			RotateToTargetWithinRange = false;

			Counter = 0;

		}

		public override void MainBehavior() {

			if(MES_SessionCore.IsServer == false) {

				return;

			}

			base.MainBehavior();

			//Logger.Write(Mode.ToString(), BehaviorDebugEnum.General);

			if (Mode != BehaviorMode.Retreat && BehaviorSettings.DoRetreat == true){

				ChangeCoreBehaviorMode(BehaviorMode.Retreat);
				AutoPilot.ActivateAutoPilot(this.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing, CheckEnum.Yes, CheckEnum.No);

			}
			
			if(Mode == BehaviorMode.Init) {

				if(!AutoPilot.Targeting.HasTarget()) {

					ChangeCoreBehaviorMode(BehaviorMode.WaitingForTarget);

				} else {

					ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					AutoPilot.ActivateAutoPilot(this.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget, CheckEnum.Yes, CheckEnum.No);

				}

			}

			if(Mode == BehaviorMode.WaitingForTarget) {

				if(AutoPilot.CurrentMode != AutoPilot.UserCustomModeIdle) {

					AutoPilot.ActivateAutoPilot(this.RemoteControl.GetPosition(), NewAutoPilotMode.None, CheckEnum.No, CheckEnum.Yes);

				}

				if(AutoPilot.Targeting.HasTarget()) {

					ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					AutoPilot.ActivateAutoPilot(this.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget, CheckEnum.Yes, CheckEnum.No);

				} else if(Despawn.NoTargetExpire == true){
					
					Despawn.Retreat();
					
				}

			}

			if(!AutoPilot.Targeting.HasTarget() && Mode != BehaviorMode.Retreat && Mode != BehaviorMode.WaitingForTarget) {

				ChangeCoreBehaviorMode(BehaviorMode.WaitingForTarget);
				AutoPilot.ActivateAutoPilot(this.RemoteControl.GetPosition(), NewAutoPilotMode.None, CheckEnum.No, CheckEnum.Yes);

			}

			//Approach
			if (Mode == BehaviorMode.ApproachTarget) {

				bool inRange = false;

				if (!_behavior.AutoPilot.InGravity() && _behavior.AutoPilot.DistanceToTargetWaypoint < _behavior.AutoPilot.Data.EngageDistanceSpace)
					inRange = true;

				if (_behavior.AutoPilot.InGravity() && _behavior.AutoPilot.DistanceToTargetWaypoint < _behavior.AutoPilot.Data.EngageDistancePlanet)
					inRange = true;

				if (inRange) {

					if (_behavior.AutoPilot.Data.PadDistanceFromTarget > 0 && _behavior.AutoPilot.DistanceToTargetWaypoint < _behavior.AutoPilot.Data.PadDistanceFromTarget) {

						//Nothing

					} else {

						BehaviorTriggerA = true;
						ChangeCoreBehaviorMode(BehaviorMode.WaitAtWaypoint);
						var rotate = _behavior.AutoPilot.Data.RotateTowardsTargetWhileAtPosition ? NewAutoPilotMode.RotateToWaypoint : NewAutoPilotMode.None;
						AutoPilot.ActivateAutoPilot(this.RemoteControl.GetPosition(), rotate | NewAutoPilotMode.WaypointFromTarget, CheckEnum.Yes, CheckEnum.No);

					}

				}

			}

			//WaitAtWaypoint
			if (Mode == BehaviorMode.WaitAtWaypoint) {

				bool inRange = false;

				if (!_behavior.AutoPilot.InGravity() && _behavior.AutoPilot.DistanceToTargetWaypoint < _behavior.AutoPilot.Data.DisengageDistanceSpace)
					inRange = true;

				if (_behavior.AutoPilot.InGravity() && _behavior.AutoPilot.DistanceToTargetWaypoint < _behavior.AutoPilot.Data.DisengageDistancePlanet)
					inRange = true;

				if (inRange) {

					//Target Too Far
					BehaviorTriggerB = true;
					ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					AutoPilot.ActivateAutoPilot(this.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget, CheckEnum.Yes, CheckEnum.No);

				} else {

					//Target Too Close
					if (AutoPilot.Data.PadDistanceFromTarget > 0 && _behavior.AutoPilot.DistanceToTargetWaypoint < AutoPilot.Data.PadDistanceFromTarget) {

						BehaviorTriggerC = true;
						ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
						AutoPilot.ActivateAutoPilot(this.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing | NewAutoPilotMode.WaypointFromTarget, CheckEnum.Yes, CheckEnum.No);

					}

				}

			}

			//Retreat
			if (Mode == BehaviorMode.Retreat) {

				if (Despawn.NearestPlayer?.Player?.Controller?.ControlledEntity?.Entity != null) {

					_behavior.AutoPilot.SetInitialWaypoint(_behavior.Despawn.GetRetreatCoords());

				}

			}

		}

		public override void BehaviorInit(IMyRemoteControl remoteControl) {

			BehaviorLogger.Write("Beginning Behavior Init For Scout", BehaviorDebugEnum.General);

			//Core Setup
			//CoreSetup(remoteControl);

			//Behavior Specific Defaults
			AutoPilot.Data = ProfileManager.GetAutopilotProfile("RAI-Generic-Autopilot-Scout");
			Despawn.UseNoTargetTimer = true;
			
			if (string.IsNullOrWhiteSpace(_behavior.BehaviorSettings.WeaponsSystemProfile)) {

				_behavior.BehaviorSettings.WeaponsSystemProfile = "MES-Weapons-GenericStandard";

			}
			
			//Get Settings From Custom Data
			InitCoreTags();
			InitTags();
			SetDefaultTargeting();

			SetupCompleted = true;

		}

		public override string ToString() {

			return "";

		}

		public void InitTags() {

			if(string.IsNullOrWhiteSpace(this.RemoteControl?.CustomData) == false) {

				var descSplit = this.RemoteControl.CustomData.Split('\n');

				foreach(var tag in descSplit) {
					
					/*
					//FighterEngageDistanceSpace
					if(tag.Contains("[FighterEngageDistanceSpace:") == true) {

						this.FighterEngageDistanceSpace = TagHelper.TagDoubleCheck(tag, this.FighterEngageDistanceSpace);

					}	
			
					//FighterEngageDistancePlanet
					if(tag.Contains("[FighterEngageDistancePlanet:") == true) {

						this.FighterEngageDistancePlanet = TagHelper.TagDoubleCheck(tag, this.FighterEngageDistancePlanet);

					}

					//FighterDisengageDistanceSpace
					if (tag.Contains("[FighterDisengageDistanceSpace:") == true) {

						this.FighterDisengageDistanceSpace = TagHelper.TagDoubleCheck(tag, this.FighterDisengageDistanceSpace);

					}

					//FighterDisengageDistancePlanet
					if (tag.Contains("[FighterDisengageDistancePlanet:") == true) {

						this.FighterDisengageDistancePlanet = TagHelper.TagDoubleCheck(tag, this.FighterDisengageDistancePlanet);

					}
					*/

				}
				
			}

		}

	}

}
	
