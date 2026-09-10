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
    /// Provides debug visualization for zones via translucent spheres.
    /// Allows admins to view zone boundaries, centers, and filter status.
    /// </summary>
    public static class ZoneDebugVisualizer {

        private static Dictionary<string, Zone> VisibleZones = new Dictionary<string, Zone>();

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

            if (VisibleZones.ContainsKey(zone.Name)) {

                chatMsg.ReturnMessage = $"Zone '{zone.PublicName}' sphere already shown.";
                return;

            }

            VisibleZones.Add(zone.PublicName, zone);
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

            if (!VisibleZones.ContainsKey(zone.PublicName)) {

                chatMsg.ReturnMessage = $"Zone '{zone.PublicName}' sphere not currently shown.";
                return;

            }

            VisibleZones.Remove(zone.PublicName);
            chatMsg.ReturnMessage = $"Zone '{zone.PublicName}' sphere hidden.";

        }

        /// <summary>
        /// Show debug spheres for all active zones in the world.
        /// Returns count of spheres shown via chatMsg.ReturnMessage.
        /// </summary>
        public static void ShowAllZones(Sync.ChatMessage chatMsg) {

            int countAdded = 0;

            foreach (var zone in ZoneManager.ActiveZones) {

                if (!VisibleZones.ContainsKey(zone.PublicName)) {

                    VisibleZones.Add(zone.PublicName, zone);
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
            VisibleZones.Clear();
            chatMsg.ReturnMessage = $"Hidden {countRemoved} zone sphere(s).";

        }

        /// <summary>
        /// Update and draw all visible zone spheres. Call once per frame from SessionCore.Update().
        /// Only draws spheres for active zones. Colors indicate zone filter status.
        /// </summary>
        public static void UpdateDraw() {

            // Clean up zones that are no longer active
            var keysToRemove = new List<string>();

            foreach (var kvp in VisibleZones) {

                if (!kvp.Value.Active) {

                    keysToRemove.Add(kvp.Key);

                }

            }

            foreach (var key in keysToRemove) {

                VisibleZones.Remove(key);

            }

            // Draw remaining visible zone spheres
            foreach (var kvp in VisibleZones) {

                var zone = kvp.Value;

                Color sphereColor = GetZoneColor(zone);
                //Color sphereColor = new Color(0, 255, 0, 255);
                MatrixD sphereMatrix = MatrixD.CreateTranslation(zone.Coordinates);

                Vector3D cameraPos = MyTransparentGeometry.Camera.Translation;
                double cameraToSphereDistance = Vector3D.Distance(cameraPos, zone.Coordinates);

                MySimpleObjectDraw.DrawTransparentSphere(
                    ref sphereMatrix,
                    (float)zone.Radius,
                    ref sphereColor,
                    MySimpleObjectRasterizer.Wireframe,
                    32,
                    faceMaterial: null,
                    lineMaterial: MyStringId.GetOrCompute("Square"),
                    lineThickness: 15f
                    //lineThickness: 0.01f + (0.01f * (float)(zone.Radius / cameraToSphereDistance))
                );

            }

        }

        /// <summary>
        /// Determine sphere color based on zone filtering flags.
        /// Green = allowlist, Red = blocklist, Yellow = both, White = none.
        /// </summary>
        private static Color GetZoneColor(Zone zone) {

            bool hasAllowFilter = zone.UseAllowedSpawnGroups || zone.UseAllowedModIDs;
            bool hasRestrictFilter = zone.UseRestrictedSpawnGroups || zone.UseRestrictedModIDs;

            if (!zone.Active)
            {
                return Color.DarkGray;  // Inactive zone
            }

            if (hasAllowFilter && hasRestrictFilter) {

                return Color.Blue;  // Both allow and restrict active

            }

            if (hasAllowFilter) {

                return Color.Green;  // Allowlist only

            }

            if (hasRestrictFilter) {

                return Color.Red;  // Restrict only

            }

            return Color.Yellow;  // No filtering flags

        }

    }

}