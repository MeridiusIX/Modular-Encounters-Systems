using ModularEncountersSystems.Events.Condition;
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

				for (int i = 0; i < actions.SetSandboxBooleansTrue.Count; i++) {

					MyAPIGateway.Utilities.SetVariable<bool>(actions.SetSandboxBooleansTrue[i], true);

				}
				for (int i = 0; i < actions.SetSandboxBooleansFalse.Count; i++) {

					MyAPIGateway.Utilities.SetVariable<bool>(actions.SetSandboxBooleansFalse[i], false);

				}

			}

			//Change Counter
			if (actions.ChangeCounters) {

				if (actions.IncreaseSandboxCounters.Count != actions.IncreaseSandboxCountersAmount.Count)
					return;

				if (actions.DecreaseSandboxCounters.Count != actions.DecreaseSandboxCountersAmount.Count)
					return;


				for (int i = 0; i < actions.IncreaseSandboxCounters.Count; i++) {
					SetSandboxCounter(actions.IncreaseSandboxCounters[i], Math.Abs(actions.IncreaseSandboxCountersAmount[i]), false);
				}

				for (int i = 0; i < actions.DecreaseSandboxCounters.Count; i++) {

					SetSandboxCounter(actions.DecreaseSandboxCounters[i], - Math.Abs(actions.DecreaseSandboxCountersAmount[i]), false);
				}

			}


			//Set Counter
			if (actions.SetCounters)
			{

				if (actions.SetSandboxCounters.Count != actions.SetSandboxCountersAmount.Count)
				{
					return;
				}


				for (int i = 0; i < actions.SetSandboxCounters.Count; i++)
				{
					SetSandboxCounter(actions.SetSandboxCounters[i], actions.SetSandboxCountersAmount[i], true);
				}

			}

			//ChatBroadcast
			if (actions.UseChatBroadcast == true) {

				foreach (var chatData in ChatData) {

					EventBroadcastSystem.BroadcastRequest(chatData);

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

		public void SetSandboxCounter(string counterName, int amount, bool hardSet = false)
		{

			if (hardSet)
			{

				MyAPIGateway.Utilities.SetVariable<int>(counterName, amount);
				return;

			}

			int existingCounter = 0;

			MyAPIGateway.Utilities.GetVariable(counterName, out existingCounter);

			//This is for ResetSandboxCounters
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



	}

}
