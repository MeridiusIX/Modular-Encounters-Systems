using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Tasks;
using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using Sandbox.ModAPI;
using Sandbox.Game;
using System.Text;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Events.Condition;

namespace ModularEncountersSystems.Events {

	public enum CheckType {
		ExecuteEvent,
		ExecuteAction
	}

	public static class EventManager {

		public static List<Event> EventsList = new List<Event>();

		/*
		public static List<EventController> EventControllersList = new List<EventController>();
		*/

		private static string _saveEventsListName = "MES-EventsList";
		private static string _saveEventControllersListName = "MES-EventControllersList";

		private static List<Event> _readyEvents = new List<Event>();

		public static void Setup() {

			//SpawnLogger.Write("Start", SpawnerDebugEnum.Dev, true);
			//Register Any Actions/Events
			MES_SessionCore.SaveActions += SaveData;
			MES_SessionCore.UnloadActions += UnloadData;
			TaskProcessor.Tick30.Tasks += ProcessEvents;

			//SpawnLogger.Write("Existing Event Data", SpawnerDebugEnum.Dev, true);
			//Get Existing Event Data
			string eventsListString = "";

			if (MyAPIGateway.Utilities.GetVariable<string>(_saveEventsListName, out eventsListString)) {

				var eventsListSerialized = Convert.FromBase64String(eventsListString);
				EventsList = MyAPIGateway.Utilities.SerializeFromBinary<List<Event>>(eventsListSerialized);

			}

			if (EventsList == null)
				EventsList = new List<Event>();

			foreach (var existingEvent in EventsList) {

				existingEvent.Init();
			
			}

			/*
			//SpawnLogger.Write("Existing EventController Data", SpawnerDebugEnum.Dev, true);
			//Get Existing EventController Data
			string eventControllersListString = "";

			if (MyAPIGateway.Utilities.GetVariable<string>(_saveEventControllersListName, out eventControllersListString)) {

				var eventsListSerialized = Convert.FromBase64String(eventControllersListString);
				EventControllersList = MyAPIGateway.Utilities.SerializeFromBinary<List<EventController>>(eventsListSerialized);

			}
			*/

			if (EventsList == null)
				EventsList = new List<Event>();

			//SpawnLogger.Write("Create New Events", SpawnerDebugEnum.Dev, true);
			//Create New Events
			foreach (var eventProfile in ProfileManager.EventProfiles) {

				bool foundEvent = false;

				foreach (var existingEvent in EventsList) {

					if (existingEvent.ProfileSubtypeId == eventProfile.Value.ProfileSubtypeId) {

						foundEvent = true;
						break;
					
					}
				
				}

				if (foundEvent)
					continue;

				EventsList.Add(new Event(eventProfile.Value.ProfileSubtypeId));

			}

			/*
			//SpawnLogger.Write("Create New Controllers", SpawnerDebugEnum.Dev, true);
			//Create New Event Controllers
			foreach (var existingEvent in EventsList) {

				if (string.IsNullOrWhiteSpace(existingEvent?.Profile?.EventControllerId))
					continue;

				bool gotController = false;

				foreach (var controller in EventControllersList) {

					if (controller.ProfileSubtypeId == existingEvent.ProfileSubtypeId) {

						gotController = true;
						break;
					
					}
				
				}

				if (gotController)
					continue;

				
				EventControllersList.Add(EventController.CreateController(existingEvent.Profile.EventControllerId));
				
			
			}
			*/

			//SpawnLogger.Write("Delete Old Events", SpawnerDebugEnum.Dev, true);
			//Delete Old Events
			for (int i = EventsList.Count - 1; i >= 0; i--) {

				bool foundEvent = false;

				foreach (var eventProfile in ProfileManager.EventProfiles) {

					if (eventProfile.Value.ProfileSubtypeId == EventsList[i].ProfileSubtypeId) {

						foundEvent = true;
						break;
					
					}
				
				}

				if (foundEvent)
					continue;

				EventsList.RemoveAt(i);

			}

			//SpawnLogger.Write("Save Data", SpawnerDebugEnum.Dev, true);
			//Save Current Data
			SaveData();
			

		}

		public static void ProcessEvents() {
			
			DateTime currentDateTime = MyAPIGateway.Session.GameDateTime;
			_readyEvents.Clear();

			//MyVisualScriptLogicProvider.ShowNotificationToAll("EventList Count: " + EventsList.Count, 500);

			for (int i = 0; i < EventsList.Count; i++) {

				var thisEvent = EventsList[i];

				if (!thisEvent.Valid) {

					continue;
				
				}

				//Process Sequential Actions
				if (thisEvent.ProcessingSequentialActions) {

					ProcessSequentialActions(thisEvent, currentDateTime);
					continue;
				
				}


                if (!thisEvent.EventEnabled)
                {
					continue;
                }

				/*
				//Does this event have an Event Controller (previously MainEvent)? If so, check it.
				if (!string.IsNullOrWhiteSpace(thisEvent.Profile.EventControllerId)) {

					if (thisEvent.Controller == null) {

						continue;
					
					}

					//TODO: Maybe we move all EventController checks to the class itself and check with a single method sometime down the road.
					//TODO: Better yet, we move all the checks against Active, Time, etc into the Condition Profile. That way you could have events that trigger when its Inactive
					if (!thisEvent.Controller.Active) {

						continue;
					
					}
				
				}
				*/

				//Did the event already happen once? && Is the event a unique event?
				if (thisEvent.RunCount > 0 && thisEvent.Profile.UniqueEvent == true) {

					continue;

				}


				//Check the cooldown
				if (!thisEvent.ValidateCooldown()) {

					continue;

				}


				//Check PersistantCondition
				if (!EventCondition.AreConditionsMet(false, thisEvent.PersistantConditions))
				{
					if (thisEvent.Profile.OnFailResetCooldown)
                    {
						thisEvent.LastTriggerTime = MyAPIGateway.Session.GameDateTime;
						thisEvent.CooldownTimeTrigger = MathTools.RandomBetween(thisEvent.Profile.MinCooldownMs, thisEvent.Profile.MaxCooldownMs);

					}

					continue;
				}

				int index = 0;
				//Check Conditions
				if (!EventCondition.AreConditionsMet(thisEvent.Profile.UseAnyPassingCondition, thisEvent.Conditions, out index)) {

					if (thisEvent.Profile.OnFailResetCooldown)
                    {
						thisEvent.LastTriggerTime = MyAPIGateway.Session.GameDateTime;
						thisEvent.CooldownTimeTrigger = MathTools.RandomBetween(thisEvent.Profile.MinCooldownMs, thisEvent.Profile.MaxCooldownMs);

					}



					continue;

				}

				
				
				thisEvent.Ready = true;
				thisEvent.RequiredConditionIndex = index;
				_readyEvents.Add(thisEvent);
			}

			for (int i = _readyEvents.Count - 1; i >= 0; i--) {

				var thisEvent = _readyEvents[i];

				if (thisEvent.Ready == true) {

					thisEvent.Ready = false;

					thisEvent.ActivateEventActions();
					thisEvent.RunCount++;
					_readyEvents.Remove(thisEvent);

				}

			}

		}

		public static void ProcessSequentialActions(Event currentEvent, DateTime currentDateTime) {

			var timeSpan = currentDateTime - currentEvent.ActionStartTime;

			if (timeSpan.TotalMilliseconds > currentEvent.IncrementMs * currentEvent.ActionIndex) {

				if (currentEvent.ActionIndex >= currentEvent.Actions.Count) {

					currentEvent.ActionIndex = 0;
					currentEvent.ProcessingSequentialActions = false;
					return;

				}

				if (currentEvent.CheckingType == CheckType.ExecuteAction) {

					currentEvent.Actions[currentEvent.ActionIndex].ExecuteAction();
					currentEvent.ActionIndex++;

					if (currentEvent.ActionIndex >= currentEvent.Actions.Count) {

						currentEvent.ActionIndex = 0;
						currentEvent.ProcessingSequentialActions = false;

					}

				}

			}

		}

		public static void SaveData() {

			//Events
			var eventsListSerialized = MyAPIGateway.Utilities.SerializeToBinary<List<Event>>(EventsList);
			var eventsListString = Convert.ToBase64String(eventsListSerialized);
			MyAPIGateway.Utilities.SetVariable<string>(_saveEventsListName, eventsListString);

			/*
			//EventControllers
			var eventControllersListSerialized = MyAPIGateway.Utilities.SerializeToBinary<List<EventController>>(EventControllersList);
			var eventControllersListString = Convert.ToBase64String(eventControllersListSerialized);
			MyAPIGateway.Utilities.SetVariable<string>(_saveEventControllersListName, eventControllersListString);
			*/

		}

		public static string GetEventData() {

			var sb = new StringBuilder();

			return sb.ToString();
		
		}

		public static void UnloadData() {

			//Unregister Any Actions/Events That Were Registered in Setup()
			MES_SessionCore.SaveActions -= SaveData;
			MES_SessionCore.UnloadActions -= UnloadData;
			TaskProcessor.Tick30.Tasks -= ProcessEvents;
		}

	}

}
