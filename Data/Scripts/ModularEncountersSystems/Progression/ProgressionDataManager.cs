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

namespace ModularEncountersSystems.Progression {

	public static class ProgressionDataManager {

		public static List<ProgressionDataContainer> ProgressionDataContainersList = new List<ProgressionDataContainer>();
		private static string _saveProgressionDataContainersListName = "MES-ProgressionDataContainersList";

		public static void Setup() {

			//SpawnLogger.Write("Start", SpawnerDebugEnum.Dev, true);
			//Register Any Actions/ProgressionDataContainers
			MES_SessionCore.SaveActions += SaveData;
			MES_SessionCore.UnloadActions += UnloadData;

			string ProgressionDataContainersListString = "";

			if (MyAPIGateway.Utilities.GetVariable<string>(_saveProgressionDataContainersListName, out ProgressionDataContainersListString)) {

				var ProgressionDataContainersListSerialized = Convert.FromBase64String(ProgressionDataContainersListString);
				ProgressionDataContainersList = MyAPIGateway.Utilities.SerializeFromBinary<List<ProgressionDataContainer>>(ProgressionDataContainersListSerialized);
			}

			if (ProgressionDataContainersList == null)
				ProgressionDataContainersList = new List<ProgressionDataContainer>();


			//SpawnLogger.Write("Save Data", SpawnerDebugEnum.Dev, true);
			//Save Current Data
			SaveData();
		}



		public static void SaveData() {

			//ProgressionDataContainers
			var ProgressionDataContainersListSerialized = MyAPIGateway.Utilities.SerializeToBinary<List<ProgressionDataContainer>>(ProgressionDataContainersList);
			var ProgressionDataContainersListString = Convert.ToBase64String(ProgressionDataContainersListSerialized);
			MyAPIGateway.Utilities.SetVariable<string>(_saveProgressionDataContainersListName, ProgressionDataContainersListString);

		}

		public static void UnloadData() {
			//Unregister Any Actions/ProgressionDataContainers That Were Registered in Setup()
			MES_SessionCore.SaveActions -= SaveData;
			MES_SessionCore.UnloadActions -= UnloadData;

		}

	}

}
