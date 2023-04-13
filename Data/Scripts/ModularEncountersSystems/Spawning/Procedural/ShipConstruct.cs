using ModularEncountersSystems.Helpers;
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

	public class ShipConstruct {

		public ShipRules Rules;

		internal Dictionary<Vector3I, MyObjectBuilder_CubeBlock> _blockMap;
		internal Dictionary<Vector3I, RestrictedCellType> _restrictedCells;

		internal int _maxWidthX;
		internal int _maxHeightY;
		internal int _maxLengthZ;

		internal MyObjectBuilder_CubeBlock _lastPrimaryBlockPlaced;
		internal MyObjectBuilder_CubeBlock _lastMirroredBlockX;
		internal MyObjectBuilder_CubeBlock _lastMirroredBlockY;
		internal MyObjectBuilder_CubeBlock _lastMirroredBlockYX;

		private List<Vector3I> _tempCellList;

		public ShipConstruct(ShipRules rules) {

			Rules = rules;

			_blockMap = new Dictionary<Vector3I, MyObjectBuilder_CubeBlock>();
			_restrictedCells = new Dictionary<Vector3I, RestrictedCellType>();

			_maxWidthX = MathTools.RandomBetween(Rules.MinX, Rules.MaxX);
			_maxHeightY = MathTools.RandomBetween(Rules.MinY, Rules.MaxY);
			_maxLengthZ = MathTools.RandomBetween(Rules.MinZ, Rules.MaxZ);

			_tempCellList = new List<Vector3I>();

		}

		public bool PlaceBlock(MyDefinitionId id, Vector3I min, Vector3I max, MyBlockOrientation orientation, bool useXSymmetry = false, bool useYSymmetry = false, RestrictedCellType allowedRestrictions = RestrictedCellType.None) {

			//Get Block Definition
			MyCubeBlockDefinition blockDef = null;

			if (!DefinitionHelper.AllBlockDefinitionsDictionary.TryGetValue(id, out blockDef))
				return false;

			//Check if placement is possible
			if (CanPlaceBlockAtMin(min, max, useXSymmetry, useYSymmetry, allowedRestrictions))
				return false;

			//Create Blocks

			//Place block

			//Register placement in Block Map

			return true;

		}

		public void CreateAndRegisterBlock(MyDefinitionId id, Vector3I min, Vector3I max, MyBlockOrientation orientation) {

			var newBlockBuilder = MyObjectBuilderSerializer.CreateNewObject(id);
			var block = newBlockBuilder as MyObjectBuilder_CubeBlock;

			if (block == null)
				return;

			block.BlockOrientation = orientation;
			block.Min = min;

		}

		public void CreateCellList() {
		
			
		
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
				
			if ((Math.Abs(min.X) + Rules.MaxOverageTolerance) > _maxWidthX)
				return false;

			if ((Math.Abs(min.Y) + Rules.MaxOverageTolerance) > _maxHeightY)
				return false;

			if ((Math.Abs(min.Z) + Rules.MaxOverageTolerance) > _maxLengthZ)
				return false;

			return true;

		}

	}

}
