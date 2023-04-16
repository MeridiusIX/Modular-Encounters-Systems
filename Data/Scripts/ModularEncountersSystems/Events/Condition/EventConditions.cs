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
using ModularEncountersSystems.Behavior.Subsystems.Trigger;
using VRageMath;

namespace ModularEncountersSystems.Events.Condition
{
    //Noting to safe? This is techniqually all "references"
    public class EventCondition {

        public string ProfileSubtypeId;
        public bool CheckTrueBooleans;
        public List<string> TrueBooleans;
        public bool AllowAnyTrueBoolean;
        public bool CheckFalseBooleans;
        public List<string> FalseBooleans;
        public bool AllowAnyFalseBoolean;

        public bool CheckCustomCounters;
        public List<string> CustomCounters;
        public List<int> CustomCountersTargets;
        public List<CounterCompareEnum> CounterCompareTypes;

        public bool CheckPlayerNear;
        public bool CheckPlayerFar;
        public int Distance;
        public Vector3D vector3;
        public List<string> PlayerFilterIds;






        public bool CheckMainEventDaysPassed;
        public int DaysPassed;

        public bool UseAnyPassingCondition;

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
            UseAnyPassingCondition = false;

            CheckCustomCounters = false;
            CustomCounters = new List<string>();
            CustomCountersTargets = new List<int>();
            CounterCompareTypes = new List<CounterCompareEnum>();


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

                //UseAnyPassingCondition
                if (tag.StartsWith("[UseAnyPassingCondition:") == true) {

                    TagParse.TagBoolCheck(tag, ref this.UseAnyPassingCondition);

                }
                //CheckCustomCounters
                if (tag.StartsWith("[CheckCustomCounters:") == true)
                {

                    TagParse.TagBoolCheck(tag, ref this.CheckCustomCounters);

                }
                //CustomCounters
                if (tag.StartsWith("[CustomCounters:") == true)
                {

                    TagParse.TagStringListCheck(tag, ref this.CustomCounters);

                }
                //CustomCountersTargets
                if (tag.StartsWith("[CustomCountersTargets:") == true)
                {

                    TagParse.TagIntListCheck(tag, ref this.CustomCountersTargets);

                }
                //CounterCompareTypes
                if (tag.StartsWith("[CounterCompareTypes:") == true)
                {

                    TagParse.TagCounterCompareEnumCheck(tag, ref this.CounterCompareTypes);

                }



            }

        }

        public static bool AreConditionsMet(bool anyPassingConditionEventProfile, List<EventCondition> profiles)
        {
            int usedProfileConditions = 0;
            int satisfieddProfileConditions = 0;
            //Holdings check 
            for (int i = 0; i < profiles.Count; i++)
            {
                usedProfileConditions++;
                if (IsConditionMet(profiles[i]))
                    satisfieddProfileConditions++;
            }

            return anyPassingConditionEventProfile ? satisfieddProfileConditions >= 1 : usedProfileConditions == satisfieddProfileConditions;

        }

        public static bool IsConditionMet(EventCondition Profile)
        {
            int usedConditions = 0;
            int satisfiedConditions = 0;

            //EventControllerActive

            
            //Bool
            if(Profile.CheckTrueBooleans == true)
            {
                usedConditions++;
                bool failedCheck = false;

                for (int i = 0; i < Profile.TrueBooleans.Count; i++)
                {

                    var boolName = Profile.TrueBooleans[i];

                    try
                    {

                        bool output = false;
                        var result = MyAPIGateway.Utilities.GetVariable(boolName, out output);

                        if (!result || !output)
                        {

                            //BehaviorLogger.Write(ProfileSubtypeId + ":  Boolean False: " + boolName, BehaviorDebugEnum.Condition);
                            failedCheck = true;
                            continue;

                        }
                        else if (Profile.AllowAnyTrueBoolean)
                        {
                            failedCheck = false;
                            break;

                        }

                    }
                    catch (Exception e)
                    {

                        //BehaviorLogger.Write("Exception: ", BehaviorDebugEnum.Condition);
                        //BehaviorLogger.Write(e.ToString(), BehaviorDebugEnum.Condition);

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

                for (int i = 0; i < Profile.FalseBooleans.Count; i++)
                {

                    var boolName = Profile.FalseBooleans[i];

                    try
                    {

                        bool output = false;
                        var result = MyAPIGateway.Utilities.GetVariable(boolName, out output);

                        if (output)
                        {

                            //BehaviorLogger.Write(ProfileSubtypeId + ":  Boolean False: " + boolName, BehaviorDebugEnum.Condition);
                            failedCheck = true;
                            continue;

                        }
                        else if (Profile.AllowAnyFalseBoolean)
                        {
                            failedCheck = false;
                            break;

                        }

                    }
                    catch (Exception e)
                    {

                        //BehaviorLogger.Write("Exception: ", BehaviorDebugEnum.Condition);
                        //BehaviorLogger.Write(e.ToString(), BehaviorDebugEnum.Condition);

                    }

                }

                if (!failedCheck)
                    satisfiedConditions++;
            }

            if (Profile.CheckCustomCounters == true)
            {
                usedConditions++;
                bool Satisfied = true;

                if (Profile.CustomCounters.Count == Profile.CustomCountersTargets.Count)
                {

                    for (int i = 0; i < Profile.CustomCounters.Count; i++)
                    {

                        try
                        {

                            int counter = 0;
                            var result = MyAPIGateway.Utilities.GetVariable(Profile.CustomCounters[i], out counter);



                            var compareType = CounterCompareEnum.GreaterOrEqual;

                            if (i <= Profile.CounterCompareTypes.Count - 1)
                                compareType = Profile.CounterCompareTypes[i];

                            bool counterResult = false;

                            if (compareType == CounterCompareEnum.GreaterOrEqual)
                                counterResult = (counter >= Profile.CustomCountersTargets[i]);

                            if (compareType == CounterCompareEnum.Greater)
                                counterResult = (counter > Profile.CustomCountersTargets[i]);

                            if (compareType == CounterCompareEnum.Equal)
                                counterResult = (counter == Profile.CustomCountersTargets[i]);

                            if (compareType == CounterCompareEnum.NotEqual)
                                counterResult = (counter != Profile.CustomCountersTargets[i]);

                            if (compareType == CounterCompareEnum.Less)
                                counterResult = (counter < Profile.CustomCountersTargets[i]);

                            if (compareType == CounterCompareEnum.LessOrEqual)
                                counterResult = (counter <= Profile.CustomCountersTargets[i]);

                            if (!result || !counterResult)
                            {
                                //BehaviorLogger.Write(ProfileSubtypeId + ":  Counter Amount Condition Not Satisfied: " + ConditionReference.CustomCounters[i], BehaviorDebugEnum.Condition);
                                Satisfied =  false;

                            }
                        }
                        catch (Exception e)
                        {
                            Satisfied = false;
                            //BehaviorLogger.Write("Exception: ", BehaviorDebugEnum.Condition);
                            //BehaviorLogger.Write(e.ToString(), BehaviorDebugEnum.Condition);
                        }
                    }

                }
                else
                {
                    //BehaviorLogger.Write(ProfileSubtypeId + ":  Counter Names and Targets List Counts Don't Match. Check Your Condition Profile", BehaviorDebugEnum.Condition);
                    Satisfied = false;
                }



                if (Satisfied)
                    satisfiedConditions++;
            }


            if (Profile.CheckPlayerNear)
            {
                usedConditions++;

                List<IMyPlayer> ListOfPlayersinRange = null;
                List<long> ListOfPlayerIds = new List<long>();

                //int amountofplayersmatch = 0;

                ListOfPlayersinRange = TargetHelper.GetPlayersWithinDistance(Profile.vector3, Profile.Distance);
                //int amountofPlayers = ListOfPlayersinRange.Count;

                foreach (IMyPlayer Player in ListOfPlayersinRange)
                {
                    ListOfPlayerIds.Add(Player.IdentityId);
                }

                foreach (var id in ListOfPlayerIds)
                {
                    if (PlayerFilter.ArePlayerFiltersMet(Profile.PlayerFilterIds, id))
                        satisfiedConditions++;
                }

                //if (amountofPlayers == amountofplayersmatch)
                    

            }


            //Date thingy

            /*
            if (Profile.CheckMainEventDaysPassed)
            {
                usedConditions++;

                if (eventController == null)
                    return false;

                var BeginTime = eventController.StartDate;

                var GameDateTime = Time.GetRealIngameTime();

                float SunRotationIntervalSeconds = MyAPIGateway.Session.SessionSettings.SunRotationIntervalMinutes * 60;

                float DaysElipsedRaw = (float)((GameDateTime - BeginTime).TotalSeconds / SunRotationIntervalSeconds);


                if (DaysElipsedRaw > Profile.DaysPassed)
                    satisfiedConditions++;

            }
            */

            //bool result = (Planet != null) ? Planet.IsInGravity() : false;
            //bool result = Planet.InGravity();

            //Vector3D coords = player?.Character?.PositionComp?.WorldAABB.Center ?? Vector3D.Zero;

            return Profile.UseAnyPassingCondition ? satisfiedConditions >= 1 : usedConditions == satisfiedConditions;

        }

    }

}



