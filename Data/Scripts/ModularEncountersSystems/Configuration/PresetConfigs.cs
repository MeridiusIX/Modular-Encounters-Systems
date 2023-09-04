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
			Settings.PlanetaryCargoShips.MaxShipsPerArea = 20;

			Settings.PlanetaryInstallations.UseTimeout = false;
			Settings.PlanetaryInstallations.MaxShipsPerArea = 20;

			Settings.RandomEncounters.UseTimeout = false;
			Settings.RandomEncounters.MaxShipsPerArea = 20;

			Settings.SpaceCargoShips.UseTimeout = false;
			Settings.SpaceCargoShips.MaxShipsPerArea = 20;

		}

		public static void FrequentSpawns() {

			Settings.Creatures.MinCreatureSpawnTime = 300;
			Settings.Creatures.MaxCreatureSpawnTime = 420;

			Settings.PlanetaryCargoShips.MinSpawnTime = 300;
			Settings.PlanetaryCargoShips.MaxSpawnTime = 420;

			Settings.PlanetaryInstallations.PlayerDistanceSpawnTrigger = 2000;
			Settings.PlanetaryInstallations.PlayerSpawnCooldown = 60;

			Settings.SpaceCargoShips.MinSpawnTime = 300;
			Settings.SpaceCargoShips.MaxSpawnTime = 420;

			Settings.RandomEncounters.PlayerTravelDistance = 4000;
			Settings.RandomEncounters.PlayerSpawnCooldown = 60;

		}

		public static void Suffering() {

			Settings.General.ThreatReductionHandicap = -5000;
			
		}

	}

}
