using ModularEncountersSystems.API;
using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Logging;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Voxels;
using VRageMath;

namespace ModularEncountersSystems.Helpers {

	public struct CollisionResultSimple {

		public CollisionType HitType;
		public IMyEntity HitEntity;
		public IMyDestroyableObject HitObject;
		public Vector3D HitPosition;

		public CollisionResultSimple(CollisionType type, IMyEntity ent, Vector3D pos, IMyDestroyableObject obj) {

			HitType = type;
			HitEntity = ent;
			HitPosition = pos;
			HitObject = obj;

		}
	
	}

	public static class CollisionHelper {

		public static List<IMyCubeGrid> NewGrids = new List<IMyCubeGrid>();
		public static List<IMyCubeGrid> ActiveGrids = new List<IMyCubeGrid>();
		public static List<MySafeZone> AllSafeZones = new List<MySafeZone>();

		public static void NewEntityDetected(IMyEntity entity) {

			var cubeGrid = entity as IMyCubeGrid;

			if (cubeGrid == null)
				return;

			if (cubeGrid.Physics != null) {

				if (!NewGrids.Contains(cubeGrid))
					NewGrids.Add(cubeGrid);

			} else {

				cubeGrid.OnPhysicsChanged += GridPhysicsChanged;

			}
		
		}

		public static void GridPhysicsChanged(IMyEntity entity) {

			var cubeGrid = entity as IMyCubeGrid;

			if (cubeGrid == null)
				return;

			if (cubeGrid.Physics != null) {

				if (!NewGrids.Contains(cubeGrid))
					NewGrids.Add(cubeGrid);

			}

		}

		public static bool VoxelIntersectionCheck(MyVoxelBase voxel, Vector3D start, Vector3D end, double stepLength, ref Vector3D hit) {

			try {

				using (voxel.Pin()) {

					double totalDistance = Vector3D.Distance(start, end);
					Vector3D pathDirection = Vector3D.Normalize(end - start);
					var voxelHit = new VoxelHitInfo();

					for (double i = 0; i < totalDistance; i += stepLength) {

						var checkCoords = pathDirection * i + start;
						var voxelCoords = Vector3I.Zero;
						Vector3I.Floor(ref checkCoords, out voxelCoords);
						voxel.Storage.ExecuteOperationFast(ref voxelHit, MyStorageDataTypeFlags.Content, ref voxelCoords, ref voxelCoords, false);

						if (voxelHit.Hit) {

							hit = checkCoords;
							return true;

						}

					}

				}

			} catch (Exception e) {

				BehaviorLogger.Write("Caught Exception While Querying Voxels:", BehaviorDebugEnum.Error);
				BehaviorLogger.Write(e.ToString(), BehaviorDebugEnum.Error);

			}

			return false;
		
		}

		public static CollisionResultSimple CheapRaycast(Vector3D start, Vector3D dir, double dist, IMyCubeGrid ignoreGrid, List<MyLineSegmentOverlapResult<MyEntity>> result, List<IMyVoxelBase> voxels) {

			var line = new LineD(start, dir * dist + start);
			var ray = new RayD(start, dir);
			result.Clear();
			MyGamePruningStructure.GetAllEntitiesInRay(ref line, result);
			MyAPIGateway.Session.VoxelMaps.GetInstances(voxels);

			Vector3D hitPosition = Vector3D.Zero;
			Vector3D closestHit = Vector3D.Zero;
			IMyEntity hitEntity = null;
			IMyDestroyableObject hitObject = null;
			double closestDistance = 0;
			CollisionType hitType = CollisionType.None;

			//Grids
			for (int i = GridManager.Grids.Count - 1; i >= 0; i--) {

				var grid = GridManager.GetSafeGridFromIndex(i);

				if (grid == null || grid == ignoreGrid)
					continue;

				if (grid.Distance(start) > dist * 2)
					continue;

				if (grid.LineIntersection(line, ray, ref hitPosition, ref hitObject) && ComparePositions(hitType, start, hitPosition, ref closestDistance)) {

					hitType = CollisionType.Grid;
					closestHit = hitPosition;
					hitEntity = grid.CubeGrid;

				}
			
			}

			//Voxels
			foreach (var voxel in voxels) {

				double minDist = 0;
				double maxDist = 0;
				bool boxCheckResult = voxel.PositionComp.WorldAABB.Intersect(ref ray, out minDist, out maxDist);

				if (!boxCheckResult)
					continue;

				Vector3D startBox = boxCheckResult ? (minDist - 5) * dir + start : start;
				Vector3D endBox = boxCheckResult ? (maxDist + 5) * dir + start : line.To;

				if (voxel as MyPlanet != null) {

					var planet = voxel as MyPlanet;
					bool positionUnderground = planet.IsUnderGround(start);

					double currentPathDistance = 0;

					while (currentPathDistance < dist) {

						if ((dist - currentPathDistance) < 50) {

							currentPathDistance = dist;

						} else {

							currentPathDistance += 50;

						}

						if (planet.IsUnderGround(currentPathDistance * dir + startBox) != positionUnderground) {

							break;
						
						}

					}

					if (VoxelIntersectionCheck(voxel as MyVoxelBase, dir * (currentPathDistance - 50) + startBox, dir * (currentPathDistance) + startBox, 10, ref hitPosition) && ComparePositions(hitType, start, hitPosition, ref closestDistance)) {

						hitType = CollisionType.Voxel;
						closestHit = hitPosition;
						hitEntity = voxel;

					}

				} else {

					if (VoxelIntersectionCheck(voxel as MyVoxelBase, startBox, endBox, 10, ref hitPosition) && ComparePositions(hitType, start, hitPosition, ref closestDistance)) {

						hitType = CollisionType.Voxel;
						closestHit = hitPosition;
						hitEntity = voxel;

					}

				}
			
			}

			//Player
			for (int i = PlayerManager.Players.Count - 1; i >= 0; i--) {

				var player = PlayerManager.Players[i];

				if (player?.Player?.Character == null || !player.ActiveEntity())
					continue;

				if (player.Player.Character.WorldAABB.Intersects(ref line)) {

					hitPosition = player.Player.Character.WorldAABB.Center;

					if (ComparePositions(hitType, start, hitPosition, ref closestDistance)) {

						hitType = CollisionType.Player;
						closestHit = hitPosition;
						hitEntity = player.Player.Character;
						hitObject = player.Player.Character;

					}

				}
			
			}

			//Safezone
			for (int i = SafeZoneManager.SafeZones.Count - 1; i >= 0; i--) {

				var zone = SafeZoneManager.SafeZones[i];

				if (zone == null || !zone.ValidEntity())
					continue;

				if (zone.SafeZone.Shape == Sandbox.Common.ObjectBuilders.MySafeZoneShape.Sphere) {

					var sphere = new BoundingSphereD(zone.SafeZone.PositionComp.WorldAABB.Center, zone.SafeZone.Radius);
					double tmin = 0;
					double tmax = 0;

					if (sphere.IntersectRaySphere(ray, out tmin, out tmax)) {

						hitPosition = dir * tmin + start;

						if (ComparePositions(hitType, start, hitPosition, ref closestDistance)) {

							hitType = CollisionType.Safezone;
							closestHit = hitPosition;
							hitEntity = zone.SafeZone;

						}

					}
				
				} else {

					double distance = 0;

					if (zone.SafeZone.PositionComp.WorldAABB.Intersects(ref line, out distance)) {

						hitPosition = dir * distance + start;

						if (ComparePositions(hitType, start, hitPosition, ref closestDistance)) {

							hitType = CollisionType.Safezone;
							closestHit = hitPosition;
							hitEntity = zone.SafeZone;

						}

					}
				
				}

			}

			//Shield
			if (AddonManager.DefenseShields) {

				var shield = APIs.Shields.ClosestShieldInLine(line, true);

				if (shield.Item1.HasValue) {

					var shieldIntersect = APIs.Shields.LineIntersectShield(shield.Item2, line);

					if (shieldIntersect.HasValue) {

						hitPosition = shieldIntersect.Value;

						if (ComparePositions(hitType, start, hitPosition, ref closestDistance)) {

							hitType = CollisionType.Shield;
							closestHit = hitPosition;
							hitEntity = shield.Item2;

						}

					}
				
				}
			
			}

			return new CollisionResultSimple(hitType, hitEntity, closestHit, hitObject);

		}

		public static bool ComparePositions(CollisionType type, Vector3D start, Vector3D hit, ref double dist) {

			if (type == CollisionType.None)
				return true;

			double thisDist = Vector3D.Distance(start, hit);

			if (thisDist < dist) {

				dist = thisDist;
				return true;
			
			}

			return false;
		
		}

		public static void RegisterCollisionHelper() {

			MyAPIGateway.Entities.OnEntityAdd += NewEntityDetected;
			var allEntities = new HashSet<IMyEntity>();
			MyAPIGateway.Entities.GetEntities(allEntities);

			foreach (var entity in allEntities) {

				var cubeGrid = entity as IMyCubeGrid;

				if (cubeGrid == null)
					return;

				if (cubeGrid.Physics != null) {

					if (!NewGrids.Contains(cubeGrid))
						NewGrids.Add(cubeGrid);

				} else {

					cubeGrid.OnPhysicsChanged += GridPhysicsChanged;

				}

			}


		}

		public static void UnregisterCollisionHelper() {

			MyAPIGateway.Entities.OnEntityAdd -= NewEntityDetected;

		}

	}

	public struct VoxelHitInfo : IVoxelOperator {

		public bool Hit;

		public VoxelOperatorFlags Flags { get { return VoxelOperatorFlags.Read; } }

		public void Op(ref Vector3I position, MyStorageDataTypeEnum dataType, ref byte inOutContent) {

			if (inOutContent != MyVoxelConstants.VOXEL_CONTENT_EMPTY)
				Hit = true;

		}


	}

}
