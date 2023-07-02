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
			CoreHullSetup();
		
		}

		public void ProcessStep(int step = 0) {
		
			
		
		}

		public void InitialHullOutline() {

			//Hull Outline Setup
			Vector3I lastPosition = Vector3I.Zero;
			Construct.Log.Clear();

			//Origin:   1
			Construct.PlaceBlock(BlockCategory.Armor, Vector3I.Zero, Vector3I.Zero, new Vector3I(0, 1, 0));

			//AngleOutA: 1-3
			int rndNum = MathTools.RandomBetween(1, 4);
			for (int i = 1; i <= rndNum; i++) {

				var pos = lastPosition;
				pos.X += 1;
				Construct.PlaceBlock(BlockCategory.Armor, pos, pos, new Vector3I(3, 1, 1), true);
				pos.Z += 1;
				Construct.PlaceBlock(BlockCategory.Armor, pos, pos, new Vector3I(0, 1, 0), true);
				lastPosition = pos;

			}

			//StraightA: 4-8
			rndNum = MathTools.RandomBetween(4, 8);
			for (int i = 1; i <= rndNum; i++) {

				var pos = lastPosition;
				pos.Z += 1;
				Construct.PlaceBlock(BlockCategory.Armor, pos, pos, new Vector3I(0, 1, 0), true);
				lastPosition = pos;

			}

			//AngleOutB: 2-4
			rndNum = MathTools.RandomBetween(2, 4);
			for (int i = 1; i <= rndNum; i++) {

				var pos = lastPosition;
				pos.X += 1;
				Construct.PlaceBlock(BlockCategory.Armor, pos, pos, new Vector3I(3, 1, 1), true);
				pos.Z += 1;
				Construct.PlaceBlock(BlockCategory.Armor, pos, pos, new Vector3I(0, 1, 0), true);
				lastPosition = pos;

			}

			//StraightB: 4-10
			rndNum = MathTools.RandomBetween(4, 10);
			for (int i = 1; i <= rndNum; i++) {

				var pos = lastPosition;
				pos.Z += 1;
				Construct.PlaceBlock(BlockCategory.Armor, pos, pos, new Vector3I(0, 1, 0), true);
				lastPosition = pos;

			}

			//AngleOutC: 2-4
			rndNum = MathTools.RandomBetween(2, 4);
			for (int i = 1; i <= rndNum; i++) {

				var pos = lastPosition;
				pos.X += 1;
				Construct.PlaceBlock(BlockCategory.Armor, pos, pos, new Vector3I(3, 1, 1), true);
				pos.Z += 1;
				Construct.PlaceBlock(BlockCategory.Armor, pos, pos, new Vector3I(0, 1, 0), true);
				lastPosition = pos;

			}

			//StraightC: 3-7 (Odd)
			rndNum = MathTools.ClampRandomOdd(MathTools.RandomBetween(3, 7), 3, 7);
			for (int i = 1; i <= rndNum; i++) {

				var pos = lastPosition;
				pos.Z += 1;
				Construct.PlaceBlock(BlockCategory.Armor, pos, pos, new Vector3I(0, 1, 0), true);
				lastPosition = pos;

			}

			//AngleInD:  2-4
			rndNum = MathTools.RandomBetween(2, 4);
			for (int i = 1; i <= rndNum; i++) {

				var pos = lastPosition;
				
				pos.Z += 1;
				Construct.PlaceBlock(BlockCategory.Armor, pos, pos, new Vector3I(3, 1, 3), true);
				pos.X -= 1;
				Construct.PlaceBlock(BlockCategory.Armor, pos, pos, new Vector3I(0, 1, 0), true);
				lastPosition = pos;

			}

			//StraightD: 4-10
			rndNum = MathTools.RandomBetween(4, 10);
			for (int i = 1; i <= rndNum; i++) {

				var pos = lastPosition;
				pos.Z += 1;
				Construct.PlaceBlock(BlockCategory.Armor, pos, pos, new Vector3I(0, 1, 0), true);
				lastPosition = pos;

			}

			//End:      ???



		}

		public void CoreHullSetup() {

			//Start with origin
			Vector3I currentPosition = Vector3I.Zero;
			currentPosition.Y = 1;
			

			if (!_thickCore) {

				Construct.PlaceBlock(BlockCategory.Armor, currentPosition, currentPosition, new Vector3I(2, 1, 5), true);

			} else {

				Construct.PlaceBlock(BlockCategory.Armor, currentPosition, currentPosition, new Vector3I(10, 1, 1), true);
				var newPos = MathTools.Vector3IPlus(currentPosition, 0, 1, 0);
				Construct.PlaceBlock(BlockCategory.Armor, newPos, newPos, new Vector3I(14, 1, -3), true);

			}

			//Next, Move 1 to the right
			currentPosition.X += 1;
			
			while (true) {

				_tempBlockA = Construct.GetBlock(MathTools.Vector3IPlus(currentPosition, 0, -1, 0));
				_tempBlockB = Construct.GetBlock(MathTools.Vector3IPlus(currentPosition, 1, -1, 1));

				if (_tempBlockA == null && _tempBlockB == null)
					break;

				bool lastBlock = _tempBlockB == null;

				//Corner
				if (!_thickCore) {

					//Construct.PlaceBlock(BlockCategory.Armor, currentPosition, currentPosition, new Vector3I(3, 1, -3), true);

				} else {

					//Construct.PlaceBlock(BlockCategory.Armor, currentPosition, currentPosition, new Vector3I(35, 1, 5), true);
					var newPos = MathTools.Vector3IPlus(currentPosition, 0, 1, 0);
					//Construct.PlaceBlock(BlockCategory.Armor, newPos, newPos, new Vector3I(21, 1, -3), true);

				}

				currentPosition = MathTools.Vector3IPlus(currentPosition, 0, 0, 1);

				//Inv Corner
				if (!lastBlock) {

					if (!_thickCore) {

						//Construct.PlaceBlock(BlockCategory.Armor, currentPosition, currentPosition, new Vector3I(3, 1, -7), true);

					} else {

						//Construct.PlaceBlock(BlockCategory.Armor, currentPosition, currentPosition, new Vector3I(39, 1, 5), true);
						var newPos = MathTools.Vector3IPlus(currentPosition, 0, 1, 0);
						//Construct.PlaceBlock(BlockCategory.Armor, newPos, newPos, new Vector3I(39, 1, -7), true);

					}

					currentPosition = MathTools.Vector3IPlus(currentPosition, 1, 0, 0);

				} else {

					break;

				}

			}

			//While loop, detect angle blocks on the outline, place new blocks as needed, stop when angle isnt detected.

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
