using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Spawning {
	public static class WaveManager {

		public static WaveSpawner Space;

		public static WaveSpawner Planet;

		public static WaveSpawner Creature;

		public static void Setup() {

			Space = new WaveSpawner(SpawningType.SpaceCargoShip);
			Planet = new WaveSpawner(SpawningType.PlanetaryCargoShip);
			Creature = new WaveSpawner(SpawningType.Creature);

		}

		public static void Run() {

			Space.ProcessWaveSpawner();
			Planet.ProcessWaveSpawner();
			Creature.ProcessWaveSpawner();

		}


	}
}
