using ModularEncountersSystems.Core;
using ModularEncountersSystems.Spawning.Procedural.Builder;
using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;

namespace ModularEncountersSystems.Spawning.Procedural {

	public enum ShipBuildingStep {

		None,

	}
	public static class ProceduralShipManager {

		public static bool Ready = false;

		public static void Setup() {

			BuilderTools.Setup();

			MES_SessionCore.UnloadActions += UnloadData;
		
		}

		public static void GenerateShip(ShipRules rules) {
		
			//Create ObjectBuilder

			//Calculate Grid Max Sizes

			//Determine Thrust Type

			//Insert Prefab Interiors

			//Determine Hull Type

			//Initial Outline Placements

			//Reserve Space For Nacelles if Applicable
		
		}

		public static void UnloadData() {
		
			
		
		}

	}

}
