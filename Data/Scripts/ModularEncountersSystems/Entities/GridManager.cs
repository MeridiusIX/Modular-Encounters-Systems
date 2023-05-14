using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Entities {
	public static class GridManager {

		public static List<GridEntity> Grids = new List<GridEntity>();
		public static Action UnloadEntities;

		public static List<MyDefinitionId> AllowedBlocks = new List<MyDefinitionId>();
		public static List<MyDefinitionId> RestrictedBlocks = new List<MyDefinitionId>();

		public static void LoadData() {

			MyAPIGateway.GridGroups.OnGridGroupCreated += OnGridGroupChanged;
			MyAPIGateway.GridGroups.OnGridGroupDestroyed += OnGridGroupChanged;
		
		}

		public static void GetBlocksFromGrid<T>(IMyCubeGrid grid, List<IMySlimBlock> blocks, bool getAttachedGrids = false) where T : class {

			lock(Grids) {

				for (int i = Grids.Count - 1; i >= 0; i--) {

					var cubeGrid = Grids[i];

					if (cubeGrid == null || !cubeGrid.ActiveEntity() || cubeGrid.CubeGrid != grid)
						continue;

					if (getAttachedGrids) {

						cubeGrid.RefreshSubGrids();

						//BehaviorLogger.Write("Linked Grids Weapon Scan: " + cubeGrid.LinkedGrids.Count, BehaviorDebugEnum.BehaviorSetup);
						lock (cubeGrid.LinkedGrids) {

							foreach (var linkedGrid in cubeGrid.LinkedGrids)
								GetBlocksFromGrid<T>(linkedGrid, blocks);

						}

					} else {

						GetBlocksFromGrid<T>(cubeGrid, blocks);

					}

					return;
				
				}
			
			}

			//Logger.Write("Could Not Get Grid For Blocks", BehaviorDebugEnum.BehaviorSetup);
		
		}

		public static void GetBlocksFromGrid<T>(GridEntity grid, List<IMySlimBlock> blocks) where T : class {

			if (grid == null || !grid.ActiveEntity())
				return;

			lock (grid.AllTerminalBlocks) {

				for (int j = grid.AllTerminalBlocks.Count - 1; j >= 0; j--) {

					var block = grid.AllTerminalBlocks[j];

					if (block == null || !block.ActiveEntity())
						continue;

					if (block.Block as T != null)
						blocks.Add(block.Block.SlimBlock);

				}

			}

		}

		public static IMyTerminalBlock GetBlockByEntityIds(long gridEntityId, long blockEntityId) {

			for (int i = Grids.Count - 1; i >= 0; i--) {

				var grid = GetSafeGridFromIndex(i);

				if (grid == null || !grid.ActiveEntity() || grid.CubeGrid.EntityId != gridEntityId)
					continue;

				for (int j = grid.AllTerminalBlocks.Count - 1; j >= 0; j--) {

					var block = grid.AllTerminalBlocks[j];

					if (block.Block.EntityId == blockEntityId)
						return block.Block;

				}

			}

			return null;

		}

		public static IMySlimBlock GetClosestBlockInDirection(MatrixD cameraMatrix, double distance) {

			var line = new LineD(cameraMatrix.Translation, cameraMatrix.Forward * 10000 + cameraMatrix.Translation);
			GridEntity thisGrid = null;

			var sb = new StringBuilder();

			foreach (var grid in GridManager.Grids) {

				if (!grid.ActiveEntity())
					continue;

				if (!grid.CubeGrid.WorldAABB.Intersects(ref line))
					continue;

				thisGrid = grid;
				break;

			}

			if (thisGrid == null) {

				return null;

			}

			var cell = thisGrid.CubeGrid.RayCastBlocks(line.From, line.To);

			if (!cell.HasValue) {

				return null;

			}

			return thisGrid.CubeGrid.GetCubeBlock(cell.Value);

		}

		public static GridEntity GetClosestGridInDirection(MatrixD cameraMatrix, double distance) {

			var line = new LineD(cameraMatrix.Translation, cameraMatrix.Forward * distance + cameraMatrix.Translation);

			for (int i = Grids.Count - 1; i >= 0; i--) {

				var grid = GetSafeGridFromIndex(i);

				if (grid == null || !grid.ActiveEntity())
					continue;

				if (grid.CubeGrid.WorldAABB.Contains(cameraMatrix.Translation) == ContainmentType.Contains || grid.CubeGrid.WorldAABB.Intersects(ref line))
					return grid;

			}

			return null;

		}

		public static GridEntity GetGridEntity(IMyCubeGrid cubeGrid, bool getNonActive = false) {

			if (cubeGrid == null)
				return null;

			for (int i = Grids.Count - 1; i >= 0; i--) {

				var grid = GetSafeGridFromIndex(i);

				if ((grid.ActiveEntity() || (getNonActive && !grid.Closed)) && grid.CubeGrid == cubeGrid)
					return grid;

			}

			return null;
		
		}

		public static GridEntity GetGridEntity(long id) {

			for (int i = Grids.Count - 1; i >= 0; i--) {

				var grid = Grids[i];

				if (grid.ActiveEntity() && grid.CubeGrid.EntityId == id)
					return grid;

			}

			return null;

		}

		public static void GetGridsWithinDistance(Vector3D coords, double distance, List<GridEntity> grids) {

			for (int i = Grids.Count - 1; i >= 0; i--) {

				var grid = GetSafeGridFromIndex(i);

				if (grid == null || !grid.ActiveEntity())
					continue;

				if (grid.Distance(coords) > distance)
					continue;

				grids.Add(grid);

			}

		}

		public static GridEntity GetSafeGridFromIndex(int index) {

			try {

				if(index < Grids.Count)
					return Grids[index];
			
			} catch (Exception) {
			
			
			
			}

			return null;
		
		}

		public static GridEntity GetSafeGridFromIndex(int index, List<GridEntity> list) {

			try {

				if (index < list.Count)
					return list[index];

			} catch (Exception) {



			}

			return null;

		}

		public static void OnGridGroupChanged(IMyGridGroupData data) {



		}

		public static bool ProcessBlock(IMySlimBlock block) {

			if (block == null) {

				return false;

			}

			if (AllowedBlocks.Contains(block.BlockDefinition.Id)) {

				return true;

			}
				
			var grid = block.CubeGrid as MyCubeGrid;

			if (grid == null) {

				return true;

			}
				
			if (RestrictedBlocks.Contains(block.BlockDefinition.Id)) {

				grid.RazeBlock(block.Min);
				return false;

			}

			if (block.BlockDefinition.Context?.ModId == null || !block.BlockDefinition.Context.ModId.Contains(".sbm")) {

				AllowedBlocks.Add(block.BlockDefinition.Id);
				return true;
			
			}

			var idString = block.BlockDefinition.Context.ModId.Replace(".sbm", "");

			bool badResult = false;

			if (block.FatBlock == null) {

				MyCube cube = null;

				if (!grid.TryGetCube(block.Min, out cube)) {

					return true;

				}

				foreach (var part in cube.Parts) {

					IMyModel model = part.Model;

					if (model == null) {

						continue;

					}


					var dummyDict = new Dictionary<string, IMyModelDummy>();
					var count = model.GetDummies(dummyDict);

					foreach (var dummy in dummyDict.Keys) {

						if (dummy.Contains("ModEncProtSys_") && !dummy.Contains(idString)) {

							badResult = true;
							break;

						}

					}

					if (badResult)
						break;

				}

			} else {

				IMyModel model = block.FatBlock.Model;

				if (model == null) {

					return true;

				}


				var dummyDict = new Dictionary<string, IMyModelDummy>();
				var count = model.GetDummies(dummyDict);

				foreach (var dummy in dummyDict.Keys) {

					if (dummy.Contains("ModEncProtSys_") && !dummy.Contains(idString)) {

						badResult = true;
						break;

					}

				}

			}

			if (badResult) {

				RestrictedBlocks.Add(block.BlockDefinition.Id);
				grid.RazeBlock(block.Min);
				return false;

			} else {

				AllowedBlocks.Add(block.BlockDefinition.Id);
				return true;

			}
		
		}

		public static void UnloadData() {

			try {

				if(MyAPIGateway.GridGroups != null)
					MyAPIGateway.GridGroups.OnGridGroupCreated -= OnGridGroupChanged;

				if (MyAPIGateway.GridGroups != null)
					MyAPIGateway.GridGroups.OnGridGroupDestroyed -= OnGridGroupChanged;

			} catch (Exception) {
			
				
			
			}
			
		}

	}
}
