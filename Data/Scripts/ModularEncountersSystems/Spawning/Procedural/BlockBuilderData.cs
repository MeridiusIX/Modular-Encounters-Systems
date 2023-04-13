using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Procedural {
	public class BlockBuilderData {

		public bool Valid;

		public MyDefinitionId Id;
		public MyDefinitionId HeavyId;

		public MyBlockOrientation PrimaryOrientation;
		public Vector3I PrimaryBlockCoords;

		public Dictionary<MyBlockOrientation, MyBlockOrientation> SymmetryX;
		public Dictionary<MyBlockOrientation, MyBlockOrientation> SymmetryY;

		public BlockBuilderData(MyDefinitionId id, string prefabIdX, string prefabIdY, byte blocks = 8, MyObjectBuilder_CubeBlock singleBlock = null) {

			PrimaryBlockCoords = new Vector3I(1, 1, -1);



		}

		private void ScanPrefab(string prefabId, Dictionary<MyBlockOrientation, MyBlockOrientation> dict) {

			var prefab = MyDefinitionManager.Static.GetPrefabDefinition(prefabId);

			if (prefab?.CubeGrids == null || prefab.CubeGrids.Length == 0)
				return;

			if (prefab.CubeGrids[0].CubeBlocks == null)
				return;

			dict = new Dictionary<MyBlockOrientation, MyBlockOrientation>();

			foreach (var block in prefab.CubeGrids[0].CubeBlocks) {
			
				
			
			}

			Valid = true;

		} 

	}

}
