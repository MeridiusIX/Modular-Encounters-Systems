using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.ModAPI;
using VRageMath;
using Sandbox.Common.ObjectBuilders;
using ModularEncountersSystems.Spawning.Manipulation;
using Sandbox.Game;

namespace ModularEncountersSystems.Entities {
	public class SafeZoneEntity : EntityBase {

		public MySafeZone SafeZone;
		public long LinkedSafezoneEntityId;
		public IMyEntity LinkedSafezoneEntity;

		public SafeZoneEntity(IMyEntity entity) : base(entity) {

			SafeZone = entity as MySafeZone;

			if (SafeZone != null)
				IsValidEntity = true;

			if (SafeZone.Storage == null)
				return;

			var entityIdString = "";
			

			if (SafeZone.Storage.TryGetValue(StorageTools.MesSafeZoneLinkedEntity, out entityIdString))
				if (long.TryParse(entityIdString, out LinkedSafezoneEntityId)) {

					//MyVisualScriptLogicProvider.ShowNotificationToAll("Got Safezone Storage", 4000);

				}
				

		}

		public bool ActiveEntity() {

			return SafeZone != null && !IsClosed();
		
		}

		public bool InZone(Vector3D coords) {

			if (GetEntity() == null || Closed || !SafeZone.Enabled)
				return false;

			if (SafeZone.Shape == MySafeZoneShape.Sphere) {

				var newSphere = new BoundingSphereD(SafeZone.PositionComp.WorldAABB.Center, SafeZone.Radius);

				if (newSphere.Contains(coords) == ContainmentType.Contains)
					return true;

			} else {

				if (SafeZone.PositionComp.WorldAABB.Contains(coords) == ContainmentType.Contains)
					return true;
			
			}

			return false;

		}

	}

}
