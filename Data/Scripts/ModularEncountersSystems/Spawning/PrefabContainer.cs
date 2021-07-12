using Sandbox.Definitions;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Game;

namespace ModularEncountersSystems.Spawning {
	public class PrefabContainer {

		public bool Valid;
		public string PrefabSubtypeId;
		public MyPrefabDefinition Prefab;
		public MyObjectBuilder_PrefabDefinition PrefabBuilder;
		public bool SpawningInProgress;
		public List<MyObjectBuilder_CubeGrid> GridList;
		public DateTime SpawnStartTime;
		public MySpawnGroupDefinition.SpawnGroupPrefab SpawnGroupPrefab;

		public PrefabContainer(string SubtypeId) {

			PrefabSubtypeId = SubtypeId;
			Prefab = MyDefinitionManager.Static.GetPrefabDefinition(SubtypeId);

			if (Prefab == null)
				return;

			Valid = true;
			PrefabBuilder = new MyObjectBuilder_PrefabDefinition();
			GridList = new List<MyObjectBuilder_CubeGrid>();

			PrefabBuilder.Description = SubtypeId;
			PrefabBuilder.DisplayName = SubtypeId;
			PrefabBuilder.Enabled = true;
			PrefabBuilder.EnvironmentType = MyEnvironmentTypes.None;
			PrefabBuilder.Icons = Prefab.Icons;
			PrefabBuilder.TooltipImage = Prefab.TooltipImage;
			PrefabBuilder.SubtypeName = SubtypeId;

		}

		public bool InitializePrefabForSpawn(string subtypeId) {

			var prefab = MyDefinitionManager.Static.GetPrefabDefinition(subtypeId);

			if (prefab == null)
				return false;

			GridList.Clear();

			foreach (var grid in prefab.CubeGrids) {

				var clonedGrid = grid.Clone() as MyObjectBuilder_CubeGrid;

				if (clonedGrid == null)
					return false;

				GridList.Add(clonedGrid);

			}

			PrefabBuilder.CubeGrids = GridList.ToArray();
			Prefab.InitLazy(PrefabBuilder);
			SpawningInProgress = true;
			SpawnStartTime = MyAPIGateway.Session.GameDateTime;

			return true;
		
		}

	}

}
