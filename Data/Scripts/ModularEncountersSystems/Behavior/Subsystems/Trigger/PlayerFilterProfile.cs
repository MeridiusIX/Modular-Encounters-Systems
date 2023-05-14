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

namespace ModularEncountersSystems.Behavior.Subsystems.Trigger
{
    //Noting to safe? This is techniqually all "references"
    public class PlayerFilter {

        public string ProfileSubtypeId;
        public bool CheckPlayerReputation;
        public List<string> CheckReputationwithFaction;
        public List<int> MinPlayerReputation;
        public List<int> MaxPlayerReputation;

        public bool CheckSandboxList;
        public List<string> IncludedSandboxListId;
        public List<string> ExcludedSandboxListId;

        public Dictionary<string, Action<string, object>> EditorReference;

        public PlayerFilter()
        {
            ProfileSubtypeId = "";
            CheckPlayerReputation = false;
            CheckReputationwithFaction = new List<string>();
            MinPlayerReputation = new List<int>();
            MaxPlayerReputation = new List<int>();

            CheckSandboxList = false;
            IncludedSandboxListId = new List<string>();
            ExcludedSandboxListId = new List<string>();


            EditorReference = new Dictionary<string, Action<string, object>> {
			{"CheckPlayerReputation", (s, o) => TagParse.TagBoolCheck(s, ref CheckPlayerReputation) },
			{"CheckReputationwithFaction", (s, o) => TagParse.TagStringListCheck(s, ref CheckReputationwithFaction) },
			{"MinPlayerReputation", (s, o) => TagParse.TagIntListCheck(s, ref MinPlayerReputation) },
			{"MaxPlayerReputation", (s, o) => TagParse.TagIntListCheck(s, ref MaxPlayerReputation) },
            {"CheckSandboxList", (s, o) => TagParse.TagBoolCheck(s, ref CheckSandboxList) },
            {"IncludedSandboxListId", (s, o) => TagParse.TagStringListCheck(s, ref IncludedSandboxListId) },
            {"ExcludedSandboxListId", (s, o) => TagParse.TagStringListCheck(s, ref ExcludedSandboxListId) },
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



        public static bool ArePlayerFiltersMet(List<string> ProfilesIds,  long PlayerId)
        {

            List<PlayerFilter> Profiles = new List<PlayerFilter>();

            foreach (var Name in ProfilesIds)
            {
                PlayerFilter PlayerFilter = null;

                if (ProfileManager.PlayerFilters.TryGetValue(Name, out PlayerFilter))
                {
                    Profiles.Add(PlayerFilter);
                }

            }

            int usedProfileConditions = 0;
            int satisfieddProfileConditions = 0;

            if (Profiles.Count <=0)
                return false;
            //Holdings check 
            for (int i = 0; i < Profiles.Count; i++)
            {
                usedProfileConditions++;
                if (IsPlayerFiltersMet(Profiles[i], PlayerId))
                    satisfieddProfileConditions++;
            }

            if (usedProfileConditions == satisfieddProfileConditions)
                return true;
            else
                return false;


        }

        public static bool IsPlayerFiltersMet(PlayerFilter profile, long PlayerId)
        {
            int usedConditions = 0;
            int satisfiedConditions = 0;


            //CheckPlayerReputation
            if (profile.CheckPlayerReputation)
            {
                usedConditions++;

                if (profile.CheckReputationwithFaction.Count == profile.MaxPlayerReputation.Count && profile.MaxPlayerReputation.Count == profile.MinPlayerReputation.Count)
                {

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
                                satisfiedConditions++;
                            }
                               
                        }


                    }
                }
                else
                {
                    BehaviorLogger.Write("CheckReputationwithFaction, MaxPlayerReputation, and MinPlayerReputation do not match in count. Condition Failed", BehaviorDebugEnum.Condition);
                }

            }

            if (profile.CheckSandboxList)
            {

                usedConditions++;


                List<long> PlayerIdentitySandboxList;

                int satisfiedIncludedSandbox = 0;
                int satisfiedExcludedSandbox = 0;


                if(profile.IncludedSandboxListId.Count > 0)
                {
                    foreach (var SandboxList in profile.IncludedSandboxListId)
                    {
                        if(MyAPIGateway.Utilities.GetVariable<List<long>>(SandboxList, out PlayerIdentitySandboxList))
                        {
                            if (PlayerIdentitySandboxList.Contains(PlayerId))
                                satisfiedIncludedSandbox++;
                        }

                    }
                }



                if(profile.ExcludedSandboxListId.Count > 0)
                {

                    foreach (var SandboxList in profile.ExcludedSandboxListId)
                    {
                        if(MyAPIGateway.Utilities.GetVariable(SandboxList, out PlayerIdentitySandboxList))
                        {
                            if (!PlayerIdentitySandboxList.Contains(PlayerId))
                                satisfiedExcludedSandbox++;
                        }
                        else
                            satisfiedExcludedSandbox++;
                    }
                }

                if (satisfiedExcludedSandbox == profile.ExcludedSandboxListId.Count && satisfiedIncludedSandbox == profile.IncludedSandboxListId.Count)
                    satisfiedConditions++;

            }

            if (usedConditions == satisfiedConditions)
                return true;
            else
                return false;

        }


    }
}



