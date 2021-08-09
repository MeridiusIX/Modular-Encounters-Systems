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

namespace ModularEncountersSystems.Spawning {
	public static class SpawnConditions {

		public static Dictionary<string, int> SandboxVariableCache = new Dictionary<string, int>();

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

		public static bool EnvironmentChecks(SpawnConditionsProfile spawnGroup, EnvironmentEvaluation environment, string failReason) {

			if (spawnGroup.MinAirDensity != -1 && environment.AtmosphereAtPosition < spawnGroup.MinAirDensity) {

				failReason = "   - MinAirDensity Check Failed";
				return false;

			}
				

			if (spawnGroup.MaxAirDensity != -1 && environment.AtmosphereAtPosition > spawnGroup.MinAirDensity) {

				failReason = "   - MaxAirDensity Check Failed";
				return false;

			}

			if (spawnGroup.MinGravity != -1 && environment.GravityAtPosition < spawnGroup.MinGravity) {

				failReason = "   - MinGravity Check Failed";
				return false;

			}

			if (spawnGroup.MaxGravity != -1 && environment.GravityAtPosition > spawnGroup.MaxGravity) {

				failReason = "   - MaxGravity Check Failed";
				return false;

			}

			if (spawnGroup.UseDayOrNightOnly) {

				if (spawnGroup.SpawnOnlyAtNight != environment.IsNight) {

					failReason = "   - Night Only Check Failed";
					return false;

				}

			}

			if (spawnGroup.UseWeatherSpawning) {

				if (!spawnGroup.AllowedWeatherSystems.Contains(environment.WeatherAtPosition)) {

					failReason = "   - Weather Check Failed";
					return false;

				}

			}

			if (spawnGroup.UseTerrainTypeValidation) {

				if (!spawnGroup.AllowedTerrainTypes.Contains(environment.CommonTerrainAtPosition)) {

					failReason = "   - Allowed Terrain Check Failed";
					return false;

				}

			}

			bool requiresWater = false;

			if (spawnGroup.PlanetaryInstallation) {

				requiresWater = (!spawnGroup.InstallationSpawnsOnDryLand && (spawnGroup.InstallationSpawnsOnWaterSurface || spawnGroup.InstallationSpawnsUnderwater));

			} else {

				requiresWater = spawnGroup.MustSpawnUnderwater;

			}

			if (requiresWater) {

				if (!APIs.WaterModApiLoaded || environment.WaterInSurroundingAreaRatio < .1) {

					failReason = "   - Water Check Failed";
					return false;

				}
					

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

			var modId = spawnGroup.SpawnGroup?.Context?.ModId ?? "N/A";
			var spawnTypeBlacklist = Settings.GetSpawnTypeBlacklist(type);
			var spawnTypePlanetBlacklist = Settings.GetSpawnTypePlanetBlacklist(type);

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

			if ((spawnTypes.HasFlag(SpawningType.PlanetaryInstallation) || spawnTypes.HasFlag(SpawningType.WaterSurfaceStation) || spawnTypes.HasFlag(SpawningType.UnderWaterStation)) && spawnGroup.PlanetaryInstallationChance < spawnGroup.ChanceCeiling)
				return MathTools.RandomChance(spawnGroup.PlanetaryInstallationChance, spawnGroup.ChanceCeiling);

			if ((spawnTypes.HasFlag(SpawningType.BossAtmo) || spawnTypes.HasFlag(SpawningType.BossGravity) || spawnTypes.HasFlag(SpawningType.BossSpace)) && spawnGroup.BossEncounterChance < spawnGroup.ChanceCeiling)
				return MathTools.RandomChance(spawnGroup.BossEncounterChance, spawnGroup.ChanceCeiling);

			if (spawnTypes.HasFlag(SpawningType.Creature) && spawnGroup.CreatureChance < spawnGroup.ChanceCeiling)
				return MathTools.RandomChance(spawnGroup.CreatureChance, spawnGroup.ChanceCeiling);

			return true;
		
		}

		public static bool CheckCommonSpawnConditions(ImprovedSpawnGroup spawnGroup, SpawnConditionsProfile conditions, SpawnGroupCollection collection, EnvironmentEvaluation environment, bool adminSpawn, SpawningType type, SpawningType spawnTypes, ref string failReason) {

			if (spawnGroup.SpawnGroupEnabled == false && !adminSpawn) {

				failReason = "   - SpawnGroup Not Enabled";
				return false;

			}

			if (conditions.AtmosphericCargoShip && spawnTypes.HasFlag(SpawningType.PlanetaryCargoShip) && !spawnTypes.HasFlag(SpawningType.GravityCargoShip)) {

				if (environment.AirTravelViabilityRatio < 0.25 && !conditions.SkipAirDensityCheck) {

					failReason = "   - Atmosphere Too Thin For Planetary Cargo Ship";
					return false;

				}
			
			}

			if (CheckBlacklists(type, spawnGroup, environment, conditions, ref failReason)) {

				return false;
			
			}

			if (!adminSpawn && !CheckChance(spawnTypes, conditions)) {

				failReason = "   - SpawnGroup Failed Chance Roll";
				return false;

			}

			if (spawnTypes.HasFlag(SpawningType.PlanetaryInstallation) || spawnTypes.HasFlag(SpawningType.WaterSurfaceStation) || spawnTypes.HasFlag(SpawningType.UnderWaterStation)) {

				if (conditions.PlanetaryInstallationType != "Small" && conditions.PlanetaryInstallationType != "Medium" && conditions.PlanetaryInstallationType != "Large") {

					conditions.PlanetaryInstallationType = "Small";

				}

			}

			if (conditions.UniqueEncounter == true && NpcManager.UniqueGroupsSpawned.Contains(spawnGroup.SpawnGroup.Id.SubtypeName) == true) {

				failReason = "   - SpawnGroup Is Unique Encounter That Already Spawned In This World";
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

			if (EnvironmentChecks(conditions, environment, failReason) == false) {

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

				if (KnownPlayerLocationManager.IsPositionInKnownPlayerLocation(environment.Position, conditions.KnownPlayerLocationMustMatchFaction, spawnGroup.FactionOwner) == false) {

					failReason = "   - Known Player Location Check Failed";
					return false;

				}

			}

			if (!ZoneValidation(spawnGroup, conditions, collection, environment.Position)) {

				failReason = "   - Zone Check Failed";
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

			if (conditions.UseThreatLevelCheck == true) {

				var threatLevel = GetThreatLevel(conditions.ThreatLevelCheckRange, conditions.ThreatIncludeOtherNpcOwners, environment.Position);
				threatLevel -= (float)Settings.General.ThreatReductionHandicap;

				if (threatLevel < (float)conditions.ThreatScoreMinimum && (float)conditions.ThreatScoreMinimum > 0) {

					failReason = "   - Minimum Threat Check Failed";
					return false;

				}

				if (threatLevel > (float)conditions.ThreatScoreMaximum && (float)conditions.ThreatScoreMaximum > 0) {

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

			//Logger.Write(spawnGroup.SpawnGroupName + " Passed Common Conditions", true);
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

		public static float GetThreatLevel(double checkRange, bool includeNpcs, Vector3D coords) {

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

				if (!validOwner)
					continue;

				totalThreatLevel += EntityEvaluator.GridTargetValue(grid);

			}

			return totalThreatLevel - Settings.General.ThreatReductionHandicap;

		}

		public static bool ZoneValidation(ImprovedSpawnGroup spawnGroup, SpawnConditionsProfile conditions, SpawnGroupCollection collection, Vector3D position){

			for (int i = 0; i < conditions.ZoneConditions.Count; i++) {

				var zoneCondition = conditions.ZoneConditions[i];

				for (int j = 0; j < ZoneManager.ActiveZones.Count; j++) {

					var zone = ZoneManager.ActiveZones[j];

					if (!zone.Persistent)
						continue;

					if (!string.IsNullOrWhiteSpace(zoneCondition.ZoneName) && zone.PublicName != zoneCondition.ZoneName)
						continue;

					var distance = Vector3D.Distance(position, zone.Coordinates);

					//In Zone Radius
					if (distance > zone.Radius)
						continue;

					//Min Dist From Center
					if (zoneCondition.MinDistanceFromZoneCenter > -1 && distance < zoneCondition.MinDistanceFromZoneCenter)
						continue;

					//Max Dist From Center
					if (zoneCondition.MaxDistanceFromZoneCenter > -1 && distance > zoneCondition.MaxDistanceFromZoneCenter)
						continue;

					//Min Spawned Encounters
					if (zoneCondition.MinSpawnedZoneEncounters > -1 && zone.SpawnedEncounters < zoneCondition.MinSpawnedZoneEncounters)
						continue;

					//Max Spawned Encounters
					if (zoneCondition.MaxSpawnedZoneEncounters > -1 && zone.SpawnedEncounters > zoneCondition.MaxSpawnedZoneEncounters)
						continue;

					//Custom Counters
					if (zoneCondition.CheckCustomZoneCounters) {

						bool failedResult = false;

						foreach (var counterName in zoneCondition.CustomZoneCounterReference.Keys) {

							long value = 0;

							if (!zone.CustomCounters.TryGetValue(counterName, out value)) {

								failedResult = true;
								break;

							}

							if (value < zoneCondition.CustomZoneCounterReference[counterName]) {

								failedResult = true;
								break;

							}
						
						}

						if (failedResult)
							continue;
					
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

						if (failedResult)
							continue;

					}

					collection.ActiveZone = zone;
					return true;

				}

			}

			//Check Against Rules For Strict and Faction Only
			if (collection.MustUseStrictZone)
				return false;

			if (collection.AllowedZoneFactions.Count > 0)
				return false;

			return true;
			
		}


		public static List<string> ValidNpcFactions(ImprovedSpawnGroup spawnGroup, SpawnConditionsProfile condition, Vector3D coords, string factionOverride = "", bool forceSpawn = false) {

			var resultList = new List<string>();
			var factionList = new List<IMyFaction>();
			var initialFactionTag = !string.IsNullOrWhiteSpace(factionOverride) ? factionOverride : spawnGroup.FactionOwner;

			if (!string.IsNullOrWhiteSpace(factionOverride) || (spawnGroup.UseRandomBuilderFaction == false && spawnGroup.UseRandomMinerFaction == false && spawnGroup.UseRandomTraderFaction == false)) {

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

			if (spawnGroup.UseRandomBuilderFaction == true) {

				var tempList = factionList.Concat(FactionHelper.NpcBuilderFactions);
				factionList = new List<IMyFaction>(tempList.ToList());

			}

			if (spawnGroup.UseRandomMinerFaction == true) {

				var tempList = factionList.Concat(FactionHelper.NpcMinerFactions);
				factionList = new List<IMyFaction>(tempList.ToList());

			}

			if (spawnGroup.UseRandomTraderFaction == true) {

				var tempList = factionList.Concat(FactionHelper.NpcTraderFactions);
				factionList = new List<IMyFaction>(tempList.ToList());

			}

			if (factionList.Count == 0) {

				var defaultFaction = MyAPIGateway.Session.Factions.TryGetFactionByTag(spawnGroup.FactionOwner);

				if (defaultFaction != null) {

					factionList.Add(defaultFaction);

				}

			}

			if (forceSpawn)
				return resultList;

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

								continue;

							}

							checkFaction = factionOvr;
							specificFactionCheck = true;

						}

					}

					foreach (var player in PlayerManager.Players) {

						if (!player.Online)
							continue;

						if (player.Player.IsBot == true || player.Player.Character == null) {

							continue;

						}

						if (player.Distance(coords) > condition.PlayerReputationCheckRadius) {

							continue;

						}

						//var playerFaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(player.IdentityId);

						int rep = 0;
						rep = MyAPIGateway.Session.Factions.GetReputationBetweenPlayerAndFaction(player.Player.IdentityId, checkFaction.FactionId);

						/*
						if(playerFaction != null) {

							rep = MyAPIGateway.Session.Factions.GetReputationBetweenFactions(playerFaction.FactionId, checkFaction.FactionId);

						} else {

							rep = MyAPIGateway.Session.Factions.GetReputationBetweenPlayerAndFaction(player.IdentityId, checkFaction.FactionId);

						}
						*/

						if (rep < condition.MinimumReputation && condition.MinimumReputation > -1501) {

							continue;

						}

						if (rep > condition.MaximumReputation && condition.MaximumReputation < 1501) {

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

			foreach (var faction in factionList) {

				if (resultList.Contains(faction.Tag) == false) {

					resultList.Add(faction.Tag);

				}

			}

			return resultList;

		}



	}

}
