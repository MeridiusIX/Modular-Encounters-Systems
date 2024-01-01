using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Spawning.Procedural.Builder;
using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.ObjectBuilders;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Procedural {

	[Flags]
	public enum RestrictedCellType {
	
		None = 0,
		Block = 1,
		ThrustExhaust = 1 << 1,
		
	}

	public enum BlockPlacementCategory {
	
		None,
		HullOutline,
		HullLayer,
		NacelleArm,
		NacelleOutline,
		NacelleLayer
	
	}

	public class ShipConstruct {

		public ShipRules Rules;

		public MyObjectBuilder_CubeGrid CubeGrid;

		internal Dictionary<Vector3I, MyObjectBuilder_CubeBlock> _blockMap;
		internal Dictionary<Vector3I, RestrictedCellType> _restrictedCells;

		public Vector3I Min;
		public Vector3I Max;
		public int _maxLengthZ;

		internal MyObjectBuilder_CubeBlock _lastPrimaryBlockPlaced;
		internal MyObjectBuilder_CubeBlock _lastMirroredBlockX;
		internal MyObjectBuilder_CubeBlock _lastMirroredBlockY;
		internal MyObjectBuilder_CubeBlock _lastMirroredBlockXY;

		private List<Vector3I> _tempCellList;
		private MyObjectBuilder_CubeBlock _tempBlock;
		private MyObjectBuilder_CubeGrid _tempGrid;

		private List<Vector3I> _stackedBlocksTemo;
		private List<Vector3I> _stackedBlocksSecond;

		public StringBuilder Log;

		public ShipConstruct(ShipRules rules) {

			Rules = rules;

			CubeGrid = new MyObjectBuilder_CubeGrid();
			CubeGrid.PositionAndOrientation = new VRage.MyPositionAndOrientation();
			CubeGrid.PersistentFlags |= MyPersistentEntityFlags2.CastShadows | MyPersistentEntityFlags2.InScene;

			_blockMap = new Dictionary<Vector3I, MyObjectBuilder_CubeBlock>();
			_restrictedCells = new Dictionary<Vector3I, RestrictedCellType>();

			Min = Vector3I.Zero;
			Max = Vector3I.Zero;

			_tempCellList = new List<Vector3I>();

			_stackedBlocksTemo = new List<Vector3I>();
			_stackedBlocksSecond = new List<Vector3I>();

			Log = new StringBuilder();

		}

		public MyObjectBuilder_CubeBlock GetBlock(Vector3I cell) {

			
			_blockMap.TryGetValue(cell, out _tempBlock);
			return _tempBlock;

		}

		public void GetBlocks(Vector3I min, Vector3I max, List<MyObjectBuilder_CubeBlock> blocks, bool xSymm, bool ySymm) {

			GetBlocks(min, max, blocks);

			if (xSymm) {

				var actualMin = CalculateSymmetryX(min, max, false);
				var actualMax = CalculateSymmetryX(min, max, true);
				GetBlocks(actualMin, actualMax, blocks);

			}

			if (ySymm) {

				var actualMin = CalculateSymmetryY(min, max, false);
				var actualMax = CalculateSymmetryY(min, max, true);
				GetBlocks(actualMin, actualMax, blocks);

			}

			if (xSymm && ySymm) {

				var actualMin = CalculateSymmetryXY(min, max, false);
				var actualMax = CalculateSymmetryXY(min, max, true);
				GetBlocks(actualMin, actualMax, blocks);

			}

		}

		public void GetBlocks(Vector3I min, Vector3I max, List<MyObjectBuilder_CubeBlock> blocks) {

			for (int x = min.X; x <= max.X; x++) {

				for (int y = min.Y; y <= max.Y; y++) {

					for (int z = min.Z; z <= max.Z; z++) {

						var cell = new Vector3I(x, y, z);
						var block = GetBlock(cell);

						if (block == null || blocks.Contains(block))
							continue;

						blocks.Add(block);

					}

				}

			}

		}

		public bool PlaceBlock(BlockCategory category, Vector3I min, Vector3I max, Vector3I refMin, bool useXSymmetry = false, bool useYSymmetry = false, RestrictedCellType allowedRestrictions = RestrictedCellType.None) {

			var refBlock = GetReferenceBlock(category, refMin);

			if (refBlock == null) {

				Log.Append("Block Definition Doesn't Exist").Append(" - ").Append(category.ToString()).Append(" - ").Append(min.ToString()).AppendLine();
				return false;

			}

			var id = refBlock.GetId();

			//Get Block Definition
			MyCubeBlockDefinition blockDef = null;

			if (!DefinitionHelper.AllBlockDefinitionsDictionary.TryGetValue(id, out blockDef)) {

				Log.Append("Block Definition Doesn't Exist").Append(" - ").Append(id.ToString()).Append(" - ").Append(min.ToString()).AppendLine();
				return false;

			}


			//Check if placement is possible
			if (!CanPlaceBlockAtMin(min, max, useXSymmetry, useYSymmetry, allowedRestrictions)) {

				Log.Append("Cannot Place At Min Coords").Append(" - ").Append(id.ToString()).Append(" - ").Append(min.ToString()).AppendLine();
				return false;

			}


			/*
			//Orientation
			MyBlockOrientation baseOrientation;

			if (BuilderTools.CubeShapedBlocks.Contains(id)) {

				baseOrientation = BuilderTools.DefaultOrientation;


			} else if (!BuilderTools.BlockCategoryPrefabReference.TryGetValue(id, out baseOrientation)){

				Log.Append("Block Not Found In Orientation Master Reference").Append(" - ").Append(id.ToString()).Append(" - ").Append(min.ToString()).AppendLine();
				return false;

			}
			
			
			var orientation = BuilderTools.RotateOrientation(baseOrientation, pitch, yaw, roll);
			*/

			//Main Block First
			CreateAndRegisterBlock(id, min, max, refBlock.BlockOrientation, ref _lastPrimaryBlockPlaced);

			if (useXSymmetry) {

				var actualMin = CalculateSymmetryX(min, max, false);
				var actualMax = CalculateSymmetryX(min, max, true);
				var newRefBlock = BuilderTools.GetSymmetryOrientation(this, category, refBlock, true, false);
				CreateAndRegisterBlock(id, actualMin, actualMax, newRefBlock.BlockOrientation, ref _lastMirroredBlockX);

			}

			if (useYSymmetry) {

				var actualMin = CalculateSymmetryY(min, max, false);
				var actualMax = CalculateSymmetryY(min, max, true);
				var newRefBlock = BuilderTools.GetSymmetryOrientation(this, category, refBlock, false, true);
				CreateAndRegisterBlock(id, actualMin, actualMax, newRefBlock.BlockOrientation, ref _lastMirroredBlockY);

			}

			if (useXSymmetry && useYSymmetry) {

				var actualMin = CalculateSymmetryXY(min, max, false);
				var actualMax = CalculateSymmetryXY(min, max, true);
				var newRefBlock = BuilderTools.GetSymmetryOrientation(this, category, refBlock, true, true);
				CreateAndRegisterBlock(id, actualMin, actualMax, newRefBlock.BlockOrientation, ref _lastMirroredBlockXY);

			}

			return true;

		}

		private bool CreateAndRegisterBlock(MyDefinitionId id, Vector3I min, Vector3I max, MyBlockOrientation orientation, ref MyObjectBuilder_CubeBlock lastBlock) {

			CreateCellList(min, max); //Symmetry is Fucked

			//Precheck
			foreach (var cell in _tempCellList) {

				if (_blockMap.ContainsKey(cell)) {

					Log.Append("Proposed Cell Overlapping Existing Block").Append(" - ").Append(id.ToString()).Append(" - ").Append(cell.ToString()).AppendLine();
					return false;

				}

				if (_restrictedCells.ContainsKey(cell)) {

					Log.Append("Proposed Cell Overlapping Restricted Cell").Append(" - ").Append(id.ToString()).Append(" - ").Append(cell.ToString()).AppendLine();
					return false;

				}

			}

			var block = CreateBlock(id, min, max, orientation);

			if (block == null) {

				Log.Append("Created Block Object Builder Null").Append(" - ").Append(id.ToString()).Append(" - ").Append(min.ToString()).AppendLine();
				return false;

			}

			block.BlockOrientation = orientation;
			block.Min = min;

			foreach (var cell in _tempCellList) {

				_blockMap.Add(cell, block);
				_restrictedCells.Add(cell, RestrictedCellType.Block);
				Log.Append("Cell Registered To BlockMap").Append(" - ").Append(cell.ToString()).AppendLine();

			}

			CubeGrid.CubeBlocks.Add(block);
			lastBlock = block;
			UpdateMinMax(min);
			UpdateMinMax(max);
			Log.Append("Block Placed Successfully").Append(" - ").Append(id.ToString()).Append(" - ").Append(min.ToString()).Append(" - ").Append(orientation.ToString()).AppendLine();
			return true;

		}

		internal void UpdateMinMax(Vector3I value) {

			if (value.X < Min.X)
				Min.X = value.X;

			if (value.Y < Min.Y)
				Min.Y = value.Y;

			if (value.Z < Min.Z)
				Min.Z = value.Z;

			if (value.X > Max.X)
				Max.X = value.X;

			if (value.Y > Max.Y)
				Max.Y = value.Y;

			if (value.Z > Max.Z)
				Max.Z = value.Z;

		}

		public MyObjectBuilder_CubeBlock CreateBlock(MyDefinitionId id, Vector3I min, Vector3I max, MyBlockOrientation orientation) {

			var newBlockBuilder = MyObjectBuilderSerializer.CreateNewObject(id);
			var block = newBlockBuilder as MyObjectBuilder_CubeBlock;

			if (block == null)
				return null;

			block.BlockOrientation = orientation;
			block.Min = min;

			return block;

		}

		public void CreateCellList(Vector3I min, Vector3I max) {

			_tempCellList.Clear();
			var actualMin = min;
			var actualMax = max;

			if (min == max) {

				_tempCellList.Add(min);
				return;

			}

			/*
			if (symmetryX && !symmetryY) {

				actualMin = CalculateSymmetryX(min, max, false);
				actualMax = CalculateSymmetryX(min, max, true);

			}

			if (!symmetryX && symmetryY) {

				actualMin = CalculateSymmetryY(min, max, false);
				actualMax = CalculateSymmetryY(min, max, true);

			}

			if (symmetryX && symmetryY) {

				actualMin = CalculateSymmetryXY(min, max, false);
				actualMax = CalculateSymmetryXY(min, max, true);

			}
			*/

			for (int x = actualMin.X; x <= actualMax.X; x++) {

				for (int y = actualMin.Y; y <= actualMax.Y; y++) {

					for (int z = actualMin.Z; z <= actualMax.Z; z++) {

						_tempCellList.Add(new Vector3I(x, y, z));

					}

				}

			}

		}

		public MyObjectBuilder_CubeBlock GetReferenceBlock(BlockCategory category, Vector3I min) {

			if (!BuilderTools.BlockCategoryPrefabReference.TryGetValue(category, out _tempGrid))
				return null;

			return BuilderTools.GetBlockAtMinPosition(min, _tempGrid);
		
		}

		private Vector3I CalculateSymmetryX(Vector3I min, Vector3I max, bool calcMax = false) {

			if (min == max)
				return new Vector3I(-min.X, min.Y, min.Z);

			var xSignInverted = Math.Sign(min.X) * -1;
			var localMaxX = max.X + (xSignInverted * min.X);
			var calculatedX = 0;

			if (!calcMax) {

				calculatedX = (min.X * -1) - localMaxX;
				return new Vector3I(calculatedX, min.Y, min.Z);

			} else {

				calculatedX = (max.X * -1) + localMaxX;
				return new Vector3I(calculatedX, max.Y, max.Z);

			}
			
		}

		private Vector3I CalculateSymmetryY(Vector3I min, Vector3I max, bool calcMax = false) {

			if (min == max)
				return new Vector3I(min.X, -min.Y, min.Z);

			var ySignInverted = Math.Sign(min.Y) * -1;
			var localMaxY = max.Y + (ySignInverted * min.Y);
			var calculatedY = 0;
			if (!calcMax) {

				calculatedY = (min.Y * -1) - localMaxY;
				return new Vector3I(min.X, calculatedY, min.Z);

			} else {

				calculatedY = (max.Y * -1) + localMaxY;
				return new Vector3I(min.X, calculatedY, max.Z);

			}

		}

		private Vector3I CalculateSymmetryXY(Vector3I min, Vector3I max, bool calcMax = false) {

			if (min == max)
				return new Vector3I(-min.X, -min.Y, min.Z);

			//Might be some broken shit down here for multi-cell blocks
			var xSignInverted = Math.Sign(min.X) * -1;
			var localMaxX = max.X + (xSignInverted * min.X);
			var calculatedX = 0;

			var ySignInverted = Math.Sign(min.Y) * -1;
			var localMaxY = max.Y + (ySignInverted * min.Y);
			var calculatedY = 0;

			if (!calcMax) {

				calculatedX = (min.X * -1) - localMaxX;
				calculatedY = (min.Y * -1) - localMaxY;
				return new Vector3I(calculatedX, calculatedY, min.Z);

			} else {

				calculatedX = (max.X * -1) + localMaxX;
				calculatedY = (max.Y * -1) + localMaxY;
				return new Vector3I(calculatedX, calculatedY, max.Z);

			}

		}

		public bool CanPlaceBlockAtMin(Vector3I min, Vector3I max, bool checkXSymmetry = false, bool checkYSymmetry = false, RestrictedCellType allowedRestrictions = RestrictedCellType.None) {

			if (min == max) {

				return CanPlaceBlockAtMin(min, checkXSymmetry, checkYSymmetry, allowedRestrictions);

			}

			for (int x = min.X; x <= max.X; x++) {

				for (int y = min.Y; y <= max.Y; y++) {

					for (int z = min.Z; z <= max.Z; z++) {
						
						var cell = min + new Vector3I(x, y, z);

						if (!CanPlaceBlockAtMin(cell, checkXSymmetry, checkYSymmetry, allowedRestrictions))
							return false;

					}

				}

			}

			return true;

		}

		public bool CanPlaceBlockAtMin(Vector3I min, bool checkXSymmetry = false, bool checkYSymmetry = false, RestrictedCellType allowedRestrictions = RestrictedCellType.None) {

			if (!CanPlaceBlockAtCell(min))
				return false;

			if (checkXSymmetry) {

				var newMin = min;
				newMin.X = -min.X;
				if (!CanPlaceBlockAtCell(newMin, allowedRestrictions))
					return false;

			}

			if (checkYSymmetry) {

				var newMin = min;
				newMin.Y = -min.Y;
				if (!CanPlaceBlockAtCell(newMin, allowedRestrictions))
					return false;

			}

			if (checkXSymmetry && checkYSymmetry) {
				
				var newMin = min;
				newMin.X = -min.X;
				newMin.Y = -min.Y;
				if (!CanPlaceBlockAtCell(newMin, allowedRestrictions))
					return false;

			}

			return true;

		}

		private bool CanPlaceBlockAtCell(Vector3I min, RestrictedCellType allowedRestictions = RestrictedCellType.None) {

			RestrictedCellType restriction = RestrictedCellType.None;

			if (_restrictedCells.TryGetValue(min, out restriction)) {

				if (restriction != RestrictedCellType.None && !allowedRestictions.HasFlag(restriction))
					return false;
			
			}
			
			/*
			if ((Math.Abs(min.X) + Rules.MaxOverageTolerance) > _maxWidthX)
				return false;

			if ((Math.Abs(min.Y) + Rules.MaxOverageTolerance) > _maxHeightY)
				return false;

			if ((Math.Abs(min.Z) + Rules.MaxOverageTolerance) > _maxLengthZ)
				return false;
			*/

			return true;

		}

		public List<Vector3I> BuildBlockStackList(Vector3I[] blocks, int stackLength, bool firstHalf = true, bool useAll = false) {

			_stackedBlocksTemo.Clear();

			if (stackLength % 2 != 0)
				return _stackedBlocksTemo;

			/*
			0 1
			1 2
			2 3

			3 4
			4 5
			5 6
			*/


			int start = (firstHalf ? 0 : blocks.Length / 2);
			int end = firstHalf ? blocks.Length / 2 : blocks.Length;

			if (useAll) {

				start = 0;
				end = blocks.Length;
			
			}

			for (int i = start; i < end; i++) {

				_stackedBlocksTemo.Add(blocks[i]);

			}

			return _stackedBlocksTemo;

		}

	}

}
