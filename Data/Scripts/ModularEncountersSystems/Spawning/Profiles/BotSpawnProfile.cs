using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Profiles {
	public class BotSpawnProfile {

		public string ProfileSubtypeId;

		public string BotType;
		public bool UseAiEnabled;
		public string BotBehavior;
		public string BotDisplayName;
		public Vector3I Color;

		public BotSpawnProfile() {

			ProfileSubtypeId = "";

			BotType = "";
			UseAiEnabled = true;
			BotBehavior = "Default";
			BotDisplayName = "";
			Color = new Vector3I(300, 300, 300);

		}

		public void InitTags(string data) {
		
			if (string.IsNullOrWhiteSpace(data))
				return;

			var descSplit = data.Split('\n');

			foreach (var tag in descSplit) {

				if (tag.Contains("[BotType:")) {

					TagParse.TagStringCheck(tag, ref BotType);
					continue;

				}

				if (tag.Contains("[UseAiEnabled:")) {

					TagParse.TagBoolCheck(tag, ref UseAiEnabled);
					continue;

				}

				if (tag.Contains("[BotBehavior:")) {

					TagParse.TagStringCheck(tag, ref BotBehavior);
					continue;

				}

				if (tag.Contains("[BotDisplayName:")) {

					TagParse.TagStringCheck(tag, ref BotDisplayName);
					continue;

				}

				if (tag.Contains("[Color:")) {

					TagParse.TagVector3ICheck(tag, ref Color);
					continue;

				}

			}
		
		}

	}

}
