using ModularEncountersSystems.Zones;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Spawning.Profiles {
	public class ZoneConditionsProfile {

		public string ProfileSubtypeId;

		public string ZoneName;
		public bool UseFactionOwnedZones;

		public double MinDistanceFromZoneCenter;
		public double MaxDistanceFromZoneCenter;

		public bool CheckCustomZoneCounters;
		public List<string> CustomZoneCounterName;
		public List<int> CustomZoneCounterValue;
		public Dictionary<string, int> CustomZoneCounterReference; //TODO: Build After Init Tags

		public bool CheckCustomZoneBools;
		public List<string> CustomZoneBoolName;
		public List<bool> CustomZoneBoolValue;
		public Dictionary<string, bool> CustomZoneBoolReference;  //TODO: Build After Init Tags

		public int MinSpawnedZoneEncounters;
		public int MaxSpawnedZoneEncounters;

		public ZoneConditionsProfile() {

			ProfileSubtypeId = "";

			ZoneName = "";
			UseFactionOwnedZones = false;

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

	}

}
