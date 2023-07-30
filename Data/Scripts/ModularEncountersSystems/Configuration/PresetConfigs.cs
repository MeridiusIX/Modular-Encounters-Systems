using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Configuration {
	public static class PresetConfigs {

		public static void DelimitSpawns() {

			Settings.Combat.EnableCombatPhaseSystem = false;

			Settings.Creatures.UseTimeout = false;

			Settings.DroneEncounters.UseTimeout = false;

			Settings.Grids.AerodynamicsModAdvLiftOverride = true;

			Settings.OtherNPCs.UseTimeout = false;

			Settings.PlanetaryCargoShips.UseTimeout = false;

			Settings.PlanetaryInstallations.UseTimeout = false;

			Settings.RandomEncounters.UseTimeout = false;

			Settings.SpaceCargoShips.UseTimeout = false;

		}

		public static void FrequentSpawns() {

			Settings.Creatures.MinCreatureSpawnTime = 300;
			Settings.Creatures.MaxCreatureSpawnTime = 600;

			Settings.PlanetaryCargoShips.MinSpawnTime = 300;
			Settings.PlanetaryCargoShips.MaxSpawnTime = 600;

			Settings.PlanetaryInstallations.PlayerDistanceSpawnTrigger = 2000;

			Settings.SpaceCargoShips.MinSpawnTime = 300;
			Settings.SpaceCargoShips.MaxSpawnTime = 600;


		}

		public static void Suffering() {
		
			
		
		}

	}

}
