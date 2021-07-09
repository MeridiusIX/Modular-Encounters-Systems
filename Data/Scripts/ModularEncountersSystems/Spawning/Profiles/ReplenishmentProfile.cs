using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;

namespace ModularEncountersSystems.Spawning.Profiles {
	public class ReplenishmentProfile {

		private List<MyDefinitionId> MaxItemId;
		private List<float> MaxItemAmount;

		public List<MyDefinitionId> RestrictedItems;

		public Dictionary<MyDefinitionId, float> MaxItems;

		public ReplenishmentProfile(string data) {

			MaxItemId = new List<MyDefinitionId>();
			MaxItemAmount = new List<float>();

			RestrictedItems = new List<MyDefinitionId>();

			MaxItems = new Dictionary<MyDefinitionId, float>();

			InitTags(data);

			int lowestCount = MaxItemId.Count;

			if (MaxItemAmount.Count < lowestCount)
				lowestCount = MaxItemAmount.Count;

			for (int i = 0; i < lowestCount; i++) {

				if (!MaxItems.ContainsKey(MaxItemId[i]))
					MaxItems.Add(MaxItemId[i], MaxItemAmount[i]);
			
			}

		}

		private void InitTags(string data) {

			if (string.IsNullOrWhiteSpace(data))
				return;

			var descSplit = data.Split('\n');

			foreach (var tagRaw in descSplit) {

				var tag = tagRaw.Trim();

				//RestrictedItems
				if (tag.Contains("[RestrictedItems:")) {

					TagParse.TagMyDefIdCheck(tag, ref RestrictedItems);
					continue;

				}

				//MaxItems
				if (tag.Contains("[MaxItems:")) {

					TagParse.TagMyDefIdCheck(tag, ref MaxItemId);
					continue;

				}

				//MaxItemAmount
				if (tag.Contains("[MaxItemAmount:")) {

					TagParse.TagFloatCheck(tag, ref MaxItemAmount);
					continue;

				}

			}

		}

	}
}
