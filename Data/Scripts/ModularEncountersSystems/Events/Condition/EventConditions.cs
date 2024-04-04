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
using ModularEncountersSystems.Spawning;
using VRageMath;
using ModularEncountersSystems.Entities;

namespace ModularEncountersSystems.Events.Condition
{
    public enum ThreatScoreTypeEnum
    {
        Player = 0,
        PlayerLocation = 1,
        Location = 2
    }

    //Noting to safe? This is techniqually all "references"
    public class EventCondition {

        public string ProfileSubtypeId;
        public bool UseFailCondition;
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
        public Vector3D PlayerNearVector3;
        public int PlayerNearDistanceFromVector3;
        public int PlayerNearMinDistanceFromVector3;

        public bool CheckPlayerCondition;
        public List<string> PlayerConditionIds;

        public bool CheckThreatScore;
        public int ThreatScoreAmount;
        public int ThreatScoreDistance;
        public Vector3D ThreatScoreVector3;
        public int ThreatScoreDistanceFromVector3;
        public ThreatScoreTypeEnum ThreatScoreType;
        public GridConfigurationEnum ThreatScoreGridConfiguration;

        //Not in use
        public bool CheckMainEventDaysPassed;
        public int DaysPassed;

        public bool UseAnyPassingCondition;

        public Dictionary<string, Action<string, object>> EditorReference;

        public EventCondition()
        {
            ProfileSubtypeId = "";
            UseFailCondition = false;
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

            CheckPlayerNear = false;
            PlayerNearDistanceFromVector3 = 1000;
            PlayerNearMinDistanceFromVector3 = 0;
            PlayerNearVector3 = new Vector3D();


            CheckPlayerCondition = false;
            PlayerConditionIds = new List<string>();

            CheckThreatScore = false;
            ThreatScoreAmount = 1000;
            ThreatScoreDistance = 5000;
            ThreatScoreVector3 = new Vector3D(0,0,0);
            ThreatScoreDistanceFromVector3 = 50000;
            ThreatScoreType = ThreatScoreTypeEnum.Player;
            ThreatScoreGridConfiguration = GridConfigurationEnum.All;


            EditorReference = new Dictionary<string, Action<string, object>>
            {
                {"CheckTrueBooleans", (s, o) => TagParse.TagBoolCheck(s, ref CheckTrueBooleans) },
                {"UseFailCondition", (s, o) => TagParse.TagBoolCheck(s, ref UseFailCondition) },
                {"TrueBooleans", (s, o) => TagParse.TagStringListCheck(s, ref TrueBooleans) },
                {"AllowAnyTrueBoolean", (s, o) => TagParse.TagBoolCheck(s, ref AllowAnyTrueBoolean) },
                {"CheckFalseBooleans", (s, o) => TagParse.TagBoolCheck(s, ref CheckFalseBooleans) },
                {"FalseBooleans", (s, o) => TagParse.TagStringListCheck(s, ref FalseBooleans) },
                {"CheckMainEventDaysPassed", (s, o) => TagParse.TagBoolCheck(s, ref CheckMainEventDaysPassed) },
                {"AllowAnyFalseBoolean", (s, o) => TagParse.TagBoolCheck(s, ref AllowAnyFalseBoolean) },
                {"DaysPassed", (s, o) => TagParse.TagIntCheck(s, ref DaysPassed) },
                {"UseAnyPassingCondition", (s, o) => TagParse.TagBoolCheck(s, ref UseAnyPassingCondition) },
                {"CheckCustomCounters", (s, o) => TagParse.TagBoolCheck(s, ref CheckCustomCounters) },
                {"CustomCounters", (s, o) => TagParse.TagStringListCheck(s, ref CustomCounters) },
                {"CustomCountersTargets", (s, o) => TagParse.TagIntListCheck(s, ref CustomCountersTargets) },
                {"CounterCompareTypes", (s, o) => TagParse.TagCounterCompareEnumCheck(s, ref CounterCompareTypes) },
                {"CheckPlayerNear", (s, o) => TagParse.TagBoolCheck(s, ref CheckPlayerNear) },
                {"PlayerNearCoords", (s, o) => TagParse.TagVector3DCheck(s, ref PlayerNearVector3) },
                {"PlayerNearDistanceFromCoords", (s, o) => TagParse.TagIntCheck(s, ref PlayerNearDistanceFromVector3) },
                {"PlayerNearMinDistanceFromCoords", (s, o) => TagParse.TagIntCheck(s, ref PlayerNearMinDistanceFromVector3) },
                {"CheckPlayerCondition", (s, o) => TagParse.TagBoolCheck(s, ref CheckPlayerCondition) },
                {"PlayerConditionIds", (s, o) => TagParse.TagStringListCheck(s, ref PlayerConditionIds) },
                {"CheckThreatScore", (s, o) => TagParse.TagBoolCheck(s, ref CheckThreatScore) },
                {"ThreatScoreAmount", (s, o) => TagParse.TagIntCheck(s, ref ThreatScoreAmount) },
                {"ThreatScoreDistance", (s, o) => TagParse.TagIntCheck(s, ref ThreatScoreDistance) },
                {"ThreatScoreCoords", (s, o) => TagParse.TagVector3DCheck(s, ref ThreatScoreVector3) },
                {"ThreatScoreDistanceFromCoords", (s, o) => TagParse.TagIntCheck(s, ref ThreatScoreDistanceFromVector3) },
                {"ThreatScoreType", (s, o) => TagParse.TagThreatScoreTypeEnumCheck(s, ref ThreatScoreType) },
                {"ThreatScoreGridConfiguration", (s, o) => TagParse.TagGridConfigurationCheck(s, ref ThreatScoreGridConfiguration) },

                
            };

        }

        public void InitTags(string data)
        {
            if (string.IsNullOrWhiteSpace(data) == false)
            {

                var descSplit = data.Split('\n');

                foreach (var tag in descSplit)
                {

                    EditValue(tag);

                }

            }
        }

        public void EditValue(string receivedValue)
        {

            var processedTag = TagParse.ProcessTag(receivedValue);

            if (processedTag.Length < 2)
                return;

            Action<string, object> referenceMethod = null;

            if (!EditorReference.TryGetValue(processedTag[0], out referenceMethod))
                //TODO: Notes About Value Not Found
                return;

            referenceMethod?.Invoke(receivedValue, null);

        }




        public static bool AreConditionsMet(bool anyPassingConditionEventProfile, List<EventCondition> profiles, out int index)
        {
            int usedProfileConditions = 0;
            int satisfieddProfileConditions = 0;
            index = -1;
            //Holdings check 
            for (int i = 0; i < profiles.Count; i++)
            {
                usedProfileConditions++;
                if (IsConditionMet(profiles[i]))
                {
                    satisfieddProfileConditions++;
                    index = i;
                }

            }

            return anyPassingConditionEventProfile ? satisfieddProfileConditions >= 1 : usedProfileConditions == satisfieddProfileConditions;

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

                ListOfPlayersinRange = TargetHelper.GetPlayersWithinRange(Profile.PlayerNearVector3, Profile.PlayerNearMinDistanceFromVector3,Profile.PlayerNearDistanceFromVector3);
                //int amountofPlayers = ListOfPlayersinRange.Count;



                if (ListOfPlayersinRange.Count >0)
                {
                    satisfiedConditions++;
                }

                /*

                foreach (IMyPlayer Player in ListOfPlayersinRange)
                {
                    ListOfPlayerIds.Add(Player.IdentityId);
                }

                foreach (var id in ListOfPlayerIds)
                {
                    if(Profile.PlayerConditionIds.Count < 1)
                    {
                        satisfiedConditions++;
                    }
                    else
                    {
                        if (PlayerCondition.ArePlayerConditionsMet(Profile.PlayerConditionIds, id))
                            satisfiedConditions++;
                    }

                }
                */
            }

            if (Profile.CheckPlayerCondition)
            {
                usedConditions++;


                var playerList = new List<IMyPlayer>();
                MyAPIGateway.Players.GetPlayers(playerList);

                foreach (var player in PlayerManager.Players)
                {
                    if (PlayerCondition.ArePlayerConditionsMet(Profile.PlayerConditionIds, player.Player.IdentityId))
                    {
                        satisfiedConditions++;
                        break;
                    }


                }

            }




            if (Profile.CheckThreatScore)
            {
                usedConditions++;

                var comparetype = Profile.ThreatScoreType;
                if(comparetype == ThreatScoreTypeEnum.Location)
                {
                    if (SpawnConditions.GetThreatLevel(Profile.ThreatScoreDistance, false, Profile.ThreatScoreVector3, Profile.ThreatScoreGridConfiguration) >= Profile.ThreatScoreAmount)
                    {
                        satisfiedConditions++;
                    }
                }
                else
                {
                    var playerList = new List<IMyPlayer>();
                    if (comparetype == ThreatScoreTypeEnum.Player)
                    {
                        MyAPIGateway.Players.GetPlayers(playerList);
                    }
                    if (comparetype == ThreatScoreTypeEnum.PlayerLocation)
                    {
                        playerList = TargetHelper.GetPlayersWithinDistance(Profile.ThreatScoreVector3, Profile.ThreatScoreDistanceFromVector3);
                    }

                    foreach (IMyPlayer Player in playerList)
                    {
                        if (SpawnConditions.GetThreatLevel(Profile.ThreatScoreDistance, false, Player.GetPosition(), Profile.ThreatScoreGridConfiguration) >= Profile.ThreatScoreAmount)
                        {
                            satisfiedConditions++;
                            break;
                        }
                    }
                }
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

            if (Profile.UseFailCondition)
            {
                if(usedConditions != satisfiedConditions)
                    return true;
                return false;
            }
            return Profile.UseAnyPassingCondition ? satisfiedConditions >= 1 : usedConditions == satisfiedConditions;

        }

    }

}



