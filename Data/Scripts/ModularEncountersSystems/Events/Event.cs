using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;
using Sandbox.ModAPI;

namespace ModularEncountersSystems.Events
{
    public enum EventActionExecutionEnum
    {
        AtOnce,
        Sequential,
        SingleRandom
    }

    public enum EventType
    {
        Global,
        Location
    }

    public class Event
    {
        public string ProfileSubtypeId;
        public bool Ready;
        public bool Happend;

        public EventType Type;
        public bool UniqueEvent;

        public List<EventCondition> Conditions;

        public EventActionExecutionEnum ActionExecution;
        public int TimeTillNextAction;
        public int AtAction;
        public List<EventAction> Actions;

        //EventType Location
        public Vector3D Coordinates;
        public string GPSLabel;
        public Vector3D GPSColor;

        

        public Event()
        {
            ProfileSubtypeId = "";
            Ready = false;
            Happend = false;
            Type = EventType.Global;
            UniqueEvent = true;
            Conditions = new List<EventCondition>();

            ActionExecution = EventActionExecutionEnum.AtOnce;
            TimeTillNextAction = 3;
            AtAction = 0;
            Actions = new List<EventAction>();

            Coordinates = new Vector3D(0, 0, 0);
            GPSLabel = "Event";
            GPSColor = new Vector3D(255, 0, 255);
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
                if (tag.StartsWith("[UniqueEvent:") == true)
                {

                    TagParse.TagBoolCheck(tag, ref this.UniqueEvent);
                }


                //Conditions
                if (tag.StartsWith("[Conditions:") == true)
                {
                    var tagSplit = TagParse.ProcessTag(tag);

                    if (tagSplit.Length < 2)
                    {
                        return;
                    }

                    var key = tagSplit[1];
                    EventCondition result = null;

                    if (ProfileManager.EventConditions.TryGetValue(key, out result))
                        this.Conditions.Add(result);
                }

                //Actions
                if (tag.StartsWith("[Actions:") == true)
                {

                    var tagSplit = TagParse.ProcessTag(tag);

                    if (tagSplit.Length < 2)
                    {
                        return;
                    }

                    var key = tagSplit[1];
                    EventAction result = null;

                    if (ProfileManager.EventActions.TryGetValue(key, out result))
                        this.Actions.Add(result);
                }

                //ActionExecution
                if (tag.StartsWith("[ActionExecution:") == true)
                {
                    EventActionExecutionEnum result = EventActionExecutionEnum.AtOnce;
                    var tagSplit = TagParse.ProcessTag(tag);

                    if (tagSplit.Length == 2)
                    {
                        if (EventActionExecutionEnum.TryParse(tagSplit[1], out result) == false)
                        {
                            return;
                        }
                    }

                    this.ActionExecution |= result;

                }

                //TimeTillNextAction
                if (tag.StartsWith("[TimeTillNextAction:") == true)
                {

                    TagParse.TagIntCheck(tag, ref this.TimeTillNextAction);
                }



            }
        }
    }
}
