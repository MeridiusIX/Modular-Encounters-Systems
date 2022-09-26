using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Entities {
    public static class SafeZoneManager {

        public static List<SafeZoneEntity> SafeZones = new List<SafeZoneEntity>();

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
