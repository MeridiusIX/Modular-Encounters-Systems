using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Spawning.Manipulation;
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
		public MyPrefabDefinition OriginalPrefab;
		public int OriginalPrefabIndex;
		public MyObjectBuilder_PrefabDefinition PrefabBuilder;
		public bool SpawningInProgress;
		public List<MyObjectBuilder_CubeGrid> GridList;
		public DateTime SpawnStartTime;
		public MySpawnGroupDefinition.SpawnGroupPrefab SpawnGroupPrefab;
		public int BlockCount;
		public double OriginalMass;
		public double CurrentMass;
		public BlockSizeEnum BlockSize;
		public bool ClearedContainerTypes;
		public bool RevertStorage;

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
			OriginalMass = 0;
			CurrentMass = 0;

		}

		public bool InitializePrefabForSpawn(string subtypeId, int index, List<MyObjectBuilder_CubeGrid> gridListOverride = null) {

			var prefab = MyDefinitionManager.Static.GetPrefabDefinition(subtypeId);
			OriginalPrefab = prefab;
			OriginalPrefabIndex = index;

			if (prefab?.CubeGrids == null || prefab.CubeGrids.Length == 0)
				return false;

			GridList.Clear();

			if (gridListOverride == null) {

				foreach (var grid in prefab.CubeGrids) {

					var clonedGrid = grid.Clone() as MyObjectBuilder_CubeGrid;

					if (clonedGrid == null)
						return false;

					GridList.Add(clonedGrid);

				}

			} else {

				foreach (var grid in gridListOverride) {

					var clonedGrid = grid.Clone() as MyObjectBuilder_CubeGrid;

					if (clonedGrid == null)
						return false;

					GridList.Add(clonedGrid);

				}

			}

			PrefabBuilder.CubeGrids = GridList.ToArray();
			Prefab.InitLazy(PrefabBuilder);
			SpawningInProgress = true;
			SpawnStartTime = MyAPIGateway.Session.GameDateTime;

			foreach (var grid in GridList) {

				OriginalMass += GeneralManipulations.GetGridMass(grid);

				if (grid.CubeBlocks != null)
					BlockCount += grid.CubeBlocks.Count;

			}

			CurrentMass = OriginalMass;

			return true;
		
		}

	}

}
