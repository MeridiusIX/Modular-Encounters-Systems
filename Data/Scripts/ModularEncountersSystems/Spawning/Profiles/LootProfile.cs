using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Profiles {
	public class LootProfile {

		public string ProfileSubtypeId;

		public List<MyDefinitionId> ContainerBlockTypes;
		public List<string> ContainerTypes;
		public int MinBlocks;
		public int MaxBlocks;
		public bool AppendNameToBlock;
		public string AppendedName;
		public bool MatchBlocksContainingName;
		public string MatchedName;
		public int Chance;

		public LootProfile() {

			ProfileSubtypeId = "";

			ContainerBlockTypes = new List<MyDefinitionId>();
			ContainerTypes = new List<string>();
			MinBlocks = 1;
			MaxBlocks = 1;
			AppendNameToBlock = false;
			AppendedName = "";
			MatchBlocksContainingName = false;
			MatchedName = "";
			Chance = 100;

		}

		public void InitTags(string data) {
		
			if (string.IsNullOrWhiteSpace(data))
				return;

			var descSplit = data.Split('\n');

			foreach (var tag in descSplit) {

				//ContainerBlockTypes
				if (tag.Contains("[ContainerBlockTypes:")) {

					TagParse.TagMyDefIdCheck(tag, ref ContainerBlockTypes);
					continue;

				}

				//ContainerTypes
				if (tag.Contains("[ContainerTypes:")) {

					TagParse.TagStringListCheck(tag, ref ContainerTypes);
					continue;

				}

				//MinBlocks
				if (tag.Contains("[MinBlocks:")) {

					TagParse.TagIntCheck(tag, ref MinBlocks);
					continue;

				}

				//MaxBlocks
				if (tag.Contains("[MaxBlocks:")) {

					TagParse.TagIntCheck(tag, ref MaxBlocks);
					continue;

				}

				//AppendNameToBlock
				if (tag.Contains("[AppendNameToBlock:")) {

					TagParse.TagBoolCheck(tag, ref AppendNameToBlock);
					continue;

				}

				//AppendedName
				if (tag.Contains("[AppendedName:")) {

					TagParse.TagStringCheck(tag, ref AppendedName);
					continue;

				}

				//MatchBlocksContainingName
				if (tag.Contains("[MatchBlocksContainingName:")) {

					TagParse.TagBoolCheck(tag, ref MatchBlocksContainingName);
					continue;

				}

				//MatchedName
				if (tag.Contains("[MatchedName:")) {

					TagParse.TagStringCheck(tag, ref MatchedName);
					continue;

				}

				//Chance
				if (tag.Contains("[Chance:")) {

					TagParse.TagIntCheck(tag, ref Chance);
					continue;

				}

			}

		}

	}

}
