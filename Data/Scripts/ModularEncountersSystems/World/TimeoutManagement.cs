using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Spawning;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.World {
	public static class TimeoutManagement {

		public static List<TimeoutZone> Timeouts = new List<TimeoutZone>();

		public static void ApplySpawnTimeoutToZones(SpawningType spawn, Vector3D coords) {

			if (spawn == SpawningType.StaticEncounter)
				return;

			bool appliedToZones = false;

			for (int i = Timeouts.Count - 1; i >= 0; i--) {

				var timeout = Timeouts[i];

				if (timeout.TimeoutType != spawn)
					continue;

				if (Vector3D.Distance(coords, timeout.Coords) > timeout.Radius)
					continue;

				timeout.IncreaseSpawns();
				appliedToZones = true;

			}

			if (appliedToZones)
				return;

			Timeouts.Add(new TimeoutZone(spawn, coords, GetRadius(spawn)));
		
		}

		public static bool IsSpawnAllowed(SpawningType spawn, Vector3D coords) {

			if (!GetEnabled(spawn))
				return true;

			var currentTime = MyAPIGateway.Session.GameDateTime;
			var timeLimit = GetCooldownLimit(spawn);
			var spawnLimit = GetSpawnLimit(spawn);

			lock (Timeouts) {

				for (int i = Timeouts.Count - 1; i >= 0; i--) {

					var timeout = Timeouts[i];

					if (timeout.Remove) {

						Timeouts.RemoveAt(i);
						continue;

					}

					if (timeout.TimeoutType != spawn)
						continue;

					if (Vector3D.Distance(coords, timeout.Coords) > timeout.Radius)
						continue;

					if ((currentTime - timeout.LastSpawnedEncounter).TotalSeconds >= timeLimit) {

						Timeouts.RemoveAt(i);
						continue;

					}

					if (timeout.SpawnedEncounters >= spawnLimit)
						return false;

				}

			}

			return true;
		
		}

		public static int GetCooldownLimit(SpawningType spawn) {

			if (spawn == SpawningType.SpaceCargoShip)
				return Settings.SpaceCargoShips.TimeoutDuration;

			if (spawn == SpawningType.RandomEncounter)
				return Settings.RandomEncounters.TimeoutDuration;

			if (spawn == SpawningType.PlanetaryCargoShip)
				return Settings.PlanetaryCargoShips.TimeoutDuration;

			if (spawn == SpawningType.PlanetaryInstallation)
				return Settings.PlanetaryInstallations.TimeoutDuration;

			if (spawn == SpawningType.BossEncounter)
				return Settings.BossEncounters.TimeoutDuration;

			if (spawn == SpawningType.OtherNPC)
				return Settings.OtherNPCs.TimeoutDuration;

			if (spawn == SpawningType.Creature)
				return Settings.Creatures.TimeoutDuration;

			return 0;
		
		}

		public static int GetSpawnLimit(SpawningType spawn) {

			if (spawn == SpawningType.SpaceCargoShip)
				return Settings.SpaceCargoShips.TimeoutSpawnLimit;

			if (spawn == SpawningType.RandomEncounter)
				return Settings.RandomEncounters.TimeoutSpawnLimit;

			if (spawn == SpawningType.PlanetaryCargoShip)
				return Settings.PlanetaryCargoShips.TimeoutSpawnLimit;

			if (spawn == SpawningType.PlanetaryInstallation)
				return Settings.PlanetaryInstallations.TimeoutSpawnLimit;

			if (spawn == SpawningType.BossEncounter)
				return Settings.BossEncounters.TimeoutSpawnLimit;

			if (spawn == SpawningType.OtherNPC)
				return Settings.OtherNPCs.TimeoutSpawnLimit;

			if (spawn == SpawningType.Creature)
				return Settings.Creatures.TimeoutSpawnLimit;

			return 0;

		}

		public static double GetRadius(SpawningType spawn) {

			if (spawn == SpawningType.SpaceCargoShip)
				return Settings.SpaceCargoShips.TimeoutRadius;

			if (spawn == SpawningType.RandomEncounter)
				return Settings.RandomEncounters.TimeoutRadius;

			if (spawn == SpawningType.PlanetaryCargoShip)
				return Settings.PlanetaryCargoShips.TimeoutRadius;

			if (spawn == SpawningType.PlanetaryInstallation)
				return Settings.PlanetaryInstallations.TimeoutRadius;

			if (spawn == SpawningType.BossEncounter)
				return Settings.BossEncounters.TimeoutRadius;

			if (spawn == SpawningType.OtherNPC)
				return Settings.OtherNPCs.TimeoutRadius;

			if (spawn == SpawningType.Creature)
				return Settings.Creatures.TimeoutRadius;

			return 0;

		}

		public static bool GetEnabled(SpawningType spawn) {

			if (spawn == SpawningType.SpaceCargoShip)
				return Settings.SpaceCargoShips.UseTimeout;

			if (spawn == SpawningType.RandomEncounter)
				return Settings.RandomEncounters.UseTimeout;

			if (spawn == SpawningType.PlanetaryCargoShip)
				return Settings.PlanetaryCargoShips.UseTimeout;

			if (spawn == SpawningType.PlanetaryInstallation)
				return Settings.PlanetaryInstallations.UseTimeout;

			if (spawn == SpawningType.BossEncounter)
				return Settings.BossEncounters.UseTimeout;

			if (spawn == SpawningType.OtherNPC)
				return Settings.OtherNPCs.UseTimeout;

			if (spawn == SpawningType.Creature)
				return Settings.Creatures.UseTimeout;

			return false;

		}

	}

	public class TimeoutZone {

		public SpawningType TimeoutType;
		public int SpawnedEncounters;
		public DateTime LastSpawnedEncounter;
		public Vector3D Coords;
		public double Radius;
		public bool Remove;

		public TimeoutZone() {

			TimeoutType = SpawningType.None;
			SpawnedEncounters = 0;
			LastSpawnedEncounter = MyAPIGateway.Session.GameDateTime;
			Coords = Vector3D.Zero;
			Radius = 0;
			Remove = false;

		}

		public TimeoutZone(SpawningType spawn, Vector3D coords, double radius) {

			TimeoutType = spawn;
			SpawnedEncounters = 1;
			LastSpawnedEncounter = MyAPIGateway.Session.GameDateTime;
			Coords = coords;
			Radius = radius;
			Remove = false;

		}

		public void IncreaseSpawns() {

			SpawnedEncounters++;
			LastSpawnedEncounter = MyAPIGateway.Session.GameDateTime;

		}

		public bool InsideRadius(Vector3D coords) {

			return Vector3D.Distance(coords, Coords) <= TimeoutManagement.GetRadius(TimeoutType);

		}

		public Vector2D TimeoutLength() {

			return new Vector2D((MyAPIGateway.Session.GameDateTime - LastSpawnedEncounter).TotalSeconds, TimeoutManagement.GetCooldownLimit(TimeoutType));
		
		}

		public string GetInfo(Vector3D coords) {

			var sb = new StringBuilder();
			sb.Append(" - [Timeout Zone] ").AppendLine();
			sb.Append("   - Spawn Type:         ").Append(TimeoutType.ToString()).AppendLine();
			var spawnLimit = TimeoutManagement.GetSpawnLimit(TimeoutType);
			sb.Append("   - Spawned Encounters: ").Append(SpawnedEncounters).Append(" / ").Append(spawnLimit).AppendLine();
			sb.Append("   - Restricting Spawns: ").Append(SpawnedEncounters >= spawnLimit).AppendLine();
			var time = TimeoutLength();
			sb.Append("   - Time Remaining:     ").Append((int)time.X).Append(" / ").Append((int)time.Y).AppendLine();
			sb.Append("   - Position In Radius: ").Append(InsideRadius(coords)).AppendLine();
			
			return sb.ToString();

		}

	}

}
