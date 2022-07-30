using ModularEncountersSystems.Helpers;

namespace ModularEncountersSystems.Behavior {

	public class Passive : IBehaviorSubClass{

		//Configurable
		public double FighterEngageDistanceSpace;
		public double FighterEngageDistancePlanet;
		
		public bool ReceivedEvadeSignal;
		public bool ReceivedRetreatSignal;
		public bool ReceivedExternalTarget;
		
		public byte Counter;
		IBehavior _behavior;
		public BehaviorSubclass SubClass { get { return _subClass; } set { _subClass = value; } }
		private BehaviorSubclass _subClass;

		public string DefaultWeaponProfile { get { return _defaultWeaponProfile; } }
		private string _defaultWeaponProfile;

		public Passive(IBehavior behavior){

			_subClass = BehaviorSubclass.Passive;
			_behavior = behavior;
			_defaultWeaponProfile = "MES-Weapons-GenericPassive";

			FighterEngageDistanceSpace = 300;
			FighterEngageDistancePlanet = 600;
			
			ReceivedEvadeSignal = false;
			ReceivedRetreatSignal = false;
			ReceivedExternalTarget = false;
			
			Counter = 0;

		}

		public void SetDefaultTags() {

			if (_behavior.AutoPilot.Collision == null)
				_behavior.AutoPilot.Collision = new Subsystems.AutoPilot.CollisionSystem(_behavior.RemoteControl, _behavior.AutoPilot);

			_behavior.AutoPilot.Collision.UseCollisionDetection = false;
			_behavior.Despawn.UsePlayerDistanceTimer = false;
			_behavior.AutoPilot.Targeting.Data.UseCustomTargeting = false;

			if (string.IsNullOrWhiteSpace(_behavior.BehaviorSettings.WeaponsSystemProfile)) {

				_behavior.BehaviorSettings.WeaponsSystemProfile = _defaultWeaponProfile;

			}
		}

		public void ProcessBehavior() {

			//Nothing Here

		}

		public override string ToString() {

			return "";

		}

		public void InitTags() {
		
			//Nothing Here
		
		}

	}

}
	
