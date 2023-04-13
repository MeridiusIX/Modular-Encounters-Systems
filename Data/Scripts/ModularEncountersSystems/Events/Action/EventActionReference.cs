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


namespace ModularEncountersSystems.Events.Action {

	public class EventActionReferenceProfile {

		public string ProfileSubtypeId;
		public bool ChangeBooleans;
		public List<string> SetSandboxBooleansTrue;
		public List<string> SetSandboxBooleansFalse;

		public bool ChangeCounters;
		public List<string> IncreaseSandboxCounters;
		public List<string> DecreaseSandboxCounters;
		public List<int> IncreaseSandboxCountersAmount;
		public List<int> DecreaseSandboxCountersAmount;

		public bool SetCounters;
		public List<string> SetSandboxCounters;
		public List<int> SetSandboxCountersAmount;



		public bool UseChatBroadcast;
		public List<ChatProfile> ChatData;

		public bool SetEventControllers;
		public List<string> EventControllerNames;
		public List<bool> EventControllersActive;
		public List<bool> EventControllersSetCurrentTime;

		public string DebugHudMessage;

		public Dictionary<string, Action<string, object>> EditorReference;

		public EventActionReferenceProfile() {

			ProfileSubtypeId = "";
			ChangeBooleans = false;
			SetSandboxBooleansTrue = new List<string>();
			SetSandboxBooleansFalse = new List<string>();

			ChangeCounters = false;
			IncreaseSandboxCounters = new List<string>();
			DecreaseSandboxCounters = new List<string>();
			IncreaseSandboxCountersAmount = new List<int>();
			DecreaseSandboxCountersAmount = new List<int>();


			SetCounters = false;
			SetSandboxCounters = new List<string>();
			SetSandboxCountersAmount = new List<int>();

			UseChatBroadcast = false;
			ChatData = new List<ChatProfile>();

			DebugHudMessage = "";

			SetEventControllers = false;
			EventControllerNames = new List<string>();
			EventControllersActive = new List<bool>();
			EventControllersSetCurrentTime = new List<bool>();

			EditorReference = new Dictionary<string, Action<string, object>> {

				{"ChangeBooleans", (s, o) => TagParse.TagBoolCheck(s, ref ChangeBooleans) },
				{"SetSandboxBooleansTrue", (s, o) => TagParse.TagStringListCheck(s, ref SetSandboxBooleansTrue) },
				{"SetSandboxBooleansFalse", (s, o) => TagParse.TagStringListCheck(s, ref SetSandboxBooleansFalse) },
				{"ChangeCounters", (s, o) => TagParse.TagBoolCheck(s, ref ChangeCounters) },
				{"IncreaseSandboxCounters", (s, o) => TagParse.TagStringListCheck(s, ref IncreaseSandboxCounters) },
				{"DecreaseSandboxCounters", (s, o) => TagParse.TagStringListCheck(s, ref DecreaseSandboxCounters) },
				{"IncreaseSandboxCountersAmount", (s, o) => TagParse.TagIntListCheck(s, ref IncreaseSandboxCountersAmount) },
				{"DecreaseSandboxCountersAmount", (s, o) => TagParse.TagIntListCheck(s, ref DecreaseSandboxCountersAmount) },
				{"SetCounters", (s, o) => TagParse.TagBoolCheck(s, ref SetCounters) },
				{"SetSandboxCounters", (s, o) => TagParse.TagStringListCheck(s, ref SetSandboxCounters) },
				{"SetSandboxCountersAmount", (s, o) => TagParse.TagIntListCheck(s, ref SetSandboxCountersAmount) },

				{"UseChatBroadcast", (s, o) => TagParse.TagBoolCheck(s, ref UseChatBroadcast) },
				{"DebugHudMessage", (s, o) => TagParse.TagStringCheck(s, ref DebugHudMessage) },
				{"SetEventControllers", (s, o) => TagParse.TagBoolCheck(s, ref SetEventControllers) },
				{"EventControllerNames", (s, o) => TagParse.TagStringListCheck(s, ref EventControllerNames) },
				{"EventControllersActive", (s, o) => TagParse.TagBoolListCheck(s, ref EventControllersActive) },
				{"EventControllersSetCurrentTime", (s, o) => TagParse.TagBoolListCheck(s, ref EventControllersSetCurrentTime) },
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



