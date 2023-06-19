using ModularEncountersSystems.API;
using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Tasks;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRageMath;

namespace ModularEncountersSystems.Spawning {

	[ProtoContract]
	public class WaveSpawner {

		[ProtoMember(1)]
		public SpawningType SpawnType;

		[ProtoMember(2)]
		public int Timer;

		[ProtoMember(3)]
		public int TimerTrigger;

		[ProtoIgnore]
		public bool Active;

		[ProtoIgnore]
		public int NextSpawnTimer;

		[ProtoIgnore]
		public int NextSpawnTimerTrigger;

		[ProtoIgnore]
		public int MaxSpawnsPerCluster;

		[ProtoIgnore]
		public List<string> SpecificGroups;

		[ProtoIgnore]
		public List<Vector3D> WaveClusterPositions;

		[ProtoIgnore]
		public List<int> WaveClusterCounts;

		[ProtoIgnore]
		public double PlayerClusterRadius;

		public WaveSpawner() {

			Timer = 0;
			TimerTrigger = GetNewTimerTrigger();

			Active = false;
			NextSpawnTimer = 0;
			NextSpawnTimerTrigger = 0;
			SpecificGroups = new List<string>();
			WaveClusterPositions = new List<Vector3D>();
			WaveClusterCounts = new List<int>();

		}

		public WaveSpawner(SpawningType type) {

			SpawnType = type;
			Timer = 0;
			TimerTrigger = GetNewTimerTrigger();

			Active = false;
			NextSpawnTimer = 0;
			NextSpawnTimerTrigger = 0;
			SpecificGroups = new List<string>();
			WaveClusterPositions = new List<Vector3D>();
			WaveClusterCounts = new List<int>();

			LoadData();

		}

		private void LoadData() {

			//SpawnLogger.Write("Loading Data", SpawnerDebugEnum.Startup);
			var newData = SerializationHelper.GetDataFromSandbox<WaveSpawner>("MES-WaveSpawner-" + SpawnType.ToString());
		   // SpawnLogger.Write("Loaded Data", SpawnerDebugEnum.Startup);

			if (newData != null) {

				if (SpawnType == SpawningType.SpaceCargoShip)
					WaveManager.Space = newData;

				if (SpawnType == SpawningType.PlanetaryCargoShip)
					WaveManager.Planet = newData;

				if (SpawnType == SpawningType.Creature)
					WaveManager.Creature = newData;

			} else {

				//SpawnLogger.Write("Saving Data", SpawnerDebugEnum.Startup);
				SerializationHelper.SaveDataToSandbox<WaveSpawner>("MES-WaveSpawner-" + SpawnType.ToString(), this);
			   // SpawnLogger.Write("Saved Data", SpawnerDebugEnum.Startup);

			}
		
		}

		public void SaveData() {

		   //SpawnLogger.Write("Saving Data", SpawnerDebugEnum.Startup);
			SerializationHelper.SaveDataToSandbox<WaveSpawner>("MES-WaveSpawner-" + SpawnType.ToString(), this);
		   // SpawnLogger.Write("Saved Data", SpawnerDebugEnum.Startup);

		}

		public void ProcessWaveSpawner(bool overrideCommand = false) {

			if (!Allowed() && !overrideCommand)
				return;

			if (!Active) {

				Timer += Settings.General.PlayerWatcherTimerTrigger;

				if (Timer >= TimerTrigger) {

					Active = true;
					Timer = 0;
					TimerTrigger = GetNewTimerTrigger();

					NextSpawnTimer = 0;
					NextSpawnTimerTrigger = GetNextSpawnTimerTrigger();
					MaxSpawnsPerCluster = GetMaxSpawnsPerCluster();
					GetSpecificSpawns();
					WaveClusterPositions.Clear();
					WaveClusterCounts.Clear();

					var clusterDist = GetPlayerClusterDistance();

					foreach (var player in PlayerManager.Players) {

						if (!player.ActiveEntity()) {

							continue;

						}

						bool tooClose = false;

						foreach (var item in WaveClusterPositions) {

							if (Vector3D.Distance(item, player.GetPosition()) < clusterDist) {

								tooClose = true;
								break;

							}

						}

						if (tooClose)
							continue;

						WaveClusterPositions.Add(player.GetPosition());
						WaveClusterCounts.Add(0);

					}

					TaskProcessor.Tick60.Tasks += ActiveSpawnerProcessing;

				}

			}

			//Save Data
			SaveData();

		}

		public void ActiveSpawnerProcessing() {

			NextSpawnTimer++;

			if (NextSpawnTimer < NextSpawnTimerTrigger)
				return;

			NextSpawnTimer = 0;

			for (int i = WaveClusterPositions.Count - 1; i >= 0; i--) {

				SpawnRequest.CalculateSpawn(WaveClusterPositions[i], "Wave Spawner", SpawnType, false, true, SpecificGroups.Count > 0 ? SpecificGroups : null);
				WaveClusterCounts[i]++;

				if (WaveClusterCounts[i] >= MaxSpawnsPerCluster) {

					WaveClusterPositions.RemoveAt(i);
					WaveClusterCounts.RemoveAt(i);

				}

				break;

			}

			if (WaveClusterPositions.Count == 0) {

				Active = false;
				TaskProcessor.Tick60.Tasks -= ActiveSpawnerProcessing;

			}

		}

		private bool Allowed() {

			if (SpawnType == SpawningType.SpaceCargoShip)
				return Settings.SpaceCargoShips.EnableWaveSpawner || AddonManager.SpaceWaveSpawner;

			if (SpawnType == SpawningType.PlanetaryCargoShip)
				return Settings.PlanetaryCargoShips.EnableWaveSpawner; 

			if (SpawnType == SpawningType.Creature)
				return Settings.Creatures.EnableWaveSpawner;

			return false;

		}

		private int GetNewTimerTrigger() {

			if (SpawnType == SpawningType.SpaceCargoShip)
				return MathTools.RandomBetween(Settings.SpaceCargoShips.MinWaveSpawnTime, Settings.SpaceCargoShips.MaxWaveSpawnTime);

			if (SpawnType == SpawningType.PlanetaryCargoShip)
				return MathTools.RandomBetween(Settings.PlanetaryCargoShips.MinWaveSpawnTime, Settings.PlanetaryCargoShips.MaxWaveSpawnTime);

			if (SpawnType == SpawningType.Creature)
				return MathTools.RandomBetween(Settings.Creatures.MinWaveSpawnTime, Settings.Creatures.MaxWaveSpawnTime);

			return 999999;

		}

		private int GetNextSpawnTimerTrigger() {

			if (SpawnType == SpawningType.SpaceCargoShip)
				return Settings.SpaceCargoShips.TimeBetweenWaveSpawns;

			if (SpawnType == SpawningType.PlanetaryCargoShip)
				return Settings.PlanetaryCargoShips.TimeBetweenWaveSpawns;

			if (SpawnType == SpawningType.Creature)
				return Settings.Creatures.TimeBetweenWaveSpawns;

			return 999999;

		}

		private int GetMaxSpawnsPerCluster() {

			if (SpawnType == SpawningType.SpaceCargoShip)
				return Settings.SpaceCargoShips.TotalSpawnEventsPerCluster;

			if (SpawnType == SpawningType.PlanetaryCargoShip)
				return Settings.PlanetaryCargoShips.TotalSpawnEventsPerCluster;

			if (SpawnType == SpawningType.Creature)
				return Settings.Creatures.TotalSpawnEventsPerCluster;

			return 999999;

		}

		private double GetPlayerClusterDistance() {

			if (SpawnType == SpawningType.SpaceCargoShip)
				return Settings.SpaceCargoShips.PlayerClusterDistance;

			if (SpawnType == SpawningType.PlanetaryCargoShip)
				return Settings.PlanetaryCargoShips.PlayerClusterDistance;

			if (SpawnType == SpawningType.Creature)
				return Settings.Creatures.PlayerClusterDistance;

			return 999999;

		}

		private void GetSpecificSpawns() {

			string[] list = null;

			if (SpawnType == SpawningType.SpaceCargoShip)
				list = Settings.SpaceCargoShips.UseSpecificRandomGroups;

			if (SpawnType == SpawningType.PlanetaryCargoShip)
				list = Settings.PlanetaryCargoShips.UseSpecificRandomGroups;

			if (SpawnType == SpawningType.Creature)
				list = Settings.Creatures.UseSpecificRandomGroups;

			SpecificGroups.Clear();

			foreach (var group in list) {

				if (string.IsNullOrWhiteSpace(group))
					continue;

				if (group == "SomeSpawnGroupNameHere" || group == "AnotherSpawnGroupNameHere" || group == "EtcEtcEtc")
					continue;

				SpecificGroups.Add(group);
			
			}

		}

		public override string ToString() {

			var sb = new StringBuilder();
			sb.Append("Enabled:               ").Append(Allowed().ToString()).AppendLine();
			sb.Append("Active:                ").Append(Active.ToString()).AppendLine();
			sb.Append("Timer:                 ").Append(Timer.ToString()).AppendLine();
			sb.Append("TimerTrigger:          ").Append(TimerTrigger.ToString()).AppendLine();
			sb.Append("NextSpawnTimer:        ").Append(NextSpawnTimer.ToString()).AppendLine();
			sb.Append("NextSpawnTimerTrigger: ").Append(NextSpawnTimerTrigger.ToString()).AppendLine();
			sb.Append("MaxSpawnsPerCluster:   ").Append(MaxSpawnsPerCluster.ToString()).AppendLine();

			return sb.ToString();
			
		}

	}

}
