using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Tasks;
using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using Sandbox.ModAPI;
using Sandbox.Game;
using System.Text;

namespace ModularEncountersSystems.Events {
	public static class EventManager {

		public static List<EventTime> EventTimes = new List<EventTime>();


		public static void Setup() {

			//Register Any Actions/Events
			MES_SessionCore.SaveActions += SaveData;
			MES_SessionCore.UnloadActions += UnloadData;
			TaskProcessor.Tick30.Tasks += ProcessEvents;

			if (!string.IsNullOrWhiteSpace(Settings.SavedData?.EventData)) {

				//TODO: Get Serialized Data From Settings.SavedData.EventData and Deserialize it. Cache it in this class

			} else {
			
				//TODO: Since no existing event data exists, create event data from scratch
			
			}
			

		}

		public static void ProcessEvents()
		{
			DateTime currentDateTime;
			List<Event> ReadyEvents = new List<Event>();


			//Which Main Events are on
			foreach (KeyValuePair<string, MainEvent> MainEvent in ProfileManager.MainEvents)
			{
				//Check if MainEvent is on
				if (!MainEvent.Value.Active)
					break;

				for (int i = 0; i < MainEvent.Value.Events.Count; i++)
				{
					//Check Times

					//Check Conditions
					if (!EventCondition.AreConditionsMet(MainEvent.Value, MainEvent.Value.Events[i].Conditions))
						break;

					//Did the event already happen? && Is the event a unique event?
					if (MainEvent.Value.Events[i].Happend == true && MainEvent.Value.Events[i].UniqueEvent == true)
						break;


					MainEvent.Value.Events[i].Ready = true;
					ReadyEvents.Add(MainEvent.Value.Events[i]);
				}
			}

			for (int i = 0; i < ReadyEvents.Count; i++)
			{
				if (ReadyEvents[i].Ready == true)
				{
					ReadyEvents[i].Happend = true;
					ReadyEvents[i].Ready = false;
					EventAction.ExecuteActions(ReadyEvents[i], ReadyEvents[i].Actions);
				}
			}


			//CheckTimers
			for (int i = 0; i < EventTimes.Count; i++)
			{
				currentDateTime = MyAPIGateway.Session.GameDateTime;
				var timeSpan = currentDateTime - EventTimes[i].StartDate;
				if (timeSpan.TotalMilliseconds > EventTimes[i].Timeinms)
                {
					if(EventTimes[i].Type == CheckType.ExecuteAction)
                    {
						EventAction.ExecuteActions(EventTimes[i].Event, EventTimes[i].Event.Actions);
						EventTimes.Remove(EventTimes[i]);
					}
                }

			}
				

		}


		public static void SaveData() {

			//TODO: Serialize Event Data

			//TODO: Use Following Method To Save Data: Settings.SavedData.UpdateData(serializedEventData, ref Settings.SavedData.EventData);

		}

		public static void UnloadData() {

			//Unregister Any Actions/Events That Were Registered in Setup()
			MES_SessionCore.SaveActions -= SaveData;
			MES_SessionCore.UnloadActions -= UnloadData;
			TaskProcessor.Tick30.Tasks -= ProcessEvents;
		}

	}
}
