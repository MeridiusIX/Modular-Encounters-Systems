using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModularEncountersSystems.Logging {
	public static class LoggerTools {

		public static string BuildKeyList(string profileType, IEnumerable<string> stringList) {

			var sb = new StringBuilder();
			sb.Append("Detected Profiles: " + profileType).AppendLine();

			foreach (var subtypeName in stringList.OrderBy(x => x)) {

				sb.Append(" - ").Append(subtypeName).AppendLine();

			}

			sb.AppendLine();
			return sb.ToString();

		}

		public static void AppendDateAndTime(StringBuilder sb) {

			DateTime time = DateTime.Now;
			sb.Append(time.ToString("yyyy-MM-dd hh-mm-ss-fff")).Append(": ");

		}

		public static string GetLogging(string[] command, ref string returnMsg) {

			if (command.Length < 5) {

				returnMsg = "Command Missing Parameters.";
				return "";
			
			}

			string result = "";

			if (command[3] == "SpawnDebug") {

				if (command[4] == "API")
					result = SpawnLogger.API.ToString();

				if (command[4] == "BlockLogic")
					result = SpawnLogger.BlockLogic.ToString();

				if (command[4] == "CleanUp")
					result = SpawnLogger.CleanUp.ToString();

				if (command[4] == "Entity")
					result = SpawnLogger.Entity.ToString();

				if (command[4] == "Error")
					result = SpawnLogger.Error.ToString();

				if (command[4] == "Manipulation")
					result = SpawnLogger.Manipulation.ToString();

				if (command[4] == "Pathing")
					result = SpawnLogger.Pathing.ToString();

				if (command[4] == "PostSpawn")
					result = SpawnLogger.PostSpawn.ToString();

				if (command[4] == "Settings")
					result = SpawnLogger.Settings.ToString();

				if (command[4] == "SpawnGroup")
					result = SpawnLogger.SpawnGroup.ToString();

				if (command[4] == "Spawning")
					result = SpawnLogger.Spawning.ToString();

				if (command[4] == "Startup")
					result = SpawnLogger.Startup.ToString();

				if (command[4] == "Zone")
					result = SpawnLogger.Zone.ToString();


			} else if (command[3] == "BehaviorDebug") {
			
				
			
			}

			returnMsg = !string.IsNullOrWhiteSpace(result) ? "Logging Info Sent To Clipboard." : "Logging Info Not Found For Provided Type.";
			return result;
		
		}

	}
}
