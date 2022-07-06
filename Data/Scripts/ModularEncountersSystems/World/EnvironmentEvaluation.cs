using ModularEncountersSystems.API;
using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.World {

	public class EnvironmentEvaluation {

		public Vector3D Position;
		public Vector3D Up;

		public double DistanceFromWorldCenter;
		public Vector3D DirectionFromWorldCenter;

		public List<string> InsideTerritories;
		public List<string> InsideStrictTerritories;

		public List<string> InsideKnownPlayerLocations;
		public List<string> InsideStrictKnownPlayerLocations;

		public List<PlanetaryLane> InsidePlanetaryLanes;

		public PlanetEntity NearestPlanet;
		public MyGravityProviderComponent Gravity;
		public bool IsOnPlanet;
		public string NearestPlanetName;
		public double PlanetDiameter;

		public Vector3D SurfaceCoords;
		public float OxygenAtPosition;
		public float AtmosphereAtPosition;
		public float GravityAtPosition;
		public double AltitudeAtPosition;
		public bool IsNight;
		public string WeatherAtPosition;
		public string CommonTerrainAtPosition;

		public bool SpaceCargoShipsEligible;
		public bool LunarCargoShipsEligible;
		public bool PlanetaryCargoShipsEligible;
		public bool GravityCargoShipsEligible;
		public bool RandomEncountersEligible;
		public bool PlanetaryInstallationEligible;
		public bool WaterInstallationEligible;

		public float AirTravelViabilityRatio;

		public bool PlanetHasWater;
		public bool PositionIsUnderWater;
		public bool SurfaceIsUnderWater;
		public float WaterInSurroundingAreaRatio;

		public bool InsideNebula;
		public float NebulaDensity;
		public float NebulaMaterial;
		public string NebulaWeather;

		public float ThreatScore;
		public double ThreatScoreCheckDistance;

		public DateTime ServerTime;

		public EnvironmentEvaluation() {

			WeatherAtPosition = "";
			CommonTerrainAtPosition = "";
			NearestPlanetName = "";

		}

		public EnvironmentEvaluation(Vector3D coords) : base() {

			//Non Planet Checks
			Position = coords;
			DistanceFromWorldCenter = Vector3D.Distance(Vector3D.Zero, coords);
			DirectionFromWorldCenter = Vector3D.Normalize(coords);

			ServerTime = DateTime.Now;

			InsideTerritories = new List<string>();
			InsideStrictTerritories = new List<string>();

			InsidePlanetaryLanes = new List<PlanetaryLane>();
			PlanetManager.GetLanesAtPosition(coords, InsidePlanetaryLanes);

			if (AddonManager.NebulaMod) {

				if (APIs.NebulaApiLoaded) {

					if (NebulaAPI.InsideNebula(Position)) {

						InsideNebula = true;
						NebulaDensity = NebulaAPI.GetNebulaDensity(Position);
						NebulaMaterial = NebulaAPI.GetMaterial(Position);
						NebulaWeather = NebulaAPI.GetWeather(Position) ?? "N/A";

					} else {

						InsideNebula = false;
						NebulaDensity = 0;
						NebulaMaterial = 0;
						NebulaWeather = "N/A";

					}

				}

			}

			//Planet Checks
			NearestPlanet = PlanetManager.GetNearestPlanet(coords);

			if (NearestPlanet?.Planet == null || NearestPlanet.IsClosed()) {

				SetEligibleSpawns();
				return;

			}
	
			var upDir = Vector3D.Normalize(coords - NearestPlanet.Center());
			Up = upDir;
			var downDir = upDir * -1;
			var forward = Vector3D.CalculatePerpendicularVector(upDir);
			var matrix = MatrixD.CreateWorld(coords, forward, upDir);
			var directionList = new List<Vector3D>();
			directionList.Add(matrix.Forward);
			directionList.Add(matrix.Backward);
			directionList.Add(matrix.Left);
			directionList.Add(matrix.Right);
			directionList.Add(Vector3D.Normalize(matrix.Forward + matrix.Right));
			directionList.Add(Vector3D.Normalize(matrix.Forward + matrix.Left));
			directionList.Add(Vector3D.Normalize(matrix.Backward + matrix.Right));
			directionList.Add(Vector3D.Normalize(matrix.Backward + matrix.Left));
			SurfaceCoords = NearestPlanet.SurfaceCoordsAtPosition(coords);

			if (APIs.WaterModApiLoaded) {

				if (NearestPlanet.HasWater()) {

					PlanetHasWater = true;
					PositionIsUnderWater = NearestPlanet.IsPositionUnderwater(coords);
					SurfaceIsUnderWater = NearestPlanet.IsPositionUnderwater(SurfaceCoords);

					if (SurfaceIsUnderWater)
						SurfaceCoords = NearestPlanet.SurfaceCoordsAtPosition(coords);

					int totalChecks = 0;
					int waterHits = 0;

					for (int j = 0; j < 12; j++) {

						foreach (var direction in directionList) {

							try {

								totalChecks++;
								var checkCoordsRough = direction * (j * 1000) + coords;
								var checkSurfaceCoords = NearestPlanet.SurfaceCoordsAtPosition(checkCoordsRough);

								if (NearestPlanet.IsPositionUnderwater(checkSurfaceCoords))
									waterHits++;

							} catch (Exception e) {

								SpawnLogger.Write("Caught Exception Trying To Determine Water Data", SpawnerDebugEnum.Error, true);
								SpawnLogger.Write(e.ToString(), SpawnerDebugEnum.Error, true);

							}

						}

					}

					SpawnLogger.Write("Water Hits: " + waterHits.ToString(), SpawnerDebugEnum.Pathing);
					SpawnLogger.Write("Total Hits: " + totalChecks.ToString(), SpawnerDebugEnum.Pathing);
					WaterInSurroundingAreaRatio = (float)waterHits / (float)totalChecks;


				}

			}

			var distToCore = NearestPlanet.DistanceToCore(Position);
			var surfaceDistToCore = NearestPlanet.DistanceToCore(SurfaceCoords);
			AltitudeAtPosition = distToCore - surfaceDistToCore;
			NearestPlanetName = NearestPlanet.Planet.Generator.Id.SubtypeName;
			PlanetDiameter = NearestPlanet.Planet.AverageRadius * 2;

			var planetEntity = NearestPlanet.Planet as IMyEntity;
			Gravity = planetEntity.Components.Get<MyGravityProviderComponent>();

			if (Gravity != null) {

				if (Gravity.IsPositionInRange(coords) == true) {

					IsOnPlanet = true;

				}

			}

			if (!IsOnPlanet) {

				SetEligibleSpawns();
				return;

			}
				

			//On Planet Checks
			GravityAtPosition = Gravity.GetGravityMultiplier(coords);
			AtmosphereAtPosition = NearestPlanet.Planet.GetAirDensity(coords);
			OxygenAtPosition = NearestPlanet.Planet.GetOxygenForPosition(coords);
			IsNight = MyVisualScriptLogicProvider.IsOnDarkSide(NearestPlanet.Planet, coords);
			WeatherAtPosition = MyVisualScriptLogicProvider.GetWeather(coords) ?? "";

			//Atmo Travel Viability
			float airTravelChecks = 0;
			float airTravelHits = 0;

			for (int i = -3; i < 4; i++) {

				for (int j = -3; j < 4; j++) {

					airTravelChecks++;
					var tempForward = matrix.Forward * (j * 500) + matrix.Translation;
					var roughCoords = matrix.Right * (i * 500) + tempForward;
					var surface = NearestPlanet.SurfaceCoordsAtPosition(roughCoords);
					var up = Vector3D.Normalize(surface - NearestPlanet.Center());
					var minAltitude = up * Settings.PlanetaryCargoShips.MinSpawningAltitude + surface;
					var airDensity = NearestPlanet.Planet.GetAirDensity(minAltitude);

					if (airDensity >= Settings.PlanetaryCargoShips.MinAirDensity)
						airTravelHits++;

				}
			
			}

			if(airTravelChecks > 0 && airTravelHits > 0)
				AirTravelViabilityRatio = airTravelHits / airTravelChecks;

			//Terrain Material Checks
			var terrainTypes = new Dictionary<string, int>();

			for (int i = 1; i < 12; i++) {

				foreach (var direction in directionList) {

					try {

						var checkCoordsRough = direction * (i * 15) + coords;
						var checkSurfaceCoords = NearestPlanet.SurfaceCoordsAtPosition(checkCoordsRough);
						
						var checkMaterial = NearestPlanet.Planet.GetMaterialAt(ref checkSurfaceCoords);

						if (checkMaterial == null)
							continue;

						if (terrainTypes.ContainsKey(checkMaterial.MaterialTypeName)) {

							terrainTypes[checkMaterial.MaterialTypeName]++;

						} else {

							terrainTypes.Add(checkMaterial.MaterialTypeName, 1);

						}

					} catch (Exception e) {

						SpawnLogger.Write("Caught Exception Trying To Determine Terrain Material", SpawnerDebugEnum.Error, true);
						SpawnLogger.Write(e.ToString(), SpawnerDebugEnum.Error, true);

					}
					
				}

			}

			string highestCountName = "";
			int highestCountNumber = 0;

			foreach (var material in terrainTypes.Keys) {

				if (string.IsNullOrWhiteSpace(highestCountName) || terrainTypes[material] > highestCountNumber) {

					highestCountName = material;
					highestCountNumber = terrainTypes[material];

				}
			
			}

			if (!string.IsNullOrWhiteSpace(highestCountName)) {

				CommonTerrainAtPosition = highestCountName;

			} else {

				CommonTerrainAtPosition = "N/A";
			
			}

			SetEligibleSpawns();

		}

		public void SetEligibleSpawns() {

			if (NearestPlanet != null && !NearestPlanet.IsClosed() && IsOnPlanet) {

				LunarCargoShipsEligible = !Gravity.IsPositionInRange(Up * Settings.SpaceCargoShips.MinLunarSpawnHeight + Position);
				SpaceCargoShipsEligible = LunarCargoShipsEligible || !IsOnPlanet;
				PlanetaryCargoShipsEligible = IsOnPlanet && AltitudeAtPosition <= Settings.PlanetaryCargoShips.PlayerSurfaceAltitude;
				GravityCargoShipsEligible = IsOnPlanet && AltitudeAtPosition > Settings.PlanetaryCargoShips.PlayerSurfaceAltitude;
				PlanetaryInstallationEligible = IsOnPlanet && AltitudeAtPosition < Settings.PlanetaryInstallations.PlayerMaximumDistanceFromSurface;
				WaterInstallationEligible = PlanetaryInstallationEligible && WaterInSurroundingAreaRatio > 0.15f;

			} else {

				SpaceCargoShipsEligible = true;
				RandomEncountersEligible = true;

			}
		
		}

		public void GetThreat(double distance, bool includeNpc) {

			ThreatScoreCheckDistance = distance;
			ThreatScore = SpawnConditions.GetThreatLevel(distance, includeNpc, Position);

		}

	}

}
