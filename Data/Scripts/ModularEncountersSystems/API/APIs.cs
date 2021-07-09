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
        public static bool WaterModApiLoaded = false;

        //AI Enabled
        public static AiEnabledApi AiEnabled;
        public static bool AiEnabledApiLoaded { get { return AiEnabled?.Valid ?? false; } }

        public static void RegisterAPIs(int phase = 0) {

            //Water Mod (LoadData)
            if (AddonManager.WaterMod && phase == 0)
                WaterAPI.LoadData();

            if (AddonManager.AiEnabled && phase == 0)
                AiEnabled = new AiEnabledApi();

            //WeaponCore (BeforeStart)
            if (AddonManager.WeaponCore && phase == 2)
                WeaponCore.Load(AddonManager.WeaponCoreCallback, true);

            //DefenseShields (BeforeStart)
            if (AddonManager.DefenseShields && phase == 2) {

                Shields.Load();

                if (Shields.IsReady) {

                    APIs.ShieldsApiLoaded = true;
                
                }

            }
                

        }

    }
}
