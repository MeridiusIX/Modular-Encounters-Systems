using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Helpers;
using Sandbox.Game;
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


    public class Event
    {
        //SAVE
        public string ProfileSubtypeId;
        public bool Ready;
        public int Happend; 


        //REF
        public bool UniqueEvent;


        //?
        public List<EventCondition> Conditions;


        //REF
        public EventActionExecutionEnum ActionExecution;
        public int TimeTillNextAction;

        //Store
        public int AtAction;

        //?
        public List<EventActionProfile> Actions;

        //REF
        public float MinCooldownMs;
        public float MaxCooldownMs;


        //Store
        public int CooldownTime;

        //Store
        public DateTime LastTriggerTime;

        //ProtoIgnore
        public Random Rnd;

        public Event()
        {
            ProfileSubtypeId = "";
            Ready = false;
            Happend = 0;
            UniqueEvent = true;
            Conditions = new List<EventCondition>();

            ActionExecution = EventActionExecutionEnum.AtOnce;
            TimeTillNextAction = 3;
            AtAction = 0;
            Actions = new List<EventActionProfile>();

            MinCooldownMs = 0;
            MaxCooldownMs = 1;
            CooldownTime = 0;
            LastTriggerTime = MyAPIGateway.Session.GameDateTime;
            Rnd = new Random();
        }

        public void TriggerEvent()
        {
            this.LastTriggerTime = MyAPIGateway.Session.GameDateTime;
            this.CooldownTime = Rnd.Next((int)MinCooldownMs, (int)MaxCooldownMs);
            EventActionProfile.ExecuteActions(this);
            							
        }

        public bool ValidateCooldown()
        {
            if (this.CooldownTime > 0)
            {


                var duration = MyAPIGateway.Session.GameDateTime - this.LastTriggerTime;

                if (duration.TotalMilliseconds > this.CooldownTime)
                    return true;
                else
                    return false;
            }



            return true;
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
                if (tag.Contains("[Actions:") == true)
                {

                    string tempValue = "";
                    TagParse.TagStringCheck(tag, ref tempValue);
                    bool gotAction = false;

                    if (string.IsNullOrWhiteSpace(tempValue) == false)
                    {

                        byte[] byteData = { };

                        if (ProfileManager.EventActionObjectTemplates.TryGetValue(tempValue, out byteData) == true)
                        {

                            try
                            {

                                var profile = MyAPIGateway.Utilities.SerializeFromBinary<EventActionProfile>(byteData);

                                if (profile != null)
                                {

                                    this.Actions.Add(profile);
                                    gotAction = true;

                                }

                            }
                            catch (Exception)
                            {

                            }

                        }

                    }

                    if (!gotAction)
                        ProfileManager.ReportProfileError(tempValue, "Could Not Load Action Profile From Trigger: " + ProfileSubtypeId);

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

                //MinCooldown
                if (tag.Contains("[MinCooldownMs:") == true)
                {

                    TagParse.TagFloatCheck(tag, ref MinCooldownMs);

                }

                //MaxCooldown
                if (tag.Contains("[MaxCooldownMs:") == true)
                {

                    TagParse.TagFloatCheck(tag, ref MaxCooldownMs);

                }

            }
        }
    }
}
