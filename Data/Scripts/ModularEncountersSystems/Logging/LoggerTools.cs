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

	}
}
