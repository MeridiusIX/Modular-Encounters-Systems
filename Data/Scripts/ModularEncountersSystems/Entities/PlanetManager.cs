using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Terminal;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Entities {
    public static class PlanetManager {

        public static List<PlanetEntity> Planets = new List<PlanetEntity>();
        public static List<PlanetaryLane> Lanes = new List<PlanetaryLane>();

        public static float AirDensityAtPosition(Vector3D pos) {

            foreach (var planet in Planets) {

                if (!planet.HasAtmosphere || !planet.IsPositionInGravity(pos))
                    continue;

                var air = planet.Planet.GetAirDensity(pos);

                if (air > 0)
                    return air;
            
            }

            return 0;
        
        }

        public static PlanetEntity GetNearestPlanet(Vector3D coords) {

            PlanetEntity planet = null;
            bool inGravity = false;
            double distance = -1;

            foreach (var planetEnt in Planets) {

                if (planetEnt?.Planet == null)
                    continue;

                if (planetEnt.IsPositionInGravity(coords)) {

                    planet = planetEnt;
                    inGravity = true;

                } else if (inGravity) {

                    continue;
                
                }

                var thisDist = Vector3D.Distance(planetEnt.Center(), coords);

                if (distance == -1 || thisDist < distance) {

                    planet = planetEnt;
                    distance = thisDist;

                }
            
            }

            return planet;

        }

        public static PlanetEntity GetPlanetWithName(string generatorName) {

            foreach (var planet in Planets) {

                if (!string.IsNullOrWhiteSpace(planet?.Planet?.Generator?.Id.SubtypeName) && planet.Planet.Generator.Id.SubtypeName == generatorName)
                    return planet;
            
            }

            return null;
        
        }

        public static PlanetEntity GetPlanetWithId(long entityId) {

            foreach (var planet in Planets) {

                if (planet?.Planet != null && planet.Planet.EntityId == entityId)
                    return planet;

            }

            return null;

        }

        public static bool InGravity(Vector3D coords) {

            var planet = GetNearestPlanet(coords);

            if (planet != null)
                return planet.IsPositionInGravity(coords);

            return false;

        }

        public static Vector3 GetTotalNaturalGravity(Vector3D coords) {

            Vector3 gravity = Vector3.Zero;

            foreach (var planet in Planets) {

                if (planet == null || planet.Planet.Closed || planet.IsPositionInGravity(coords))
                    continue;

                gravity += planet.Gravity.GetWorldGravity(coords);
            
            }

            return gravity;
        
        }

        public static void CalculateLanes() {

            //SpawnLogger.Write("Lanes Prior To Calculation: " + Lanes.Count, SpawnerDebugEnum.Dev);
            //SpawnLogger.Write("Planets During Lane Calculation: " + Planets.Count, SpawnerDebugEnum.Dev);
            Lanes.Clear();

            for (int i = PlanetManager.Planets.Count - 1; i >= 0; i--) {

                var planetA = PlanetManager.Planets[i];

                if (planetA == null || planetA.Closed)
                    continue;

                for (int j = PlanetManager.Planets.Count - 1; j >= 0; j--) {

                    var planetB = PlanetManager.Planets[j];

                    if (planetB == null || planetB.Closed || planetA == planetB)
                        continue;

                    var laneExists = false;

                    if (Lanes.Count > 0) {

                        for (int k = Lanes.Count - 1; k >= 0; k--) {

                            var lane = Lanes[k];

                            if (!lane.Valid)
                                continue;

                            if (lane.CheckExistingLane(planetA, planetB)) {

                                laneExists = true;
                                break;

                            }

                        }

                    }
                    
                    if (!laneExists) {

                        Lanes.Add(new PlanetaryLane(planetA, planetB));

                    }

                }

            }

        }

        public static bool IsPositionInsideLane(Vector3D pos, string requiredPlanetA = null, string requiredPlanetB = null) {

            for (int i = Lanes.Count - 1; i >= 0; i--) {

                var lane = Lanes[i];

                if (!lane.Valid)
                    continue;

                if (lane.PositionCheck(pos)) {

                    var result = lane.NameCheck(requiredPlanetA) && lane.NameCheck(requiredPlanetB);

                    if (result)
                        return result;

                }

            }

            return false;

        }

        public static void GetLanesAtPosition(Vector3D pos, List<PlanetaryLane> lanes) {

            lanes.Clear();
            
            for (int i = Lanes.Count - 1; i >= 0; i--) {

                var lane = Lanes[i];

                if (!lane.Valid)
                    continue;

                //MyVisualScriptLogicProvider.ShowNotificationToAll("Lane: " + lane.PathBox.ToString(), 4000);

                if (lane.PositionCheck(pos)) {

                    lanes.Add(lane);

                }

            }
        
        }

        public static void Unload() {



        }

    }

    public class PlanetaryLane {

        public bool Valid;

        public PlanetEntity PlanetA;
        public PlanetEntity PlanetB;

        public double PlanetDistance;
        public Vector3D LaneDirection;
        public double Radius;

        //public BoundingBoxD PathBox;

        public PlanetaryLane(PlanetEntity planetA, PlanetEntity planetB) {

            Valid = true;

            PlanetA = planetA;
            planetA.ParentEntity.OnMarkForClose += CloseEntity;

            PlanetB = planetB;
            planetB.ParentEntity.OnMarkForClose += CloseEntity;

            CalculateLaneBox();

        }

        public void CalculateLaneBox() {

            var planetA = PlanetA.Planet.AverageRadius > PlanetB.Planet.AverageRadius ? PlanetA : PlanetB;
            var planetB = planetA == PlanetA ? PlanetB : PlanetA;
            PlanetA = planetA;
            PlanetB = planetB;
            LaneDirection = Vector3D.Normalize(planetB.Center() - planetA.Center());
            var avgCornerDist = (planetA.Planet.AverageRadius + planetB.Planet.AverageRadius) / 2;
            PlanetDistance = Vector3D.Distance(planetB.Center(), planetA.Center());
            Radius = avgCornerDist;

        }

        public bool CheckExistingLane(PlanetEntity a, PlanetEntity b) {

            return (a == PlanetA && b == PlanetB) || (a == PlanetB && b == PlanetA);

        }

        public bool PositionCheck(Vector3D pos) {

            var pathDistance = Math.Abs(Vector3D.Dot(PlanetA.Center() - pos, LaneDirection));
            var pointOnLine = LaneDirection * pathDistance + PlanetA.Center();
            //var gps = MyAPIGateway.Session.GPS.Create("LinePoint", "", pointOnLine, true, true);
            //MyAPIGateway.Session.GPS.AddLocalGps(gps);
            return Vector3D.Distance(pointOnLine, pos) <= Radius;

        }

        public bool NameCheck(string name = null) {

            if (!Valid)
                return false;

            return name == null || PlanetA.Planet.Generator.Id.SubtypeName == name || PlanetB.Planet.Generator.Id.SubtypeName == name;

        }

        public void CloseEntity(IMyEntity entity) {

            Valid = false;

        }

    }

}
