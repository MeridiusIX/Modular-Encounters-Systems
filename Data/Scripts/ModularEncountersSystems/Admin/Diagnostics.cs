using ModularEncountersSystems.API;
using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Sync;
using ModularEncountersSystems.World;
using ModularEncountersSystems.Zones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Admin {

	public static class Diagnostics {

		private static EnvironmentEvaluation _environment = null;
		private static float _threatLevel = 0;
		private static float _pcuLevel = 0;
		private static byte _combatPhase = 0;

		private static SpawnGroupCollection _collection = null;
		private static SpawnConditionTypeTally _tally = new SpawnConditionTypeTally();

		private static bool _aerodynamicsMod = false;
		private static bool _infestationEnabledMod = false;
		private static bool _airDensityRatioBelowRecommendation = false;
		private static bool _globalNpcLimitReached = false;

		public static void RunDiagnostics(Vector3D playerPostion) {

			ClearPreviousValues();


		}

		private static void ClearPreviousValues() {

			_environment = null;
			_threatLevel = 0;
			_pcuLevel = 0;
			_combatPhase = 0;

			_collection = null;
			_tally.ClearReasons();

			_aerodynamicsMod = false;
			_infestationEnabledMod = false;
			_airDensityRatioBelowRecommendation = false;
			_globalNpcLimitReached = false;

		}

		public static string GetEligibleSpawnsAtPosition(ChatMessage msg) {

			//StringBuilder
			var sb = new StringBuilder();

			//Environment
			var environment = new EnvironmentEvaluation(msg.PlayerPosition);
			var threatLevel = SpawnConditions.GetThreatLevel(5000, false, msg.PlayerPosition);
			var pcuLevel = SpawnConditions.GetPCULevel(5000, msg.PlayerPosition);
			SpawnGroupCollection collection = null;

			sb.Append("::: Spawn Data Near Player :::").AppendLine();
			sb.Append(" - Threat Score: ").Append(threatLevel.ToString()).AppendLine();
			sb.Append(" - PCU Score:    ").Append(pcuLevel.ToString()).AppendLine();
			sb.Append(" - Combat Phase: ").Append(!Settings.Combat.EnableCombatPhaseSystem ? "Disabled In Settings" : (CombatPhaseManager.Active ? "Active" : "Inactive")).AppendLine();

			sb.AppendLine();

			//Environment Data Near Player
			sb.Append("::: Environment Data Near Player :::").AppendLine();
			sb.Append(" - Distance From World Center:      ").Append(environment.DistanceFromWorldCenter.ToString()).AppendLine();
			sb.Append(" - Direction From World Center:     ").Append(environment.DirectionFromWorldCenter.ToString()).AppendLine();
			sb.Append(" - Is On Planet:                    ").Append(environment.IsOnPlanet.ToString()).AppendLine();
			sb.Append(" - Planet Name:                     ").Append(environment.IsOnPlanet ? environment.NearestPlanetName : "N/A").AppendLine();
			sb.Append(" - Planet Entity Id:                ").Append(environment.IsOnPlanet ? environment.NearestPlanet.Planet.EntityId.ToString() : "N/A").AppendLine();
			sb.Append(" - Planet Center Coordinates:       ").Append(environment.IsOnPlanet ? environment.NearestPlanet.Center().ToString() : "N/A").AppendLine();
			sb.Append(" - Planet Surface Coordinates:      ").Append(environment.IsOnPlanet ? environment.SurfaceCoords.ToString() : "N/A").AppendLine();
			sb.Append(" - Planet Diameter:                 ").Append(environment.IsOnPlanet ? environment.PlanetDiameter.ToString() : "N/A").AppendLine();
			sb.Append(" - Oxygen At Position:              ").Append(environment.IsOnPlanet ? environment.OxygenAtPosition.ToString() : "N/A").AppendLine();
			sb.Append(" - Atmosphere At Position:          ").Append(environment.IsOnPlanet ? environment.AtmosphereAtPosition.ToString() : "N/A").AppendLine();
			sb.Append(" - Gravity At Position:             ").Append(environment.IsOnPlanet ? environment.GravityAtPosition.ToString() : "N/A").AppendLine();
			sb.Append(" - Altitude At Position:            ").Append(environment.IsOnPlanet ? environment.AltitudeAtPosition.ToString() : "N/A").AppendLine();
			sb.Append(" - Is Night At Position:            ").Append(environment.IsOnPlanet ? environment.IsNight.ToString() : "N/A").AppendLine();
			sb.Append(" - Weather At Position:             ").Append(environment.IsOnPlanet && !string.IsNullOrWhiteSpace(environment.WeatherAtPosition) ? environment.WeatherAtPosition.ToString() : "N/A").AppendLine();
			sb.Append(" - Common Terrain At Position:      ").Append(environment.IsOnPlanet ? environment.CommonTerrainAtPosition.ToString() : "N/A").AppendLine();
			sb.Append(" - Air Travel Viability Ratio:      ").Append(environment.IsOnPlanet ? (Math.Round(environment.AirTravelViabilityRatio, 3)).ToString() : "N/A").AppendLine().AppendLine();

			sb.Append(" - Water Mod Enabled:               ").Append(AddonManager.WaterMod).AppendLine();
			sb.Append(" - Planet Has Water:                ").Append(environment.IsOnPlanet ? environment.PlanetHasWater.ToString() : "N/A").AppendLine();
			sb.Append(" - Position Underwater:             ").Append(environment.IsOnPlanet ? environment.PositionIsUnderWater.ToString() : "N/A").AppendLine();
			sb.Append(" - Surface Underwater:              ").Append(environment.IsOnPlanet ? environment.SurfaceIsUnderWater.ToString() : "N/A").AppendLine();
			sb.Append(" - Water Coverage Ratio:            ").Append(environment.IsOnPlanet ? (Math.Round(environment.WaterInSurroundingAreaRatio, 3)).ToString() : "N/A").AppendLine().AppendLine();

			sb.Append(" - Nebula Mod Enabled:              ").Append(AddonManager.NebulaMod).AppendLine();
			sb.Append(" - Inside Nebula:                   ").Append(environment.InsideNebula).AppendLine();
			sb.Append(" - Nebula Density:                  ").Append(environment.NebulaDensity).AppendLine();
			sb.Append(" - Nebula Material:                 ").Append(environment.NebulaMaterial).AppendLine();
			sb.Append(" - Nebula Weather:                  ").Append(environment.NebulaWeather).AppendLine().AppendLine();

			sb.Append(" - Space Cargo Ship Eligible:       ").Append(environment.SpaceCargoShipsEligible).AppendLine();
			sb.Append(" - Lunar Cargo Ship Eligible:       ").Append(environment.LunarCargoShipsEligible).AppendLine();
			sb.Append(" - Planetary Cargo Ship Eligible:   ").Append(environment.PlanetaryCargoShipsEligible).AppendLine();
			sb.Append(" - Gravity Cargo Ship Eligible:     ").Append(environment.GravityCargoShipsEligible).AppendLine();
			sb.Append(" - Random Encounter Eligible:       ").Append(environment.RandomEncountersEligible).AppendLine();
			sb.Append(" - Planetary Installation Eligible: ").Append(environment.PlanetaryInstallationEligible).AppendLine();
			sb.Append(" - Water Installation Eligible:     ").Append(environment.WaterInstallationEligible).AppendLine();

			sb.AppendLine();

			SpawnLogger.SpawnGroup.Clear();

			//Space Cargo
			collection = new SpawnGroupCollection();
			SpawnGroupManager.GetSpawnGroups(SpawningType.SpaceCargoShip, "Debug", environment, "", collection);

			if (collection.SpawnGroups.Count > 0) {

				sb.Append("::: Space / Lunar Cargo Ship Eligible Spawns :::").AppendLine();

				foreach (var sgroup in collection.SpawnGroups.Distinct()) {

					sb.Append(" - ").Append(sgroup.SpawnGroupName).AppendLine();

				}

				sb.AppendLine();

			}

			//Random Encounter
			collection = new SpawnGroupCollection();
			SpawnGroupManager.GetSpawnGroups(SpawningType.RandomEncounter, "Debug", environment, "", collection);

			if (collection.SpawnGroups.Count > 0) {

				sb.Append("::: Random Encounter Eligible Spawns :::").AppendLine();

				foreach (var sgroup in collection.SpawnGroups.Distinct()) {

					sb.Append(" - ").Append(sgroup.SpawnGroupName).AppendLine();

				}

				sb.AppendLine();

			}

			//Planetary Cargo
			collection = new SpawnGroupCollection();
			SpawnGroupManager.GetSpawnGroups(SpawningType.PlanetaryCargoShip, "Debug", environment, "", collection);

			if (environment.PlanetaryCargoShipsEligible || environment.GravityCargoShipsEligible) {

				sb.Append("::: Planetary / Gravity Cargo Ship Eligible Spawns :::").AppendLine();

				if (APIs.DragApiLoaded && APIs.Drag.AdvLift && !Settings.Grids.AerodynamicsModAdvLiftOverride) {

					sb.Append(" > Aerodynamics Mod AdvLift Detected. Most Planetary Cargo Ships Will Not Be Compatible.").AppendLine();
					sb.Append(" > To Restore Cargo Ships, Use One Of The Following Options:").AppendLine();
					sb.Append("   > Disable AdvLift in Aerodynamics Mod Config.").AppendLine();
					sb.Append("   > Remove Aerodynamics Mod.").AppendLine();
					sb.Append("   > Enable [AerodynamicsModAdvLiftOverride] in MES Config File Config-Grids.xml (Not Recommended / Ships May Not Behave Properly)").AppendLine();

				}

				if (collection.SpawnGroups.Count > 0) {

					foreach (var sgroup in collection.SpawnGroups.Distinct()) {

						sb.Append(" - ").Append(sgroup.SpawnGroupName).AppendLine();

					}

				}

				sb.AppendLine();

			}

			//Planetary Installation
			collection = new SpawnGroupCollection();
			SpawnGroupManager.GetSpawnGroups(SpawningType.PlanetaryInstallation, "Debug", environment, "", collection);

			if (collection.SpawnGroups.Count > 0) {

				sb.Append("::: Planetary Installation Eligible Spawns :::").AppendLine();

				foreach (var sgroup in collection.SpawnGroups.Distinct()) {

					sb.Append(" - ").Append(sgroup.SpawnGroupName).AppendLine();

				}

				sb.AppendLine();

			}

			//Boss
			collection = new SpawnGroupCollection();
			SpawnGroupManager.GetSpawnGroups(SpawningType.BossEncounter, "Debug", environment, "", collection);

			if (collection.SpawnGroups.Count > 0) {

				sb.Append("::: Boss Encounter Eligible Spawns :::").AppendLine();

				foreach (var sgroup in collection.SpawnGroups.Distinct()) {

					sb.Append(" - ").Append(sgroup.SpawnGroupName).AppendLine();

				}

				sb.AppendLine();

			}

			//Creature
			collection = new SpawnGroupCollection();
			SpawnGroupManager.GetSpawnGroups(SpawningType.Creature, "Debug", environment, "", collection);

			if (collection.SpawnGroups.Count > 0) {

				sb.Append("::: Creature / Bot Eligible Spawns :::").AppendLine();

				foreach (var sgroup in collection.SpawnGroups.Distinct()) {

					sb.Append(" - ").Append(sgroup.SpawnGroupName).AppendLine();

				}

				sb.AppendLine();

			}

			//Drone Encounters
			collection = new SpawnGroupCollection();
			SpawnGroupManager.GetSpawnGroups(SpawningType.DroneEncounter, "Debug", environment, "", collection);

			if (collection.SpawnGroups.Count > 0) {

				sb.Append("::: Drone Encounter Eligible Spawns :::").AppendLine();

				foreach (var sgroup in collection.SpawnGroups.Distinct()) {

					sb.Append(" - ").Append(sgroup.SpawnGroupName).AppendLine();

				}

				sb.AppendLine();

			}

			//StaticEncounters
			if (NpcManager.StaticEncounters != null) {

				if (NpcManager.StaticEncounters.Count > 0) {

					sb.Append("::: Static Encounter Eligible Spawns :::").AppendLine();

					foreach (var enc in NpcManager.StaticEncounters) {

						if (enc == null)
							continue;

						sb.Append(" - ").Append(!string.IsNullOrWhiteSpace(enc?.SpawnGroupName) ? enc.SpawnGroupName : "(null)").AppendLine();
						sb.Append("   - Coodinates: ").Append(enc.ExactLocationCoords.ToString()).AppendLine();
						sb.Append("   - Radius:     ").Append(enc.TriggerRadius.ToString()).AppendLine();

					}

					sb.AppendLine();

				}

			} else {

				sb.Append("::: WARNING: Static Encounter List Null! :::").AppendLine();
				sb.AppendLine();

			}



			//UniqueEncounters
			if (NpcManager.UniqueGroupsSpawned != null) {

				if (NpcManager.UniqueGroupsSpawned.Count > 0) {

					sb.Append("::: Unique Encounters Already Spawned :::").AppendLine();

					foreach (var enc in NpcManager.UniqueGroupsSpawned) {

						sb.Append(" - ").Append(enc).AppendLine();

					}

					sb.AppendLine();

				}

			} else {

				sb.Append("::: WARNING: Unique Encounter List Null! :::").AppendLine();
				sb.AppendLine();

			}



			//Zones
			if (ZoneManager.ActiveZones.Count > 0) {

				sb.Append("::: Active Zones In Range :::").AppendLine();

				foreach (var zone in ZoneManager.ActiveZones) {

					if (zone.PositionInsideZone(environment.Position))
						sb.Append(zone.GetInfo(environment.Position)).AppendLine();

				}

			}

			//Timeouts
			if (TimeoutManagement.Timeouts.Count > 0) {

				sb.Append("::: Timeout Zones In Range :::").AppendLine();

				foreach (var timeout in TimeoutManagement.Timeouts) {

					var timeoutRemaining = timeout.TimeoutLength();

					if (timeoutRemaining.X >= timeoutRemaining.Y) {

						timeout.Remove = true;
						continue;

					}

					sb.Append(timeout.GetInfo(environment.Position)).AppendLine();

				}

				sb.AppendLine();

			}

			//Planetary Lanes
			if (environment.InsidePlanetaryLanes.Count > 0) {

				sb.Append("::: Planetary Lanes In Position :::").AppendLine();

				foreach (var lane in environment.InsidePlanetaryLanes) {

					sb.Append(" - ").Append(lane.PlanetA.Planet.Generator.Id.SubtypeName).Append(" - ").Append(lane.PlanetB.Planet.Generator.Id.SubtypeName).AppendLine();

				}

				sb.AppendLine();

			}

			return sb.ToString();

		}

	}

}
