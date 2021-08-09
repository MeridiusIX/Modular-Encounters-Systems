using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Spawning.Profiles {
	public class LootGroup {

		public string ProfileSubtypeId;
		public List<LootProfile> LootProfiles;

		public LootGroup(string data) {

			ProfileSubtypeId = "";
			LootProfiles = new List<LootProfile>();
			InitTags(data);

		}

		public void InitTags(string data) {

			if (string.IsNullOrWhiteSpace(data))
				return;

			var descSplit = data.Split('\n');

			foreach (var tag in descSplit) {

				//LootProfiles
				if (tag.Contains("[LootProfiles:")) {

					TagParse.TagLootProfileCheck(tag, ref LootProfiles);
					continue;

				}

			}

		}

	}
}
