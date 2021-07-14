using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Tasks;
using ModularEncountersSystems.World;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Watchers {
	public static class CargoShipWatcher {

		public static List<GridEntity> CargoShips = new List<GridEntity>();

		public static void Setup() {

			TaskProcessor.Tick60.Tasks += ProcessCargoShips;
			//MES_SessionCore.UnloadActions += Unload;

		}

		public static void ProcessCargoShips() {

			for (int i = CargoShips.Count - 1; i >= 0; i--) {

				var cargoShip = CargoShips[i];

				if (!cargoShip.ActiveEntity() || cargoShip.Npc == null || !cargoShip.Npc.Attributes.HasFlag(NpcAttributes.IsCargoShip)) {

					SpawnLogger.Write("Drifting Cargo Ship Entity Not Valid, No Longer NPC, or is Missing Cargo Ship Attribute. Removed From Watcher", SpawnerDebugEnum.PostSpawn);
					CargoShips.RemoveAt(i);
					continue;

				}

				if (cargoShip.Behavior != null) {

					if (cargoShip.Behavior.Settings.ActiveBehaviorType != BehaviorSubclass.Passive) {

						SpawnLogger.Write("Drifting Cargo Ship " + cargoShip.CubeGrid.CustomName +" Using RivalAI Behavior for Navigation. Removed From Watcher", SpawnerDebugEnum.PostSpawn);
						CargoShips.RemoveAt(i);
						continue;

					}
				
				}

				var distFromStart = Vector3D.Distance(cargoShip.Npc.StartCoords, cargoShip.Npc.EndCoords);
				var shipDistFromStart = Vector3D.Distance(cargoShip.Npc.StartCoords, cargoShip.GetPosition());
				var shipDistToEnd = Vector3D.Distance(cargoShip.Npc.EndCoords, cargoShip.GetPosition());
				var dirFromStartToEnd = Vector3D.Normalize(cargoShip.Npc.EndCoords - cargoShip.Npc.StartCoords);
				var dirFromStartToShip = Vector3D.Normalize(cargoShip.GetPosition() - cargoShip.Npc.StartCoords);
				var pathAngle = VectorHelper.GetAngleBetweenDirections(dirFromStartToShip, dirFromStartToEnd);

				if (pathAngle > 10 && shipDistFromStart > 1000) {

					SpawnLogger.Write("Drifting Cargo Ship " + cargoShip.CubeGrid.CustomName + " Deviated From Path By More Than 10 Degrees (" + pathAngle + "). Removed From Watcher", SpawnerDebugEnum.PostSpawn);
					CargoShips.RemoveAt(i);
					continue;

				}

				//Drift
				if (!CargoShipDriftCheck(cargoShip)) {

					CargoShips.RemoveAt(i);
					continue;

				}

				if (shipDistFromStart < distFromStart && shipDistToEnd > Settings.SpaceCargoShips.DespawnDistanceFromEndPath) {

					continue;
				
				}

				var player = PlayerManager.GetNearestPlayer(cargoShip.GetPosition());

				if (player == null || player.Distance(cargoShip.GetPosition()) < Settings.SpaceCargoShips.DespawnDistanceFromPlayer) {

					continue;
				
				}

				if (Cleaning.BasicCleanupChecks(cargoShip)) {

					for (int j = 0; j < cargoShip.LinkedGrids.Count; j++) {

						var linkedGrid = cargoShip.LinkedGrids[j];
						Cleaning.RemoveGrid(linkedGrid);
						SpawnLogger.Write("Drifting Cargo Ship " + linkedGrid.CubeGrid.CustomName + " At End of Path and Eligible for Despawn.", SpawnerDebugEnum.PostSpawn);
						SpawnLogger.Write("Drifting Cargo Ship " + linkedGrid.CubeGrid.CustomName + " Queued For Removal and Removed From Watcher.", SpawnerDebugEnum.PostSpawn);
						linkedGrid.Npc.DespawnSource = "Cargo Ship Reached End of Path / Distance";
						CargoShips.RemoveAt(i);

					}

				} else {

					SpawnLogger.Write("Drifting Cargo Ship " + cargoShip.CubeGrid.CustomName + " Was Not Eligible For NPC Despawn. Removed From Watcher", SpawnerDebugEnum.PostSpawn);
					CargoShips.RemoveAt(i);

				}
			
			}
		
		}

		private static void CargoShipAutopilotManager(GridEntity grid, PlayerEntity player) {

			if (!PlanetManager.InGravity(grid.GetPosition()))
				return;

			if (grid.Npc.PrimaryRemoteControlId == -1)
				return;

			if (grid.Npc.PrimaryRemoteControlId == 0) {
			
				//TODO: Get Remote
			
			}
		
		}

		private static bool CargoShipDriftCheck(GridEntity cargoShip) {

			if (!cargoShip.Npc.CargoShipDriftCheck) {

				if (cargoShip.CubeGrid.Physics != null) {

					cargoShip.Npc.CargoShipDriftCheck = true;
					cargoShip.Npc.CargoShipDriftVelocity = cargoShip.CubeGrid.Physics.LinearVelocity;

					if (cargoShip.Npc.CargoShipDriftVelocity.Length() < 0.2) {

						SpawnLogger.Write("Drifting Cargo Ship " + cargoShip.CubeGrid.CustomName + " Initial Speed Lower Than 0.2. Removed From Watcher", SpawnerDebugEnum.PostSpawn);
						return false;

					}
						

				}

				return true;

			}

			var currentVelocity = cargoShip.CubeGrid.Physics.LinearVelocity;

			if (currentVelocity.Length() <= 0.1) {

				Vector3 previousSpeed = cargoShip.Npc.CargoShipDriftVelocity;

				if (previousSpeed != Vector3D.Zero) {

					cargoShip.CubeGrid.Physics.LinearVelocity = previousSpeed;

				} else {

					SpawnLogger.Write("Drifting Cargo Ship " + cargoShip.CubeGrid.CustomName + " Previous Speed Was 0. Removed From Watcher", SpawnerDebugEnum.PostSpawn);
					return false;
				
				}

			} else {

				Vector3 previousSpeed = cargoShip.Npc.CargoShipDriftVelocity;
				var diff = previousSpeed.Length() / currentVelocity.Length();

				if (diff <= 0.8f || diff >= 1.2f) {

					SpawnLogger.Write("Drifting Cargo Ship " + cargoShip.CubeGrid.CustomName + " Slowed Down From External Influence. Removed From Watcher", SpawnerDebugEnum.PostSpawn);
					return false;

				} else {

					cargoShip.Npc.CargoShipDriftVelocity = currentVelocity;

				}

			}

			return true;

		}

	}

}
