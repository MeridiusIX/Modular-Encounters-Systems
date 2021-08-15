using ModularEncountersSystems.Core;
using ModularEncountersSystems.Entities;
using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Collections;
using VRage.Game;

namespace ModularEncountersSystems.Helpers {
	public static class DefinitionHelper {

		//All
		public static DictionaryValuesReader<MyDefinitionId, MyDefinitionBase> AllDefinitions = new DictionaryValuesReader<MyDefinitionId, MyDefinitionBase>();

		//Blocks
		public static List<MyCubeBlockDefinition> AllBlockDefinitions = new List<MyCubeBlockDefinition>();
		public static Dictionary<MyDefinitionId, float> BatteryMaxCapacityReference = new Dictionary<MyDefinitionId, float>();
		public static Dictionary<MyDefinitionId, MyReactorDefinition> ReactorDefinitions = new Dictionary<MyDefinitionId, MyReactorDefinition>();
		public static List<string> RivalAiControlModules = new List<string>();

		public static Dictionary<MyDefinitionId, MyWeaponBlockDefinition> WeaponBlockReferences = new Dictionary<MyDefinitionId, MyWeaponBlockDefinition>();
		public static List<MyDefinitionId> WeaponBlockIDs = new List<MyDefinitionId>();

		//Items
		public static Dictionary<MyDefinitionId, MyPhysicalItemDefinition> AllItemDefinitions = new Dictionary<MyDefinitionId, MyPhysicalItemDefinition>();

		public static Dictionary<MyDefinitionId, MyAmmoMagazineDefinition> NormalAmmoMagReferences = new Dictionary<MyDefinitionId, MyAmmoMagazineDefinition>();
		public static Dictionary<MyDefinitionId, MyAmmoDefinition> NormalAmmoReferences = new Dictionary<MyDefinitionId, MyAmmoDefinition>();

		public static Dictionary<MyDefinitionId, List<MyDefinitionId>> WeaponCoreAmmoReferences = new Dictionary<MyDefinitionId, List<MyDefinitionId>>();

		//Entity Components
		public static List<MyComponentDefinitionBase> EntityComponentDefinitions = new List<MyComponentDefinitionBase>();

		//Weight
		public static Dictionary<MyDefinitionId, float> ItemWeightReference = new Dictionary<MyDefinitionId, float>();
		public static Dictionary<MyDefinitionId, float> BlockWeightReference = new Dictionary<MyDefinitionId, float>();

		//Volume
		public static Dictionary<MyDefinitionId, float> ItemVolumeReference = new Dictionary<MyDefinitionId, float>();
		public static Dictionary<MyDefinitionId, float> WeaponVolumeReference = new Dictionary<MyDefinitionId, float>();

		//Grids
		public static List<string> DropContainerNames = new List<string>();


		public static void Setup() {

			AllDefinitions = MyDefinitionManager.Static.GetAllDefinitions();

			//Items
			var physicalItems = MyDefinitionManager.Static.GetPhysicalItemDefinitions();

			foreach (var item in physicalItems) {

				//Main Item
				if (!AllItemDefinitions.ContainsKey(item.Id))
					AllItemDefinitions.Add(item.Id, item);

				//Weight
				if (!ItemWeightReference.ContainsKey(item.Id))
					ItemWeightReference.Add(item.Id, item.Mass);

				//Volume
				if (!ItemVolumeReference.ContainsKey(item.Id))
					ItemVolumeReference.Add(item.Id, item.Volume);

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

			//Blocks
			foreach (var def in AllDefinitions) {

				var block = def as MyCubeBlockDefinition;

				if (block != null) {

					AllBlockDefinitions.Add(block);

					if (!BlockWeightReference.ContainsKey(block.Id)) {

						float totalWeight = 0;

						foreach (var comp in block.Components) {

							float weight = 0;

							if (ItemWeightReference.TryGetValue(comp.Definition.Id, out weight)) {

								totalWeight = weight * comp.Count;

							}
						
						}

						BlockWeightReference.Add(block.Id, totalWeight);

					}

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

						if (!WeaponBlockReferences.ContainsKey(weapon.Id)) {

							WeaponBlockReferences.Add(weapon.Id, weapon);
							WeaponBlockIDs.Add(weapon.Id);

						}
							

						if (!WeaponVolumeReference.ContainsKey(weapon.Id))
							WeaponVolumeReference.Add(weapon.Id, weapon.InventoryMaxVolume);

						continue;

					}

					//Weapon-Sorter
					var weaponSorter = block as MyConveyorSorterDefinition;

					if (weaponSorter != null && BlockManager.AllWeaponCoreBlocks.Contains(block.Id)) {

						if (!WeaponBlockIDs.Contains(weaponSorter.Id))
							WeaponBlockIDs.Add(weaponSorter.Id);

						if (!WeaponVolumeReference.ContainsKey(weaponSorter.Id))
							WeaponVolumeReference.Add(weaponSorter.Id, weaponSorter.InventorySize.X * weaponSorter.InventorySize.Y * weaponSorter.InventorySize.Z);

						continue;

					}

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

			//DropPods
			var containers = MyDefinitionManager.Static.GetDropContainerDefinitions();

			foreach (var container in containers.Keys) {

				MyDropContainerDefinition drop = null;

				if (!containers.TryGetValue(container, out drop))
					continue;

				if (drop.Prefab?.CubeGrids == null)
					continue;

				if (drop.Prefab.CubeGrids.Length == 0)
					continue;

				if (string.IsNullOrWhiteSpace(drop.Prefab.CubeGrids[0].DisplayName))
					continue;

				if (!DropContainerNames.Contains(drop.Prefab.CubeGrids[0].DisplayName))
					DropContainerNames.Add(drop.Prefab.CubeGrids[0].DisplayName);
			
			}

			MES_SessionCore.UnloadActions += Unload;

		}

		public static string GetBlockDefinitionInfo() {

			var sb = new StringBuilder();

			foreach (var def in AllBlockDefinitions) {

				if (def == null) {

					continue;

				}

				sb.Append("Block Name: ").Append(def.DisplayNameText).AppendLine();
				sb.Append("Block ID:   ").Append(def.Id.ToString()).AppendLine();

				if (def.Context?.ModId != null) {

					if (string.IsNullOrWhiteSpace(def.Context.ModId) == false) {

						sb.Append("Mod ID:     ").Append(def.Context.ModId).AppendLine();

					}

				}

				sb.Append("Is Public:  ").Append(def.Public.ToString()).AppendLine();
				sb.Append("Size:       ").Append(def.CubeSize.ToString()).AppendLine();
				sb.Append("Is Weapon:  ").Append(WeaponBlockIDs.Contains(def.Id)).AppendLine();

				sb.AppendLine();

			}

			return sb.ToString();

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
