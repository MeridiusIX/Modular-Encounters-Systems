using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Events {
	public static class EventManager {

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

		public static void SaveData() {

			//TODO: Serialize Event Data

			//TODO: Use Following Method To Save Data: Settings.SavedData.UpdateData(serializedEventData, ref Settings.SavedData.EventData);

		}

		public static void ProcessEvents() {
			
			//Loop Events

				//Check Event is On

				//Check Times

				//Check Conditions

				//If Satisfied, Set Triggered = true

			//Loop Events Again
				
				//If Triggered == false, Skip

				//Run Actions

				//Set Triggered = false
		
		}

		public static void UnloadData() {

			//Unregister Any Actions/Events That Were Registered in Setup()
			MES_SessionCore.SaveActions -= SaveData;
			MES_SessionCore.UnloadActions -= UnloadData;
			TaskProcessor.Tick30.Tasks -= ProcessEvents;

		}

	}
}
