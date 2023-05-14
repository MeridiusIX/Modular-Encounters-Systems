using System.Collections.Generic;
using ModularEncountersSystems.Behavior.Subsystems.Trigger;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Spawning {


	public struct PendingSpawn {

		public SpawnProfile Spawn;
		public long OwnerOverride;

		public PendingSpawn(SpawnProfile spawn, long owner) {

			Spawn = spawn;
			OwnerOverride = owner;
		
		}
	
	}

	public static class BehaviorSpawnHelper {

		private static bool _spawnInProgress = false;
		private static List <PendingSpawn> _pendingSpawns = new List<PendingSpawn>();

		private static SpawnProfile _currentSpawn;
		private static long _ownerOverride;
		private static MatrixD _spawnMatrix;


		public static void BehaviorSpawnRequest(SpawnProfile spawn = null, long ownerOverride = -1) {

			if (spawn != null) {

				_pendingSpawns.Add(new PendingSpawn(spawn, ownerOverride));

			}

			if (_spawnInProgress == true || _pendingSpawns.Count == 0)
				return;

			_currentSpawn = _pendingSpawns[0].Spawn;
			_ownerOverride = _pendingSpawns[0].OwnerOverride;

			_pendingSpawns.RemoveAt(0);
			_spawnInProgress = true;
			MyAPIGateway.Parallel.Start(SpawningParallelChecks, CompleteSpawning);

		}

		private static void SpawningParallelChecks() {

			if (_currentSpawn.SpawningType != SpawnTypeEnum.CustomSpawn) {

				_spawnMatrix = _currentSpawn.CurrentPositionMatrix;
				return;

			}

			if (_currentSpawn.UseRelativeSpawnPosition) {

				var spawnCoords = Vector3D.Transform(_currentSpawn.RelativeSpawnOffset, _currentSpawn.CurrentPositionMatrix);
				_spawnMatrix = MatrixD.CreateWorld(spawnCoords, _currentSpawn.CurrentPositionMatrix.Forward, _currentSpawn.CurrentPositionMatrix.Up);

			} else {

				var upDir = VectorHelper.GetPlanetUpDirection(_currentSpawn.CurrentPositionMatrix.Translation);
				var playerList = new List<IMyPlayer>();
				MyAPIGateway.Players.GetPlayers(playerList);

				for (int i = 0; i < 15; i++) {

					if (upDir == Vector3D.Zero) {

						var spawnCoords = VectorHelper.RandomDirection() * VectorHelper.RandomDistance(_currentSpawn.MinDistance, _currentSpawn.MaxDistance) + _currentSpawn.CurrentPositionMatrix.Translation;
						var forwardDir = Vector3D.Normalize(spawnCoords - _currentSpawn.CurrentPositionMatrix.Translation);
						var upPerpDir = Vector3D.CalculatePerpendicularVector(forwardDir);
						_spawnMatrix = MatrixD.CreateWorld(spawnCoords, forwardDir, upPerpDir);

					} else {

						_spawnMatrix = VectorHelper.GetPlanetRandomSpawnMatrix(_currentSpawn.CurrentPositionMatrix.Translation, _currentSpawn.MinDistance, _currentSpawn.MaxDistance, _currentSpawn.MinAltitude, _currentSpawn.MaxAltitude, _currentSpawn.InheritNpcAltitude);

					}

					foreach (var player in playerList) {

						if (player.IsBot || player.Controller?.ControlledEntity?.Entity == null) {

							continue;

						}

						if (Vector3D.Distance(_spawnMatrix.Translation, player.GetPosition()) < 100) {

							SpawnLogger.Write(_currentSpawn.ProfileSubtypeId + ": Player Too Close To Possible Spawn Coords. Attempt " + (i + 1).ToString(), SpawnerDebugEnum.Spawning);
							_spawnMatrix = MatrixD.Identity;
							break;

						}

					}

					if (_spawnMatrix != MatrixD.Identity) {

						break;

					}

				}

			}

		}

		private static void CompleteSpawning() {

			if (_spawnMatrix == MatrixD.Identity) {

				SpawnLogger.Write(_currentSpawn.ProfileSubtypeId + ": Spawn Coords Could Not Be Calculated. Aborting Process", SpawnerDebugEnum.Spawning);
				PerformNextSpawn();
				return;

			}

			if (_currentSpawn.SpawningType == SpawnTypeEnum.CustomSpawn) {

				SpawnLogger.Write(_currentSpawn.ProfileSubtypeId + ": Sending CustomSpawn Data to Spawner", SpawnerDebugEnum.Spawning);
				var velocity = Vector3D.Transform(_currentSpawn.RelativeSpawnVelocity, _spawnMatrix) - _spawnMatrix.Translation;
				var result = SpawnRequest.CalculateSpawn(_spawnMatrix.Translation, _currentSpawn.ProfileSubtypeId, SpawningType.OtherNPC, false, _currentSpawn.ProcessAsAdminSpawn, _currentSpawn.SpawnGroups, _currentSpawn.CurrentFactionTag, _spawnMatrix, velocity, _currentSpawn.IgnoreSafetyChecks, ownerOverride:_ownerOverride);

				if (result == true) {

					SpawnLogger.Write(_currentSpawn.ProfileSubtypeId + ": Spawn Successful", SpawnerDebugEnum.Spawning);
					_currentSpawn.SpawnCount++;
					_currentSpawn.FailedAttempts = 0;
					_currentSpawn.LastSpawnTime = MyAPIGateway.Session.GameDateTime;


				} else {

					SpawnLogger.Write(_currentSpawn.ProfileSubtypeId + ": Spawn Failed", SpawnerDebugEnum.Spawning);
					_currentSpawn.FailedAttempts++;

				}

			}

			if (_currentSpawn.SpawningType == SpawnTypeEnum.SpaceCargoShip) {

				SpawnLogger.Write(_currentSpawn.ProfileSubtypeId + ": Sending SpaceCargoShip Data to Spawner", SpawnerDebugEnum.Spawning);
				var spawns = _currentSpawn.SpawnGroups.Count > 0 ? _currentSpawn.SpawnGroups : null;
				var result = SpawnRequest.CalculateSpawn(_spawnMatrix.Translation, _currentSpawn.ProfileSubtypeId, SpawningType.SpaceCargoShip, _currentSpawn.IgnoreSafetyChecks, _currentSpawn.ProcessAsAdminSpawn, _currentSpawn.SpawnGroups, _currentSpawn.CurrentFactionTag);

				if (result == true) {

					SpawnLogger.Write(_currentSpawn.ProfileSubtypeId + ": Spawn Successful", SpawnerDebugEnum.Spawning);
					_currentSpawn.SpawnCount++;
					_currentSpawn.FailedAttempts = 0;
					_currentSpawn.LastSpawnTime = MyAPIGateway.Session.GameDateTime;


				} else {

					SpawnLogger.Write(_currentSpawn.ProfileSubtypeId + ": Spawn Failed", SpawnerDebugEnum.Spawning);
					_currentSpawn.FailedAttempts++;

				}

			}

			if (_currentSpawn.SpawningType == SpawnTypeEnum.RandomEncounter) {

				SpawnLogger.Write(_currentSpawn.ProfileSubtypeId + ": Sending RandomEncounter Data to Spawner", SpawnerDebugEnum.Spawning);
				var spawns = _currentSpawn.SpawnGroups.Count > 0 ? _currentSpawn.SpawnGroups : null;
				var result = SpawnRequest.CalculateSpawn(_spawnMatrix.Translation, _currentSpawn.ProfileSubtypeId, SpawningType.RandomEncounter, _currentSpawn.IgnoreSafetyChecks, _currentSpawn.ProcessAsAdminSpawn, _currentSpawn.SpawnGroups, _currentSpawn.CurrentFactionTag);

				if (result == true) {

					SpawnLogger.Write(_currentSpawn.ProfileSubtypeId + ": Spawn Successful", SpawnerDebugEnum.Spawning);
					_currentSpawn.SpawnCount++;
					_currentSpawn.FailedAttempts = 0;
					_currentSpawn.LastSpawnTime = MyAPIGateway.Session.GameDateTime;


				} else {

					SpawnLogger.Write(_currentSpawn.ProfileSubtypeId + ": Spawn Failed", SpawnerDebugEnum.Spawning);
					_currentSpawn.FailedAttempts++;

				}

			}

			if (_currentSpawn.SpawningType == SpawnTypeEnum.PlanetaryCargoShip) {

				SpawnLogger.Write(_currentSpawn.ProfileSubtypeId + ": Sending PlanetaryCargoShip Data to Spawner", SpawnerDebugEnum.Spawning);
				var spawns = _currentSpawn.SpawnGroups.Count > 0 ? _currentSpawn.SpawnGroups : null;
				var result = SpawnRequest.CalculateSpawn(_spawnMatrix.Translation, _currentSpawn.ProfileSubtypeId, SpawningType.PlanetaryCargoShip, _currentSpawn.IgnoreSafetyChecks, _currentSpawn.ProcessAsAdminSpawn, _currentSpawn.SpawnGroups, _currentSpawn.CurrentFactionTag);

				if (result == true) {

					SpawnLogger.Write(_currentSpawn.ProfileSubtypeId + ": Spawn Successful", SpawnerDebugEnum.Spawning);
					_currentSpawn.SpawnCount++;
					_currentSpawn.FailedAttempts = 0;
					_currentSpawn.LastSpawnTime = MyAPIGateway.Session.GameDateTime;


				} else {

					SpawnLogger.Write(_currentSpawn.ProfileSubtypeId + ": Spawn Failed", SpawnerDebugEnum.Spawning);
					_currentSpawn.FailedAttempts++;

				}

			}

			if (_currentSpawn.SpawningType == SpawnTypeEnum.PlanetaryInstallation) {

				SpawnLogger.Write(_currentSpawn.ProfileSubtypeId + ": Sending PlanetaryInstallation Data to Spawner", SpawnerDebugEnum.Spawning);
				var spawns = _currentSpawn.SpawnGroups.Count > 0 ? _currentSpawn.SpawnGroups : null;
				var result = SpawnRequest.CalculateSpawn(_spawnMatrix.Translation, _currentSpawn.ProfileSubtypeId, SpawningType.PlanetaryInstallation, _currentSpawn.IgnoreSafetyChecks, _currentSpawn.ProcessAsAdminSpawn, _currentSpawn.SpawnGroups, _currentSpawn.CurrentFactionTag);

				if (result == true) {

					SpawnLogger.Write(_currentSpawn.ProfileSubtypeId + ": Spawn Successful", SpawnerDebugEnum.Spawning);
					_currentSpawn.SpawnCount++;
					_currentSpawn.FailedAttempts = 0;
					_currentSpawn.LastSpawnTime = MyAPIGateway.Session.GameDateTime;


				} else {

					SpawnLogger.Write(_currentSpawn.ProfileSubtypeId + ": Spawn Failed", SpawnerDebugEnum.Spawning);
					_currentSpawn.FailedAttempts++;

				}

			}

			if (_currentSpawn.SpawningType == SpawnTypeEnum.BossEncounter) {

				SpawnLogger.Write(_currentSpawn.ProfileSubtypeId + ": Sending BossEncounter Data to Spawner", SpawnerDebugEnum.Spawning);
				var spawns = _currentSpawn.SpawnGroups.Count > 0 ? _currentSpawn.SpawnGroups : null;
				var result = SpawnRequest.CalculateSpawn(_spawnMatrix.Translation, _currentSpawn.ProfileSubtypeId, SpawningType.BossEncounter, _currentSpawn.IgnoreSafetyChecks, _currentSpawn.ProcessAsAdminSpawn, _currentSpawn.SpawnGroups, _currentSpawn.CurrentFactionTag);

				if (result == true) {

					SpawnLogger.Write(_currentSpawn.ProfileSubtypeId + ": Spawn Successful", SpawnerDebugEnum.Spawning);
					_currentSpawn.SpawnCount++;
					_currentSpawn.FailedAttempts = 0;
					_currentSpawn.LastSpawnTime = MyAPIGateway.Session.GameDateTime;


				} else {

					SpawnLogger.Write(_currentSpawn.ProfileSubtypeId + ": Spawn Failed", SpawnerDebugEnum.Spawning);
					_currentSpawn.FailedAttempts++;

				}

			}

			if (_currentSpawn.SpawningType == SpawnTypeEnum.Creature) {

				SpawnLogger.Write(_currentSpawn.ProfileSubtypeId + ": Sending Creature Data to Spawner", SpawnerDebugEnum.Spawning);
				var spawns = _currentSpawn.SpawnGroups.Count > 0 ? _currentSpawn.SpawnGroups : null;
				var result = SpawnRequest.CalculateSpawn(_spawnMatrix.Translation, _currentSpawn.ProfileSubtypeId, SpawningType.Creature, _currentSpawn.IgnoreSafetyChecks, _currentSpawn.ProcessAsAdminSpawn, _currentSpawn.SpawnGroups, _currentSpawn.CurrentFactionTag);

				if (result == true) {

					SpawnLogger.Write(_currentSpawn.ProfileSubtypeId + ": Spawn Successful", SpawnerDebugEnum.Spawning);
					_currentSpawn.SpawnCount++;
					_currentSpawn.FailedAttempts = 0;
					_currentSpawn.LastSpawnTime = MyAPIGateway.Session.GameDateTime;


				} else {

					SpawnLogger.Write(_currentSpawn.ProfileSubtypeId + ": Spawn Failed", SpawnerDebugEnum.Spawning);
					_currentSpawn.FailedAttempts++;

				}

			}

			if (_currentSpawn.FailedAttempts >= _currentSpawn.FailedAttemptsToIncreaseCount) {

				_currentSpawn.FailedAttempts = 0;
				_currentSpawn.SpawnCount++;
				_currentSpawn.LastSpawnTime = MyAPIGateway.Session.GameDateTime;

			}

			PerformNextSpawn();

		}

		private static void PerformNextSpawn() {

			_spawnInProgress = false;

			if (_pendingSpawns.Count > 0)
				BehaviorSpawnRequest();

		}

	}

}
