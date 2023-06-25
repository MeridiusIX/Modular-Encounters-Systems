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
		public List<string> ResetEventCooldownIds;
		public List<string> ResetEventCooldownTags;

		public bool ToggleEvents;
		public List<string> ToggleEventIds;
		public List<bool> ToggleEventIdModes;
		public List<string> ToggleEventTags;
		public List<bool> ToggleEventTagModes;



		public bool ChangeZoneAtPosition;
		public List<string> ZoneNames;
		public List<Vector3D> ZoneCoords;
		public List<bool> ZoneToggleActiveModes;



		//To do GPS for specific players only
		public bool AddGPSForAll;
		public bool RemoveGPSForAll;

		public bool UseGPSObjective;
		public List<string> GPSNames;
		public List<string> GPSDescriptions;
		public List<Vector3D> GPSVector3Ds;
		public List<Vector3D> GPSColors;



		public bool SpawnEncounter;
		//public List<SpawnProfile> SpawnData;
		public List<Vector3D> SpawnVector3Ds;
		public List<string> SpawnFactionTags;


		public bool UseChatBroadcast;
		public List<ChatProfile> ChatData;

		public bool SetEventControllers;
		public List<string> EventControllerNames;
		public List<bool> EventControllersActive;
		public List<bool> EventControllersSetCurrentTime;

		public bool ActivateCustomAction;
		public string CustomActionName;
		public List<object> CustomActionArguments;





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
			ResetEventCooldownIds = new List<string>();
			ResetEventCooldownTags = new List<string>();

			ChangeZoneAtPosition = false;
			ZoneNames = new List<string>();
			ZoneCoords = new List<Vector3D>();
			ZoneToggleActiveModes = new List<bool>();

			AddGPSForAll = false;
			RemoveGPSForAll = false;
			UseGPSObjective = false;
			GPSNames = new List<string>();
			GPSDescriptions = new List<string>();
			GPSVector3Ds = new List<Vector3D>();
			GPSColors = new List<Vector3D>();

			ToggleEvents = false;
			ToggleEventIds = new List<string>();
			ToggleEventIdModes = new List<bool>();
			ToggleEventTags = new List<string>();
			ToggleEventTagModes = new List<bool>();
			SpawnEncounter = true;
			//SpawnData = new List<SpawnProfile>();
			SpawnVector3Ds = new List<Vector3D>();
			SpawnFactionTags = new List<string>();

			UseChatBroadcast = false;
			ChatData = new List<ChatProfile>();

			ActivateCustomAction = false;
			CustomActionName = "";
			CustomActionArguments = new List<object>();


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

				{"ResetEventCooldownIds", (s, o) => TagParse.TagStringListCheck(s, ref ResetEventCooldownIds) },
				{"ResetEventCooldownTags", (s, o) => TagParse.TagStringListCheck(s, ref ResetEventCooldownTags) },


				{"ToggleEvents", (s, o) => TagParse.TagBoolCheck(s, ref ToggleEvents) },
				{"ToggleEventIds", (s, o) => TagParse.TagStringListCheck(s, ref ToggleEventIds) },
				{"ToggleEventIdModes", (s, o) => TagParse.TagBoolListCheck(s, ref ToggleEventIdModes) },
				{"ToggleEventTags", (s, o) => TagParse.TagStringListCheck(s, ref ToggleEventTags) },
				{"ToggleEventTagModes", (s, o) => TagParse.TagBoolListCheck(s, ref ToggleEventTagModes) },


				{"AddGPSForAll", (s, o) => TagParse.TagBoolCheck(s, ref AddGPSForAll) },
				{"RemoveGPSForAll", (s, o) => TagParse.TagBoolCheck(s, ref RemoveGPSForAll) },
				{"UseGPSObjective", (s, o) => TagParse.TagBoolCheck(s, ref UseGPSObjective) },
				{"GPSNames", (s, o) => TagParse.TagStringListCheck(s, ref GPSNames) },
				{"GPSDescriptions", (s, o) => TagParse.TagStringListCheck(s, ref GPSDescriptions) },
				{"GPSCoords", (s, o) => TagParse.TagVector3DListCheck(s, ref GPSVector3Ds) },
				{"GPSColors", (s, o) => TagParse.TagVector3DListCheck(s, ref GPSColors) },

				{"SpawnEncounter", (s, o) => TagParse.TagBoolCheck(s, ref SpawnEncounter) },
				{"SpawnCoords", (s, o) => TagParse.TagVector3DListCheck(s, ref SpawnVector3Ds) },
				{"SpawnFactionTags", (s, o) => TagParse.TagStringListCheck(s, ref SpawnFactionTags) },

				{"ChangeZoneAtPosition", (s, o) => TagParse.TagBoolCheck(s, ref ChangeZoneAtPosition) },
				{"ZoneNames", (s, o) => TagParse.TagStringListCheck(s, ref ZoneNames) },
				{"ZoneCoords", (s, o) => TagParse.TagVector3DListCheck(s, ref ZoneCoords) },
				{"ZoneToggleActiveModes", (s, o) => TagParse.TagBoolListCheck(s, ref ZoneToggleActiveModes) },

				{"ActivateCustomAction", (s, o) => TagParse.TagBoolCheck(s, ref ActivateCustomAction) },
				{"CustomActionName", (s, o) => TagParse.TagStringCheck(s, ref CustomActionName) },
				{"CustomActionArguments", (s, o) => TagParse.TagObjectListCheck(s, ref CustomActionArguments) },
				

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



