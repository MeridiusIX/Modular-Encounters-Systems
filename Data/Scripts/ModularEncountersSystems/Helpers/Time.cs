using ModularEncountersSystems.Logging;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;

namespace ModularEncountersSystems.Helpers {

	public class Time {

		public static DateTime GetRealIngameTime()
		{
			DateTime GameDateTime = MyAPIGateway.Session.GameDateTime;
			float SunRotationIntervalSeconds = MyAPIGateway.Session.SessionSettings.SunRotationIntervalMinutes * 60;

			float DaysElipsedRaw = (float)((GameDateTime - new DateTime(2081, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds / SunRotationIntervalSeconds);
			int DaysElipsed = (int)Math.Floor(DaysElipsedRaw);
			float TimeRatioUTC = DaysElipsedRaw - DaysElipsed;

			return new DateTime(2081, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddDays(DaysElipsed).AddMilliseconds(TimeRatioUTC * 86400000);
		}
				
		
	

	}

}
