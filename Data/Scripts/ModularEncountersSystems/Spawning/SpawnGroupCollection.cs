using ModularEncountersSystems.World;
using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModularEncountersSystems.Spawning.Profiles;
using ModularEncountersSystems.Zones;
using VRageMath;

namespace ModularEncountersSystems.Spawning {
	public class SpawnGroupCollection {

		public ImprovedSpawnGroup SpawnGroup;
		public int ConditionsIndex;
		public int ZoneIndex;
		public string Faction;

		public SpawnConditionsProfile Conditions;
		public Zone ActiveZone;

		public StaticEncounter StaticEncounterInstance;

		public List<ImprovedSpawnGroup> SpawnGroups;

		public List<ImprovedSpawnGroup> SmallStations;
		public List<ImprovedSpawnGroup> MediumStations;
		public List<ImprovedSpawnGroup> LargeStations;

		public List<int> PrefabIndexes;
		public int SpawnedPrefabs;

		public Dictionary<string, List<string>> ValidFactions;
		public Dictionary<string, int> ActiveConditions;

		public long OwnerOverride;

		public Dictionary<string, List<ImprovedSpawnGroup>> SpawnGroupSublists;
		public Dictionary<string, List<ImprovedSpawnGroup>> SmallSpawnGroupSublists;
		public Dictionary<string, List<ImprovedSpawnGroup>> MediumSpawnGroupSublists;
		public Dictionary<string, List<ImprovedSpawnGroup>> LargeSpawnGroupSublists;

		public Dictionary<string, int> EligibleSpawnsByModId;
		public Dictionary<string, int> EligibleSmallSpawnsByModId;
		public Dictionary<string, int> EligibleMediumSpawnsByModId;
		public Dictionary<string, int> EligibleLargeSpawnsByModId;

		public bool SkippedAbsentMediumStation;
		public bool SkippedAbsentLargeStation;

		public bool MustUseStrictZone;
		public bool MustUseAllowedZoneSpawns;
		public List<string> AllowedZoneSpawns;
		public List<string> OnlyAllowedZoneSpawns;
		public List<string> AllowedZoneFactions;
		public List<string> RestrictedZoneSpawnGroups;

		public bool IgnoreAllSafetyChecks = false;
		public bool SkipGridSpawnChecks { get {

				if (IgnoreAllSafetyChecks)
					return true;

				if (Conditions == null)
					return false;

				return Conditions.SkipGridSpawnChecks;

			} 

		}

		public bool SkipVoxelSpawnChecks {
			get {

				if (IgnoreAllSafetyChecks)
					return true;

				if (Conditions == null)
					return false;

				return Conditions.SkipVoxelSpawnChecks;

			}

		}

		public SpawnGroupCollection() {

			SpawnGroup = null;
			ConditionsIndex = 0;
			ZoneIndex = -1;
			Faction = "";

			SpawnGroups = new List<ImprovedSpawnGroup>();

			SmallStations = new List<ImprovedSpawnGroup>();
			MediumStations = new List<ImprovedSpawnGroup>();
			LargeStations = new List<ImprovedSpawnGroup>();

			PrefabIndexes = new List<int>();

			ValidFactions = new Dictionary<string, List<string>>();
			ActiveConditions = new Dictionary<string, int>();

			OwnerOverride = -1;

			SpawnGroupSublists = new Dictionary<string, List<ImprovedSpawnGroup>>();
			SmallSpawnGroupSublists = new Dictionary<string, List<ImprovedSpawnGroup>>();
			MediumSpawnGroupSublists = new Dictionary<string, List<ImprovedSpawnGroup>>();
			LargeSpawnGroupSublists = new Dictionary<string, List<ImprovedSpawnGroup>>();

			EligibleSpawnsByModId = new Dictionary<string, int>();
			EligibleSmallSpawnsByModId = new Dictionary<string, int>();
			EligibleMediumSpawnsByModId = new Dictionary<string, int>();
			EligibleLargeSpawnsByModId = new Dictionary<string, int>();

			MustUseStrictZone = false;
			MustUseAllowedZoneSpawns = false;
			AllowedZoneSpawns = new List<string>();
			OnlyAllowedZoneSpawns = new List<string>();
			AllowedZoneFactions = new List<string>();
			RestrictedZoneSpawnGroups = new List<string>();

		}

		public void InitFromBossEncounter(StaticEncounter encounter) {

			SpawnGroup = SpawnGroupManager.GetSpawnGroupByName(encounter.SpawnGroupName);

			if (SpawnGroup == null)
				return;

			ConditionsIndex = encounter.ConditionIndex;
			Conditions = SpawnGroup.SpawnConditionsProfiles[ConditionsIndex];
			Faction = encounter.Faction;
			SelectPrefabIndexes();

		}

		public bool SelectRandomSpawnGroup(SpawningType type, EnvironmentEvaluation environment) {

			if (type == SpawningType.PlanetaryInstallation) {

				//Logger.Write("Found " + (spawnGroupList.Count / 10).ToString() + " Potential Spawn Groups. Small: " + (smallStations.Count / 10).ToString() + " // Medium: " + (mediumStations.Count / 10).ToString() + " // Large: " + (largeStations.Count / 10).ToString(), true);

				string stationSize = "Small";
				SpawnGroups = SmallStations; ;

				//Start With Small Station Always, Try Chance For Medium.
				if (stationSize == "Small" && SmallStations.Count == 0) {

					//No Small Stations Available For This Area, So Try Medium.
					stationSize = "Medium";
					SpawnGroups = MediumStations;

				} else if (stationSize == "Small" && SmallStations.Count != 0) {

					int mediumChance = 0;
					string varName = "MES-" + environment.NearestPlanet.Planet.EntityId.ToString() + "-Medium";

					if (MyAPIGateway.Utilities.GetVariable<int>(varName, out mediumChance) == false) {

						mediumChance = Settings.PlanetaryInstallations.MediumSpawnChanceBaseValue;
						MyAPIGateway.Utilities.SetVariable<int>(varName, mediumChance);

					}

					if (MathTools.RandomBetween(0, 100) < mediumChance) {

						stationSize = "Medium";
						SpawnGroups = MediumStations;

					}

				}

				if (stationSize == "Medium" && MediumStations.Count == 0) {

					//No Medium Stations Available For This Area, So Try Large.
					SkippedAbsentMediumStation = true;
					stationSize = "Large";
					SpawnGroups = LargeStations;

				} else if (stationSize == "Medium" && MediumStations.Count != 0) {

					int largeChance = 0;
					string varName = "MES-" + environment.NearestPlanet.Planet.EntityId.ToString() + "-Large";

					if (MyAPIGateway.Utilities.GetVariable<int>(varName, out largeChance) == false) {

						largeChance = Settings.PlanetaryInstallations.LargeSpawnChanceBaseValue;
						MyAPIGateway.Utilities.SetVariable<int>(varName, largeChance);

					}

					if (MathTools.RandomBetween(0, 100) < largeChance) {

						stationSize = "Large";
						SpawnGroups = LargeStations;

					}

				}

				if (stationSize == "Large" && LargeStations.Count == 0) {

					SkippedAbsentLargeStation = true;
					stationSize = "Medium";
					SpawnGroups = MediumStations;

					if (MediumStations.Count == 0) {

						SkippedAbsentMediumStation = true;
						stationSize = "Small";
						SpawnGroups = SmallStations;

					}

				}

			}

			if (SpawnGroups.Count > 0) {

				SpawnGroup = SpawnGroups[MathTools.RandomBetween(0, SpawnGroups.Count)];

				if (!ActiveConditions.TryGetValue(SpawnGroup.SpawnGroupName, out ConditionsIndex)) {

					SpawnLogger.Write("Failed To Get SpawnConditionProfile Index At Spawn Selection. Using Index 0", SpawnerDebugEnum.Spawning);
					ConditionsIndex = 0;

				}
				
				Conditions = SpawnGroup.SpawnConditionsProfiles[ConditionsIndex];
				SelectPrefabIndexes();
				return true;

			}

			return false;

		}

		public void SelectPrefabIndexes() {

			if (Conditions == null)
				return;

			SpawnConditionsProfile conditions = Conditions.UseSpawnGroupPrefabSpawningMode ? SpawnGroup.SpawnConditionsProfiles[0] : Conditions;
			var spawnGroup = SpawnGroup.SpawnGroup;
			SpawnLogger.Write("Prefab Spawning Mode: " + conditions.PrefabSpawningMode, SpawnerDebugEnum.Spawning);

			if (conditions.PrefabSpawningMode == PrefabSpawnMode.All) {

				for (int i = 0; i < spawnGroup.Prefabs.Count; i++) {

					PrefabIndexes.Add(i);
				
				}
			
			}

			if (conditions.PrefabSpawningMode == PrefabSpawnMode.Random) {

				if (spawnGroup.Prefabs.Count == 1) {

					PrefabIndexes.Add(0);

				} else {

					PrefabIndexes.Add(MathTools.RandomBetween(0, spawnGroup.Prefabs.Count));

				}
			
			}

			if (conditions.PrefabSpawningMode == PrefabSpawnMode.SelectedIndexes) {

				for (int i = 0; i < conditions.PrefabIndexes.Count; i++) {

					if (!PrefabIndexes.Contains(conditions.PrefabIndexes[i]) || conditions.AllowPrefabIndexReuse)
						PrefabIndexes.Add(conditions.PrefabIndexes[i]);

				}

			}

			if (conditions.PrefabSpawningMode == PrefabSpawnMode.RandomSelectedIndexes) {

				foreach (var group in conditions.PrefabIndexGroups.Keys) {

					int indexToAdd = -1;
					var list = conditions.PrefabIndexGroups[group];

					if (spawnGroup.Prefabs.Count == 1) {

						indexToAdd = 0;

					} else {

						indexToAdd = MathTools.RandomBetween(0, list.Count);

					}

					if (list[indexToAdd] >= 0 && (!PrefabIndexes.Contains(list[indexToAdd]) || conditions.AllowPrefabIndexReuse)) {

						SpawnLogger.Write("Prefab Index Selected: " + list[indexToAdd], SpawnerDebugEnum.Spawning);
						PrefabIndexes.Add(list[indexToAdd]);

					}

				}

			}

			if (conditions.PrefabSpawningMode == PrefabSpawnMode.RandomFixedCount) {

				for (int i = 0; i < conditions.PrefabFixedCount; i++) {

					int indexToAdd = -1;

					for (int j = 0; j < 10; j++) {

						indexToAdd = MathTools.RandomBetween(0, spawnGroup.Prefabs.Count);

						if (indexToAdd >= 0 && (!PrefabIndexes.Contains(indexToAdd) || conditions.AllowPrefabIndexReuse)) {

							SpawnLogger.Write("Prefab Index Selected: " + indexToAdd, SpawnerDebugEnum.Spawning);
							PrefabIndexes.Add(indexToAdd);
							break;

						}

					}

				}

			}

			for (int i = PrefabIndexes.Count - 1; i >= 0; i--) {

				if (PrefabIndexes[i] >= spawnGroup.Prefabs.Count)
					PrefabIndexes.RemoveAt(i);

			}

		}

		public Vector3 SelectPrefabOffet(Vector3 originalOffset, int customIndex = -1) {

			var index = customIndex > -1 ? customIndex : SpawnedPrefabs;

			if (index < Conditions.PrefabOffsetOverrides.Count)
				return (Vector3)Conditions.PrefabOffsetOverrides[index];

			return originalOffset;

		}

		public string SelectRandomFaction() {

			if (!string.IsNullOrWhiteSpace(Faction))
				return Faction;

			if (SpawnGroup == null) {
			
				Faction = "Nobody";
				SpawnLogger.Write("SpawnGroup Null: " + SpawnGroup.SpawnGroupName, SpawnerDebugEnum.Spawning);
				SpawnLogger.Write("Faction [" + Faction + "] Selected For SpawnGroup " + SpawnGroup.SpawnGroupName, SpawnerDebugEnum.Spawning);
				return Faction;

			}
			

			List<string> factions = null;

			if (!string.IsNullOrWhiteSpace(Faction))
				return Faction;

			if (!ValidFactions.TryGetValue(SpawnGroup.SpawnGroupName, out factions)) {

				Faction = "Nobody";
				SpawnLogger.Write("No Factions Found Assigned To SpawnGroup: " + SpawnGroup.SpawnGroupName, SpawnerDebugEnum.Spawning);
				SpawnLogger.Write("Faction [" + Faction + "] Selected For SpawnGroup " + SpawnGroup.SpawnGroupName, SpawnerDebugEnum.Spawning);
				return Faction;

			}

			if (factions == null || factions.Count == 0) {

				Faction = "Nobody";
				SpawnLogger.Write("Eligible Factions Empty or Null for SpawnGroup: " + SpawnGroup.SpawnGroupName, SpawnerDebugEnum.Spawning);
				SpawnLogger.Write("Faction [" + Faction + "] Selected For SpawnGroup " + SpawnGroup.SpawnGroupName, SpawnerDebugEnum.Spawning);
				return Faction;

			}

			Faction = factions[MathTools.RandomBetween(0, factions.Count)];
			SpawnLogger.Write("Faction [" + Faction + "] Selected For SpawnGroup " + SpawnGroup.SpawnGroupName, SpawnerDebugEnum.Spawning);
			return Faction;

		}

		public void SelectSpawnGroupSublist(Dictionary<string, List<ImprovedSpawnGroup>> sublists, Dictionary<string, int> modIdEligibleGroups, ref List<ImprovedSpawnGroup> result) {

			var sublistKeys = sublists.Keys;

			if (sublistKeys.Count == 0) {

				result.Clear();
				return;

			}

			if (Settings.General.UseWeightedModIdSelection == true) {

				var weighedKeyList = new List<string>();

				//SpawnLogger.Write("Mods Using SpawnGroups: " + sublistKeys.Count, SpawnerDebugEnum.SpawnGroup);

				foreach (var key in sublistKeys) {

					int groupCount = 0;
					int listCount = 1;

					if (modIdEligibleGroups.TryGetValue(key, out groupCount) == false) {

						weighedKeyList.Add(key);
						continue;

					}

					if (groupCount >= 0 && groupCount <= Settings.General.LowWeightModIdSpawnGroups) {

						listCount = Settings.General.LowWeightModIdModifier;
						SpawnLogger.Write("Eligible Spawns For: " + key + " / " + groupCount.ToString() + " - Classified as Low Weighted Mod With Value Of " + listCount.ToString(), SpawnerDebugEnum.SpawnGroup);

					}

					if (groupCount > Settings.General.LowWeightModIdSpawnGroups && groupCount <= Settings.General.MediumWeightModIdSpawnGroups) {

						listCount = Settings.General.MediumWeightModIdModifier;
						SpawnLogger.Write("Eligible Spawns For: " + key + " / " + groupCount.ToString() + " - Classified as Medium Weighted Mod With Value Of " + listCount.ToString(), SpawnerDebugEnum.SpawnGroup);

					}

					if (groupCount >= Settings.General.HighWeightModIdSpawnGroups) {

						listCount = Settings.General.HighWeightModIdModifier;
						SpawnLogger.Write("Eligible Spawns For: " + key + " / " + groupCount.ToString() + " - Classified as High Weighted Mod With Value Of " + listCount.ToString(), SpawnerDebugEnum.SpawnGroup);

					}

					for (int i = 0; i < listCount; i++) {

						weighedKeyList.Add(key);

					}

				}

				if (weighedKeyList.Count == 0) {

					result.Clear();
					return;

				}

				var randkey = weighedKeyList[MathTools.RandomBetween(0, weighedKeyList.Count)];
				SpawnLogger.Write(randkey + " Used For Weighted SpawnGroup Selection.", SpawnerDebugEnum.SpawnGroup);

				result = sublists[randkey];

			} else {

				SpawnLogger.Write("Weighted SpawnGroup Selection Not In Effect", SpawnerDebugEnum.SpawnGroup);
				var keyList = sublists.Keys.ToList();
				var key = keyList[MathTools.RandomBetween(0, keyList.Count)];
				result = sublists[key];

			}

		}

	}

}
