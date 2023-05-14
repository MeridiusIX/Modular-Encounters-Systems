using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Mission {
	public abstract class BaseMission {

		//Main IDs
		public long OriginEntityID;
		public long AssignedPlayerIdentity;

		//Inventory
		public long DestinationInventoryEntityID;

		//Target IDs
		public List<long> TargetEntityIDs;



	}

}
