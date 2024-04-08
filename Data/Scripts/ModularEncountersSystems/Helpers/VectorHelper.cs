using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Logging;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace ModularEncountersSystems.Helpers {

	public static class VectorHelper {

		public static Random Rnd = new Random();

		public static Vector3D CreateCoords(Vector3D start, Vector3D dir, double dist) {

			return start * dir + dist;

		}

		public static void GenerateCircularDirections(MatrixD matrix, List<Vector3D> result) {

			result.Add(matrix.Forward);
			result.Add(Vector3D.Normalize(matrix.Forward + matrix.Right));
			result.Add(matrix.Right);
			result.Add(Vector3D.Normalize(matrix.Right + matrix.Backward));
			result.Add(matrix.Backward);
			result.Add(Vector3D.Normalize(matrix.Backward + matrix.Left));
			result.Add(matrix.Left);
			result.Add(Vector3D.Normalize(matrix.Left + matrix.Forward));

		}

		public static Vector3D GetClosestDirectionFromList(Vector3D originalDirection, List<Vector3D> directions) {

			Vector3D result = originalDirection;
			double angle = 180;

			foreach (var dir in directions) {

				var thisAngle = GetAngleBetweenDirections(originalDirection, dir);

				if (thisAngle < angle) {

					result = dir;
					angle = thisAngle;
				
				}
			
			}

			return result;
		
		}

		public static int GetClosestDirectionIndexFromList(Vector3D originalDirection, List<Vector3D> directions, double targetAngle = 0) {

			int result = 0;
			double angle = 180;

			for (int i = 0; i < directions.Count; i++) {

				var dir = directions[i];
				var thisAngle = GetAngleBetweenDirections(originalDirection, dir);

				if (MathTools.IsValueCloser(targetAngle, angle, thisAngle)) {

					result = i;
					angle = thisAngle;

				}

			}

			return result;

		}

		public static Vector3D GetRandomDespawnCoords(Vector3D coords, double distance = 8000, double altitude = 1500) {

			Vector3D result = Vector3D.Zero;

			var planet = MyGamePruningStructure.GetClosestPlanet(coords);
			var planetEntity = planet as IMyEntity;
			var gravityProvider = planetEntity?.Components?.Get<MyGravityProviderComponent>();

			if (gravityProvider != null && gravityProvider.IsPositionInRange(coords)) {

				var up = Vector3D.Normalize(coords - planetEntity.GetPosition());
				Vector3D randDirection = MyUtils.GetRandomPerpendicularVector((Vector3)up);
				var surfaceCoords = planet.GetClosestSurfacePointGlobal(randDirection * distance + coords);
				result = Vector3D.Normalize(surfaceCoords - planetEntity.GetPosition()) * altitude + surfaceCoords;

			} else if (planet != null) {

				var up = Vector3D.Normalize(coords - planetEntity.GetPosition());
				Vector3D randDirection = MyUtils.GetRandomPerpendicularVector((Vector3)up);
				result = randDirection * distance + coords;

			} else {

				Vector3D randDirection = Vector3D.Normalize(MyUtils.GetRandomVector3D());
				return randDirection * distance + coords;

			}

			return result;

		}

		public static Vector3D GetRandomDirectionAtAngle(Vector3D direction, int minAngle, int maxAngle) {

			double oldAzimuth = 0;
			double oldElevation = 0;
			Vector3D.GetAzimuthAndElevation(direction, out oldAzimuth, out oldElevation);

			double randAzAngle = Rnd.Next(minAngle, maxAngle);
			double randElAngle = Rnd.Next(minAngle, maxAngle);
			double azimuth = AdjustAzimuthElevationByDegrees(oldAzimuth, randAzAngle);
			double elevation = AdjustAzimuthElevationByDegrees(oldElevation, randElAngle);

			Vector3D result = Vector3D.Zero;
			Vector3D.CreateFromAzimuthAndElevation(azimuth, elevation, out result);
			return result;

		}

		public static double AdjustAzimuthElevationByDegrees(double existingValue, double degrees) {

			double result = 0;
			double changeBy = degrees * 0.034;

			if (existingValue >= 0) {

				result = existingValue + changeBy;

				if (result > 3.14)
					result = -3.14 - (result - 3.14);

			} else {

				result = existingValue + changeBy;

				if (result < -3.14)
					result = 3.14 - (result - -3.14);

			}

			return result;

		}

		//CreateDirectionAndTarget
		public static Vector3D CreateDirectionAndTarget(Vector3D startDir, Vector3D endDir, Vector3D startCoords, double distance) {

			var direction = Vector3D.Normalize(endDir - startDir);
			return direction * distance + startCoords;

		}

		public static double GetAltitudeAtPosition(Vector3D coords, MyPlanet planet = null) {

			if (planet == null) {

				planet = MyGamePruningStructure.GetClosestPlanet(coords);

				if (planet == null)
					return 0;

			}

			return Vector3D.Distance(coords, GetPlanetSurfaceCoordsAtPosition(coords, planet));

		}

		//GetAngleBetweenDirections
		public static double GetAngleBetweenDirections(Vector3D dirA, Vector3D dirB) {

			if (dirA == Vector3D.Zero || dirB == Vector3D.Zero)
				return 0;

			var radians = MyUtils.GetAngleBetweenVectors((Vector3)dirA, (Vector3)dirB);
			return (180 / Math.PI) * radians;

		}

		//GetDirectionAwayFromTarget
		public static Vector3D GetDirectionAwayFromTarget(Vector3D myPosition, Vector3D targetPosition) {

			var planet = MyGamePruningStructure.GetClosestPlanet(myPosition);

			if (planet == null) {

				return Vector3D.Normalize(myPosition - targetPosition);

			} else {

				var mySealevel = GetPlanetSealevelAtPosition(myPosition, planet);
				var targetSealevel = GetPlanetSealevelAtPosition(targetPosition, planet);
				return Vector3D.Normalize(mySealevel - targetSealevel);

			}

		}

		public static Vector3D GetDirectionClosestTo(Vector3D direction, params Vector3D[] directions) {

			var result = Vector3D.Zero;
			double angle = -1;

			foreach (var dir in directions) {

				var thisAngle = GetAngleBetweenDirections(direction, dir);

				if (angle > -1 && thisAngle >= angle)
					continue;

				result = dir;
				angle = thisAngle;

			}

			return result;
		
		}

		//Only Works For Forward, Right, Backward, Left. Up or Down will return original result.
		public static Vector3D GetNextClockwiseDirection(Vector3D direction, MatrixD matrix) {

			if (direction == matrix.Forward)
				return matrix.Right;

			if (direction == matrix.Right)
				return matrix.Backward;

			if (direction == matrix.Backward)
				return matrix.Left;

			if (direction == matrix.Left)
				return matrix.Forward;

			return direction;
		
		}

		public static double GetDistanceToTargetAtMyAltitude(Vector3D myCoords, Vector3D targetCoords, PlanetEntity planet) {

			if (planet == null || targetCoords == Vector3D.Zero)
				return -1;

			var myDistance = Vector3D.Distance(myCoords, planet.Center());
			var targetDirection = Vector3D.Normalize(targetCoords - planet.Center());
			var targetCoordsAtMyAltitude = myDistance * targetDirection + planet.Center();
			return Vector3D.Distance(myCoords, targetCoordsAtMyAltitude);

		}

		public static LineD GetLineThroughBox(Vector3D start, Vector3D end, BoundingBoxD box) {

			var line = new LineD(start, end);
			var result = new LineD(start, end);
			box.Intersect(ref line, out result);
			return result;
		
		}

		public static void GetPathSteps(Vector3D startCoords, Vector3D directionToTarget, double distanceToTarget, double stepDistance, List<Vector3D> result) {

			double currentPathDistance = 0;

			while (currentPathDistance < distanceToTarget) {

				if ((distanceToTarget - currentPathDistance) < stepDistance) {

					currentPathDistance = distanceToTarget;

				} else {

					currentPathDistance += stepDistance;

				}

				result.Add(directionToTarget * currentPathDistance + startCoords);

			}

		}

		//GetThrustDirectionsAwayFromPosition
		public static Vector3I GetThrustDirectionsAwayFromDirection(MatrixD myMatrix, Vector3D targetPosition) {

			var targetDir = targetPosition;
			Vector3I closestVector = Vector3I.Zero;
			double closestAngle = 180;

			var left = GetAngleBetweenDirections(targetDir, myMatrix.Left);
			if (left < closestAngle) {

				closestVector = new Vector3I(1, 0, 0);
				closestAngle = left;

			}

			var right = GetAngleBetweenDirections(targetDir, myMatrix.Right);
			if (right < closestAngle) {

				closestVector = new Vector3I(-1, 0, 0);
				closestAngle = right;

			}

			var down = GetAngleBetweenDirections(targetDir, myMatrix.Down);
			if (down < closestAngle) {

				closestVector = new Vector3I(0, 1, 0);
				closestAngle = down;

			}

			var up = GetAngleBetweenDirections(targetDir, myMatrix.Up);
			if (up < closestAngle) {

				closestVector = new Vector3I(0, -1, 0);
				closestAngle = up;

			}

			var back = GetAngleBetweenDirections(targetDir, myMatrix.Backward);
			if (back < closestAngle) {

				closestVector = new Vector3I(0, 0, 1);
				closestAngle = back;

			}

			var forward = GetAngleBetweenDirections(targetDir, myMatrix.Forward);
			if (forward < closestAngle) {

				closestVector = new Vector3I(0, 0, -1);
				closestAngle = forward;

			}

			return closestVector;

		}

		public static Vector3I GetThrustDirectionsAwayFromSurface(MatrixD myMatrix, Vector3D upDirection, Vector3I oldDirections) {

			Vector3I directions = new Vector3I(0, 0, 0);
			Vector3I newDirections = oldDirections;
			double closestAngle = 180;

			if (GetAngleBetweenDirections(-upDirection, myMatrix.Forward) < closestAngle) {

				closestAngle = GetAngleBetweenDirections(-upDirection, myMatrix.Forward);
				directions = new Vector3I(0, 0, -1);

			}

			if (GetAngleBetweenDirections(-upDirection, myMatrix.Backward) < closestAngle) {

				closestAngle = GetAngleBetweenDirections(-upDirection, myMatrix.Backward);
				directions = new Vector3I(0, 0, 1);

			}

			if (GetAngleBetweenDirections(-upDirection, myMatrix.Up) < closestAngle) {

				closestAngle = GetAngleBetweenDirections(-upDirection, myMatrix.Up);
				directions = new Vector3I(0, -1, 0);

			}

			if (GetAngleBetweenDirections(-upDirection, myMatrix.Down) < closestAngle) {

				closestAngle = GetAngleBetweenDirections(-upDirection, myMatrix.Down);
				directions = new Vector3I(0, 1, 0);

			}

			if (GetAngleBetweenDirections(-upDirection, myMatrix.Right) < closestAngle) {

				closestAngle = GetAngleBetweenDirections(-upDirection, myMatrix.Right);
				directions = new Vector3I(-1, 0, 0);

			}

			if (GetAngleBetweenDirections(-upDirection, myMatrix.Left) < closestAngle) {

				closestAngle = GetAngleBetweenDirections(-upDirection, myMatrix.Left);
				directions = new Vector3I(1, 0, 0);

			}

			if (directions.X != 0) {

				newDirections.X = directions.X;

			}

			if (directions.Y != 0) {

				newDirections.Y = directions.Y;

			}

			if (directions.Z != 0) {

				newDirections.Z = directions.Z;

			}

			return newDirections;


		}

		//GetPlanetSealevelAtPosition
		public static Vector3D GetPlanetSealevelAtPosition(Vector3D coords, MyPlanet planet = null) {

			if (planet == null) {

				return Vector3D.Zero;

			}

			var planetEntity = planet as IMyEntity;
			var direction = Vector3D.Normalize(coords - planetEntity.GetPosition());
			return direction * (double)planet.MinimumRadius + planetEntity.GetPosition();

		}

		public static Vector3D GetPositionCenter(IMyEntity entity) {

			if (MyAPIGateway.Entities.Exist(entity) == false) {

				return Vector3D.Zero;

			}

			if (entity?.PositionComp == null) {

				return Vector3D.Zero;

			}

			return entity.PositionComp.WorldAABB.Center;

		}

		//GetPlanetSurfaceCoordsAtPosition
		public static Vector3D GetPlanetSurfaceCoordsAtPosition(Vector3D coords, MyPlanet planet = null) {

			if (planet == null) {

				return Vector3D.Zero;

			}

			var checkCoords = coords;

			return planet.GetClosestSurfacePointGlobal(ref checkCoords);

		}

		public static Vector3D GetPlanetSurfaceCoordsAtPosition(Vector3D coords) {

			var planet = MyGamePruningStructure.GetClosestPlanet(coords);

			if (planet == null)
				return Vector3D.Zero;

			return GetPlanetSurfaceCoordsAtPosition(coords, planet);

		}

		//GetPlanetSurfaceDifference
		public static double GetPlanetSurfaceDifference(Vector3D myCoords, Vector3D testCoords, MyPlanet planet = null) {

			if (planet == null) {

				return 0;

			}

			var testSurfaceCoords = GetPlanetSurfaceCoordsAtPosition(testCoords, planet);
			var mySealevelCoords = GetPlanetSealevelAtPosition(myCoords, planet);
			var testSealevelCoords = GetPlanetSealevelAtPosition(testSurfaceCoords, planet);
			var myDistance = Vector3D.Distance(mySealevelCoords, myCoords);
			var testDistance = Vector3D.Distance(testSealevelCoords, testSurfaceCoords);
			return myDistance - testDistance;

		}

		public static Vector3D GetPlanetUpDirection(Vector3D position) {

			var planet = MyGamePruningStructure.GetClosestPlanet(position);

			if (planet == null) {

				return Vector3D.Zero;

			}

			var planetEntity = planet as IMyEntity;
			var gravityProvider = planetEntity.Components.Get<MyGravityProviderComponent>();

			if (gravityProvider.IsPositionInRange(position) == true) {

				return Vector3D.Normalize(position - planetEntity.GetPosition());

			}

			return Vector3D.Zero;

		}

		public static MatrixD GetPlanetRandomSpawnMatrix(Vector3D coords, double minDist, double maxDist, double minAltitude, double maxAltitude, bool inheritAltitude = false) {

			MatrixD result = MatrixD.Identity;
			var NearestPlanet = PlanetManager.GetNearestPlanet(coords);

			if (NearestPlanet == null)
				return result;

			

			var upDir = NearestPlanet.UpAtPosition(coords);



			double inheritedAltitude = 0;

			if (inheritAltitude) {

				var npcSurface = NearestPlanet.SurfaceCoordsAtPosition(coords, true);
				inheritedAltitude = Vector3D.Distance(npcSurface, coords);

			}

			var perpDir = RandomPerpendicular(upDir);
			var roughArea = perpDir * RandomDistance(minDist, maxDist) + coords;
			var surfaceCoords = NearestPlanet.SurfaceCoordsAtPosition(roughArea);
			var upAtSurface = Vector3D.Normalize(surfaceCoords - NearestPlanet.Center());
			var spawnCoords = upAtSurface * (RandomDistance(minAltitude, maxAltitude) + inheritedAltitude) + surfaceCoords;
			var perpSurfaceDir = RandomPerpendicular(upAtSurface);
			result = MatrixD.CreateWorld(spawnCoords, perpSurfaceDir, upAtSurface);
			return result;

		}

		/*
		The intent of this method is to calculate whether or not the approach to a target
		on a planet is safe, and to return coordinates that properly avoid terrain obstacles.
		
		The calculation starts by checking a path of waypoints in 50m steps toward the 
		target, upto maxDistanceToCheck value or the target if it is closer than that
		value.
		
		At each 50m step on the calculated path, the distance from the surface at that step
		to the planet core is measured. If at any point a path core distance is a higher
		value than the NPC distance to the core, it means there is a terrain obstacle detected.
		
		If a terrain obstacle is detected, a waypoint is created between the NPC and the first-order
		set of path coordinates that was considered higher terrain than the NPC. From that waypoint,
		a new waypoint is returned in the sky at the altitude of the highest terrain + minAltitude value
		
		
		*/

		public static Vector3D GetPlanetWaypointPathing(Vector3D myCoords, Vector3D targetCoords, double minAltitude = 200, double maxDistanceToCheck = 1000, bool returnOriginalTarget = false) {

			double minAltitudeRelaxation = 0.75;
			var planet = MyGamePruningStructure.GetClosestPlanet(targetCoords);

			if (planet == null) {

				return targetCoords;

			}

			var planetCoords = planet.PositionComp.WorldAABB.Center;
			var targetUp = Vector3D.Normalize(targetCoords - planetCoords);

			var dirToTarget = Vector3D.Normalize(targetCoords - myCoords);
			var distToTarget = Vector3D.Distance(targetCoords, myCoords);
			double distanceToUse = distToTarget;

			if (distToTarget > maxDistanceToCheck) {

				distanceToUse = maxDistanceToCheck;

			}

			List<Vector3D> pathSteps = new List<Vector3D>();
			double currentPathDistance = 0;

			while (currentPathDistance < distanceToUse) {

				if ((distanceToUse - currentPathDistance) < 50) {

					currentPathDistance = distanceToUse;

				} else {

					currentPathDistance += 50;

				}

				pathSteps.Add(dirToTarget * currentPathDistance + myCoords);

			}

			var myDistToCore = Vector3D.Distance(myCoords, planetCoords);
			var targetDistToCore = Vector3D.Distance(targetCoords, planetCoords);

			double currentHighestDistance = myDistToCore;
			double currentHighestTerrain = 0;
			Vector3D highestTerrainPoint = Vector3D.Zero;
			Vector3D closestHigherNPCTerrain = Vector3D.Zero;

			foreach (var pathPoint in pathSteps) {

				Vector3D pathPointRef = pathPoint;
				Vector3D surfacePoint = planet.GetClosestSurfacePointGlobal(ref pathPointRef);
				double surfacePointToCore = Vector3D.Distance(surfacePoint, planetCoords);

				if (currentHighestTerrain < surfacePointToCore) {

					currentHighestTerrain = surfacePointToCore;
					highestTerrainPoint = surfacePoint;

				}

				if (currentHighestDistance < surfacePointToCore) {

					currentHighestDistance = surfacePointToCore;

					if (closestHigherNPCTerrain == Vector3D.Zero) {

						closestHigherNPCTerrain = surfacePoint;

					}

				}

			}

			//If Highest Terrain + minAltitude Is Higher Than NPC
			if (currentHighestTerrain + (minAltitude * minAltitudeRelaxation) > myDistToCore) {

				if (closestHigherNPCTerrain == Vector3D.Zero) {

					closestHigherNPCTerrain = highestTerrainPoint;

				}

				//Logger.Write(string.Format("Higher Terrain Near NPC"), true);
				var forwardStep = dirToTarget * 50 + myCoords;
				return Vector3D.Normalize(forwardStep - planetCoords) * (currentHighestDistance + minAltitude) + planetCoords;

			}

			//If NPC is Higher Than Highest Detected Terrain, but Target is Not
			if ((currentHighestTerrain - targetDistToCore) > minAltitude) {

				//Logger.Write(string.Format("Higher Terrain Near Target"), true);
				return targetUp * (currentHighestTerrain + minAltitude) + planetCoords;

			}

			//No Terrain Obstacle Between Target and NPC.
			if (returnOriginalTarget == false) {

				return targetUp * minAltitude + targetCoords;

			} else {

				return targetCoords;

			}

		}

		public static Dictionary<string, Vector3D> GetTransformedGyroRotations(MatrixD refBlock, MatrixD gyro) {

			//Get Reference Rotation Directions
			var refPitchDirections = new Dictionary<Vector3D, Vector3D>();
			var refYawDirections = new Dictionary<Vector3D, Vector3D>();
			var refRollDirections = new Dictionary<Vector3D, Vector3D>();

			refPitchDirections.Add(refBlock.Forward, refBlock.Up);
			refPitchDirections.Add(refBlock.Up, refBlock.Backward);
			refPitchDirections.Add(refBlock.Backward, refBlock.Down);
			refPitchDirections.Add(refBlock.Down, refBlock.Forward);
			var refPitchDirectionsList = refPitchDirections.Keys.ToList();

			refYawDirections.Add(refBlock.Forward, refBlock.Right);
			refYawDirections.Add(refBlock.Right, refBlock.Backward);
			refYawDirections.Add(refBlock.Backward, refBlock.Left);
			refYawDirections.Add(refBlock.Left, refBlock.Forward);
			var refYawDirectionsList = refYawDirections.Keys.ToList();

			refRollDirections.Add(refBlock.Up, refBlock.Right);
			refRollDirections.Add(refBlock.Right, refBlock.Down);
			refRollDirections.Add(refBlock.Down, refBlock.Left);
			refRollDirections.Add(refBlock.Left, refBlock.Up);
			var refRollDirectionsList = refRollDirections.Keys.ToList();

			//Gyro Rotation Directions
			var gyroPitchDirections = new Dictionary<Vector3D, Vector3D>();
			var gyroYawDirections = new Dictionary<Vector3D, Vector3D>();
			var gyroRollDirections = new Dictionary<Vector3D, Vector3D>();

			gyroPitchDirections.Add(gyro.Forward, gyro.Up);
			gyroPitchDirections.Add(gyro.Up, gyro.Backward);
			gyroPitchDirections.Add(gyro.Backward, gyro.Down);
			gyroPitchDirections.Add(gyro.Down, gyro.Forward);

			gyroYawDirections.Add(gyro.Forward, gyro.Right);
			gyroYawDirections.Add(gyro.Right, gyro.Backward);
			gyroYawDirections.Add(gyro.Backward, gyro.Left);
			gyroYawDirections.Add(gyro.Left, gyro.Forward);

			gyroRollDirections.Add(gyro.Up, gyro.Right);
			gyroRollDirections.Add(gyro.Right, gyro.Down);
			gyroRollDirections.Add(gyro.Down, gyro.Left);
			gyroRollDirections.Add(gyro.Left, gyro.Up);

			var localPitchDirections = new Dictionary<Vector3D, Vector3D>();
			var localYawDirections = new Dictionary<Vector3D, Vector3D>();
			var localRollDirections = new Dictionary<Vector3D, Vector3D>();
			var result = new Dictionary<string, Vector3D>();

			//Calculate Pitch Axis
			while (true) {

				var checkPitchPitch = refPitchDirectionsList.Except(gyroPitchDirections.Keys.ToList()).ToList();
				if (checkPitchPitch.Count == 0) {

					//Logger.Write("PitchPitch", true);
					localPitchDirections = gyroPitchDirections;
					var sign = GetSignForRotationDirection(refPitchDirections, gyroPitchDirections, refBlock.Forward);
					result.Add("Pitch", new Vector3D(-sign, 0, 0));
					break;

				}

				var checkPitchYaw = refPitchDirectionsList.Except(gyroYawDirections.Keys.ToList()).ToList();
				if (checkPitchYaw.Count == 0) {

					//Logger.Write("PitchYaw", true);
					localPitchDirections = gyroYawDirections;
					var sign = GetSignForRotationDirection(refPitchDirections, gyroYawDirections, refBlock.Forward);
					result.Add("Pitch", new Vector3D(0, sign, 0));
					break;

				}

				var checkPitchRoll = refPitchDirectionsList.Except(gyroRollDirections.Keys.ToList()).ToList();
				if (checkPitchRoll.Count == 0) {

					//Logger.Write("PitchRoll", true);
					localPitchDirections = gyroRollDirections;
					var sign = GetSignForRotationDirection(refPitchDirections, gyroRollDirections, refBlock.Forward);
					result.Add("Pitch", new Vector3D(0, 0, sign));
					break;

				}

				break;

			}


			//Calculate Yaw Axis
			while (true) {

				var checkYawPitch = refYawDirectionsList.Except(gyroPitchDirections.Keys.ToList()).ToList();
				if (checkYawPitch.Count == 0) {

					//Logger.Write("YawPitch", true);
					localYawDirections = gyroPitchDirections;
					var sign = GetSignForRotationDirection(refYawDirections, gyroPitchDirections, refBlock.Forward);
					result.Add("Yaw", new Vector3D(-sign, 0, 0));
					break;

				}

				var checkYawYaw = refYawDirectionsList.Except(gyroYawDirections.Keys.ToList()).ToList();
				if (checkYawYaw.Count == 0) {

					//Logger.Write("YawYaw", true);
					localYawDirections = gyroYawDirections;
					var sign = GetSignForRotationDirection(refYawDirections, gyroYawDirections, refBlock.Forward);
					result.Add("Yaw", new Vector3D(0, sign, 0));
					break;

				}

				var checkYawRoll = refYawDirectionsList.Except(gyroRollDirections.Keys.ToList()).ToList();
				if (checkYawRoll.Count == 0) {

					//Logger.Write("YawRoll", true);
					localYawDirections = gyroRollDirections;
					var sign = GetSignForRotationDirection(refYawDirections, gyroRollDirections, refBlock.Forward);
					result.Add("Yaw", new Vector3D(0, 0, sign));
					break;

				}

				break;

			}

			//Calculate Roll Axis
			while (true) {

				var checkRollPitch = refRollDirectionsList.Except(gyroPitchDirections.Keys.ToList()).ToList();
				if (checkRollPitch.Count == 0) {

					//Logger.Write("RollPitch", true);
					localRollDirections = gyroPitchDirections;
					var sign = GetSignForRotationDirection(refRollDirections, gyroPitchDirections, refBlock.Up);
					result.Add("Roll", new Vector3D(-sign, 0, 0));
					break;

				}

				var checkRollYaw = refRollDirectionsList.Except(gyroYawDirections.Keys.ToList()).ToList();
				if (checkRollYaw.Count == 0) {

					//Logger.Write("RollYaw", true);
					localRollDirections = gyroYawDirections;
					var sign = GetSignForRotationDirection(refRollDirections, gyroYawDirections, refBlock.Up);
					result.Add("Roll", new Vector3D(0, sign, 0));
					break;

				}

				var checkRollRoll = refRollDirectionsList.Except(gyroRollDirections.Keys.ToList()).ToList();
				if (checkRollRoll.Count == 0) {

					//Logger.Write("RollRoll", true);
					localRollDirections = gyroRollDirections;
					var sign = GetSignForRotationDirection(refRollDirections, gyroRollDirections, refBlock.Up);
					result.Add("Roll", new Vector3D(0, 0, sign));
					break;

				}

				break;

			}

			//Logger.Write("End Getting Rotations");

			return result;

		}

		public static double GetSignForRotationDirection(Dictionary<Vector3D, Vector3D> refDict, Dictionary<Vector3D, Vector3D> gyroDict, Vector3D dir) {

			try {

				if (refDict[dir] == gyroDict[dir]) {

					return 1; //

				}

				return -1;

			} catch (Exception e) {

				SpawnLogger.Write("Caught Exception: ", SpawnerDebugEnum.Error, true);
				SpawnLogger.Write(e.ToString(), SpawnerDebugEnum.Error, true);

			}

			return 0;


		}

		public static bool IsPerpendicular(Vector3D a, Vector3D b) {

			var coordsA = a;
			var coordsB = b;
			return Vector3D.ArePerpendicular(ref a, ref b);

		}

		//IsPositionUnderground
		public static bool IsPositionUnderground(Vector3D coords, MyPlanet planet) {

			if (planet == null) {

				return false;

			}

			var closestSurfacePoint = planet.GetClosestSurfacePointGlobal(coords);
			var planetEntity = planet as IMyEntity;

			if (Vector3D.Distance(planetEntity.GetPosition(), coords) < Vector3D.Distance(planetEntity.GetPosition(), closestSurfacePoint)) {

				return true;

			}

			return false;

		}

		public static double RandomDistance(double a, double b) {

			return Rnd.Next((int)a, (int)b);

		}

		//RandomDirection
		public static Vector3D RandomDirection() {

			return Vector3D.Normalize(MyUtils.GetRandomVector3D());

		}

		public static Vector3D RandomBaseDirection(MatrixD matrix, bool ignoreForward = false, bool ignoreBackward = false, bool ignoreUp = false, bool ignoreDown = false, bool ignoreLeft = false, bool ignoreRight = false) {

			var directionList = new List<Vector3D>();

			if (ignoreForward == false) {

				directionList.Add(matrix.Forward);

			}

			if (ignoreBackward == false) {

				directionList.Add(matrix.Backward);

			}

			if (ignoreUp == false) {

				directionList.Add(matrix.Up);

			}

			if (ignoreDown == false) {

				directionList.Add(matrix.Down);

			}

			if (ignoreLeft == false) {

				directionList.Add(matrix.Left);

			}

			if (ignoreRight == false) {

				directionList.Add(matrix.Right);

			}

			return directionList[Rnd.Next(0, directionList.Count)];

		}

		//RandomPerpendicular
		public static Vector3D RandomPerpendicular(Vector3D referenceDir) {

			var refDir = Vector3D.Normalize(referenceDir);
			return Vector3D.Normalize(MyUtils.GetRandomPerpendicularVector(ref refDir));

		}

		//Following Code Provided By Whiplash141, DarkStar, and WeaponCore Team
		public static Vector3D TrajectoryEstimation(Vector3D targetPos, Vector3D targetVel, Vector3D targetAcc, double targetMaxSpeed, Vector3D shooterPos, Vector3D shooterVel, double projectileMaxSpeed, double projectileInitSpeed = 0, double projectileAccMag = 0, double gravityMultiplier = 0, Vector3D gravity = default(Vector3D), bool basic = false) {

			Vector3D deltaPos = targetPos - shooterPos;
			Vector3D deltaVel = targetVel - shooterVel;

			Vector3D deltaPosNorm;
			if (Vector3D.IsZero(deltaPos)) deltaPosNorm = Vector3D.Zero;
			else if (Vector3D.IsUnit(ref deltaPos)) deltaPosNorm = deltaPos;
			else deltaPosNorm = Vector3D.Normalize(deltaPos);

			double closingSpeed = Vector3D.Dot(deltaVel, deltaPosNorm);
			Vector3D closingVel = closingSpeed * deltaPosNorm;
			Vector3D lateralVel = deltaVel - closingVel;
			double projectileMaxSpeedSqr = projectileMaxSpeed * projectileMaxSpeed;
			double ttiDiff = projectileMaxSpeedSqr - lateralVel.LengthSquared();
			double projectileClosingSpeed = Math.Sqrt(ttiDiff) - closingSpeed;
			double closingDistance = Vector3D.Dot(deltaPos, deltaPosNorm);
			double timeToIntercept = ttiDiff < 0 ? 0 : closingDistance / projectileClosingSpeed;
			double maxSpeedSqr = targetMaxSpeed * targetMaxSpeed;
			double shooterVelScaleFactor = 1;
			bool projectileAccelerates = projectileAccMag > 1e-6;
			bool hasGravity = gravityMultiplier > 1e-6;

			if (projectileAccelerates) {
				/*
				This is a rough estimate to smooth out our initial guess based upon the missile parameters.
				The reasoning is that the longer it takes to reach max velocity, the more the initial velocity
				has an overall impact on the estimated impact point.
				*/
				shooterVelScaleFactor = Math.Min(1, (projectileMaxSpeed - projectileInitSpeed) / projectileAccMag);
			}

			/*
			Estimate our predicted impact point and aim direction
			*/

			Vector3D estimatedImpactPoint = targetPos + timeToIntercept * (targetVel - shooterVel * shooterVelScaleFactor);

			if (basic) return estimatedImpactPoint;

			Vector3D aimDirection = estimatedImpactPoint - shooterPos;

			Vector3D projectileVel = shooterVel;
			Vector3D projectilePos = shooterPos;

			Vector3D aimDirectionNorm;

			if (projectileAccelerates) {

				if (Vector3D.IsZero(deltaPos)) aimDirectionNorm = Vector3D.Zero;
				else if (Vector3D.IsUnit(ref deltaPos)) aimDirectionNorm = aimDirection;
				else aimDirectionNorm = Vector3D.Normalize(aimDirection);
				projectileVel += aimDirectionNorm * projectileInitSpeed;

			} else {

				if (targetAcc.LengthSquared() < 1 && !hasGravity)
					return estimatedImpactPoint;

				if (Vector3D.IsZero(deltaPos)) aimDirectionNorm = Vector3D.Zero;
				else if (Vector3D.IsUnit(ref deltaPos)) aimDirectionNorm = aimDirection;
				else aimDirectionNorm = Vector3D.Normalize(aimDirection);

				projectileVel += aimDirectionNorm * projectileMaxSpeed;

			}

			var count = projectileAccelerates ? 600 : 60;

			double dt = Math.Max(MyEngineConstants.UPDATE_STEP_SIZE_IN_SECONDS, timeToIntercept / count); // This can be a const somewhere
			double dtSqr = dt * dt;
			Vector3D targetAccStep = targetAcc * dt;
			Vector3D projectileAccStep = aimDirectionNorm * projectileAccMag * dt;
			Vector3D gravityStep = gravity * gravityMultiplier * dt;
			Vector3D aimOffset = Vector3D.Zero;
			double minDiff = double.MaxValue;

			for (int i = 0; i < count; ++i) {

				targetVel += targetAccStep;

				if (targetVel.LengthSquared() > maxSpeedSqr)
					targetVel = Vector3D.Normalize(targetVel) * targetMaxSpeed;

				targetPos += targetVel * dt;

				if (projectileAccelerates) {
					projectileVel += projectileAccStep;

					if (projectileVel.LengthSquared() > projectileMaxSpeedSqr) {

						projectileVel = Vector3D.Normalize(projectileVel) * projectileMaxSpeed;

					}

				}

				if (hasGravity)
					projectileVel += gravityStep;

				projectilePos += projectileVel * dt;
				Vector3D diff = (targetPos - projectilePos);
				double diffLenSq = diff.LengthSquared();

				if (diffLenSq < projectileMaxSpeedSqr * dtSqr) {

					aimOffset = diff;
					break;

				}

				if (diffLenSq < minDiff) {

					minDiff = diffLenSq;
					aimOffset = diff;

				}

			}

			return estimatedImpactPoint + aimOffset;
		}


		//License Details For FirstOrderIntercept and FirstOrderInterceptTime

		/*The MIT License (MIT)

		Copyright (c) 2008 Daniel Brauer

		Permission is hereby granted, free of charge, to any person obtaining a copy 
		of this software and associated documentation files (the "Software"), to deal 
		in the Software without restriction, including without limitation the rights 
		to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
		copies of the Software, and to permit persons to whom the Software is furnished 
		to do so, subject to the following conditions:

		The above copyright notice and this permission notice shall be included in all 
		copies or substantial portions of the Software.

		THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
		IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
		FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
		AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
		WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN 
		CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */

		public static Vector3 FirstOrderIntercept(Vector3 shooterPosition, Vector3 shooterVelocity, float shotSpeed, Vector3 targetPosition, Vector3 targetVelocity) {

			Vector3 targetRelativePosition = targetPosition - shooterPosition;
			Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
			float t = FirstOrderInterceptTime(shotSpeed, targetRelativePosition, targetRelativeVelocity);
			return targetPosition + t * (targetRelativeVelocity);

		}

		//first-order intercept using relative target position
		public static float FirstOrderInterceptTime(float shotSpeed, Vector3 targetRelativePosition, Vector3 targetRelativeVelocity) {

			float velocitySquared = (float)Math.Pow(targetRelativePosition.Length(), 2);

			if (velocitySquared < 0.001f) {

				return 0f;

			}

			float a = velocitySquared - shotSpeed * shotSpeed;

			//handle similar velocities
			if ((float)Math.Abs(a) < 0.001f) {

				float t = -(float)Math.Pow(targetRelativePosition.Length(), 2) / (2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition));
				return (float)Math.Max(t, 0f); //don't shoot back in time

			}

			float b = 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
			float c = (float)Math.Pow(targetRelativePosition.Length(), 2);
			float determinant = b * b - 4f * a * c;

			if (determinant > 0f) { //determinant > 0; two intercept paths (most common)

				float t1 = (-b + (float)Math.Sqrt(determinant)) / (2f * a);
				float t2 = (-b - (float)Math.Sqrt(determinant)) / (2f * a);

				if (t1 > 0f) {

					if (t2 > 0f) {

						return (float)Math.Min(t1, t2); //both are positive

					} else {

						return t1; //only t1 is positive

					}

				} else {

					return (float)Math.Max(t2, 0f); //don't shoot back in time

				}

			} else if (determinant < 0f) {

				return 0f; //determinant < 0; no intercept path

			} else { //determinant = 0; one intercept path, pretty much never happens

				return (float)Math.Max(-b / (2f * a), 0f); //don't shoot back in time

			}

		}

	}

}
