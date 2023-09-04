using ModularEncountersSystems.Core;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Spawning.Profiles;
using ModularEncountersSystems.Tasks;
using ModularEncountersSystems.Watchers;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.World {
	public static class NpcManager {

		public static List<GridEntity> ActiveNpcs = new List<GridEntity>();
		public static List<StaticEncounter> StaticEncounters = new List<StaticEncounter>();
		public static List<string> UniqueGroupsSpawned = new List<string>();

		public static List<NpcData> SpawnedNpcData = new List<NpcData>();
		public static int SpawnIncrement { get { _spawnIncrement++; return _spawnIncrement; } }
		private static int _spawnIncrement = 0;

		public static Dictionary<IMyRemoteControl, string> RemoteControlCodes = new Dictionary<IMyRemoteControl, string>();

		public static Action<IMyCubeGrid, string> DespawnSource;

		public static void Setup() {

			SetupUniqueEncounters();
			SetupStaticEncounters();
			MES_SessionCore.UnloadActions += Unload;
		
		}

		public static void SetupExistingNpcGrids() {

			
		
		}

		public static void SetupUniqueEncounters() {

			//Get Unique Encounters
			var stringData = "";

			if (MyAPIGateway.Utilities.GetVariable<string>("MES-UniqueEncountersSpawned", out stringData)) {

				var data = Convert.FromBase64String(stringData);

				if (data != null) {

					UniqueGroupsSpawned = MyAPIGateway.Utilities.SerializeFromBinary<List<string>>(data);

					if (UniqueGroupsSpawned == null) {

						UniqueGroupsSpawned = new List<string>();

					}

				}

			}

		}

		public static void SetupStaticEncounters() {

			//Get Static Encounters
			var stringData = "";
			bool updateEncounters = false;

			if (MyAPIGateway.Utilities.GetVariable<string>("MES-ActiveStaticEncounters", out stringData)) {

				var data = Convert.FromBase64String(stringData);

				if (data != null) {

					StaticEncounters = MyAPIGateway.Utilities.SerializeFromBinary<List<StaticEncounter>>(data);

					if (StaticEncounters == null) {

						StaticEncounters = new List<StaticEncounter>();

					}

				}

			}

			//Compare Against Existing SpawnGroups
			for (int i = StaticEncounters.Count - 1; i >= 0; i--) {

				var encounter = StaticEncounters[i];
				var gotSpawnGroup = false;
				var planetMissing = false;

				

				foreach (var spawnGroup in SpawnGroupManager.SpawnGroups) {

					if (spawnGroup.SpawnGroupName != encounter.SpawnGroupName)
						continue;

					foreach (var condition in spawnGroup.SpawnConditionsProfiles) {

						if (condition.StaticEncounter && encounter.PlanetEntityId > 0) {

							var planet = PlanetManager.GetPlanetWithId(encounter.PlanetEntityId);

							if (planet?.Planet == null || planet.Planet.EntityId != encounter.PlanetEntityId) {

								planetMissing = true;

							}
						
						}

						if (condition.StaticEncounter || condition.BossEncounterAny || condition.BossEncounterAtmo || condition.BossEncounterSpace) {

							gotSpawnGroup = true;

							//TODO: Compare Existing Properties Against Current SpawnGroup
							//Will Need New Method To Do This

							break;

						}

					}

					if (gotSpawnGroup)
						break;

				}

				if (!gotSpawnGroup || planetMissing) {

					updateEncounters = true;
					UniqueGroupsSpawned.Remove(encounter.SpawnGroupName);
					StaticEncounters.RemoveAt(i);

				}

			}

			LoadNewStaticEncounters(ref updateEncounters);

			if (updateEncounters)
				UpdateStaticEncounters();

			TaskProcessor.Tick60.Tasks += CheckStaticEncounters;

		}

		public static void LoadNewStaticEncounters(ref bool updateEncounters) {

			//TODO: Check For New Static SpawnGroups
			for (int i = SpawnGroupManager.SpawnGroups.Count - 1; i >= 0; i--) {

				var spawnGroup = SpawnGroupManager.SpawnGroups[i];

				if (string.IsNullOrWhiteSpace(spawnGroup.SpawnGroupName)) {

					SpawnLogger.Write("Null or Blank SpawnGroupName Found While Checking For Static Encounters", SpawnerDebugEnum.Startup);
					continue;

				}

				SpawnConditionsProfile activeConditions = null;

				var conditionValid = false;

				if (UniqueGroupsSpawned.Contains(spawnGroup.SpawnGroupName)) {

					SpawnLogger.Write(spawnGroup.SpawnGroupName + " Found in Unique Spawned Encounters While Checking For Static Encounters", SpawnerDebugEnum.Startup);
					continue;

				}


				foreach (var condition in spawnGroup.SpawnConditionsProfiles) {

					if (condition.StaticEncounter) {

						SpawnLogger.Write(spawnGroup.SpawnGroupName + " Found as Potential Static Encounter", SpawnerDebugEnum.Startup);
						activeConditions = condition;
						conditionValid = true;
						break;

					}

				}

				if (!conditionValid)
					continue;

				var gotSpawnGroup = false;

				foreach (var encounter in StaticEncounters) {

					if (encounter.SpawnGroupName == spawnGroup.SpawnGroupName) {

						SpawnLogger.Write(spawnGroup.SpawnGroupName + " Exists already in world as Static Encounter", SpawnerDebugEnum.Startup);
						gotSpawnGroup = true;
						break;

					}

				}

				if (gotSpawnGroup)
					continue;

				//Create Static Encounter
				var activeEncounter = new StaticEncounter();

				activeEncounter.InitStaticEncounter(spawnGroup, activeConditions);

				if (activeEncounter.IsValid) {

					SpawnLogger.Write("Adding Static Encounter: " + (!string.IsNullOrWhiteSpace(activeEncounter.SpawnGroupName) ? activeEncounter.SpawnGroupName : "(invalid)"), SpawnerDebugEnum.Startup);
					StaticEncounters.Add(activeEncounter);
					updateEncounters = true;

				} else {

					SpawnLogger.Write(spawnGroup.SpawnGroupName + " Static Encounter Init Failed", SpawnerDebugEnum.Startup);

				}

			}

		}

		public static void CheckStaticEncounters() {

			bool updateStatics = false;

			//Static Encounters
			for (int i = StaticEncounters.Count - 1; i >= 0; i--) {

				var encounter = StaticEncounters[i];
				encounter.ProcessEncounter(ref updateStatics);

				if (!encounter.IsValid) {

					StaticEncounters.RemoveAt(i);
					updateStatics = true;

				}


			}

			if (updateStatics)
				UpdateStaticEncounters();

		}

		public static void ResetThisResetThisStaticEncounter(string spawnGroupName)
		{
			if (NpcManager.UniqueGroupsSpawned.Contains(spawnGroupName))
			{
				NpcManager.UniqueGroupsSpawned.Remove(spawnGroupName);


				for (int i = SpawnGroupManager.SpawnGroups.Count - 1; i >= 0; i--)
				{

					var spawnGroup = SpawnGroupManager.SpawnGroups[i];
					if (spawnGroup.SpawnGroupName == spawnGroupName)
					{
						SpawnConditionsProfile activeConditions = null;

						foreach (var condition in spawnGroup.SpawnConditionsProfiles)
						{

							if (condition.StaticEncounter)
							{

								SpawnLogger.Write(spawnGroup.SpawnGroupName + " Found as Potential Static Encounter", SpawnerDebugEnum.Startup);
								activeConditions = condition;
							}
							else
								return; //prevent crash

						}

						//Create Static Encounter
						var activeEncounter = new StaticEncounter();

						activeEncounter.InitStaticEncounter(spawnGroup, activeConditions);

						if (activeEncounter.IsValid)
						{

							SpawnLogger.Write("Adding Static Encounter: " + (!string.IsNullOrWhiteSpace(activeEncounter.SpawnGroupName) ? activeEncounter.SpawnGroupName : "(invalid)"), SpawnerDebugEnum.Startup);
							NpcManager.StaticEncounters.Add(activeEncounter);

						}
						else
						{

							SpawnLogger.Write(spawnGroup.SpawnGroupName + " Static Encounter Init Failed", SpawnerDebugEnum.Startup);
						}

					}

				}

				UpdateStaticEncounters();
				//MyVisualScriptLogicProvider.ShowNotificationToAll($"{spawnGroupName} despawned, and reset", 5000, "Red");
			}

		}






		public static void UpdateStaticEncounters() {

			SerializationHelper.SaveDataToSandbox<List<StaticEncounter>>("MES-ActiveStaticEncounters", StaticEncounters);
			SerializationHelper.SaveDataToSandbox<List<string>>("MES-UniqueEncountersSpawned", UniqueGroupsSpawned);

		}

		public static string GetActiveNpcData() {

			var sb = new StringBuilder();
			sb.Append("::: Active NPC Data :::").AppendLine().AppendLine();
			sb.Append("Total Npc Grids: ").Append(ActiveNpcs.Count).AppendLine().AppendLine();

			foreach (var grid in ActiveNpcs) {

				if (!grid.ActiveEntity())
					continue;

				sb.Append("Name:                   ").Append(grid.CubeGrid.CustomName).AppendLine();
				sb.Append("Type:                   ").Append(grid.Npc.SpawnType).AppendLine();
				sb.Append("Start Coords:           ").Append(grid.Npc.StartCoords).AppendLine();
				sb.Append("End Coords:             ").Append(grid.Npc.EndCoords).AppendLine();
				sb.Append("Path Distance:          ").Append(Vector3D.Distance(grid.Npc.StartCoords, grid.Npc.EndCoords)).AppendLine();
				sb.Append("Distance From Start:    ").Append(Vector3D.Distance(grid.Npc.StartCoords, grid.GetPosition())).AppendLine();
				sb.Append("Distance To End:        ").Append(Vector3D.Distance(grid.Npc.EndCoords, grid.GetPosition())).AppendLine();
				sb.Append("Is Drifting Cargo Ship: ").Append(CargoShipWatcher.CargoShips.Contains(grid)).AppendLine();
				sb.Append("Ownership:              ").AppendLine();


				if (grid.Ownership != GridOwnershipEnum.None) {

					if (grid.Ownership.HasFlag(GridOwnershipEnum.NpcMajority))
						sb.Append(" - Npc Majority").AppendLine();

					if (grid.Ownership.HasFlag(GridOwnershipEnum.NpcMinority))
						sb.Append(" - Npc Minority").AppendLine();

					if (grid.Ownership.HasFlag(GridOwnershipEnum.PlayerMajority))
						sb.Append(" - Player Majority").AppendLine();

					if (grid.Ownership.HasFlag(GridOwnershipEnum.PlayerMinority))
						sb.Append(" - Player Minority").AppendLine();

				} else {

					sb.Append(" - No Ownership").AppendLine();

				}

				sb.AppendLine();

			}

			return sb.ToString();
		
		}

		public static int GetGlobalNpcCount() {

			int totalNPCs = 0;

			foreach (var npc in ActiveNpcs) {

				if (!npc.ActiveEntity())
					continue;

				if (npc.Ownership == GridOwnershipEnum.NpcMajority)
					totalNPCs++;
			
			}

			return totalNPCs;

		}

		public static int GetAreaNpcCount(SpawningType type, Vector3D coords, double radius) {

			int totalNPCs = 0;

			foreach (var npc in ActiveNpcs) {

				if (!npc.ActiveEntity())
					continue;

				if (SpawnRequest.GetPrimarySpawningType(npc.Npc.SpawnType) != type)
					continue;

				if (npc.Ownership.HasFlag(GridOwnershipEnum.PlayerMajority) || npc.Ownership.HasFlag(GridOwnershipEnum.PlayerMinority))
					continue;

				if (npc.Distance(coords) > radius)
					continue;

				totalNPCs++;

			}

			return totalNPCs;

		}

		public static GridEntity GetNpcFromGrid(IMyCubeGrid grid) {

			var gridEnt = GridManager.GetGridEntity(grid);

			if (gridEnt == null)
				return null;

			if (ActiveNpcs.Contains(gridEnt))
				return gridEnt;

			return null;
		
		}

		public static void OwnershipMajorityChange(GridEntity grid) {

			if (!grid.ActiveEntity()) {

				ActiveNpcs.Remove(grid);
				grid.OwnershipMajorityChange -= OwnershipMajorityChange;
				return;

			}

			if (!grid.Ownership.HasFlag(GridOwnershipEnum.NpcMajority) && !grid.Ownership.HasFlag(GridOwnershipEnum.NpcMinority) && grid.Ownership != GridOwnershipEnum.None) {

				ActiveNpcs.Remove(grid);
				grid.OwnershipMajorityChange -= OwnershipMajorityChange;

			}
		
		}

		public static bool RegisterDespawnWatcher(IMyCubeGrid cubeGrid, Action<IMyCubeGrid, string> action) {

			lock (ActiveNpcs) {

				foreach (var npc in ActiveNpcs) {

					if (!npc.ActiveEntity() || npc.CubeGrid != cubeGrid)
						continue;

					if (npc.Npc.DespawnActions == null)
						npc.Npc.DespawnActions = new List<Action<IMyCubeGrid, string>>();

					npc.Npc.DespawnActions.Add(action);
					return true;

				}

			}

			return false;

		}

		public static void RegisterRemoteControlCode(IMyRemoteControl remoteControl, string code) {

			lock (RemoteControlCodes) {

				if (remoteControl == null || RemoteControlCodes.ContainsKey(remoteControl))
					return;

				RemoteControlCodes.Add(remoteControl, code);

			}

		}

		public static void Unload() {

			TaskProcessor.Tick60.Tasks -= CheckStaticEncounters;

		}

	}
}
