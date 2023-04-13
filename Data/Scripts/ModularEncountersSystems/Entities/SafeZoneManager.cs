using ModularEncountersSystems.Spawning.Profiles;
using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;

namespace ModularEncountersSystems.Entities {
    public static class SafeZoneManager {

        public static List<SafeZoneEntity> SafeZones = new List<SafeZoneEntity>();

        public static IMyEntity CreateSafeZone(MatrixD coords, SafeZoneProfile profile) {

            var zoneOb = new MyObjectBuilder_SafeZone();
            zoneOb.Enabled = profile.Enabled;
            zoneOb.IsVisible = profile.IsVisible;
            zoneOb.ModelColor = profile.Color / 255;
            zoneOb.Radius = (float)profile.Radius;
            zoneOb.Shape = profile.Shape;
            zoneOb.Texture = profile.Texture;
            zoneOb.Size = profile.Size;
            zoneOb.PersistentFlags = MyPersistentEntityFlags2.CastShadows | MyPersistentEntityFlags2.InScene;
            //The following is hacky because keen forgot to whitelist the safezone enums. >_>
            Enum.TryParse((profile.FactionAccess.ToString()), out zoneOb.AccessTypeFactions);
            Enum.TryParse((profile.GridAccess.ToString()), out zoneOb.AccessTypeGrids);
            Enum.TryParse((profile.PlayerAccess.ToString()), out zoneOb.AccessTypePlayers);
            Enum.TryParse((profile.AllowedActions.ToString()), out zoneOb.AllowedActions);

            MyAPIGateway.Entities.RemapObjectBuilder(zoneOb);
            return MyAPIGateway.Entities.CreateFromObjectBuilderAndAdd(zoneOb);

        }

        public static SafeZoneEntity GetSafeZoneAtPosition(Vector3D coords) {

            for (int i = SafeZones.Count - 1; i >= 0; i--) {

                var safezone = SafeZones[i];

                if (safezone.InZone(coords))
                    return safezone;
            
            }

            return null;
        
        }

        public static SafeZoneEntity GetSafeZoneWithEntityId(long id) {

            for (int i = SafeZones.Count - 1; i >= 0; i--) {

                var safezone = SafeZones[i];

                if ((safezone.SafeZone?.EntityId ?? 0) == id)
                    return safezone;

            }

            return null;

        }

        public static bool IsPositionInSafeZone(Vector3D coords) {

            foreach (var zone in SafeZones) {

                if (zone.InZone(coords))
                    return true;
            
            }

            return false;
        
        }

    }

}
