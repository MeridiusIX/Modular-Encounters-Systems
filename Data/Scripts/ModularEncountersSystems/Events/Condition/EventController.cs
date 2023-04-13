using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;
using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using ModularEncountersSystems.Logging;
using ProtoBuf;
using System.Text;

namespace ModularEncountersSystems.Events.Condition {

    [ProtoContract]
    public class ObsEventController
    {

        [ProtoMember(1)] public string ProfileSubtypeId;
        [ProtoMember(2)] public bool Active;
        [ProtoMember(3)] public DateTime StartDate;

        public ObsEventController()
        {
            ProfileSubtypeId = "";
            Active = false;
            StartDate = new DateTime(2081, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }
        
        public static ObsEventController CreateController(string name) {

            var controller = new ObsEventController();
            controller.ProfileSubtypeId = name;
            return controller;
        
        }

        public string GetInfo()
        {

            var sb = new StringBuilder();
            sb.Append(" - Profile SubtypeId:           ").Append(ProfileSubtypeId).AppendLine();
            sb.Append(" - Active:                      ").Append(Active).AppendLine();
            sb.Append(" - StartDate:                   ").Append(StartDate).AppendLine();


            return sb.ToString();


        }







    }

}
