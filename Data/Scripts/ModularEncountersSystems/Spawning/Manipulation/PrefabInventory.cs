using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Spawning.Profiles;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;

namespace ModularEncountersSystems.Spawning.Manipulation {
	public static class PrefabInventory {

		public static void ApplyLootProfiles(PrefabContainer prefab, ManipulationProfile profile) {

			List<MyObjectBuilder_CargoContainer> eligibleCargoContainers = new List<MyObjectBuilder_CargoContainer>();

			foreach (var lootProfile in profile.LootProfiles) {

				eligibleCargoContainers.Clear();

				if (lootProfile.Chance < 100 && !MathTools.RandomChance(lootProfile.Chance, 100))
					continue;

				if (lootProfile.ContainerTypes.Count == 0)
					continue;

				foreach (var grid in prefab.Prefab.CubeGrids) {

					if (grid?.CubeBlocks == null)
						continue;

					foreach (var block in grid.CubeBlocks) {

						if (block == null)
							continue;

						if (!lootProfile.ContainerBlockTypes.Contains(block.GetId()))
							continue;

						var container = block as MyObjectBuilder_CargoContainer;

						if (container == null)
							continue;

						if (!string.IsNullOrWhiteSpace(container.ContainerType)) {

							if (prefab.ClearedContainerTypes || !profile.ClearExistingContainerTypes)
								continue;
							else
								container.ContainerType = "";

						}
							

						if (lootProfile.MatchBlocksContainingName && (string.IsNullOrWhiteSpace(container.CustomName) || !container.CustomName.Contains(lootProfile.MatchedName)))
							continue;

						eligibleCargoContainers.Add(container);

					}

				}

				if (!prefab.ClearedContainerTypes || profile.ClearExistingContainerTypes)
					prefab.ClearedContainerTypes = true;

				if (eligibleCargoContainers.Count > 0) {

					int affectedBlocks = MathTools.RandomBetween(lootProfile.MinBlocks, lootProfile.MaxBlocks);

					for (int i = 0; i < affectedBlocks; i++) {

						var block = eligibleCargoContainers[MathTools.RandomBetween(0, eligibleCargoContainers.Count)];
						block.ContainerType = lootProfile.ContainerTypes[MathTools.RandomBetween(0, lootProfile.ContainerTypes.Count)];

						if (lootProfile.AppendNameToBlock)
							block.CustomName += lootProfile.AppendedName;

						if (eligibleCargoContainers.Count == 0)
							break;
					
					}
				
				}

			}

		}

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
