using ModularEncountersSystems.Spawners;
using ModularEncountersSystems.World;
using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRageMath;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning.Profiles;
using Sandbox.ModAPI;
using Sandbox.Game;

namespace ModularEncountersSystems.Spawning {

	public class PathDetails {

		public SpawningType SpawnType;
		public bool ValidPath;

		public MatrixD SpawnMatrix;
		public Vector3D StartCoords;

		public double PathDistance;
		public Vector3D PathDirection;

		public float MinSpeed;
		public float OverrideSpeed;

		public Vector3D CustomVelocity;

		public List<Vector3D> CreatureCoords;

		public StringBuilder PathDebugging;

		public PathDetails() {

			SpawnType = SpawningType.None;
			ValidPath = false;

			SpawnMatrix = MatrixD.Identity;
			StartCoords = Vector3D.Zero;

			PathDistance = 0;
			PathDirection = Vector3D.Zero;

			MinSpeed = -1;
			OverrideSpeed = -1;

			CustomVelocity = new Vector3D();

			PathDebugging = new StringBuilder();

		}

		public Vector3D GetPrefabStartCoords(Vector3D offset, EnvironmentEvaluation environment, double customStartAltitude = -1) {

			if (SpawnType == SpawningType.SpaceCargoShip || SpawnType == SpawningType.LunarCargoShip || SpawnType == SpawningType.PlanetaryCargoShip || SpawnType == SpawningType.GravityCargoShip) {

				if (customStartAltitude <= -1) {

					return Vector3D.Transform(offset, SpawnMatrix);

				} else {

					var surface = environment.NearestPlanet.SurfaceCoordsAtPosition(SpawnMatrix.Translation);
					var matrix = SpawnMatrix;
					surface = matrix.Up * customStartAltitude + surface;
					matrix.Translation = surface;
					return Vector3D.Transform(offset, matrix);

				}

			}

			if (SpawnType == SpawningType.RandomEncounter || SpawnType == SpawningType.DroneEncounter) {

				return SpawnMatrix.Translation + offset;

			}

			if (SpawnType == SpawningType.BossEncounter || SpawnType == SpawningType.BossSpace || SpawnType == SpawningType.BossGravity || SpawnType == SpawningType.BossAtmo || SpawnType == SpawningType.OtherNPC) {

				return Vector3D.Transform(offset, SpawnMatrix);

			}

			if (SpawnType == SpawningType.DryLandInstallation || SpawnType == SpawningType.WaterSurfaceStation) {

				var rough = Vector3D.Transform(offset, SpawnMatrix);
				var surface = environment.NearestPlanet.SurfaceCoordsAtPosition(rough);
				var up = environment.NearestPlanet.UpAtPosition(surface);
				return up * offset.Y + surface;

			}

			if (SpawnType == SpawningType.UnderWaterStation) {

				var rough = Vector3D.Transform(offset, SpawnMatrix);
				var surface = environment.NearestPlanet.SurfaceCoordsAtPosition(rough, true);
				var up = environment.NearestPlanet.UpAtPosition(surface);
				return up * offset.Y + surface;

			}

			return StartCoords;
		
		}

		public Vector3D GetPrefabEndCoords(Vector3D offset, EnvironmentEvaluation environment, double customEndAltitude = -1) {

			if (SpawnType == SpawningType.SpaceCargoShip || SpawnType == SpawningType.LunarCargoShip || SpawnType == SpawningType.GravityCargoShip) {

				return PathDirection * PathDistance + Vector3D.Transform(offset, SpawnMatrix);

			}

			if (SpawnType == SpawningType.PlanetaryCargoShip) {

				if (customEndAltitude > -1) {

					var start = Vector3D.Transform(offset, SpawnMatrix);
					var roughEnd = PathDirection * PathDistance + start;
					var surface = environment.NearestPlanet.SurfaceCoordsAtPosition(roughEnd);
					var matrix = SpawnMatrix;
					surface = matrix.Up * customEndAltitude + surface;
					matrix.Translation = surface;
					return Vector3D.Transform(offset, matrix);

				} else {

					var start = Vector3D.Transform(offset, SpawnMatrix);
					var roughEnd = PathDirection * PathDistance + start;
					var startCoreDist = environment.NearestPlanet.DistanceToCore(start);
					return environment.NearestPlanet.UpAtPosition(roughEnd) * startCoreDist + environment.NearestPlanet.Center();

				}

				

			}

			return GetPrefabStartCoords(offset, environment, customEndAltitude);

		}

	}

	public static class PathPlacements {

		public static PathDetails GetSpawnPlacement(SpawningType type, ref SpawningType spawnTypes, SpawnGroupCollection collection, EnvironmentEvaluation environment, MatrixD spawningMatrix) {

			var path = new PathDetails();

			//Space and Lunar CargoShips
			if (type == SpawningType.SpaceCargoShip) {

				path.MinSpeed = Settings.SpaceCargoShips.UseMinimumSpeed ? Settings.SpaceCargoShips.MinimumSpeed : -1;
				path.OverrideSpeed = Settings.SpaceCargoShips.UseSpeedOverride ? Settings.SpaceCargoShips.SpeedOverride: -1;

				if (environment.LunarCargoShipsEligible) {

					CalculateLunarPath(path, collection, environment);

				}
					
				else if (environment.SpaceCargoShipsEligible) {

					CalculateSpacePath(path, collection, environment);

				}

				return path;
			
			}

			//Random Encounter
			if (type == SpawningType.RandomEncounter) {

				CalculateRandomEncounterCoords(path, collection, environment);
				return path;

			}

			//Planetary and Gravity CargoShips
			if (type == SpawningType.PlanetaryCargoShip) {

				path.MinSpeed = Settings.PlanetaryCargoShips.UseMinimumSpeed ? Settings.PlanetaryCargoShips.MinimumSpeed : -1;
				path.OverrideSpeed = Settings.PlanetaryCargoShips.UseSpeedOverride ? Settings.PlanetaryCargoShips.SpeedOverride : -1;

				if (environment.PlanetaryCargoShipsEligible) {

					CalculateAtmoPath(path, collection, environment);
				
				} else if (environment.GravityCargoShipsEligible) {

					CalculateGravityPath(path, collection, environment);

				}

				return path;

			}

			//Planetary Installations
			if (type == SpawningType.PlanetaryInstallation) {

				//Prune spawnTypes
				var spawnTypesPruned = spawnTypes;

				if (spawnTypesPruned.HasFlag(SpawningType.DryLandInstallation) && !collection.Conditions.InstallationSpawnsOnDryLand)
					spawnTypesPruned &= ~SpawningType.DryLandInstallation;

				if (spawnTypesPruned.HasFlag(SpawningType.WaterSurfaceStation) && !collection.Conditions.InstallationSpawnsOnWaterSurface)
					spawnTypesPruned &= ~SpawningType.WaterSurfaceStation;

				if (spawnTypesPruned.HasFlag(SpawningType.UnderWaterStation) && !collection.Conditions.InstallationSpawnsUnderwater)
					spawnTypesPruned &= ~SpawningType.UnderWaterStation;

				spawnTypesPruned &= ~SpawningType.PlanetaryInstallation;

				CalculatePlanetaryInstallationCoords(path, collection, environment, spawnTypesPruned);
				path.SpawnType = spawnTypesPruned;
				return path;

			}

			//Boss Encounters
			if (type == SpawningType.BossEncounter) {

				if (environment.IsOnPlanet && spawnTypes.HasFlag(SpawningType.BossGravity)) {

					CalculatePlanetBossSignalCoords(path, collection, environment, spawnTypes);

				} else if (!environment.IsOnPlanet && spawnTypes.HasFlag(SpawningType.BossSpace)) {

					CalculateSpaceBossSignalCoords(path, collection, environment);

				}

				return path;

			}

			//Creatures
			if (type == SpawningType.Creature) {

				CalculateCreatureCoords(path, collection, environment);
				return path;

			}

			//Drone Encounters
			if (type == SpawningType.DroneEncounter) {

				CalculateDroneEncounterPath(path, collection, environment);
				return path;

			}

			//Other NPCs
			if (type == SpawningType.OtherNPC) {

				CalculateOtherPath(path, collection, environment, spawningMatrix);
				return path;

			}

			SpawnLogger.Write("No Path For Selected Type: " + type.ToString(), SpawnerDebugEnum.Pathing);
			return path;
		
		}

		public static PathDetails GetStaticSpawnPlacement(SpawningType type, SpawningType spawnTypes, SpawnGroupCollection collection, EnvironmentEvaluation environment, StaticEncounter encounter) {

			var path = new PathDetails();

			if (type == SpawningType.BossEncounter) {

				CalculateBossSpawnCoords(path, encounter.TriggerCoords, collection, spawnTypes);
				return path;

			}

			//Calculate Static Coords
			if (type == SpawningType.StaticEncounter) {

				CalculateStaticCoords(path, collection, environment, spawnTypes);
				return path;
			
			}

			SpawnLogger.Write("Static/Boss No Matching SpawnType For Pathing. Got: " + type.ToString() + " and " + spawnTypes.ToString(), SpawnerDebugEnum.Pathing);
			return path;

		}

		private static void CalculateSpacePath(PathDetails path, SpawnGroupCollection collection, EnvironmentEvaluation environment) {

			path.SpawnType = SpawningType.SpaceCargoShip;

			var voxelList = new List<MyVoxelBase>();
			var voxelSphere = new BoundingSphereD(environment.Position, 20000);
			MyGamePruningStructure.GetAllVoxelMapsInSphere(ref voxelSphere, voxelList);
			var planetGravitySphere = environment.NearestPlanet?.GetGravitySphere(0.05) ?? new BoundingSphereD(Vector3D.Zero, 0);

			for (int i = 0; i < Settings.SpaceCargoShips.MaxSpawnAttempts; i++) {

				//Setup Path
				var closestPathDist = MathTools.RandomBetween(Settings.SpaceCargoShips.MinPathDistanceFromPlayer, Settings.SpaceCargoShips.MaxPathDistanceFromPlayer);
				var randomDir = VectorHelper.RandomDirection();
				var randomPerpDir = VectorHelper.RandomPerpendicular(randomDir);
				var initialMatrix = MatrixD.CreateWorld(environment.Position, randomPerpDir, randomDir);
				var halfPoint = closestPathDist * randomDir + environment.Position;
				var pathDistance = MathTools.RandomBetween(Settings.SpaceCargoShips.MinPathDistance, Settings.SpaceCargoShips.MaxPathDistance);
				var pathDir = randomPerpDir;
				var playerBox = new BoundingBoxD(Vector3D.Transform(new Vector3D(-800, -800, -800), initialMatrix), Vector3D.Transform(new Vector3D(800, 800, 800), initialMatrix));

				if (environment.InsidePlanetaryLanes.Count > 0) {

					PlanetaryLane lane = null;

					if (!string.IsNullOrWhiteSpace(collection.Conditions.PlanetaryLanePlanetNameA) || !string.IsNullOrWhiteSpace(collection.Conditions.PlanetaryLanePlanetNameB)) {

						foreach (var ln in PlanetManager.Lanes) {

							if (ln.NameCheck(collection.Conditions.PlanetaryLanePlanetNameA) && ln.NameCheck(collection.Conditions.PlanetaryLanePlanetNameB)) {

								lane = ln;
								break;

							}

						}

					} else {

						lane = environment.InsidePlanetaryLanes[MathTools.RandomBetween(0, environment.InsidePlanetaryLanes.Count)];

					}

					if (lane != null) {

						//MyVisualScriptLogicProvider.ShowNotificationToAll("Planetary Lane Pathing", 4000);
						var dir = lane.LaneDirection * (double)MathTools.RandomSign();
						randomPerpDir = VectorHelper.RandomPerpendicular(dir);
						initialMatrix = MatrixD.CreateWorld(environment.Position, dir, randomPerpDir);
						halfPoint = closestPathDist * randomPerpDir + environment.Position;
						playerBox = new BoundingBoxD(Vector3D.Transform(new Vector3D(-800, -800, -800), initialMatrix), Vector3D.Transform(new Vector3D(800, 800, 800), initialMatrix));
						pathDir = dir;

					}

				}

				//Save To PathDetails
				path.StartCoords = pathDir * -(pathDistance / 2) + halfPoint;
				initialMatrix.Translation = path.StartCoords;
				path.SpawnMatrix = initialMatrix;
				path.PathDirection = pathDir;
				path.PathDistance = pathDistance;

				if (PlanetManager.InGravity(path.StartCoords))
					continue;

				//Check For Obstructions On Each Prefab
				bool obstructed = false;

				for (int j = 0; j < collection.PrefabIndexes.Count; j++) {

					var prefab = collection.SpawnGroup.SpawnGroup.Prefabs[collection.PrefabIndexes[j]];
					var prefabOffset = collection.SelectPrefabOffet(prefab.Position, j);

					var spawnGroup = collection.SpawnGroup.SpawnGroup;
					var startPath = path.GetPrefabStartCoords(prefabOffset, environment);
					var endPath = path.GetPrefabEndCoords(prefabOffset, environment);
					obstructed = IsSpacePathObstructed(planetGravitySphere, playerBox, startPath, endPath, voxelList, collection.SkipGridSpawnChecks, collection.SkipVoxelSpawnChecks);

					if (obstructed)
						break;

				}

				if (obstructed)
					continue;

				path.ValidPath = true;
				break;

			}

		}

		private static void CalculateLunarPath(PathDetails path, SpawnGroupCollection collection, EnvironmentEvaluation environment) {

			path.SpawnType = SpawningType.LunarCargoShip;

			var voxelList = new List<MyVoxelBase>();
			var voxelSphere = new BoundingSphereD(environment.Position, 20000);
			MyGamePruningStructure.GetAllVoxelMapsInSphere(ref voxelSphere, voxelList);
			var planetGravitySphere = environment.NearestPlanet.GetGravitySphere(0.05);

			for (int i = 0; i < Settings.SpaceCargoShips.MaxSpawnAttempts; i++) {

				//Setup Path
				var initialUp = environment.NearestPlanet.UpAtPosition(environment.Position);
				var randInitialPerp = VectorHelper.RandomPerpendicular(initialUp);
				var initialMatrix = MatrixD.CreateWorld(environment.Position, randInitialPerp, initialUp);
				var playerBox = new BoundingBoxD(Vector3D.Transform(new Vector3D(-800, -800, -800), initialMatrix), Vector3D.Transform(new Vector3D(800, 800, 800), initialMatrix));
				var closestPathDist = MathTools.RandomBetween(Settings.SpaceCargoShips.MinPathDistanceFromPlayer, Settings.SpaceCargoShips.MaxPathDistanceFromPlayer);
				var pathHeight = MathTools.RandomBetween(Settings.SpaceCargoShips.MinLunarSpawnHeight, Settings.SpaceCargoShips.MaxLunarSpawnHeight);
				var halfPathPointRough = randInitialPerp * closestPathDist + environment.Position;

				if (environment.NearestPlanet.IsPositionUnderground(halfPathPointRough))
					halfPathPointRough = environment.NearestPlanet.SurfaceCoordsAtPosition(halfPathPointRough);

				var upAtHalfPath = environment.NearestPlanet.UpAtPosition(halfPathPointRough);
				var randomPerpAtHalf = VectorHelper.RandomPerpendicular(upAtHalfPath);
				var halfPathPoint = upAtHalfPath * pathHeight + halfPathPointRough;
				var halfMatrix = MatrixD.CreateWorld(halfPathPoint, randomPerpAtHalf, upAtHalfPath);
				var pathDistance = MathTools.RandomBetween(Settings.SpaceCargoShips.MinPathDistance, Settings.SpaceCargoShips.MaxPathDistance);

				//Save To PathDetails
				path.StartCoords = halfMatrix.Forward * -(pathDistance / 2) + halfPathPoint;
				path.SpawnMatrix = MatrixD.CreateWorld(path.StartCoords, halfMatrix.Forward, halfMatrix.Up);
				path.PathDirection = halfMatrix.Forward;
				path.PathDistance = pathDistance;

				//Check For Obstructions On Each Prefab

				bool obstructed = false;

				for (int j = 0; j < collection.PrefabIndexes.Count; j++) {

					var prefab = collection.SpawnGroup.SpawnGroup.Prefabs[collection.PrefabIndexes[j]];
					var prefabOffset = collection.SelectPrefabOffet(prefab.Position, j);


					var startPath = Vector3D.Transform(prefabOffset, path.SpawnMatrix);
					var endPath = path.SpawnMatrix.Forward * pathDistance + startPath;
					obstructed = IsSpacePathObstructed(planetGravitySphere, playerBox, startPath, endPath, voxelList, collection.SkipGridSpawnChecks, collection.SkipVoxelSpawnChecks);

					if (obstructed)
						break;

				}

				if (obstructed)
					continue;

				path.ValidPath = true;
				break;

			}

		}

		

		private static void CalculateAtmoPath(PathDetails path, SpawnGroupCollection collection, EnvironmentEvaluation environment) {

			path.SpawnType = SpawningType.PlanetaryCargoShip;
			path.PathDebugging.Append("Planetary Cargo Ship Pathing Result: ").AppendLine();

			for (int i = 0; i < Settings.PlanetaryCargoShips.MaxSpawnAttempts; i++) {

				//Setup Path
				var initialUp = environment.NearestPlanet.UpAtPosition(environment.Position);
				var randInitialPerp = VectorHelper.RandomPerpendicular(initialUp);
				var initialMatrix = MatrixD.CreateWorld(environment.Position, randInitialPerp, initialUp);
				var playerBox = new BoundingBoxD(Vector3D.Transform(new Vector3D(-800, -800, -800), initialMatrix), Vector3D.Transform(new Vector3D(800, 800, 800), initialMatrix));
				var closestPathDist = MathTools.RandomBetween(Settings.PlanetaryCargoShips.MinPathDistanceFromPlayer, Settings.PlanetaryCargoShips.MaxPathDistanceFromPlayer);
				var pathHeight = MathTools.RandomBetween(Settings.PlanetaryCargoShips.MinSpawningAltitude, Settings.PlanetaryCargoShips.MaxSpawningAltitude);
				var halfPathPointRough = environment.NearestPlanet.SurfaceCoordsAtPosition(randInitialPerp * closestPathDist + environment.Position);

				var upAtHalfPath = environment.NearestPlanet.UpAtPosition(halfPathPointRough);
				var randomPerpAtHalf = VectorHelper.RandomPerpendicular(upAtHalfPath);
				var startPathPoint = upAtHalfPath * pathHeight + halfPathPointRough;

				var pathDistance = MathTools.RandomBetween(Settings.PlanetaryCargoShips.MinPathDistance, Settings.PlanetaryCargoShips.MaxPathDistance);
				
				if (environment.NearestPlanet.Planet.GetAirDensity(startPathPoint) < Settings.PlanetaryCargoShips.MinAirDensity) {

					pathHeight = MathTools.Average(Settings.PlanetaryCargoShips.MinSpawningAltitude, Settings.PlanetaryCargoShips.MaxSpawningAltitude);
					startPathPoint = upAtHalfPath * pathHeight + halfPathPointRough;

					if (environment.NearestPlanet.Planet.GetAirDensity(startPathPoint) < Settings.PlanetaryCargoShips.MinAirDensity) {

						pathHeight = Settings.PlanetaryCargoShips.MinSpawningAltitude;
						startPathPoint = upAtHalfPath * pathHeight + halfPathPointRough;

					}

				}

				var startMatrix = MatrixD.CreateWorld(startPathPoint, randomPerpAtHalf, upAtHalfPath);

				var pathDirection = VectorHelper.GetDirectionClosestTo(Vector3D.Normalize(environment.Position - startPathPoint), startMatrix.Forward, startMatrix.Right, startMatrix.Backward, startMatrix.Left);
				pathDirection = VectorHelper.GetNextClockwiseDirection(pathDirection, startMatrix);
				startMatrix = MatrixD.CreateWorld(startPathPoint, pathDirection, upAtHalfPath);

				//Save To PathDetails
				path.StartCoords = startPathPoint;
				path.SpawnMatrix = startMatrix;
				path.PathDirection = pathDirection;
				path.PathDistance = pathDistance;

				//Check For Obstructions On Each Prefab

				bool obstructed = false;

				for (int j = 0; j < collection.PrefabIndexes.Count; j++) {

					var prefab = collection.SpawnGroup.SpawnGroup.Prefabs[collection.PrefabIndexes[j]];
					var prefabOffset = collection.SelectPrefabOffet(prefab.Position, j);

					var startPath = Vector3D.Transform(prefabOffset, path.SpawnMatrix);
					var startAltitude = environment.NearestPlanet.DistanceToCore(startPath);
					var endPathRough = path.SpawnMatrix.Forward * pathDistance + startPath;
					var endPathUp = environment.NearestPlanet.UpAtPosition(endPathRough);
					var endPath = endPathUp * startAltitude + environment.NearestPlanet.Center();
					var endSurface = environment.NearestPlanet.SurfaceCoordsAtPosition(endPath);

					if (environment.NearestPlanet.AltitudeAtPosition(endPath) < Settings.PlanetaryCargoShips.MinSpawningAltitude) {

						endPath = endPathUp * Settings.PlanetaryCargoShips.MinSpawningAltitude + endSurface;

					}

					/*
					var midPathAltitude = MidPointAltitude(startPath, endPath, pathDistance, environment);

					if (midPathAltitude < Settings.PlanetaryCargoShips.MinPathAltitude) {

						//TODO: Recalculate A Bunch Of Crap
						svar altitudeAddition = (Settings.PlanetaryCargoShips.MinPathAltitude - midPathAltitude) * 1.15;
						startPathPoint = upAtHalfPath * (pathHeight + altitudeAddition) + halfPathPointRough;

					}
					*/

					if (!collection.Conditions.SkipAirDensityCheck) {

						var startDensity = environment.NearestPlanet.Planet.GetAirDensity(startPath);
						if (startDensity < Settings.PlanetaryCargoShips.MinAirDensity) {

							path.PathDebugging.Append(" - Attempt Failed: Air Density Too Low At Start Path").AppendLine();
							obstructed = true;
							break;

						}

						var endDensity = environment.NearestPlanet.Planet.GetAirDensity(endPath);
						if (endDensity < Settings.PlanetaryCargoShips.MinAirDensity) {

							path.PathDebugging.Append(" - Attempt Failed: Air Density Too Low At End Path").AppendLine();
							obstructed = true;
							break;

						}

					}

					obstructed = IsPlanetPathObstructed(playerBox, startPath, endPath, pathDistance, environment, collection.SkipGridSpawnChecks, collection.SkipVoxelSpawnChecks, path.PathDebugging);

					if (obstructed) {

						path.PathDebugging.Append(" - Attempt Failed: Path Obstructed By Voxel or Entity").AppendLine();
						break;

					}
	
				}

				if (obstructed)
					continue;

				path.ValidPath = true;
				break;

			}

		}

		private static void CalculateGravityPath(PathDetails path, SpawnGroupCollection collection, EnvironmentEvaluation environment) {

			path.SpawnType = SpawningType.GravityCargoShip;

			var voxelList = new List<MyVoxelBase>();
			var voxelSphere = new BoundingSphereD(environment.Position, 20000);
			MyGamePruningStructure.GetAllVoxelMapsInSphere(ref voxelSphere, voxelList);
			var planetGravitySphere = environment.NearestPlanet.GetGravitySphere(0.05);

			for (int i = 0; i < Settings.PlanetaryCargoShips.MaxSpawnAttempts; i++) {

				var initialUp = environment.NearestPlanet.UpAtPosition(environment.Position);
				var randInitialPerp = VectorHelper.RandomPerpendicular(initialUp);
				var initialMatrix = MatrixD.CreateWorld(environment.Position, randInitialPerp, initialUp);
				var playerBox = new BoundingBoxD(Vector3D.Transform(new Vector3D(-800, -800, -800), initialMatrix), Vector3D.Transform(new Vector3D(800, 800, 800), initialMatrix));
				var closestPathDist = MathTools.RandomBetween(Settings.PlanetaryCargoShips.MinPathDistanceFromPlayer, Settings.PlanetaryCargoShips.MaxPathDistanceFromPlayer);
				var roughPathHeight = MathTools.RandomBetween(Settings.PlanetaryCargoShips.MinSpawningAltitude, Settings.PlanetaryCargoShips.MaxSpawningAltitude);
				var pathHeight = MathTools.RandomBetween(-roughPathHeight, roughPathHeight);
				var halfPathPointRough = randInitialPerp * closestPathDist + environment.Position;

				var gravityAtHalf = environment.NearestPlanet.Gravity.GetGravityMultiplier(halfPathPointRough);

				if ((collection.Conditions.MinGravity > -1 && gravityAtHalf < collection.Conditions.MinGravity) || (collection.Conditions.MaxGravity > -1 && gravityAtHalf > collection.Conditions.MaxGravity)) {

					continue;
				
				}

				var upAtHalfPath = environment.NearestPlanet.UpAtPosition(halfPathPointRough);
				var randomPerpAtHalf = VectorHelper.RandomPerpendicular(upAtHalfPath);
				var startPathPoint = upAtHalfPath * pathHeight + halfPathPointRough;
				var startMatrix = MatrixD.CreateWorld(startPathPoint, randomPerpAtHalf, upAtHalfPath);

				var pathDirection = VectorHelper.GetDirectionClosestTo(Vector3D.Normalize(environment.Position - startPathPoint), startMatrix.Forward, startMatrix.Right, startMatrix.Backward, startMatrix.Left);
				pathDirection = VectorHelper.GetNextClockwiseDirection(pathDirection, startMatrix);
				startMatrix = MatrixD.CreateWorld(startPathPoint, pathDirection, upAtHalfPath);

				var pathDistance = MathTools.RandomBetween(Settings.PlanetaryCargoShips.MinPathDistance, Settings.PlanetaryCargoShips.MaxPathDistance);

				//Save To PathDetails
				path.StartCoords = startPathPoint;
				path.SpawnMatrix = startMatrix;
				path.PathDirection = pathDirection;
				path.PathDistance = pathDistance;

				bool obstructed = false;

				for (int j = 0; j < collection.PrefabIndexes.Count; j++) {

					var prefab = collection.SpawnGroup.SpawnGroup.Prefabs[collection.PrefabIndexes[j]];
					var prefabOffset = collection.SelectPrefabOffet(prefab.Position, j);

					var startPath = Vector3D.Transform(prefabOffset, path.SpawnMatrix);
					var endPath = path.SpawnMatrix.Forward * pathDistance + startPath;

					var ray = new RayD(startPath, Vector3D.Normalize(endPath - startPath));

					//Check For Intersection With Player Box
					if (ray.Intersects(playerBox).HasValue) {

						//Prefab Path Intersecting Player Box, Abandoning Path
						obstructed = true;
						break;

					}

					if (IsGridWithinMinDistance(startPath, Settings.PlanetaryCargoShips.MinSpawnDistFromEntities, collection.SkipGridSpawnChecks)) {

						obstructed = true;
						break;

					}

					if (IsVoxelIntersecting(voxelList, startPath, endPath, Settings.PlanetaryCargoShips.MinSpawnDistFromEntities, collection.SkipVoxelSpawnChecks)) {

						obstructed = true;
						break;

					}


				}

				if (obstructed)
					continue;

				path.ValidPath = true;
				break;

			}

		}

		private static void CalculateRandomEncounterCoords(PathDetails path, SpawnGroupCollection collection, EnvironmentEvaluation environment) {

			path.SpawnType = SpawningType.RandomEncounter;

			var voxelList = new List<MyVoxelBase>();
			var voxelSphere = new BoundingSphereD(environment.Position, 20000);
			MyGamePruningStructure.GetAllVoxelMapsInSphere(ref voxelSphere, voxelList);

			for (int i = 0; i < Settings.RandomEncounters.SpawnAttempts; i++) {

				//Determine Initial Coords
				var randDir = VectorHelper.RandomDirection();
				var randDist = MathTools.RandomBetween(Settings.RandomEncounters.MinSpawnDistanceFromPlayer, Settings.RandomEncounters.MaxSpawnDistanceFromPlayer);
				var initialCoords = randDir * randDist + environment.Position;

				if(environment.NearestPlanet != null && environment.NearestPlanet.IsPositionInGravity(initialCoords))
					initialCoords = -randDir * randDist + environment.Position;



				//Set Paths
				path.StartCoords = initialCoords;
				path.SpawnMatrix = MatrixD.CreateWorld(initialCoords);
				path.PathDirection = -randDir;
				path.PathDistance = randDist;

				//Check for Voxels or Grids
				bool obstructed = false;

				for (int j = 0; j < collection.PrefabIndexes.Count; j++) {

					var prefab = collection.SpawnGroup.SpawnGroup.Prefabs[collection.PrefabIndexes[j]];
					var prefabOffset = collection.SelectPrefabOffet(prefab.Position, j);

					var startPath = Vector3D.Transform(prefabOffset, path.SpawnMatrix);

					if (IsGridWithinMinDistance(startPath, Settings.RandomEncounters.MinDistanceFromOtherEntities, collection.SkipGridSpawnChecks)) {

						obstructed = true;
						break;

					}

					if (IsVoxelIntersecting(voxelList, startPath + (path.PathDirection + 500), startPath + (path.PathDirection - 500), Settings.RandomEncounters.MinDistanceFromOtherEntities, collection.SkipVoxelSpawnChecks)) {

						obstructed = true;
						break;

					}

					if (obstructed)
						continue;

					path.ValidPath = true;
					break;

				}

			}

		}

		private static void CalculatePlanetaryInstallationCoords(PathDetails path, SpawnGroupCollection collection, EnvironmentEvaluation environment, SpawningType spawnType) {

			//path.SpawnType = SpawningType.PlanetaryInstallation;

			var upAtCoords = environment.NearestPlanet.UpAtPosition(environment.Position);
			var randomDir = VectorHelper.RandomPerpendicular(upAtCoords);
			var searchMatrix = MatrixD.CreateWorld(environment.Position, randomDir, upAtCoords);
			double extraDistance = 0;
			double terrainVarianceCheckTarget = Settings.PlanetaryInstallations.SmallTerrainCheckDistance;

			SpawnLogger.Write("Installation Spawning With Condition: " + collection.Conditions.ProfileSubtypeId, SpawnerDebugEnum.Pathing);
			SpawnLogger.Write("Installation Size: " + collection.Conditions.PlanetaryInstallationType ?? "Null", SpawnerDebugEnum.Pathing);

			if (collection.Conditions.PlanetaryInstallationType == "Medium") {

				SpawnLogger.Write("Applying MediumSpawnDistanceIncrement of: " + Settings.PlanetaryInstallations.MediumSpawnDistanceIncrement, SpawnerDebugEnum.Pathing);
				extraDistance = Settings.PlanetaryInstallations.MediumSpawnDistanceIncrement;
				terrainVarianceCheckTarget = Settings.PlanetaryInstallations.MediumTerrainCheckDistance;

			}

			if (collection.Conditions.PlanetaryInstallationType == "Large") {

				SpawnLogger.Write("Applying LargeSpawnDistanceIncrement of: " + Settings.PlanetaryInstallations.LargeSpawnDistanceIncrement, SpawnerDebugEnum.Pathing);
				extraDistance = Settings.PlanetaryInstallations.LargeSpawnDistanceIncrement;
				terrainVarianceCheckTarget = Settings.PlanetaryInstallations.LargeTerrainCheckDistance;

			}

			var startDist = Settings.PlanetaryInstallations.MinimumSpawnDistanceFromPlayers + extraDistance;
			var endDist = Settings.PlanetaryInstallations.MaximumSpawnDistanceFromPlayers + extraDistance;

			var searchDirections = new List<Vector3D>();
			var checkDirections = new List<Vector3D>();

			PopulateSearchDirections(searchDirections, searchMatrix, Settings.PlanetaryInstallations.AggressivePathCheck);
			
			int searchDirectionAttempts = 0; //This is for Debug

			foreach (var searchDirection in searchDirections) {

				searchDirectionAttempts++; 
				double searchIncrement = startDist - Settings.PlanetaryInstallations.SearchPathIncrement;

				while (searchIncrement < endDist) {

					searchIncrement += Settings.PlanetaryInstallations.SearchPathIncrement;
					var checkCoords = searchDirection * searchIncrement + searchMatrix.Translation;
					var coords = environment.NearestPlanet.SurfaceCoordsAtPosition(checkCoords, true);
					bool isUnderwater = environment.NearestPlanet.IsPositionUnderwater(coords);
					double depth = environment.NearestPlanet.WaterDepthAtPosition(coords);

					//SpawnLogger.Write("Underwater: " + isUnderwater, SpawnerDebugEnum.Pathing);
					//SpawnLogger.Write("Depth: " + depth, SpawnerDebugEnum.Pathing);

					if (!IsPositionSurfaceValid(coords, environment, collection, spawnType, isUnderwater, depth))
						continue;

					if (IsGridWithinMinDistance(coords, Settings.PlanetaryInstallations.MinimumSpawnDistanceFromOtherGrids, collection.SkipGridSpawnChecks))
						continue;

					var checkUpDir = Vector3D.Normalize(coords - environment.NearestPlanet.Center());
					var checkForwardDir = VectorHelper.RandomPerpendicular(checkUpDir);
					var checkMatrix = MatrixD.CreateWorld(coords, checkForwardDir, checkUpDir);

					PopulateSearchDirections(checkDirections, checkMatrix, Settings.PlanetaryInstallations.AggressiveTerrainCheck);

					var distToCore = environment.NearestPlanet.DistanceToCore(coords);
					bool badPosition = false;

					foreach (var checkDirection in checkDirections) {

						double terrainCheckIncrement = 0;

						while (terrainCheckIncrement < terrainVarianceCheckTarget) {

							var checkTerrainCoords = checkDirection * terrainCheckIncrement + coords;
							var checkTerrainSurfaceCoords = environment.NearestPlanet.SurfaceCoordsAtPosition(checkTerrainCoords, true);
							double checkDepth = isUnderwater ? environment.NearestPlanet.WaterDepthAtPosition(checkTerrainSurfaceCoords) : depth;

							//Check Surface or Water

							//TODO, Check if this breaks stuff, change DryLandIntallation back to PlanetaryInstallation if it does
							if (spawnType.HasFlag(SpawningType.DryLandInstallation) || spawnType.HasFlag(SpawningType.UnderWaterStation)) {

								if (!IsTerrainLevelWithinTolerance(distToCore, checkTerrainSurfaceCoords, environment)) {

									badPosition = true;
									break;

								}

								if (spawnType.HasFlag(SpawningType.UnderWaterStation) && checkDepth < collection.Conditions.MinWaterDepth) {

									badPosition = true;
									break;

								}

							} else if (spawnType.HasFlag(SpawningType.WaterSurfaceStation)) {

								if (checkDepth < collection.Conditions.MinWaterDepth) {

									badPosition = true;
									break;

								}

							} else {

								badPosition = true;
								break;
							
							}

							terrainCheckIncrement += Settings.PlanetaryInstallations.TerrainCheckIncrementDistance;

						}

						if (badPosition)
							break;

					}

					if (badPosition == false || collection.Conditions.SkipTerrainCheck) {

						//CheckSpawnType
						var finalCoords = spawnType.HasFlag(SpawningType.WaterSurfaceStation) ? environment.NearestPlanet.SurfaceCoordsAtPosition(checkCoords, false) : coords;

						path.ValidPath = true;
						path.StartCoords = finalCoords;
						var finalUp = environment.NearestPlanet.UpAtPosition(finalCoords);
						var finalForward = VectorHelper.RandomPerpendicular(finalUp);
						path.SpawnMatrix = MatrixD.CreateWorld(finalCoords, finalForward, finalUp);

						return;

					}

				}

			}

		}

		private static void CalculateDroneEncounterPath(PathDetails path, SpawnGroupCollection collection, EnvironmentEvaluation environment) {

			path.SpawnType = SpawningType.DroneEncounter;

			var voxelList = new List<MyVoxelBase>();
			var voxelSphere = new BoundingSphereD(environment.Position, 20000);
			MyGamePruningStructure.GetAllVoxelMapsInSphere(ref voxelSphere, voxelList);

			for (int i = 0; i < Settings.DroneEncounters.MaxSpawnAttempts; i++) {

				//Setup Path

				var coords = Vector3D.Zero;
				var distance = MathTools.RandomBetween(collection.Conditions.MinDroneDistance, collection.Conditions.MaxDroneDistance);

				if (!environment.IsOnPlanet) {

					var dir = VectorHelper.RandomDirection();
					coords = distance * dir + environment.Position;

					if(environment.NearestPlanet != null && environment.NearestPlanet.IsPositionInGravity(coords))
						coords = distance * -dir + environment.Position;

					var dirToPosition = Vector3D.Normalize(environment.Position - coords);

					path.SpawnMatrix = MatrixD.CreateWorld(coords, dirToPosition, VectorHelper.RandomPerpendicular(dirToPosition));
					path.PathDirection = dirToPosition;
					


				} else {

					var randomCompass = VectorHelper.RandomPerpendicular(environment.NearestPlanet.UpAtPosition(environment.Position));
					var surfaceCoords = environment.NearestPlanet.SurfaceCoordsAtPosition(randomCompass * distance + environment.Position);
					var addAltitude = collection.Conditions.DroneInheritsSourceAltitude ? Math.Abs(environment.AltitudeAtPosition) : 0;
					var altitude = MathTools.RandomBetween(collection.Conditions.MinDroneAltitude, collection.Conditions.MaxDroneAltitude) + addAltitude;
					var upAtSurface = environment.NearestPlanet.UpAtPosition(surfaceCoords);
					coords = upAtSurface * altitude + surfaceCoords;
					var randDirAtUp = VectorHelper.RandomPerpendicular(upAtSurface);
					path.SpawnMatrix = MatrixD.CreateWorld(coords, randDirAtUp, upAtSurface);
					path.PathDirection = randDirAtUp;

				}


				//Save To PathDetails
				path.StartCoords = coords;
				path.PathDistance = distance;

				//Check For Obstructions On Each Prefab

				bool obstructed = false;

				for (int j = 0; j < collection.PrefabIndexes.Count; j++) {

					var prefab = collection.SpawnGroup.SpawnGroup.Prefabs[collection.PrefabIndexes[j]];
					var prefabOffset = collection.SelectPrefabOffet(prefab.Position, j);
					var startPath = Vector3D.Transform(prefabOffset, path.SpawnMatrix);

					obstructed = IsGridWithinMinDistance(path.StartCoords, Settings.DroneEncounters.MinDistanceFromOtherEntities, collection.SkipGridSpawnChecks) && IsVoxelIntersecting(voxelList, path.StartCoords + new Vector3D(-300, -300, -300), path.StartCoords + new Vector3D(300, 300, 300), -1, collection.SkipVoxelSpawnChecks);

					if (obstructed)
						break;

				}

				if (obstructed)
					continue;

				path.ValidPath = true;
				break;

			}

		}


		private static void PopulateSearchDirections(List<Vector3D> directions, MatrixD matrix, bool useAggressive) {

			directions.Clear();
			directions.Add(matrix.Forward);
			directions.Add(matrix.Backward);
			directions.Add(matrix.Left);
			directions.Add(matrix.Right);

			if (Settings.PlanetaryInstallations.AggressiveTerrainCheck == true) {

				directions.Add(Vector3D.Normalize(matrix.Forward + matrix.Left));
				directions.Add(Vector3D.Normalize(matrix.Forward + matrix.Right));
				directions.Add(Vector3D.Normalize(matrix.Backward + matrix.Left));
				directions.Add(Vector3D.Normalize(matrix.Backward + matrix.Right));

			}

		}

		private static bool IsTerrainLevelWithinTolerance(double mainDist, Vector3D branchCoords, EnvironmentEvaluation environment) {

			var branchDist = environment.NearestPlanet.DistanceToCore(branchCoords);
			var differenceToCore = branchDist - mainDist;

			if (differenceToCore < Settings.PlanetaryInstallations.MinimumTerrainVariance || differenceToCore > Settings.PlanetaryInstallations.MaximumTerrainVariance)
				return false;

			return true;

		}

		private static bool IsPositionSurfaceValid(Vector3D coords, EnvironmentEvaluation environment, SpawnGroupCollection collection, SpawningType spawnType, bool isUnderwater, double depth) {

			//Check For Terrain Validation
			if (collection.Conditions.InstallationTerrainValidation && !spawnType.HasFlag(SpawningType.WaterSurfaceStation)) {

				var terrain = environment.NearestPlanet.Planet.GetMaterialAt(ref coords);

				if (terrain != null) {

					if (!collection.Conditions.AllowedTerrainTypes.Contains(terrain.MaterialTypeName)) {

						return false;

					}

				} else {

					return false;

				}

			}

			//Check For Regular Surface Spawn
			if (spawnType.HasFlag(SpawningType.DryLandInstallation) && !isUnderwater) {

				return true;
			
			}

			if (depth >= collection.Conditions.MinWaterDepth) {

				//Check For Water Surface Spawn
				if (spawnType.HasFlag(SpawningType.WaterSurfaceStation) && isUnderwater) {

					return true;

				}

				//Check For Underwater Surface Spawn
				if (spawnType.HasFlag(SpawningType.UnderWaterStation) && isUnderwater) {

					return true;

				}

			}

			return false;
		
		}

		private static void CalculateSpaceBossSignalCoords(PathDetails path, SpawnGroupCollection collection, EnvironmentEvaluation environment) {

			path.SpawnType = SpawningType.BossSpace;

			var voxelList = new List<MyVoxelBase>();
			var voxelSphere = new BoundingSphereD(environment.Position, 20000);
			MyGamePruningStructure.GetAllVoxelMapsInSphere(ref voxelSphere, voxelList);

			for (int i = 0; i < Settings.BossEncounters.PathCalculationAttempts; i++) {

				//Determine Initial Coords
				var randDir = VectorHelper.RandomDirection();
				var randDist = MathTools.RandomBetween(Settings.BossEncounters.MinCoordsDistanceSpace, Settings.BossEncounters.MaxCoordsDistanceSpace);
				var initialCoords = randDir * randDist + environment.Position;

				if (environment.NearestPlanet != null && environment.NearestPlanet.IsPositionInGravity(initialCoords))
					initialCoords = -randDir * randDist + environment.Position;

				//Set Paths
				path.StartCoords = initialCoords;
				path.SpawnMatrix = MatrixD.CreateWorld(initialCoords, -randDir, VectorHelper.RandomPerpendicular(-randDir));
				path.PathDirection = -randDir;
				path.PathDistance = randDist;

				//Check for Voxels or Grids
				
				if (IsGridWithinMinDistance(initialCoords, Settings.BossEncounters.MinSignalDistFromOtherEntities, collection.SkipGridSpawnChecks)) {

					continue;

				}

				if (IsVoxelIntersecting(voxelList, initialCoords + (path.PathDirection + 500), initialCoords + (path.PathDirection - 500), Settings.BossEncounters.MinSignalDistFromOtherEntities, collection.SkipVoxelSpawnChecks)) {

					continue;

				}

				path.ValidPath = true;
				break;

			}

		}

		private static void CalculatePlanetBossSignalCoords(PathDetails path, SpawnGroupCollection collection, EnvironmentEvaluation environment, SpawningType spawnType) {

			path.SpawnType = SpawningType.BossGravity;

			for (int i = 0; i < Settings.BossEncounters.PathCalculationAttempts; i++) {

				var initialUp = environment.NearestPlanet.UpAtPosition(environment.Position);
				var initialPerp = VectorHelper.RandomPerpendicular(initialUp);
				var initialDist = MathTools.RandomBetween(Settings.BossEncounters.MinCoordsDistancePlanet, Settings.BossEncounters.MaxCoordsDistancePlanet);
				var surfaceCoords = environment.NearestPlanet.SurfaceCoordsAtPosition(initialPerp * initialDist + environment.Position);
				var surfaceUp = environment.NearestPlanet.UpAtPosition(surfaceCoords);
				var randUpDistance = Settings.BossEncounters.MinPlanetAltitude;
				var coords = surfaceUp * randUpDistance + surfaceCoords;

				if (spawnType.HasFlag(SpawningType.BossAtmo) && !collection.Conditions.SkipAirDensityCheck) {

					if (environment.NearestPlanet.Planet.GetAirDensity(coords) < Settings.BossEncounters.MinAirDensity)
						continue;

					path.SpawnType = SpawningType.BossAtmo;

				}

				path.StartCoords = coords;
				path.SpawnMatrix = MatrixD.CreateWorld(coords, VectorHelper.RandomPerpendicular(surfaceUp), surfaceUp);
				path.PathDirection = initialPerp;
				path.PathDistance = initialDist;

				if (IsGridWithinMinDistance(coords, Settings.BossEncounters.MinSignalDistFromOtherEntities, collection.SkipGridSpawnChecks)) {

					continue;

				}

				path.ValidPath = true;
				break;

			}

		}

		private static void CalculateBossSpawnCoords(PathDetails path, Vector3D coords, SpawnGroupCollection collection, SpawningType spawnType) {

			SpawnLogger.Write("Attempting Boss Encounter Coordinate Generation", SpawnerDebugEnum.Pathing);

			path.SpawnType = SpawningType.BossEncounter;

			var environment = new EnvironmentEvaluation(coords);

			var voxelList = new List<MyVoxelBase>();
			var voxelSphere = new BoundingSphereD(coords, 20000);
			MyGamePruningStructure.GetAllVoxelMapsInSphere(ref voxelSphere, voxelList);

			for (int i = 0; i < Settings.BossEncounters.PathCalculationAttempts; i++) {

				var randDist = MathTools.RandomBetween(Settings.BossEncounters.MinSpawnDistFromCoords, Settings.BossEncounters.MaxSpawnDistFromCoords);

				if (spawnType.HasFlag(SpawningType.BossSpace) && !environment.IsOnPlanet) {

					SpawnLogger.Write("Boss is in Space", SpawnerDebugEnum.Pathing);
					var randDir = VectorHelper.RandomDirection();
					var roughCoords = randDir * randDist + coords;

					if (environment.NearestPlanet?.IsPositionInGravity(roughCoords) ?? false)
						roughCoords = -randDir * randDist + coords;

					if (environment.NearestPlanet?.IsPositionInGravity(roughCoords) ?? false)
						continue;

					path.StartCoords = roughCoords;
					path.SpawnMatrix = MatrixD.CreateWorld(path.StartCoords, -randDir, VectorHelper.RandomPerpendicular(-randDir));

				} else if (spawnType.HasFlag(SpawningType.BossGravity) && environment.IsOnPlanet) {

					SpawnLogger.Write("Boss is in Gravity", SpawnerDebugEnum.Pathing);
					var upDir = environment.NearestPlanet.UpAtPosition(coords);
					var perp = VectorHelper.RandomPerpendicular(upDir);
					var roughCoords = environment.NearestPlanet.SurfaceCoordsAtPosition(perp * randDist + coords);
					var upRough = environment.NearestPlanet.UpAtPosition(roughCoords);
					var upDist = MathTools.RandomBetween(Settings.BossEncounters.MinSpawnDistFromCoords, Settings.BossEncounters.MaxSpawnDistFromCoords);
					var coordsCandidate = upRough * upDist + roughCoords;

					if (spawnType.HasFlag(SpawningType.BossAtmo) && !collection.Conditions.SkipAirDensityCheck) {

						if (environment.NearestPlanet.Planet.GetAirDensity(coordsCandidate) < Settings.BossEncounters.MinAirDensity)
							continue;

					}

					path.StartCoords = coordsCandidate;
					path.SpawnMatrix = MatrixD.CreateWorld(path.StartCoords, VectorHelper.RandomPerpendicular(upRough), upRough);

				} else {

					SpawnLogger.Write("Unidentified Boss SpawnType For Path", SpawnerDebugEnum.Pathing);
					continue;
				
				}

				bool badPosition = false;

				for (int j = 0; j < collection.PrefabIndexes.Count; j++) {

					var prefab = collection.SpawnGroup.SpawnGroup.Prefabs[collection.PrefabIndexes[j]];
					var prefabOffset = collection.SelectPrefabOffet(prefab.Position, j);

					var offsetCoords = Vector3D.Transform(prefabOffset, path.SpawnMatrix);

					if (IsGridWithinMinDistance(offsetCoords, Settings.BossEncounters.MinSignalDistFromOtherEntities, collection.SkipGridSpawnChecks)) {

						badPosition = true;
						break;

					}

				}

				if (badPosition)
					continue;

				path.ValidPath = true;
				return;

			}

		}

		private static void CalculateStaticCoords(PathDetails path, SpawnGroupCollection collection, EnvironmentEvaluation environment, SpawningType spawnTypes) {

			var matrix = MatrixD.Identity;
			bool generateDirections = false;

			if (collection.StaticEncounterInstance.ExactLocationForward != Vector3D.Zero) {

				if (Vector3D.ArePerpendicular(ref collection.StaticEncounterInstance.ExactLocationForward, ref collection.StaticEncounterInstance.ExactLocationUp)) {

					matrix = MatrixD.CreateWorld(collection.StaticEncounterInstance.ExactLocationCoords, collection.StaticEncounterInstance.ExactLocationForward, collection.StaticEncounterInstance.ExactLocationUp);

				} else {

					SpawnLogger.Write("Provided Directions Not Perpendicular, Generating Default Directions", SpawnerDebugEnum.Pathing);
					generateDirections = true;

				}
			
			} else {

				generateDirections = true;

			}


			if (spawnTypes.HasFlag(SpawningType.StaticEncounterPlanet) && environment.IsOnPlanet) {

				path.SpawnType = SpawningType.StaticEncounterPlanet;

				if (generateDirections) {

					var coords = collection.StaticEncounterInstance.ExactLocationCoords;
					var up = environment.NearestPlanet.UpAtPosition(coords);
					var forward = VectorHelper.RandomPerpendicular(up);
					matrix = MatrixD.CreateWorld(coords, forward, up);

				}

			} else {

				path.SpawnType = SpawningType.StaticEncounterSpace;

				if (generateDirections) {

					matrix.Translation = collection.StaticEncounterInstance.ExactLocationCoords;

				}

			}

			path.SpawnMatrix = matrix;
			path.StartCoords = matrix.Translation;

			if (IsGridWithinMinDistance(path.StartCoords, 250, collection.SkipGridSpawnChecks)) {

				return;

			}

			path.ValidPath = true;

		}

		private static void CalculateCreatureCoords(PathDetails path, SpawnGroupCollection collection, EnvironmentEvaluation environment) {

			path.SpawnType = SpawningType.Creature;
			path.CreatureCoords = new List<Vector3D>();

			var up = environment.NearestPlanet.UpAtPosition(environment.Position);
			var cells = new List<Vector3I>();

			for (int i = 0; i < collection.Conditions.MaxCreatureCount; i++) {

				for (int j = 0; j < Settings.Creatures.CoordsAttemptsPerCreature; j++) {

					var forward = VectorHelper.RandomPerpendicular(up);
					var dist = MathTools.RandomBetween(collection.Conditions.MinCreatureDistance, collection.Conditions.MaxCreatureDistance);
					var roughcoords = forward * dist + environment.Position;
					var coords = environment.NearestPlanet.SurfaceCoordsAtPosition(roughcoords, true) + up;
					var upCoords = up * 100 + coords;
					bool badCoords = false;

					//Water
					if (collection.Conditions.CanSpawnUnderwater) {

						if (environment.NearestPlanet.WaterDepthAtPosition(coords) < collection.Conditions.MinWaterDepth)
							continue;
					
					} else {

						if (environment.NearestPlanet.IsPositionUnderwater(coords))
							continue;
					
					}

					//Grids
					for (int k = GridManager.Grids.Count - 1; k >= 0; k--) {

						var grid = GridManager.Grids[k];

						if (!grid.ActiveEntity())
							continue;

						if (grid.CubeGrid.WorldAABB.Contains(coords) == ContainmentType.Disjoint)
							continue;

						cells.Clear();
						grid.CubeGrid.RayCastCells(upCoords, coords, cells);

						foreach (var cell in cells) {

							var block = grid.CubeGrid.GetCubeBlock(cell);

							if (block != null) {

								badCoords = true;
								break;

							}

						}

						if (badCoords)
							break;

					}

					if (badCoords)
						continue;

					//Check against other existing coords

					foreach (var coordinate in path.CreatureCoords) {

						if (Vector3D.Distance(coordinate, coords) < collection.Conditions.MinDistFromOtherCreaturesInGroup) {

							badCoords = true;
							break;
						
						}
					
					}

					if (badCoords)
						continue;

					path.CreatureCoords.Add(coords);
					path.ValidPath = true;
					break;

				}
			
			}
		
		}

		private static void CalculateOtherPath(PathDetails path, SpawnGroupCollection collection, EnvironmentEvaluation environment, MatrixD spawnMatrix) {

			path.SpawnType = SpawningType.OtherNPC;
			var voxelList = new List<MyVoxelBase>();
			var voxelSphere = new BoundingSphereD(spawnMatrix.Translation, 20000);
			MyGamePruningStructure.GetAllVoxelMapsInSphere(ref voxelSphere, voxelList);

			path.SpawnMatrix = spawnMatrix;
			path.StartCoords = spawnMatrix.Translation;

			if (path.SpawnMatrix.Forward == path.SpawnMatrix.Up || !VectorHelper.IsPerpendicular(path.SpawnMatrix.Forward, path.SpawnMatrix.Up))
				path.SpawnMatrix = MatrixD.CreateWorld(path.StartCoords, Vector3D.Forward, Vector3D.Up);

			bool badCoords = false;

			for (int j = 0; j < collection.PrefabIndexes.Count; j++) {

				var prefab = collection.SpawnGroup.SpawnGroup.Prefabs[collection.PrefabIndexes[j]];
				var prefabOffset = collection.SelectPrefabOffet(prefab.Position, j);

				if (!collection.SkipVoxelSpawnChecks) {

					if (IsVoxelIntersecting(voxelList, path.StartCoords, path.StartCoords, -1, collection.SkipVoxelSpawnChecks)) {

						badCoords = true;
						break;

					}
				
				}

				if (!collection.SkipGridSpawnChecks) {

					if (IsGridWithinMinDistance(path.StartCoords, 0, collection.SkipGridSpawnChecks)) {

						badCoords = true;
						break;

					}

				}

			}

			if (badCoords)
				return;

			path.ValidPath = true;
			
		}

		private static bool IsSpacePathObstructed(BoundingSphereD sphere, BoundingBoxD box, Vector3D startPath, Vector3D endPath, List<MyVoxelBase> voxels, bool skipGridCheck, bool skipVoxelCheck) {

			var ray = new RayD(startPath, Vector3D.Normalize(endPath - startPath));

			//Check For Intersection With Gravity
			if (ray.Intersects(sphere).HasValue) {

				//Prefab Path Intersecting Gravity, Abandoning Path
				return true;

			}

			//Check For Intersection With Player Box
			if (ray.Intersects(box).HasValue) {

				//Prefab Path Intersecting Player Box, Abandoning Path
				return true;

			}

			//Check For Grid or Voxel Obstructions
			if (IsGridWithinMinDistance(startPath, Settings.SpaceCargoShips.MinSpawnDistFromEntities, skipGridCheck)) {

				return true;
			
			}
			
			if (IsVoxelIntersecting(voxels, startPath, endPath, Settings.SpaceCargoShips.MinSpawnDistFromEntities, skipVoxelCheck)) {

				return true;

			}
				
			return false;

		}

		private static double MidPointAltitude(Vector3D start, Vector3D direction, double pathDistance, EnvironmentEvaluation environment) {

			var midPoint = direction * (pathDistance / 2) + start;
			return environment.NearestPlanet?.AltitudeAtPosition(midPoint) ?? 0;
		
		}

		private static bool IsPlanetPathObstructed(BoundingBoxD box, Vector3D start, Vector3D end, double pathDistance, EnvironmentEvaluation environment, bool skipGridCheck, bool skipVoxelCheck, StringBuilder sb = null) {

			var ray = new RayD(start, Vector3D.Normalize(end - start));
			var pathDirection = Vector3D.Normalize(end - start);

			//Check For Intersection With Player Box
			if (ray.Intersects(box).HasValue) {

				if (sb != null)
					sb.Append("   - Ray Intersects Box").AppendLine();

				return true;

			}

			if (IsGridWithinMinDistance(start, Settings.PlanetaryCargoShips.MinSpawnDistFromEntities, skipGridCheck)) {

				if (sb != null)
					sb.Append("   - Grid Within Min Distance").AppendLine();
				return true;

			}

			double stepDistance = -Settings.PlanetaryCargoShips.PathStepCheckDistance;
			bool badPath = false;

			while (stepDistance < pathDistance) {

				stepDistance += Settings.PlanetaryCargoShips.PathStepCheckDistance;
				var stepCoords = pathDirection * stepDistance + start;
				var altitude = environment.NearestPlanet.AltitudeAtPosition(stepCoords);

				if (altitude < Settings.PlanetaryCargoShips.MinPathAltitude) {

					if (sb != null)
						sb.Append("   - Planet Altitude Below Acceptable Level: " + altitude).AppendLine();

					badPath = true;
					break;

				}

			}

			return badPath;

		}

		private static bool IsVoxelIntersecting(List<MyVoxelBase> voxels, Vector3D start, Vector3D end, double minDist = -1, bool skipCheck = false) {

			if (voxels == null || skipCheck)
				return false;

			var ray = new RayD(start, Vector3D.Normalize(end - start));

			for (int i = voxels.Count - 1; i >= 0; i--) {

				var voxel = voxels[i];

				if (voxel == null || voxel.Closed)
					continue;

				if ((voxel as MyPlanet) != null)
					continue;

				if (minDist > -1 && Vector3D.Distance(start, voxel.PositionComp.WorldAABB.Center) < minDist)
					return true;

				if (start != end) {

					if (ray.Intersects(voxel.PositionComp.WorldAABB).HasValue)
						return true;

				} else {

					if (voxel.PositionComp.WorldAABB.Contains(start) != ContainmentType.Disjoint)
						return true;

				}
				

			}

			return false;

		}

		private static bool IsGridWithinMinDistance(Vector3D startPath, double minDist, bool skipCheck, bool boxCheck = false) {

			if (skipCheck)
				return false;

			for (int i = GridManager.Grids.Count - 1; i >= 0; i--) {

				var grid = GridManager.Grids[i];

				if (!grid.ActiveEntity())
					continue;

				if (!boxCheck) {

					if (grid.Distance(startPath) < minDist)
						return true;

				} else {

					if (grid.CubeGrid.PositionComp.WorldAABB.Contains(startPath) != ContainmentType.Disjoint)
						return true;
				
				}

			}

			return false;

		}

		public static MatrixD CalculateDerelictSpawnMatrix(MatrixD existingMatrix, Vector3D providedRotationValues) {

			//X: Pitch - Up/Forward | +Up -Down
			//Y: Yaw   - Forward/Up | +Right -Left
			//Z: Roll  - Up/Forward | +Right -Left
			var rotationValues = providedRotationValues;

			if (rotationValues.X == 100 && rotationValues.Y == 100 && rotationValues.Z == 100) {

				rotationValues.X = MathTools.RandomBetween(-100, 110) / 10;
				rotationValues.Y = MathTools.RandomBetween(-100, 110) / 10;
				rotationValues.Z = MathTools.RandomBetween(-100, 110) / 10;

			}

			var resultMatrix = existingMatrix;

			if (rotationValues.X != 0) {

				var translation = resultMatrix.Translation;
				var fowardPos = resultMatrix.Forward * 45;
				var upPos = resultMatrix.Up * 45;
				var pitchForward = Vector3D.Normalize(resultMatrix.Up * rotationValues.X + fowardPos);
				var pitchUp = Vector3D.Normalize(resultMatrix.Backward * rotationValues.X + upPos);
				resultMatrix = MatrixD.CreateWorld(translation, pitchForward, pitchUp);

			}

			if (rotationValues.Y != 0) {

				var translation = resultMatrix.Translation;
				var fowardPos = resultMatrix.Forward * 45;
				var upPos = resultMatrix.Up * 45;
				var yawForward = Vector3D.Normalize(resultMatrix.Right * rotationValues.Y + fowardPos);
				var yawUp = resultMatrix.Up;
				resultMatrix = MatrixD.CreateWorld(translation, yawForward, yawUp);

			}

			if (rotationValues.Z != 0) {

				var translation = resultMatrix.Translation;
				var fowardPos = resultMatrix.Forward * 45;
				var upPos = resultMatrix.Up * 45;
				var rollForward = resultMatrix.Forward;
				var rollUp = Vector3D.Normalize(resultMatrix.Right * rotationValues.Z + upPos);
				resultMatrix = MatrixD.CreateWorld(translation, rollForward, rollUp);

			}

			return resultMatrix;

		}

	}

}
