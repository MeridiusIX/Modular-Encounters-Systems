using ModularEncountersSystems.Logging;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.ModAPI;

namespace ModularEncountersSystems.Helpers {
	public static class SerializationHelper {

		public static T ConvertClassFromString<T>(string data) where T : class {

			byte[] byteData = Convert.FromBase64String(data);

			if (byteData == null) {

				return null;

			}
				
			return MyAPIGateway.Utilities.SerializeFromBinary<T>(byteData);

		}

		public static string ConvertClassToString<T>(T classObject) where T : class {

			var byteData = MyAPIGateway.Utilities.SerializeToBinary<T>(classObject);
			return Convert.ToBase64String(byteData);

		}

		public static T GetDataFromEntity<T>(IMyEntity entity, Guid guid) where T : class {

			if (entity?.Storage == null)
				return null;

			string stringData = null;

			if (!entity.Storage.TryGetValue(guid, out stringData))
				return null;

			return ConvertClassFromString<T>(stringData);

		}

		public static T GetDataFromSandbox<T>(string name) where T : class {

			string stringData = null;

			if (!MyAPIGateway.Utilities.GetVariable<string>(name, out stringData))
				return null;

			byte[] byteData = Convert.FromBase64String(stringData);

			if (byteData == null)
				return null;

			return MyAPIGateway.Utilities.SerializeFromBinary<T>(byteData);

		}

		public static void SaveDataToEntity<T>(IMyEntity entity, T saveData, Guid guid) where T : class {

			if (entity == null)
				return;

			if (entity.Storage == null)
				entity.Storage = new MyModStorageComponent();

			var byteData = MyAPIGateway.Utilities.SerializeToBinary<T>(saveData);
			var stringData = Convert.ToBase64String(byteData);

			if (stringData == null) {

				//SpawnLogger.Write("Failed To Convert Class To String While Saving To Entity", SpawnerDebugEnum.Error, true);
				return;

			}

			if (entity.Storage.ContainsKey(guid)) {

				entity.Storage[guid] = stringData;
			
			} else {

				entity.Storage.Add(guid, stringData);
			
			}
		
		}

		public static void SaveDataToSandbox<T>(string name, T data) where T : class {

			var byteData = MyAPIGateway.Utilities.SerializeToBinary<T>(data);
			var stringData = Convert.ToBase64String(byteData);
			MyAPIGateway.Utilities.SetVariable<string>(name, stringData);
		
		}

	}

}
