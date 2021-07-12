using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Watchers;
using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Tasks {

	public class GridCleanup : TaskItem, ITaskItem {

		public GridCleanup() {

			_tickTrigger = 10;
			_isValid = true;
		
		}

		public override void Run() {

			bool cleanedGrid = false;

			SpawnLogger.Write("Remaining Grids To Be Despawned: " + Cleaning.FlaggedForRemoval.Count, SpawnerDebugEnum.CleanUp);

			for (int i = Cleaning.FlaggedForRemoval.Count - 1; i >= 0; i--) {

				var grid = Cleaning.FlaggedForRemoval[i];

				if (!grid.ActiveEntity()) {

					SpawnLogger.Write("Previously Cleaned Grid Confirmed as Despawned. Removing From Queue.", SpawnerDebugEnum.CleanUp);
					Cleaning.FlaggedForRemoval.RemoveAt(i);
					continue;

				}

				if (grid.Npc != null) {

					if (grid.Npc.DespawnAttempts > 10) {

						SpawnLogger.Write(grid.CubeGrid.CustomName + " Attempted Despawn 10 Times. Aborting Process.", SpawnerDebugEnum.CleanUp);
						Cleaning.FlaggedForRemoval.RemoveAt(i);
						continue;

					} else {

						grid.Npc.DespawnAttempts++;

					}
				
				}

				var cubeGrid = grid.CubeGrid as MyCubeGrid;

				using (cubeGrid.Pin()) {

					SpawnLogger.Write(grid.CubeGrid.CustomName + " Grid is being Closed.", SpawnerDebugEnum.CleanUp, true);
					cubeGrid.Close();
					cleanedGrid = true;
					SpawnLogger.Write(grid.CubeGrid.CustomName + " Grid is Closed.", SpawnerDebugEnum.CleanUp);

				}

				break;

			}

			if (!cleanedGrid) {

				SpawnLogger.Write("All Grids in Despawn Queue Have Been Processed", SpawnerDebugEnum.CleanUp);
				_isValid = false;
				Cleaning.PendingGridsForRemoval = false;

			}
				

		}

	}

}
