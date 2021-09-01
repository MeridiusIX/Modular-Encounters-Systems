using ModularEncountersSystems.Core;
using ModularEncountersSystems.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.API {
    public static class APIs {

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

        public static void RegisterAPIs(int phase = 0) {

            //Water Mod (LoadData)
            if (AddonManager.WaterMod && phase == 0) {

                WaterAPI.LoadData();

            }

            if (AddonManager.NebulaMod && phase == 0) {

                NebulaAPI.LoadData();
                MES_SessionCore.UnloadActions += NebulaAPI.UnloadData;

            }

            if (AddonManager.AiEnabled && phase == 0)
                AiEnabled = new RemoteBotAPI();

            //WeaponCore (BeforeStart)
            if (AddonManager.WeaponCore && phase == 2)
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
