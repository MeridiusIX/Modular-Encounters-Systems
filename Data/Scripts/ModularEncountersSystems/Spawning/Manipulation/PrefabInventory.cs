using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;

namespace ModularEncountersSystems.Spawning.Manipulation {
	public static class PrefabInventory {

		public static void RemoveInventoryFromGrid(MyObjectBuilder_CubeGrid grid) {

			if (grid?.CubeBlocks == null)
				return;

			foreach (var block in grid.CubeBlocks)
				RemoveInventoryFromBlock(block);
		
		}

		public static void RemoveInventoryFromBlock(MyObjectBuilder_CubeBlock block) {

			if (block?.ComponentContainer?.Components != null) {

				foreach (var componentData in block.ComponentContainer.Components) {

					TryRemoveInventory(componentData);
					TryRemoveInventoryAggregate(componentData);
					
				}
			
			}
		
		}

		private static void TryRemoveInventoryAggregate(MyObjectBuilder_ComponentContainer.ComponentData componentData) {

			var invAgg = componentData?.Component as MyObjectBuilder_InventoryAggregate;

			if (invAgg?.Inventories != null) {

				foreach (var inv in invAgg.Inventories) {

					var inventory = inv as MyObjectBuilder_Inventory;

					if (inventory?.Items != null)
						inventory.Items.Clear();

				}

			}

		}

		private static void TryRemoveInventory(MyObjectBuilder_ComponentContainer.ComponentData componentData) {

			var inventory = componentData?.Component as MyObjectBuilder_Inventory;

			if (inventory?.Items != null)
				inventory.Items.Clear();

		}

	}

}
