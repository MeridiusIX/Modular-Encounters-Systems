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

		public Passive(IBehavior behavior){

			_behavior = behavior;

			FighterEngageDistanceSpace = 300;
			FighterEngageDistancePlanet = 600;
			
			ReceivedEvadeSignal = false;
			ReceivedRetreatSignal = false;
			ReceivedExternalTarget = false;
			
			Counter = 0;

		}

		public void SetDefaultTags() {

			_behavior.AutoPilot.Collision.UseCollisionDetection = false;
			_behavior.Despawn.UsePlayerDistanceTimer = false;
			_behavior.AutoPilot.Targeting.Data.UseCustomTargeting = false;

		}

		public void ProcessBehavior() {

			//Nothing Here

		}

		public void InitTags() {
		
			//Nothing Here
		
		}

	}

}
	
