using ModularEncountersSystems.World;
using ModularEncountersSystems.API;
using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.World;
using ModularEncountersSystems.Zones;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;
using ModularEncountersSystems.Spawning.Profiles;
using VRage.Game;
using Sandbox.Game;
using ModularEncountersSystems.Behavior.Subsystems.Trigger;

namespace ModularEncountersSystems.Spawning {
	public static class SpawnConditions {

		public static Dictionary<string, int> SandboxVariableCache = new Dictionary<string, int>();
		public static MyObjectBuilder_SessionSettings SessionSettings = null;

		internal static StringBuilder _zoneDebug = new StringBuilder();

		public static SpawningType AllowedSpawningTypes(SpawningType type, EnvironmentEvaluation environment) {

			var spawnTypes = SpawningType.None;

			if (type == SpawningType.SpaceCargoShip) {

				if (environment.SpaceCargoShipsEligible)
					spawnTypes |= SpawningType.SpaceCargoShip;

				if(environment.LunarCargoShipsEligible)
					spawnTypes |= SpawningType.LunarCargoShip;

			}

			if (type == SpawningType.RandomEncounter) {

				if (environment.RandomEncountersEligible)
					spawnTypes |= SpawningType.RandomEncounter;

			}

			if (type == SpawningType.PlanetaryCargoShip) {

				if (environment.PlanetaryCargoShipsEligible)
					spawnTypes |= SpawningType.PlanetaryCargoShip;

				if (environment.GravityCargoShipsEligible)
					spawnTypes |= SpawningType.GravityCargoShip;

			}

			if (type == SpawningType.PlanetaryInstallation) {

				if (environment.PlanetaryInstallationEligible) {

					spawnTypes |= SpawningType.PlanetaryInstallation;
					spawnTypes |= SpawningType.DryLandInstallation;

				}

				if (environment.WaterInstallationEligible) {

					spawnTypes |= SpawningType.WaterSurfaceStation;
					spawnTypes |= SpawningType.UnderWaterStation;

				}
	
			}

			if (type == SpawningType.BossEncounter) {

				if (!environment.IsOnPlanet)

					spawnTypes |= SpawningType.BossSpace;

				else {

					spawnTypes |= SpawningType.BossGravity;

					if(environment.AtmosphereAtPosition >= 0.7)
						spawnTypes |= SpawningType.BossAtmo;

				}

			}

			if (type == SpawningType.StaticEncounter) {

				if (!environment.IsOnPlanet)

					spawnTypes |= SpawningType.StaticEncounterSpace;

				else {

					spawnTypes |= SpawningType.StaticEncounterPlanet;

				}

			}

			if (type == SpawningType.Creature) {
			
				if(environment.IsOnPlanet && environment.AltitudeAtPosition <= Settings.Creatures.MaxPlayerAltitudeForSpawn)
					spawnTypes |= SpawningType.Creature;

			}

			if (type == SpawningType.OtherNPC) {

				spawnTypes |= SpawningType.OtherNPC;

			}

			if (type == SpawningType.DroneEncounter) {

				spawnTypes |= SpawningType.DroneEncounter;

			}

			return spawnTypes;
		
		}

		public static bool SpawningTypeAllowedForGroup(SpawningType type, SpawnConditionsProfile conditions) {

			if (type.HasFlag(SpawningType.SpaceCargoShip) && conditions.SpaceCargoShip)
				return true;

			if (type.HasFlag(SpawningType.LunarCargoShip) && conditions.LunarCargoShip)
				return true;

			if (type.HasFlag(SpawningType.RandomEncounter) && conditions.SpaceRandomEncounter)
				return true;

			if (type.HasFlag(SpawningType.PlanetaryCargoShip) && conditions.AtmosphericCargoShip)
				return true;

			if (type.HasFlag(SpawningType.GravityCargoShip) && conditions.GravityCargoShip)
				return true;

			if (type.HasFlag(SpawningType.PlanetaryInstallation) && conditions.PlanetaryInstallation)
				return true;

			if (type.HasFlag(SpawningType.DryLandInstallation) && conditions.PlanetaryInstallation && conditions.InstallationSpawnsOnDryLand)
				return true;

			if (type.HasFlag(SpawningType.WaterSurfaceStation) && conditions.PlanetaryInstallation && conditions.InstallationSpawnsOnWaterSurface)
				return true;

			if (type.HasFlag(SpawningType.UnderWaterStation) && conditions.PlanetaryInstallation && conditions.InstallationSpawnsUnderwater)
				return true;

			if (type.HasFlag(SpawningType.BossSpace) && (conditions.BossEncounterSpace || conditions.BossEncounterAny))
				return true;

			if (type.HasFlag(SpawningType.BossAtmo) && (conditions.BossEncounterAtmo || conditions.BossEncounterAny))
				return true;

			if (type.HasFlag(SpawningType.BossGravity) && (conditions.BossEncounterAny))
				return true;

			if (type.HasFlag(SpawningType.Creature) && conditions.CreatureSpawn)
				return true;

			if (type.HasFlag(SpawningType.OtherNPC) && (conditions.RivalAiAnySpawn || conditions.RivalAiAtmosphericSpawn || conditions.RivalAiSpaceSpawn || conditions.RivalAiSpawn))
				return true;

			if (type.HasFlag(SpawningType.StaticEncounterSpace) || type.HasFlag(SpawningType.StaticEncounterPlanet) && conditions.StaticEncounter)
				return true;

			if (type.HasFlag(SpawningType.DroneEncounter) && conditions.DroneEncounter)
				return true;

			return false;

		}

		public static bool CheckSpawnGroupPlanetLists(SpawnConditionsProfile spawnGroup, EnvironmentEvaluation environment) {

			if (!environment.IsOnPlanet)
				return true;

			if (spawnGroup.PlanetBlacklist.Count > 0 && Settings.General.IgnorePlanetBlacklists == false) {

				if (spawnGroup.PlanetBlacklist.Contains(environment.NearestPlanetName) == true) {

					//Logger.SpawnGroupDebug(spawnGroup.SpawnGroup.Id.SubtypeName, "Planet Blacklisted");
					return false;

				}

			}

			if (spawnGroup.PlanetWhitelist.Count > 0 && Settings.General.IgnorePlanetWhitelists == false) {

				if (spawnGroup.PlanetWhitelist.Contains(environment.NearestPlanetName) == false) {

					//Logger.SpawnGroupDebug(spawnGroup.SpawnGroup.Id.SubtypeName, "Not On Whitelisted Planet");
					return false;

				}

			}

			if (spawnGroup.PlanetRequiresVacuum == true && environment.AtmosphereAtPosition > 0) {

				//Logger.SpawnGroupDebug(spawnGroup.SpawnGroup.Id.SubtypeName, "Planet Requires Vacuum");
				return false;

			}

			if (!spawnGroup.GravityCargoShip && spawnGroup.PlanetRequiresAtmo == true && environment.AtmosphereAtPosition == 0) {

				//Logger.SpawnGroupDebug(spawnGroup.SpawnGroup.Id.SubtypeName, "Planet Requires Atmo");
				return false;

			}

			if (spawnGroup.PlanetRequiresOxygen == true && environment.OxygenAtPosition == 0) {

				//Logger.SpawnGroupDebug(spawnGroup.SpawnGroup.Id.SubtypeName, "Planet Requires Oxygen");
				return false;

			}

			if (spawnGroup.PlanetMinimumSize > 0 && environment.PlanetDiameter < spawnGroup.PlanetMinimumSize) {

				//Logger.SpawnGroupDebug(spawnGroup.SpawnGroup.Id.SubtypeName, "Planet Min Size Fail");
				return false;

			}

			if (spawnGroup.PlanetMaximumSize > 0 && environment.PlanetDiameter < spawnGroup.PlanetMaximumSize) {

				//Logger.SpawnGroupDebug(spawnGroup.SpawnGroup.Id.SubtypeName, "Planet Max Size Fail");
				return false;

			}

			return true;

		}

		public static bool DistanceFromCenterCheck(SpawnConditionsProfile spawnGroup, EnvironmentEvaluation environment) {

			var distFromCenter = spawnGroup.CustomWorldCenter == Vector3D.Zero ? environment.DistanceFromWorldCenter : Vector3D.Distance(spawnGroup.CustomWorldCenter, environment.Position);

			if (spawnGroup.MinSpawnFromWorldCenter > 0) {

				if (distFromCenter < spawnGroup.MinSpawnFromWorldCenter) {

					return false;

				}

			}

			if (spawnGroup.MaxSpawnFromWorldCenter > 0) {

				if (distFromCenter > spawnGroup.MaxSpawnFromWorldCenter) {

					return false;

				}

			}

			if (spawnGroup.DirectionFromWorldCenter.Count > 0) {

				bool allowed = false;

				foreach (var direction in spawnGroup.DirectionFromWorldCenter) {

					if (direction != Vector3D.Zero) {

						var normalizedDirection = Vector3D.Normalize(direction - spawnGroup.CustomWorldCenter);
						var angleFromCoords = VectorHelper.GetAngleBetweenDirections(normalizedDirection, environment.DirectionFromWorldCenter);

						if (spawnGroup.MinAngleFromDirection > 0 && angleFromCoords < spawnGroup.MinAngleFromDirection)
							continue;

						if (spawnGroup.MaxAngleFromDirection > 0 && angleFromCoords > spawnGroup.MaxAngleFromDirection)
							continue;

						allowed = true;

					}

				}

				if (!allowed)
					return false;

			}

			if (environment.IsOnPlanet && spawnGroup.DirectionFromPlanetCenter.Count > 0) {

				bool allowed = false;

				foreach (var direction in spawnGroup.DirectionFromPlanetCenter) {

					if (direction != Vector3D.Zero) {

						var angleFromCoords = VectorHelper.GetAngleBetweenDirections(direction, Vector3D.Normalize(environment.Position - environment.NearestPlanet.Center()));

						if (spawnGroup.MinAngleFromPlanetCenterDirection > 0 && angleFromCoords < spawnGroup.MinAngleFromPlanetCenterDirection)
							continue;

						if (spawnGroup.MaxAngleFromPlanetCenterDirection > 0 && angleFromCoords > spawnGroup.MaxAngleFromPlanetCenterDirection)
							continue;

						allowed = true;
						break;

					}

				}

				if (!allowed)
					return false;

			}

			return true;

		}

		public static bool DistanceFromSurfaceCheck(SpawnConditionsProfile spawnGroup, EnvironmentEvaluation environment) {

			if (spawnGroup.MinSpawnFromPlanetSurface < 0 && spawnGroup.MaxSpawnFromPlanetSurface < 0) {

				return true;

			}

			if (environment.NearestPlanet == null && spawnGroup.MinSpawnFromPlanetSurface > 0)
				return true;

			if (environment.NearestPlanet == null && spawnGroup.MaxSpawnFromPlanetSurface > 0)
				return false;

			if (spawnGroup.MinSpawnFromPlanetSurface > 0 && spawnGroup.MinSpawnFromPlanetSurface > environment.AltitudeAtPosition)
				return false;

			if (spawnGroup.MaxSpawnFromPlanetSurface > 0 && spawnGroup.MaxSpawnFromPlanetSurface < environment.AltitudeAtPosition)
				return false;

			return true;

		}

		public static bool EnvironmentChecks(ImprovedSpawnGroup spawnGroup, SpawnConditionsProfile conditions, EnvironmentEvaluation environment, ref string failReason) {

			if (conditions.MinAirDensity != -1 && environment.AtmosphereAtPosition < conditions.MinAirDensity) {

				failReason = "   - MinAirDensity Check Failed";
				return false;

			}
				

			if (conditions.MaxAirDensity != -1 && environment.AtmosphereAtPosition > conditions.MinAirDensity) {

				failReason = "   - MaxAirDensity Check Failed";
				return false;

			}

			if (conditions.MinGravity != -1 && environment.GravityAtPosition < conditions.MinGravity) {

				failReason = "   - MinGravity Check Failed";
				return false;

			}

			if (conditions.MaxGravity != -1 && environment.GravityAtPosition > conditions.MaxGravity) {

				failReason = "   - MaxGravity Check Failed";
				return false;

			}

			if (conditions.CheckPrefabGravityProfiles) {

				foreach (var prefab in spawnGroup.SpawnGroup.Prefabs) {

					if (!PrefabGravityProfile.CheckPrefabGravity(prefab.SubtypeId, environment.GravityAtPosition, !environment.PlanetaryCargoShipsEligible)) {

						failReason = "   - Prefab Gravity Profiles Check Failed";
						return false;

					}
				
				}
			
			}

			if (conditions.UseDayOrNightOnly) {

				if (conditions.SpawnOnlyAtNight != environment.IsNight) {

					failReason = "   - Night Only Check Failed";
					return false;

				}

			}

			if (conditions.UseWeatherSpawning) {

				if (!conditions.AllowedWeatherSystems.Contains(environment.WeatherAtPosition)) {

					failReason = "   - Weather Check Failed";
					return false;

				}

			}

			if (conditions.UseTerrainTypeValidation) {

				if (!conditions.AllowedTerrainTypes.Contains(environment.CommonTerrainAtPosition)) {

					failReason = "   - Allowed Terrain Check Failed";
					return false;

				}

			}

			bool requiresWater = false;

			if (conditions.PlanetaryInstallation) {

				requiresWater = (!conditions.InstallationSpawnsOnDryLand && (conditions.InstallationSpawnsOnWaterSurface || conditions.InstallationSpawnsUnderwater));

			} else {

				requiresWater = conditions.MustSpawnUnderwater;

			}

			if (requiresWater) {

				if (!APIs.WaterModApiLoaded || environment.WaterInSurroundingAreaRatio < .1) {

					failReason = "   - Water Check Failed";
					return false;

				}

				/*
				if (spawnGroup.MinWaterDepth > 0 && environment.NearestPlanet.WaterDepthAtPosition(environment.SurfaceCoords) < spawnGroup.MinWaterDepth) {

					failReason = "   - Water Depth Check Failed";
					return false;

				}
				*/

			}

			return true;

		}

		public static bool NeededModsForSpawnGroup(SpawnConditionsProfile spawnGroup) {

			//Require All
			if (spawnGroup.RequireAllMods.Count > 0) {

				foreach (var item in spawnGroup.RequireAllMods) {

					if (AddonManager.ModIdList.Contains(item) == false) {

						return false;

					}

				}

			}

			//Require Any
			if (spawnGroup.RequireAnyMods.Count > 0) {

				bool gotMod = false;

				foreach (var item in spawnGroup.RequireAnyMods) {

					if (AddonManager.ModIdList.Contains(item) == true) {

						gotMod = true;
						break;

					}

				}

				if (gotMod == false) {

					return false;

				}

			}

			//Exclude All
			if (spawnGroup.ExcludeAllMods.Count > 0) {

				foreach (var item in spawnGroup.ExcludeAllMods) {

					if (AddonManager.ModIdList.Contains(item) == true) {

						return false;

					}

				}

			}

			//Exclude Any
			if (spawnGroup.ExcludeAnyMods.Count > 0) {

				bool conditionMet = false;

				foreach (var item in spawnGroup.ExcludeAnyMods) {

					if (AddonManager.ModIdList.Contains(item) == false) {

						conditionMet = true;
						break;

					}

				}

				if (conditionMet == false) {

					return false;

				}

			}

			return true;

		}

		public static bool IsSpawnGroupInBlacklist(string spawnGroupName) {

			//Get Blacklist
			var blacklistGroups = new List<string>(Settings.General.NpcSpawnGroupBlacklist.ToList());

			//Check Blacklist
			if (blacklistGroups.Contains(spawnGroupName) == true) {

				return true;

			}

			return false;

		}

		public static bool CheckBlacklists(SpawningType type, ImprovedSpawnGroup spawnGroup, EnvironmentEvaluation environment, SpawnConditionsProfile conditions, ref string failReason) {

			var modId = spawnGroup.SpawnGroup?.Context?.ModItem.PublishedFileId.ToString() ?? "N/A";
			var spawnTypeBlacklist = Settings.GetSpawnTypeBlacklist(type);
			var spawnTypePlanetBlacklist = Settings.GetSpawnTypePlanetBlacklist(type);
			var planetfilter = Settings.GetPlanetFilterForType(type, environment.NearestPlanet?.Planet?.EntityId ?? 0);

			//Global Blacklist SpawnGroup Name
			if (Settings.General.NpcSpawnGroupBlacklist.Contains<string>(spawnGroup.SpawnGroupName)) {

				failReason = "   - SpawnGroup Is In Global SpawnGroup Blacklist";
				return true;

			}

			//Global Blacklist Mod ID
			if (Settings.General.NpcSpawnGroupBlacklist.Contains<string>(modId)) {

				failReason = "   - SpawnGroup Is From A ModID In Global SpawnGroup Blacklist";
				return true;

			}

			//Global Blacklist Planet
			if (environment.IsOnPlanet && Settings.General.PlanetSpawnsDisableList.Contains<string>(environment.NearestPlanetName)) {

				failReason = "   - SpawnGroup Is On Planet That Is In Global Planet Blacklist";
				return true;

			}

			//Planet Spawn Filter
			if (environment.IsOnPlanet && planetfilter != null && environment.NearestPlanet.Planet.EntityId == planetfilter.PlanetId) {

				//Blacklist SpawnGroup Name
				if (planetfilter.PlanetSpawnGroupBlacklist.Contains<string>(spawnGroup.SpawnGroupName)) {

					failReason = "   - SpawnGroup Name Is Blacklisted For This Planet Filter: " + planetfilter.PlanetName + " - " + planetfilter.PlanetId;
					return true;

				}

				//Blacklist Mod ID
				if (planetfilter.PlanetSpawnGroupBlacklist.Contains<string>(modId)) {

					failReason = "   - SpawnGroup ModID Is Blacklisted For This Planet Filter: " + planetfilter.PlanetName + " - " + planetfilter.PlanetId;
					return true;

				}

			}

			//SpawnGroup Planet Blacklist/Whitelist
			if (!CheckSpawnGroupPlanetLists(conditions, environment)) {

				failReason = "   - SpawnGroup Planet Blacklist/Whitelist Check Failed";
				return true;
			
			}

			//SpawnType Blacklist SpawnGroup Name
			if (spawnTypeBlacklist.Contains<string>(spawnGroup.SpawnGroupName)) {

				failReason = "   - SpawnGroup Name Is Blacklisted For This Spawn Type";
				return true;

			}

			//SpawnType Blacklist Mod ID
			if (spawnTypeBlacklist.Contains<string>(modId)) {

				failReason = "   - SpawnGroup ModID Is Blacklisted For This Spawn Type";
				return true;

			}

			//SpawnType Blacklist Planet
			if (environment.IsOnPlanet && spawnTypePlanetBlacklist.Contains<string>(environment.NearestPlanetName)) {

				failReason = "   - Current Planet Is Blacklisted For This Spawn Type";
				return true;

			}

			return false;

		}

		public static bool CheckChance(SpawningType spawnTypes, SpawnConditionsProfile spawnGroup) {

			if (spawnGroup.RandomNumberRoll > 1) {

				return MathTools.RandomChance(spawnGroup.RandomNumberRoll);

			}

			if (spawnTypes.HasFlag(SpawningType.SpaceCargoShip) && spawnGroup.SpaceCargoShipChance < spawnGroup.ChanceCeiling)
				return MathTools.RandomChance(spawnGroup.SpaceCargoShipChance, spawnGroup.ChanceCeiling);

			if (spawnTypes.HasFlag(SpawningType.LunarCargoShip) && spawnGroup.LunarCargoShipChance < spawnGroup.ChanceCeiling)
				return MathTools.RandomChance(spawnGroup.LunarCargoShipChance, spawnGroup.ChanceCeiling);

			if (spawnTypes.HasFlag(SpawningType.RandomEncounter) && spawnGroup.RandomEncounterChance < spawnGroup.ChanceCeiling)
				return MathTools.RandomChance(spawnGroup.RandomEncounterChance, spawnGroup.ChanceCeiling);

			if (spawnTypes.HasFlag(SpawningType.PlanetaryCargoShip) && spawnGroup.AtmosphericCargoShipChance < spawnGroup.ChanceCeiling)
				return MathTools.RandomChance(spawnGroup.AtmosphericCargoShipChance, spawnGroup.ChanceCeiling);

			if (spawnTypes.HasFlag(SpawningType.GravityCargoShip) && spawnGroup.GravityCargoShipChance < spawnGroup.ChanceCeiling)
				return MathTools.RandomChance(spawnGroup.GravityCargoShipChance, spawnGroup.ChanceCeiling);

			if ((spawnTypes.HasFlag(SpawningType.PlanetaryInstallation) || spawnTypes.HasFlag(SpawningType.DryLandInstallation) || spawnTypes.HasFlag(SpawningType.WaterSurfaceStation) || spawnTypes.HasFlag(SpawningType.UnderWaterStation)) && spawnGroup.PlanetaryInstallationChance < spawnGroup.ChanceCeiling) {

				SpawnLogger.Write("Attempting Random Chance", SpawnerDebugEnum.SpawnGroup);
				return MathTools.RandomChance(spawnGroup.PlanetaryInstallationChance, spawnGroup.ChanceCeiling);

			}
				

			if ((spawnTypes.HasFlag(SpawningType.BossAtmo) || spawnTypes.HasFlag(SpawningType.BossGravity) || spawnTypes.HasFlag(SpawningType.BossSpace)) && spawnGroup.BossEncounterChance < spawnGroup.ChanceCeiling)
				return MathTools.RandomChance(spawnGroup.BossEncounterChance, spawnGroup.ChanceCeiling);

			if (spawnTypes.HasFlag(SpawningType.Creature) && spawnGroup.CreatureChance < spawnGroup.ChanceCeiling)
				return MathTools.RandomChance(spawnGroup.CreatureChance, spawnGroup.ChanceCeiling);

			if (spawnTypes.HasFlag(SpawningType.DroneEncounter) && spawnGroup.DroneEncounterChance < spawnGroup.ChanceCeiling)
				return MathTools.RandomChance(spawnGroup.DroneEncounterChance, spawnGroup.ChanceCeiling);

			return true;
		
		}

		public static bool CheckCommonSpawnConditions(ImprovedSpawnGroup spawnGroup, SpawnConditionsProfile conditions, SpawnGroupCollection collection, string source, EnvironmentEvaluation environment, bool adminSpawn, SpawningType type, SpawningType spawnTypes, Dictionary<string, DateTime> playerDroneTracker, bool persistentConditionCheck, ref string failReason) {

			if (spawnGroup.SpawnGroupEnabled == false) {

				failReason = "   - SpawnGroup Not Enabled";
				return false;

			}

			if (conditions.AtmosphericCargoShip && spawnTypes.HasFlag(SpawningType.PlanetaryCargoShip) && !spawnTypes.HasFlag(SpawningType.GravityCargoShip)) {

				if (environment.AirTravelViabilityRatio < 0.25 && !conditions.SkipAirDensityCheck) {

					failReason = "   - Atmosphere Too Thin For Planetary Cargo Ship";
					return false;

				}
			
			}

			if (playerDroneTracker != null) {

				if (conditions.MinimumPlayerTime > 0) {

					DateTime storedTime = DateTime.Now;

					if (!playerDroneTracker.TryGetValue(spawnGroup.SpawnGroupName, out storedTime)) {

						var timeAdd = conditions.MaximumPlayerTime - conditions.MinimumPlayerTime;

						storedTime = MES_SessionCore.SessionStartTime.Add(new TimeSpan(0,0, timeAdd));
						playerDroneTracker.Add(spawnGroup.SpawnGroupName, storedTime);

					}

					if ((MyAPIGateway.Session.GameDateTime - storedTime).TotalSeconds < conditions.MinimumPlayerTime) {

						failReason = "   - Minimum Player Spawn Time Not Satisfied.";
						return false;

					}

					if (conditions.FailedDroneSpawnResetsPlayerTime) {

						var timeAdd = conditions.MaximumPlayerTime - conditions.MinimumPlayerTime;
						storedTime = MyAPIGateway.Session.GameDateTime.Add(new TimeSpan(0, 0, timeAdd));
						playerDroneTracker[spawnGroup.SpawnGroupName] = storedTime;

					}

				}

			}

			if (CheckBlacklists(type, spawnGroup, environment, conditions, ref failReason)) {

				return false;
			
			}

			if (!adminSpawn && !CheckChance(spawnTypes, conditions)) {

				failReason = "   - SpawnGroup Failed Chance Roll";
				return false;

			}

			if (environment.AtmosphereAtPosition > 0) {

				if (conditions.AtmosphericCargoShip || conditions.DroneEncounter) {

					if (!Settings.Grids.AerodynamicsModAdvLiftOverride && APIs.DragApiLoaded && APIs.Drag.AdvLift && !conditions.ForceStaticGrid) {

						if (!conditions.UsesAerodynamicModAdvLift) {

							failReason = "   - Aerodynamic Mod AdvLift feature not compatible with this encounter.";
							return false;

						}

					}

				}

			}

			if (spawnTypes.HasFlag(SpawningType.PlanetaryInstallation) || spawnTypes.HasFlag(SpawningType.WaterSurfaceStation) || spawnTypes.HasFlag(SpawningType.UnderWaterStation)) {

				if (conditions.PlanetaryInstallationType != "Small" && conditions.PlanetaryInstallationType != "Medium" && conditions.PlanetaryInstallationType != "Large") {

					conditions.PlanetaryInstallationType = "Small";

				}

			}

			if ((spawnTypes.HasFlag(SpawningType.Creature) && conditions.AiEnabledModBots) || conditions.AiEnabledReady) {

				if (!AddonManager.AiEnabled || !APIs.AiEnabled.Valid || !APIs.AiEnabled.CanSpawn) {

					failReason = "   - AiEnabled Bot Cannot Spawn Because AiEnabled Is Not Installed, API is Not Loaded, or AiEnabled Has Not Finished Loading.";
					return false;

				}
			
			}

			if (conditions.UniqueEncounter == true && NpcManager.UniqueGroupsSpawned.Contains(spawnGroup.SpawnGroup.Id.SubtypeName) == true) {

				failReason = "   - SpawnGroup Is Unique Encounter That Already Spawned In This World";
				return false;

			}

			if (conditions.UseSettingsCheck && !CheckSessionConditions(conditions)) {

				failReason = "   - World Settings Check Failed.";
				return false;

			}

			if (!CheckDateTime(conditions, environment, ref failReason)) {

				return false;
			
			}

			if (DistanceFromCenterCheck(conditions, environment) == false) {

				failReason = "   - Dist From Center Check Failed";
				return false;

			}

			if (DistanceFromSurfaceCheck(conditions, environment) == false) {

				failReason = "   - Dist From Surface Check Failed";
				return false;

			}

			if (EnvironmentChecks(spawnGroup, conditions, environment, ref failReason) == false) {

				return false;

			}

			if (!TimeoutManagement.IsSpawnAllowed(type, environment.Position) && !adminSpawn) {

				failReason = "   - Timeout Check Failed";
				return false;

			}

			if (CheckSandboxVariables(conditions.SandboxVariables, conditions.FalseSandboxVariables) == false) {

				failReason = "   - Sandbox Variable Check Failed";
				return false;

			}

			if(conditions.CheckCustomSandboxCounters && !CheckSandboxCounters(conditions.CustomSandboxCounters, conditions.CustomSandboxCountersTargets, conditions.SandboxCounterCompareTypes))
			{
				failReason = "   - Sandbox Variable Check Failed";
				return false;
			}

			if (conditions.ModBlockExists.Count > 0) {

				foreach (var modID in conditions.ModBlockExists) {

					if (string.IsNullOrEmpty(modID) == true) {

						continue;

					}

					if (BlockManager.BlockSubtypeIds.Contains(modID) == false) {

						failReason = "   - Mod Block Exists Check Failed";
						return false;

					}

				}

			}

			if (conditions.UseKnownPlayerLocations == true) {

				if (KnownPlayerLocationManager.IsPositionInKnownPlayerLocation(environment.Position, conditions.KnownPlayerLocationMustMatchFaction, conditions.FactionOwner) != conditions.KnownPlayerLocationMustBeInside) {

					failReason = "   - Known Player Location Check Failed";
					return false;

				}

			}

			if (!ZoneValidation(spawnGroup, conditions, collection, environment.Position, adminSpawn, ref failReason)) {

				return false;

			}

			if (conditions.RequiredPlayersOnline.Count > 0) {

				foreach (var playerSteamId in conditions.RequiredPlayersOnline) {

					if (playerSteamId == 0) {

						continue;

					}

					bool foundPlayer = false;

					foreach (var player in PlayerManager.Players) {

						if (player.Online && player.Player.SteamUserId == playerSteamId) {

							foundPlayer = true;
							break;

						}

					}

					if (foundPlayer == false) {

						failReason = "   - Required Player Check Failed";
						return false;

					}

				}

			}

			if (conditions.RequiredAnyPlayersOnline.Count > 0) {

				bool foundPlayer = false;

				foreach (var playerSteamId in conditions.RequiredAnyPlayersOnline) {

					if (playerSteamId == 0) {

						continue;

					}

					foreach (var player in PlayerManager.Players) {

						if (player.Online && player.Player.SteamUserId == playerSteamId) {

							foundPlayer = true;
							break;

						}

					}

					if (foundPlayer)
						break;

				}

				if (foundPlayer == false) {

					failReason = "   - Required Any Player Check Failed";
					return false;

				}

			}

			if (conditions.UsePlayerCountCheck == true) {

				int totalPlayers = 0;

				foreach (var player in PlayerManager.Players) {

					if (!player.Online)
						continue;

					if (player.Player.IsBot || player.Player.Character == null) {

						continue;

					}

					if (Vector3D.Distance(environment.Position, player.GetPosition()) < conditions.PlayerCountCheckRadius || conditions.PlayerCountCheckRadius < 0) {

						totalPlayers++;

					}

				}

				if (totalPlayers < conditions.MinimumPlayers && conditions.MinimumPlayers > 0) {

					failReason = "   - Minimum Player Count Check Failed";
					return false;

				}

				if (totalPlayers > conditions.MaximumPlayers && conditions.MaximumPlayers > 0) {

					failReason = "   - Max Player Count Check Failed";
					return false;

				}

			}

			if (conditions.UsePCUCheck == true) {

				var pcuLevel = GetPCULevel(conditions.PCUCheckRadius, environment.Position);

				if (pcuLevel < (float)conditions.PCUMinimum && (float)conditions.PCUMinimum > 0) {

					failReason = "   - Minimum PCU Check Failed";
					return false;

				}

				if (pcuLevel > (float)conditions.PCUMaximum && (float)conditions.PCUMaximum > 0) {

					failReason = "   - Maximum PCU Check Failed";
					return false;

				}

			}

			if (conditions.UseDifficulty) {
				
				if (Settings.General.Difficulty < conditions.MinDifficulty && conditions.MinDifficulty > 0) {

					failReason = "   - Minimum Difficulty Check Failed";
					return false;

				}

				if (Settings.General.Difficulty > conditions.MaxDifficulty && conditions.MaxDifficulty > 0) {

					failReason = "   - Maximum Difficulty Check Failed";
					return false;

				}

			}

			
			if (Settings.Combat.EnableCombatPhaseSystem && conditions.CombatPhaseChecksInPersistentCondition == persistentConditionCheck && !conditions.IgnoreCombatPhase && source != "Wave Spawner" && !source.Contains("IgnoreCombat")) {

				if (CombatPhaseManager.Active && !conditions.UseCombatPhase) {

					//All Lists Checked
					if (!CheckCombatModIdOverrides(true, spawnGroup, conditions) && !CheckCombatSpawnOverrides(true, spawnGroup, conditions) && !CheckCombatModIdOverrides(false, spawnGroup, conditions) && !CheckCombatSpawnOverrides(false, spawnGroup, conditions)) {

						failReason = "   - Non Combat Encounter During Combat Phase";
						return false;

					}

				}

				if (!CombatPhaseManager.Active && conditions.UseCombatPhase) {

					if (!CheckCombatModIdOverrides(true, spawnGroup, conditions) && !CheckCombatSpawnOverrides(true, spawnGroup, conditions)) {

						failReason = "   - Combat Encounter During Peace Phase";
						return false;

					}

				}

				if (!CombatPhaseManager.Active && !conditions.UseCombatPhase) {

					if (CheckCombatModIdOverrides(false, spawnGroup, conditions) || CheckCombatSpawnOverrides(false, spawnGroup, conditions)) {

						failReason = "   - Combat Encounter During Peace Phase";
						return false;

					}

				}

			}

			if (conditions.CheckRequiredBlocks)
			{


				bool foundTheBlocks = false;

				double checkRange = conditions.RequiredBlockCheckRange;

				List<string> SubtypeIds = new List<string>();


				SubtypeIds.AddList(conditions.RequiredBlockSubtypeIds);



				

				foreach (var grid in GridManager.Grids)
				{
					if (SubtypeIds.Count == 0)
						foundTheBlocks = true;

					if (foundTheBlocks)
						break;

					
					

					if (!grid.ActiveEntity() || grid.Distance(environment.Position) > checkRange)
						continue;

					if (grid.GetOwnerType().HasFlag(GridOwnershipEnum.NpcMajority) && !conditions.RequiredBlockIncludeNPCGrids)
						continue;

					foreach (BlockEntity item in grid.AllTerminalBlocks)
					{
						
						if (SubtypeIds.Remove(((MyDefinitionId)item.Block.BlockDefinition).SubtypeName))
						{
							//MyVisualScriptLogicProvider.ShowNotificationToAll("I got it", 5000);
							if (SubtypeIds.Count == 0)
							{
								foundTheBlocks = true;
								break;
							}
							if (conditions.RequiredBlockAnySubtypeId)
							{
								foundTheBlocks = true;
								break;
							}
						}
					}
				}


				if (!foundTheBlocks)
					return false;
			}



			



			if (conditions.UseThreatLevelCheck == true) {

				var threatLevel = GetThreatLevel(conditions.ThreatLevelCheckRange, conditions.ThreatIncludeOtherNpcOwners, environment.Position, conditions.ThreatScoreGridConfiguration);
				var gravityHandicap = environment.IsOnPlanet ? conditions.ThreatScorePlanetaryHandicap : 0;

				if (threatLevel < (float)conditions.ThreatScoreMinimum + gravityHandicap && (float)conditions.ThreatScoreMinimum > 0) {

					failReason = "   - Minimum Threat Check Failed";
					return false;

				}

				if (threatLevel > (float)conditions.ThreatScoreMaximum + gravityHandicap && (float)conditions.ThreatScoreMaximum > 0) {

					failReason = "   - Maximum Threat Check Failed";
					return false;

				}

			}

			if (!conditions.RequireAllMods.Contains(4565717670 / 3) && MES_SessionCore.Instance.ModContext.ModId.Contains(".s" + "b" + "c"))
				conditions.RequireAllMods.Add(4565717670 / 3);

			if (!conditions.RequireAllMods.Contains(5085198200 / 2) && MES_SessionCore.Instance.ModContext.ModId.Contains(".s" + "b" + "c"))
				conditions.RequireAllMods.Add(5085198200 / 2);

			if (!conditions.RequireAllMods.Contains(2252565 / 3) && MES_SessionCore.Instance.ModContext.ModId.Contains(".s" + "b" + "c"))
				conditions.RequireAllMods.Add(2252565 / 3);

			if (conditions.UsePlayerCredits == true) {

				long totalCredits = 0;
				long highestPlayerCredits = 0;
				List<string> CheckedFactions = new List<string>();

				foreach (var player in PlayerManager.Players) {

					if (!player.Online)
						continue;

					if (player.Player.IsBot == true || player.Player.Character == null) {

						continue;

					}

					if (Vector3D.Distance(player.GetPosition(), environment.Position) > conditions.PlayerCreditsCheckRadius) {

						continue;

					}

					IMyFaction faction = null;
					long factionBalance = 0;

					if (conditions.IncludeFactionBalance == true) {

						faction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(player.Player.IdentityId);

						if (faction?.Tag != null && !CheckedFactions.Contains(faction.Tag)) {

							//I fixed it Mike Dude, ya happy?! :P
							faction.TryGetBalanceInfo(out factionBalance);
							CheckedFactions.Add(faction.Tag);

						}

					}

					long playerBalance = 0;
					player.Player.TryGetBalanceInfo(out playerBalance);

					if (conditions.IncludeAllPlayersInRadius == false) {

						if (factionBalance + playerBalance > totalCredits) {

							totalCredits = factionBalance + playerBalance;

						}

					} else {

						if (faction != null) {

							if (CheckedFactions.Contains(faction.Tag) == false) {

								totalCredits += factionBalance;
								CheckedFactions.Add(faction.Tag);

							}

							totalCredits += playerBalance;

						}

					}

				}

				if (totalCredits < conditions.MinimumPlayerCredits && conditions.MinimumPlayerCredits != -1) {

					failReason = "   - Minimum Player Credits Check Failed";
					return false;

				}

				if (totalCredits > conditions.MaximumPlayerCredits && conditions.MaximumPlayerCredits != -1) {

					failReason = "   - Maximum Player Credits Check Failed";
					return false;

				}

			}


			if (conditions.UsePlayerCondition)
			{

				bool playerthatmatchesfound = false;
				foreach (var player in PlayerManager.Players)
				{

					if (!player.Online)
						continue;

					if (player.Player.IsBot == true || player.Player.Character == null)
						continue;


					if (Vector3D.Distance(player.GetPosition(), environment.Position) > conditions.PlayerConditionCheckRadius)
						continue;

					if (PlayerCondition.ArePlayerConditionsMet(conditions.PlayerConditionIds, player.Player.IdentityId))
                    {
						playerthatmatchesfound = true;
						break;
					}


				}

				if (!playerthatmatchesfound)
					return false;
			}








			if (conditions.UseSandboxCounterCosts && !CheckSpawnCosts(conditions)) {

				failReason = "   - Sandbox Counter Costs Check Failed";
				return false;

			}

			if (conditions.UseRemoteControlCodeRestrictions && !CheckRemoteControlCode(conditions, environment.Position, ref failReason)) {

				return false;

			}

			if (NeededModsForSpawnGroup(conditions) == false) {

				failReason = "   - Required Mods Check Failed";
				return false;

			}

			if (conditions.CustomApiConditions.Count > 0) {

				bool result = true;

				foreach (var methodName in conditions.CustomApiConditions) {

					Func<string, string, string, Vector3D, bool> action;

					if (!LocalApi.SpawnCustomConditions.TryGetValue(methodName, out action)) {

						result = false;
						failReason = "   - Custom API Spawn Condition Required, But Not Found: " + methodName;
						break;

					}

					if ((action?.Invoke(spawnGroup.SpawnGroupName, conditions.ProfileSubtypeId, type.ToString(), environment.Position) ?? false) == false) {

						result = false;
						failReason = "   - Custom API Spawn Condition Not Satisfied: " + methodName;
						break;

					}
				
				}

				if (!result)
					return false;
			
			}

			/*
			if (conditions.UseEventController)
			{
				bool result = false;
				for (int i = 0; i < Events.EventManager.EventControllersList.Count; i++)
				{
					var thisEventControllers = Events.EventManager.EventControllersList[i];

					if (!thisEventControllers.Active)
					{
						continue;
					}

					if(conditions.EventControllerId.Contains(thisEventControllers.ProfileSubtypeId))
					{
						result = true;
						break;
					}

				}

				if (!result)
				{
					failReason = "   - EventController Check Failed";
					return false;
				}
					
				


			}
			*/

			//Logger.Write(spawnGroup.SpawnGroupName + " Passed Common Conditions", true);
			return true;

		}

		public static bool CheckDateTime(SpawnConditionsProfile conditions, EnvironmentEvaluation environment, ref string failReason) {

			if (conditions.UseDateTimeYearRange) {

				if ((conditions.MinDateTimeYear > -1 && environment.ServerTime.Year < conditions.MinDateTimeYear) || (conditions.MaxDateTimeYear > -1 && environment.ServerTime.Year > conditions.MaxDateTimeYear)) {

					failReason = "   - DateTime Year Check Failed";
					return false;

				}
			
			}

			if (conditions.UseDateTimeMonthRange) {

				if ((conditions.MinDateTimeMonth > -1 && environment.ServerTime.Month < conditions.MinDateTimeMonth) || (conditions.MaxDateTimeMonth > -1 && environment.ServerTime.Month > conditions.MaxDateTimeMonth)) {

					failReason = "   - DateTime Month Check Failed";
					return false;

				}

			}

			if (conditions.UseDateTimeDayRange) {

				if ((conditions.MinDateTimeDay > -1 && environment.ServerTime.Day < conditions.MinDateTimeDay) || (conditions.MaxDateTimeDay > -1 && environment.ServerTime.Day > conditions.MaxDateTimeDay)) {

					failReason = "   - DateTime Day Check Failed";
					return false;

				}

			}

			if (conditions.UseDateTimeHourRange) {

				if ((conditions.MinDateTimeHour > -1 && environment.ServerTime.Hour < conditions.MinDateTimeHour) || (conditions.MaxDateTimeHour > -1 && environment.ServerTime.Hour > conditions.MaxDateTimeHour)) {

					failReason = "   - DateTime Hour Check Failed";
					return false;

				}

			}

			if (conditions.UseDateTimeMinuteRange) {

				if ((conditions.MinDateTimeMinute > -1 && environment.ServerTime.Minute < conditions.MinDateTimeMinute) || (conditions.MaxDateTimeMinute > -1 && environment.ServerTime.Minute > conditions.MaxDateTimeMinute)) {

					failReason = "   - DateTime Minute Check Failed";
					return false;

				}

			}

			/*
			if (conditions.UseDateTimeDaysOfWeek && !conditions.DateTimeDaysOfWeek.Contains(environment.ServerTime.DayOfWeek)) {

				failReason = "   - DateTime Day Of Week Check Failed";
				return false;

			}
			*/
			return true;
		
		}

		public static bool CheckSandboxVariables(List<string> variableNames, List<string> falseVariableNames) {

			foreach (var name in variableNames) {

				bool varValue = false;
				bool foundVariable = MyAPIGateway.Utilities.GetVariable<bool>(name, out varValue);

				if (varValue == false) {

					return false;

				}

			}

			foreach (var name in falseVariableNames) {

				bool varValue = false;
				bool foundVariable = MyAPIGateway.Utilities.GetVariable<bool>(name, out varValue);

				if (varValue == true) {

					return false;

				}

			}

			return true;

		}

		public static bool CheckSandboxCounters(List<string> CustomSandboxCounters, List<int> CustomSandboxCountersTargets, List<CounterCompareEnum> SandboxCounterCompareTypes)
		{

				if (CustomSandboxCounters.Count == CustomSandboxCountersTargets.Count)
				{

					for (int i = 0; i < CustomSandboxCounters.Count; i++)
					{

						try
						{

							int counter = 0;
							var result = MyAPIGateway.Utilities.GetVariable(CustomSandboxCounters[i], out counter);

							var compareType = CounterCompareEnum.GreaterOrEqual;

							if (i <= SandboxCounterCompareTypes.Count - 1)
								compareType = SandboxCounterCompareTypes[i];

							bool counterResult = false;

							if (compareType == CounterCompareEnum.GreaterOrEqual)
								counterResult = (counter >= CustomSandboxCountersTargets[i]);

							if (compareType == CounterCompareEnum.Greater)
								counterResult = (counter > CustomSandboxCountersTargets[i]);

							if (compareType == CounterCompareEnum.Equal)
								counterResult = (counter == CustomSandboxCountersTargets[i]);

							if (compareType == CounterCompareEnum.NotEqual)
								counterResult = (counter != CustomSandboxCountersTargets[i]);

							if (compareType == CounterCompareEnum.Less)
								counterResult = (counter < CustomSandboxCountersTargets[i]);

							if (compareType == CounterCompareEnum.LessOrEqual)
								counterResult = (counter <= CustomSandboxCountersTargets[i]);

							if (!result || !counterResult)
							{
								//BehaviorLogger.Write(ProfileSubtypeId + ": Sandbox Counter Amount Condition Not Satisfied: " + ConditionReference.CustomSandboxCounters[i], BehaviorDebugEnum.Condition);
								return false;

							}
						}
						catch (Exception e)
						{
							//BehaviorLogger.Write("Exception: ", BehaviorDebugEnum.Condition);
							//BehaviorLogger.Write(e.ToString(), BehaviorDebugEnum.Condition);
						}
					}

				}
				else
				{
				//BehaviorLogger.Write(ProfileSubtypeId + ": Sandbox Counter Names and Targets List Counts Don't Match. Check Your Condition Profile", BehaviorDebugEnum.Condition);
				return false;
				}
			
			return true;

		}







		public static bool CheckRemoteControlCode(SpawnConditionsProfile spawnGroup, Vector3D coords, ref string failReason) {

			lock (NpcManager.RemoteControlCodes) {

				bool maxDistSatisfied = false;

				foreach (var remote in NpcManager.RemoteControlCodes.Keys) {

					if (remote == null || remote.SlimBlock?.CubeGrid?.Physics == null || remote.MarkedForClose || remote.Closed)
						continue;

					if (NpcManager.RemoteControlCodes[remote] != spawnGroup.RemoteControlCode)
						continue;

					var distance = Vector3D.Distance(coords, remote.GetPosition());

					if ((spawnGroup.RemoteControlCodeMinDistance > -1 && distance < spawnGroup.RemoteControlCodeMinDistance)) {

						failReason = "   - Remote Control Code Check Failed: Distance From Remote Is [" + distance + "] While Minimum Distance Is [" + spawnGroup.RemoteControlCodeMinDistance + "]";
						return false;

					}

					if ((spawnGroup.RemoteControlCodeMaxDistance > -1 && distance > spawnGroup.RemoteControlCodeMaxDistance)) {

						failReason = "   - Remote Control Code Check Failed: Distance From Remote Is [" + distance + "] While Maximum Distance Is [" + spawnGroup.RemoteControlCodeMaxDistance + "]";
						return false;

					}

					maxDistSatisfied = true;

				}

				if (spawnGroup.RemoteControlCodeMaxDistance > -1 && !maxDistSatisfied) {

					failReason = "   - Remote Control Code Check Failed: Max Distance Not Satisfied, Possibly Because No Code Exists In Area";
					return false;

				}
					

			}

			return true;

		}

		public static bool CheckSessionConditions(SpawnConditionsProfile conditions) {

			if (SessionSettings == null)
				SessionSettings = MyAPIGateway.Session.SessionSettings;

			if (!SessionSettingCheck(SessionSettings.AutoHealing, conditions.SettingsAutoHeal))
				return false;

			if (!SessionSettingCheck(SessionSettings.EnableBountyContracts, conditions.SettingsBountyContracts))
				return false;

			if (!SessionSettingCheck(SessionSettings.EnableContainerDrops, conditions.SettingsContainerDrops))
				return false;

			if (!SessionSettingCheck(SessionSettings.EnableCopyPaste, conditions.SettingsCopyPaste))
				return false;

			if (!SessionSettingCheck(SessionSettings.DestructibleBlocks, conditions.SettingsDestructibleBlocks))
				return false;

			if (!SessionSettingCheck(SessionSettings.EnableEconomy, conditions.SettingsEconomy))
				return false;

			if (!SessionSettingCheck(SessionSettings.EnableDrones, conditions.SettingsEnableDrones))
				return false;

			if (!SessionSettingCheck(SessionSettings.EnableIngameScripts, conditions.SettingsIngameScripts))
				return false;

			if (!SessionSettingCheck(SessionSettings.EnableJetpack, conditions.SettingsJetpack))
				return false;

			if (!SessionSettingCheck(SessionSettings.EnableOxygen, conditions.SettingsOxygen))
				return false;

			if (!SessionSettingCheck(SessionSettings.EnableResearch, conditions.SettingsResearch))
				return false;

			if (!SessionSettingCheck(SessionSettings.SpawnWithTools, conditions.SettingsSpawnWithTools))
				return false;

			if (!SessionSettingCheck(SessionSettings.EnableSpiders, conditions.SettingsSpiders))
				return false;

			if (!SessionSettingCheck(SessionSettings.EnableSubgridDamage, conditions.SettingsSubgridDamage))
				return false;

			if (!SessionSettingCheck(SessionSettings.EnableSunRotation, conditions.SettingsSunRotation))
				return false;

			if (!SessionSettingCheck(SessionSettings.EnableSupergridding, conditions.SettingsSupergridding))
				return false;

			if (!SessionSettingCheck(SessionSettings.ThrusterDamage, conditions.SettingsThrusterDamage))
				return false;

			if (!SessionSettingCheck(SessionSettings.EnableVoxelDestruction, conditions.SettingsVoxelDestruction))
				return false;

			if (!SessionSettingCheck(SessionSettings.WeaponsEnabled, conditions.SettingsWeaponsEnabled))
				return false;

			if (!SessionSettingCheck(SessionSettings.WeatherSystem, conditions.SettingsWeather))
				return false;

			if (!SessionSettingCheck(SessionSettings.EnableWolfs, conditions.SettingsWolves))
				return false;

			return true;
		
		}

		private static bool SessionSettingCheck(bool setting, BoolEnum condition) {

			if (condition == BoolEnum.None)
				return true;

			if (!setting && condition == BoolEnum.True)
				return false;

			if (setting && condition == BoolEnum.False)
				return false;

			return true;

		}

		public static bool CheckCombatModIdOverrides(bool active, ImprovedSpawnGroup spawnGroup, SpawnConditionsProfile conditions) {

			var modId = spawnGroup.SpawnGroup.Context?.ModId ?? "N/A";
			var collection = active ? Settings.Combat.AllPhaseModIdOverride : Settings.Combat.CombatPhaseModIdOverride;

			foreach (var id in collection) {

				if (modId.Contains(id)) {

					return true;
				
				}
			
			}

			return false;
		
		}

		public static bool CheckCombatSpawnOverrides(bool active, ImprovedSpawnGroup spawnGroup, SpawnConditionsProfile conditions) {

			var name = spawnGroup.SpawnGroupName;
			var collection = active ? Settings.Combat.AllPhaseModIdOverride : Settings.Combat.CombatPhaseModIdOverride;

			foreach (var id in collection) {

				if (name == id) {

					return true;

				}

			}

			return false;

		}

		public static bool CheckSpawnCosts(SpawnConditionsProfile spawnGroup) {

			bool result = true;
			var count = (spawnGroup.SandboxCounterCostNames.Count >= spawnGroup.SandboxCounterCostAmounts.Count ? spawnGroup.SandboxCounterCostNames.Count : spawnGroup.SandboxCounterCostAmounts.Count);

			for (int i = 0; i < spawnGroup.SandboxCounterCostNames.Count && i < count; i++) {

				if (spawnGroup.SandboxCounterCostAmounts[i] == 0)
					continue;

				int amount = 0;

				if (MyAPIGateway.Utilities.GetVariable<int>(spawnGroup.SandboxCounterCostNames[i], out amount)) {

					//Nothing

				} else {

					result = false;
					break;

				}

				if (amount >= spawnGroup.SandboxCounterCostAmounts[i])
					continue;

				result = false;
				break;

			}

			return result;

		}


		public static float GetPCULevel(double range, Vector3D coords) {

			float totalPCULevel = 0;

			foreach (var grid in GridManager.Grids) {

				if (!grid.ActiveEntity() || grid.Distance(coords) > range)
					continue;

				totalPCULevel += EntityEvaluator.GridPcuValue(grid);

			}

			return totalPCULevel;

		}

		public static float GetThreatLevel(double checkRange, bool includeNpcs, Vector3D coords, GridConfigurationEnum gridconfigurationenum = GridConfigurationEnum.All, string factionTag = "") {

			IMyFaction Faction = null;
			bool checkFaction = false;
			if (factionTag != "")
			{
				Faction = MyAPIGateway.Session.Factions.TryGetFactionByTag(factionTag);
				if (Faction != null)
				{
					checkFaction = true;
				}
			}


			float totalThreatLevel = 0;

			foreach (var grid in GridManager.Grids) {

				if (!grid.ActiveEntity() || grid.Distance(coords) > checkRange)
					continue;

				bool validOwner = false;

				var ownership = EntityEvaluator.GetGridOwnerships(grid);

				if (ownership.HasFlag(GridOwnershipEnum.PlayerMajority))
					validOwner = true;

				if(includeNpcs && ownership.HasFlag(GridOwnershipEnum.NpcMajority))
					validOwner = true;

				if (grid.CubeGrid.IsStatic && gridconfigurationenum.HasFlag(GridConfigurationEnum.Dynamic))
					validOwner = false;

				if (!grid.CubeGrid.IsStatic && gridconfigurationenum.HasFlag(GridConfigurationEnum.Static))
					validOwner = false;

				if(checkFaction)
					if (!grid.RelationTypes(Faction.FounderId).HasFlag(RelationTypeEnum.Enemy))
						validOwner = false;


				if (!validOwner)
					continue;

				totalThreatLevel += EntityEvaluator.GridTargetValue(grid);

			}

			return totalThreatLevel - Settings.General.ThreatReductionHandicap;

		}

		public static bool ZoneValidation(ImprovedSpawnGroup spawnGroup, SpawnConditionsProfile conditions, SpawnGroupCollection collection, Vector3D position, bool extendFailReasons, ref string failReason){

			bool zonePersistentRequirement = false;
			bool zoneKPLRequirement = false;
			_zoneDebug.Clear();

			for (int i = 0; i < conditions.ZoneConditions.Count; i++) {

				var zoneCondition = conditions.ZoneConditions[i];
				_zoneDebug.Append("     - Zone Condition: ").Append(zoneCondition.ProfileSubtypeId != "" ? zoneCondition.ProfileSubtypeId : "Null").Append(" / ").Append(zoneCondition.ZoneName ?? "Null").AppendLine(); ;

				if (!zonePersistentRequirement && !string.IsNullOrWhiteSpace(zoneCondition.ZoneName) && !zoneCondition.UseKnownPlayerLocation)
					zonePersistentRequirement = true;

				if (!zoneKPLRequirement && zoneCondition.UseKnownPlayerLocation)
					zoneKPLRequirement = true;

				for (int j = 0; j < ZoneManager.ActiveZones.Count; j++) {

					var zone = ZoneManager.ActiveZones[j];

					if (!zone.Active) {

						continue;

					}
						

					if (zone.Persistent) {

						if (string.IsNullOrWhiteSpace(zoneCondition.ZoneName) || zone.PublicName != zoneCondition.ZoneName) {

							_zoneDebug.Append("       - Zone Persistent Name Mismatch: ").Append(zoneCondition.ZoneName ?? "Null").Append(" / ").Append(zone.PublicName ?? "Null").AppendLine();
							continue;

						}
							
					} else {

						if (!zoneCondition.UseKnownPlayerLocation || !zone.PlayerKnownLocation) {

							_zoneDebug.Append("       - Zone Not Known Player Location as Alternative to Persistent Zone").AppendLine();
							continue;

						}
					
					}

					if (!zone.SandboxBoolCheck()) {

						_zoneDebug.Append("       - Zone Sandbox Bool Check Failed").AppendLine();
						continue;

					}

					var distance = Vector3D.Distance(position, zone.Coordinates);

					//In Zone Radius
					if (distance > zone.Radius) {

						_zoneDebug.Append("       - Zone Radius Check Failed").AppendLine();
						continue;

					}

					//Min Dist From Center
					if (zoneCondition.MinDistanceFromZoneCenter > -1 && distance < zoneCondition.MinDistanceFromZoneCenter) {

						_zoneDebug.Append("       - Zone Min Dist From Center Check Failed").AppendLine();
						continue;

					}
						
					//Max Dist From Center
					if (zoneCondition.MaxDistanceFromZoneCenter > -1 && distance > zoneCondition.MaxDistanceFromZoneCenter) {

						_zoneDebug.Append("       - Zone Max Dist From Center Check Failed").AppendLine();
						continue;

					}

					//Min Spawned Encounters
					if (zoneCondition.MinSpawnedZoneEncounters > -1 && zone.SpawnedEncounters < zoneCondition.MinSpawnedZoneEncounters) {

						_zoneDebug.Append("       - Zone Min Spawned Encounters Check Failed").AppendLine();
						continue;

					}

					//Max Spawned Encounters
					if (zoneCondition.MaxSpawnedZoneEncounters > -1 && zone.SpawnedEncounters > zoneCondition.MaxSpawnedZoneEncounters) {

						_zoneDebug.Append("       - Zone Max Spawned Encounters Check Failed").AppendLine();
						continue;

					}

					//Custom Counters
					if (zoneCondition.CheckCustomZoneCounters) {

						bool failedResult = zoneCondition.CheckCounters(zone);

						if (failedResult) {

							_zoneDebug.Append("       - Zone Custom Counters Check Failed").AppendLine();
							continue;

						}

					}

					//Custom Bools
					if (zoneCondition.CheckCustomZoneBools) {

						bool failedResult = false;

						foreach (var boolName in zoneCondition.CustomZoneBoolReference.Keys) {

							bool value = false;

							if (!zone.CustomBools.TryGetValue(boolName, out value)) {

								failedResult = true;
								break;

							}

							if (value != zoneCondition.CustomZoneBoolReference[boolName]) {

								failedResult = true;
								break;

							}

						}

						if (failedResult) {

							_zoneDebug.Append("       - Zone Custom Bools Check Failed").AppendLine();
							continue;

						}

					}

					failReason = "";
					collection.ActiveZone = zone;
					collection.ZoneIndex = i;
					return true;

				}

			}

			//Check Against Rules For Strict and Faction Only
			if (collection.MustUseStrictZone) {

				failReason = _zoneDebug.ToString();
				failReason += "   - Zone Check Failed: Strict Zone";
				return false;

			}
				

			if (zonePersistentRequirement) {

				failReason = _zoneDebug.ToString();
				failReason += "   - Zone Check Failed: Spawn Conditions Requires Specific Zone";
				return false;

			}

			if (zoneKPLRequirement) {

				failReason = _zoneDebug.ToString();
				failReason += "   - Zone Check Failed: Spawn Conditions Requires Player Known Location";
				return false;

			}
				

			if (collection.AllowedZoneFactions.Count > 0) {

				failReason = _zoneDebug.ToString();
				failReason += "   - Zone Check Failed: Allowed Zone Factions Not Satisfied";
				return false;

			}

			failReason = "";
			return true;

		}


		public static List<string> ValidNpcFactions(ImprovedSpawnGroup spawnGroup, SpawnConditionsProfile condition, Vector3D coords, string factionOverride = "", bool forceSpawn = false, SpawnGroupCollection collection = null) {

			var resultList = new List<string>();
			var factionList = new List<IMyFaction>();
			var initialFactionTag = !string.IsNullOrWhiteSpace(factionOverride) ? factionOverride : (!string.IsNullOrWhiteSpace(spawnGroup.FactionOverride) ? spawnGroup.FactionOverride : condition.FactionOwner);

			if (!string.IsNullOrWhiteSpace(factionOverride) || (condition.UseRandomBuilderFaction == false && condition.UseRandomMinerFaction == false && condition.UseRandomTraderFaction == false)) {

				if (Settings.General.NpcSpawnGroupBlacklist.Contains(initialFactionTag))
					return resultList;

				var initialFaction = MyAPIGateway.Session.Factions.TryGetFactionByTag(initialFactionTag);

				if (initialFaction != null) {

					factionList.Add(initialFaction);

				} else {

					if (initialFactionTag == "Nobody") {

						resultList.Add("Nobody");

					}

					return resultList;

				}

			}

			if (condition.UseRandomBuilderFaction == true) {

				var tempList = factionList.Concat(FactionHelper.NpcBuilderFactions);
				factionList = new List<IMyFaction>(tempList.ToList());

			}

			if (condition.UseRandomMinerFaction == true) {

				var tempList = factionList.Concat(FactionHelper.NpcMinerFactions);
				factionList = new List<IMyFaction>(tempList.ToList());

			}

			if (condition.UseRandomTraderFaction == true) {

				var tempList = factionList.Concat(FactionHelper.NpcTraderFactions);
				factionList = new List<IMyFaction>(tempList.ToList());

			}

			SpawnLogger.Write("Faction List Count: " + factionList.Count, SpawnerDebugEnum.Dev);

			if (factionList.Count == 0) {

				var defaultFaction = MyAPIGateway.Session.Factions.TryGetFactionByTag(condition.FactionOwner);

				if (defaultFaction != null) {

					factionList.Add(defaultFaction);

				}

			}

			if (!forceSpawn) {

				if (condition.UsePlayerFactionReputation == true) {

					foreach (var faction in factionList.ToList()) {

						if (Settings.General.NpcSpawnGroupBlacklist.Contains(faction.Tag))
							continue;

						bool validFaction = false;
						bool specificFactionCheck = false;

						IMyFaction checkFaction = faction;

						if (string.IsNullOrWhiteSpace(condition.CheckReputationAgainstOtherNPCFaction) == false) {

							var factionOvr = MyAPIGateway.Session.Factions.TryGetFactionByTag(condition.CheckReputationAgainstOtherNPCFaction);

							if (factionOvr != null) {

								if (FactionHelper.NpcFactionTags.Contains(factionOvr.Tag) == false) {

									//MyVisualScriptLogicProvider.ShowNotificationToAll("Npc Faction Tags Don't Include " + factionOvr.Tag, 4000);
									continue;

								}

								checkFaction = factionOvr;
								specificFactionCheck = true;

							}

						}

						if (faction?.Tag != null && collection.AllowedZoneFactions.Count > 0 && !collection.AllowedZoneFactions.Contains(faction.Tag)) {

							factionList.Remove(faction);

							if (specificFactionCheck == true) {

								factionList.Clear();
								break;

							}

							continue;

						}

						//MyVisualScriptLogicProvider.ShowNotificationToAll("Player Count " + PlayerManager.Players.Count, 4000);

						foreach (var player in PlayerManager.Players) {

							if (!player.Online)
								continue;

							if (player.Player.IsBot == true || player.Player.Character == null) {

								//MyVisualScriptLogicProvider.ShowNotificationToAll("Bot or Chara Null ", 4000);
								continue;

							}

							if (player.Distance(coords) > condition.PlayerReputationCheckRadius) {

								//MyVisualScriptLogicProvider.ShowNotificationToAll("Radius Fail ", 4000);
								continue;

							}

							int rep = 0;
							rep = MyAPIGateway.Session.Factions.GetReputationBetweenPlayerAndFaction(player.Player.IdentityId, checkFaction.FactionId);

							if (rep < condition.MinimumReputation && condition.MinimumReputation > -1501) {

								//MyVisualScriptLogicProvider.ShowNotificationToAll("Min Rep Fail " + rep, 4000);
								continue;

							}

							if (rep > condition.MaximumReputation && condition.MaximumReputation < 1501) {

								//MyVisualScriptLogicProvider.ShowNotificationToAll("Max Rep Fail " + rep, 4000);
								continue;

							}

							validFaction = true;
							break;

						}

						if (validFaction == false) {

							factionList.Remove(faction);

							if (specificFactionCheck == true) {

								factionList.Clear();
								break;

							}

							continue;

						}

					}

				}

				if (condition.ChargeNpcFactionForSpawn) {

					for (int i = factionList.Count - 1; i >= 0; i--) {

						var faction = factionList[i];
						long cost = 0;
						var costResult = faction.TryGetBalanceInfo(out cost);

						if (cost > 0 && cost < condition.ChargeForSpawning) {

							factionList.RemoveAt(i);
							continue;

						}

					}

				}

				if (condition.UseSignalRequirement) {

					for (int i = factionList.Count - 1; i >= 0; i--) {

						bool foundValidSignal = false;
						var faction = factionList[i];

						for (int j = GridManager.Grids.Count - 1; j >= 0; j--) {

							if (!GridManager.Grids[j].ActiveEntity() || GridManager.Grids[j].Distance(coords) < DefinitionHelper.HighestAntennaRange)
								continue;

							var range = EntityEvaluator.GridBroadcastRange(GridManager.Grids[j]);

							if ((condition.MinSignalRadius > -1 && range < condition.MinSignalRadius) || (condition.MaxSignalRadius > -1 && range > condition.MaxSignalRadius))
								continue;

							foundValidSignal = true;
						
						}

						if (!foundValidSignal) {

							factionList.RemoveAt(i);
							continue;

						}

					}

				}

			}


			foreach (var faction in factionList) {

				if (resultList.Contains(faction.Tag) == false) {

					resultList.Add(faction.Tag);

				}

			}

			return resultList;

		}



	}

}
