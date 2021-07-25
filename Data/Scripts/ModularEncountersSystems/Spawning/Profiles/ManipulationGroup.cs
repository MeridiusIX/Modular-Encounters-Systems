using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Spawning.Profiles {
	public class ManipulationGroup {

		public string ProfileSubtypeId;
		public List<ManipulationProfile> ManipulationProfiles;

		public ManipulationGroup(string data) {

			ProfileSubtypeId = "";
			ManipulationProfiles = new List<ManipulationProfile>();
			InitTags(data);

		}

		public void InitTags(string data) {

			if (string.IsNullOrWhiteSpace(data))
				return;

			var descSplit = data.Split('\n');

			foreach (var tag in descSplit) {

				//ManipulationProfiles
				if (tag.Contains("[ManipulationProfiles:")) {

					TagParse.TagManipulationProfileCheck(tag, ref ManipulationProfiles);
					continue;

				}

			}

		}

	}
}
