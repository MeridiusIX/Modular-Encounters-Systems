using ModularEncountersSystems.Spawning.Procedural.Builder;
using Sandbox.Definitions;
using Sandbox.Game;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Procedural.Hull {
	public abstract class HullTypeBase {

		public ShipConstruct Construct { get { return _construct; } set { _construct = value; } }
		public ShipRules Rules { get { return _rules; } set { _rules = value; } }
		internal ShipConstruct _construct;
		internal ShipRules _rules;

		internal List<LineData> _lineDataCollection;
		internal List<MyObjectBuilder_CubeBlock> _blockCollection;

		internal void BaseSetup(ShipRules rules) {

			_construct = new ShipConstruct(rules);
			_rules = rules;
			_lineDataCollection = new List<LineData>();
			_blockCollection = new List<MyObjectBuilder_CubeBlock>();

		}

		public void SpawnCurrentConstruct(MatrixD matrix, string prefabId) {

			var prefab = MyDefinitionManager.Static.GetPrefabDefinition(prefabId);

			if (prefab?.CubeGrids == null || prefab.CubeGrids.Length == 0) {

				MyVisualScriptLogicProvider.ShowNotificationToAll("Prefab Null", 5000);
				return;

			}

			var prefabBuilder = new MyObjectBuilder_PrefabDefinition();
			var gridList = new List<MyObjectBuilder_CubeGrid>();
			gridList.Add(Construct.CubeGrid.Clone() as MyObjectBuilder_CubeGrid);

			prefabBuilder.Description = prefabId;
			prefabBuilder.DisplayName = prefabId;
			prefabBuilder.Enabled = true;
			prefabBuilder.EnvironmentType = MyEnvironmentTypes.None;
			prefabBuilder.Icons = prefab.Icons;
			prefabBuilder.TooltipImage = prefab.TooltipImage;
			prefabBuilder.SubtypeName = prefabId;
			prefabBuilder.CubeGrids = gridList.ToArray();
			prefab.InitLazy(prefabBuilder);

			PrefabSpawner.PrefabSpawnDebug(prefabId, matrix);

		}

	}
}
