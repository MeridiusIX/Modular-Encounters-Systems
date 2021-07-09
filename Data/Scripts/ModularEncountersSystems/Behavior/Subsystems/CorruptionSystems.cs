using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Weapons;
using SpaceEngineers.Game.ModAPI;
using ProtoBuf;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Utils;
using VRageMath;
using ModularEncountersSystems;
using ModularEncountersSystems.Behavior;
using ModularEncountersSystems.Behavior.Subsystems;
using ModularEncountersSystems.Helpers;

namespace ModularEncountersSystems.Behavior.Subsystems {

    public enum CorruptionHackingEnumA {

        LightShow,
        GlitchyLcds,
        ProductionStop,
        AutomationFreeze,
        ItemDisassembly

    }

    public enum CorruptionHackingEnumB {

        ShieldDrop,
        GyroSpin,
        ThrustOverride,
        WeaponDisarm,
        TurretConfusion

    }

    public enum CorruptionHackingEnumC {

        HudSpam,
        TerminalCorruption,
        JoyRide,
        BlockSubjugation,
        ClangAttacks,
        ConnectorVomit

    }

    public enum CorruptionHackingResult {

        UnknownFail,
        TargetNotFound,
        MyAntennaNotFound,
        MyAntennaNotWorking,
        TargetOutOfMyAntennaRange,
        TargetNotBroadcasting,
        TargetBroadcastNotInRange,
        DecoyInterference,
        Success

    }

    public class CorruptionSystems {

        public static CorruptionHackingResult AttemptHacking(IMyCubeGrid targetGrid, string hackingType, IMyRadioAntenna myAntenna, int interferenceLimit = 3) {

            if(targetGrid == null || MyAPIGateway.Entities.Exist(targetGrid) == false) {

                return CorruptionHackingResult.TargetNotFound;

            }

            if(myAntenna == null || MyAPIGateway.Entities.Exist(myAntenna?.SlimBlock?.CubeGrid) == false) {

                return CorruptionHackingResult.MyAntennaNotFound;

            }

            if(myAntenna.IsFunctional == false || myAntenna.IsWorking == false || myAntenna.EnableBroadcasting == false) {

                return CorruptionHackingResult.MyAntennaNotWorking;

            }



            var blockList = TargetHelper.GetAllBlocks(targetGrid);
            var decoyList = new List<IMySlimBlock>();

            return CorruptionHackingResult.UnknownFail;

        }

    }

}