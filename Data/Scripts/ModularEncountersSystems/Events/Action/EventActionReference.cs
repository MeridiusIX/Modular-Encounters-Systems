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
		public List<string> SetBooleansTrue;
		public List<string> SetBooleansFalse;

		public bool ChangeCounters;
		public List<string> IncreaseCounters;
		public List<string> DecreaseCounters;
		public List<int> IncreaseCountersAmount;
		public List<int> DecreaseCountersAmount;

		public List<string> SetCounters;
		public List<int> SetCountersAmount;


		public bool ResetCooldownTimeOfEvents;
		public List<string> ResetEventCooldownNames;
		public List<string> ResetEventCooldownTags;




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
			SetBooleansTrue = new List<string>();
			SetBooleansFalse = new List<string>();

			ChangeCounters = false;
			IncreaseCounters = new List<string>();
			DecreaseCounters = new List<string>();
			IncreaseCountersAmount = new List<int>();
			DecreaseCountersAmount = new List<int>();

			SetCounters = new List<string>();
			SetCountersAmount = new List<int>();

			ResetCooldownTimeOfEvents = false;
			ResetEventCooldownNames = new List<string>();
			ResetEventCooldownTags = new List<string>();


			UseChatBroadcast = false;
			ChatData = new List<ChatProfile>();

			DebugHudMessage = "";

			SetEventControllers = false;
			EventControllerNames = new List<string>();
			EventControllersActive = new List<bool>();
			EventControllersSetCurrentTime = new List<bool>();

			EditorReference = new Dictionary<string, Action<string, object>> {

				{"ChangeBooleans", (s, o) => TagParse.TagBoolCheck(s, ref ChangeBooleans) },
				{"SetBooleansTrue", (s, o) => TagParse.TagStringListCheck(s, ref SetBooleansTrue) },
				{"SetBooleansFalse", (s, o) => TagParse.TagStringListCheck(s, ref SetBooleansFalse) },
				{"ChangeCounters", (s, o) => TagParse.TagBoolCheck(s, ref ChangeCounters) },
				{"IncreaseCounters", (s, o) => TagParse.TagStringListCheck(s, ref IncreaseCounters) },
				{"DecreaseCounters", (s, o) => TagParse.TagStringListCheck(s, ref DecreaseCounters) },
				{"IncreaseCountersAmount", (s, o) => TagParse.TagIntListCheck(s, ref IncreaseCountersAmount) },
				{"DecreaseCountersAmount", (s, o) => TagParse.TagIntListCheck(s, ref DecreaseCountersAmount) },
				{"SetCounters", (s, o) => TagParse.TagStringListCheck(s, ref SetCounters) },
				{"SetCountersAmount", (s, o) => TagParse.TagIntListCheck(s, ref SetCountersAmount) },
				{"ResetCooldownTimeOfEvents", (s, o) => TagParse.TagBoolCheck(s, ref ResetCooldownTimeOfEvents) },
				{"ResetEventCooldownNames", (s, o) => TagParse.TagStringListCheck(s, ref ResetEventCooldownNames) },
				{"ResetEventCooldownTags", (s, o) => TagParse.TagStringListCheck(s, ref ResetEventCooldownTags) },

				
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



