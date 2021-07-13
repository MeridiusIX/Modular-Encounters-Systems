using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRageMath;

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

		public static string GetGridMatrixInfo(ChatMessage message) {

			var line = new LineD(message.CameraPosition, message.CameraDirection * 400 + message.CameraPosition);
			GridEntity thisGrid = null;

			foreach (var grid in GridManager.Grids) {

				if (!grid.ActiveEntity())
					continue;

				if (!grid.CubeGrid.WorldAABB.Intersects(ref line))
					continue;

				thisGrid = grid;
				break;

			}

			if (thisGrid == null) {

				message.ReturnMessage = "Could Not Locate Grid At Player Camera Position.";
				return "";

			}

			var sb = new StringBuilder();

			sb.Append("Grid Name:          ").Append(thisGrid.CubeGrid.CustomName).AppendLine().AppendLine();

			sb.Append("Tags For SpawnGroup:").AppendLine();
			sb.Append("[StaticEncounterCoords:{").Append(thisGrid.CubeGrid.WorldMatrix.Translation).Append("}]").AppendLine();
			sb.Append("[StaticEncounterForward:{").Append(thisGrid.CubeGrid.WorldMatrix.Forward).Append("}]").AppendLine();
			sb.Append("[StaticEncounterUp:{").Append(thisGrid.CubeGrid.WorldMatrix.Up).Append("}]").AppendLine().AppendLine();

			var planet = PlanetManager.GetNearestPlanet(thisGrid.CubeGrid.WorldMatrix.Translation);

			if (planet != null) {

				var up = planet.UpAtPosition(thisGrid.CubeGrid.WorldMatrix.Translation);
				var dist = planet.AltitudeAtPosition(thisGrid.CubeGrid.WorldMatrix.Translation, false);

				sb.Append("Optional Tags For Dynamic Planet Spawning:").AppendLine();
				sb.Append("[StaticEncounterUsePlanetDirectionAndAltitude:").Append("true").Append("]").AppendLine();
				sb.Append("[StaticEncounterPlanet:").Append(planet.Planet.Generator.Id.SubtypeName).Append("]").AppendLine();
				sb.Append("[StaticEncounterPlanetDirection:{").Append(up).Append("}]").AppendLine();
				sb.Append("[StaticEncounterPlanetAltitude:").Append(dist).Append("]").AppendLine();

			}

			message.ReturnMessage = "Registered SpawnGroups Sent To Clipboard.";
			return sb.ToString();

		}

	}
}
