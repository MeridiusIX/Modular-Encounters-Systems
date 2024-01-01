using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Spawning.Procedural.Builder;
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

		internal Vector3I _mainHullEndPoint;
		internal int _mainHullOuterWidth;

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

			//Step 2
			FillFirstHullLayer();

			//Step 3
			SecondHullLayer();
		
		}

		public void ProcessStep(int step = -1) {

			if (step == 0) {

				InitialHullOutline();
				return;

			}

			if (step == 1) {

				FirstHullLayer();
				return;

			}

			if (step == 2) {

				FillFirstHullLayer();
				return;

			}

			if (step == 3) {

				SecondHullLayer();
				return;

			}

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
			LineBuilder.BuildDualBlockLine(Construct, BlockCategory.Armor, MathTools.RandomBetween(1, 4), -1, BuilderTools.IncrementX1Z1, BuilderTools.IncrementZ1, BuilderTools.IncrementX1Z1, Vector3I.Backward, new Vector3I(3, 1, 1), new Vector3I(0, 1, 0), true, false, ref currentPosition);

			//3: StraightA: 4-8
			LineBuilder.BuildStraightArmorLine(Construct, BlockCategory.Armor, new Vector3I(0, 1, 0), MathTools.RandomBetween(4, 9), BuilderTools.IncrementZ1, BuilderTools.IncrementX1, true, false, ref currentPosition);

			//4: AngleOutB: 2-4
			LineBuilder.BuildDualBlockLine(Construct, BlockCategory.Armor, MathTools.RandomBetween(2, 5), -1, BuilderTools.IncrementX1Z1, BuilderTools.IncrementZ1, BuilderTools.IncrementX1Z1, Vector3I.Backward, new Vector3I(3, 1, 1), new Vector3I(0, 1, 0), true, false, ref currentPosition);

			//5: StraightB: 4-10
			LineBuilder.BuildStraightArmorLine(Construct, BlockCategory.Armor, new Vector3I(0, 1, 0), MathTools.RandomBetween(4, 11), BuilderTools.IncrementZ1, BuilderTools.IncrementX1, true, false, ref currentPosition);

			//6: AngleOutC: 2-4
			LineBuilder.BuildDualBlockLine(Construct, BlockCategory.Armor, MathTools.RandomBetween(2, 5), -1, BuilderTools.IncrementX1Z1, BuilderTools.IncrementZ1, BuilderTools.IncrementX1Z1, Vector3I.Backward, new Vector3I(3, 1, 1), new Vector3I(0, 1, 0), true, false, ref currentPosition);

			//7: StraightC: 3-7 (Odd)
			LineBuilder.BuildStraightArmorLine(Construct, BlockCategory.Armor, new Vector3I(0, 1, 0), MathTools.ClampRandomOdd(MathTools.RandomBetween(3, 8), 3, 7), BuilderTools.IncrementZ1, BuilderTools.IncrementZ1, true, false, ref currentPosition);
			_mainHullOuterWidth = currentPosition.X;

			//8: AngleInD:  2-4
			var randNum = MathTools.RandomBetween(2, 5);
			if (_mainHullOuterWidth - randNum <= 1) {

				randNum--;

			}

			LineBuilder.BuildDualBlockLine(Construct, BlockCategory.Armor, randNum, 0, new Vector3I(-1,0,1), new Vector3I(-1,0,0), BuilderTools.IncrementZ1, Vector3I.Backward, new Vector3I(3, 1, 3), new Vector3I(0, 1, 0), true, false, ref currentPosition);

			//9: StraightD: 4-10
			LineBuilder.BuildStraightArmorLine(Construct, BlockCategory.Armor, new Vector3I(0, 1, 0), MathTools.RandomBetween(4, 11), BuilderTools.IncrementZ1, BuilderTools.IncrementZ1, true, false, ref currentPosition);

			//10: Finishing End of Ship
			_mainHullEndPoint = new Vector3I(0, 0, currentPosition.Z);
			var hullEndPointTemp = _mainHullEndPoint;
			LineBuilder.BuildStraightArmorLine(Construct, BlockCategory.Armor, new Vector3I(0, 1, 0), currentPosition.X, BuilderTools.IncrementX1, BuilderTools.IncrementX1, true, false, ref hullEndPointTemp);
			Construct.PlaceBlock(BlockCategory.Armor, hullEndPointTemp, hullEndPointTemp, new Vector3I(3, 1, 3), true);


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
				LineBuilder.BuildStackedBlocks(Construct, BlockCategory.Armor, Vector3I.Up, currentPosition, false, true, new Vector3I(10, 1, 1), new Vector3I(14, 1, -3));

			//1: Next, Move 1 to the right and scan angle blocks
			currentPosition.X += 1;

			//2: Scan Angle Blocks
			scannedBlockCount = PatternSearch.CheckLine(Construct, BlockCategory.Armor, new Vector3I(3, 1, 1), BuilderTools.IncrementX1Z1, MathTools.Vector3IPlus(currentPosition, 0, -1, 0));

			//2: Place blocks on angle blocks
			if (!_thickCore)
				LineBuilder.BuildDualBlockLine(Construct, BlockCategory.Armor, scannedBlockCount, -1, BuilderTools.IncrementX1Z1, BuilderTools.IncrementZ1, BuilderTools.IncrementX1Z1, Vector3I.Backward, new Vector3I(3, 1, -3), new Vector3I(3, 1, -7), true, true, ref currentPosition);
			else
				LineBuilder.BuildDualStackedBlockLine(Construct, BlockCategory.Armor, scannedBlockCount, -1, BuilderTools.IncrementX1Z1, Vector3I.Up, BuilderTools.IncrementZ1, BuilderTools.IncrementX1Z1, Vector3I.Backward, true, true, 2, ref currentPosition, new Vector3I(35, 1, 5), new Vector3I(35, 1, -7), new Vector3I(39, 1, 5), new Vector3I(39, 1, -7));

			//3: Scan Line Minus 1
			scannedBlockCount = PatternSearch.CheckLine(Construct, BlockCategory.Armor, new Vector3I(0, 1, 0), BuilderTools.IncrementZ1, MathTools.Vector3IPlus(currentPosition, 0, -1, 0)) - 1;

			//3: Place blocks on armor cubes
			if (!_thickCore)
				LineBuilder.BuildStraightArmorLine(Construct, BlockCategory.Armor, new Vector3I(3, 1, 6), scannedBlockCount, BuilderTools.IncrementZ1, BuilderTools.IncrementX1Z1, true, true, ref currentPosition);
			else
				LineBuilder.BuildStraightStackedArmorLine(Construct, BlockCategory.Armor, scannedBlockCount, BuilderTools.IncrementZ1, Vector3I.Up, BuilderTools.IncrementX1Z1, true, true, ref currentPosition, new Vector3I(11, 1, 2), new Vector3I(15, 1, -2));

			//4: Scan Angle Blocks
			scannedBlockCount = PatternSearch.CheckLine(Construct, BlockCategory.Armor, new Vector3I(3, 1, 1), BuilderTools.IncrementX1Z1, MathTools.Vector3IPlus(currentPosition, 0, -1, 0));

			//4: Place blocks on angle blocks
			if (!_thickCore)
				LineBuilder.BuildDualBlockLine(Construct, BlockCategory.Armor, scannedBlockCount, 0, BuilderTools.IncrementX1Z1, Vector3I.Left, BuilderTools.IncrementX1Z1, Vector3I.Backward, new Vector3I(3, 1, -3), new Vector3I(3, 1, -7), true, true, ref currentPosition);
			else
				LineBuilder.BuildDualStackedBlockLine(Construct, BlockCategory.Armor, scannedBlockCount, 0, BuilderTools.IncrementX1Z1, Vector3I.Up, Vector3I.Left, BuilderTools.IncrementX1Z1, Vector3I.Backward, true, true, 2, ref currentPosition, new Vector3I(35, 1, 5), new Vector3I(35, 1, -7), new Vector3I(39, 1, 5), new Vector3I(39, 1, -7));

			//5: Scan Line Minus 1
			scannedBlockCount = PatternSearch.CheckLine(Construct, BlockCategory.Armor, new Vector3I(0, 1, 0), BuilderTools.IncrementZ1, MathTools.Vector3IPlus(currentPosition, 0, -1, 0)) - 1;

			//5: Place blocks on armor cubes
			if (!_thickCore)
				LineBuilder.BuildStraightArmorLine(Construct, BlockCategory.Armor, new Vector3I(3, 1, 6), scannedBlockCount, BuilderTools.IncrementZ1, BuilderTools.IncrementX1Z1, true, true, ref currentPosition);
			else
				LineBuilder.BuildStraightStackedArmorLine(Construct, BlockCategory.Armor, scannedBlockCount, BuilderTools.IncrementZ1, Vector3I.Up, BuilderTools.IncrementX1Z1, true, true, ref currentPosition, new Vector3I(11, 1, 2), new Vector3I(15, 1, -2));

			//6: Scan Angle Blocks
			scannedBlockCount = PatternSearch.CheckLine(Construct, BlockCategory.Armor, new Vector3I(3, 1, 1), BuilderTools.IncrementX1Z1, MathTools.Vector3IPlus(currentPosition, 0, -1, 0));

			//6: Place blocks on angle blocks
			if (!_thickCore)
				LineBuilder.BuildDualBlockLine(Construct, BlockCategory.Armor, scannedBlockCount, 0, BuilderTools.IncrementX1Z1, Vector3I.Left, BuilderTools.IncrementX1Z1, Vector3I.Backward, new Vector3I(3, 1, -3), new Vector3I(3, 1, -7), true, true, ref currentPosition);
			else
				LineBuilder.BuildDualStackedBlockLine(Construct, BlockCategory.Armor, scannedBlockCount, 0, BuilderTools.IncrementX1Z1, Vector3I.Up, Vector3I.Left, BuilderTools.IncrementX1Z1, Vector3I.Backward, true, true, 2, ref currentPosition, new Vector3I(35, 1, 5), new Vector3I(35, 1, -7), new Vector3I(39, 1, 5), new Vector3I(39, 1, -7));

			//7: Scan Line
			scannedBlockCount = PatternSearch.CheckLine(Construct, BlockCategory.Armor, new Vector3I(0, 1, 0), BuilderTools.IncrementZ1, MathTools.Vector3IPlus(currentPosition, 0, -1, 0));

			//7: Determine Nacelle Usage
			if (!_useNacelles) {

				//7: No Nacelles
				if (!_thickCore)
					LineBuilder.BuildStraightArmorLine(Construct, BlockCategory.Armor, new Vector3I(3, 1, 6), scannedBlockCount, BuilderTools.IncrementZ1, BuilderTools.IncrementZ1, true, true, ref currentPosition);
				else
					LineBuilder.BuildStraightStackedArmorLine(Construct, BlockCategory.Armor, scannedBlockCount, BuilderTools.IncrementZ1, Vector3I.Up, BuilderTools.IncrementZ1, true, true, ref currentPosition, new Vector3I(11, 1, 2), new Vector3I(15, 1, -2));


			} else {

				var originPos = currentPosition;

				//7: Yes Nacelles
				if (!_thickCore) {

					LineBuilder.BuildStraightArmorLine(Construct, BlockCategory.Armor, new Vector3I(3, 1, -7), 1, BuilderTools.IncrementZ1, BuilderTools.IncrementZ1, true, true, ref currentPosition);
					LineBuilder.BuildStraightArmorLine(Construct, BlockCategory.Armor, new Vector3I(0, 1, 0), scannedBlockCount - 2, BuilderTools.IncrementZ1, BuilderTools.IncrementZ1, true, true, ref currentPosition);
					LineBuilder.BuildStraightArmorLine(Construct, BlockCategory.Armor, new Vector3I(3, 1, -5), 1, BuilderTools.IncrementZ1, BuilderTools.IncrementZ1, true, true, ref currentPosition);

				} else {

					LineBuilder.BuildStraightStackedArmorLine(Construct, BlockCategory.Armor, 1, BuilderTools.IncrementZ1, Vector3I.Up, BuilderTools.IncrementZ1, true, true, ref currentPosition, new Vector3I(39, 1, 5), new Vector3I(39, 1, -7));
					LineBuilder.BuildStraightStackedArmorLine(Construct, BlockCategory.Armor, scannedBlockCount -2, BuilderTools.IncrementZ1, Vector3I.Up, BuilderTools.IncrementZ1, true, true, ref currentPosition, new Vector3I(0, 1, 0), new Vector3I(0, 1, 0));
					LineBuilder.BuildStraightStackedArmorLine(Construct, BlockCategory.Armor, 1, BuilderTools.IncrementZ1, Vector3I.Up, BuilderTools.IncrementZ1, true, true, ref currentPosition, new Vector3I(39, 1, 7), new Vector3I(39, 1, -5));

				}

			}

			//8: Scan Angle-In Line
			scannedBlockCount = PatternSearch.CheckLine(Construct, BlockCategory.Armor, new Vector3I(3, 1, 3), BuilderTools.IncrementnX1Z1, MathTools.Vector3IPlus(currentPosition, 0, -1, 0));

			//8: Place Blocks on Angle
			if (!_thickCore)
				LineBuilder.BuildDualBlockLine(Construct, BlockCategory.Armor, scannedBlockCount, 0, BuilderTools.IncrementnX1Z1, Vector3I.Left, BuilderTools.IncrementZ1, Vector3I.Backward, new Vector3I(3, 1, -1), new Vector3I(3, 1, -5), true, true, ref currentPosition);
			else
				LineBuilder.BuildDualStackedBlockLine(Construct, BlockCategory.Armor, scannedBlockCount, 0, BuilderTools.IncrementnX1Z1, Vector3I.Up, Vector3I.Left, BuilderTools.IncrementZ1, Vector3I.Backward, true, true, 2, ref currentPosition, new Vector3I(35, 1, 7), new Vector3I(35, 1, -5), new Vector3I(39, 1, 7), new Vector3I(39, 1, -5));


			//9: Scan Line
			scannedBlockCount = PatternSearch.CheckLine(Construct, BlockCategory.Armor, new Vector3I(0, 1, 0), BuilderTools.IncrementZ1, MathTools.Vector3IPlus(currentPosition, 0, -1, 0));

			//9: Place Blocks on Line
			if (!_thickCore)
				LineBuilder.BuildStraightArmorLine(Construct, BlockCategory.Armor, new Vector3I(3, 1, 6), scannedBlockCount, BuilderTools.IncrementZ1, BuilderTools.IncrementZ1, true, true, ref currentPosition);
			else
				LineBuilder.BuildStraightStackedArmorLine(Construct, BlockCategory.Armor, scannedBlockCount, BuilderTools.IncrementZ1, Vector3I.Up, BuilderTools.IncrementZ1, true, true, ref currentPosition, new Vector3I(11, 1, 2), new Vector3I(15, 1, -2));

			//10: Complete End Section
			var hullEndPointTemp = _mainHullEndPoint + Vector3I.Up;

			if (!_thickCore) {

				LineBuilder.BuildStraightArmorLine(Construct, BlockCategory.Armor, new Vector3I(2, 1, 7), currentPosition.X, Vector3I.Right, Vector3I.Right, true, true, ref hullEndPointTemp);
				Construct.PlaceBlock(BlockCategory.Armor, hullEndPointTemp, hullEndPointTemp, new Vector3I(3, 1, -1), true, true);

			} else {

				LineBuilder.BuildStraightStackedArmorLine(Construct, BlockCategory.Armor, currentPosition.X, Vector3I.Right, Vector3I.Up, Vector3I.Right, true, true, ref hullEndPointTemp, new Vector3I(10, 1, 3), new Vector3I(14, 1, -1));
				LineBuilder.BuildStackedBlocks(Construct, BlockCategory.Armor, Vector3I.Up, hullEndPointTemp, true, true, new Vector3I(35, 1, 7), new Vector3I(35, 1, -5));

			}


		}

		public void FillFirstHullLayer() {

			var noseTop = _thickCore ? new Vector3I(0, 2, 1) : new Vector3I(0, 1, 1);
			LineBuilder.FillSpaceWithLines(Construct, BlockCategory.Armor, noseTop, BuilderTools.IncrementZ1, Vector3I.Right, 20, new Vector3I(0, 1, 0), true, true);
		
		}

		public void SecondHullLayer() {

			var startCoords = Vector3I.Up + BuilderTools.IncrementZ1;
			startCoords.Y += Construct.Max.Y;
			PatternSearch.FindAllLines(_lineDataCollection, Construct, BlockCategory.Armor, new Vector3I(0, 1, 0), startCoords, Vector3I.Down, BuilderTools.IncrementZ1, Vector3I.Right, 1, 1);
			
			//This is Debug Stuff
			int coloredBlocks = 0;

			foreach (var line in _lineDataCollection) {

				_blockCollection.Clear();
				Construct.GetBlocks(line.Start, line.End, _blockCollection, true, true);

				foreach (var block in _blockCollection) {

					if (block == null)
						continue;

					block.ColorMaskHSV.X = 0.5472222f;
					block.ColorMaskHSV.Y = 1;
					block.ColorMaskHSV.Z = 1;
					coloredBlocks++;

				}
			
			}

			Construct.Log.Append("LineData Collections: " + _lineDataCollection.Count).AppendLine();
			Construct.Log.Append("Block Collection:     " + _blockCollection.Count).AppendLine();
			Construct.Log.Append("Colored Blocks:       " + coloredBlocks).AppendLine();
			//Debug Stuff Ends

			string startPattern = null;
			string additionalPattern = null;
			LineBuilder.GetTopLinePatternRandomPair(ref startPattern, ref additionalPattern);
			LineBuilder.BuildPatternedLines(Construct, _lineDataCollection, Vector3I.Up, startPattern, additionalPattern, true, true);

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
