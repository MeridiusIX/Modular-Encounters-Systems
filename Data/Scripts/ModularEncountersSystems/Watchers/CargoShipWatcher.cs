using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Tasks;
using ModularEncountersSystems.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Watchers {
	public static class CargoShipWatcher {

		public static List<GridEntity> CargoShips = new List<GridEntity>();
		public static List<GridEntity> LegacyAutopilot = new List<GridEntity>();

		public static void Setup() {

			TaskProcessor.Tick60.Tasks += ProcessCargoShips;
			//MES_SessionCore.UnloadActions += Unload;

		}

		public static void ProcessCargoShips() {

			for (int i = CargoShips.Count - 1; i >= 0; i--) {

				var cargoShip = CargoShips[i];

				if (!cargoShip.ActiveEntity() || cargoShip.Npc == null || !cargoShip.Npc.Attributes.IsCargoShip) {

					SpawnLogger.Write("Drifting Cargo Ship Entity Not Valid, No Longer NPC, or is Missing Cargo Ship Attribute. Removed From Watcher", SpawnerDebugEnum.PostSpawn);

					if (cargoShip.ActiveEntity())
						cargoShip.AppendDebug("Drifting Cargo Ship Entity Not Valid, No Longer NPC, or is Missing Cargo Ship Attribute. Removed From Watcher");

					CargoShips.RemoveAt(i);
					LegacyAutopilot.Remove(cargoShip);
					continue;

				}

				if (cargoShip.Behavior != null) {

					if (cargoShip.Behavior.BehaviorSettings.ActiveBehaviorType != BehaviorSubclass.Passive) {

						SpawnLogger.Write("Drifting Cargo Ship " + cargoShip.CubeGrid.CustomName +" Using RivalAI Behavior for Navigation. Removed From Watcher", SpawnerDebugEnum.PostSpawn);
						cargoShip.AppendDebug("Drifting Cargo Ship " + cargoShip.CubeGrid.CustomName + " Using RivalAI Behavior for Navigation. Removed From Watcher");
						CargoShips.RemoveAt(i);
						LegacyAutopilot.Remove(cargoShip);
						continue;

					}
				
				}

				var distFromStart = Vector3D.Distance(cargoShip.Npc.StartCoords, cargoShip.Npc.EndCoords);
				var shipDistFromStart = Vector3D.Distance(cargoShip.Npc.StartCoords, cargoShip.GetPosition());
				var shipDistToEnd = Vector3D.Distance(cargoShip.Npc.EndCoords, cargoShip.GetPosition());
				var dirFromStartToEnd = Vector3D.Normalize(cargoShip.Npc.EndCoords - cargoShip.Npc.StartCoords);
				var dirFromStartToShip = Vector3D.Normalize(cargoShip.GetPosition() - cargoShip.Npc.StartCoords);
				var pathAngle = VectorHelper.GetAngleBetweenDirections(dirFromStartToShip, dirFromStartToEnd);
				var player = PlayerManager.GetNearestPlayer(cargoShip.GetPosition());

				CargoShipAutopilotManager(cargoShip, player);

				if (!LegacyAutopilot.Contains(cargoShip)) {

					if (pathAngle > 10 && shipDistFromStart > 1000) {

						SpawnLogger.Write("Drifting Cargo Ship " + cargoShip.CubeGrid.CustomName + " Deviated From Path By More Than 10 Degrees (" + pathAngle + "). Removed From Watcher", SpawnerDebugEnum.PostSpawn);
						cargoShip.AppendDebug("Drifting Cargo Ship " + cargoShip.CubeGrid.CustomName + " Deviated From Path By More Than 10 Degrees (" + pathAngle + "). Removed From Watcher");
						CargoShips.RemoveAt(i);
						continue;

					}

					//Drift
					if (!CargoShipDriftCheck(cargoShip)) {

						CargoShips.RemoveAt(i);
						continue;

					}

				}

				if (shipDistFromStart < distFromStart && shipDistToEnd > Settings.SpaceCargoShips.DespawnDistanceFromEndPath) {

					continue;
				
				}

				if (player == null || player.Distance(cargoShip.GetPosition()) < Settings.SpaceCargoShips.DespawnDistanceFromPlayer) {

					continue;
				
				}

				if (Cleaning.BasicCleanupChecks(cargoShip)) {

					for (int j = 0; j < cargoShip.LinkedGrids.Count; j++) {

						var linkedGrid = cargoShip.LinkedGrids[j];
						Cleaning.RemoveGrid(linkedGrid);
						SpawnLogger.Write("Drifting Cargo Ship " + linkedGrid.CubeGrid.CustomName + " At End of Path and Eligible for Despawn.", SpawnerDebugEnum.PostSpawn);
						SpawnLogger.Write("Drifting Cargo Ship " + linkedGrid.CubeGrid.CustomName + " Queued For Removal and Removed From Watcher.", SpawnerDebugEnum.PostSpawn);
						cargoShip.AppendDebug("Drifting Cargo Ship " + linkedGrid.CubeGrid.CustomName + " At End of Path and Eligible for Despawn.");
						cargoShip.AppendDebug("Drifting Cargo Ship " + linkedGrid.CubeGrid.CustomName + " Queued For Removal and Removed From Watcher.");
						linkedGrid.DespawnSource = "Cargo Ship Reached End of Path / Distance";

					}

					CargoShips.RemoveAt(i);
					LegacyAutopilot.Remove(cargoShip);

				} else {

					SpawnLogger.Write("Drifting Cargo Ship " + cargoShip.CubeGrid.CustomName + " Was Not Eligible For NPC Despawn. Removed From Watcher", SpawnerDebugEnum.PostSpawn);
					cargoShip.AppendDebug("Drifting Cargo Ship " + cargoShip.CubeGrid.CustomName + " Was Not Eligible For NPC Despawn. Removed From Watcher");
					CargoShips.RemoveAt(i);
					LegacyAutopilot.Remove(cargoShip);

				}
			
			}
		
		}

		private static void CargoShipAutopilotManager(GridEntity grid, PlayerEntity player) {

			if (grid.Npc.PrimaryRemoteControlId == -1)
				return;

			if (!PlanetManager.InGravity(grid.GetPosition()) && (grid.Behavior == null || grid.Behavior.BehaviorSettings.ActiveBehaviorType != BehaviorSubclass.Passive)) {

				DeactivateKeenAutopilot(grid, "Grid Not In Gravity, Has Null Behavior, Or Behavior Isn't Passive");
				return;

			}
				
			if (grid.Npc.PrimaryRemoteControlId == 0 || grid.Npc.PrimaryRemoteControl == null) {

				IMyRemoteControl mainRemote = null;
				IMyEntity mainRemoteEntity = null;

				if (!MyAPIGateway.Entities.TryGetEntityById(grid.Npc.PrimaryRemoteControlId, out mainRemoteEntity)) {

					foreach (var block in grid.Controllers) {

						if (!block.ActiveEntity())
							continue;

						var remote = block.Block as IMyRemoteControl;

						if (remote == null)
							continue;

						if (mainRemote == null) {

							mainRemote = remote as IMyRemoteControl;

						}

						if (remote.IsMainCockpit || grid.Behavior?.RemoteControl == remote) {

							mainRemote = remote as IMyRemoteControl;
							break;

						}

					}

				} else {

					mainRemote = mainRemoteEntity as IMyRemoteControl;
				
				}

				if (mainRemote == null) {

					DeactivateKeenAutopilot(grid, "Remote Control Could Not Be Found For Cargo Ship Movement");
					return;

				} else {

					grid.Npc.PrimaryRemoteControlId = mainRemote.EntityId;
					grid.Npc.PrimaryRemoteControl = mainRemote;
					LegacyAutopilot.Add(grid);

				}
			
			}

			if (grid.Npc.PrimaryRemoteControl != null) {

				double distance = -1;
				double pauseDistance = grid.Npc.SpawnGroup?.PauseAutopilotAtPlayerDistance ?? -1;

				if (player != null && grid.Npc.SpawnGroup != null && pauseDistance > 0) {

					distance = player.Distance(grid.GetPosition());

				}

				if (pauseDistance > 0 && distance > 0) {

					if (distance < pauseDistance && grid.Npc.PrimaryRemoteControl.IsAutoPilotEnabled) {

						grid.Npc.PrimaryRemoteControl.SetAutoPilotEnabled(false);

					} else if (distance > pauseDistance && !grid.Npc.PrimaryRemoteControl.IsAutoPilotEnabled) {

						AutopilotToCoords(grid);
						return;

					}
				
				} else {

					if (!grid.Npc.PrimaryRemoteControl.IsAutoPilotEnabled) {

						AutopilotToCoords(grid);
						return;

					}
				
				}

				if (grid.Npc.PrimaryRemoteControl.IsAutoPilotEnabled && grid.CubeGrid.Physics.LinearVelocity.Length() < 0.1) {

					grid.Npc.PrimaryRemoteControl.SetAutoPilotEnabled(false);

				}
			
			}
		
		}

		private static void AutopilotToCoords(GridEntity grid) {

			grid.Npc.PrimaryRemoteControl.SetAutoPilotEnabled(false);
			grid.Npc.PrimaryRemoteControl.ClearWaypoints();
			grid.Npc.PrimaryRemoteControl.SpeedLimit = (float)grid.Npc.PrefabSpeed;
			grid.Npc.PrimaryRemoteControl.FlightMode = Sandbox.ModAPI.Ingame.FlightMode.OneWay;
			grid.Npc.PrimaryRemoteControl.AddWaypoint(grid.Npc.EndCoords, "Destination");
			grid.Npc.PrimaryRemoteControl.SetAutoPilotEnabled(true);

		}

		private static void DeactivateKeenAutopilot(GridEntity grid, string reason = "") {

			grid.Npc.PrimaryRemoteControlId = -1;

			if (grid.Npc.PrimaryRemoteControl != null) {

				grid.Npc.PrimaryRemoteControl.SetAutoPilotEnabled(false);
				grid.Npc.PrimaryRemoteControl.ClearWaypoints();

			}

			grid.AppendDebug(reason);
			LegacyAutopilot.Remove(grid);

		}

		private static bool CargoShipDriftCheck(GridEntity cargoShip) {

			if (!cargoShip.Npc.CargoShipDriftCheck) {

				if (cargoShip.CubeGrid.Physics != null) {

					cargoShip.Npc.CargoShipDriftCheck = true;
					cargoShip.Npc.CargoShipDriftVelocity = cargoShip.CubeGrid.Physics.LinearVelocity;

					if (cargoShip.Npc.CargoShipDriftVelocity.Length() < 0.2) {

						SpawnLogger.Write("Drifting Cargo Ship " + cargoShip.CubeGrid.CustomName + " Initial Speed Lower Than 0.2. Removed From Watcher", SpawnerDebugEnum.PostSpawn);
						cargoShip.AppendDebug("Drifting Cargo Ship " + cargoShip.CubeGrid.CustomName + " Initial Speed Lower Than 0.2. Removed From Watcher");
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
					cargoShip.AppendDebug("Drifting Cargo Ship " + cargoShip.CubeGrid.CustomName + " Previous Speed Was 0. Removed From Watcher");
					return false;
				
				}

			} else {

				Vector3 previousSpeed = cargoShip.Npc.CargoShipDriftVelocity;
				var diff = previousSpeed.Length() / currentVelocity.Length();

				if (diff <= 0.8f || diff >= 1.2f) {

					SpawnLogger.Write("Drifting Cargo Ship " + cargoShip.CubeGrid.CustomName + " Slowed Down From External Influence. Removed From Watcher", SpawnerDebugEnum.PostSpawn);
					cargoShip.AppendDebug("Drifting Cargo Ship " + cargoShip.CubeGrid.CustomName + " Slowed Down From External Influence. Removed From Watcher");
					return false;

				} else {

					cargoShip.Npc.CargoShipDriftVelocity = currentVelocity;

				}

			}

			return true;

		}

	}

}
