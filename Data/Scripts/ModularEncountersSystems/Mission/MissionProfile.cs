using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Missions {


    public class MissionProfile
    {

        public string ProfileSubtypeId;
        public string Title;
        public string Description;

        public List<string> EventConditionIds;

        public bool OverrideFaction;
        public string Faction;

        public List<string> ReplaceKeys;
        public List<string> ReplaceValues;

        public string InstanceEventGroupId;

        public bool Unique;
        public bool Exclusive;


        public Dictionary<string, Action<string, object>> EditorReference;

        public MissionProfile()
        {
            ProfileSubtypeId = "";
            Title = "";
            Description = "";
            EventConditionIds = new List<string>();
            OverrideFaction = false;
            Faction = "";


            ReplaceKeys = new List<string>();
            ReplaceValues = new List<string>();

            InstanceEventGroupId = "";

            Unique = false;
            Exclusive = false;


        EditorReference = new Dictionary<string, Action<string, object>> {
                {"Title", (s, o) => TagParse.TagStringCheck(s, ref Title) },
                {"Description", (s, o) => TagParse.TagStringCheck(s, ref Description) },
                {"OverrideFaction", (s, o) => TagParse.TagBoolCheck(s, ref OverrideFaction) },
                {"Faction", (s, o) => TagParse.TagStringCheck(s, ref Faction) },
                {"EventConditionIds", (s, o) => TagParse.TagStringListCheck(s, ref EventConditionIds) },
                {"ReplaceKeys", (s, o) => TagParse.TagStringListCheck(s, ref ReplaceKeys) },
                {"ReplaceValues", (s, o) => TagParse.TagStringListCheck(s, ref ReplaceValues) },
                {"InstanceEventGroupId", (s, o) => TagParse.TagStringCheck(s, ref InstanceEventGroupId) },
                {"Exclusive", (s, o) => TagParse.TagBoolCheck(s, ref Exclusive) },
                {"Unique", (s, o) => TagParse.TagBoolCheck(s, ref Unique) },
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

