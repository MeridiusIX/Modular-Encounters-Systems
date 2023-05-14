using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.Spawning.Profiles;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;

namespace ModularEncountersSystems.Entities {
    public static class SafeZoneManager {

        public static List<SafeZoneEntity> SafeZones = new List<SafeZoneEntity>();

        public static IMyEntity CreateSafeZone(MatrixD coords, SafeZoneProfile profile, string name, IMyEntity parentEntity = null) {

            var zoneOb = new MyObjectBuilder_SafeZone();
            zoneOb.Enabled = profile.Enabled;
            zoneOb.IsVisible = profile.IsVisible;
            var color = new Color(profile.Color.X, profile.Color.Y, profile.Color.Z).ToVector3();
            zoneOb.ModelColor = color;
            zoneOb.Radius = (float)profile.Radius;

            if (parentEntity != null) {

                var parent = parentEntity as IMyCubeGrid;

                if (parent == null) {

                    var block = parentEntity as IMyCubeBlock;

                    if (block != null)
                        parent = block.SlimBlock.CubeGrid;
                
                }

                if (parent != null) {

                    if (profile.RadiusFromParentEntity) {

                        var boxCenter = (parent.WorldAABB.Min + parent.WorldAABB.Max) * 0.5;
                        zoneOb.Radius = (float)((Vector3D.Distance(boxCenter, parent.WorldAABB.Max)) * profile.ParentEntityRadiusMultiplier);

                    }

                    if (profile.LinkToParentEntity) {

                        //MyVisualScriptLogicProvider.ShowNotificationToAll("Adding Safezone Storage", 4000);
                        StorageTools.ApplyCustomEntityStorage(zoneOb, StorageTools.MesSafeZoneLinkedEntity, parent.EntityId.ToString());
                    
                    }
                    
                }
            
            }

            zoneOb.Shape = profile.Shape;
            zoneOb.Texture = profile.Texture;
            zoneOb.Size = profile.Size;
            zoneOb.PersistentFlags = MyPersistentEntityFlags2.CastShadows | MyPersistentEntityFlags2.InScene;
            zoneOb.DisplayName = name;

            var matrix = coords;
            matrix.Translation = Vector3D.Transform(profile.Offset, matrix);

            if (profile.UseDiamondBoxOrientation) {

                var forward = Vector3D.Normalize(matrix.Forward + matrix.Up + matrix.Right);
                var newMatrix = MatrixD.CreateWorld(matrix.Translation, forward, Vector3D.Normalize(Vector3D.CalculatePerpendicularVector(forward)));
                matrix = newMatrix;

            }

            //MyVisualScriptLogicProvider.ShowNotificationToAll(color.ToString(), 10000);

            zoneOb.PositionAndOrientation = new VRage.MyPositionAndOrientation(matrix.Translation, matrix.Forward, matrix.Up);

            //The following is hacky because keen forgot* to whitelist the safezone enums. >_>
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

        public static void MonitorEntitySafezones() {

            //MyVisualScriptLogicProvider.ShowNotificationToAll("Check Safezones", 2000);

            for (int i = SafeZones.Count - 1; i >= 0; i--) {

                var safezone = SafeZones[i];

                if (safezone?.SafeZone == null) {

                    SafeZones.RemoveAt(i);
                    continue;

                }

                if (safezone.LinkedSafezoneEntityId == 0)
                    continue;

                bool remove = false;

                if (safezone.LinkedSafezoneEntity == null) {

                    if (!MyAPIGateway.Entities.TryGetEntityById(safezone.LinkedSafezoneEntityId, out safezone.LinkedSafezoneEntity))
                        remove = true;
                   
                }

                if (remove || (safezone.LinkedSafezoneEntity?.MarkedForClose ?? true)) {

                    safezone.SafeZone.Close();
                    SafeZones.RemoveAt(i);
                    continue;

                }
                  
            }

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
