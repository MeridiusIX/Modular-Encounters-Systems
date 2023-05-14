using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.World;
using ModularEncountersSystems.Zones;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRageMath;

namespace ModularEncountersSystems.Spawning {

	[Flags]
	public enum SpawningType {
	
		None = 0,
		SpaceCargoShip = 1,
		LunarCargoShip = 1 << 1,
		RandomEncounter = 1 << 2,
		PlanetaryCargoShip = 1 << 3,
		GravityCargoShip = 1 << 4,
		PlanetaryInstallation = 1 << 5,
		WaterSurfaceStation = 1 << 6,
		UnderWaterStation = 1 << 7,
		BossEncounter = 1 << 8,
		BossSpace = 1 << 9,
		BossAtmo = 1 << 10,
		BossGravity = 1 << 11,
		Creature = 1 << 12,
		ForcedCreature = 1 << 13,
		OtherNPC = 1 << 14,
		StaticEncounter = 1 << 15,
		StaticEncounterPlanet = 1 << 16,
		StaticEncounterSpace = 1 << 17,
		DryLandInstallation = 1 << 18,
		DroneEncounter = 1 << 19,

	}

	public static class SpawnRequest {

		public static Dictionary<string, Dictionary<string, DateTime>> DroneEncounterPlayerTracker = new Dictionary<string, Dictionary<string, DateTime>>();

		public static SpawningType GetPrimarySpawningType(SpawningType type) {

			if (type.HasFlag(SpawningType.SpaceCargoShip) || type.HasFlag(SpawningType.LunarCargoShip))
				return SpawningType.SpaceCargoShip;

			if (type.HasFlag(SpawningType.RandomEncounter))
				return SpawningType.RandomEncounter;

			if (type.HasFlag(SpawningType.PlanetaryCargoShip) || type.HasFlag(SpawningType.GravityCargoShip))
				return SpawningType.PlanetaryCargoShip;

			if (type.HasFlag(SpawningType.PlanetaryInstallation) || type.HasFlag(SpawningType.WaterSurfaceStation) || type.HasFlag(SpawningType.UnderWaterStation) || type.HasFlag(SpawningType.DryLandInstallation))
				return SpawningType.PlanetaryInstallation;

			if (type.HasFlag(SpawningType.BossEncounter) || type.HasFlag(SpawningType.BossSpace) || type.HasFlag(SpawningType.BossAtmo) || type.HasFlag(SpawningType.BossGravity))
				return SpawningType.BossEncounter;

			if (type.HasFlag(SpawningType.Creature) || type.HasFlag(SpawningType.ForcedCreature))
				return SpawningType.Creature;

			if (type.HasFlag(SpawningType.OtherNPC))
				return SpawningType.OtherNPC;

			if (type.HasFlag(SpawningType.StaticEncounter) || type.HasFlag(SpawningType.StaticEncounterPlanet))
				return SpawningType.StaticEncounter;

			if (type.HasFlag(SpawningType.DroneEncounter))
				return SpawningType.DroneEncounter;

			return SpawningType.None;

		}

		public static bool IsCargoShip(SpawningType type) {

			if (type.HasFlag(SpawningType.SpaceCargoShip))
				return true;

			if (type.HasFlag(SpawningType.PlanetaryCargoShip))
				return true;

			if (type.HasFlag(SpawningType.LunarCargoShip))
				return true;

			if (type.HasFlag(SpawningType.GravityCargoShip))
				return true;

			return false;

		}

		public static bool PlayerSpawnEligiblity(SpawningType spawnType, WatchedPlayer player) {

			if (!player.CheckTimer(spawnType)) {

				//SpawnLogger.Write(player.Player.Player.DisplayName + " Timer Check Failed (" + player.GetTimerValue(spawnType) + ") For Spawn Type: " + spawnType, SpawnerDebugEnum.Dev);
				return false;

			}
				

			player.ResetTimer(spawnType);

			if (player.Player.GetPosition() == Vector3D.Zero) {

				player.Player.RefreshPlayerEntity();

				if (player.Player.GetPosition() == Vector3D.Zero) {

					SpawnLogger.Write(player.Player.Player.DisplayName + " Position Recorded As Vector3D.Zero for Spawning. Player may be online, but not controlling a character/grid entity.", SpawnerDebugEnum.Spawning);

				}

			}

			if (!LocationSpawnEligibility(spawnType, player.Player.GetPosition())) {

				//SpawnLogger.Write(player.Player.Player.DisplayName + " Position Invalid For Spawn Type: " + spawnType, SpawnerDebugEnum.Dev);
				player.ResetPosition(spawnType);
				return false;

			} else {

				if (!LocationSpawnEligibility(spawnType, player.GetLastPosition(spawnType))) {

					//SpawnLogger.Write(player.Player.Player.DisplayName + " Previous Position Invalid For Spawn Type: " + spawnType, SpawnerDebugEnum.Dev);
					player.ResetPosition(spawnType);
					return false;

				}
			
			}

			if (!PlayerDistanceToNextEncounter(spawnType, player)) {

				//SpawnLogger.Write(player.Player.Player.DisplayName + " Hasn't Travelled Far Enough To Trigger Spawn Type: " + spawnType, SpawnerDebugEnum.Dev);
				return false;
			
			}

			return true;
		
		}

		public static bool PlayerDistanceToNextEncounter(SpawningType spawnType, WatchedPlayer player) {

			PlanetEntity planet = null;
			var coords = player.Player.GetPosition();

			foreach (var planetEnt in PlanetManager.Planets) {

				if (!planetEnt.IsPositionInGravity(coords))
					continue;

				planet = planetEnt;
				break;

			}

			if (spawnType == SpawningType.RandomEncounter) {

				if (planet != null)
					return false;

				if (Vector3D.Distance(coords, player.RandomEncounterDistanceCoordCheck) < Settings.RandomEncounters.PlayerTravelDistance)
					return false;

				player.RandomEncounterDistanceCoordCheck = coords;
				return true;

			}

			if (spawnType == SpawningType.PlanetaryInstallation) {

				if (planet == null)
					return false;

				if (!planet.IsPositionInGravity(player.InstallationDistanceCoordCheck))
					player.InstallationDistanceCoordCheck = coords;

				if (Vector3D.Distance(planet.GetPositionAtAverageRadius(coords), planet.GetPositionAtAverageRadius(player.InstallationDistanceCoordCheck)) < Settings.PlanetaryInstallations.PlayerDistanceSpawnTrigger)
					return false;

				player.InstallationDistanceCoordCheck = coords;
				return true;

			}

			return true;
		
		}

		public static bool LocationSpawnEligibility(SpawningType spawnType, Vector3D coords) {

			bool InGravity = false;
			double Altitude = 0;

			foreach (var planet in PlanetManager.Planets) {

				if (!planet.IsPositionInGravity(coords))
					continue;

				InGravity = true;
				Altitude = planet.AltitudeAtPosition(coords);
				break;

			}

			if (spawnType == SpawningType.SpaceCargoShip) {

				return true;
			
			}

			if (spawnType == SpawningType.RandomEncounter) {

				if (!InGravity)
					return true;

			}

			if (spawnType == SpawningType.PlanetaryCargoShip) {

				if (InGravity)
					return true;

			}

			if (spawnType == SpawningType.PlanetaryInstallation) {

				if (InGravity && Altitude <= Settings.PlanetaryInstallations.PlayerMaximumDistanceFromSurface)
					return true;

			}

			if (spawnType == SpawningType.BossEncounter) {

				return true;

			}

			if (spawnType == SpawningType.Creature) {

				if (InGravity && Altitude <= Settings.PlanetaryInstallations.PlayerMaximumDistanceFromSurface)
					return true;

			}

			if (spawnType == SpawningType.DroneEncounter) {

				return true;
			
			}

			return false;
		
		}

		public static bool CalculateSpawn(Vector3D coords, string source, SpawningType type = SpawningType.None, bool forceSpawn = false, bool adminSpawn = false, List<string> eligibleNames = null, string factionOverride = null, MatrixD spawnMatrix = new MatrixD(), Vector3D customVelocity = new Vector3D(), bool ignoreSafetyChecks = false, long ownerOverride = -1) {

			SpawnLogger.Write("Spawn Request Received From: " + source, SpawnerDebugEnum.Spawning);

			//Main Spawner Enabled
			if (!MES_SessionCore.ModEnabled) {

				SpawnLogger.Write("Spawner Not Enabled", SpawnerDebugEnum.Spawning);
				SpawnLogger.Write(string.Format("[{0}] Spawner Not Enabled", string.IsNullOrWhiteSpace(source) ? "null" : source), SpawnerDebugEnum.SpawnRecord);
				return false;

			}

			//No Spawning Type
			if (type == SpawningType.None) {

				SpawnLogger.Write("No Spawning Type Provided", SpawnerDebugEnum.Spawning);
				SpawnLogger.Write(string.Format("[{0}] No Spawning Type Provided", string.IsNullOrWhiteSpace(source) ? "null" : source), SpawnerDebugEnum.SpawnRecord);
				return false;
			
			}

			//Max NPCs
			if (!adminSpawn && Settings.General.UseMaxNpcGrids && type != SpawningType.Creature && NpcManager.GetGlobalNpcCount() >= Settings.General.MaxGlobalNpcGrids) {

				SpawnLogger.Write("Max Global NPCs Reached/Exceeded", SpawnerDebugEnum.Spawning);
				SpawnLogger.Write(string.Format("[{0}] Max Global NPCs Reached/Exceeded", string.IsNullOrWhiteSpace(source) ? "null" : source), SpawnerDebugEnum.SpawnRecord);
				return false;
			
			}

			//Max NPCs Of Type in Area
			var areaSize = Settings.GetSpawnAreaRadius(type);

			if (!adminSpawn && areaSize > -1 && NpcManager.GetAreaNpcCount(type, coords, areaSize) >= Settings.GetMaxAreaSpawns(type)) {

				SpawnLogger.Write("Max SpawnType NPCs Reached/Exceeded for: " + type.ToString(), SpawnerDebugEnum.Spawning);
				SpawnLogger.Write(string.Format("[{0}] Max SpawnType NPCs Reached/Exceeded for: " + type.ToString(), string.IsNullOrWhiteSpace(source) ? "null" : source), SpawnerDebugEnum.SpawnRecord);
				return false;
			
			}

			

			KnownPlayerLocationManager.CleanExpiredLocations();

			if (!forceSpawn && !adminSpawn) {

				if (!TimeoutManagement.IsSpawnAllowed(type, coords)) {

					//TODO: Add Wave Spawner Exceptions
					SpawnLogger.Write("Spawning For This Encounter Type Is Timed Out In This Area: " + type.ToString(), SpawnerDebugEnum.Spawning);
					SpawnLogger.Write(string.Format("[{0}] Spawning For This Encounter Type Is Timed Out In This Area: " + type.ToString(), string.IsNullOrWhiteSpace(source) ? "null" : source), SpawnerDebugEnum.SpawnRecord);
					return false;

				}
			
			}

			//Generate Environment Object
			var environment = new EnvironmentEvaluation(coords);

			//Underground Condition
			if (environment.IsOnPlanet && environment.AltitudeAtPosition < -1000) {

				SpawnLogger.Write("Proposed Spawn Location Is 1000m Beneath Planet Surface. Aborting Spawn.", SpawnerDebugEnum.Spawning);
				SpawnLogger.Write(string.Format("[{0}] Proposed Spawn Location Is 1000m Beneath Planet Surface. Aborting Spawn.", string.IsNullOrWhiteSpace(source) ? "null" : source), SpawnerDebugEnum.SpawnRecord);
				return false;

			}

			Dictionary<string, DateTime> dronePlayerTracker = null;

			if (!string.IsNullOrWhiteSpace(source) && source.StartsWith("Player")) {

				if (!DroneEncounterPlayerTracker.TryGetValue(source, out dronePlayerTracker)) {

					dronePlayerTracker = new Dictionary<string, DateTime>();
					DroneEncounterPlayerTracker.Add(source, dronePlayerTracker);

				}
			
			}

			//Get SpawnGroups and Valid Factions
			var spawnGroupCollection = new SpawnGroupCollection();
			spawnGroupCollection.IgnoreAllSafetyChecks = ignoreSafetyChecks;
			spawnGroupCollection.OwnerOverride = ownerOverride;
			SpawnLogger.SpawnGroup.Clear();
			SpawnGroupManager.GetSpawnGroups(type, source, environment, factionOverride, spawnGroupCollection, forceSpawn, adminSpawn, eligibleNames, dronePlayerTracker);

			//Select By ModID
			if (Settings.General.UseModIdSelectionForSpawning == true) {

				spawnGroupCollection.SelectSpawnGroupSublist(spawnGroupCollection.SpawnGroupSublists, spawnGroupCollection.EligibleSpawnsByModId, ref spawnGroupCollection.SpawnGroups);
				spawnGroupCollection.SelectSpawnGroupSublist(spawnGroupCollection.SmallSpawnGroupSublists, spawnGroupCollection.EligibleSmallSpawnsByModId, ref spawnGroupCollection.SmallStations);
				spawnGroupCollection.SelectSpawnGroupSublist(spawnGroupCollection.MediumSpawnGroupSublists, spawnGroupCollection.EligibleMediumSpawnsByModId, ref spawnGroupCollection.MediumStations);
				spawnGroupCollection.SelectSpawnGroupSublist(spawnGroupCollection.LargeSpawnGroupSublists, spawnGroupCollection.EligibleLargeSpawnsByModId, ref spawnGroupCollection.LargeStations);

			}

			if (spawnGroupCollection.SpawnGroups.Count == 0) {

				SpawnLogger.Write("Eligible SpawnGroup Count 0", SpawnerDebugEnum.Spawning);
				SpawnLogger.Write(string.Format("[{0}] No Eligible SpawnGroups In Area For Type: " + type.ToString(), string.IsNullOrWhiteSpace(source) ? "null" : source), SpawnerDebugEnum.SpawnRecord);
				return false;

			}

			SpawnLogger.Write("Selecting Random SpawnGroup", SpawnerDebugEnum.Spawning);
			//Select Random Group
			if (!spawnGroupCollection.SelectRandomSpawnGroup(type, environment)) {

				SpawnLogger.Write("Failed To Select Random SpawnGroup", SpawnerDebugEnum.Spawning);
				SpawnLogger.Write(string.Format("[{0}] Failed To Select Random SpawnGroup For Type: " + type.ToString(), string.IsNullOrWhiteSpace(source) ? "null" : source), SpawnerDebugEnum.SpawnRecord);
				return false;
			
			}

			SpawnLogger.Write("Spawn Conditions Selected: " + spawnGroupCollection.Conditions.ProfileSubtypeId, SpawnerDebugEnum.Spawning);
			SpawnLogger.Write("SpawnGroup Selected: " + spawnGroupCollection.SpawnGroup.SpawnGroupName, SpawnerDebugEnum.Spawning);

			SpawnLogger.Write("Start Pathing", SpawnerDebugEnum.Spawning);
			//Determine Path or Placement
			var spawnTypes = SpawnConditions.AllowedSpawningTypes(type, environment);
			var path = PathPlacements.GetSpawnPlacement(type, ref spawnTypes, spawnGroupCollection, environment, spawnMatrix);

			SpawnLogger.Write("End Pathing", SpawnerDebugEnum.Spawning);
			if (!path.ValidPath) {

				SpawnLogger.Write("SpawnGroup Path/Placement Invalid", SpawnerDebugEnum.Spawning);
				SpawnLogger.Write(path.PathDebugging.ToString(), SpawnerDebugEnum.Pathing);
				SpawnLogger.Write(string.Format("[{0}] Could Not Generate Path / Coords For Type: " + type.ToString(), string.IsNullOrWhiteSpace(source) ? "null" : source), SpawnerDebugEnum.SpawnRecord);
				return false;
			
			}

			SpawnLogger.Write("Pathing Successful", SpawnerDebugEnum.Spawning);

			if (Vector3D.Distance(Vector3D.Zero, coords) > 6500000) {

				SpawnLogger.Write("WARNING: Spawning NPC Grids beyond 6500km from world center may result in loss of precision when grids are placed in the world or when they utilize path finding. This is because of game engine limitations.", SpawnerDebugEnum.SpawnRecord, true);

			}

			//Create Boss Encounter
			if (type.HasFlag(SpawningType.BossEncounter)) {

				SpawnLogger.Write("Initializing Boss Encounter", SpawnerDebugEnum.Spawning);
				SpawnLogger.Write(string.Format("[{0}] Initializing Boss Encounter Event / Signal", string.IsNullOrWhiteSpace(source) ? "null" : source), SpawnerDebugEnum.SpawnRecord);
				var bossEncounter = new StaticEncounter();
				bossEncounter.InitBossEncounter(spawnGroupCollection.SpawnGroup.SpawnGroupName, spawnGroupCollection.ConditionsIndex, path.StartCoords, spawnGroupCollection.SelectRandomFaction(), spawnTypes);
				NpcManager.StaticEncounters.Add(bossEncounter);
				NpcManager.UpdateStaticEncounters();
				return true;
			
			}

			path.CustomVelocity = customVelocity;

			//Send Request To Prefab Spawner

			bool result = false;

			if (type == SpawningType.Creature) {

				SpawnLogger.Write("SpawnGroup Sent To Bot Spawner", SpawnerDebugEnum.Spawning);
				result = BotSpawner.SpawnBots(spawnGroupCollection, path, environment);
			
			} else {

				SpawnLogger.Write("SpawnGroup Sent To Prefab Spawner", SpawnerDebugEnum.Spawning);
				result = PrefabSpawner.ProcessSpawning(spawnGroupCollection, path, environment);

			}

			if (!result)
				return false;

			SpawnLogger.Write("Spawn Successful: " + spawnGroupCollection.SpawnGroup.SpawnGroupName, SpawnerDebugEnum.Spawning);
			SpawnLogger.Write(string.Format("[{0}] Spawn Successfully Processed: [" + spawnGroupCollection.SpawnGroup.SpawnGroupName + "] for Type: " + type.ToString(), string.IsNullOrWhiteSpace(source) ? "null" : source), SpawnerDebugEnum.SpawnRecord);

			//Post Spawn Checks
			PostSpawn(type, path, spawnGroupCollection, environment, dronePlayerTracker);

			return true;
		
		}

		public static bool CalculateStaticSpawn(StaticEncounter encounter, PlayerEntity player, SpawningType type, SpawningType spawnTypes) {

			SpawnLogger.Write("Static/Boss Encounter Requested", SpawnerDebugEnum.Spawning);

			//Main Spawner Enabled
			if (!MES_SessionCore.ModEnabled) {

				SpawnLogger.Write("Spawner Not Enabled", SpawnerDebugEnum.Spawning);
				return false;

			}

			//Check SpawnGroup
			if (encounter.SpawnGroup == null) {

				SpawnLogger.Write("SpawnGroup Null", SpawnerDebugEnum.Spawning);
				return false;

			}

			//TODO: Clean KPLs and Timeouts
			KnownPlayerLocationManager.CleanExpiredLocations();

			//Generate Environment Object
			var environment = new EnvironmentEvaluation(encounter.TriggerCoords);

			//Get SpawnGroups and Valid Factions
			var spawnGroupCollection = new SpawnGroupCollection();
			spawnGroupCollection.StaticEncounterInstance = encounter;

			if (type == SpawningType.BossEncounter) {

				spawnGroupCollection.InitFromBossEncounter(encounter);

			} else {

				var spawnNames = new List<string>();
				spawnNames.Add(encounter.SpawnGroupName);
				SpawnLogger.SpawnGroup.Clear();
				SpawnGroupManager.GetSpawnGroups(type, "Static Spawn", environment, encounter.Faction, spawnGroupCollection, false, true, spawnNames);

				if (spawnGroupCollection.SpawnGroups.Count == 0) {

					SpawnLogger.Write("Eligible SpawnGroup Count 0", SpawnerDebugEnum.Spawning);
					return false;

				}

				if (!spawnGroupCollection.SelectRandomSpawnGroup(type, environment)) {

					SpawnLogger.Write("Failed To Select Random SpawnGroup", SpawnerDebugEnum.Spawning);
					return false;

				}

			}

			if (spawnGroupCollection.SpawnGroup == null) {

				SpawnLogger.Write("Static/Boss SpawnGroup Null", SpawnerDebugEnum.Spawning);
				return false;

			}

			SpawnLogger.Write("Static/Boss SpawnGroup Selected: " + spawnGroupCollection.SpawnGroup.SpawnGroupName, SpawnerDebugEnum.Spawning);

			//Determine Path or Placement
			var path = PathPlacements.GetStaticSpawnPlacement(type, spawnTypes, spawnGroupCollection, environment, encounter);

			if (!path.ValidPath) {

				SpawnLogger.Write("Static/Boss SpawnGroup Path/Placement Invalid", SpawnerDebugEnum.Spawning);
				return false;

			}

			//Send Request To Prefab Spawner

			SpawnLogger.Write("Attempting Spawn", SpawnerDebugEnum.Spawning);

			var result = PrefabSpawner.ProcessSpawning(spawnGroupCollection, path, environment);

			if (!result)
				return false;

			SpawnLogger.Write("Spawn Successful: " + spawnGroupCollection.SpawnGroup.SpawnGroupName, SpawnerDebugEnum.Spawning);

			PostSpawn(type, path, spawnGroupCollection, environment, null);

			return true;

		}

		public static void PostSpawn(SpawningType type, PathDetails path, SpawnGroupCollection spawnGroupCollection, EnvironmentEvaluation environment, Dictionary<string, DateTime> playerDroneTracker) {

			//Post Spawn Checks
			if (GetPrimarySpawningType(path.SpawnType) == SpawningType.PlanetaryInstallation)
				PrefabSpawner.ApplyInstallationIncrement(spawnGroupCollection, environment);
			
			PrefabSpawner.ApplySpawningCosts(spawnGroupCollection.Conditions, spawnGroupCollection.SelectRandomFaction());

			//Apply Timeout Increases
			TimeoutManagement.ApplySpawnTimeoutToZones(type, path.StartCoords);

			//Apply Unique Encounter
			if (spawnGroupCollection.Conditions.StaticEncounter || spawnGroupCollection.Conditions.UniqueEncounter) {

				NpcManager.UniqueGroupsSpawned.Add(spawnGroupCollection.SpawnGroup.SpawnGroupName);
				SerializationHelper.SaveDataToSandbox<List<string>>("MES-UniqueEncountersSpawned", NpcManager.UniqueGroupsSpawned);
			
			}

			//KPL and Zone Increases
			KnownPlayerLocationManager.IncreaseSpawnCountOfLocations(path.StartCoords, spawnGroupCollection.Faction);

			//SpawnSound
			if (spawnGroupCollection.Conditions != null && spawnGroupCollection.Conditions.PlaySoundAtSpawnTriggerPosition && spawnGroupCollection.Conditions.SpawnTriggerPositionSoundId != null) {

				MyVisualScriptLogicProvider.PlaySingleSoundAtPosition(spawnGroupCollection.Conditions.SpawnTriggerPositionSoundId, environment.Position);
			
			}

			//Player Timers
			if (playerDroneTracker != null && spawnGroupCollection.Conditions.MinimumPlayerTime > 0) {

				var timeAdd = spawnGroupCollection.Conditions.MaximumPlayerTime - spawnGroupCollection.Conditions.MinimumPlayerTime;
				playerDroneTracker[spawnGroupCollection.SpawnGroup.SpawnGroupName] = MyAPIGateway.Session.GameDateTime.Add(new TimeSpan(0,0, timeAdd));

			}

		}

	}

}
