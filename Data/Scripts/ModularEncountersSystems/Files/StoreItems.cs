using ModularEncountersSystems.Helpers;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions;

namespace ModularEncountersSystems.Files {

	public enum StoreItemTypes {
	
		None,
		Ore,
		Ingot,
		Component,
		Ammo,
		Tool,
		Oxygen,
		Hydrogen,
		Prefab,
	
	}

	public struct StoreItemsContainer {

		public StoreItem[] StoreItems;

	}

	public struct StoreItem {

		public string StoreItemId;
		public StoreItemTypes ItemType;
		public string ItemSubtypeId;
		public ulong RequiredMod;

		public int Frequency;

		public StoreItemValues Offer;
		public StoreItemValues Order;

		public long GetPrice(float customMultiplier = 100, bool offer = true, string overrideSubtypeId = null) {

			if (ItemType == StoreItemTypes.None)
				return 0;

			long price = 0;
			var itemValue = offer ? Offer : Order;
			var id = GetItemId(overrideSubtypeId != null ? overrideSubtypeId : ItemSubtypeId);

			if (!EconomyHelper.MinimumValuesMaster.TryGetValue(id, out price)) {

				if (ItemType == StoreItemTypes.Prefab) {

					price = EconomyHelper.CalculatePrefabCost(id.SubtypeName);

				}

			}

			if (price == 0)
				return 0;

			if (itemValue.CustomPrice > -1)
				price = itemValue.CustomPrice;

			//Apply Custom Multiplier
			price = (long)Math.Floor(price * (customMultiplier / 100));

			//Apply Sandbox Multiplier
			if (!string.IsNullOrWhiteSpace(itemValue.SandboxCounterMultiplier)) {

				int sandboxCounter = 100;

				if (!MyAPIGateway.Utilities.GetVariable<int>(itemValue.SandboxCounterMultiplier, out sandboxCounter))
					sandboxCounter = 100;

				price = (long)Math.Floor((float)price * (float)(sandboxCounter / 100));

			}

			//Apply Random Multiplier
			price = (long)Math.Floor(price * ((float)MathTools.RandomBetween(itemValue.MinPriceMultiplier, itemValue.MaxPriceMultiplier) / 100));

			return price;

		}

		internal MyDefinitionId GetItemId(string subtypeId) {

			Type type = null;

			if (ItemType == StoreItemTypes.Hydrogen)
				return new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Hydrogen");

			if (ItemType == StoreItemTypes.Oxygen)
				return new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Oxygen");

			if (ItemType == StoreItemTypes.None)
				type = typeof(MyObjectBuilder_DefinitionBase);

			if (ItemType == StoreItemTypes.Ammo)
				type = typeof(MyObjectBuilder_AmmoMagazine);

			if (ItemType == StoreItemTypes.Ore)
				type = typeof(MyObjectBuilder_AmmoMagazine);

			if (ItemType == StoreItemTypes.Ingot)
				type = typeof(MyObjectBuilder_AmmoMagazine);

			if (ItemType == StoreItemTypes.Component)
				type = typeof(MyObjectBuilder_AmmoMagazine);

			if (ItemType == StoreItemTypes.Tool)
				type = typeof(MyObjectBuilder_PhysicalGunObject);

			if (ItemType == StoreItemTypes.Prefab)
				type = typeof(MyObjectBuilder_PrefabDefinition);

			return new MyDefinitionId(type, ItemSubtypeId);

		}

	}

	public struct StoreItemValues {

		public long CustomPrice;

		public short MinPriceMultiplier;
		public short MaxPriceMultiplier;

		public string SandboxCounterMultiplier;

	}

}
