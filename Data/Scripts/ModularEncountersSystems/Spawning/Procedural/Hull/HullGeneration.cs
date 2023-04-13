using ModularEncountersSystems.Spawning.Procedural;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;

namespace ModularEncountersSystems.Spawning.Procedural.Hull {

	

	public static class HullGeneration {

		//Somerset
		//Dorchester
		//Winter
		//Hawthorne
		//Wright
		//Rockland
		//Union

		public static MyDefinitionId ArmorBlock = new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorBlock");
		public static MyDefinitionId ArmorSlope = new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorSlope");
		public static MyDefinitionId ArmorCorner = new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorCorner");
		public static MyDefinitionId ArmorInvCorner = new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorCornerInv");


	}

}
