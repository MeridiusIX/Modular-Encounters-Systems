using ModularEncountersSystems.Spawning.Profiles;
using ModularEncountersSystems.Zones;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace ModularEncountersSystems.Spawners {

	public static class SpawnResources{

		public static DateTime GameStartTime = DateTime.Now;

		public static Random rnd = new Random();
		
		
		
		

		

		
		
		

		

		/*
		public static Water GetWaterAtPlanet(MyPlanet planet) {

			if (!MES_SessionCore.Instance.WaterMod.Registered)
				return null;

			for (int j = MES_SessionCore.Instance.WaterMod.Waters.Count - 1; j >= 0; j++) {

				if (j >= MES_SessionCore.Instance.WaterMod.Waters.Count)
					continue;

				var water = MES_SessionCore.Instance.WaterMod.Waters[j];

				if (water.planetID != planet.EntityId)
					continue;

				return water;

			}

			return null;

		}
		*/

		public static bool IsNight(Vector3D coords) {

			//Get Planet
			MyPlanet myPlanet = MyGamePruningStructure.GetClosestPlanet(coords);
			if (myPlanet == null)
				return false;

			return MyVisualScriptLogicProvider.IsOnDarkSide(myPlanet, coords);

		}

		
		
		



		
		
	}
	
}