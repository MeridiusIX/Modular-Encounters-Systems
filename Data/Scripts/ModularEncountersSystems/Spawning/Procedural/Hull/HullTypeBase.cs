using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Procedural.Hull {
	public abstract class HullTypeBase {

		public ShipConstruct Construct { get { return _construct; } set { _construct = value; } }
		private ShipConstruct _construct;

		private void BaseSetup(ShipRules rules) {

			_construct = new ShipConstruct(rules);

		}

		public void SpawnCurrentConstruct(Vector3D coords) {
		
			
		
		}

	}
}
