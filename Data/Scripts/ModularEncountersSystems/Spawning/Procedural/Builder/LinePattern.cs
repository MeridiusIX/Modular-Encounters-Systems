using VRage.Game;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Procedural.Builder {
	public class LinePattern {

		public BlockCategory Category;

		public Vector3I[] LinePatternStart;
		public Vector3I LinePatternCenter;
		public Vector3I[] LinePatternEnd;

		public Vector3I?[] AllowedAdjacentStartBlocks;
		public Vector3I?[] RestrictedAdjacentEndBlocks;

		internal MyObjectBuilder_CubeBlock _block;
		internal MyObjectBuilder_CubeBlock _refblock;

		public LinePattern(string name) {

			//Single Slopes and Corners
			if (name == "TopSingleSlopeLine") {

				Category = BlockCategory.Armor;
				LinePatternStart = new Vector3I[]{ new Vector3I(2,1,5) };
				LinePatternCenter = new Vector3I(0, 1, 0);
				LinePatternEnd = new Vector3I[] { new Vector3I(2, 1, 7) };
				return;

			}

			if (name == "TopSlopeCornerLine") {

				Category = BlockCategory.Armor;
				LinePatternStart = new Vector3I[] { new Vector3I(3, 1, -3), new Vector3I(3, 1, -7) };
				LinePatternCenter = new Vector3I(0, 1, 0);
				LinePatternEnd = new Vector3I[] { new Vector3I(3, 1, -5), new Vector3I(3, 1, -1) };
				AllowedAdjacentStartBlocks = new Vector3I?[] { new Vector3I(0, 1, 0), new Vector3I(2, 1, 5), new Vector3I(3, 1, -7) };
				RestrictedAdjacentEndBlocks = new Vector3I?[] { null, new Vector3I(3, 1, -5), new Vector3I(2, 1, 7) };
				return;

			}

			//Ramps and RampCorners
			if (name == "TopRampLine") {

				Category = BlockCategory.Armor;
				LinePatternStart = new Vector3I[] { new Vector3I(14, 1, 1), new Vector3I(10, 1, -3) };
				LinePatternCenter = new Vector3I(0, 1, 0);
				LinePatternEnd = new Vector3I[] { new Vector3I(10, 1, -1), new Vector3I(14, 1, 3) };
				return;

			}

			if (name == "TopRampCornerLine") {

				Category = BlockCategory.Armor;
				LinePatternStart = new Vector3I[] { new Vector3I(35, 1, -3), new Vector3I(31, 1, 5), new Vector3I(43, 1, -7), new Vector3I(39, 1, 1)};
				LinePatternCenter = new Vector3I(0, 1, 0);
				LinePatternEnd = new Vector3I[] {new Vector3I(39, 1, 3), new Vector3I(43, 1, -5), new Vector3I(31, 1, 7), new Vector3I(35, 1, -1) };
				AllowedAdjacentStartBlocks = new Vector3I?[] { new Vector3I(0, 1, 0), new Vector3I(2, 1, 5), new Vector3I(3, 1, -7), new Vector3I(43, 1, -7), new Vector3I(39, 1, 1) };
				RestrictedAdjacentEndBlocks = new Vector3I?[] { null, new Vector3I(3, 1, -5), new Vector3I(2, 1, 7), new Vector3I(14, 1, 3), new Vector3I(39, 1, 3)};
				return;

			}

		}

		public Vector3I GetNextBlock(ref int startIndex, ref int endIndex) {

			Vector3I result = LinePatternCenter;

			if (startIndex < LinePatternStart.Length) {

				result = LinePatternStart[startIndex];
				startIndex++;
				return result;

			}

			if (endIndex >= 0 && endIndex < LinePatternEnd.Length) {

				result = LinePatternEnd[endIndex];
				endIndex++;
				return result;

			}
			
			return result;
		
		}

		public bool StartLine(ShipConstruct construct, LineData data, int index, Vector3I pos, Vector3I adjacentDir) {

			//DoSomethingWithRestrictedBlocksInHere;

			if ((data.Length - index) < (LinePatternStart.Length + LinePatternEnd.Length))
				return false;

			if (AllowedAdjacentStartBlocks == null)
				return true;

			var adjacentBlock = construct.GetBlock(pos + adjacentDir);

			for (int i = 0; i < AllowedAdjacentStartBlocks.Length; i++) {

				if (!AllowedAdjacentStartBlocks[i].HasValue)
					if (adjacentBlock == null)
						return true;
					else
						continue;

				var block = construct.GetReferenceBlock(Category, AllowedAdjacentStartBlocks[i].Value);

				if (block == null || adjacentBlock == null)
					continue;

				if (block.GetId() == adjacentBlock.GetId()) {

					if (block.BlockOrientation == adjacentBlock.BlockOrientation)
						return true;
				
				}
			
			}

			return false;
		
		}

		public bool EndLine(ShipConstruct construct, LineData data, int index, Vector3I pos, Vector3I advDir, Vector3I adjacentDir) {

			if ((data.Length - index) <= LinePatternEnd.Length)
				return true;

			if (RestrictedAdjacentEndBlocks == null)
				return false;

			for (int i = 0; i < LinePatternEnd.Length; i++) {

				var checkPos = pos + adjacentDir + (advDir * i);
				_block = construct.GetBlock(checkPos);

				foreach (var restrictedBlock in RestrictedAdjacentEndBlocks) {

					if (!restrictedBlock.HasValue)
						if (_block == null)
							return true;
						else
							continue;

					if (_block == null)
						continue;

					_refblock = construct.GetReferenceBlock(Category, restrictedBlock.Value);

					if (_refblock == null)
						continue;

					if (_block.GetId() == _refblock.GetId())
						if (_block.BlockOrientation == _refblock.BlockOrientation)
							return true;
					
				
				}

			}

			return false;

		}

	}

}
