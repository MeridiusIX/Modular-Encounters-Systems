using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Tasks;
using ModularEncountersSystems.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Watchers {

	public static class Cleaning {
		
		public static List<GridCleanupExemption> ExemptGrids = new List<GridCleanupExemption>();

		public static bool PendingGridsForRemoval = false;
		public static List<GridEntity> FlaggedForRemoval = new List<GridEntity>();

		public static int CleanupTimer = 0;

		public static void Setup() {

			//Get Cleanup Data and Init
			GridCleanupData.LoadData();

			TaskProcessor.Tick60.Tasks += CleanupProcess;
			
		}

		public static void CleanupProcess() {

			CleanupTimer++;

			if (CleanupTimer < Settings.General.NpcCleanupCheckTimerTrigger)
				return;

			CleanupTimer = 0;

			SpawnLogger.Write("Running Cleanup Process", SpawnerDebugEnum.CleanUp);

			for (int i = NpcManager.ActiveNpcs.Count - 1; i >= 0; i--) {

				var grid = NpcManager.ActiveNpcs[i];

				//Basic Checks
				if (!BasicCleanupChecks(grid)) {

					GridCleanupData.RemoveData(grid);
					continue;
				
				}

				//Economy Station Check
				if (grid.CubeGrid.IsStatic && IsKeenEconomyStation(grid)) {

					GridCleanupData.RemoveData(grid);
					continue;
				
				}

				//Drop Container
				if (!string.IsNullOrWhiteSpace(grid.CubeGrid.CustomName) && DefinitionHelper.DropContainerNames.Contains(grid.CubeGrid.CustomName)) {

					GridCleanupData.RemoveData(grid);
					continue;

				}

				//Safezone Check
				if (SafeZoneManager.IsPositionInSafeZone(grid.GetPosition())) {

					continue;

				}

				//Exemption Check
				bool gotExemptGrid = false;

				for (int j = ExemptGrids.Count - 1; j >= 0; j--) {

					if ((MyAPIGateway.Session.GameDateTime - ExemptGrids[j].StartTime).TotalSeconds >= ExemptGrids[j].Duration) {

						ExemptGrids.RemoveAt(j);
						continue;

					}

					if (grid == ExemptGrids[j].Grid) {

						gotExemptGrid = true;
						break;

					}
				
				}

				if (gotExemptGrid)
					continue;

				//Get Config and Cleanup Data
				var type = grid.GetSpawningTypeFromLinkedGrids();
				
				if (type == SpawningType.None) {

					continue;
				
				}

				var config = Settings.GetConfig(type);

				if (config != null && config != Settings.OtherNPCs && config.UseTypeDisownTimer) {

					if ((MyAPIGateway.Session.GameDateTime - grid.Npc.SpawnTime).TotalSeconds >= config.TypeDisownTimer) {

						SpawnLogger.Write(grid.Name() + ": SpawnType Has Been Disowned After " + config.TypeDisownTimer + " Seconds. It is now registered as OtherNPC", SpawnerDebugEnum.CleanUp);
						grid.Npc.SpawnType = SpawningType.OtherNPC;
						grid.Npc.Update();
						config = Settings.GetConfig(SpawningType.OtherNPC);

					}
				
				}

				var data = GridCleanupData.GetData(grid);

				if (config == null || data == null || config.UseCleanupSettings == false) {

					continue;
				
				}

				//MES Grids Only
				if (config.OnlyCleanNpcsFromMes && !grid.Npc.SpawnedByMES) {

					GridCleanupData.RemoveData(grid);
					continue;

				}

				//Unowned Filtering
				if (config.CleanupIncludeUnowned) {

					if (grid.Ownership.HasFlag(GridOwnershipEnum.NpcMajority) || grid.Ownership.HasFlag(GridOwnershipEnum.NpcMinority)) {

						continue;

					}

					if (!grid.Npc.SpawnedByMES) {

						continue;
					
					}

				}

				//Determine Block Count
				if (config.CleanupUseBlockLimit) {

					if (grid.AllBlocks.Count >= config.CleanupBlockLimitTrigger) {

						SpawnLogger.Write(grid.CubeGrid.CustomName + " Exceeds Block Count Allowed For NPCs. Marking For Removal.", SpawnerDebugEnum.CleanUp);
						RemoveGrid(grid);
						GridCleanupData.RemoveData(grid);

						if (grid.Npc != null)
							grid.DespawnSource = "CleanUp-BlockCount";

						continue;

					}
				
				}

				var player = PlayerManager.GetNearestPlayer(grid.GetPosition());
				var distance = player?.Distance(grid.GetPosition()) ?? 0;
				var powered = grid.IsPowered();
				var distanceTrigger = (!config.CleanupUnpoweredOverride || !powered) ? config.CleanupDistanceTrigger : config.CleanupUnpoweredDistanceTrigger;
				var timerTrigger = (!config.CleanupUnpoweredOverride || !powered) ? config.CleanupTimerTrigger : config.CleanupUnpoweredTimerTrigger;

				//Determine Distance
				if (config.CleanupUseDistance && player != null) {

					if (distance > distanceTrigger && !config.CleanupDistanceStartsTimer) {

						SpawnLogger.Write(grid.CubeGrid.CustomName + " Is Further Than Allowed Distance From Nearest Player. Marking For Removal.", SpawnerDebugEnum.CleanUp);
						RemoveGrid(grid);
						GridCleanupData.RemoveData(grid);

						if (grid.Npc != null)
							grid.DespawnSource = "CleanUp-Distance";

						continue;

					}
				
				}

				//Determine Timer
				if (config.CleanupUseTimer) {

					if (config.CleanupDistanceStartsTimer) {

						if (player != null && distance > distanceTrigger) {

							data.Timer += Settings.General.NpcCleanupCheckTimerTrigger;

						} else if (config.CleanupResetTimerWithinDistance) {

							data.Timer = 0;

						}

					} else {

						data.Timer += Settings.General.NpcCleanupCheckTimerTrigger;

					}

					if (data.Timer >= timerTrigger) {

						SpawnLogger.Write(grid.CubeGrid.CustomName + " Cleanup Timer Expired. Marking For Removal.", SpawnerDebugEnum.CleanUp);
						RemoveGrid(grid);
						GridCleanupData.RemoveData(grid);

						if (grid.Npc != null)
							grid.DespawnSource = "CleanUp-Timer";

						continue;

					}

				}

			}

		}


		public static bool BasicCleanupChecks(GridEntity grid) {

			if (!grid.ActiveEntity())
				return false;

			//Refresh Grid Links
			grid.RefreshSubGrids();

			for (int i = grid.LinkedGrids.Count - 1; i >= 0; i--) {

				var linkedGrid = GridManager.GetSafeGridFromIndex(i, grid.LinkedGrids);

				if (linkedGrid == null || !linkedGrid.ActiveEntity()) {

					continue;

				}

				//Check Ignore Flag
				if (linkedGrid.Npc != null) {

					if (linkedGrid.Npc.Attributes.IgnoreCleanup) {

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

		public static void ForceRemoveGrid(GridEntity grid, string reason = null) {

			grid.ForceRemove = true;
			SpawnLogger.Write(grid.CubeGrid.CustomName + " Force Remove Request. Marking For Removal. Reason: " + reason ?? "null", SpawnerDebugEnum.CleanUp, true);
			RemoveGrid(grid, true);
			GridCleanupData.RemoveData(grid);

			if (grid.Npc != null)
				grid.DespawnSource = reason != null ? reason : "ForceRemove";


		}

		public static bool IsKeenEconomyStation(GridEntity grid) {

			bool spawnedByOther = false;
			bool stationNameMatchesStyle = false;

			if (grid.Npc != null) {

				if (grid.Npc.KeenEconomyStation != BoolEnum.None) {

					return grid.Npc.KeenEconomyStation == BoolEnum.True ? true : false;

				}
					

				spawnedByOther = !grid.Npc.SpawnedByMES;

			} else {

				spawnedByOther = true;

			}

			stationNameMatchesStyle = IsGridNameEconomyPattern(grid.CubeGrid.CustomName);

			var result = spawnedByOther && stationNameMatchesStyle;

			if (grid.Npc != null) {

				grid.Npc.KeenEconomyStation = result ? BoolEnum.True : BoolEnum.False;

			}

			return result;
		
		}

		public static bool IsGridNameEconomyPattern(string gridName) {

			if (string.IsNullOrWhiteSpace(gridName))
				return false;

			if (gridName.StartsWith("Economy_MiningStation_") == true) {

				return true;

			}

			if (gridName.StartsWith("Economy_OrbitalStation_") == true) {

				return true;

			}

			if (gridName.StartsWith("Economy_Outpost_") == true) {

				return true;

			}

			if (gridName.StartsWith("Economy_SpaceStation_") == true) {

				return true;

			}

			var nameSplit = gridName.Split(' ');
			
			if (nameSplit.Length < 3) {

				return false;

			}

			if (FactionHelper.EconomyFactionTags.Contains(nameSplit[0]) == false) {

				return false;

			}

			if (FactionHelper.EconomyStationTypes.Contains(nameSplit[1]) == false) {

				return false;

			}

			long stationId = 0;

			if (long.TryParse(nameSplit[2], out stationId) == false) {

				return false;

			}

			return true;

		}

		public static void RemoveGrid(GridEntity grid, bool singleGrid = false) {

			if (!FlaggedForRemoval.Contains(grid))
				FlaggedForRemoval.Add(grid);

			if (singleGrid) {

				if (!PendingGridsForRemoval) {

					PendingGridsForRemoval = true;
					TaskProcessor.Tasks.Add(new GridCleanup());
					
				}

				return;

			}

			foreach (var gridEntity in grid.LinkedGrids) {

				if (gridEntity.Ownership.HasFlag(GridOwnershipEnum.PlayerMajority) || gridEntity.Ownership.HasFlag(GridOwnershipEnum.PlayerMinority)) {

					continue;
				
				}

				if (!PendingGridsForRemoval) {

					PendingGridsForRemoval = true;
					TaskProcessor.Tasks.Add(new GridCleanup());

				}

				if (!FlaggedForRemoval.Contains(gridEntity))
					FlaggedForRemoval.Add(gridEntity);

			}

		}

	}

}
