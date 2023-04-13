using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Spawning.Profiles {
	public class SuitUpgradesProfile {

		public string ProfileSubtypeId;

		public string BlockName;

		public bool AllowJetpackInhibitorMod;
		public bool AllowHandDrillInhibitorMod;
		public bool AllowPersonnelInhibitorMod;
		public bool AllowEnergyInhibitorMod;
		public bool AllowSolarChargingMod;
		public bool AllowDamageReductionMod;

		public Dictionary<string, Action<string, object>> EditorReference;

		public SuitUpgradesProfile() {

			ProfileSubtypeId = "";

			BlockName = "";

			AllowJetpackInhibitorMod = false;
			AllowHandDrillInhibitorMod = false;
			AllowPersonnelInhibitorMod = false;
			AllowEnergyInhibitorMod = false;
			AllowSolarChargingMod = false;
			AllowDamageReductionMod = false;

			EditorReference = new Dictionary<string, Action<string, object>> {

				{"BlockName", (s, o) => TagParse.TagStringCheck(s, ref BlockName) },
				{"AllowJetpackInhibitorMod", (s, o) => TagParse.TagBoolCheck(s, ref AllowJetpackInhibitorMod) },
				{"AllowHandDrillInhibitorMod", (s, o) => TagParse.TagBoolCheck(s, ref AllowHandDrillInhibitorMod) },
				{"AllowPersonnelInhibitorMod", (s, o) => TagParse.TagBoolCheck(s, ref AllowPersonnelInhibitorMod) },
				{"AllowEnergyInhibitorMod", (s, o) => TagParse.TagBoolCheck(s, ref AllowEnergyInhibitorMod) },
				{"AllowSolarChargingMod", (s, o) => TagParse.TagBoolCheck(s, ref AllowSolarChargingMod) },
				{"AllowDamageReductionMod", (s, o) => TagParse.TagBoolCheck(s, ref AllowDamageReductionMod) },

			};

		}

		public void EditValue(string receivedValue) {

			var processedTag = TagParse.ProcessTag(receivedValue);

			if (processedTag.Length < 2)
				return;

			Action<string, object> referenceMethod = null;

			if (!EditorReference.TryGetValue(processedTag[0], out referenceMethod))
				//TODO: Notes About Value Not Found
				return;

			referenceMethod?.Invoke(receivedValue, null);

		}

		public void InitTags(string customData) {

			if (string.IsNullOrWhiteSpace(customData) == false) {

				var descSplit = customData.Split('\n');

				foreach (var tag in descSplit) {

					EditValue(tag);

				}

			}

		}

	}
}
