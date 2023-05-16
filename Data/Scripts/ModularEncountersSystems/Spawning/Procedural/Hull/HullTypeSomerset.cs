using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Procedural.Hull {
	public class HullTypeSomerset : HullTypeBase, IHullType {

		public void InitialHullSetup() {

			//Hull Outline Setup
			Vector3I lastPosition = Vector3I.Zero;

			//Origin:   1
			Construct.PlaceBlock(BlockIDs.ArmorBlock, Vector3I.Zero, Vector3I.Zero, 0, 0, 0);

			//AngleOutA: 1-3
			int rndNum = MathTools.RandomBetween(1, 4);
			for (int i = 1; i <= rndNum; i++) {

				var pos = lastPosition;
				pos.X += 1;
				Construct.PlaceBlock(BlockIDs.ArmorSlope, pos, pos, 0, 0, 1, true);
				pos.Z -= 1;
				Construct.PlaceBlock(BlockIDs.ArmorBlock, pos, pos, 0, 0, 0, true);
				lastPosition = pos;

			}

			//StraightA: 4-8
			rndNum = MathTools.RandomBetween(4, 8);
			for (int i = 1; i <= rndNum; i++) {

				var pos = lastPosition;
				pos.Z -= 1;
				Construct.PlaceBlock(BlockIDs.ArmorBlock, pos, pos, 0, 0, 0, true);
				lastPosition = pos;

			}

			//AngleOutB: 2-4
			rndNum = MathTools.RandomBetween(2, 4);
			for (int i = 1; i <= rndNum; i++) {

				var pos = lastPosition;
				pos.X += 1;
				Construct.PlaceBlock(BlockIDs.ArmorSlope, pos, pos, 0, 0, 1, true);
				pos.Z -= 1;
				Construct.PlaceBlock(BlockIDs.ArmorBlock, pos, pos, 0, 0, 0, true);
				lastPosition = pos;

			}

			//StraightB: 4-10
			rndNum = MathTools.RandomBetween(4, 10);
			for (int i = 1; i <= rndNum; i++) {

				var pos = lastPosition;
				pos.Z -= 1;
				Construct.PlaceBlock(BlockIDs.ArmorBlock, pos, pos, 0, 0, 0, true);
				lastPosition = pos;

			}

			//AngleOutC: 2-4
			rndNum = MathTools.RandomBetween(2, 4);
			for (int i = 1; i <= rndNum; i++) {

				var pos = lastPosition;
				pos.X += 1;
				Construct.PlaceBlock(BlockIDs.ArmorSlope, pos, pos, 0, 0, 1, true);
				pos.Z -= 1;
				Construct.PlaceBlock(BlockIDs.ArmorBlock, pos, pos, 0, 0, 0, true);
				lastPosition = pos;

			}

			//StraightC: 3-7 (Odd)
			rndNum = MathTools.ClampRandomOdd(MathTools.RandomBetween(3, 7), 3, 7);
			for (int i = 1; i <= rndNum; i++) {

				var pos = lastPosition;
				pos.Z -= 1;
				Construct.PlaceBlock(BlockIDs.ArmorBlock, pos, pos, 0, 0, 0, true);
				lastPosition = pos;

			}

			//AngleInD:  2-4
			rndNum = MathTools.RandomBetween(2, 4);
			for (int i = 1; i <= rndNum; i++) {

				var pos = lastPosition;
				
				pos.Z -= 1;
				Construct.PlaceBlock(BlockIDs.ArmorSlope, pos, pos, 3, 0, 1, true);
				pos.X -= 1;
				Construct.PlaceBlock(BlockIDs.ArmorBlock, pos, pos, 0, 0, 0, true);
				lastPosition = pos;

			}

			//StraightD: 4-10
			rndNum = MathTools.RandomBetween(4, 10);
			for (int i = 1; i <= rndNum; i++) {

				var pos = lastPosition;
				pos.Z -= 1;
				Construct.PlaceBlock(BlockIDs.ArmorBlock, pos, pos, 0, 0, 0, true);
				lastPosition = pos;

			}

			//End:      ???



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
