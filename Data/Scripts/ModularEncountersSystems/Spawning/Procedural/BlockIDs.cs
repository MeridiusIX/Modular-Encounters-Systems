using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;

namespace ModularEncountersSystems.Spawning.Procedural {
	public static class BlockIDs {

		public static MyDefinitionId ArmorBlock = new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorBlock");
		public static MyDefinitionId ArmorSlope = new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorSlope");
		public static MyDefinitionId ArmorCorner = new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorCorner");
		public static MyDefinitionId ArmorInvCorner = new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorCornerInv");


	}
}
