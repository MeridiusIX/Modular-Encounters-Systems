using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.World;
using ModularEncountersSystems.Zones;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.API {
	public static class LocalApi {

		public static void SendApiToMods() {

			//Create a Dictionary of delegates that point to methods in the MES code.
			//Send the Dictionary To Other Mods That Registered To This ID in LoadData()
			MyAPIGateway.Utilities.SendModMessage(1521905890, GetApiDictionary());

		}

		public static Dictionary<string, Delegate> GetApiDictionary() {

			var dict = new Dictionary<string, Delegate>();
			dict.Add("AddKnownPlayerLocation", new Action<Vector3D, string, double, int, int, int>(KnownPlayerLocationManager.AddKnownPlayerLocation));
			dict.Add("CustomSpawnRequest", new Func<List<string>, MatrixD, Vector3, bool, string, string, bool>(CustomSpawnRequest));
			dict.Add("GetDespawnCoords", new Func<IMyCubeGrid, Vector3D>(GetDespawnCoords));
			dict.Add("GetSpawnGroupBlackList", new Func<List<string>>(GetSpawnGroupBlackList));
			dict.Add("GetNpcNameBlackList", new Func<List<string>>(GetNpcNameBlackList));
			dict.Add("IsPositionInKnownPlayerLocation", new Func<Vector3D, bool, string, bool>(KnownPlayerLocationManager.IsPositionInKnownPlayerLocation));
			dict.Add("ConvertRandomNamePatterns", new Func<string, string>(RandomNameGenerator.CreateRandomNameFromPattern));
			dict.Add("GetNpcStartCoordinates", new Func<IMyCubeGrid, Vector3D>(GetNpcStartCoordinates));
			dict.Add("GetNpcEndCoordinates", new Func<IMyCubeGrid, Vector3D>(GetNpcEndCoordinates));
			dict.Add("RegisterDespawnWatcher", new Func<IMyCubeGrid, Action<IMyCubeGrid, string>, bool>(RegisterDespawnWatcher));
			dict.Add("RegisterRemoteControlCode", new Action<IMyRemoteControl, string>(RegisterRemoteControlCode));
			dict.Add("RemoveKnownPlayerLocation", new Action<Vector3D, string, bool>(KnownPlayerLocationManager.RemoveLocation));
			dict.Add("SetSpawnerIgnoreForDespawn", new Func<IMyCubeGrid, bool, bool>(SetSpawnerIgnoreForDespawn));
			dict.Add("SpawnBossEncounter", new Func<Vector3D, List<string>, bool>(SpawnBossEncounter));
			dict.Add("SpawnPlanetaryCargoShip", new Func<Vector3D, List<string>, bool>(SpawnPlanetaryCargoShip));
			dict.Add("SpawnPlanetaryInstallation", new Func<Vector3D, List<string>, bool>(SpawnPlanetaryInstallation));
			dict.Add("SpawnRandomEncounter", new Func<Vector3D, List<string>, bool>(SpawnRandomEncounter));
			dict.Add("SpawnSpaceCargoShip", new Func<Vector3D, List<string>, bool>(SpawnSpaceCargoShip));

			return dict;

		}

		//CustomSpawnRequest
		public static bool CustomSpawnRequest(List<string> spawnGroups, MatrixD spawningMatrix, Vector3 velocity, bool ignoreSafetyCheck, string factionOverride, string spawnProfileId) {

			return SpawnRequest.CalculateSpawn(spawningMatrix.Translation, "API Request: " + spawnProfileId, SpawningType.OtherNPC, ignoreSafetyCheck, false, spawnGroups, factionOverride);

		}

		public static Vector3D GetDespawnCoords(IMyCubeGrid cubeGrid) {

			var npcGrid = NpcManager.GetNpcFromGrid(cubeGrid);

			if (npcGrid?.Npc == null)
				return Vector3D.Zero;

			return npcGrid.Npc.EndCoords;

		}

		public static List<string> GetSpawnGroupBlackList() {

			return new List<string>(Settings.General.NpcSpawnGroupBlacklist.ToList());

		}

		public static List<string> GetNpcNameBlackList() {

			return new List<string>(Settings.General.NpcGridNameBlacklist.ToList());

		}

		public static Vector3D GetNpcStartCoordinates(IMyCubeGrid cubeGrid) {

			var npcGrid = NpcManager.GetNpcFromGrid(cubeGrid);

			if (npcGrid?.Npc == null)
				return Vector3D.Zero;

			return npcGrid.Npc.StartCoords;

		}

		public static Vector3D GetNpcEndCoordinates(IMyCubeGrid cubeGrid) {

			var npcGrid = NpcManager.GetNpcFromGrid(cubeGrid);

			if (npcGrid?.Npc == null)
				return Vector3D.Zero;

			return npcGrid.Npc.EndCoords;

		}

		public static bool IsPositionInKnownPlayerLocation(Vector3D coords, bool mustMatchFaction, string faction) {

			return KnownPlayerLocationManager.IsPositionInKnownPlayerLocation(coords, mustMatchFaction, faction);

		}

		public static bool RegisterDespawnWatcher(IMyCubeGrid cubeGrid, Action<IMyCubeGrid, string> action) {

			var npcGrid = NpcManager.GetNpcFromGrid(cubeGrid);

			if (npcGrid?.Npc == null)
				return false;

			if (npcGrid.Npc.DespawnActions == null)
				npcGrid.Npc.DespawnActions = new List<Action<IMyCubeGrid, string>>();

			npcGrid.Npc.DespawnActions.Add(action);
			return true;

		}

		public static void RegisterRemoteControlCode(IMyRemoteControl remoteControl, string code) {

			lock (NpcManager.RemoteControlCodes) {

				if (remoteControl == null || NpcManager.RemoteControlCodes.ContainsKey(remoteControl))
					return;

				NpcManager.RemoteControlCodes.Add(remoteControl, code);

			}

		}

		public static bool SetSpawnerIgnoreForDespawn(IMyCubeGrid cubeGrid, bool ignore) {

			var npcGrid = NpcManager.GetNpcFromGrid(cubeGrid);

			if (npcGrid?.Npc == null)
				return false;

			npcGrid.Npc.Attributes |= NpcAttributes.IgnoreCleanup;
			npcGrid.Npc.Update();
			return true;

		}

		//SpawnBossEncounter
		public static bool SpawnBossEncounter(Vector3D coords, List<string> spawnGroups) {

			return SpawnRequest.CalculateSpawn(coords, "API Request", SpawningType.BossEncounter, false, false, spawnGroups);

		}

		//SpawnCreature
		public static bool SpawnCreature(Vector3D coords, List<string> spawnGroups) {

			return SpawnRequest.CalculateSpawn(coords, "API Request", SpawningType.Creature, false, false, spawnGroups);

		}

		//SpawnPlanetaryCargoShip
		public static bool SpawnPlanetaryCargoShip(Vector3D coords, List<string> spawnGroups) {

			return SpawnRequest.CalculateSpawn(coords, "API Request", SpawningType.PlanetaryCargoShip, false, false, spawnGroups);

		}

		//SpawnPlanetaryInstallation
		public static bool SpawnPlanetaryInstallation(Vector3D coords, List<string> spawnGroups) {

			return SpawnRequest.CalculateSpawn(coords, "API Request", SpawningType.PlanetaryInstallation, false, false, spawnGroups);

		}

		//SpawnRandomEncounter
		public static bool SpawnRandomEncounter(Vector3D coords, List<string> spawnGroups) {

			return SpawnRequest.CalculateSpawn(coords, "API Request", SpawningType.RandomEncounter, false, false, spawnGroups);

		}

		//SpawnSpaceCargoShip
		public static bool SpawnSpaceCargoShip(Vector3D coords, List<string> spawnGroups) {

			return SpawnRequest.CalculateSpawn(coords, "API Request", SpawningType.SpaceCargoShip, false, false, spawnGroups);

		}

	}

}
