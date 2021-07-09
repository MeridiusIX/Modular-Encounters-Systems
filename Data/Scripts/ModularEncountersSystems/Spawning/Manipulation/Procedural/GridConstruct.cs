using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRageMath;

namespace ModularEncountersSystems.Manipulation.Procedural {
	public class GridConstruct {

		MyObjectBuilder_CubeGrid GridBuilder;
		Dictionary<Vector3I, MyObjectBuilder_CubeBlock> BlockMap;


		public bool IsAtmoCapable;
		public float MaxGravity;

		public GridConstruct() {

			GridBuilder = null;
			BlockMap = new Dictionary<Vector3I, MyObjectBuilder_CubeBlock>();

		}

		public double CalculateGridMass() {

			return 0;
		
		}

		public double CalculateAvailableLift() {

			return 0;
		
		}

	}
}
