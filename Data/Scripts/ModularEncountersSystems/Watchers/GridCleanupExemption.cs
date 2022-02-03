using ModularEncountersSystems.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Watchers {
	public class GridCleanupExemption {

		public GridEntity Grid;
		public DateTime StartTime;
		public int Duration;

		public GridCleanupExemption(GridEntity grid, int duration) {

			Grid = grid;
			StartTime = MyAPIGateway.Session.GameDateTime;
			Duration = duration;
		
		}

	}

}
