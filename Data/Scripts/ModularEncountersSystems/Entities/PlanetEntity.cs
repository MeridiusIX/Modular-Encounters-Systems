using ModularEncountersSystems.API;
using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.Tasks;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Entities {
	public class PlanetEntity : EntityBase {

		public MyPlanet Planet;

		public bool HasAtmosphere;
		public bool HasGravity;
		public bool HasOxygen;

		public MyPlanet Water;

		public MyGravityProviderComponent Gravity;


		public PlanetEntity(IMyEntity entity) : base(entity) {

			Planet = entity as MyPlanet;
			HasGravity = entity.Components.TryGet<MyGravityProviderComponent>(out Gravity);
			HasAtmosphere = Planet.HasAtmosphere;
			HasOxygen = Planet.GetOxygenForPosition(GetPosition()) > 0 ? true : false;

			var storage = "";

			if (entity?.Storage != null && entity.Storage.TryGetValue(StorageTools.MesTemporaryPlanetKey, out storage)) {

				var data = Convert.FromBase64String(storage);
				var dateTime = MyAPIGateway.Utilities.SerializeFromBinary<DateTime>(data);
				var timeSpan = dateTime - MyAPIGateway.Session.GameDateTime;
				TaskProcessor.Tasks.Add(new TimedAction((int)timeSpan.TotalSeconds, DeletePlanet));

			}

			if (Settings.SpaceCargoShips.GetPlanetSpawnFilter(entity.EntityId) == null)
				Settings.SpaceCargoShips.AddPlanetSpawnFilter(new PlanetSpawnFilter(Planet.Generator.Id.SubtypeName, entity.EntityId));

			if (Settings.RandomEncounters.GetPlanetSpawnFilter(entity.EntityId) == null)
				Settings.RandomEncounters.AddPlanetSpawnFilter(new PlanetSpawnFilter(Planet.Generator.Id.SubtypeName, entity.EntityId));

			if (Settings.PlanetaryCargoShips.GetPlanetSpawnFilter(entity.EntityId) == null)
				Settings.PlanetaryCargoShips.AddPlanetSpawnFilter(new PlanetSpawnFilter(Planet.Generator.Id.SubtypeName, entity.EntityId));

			if (Settings.PlanetaryInstallations.GetPlanetSpawnFilter(entity.EntityId) == null)
				Settings.PlanetaryInstallations.AddPlanetSpawnFilter(new PlanetSpawnFilter(Planet.Generator.Id.SubtypeName, entity.EntityId));

			if (Settings.BossEncounters.GetPlanetSpawnFilter(entity.EntityId) == null)
				Settings.BossEncounters.AddPlanetSpawnFilter(new PlanetSpawnFilter(Planet.Generator.Id.SubtypeName, entity.EntityId));

			if (Settings.DroneEncounters.GetPlanetSpawnFilter(entity.EntityId) == null)
				Settings.DroneEncounters.AddPlanetSpawnFilter(new PlanetSpawnFilter(Planet.Generator.Id.SubtypeName, entity.EntityId));

		}

		public double AltitudeAtPosition(Vector3D coords, bool ignoreWater = false) {

			var surface = Vector3D.Zero;

			if (!ignoreWater && HasWater()) {

				surface = SurfaceCoordsAtPosition(coords, false);

			} else {

				surface = SurfaceCoordsAtPosition(coords, true);

			}

			if (surface != Vector3D.Zero) {

				var coordsCore = DistanceToCore(coords);
				var surfaceCore = DistanceToCore(surface);
				return coordsCore - surfaceCore;

			}

			return 0;

		}

		public Vector3D Center() {

			return Planet?.PositionComp?.WorldAABB.Center ?? Vector3D.Zero;

		}

		public override void CloseEntity(IMyEntity entity) {

			base.CloseEntity(entity);
			IsValidEntity = false;

		}

		public void DeletePlanet() {

			if (Planet == null)
				return;

			Planet.Close();
			CloseEntity(Planet);

		}

		public double DistanceToCore(Vector3D coords) {

			Vector3D core = Center();
			return Vector3D.Distance(coords, core);

		}

		public bool GetDepth(Vector3D coords, ref double depth) {

			float? depthActual = null;

			if (!HasWater()) {

				depth = 0;
				return false;

			}

			depthActual = WaterAPI.GetDepth(coords, Water);

			if (depthActual.HasValue) {

				depth = depthActual.Value;
				return depth < 0;

			}

			depth = 0;
			return false;

		}

		public BoundingSphereD GetGravitySphere(double gravityLimit) {

			if (!ValidEntity())
				return new BoundingSphereD(Vector3D.Zero, 0);

			return new BoundingSphereD(Center(), MathTools.GravityToDistance(gravityLimit, Planet.Generator.SurfaceGravity, Planet.Generator.GravityFalloffPower, Planet.MinimumRadius, Planet.MaximumRadius));

		}

		public Vector3D GetPositionAtAverageRadius(Vector3D coords) {

			return Vector3D.Normalize(coords - Center()) * Planet.AverageRadius + Center();

		}

		public bool HasWater() {

			if (!APIs.WaterModApiLoaded) {

				//SpawnLogger.Write("Water API Not Loaded", SpawnerDebugEnum.API);
				return false;

			}
				

			if (!WaterAPI.HasWater(Planet)) {

				//SpawnLogger.Write("Water API Says Planet Does Not Have Water", SpawnerDebugEnum.API);
				Water = null;
				return false;

			}

			if (Water == null) {

				Water = Planet;

			}
		
			return true;

		}

		public bool IsPositionInGravity(Vector3D coords) {

			if (Planet == null || IsClosed() || Gravity == null)
				return false;

			return Gravity.IsPositionInRange(coords);
		
		}

		public bool IsPositionInRange(Vector3D coords) {

			if (Planet.PositionComp.WorldAABB.Contains(coords) == ContainmentType.Contains)
				return true;

			return false;
		
		}

		public bool IsPositionUnderground(Vector3D coords) {

			return AltitudeAtPosition(coords) < 0;
		
		}

		public bool IsPositionUnderwater(Vector3D coords) {

			if (!HasWater())
				return false;

			return WaterAPI.IsUnderwater(coords, Water);

		}

		public Vector3D SurfaceCoordsAtPosition(Vector3D coords, bool ignoreWater = false, bool onlyGetWaterSurface = false) {

			if (!ignoreWater && HasWater()) {

				var waterCoords = WaterAPI.GetClosestSurfacePoint(coords, Water);
				var terrainCoords = Planet?.GetClosestSurfacePointGlobal(coords) ?? Vector3D.Zero;

				if (onlyGetWaterSurface)
					return waterCoords;

				if (DistanceToCore(terrainCoords) > DistanceToCore(waterCoords))
					return terrainCoords;
				else
					return waterCoords;

			}

			return Planet?.GetClosestSurfacePointGlobal(coords) ?? Vector3D.Zero;
		
		}

		public Vector3D SurfaceCoordsAtPosition(Vector3D coords, ref double distanceAboveWater) {

			if(!HasWater())
				return SurfaceCoordsAtPosition(coords);

			var planetSurface = SurfaceCoordsAtPosition(coords, true);
			var waterSurface = SurfaceCoordsAtPosition(coords, false, true);
			var planetDist = Vector3D.Distance(planetSurface, Center());
			var waterDist = Vector3D.Distance(waterSurface, Center());
			bool dryLand = planetDist > waterDist;
			distanceAboveWater = planetDist - waterDist;
			return dryLand ? planetSurface : waterSurface;

		}

		public bool UnderwaterAndDepthCheck(Vector3D pos, bool targetState, double minDepth, double maxDepth) {

			double depth = 0;
			var underwater = GetDepth(pos, ref depth);

			if (!underwater && !targetState) {

				return true;

			}

			if (underwater && (minDepth > -1 || maxDepth > -1)) {

				depth = Math.Abs(depth);

				if (minDepth > -1 && depth < minDepth)
					underwater = false;

				if (maxDepth > -1 && depth > maxDepth)
					underwater = false;

			}

			return (underwater == targetState);

		}

		public Vector3D UpAtPosition(Vector3D coords) {

			return Vector3D.Normalize(coords - Center());
		
		}

		public double WaterDepthAtPosition(Vector3D coords) {

			if (!HasWater())
				return 0;

			var result = WaterAPI.GetDepth(coords, Water);

			if (!result.HasValue)
				return 0;

			return result.Value > 0 ? result.Value : 0;

		}

	}
}
