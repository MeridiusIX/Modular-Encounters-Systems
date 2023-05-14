using ModularEncountersSystems.Events.Condition;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Zones;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Events.Action {
	public partial class EventActionProfile {

		public void ExecuteAction() {

			var actions = ActionReference;
			var EventBroadcastSystem = new EventBroadcastSystem();

			//DebugHudMessage
			if (!string.IsNullOrWhiteSpace(actions.DebugHudMessage))
				MyVisualScriptLogicProvider.ShowNotificationToAll(actions.DebugHudMessage, 3000);

			//Booleans
			if (actions.ChangeBooleans == true) {

				for (int i = 0; i < actions.SetBooleansTrue.Count; i++) {

					MyAPIGateway.Utilities.SetVariable<bool>(actions.SetBooleansTrue[i], true);

				}
				for (int i = 0; i < actions.SetBooleansFalse.Count; i++) {

					MyAPIGateway.Utilities.SetVariable<bool>(actions.SetBooleansFalse[i], false);

				}

			}

			//Change Counter
			if (actions.ChangeCounters) {

				if (actions.IncreaseCounters.Count != actions.IncreaseCountersAmount.Count)
					return;

				if (actions.DecreaseCounters.Count != actions.DecreaseCountersAmount.Count)
					return;

				if (actions.SetCounters.Count != actions.SetCountersAmount.Count)
				{
					return;
				}


				for (int i = 0; i < actions.SetCounters.Count; i++)
				{
					SetCounter(actions.SetCounters[i], actions.SetCountersAmount[i], true);
				}


				for (int i = 0; i < actions.IncreaseCounters.Count; i++) {
					SetCounter(actions.IncreaseCounters[i], Math.Abs(actions.IncreaseCountersAmount[i]), false);
				}

				for (int i = 0; i < actions.DecreaseCounters.Count; i++) {

					SetCounter(actions.DecreaseCounters[i], - Math.Abs(actions.DecreaseCountersAmount[i]), false);
				}

			}



            if (actions.ResetCooldownTimeOfEvents)
            {
				ResetCooldownTimeOfEvents(actions.ResetEventCooldownNames, actions.ResetEventCooldownTags);
			}

			//ChatBroadcast
			if (actions.UseChatBroadcast == true) {

				foreach (var chatData in ChatData) {

					EventBroadcastSystem.BroadcastRequest(chatData);

				}

			}


			if (actions.ChangeZoneAtPosition)
			{

				if (actions.ZoneNames.Count != actions.ZoneCoords.Count)
					return;


				if (actions.ZoneNames.Count != actions.ZoneToggleActiveModes.Count)
					return;


				for (int i = 0; i < actions.ZoneNames.Count; i++)
				{
					ZoneManager.ToggleZonesAtPosition(actions.ZoneCoords[i], actions.ZoneNames[i], actions.ZoneToggleActiveModes[i]);
				}


			}


				/*
				//SetEventControllers
				if (actions.SetEventControllers)
					EventControllerSettings(actions.EventControllerNames, actions.EventControllersActive, actions.EventControllersSetCurrentTime);
				*/

			}

		/*
		private void EventControllerSettings(List<string> names, List<bool> active, List<bool> setCurrentTime) {

			for (int i = 0; i < names.Count && i < active.Count && i < setCurrentTime.Count; i++) {

				bool found = false;

				foreach (var controller in EventManager.EventControllersList) {

					if (controller.ProfileSubtypeId != names[i])
						continue;

					found = true;

					controller.Active = active[i];

					if (setCurrentTime[i])
						controller.StartDate = Helpers.Time.GetRealIngameTime();

					break;

				}

				if (found)
					continue;

				var newcontroller = EventController.CreateController(names[i]);
				newcontroller.Active = active[i];
				newcontroller.StartDate = MyAPIGateway.Session.GameDateTime;
				EventManager.EventControllersList.Add(newcontroller);

			}
		
		}
		*/

		public void SetCounter(string counterName, int amount, bool hardSet = false)
		{

			if (hardSet)
			{

				MyAPIGateway.Utilities.SetVariable<int>(counterName, amount);
				return;

			}

			int existingCounter = 0;

			MyAPIGateway.Utilities.GetVariable(counterName, out existingCounter);

			//This is for ResetCounters
			if (amount == 0)
			{

				MyAPIGateway.Utilities.SetVariable<int>(counterName, 0);
				return;

			}

			else
			{
				existingCounter += amount;
				MyAPIGateway.Utilities.SetVariable<int>(counterName, existingCounter);
				return;

			}

		}


		public static void ResetCooldownTimeOfEvents(List<string> ResetEventCooldownNames, List<string> ResetEventCooldownTags, string SpawnGroupName = "")
		{
			foreach (var EventName in ResetEventCooldownNames)
			{
				var Name = EventName;
				if (EventName.Contains("{SpawnGroupName}") && SpawnGroupName != null)
				{
					Name = EventName.Replace("{SpawnGroupName}", SpawnGroupName);
				}
				foreach (var Event in EventManager.EventsList)
				{
					if (Name == Event.ProfileSubtypeId)
					{
						Event.LastTriggerTime = MyAPIGateway.Session.GameDateTime;
						Event.CooldownTimeTrigger = MathTools.RandomBetween(Event.Profile.MinCooldownMs, Event.Profile.MaxCooldownMs);
					}
				}
			}

			foreach (var Tag in ResetEventCooldownTags)
			{
				foreach (var Event in EventManager.EventsList)
				{
					if (Event.Profile.Tags.Contains(Tag))
					{
						Event.LastTriggerTime = MyAPIGateway.Session.GameDateTime;
						Event.CooldownTimeTrigger = MathTools.RandomBetween(Event.Profile.MinCooldownMs, Event.Profile.MaxCooldownMs);
					}
				}
			}

		}






	}

}
