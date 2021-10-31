using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;

namespace ModularEncountersSystems.Configuration {
	public static class Settings {

		public static ConfigGeneral General = new ConfigGeneral();
		public static ConfigGrids Grids = new ConfigGrids();
		public static ConfigSpaceCargoShips SpaceCargoShips = new ConfigSpaceCargoShips();
		public static ConfigPlanetaryCargoShips PlanetaryCargoShips = new ConfigPlanetaryCargoShips();
		public static ConfigRandomEncounters RandomEncounters = new ConfigRandomEncounters();
		public static ConfigPlanetaryInstallations PlanetaryInstallations = new ConfigPlanetaryInstallations();
		public static ConfigBossEncounters BossEncounters = new ConfigBossEncounters();
		public static ConfigOtherNPCs OtherNPCs = new ConfigOtherNPCs();
		public static ConfigCustomBlocks CustomBlocks = new ConfigCustomBlocks();
		public static ConfigCreatures Creatures = new ConfigCreatures();

		public static void InitSettings() {

			if (!MyAPIGateway.Multiplayer.IsServer)
				return;

			General = General.LoadSettings();
			Grids = Grids.LoadSettings();
			SpaceCargoShips = SpaceCargoShips.LoadSettings();
			PlanetaryCargoShips = PlanetaryCargoShips.LoadSettings();
			RandomEncounters = RandomEncounters.LoadSettings();
			PlanetaryInstallations = PlanetaryInstallations.LoadSettings();
			BossEncounters = BossEncounters.LoadSettings();
			OtherNPCs = OtherNPCs.LoadSettings();
			CustomBlocks = CustomBlocks.LoadSettings();
			Creatures = Creatures.LoadSettings();
			CheckGlobalEvents();

			SpaceCargoShips.InitDefinitionDisableList();
			PlanetaryCargoShips.InitDefinitionDisableList();
			RandomEncounters.InitDefinitionDisableList();
			PlanetaryInstallations.InitDefinitionDisableList();
			BossEncounters.InitDefinitionDisableList();
			OtherNPCs.InitDefinitionDisableList();

		}

		public static void CheckGlobalEvents() {

			if (General.UseGlobalEventsTimers == false) {

				SpawnLogger.Write("Global Events Timings Disabled. Using Default Or User Defined Settings.", SpawnerDebugEnum.Startup);
				return;

			}

			var allDefs = MyDefinitionManager.Static.GetAllDefinitions();

			foreach (MyDefinitionBase definition in allDefs.Where(x => x is MyGlobalEventDefinition)) {

				var eventDef = definition as MyGlobalEventDefinition;

				if (eventDef.Id.SubtypeId.ToString() == "SpawnCargoShip") {

					SpawnLogger.Write("Using Spawner Timings From Global Events For Space/Lunar Cargo Ships.", SpawnerDebugEnum.Startup);

					if (eventDef.FirstActivationTime != null) {

						var span = (TimeSpan)eventDef.FirstActivationTime;
						SpaceCargoShips.FirstSpawnTime = (int)span.TotalSeconds;

					}

					if (eventDef.MinActivationTime != null) {

						var span = (TimeSpan)eventDef.MinActivationTime;
						SpaceCargoShips.MinSpawnTime = (int)span.TotalSeconds;

					}

					if (eventDef.MaxActivationTime != null) {

						var span = (TimeSpan)eventDef.MaxActivationTime;
						SpaceCargoShips.MaxSpawnTime = (int)span.TotalSeconds;

					}

				}

				if (eventDef.Id.SubtypeId.ToString() == "SpawnRandomEncounter") {

					SpawnLogger.Write("Using Spawner Timings From Global Events For Random Encounters.", SpawnerDebugEnum.Startup);

					if (eventDef.FirstActivationTime != null) {

						var span = (TimeSpan)eventDef.FirstActivationTime;
						//RandomEncounters.FirstSpawnTime = (int)span.TotalSeconds;

					}

					if (eventDef.MinActivationTime != null) {

						var span = (TimeSpan)eventDef.MinActivationTime;
						//RandomEncounters.MinSpawnTime = (int)span.TotalSeconds;

					}

					if (eventDef.MaxActivationTime != null) {

						var span = (TimeSpan)eventDef.MaxActivationTime;
						//RandomEncounters.MaxSpawnTime = (int)span.TotalSeconds;

					}

				}

				if (eventDef.Id.SubtypeId.ToString() == "SpawnBossEncounter") {

					SpawnLogger.Write("Using Spawner Timings From Global Events For Boss Encounters.", SpawnerDebugEnum.Startup);

					if (eventDef.FirstActivationTime != null) {

						var span = (TimeSpan)eventDef.FirstActivationTime;
						//BossEncounters.FirstSpawnTime = (int)span.TotalSeconds;

					}

					if (eventDef.MinActivationTime != null) {

						var span = (TimeSpan)eventDef.MinActivationTime;
						//BossEncounters.MinSpawnTime = (int)span.TotalSeconds;

					}

					if (eventDef.MaxActivationTime != null) {

						var span = (TimeSpan)eventDef.MaxActivationTime;
						//BossEncounters.MaxSpawnTime = (int)span.TotalSeconds;

					}

				}

				if (eventDef.Id.SubtypeId.ToString() == "SpawnAtmoCargoShip") {

					SpawnLogger.Write("Using Spawner Timings From Global Events For Planetary Cargo Ships.", SpawnerDebugEnum.Startup);

					if (eventDef.FirstActivationTime != null) {

						var span = (TimeSpan)eventDef.FirstActivationTime;
						PlanetaryCargoShips.FirstSpawnTime = (int)span.TotalSeconds;

					}

					if (eventDef.MinActivationTime != null) {

						var span = (TimeSpan)eventDef.MinActivationTime;
						PlanetaryCargoShips.MinSpawnTime = (int)span.TotalSeconds;

					}

					if (eventDef.MaxActivationTime != null) {

						var span = (TimeSpan)eventDef.MaxActivationTime;
						PlanetaryCargoShips.MaxSpawnTime = (int)span.TotalSeconds;

					}

				}

				if (eventDef.Id.SubtypeId.ToString() == "SpawnPlanetaryCargoShip") {

					SpawnLogger.Write("Using Spawner Timings From Global Events For Planetary Installations.", SpawnerDebugEnum.Startup);

					if (eventDef.FirstActivationTime != null) {

						var span = (TimeSpan)eventDef.FirstActivationTime;
						//PlanetaryInstallations.FirstSpawnTime = (int)span.TotalSeconds;

					}

					if (eventDef.MinActivationTime != null) {

						var span = (TimeSpan)eventDef.MinActivationTime;
						//PlanetaryInstallations.MinSpawnTime = (int)span.TotalSeconds;

					}

					if (eventDef.MaxActivationTime != null) {

						var span = (TimeSpan)eventDef.MaxActivationTime;
						//PlanetaryInstallations.MaxSpawnTime = (int)span.TotalSeconds;

					}

				}

				


			}

		}

		public static double GetSpawnAreaRadius(SpawningType type) {

			if (type == SpawningType.SpaceCargoShip)
				return SpaceCargoShips.AreaSize;

			if (type == SpawningType.RandomEncounter)
				return RandomEncounters.AreaSize;

			if (type == SpawningType.PlanetaryCargoShip)
				return PlanetaryCargoShips.AreaSize;

			if (type == SpawningType.PlanetaryInstallation)
				return PlanetaryInstallations.AreaSize;

			if (type == SpawningType.BossEncounter)
				return BossEncounters.AreaSize;

			return -1;
		
		}

		public static int GetMaxAreaSpawns(SpawningType type) {

			if (type == SpawningType.SpaceCargoShip)
				return SpaceCargoShips.MaxShipsPerArea;

			if (type == SpawningType.RandomEncounter)
				return RandomEncounters.MaxShipsPerArea;

			if (type == SpawningType.PlanetaryCargoShip)
				return PlanetaryCargoShips.MaxShipsPerArea;

			if (type == SpawningType.PlanetaryInstallation)
				return PlanetaryInstallations.MaxShipsPerArea;

			if (type == SpawningType.BossEncounter)
				return BossEncounters.MaxShipsPerArea;

			return -1;

		}

		public static string[] GetSpawnTypeBlacklist(SpawningType type) {

			if (type == SpawningType.SpaceCargoShip)
				return SpaceCargoShips.SpawnTypeBlacklist;

			if (type == SpawningType.RandomEncounter)
				return RandomEncounters.SpawnTypeBlacklist;

			if (type == SpawningType.PlanetaryCargoShip)
				return PlanetaryCargoShips.SpawnTypeBlacklist;

			if (type == SpawningType.PlanetaryInstallation)
				return PlanetaryInstallations.SpawnTypeBlacklist;

			if (type == SpawningType.BossEncounter)
				return BossEncounters.SpawnTypeBlacklist;

			if (type == SpawningType.Creature)
				return Creatures.SpawnTypeBlacklist;

			return new string[] { };

		}

		public static string[] GetSpawnTypePlanetBlacklist(SpawningType type) {

			if (type == SpawningType.SpaceCargoShip)
				return SpaceCargoShips.SpawnTypePlanetBlacklist;

			if (type == SpawningType.RandomEncounter)
				return RandomEncounters.SpawnTypePlanetBlacklist;

			if (type == SpawningType.PlanetaryCargoShip)
				return PlanetaryCargoShips.SpawnTypePlanetBlacklist;

			if (type == SpawningType.PlanetaryInstallation)
				return PlanetaryInstallations.SpawnTypePlanetBlacklist;

			if (type == SpawningType.BossEncounter)
				return BossEncounters.SpawnTypePlanetBlacklist;

			if(type == SpawningType.Creature)
				return Creatures.SpawnTypePlanetBlacklist;

			return new string[] { };

		}

		public static ConfigBase GetConfig(SpawningType providedType) {

			var type = SpawnRequest.GetPrimarySpawningType(providedType);

			if (type == SpawningType.SpaceCargoShip)
				return SpaceCargoShips;

			if (type == SpawningType.RandomEncounter)
				return RandomEncounters;

			if (type == SpawningType.PlanetaryCargoShip)
				return PlanetaryCargoShips;

			if (type == SpawningType.PlanetaryInstallation)
				return PlanetaryInstallations;

			if (type == SpawningType.BossEncounter)
				return BossEncounters;

			if (type == SpawningType.OtherNPC || type == SpawningType.DroneEncounter || type == SpawningType.None)
				return OtherNPCs;

			return null;

		}

		public static float GetFrequencyLimit(SpawningType type) {

			if (type == SpawningType.SpaceCargoShip && SpaceCargoShips.UseMaxSpawnGroupFrequency)
				return SpaceCargoShips.MaxSpawnGroupFrequency;

			if (type == SpawningType.RandomEncounter && RandomEncounters.UseMaxSpawnGroupFrequency)
				return RandomEncounters.MaxSpawnGroupFrequency;

			if (type == SpawningType.PlanetaryCargoShip && PlanetaryCargoShips.UseMaxSpawnGroupFrequency)
				return PlanetaryCargoShips.MaxSpawnGroupFrequency;

			if (type == SpawningType.PlanetaryInstallation && PlanetaryInstallations.UseMaxSpawnGroupFrequency)
				return PlanetaryInstallations.MaxSpawnGroupFrequency;

			if (type == SpawningType.BossEncounter && BossEncounters.UseMaxSpawnGroupFrequency)
				return BossEncounters.MaxSpawnGroupFrequency;

			return -1;

		}

	}

}
