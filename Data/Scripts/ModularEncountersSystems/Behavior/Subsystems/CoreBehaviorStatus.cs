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

    [ProtoContract]
    public class CoreBehaviorStatus {

        //Mode
        public BehaviorMode CurrentAiMode;
        public BehaviorMode PreviousAiMode;

        //Chat
        public bool GreetingChatSent;
        public bool RetreatChatSent;
        public string LastChatSent;

        //Origin
        public Vector3D OriginCoords;

        //Target
        public long TargetEntityId;

        //AutoPilot
        public string AutoPilotMode;
        public Vector3D Waypoint;

        public CoreBehaviorStatus() {

            CurrentAiMode = BehaviorMode.Init;
            PreviousAiMode = BehaviorMode.Init;

        }

        public CoreBehaviorStatus(IMyRemoteControl remoteControl) {



        }

    }

}
