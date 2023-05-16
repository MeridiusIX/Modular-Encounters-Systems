using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions;

namespace ModularEncountersSystems.Files {

	public enum StoreProfileItemTypes {
	
		None,
		Ore,
		Ingot,
		Component,
		Ammo,
		Tool,
		Consumable,
		RandomCraftable,
		RandomItem,
		Oxygen,
		Hydrogen,
		Prefab,
	
	}

	public class StoreItemsContainer {

		[XmlArrayItem("StoreItem")]	public StoreItem[] StoreItems;
		public List<StoreItem> StoreItemsList;

		[XmlArrayItem("ItemLimits")] public StoreLimits[] StoreItemLimits;

	}

	public class StoreLimits {

		public int MaxOreAmount = -1;
		public int MaxOreValue = -1;

		public int MaxIngotAmount = -1;
		public int MaxIngotValue = -1;

		public int MaxComponentAmount = -1;
		public int MaxComponentValue = -1;

		public int MaxToolAmount = -1;
		public int MaxToolValue = -1;

		public int MaxConsumableAmount = -1;
		public int MaxConsumableValue = -1;

		public int MaxGasAmount = -1;
		public int MaxGasValue = -1;

		public int MaxAmmoAmount = -1;
		public int MaxAmmoValue = -1;

		public StoreLimits() {

			MaxOreAmount = -1;
			MaxOreValue = -1;

			MaxIngotAmount = -1;
			MaxIngotValue = -1;

			MaxComponentAmount = -1;
			MaxComponentValue = -1;

			MaxToolAmount = -1;
			MaxToolValue = -1;

			MaxConsumableAmount = -1;
			MaxConsumableValue = -1;

			MaxGasAmount = -1;
			MaxGasValue = -1;

			MaxAmmoAmount = -1;
			MaxAmmoValue = -1;

		}

	}

	public class StoreItem {

		public string StoreItemId;
		public StoreProfileItemTypes ItemType;
		public string ItemSubtypeId;
		public ulong RequiredMod;
		public int PCU;
		public bool IsCustomItem;

		[XmlArrayItem("ItemSubtypeId")] public string[] ItemSubtypeIds;

		public StoreItemValues Offer;
		public StoreItemValues Order;

		public StoreItem() {

			StoreItemId = "";
			ItemType = StoreProfileItemTypes.None;
			ItemSubtypeId = "";
			RequiredMod = 0;
			PCU = 0;
			IsCustomItem = true;

			Offer = new StoreItemValues();
			Order = new StoreItemValues();

		}

		public StoreItem(string subtypeId, StoreItem parentItem) {

			ItemType = parentItem.ItemType;
			ItemSubtypeId = subtypeId;
			RequiredMod = parentItem.RequiredMod;
			PCU = parentItem.PCU;
			IsCustomItem = parentItem.IsCustomItem;

			Offer = parentItem.Offer;
			Order = parentItem.Order;

			StoreItemId =  ItemType.ToString() + "/" + ItemSubtypeId;

		}

		public int GetAmount(bool offer) {

			//BehaviorLogger.Write("Offer Min Amount: " + Offer.MinAmount.ToString(), BehaviorDebugEnum.Action);
			//BehaviorLogger.Write("Offer Min Amount: " + Offer.MaxAmount.ToString(), BehaviorDebugEnum.Action);

			if (offer)
				return MathTools.RandomBetween(Offer.MinAmount, Offer.MaxAmount + 1);
			return MathTools.RandomBetween(Order.MinAmount, Order.MaxAmount + 1);

		}

		public long GetPrice(float customMultiplier = 100, bool offer = true, string overrideSubtypeId = null, MyDefinitionId? overrideId = null) {

			if (ItemType == StoreProfileItemTypes.None)
				return 0;

			long price = 0;
			var itemValue = offer ? Offer : Order;

			var id = overrideId.HasValue ? overrideId.Value : GetItemId(overrideSubtypeId != null ? overrideSubtypeId : ItemSubtypeId);

			BehaviorLogger.Write("Random Id For Price: " + id.ToString() + " // Override Has Value: " + overrideId.HasValue, BehaviorDebugEnum.Action);

			if (!EconomyHelper.MinimumValuesMaster.TryGetValue(id, out price)) {

				if (ItemType == StoreProfileItemTypes.Prefab) {

					price = EconomyHelper.CalculatePrefabCost(id.SubtypeName);

				}

			}

			if (price == 0 && itemValue.CustomPrice <= -1)
				return 0;

			if (itemValue.CustomPrice > -1)
				price = itemValue.CustomPrice;

			//Apply Custom Multiplier
			price = (long)MathTools.ApplyNonFloatMultiplier(price, customMultiplier);

			//Apply Sandbox Multiplier
			if (!string.IsNullOrWhiteSpace(itemValue.SandboxCounterMultiplier)) {

				int sandboxCounter = 100;

				if (!MyAPIGateway.Utilities.GetVariable<int>(itemValue.SandboxCounterMultiplier, out sandboxCounter))
					sandboxCounter = 100;

				price = (long)MathTools.ApplyNonFloatMultiplier(price, sandboxCounter);

			}

			//Apply Random Multiplier
			price = (long)Math.Floor(MathTools.ApplyNonFloatMultiplier(price, MathTools.RandomBetween(itemValue.MinPriceMultiplier, itemValue.MaxPriceMultiplier)));

			return price;

		}

		public MyDefinitionId GetItemId() {

			return GetItemId(ItemSubtypeId);

		}

		internal MyDefinitionId GetItemId(string subtypeId) {

			Type type = null;

			if (ItemType == StoreProfileItemTypes.Hydrogen)
				return new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Hydrogen");

			if (ItemType == StoreProfileItemTypes.Oxygen)
				return new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Oxygen");

			if (ItemType == StoreProfileItemTypes.None)
				type = typeof(MyObjectBuilder_DefinitionBase);

			if (ItemType == StoreProfileItemTypes.Ammo)
				type = typeof(MyObjectBuilder_AmmoMagazine);

			if (ItemType == StoreProfileItemTypes.Ore)
				type = typeof(MyObjectBuilder_Ore);

			if (ItemType == StoreProfileItemTypes.Ingot)
				type = typeof(MyObjectBuilder_Ingot);

			if (ItemType == StoreProfileItemTypes.Component)
				type = typeof(MyObjectBuilder_Component);

			if (ItemType == StoreProfileItemTypes.Tool)
				type = typeof(MyObjectBuilder_PhysicalGunObject);

			if (ItemType == StoreProfileItemTypes.Prefab)
				type = typeof(MyObjectBuilder_PrefabDefinition);

			if (ItemType == StoreProfileItemTypes.Consumable)
				type = typeof(MyObjectBuilder_ConsumableItem);

			return new MyDefinitionId(type, subtypeId);

		}

	}

	public class StoreItemValues {

		public long CustomPrice;

		public short MinPriceMultiplier;
		public short MaxPriceMultiplier;

		public int MinAmount;
		public int MaxAmount;

		public string SandboxCounterMultiplier;

		public StoreItemValues() {

			CustomPrice = -1;

			MinPriceMultiplier = 100;
			MaxPriceMultiplier = 100;

			MinAmount = 1;
			MaxAmount = 1;

			SandboxCounterMultiplier = "";

		}

	}

}
