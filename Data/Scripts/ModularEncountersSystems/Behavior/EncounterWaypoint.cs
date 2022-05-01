using Sandbox.ModAPI;
using VRageMath;
using System;
using VRage.ModAPI;
using ProtoBuf;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Behavior.Subsystems.Trigger;
using VRage.Game.ModAPI;
using ModularEncountersSystems.Entities;

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

		public static EncounterWaypoint CalculateWaypoint(IBehavior behavior, string waypointProfileId) {

			EncounterWaypoint waypoint = null;
			WaypointProfile waypointProfile = null;

			if (ProfileManager.WaypointProfiles.TryGetValue(waypointProfileId, out waypointProfile)) {

				if ((int)waypointProfile.Waypoint > 2) {

					if (waypointProfile.RelativeEntity == RelativeEntityType.Self)
						waypoint = waypointProfile.GenerateEncounterWaypoint(behavior.RemoteControl);

					if (waypointProfile.RelativeEntity == RelativeEntityType.Target && behavior.AutoPilot.Targeting.HasTarget())
						waypoint = waypointProfile.GenerateEncounterWaypoint(behavior.AutoPilot.Targeting.Target.GetEntity());

					if (waypointProfile.RelativeEntity == RelativeEntityType.Damager) {

						IMyEntity entity = null;

						if (MyAPIGateway.Entities.TryGetEntityById(behavior.BehaviorSettings.LastDamagerEntity, out entity)) {

							var parentEnt = entity.GetTopMostParent();

							if (parentEnt != null) {

								if (parentEnt as IMyCubeGrid != null) {

									//BehaviorLogger.Write("Damager Parent Entity Valid", BehaviorDebugEnum.General);
									var gridGroup = MyAPIGateway.GridGroups.GetGroup(behavior.RemoteControl.SlimBlock.CubeGrid, GridLinkTypeEnum.Physical);
									bool isSameGridConstrust = false;

									foreach (var grid in gridGroup) {

										if (grid.EntityId == parentEnt.EntityId) {

											//BehaviorLogger.Write("Damager Parent Entity Was Same Grid", BehaviorDebugEnum.General);
											isSameGridConstrust = true;
											break;

										}

									}

									if (!isSameGridConstrust) {

										waypoint = waypointProfile.GenerateEncounterWaypoint(parentEnt);

									}

								} else {

									var potentialPlayer = PlayerManager.GetPlayerUsingTool(entity);

									if (potentialPlayer != null) {

										waypoint = waypointProfile.GenerateEncounterWaypoint(parentEnt);

									}

								}

							}

						}

					}

				} else {

					waypoint = waypointProfile.GenerateEncounterWaypoint(behavior.RemoteControl);

				}


			}

			return waypoint;

		}

	}

}
	
