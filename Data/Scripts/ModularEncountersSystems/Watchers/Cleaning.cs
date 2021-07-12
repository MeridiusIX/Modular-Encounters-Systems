using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Tasks;
using ModularEncountersSystems.World;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Watchers {
	public static class Cleaning {

		public static bool PendingGridsForRemoval = false;
		public static List<GridEntity> FlaggedForRemoval = new List<GridEntity>();

		public static bool BasicCleanupChecks(GridEntity grid) {

			if (!grid.ActiveEntity())
				return false;

			//Refresh Grid Links
			grid.RefreshSubGrids();

			for (int i = grid.LinkedGrids.Count - 1; i >= 0; i--) {

				var linkedGrid = grid.LinkedGrids[i];

				if (!linkedGrid.ActiveEntity()) {

					continue;

				}

				//Check Ignore Flag
				if (linkedGrid.Npc != null) {

					if (linkedGrid.Npc.Attributes.HasFlag(NpcAttributes.IgnoreCleanup)) {

						SpawnLogger.Write(linkedGrid.CubeGrid.CustomName + " Fails Basic Cleanup. Has IgnoreCleanup Flag", SpawnerDebugEnum.CleanUp);
						return false;

					}

				}

				//Check Grid Ignore Flag
				if (linkedGrid.CubeGrid.CustomName != null && linkedGrid.CubeGrid.CustomName.Contains("[NPC-IGNORE]")) {

					SpawnLogger.Write(linkedGrid.CubeGrid.CustomName + " Fails Basic Cleanup. Has [NPC-IGNORE] Tag in Grid Name", SpawnerDebugEnum.CleanUp);
					return false;

				}


				//Check For Player Ownerships
				if (linkedGrid.Ownership.HasFlag(GridOwnershipEnum.PlayerMajority) || linkedGrid.Ownership.HasFlag(GridOwnershipEnum.PlayerMinority)) {

					SpawnLogger.Write(linkedGrid.CubeGrid.CustomName + " Fails Basic Cleanup. Has Partial or Full Player Ownership", SpawnerDebugEnum.CleanUp);
					return false;

				}
				
			}

			return true;
		
		}

		public static void RemoveGrid(GridEntity grid) {

			if (!FlaggedForRemoval.Contains(grid))
				FlaggedForRemoval.Add(grid);

			if (!PendingGridsForRemoval) {

				PendingGridsForRemoval = true;
				TaskProcessor.Tasks.Add(new GridCleanup());
			
			}


		}

	}

}
