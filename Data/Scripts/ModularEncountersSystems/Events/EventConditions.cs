using System;
using System.Collections.Generic;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.API;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;

namespace ModularEncountersSystems.Events
{
    public class EventCondition{
        public string ProfileSubtypeId;
        public bool CheckTrueBooleans;
        public List<string> TrueBooleans;
        public bool AllowAnyTrueBoolean;
        public bool CheckFalseBooleans;
        public List<string> FalseBooleans;
        public bool AllowAnyFalseBoolean;

        public bool CheckMainEventDaysPassed;
        public int DaysPassed ;



        public static DateTime BeginTime;
        public static DateTime GameDateTime;
        public EventCondition()
        {
            ProfileSubtypeId = "";
            CheckTrueBooleans = false;
            TrueBooleans = new List<string>();
            AllowAnyTrueBoolean = false;
            CheckFalseBooleans = false;
            FalseBooleans = new List<string>();
            AllowAnyFalseBoolean = false;
            CheckMainEventDaysPassed = false;
            DaysPassed = 1;
           
        }

        public void InitTags(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return;

            var descSplit = data.Split('\n');

            foreach (var tagRaw in descSplit)
            {

                var tag = tagRaw.Trim();

                //CheckTrueBooleans
                if (tag.StartsWith("[CheckTrueBooleans:") == true)
                {

                    TagParse.TagBoolCheck(tag, ref this.CheckTrueBooleans);
                }
                //TrueBooleans
                if (tag.StartsWith("[TrueBooleans:") == true)
                {

                    TagParse.TagStringListCheck(tag, ref this.TrueBooleans);
                }

                //AllowAnyTrueBoolean
                if (tag.StartsWith("[AllowAnyTrueBoolean:") == true)
                {

                    TagParse.TagBoolCheck(tag, ref this.AllowAnyTrueBoolean);
                }

                //CheckFalseBooleans
                if (tag.StartsWith("[CheckFalseBooleans:") == true)
                {

                    TagParse.TagBoolCheck(tag, ref this.CheckFalseBooleans);
                }
                //FalseBooleans
                if (tag.StartsWith("[FalseBooleans:") == true)
                {

                    TagParse.TagStringListCheck(tag, ref this.FalseBooleans);
                }
                //CheckMainEventDaysPassed
                if (tag.StartsWith("[CheckMainEventDaysPassed:") == true)
                {

                    TagParse.TagBoolCheck(tag, ref this.CheckMainEventDaysPassed);
                }

                //AllowAnyFalseBoolean
                if (tag.StartsWith("[AllowAnyFalseBoolean:") == true)
                {

                    TagParse.TagBoolCheck(tag, ref this.AllowAnyFalseBoolean);
                }
                //DaysPassed
                if (tag.StartsWith("[DaysPassed:") == true)
                {

                    TagParse.TagIntCheck(tag, ref this.DaysPassed);
                }


                
            }
        }
        public static bool AreConditionsMet(MainEvent MainEvent , List<EventCondition> Profiles)
        {
            int usedProfileConditions = 0;
            int satisfieddProfileConditions = 0;
            //Holdings check 
            for (int i = 0; i < Profiles.Count; i++)
            {
                usedProfileConditions++;
                if (IsConditionMet(MainEvent,Profiles[i]))
                    satisfieddProfileConditions++;
            }

            if (usedProfileConditions == satisfieddProfileConditions)
                return true;
            else
                return false;


        }

            public static bool IsConditionMet(MainEvent MainEvent, EventCondition Profile)
        {
            int usedConditions = 0;
            int satisfiedConditions = 0;

            
            //Bool
            if(Profile.CheckTrueBooleans == true)
            {
                usedConditions++;
                bool failedCheck = false;
                bool placeholder;
                foreach (var boolName in Profile.TrueBooleans)
                {

                    if (!MyAPIGateway.Utilities.GetVariable<bool>(boolName, out placeholder))
                    {

                        failedCheck = true;

                        if (!Profile.AllowAnyTrueBoolean)
                        {

                            failedCheck = true;
                            break;

                        }

                    }
                    else if (Profile.AllowAnyTrueBoolean)
                    {

                        failedCheck = false;
                        break;

                    }

                }

                if (!failedCheck)
                    satisfiedConditions++;

            }

            //Bool False
            if (Profile.CheckFalseBooleans == true)
            {
                usedConditions++;
                bool failedCheck = false;
                bool placeholder;
                foreach (var boolName in Profile.FalseBooleans)
                {

                    if (MyAPIGateway.Utilities.GetVariable<bool>(boolName, out placeholder))
                    {

                        failedCheck = true;

                        if (!Profile.AllowAnyFalseBoolean)
                        {

                            failedCheck = true;
                            break;

                        }

                    }
                    else if (Profile.AllowAnyFalseBoolean)
                    {

                        failedCheck = false;
                        break;

                    }

                }

                if (!failedCheck)
                    satisfiedConditions++;

            }

            //Date thingy

            if (Profile.CheckMainEventDaysPassed)
            {
                usedConditions++;

                BeginTime = MainEvent.StartDate;

                GameDateTime = MyAPIGateway.Session.GameDateTime;

                float SunRotationIntervalSeconds = MyAPIGateway.Session.SessionSettings.SunRotationIntervalMinutes * 60;

                float DaysElipsedRaw = (float)((GameDateTime - BeginTime).TotalSeconds / SunRotationIntervalSeconds);


                if (DaysElipsedRaw > Profile.DaysPassed)
                    satisfiedConditions++;

            }
            
            if (usedConditions == satisfiedConditions)
                return true;
            else
                return false;

        }


    }
}



