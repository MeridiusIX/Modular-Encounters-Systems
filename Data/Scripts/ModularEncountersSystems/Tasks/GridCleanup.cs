using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Watchers;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Tasks {

	public class GridCleanup : TaskItem, ITaskItem {

		public GridCleanup() {

			_tickTrigger = 10;
			_isValid = true;

		}

		public override void Run() {

			bool cleanedGrid = false;

			for (int i = Cleaning.FlaggedForRemoval.Count - 1; i >= 0; i--) {

				SpawnLogger.Write("Remaining Grids To Be Despawned: " + Cleaning.FlaggedForRemoval.Count, SpawnerDebugEnum.CleanUp);

				var grid = Cleaning.FlaggedForRemoval[i];

				if (!grid.ActiveEntity()) {

					SpawnLogger.Write("Grid [" + grid.GridName + "] Marked For Cleanup Already Removed Before MES Could Process It.", SpawnerDebugEnum.CleanUp);
					SpawnLogger.Write("Closed: " + grid.Closed + " / Physics: " + grid.HasPhysics, SpawnerDebugEnum.CleanUp);
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

				//Check For DespawnActions
				if (grid?.Npc?.DespawnActions != null) {

					foreach (var despawnAction in grid.Npc.DespawnActions) {

						despawnAction?.Invoke(cubeGrid, grid.DespawnSource);

					}

					grid.Npc.DespawnActions.Clear();

				}

				SpawnLogger.Write(grid.CubeGrid.CustomName + " Grid is being Cleaned.", SpawnerDebugEnum.CleanUp);

				if (!string.IsNullOrWhiteSpace(grid?.DespawnSource)) {

					SpawnLogger.Write(string.Format(" - Despawn Source: [{0}]", grid.DespawnSource), SpawnerDebugEnum.CleanUp);

				} else {

					SpawnLogger.Write(string.Format(" - Despawn Source: Unknown"), SpawnerDebugEnum.CleanUp);

				}

				if (!grid.ForceRemove) {

					bool abort = false;

					foreach (var owner in grid.CubeGrid.BigOwners) {

						var npcOwner = OwnershipHelper.IsNPC(owner);
						SpawnLogger.Write(string.Format(" - Grid [{0}] Majority Owner [{1}]. NPC Ownership: {2}", grid.CubeGrid.CustomName ?? "null", owner, npcOwner), SpawnerDebugEnum.CleanUp);

						if (!npcOwner)
							abort = true;

					}

					foreach (var owner in grid.CubeGrid.SmallOwners) {

						var npcOwner = OwnershipHelper.IsNPC(owner);
						SpawnLogger.Write(string.Format(" - Grid [{0}] Minority Owner [{1}]. NPC Ownership: {2}", grid.CubeGrid.CustomName ?? "null", owner, npcOwner), SpawnerDebugEnum.CleanUp);

						if (!npcOwner)
							abort = true;

					}

					if (abort) {

						SpawnLogger.Write(" - " + grid.CubeGrid.CustomName + " Has Non-NPC Ownership. Cleaning Aborted.", SpawnerDebugEnum.CleanUp);
						Cleaning.FlaggedForRemoval.RemoveAt(i);
						
						if(!grid.ForceRemove)
							break;

					}

				} else {



				}

				cubeGrid.DismountAllCockpits();
				grid.DisconnectSubgrids();

				using (cubeGrid.Pin()) {

					MyAPIGateway.Utilities.InvokeOnGameThread(() => { cubeGrid.Close(); });

				}

				cleanedGrid = true;
				Cleaning.FlaggedForRemoval.RemoveAt(i);
				SpawnLogger.Write(" - " + grid.CubeGrid.CustomName + " Grid is Closed.", SpawnerDebugEnum.CleanUp);

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
