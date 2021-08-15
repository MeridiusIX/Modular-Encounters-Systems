using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.Spawning.Profiles;
using ModularEncountersSystems.Sync;
using ModularEncountersSystems.World;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Spawning {

	public static class PrefabSpawner {

		public static List<PrefabContainer> Prefabs = new List<PrefabContainer>();
		public static List<IMyCubeGrid> gridListDummy = new List<IMyCubeGrid>();

		public static void Setup() {

			//Generate and Populate Dummy Prefabs
			for (int i = 1; i < 31; i++) {

				var name = "MES-Prefab-1-" + i.ToString();
				var prefab = new PrefabContainer(name);

				if (prefab.Valid) {

					Prefabs.Add(prefab);
				
				} else {
				
					//TODO: Logger That Prefab isnt valid
				
				}

			}

			MES_SessionCore.UnloadActions += Unload;
		
		}

		public static bool ProcessSpawning(SpawnGroupCollection spawnCollection, PathDetails path, EnvironmentEvaluation environment) {

			if (!path.SpawnType.HasFlag(SpawningType.Creature)) {

				//Do Voxels First
				SpawnVoxels(spawnCollection, path);

				//Then Do Prefabs
				SpawnPrefab(spawnCollection, path, environment);

			} else {
			
				//Determine if Creatures are Keen or jTurp
			
			}

			

			return true;
		
		}

		public static void SpawnVoxels(SpawnGroupCollection spawnCollection, PathDetails path) {

			if (spawnCollection.SpawnGroup.SpawnGroup.Voxels.Count > 0) {

				foreach (var voxel in spawnCollection.SpawnGroup.SpawnGroup.Voxels) {

					Vector3D coords = Vector3D.Transform(voxel.Offset, path.SpawnMatrix);
					IMyVoxelMap voxelSpawn = null;

					try {

						voxelSpawn = MyAPIGateway.Session.VoxelMaps.CreateVoxelMapFromStorageName(voxel.StorageName, voxel.StorageName, coords);

						if (voxelSpawn != null) {

							if (spawnCollection.Conditions.AlignVoxelsToSpawnMatrix) {

								var newMatrix = MatrixD.CreateWorld(voxelSpawn.WorldMatrix.Translation, path.SpawnMatrix.Forward, path.SpawnMatrix.Up);
								voxelSpawn.SetWorldMatrix(newMatrix);

							}

							if (!voxel.CenterOffset) {

								var center = voxelSpawn.PositionComp.WorldAABB.Center;
								var corner = voxelSpawn.PositionLeftBottomCorner;
								var dir = Vector3D.Normalize(corner - center);
								var newCoords = dir * Vector3D.Distance(corner, center) + corner;
								var newMatrix = MatrixD.CreateWorld(newCoords, voxelSpawn.WorldMatrix.Forward, voxelSpawn.WorldMatrix.Up);
								voxelSpawn.SetWorldMatrix(newMatrix);

							}

						}

					} catch (Exception e) {

						//TODO: Add Exception Message

					}

				}

			}

		}

		public static void SpawnPrefab(SpawnGroupCollection spawnCollection, PathDetails path, EnvironmentEvaluation environment) {

			string faction = spawnCollection.SelectRandomFaction();
			long factionOwner = FactionHelper.GetFactionMemberIdFromTag(faction);
			environment.GetThreat(spawnCollection.Conditions.ThreatLevelCheckRange, spawnCollection.Conditions.ThreatIncludeOtherNpcOwners);
			SpawnLogger.Write("Spawning " + spawnCollection.PrefabIndexes.Count + " Prefabs With Ownership: " + faction + " / " + factionOwner.ToString(), SpawnerDebugEnum.Spawning);

			for (int i = 0; i < spawnCollection.PrefabIndexes.Count; i++) {

				var sgPrefab = spawnCollection.SpawnGroup.SpawnGroup.Prefabs[spawnCollection.PrefabIndexes[i]];

				//Select Prefab Container
				PrefabContainer prefab = null;

				foreach (var prefabContainer in Prefabs) {

					if (!prefabContainer.Valid || prefabContainer.SpawningInProgress)
						continue;

					if (prefabContainer.InitializePrefabForSpawn(sgPrefab.SubtypeId)) {

						prefab = prefabContainer;
						prefab.SpawnGroupPrefab = sgPrefab;
						break;

					}
				
				}

				if (prefab == null) {

					SpawnLogger.Write("Prefab Null", SpawnerDebugEnum.Spawning);
					continue;

				}

				//NPC Data
				var npcData = new NpcData();
				npcData.AssignAttributes(spawnCollection.SpawnGroup, path.SpawnType);
				npcData.SpawnType = path.SpawnType;
				npcData.SpawnGroupName = spawnCollection.SpawnGroup.SpawnGroupName;
				npcData.OriginalPrefabId = sgPrefab.SubtypeId;
				npcData.SpawnerPrefabId = prefab.PrefabSubtypeId;
				npcData.BehaviorName = sgPrefab.Behaviour;
				npcData.BehaviorTriggerDist = sgPrefab.BehaviourActivationDistance;
				npcData.InitialFaction = faction;
				npcData.PrefabSpeed = sgPrefab.Speed;
				npcData.SpawnedByMES = true;

				//Calculate Coordinates
				npcData.StartCoords = path.GetPrefabStartCoords(sgPrefab.Position, environment, spawnCollection.Conditions.CustomPathStartAltitude);
				npcData.EndCoords = path.GetPrefabEndCoords(sgPrefab.Position, environment, spawnCollection.Conditions.CustomPathEndAltitude);
				npcData.Forward = path.SpawnMatrix.Forward;
				npcData.Up = path.SpawnMatrix.Up;

				Vector3 linearVelocity = Vector3.Zero;
				Vector3 angularVelocity = Vector3.Zero;

				if (path.CustomVelocity == new Vector3D()) {

					if (npcData.StartCoords != npcData.EndCoords) {

						//TODO: Consider if using Autopilot or Behavior

						Vector3D dir = Vector3D.Normalize(npcData.EndCoords - npcData.StartCoords);

						linearVelocity = dir * sgPrefab.Speed;

						if (path.OverrideSpeed > -1) {

							linearVelocity = dir * path.OverrideSpeed;

						}

						if (path.MinSpeed > linearVelocity.Length()) {

							linearVelocity = dir * path.MinSpeed;

						}

						npcData.PrefabSpeed = linearVelocity.Length();

					}

				} else {

					linearVelocity = path.CustomVelocity;

				}

				var options = SpawnGroupManager.CreateSpawningOptions(spawnCollection.Conditions, sgPrefab);

				if (spawnCollection.Conditions.ForceExactPositionAndOrientation) {

					npcData.Attributes |= NpcAttributes.SetMatrixPostSpawn;

					if (!options.HasFlag(SpawningOptions.UseGridOrigin))
						options |= SpawningOptions.UseGridOrigin;

					if (options.HasFlag(SpawningOptions.RotateFirstCockpitTowardsDirection))
						options &= ~SpawningOptions.RotateFirstCockpitTowardsDirection;

				}

				//Prefab Manipulation
				SpawnLogger.Write("Starting Prefab Manipulations", SpawnerDebugEnum.Spawning);
				PrefabManipulation.PrepareManipulations(prefab, spawnCollection, environment, npcData);

				var spawnMatrix = path.SpawnMatrix;

				if (spawnCollection.PrefabIndexes[i] < spawnCollection.Conditions.RotateInstallations.Count) {

					spawnMatrix.Translation = npcData.StartCoords;
					spawnMatrix = PathPlacements.CalculateDerelictSpawnMatrix(spawnMatrix, spawnCollection.Conditions.RotateInstallations[spawnCollection.PrefabIndexes[i]]);

				}

				SpawnLogger.Write("Relative Spawn Coordinates:   " + spawnMatrix.Translation, SpawnerDebugEnum.Pathing);
				SpawnLogger.Write("Final Prefab Coordinates:     " + npcData.StartCoords, SpawnerDebugEnum.Pathing);
				SpawnLogger.Write("Distance From Relative 0,0,0: " + Vector3D.Distance(spawnMatrix.Translation, npcData.StartCoords), SpawnerDebugEnum.Pathing);
				//SpawnLogger.Write("Final Spawn Matrix Forward:     " + spawnMatrix.Forward, SpawnerDebugEnum.Pathing);
				//SpawnLogger.Write("Final Spawn Matrix Up:          " + spawnMatrix.Up, SpawnerDebugEnum.Pathing);

				//Send to IMyPrefabManager
				try {

					gridListDummy.Clear();
					MyAPIGateway.PrefabManager.SpawnPrefab(gridListDummy, prefab.PrefabSubtypeId, npcData.StartCoords, spawnMatrix.Forward, spawnMatrix.Up, linearVelocity, angularVelocity, !string.IsNullOrWhiteSpace(sgPrefab.BeaconText) ? sgPrefab.BeaconText : null, options, factionOwner);

				} catch (Exception exc) {

					SpawnLogger.Write("Error Spawning Prefab", SpawnerDebugEnum.Error);
					SpawnLogger.Write(exc.ToString(), SpawnerDebugEnum.Error);

				}

			}


		}

		//Tie in Spawn Costs
		public static void ApplySpawningCosts(SpawnConditionsProfile spawnGroup, string factionTag) {

			if (factionTag != "Nobody" && spawnGroup.ChargeNpcFactionForSpawn) {

				var faction = MyAPIGateway.Session.Factions.TryGetFactionByTag(factionTag);

				if (faction != null) {

					long currentBalance = 0;
					faction.TryGetBalanceInfo(out currentBalance);
					faction.RequestChangeBalance(spawnGroup.ChargeForSpawning > currentBalance ? -currentBalance : -spawnGroup.ChargeForSpawning);

				}

			}

			if (spawnGroup.UseSandboxCounterCosts) {

				var count = (spawnGroup.SandboxCounterCostNames.Count >= spawnGroup.SandboxCounterCostAmounts.Count ? spawnGroup.SandboxCounterCostNames.Count : spawnGroup.SandboxCounterCostAmounts.Count);

				for (int i = 0; i < spawnGroup.SandboxCounterCostNames.Count && i < count; i++) {

					int amount = 0;
					MyAPIGateway.Utilities.GetVariable<int>(spawnGroup.SandboxCounterCostNames[i], out amount);
					amount -= spawnGroup.SandboxCounterCostAmounts[i];
					MyAPIGateway.Utilities.SetVariable<int>(spawnGroup.SandboxCounterCostNames[i], amount);

				}

			}

		}

		public static void ApplyInstallationIncrement(SpawnGroupCollection collection, EnvironmentEvaluation environment) {

			var conditions = collection.Conditions;
			var planetEntity = environment.NearestPlanet.Planet;

			if (conditions.PlanetaryInstallationType == "Small") {

				int mediumChance = 0;
				string varName = "MES-" + planetEntity.EntityId.ToString() + "-Medium";
				if (MyAPIGateway.Utilities.GetVariable<int>(varName, out mediumChance) == true) {

					mediumChance += Settings.PlanetaryInstallations.MediumSpawnChanceIncrement;
					MyAPIGateway.Utilities.SetVariable<int>(varName, mediumChance);

				}

				SpawnLogger.Write("Medium Installation Spawning Chance Now Set To: " + mediumChance.ToString() + " / 100", SpawnerDebugEnum.Spawning);

			}

			if (conditions.PlanetaryInstallationType == "Medium" || collection.SkippedAbsentMediumStation == true) {

				int mediumChance = 0;
				string varName = "MES-" + planetEntity.EntityId.ToString() + "-Medium";
				if (MyAPIGateway.Utilities.GetVariable<int>(varName, out mediumChance) == true) {

					mediumChance = Settings.PlanetaryInstallations.MediumSpawnChanceBaseValue;
					MyAPIGateway.Utilities.SetVariable<int>(varName, mediumChance);

				}

				SpawnLogger.Write("Medium Installation Spawning Chance Now Set To: " + mediumChance.ToString() + " / 100", SpawnerDebugEnum.Spawning);

				int largeChance = 0;
				varName = "MES-" + planetEntity.EntityId.ToString() + "-Large";
				if (MyAPIGateway.Utilities.GetVariable<int>(varName, out largeChance) == true) {

					largeChance += Settings.PlanetaryInstallations.LargeSpawnChanceIncrement;
					MyAPIGateway.Utilities.SetVariable<int>(varName, largeChance);

				}

				SpawnLogger.Write("Large Installation Spawning Chance Now Set To: " + largeChance.ToString() + " / 100", SpawnerDebugEnum.Spawning);

			}

			if (conditions.PlanetaryInstallationType == "Large" || collection.SkippedAbsentLargeStation == true) {

				int largeChance = 0;
				string varName = "MES-" + planetEntity.EntityId.ToString() + "-Large";
				if (MyAPIGateway.Utilities.GetVariable<int>(varName, out largeChance) == true) {

					largeChance = Settings.PlanetaryInstallations.LargeSpawnChanceBaseValue;
					MyAPIGateway.Utilities.SetVariable<int>(varName, largeChance);

				}

				SpawnLogger.Write("Large Installation Spawning Chance Now Set To: " + largeChance.ToString() + " / 100", SpawnerDebugEnum.Spawning);

			}

		}

		public static void PrefabSpawnDebug(ChatMessage msg) {


			var msgSplit = msg.GetArray(4,4);

			if (msgSplit.Length != 4) {

				MyVisualScriptLogicProvider.ShowNotification("Invalid Command Received", 5000, "White", msg.PlayerId);
				return;

			}

			var prefab = MyDefinitionManager.Static.GetPrefabDefinition(msgSplit[3]);

			if (prefab == null) {

				MyVisualScriptLogicProvider.ShowNotification("Could Not Find Prefab With Name: " + msgSplit[3], 5000, "White", msg.PlayerId);
				return;

			}

			var matrix = MatrixD.Identity;
			matrix.Translation = msg.PlayerPosition;
			var player = PlayerManager.GetPlayerWithIdentityId(msg.PlayerId);

			if (player?.Player?.Character != null)
				matrix = player.Player.Character.WorldMatrix;

			Vector3D coords = prefab.BoundingSphere.Radius * 1.2 * matrix.Forward + matrix.Translation;

			var dummyList = new List<IMyCubeGrid>();
			MyVisualScriptLogicProvider.ShowNotification("Spawning Prefab [" + msgSplit[2] + "]", 5000, "White", msg.PlayerId);
			MyAPIGateway.PrefabManager.SpawnPrefab(dummyList, msgSplit[3], coords, matrix.Backward, matrix.Up, Vector3.Zero, Vector3.Zero, null, SpawningOptions.RotateFirstCockpitTowardsDirection, msg.PlayerId);

		}

		public static void PrefabStationSpawnDebug(ChatMessage msg) {

			var msgSplit = msg.GetArray(5, 5);

			if (msgSplit.Length != 5) {

				MyVisualScriptLogicProvider.ShowNotification("Invalid Command Received", 5000, "White", msg.PlayerId);
				return;

			}

			double depth = 0;
			var prefab = MyDefinitionManager.Static.GetPrefabDefinition(msgSplit[4]);

			if (!double.TryParse(msgSplit[3], out depth)) {

				MyVisualScriptLogicProvider.ShowNotification("Could Not Parse Number Value: " + msgSplit[3], 5000, "White", msg.PlayerId);
				return;

			}

			if (prefab == null) {

				MyVisualScriptLogicProvider.ShowNotification("Could Not Find Prefab With Name: " + msgSplit[4], 5000, "White", msg.PlayerId);
				return;

			}

			var planet = PlanetManager.GetNearestPlanet(msg.PlayerPosition) ;

			if (planet == null) {

				MyVisualScriptLogicProvider.ShowNotification("Could Not Find Nearby Planet", 5000, "White", msg.PlayerId);
				return;

			}

			var matrix = MatrixD.Identity;
			matrix.Translation = msg.PlayerPosition;
			var player = PlayerManager.GetPlayerWithIdentityId(msg.PlayerId);

			if (player?.Player?.Character != null)
				matrix = player.Player.Character.WorldMatrix;

			Vector3D roughcoords = prefab.BoundingSphere.Radius * 1.2 * matrix.Forward + matrix.Translation;
			Vector3D surfacecoords = planet.SurfaceCoordsAtPosition(roughcoords);
			Vector3D up = planet.UpAtPosition(surfacecoords);
			Vector3D coords = up * depth + surfacecoords;

			if (Vector3D.Distance(coords, msg.PlayerPosition) < prefab.BoundingSphere.Radius) {

				MyVisualScriptLogicProvider.ShowNotification("Player Too Close To Spawn Location", 5000, "White", msg.PlayerId);
				return;

			}

			matrix = MatrixD.CreateWorld(coords, Vector3D.CalculatePerpendicularVector(up), up);

			var dummyList = new List<IMyCubeGrid>();
			MyAPIGateway.PrefabManager.SpawnPrefab(dummyList, msgSplit[4], coords, matrix.Backward, matrix.Up, Vector3.Zero, Vector3.Zero, null, SpawningOptions.RotateFirstCockpitTowardsDirection, msg.PlayerId);
			MyVisualScriptLogicProvider.ShowNotification("Spawning Prefab [" + msgSplit[4] + "] As Planetary Installation", 5000, "White", msg.PlayerId);

		}

		public static void Unload() {

			Prefabs.Clear();
		
		}

	}
}
