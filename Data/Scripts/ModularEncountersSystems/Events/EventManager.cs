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

		private static int ticks = 0;

		private static string _saveEventsListName = "MES-EventsList";

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



			//SpawnLogger.Write("Delete Old Events", SpawnerDebugEnum.Dev, true);
			//Delete Old Events
			for (int i = EventsList.Count - 1; i >= 0; i--) {

				bool foundEvent = false;

				foreach (var eventProfile in ProfileManager.EventProfiles) {

					if (EventsList[i].Insertable)
                    {
						foundEvent = true;
						break;
					}


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


			// As of writing this, whenever you load into a world the your view is locked for the first second. Players could miss any event messages then.   - CptArthur 
			if (ticks < 10)
			{
				ticks++;
				return;
			}


			//MyVisualScriptLogicProvider.ShowNotificationToAll("EventList Count: " + EventsList.Count, 500);

			for (int i = EventsList.Count - 1; i >= 0; i--)
			{
				var thisEvent = EventsList[i];


				if (thisEvent.MarkedforRemoval)
                {
					EventsList.RemoveAt(i);
					continue;
				}


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

					currentEvent.Actions[currentEvent.ActionIndex].ExecuteAction(currentEvent.InstanceId);
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
