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
		public int Chance;

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

		public bool IncreaseRunCountOfEvents;
		public List<string> IncreaseRunCountEventIds;
		public List<int> IncreaseRunCountEventIdAmount;

		public List<string> IncreaseRunCountEventTags;
		public List<int> IncreaseRunCountEventTagAmount;

		public bool ChangeZoneAtPosition;
		public List<string> ZoneNames;
		public List<Vector3D> ZoneCoords;
		public List<bool> ZoneToggleActiveModes;


		//Player Start
		public bool AddTagstoPlayers;
		public List<string> AddTagsPlayerConditionIds;
		public List<string> AddTags;

		public bool RemoveTagsFromPlayers;
		public List<string> RemoveTagsPlayerConditionIds;
		public List<string> RemoveTags;

		public bool FadeOutPlayers;
		public List<string> FadeOutPlayerConditionIds;

		public bool FadeInPlayers;
		public List<string> FadeInPlayerConditionIds;

		public bool AddItemToPlayersInventory;
		public List<string> AddItemPlayerConditionIds;
		public List<string> ItemIds;

		public bool TeleportPlayers;
		public List<string> TeleportPlayerConditionIds;
		public Vector3D TeleportPlayerCoords;
		public float TeleportRadius;

		public bool AddGPSToPlayers;
		public bool AddGPSToAll;
		public bool RemoveGPSFromPlayers;
		public List<string> AddGPSPlayerConditionIds;
		public List<string> RemoveGPSPlayerConditionIds;

		public bool UseGPSObjective;
		public List<string> GPSNames;
		public List<string> GPSDescriptions;
		public List<Vector3D> GPSVector3Ds;
		public List<Vector3D> GPSColors;

		public bool ChangeReputationWithPlayers;
		public List<string> ReputationPlayerConditionIds;
		public List<int> ReputationChangeAmount;
		public List<string> ReputationChangeFactions;
		public bool ReputationChangesForAllRadiusPlayerFactionMembers;
		public int ReputationMinCap;
		public int ReputationMaxCap;
		//PlayersEnd

		public bool BroadcastCommandProfiles;
		public List<string> CommandProfileIds;
		public Vector3D CommandProfileOriginCoords;

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
		public List<string> CustomActionArgumentsString;
		public List<bool> CustomActionArgumentsBool;
		public List<int> CustomActionArgumentsInt;
		public List<float> CustomActionArgumentsFloat;
		public List<long> CustomActionArgumentsLong;
		public List<double> CustomActionArgumentsDouble;
		public List<Vector3D> CustomActionArgumentsVector3D;



		public string DebugHudMessage;

		public Dictionary<string, Action<string, object>> EditorReference;

		public EventActionReferenceProfile() {

			ProfileSubtypeId = "";
			Chance = 100;
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


			ChangeZoneAtPosition = false;
			ZoneNames = new List<string>();
			ZoneCoords = new List<Vector3D>();
			ZoneToggleActiveModes = new List<bool>();

			AddGPSToPlayers = false;
			AddGPSToAll = false;
			RemoveGPSFromPlayers = false;

			BroadcastCommandProfiles = false;
			CommandProfileIds = new List<string>();
			CommandProfileOriginCoords = new Vector3D();

			UseGPSObjective = false;
			GPSNames = new List<string>();
			GPSDescriptions = new List<string>();
			GPSVector3Ds = new List<Vector3D>();
			GPSColors = new List<Vector3D>();

			AddGPSPlayerConditionIds = new List<string>();
			RemoveGPSPlayerConditionIds = new List<string>();

			ToggleEvents = false;
			ToggleEventIds = new List<string>();
			ToggleEventIdModes = new List<bool>();
			ToggleEventTags = new List<string>();
			ToggleEventTagModes = new List<bool>();


			ResetCooldownTimeOfEvents = false;
			ResetEventCooldownIds = new List<string>();
			ResetEventCooldownTags = new List<string>();

			IncreaseRunCountOfEvents =false;
			IncreaseRunCountEventIds = new List<string>();
			IncreaseRunCountEventIdAmount = new List<int>() { 1 };

			IncreaseRunCountEventTags = new List<string>();
			IncreaseRunCountEventTagAmount = new List<int>() { 1 };


		AddTagstoPlayers = false;
			AddTagsPlayerConditionIds = new List<string>();
			AddTags = new List<string>();

			RemoveTagsFromPlayers = false;
			RemoveTagsPlayerConditionIds = new List<string>();
			RemoveTags = new List<string>();

			FadeInPlayers = false;
			FadeInPlayerConditionIds = new List<string>();

			FadeOutPlayers = false;
			FadeOutPlayerConditionIds = new List<string>();

			AddItemToPlayersInventory=false;
			AddItemPlayerConditionIds = new List<string>();
			ItemIds = new List<string>();

			TeleportPlayers = false;
			TeleportPlayerConditionIds = new List<string>();
			TeleportPlayerCoords = new Vector3D();
			TeleportRadius = 0;

			ChangeReputationWithPlayers = false;
			ReputationPlayerConditionIds = new List<string>();
			ReputationChangeFactions = new List<string>();
			ReputationChangeAmount = new List<int>();
			ReputationChangesForAllRadiusPlayerFactionMembers = false;
			ReputationMinCap = -1500;
			ReputationMaxCap = 1500;




			SpawnEncounter = true;
			//SpawnData = new List<SpawnProfile>();
			SpawnVector3Ds = new List<Vector3D>();
			SpawnFactionTags = new List<string>();

			UseChatBroadcast = false;
			ChatData = new List<ChatProfile>();

			ActivateCustomAction = false;
			CustomActionName = "";

			CustomActionArgumentsString = new List<string>();
			CustomActionArgumentsBool = new List<bool>();	
			CustomActionArgumentsInt = new List<int>();
			CustomActionArgumentsFloat = new List<float>();
			CustomActionArgumentsLong = new List<long>();
			CustomActionArgumentsDouble = new List<double>();
			CustomActionArgumentsVector3D = new List<Vector3D>();

			DebugHudMessage = "";

			SetEventControllers = false;
			EventControllerNames = new List<string>();
			EventControllersActive = new List<bool>();
			EventControllersSetCurrentTime = new List<bool>();

			EditorReference = new Dictionary<string, Action<string, object>> {

				{"ChangeBooleans", (s, o) => TagParse.TagBoolCheck(s, ref ChangeBooleans) },
				{"Chance", (s, o) => TagParse.TagIntCheck(s, ref Chance) },
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

				{"AddTagstoPlayers", (s, o) => TagParse.TagBoolCheck(s, ref AddTagstoPlayers) },

				{"AddTagsToPlayers", (s, o) => TagParse.TagBoolCheck(s, ref AddTagstoPlayers) },
				{"AddTagsPlayerConditionIds", (s, o) => TagParse.TagStringListCheck(s, ref AddTagsPlayerConditionIds) },
				{"AddTags", (s, o) => TagParse.TagStringListCheck(s, ref AddTags) },

				{"RemoveTagsFromPlayers", (s, o) => TagParse.TagBoolCheck(s, ref RemoveTagsFromPlayers) },
				{"RemoveTagsPlayerConditionIds", (s, o) => TagParse.TagStringListCheck(s, ref RemoveTagsPlayerConditionIds) },
				{"RemoveTags", (s, o) => TagParse.TagStringListCheck(s, ref RemoveTags) },

				{"FadeInPlayers", (s, o) => TagParse.TagBoolCheck(s, ref FadeInPlayers) },
				{"FadeInPlayerConditionIds", (s, o) => TagParse.TagStringListCheck(s, ref FadeInPlayerConditionIds) },

				{"FadeOutPlayers", (s, o) => TagParse.TagBoolCheck(s, ref FadeOutPlayers) },
				{"FadeOutPlayerConditionIds", (s, o) => TagParse.TagStringListCheck(s, ref FadeOutPlayerConditionIds) },

				{"AddItemToPlayersInventory", (s, o) => TagParse.TagBoolCheck(s, ref AddItemToPlayersInventory) },
				{"AddItemPlayerConditionIds", (s, o) => TagParse.TagStringListCheck(s, ref AddItemPlayerConditionIds) },
				{"ItemIds", (s, o) => TagParse.TagStringListCheck(s, ref ItemIds) },

				{ "BroadcastCommandProfiles", (s, o) => TagParse.TagBoolCheck(s, ref BroadcastCommandProfiles) },
				{"CommandProfileIds", (s, o) => TagParse.TagStringListCheck(s, ref CommandProfileIds) },
				{"CommandProfileOriginCoords", (s, o) => TagParse.TagVector3DCheck(s, ref CommandProfileOriginCoords) },
				
				{"ToggleEvents", (s, o) => TagParse.TagBoolCheck(s, ref ToggleEvents) },
				{"ToggleEventIds", (s, o) => TagParse.TagStringListCheck(s, ref ToggleEventIds) },
				{"ToggleEventIdModes", (s, o) => TagParse.TagBoolListCheck(s, ref ToggleEventIdModes) },
				{"ToggleEventTags", (s, o) => TagParse.TagStringListCheck(s, ref ToggleEventTags) },
				{"ToggleEventTagModes", (s, o) => TagParse.TagBoolListCheck(s, ref ToggleEventTagModes) },

				{"IncreaseRunCountOfEvents", (s, o) => TagParse.TagBoolCheck(s, ref IncreaseRunCountOfEvents) },
				{"IncreaseRunCountEventIds", (s, o) => TagParse.TagStringListCheck(s, ref IncreaseRunCountEventIds) },
				{"IncreaseRunCountEventIdAmount", (s, o) => TagParse.TagIntListCheck(s, ref IncreaseRunCountEventIdAmount) },
				{"IncreaseRunCountEventTags", (s, o) => TagParse.TagStringListCheck(s, ref IncreaseRunCountEventTags) },
				{"IncreaseRunCountEventTagAmount", (s, o) => TagParse.TagIntListCheck(s, ref IncreaseRunCountEventTagAmount) },



			{ "AddGPSToPlayers", (s, o) => TagParse.TagBoolCheck(s, ref AddGPSToPlayers) },
				{"RemoveGPSFromPlayers", (s, o) => TagParse.TagBoolCheck(s, ref RemoveGPSFromPlayers) },
				{"UseGPSObjective", (s, o) => TagParse.TagBoolCheck(s, ref UseGPSObjective) },
				{"GPSNames", (s, o) => TagParse.TagStringListCheck(s, ref GPSNames) },
				{"GPSDescriptions", (s, o) => TagParse.TagStringListCheck(s, ref GPSDescriptions) },
				{"GPSCoords", (s, o) => TagParse.TagVector3DListCheck(s, ref GPSVector3Ds) },
				{"GPSVector3Ds", (s, o) => TagParse.TagVector3DListCheck(s, ref GPSVector3Ds) },

				{"GPSColors", (s, o) => TagParse.TagVector3DListCheck(s, ref GPSColors) },
				{"AddGPSPlayerConditionIds", (s, o) => TagParse.TagStringListCheck(s, ref AddGPSPlayerConditionIds) },
				{"RemoveGPSPlayerConditionIds", (s, o) => TagParse.TagStringListCheck(s, ref RemoveGPSPlayerConditionIds) },



				{"SpawnEncounter", (s, o) => TagParse.TagBoolCheck(s, ref SpawnEncounter) },
				{"SpawnCoords", (s, o) => TagParse.TagVector3DListCheck(s, ref SpawnVector3Ds) },
				{"SpawnFactionTags", (s, o) => TagParse.TagStringListCheck(s, ref SpawnFactionTags) },

				{"ChangeZoneAtPosition", (s, o) => TagParse.TagBoolCheck(s, ref ChangeZoneAtPosition) },
				{"ZoneNames", (s, o) => TagParse.TagStringListCheck(s, ref ZoneNames) },
				{"ZoneCoords", (s, o) => TagParse.TagVector3DListCheck(s, ref ZoneCoords) },
				{"ZoneToggleActiveModes", (s, o) => TagParse.TagBoolListCheck(s, ref ZoneToggleActiveModes) },

				{"TeleportPlayers", (s, o) => TagParse.TagBoolCheck(s, ref TeleportPlayers) },
				{"TeleportPlayerConditionIds", (s, o) => TagParse.TagStringListCheck(s, ref TeleportPlayerConditionIds) },
				{"TeleportPlayerCoords", (s, o) => TagParse.TagVector3DCheck(s, ref TeleportPlayerCoords) },
				{"TeleportRadius", (s, o) => TagParse.TagFloatCheck(s, ref TeleportRadius) },

				{"ChangeReputationWithPlayers", (s, o) => TagParse.TagBoolCheck(s, ref ChangeReputationWithPlayers) },
				{"ReputationPlayerConditionIds", (s, o) => TagParse.TagStringListCheck(s, ref ReputationPlayerConditionIds) },
				{"ReputationChangeFactions", (s, o) => TagParse.TagStringListCheck(s, ref ReputationChangeFactions) },
				{"ReputationChangeAmount", (s, o) => TagParse.TagIntListCheck(s, ref ReputationChangeAmount) },
				{"ReputationChangesForAllRadiusPlayerFactionMembers", (s, o) => TagParse.TagBoolCheck(s, ref ReputationChangesForAllRadiusPlayerFactionMembers) },
				{"ReputationMinCap", (s, o) => TagParse.TagIntCheck(s, ref ReputationMinCap) },
				{"ReputationMaxCap", (s, o) => TagParse.TagIntCheck(s, ref ReputationMaxCap) },

				{"ActivateCustomAction", (s, o) => TagParse.TagBoolCheck(s, ref ActivateCustomAction) },
				{"CustomActionName", (s, o) => TagParse.TagStringCheck(s, ref CustomActionName) },
				{"CustomActionArgumentsString", (s, o) => TagParse.TagStringListCheck(s,false ,ref CustomActionArgumentsString) },
				{"CustomActionArgumentsBool", (s, o) => TagParse.TagBoolListCheck(s, ref CustomActionArgumentsBool) },
				{"CustomActionArgumentsInt", (s, o) => TagParse.TagIntListCheck(s, ref CustomActionArgumentsInt) },
				{"CustomActionArgumentsFloat", (s, o) => TagParse.TagFloatCheck(s, ref CustomActionArgumentsFloat) },
				{"CustomActionArgumentsLong", (s, o) => TagParse.TagLongCheck(s, ref CustomActionArgumentsLong) },
				{"CustomActionArgumentsDouble", (s, o) => TagParse.TagDoubleCheck(s, ref CustomActionArgumentsDouble) },
				{"CustomActionArgumentsVector3D", (s, o) => TagParse.TagVector3DListCheck(s, ref CustomActionArgumentsVector3D) },

				{ "UseChatBroadcast", (s, o) => TagParse.TagBoolCheck(s, ref UseChatBroadcast) },
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



