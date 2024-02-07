using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Helpers;
using Sandbox.Game;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;
using Sandbox.ModAPI;
using ModularEncountersSystems.Behavior.Subsystems.Trigger;

namespace ModularEncountersSystems.Events
{

    public class EventProfile
    {

        public string ProfileSubtypeId;

        public bool UseEvent;

        public string EventControllerId;
        public bool UniqueEvent;

        public int MinCooldownMs;
        public int MaxCooldownMs;

        public List<string> PersistantConditionIds;

        public List<string> ConditionIds;
        public bool UseAnyPassingCondition;

        public List<string> ActionIds;

        public ActionExecutionEnum ActionExecution;
        public int TimeUntilNextActionMs;

        public bool OnFailResetCooldown;
        public List<string> Tags;

        public Dictionary<string, Action<string, object>> EditorReference;

        public EventProfile()
        {
            ProfileSubtypeId = "";
            UseEvent = true;
            EventControllerId = "";
            UniqueEvent = true;

            ActionExecution = ActionExecutionEnum.All;
            TimeUntilNextActionMs = 5000;

            Tags = new List<string>();

            MinCooldownMs = 0;
            MaxCooldownMs = 1;

            ActionIds = new List<string>();

            OnFailResetCooldown = false;
            PersistantConditionIds = new List<string>();
            ConditionIds = new List<string>();
            UseAnyPassingCondition = false;

            EditorReference = new Dictionary<string, Action<string, object>> {
                {"UseEvent", (s, o) => TagParse.TagBoolCheck(s, ref UseEvent) },
                {"EventControllerId", (s, o) => TagParse.TagStringCheck(s, ref EventControllerId) },
                {"UniqueEvent", (s, o) => TagParse.TagBoolCheck(s, ref UniqueEvent) },
                {"MinCooldownMs", (s, o) => TagParse.TagIntCheck(s, ref MinCooldownMs) },
                {"MaxCooldownMs", (s, o) => TagParse.TagIntCheck(s, ref MaxCooldownMs) },
                {"PersistantConditionIds", (s, o) => TagParse.TagStringListCheck(s, ref PersistantConditionIds) },
                {"ConditionIds", (s, o) => TagParse.TagStringListCheck(s, ref ConditionIds) },
                {"UseAnyPassingCondition", (s, o) => TagParse.TagBoolCheck(s, ref UseAnyPassingCondition) },
                {"OnFailResetCooldown", (s, o) => TagParse.TagBoolCheck(s, ref OnFailResetCooldown) },
                
                {"ActionIds", (s, o) => TagParse.TagStringListCheck(s, ref ActionIds) },
                {"ActionExecution", (s, o) => TagParse.TagActionExecutionCheck(s, ref ActionExecution) },
                {"TimeUntilNextActionMs", (s, o) => TagParse.TagIntCheck(s, ref TimeUntilNextActionMs) },
                {"Tags", (s, o) => TagParse.TagStringListCheck(s, ref Tags) },

            };

        }

        public void InitTags(string customData) {

            if (string.IsNullOrWhiteSpace(customData) == false) {

                var descSplit = customData.Split('\n');

                foreach (var tag in descSplit) {

                    EditValue(tag);

                }

            }

        }

        public void EditValue(string receivedValue) {

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
