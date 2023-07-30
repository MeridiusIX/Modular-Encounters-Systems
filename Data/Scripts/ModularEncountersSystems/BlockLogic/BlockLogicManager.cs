using ModularEncountersSystems.Behavior;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.Tasks;
using ModularEncountersSystems.Terminal;
using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;

namespace ModularEncountersSystems.BlockLogic {
    public static class BlockLogicManager {

        public static Dictionary<long, IBlockLogic> LogicBlocks = new Dictionary<long, IBlockLogic>();

        public static List<MyDefinitionId> RivalAiControlModules = new List<MyDefinitionId>();
        public static List<MyDefinitionId> DisposableBeaconIds = new List<MyDefinitionId>();
        public static List<MyDefinitionId> NpcThrustIds = new List<MyDefinitionId>();
        public static List<MyDefinitionId> ProprietaryReactorsIds = new List<MyDefinitionId>();
        public static List<MyDefinitionId> JetpackInhibitorIds = new List<MyDefinitionId>();
        public static List<MyDefinitionId> HandDrillInhibitorIds = new List<MyDefinitionId>();
        public static List<MyDefinitionId> JumpInhibitorIds = new List<MyDefinitionId>();
        public static List<MyDefinitionId> NanobotInhibitorIds = new List<MyDefinitionId>();
        public static List<MyDefinitionId> PlayerInhibitorIds = new List<MyDefinitionId>();
        public static List<MyDefinitionId> EnergyInhibitorIds = new List<MyDefinitionId>();
        public static List<MyDefinitionId> TurretControllers = new List<MyDefinitionId>();

        public static List<long> TrackedLogicIds = new List<long>();

        public static void Setup() {

            RivalAiControlModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RemoteControl), "RivalAIRemoteControlSmall"));
            RivalAiControlModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RemoteControl), "RivalAIRemoteControlLarge"));
            RivalAiControlModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RemoteControl), "K_Imperial_Dropship_Guild_RC"));
            RivalAiControlModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RemoteControl), "K_TIE_Fighter_RC"));
            RivalAiControlModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RemoteControl), "K_NewRepublic_EWing_RC"));
            RivalAiControlModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RemoteControl), "K_Imperial_RC_Largegrid"));
            RivalAiControlModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RemoteControl), "K_TIE_Drone_Core"));
            RivalAiControlModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RemoteControl), "K_Imperial_SpeederBike_FakePilot"));
            RivalAiControlModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RemoteControl), "K_Imperial_ProbeDroid_Top_II"));
            RivalAiControlModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RemoteControl), "K_Imperial_DroidCarrier_DroidBrain"));
            RivalAiControlModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RemoteControl), "K_Imperial_DroidCarrier_DroidBrain_Aggressor"));

            DisposableBeaconIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Beacon), "DisposableNpcBeaconSmall"));
            DisposableBeaconIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Beacon), "DisposableNpcBeaconLarge"));

            ProprietaryReactorsIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Reactor), "ProprietarySmallBlockSmallGenerator"));
            ProprietaryReactorsIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Reactor), "ProprietarySmallBlockLargeGenerator"));
            ProprietaryReactorsIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Reactor), "ProprietaryLargeBlockSmallGenerator"));
            ProprietaryReactorsIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Reactor), "ProprietaryLargeBlockLargeGenerator"));

            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-Ion-LargeGrid-Large"));
            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-Ion-LargeGrid-Small"));
            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-Ion-SmallGrid-Large"));
            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-Ion-SmallGrid-Small"));

            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-Hydro-LargeGrid-Large"));
            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-Hydro-LargeGrid-Small"));
            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-Hydro-SmallGrid-Large"));
            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-Hydro-SmallGrid-Small"));

            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-Atmo-LargeGrid-Large"));
            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-Atmo-LargeGrid-Small"));
            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-Atmo-SmallGrid-Large"));
            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-Atmo-SmallGrid-Small"));

            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-IonSciFi-LargeGrid-Large"));
            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-IonSciFi-LargeGrid-Small"));
            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-IonSciFi-SmallGrid-Large"));
            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-IonSciFi-SmallGrid-Small"));

            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-WarfareIon-LargeGrid-Large"));
            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-WarfareIon-LargeGrid-Small"));
            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-WarfareIon-SmallGrid-Large"));
            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-WarfareIon-SmallGrid-Small"));

            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-AtmoSciFi-LargeGrid-Large"));
            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-AtmoSciFi-LargeGrid-Small"));
            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-AtmoSciFi-SmallGrid-Large"));
            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-AtmoSciFi-SmallGrid-Small"));

            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-IndustryHydro-LargeGrid-Large"));
            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-IndustryHydro-LargeGrid-Small"));
            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-IndustryHydro-SmallGrid-Large"));
            NpcThrustIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Thrust), "MES-NPC-Thrust-IndustryHydro-SmallGrid-Small"));

            JetpackInhibitorIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Jetpack-Small"));
            JetpackInhibitorIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Jetpack-Large"));
            HandDrillInhibitorIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Drill-Small"));
            HandDrillInhibitorIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Drill-Large"));
            NanobotInhibitorIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Nanobots-Small"));
            NanobotInhibitorIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Nanobots-Large"));
            JumpInhibitorIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-JumpDrive-Small"));
            JumpInhibitorIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-JumpDrive-Large"));
            PlayerInhibitorIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Player-Small"));
            PlayerInhibitorIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Player-Large"));
            EnergyInhibitorIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Energy-Small"));
            EnergyInhibitorIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Energy-Large"));

            if (ArmorModuleReplacement.SmallModules.Count != 6) {

                ArmorModuleReplacement.SmallModules.Clear();
                ArmorModuleReplacement.SmallModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Jetpack-Small"));
                ArmorModuleReplacement.SmallModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Drill-Small"));
                ArmorModuleReplacement.SmallModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Nanobots-Small"));
                ArmorModuleReplacement.SmallModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-JumpDrive-Small"));
                ArmorModuleReplacement.SmallModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Player-Small"));
                ArmorModuleReplacement.SmallModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Energy-Small"));
                ArmorModuleReplacement.SmallModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_DefensiveCombatBlock), "SmallDefensiveCombat"));
                ArmorModuleReplacement.SmallModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_FlightMovementBlock), "SmallFlightMovement"));

            }

            if (ArmorModuleReplacement.LargeModules.Count != 6) {

                ArmorModuleReplacement.LargeModules.Clear();
                ArmorModuleReplacement.LargeModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Jetpack-Large"));
                ArmorModuleReplacement.LargeModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Drill-Large"));
                ArmorModuleReplacement.LargeModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Nanobots-Large"));
                ArmorModuleReplacement.LargeModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-JumpDrive-Large"));
                ArmorModuleReplacement.LargeModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Player-Large"));
                ArmorModuleReplacement.LargeModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Energy-Large"));
                ArmorModuleReplacement.LargeModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_DefensiveCombatBlock), "LargeDefensiveCombat"));
                ArmorModuleReplacement.LargeModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_FlightMovementBlock), "LargeFlightMovement"));

            }

            TurretControllers.Add(new MyDefinitionId(typeof(MyObjectBuilder_TurretControlBlock), "MES-NpcSmallTurretControlBlock"));
            TurretControllers.Add(new MyDefinitionId(typeof(MyObjectBuilder_TurretControlBlock), "MES-NpcLargeTurretControlBlock"));

            MES_SessionCore.UnloadActions += Unload;
        
        }

        public static void RegisterBlockWithLogic(BlockEntity block) {

            IBlockLogic result = null;

            if (LogicBlocks.TryGetValue(block.Entity.EntityId, out result))
                return;

            if (RivalAiControlModules.Contains(block.Block.SlimBlock.BlockDefinition.Id)) {

                var remoteControl = block.Block as IMyRemoteControl;

                if (remoteControl == null)
                    return;

                if (string.IsNullOrEmpty(remoteControl?.CustomData) == true) {

                    BehaviorLogger.Write("Remote Control Null Or Has No Behavior Data In CustomData.", BehaviorDebugEnum.BehaviorSetup);
                    return;

                }

                if (!remoteControl.CustomData.Contains("[RivalAI Behavior]") && !remoteControl.CustomData.Contains("[Rival AI Behavior]") && !remoteControl.CustomData.Contains("[MES AI Behavior]") && remoteControl.CustomData.Contains("[RivalAI Behaviour]") == false && remoteControl.CustomData.Contains("[Rival AI Behaviour]") == false) {

                    BehaviorLogger.Write("Remote Control CustomData Does Not Contain Initializer.", BehaviorDebugEnum.BehaviorSetup);
                    return;

                }

                TaskProcessor.Tasks.Add(new BehaviorRegister(remoteControl));
                /*
                MyAPIGateway.Parallel.Start(() => {

                    BehaviorManager.RegisterBehaviorFromRemoteControl(remoteControl);

                });
                */
                return;

            }

            if (JetpackInhibitorIds.Contains(block.Block.SlimBlock.BlockDefinition.Id)) {

                LogicBlocks.Add(block.Block.EntityId, new JetpackInhibitor(block));
                return;

            }

            if (HandDrillInhibitorIds.Contains(block.Block.SlimBlock.BlockDefinition.Id)) {

                LogicBlocks.Add(block.Block.EntityId, new DrillInhibitor(block));
                return;

            }

            if (JumpInhibitorIds.Contains(block.Block.SlimBlock.BlockDefinition.Id)) {

                LogicBlocks.Add(block.Block.EntityId, new JumpDriveInhibitor(block));
                return;

            }

            if (PlayerInhibitorIds.Contains(block.Block.SlimBlock.BlockDefinition.Id)) {

                LogicBlocks.Add(block.Block.EntityId, new PlayerInhibitor(block));
                return;

            }

            if (EnergyInhibitorIds.Contains(block.Block.SlimBlock.BlockDefinition.Id)) {

                LogicBlocks.Add(block.Block.EntityId, new EnergyInhibitor(block));
                return;

            }

            if (NpcThrustIds.Contains(block.Block.SlimBlock.BlockDefinition.Id)) {

                LogicBlocks.Add(block.Block.EntityId, new NpcThrusterLogic(block));
                return;

            }

            if (DisposableBeaconIds.Contains(block.Block.SlimBlock.BlockDefinition.Id)) {

                LogicBlocks.Add(block.Block.EntityId, new DisposableBeaconLogic(block));
                return;

            }

            if (ProprietaryReactorsIds.Contains(block.Block.SlimBlock.BlockDefinition.Id)) {

                LogicBlocks.Add(block.Block.EntityId, new ReactorPrimingLogic(block));
                return;

            }

            if (block.Block as IMyGasTank != null) {

                LogicBlocks.Add(block.Block.EntityId, new InfiniteTank(block));
                return;

            }

            if (block.Block as IMyGyro != null) {

                LogicBlocks.Add(block.Block.EntityId, new NpcGyro(block));
                return;

            }

            if (block.Block as IMyTurretControlBlock != null && TurretControllers.Contains(block.Block.SlimBlock.BlockDefinition.Id)) {

                LogicBlocks.Add(block.Block.EntityId, new NpcTurretController(block));
                return;

            }

            if (block.Block.SlimBlock.BlockDefinition.Id == ControlManager.ShipyardBlockId) {

                LogicBlocks.Add(block.Block.EntityId, new PublicUsableBlock(block));
                return;

            }

            if (block.Block.SlimBlock.BlockDefinition.Id == ControlManager.SuitUpgradeBlockId) {

                LogicBlocks.Add(block.Block.EntityId, new PublicUsableBlock(block));
                return;

            }

        }

        public static void Unload() {

            LogicBlocks.Clear();


        }

    }

}
