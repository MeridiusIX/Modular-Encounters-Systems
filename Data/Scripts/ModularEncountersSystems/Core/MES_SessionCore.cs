using ModularEncountersSystems.API;
using ModularEncountersSystems.Behavior;
using ModularEncountersSystems.BlockLogic;
using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.Sync;
using ModularEncountersSystems.Tasks;
using ModularEncountersSystems.Watchers;
using ModularEncountersSystems.World;
using ModularEncountersSystems.Zones;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.Components;

namespace ModularEncountersSystems.Core {

    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    public class MES_SessionCore : MySessionComponentBase {

        public static bool ModEnabled = true;

        public static string ModVersion = "2.0.21";
        public static MES_SessionCore Instance;

        public static bool IsServer;
        public static bool IsDedicated;

        public static Action UnloadActions;

        public override void LoadData() {

            IsServer = MyAPIGateway.Multiplayer.IsServer;
            IsDedicated = MyAPIGateway.Utilities.IsDedicated;
            ModEnabled = CheckSyncRules();

            if (!ModEnabled)
                return;

            Instance = this;

            SpawnLogger.Setup();
            BehaviorLogger.Setup();
            TaskProcessor.Setup();
            SyncManager.Setup(); //Register Network and Chat Handlers
            DefinitionHelper.Setup();
            Settings.InitSettings(); //Get Existing Settings From XML or Create New
            AddonManager.DetectAddons(); //Check Add-on Mods
            BotSpawner.Setup();

            if (!IsServer)
                return;

            APIs.RegisterAPIs(0); //Register Any Applicable APIs
            BlockManager.Setup(); //Build Lists of Special Blocks
            PlayerSpawnWatcher.Setup();
            PrefabSpawner.Setup();
            PrefabManipulation.Setup();

        }

        public override void Init(MyObjectBuilder_SessionComponent sessionComponent) {

            if (!ModEnabled)
                return;

            if (!MyAPIGateway.Multiplayer.IsServer)
                return;

            

        }

        public override void BeforeStart() {

            if (!ModEnabled)
                return;

            ProfileManager.Setup();
            BlockLogicManager.Setup();
            SpawnGroupManager.CreateSpawnLists();
            EntityWatcher.RegisterWatcher(); //Scan World For Entities and Setup AutoDetect For New Entities
            SetDefaultSettings();

            if (!MyAPIGateway.Multiplayer.IsServer)
                return;

            APIs.RegisterAPIs(2); //Register Any Applicable APIs
            FactionHelper.PopulateNpcFactionLists();
            EventWatcher.Setup();
            NpcManager.Setup();
            CargoShipWatcher.Setup();
            ZoneManager.Setup();
            BehaviorManager.Setup();
            RelationManager.Setup();
            Cleaning.Setup();
            WaveManager.Setup();
            DamageHelper.Setup();

            //AttributeApplication

        }

        public override void UpdateBeforeSimulation() {

            if (!ModEnabled) {

                MyAPIGateway.Utilities.InvokeOnGameThread(() => { this.UpdateOrder = MyUpdateOrder.NoUpdate; });
                return;
            
            }

            TaskProcessor.Process();

        }

        protected override void UnloadData() {

            UnloadActions?.Invoke();

        }

        private static bool CheckSyncRules() {

            if (!IsDedicated)
                return true;

            if (MyAPIGateway.Session.SessionSettings.EnableSelectivePhysicsUpdates && MyAPIGateway.Session.SessionSettings.SyncDistance < 10000) {

                //TODO: Log SPU Restriction
                SpawnLogger.Write("Mod Disabled: Selective Physics Updates is Enabled with SyncDistance Less Than 10000", SpawnerDebugEnum.Startup, true);
                SpawnLogger.Write("Disable Selective Physics Updates OR Increase SyncDistance To Minimum of 10000", SpawnerDebugEnum.Startup, true);
                return false;

            }
                

            return true;
        
        }

        private static void SetDefaultSettings() {

            if (MyAPIGateway.Session.SessionSettings.CargoShipsEnabled)
                MyAPIGateway.Session.SessionSettings.CargoShipsEnabled = false;

            if (MyAPIGateway.Session.SessionSettings.EnableEncounters)
                MyAPIGateway.Session.SessionSettings.EnableEncounters = false;

        }

    }
}
