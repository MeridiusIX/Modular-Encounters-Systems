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
    //SAVE the whole class?
    public class MainEvent
    {
        //SAVE
        public string ProfileSubtypeId;

        //SAVE
        public bool Active;

        //SAVE
        public List<Event> Events;
        
        //SAVE
        public DateTime StartDate;

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

                
                if (tag.StartsWith("[Active:") == true)
                {

                    TagParse.TagBoolCheck(tag, ref this.Active);

                   
                }

               
                if (tag.Contains("[Events:"))
                {

                    TagParse.TagEventProfileCheck(tag, ref this.Events);
                    continue;

                }
            }


        }
    }
}
