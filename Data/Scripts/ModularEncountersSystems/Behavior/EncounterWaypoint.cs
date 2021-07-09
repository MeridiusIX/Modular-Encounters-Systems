using Sandbox.ModAPI;
using VRageMath;
using System;
using VRage.ModAPI;
using ProtoBuf;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Behavior.Subsystems.Trigger;

namespace ModularEncountersSystems.Behavior {

	[ProtoContract]
	public class EncounterWaypoint {

		[ProtoMember(1)]
		public bool Valid;

		[ProtoMember(2)]
		public WaypointType Waypoint;

		[ProtoMember(3)]
		public Vector3D Offset;

		[ProtoMember(4)]
		public long EntityId;

		[ProtoMember(5)]
		public int AliveTimeSeconds;

		[ProtoMember(6)]
		public Vector3D LastValidWaypoint;

		[ProtoMember(7)]
		public DateTime CreationTime;

		[ProtoMember(8)]
		public bool ReachedWaypoint;

		[ProtoMember(9)]
		public DateTime ReachedWaypointTime;

		[ProtoMember(10)]
		public bool UpdateWithEntity;

		[ProtoIgnore]
		public IMyEntity Entity;

		public EncounterWaypoint() {

			Init();

		}

		public EncounterWaypoint(Vector3D coords){

			Init();
			SetValid(true);
			Waypoint = WaypointType.Static;
			Offset = coords;

		}

		public EncounterWaypoint(Vector3D offset, long entityId){

			Init();
			SetValid(true);
			Waypoint = WaypointType.RelativeOffset;
			Offset = offset;
			EntityId = entityId;

		}

		public EncounterWaypoint(Vector3D offset, IMyEntity entity) {

			Init();
			SetValid(true);
			Waypoint = WaypointType.RelativeOffset;
			Offset = offset;
			EntityId = entity != null ? entity.EntityId : 0;
			Entity = entity;

		}

		public void Init() {

			SetValid(false);
			Waypoint = WaypointType.None;
			Offset = Vector3D.Zero;
			EntityId = 0;
			AliveTimeSeconds = -1;
			LastValidWaypoint = Vector3D.Zero;
			CreationTime = MyAPIGateway.Session.GameDateTime;
			ReachedWaypoint = false;
			ReachedWaypointTime = MyAPIGateway.Session.GameDateTime;
			Entity = null;
			UpdateWithEntity = false;

		}

		public void SetValid(bool state) {

			Valid = state;
		
		}

		public Vector3D GetCoords() {

			if (AliveTimeSeconds > 0) {

				var span = MyAPIGateway.Session.GameDateTime - CreationTime;

				if (span.TotalSeconds >= AliveTimeSeconds)
					SetValid(false);

			}

			if (Waypoint == WaypointType.RelativeOffset) {

				if (Entity == null) {

					if (!MyAPIGateway.Entities.TryGetEntityById(EntityId, out Entity)) {

						SetValid(false);
						return LastValidWaypoint;

					}
						

				} else if (Entity.MarkedForClose || Entity.Closed) {

					SetValid(false);
					return LastValidWaypoint;
				
				}

				LastValidWaypoint = Vector3D.Transform(Offset, Entity.WorldMatrix);
				return LastValidWaypoint;

			} else {

				if (LastValidWaypoint == Vector3D.Zero)
					LastValidWaypoint = Offset;

				return LastValidWaypoint;

			}
		
		}

	}

}
	
