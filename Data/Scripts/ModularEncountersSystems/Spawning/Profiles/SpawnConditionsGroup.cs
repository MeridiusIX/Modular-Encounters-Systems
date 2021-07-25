using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Spawning.Profiles {
	public class SpawnConditionsGroup {

		public string ProfileSubtypeId;
		public List<SpawnConditionsProfile> SpawnConditionProfiles;

		public SpawnConditionsGroup(string data) {

			ProfileSubtypeId = "";
			SpawnConditionProfiles = new List<SpawnConditionsProfile>();
			InitTags(data);

		}

		public void InitTags(string data) {

			if (string.IsNullOrWhiteSpace(data))
				return;

			var descSplit = data.Split('\n');

			foreach (var tag in descSplit) {

				//SpawnConditionProfiles
				if (tag.Contains("[SpawnConditionProfiles:")) {

					TagParse.TagSpawnConditionsProfileCheck(tag, ref SpawnConditionProfiles);
					continue;

				}

			}

		}

	}
}
