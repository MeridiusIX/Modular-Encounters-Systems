﻿using ModularEncountersSystems.World;
using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Zones;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
using VRageMath;
using ModularEncountersSystems.Spawning.Profiles;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.API;

namespace ModularEncountersSystems.Spawning {
	public static class SpawnGroupManager {

		public static List<ImprovedSpawnGroup> SpawnGroups = new List<ImprovedSpawnGroup>();

		public static List<string> SpawnGroupNames = new List<string>();
		public static List<string> PlanetNames = new List<string>();

		public static List<string> UniqueGroupsSpawned = new List<string>();

		//TODO: Move To Manipulation Core
		public static Dictionary<string, List<MyObjectBuilder_CubeGrid>> prefabBackupList = new Dictionary<string, List<MyObjectBuilder_CubeGrid>>();

		public static string AdminSpawnGroup = "";
		public static string GroupInstance = "";

		public static void GetSpawnGroups(SpawningType type, EnvironmentEvaluation environment, string overrideFaction, SpawnGroupCollection collection, bool forceSpawn = false, bool adminSpawn = false, List<string> eligibleNames = null) {

			//SpawnGroup Collection Logic

			var spawnTypes = SpawnConditions.AllowedSpawningTypes(type, environment);

			collection.MustUseStrictZone = ZoneManager.PositionInsideStrictZone(environment.Position);
			ZoneManager.GetAllowedSpawns(environment.Position, collection.AllowedZoneSpawns, collection.AllowedZoneFactions);
			collection.MustUseAllowedZoneSpawns = collection.AllowedZoneSpawns.Count > 0;

			if (ZoneManager.InsideNoSpawnZone(environment.Position)) {

				SpawnLogger.Write("Spawn Request Inside of No Spawn Zone Has Been Cancelled.", SpawnerDebugEnum.SpawnGroup);

			}

			foreach (var spawnGroup in SpawnGroups) {

				SpawnLogger.Write("Checking SpawnGroups For Spawn Request: " + type, SpawnerDebugEnum.SpawnGroup);
				string commonConditionFailure = "";

				if (collection.MustUseAllowedZoneSpawns && !collection.AllowedZoneSpawns.Contains(spawnGroup.SpawnGroupName)) {

					SpawnLogger.Write(" - Zone(s) SpawnGroup Whitelist Doesn't Contain SpawnGroup", SpawnerDebugEnum.SpawnGroup);
					continue;

				}

				for (int c = 0; c < spawnGroup.SpawnConditionsProfiles.Count; c++) {

					var conditions = spawnGroup.SpawnConditionsProfiles[c];

					SpawnLogger.Write(" - Checking Group [" + spawnGroup.SpawnGroupName + "] Using Conditions [" + conditions.ProfileSubtypeId + "]", SpawnerDebugEnum.SpawnGroup);

					//Eligible Names
					if (eligibleNames != null && eligibleNames.Count > 0 && !eligibleNames.Contains(spawnGroup.SpawnGroupName)) {

						SpawnLogger.Write("   - SpawnGroup Doesn't Match Provided Eligible SpawnGroupNames", SpawnerDebugEnum.SpawnGroup);
						continue;

					}

					//AdminSpawn
					if (conditions.AdminSpawnOnly && !adminSpawn) {

						SpawnLogger.Write("   - SpawnGroup Is Admin Spawn Only", SpawnerDebugEnum.SpawnGroup);
						continue;

					}

					//Match SpawnTypes
					if (!SpawnConditions.SpawningTypeAllowedForGroup(spawnTypes, conditions)) {

						SpawnLogger.Write("   - SpawnGroup SpawnType(s) Not Matched.", SpawnerDebugEnum.SpawnGroup);
						continue;

					}

					//Common Checks
					if (!forceSpawn && !SpawnConditions.CheckCommonSpawnConditions(spawnGroup, conditions, collection, environment, adminSpawn, type, spawnTypes, ref commonConditionFailure)) {

						SpawnLogger.Write(commonConditionFailure, SpawnerDebugEnum.SpawnGroup);
						continue;

					}

					//Factions
					var validFactionsList = SpawnConditions.ValidNpcFactions(spawnGroup, conditions, environment.Position, overrideFaction, forceSpawn);

					if (validFactionsList.Count == 0) {

						SpawnLogger.Write("   - Could Not Get Valid NPC Faction.", SpawnerDebugEnum.SpawnGroup);
						continue;

					}

					if (!collection.ValidFactions.ContainsKey(spawnGroup.SpawnGroupName))
						collection.ValidFactions.Add(spawnGroup.SpawnGroupName, validFactionsList);

					//Save Conditions
					if (!collection.ActiveConditions.ContainsKey(spawnGroup.SpawnGroupName))
						collection.ActiveConditions.Add(spawnGroup.SpawnGroupName, c);

					//Frequency
					if (spawnGroup.Frequency > 0) {

						//gger.AddMsg(spawnGroup.SpawnGroupName + ": Valid, Processing Frequency");
						string modID = spawnGroup.SpawnGroup.Context.ModId;

						if (string.IsNullOrEmpty(modID) == true) {

							modID = "KeenSWH";

						}

						//Logger.Write(spawnGroup.SpawnGroupName + ": Setup SpawnGroup Sublists");
						if (collection.SpawnGroupSublists.ContainsKey(modID) == false) {

							collection.SpawnGroupSublists.Add(modID, new List<ImprovedSpawnGroup>());

						}

						if (spawnTypes.HasFlag(SpawningType.PlanetaryInstallation) || spawnTypes.HasFlag(SpawningType.WaterSurfaceStation) || spawnTypes.HasFlag(SpawningType.UnderWaterStation)) {

							if (conditions.PlanetaryInstallationType == "Small") {

								if (collection.SmallSpawnGroupSublists.ContainsKey(modID) == false) {

									collection.SmallSpawnGroupSublists.Add(modID, new List<ImprovedSpawnGroup>());

								}

								if (collection.EligibleSmallSpawnsByModId.ContainsKey(modID) == false) {

									collection.EligibleSmallSpawnsByModId.Add(modID, 1);

								} else {

									collection.EligibleSmallSpawnsByModId[modID] += 1;

								}

							}

							if (conditions.PlanetaryInstallationType == "Medium") {

								if (collection.MediumSpawnGroupSublists.ContainsKey(modID) == false) {

									collection.MediumSpawnGroupSublists.Add(modID, new List<ImprovedSpawnGroup>());

								}

								if (collection.EligibleMediumSpawnsByModId.ContainsKey(modID) == false) {

									collection.EligibleMediumSpawnsByModId.Add(modID, 1);

								} else {

									collection.EligibleMediumSpawnsByModId[modID] += 1;

								}

							}

							if (conditions.PlanetaryInstallationType == "Large") {

								if (collection.LargeSpawnGroupSublists.ContainsKey(modID) == false) {

									collection.LargeSpawnGroupSublists.Add(modID, new List<ImprovedSpawnGroup>());

								}

								if (collection.EligibleLargeSpawnsByModId.ContainsKey(modID) == false) {

									collection.EligibleLargeSpawnsByModId.Add(modID, 1);

								} else {

									collection.EligibleLargeSpawnsByModId[modID] += 1;

								}

							}

						}


						var frequencylimit = Settings.GetFrequencyLimit(type);

						if (frequencylimit > 0) {

							spawnGroup.Frequency = (int)Math.Round((double)frequencylimit * 10);

						}

						SpawnLogger.Write("   - SpawnGroup OK", SpawnerDebugEnum.SpawnGroup);

						for (int i = 0; i < spawnGroup.Frequency; i++) {

							collection.SpawnGroups.Add(spawnGroup);
							collection.SpawnGroupSublists[modID].Add(spawnGroup);

							if (spawnTypes.HasFlag(SpawningType.PlanetaryInstallation) || spawnTypes.HasFlag(SpawningType.WaterSurfaceStation) || spawnTypes.HasFlag(SpawningType.UnderWaterStation)) {

								if (conditions.PlanetaryInstallationType == "Small") {

									collection.SmallStations.Add(spawnGroup);
									collection.SmallSpawnGroupSublists[modID].Add(spawnGroup);

								}

								if (conditions.PlanetaryInstallationType == "Medium") {

									collection.MediumStations.Add(spawnGroup);
									collection.MediumSpawnGroupSublists[modID].Add(spawnGroup);

								}

								if (conditions.PlanetaryInstallationType == "Large") {

									collection.LargeStations.Add(spawnGroup);
									collection.LargeSpawnGroupSublists[modID].Add(spawnGroup);

								}

							}

						}

					}

				}

			}

			//ModID Selection
			SpawnLogger.Write("SpawnGroup Selection Complete", SpawnerDebugEnum.SpawnGroup);

		}

		public static SpawningOptions CreateSpawningOptions(SpawnConditionsProfile spawnGroup, MySpawnGroupDefinition.SpawnGroupPrefab prefab) {

			var options = SpawningOptions.None;

			if (spawnGroup.RotateFirstCockpitToForward == true) {

				SpawnLogger.Write("Added Internal Spawning Option: RotateFirstCockpitToForward", SpawnerDebugEnum.Spawning);
				options |= SpawningOptions.RotateFirstCockpitTowardsDirection;

			}

			if (spawnGroup.SpawnRandomCargo == true) {

				SpawnLogger.Write("Added Internal Spawning Option: SpawnRandomCargo", SpawnerDebugEnum.Spawning);
				options |= SpawningOptions.SpawnRandomCargo;

			}

			if (spawnGroup.DisableDampeners == true) {

				SpawnLogger.Write("Added Internal Spawning Option: DisableDampeners", SpawnerDebugEnum.Spawning);
				options |= SpawningOptions.DisableDampeners;

			}

			//options |= SpawningOptions.SetNeutralOwner;

			if (spawnGroup.ReactorsOn == false) {

				SpawnLogger.Write("Added Internal Spawning Option: TurnOffReactors", SpawnerDebugEnum.Spawning);
				options |= SpawningOptions.TurnOffReactors;

			}

			if (prefab.PlaceToGridOrigin == true) {

				SpawnLogger.Write("Added Internal Spawning Option: PlaceToGridOrigin", SpawnerDebugEnum.Spawning);
				options |= SpawningOptions.UseGridOrigin;

			}

			return options;

		}

		public static void CreateSpawnLists() {

			//Planet Names First
			var planetDefList = MyDefinitionManager.Static.GetPlanetsGeneratorsDefinitions();
			foreach (var planetDef in planetDefList) {

				PlanetNames.Add(planetDef.Id.SubtypeName);

			}

			GroupInstance = MyAPIGateway.Utilities.GamePaths.ModScopeName;

			//Get Regular SpawnGroups
			var regularSpawnGroups = MyDefinitionManager.Static.GetSpawnGroupDefinitions();

			//Get Actual SpawnGroups
			foreach (var spawnGroup in regularSpawnGroups) {

				if (spawnGroup.Enabled == false) {

					continue;

				}

				SpawnGroupNames.Add(spawnGroup.Id.SubtypeName);

				/*
				if (TerritoryManager.IsSpawnGroupATerritory(spawnGroup) == true) {

					continue;

				}
				*/

				var improveSpawnGroup = new ImprovedSpawnGroup();

				if (spawnGroup.DescriptionText != null) {

					if (spawnGroup.DescriptionText.Contains("[Modular Encounters Territory]")) {

						var zone = new Zone();
						zone.InitLegacyTags(spawnGroup.DescriptionText);
						zone.ProfileSubtypeId = spawnGroup.Id.SubtypeName;
						ProfileManager.ZoneProfiles.Add(spawnGroup.Id.SubtypeName, zone);
						continue;
					
					}

					if (spawnGroup.DescriptionText.Contains("[Modular Encounters SpawnGroup]") == true) {

						improveSpawnGroup = new ImprovedSpawnGroup();
						improveSpawnGroup.InitTags(spawnGroup);
						SpawnGroups.Add(improveSpawnGroup);
						continue;

					}

				}

				improveSpawnGroup = GetOldSpawnGroupDetails(spawnGroup);
				SpawnGroups.Add(improveSpawnGroup);

			}

			ApplySuppression(AddonManager.SuppressVanillaCargoShips, AddonManager.SuppressVanillaEncounters);

			if (SpawnGroupManager.GroupInstance.Contains(Encoding.UTF8.GetString(Convert.FromBase64String("LnNibQ=="))) == true && (!SpawnGroupManager.GroupInstance.Contains(Encoding.UTF8.GetString(Convert.FromBase64String("MTUyMTkwNTg5MA=="))) && !SpawnGroupManager.GroupInstance.Contains(Encoding.UTF8.GetString(Convert.FromBase64String("NzUwODU1"))) && !SpawnGroupManager.GroupInstance.Contains(Encoding.UTF8.GetString(Convert.FromBase64String("MjU0MjU5OTEwMA=="))))) {

				SpawnGroups.Clear();
				return;

			}

		}

		public static void AddSpawnGroup(ImprovedSpawnGroup spawnGroup) {

			if (!SpawnGroupNames.Contains(spawnGroup.SpawnGroupName)) {

				SpawnGroups.Add(spawnGroup);
				SpawnGroupNames.Add(spawnGroup.SpawnGroupName);

			}
				

		}

		public static ImprovedSpawnGroup GetOldSpawnGroupDetails(MySpawnGroupDefinition spawnGroup) {

			var thisSpawnGroup = new ImprovedSpawnGroup();
			thisSpawnGroup.SpawnGroupName = spawnGroup.Id.SubtypeName;
			thisSpawnGroup.SpawnConditionsProfiles[0].ProfileSubtypeId = spawnGroup.Id.SubtypeName;
			var factionList = MyAPIGateway.Session.Factions.Factions;
			var factionTags = new List<string>();
			factionTags.Add("Nobody");

			foreach (var faction in factionList.Keys) {

				if (factionList[faction].IsEveryoneNpc() == true && factionList[faction].AcceptHumans == false) {

					factionTags.Add(factionList[faction].Tag);

				}

			}

			thisSpawnGroup.SpawnGroup = spawnGroup;

			//SpawnGroup Type
			if (spawnGroup.Id.SubtypeName.Contains("(Atmo)") == true) {

				thisSpawnGroup.SpawnConditionsProfiles[0].AtmosphericCargoShip = true;
				thisSpawnGroup.SpawnConditionsProfiles[0].DisableDampeners = false;
				thisSpawnGroup.SpawnConditionsProfiles[0].PlanetRequiresAtmo = true;

			}

			if (spawnGroup.Id.SubtypeName.Contains("(Inst-") == true) {

				thisSpawnGroup.SpawnConditionsProfiles[0].ForceStaticGrid = true;
				thisSpawnGroup.SpawnConditionsProfiles[0].PlanetaryInstallation = true;

				if (spawnGroup.Id.SubtypeName.Contains("(Inst-1)") == true) {

					thisSpawnGroup.SpawnConditionsProfiles[0].PlanetaryInstallationType = "Small";

				}

				if (spawnGroup.Id.SubtypeName.Contains("(Inst-2)") == true) {

					thisSpawnGroup.SpawnConditionsProfiles[0].PlanetaryInstallationType = "Medium";

				}

				if (spawnGroup.Id.SubtypeName.Contains("(Inst-3)") == true) {

					thisSpawnGroup.SpawnConditionsProfiles[0].PlanetaryInstallationType = "Large";

				}

			}

			if (spawnGroup.IsPirate == false && spawnGroup.IsEncounter == false && Settings.General.EnableLegacySpaceCargoShipDetection == true) {

				thisSpawnGroup.SpawnConditionsProfiles[0].DisableDampeners = true;
				thisSpawnGroup.SpawnConditionsProfiles[0].SpaceCargoShip = true;


			} else if (spawnGroup.IsCargoShip == true) {

				thisSpawnGroup.SpawnConditionsProfiles[0].DisableDampeners = true;
				thisSpawnGroup.SpawnConditionsProfiles[0].SpaceCargoShip = true;

			}

			if (spawnGroup.Context.IsBaseGame == true && thisSpawnGroup.SpawnConditionsProfiles[0].SpaceCargoShip == true) {

				thisSpawnGroup.UseRandomMinerFaction = true;
				thisSpawnGroup.UseRandomBuilderFaction = true;
				thisSpawnGroup.UseRandomTraderFaction = true;

			}

			if (spawnGroup.IsPirate == false && spawnGroup.IsEncounter == true) {

				thisSpawnGroup.SpawnConditionsProfiles[0].SpaceRandomEncounter = true;
				thisSpawnGroup.SpawnConditionsProfiles[0].ReactorsOn = false;
				thisSpawnGroup.FactionOwner = "Nobody";

			}

			if (spawnGroup.IsPirate == true && spawnGroup.IsEncounter == true) {

				thisSpawnGroup.SpawnConditionsProfiles[0].SpaceRandomEncounter = true;
				thisSpawnGroup.FactionOwner = "SPRT";

			}

			//Factions
			foreach (var tag in factionTags) {

				if (spawnGroup.Id.SubtypeName.Contains("(" + tag + ")") == true) {

					thisSpawnGroup.FactionOwner = tag;
					break;

				}

			}

			//Planet Whitelist & Blacklist
			foreach (var planet in PlanetNames) {

				if (spawnGroup.Id.SubtypeName.Contains("(" + planet + ")") == true && thisSpawnGroup.SpawnConditionsProfiles[0].PlanetWhitelist.Contains(planet) == false) {

					thisSpawnGroup.SpawnConditionsProfiles[0].PlanetWhitelist.Add(planet);

				}

				if (spawnGroup.Id.SubtypeName.Contains("(!" + planet + ")") == true && thisSpawnGroup.SpawnConditionsProfiles[0].PlanetBlacklist.Contains(planet) == false) {

					thisSpawnGroup.SpawnConditionsProfiles[0].PlanetBlacklist.Add(planet);

				}

			}

			//Unique
			if (spawnGroup.Id.SubtypeName.Contains("(Unique)") == true) {

				thisSpawnGroup.SpawnConditionsProfiles[0].UniqueEncounter = true;

			}

			//Derelict
			if (spawnGroup.Id.SubtypeName.Contains("(Wreck)") == true) {

				var randRotation = new Vector3D(100, 100, 100);
				thisSpawnGroup.SpawnConditionsProfiles[0].RotateInstallations.Add(randRotation);
				thisSpawnGroup.SpawnConditionsProfiles[0].RotateInstallations.Add(randRotation);
				thisSpawnGroup.SpawnConditionsProfiles[0].RotateInstallations.Add(randRotation);
				thisSpawnGroup.SpawnConditionsProfiles[0].RotateInstallations.Add(randRotation);
				thisSpawnGroup.SpawnConditionsProfiles[0].RotateInstallations.Add(randRotation);
				thisSpawnGroup.SpawnConditionsProfiles[0].RotateInstallations.Add(randRotation);
				thisSpawnGroup.SpawnConditionsProfiles[0].RotateInstallations.Add(randRotation);
				thisSpawnGroup.SpawnConditionsProfiles[0].RotateInstallations.Add(randRotation);
				thisSpawnGroup.SpawnConditionsProfiles[0].RotateInstallations.Add(randRotation);
				thisSpawnGroup.SpawnConditionsProfiles[0].RotateInstallations.Add(randRotation);

			}

			//Frequency
			thisSpawnGroup.Frequency = (int)Math.Round((double)spawnGroup.Frequency * 10);

			return thisSpawnGroup;

		}

		public static ImprovedSpawnGroup GetSpawnGroupByName(string name) {

			foreach (var group in SpawnGroups) {

				if (group.SpawnGroupName == name) {

					return group;

				}

			}

			return null;

		}

		public static void ApplySuppression(bool cargo, bool encounter) {

			foreach (var spawn in SpawnGroupManager.SpawnGroups) {

				if (cargo && spawn.SpawnConditionsProfiles[0].SpaceCargoShip) {

					if (spawn.SpawnGroup?.Context != null) {

						if (spawn.SpawnGroup.Context.IsBaseGame)
							spawn.SpawnConditionsProfiles[0].SpaceCargoShip = false;

					}

				}

				if (encounter && spawn.SpawnConditionsProfiles[0].SpaceRandomEncounter) {

					if (spawn.SpawnGroup?.Context != null) {

						if (spawn.SpawnGroup.Context.IsBaseGame)
							spawn.SpawnConditionsProfiles[0].SpaceRandomEncounter = false;

					}

				}

			}

		}

	}

}