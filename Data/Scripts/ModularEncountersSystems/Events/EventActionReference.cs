using System;
using System.Collections.Generic;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Behavior.Subsystems;
using ModularEncountersSystems.Behavior.Subsystems.Trigger;
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
    public class EventActionReferenceProfile
    {
        public string ProfileSubtypeId;
        public bool ChangeBooleans;
        public List<string> SetSandboxBooleansTrue;
        public List<string> SetSandboxBooleansFalse;

        public bool ChangeCounters;
        public List<string> IncreaseSandboxCounters;
        public List<string> DecreaseSandboxCounters;
        public int IncreaseSandboxCountersAmount;
        public int DecreaseSandboxCountersAmount;

        public bool UseChatBroadcast;
        public List<ChatProfile> ChatData;
        public string DebugHudMessage;
        public Dictionary<string, Action<string, object>> EditorReference;

        public EventActionReferenceProfile()
        {
            ProfileSubtypeId = "";
            ChangeBooleans = false;
			SetSandboxBooleansTrue = new List<string>();
			SetSandboxBooleansFalse = new List<string>();

			ChangeCounters = false;
			IncreaseSandboxCounters = new List<string>();
			DecreaseSandboxCounters = new List<string>();
            IncreaseSandboxCountersAmount = 1;

            UseChatBroadcast = false;
            ChatData = new List<ChatProfile>();


            DebugHudMessage = "";



			EditorReference = new Dictionary<string, Action<string, object>> {

                {"ChangeBooleans", (s, o) => TagParse.TagBoolCheck(s, ref ChangeBooleans) },
                {"SetSandboxBooleansTrue", (s, o) => TagParse.TagStringListCheck(s, ref SetSandboxBooleansTrue) },
                {"SetSandboxBooleansFalse", (s, o) => TagParse.TagStringListCheck(s, ref SetSandboxBooleansFalse) },
                {"ChangeCounters", (s, o) => TagParse.TagBoolCheck(s, ref ChangeCounters) },
                {"IncreaseSandboxCounters", (s, o) => TagParse.TagStringListCheck(s, ref IncreaseSandboxCounters) },
                {"DecreaseSandboxCounters", (s, o) => TagParse.TagStringListCheck(s, ref DecreaseSandboxCounters) },
                {"IncreaseSandboxCountersAmount", (s, o) => TagParse.TagIntCheck(s, ref IncreaseSandboxCountersAmount) },
                {"DecreaseSandboxCountersAmount", (s, o) => TagParse.TagIntCheck(s, ref DecreaseSandboxCountersAmount) },
                {"UseChatBroadcast", (s, o) => TagParse.TagBoolCheck(s, ref UseChatBroadcast) },
                {"DebugHudMessage", (s, o) => TagParse.TagStringCheck(s, ref DebugHudMessage) },
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





    }
}



