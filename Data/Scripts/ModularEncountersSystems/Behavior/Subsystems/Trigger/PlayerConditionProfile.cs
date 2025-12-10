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
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Zones;

namespace ModularEncountersSystems.Behavior.Subsystems.Trigger
{
    //Noting to safe? This is techniqually all "references"
    public class PlayerCondition {

        public string ProfileSubtypeId;

        public bool AllowOverrides;
        public bool AllPlayersMatchThisCondition;

        public bool UseFailCondition;
        public bool UseAnyPassingCondition;

        public bool CheckPlayerReputation;
        public List<string> CheckReputationwithFaction;
        public List<int> MinPlayerReputation;
        public List<int> MaxPlayerReputation;


        public bool CheckLastRespawnShipName;
        public string LastRespawnShipName;

        public bool CheckPlayerTags;
        public List<string> IncludedPlayerTags;
        public bool AllowAnyIncludedPlayerTag;
        public List<string> ExcludedPlayerTags;
        public bool AllowAnyExcludedPlayerTag;



        public bool CheckPlayerCreditBalance;
        public int MinPlayerCreditBalance;
        public int MaxPlayerCreditBalance;


        public bool CheckPlayerNear;
        public Vector3D PlayerNearVector3;
        public int PlayerNearDistanceFromVector3;
        public int PlayerNearMinDistanceFromVector3;


        public bool CheckPlayerInZone;
        public List<string> ZoneName;

        //Todo:
        public bool CheckPlayerCredits;
        public int MinPlayerCredits;
        public int MaxPlayerCredits;

        public Dictionary<string, Action<string, object>> EditorReference;

        public PlayerCondition()
        {
            ProfileSubtypeId = "";
            AllPlayersMatchThisCondition = false;
            AllowOverrides = true;
            UseFailCondition = false;
            UseAnyPassingCondition = false;

            CheckPlayerReputation = false;
            CheckReputationwithFaction = new List<string>();
            MinPlayerReputation = new List<int>();
            MaxPlayerReputation = new List<int>();

            CheckPlayerTags = false;
            IncludedPlayerTags = new List<string>();
            ExcludedPlayerTags = new List<string>();

            CheckPlayerCreditBalance = false;
            MinPlayerCreditBalance = -1;
            MaxPlayerCreditBalance = -1;

            CheckPlayerInZone = false;
            ZoneName = new List<string>();

            CheckLastRespawnShipName = false;
            LastRespawnShipName = "";

            CheckPlayerNear = false;
            PlayerNearDistanceFromVector3 = 1000;
            PlayerNearMinDistanceFromVector3 = 0;
            PlayerNearVector3 = new Vector3D();

            EditorReference = new Dictionary<string, Action<string, object>> {
            {"UseFailCondition", (s, o) => TagParse.TagBoolCheck(s, ref UseFailCondition) },
            {"AllowOverrides", (s, o) => TagParse.TagBoolCheck(s, ref AllowOverrides) },
            {"AllPlayersMatchThisCondition", (s, o) => TagParse.TagBoolCheck(s, ref AllPlayersMatchThisCondition) },

            {"UseAnyPassingCondition", (s, o) => TagParse.TagBoolCheck(s, ref UseAnyPassingCondition) },
            {"CheckPlayerReputation", (s, o) => TagParse.TagBoolCheck(s, ref CheckPlayerReputation) },
            {"CheckReputationwithFaction", (s, o) => TagParse.TagStringListCheck(s, ref CheckReputationwithFaction) },
			{"MinPlayerReputation", (s, o) => TagParse.TagIntListCheck(s, ref MinPlayerReputation) },
			{"MaxPlayerReputation", (s, o) => TagParse.TagIntListCheck(s, ref MaxPlayerReputation) },
            {"CheckPlayerTags", (s, o) => TagParse.TagBoolCheck(s, ref CheckPlayerTags) },
            {"IncludedPlayerTags", (s, o) => TagParse.TagStringListCheck(s, ref IncludedPlayerTags) },
            {"IncludedPlayerTag", (s, o) => TagParse.TagStringListCheck(s, ref IncludedPlayerTags) }, // I fucked up
            {"ExcludedPlayerTags", (s, o) => TagParse.TagStringListCheck(s, ref ExcludedPlayerTags) },
            {"ExcludedPlayerTag", (s, o) => TagParse.TagStringListCheck(s, ref ExcludedPlayerTags) },// I fucked up
            {"AllowAnyExcludedPlayerTag", (s, o) => TagParse.TagBoolCheck(s, ref AllowAnyExcludedPlayerTag) },
            {"AllowAnyIncludedPlayerTag", (s, o) => TagParse.TagBoolCheck(s, ref AllowAnyIncludedPlayerTag) },
            {"CheckPlayerCreditBalance", (s, o) => TagParse.TagBoolCheck(s, ref CheckPlayerCreditBalance) },//CheckPlayerCreditBalance
			{"MinPlayerCreditBalance", (s, o) => TagParse.TagIntCheck(s, ref MinPlayerCreditBalance) },//MinPlayerCreditBalance
			{"MaxPlayerCreditBalance", (s, o) => TagParse.TagIntCheck(s, ref MaxPlayerCreditBalance) },//MaxPlayerCreditBalance
            {"CheckPlayerInZone", (s, o) => TagParse.TagBoolCheck(s, ref CheckPlayerInZone) },
            {"ZoneName", (s, o) => TagParse.TagStringListCheck(s, ref ZoneName) },


            {"CheckLastRespawnShipName", (s, o) => TagParse.TagBoolCheck(s, ref CheckLastRespawnShipName) },
            {"LastRespawnShipName", (s, o) => TagParse.TagStringCheck(s, ref LastRespawnShipName) },
            {"CheckPlayerNear", (s, o) => TagParse.TagBoolCheck(s, ref CheckPlayerNear) },
            {"PlayerNearCoords", (s, o) => TagParse.TagVector3DCheck(s, ref PlayerNearVector3) },
            {"PlayerNearDistanceFromCoords", (s, o) => TagParse.TagIntCheck(s, ref PlayerNearDistanceFromVector3) },
            {"PlayerNearMinDistanceFromCoords", (s, o) => TagParse.TagIntCheck(s, ref PlayerNearMinDistanceFromVector3) },
            };

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

        public void InitTags(string customData)
        {

            if (string.IsNullOrWhiteSpace(customData) == false)
            {

                var descSplit = customData.Split('\n');

                foreach (var tag in descSplit)
                {

                    EditValue(tag);

                }

            }

        }



        public static bool ArePlayerConditionsMet(List<string> ProfilesIds,  long PlayerId, bool UsePositionOverride = false,Vector3D PositionOverride = new Vector3D(), bool AddPlayerConditionPlayerTags = false, List<string> IncludedPlayerTags = null, List<string> AddExcludedPlayerTag = null)
        {
            List<PlayerCondition> Profiles = new List<PlayerCondition>();

            if (ProfilesIds == null)
                return false;

            if (ProfilesIds.Count <= 0)
                return true;


            foreach (var Name in ProfilesIds)
            {
                PlayerCondition PlayerCondition = null;

                if (ProfileManager.PlayerConditions.TryGetValue(Name, out PlayerCondition))
                {
                    Profiles.Add(PlayerCondition);
                }

            }
            if (Profiles.Count <= 0)
                return false;

            int usedProfileConditions = 0;
            int satisfieddProfileConditions = 0;


            //Holdings check 
            for (int i = 0; i < Profiles.Count; i++)
            {
                usedProfileConditions++;
                if (IsPlayerConditionsMet(Profiles[i], PlayerId,UsePositionOverride,PositionOverride, AddPlayerConditionPlayerTags,IncludedPlayerTags,AddExcludedPlayerTag))
                    satisfieddProfileConditions++;
            }

            if (usedProfileConditions == satisfieddProfileConditions)
                return true;
            else
                return false;
        }

        public static bool ArePlayerConditionsMet(List<PlayerCondition> Profiles, long PlayerId, bool UsePositionOverride = false, Vector3D PositionOverride = new Vector3D())
        {
            if (Profiles == null)
                return false;

            if (Profiles.Count <= 0)
                return true;

            int usedProfileConditions = 0;
            int satisfieddProfileConditions = 0;


            //Holdings check 
            for (int i = 0; i < Profiles.Count; i++)
            {
                usedProfileConditions++;
                if (IsPlayerConditionsMet(Profiles[i], PlayerId, UsePositionOverride, PositionOverride))
                    satisfieddProfileConditions++;
            }

            if (usedProfileConditions == satisfieddProfileConditions)
                return true;
            else
                return false;


        }




        public static bool IsPlayerConditionsMet(PlayerCondition profile, long PlayerId, bool UsepositionOverride = false, Vector3D positionoverride = new Vector3D(), bool AddPlayerConditionPlayerTags = false, List<string> IncludedPlayerTags = null, List<string> AddExcludedPlayerTag = null)
        {
            int usedConditions = 0;
            int satisfiedConditions = 0;

            var player = PlayerManager.GetPlayerWithIdentityId(PlayerId);

            if (player == null)
                return false;

            if (profile.AllPlayersMatchThisCondition)
            {
                return true;
            }

            //CheckPlayerReputation
            if (profile.CheckPlayerReputation)
            {
                usedConditions++;

                if (profile.CheckReputationwithFaction.Count == profile.MaxPlayerReputation.Count && profile.MaxPlayerReputation.Count == profile.MinPlayerReputation.Count)
                {
                    int satisfiedFaction = 0;

                    for (int i = 0; i < profile.CheckReputationwithFaction.Count; i++)
                    {
                        long FactionId = 0;

                        var customfaction = MyAPIGateway.Session.Factions.TryGetFactionByTag(profile.CheckReputationwithFaction[i]);
                        if (customfaction != null)
                            FactionId = customfaction.FactionId;

                       
                        if (FactionId != 0)
                        {
                            var rep = MyAPIGateway.Session.Factions.GetReputationBetweenPlayerAndFaction(PlayerId, FactionId);
                            if (rep >= profile.MinPlayerReputation[i] && rep <= profile.MaxPlayerReputation[i])
                            {
                                satisfiedFaction++;
                            }
                               
                        }


                    }

                    if (satisfiedFaction == profile.CheckReputationwithFaction.Count)
                        satisfiedConditions++;

                }
                else
                {
                    BehaviorLogger.Write("CheckReputationwithFaction, MaxPlayerReputation, and MinPlayerReputation do not match in count. Condition Failed", BehaviorDebugEnum.Condition);
                }



            }

            if (profile.CheckPlayerTags)
            {

                usedConditions++;
                bool failedCheck = false;


                var _includedPlayerTag = new List<string>(profile.IncludedPlayerTags);
                var _excludedPlayerTag = new List<string>(profile.ExcludedPlayerTags);


                if (profile.AllowOverrides && AddPlayerConditionPlayerTags && IncludedPlayerTags != null)
                {
                    _includedPlayerTag.AddList(IncludedPlayerTags);
                }

                if (profile.AllowOverrides && AddPlayerConditionPlayerTags && AddExcludedPlayerTag != null)
                {
                    _excludedPlayerTag.AddList(AddExcludedPlayerTag);
                }


                if (_includedPlayerTag.Count > 0)
                {
                    foreach (var tag in _includedPlayerTag)
                    {
                        if (!player.ProgressionData.Tags.Contains(tag))
                        {
                            failedCheck = true;
                        }
                        else if (profile.AllowAnyIncludedPlayerTag)
                        {
                            failedCheck = false;
                            break;
                        }
                            
                    }
                }

                if (_excludedPlayerTag.Count > 0 && !failedCheck)
                {
                    foreach (var tag in _excludedPlayerTag)
                    {
                        if (player.ProgressionData.Tags.Contains(tag))
                        {
                            failedCheck = true;
                        }
                        else if (profile.AllowAnyExcludedPlayerTag)
                        {
                            failedCheck = false;
                            break;
                        }
                    }
                }




                if (!failedCheck)
                    satisfiedConditions++;


            }


            if (profile.CheckPlayerNear)
            {
                usedConditions++;

                var coords = profile.PlayerNearVector3;

                if (profile.AllowOverrides && UsepositionOverride && positionoverride != null)
                {
                    coords = positionoverride;
                }


                if (Vector3D.Distance(coords, player.GetPosition()) < profile.PlayerNearDistanceFromVector3 && Vector3D.Distance(coords, player.GetPosition()) > profile.PlayerNearMinDistanceFromVector3)
                    satisfiedConditions++;

            }

            if (profile.CheckPlayerInZone)
            {
                usedConditions++;

                foreach (var item in profile.ZoneName)
                {
                    if (ZoneManager.InsideZoneWithName(player.GetPosition(), item))
                    {
                        satisfiedConditions++;
                        break;
                    }

                }

            }

            if (profile.CheckLastRespawnShipName)
            {
                usedConditions++;

                if (player.ProgressionData.LastRespawnShipName == profile.LastRespawnShipName)
                    satisfiedConditions++;
            }

            if (profile.CheckPlayerCreditBalance)
            {
                usedConditions++;

                bool fail = false;

                long credits = 0;

                if (player != null)
                {
                    player.Player.TryGetBalanceInfo(out credits);
                }

                if (credits == 0)
                    fail = true;

                if (!fail)
                {

                    if ((profile.MinPlayerCreditBalance == -1 || credits > profile.MinPlayerCreditBalance) && (profile.MaxPlayerCreditBalance == -1 || credits < profile.MaxPlayerCreditBalance))
                    {
                        satisfiedConditions++;
                    }
                }

            }


            if (profile.UseFailCondition)
            {
                if (usedConditions != satisfiedConditions)
                    return true;
                return false;
            }
            return profile.UseAnyPassingCondition ? satisfiedConditions >= 1 : usedConditions == satisfiedConditions;


        }


    }
}



