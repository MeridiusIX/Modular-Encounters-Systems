using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Missions {

    public enum MissionType
    {
        Custom,
        Acquisition
    }

    public class MissionProfile
    {
        public string ProfileSubtypeId;
        public List<string> Tags;
        public string Title;
        public string Description;

        public string Reward;
        public string Collateral;
        public string ReputationReward;
        public string FailReputationPrice;
        public string Duration;


        public List<string> ReplaceKeys;
        public List<string> ReplaceValues;

        public MissionType MissionType;

        public string StoreProfileId;

        public bool SoloMission;

        //Conditions
        public List<string> PersistantEventConditionIds;
        public List<string> EventConditionIds;
        public bool UseAnyPassingEventCondition;

        public List<string> LeadPlayerConditionIds;
        public List<string> PlayerConditionIds;

        public string OverrideFaction;

        public string InstanceEventGroupId;
        public bool Exclusive;

        public List<string> CustomApiMapping;

        public Dictionary<string, Action<string, object>> EditorReference;

        public MissionProfile()
        {
            ProfileSubtypeId = "";
            Tags = new List<string>();
            Title = "";
            Description = "";
            Reward = "";
            Collateral = "";
            ReputationReward = "";
            FailReputationPrice = "";
            Duration = "-1";
            OverrideFaction = "";

            MissionType = MissionType.Custom;
            StoreProfileId = "";
            ReplaceKeys = new List<string>();
            ReplaceValues = new List<string>();

            SoloMission = false;

            PersistantEventConditionIds = new List<string>();
            UseAnyPassingEventCondition = false;
            EventConditionIds = new List<string>();

            LeadPlayerConditionIds = new List<string>();

            PlayerConditionIds = new List<string>();


            InstanceEventGroupId = "";

            Exclusive = false;
            CustomApiMapping = new List<string>();

            EditorReference = new Dictionary<string, Action<string, object>> {
                {"Tags", (s, o) => TagParse.TagStringListCheck(s, ref Tags) },
                {"Title", (s, o) => TagParse.TagStringCheck(s, ref Title) },
                {"Description", (s, o) => TagParse.TagStringCheck(s, ref Description) },

                {"Reward", (s, o) => TagParse.TagStringCheck(s, ref Reward) },
                {"Collateral", (s, o) => TagParse.TagStringCheck(s, ref Collateral) },
                {"ReputationReward", (s, o) => TagParse.TagStringCheck(s, ref ReputationReward) },
                {"FailReputationPrice", (s, o) => TagParse.TagStringCheck(s, ref FailReputationPrice) },
                {"Duration", (s, o) => TagParse.TagStringCheck(s, ref Duration) },

                {"ReplaceKeys", (s, o) => TagParse.TagStringListCheck(s, ref ReplaceKeys) },
                {"ReplaceValues", (s, o) => TagParse.TagStringListCheck(s, ref ReplaceValues) },
         
                {"PersistantEventConditionIds", (s, o) => TagParse.TagStringListCheck(s, ref PersistantEventConditionIds) },
                {"UseAnyPassingEventCondition", (s, o) => TagParse.TagBoolCheck(s, ref UseAnyPassingEventCondition) },
                {"EventConditionIds", (s, o) => TagParse.TagStringListCheck(s, ref EventConditionIds) },

                {"OverrideFaction", (s, o) => TagParse.TagStringCheck(s, ref OverrideFaction) },

                {"StoreProfileId", (s, o) => TagParse.TagStringCheck(s, ref StoreProfileId) },
 
                {"MissionType", (s, o) => TagParse.TagMissionTypeCheck(s, ref MissionType) },

                {"LeadPlayerConditionIds", (s, o) => TagParse.TagStringListCheck(s, ref LeadPlayerConditionIds) },

                {"PlayerConditionIds", (s, o) => TagParse.TagStringListCheck(s, ref PlayerConditionIds) },

                {"SoloMission", (s, o) => TagParse.TagBoolCheck(s, ref SoloMission) },
                
                {"InstanceEventGroupId", (s, o) => TagParse.TagStringCheck(s, ref InstanceEventGroupId) },
                {"Exclusive", (s, o) => TagParse.TagBoolCheck(s, ref Exclusive) },
                {"CustomApiMapping", (s, o) => TagParse.TagStringListCheck(s, ref CustomApiMapping) },
            };

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

    }
}

