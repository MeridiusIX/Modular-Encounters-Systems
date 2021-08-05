using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Profiles {
	public class WeaponModRulesProfile {

		public string ProfileSubtypeId;
		public MyDefinitionId WeaponBlock;
		public bool AllowInRandomization;
		public bool AllowIfNonPublic;
		public bool AllowOnlyIfExactSize;

		public List<MyDefinitionId> AllowedTargetBlocks;
		public List<MyDefinitionId> RestrictedTargetBlocks;

		public WeaponModRulesProfile() {

			ProfileSubtypeId = "";
			WeaponBlock = new MyDefinitionId();
			AllowInRandomization = true;
			AllowIfNonPublic = false;
			AllowOnlyIfExactSize = false;

			AllowedTargetBlocks = new List<MyDefinitionId>();
			RestrictedTargetBlocks = new List<MyDefinitionId>();

		}

		public void InitTags(string data) {
		
			if (string.IsNullOrWhiteSpace(data))
				return;

			var descSplit = data.Split('\n');

			foreach (var tag in descSplit) {

				//WeaponBlock
				if (tag.Contains("[WeaponBlock:")) {

					TagParse.TagMyDefIdCheck(tag, ref WeaponBlock);
					continue;

				}

				//AllowInRandomization
				if (tag.Contains("[AllowInRandomization:")) {

					TagParse.TagBoolCheck(tag, ref AllowInRandomization);
					continue;

				}

				//AllowIfNonPublic
				if (tag.Contains("[AllowIfNonPublic:")) {

					TagParse.TagBoolCheck(tag, ref AllowIfNonPublic);
					continue;

				}

				//AllowOnlyIfExactSize
				if (tag.Contains("[AllowOnlyIfExactSize:")) {

					TagParse.TagBoolCheck(tag, ref AllowOnlyIfExactSize);
					continue;

				}

				//AllowedTargetBlocks
				if (tag.Contains("[AllowedTargetBlocks:")) {

					TagParse.TagMyDefIdCheck(tag, ref AllowedTargetBlocks);
					continue;

				}

				//RestrictedTargetBlocks
				if (tag.Contains("[RestrictedTargetBlocks:")) {

					TagParse.TagMyDefIdCheck(tag, ref RestrictedTargetBlocks);
					continue;

				}

			}

		}

	}

}
