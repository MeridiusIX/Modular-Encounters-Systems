using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Procedural.Hull {
	public abstract class HullTypeBase {

		public ShipConstruct Construct { get { return _construct; } set { _construct = value; } }
		public ShipRules Rules { get { return _rules; } set { _rules = value; } }
		internal ShipConstruct _construct;
		internal ShipRules _rules;

		internal void BaseSetup(ShipRules rules) {

			_construct = new ShipConstruct(rules);
			_rules = rules;

		}

		public void SpawnCurrentConstruct(MatrixD matrix) {

			var prefab = MyDefinitionManager.Static.GetPrefabDefinition("MES-Prefab-ProceduralPrefabDebug");

			if (prefab?.CubeGrids == null || prefab.CubeGrids.Length == 0) {

				return;

			}

			prefab.CubeGrids[0] = Construct.CubeGrid;

			PrefabSpawner.PrefabSpawnDebug("MES-Prefab-ProceduralPrefabDebug", matrix);

		}

	}
}
