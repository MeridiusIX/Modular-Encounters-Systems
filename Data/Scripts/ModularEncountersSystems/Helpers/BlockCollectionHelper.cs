using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Logging;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.ModAPI;

namespace ModularEncountersSystems.Helpers {

	public static class BlockCollectionHelper {

		private static Dictionary<string, long> _factionData = new Dictionary<string, long>();

		public static void ChangeBlockOwnership(IMyCubeGrid cubeGrid, List<string> blockNames, List<string> factionNames) {

			if (blockNames.Count != factionNames.Count) {

				return;

			}
				

			Dictionary<string, long> nameToOwner = new Dictionary<string, long>();

			for (int i = 0; i < blockNames.Count; i++) {

				if (factionNames[i] == "Nobody") {

					if (!nameToOwner.ContainsKey(blockNames[i])) {

						BehaviorLogger.Write(" - Owner Collection 1: " + blockNames[i] + " / " + 0, BehaviorDebugEnum.Action);
						nameToOwner.Add(blockNames[i], 0);

					}

					continue;
				
				}

				long owner = -1;

				if (_factionData.TryGetValue(factionNames[i], out owner)) {

					if (!nameToOwner.ContainsKey(blockNames[i])) {

						BehaviorLogger.Write(" - Owner Collection 2: " + blockNames[i] + " / " + owner, BehaviorDebugEnum.Action);
						nameToOwner.Add(blockNames[i], owner);

					}

					continue;

				} else {

					IMyFaction faction = MyAPIGateway.Session.Factions.TryGetFactionByTag(factionNames[i]);
					var factOwner = FactionHelper.GetFactionOwner(faction);

					if (faction != null) {

						_factionData.Add(factionNames[i], factOwner);

						if (!nameToOwner.ContainsKey(blockNames[i])) {

							BehaviorLogger.Write(" - Owner Collection 3: " + blockNames[i] + " / " + factOwner, BehaviorDebugEnum.Action);
							nameToOwner.Add(blockNames[i], factOwner);

						}

					}
				
				}
			
			}

			var blockList = GetBlocksOfType<IMyTerminalBlock>(cubeGrid);

			foreach (var block in blockList) {

				if (block.CustomName == null)
					continue;

				long owner = -1;

				if (nameToOwner.TryGetValue(block.CustomName, out owner)) {

					var cubeBlock = block as MyCubeBlock;
					cubeBlock.ChangeOwner(owner, MyOwnershipShareModeEnum.Faction);
					cubeBlock.ChangeBlockOwnerRequest(owner, MyOwnershipShareModeEnum.Faction);
				
				}
			
			}

		}

		/*
		public static List<IMySlimBlock> GetAllBlocks(IMyCubeGrid cubeGrid) {

			List<IMySlimBlock> totalList = new List<IMySlimBlock>();
			List<IMySlimBlock> blockList = new List<IMySlimBlock>();
			//cubeGrid.GetBlocks(totalList);
			var gridGroup = MyAPIGateway.GridGroups.GetGroup(cubeGrid, GridLinkTypeEnum.Physical);

			foreach(var grid in gridGroup) {

				blockList.Clear();
				grid.GetBlocks(blockList);

				foreach (var block in blockList)
					if (!totalList.Contains(block))
						totalList.Add(block);

			}

			return totalList;

		}
		*/

		public static List<IMySlimBlock> GetAllBlocks(GridEntity grid, bool getLinkedGrids = true) {

			List<IMySlimBlock> totalList = new List<IMySlimBlock>();

			if (grid == null || !grid.ActiveEntity())
				return totalList;

			for (int i = grid.LinkedGrids.Count - 1; i >= 0; i--) {

				var link = GridManager.GetSafeGridFromIndex(i, grid.LinkedGrids);

				if (link == null)
					continue;

				if (!getLinkedGrids && link != grid)
					continue;

				for (int j = link.AllBlocks.Count - 1; j >= 0; j--) {

					//SpawnLogger.Write(link.CubeGrid.CustomName + " AllBlocks Count: " + link.AllBlocks.Count, SpawnerDebugEnum.Dev);
					var block = GetSafeBlockFromIndex(j, link.AllBlocks);

					if (block == null)
						continue;

					if (totalList.Contains(block))
						continue;

					totalList.Add(block);

				}

			}

			SpawnLogger.Write("GetAllBlocks Count: " + totalList.Count, SpawnerDebugEnum.Dev);
			return totalList;

		}

		public static BlockEntity GetSafeBlockFromIndex(int index, List<BlockEntity> list) {

			try {

				if (index < list.Count)
					return list[index];

			} catch (Exception) {



			}

			return null;

		}

		public static IMySlimBlock GetSafeBlockFromIndex(int index, List<IMySlimBlock> list) {

			try {

				if (index < list.Count)
					return list[index];

			} catch (Exception) {



			}

			return null;

		}

		public static List<IMyTerminalBlock> GetBlocksOfType<T>(IMyCubeGrid cubeGrid) where T : class {

			return GetBlocksOfType<T>(GridManager.GetGridEntity(cubeGrid));

		}

		public static List<IMyTerminalBlock> GetBlocksOfType<T>(GridEntity cubeGrid) where T : class {

			var resultList = new List<IMyTerminalBlock>();

			if (cubeGrid == null || !cubeGrid.ActiveEntity() && cubeGrid.LinkedGrids == null)
				return resultList;

			for (int i = cubeGrid.LinkedGrids.Count - 1; i >= 0; i--) {

				var grid = cubeGrid.LinkedGrids[i];

				for (int j = grid.AllTerminalBlocks.Count - 1; j >= 0; j--) {

					var terminalBlock = grid.AllTerminalBlocks[j];

					if (terminalBlock == null || !terminalBlock.ActiveEntity() || terminalBlock.Block as T == null)
						continue;

					resultList.Add(terminalBlock.Block);

				}

			}

			return resultList;

		}

		public static List<IMyTerminalBlock> GetBlocksWithNames(IMyCubeGrid cubeGrid, List<string> names) {

			var resultList = GetBlocksOfType<IMyTerminalBlock>(cubeGrid);

			for(int i = resultList.Count - 1; i >= 0; i--) {

				var tBlock = resultList[i];

				if(tBlock?.CustomName == null || !names.Contains(tBlock.CustomName)) {

					resultList.RemoveAt(i);
					continue;

				}

			}

			return resultList;

		}

		public static List<IMyRadioAntenna> GetGridAntennas(IMyCubeGrid cubeGrid) {

			var resultList = new List<IMyRadioAntenna>();
			var grid = GridManager.GetGridEntity(cubeGrid);

			if(grid == null || !grid.ActiveEntity())
				return resultList;

			for (int i = grid.LinkedGrids.Count - 1; i >= 0; i--) {

				var link = GridManager.GetSafeGridFromIndex(i, grid.LinkedGrids);

				if (link == null)
					continue;

				for (int j = link.Antennas.Count - 1; j >= 0; j--) {

					var antenna = GetSafeBlockFromIndex(j, link.Antennas);

					if (antenna == null || !antenna.ActiveEntity())
						continue;

					resultList.Add(antenna.Block as IMyRadioAntenna);

				}

			}
			
			return resultList;

		}
	
		public static List<IMyShipController> GetGridControllers(IMyCubeGrid cubeGrid){

			var resultList = new List<IMyShipController>();
			var grid = GridManager.GetGridEntity(cubeGrid);

			if (grid == null || !grid.ActiveEntity())
				return resultList;

			for (int i = grid.LinkedGrids.Count - 1; i >= 0; i--) {

				var link = GridManager.GetSafeGridFromIndex(i, grid.LinkedGrids);

				if (link == null)
					continue;

				for (int j = link.Antennas.Count - 1; j >= 0; j--) {

					var controller = GetSafeBlockFromIndex(j, link.Controllers);

					if (controller == null || !controller.ActiveEntity())
						continue;

					resultList.Add(controller.Block as IMyShipController);

				}

			}

			return resultList;

		}

	}

}
