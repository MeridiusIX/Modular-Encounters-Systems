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
		public List<CounterCompareEnum> CustomZoneCounterCompare;
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
			CustomZoneCounterCompare = new List<CounterCompareEnum>();
			CustomZoneCounterReference = new Dictionary<string, int>();

			CheckCustomZoneBools = false;
			CustomZoneBoolName = new List<string>();
			CustomZoneBoolValue = new List<bool>();
			CustomZoneBoolReference = new Dictionary<string, bool>();

			MinSpawnedZoneEncounters = -1;
			MaxSpawnedZoneEncounters = -1;

		}

		public bool CheckCounters(Zone zone) {

			bool failedCheck = false;

			for (int i = 0; i < CustomZoneCounterName.Count && i < CustomZoneCounterValue.Count; i++) {

				var compareType = GetCompare(i);
				long counter = 0;

				if (!zone.CustomCounters.TryGetValue(CustomZoneCounterName[i], out counter)) {

					failedCheck = true;
					break;

				}

				int target = CustomZoneCounterValue[i];

				if (compareType == CounterCompareEnum.GreaterOrEqual)
					if (counter >= target)
						continue;

				if (compareType == CounterCompareEnum.Greater)
					if (counter > target)
						continue;

				if (compareType == CounterCompareEnum.Equal)
					if (counter == target)
						continue;

				if (compareType == CounterCompareEnum.NotEqual)
					if (counter != target)
						continue;

				if (compareType == CounterCompareEnum.Less)
					if (counter < target)
						continue;

				if (compareType == CounterCompareEnum.LessOrEqual)
					if (counter <= target)
						continue;

				failedCheck = true;
				break;

			}

			return !failedCheck;		

		}

		private CounterCompareEnum GetCompare(int index) {

			if (index < CustomZoneCounterCompare.Count)
				return CustomZoneCounterCompare[index];

			return CounterCompareEnum.GreaterOrEqual;
		
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

				//CustomZoneCounterCompare
				if (tag.StartsWith("[CustomZoneCounterCompare:") == true) {

					TagParse.TagCounterCompareEnumCheck(tag, ref this.CustomZoneCounterCompare);

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

			/*
			for (int i = 0; i < CustomZoneCounterName.Count && i < CustomZoneCounterValue.Count; i++) {

				if (!CustomZoneCounterReference.ContainsKey(CustomZoneCounterName[i]))
					CustomZoneCounterReference.Add(CustomZoneCounterName[i], CustomZoneCounterValue[i]);

			}
			*/

			for (int i = 0; i < CustomZoneBoolName.Count && i < CustomZoneBoolValue.Count; i++) {

				if (!CustomZoneBoolReference.ContainsKey(CustomZoneBoolName[i]))
					CustomZoneBoolReference.Add(CustomZoneBoolName[i], CustomZoneBoolValue[i]);

			}

		}

	}

}
