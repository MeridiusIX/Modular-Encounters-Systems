using ModularEncountersSystems.Zones;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace ModularEncountersSystems.Helpers {

    /// <summary>
    /// Provides debug visualization for zones via translucent spheres and GPS markers.
    /// Allows admins to view zone boundaries, centers, and filter status.
    /// </summary>
    public static class ZoneDebugVisualizer {

        private class VisibleZoneEntry {

            public Zone Zone;
            public long PlayerId;
            public List<string> GpsNames = new List<string>();

        }

        private static Dictionary<string, VisibleZoneEntry> VisibleZones = new Dictionary<string, VisibleZoneEntry>();

        private static string GetZoneKey(Zone zone) {

            return !string.IsNullOrWhiteSpace(zone.PublicName) ? zone.PublicName : zone.Name;

        }

        private static string BuildGpsName(Zone zone) {

            return $"[MES Zone] {zone.PublicName}";

        }

        private static void AddZoneGps(VisibleZoneEntry entry) {

            var zone = entry.Zone;
            var color = GetZoneColor(zone);
            entry.GpsNames = new List<string>();

            var name = $"[MES Zone] {zone.PublicName}";
            entry.GpsNames.Add(name);
            MyVisualScriptLogicProvider.AddGPS(
                name,
                $"Debug marker for zone '{zone.PublicName}' (r={zone.Radius:F0}m)",
                zone.Coordinates,
                color,
                0,
                entry.PlayerId);


            if (zone.CoordinateRadiusPairs == null)
                return;

            for (int i = 0; i < zone.CoordinateRadiusPairs.Count; i++) {

                var pair = zone.CoordinateRadiusPairs[i];
                var gpsname = $"[MES Zone] {zone.PublicName} #{i + 1}";
                entry.GpsNames.Add(gpsname);
                MyVisualScriptLogicProvider.AddGPS(
                    gpsname,
                    $"Debug marker for zone '{zone.PublicName}' pair {i + 1} (r={pair.Value:F0}m)",
                    pair.Key,
                    color,
                    0,
                    entry.PlayerId);

            }

        }

        private static void RemoveZoneGps(VisibleZoneEntry entry) {

            if (entry?.GpsNames == null)
                return;

            foreach (var name in entry.GpsNames) {

                if (!string.IsNullOrWhiteSpace(name))
                    MyVisualScriptLogicProvider.RemoveGPS(name, entry.PlayerId);

            }

            entry.GpsNames.Clear();

        }

        private static void AddVisibleZone(Zone zone, long playerId) {

            var entry = new VisibleZoneEntry {
                Zone = zone,
                PlayerId = playerId
            };

            VisibleZones[GetZoneKey(zone)] = entry;
            AddZoneGps(entry);

        }

        private static void RemoveVisibleZone(string key) {

            VisibleZoneEntry entry;

            if (!VisibleZones.TryGetValue(key, out entry))
                return;

            RemoveZoneGps(entry);
            VisibleZones.Remove(key);

        }

        /// <summary>
        /// Show a debug sphere for a zone by name or ProfileSubtypeId.
        /// Returns status/error message via chatMsg.ReturnMessage.
        /// </summary>
        public static void ShowZone(string zoneIdentifier, Sync.ChatMessage chatMsg) {

            if (string.IsNullOrWhiteSpace(zoneIdentifier)) {

                chatMsg.ReturnMessage = "Zone identifier cannot be empty.";
                return;

            }

            var zone = ZoneManager.FindZoneByNameOrSubtype(zoneIdentifier);

            if (zone == null) {

                chatMsg.ReturnMessage = $"Zone '{zoneIdentifier}' not found.";
                return;

            }

            var key = GetZoneKey(zone);

            if (VisibleZones.ContainsKey(key)) {

                chatMsg.ReturnMessage = $"Zone '{zone.PublicName}' sphere already shown.";
                return;

            }

            AddVisibleZone(zone, chatMsg.PlayerId);
            chatMsg.ReturnMessage = $"Zone '{zone.PublicName}' sphere now visible at {zone.Coordinates}.";

        }

        /// <summary>
        /// Hide a previously shown zone debug sphere by name or ProfileSubtypeId.
        /// Returns status/error message via chatMsg.ReturnMessage.
        /// </summary>
        public static void HideZone(string zoneIdentifier, Sync.ChatMessage chatMsg) {

            if (string.IsNullOrWhiteSpace(zoneIdentifier)) {

                chatMsg.ReturnMessage = "Zone identifier cannot be empty.";
                return;

            }

            var zone = ZoneManager.FindZoneByNameOrSubtype(zoneIdentifier);

            if (zone == null) {

                chatMsg.ReturnMessage = $"Zone '{zoneIdentifier}' not found.";
                return;

            }

            var key = GetZoneKey(zone);

            if (!VisibleZones.ContainsKey(key)) {

                chatMsg.ReturnMessage = $"Zone '{zone.PublicName}' sphere not currently shown.";
                return;

            }

            RemoveVisibleZone(key);
            chatMsg.ReturnMessage = $"Zone '{zone.PublicName}' sphere hidden.";

        }

        /// <summary>
        /// Show debug spheres for all active zones in the world.
        /// Returns count of spheres shown via chatMsg.ReturnMessage.
        /// </summary>
        public static void ShowAllZones(Sync.ChatMessage chatMsg) {

            int countAdded = 0;

            foreach (var zone in ZoneManager.ActiveZones) {

                var key = GetZoneKey(zone);

                if (!VisibleZones.ContainsKey(key)) {

                    AddVisibleZone(zone, chatMsg.PlayerId);
                    countAdded++;

                }

            }

            int totalVisible = VisibleZones.Count;
            chatMsg.ReturnMessage = $"Now showing spheres for {countAdded} additional zones. Total visible: {totalVisible}.";

        }

        /// <summary>
        /// Hide all currently visible zone debug spheres.
        /// Returns count of spheres hidden via chatMsg.ReturnMessage.
        /// </summary>
        public static void HideAllZones(Sync.ChatMessage chatMsg) {

            int countRemoved = VisibleZones.Count;
            var keys = new List<string>(VisibleZones.Keys);

            foreach (var key in keys) {

                RemoveVisibleZone(key);

            }

            chatMsg.ReturnMessage = $"Hidden {countRemoved} zone sphere(s).";

        }

        /// <summary>
        /// Update and draw all visible zone spheres. Call once per frame from SessionCore.Update().
        /// Draws both active and inactive zones. Colors indicate filter status / inactive state.
        /// </summary>
        public static void UpdateDraw() {

            foreach (var kvp in VisibleZones) {

                var zone = kvp.Value.Zone;
                Color sphereColor = GetZoneColor(zone);

                DrawZoneSphere(zone.Coordinates, zone.Radius, ref sphereColor);

                if (zone.CoordinateRadiusPairs == null)
                    continue;

                foreach (var pair in zone.CoordinateRadiusPairs) {

                    DrawZoneSphere(pair.Key, pair.Value, ref sphereColor);

                }

            }

        }

        private static void DrawZoneSphere(Vector3D center, double radius, ref Color color) {

            if (radius <= 0)
                return;

            MatrixD sphereMatrix = MatrixD.CreateTranslation(center);

            Vector3D cameraPos = MyTransparentGeometry.Camera.Translation;
            double distanceToCenter = Vector3D.Distance(cameraPos, center);
            // Nearest point on the sphere surface (0 when camera is on the surface)
            double distanceToSurface = Math.Abs(distanceToCenter - radius);

            // World-space thickness scales with distance to the surface so wireframe
            // width stays roughly consistent on-screen; clamp so it never disappears up close.
            float lineThickness = Math.Max(1f, (float)(distanceToSurface * 0.002));

            MySimpleObjectDraw.DrawTransparentSphere(
                ref sphereMatrix,
                (float)radius,
                ref color,
                MySimpleObjectRasterizer.Wireframe,
                32,
                faceMaterial: null,
                lineMaterial: MyStringId.GetOrCompute("Square"),
                lineThickness: lineThickness
            );

        }

        /// <summary>
        /// Determine sphere/GPS color based on zone filtering flags.
        /// Green = allowlist, Red = blocklist, Yellow = both, Blue = none, DarkGray = inactive.
        /// </summary>
        private static Color GetZoneColor(Zone zone) {

            bool hasAllowFilter = zone.UseAllowedSpawnGroups || zone.UseAllowedModIDs;
            bool hasRestrictFilter = zone.UseRestrictedSpawnGroups || zone.UseRestrictedModIDs;

            if (!zone.Active) {

                return Color.DarkGray;

            }

            if (hasAllowFilter && hasRestrictFilter) {

                return Color.Yellow;

            }

            if (hasAllowFilter) {

                return Color.Green;

            }

            if (hasRestrictFilter) {

                return Color.Red;

            }

            return Color.Blue;

        }

    }

}