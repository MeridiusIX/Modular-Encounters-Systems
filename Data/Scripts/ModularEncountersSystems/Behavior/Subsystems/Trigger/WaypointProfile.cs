using ModularEncountersSystems.Helpers;
using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace ModularEncountersSystems.Behavior.Subsystems.Trigger {

	public enum WaypointType {
	
		None = 0,
		Static = 1,
		StaticRandom = 2,
		RelativeOffset = 3,
		RelativeRandom = 4,
		EntityOffset = 5,
		EntityRandom = 6
	
	}

	public enum RelativeEntityType {
	
		None,
		Self,
		Target,
		Damager
	
	}

	public class WaypointProfile {

		public string ProfileSubtypeId;

		public WaypointType Waypoint;
		public RelativeEntityType RelativeEntity;

		public Vector3D Coordinates;
		public Vector3D Offset;

		public double MinDistance;
		public double MaxDistance;

		public double MinAltitude;
		public double MaxAltitude;
		public bool InheritRelativeAltitude;

		public WaypointProfile() {

			ProfileSubtypeId = "";

			Waypoint = WaypointType.None;
			RelativeEntity = RelativeEntityType.None;

			Coordinates = Vector3D.Zero;
			Offset = Vector3D.Zero;

			MinDistance = 100;
			MaxDistance = 101;

			MinAltitude = 100;
			MaxAltitude = 101;
			InheritRelativeAltitude = false;

		}

		public EncounterWaypoint GenerateEncounterWaypoint(IMyEntity entity) {

			if (Waypoint == WaypointType.Static) {

				return new EncounterWaypoint(Coordinates);

			}

			if (Waypoint == WaypointType.StaticRandom) {

				return new EncounterWaypoint(GetRandomCoords(Coordinates));

			}

			if (Waypoint == WaypointType.EntityOffset && entity?.PositionComp != null) {

				return new EncounterWaypoint(Vector3D.Transform(this.Coordinates, entity.WorldMatrix));

			}

			if (Waypoint == WaypointType.EntityRandom && entity?.PositionComp != null) {

				return new EncounterWaypoint(GetRandomCoords(entity.PositionComp.WorldAABB.Center));

			}

			if (Waypoint == WaypointType.RelativeOffset && entity?.PositionComp != null) {

				return new EncounterWaypoint(this.Offset, entity);

			}

			if (Waypoint == WaypointType.RelativeRandom && entity?.PositionComp != null) {

				var offset = GetRandomCoords(entity.PositionComp.WorldAABB.Center) - entity.PositionComp.WorldAABB.Center;
				return new EncounterWaypoint(offset, entity);

			}

			return null;

		}

		public Vector3D GetRandomCoords(Vector3D coords) {

			var planet = MyGamePruningStructure.GetClosestPlanet(coords);

			if (planet != null) {

				var grav = planet.Components.Get<MyGravityProviderComponent>();

				if (grav != null && grav.IsPositionInRange(coords)) {

					//Gravity Calcs
					var surfaceAtCoords = planet.GetClosestSurfacePointGlobal(coords);
					var altitudeAdd = this.InheritRelativeAltitude ? Vector3D.Distance(coords, planet.PositionComp.WorldAABB.Center) - Vector3D.Distance(surfaceAtCoords, planet.PositionComp.WorldAABB.Center) : 0;
					var up = Vector3D.Normalize(surfaceAtCoords - planet.PositionComp.WorldAABB.Center);
					var randForward = Vector3D.Normalize(MyUtils.GetRandomPerpendicularVector(ref up));
					var randForwardDist = MathTools.RandomBetween(this.MinDistance, this.MaxDistance);
					var upRandDist = MathTools.RandomBetween(this.MinAltitude, this.MaxAltitude) + altitudeAdd;
					var roughCoords = planet.GetClosestSurfacePointGlobal(randForwardDist * randForward + coords);
					return up * upRandDist + roughCoords;

				}

			}

			//Space Calcs
			var dir = Vector3D.Normalize(MyUtils.GetRandomVector3D());
			var dist = MathTools.RandomBetween(this.MinDistance, this.MaxDistance);
			return dir * dist + coords;
		
		}

		public void InitTags(string tagData) {

			if (!string.IsNullOrWhiteSpace(tagData)) {

				var descSplit = tagData.Split('\n');

				foreach (var tag in descSplit) {

					//Waypoint
					if (tag.Contains("[Waypoint:") == true) {

						TagParse.TagWaypointTypeEnumCheck(tag, ref Waypoint);

					}

					//RelativeEntity
					if (tag.Contains("[RelativeEntity:") == true) {

						TagParse.TagRelativeEntityEnumCheck(tag, ref RelativeEntity);

					}

					//Coordinates
					if (tag.Contains("[Coordinates:") == true) {

						TagParse.TagVector3DCheck(tag, ref Coordinates);

					}

					//Offset
					if (tag.Contains("[Offset:") == true) {

						TagParse.TagVector3DCheck(tag, ref Offset);

					}

					//MinDistance
					if (tag.Contains("[MinDistance:") == true) {

						TagParse.TagDoubleCheck(tag, ref this.MinDistance);

					}

					//MaxDistance
					if (tag.Contains("[MaxDistance:") == true) {

						TagParse.TagDoubleCheck(tag, ref this.MaxDistance);

					}

					//MinAltitude
					if (tag.Contains("[MinAltitude:") == true) {

						TagParse.TagDoubleCheck(tag, ref this.MinAltitude);

					}

					//MaxAltitude
					if (tag.Contains("[MaxAltitude:") == true) {

						TagParse.TagDoubleCheck(tag, ref this.MaxAltitude);

					}

					//InheritRelativeAltitude
					if (tag.Contains("[InheritRelativeAltitude:") == true) {

						TagParse.TagBoolCheck(tag, ref InheritRelativeAltitude);

					}

				}

			}

		}

	}

}
