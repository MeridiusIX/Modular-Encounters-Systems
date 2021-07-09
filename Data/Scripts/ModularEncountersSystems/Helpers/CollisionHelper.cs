using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Logging;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Voxels;
using VRageMath;

namespace ModularEncountersSystems.Helpers {
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

				BehaviorLogger.Write("Caught Exception While Querying Voxels:", BehaviorDebugEnum.General);
				BehaviorLogger.Write(e.ToString(), BehaviorDebugEnum.General);

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
