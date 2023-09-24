using VRage.Game;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Procedural.Builder {
	public struct LinePattern {

		public BlockCategory Category;

		public Vector3I[] LinePatternStart;
		public Vector3I LinePatternCenter;
		public Vector3I[] LinePatternEnd;

		public Vector3I?[] AllowedAdjacentStartBlocks;
		public Vector3I?[] RestrictedAdjacentEndBlocks;

		public LinePattern(string name) {

			LinePatternStart = null;
			LinePatternCenter = Vector3I.Zero;
			LinePatternEnd = null;
			AllowedAdjacentStartBlocks = null;
			RestrictedAdjacentEndBlocks = null;
			Category = BlockCategory.None;

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
				LinePatternStart = new Vector3I[] { new Vector3I(14, 1, 1), new Vector3I(8, 1, -3) };
				LinePatternCenter = new Vector3I(0, 1, 0);
				LinePatternEnd = new Vector3I[] { new Vector3I(8, 1, -1), new Vector3I(14, 1, 3) };
				return;

			}

		}

		public bool StartLine(ShipConstruct construct, LineData data, int index, Vector3I pos, Vector3I adjacentDir) {

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

		/*
		public bool EndLine(ShipConstruct contruct, LineData data, int index, Vector3I pos, Vector3I advDir, Vector3I adjacentDir) {
		
			
		
		}
		*/
	}

}
