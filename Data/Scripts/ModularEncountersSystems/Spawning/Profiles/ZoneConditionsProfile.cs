using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Zones;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Spawning.Profiles {
	public class ZoneConditionsProfile {

		public string ProfileSubtypeId;

		public string ZoneName;

		public bool UseKnownPlayerLocation;

		public double MinDistanceFromZoneCenter;
		public double MaxDistanceFromZoneCenter;

		public bool CheckCustomZoneCounters;
		public List<string> CustomZoneCounterName;
		public List<int> CustomZoneCounterValue;
		public Dictionary<string, int> CustomZoneCounterReference;

		public bool CheckCustomZoneBools;
		public List<string> CustomZoneBoolName;
		public List<bool> CustomZoneBoolValue;
		public Dictionary<string, bool> CustomZoneBoolReference;

		public int MinSpawnedZoneEncounters;
		public int MaxSpawnedZoneEncounters;

		public ZoneConditionsProfile() {

			ProfileSubtypeId = "";

			ZoneName = "";

			MinDistanceFromZoneCenter = -1;
			MaxDistanceFromZoneCenter = -1;

			CheckCustomZoneCounters = false;
			CustomZoneCounterName = new List<string>();
			CustomZoneCounterValue = new List<int>();
			CustomZoneCounterReference = new Dictionary<string, int>();

			CheckCustomZoneBools = false;
			CustomZoneBoolName = new List<string>();
			CustomZoneBoolValue = new List<bool>();
			CustomZoneBoolReference = new Dictionary<string, bool>();

			MinSpawnedZoneEncounters = -1;
			MaxSpawnedZoneEncounters = -1;

		}

		public void InitTags(string data) {
		
			if (string.IsNullOrWhiteSpace(data))
				return;

			var descSplit = data.Split('\n');

			foreach (var tagRaw in descSplit) {

				var tag = tagRaw.Trim();

				//ZoneName
				if (tag.StartsWith("[ZoneName:") == true) {

					TagParse.TagStringCheck(tag, ref this.ZoneName);

				}

				//UseKnownPlayerLocation
				if (tag.StartsWith("[UseKnownPlayerLocation:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseKnownPlayerLocation);

				}

				//MinDistanceFromZoneCenter
				if (tag.StartsWith("[MinDistanceFromZoneCenter:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.MinDistanceFromZoneCenter);

				}

				//MaxDistanceFromZoneCenter
				if (tag.StartsWith("[MaxDistanceFromZoneCenter:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.MaxDistanceFromZoneCenter);

				}

				//CheckCustomZoneCounters
				if (tag.StartsWith("[CheckCustomZoneCounters:") == true) {

					TagParse.TagBoolCheck(tag, ref this.CheckCustomZoneCounters);

				}

				//CustomZoneCounterName
				if (tag.StartsWith("[CustomZoneCounterName:") == true) {

					TagParse.TagStringListCheck(tag, ref this.CustomZoneCounterName);

				}

				//CustomZoneCounterValue
				if (tag.StartsWith("[CustomZoneCounterValue:") == true) {

					TagParse.TagIntListCheck(tag, ref this.CustomZoneCounterValue);

				}

				//CheckCustomZoneBools
				if (tag.StartsWith("[CheckCustomZoneBools:") == true) {

					TagParse.TagBoolCheck(tag, ref this.CheckCustomZoneBools);

				}

				//CustomZoneBoolName
				if (tag.StartsWith("[CustomZoneBoolName:") == true) {

					TagParse.TagStringListCheck(tag, ref this.CustomZoneBoolName);

				}

				//CustomZoneBoolValue
				if (tag.StartsWith("[CustomZoneBoolValue:") == true) {

					TagParse.TagBoolListCheck(tag, ref this.CustomZoneBoolValue);

				}

				//MinSpawnedZoneEncounters
				if (tag.StartsWith("[MinSpawnedZoneEncounters:") == true) {

					TagParse.TagIntCheck(tag, ref this.MinSpawnedZoneEncounters);

				}

				//MaxSpawnedZoneEncounters
				if (tag.StartsWith("[MaxSpawnedZoneEncounters:") == true) {

					TagParse.TagIntCheck(tag, ref this.MaxSpawnedZoneEncounters);

				}

			}

			for (int i = 0; i < CustomZoneCounterName.Count && i < CustomZoneCounterValue.Count; i++) {

				if (!CustomZoneCounterReference.ContainsKey(CustomZoneCounterName[i]))
					CustomZoneCounterReference.Add(CustomZoneCounterName[i], CustomZoneCounterValue[i]);

			}

			for (int i = 0; i < CustomZoneBoolName.Count && i < CustomZoneBoolValue.Count; i++) {

				if (!CustomZoneBoolReference.ContainsKey(CustomZoneBoolName[i]))
					CustomZoneBoolReference.Add(CustomZoneBoolName[i], CustomZoneBoolValue[i]);

			}

		}

	}

}
