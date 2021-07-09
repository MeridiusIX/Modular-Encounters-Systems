using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Entities {
    public static class SafeZoneManager {

        public static List<SafeZoneEntity> SafeZones = new List<SafeZoneEntity>();

        public static bool IsPositionInSafeZone(Vector3D coords) {

            foreach (var zone in SafeZones) {

                if (zone.InZone(coords))
                    return true;
            
            }

            return false;
        
        }

    }

}
