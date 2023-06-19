using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;

namespace ModularEncountersSystems.Spawning.Procedural {
	public static class BlockIDs {

		public static MyDefinitionId ArmorBlock = new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorBlock");

		//Symm-ArmorRamp [X:2 Y:3 Z:1?]
		public static MyDefinitionId ArmorSlope = new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorSlope");

		//Symm-ArmorCorner [X:3 Y:3 Z:1?]
		public static MyDefinitionId ArmorCorner = new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorCorner");

		//Symm-ArmorCornerInv [X:3 Y:3 Z:1?]
		public static MyDefinitionId ArmorInvCorner = new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorCornerInv");


	}

}
