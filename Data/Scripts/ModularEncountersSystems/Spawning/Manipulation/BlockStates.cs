using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Spawning.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;

namespace ModularEncountersSystems.Spawning.Manipulation {
	public static class BlockStates {

		public static List<MyDefinitionId> PartialBuiltAllowedBlocks = new List<MyDefinitionId>();

		public static Random Rnd = new Random();

		public static void Setup() {

			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorBlock"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeHeavyBlockArmorBlock"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorSlope"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeHeavyBlockArmorSlope"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorCorner"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeHeavyBlockArmorCorner"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorCornerInv"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeHeavyBlockArmorCornerInv"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallBlockArmorBlock"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallHeavyBlockArmorBlock"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallBlockArmorSlope"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallHeavyBlockArmorSlope"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallBlockArmorCorner"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallHeavyBlockArmorCorner"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallBlockArmorCornerInv"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallHeavyBlockArmorCornerInv"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeHalfArmorBlock"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeHeavyHalfArmorBlock"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeHalfSlopeArmorBlock"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeHeavyHalfSlopeArmorBlock"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "HalfArmorBlock"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "HeavyHalfArmorBlock"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "HalfSlopeArmorBlock"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "HeavyHalfSlopeArmorBlock"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorRoundSlope"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallHeavyBlockArmorRoundSlope"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorRoundCorner"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallHeavyBlockArmorRoundCorner"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorRoundCornerInv"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeHeavyBlockArmorRoundCornerInv"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallBlockArmorRoundSlope"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallHeavyBlockArmorRoundSlope"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallBlockArmorRoundCorner"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallHeavyBlockArmorRoundCorner"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallBlockArmorRoundCornerInv"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallHeavyBlockArmorRoundCornerInv"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorSlope2Base"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeHeavyBlockArmorSlope2Base"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorSlope2Tip"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeHeavyBlockArmorSlope2Tip"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorCorner2Base"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeHeavyBlockArmorCorner2Base"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorCorner2Tip"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeHeavyBlockArmorCorner2Tip"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorInvCorner2Base"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeHeavyBlockArmorInvCorner2Base"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorInvCorner2Tip"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeHeavyBlockArmorInvCorner2Tip"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallBlockArmorSlope2Base"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallHeavyBlockArmorSlope2Base"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallBlockArmorSlope2Tip"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallHeavyBlockArmorSlope2Tip"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallBlockArmorCorner2Base"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallHeavyBlockArmorCorner2Base"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallBlockArmorCorner2Tip"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallHeavyBlockArmorCorner2Tip"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallBlockArmorInvCorner2Base"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallHeavyBlockArmorInvCorner2Base"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallBlockArmorInvCorner2Tip"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallHeavyBlockArmorInvCorner2Tip"));

			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "ArmorCenter"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "ArmorCorner"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "ArmorInvCorner"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "ArmorSide"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallArmorCenter"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallArmorCorner"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallArmorInvCorner"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallArmorSide"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "Window3x3FlatInv"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "Window3x3Flat"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "Window2x3FlatInv"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "Window2x3Flat"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "Window1x2Slope"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "Window1x2SideRight"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "Window1x2SideLeft"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "Window1x2Inv"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "Window1x2FlatInv"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "Window1x2Flat"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "Window1x2Face"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "Window1x1Slope"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "Window1x1Side"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "Window1x1Inv"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "Window1x1FlatInv"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "Window1x1Flat"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "Window1x1Face"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeWindowSquare"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeWindowEdge"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeWindowCen"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeSteelCatwalkPlate"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeSteelCatwalkCorner"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeSteelCatwalk2Sides"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeSteelCatwalk"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeStairs"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeRamp"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeRailStraight"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeInteriorPillar"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeCoverWallHalf"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeCoverWall"));
			PartialBuiltAllowedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockInteriorWall"));


		}

		public static void ProcessDereliction(MyObjectBuilder_CubeGrid grid, ManipulationProfile manipulation) {

			if (grid?.CubeBlocks == null)
				return;

			foreach (var profileName in manipulation.DerelictionProfiles) {

				DerelictionProfile profile = null;

				if (!ProfileManager.DerelictionProfiles.TryGetValue(profileName, out profile))
					continue;

				foreach (var block in grid.CubeBlocks) {

					profile.ProcessBlock(block);

				}

			}

		}

		public static void PartialBlockBuildStates(MyObjectBuilder_CubeGrid cubeGrid, ManipulationProfile manipulation) {

			if (manipulation.MinimumBlocksPercent >= manipulation.MaximumBlocksPercent) {

				return;

			}

			if (manipulation.MinimumBuildPercent >= manipulation.MaximumBuildPercent) {

				return;

			}

			var targetBlocks = new List<MyObjectBuilder_CubeBlock>();

			foreach (var block in cubeGrid.CubeBlocks.ToList()) {

				var defIdBlock = block.GetId();

				if (PartialBuiltAllowedBlocks.Contains(defIdBlock) == true) {

					targetBlocks.Add(block);

				}

			}

			var percentOfBlocks = (double)Rnd.Next(manipulation.MinimumBlocksPercent, manipulation.MaximumBlocksPercent) / 100;
			var actualPercentOfBlocks = (int)Math.Floor(targetBlocks.Count * percentOfBlocks);

			if (actualPercentOfBlocks <= 0) {

				return;

			}

			while (targetBlocks.Count > actualPercentOfBlocks) {

				if (targetBlocks.Count <= 1) {

					break;

				}

				targetBlocks.RemoveAt(Rnd.Next(0, targetBlocks.Count));

			}

			foreach (var block in targetBlocks) {

				var buildPercent = (float)Rnd.Next(manipulation.MinimumBuildPercent, manipulation.MaximumBuildPercent);
				buildPercent /= 100;

				if (buildPercent <= 0 || buildPercent > 1) {

					buildPercent = 1;

				}

				block.BuildPercent = buildPercent;
				block.IntegrityPercent = buildPercent;

			}

		}


	}
}
