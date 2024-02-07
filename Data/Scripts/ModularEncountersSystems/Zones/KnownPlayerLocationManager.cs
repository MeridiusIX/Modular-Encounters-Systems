using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Logging;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Game.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Zones {

    public static class KnownPlayerLocationManager {

        public static void AddKnownPlayerLocation(Vector3D coords, string faction, double radius, int duration = -1, int maxEncounters = -1, int minThreatToAvoidAbandonment = -1) {

            bool foundExistingLocation = false;
            var sphere = new BoundingSphereD(coords, radius);
            List<Zone> intersectingLocations = new List<Zone>();

            foreach (var zone in ZoneManager.ActiveZones) {

                if (!zone.PlayerKnownLocation)
                    continue;

                if (!zone.Factions.Contains(faction)) {

                    continue;

                }

                if (!sphere.Intersects(zone.Sphere) && zone.Sphere.Contains(coords) == ContainmentType.Disjoint) {

                    continue;

                }

                intersectingLocations.Add(zone);
                foundExistingLocation = true;

            }

            if (foundExistingLocation == false) {

                SpawnLogger.Write("Creating KnownPlayerLocation at " + coords.ToString() + " / Radius: " + radius.ToString(), SpawnerDebugEnum.Zone);

                var newZone = new Zone();
                newZone.InitAsKnownPlayerLocation(faction, coords, radius, duration, maxEncounters, minThreatToAvoidAbandonment);
                ZoneManager.ActiveZones.Add(newZone);

                AlertPlayersOfNewKPL(coords, radius, faction);

            } else {

                SpawnLogger.Write("Known Player Location(s) Already Exist, Merging Locations.", SpawnerDebugEnum.Zone);
                var newZone = new Zone();
                newZone.InitAsKnownPlayerLocation(faction, coords, radius, duration, maxEncounters, minThreatToAvoidAbandonment);

                foreach (var location in intersectingLocations) {

                    newZone.MergeExistingKnownPlayerLocation(location);
                    ZoneManager.ActiveZones.Remove(location);

                }

                ZoneManager.ActiveZones.Add(newZone);
                AlertPlayersOfNewKPL(newZone.Coordinates, newZone.Radius, faction);

            }

            ZoneManager.FlagUpdateZoneStorage();

        }

        public static void AlertPlayersOfNewKPL(Vector3D coords, double radius, string faction) {

            foreach (var playerEnt in PlayerManager.Players) {

                if (!playerEnt.ActiveEntity())
                    continue;

                if (playerEnt.Player.IsBot == true) {

                    continue;

                }

                if (Vector3D.Distance(playerEnt.Player.GetPosition(), coords) > radius) {

                    continue;

                }

                if (string.IsNullOrWhiteSpace(faction)) {

                    MyVisualScriptLogicProvider.ShowNotification("This area has been identified as \"Player Occupied\"", 5000, "Red", playerEnt.Player.IdentityId);

                } else {

                    MyVisualScriptLogicProvider.ShowNotification("This area has been identified as \"Player Occupied\" by " + faction, 5000, "Red", playerEnt.Player.IdentityId);

                }

            }

        }

        public static void ChangeZoneSizeAtLocation(Vector3D coords, string faction = "", double size = 0, bool isMultiplicative = false) {

            foreach (var location in ZoneManager.ActiveZones) {

                if (IsPositionInKnownPlayerLocation(location, coords, true, faction))
                    continue;

                if (!isMultiplicative) {

                    location.Radius += size;

                } else {

                    location.Radius *= size;

                }

            }

        }

        public static void CleanExpiredLocations() {

            bool needsUpdate = false;

            for (int i = ZoneManager.ActiveZones.Count - 1; i >= 0; i--) {

                if (!ZoneManager.ActiveZones[i].PlayerKnownLocation)
                    continue;

                var zone = ZoneManager.ActiveZones[i];
                var duration = MyAPIGateway.Session.GameDateTime - zone.TimeCreated;

                if (zone.Radius <= 0) {

                    SpawnLogger.Write(string.Format("Player Known Location At [{0}] Has Been Removed Because its Radius is 0 or Less", zone.Coordinates), SpawnerDebugEnum.Zone);
                    ZoneManager.ActiveZones.RemoveAt(i);
                    needsUpdate = true;
                    continue;

                }

                if (zone.MinutesToExpiration >= 0 && duration.TotalSeconds / 60 >= zone.MinutesToExpiration) {

                    SpawnLogger.Write(string.Format("Player Known Location At [{0}] Has Been Removed Because its Timer Expired", zone.Coordinates), SpawnerDebugEnum.Zone);
                    ZoneManager.ActiveZones.RemoveAt(i);
                    needsUpdate = true;
                    continue;

                }

                if (zone.MaxSpawnedEncounters >= 0 && zone.SpawnedEncounters >= zone.MaxSpawnedEncounters) {

                    SpawnLogger.Write(string.Format("Player Known Location At [{0}] Has Been Removed Because it has Exceeded Spawn Count", zone.Coordinates), SpawnerDebugEnum.Zone);
                    ZoneManager.ActiveZones.RemoveAt(i);
                    needsUpdate = true;
                    continue;

                }

            }

            if (needsUpdate == true) {

                ZoneManager.FlagUpdateZoneStorage();

            }

        }

        public static bool IsPositionInKnownPlayerLocation(Vector3D coords, bool matchFaction = false, string faction = "") {

            foreach (var location in ZoneManager.ActiveZones) {

                if (IsPositionInKnownPlayerLocation(location, coords, matchFaction, faction))
                    return true;

            }

            return false;

        }

        public static bool IsPositionInKnownPlayerLocation(Zone zone, Vector3D coords, bool matchFaction = false, string faction = "") {

            if (!zone.PlayerKnownLocation)
                return false;

            if (matchFaction == true && !zone.Factions.Contains(faction)) {

                return false;

            }

            if (Vector3D.Distance(coords, zone.Coordinates) > zone.Radius) {

                return false;

            }

            return true;

        }

        public static void IncreaseSpawnCountOfLocations(Vector3D coords, string faction) {

            var updateZones = false;

            foreach (var zone in ZoneManager.ActiveZones) {

                if (!zone.PlayerKnownLocation)
                    continue;

                if (zone.Factions.Count > 0 && !zone.Factions.Contains(faction))
                    continue;

                if (IsPositionInKnownPlayerLocation(zone, coords, false)) {

                    updateZones = true;
                    zone.TimeCreated = MyAPIGateway.Session.GameDateTime;
                    zone.SpawnedEncounters++;

                }

            }

            if(updateZones)
                ZoneManager.FlagUpdateZoneStorage();

        }

        public static void RemoveLocation(Vector3D coords, string faction = "", bool removeAllZones = false) {

            bool removedLocation = false;

            for (int i = ZoneManager.ActiveZones.Count - 1; i >= 0; i--) {

                var zone = ZoneManager.ActiveZones[i];

                if (!zone.PlayerKnownLocation)
                    continue;

                if (zone.Sphere.Contains(coords) != ContainmentType.Disjoint) {

                    if (removeAllZones || !zone.Factions.Contains(faction) || zone.Factions.Count == 0) {

                        SpawnLogger.Write(string.Format("Player Known Location At [{0}] Has Been Removed", zone.Coordinates), SpawnerDebugEnum.Zone);
                        ZoneManager.ActiveZones.RemoveAt(i);
                        removedLocation = true;

                    }

                }

            }

            if (removedLocation)
                ZoneManager.FlagUpdateZoneStorage();

        }

    }

}
