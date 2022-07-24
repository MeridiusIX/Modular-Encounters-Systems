using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;

namespace ModularEncountersSystems.Spawning.Profiles {
	public class DerelictionProfile {

		public string ProfileSubtypeId;

		public List<MyDefinitionId> Blocks;
		public List<Type> Types;
		public bool MatchOnlyTypeId;
		
		public int Chance;

		public int MinPercentage;
		public int MaxPercentage;

		public bool UseSeparatePercentages;

		public int MinIntegrityPercentage;
		public int MaxIntegrityPercentage;

		public int MinBuildPercentage;
		public int MaxBuildPercentage;

		public DerelictionProfile(string data = null) {

			ProfileSubtypeId = "";

			Blocks = new List<MyDefinitionId>();
			Types = new List<Type>();
			MatchOnlyTypeId = false;
			UseSeparatePercentages = false;

			Chance = 100;

			MinPercentage = 100;
			MaxPercentage = 100;

			MinIntegrityPercentage = 100;
			MaxIntegrityPercentage = 100;

			MinBuildPercentage = 100;
			MaxBuildPercentage = 100;

			InitTags(data);

			foreach (var block in Blocks) {

				Types.Add(block.TypeId);

			}

		}

		public void InitTags(string data) {

			if (string.IsNullOrWhiteSpace(data))
				return;

			var descSplit = data.Split('\n');

			foreach (var tag in descSplit) {

				if (tag.Contains("[Blocks:")) {

					TagParse.TagMyDefIdCheck(tag, ref Blocks);
					continue;

				}

				if (tag.Contains("[MatchOnlyTypeId:")) {

					TagParse.TagBoolCheck(tag, ref MatchOnlyTypeId);
					continue;

				}

				if (tag.Contains("[UseSeparatePercentages:")) {

					TagParse.TagBoolCheck(tag, ref UseSeparatePercentages);
					continue;

				}

				if (tag.Contains("[Chance:")) {

					TagParse.TagIntCheck(tag, ref Chance);
					continue;

				}

				if (tag.Contains("[MinPercentage:")) {

					TagParse.TagIntCheck(tag, ref MinPercentage);
					continue;

				}

				if (tag.Contains("[MaxPercentage:")) {

					TagParse.TagIntCheck(tag, ref MaxPercentage);
					continue;

				}

				if (tag.Contains("[MinIntegrityPercentage:")) {

					TagParse.TagIntCheck(tag, ref MinIntegrityPercentage);
					continue;

				}

				if (tag.Contains("[MaxIntegrityPercentage:")) {

					TagParse.TagIntCheck(tag, ref MaxIntegrityPercentage);
					continue;

				}

				if (tag.Contains("[MinBuildPercentage:")) {

					TagParse.TagIntCheck(tag, ref MinBuildPercentage);
					continue;

				}

				if (tag.Contains("[MaxBuildPercentage:")) {

					TagParse.TagIntCheck(tag, ref MaxBuildPercentage);
					continue;

				}

			}

		}

		//Integrity Can't be Higher Than Build

		public void ProcessBlock(MyObjectBuilder_CubeBlock block) {

			if (!MatchOnlyTypeId) {

				if (!Blocks.Contains(block.GetId()))
					return;

			} else {

				if (!Types.Contains(block.GetId().TypeId))
					return;

			}

			//Chance Check Here
			if (Chance < 100) {

				int roll = MathTools.RandomBetween(0, 101);

				if (roll > Chance)
					return;

			}

			float integrity = 1;
			float build = 1;

			if (!UseSeparatePercentages) {

				var value = (float)MathTools.RandomBetween(MinPercentage, MaxPercentage) / 100;
				integrity = value;
				build = value;

			} else {

				integrity = (float)MathTools.RandomBetween(MinIntegrityPercentage, MaxIntegrityPercentage) / 100;
				build = (float)MathTools.RandomBetween(MinBuildPercentage, MaxBuildPercentage) / 100;

			}

			block.BuildPercent = build;
			block.IntegrityPercent = integrity;

			if (block.IntegrityPercent > block.BuildPercent)
				block.IntegrityPercent = block.BuildPercent;

		}

	}

}
