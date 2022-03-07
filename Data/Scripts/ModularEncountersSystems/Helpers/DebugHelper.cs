using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Helpers {
	public static class DebugHelper {


		public static void FireTurretInSafeDirection(IMyLargeTurretBase turret) {

			if (turret == null)
				return;

			//Up 1.5
			if (CheckDirectionForSafeTurretFire(turret, turret.WorldMatrix.Up)) {

				turret.SetManualAzimuthAndElevation(0, 1.5f);
				turret.SyncAzimuth();
				turret.SyncElevation();
				turret.ShootOnce();
				return;

			}

			//Forward 0
			if (CheckDirectionForSafeTurretFire(turret, turret.WorldMatrix.Forward)) {

				turret.SetManualAzimuthAndElevation(0, 0);
				turret.SyncAzimuth();
				turret.SyncElevation();
				turret.ShootOnce();
				return;
			
			}

			//Left -4.71
			if (CheckDirectionForSafeTurretFire(turret, turret.WorldMatrix.Left)) {

				turret.SetManualAzimuthAndElevation(-4.71f, 0);
				turret.SyncAzimuth();
				turret.SyncElevation();
				turret.ShootOnce();
				return;

			}

			//Right -1.57
			if (CheckDirectionForSafeTurretFire(turret, turret.WorldMatrix.Right)) {

				turret.SetManualAzimuthAndElevation(1.57f, 0);
				turret.SyncAzimuth();
				turret.SyncElevation();
				turret.ShootOnce();
				return;

			}

			//Backward -3.14
			if (CheckDirectionForSafeTurretFire(turret, turret.WorldMatrix.Backward)) {

				turret.SetManualAzimuthAndElevation(-3.14f, 0);
				turret.SyncAzimuth();
				turret.SyncElevation();
				turret.ShootOnce();
				return;

			}

		}

		private static bool CheckDirectionForSafeTurretFire(IMyLargeTurretBase turret, Vector3D dir) {

			var list = new List<Vector3I>();
			var end = dir * 200 + turret.GetPosition();
			turret.SlimBlock.CubeGrid.RayCastCells(turret.GetPosition(), end, list);

			foreach (var cell in list) {

				var block = turret.SlimBlock.CubeGrid.GetCubeBlock(cell);

				if (block == null || block == turret.SlimBlock)
					continue;

				return false;
			
			}

			return true;
		
		}

	}
}
