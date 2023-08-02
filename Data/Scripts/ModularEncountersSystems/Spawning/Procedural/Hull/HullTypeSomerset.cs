using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Procedural.Hull {
	public class HullTypeSomerset : HullTypeBase, IHullType {

		internal List<int> _firstSequence;

		internal bool _thickCore;
		internal bool _useNacelles;

		internal bool _failedBuild;
		internal MyObjectBuilder_CubeBlock _tempBlockA;
		internal MyObjectBuilder_CubeBlock _tempBlockB;

		public HullTypeSomerset(ShipRules rules) {

			BaseSetup(rules);
			_firstSequence = new List<int>();
			_thickCore = MathTools.RandomBool();
			_useNacelles = MathTools.RandomBool();

		}

		public void GenerateShip(int steps = -1) {

			//Step 0
			InitialHullOutline();

			//Step 1
			FirstHullLayer();
		
		}

		public void ProcessStep(int step = 0) {
		
			
		
		}

		public void InitialHullOutline() {

			//Hull Outline Setup
			Vector3I currentPosition = Vector3I.Zero;
			Construct.Log.Clear();
			int rndNum = 0;

			//1: Origin:   1
			Construct.PlaceBlock(BlockCategory.Armor, Vector3I.Zero, Vector3I.Zero, new Vector3I(0, 1, 0));
			currentPosition.X += 1;

			//2: AngleOutA: 1-3
			BuilderTools.BuildDualBlockLine(Construct, BlockCategory.Armor, MathTools.RandomBetween(1, 4), -1, BuilderTools.IncrementX1Z1, BuilderTools.IncrementZ1, BuilderTools.IncrementX1Z1, Vector3I.Backward, new Vector3I(3, 1, 1), new Vector3I(0, 1, 0), true, false, ref currentPosition);

			//3: StraightA: 4-8
			BuilderTools.BuildStraightArmorLine(Construct, BlockCategory.Armor, new Vector3I(0, 1, 0), MathTools.RandomBetween(4, 9), BuilderTools.IncrementZ1, BuilderTools.IncrementX1, true, false, ref currentPosition);

			//4: AngleOutB: 2-4
			BuilderTools.BuildDualBlockLine(Construct, BlockCategory.Armor, MathTools.RandomBetween(2, 5), -1, BuilderTools.IncrementX1Z1, BuilderTools.IncrementZ1, BuilderTools.IncrementX1Z1, Vector3I.Backward, new Vector3I(3, 1, 1), new Vector3I(0, 1, 0), true, false, ref currentPosition);

			//5: StraightB: 4-10
			BuilderTools.BuildStraightArmorLine(Construct, BlockCategory.Armor, new Vector3I(0, 1, 0), MathTools.RandomBetween(4, 11), BuilderTools.IncrementZ1, BuilderTools.IncrementX1, true, false, ref currentPosition);

			//6: AngleOutC: 2-4
			BuilderTools.BuildDualBlockLine(Construct, BlockCategory.Armor, MathTools.RandomBetween(2, 5), -1, BuilderTools.IncrementX1Z1, BuilderTools.IncrementZ1, BuilderTools.IncrementX1Z1, Vector3I.Backward, new Vector3I(3, 1, 1), new Vector3I(0, 1, 0), true, false, ref currentPosition);

			//7: StraightC: 3-7 (Odd)
			BuilderTools.BuildStraightArmorLine(Construct, BlockCategory.Armor, new Vector3I(0, 1, 0), MathTools.ClampRandomOdd(MathTools.RandomBetween(3, 8), 3, 7), BuilderTools.IncrementZ1, BuilderTools.IncrementZ1, true, false, ref currentPosition);

			//8: AngleInD:  2-4
			BuilderTools.BuildDualBlockLine(Construct, BlockCategory.Armor, MathTools.RandomBetween(2, 5), 0, new Vector3I(-1,0,1), new Vector3I(-1,0,0), BuilderTools.IncrementZ1, Vector3I.Backward, new Vector3I(3, 1, 3), new Vector3I(0, 1, 0), true, false, ref currentPosition);

			//9: StraightD: 4-10
			BuilderTools.BuildStraightArmorLine(Construct, BlockCategory.Armor, new Vector3I(0, 1, 0), MathTools.RandomBetween(4, 11), BuilderTools.IncrementZ1, BuilderTools.IncrementX1, true, false, ref currentPosition);

			//End:      ???



		}

		public void FirstHullLayer() {

			//Setup
			Vector3I currentPosition = Vector3I.Zero;
			currentPosition.Y = 1;
			int scannedBlockCount = 0;
			
			//1: Origin Placement
			if (!_thickCore) 
				Construct.PlaceBlock(BlockCategory.Armor, currentPosition, currentPosition, new Vector3I(2, 1, 5), false, true);
			else
				BuilderTools.BuildStackedBlocks(Construct, BlockCategory.Armor, Vector3I.Up, currentPosition, false, true, new Vector3I(10, 1, 1), new Vector3I(14, 1, -3));

			//1: Next, Move 1 to the right and scan angle blocks
			currentPosition.X += 1;

			//2: Scan Angle Blocks
			scannedBlockCount = BuilderTools.CheckLine(Construct, BlockCategory.Armor, new Vector3I(3, 1, 1), BuilderTools.IncrementX1Z1, MathTools.Vector3IPlus(currentPosition, 0, -1, 0));

			//2: Place blocks on angle blocks
			if (!_thickCore)
				BuilderTools.BuildDualBlockLine(Construct, BlockCategory.Armor, scannedBlockCount, -1, BuilderTools.IncrementX1Z1, BuilderTools.IncrementZ1, BuilderTools.IncrementX1Z1, Vector3I.Backward, new Vector3I(3, 1, -3), new Vector3I(3, 1, -7), true, true, ref currentPosition);
			else
				BuilderTools.BuildDualStackedBlockLine(Construct, BlockCategory.Armor, scannedBlockCount, -1, BuilderTools.IncrementX1Z1, Vector3I.Up, BuilderTools.IncrementZ1, BuilderTools.IncrementX1Z1, Vector3I.Backward, true, true, 2, ref currentPosition, new Vector3I(35, 1, 5), new Vector3I(35, 1, -7), new Vector3I(39, 1, 5), new Vector3I(39, 1, -7));

			//3: Scan Line Minus 1
			scannedBlockCount = BuilderTools.CheckLine(Construct, BlockCategory.Armor, new Vector3I(0, 1, 0), BuilderTools.IncrementZ1, MathTools.Vector3IPlus(currentPosition, 0, -1, 0)) - 1;

			//3: Place blocks on armor cubes
			if (!_thickCore)
				BuilderTools.BuildStraightArmorLine(Construct, BlockCategory.Armor, new Vector3I(3, 1, 6), scannedBlockCount, BuilderTools.IncrementZ1, BuilderTools.IncrementX1Z1, true, true, ref currentPosition);
			else
				BuilderTools.BuildStraightStackedArmorLine(Construct, BlockCategory.Armor, scannedBlockCount, BuilderTools.IncrementZ1, Vector3I.Up, BuilderTools.IncrementX1Z1, true, true, ref currentPosition, new Vector3I(11, 1, 2), new Vector3I(15, 1, -2));

			//4: Scan Angle Blocks
			scannedBlockCount = BuilderTools.CheckLine(Construct, BlockCategory.Armor, new Vector3I(3, 1, 1), BuilderTools.IncrementX1Z1, MathTools.Vector3IPlus(currentPosition, 0, -1, 0));

			//4: Place blocks on angle blocks
			if (!_thickCore)
				BuilderTools.BuildDualBlockLine(Construct, BlockCategory.Armor, scannedBlockCount, 0, BuilderTools.IncrementX1Z1, Vector3I.Left, BuilderTools.IncrementX1Z1, Vector3I.Backward, new Vector3I(3, 1, -3), new Vector3I(3, 1, -7), true, true, ref currentPosition);
			else
				BuilderTools.BuildDualStackedBlockLine(Construct, BlockCategory.Armor, scannedBlockCount, 0, BuilderTools.IncrementX1Z1, Vector3I.Up, Vector3I.Left, BuilderTools.IncrementX1Z1, Vector3I.Backward, true, true, 2, ref currentPosition, new Vector3I(35, 1, 5), new Vector3I(35, 1, -7), new Vector3I(39, 1, 5), new Vector3I(39, 1, -7));

			//5: Scan Line Minus 1
			scannedBlockCount = BuilderTools.CheckLine(Construct, BlockCategory.Armor, new Vector3I(0, 1, 0), BuilderTools.IncrementZ1, MathTools.Vector3IPlus(currentPosition, 0, -1, 0)) - 1;

			//5: Place blocks on armor cubes
			if (!_thickCore)
				BuilderTools.BuildStraightArmorLine(Construct, BlockCategory.Armor, new Vector3I(3, 1, 6), scannedBlockCount, BuilderTools.IncrementZ1, BuilderTools.IncrementX1Z1, true, true, ref currentPosition);
			else
				BuilderTools.BuildStraightStackedArmorLine(Construct, BlockCategory.Armor, scannedBlockCount, BuilderTools.IncrementZ1, Vector3I.Up, BuilderTools.IncrementX1Z1, true, true, ref currentPosition, new Vector3I(11, 1, 2), new Vector3I(15, 1, -2));

			//6: Scan Angle Blocks
			scannedBlockCount = BuilderTools.CheckLine(Construct, BlockCategory.Armor, new Vector3I(3, 1, 1), BuilderTools.IncrementX1Z1, MathTools.Vector3IPlus(currentPosition, 0, -1, 0));

			//6: Place blocks on angle blocks
			if (!_thickCore)
				BuilderTools.BuildDualBlockLine(Construct, BlockCategory.Armor, scannedBlockCount, 0, BuilderTools.IncrementX1Z1, Vector3I.Left, BuilderTools.IncrementX1Z1, Vector3I.Backward, new Vector3I(3, 1, -3), new Vector3I(3, 1, -7), true, true, ref currentPosition);
			else
				BuilderTools.BuildDualStackedBlockLine(Construct, BlockCategory.Armor, scannedBlockCount, 0, BuilderTools.IncrementX1Z1, Vector3I.Up, Vector3I.Left, BuilderTools.IncrementX1Z1, Vector3I.Backward, true, true, 2, ref currentPosition, new Vector3I(35, 1, 5), new Vector3I(35, 1, -7), new Vector3I(39, 1, 5), new Vector3I(39, 1, -7));


		}

		public void ThrusterPlacement() {



		}

		public void InteriorPlacement() {



		}

		public void SystemsPlacement() {
		
			
		
		}

		public void GreebleHull() {
		
			
		
		}

		public void PaintingAndSkins() {



		}

	}

}
