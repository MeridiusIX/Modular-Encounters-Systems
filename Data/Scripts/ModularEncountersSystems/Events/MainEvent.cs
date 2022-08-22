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


namespace ModularEncountersSystems.Events
{
    //ProtoContract stuff
    public class MainEvent
    {
        public string ProfileSubtypeId;
        public bool Active;
        public List<Event> Events;
        public DateTime StartDate;

        //Protoignore
        public List<string> ExistingEvents;

        public MainEvent()
        {
            ProfileSubtypeId = "";
            Active = false;
            Events = new List<Event>();
            StartDate = new DateTime(2081, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }
        

        public void InitTags(string data = null)
        {

            if (string.IsNullOrWhiteSpace(data))
                return;

            var descSplit = data.Split('\n');

            foreach (var tagRaw in descSplit)
            {

                var tag = tagRaw.Trim();

                //Active
                if (tag.StartsWith("[Active:") == true)
                {

                    TagParse.TagBoolCheck(tag, ref this.Active);


                }

                //SpawnConditionProfiles
                if (tag.Contains("[Events:"))
                {

                    TagParse.TagEventProfileCheck(tag, ref this.Events);
                    continue;

                }
            }


        }
    }
}
