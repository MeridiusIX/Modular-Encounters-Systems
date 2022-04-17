using ModularEncountersSystems.Core;
using ModularEncountersSystems.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.API {
    public static class APIs {

        //Systems
        public static MESApi MES;

        //WeaponCore
        public static WcApi WeaponCore = new WcApi(); //Register in BeforeStart
        public static bool WeaponCoreApiLoaded = false;

        //DefenseShields
        public static ShieldApi Shields = new ShieldApi(); //Register in Before Start
        public static bool ShieldsApiLoaded = false;

        //WaterMod
        public static bool WaterModApiLoaded => WaterAPI.Registered;

        //Nebula Mod
        public static NebulaAPI Nebula = new NebulaAPI();
        public static bool NebulaApiLoaded => NebulaAPI.Registered;

        //AI Enabled
        public static RemoteBotAPI AiEnabled;
        public static bool AiEnabledApiLoaded { get { return AiEnabled?.Valid ?? false; } }

        //Aerodynamic Drag
        public static RemoteDragSettings Drag;
        public static bool DragApiLoaded { get { return Drag?.Heartbeat ?? false; } }

        //Text Hud
        public static HudAPIv2 TextHud;
        public static bool TextHudApiLoaded { get { return TextHud?.Heartbeat ?? false; } }
        public static HudAPIv2.HUDMessage HudMessage = null;

        public static void RegisterAPIs(int phase = 0) {

            if (MES == null && phase == 0 && MES_SessionCore.IsServer)
                MES = new MESApi();

            //Water Mod (LoadData)
            if (AddonManager.WaterMod && phase == 0) {

                WaterAPI.LoadData();

            }

            if (AddonManager.NebulaMod && phase == 0 && MES_SessionCore.IsServer) {

                NebulaAPI.LoadData();
                MES_SessionCore.UnloadActions += NebulaAPI.UnloadData;

            }

            if (AddonManager.AiEnabled && phase == 0 && MES_SessionCore.IsServer)
                AiEnabled = new RemoteBotAPI();

            if (AddonManager.AerodynamicDrag && phase == 0 && MES_SessionCore.IsServer)
                Drag = new RemoteDragSettings();

            //WeaponCore (BeforeStart)
            if (AddonManager.WeaponCore && phase == 2 && MES_SessionCore.IsServer)
                WeaponCore.Load(AddonManager.WeaponCoreCallback, true);

            //DefenseShields (BeforeStart)
            if (AddonManager.DefenseShields && phase == 2) {

                Shields.Load();

                if (Shields.IsReady) {

                    APIs.ShieldsApiLoaded = true;
                    SpawnLogger.Write("DefenseShield API Loaded", SpawnerDebugEnum.Startup);
                }

            }
                

        }

    }
}
