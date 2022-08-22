using ModularEncountersSystems.Logging;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ModularEncountersSystems.Configuration {
	public class SavedInternalData {

		[XmlIgnore]
		public bool DataChanged;

		public string PlayerProgressionData;
		public string EventData;
		public string ActiveResearchPoints;

		public SavedInternalData() {

			DataChanged = false;

			PlayerProgressionData = "";
			EventData = "";

		}

		public void UpdateData(string newData, ref string oldData) {

			if (newData == oldData)
				return;

			oldData = newData;
			DataChanged = true;

		}

		public static SavedInternalData LoadSettings(string phase) {

			if (MyAPIGateway.Utilities.FileExistsInLocalStorage("SavedInternalData-" + Settings.UniqueModId + ".mes", typeof(SavedInternalData)) == true) {

				try {

					SavedInternalData config = null;
					var reader = MyAPIGateway.Utilities.ReadFileInLocalStorage("SavedInternalData-" + Settings.UniqueModId + ".mes", typeof(SavedInternalData));
					string configcontents = reader.ReadToEnd();
					config = MyAPIGateway.Utilities.SerializeFromXML<SavedInternalData>(configcontents);
					SpawnLogger.Write("Loaded Existing Settings From SavedInternalData.mes. Phase: " + phase, SpawnerDebugEnum.Startup, true);
					return config;

				} catch (Exception exc) {

					SpawnLogger.Write("ERROR: Could Not Load Settings From SavedInternalData.mes. Using Default Data. Phase: " + phase, SpawnerDebugEnum.Error, true);
					var defaultSettings = new SavedInternalData();
					return defaultSettings;

				}

			} else {

				SpawnLogger.Write("SavedInternalData.mes Doesn't Exist. Creating Default Data. Phase: " + phase, SpawnerDebugEnum.Startup, true);

			}

			var settings = new SavedInternalData();

			try {

				using (var writer = MyAPIGateway.Utilities.WriteFileInLocalStorage("SavedInternalData-" + Settings.UniqueModId + ".mes", typeof(SavedInternalData))) {

					writer.Write(MyAPIGateway.Utilities.SerializeToXML<SavedInternalData>(settings));

				}

			} catch (Exception exc) {

				SpawnLogger.Write("ERROR: Could Not Create SavedInternalData.mes. Default Data Will Be Used. Phase: " + phase, SpawnerDebugEnum.Error, true);

			}

			return settings;

		}

		public string SaveSettings() {

			DataChanged = false;

			try {

				using (var writer = MyAPIGateway.Utilities.WriteFileInLocalStorage("SavedInternalData-" + Settings.UniqueModId + ".mes", typeof(SavedInternalData))) {

					writer.Write(MyAPIGateway.Utilities.SerializeToXML<SavedInternalData>(this));

				}

				SpawnLogger.Write("Settings In SavedInternalData.mes Updated Successfully!", SpawnerDebugEnum.Settings);
				return "Settings Updated Successfully.";

			} catch (Exception exc) {

				SpawnLogger.Write("ERROR: Could Not Save To SavedInternalData.mes. Changes Will Be Lost On World Reload.", SpawnerDebugEnum.Settings);

			}

			return "Settings Changed, But Could Not Be Saved To File. Changes May Be Lost On Session Reload.";

		}

	}
}
