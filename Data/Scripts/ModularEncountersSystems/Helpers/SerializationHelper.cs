using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Helpers {
	public static class SerializationHelper {

		//Write This For Saved String Lists

		public static void SaveDataToSandbox<T>(string name, T data) where T : class {

			var byteData = MyAPIGateway.Utilities.SerializeToBinary<T>(data);
			var stringData = Convert.ToBase64String(byteData);
			MyAPIGateway.Utilities.SetVariable<string>(name, stringData);
		
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

		public static T ConvertClassFromString<T>(string data) where T : class {

			byte[] byteData = Convert.FromBase64String(data);

			if (byteData == null)
				return null;

			return MyAPIGateway.Utilities.SerializeFromBinary<T>(byteData);

		}

		public static string ConvertClassToString<T>(T classObject) where T : class {

			var byteData = MyAPIGateway.Utilities.SerializeToBinary<T>(classObject);
			return Convert.ToBase64String(byteData);

		}

	}

}
