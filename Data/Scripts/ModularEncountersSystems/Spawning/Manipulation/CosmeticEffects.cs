using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Spawning.Profiles;
using ModularEncountersSystems.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Manipulation {
	public static class CosmeticEffects {

		//Apply Cosmetics
		public static void ApplyCosmetics(MyObjectBuilder_CubeGrid[] grids, ManipulationProfile profile, EnvironmentEvaluation environment) {

			float shiftAmount = 0;

			if (profile.RandomHueShift == true) {

				var randHue = MathTools.RandomBetween(-360, 360);
				shiftAmount = (float)randHue;

			} else {

				shiftAmount = (float)profile.ShiftBlockColorAmount;

			}

			string newSkin = "";

			if (profile.AssignGridSkin.Count > 0) {

				newSkin = profile.AssignGridSkin[MathTools.RandomBetween(0, profile.AssignGridSkin.Count)];

			}

			foreach (var grid in grids) {

				foreach (var block in grid.CubeBlocks) {

					//Shift Hue
					if (profile.ShiftBlockColorsHue)
						ShiftHue(block, shiftAmount);

					//Apple Whole Grid Random Skin
					if(newSkin != "")
						block.SkinSubtypeId = newSkin;

					//Color Replacement & Skin Assignment
					if (profile.RecolorGrid)
						ReplaceColorsAndSkins(block, profile);

				}

				//Random Block Skinning
				RandomBlockSkinning(grid, profile);

			}

		}

		public static void ShiftHue(MyObjectBuilder_CubeBlock block, float shiftAmount) {

			var originalH = block.ColorMaskHSV.X * 360;
			float newH = originalH + shiftAmount;

			if (shiftAmount > 0) {

				if (newH > 360)
					newH -= 360;

			}

			if (shiftAmount < 0) {

				if (newH < 0)
					newH = 360 - (newH + 360);

			}

			block.ColorMaskHSV.X = newH / 360;

		}

		public static void ReplaceColorsAndSkins(MyObjectBuilder_CubeBlock block, ManipulationProfile profile) {

			var blockColor = new Vector3(block.ColorMaskHSV.X, block.ColorMaskHSV.Y, block.ColorMaskHSV.Z);

			//Replace Colors
			Vector3 outputColor = Vector3.Zero;
			string outputSkin = "";

			if (profile.ColorReferencePairs.TryGetValue(blockColor, out outputColor)) {

				block.ColorMaskHSV = profile.ColorReferencePairs[blockColor];
				blockColor = profile.ColorReferencePairs[blockColor];

			}

			//Replace Skins
			if (profile.ColorSkinReferencePairs.TryGetValue(blockColor, out outputSkin)) {

				block.SkinSubtypeId = profile.ColorSkinReferencePairs[blockColor];

			}

		}

		public static void RandomBlockSkinning(MyObjectBuilder_CubeGrid cubeGrid, ManipulationProfile profile) {

			if (profile.MinPercentageSkinRandomBlocks >= profile.MaxPercentageSkinRandomBlocks) {

				return;

			}

			if (profile.SkinRandomBlocksTextures.Count == 0)
				return;

			int skinCount = profile.SkinRandomBlocksTextures.Count;
			bool singleSkin = profile.SkinRandomBlocksTextures.Count == 1;

			var targetBlocks = cubeGrid.CubeBlocks.ToList();

			var percentOfBlocks = (double)MathTools.RandomBetween(profile.MinPercentageSkinRandomBlocks, profile.MaxPercentageSkinRandomBlocks) / 100;
			var actualPercentOfBlocks = (int)Math.Floor(targetBlocks.Count * percentOfBlocks);

			if (actualPercentOfBlocks <= 0) {

				return;

			}

			while (targetBlocks.Count > actualPercentOfBlocks) {

				if (targetBlocks.Count <= 1) {

					break;

				}

				targetBlocks.RemoveAt(MathTools.RandomBetween(0, targetBlocks.Count));

			}

			foreach (var block in targetBlocks) {

				var index = singleSkin ? 0 : MathTools.RandomBetween(0, skinCount);
				block.SkinSubtypeId = profile.SkinRandomBlocksTextures[index];

			}

		}

	}
}
