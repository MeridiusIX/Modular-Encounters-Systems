using ModularEncountersSystems.Helpers;
using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Profiles {
	public class PrefabDataProfile {

		public string ProfileSubtypeId;

		public List<string> Prefabs;

		public List<string> CustomTags;
		public List<ManipulationProfile> ManipulationProfiles;
		public List<string> ManipulationGroups;

		public PrefabDataProfile() {

			ProfileSubtypeId = "";

			Prefabs = new List<string>();

			CustomTags = new List<string>();
			ManipulationProfiles = new List<ManipulationProfile>();
			ManipulationGroups = new List<string>();

		}

		public void InitTags(string data) {
		
			if (string.IsNullOrWhiteSpace(data))
				return;

			var descSplit = data.Split('\n');

			foreach (var tag in descSplit) {

				//Prefabs
				if (tag.Contains("[Prefabs:")) {

					TagParse.TagStringListCheck(tag, ref Prefabs);
					continue;

				}

				//CustomTags
				if (tag.Contains("[CustomTags:")) {

					TagParse.TagStringListCheck(tag, ref CustomTags);
					continue;

				}

				//ManipulationProfiles
				if (tag.Contains("[ManipulationProfiles:")) {

					TagParse.TagManipulationProfileCheck(tag, ref ManipulationProfiles);
					continue;

				}

				//ManipulationGroups
				if (tag.Contains("[ManipulationGroups:")) {

					TagParse.TagStringListCheck(tag, ref ManipulationGroups);
					continue;

				}

			}

			foreach (var manipulationName in ManipulationGroups) {

				if (string.IsNullOrWhiteSpace(manipulationName))
					continue;

				ManipulationGroup group = null;

				if (ProfileManager.ManipulationGroups.TryGetValue(manipulationName, out group)) {

					foreach (var condition in group.ManipulationProfiles) {

						if (!ManipulationProfiles.Contains(condition))
							ManipulationProfiles.Add(condition);

					}

				}

			}

		}

	}

}
