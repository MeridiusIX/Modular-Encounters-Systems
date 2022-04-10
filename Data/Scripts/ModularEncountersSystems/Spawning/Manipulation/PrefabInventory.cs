using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
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
			List<MyObjectBuilder_CubeBlock> eligibleBlocks = new List<MyObjectBuilder_CubeBlock>();

			foreach (var lootProfile in profile.LootProfiles) {

				eligibleCargoContainers.Clear();
				eligibleBlocks.Clear();

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

						if (container != null) {

							if (!string.IsNullOrWhiteSpace(container.ContainerType)) {

								if (prefab.ClearedContainerTypes || !profile.ClearExistingContainerTypes)
									continue;
								else
									container.ContainerType = "";

							}

							if (lootProfile.MatchBlocksContainingName && (string.IsNullOrWhiteSpace(container.CustomName) || !container.CustomName.Contains(lootProfile.MatchedName)))
								continue;

							eligibleCargoContainers.Add(container);
							eligibleBlocks.Add(block);
							

						} else {

							var existingContainerType = StorageTools.GetContainerStorage(block.ComponentContainer, StorageTools.MesContainerTypeKey);

							if (!string.IsNullOrWhiteSpace(existingContainerType)) {

								if (prefab.ClearedContainerTypes || !profile.ClearExistingContainerTypes)
									continue;
								else
									StorageTools.ApplyCustomBlockStorage(block, StorageTools.MesContainerTypeKey, "");

							}

							var termBlock = block as MyObjectBuilder_TerminalBlock;

							if (lootProfile.MatchBlocksContainingName && (string.IsNullOrWhiteSpace(termBlock?.CustomName) || !termBlock.CustomName.Contains(lootProfile.MatchedName)))
								continue;

							eligibleBlocks.Add(block);

						}
							
					}

				}

				//TODO: Add Logging
				SpawnLogger.Write(string.Format("{0} Loot Profile Results: Containers = {1} // Blocks = {2}", lootProfile.ProfileSubtypeId, eligibleCargoContainers.Count, eligibleBlocks.Count), SpawnerDebugEnum.Manipulation);

				if (!prefab.ClearedContainerTypes || profile.ClearExistingContainerTypes)
					prefab.ClearedContainerTypes = true;

				if (eligibleBlocks.Count > 0) {

					int affectedBlocks = MathTools.RandomBetween(lootProfile.MinBlocks, lootProfile.MaxBlocks);

					for (int i = 0; i < affectedBlocks; i++) {

						var containerIndex = MathTools.RandomBetween(0, eligibleBlocks.Count);
						var block = eligibleBlocks[containerIndex];

						if (eligibleCargoContainers.Contains(block as MyObjectBuilder_CargoContainer)) {

							((MyObjectBuilder_CargoContainer)block).ContainerType = lootProfile.ContainerTypes[MathTools.RandomBetween(0, lootProfile.ContainerTypes.Count)];
							SpawnLogger.Write("Applying Container Type to Container Block", SpawnerDebugEnum.Manipulation);

						} else {

							StorageTools.ApplyCustomBlockStorage(block, StorageTools.MesContainerTypeKey, lootProfile.ContainerTypes[MathTools.RandomBetween(0, lootProfile.ContainerTypes.Count)]);
							SpawnLogger.Write("Applying ModStorage Container Type to Non-Container Block", SpawnerDebugEnum.Manipulation);

						}

						var termBlock = block as MyObjectBuilder_TerminalBlock;
						if (!string.IsNullOrWhiteSpace(termBlock?.CustomName) && lootProfile.AppendNameToBlock && !termBlock.CustomName.Contains(lootProfile.AppendedName))
							termBlock.CustomName += lootProfile.AppendedName;

						eligibleBlocks.RemoveAt(containerIndex);

						if (eligibleBlocks.Count == 0)
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
