using ModularEncountersSystems.Core;
using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;

namespace ModularEncountersSystems.Helpers {
	public static class DefinitionHelper {

		//Blocks
		public static List<MyCubeBlockDefinition> AllBlockDefinitions = new List<MyCubeBlockDefinition>();
		public static Dictionary<MyDefinitionId, float> BatteryMaxCapacityReference = new Dictionary<MyDefinitionId, float>();
		public static Dictionary<MyDefinitionId, MyReactorDefinition> ReactorDefinitions = new Dictionary<MyDefinitionId, MyReactorDefinition>();
		public static List<string> RivalAiControlModules = new List<string>();

		public static Dictionary<MyDefinitionId, MyWeaponBlockDefinition> WeaponBlockReferences = new Dictionary<MyDefinitionId, MyWeaponBlockDefinition>();

		//Items
		public static Dictionary<MyDefinitionId, MyPhysicalItemDefinition> AllItemDefinitions = new Dictionary<MyDefinitionId, MyPhysicalItemDefinition>();

		public static Dictionary<MyDefinitionId, MyAmmoMagazineDefinition> NormalAmmoMagReferences = new Dictionary<MyDefinitionId, MyAmmoMagazineDefinition>();
		public static Dictionary<MyDefinitionId, MyAmmoDefinition> NormalAmmoReferences = new Dictionary<MyDefinitionId, MyAmmoDefinition>();


		//Entity Components
		public static List<MyComponentDefinitionBase> EntityComponentDefinitions = new List<MyComponentDefinitionBase>();



		public static void Setup() {

			var defs = MyDefinitionManager.Static.GetAllDefinitions();

			//Blocks
			foreach (var def in defs) {

				var block = def as MyCubeBlockDefinition;

				if (block != null) {

					AllBlockDefinitions.Add(block);

					//Battery Max Capacity
					var battery = block as MyBatteryBlockDefinition;

					if (battery != null) {

						if (!BatteryMaxCapacityReference.ContainsKey(battery.Id))
							BatteryMaxCapacityReference.Add(battery.Id, battery.MaxStoredPower);

						continue;

					}

					//Weapon
					var weapon = block as MyWeaponBlockDefinition;

					if (weapon != null) {

						if (!WeaponBlockReferences.ContainsKey(weapon.Id))
							WeaponBlockReferences.Add(weapon.Id, weapon);

						continue;

					}


				}

			}

			//Items
			var physicalItems = MyDefinitionManager.Static.GetPhysicalItemDefinitions();

			foreach (var item in physicalItems) {

				if (!AllItemDefinitions.ContainsKey(item.Id))
					AllItemDefinitions.Add(item.Id, item);

				//Ammo
				if (item as MyAmmoMagazineDefinition != null) {

					var ammoMag = item as MyAmmoMagazineDefinition;

					if (!NormalAmmoMagReferences.ContainsKey(item.Id))
						NormalAmmoMagReferences.Add(item.Id, ammoMag);

					var ammo = MyDefinitionManager.Static.GetAmmoDefinition(ammoMag.AmmoDefinitionId);

					if (ammo != null && !NormalAmmoReferences.ContainsKey(item.Id))
						NormalAmmoReferences.Add(item.Id, ammo);

				}

			}

			//Entity Components
			var entityComps = MyDefinitionManager.Static.GetEntityComponentDefinitions();

			foreach (var comp in entityComps) {

				if (comp != null && !string.IsNullOrWhiteSpace(comp.DescriptionText)) {

					EntityComponentDefinitions.Add(comp);

				}

			}

			//Build List of RivalAI Control Module SubtypeNames
			RivalAiControlModules.Add("RivalAIRemoteControlSmall");
			RivalAiControlModules.Add("RivalAIRemoteControlLarge");
			RivalAiControlModules.Add("K_Imperial_Dropship_Guild_RC");
			RivalAiControlModules.Add("K_TIE_Fighter_RC");
			RivalAiControlModules.Add("K_NewRepublic_EWing_RC");
			RivalAiControlModules.Add("K_Imperial_RC_Largegrid");
			RivalAiControlModules.Add("K_TIE_Drone_Core");
			RivalAiControlModules.Add("K_Imperial_SpeederBike_FakePilot");
			RivalAiControlModules.Add("K_Imperial_ProbeDroid_Top_II");
			RivalAiControlModules.Add("K_Imperial_DroidCarrier_DroidBrain");
			RivalAiControlModules.Add("K_Imperial_DroidCarrier_DroidBrain_Aggressor");

			MES_SessionCore.UnloadActions += Unload;

		}

		public static void UnlockNpcBlocks() {

			foreach (var block in AllBlockDefinitions) {

				if (!string.IsNullOrWhiteSpace(block?.Context?.ModId)) {

					if (block.Context.ModId.Contains("1521905890") || block.Context.ModId.Contains("Modular Encounters Systems")) {

						block.Public = true;
					
					}
				
				}
			
			}
		
		}

		public static void Unload() {
		
			
		
		}

	}
}
