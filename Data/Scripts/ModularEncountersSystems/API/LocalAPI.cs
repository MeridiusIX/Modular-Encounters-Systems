using ModularEncountersSystems.BlockLogic;
using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.Sync;
using ModularEncountersSystems.World;
using ModularEncountersSystems.Zones;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.API {
	public static class LocalApi {

		public static Action<IMyRemoteControl, string, string, IMyEntity, Vector3D> BehaviorTriggerWatcher;
		public static Action<IMyRemoteControl, IMyCubeGrid> CompromisedRemoteEvent;
		public static Action<IMyCubeGrid> SuccessfulSpawnEvent;
		public static Dictionary<string, Func<IMyRemoteControl, string, IMyEntity, Vector3D, bool>> BehaviorCustomTriggers = new Dictionary<string, Func<IMyRemoteControl, string, IMyEntity, Vector3D, bool>>();
		public static Dictionary<string, Func<string, string, string, Vector3D, bool>> SpawnCustomConditions = new Dictionary<string, Func<string, string, string, Vector3D, bool>>();

		public static Dictionary<string, Action<object[]>> CustomActions = new Dictionary<string, Action<object[]>>();



		public static void SendApiToMods() {

			//Create a Dictionary of delegates that point to methods in the MES code.
			//Send the Dictionary To Other Mods That Registered To This ID in LoadData()
			MyAPIGateway.Utilities.SendModMessage(1521905890, GetApiDictionary());

		}

		public static Dictionary<string, Delegate> GetApiDictionary() {

			var dict = new Dictionary<string, Delegate>();
			
			dict.Add("AddKnownPlayerLocation", new Action<Vector3D, string, double, int, int, int>(KnownPlayerLocationManager.AddKnownPlayerLocation));
			dict.Add("ApiSpawnRequest", new Func<Vector3D, string, string, bool, bool, List<string>, string, MatrixD, Vector3D, bool, long, bool>(ApiSpawnRequest));
			dict.Add("BehaviorTriggerActivationWatcher", new Action<bool, Action<IMyRemoteControl, string, string, IMyEntity, Vector3D>>(ChangeBehaviorTriggerWatcher));
			dict.Add("ChatCommand", new Action<string, MatrixD, long, ulong>(ChatManager.ChatFromApi));
			dict.Add("CustomSpawnRequest", new Func<List<string>, MatrixD, Vector3, bool, string, string, bool>(CustomSpawnRequest));
			dict.Add("GetDespawnCoords", new Func<IMyCubeGrid, Vector3D>(GetDespawnCoords));
			dict.Add("GetPlayerInhibitorData", new Action<long, string, List<MyTuple<IMyRadioAntenna, DateTime>>>(GetPlayerInhibitorData));
			dict.Add("GetSpawnGroupBlackList", new Func<List<string>>(GetSpawnGroupBlackList));
			dict.Add("GetSpawnGroupsByType", new Action<string, List<string>>(GetSpawnGroupsByType));
			dict.Add("GetNpcNameBlackList", new Func<List<string>>(GetNpcNameBlackList));
			//GetZonesAtPosition
			dict.Add("IsPositionInKnownPlayerLocation", new Func<Vector3D, bool, string, bool>(KnownPlayerLocationManager.IsPositionInKnownPlayerLocation));
			dict.Add("ConvertRandomNamePatterns", new Func<string, string>(RandomNameGenerator.CreateRandomNameFromPattern));
			dict.Add("GetNpcStartCoordinates", new Func<IMyCubeGrid, Vector3D>(GetNpcStartCoordinates));
			dict.Add("GetNpcEndCoordinates", new Func<IMyCubeGrid, Vector3D>(GetNpcEndCoordinates));
			dict.Add("RegisterBehaviorCustomTrigger", new Action<bool, string, Func<IMyRemoteControl, string, IMyEntity, Vector3D, bool>>(RegisterBehaviorCustomTrigger));
			dict.Add("RegisterCompromisedRemoteWatcher", new Action<bool, Action<IMyRemoteControl, IMyCubeGrid>>(RegisterCompromisedRemoteWatcher));
			dict.Add("RegisterCustomSpawnCondition", new Action<bool, string, Func<string, string, string, Vector3D, bool>>(RegisterCustomSpawnCondition));
			dict.Add("RegisterDespawnWatcher", new Func<IMyCubeGrid, Action<IMyCubeGrid, string>, bool>(RegisterDespawnWatcher));
			dict.Add("RegisterRemoteControlCode", new Action<IMyRemoteControl, string>(RegisterRemoteControlCode));
			dict.Add("RegisterSuccessfulSpawnAction", new Action<Action<IMyCubeGrid>, bool>(RegisterSuccessfulSpawnAction));
			dict.Add("RemoveKnownPlayerLocation", new Action<Vector3D, string, bool>(KnownPlayerLocationManager.RemoveLocation));
			dict.Add("SetCombatPhase", new Action<bool>(CombatPhaseManager.ForcePhase));
			dict.Add("SetSpawnerIgnoreForDespawn", new Func<IMyCubeGrid, bool, bool>(SetSpawnerIgnoreForDespawn));
			dict.Add("SetZoneEnabled", new Action<string, bool, Vector3D?>(SetZoneEnabled));
			dict.Add("SpawnBossEncounter", new Func<Vector3D, List<string>, bool>(SpawnBossEncounter));
			dict.Add("SpawnPlanetaryCargoShip", new Func<Vector3D, List<string>, bool>(SpawnPlanetaryCargoShip));
			dict.Add("SpawnPlanetaryInstallation", new Func<Vector3D, List<string>, bool>(SpawnPlanetaryInstallation));
			dict.Add("SpawnRandomEncounter", new Func<Vector3D, List<string>, bool>(SpawnRandomEncounter));
			dict.Add("SpawnSpaceCargoShip", new Func<Vector3D, List<string>, bool>(SpawnSpaceCargoShip));
			dict.Add("ToggleSpawnGroupEnabled", new Action<string, bool>(ToggleSpawnGroupEnabled));
			dict.Add("RegisterCustomAction", new Action<bool, string, Action<object[]>>(RegisterCustomAction));
			return dict;

		}


		public static void ChangeBehaviorTriggerWatcher(bool register, Action<IMyRemoteControl, string, string, IMyEntity, Vector3D> action) {

			if (register)
				BehaviorTriggerWatcher += action;
			else
				BehaviorTriggerWatcher -= action;

		}

		public static void RegisterBehaviorCustomTrigger(bool register, string methodIdentifier, Func<IMyRemoteControl, string, IMyEntity, Vector3D, bool> action) {

			if (register && !BehaviorCustomTriggers.ContainsKey(methodIdentifier))
				BehaviorCustomTriggers.Add(methodIdentifier, action);
			else
				BehaviorCustomTriggers.Remove(methodIdentifier);

		}

		
		public static void RegisterCustomSpawnCondition(bool register, string methodIdentifier, Func<string, string, string, Vector3D, bool> action) {

			//SpawnGroup
			//SpawnCondition
			//SpawnType
			//Vector3D

			if (register && !SpawnCustomConditions.ContainsKey(methodIdentifier))
				SpawnCustomConditions.Add(methodIdentifier, action);
			else
				SpawnCustomConditions.Remove(methodIdentifier);

		}

		public static void RegisterCustomAction(bool register, string methodIdentifier, Action<object[]> action)
		{

			//SpawnGroup
			//SpawnCondition
			//SpawnType
			//Vector3D

			if (register && !CustomActions.ContainsKey(methodIdentifier))
				CustomActions.Add(methodIdentifier, action);
			else
				CustomActions.Remove(methodIdentifier);

		}



		public static void RegisterSuccessfulSpawnAction(Action<IMyCubeGrid> action, bool register) {

			if (action != null) {

				if (register)
					SuccessfulSpawnEvent += action;
				else
					SuccessfulSpawnEvent -= action;
						
			}
		
		}

		//CustomSpawnRequest
		public static bool CustomSpawnRequest(List<string> spawnGroups, MatrixD spawningMatrix, Vector3 velocity, bool ignoreSafetyCheck, string factionOverride, string spawnProfileId) {

			return SpawnRequest.CalculateSpawn(spawningMatrix.Translation, "API Request: " + spawnProfileId, SpawningType.OtherNPC, ignoreSafetyCheck, true, spawnGroups, factionOverride, spawningMatrix, velocity);

		}

		//ApiSpawnRequest
		public static bool ApiSpawnRequest(Vector3D coords, string source, string spawningTypeString, bool forceSpawn = false, bool adminSpawn = false, List<string> eligibleNames = null, string factionOverride = null, MatrixD spawnMatrix = new MatrixD(), Vector3D customVelocity = new Vector3D(), bool ignoreSafetyChecks = false, long ownerOverride = -1) {

			SpawningType type = SpawningType.None;

			if (!Enum.TryParse<SpawningType>(spawningTypeString, out type))
				return false;

			return SpawnRequest.CalculateSpawn(coords, "API Request: " + (source ?? "null"), type, forceSpawn, adminSpawn, eligibleNames, factionOverride, spawnMatrix, customVelocity, ignoreSafetyChecks, ownerOverride);

		}

		public static Vector3D GetDespawnCoords(IMyCubeGrid cubeGrid) {

			var npcGrid = NpcManager.GetNpcFromGrid(cubeGrid);

			if (npcGrid?.Npc == null)
				return Vector3D.Zero;

			return npcGrid.Npc.EndCoords;

		}

		public static void GetPlayerInhibitorData(long playerIdentityId, string inhibitorType, List<MyTuple<IMyRadioAntenna, DateTime>> inhibitorData) {

			var player = PlayerManager.GetPlayerWithIdentityId(playerIdentityId);

			if (player == null)
				return;

			if (inhibitorData == null)
				inhibitorData = new List<MyTuple<IMyRadioAntenna, DateTime>>();

			inhibitorData.Clear();

			var inhibType = InhibitorLogic.GetInhibitorType(inhibitorType);

			if (!player.InhibitorIdsInRange.ContainsKey(inhibType))
				return;

			foreach (var block in player.InhibitorIdsInRange[inhibType])
				inhibitorData.Add(block);

		}

		public static List<string> GetSpawnGroupBlackList() {

			return new List<string>(Settings.General.NpcSpawnGroupBlacklist.ToList());

		}

		public static void GetSpawnGroupsByType(string spawntype, List<string> output) {

			SpawningType type = SpawningType.None;

			if (!Enum.TryParse<SpawningType>(spawntype, out type))
				return;

			foreach (var spawnGroup in SpawnGroupManager.SpawnGroups) {

				if (spawnGroup?.SpawnGroupName == null)
					continue;

				foreach (var condition in spawnGroup.SpawnConditionsProfiles) {

					if (SpawnConditions.SpawningTypeAllowedForGroup(type, condition)) {

						output.Add(spawnGroup.SpawnGroupName);
						break;
					
					}
				
				}
			
			}
		
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

		public static void RegisterCompromisedRemoteWatcher(bool register, Action<IMyRemoteControl, IMyCubeGrid> action) {

			if (register)
				CompromisedRemoteEvent += action;
			else
				CompromisedRemoteEvent -= action;

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

			npcGrid.Npc.Attributes.IgnoreCleanup = true;
			npcGrid.Npc.Update();
			return true;

		}

		public static void SetZoneEnabled(string zoneName, bool enabled, Vector3D? zoneCoords) {

			ZoneManager.ToggleZones(zoneName, enabled, zoneCoords);

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

		public static void ToggleSpawnGroupEnabled(string spawnGroupName, bool toggle) {

			foreach (var spawnGroup in SpawnGroupManager.SpawnGroups) {

				if (spawnGroup.SpawnGroupName == spawnGroupName) {

					spawnGroup.SpawnGroupEnabled = toggle;
					break;

				}

			}
								
		}

	}

}
