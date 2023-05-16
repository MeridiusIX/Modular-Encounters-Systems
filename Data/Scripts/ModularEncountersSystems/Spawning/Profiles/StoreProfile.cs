using ModularEncountersSystems.Files;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Definitions;

namespace ModularEncountersSystems.Spawning.Profiles {

	public class StoreProfile {

		public string ProfileSubtypeId;
		public bool SetupComplete;

		public string FileSource;

		public int MinOfferItems;
		public int MaxOfferItems;

		public int MinOrderItems;
		public int MaxOrderItems;

		public bool ItemsRequireInventory;

		public List<string> Offers;
		public List<string> Orders;

		public List<string> RequiredOffers;
		public List<string> RequiredOrders;

		public List<StoreItem> OfferItems;
		public List<StoreItem> OrderItems;

		public List<StoreItem> RequiredOfferItems;
		public List<StoreItem> RequiredOrderItems;

		public bool AddedItemsCombineQuantity;
		public bool AddedItemsAveragePrice;

		public bool EqualizeOffersAndOrders;

		private static StoreItemsContainer _storeContainer;
		private static StoreLimits _storeLimits;

		private static List<IMyStoreItem> _tempItems;
		private static List<int> _tempOfferIndexes;
		private static List<int> _tempOrderIndexes;
		private static IMyStoreItem _tempFirstItem;
		private static IMyStoreItem _tempSecondItem;
		private static IMyStoreItem _tempNewItem;

		private List<MyDefinitionId> _tempUniqueOffers;
		private List<MyDefinitionId> _tempUniqueOrders;

		public Dictionary<string, Action<string, object>> EditorReference;

		public StoreProfile() {

			ProfileSubtypeId = "";
			SetupComplete = false;

			FileSource = "";

			MinOfferItems = 10;
			MaxOfferItems = 10;

			MinOrderItems = 10;
			MaxOrderItems = 10;

			Offers = new List<string>();
			Orders = new List<string>();

			RequiredOffers = new List<string>();
			RequiredOrders = new List<string>();

			OfferItems = new List<StoreItem>();
			OrderItems = new List<StoreItem>();

			RequiredOfferItems = new List<StoreItem>();
			RequiredOrderItems = new List<StoreItem>();

			AddedItemsCombineQuantity = false;
			AddedItemsAveragePrice = false;

			EqualizeOffersAndOrders = false;

			_tempItems = new List<IMyStoreItem>();
			_tempOfferIndexes = new List<int>();
			_tempOrderIndexes = new List<int>();
			_tempUniqueOffers = new List<MyDefinitionId>();
			_tempUniqueOrders = new List<MyDefinitionId>();

			EditorReference = new Dictionary<string, Action<string, object>> {

				{"FileSource", (s, o) => TagParse.TagStringCheck(s, ref FileSource) },
				{"MinOfferItems", (s, o) => TagParse.TagIntCheck(s, ref MinOfferItems) },
				{"MaxOfferItems", (s, o) => TagParse.TagIntCheck(s, ref MaxOfferItems) },
				{"MinOrderItems", (s, o) => TagParse.TagIntCheck(s, ref MinOrderItems) },
				{"MaxOrderItems", (s, o) => TagParse.TagIntCheck(s, ref MaxOrderItems) },
				{"Offers", (s, o) => TagParse.TagStringListCheck(s, ref Offers) },
				{"Orders", (s, o) => TagParse.TagStringListCheck(s, ref Orders) },
				{"RequiredOffers", (s, o) => TagParse.TagStringListCheck(s, ref RequiredOffers) },
				{"RequiredOrders", (s, o) => TagParse.TagStringListCheck(s, ref RequiredOrders) },
				{"AddedItemsCombineQuantity", (s, o) => TagParse.TagBoolCheck(s, ref AddedItemsCombineQuantity) },
				{"AddedItemsAveragePrice", (s, o) => TagParse.TagBoolCheck(s, ref AddedItemsAveragePrice) },
				{"EqualizeOffersAndOrders", (s, o) => TagParse.TagBoolCheck(s, ref EqualizeOffersAndOrders) },

			};

		}

		public void Setup() {

			if (SetupComplete)
				return;

			SetupComplete = true;

			if (string.IsNullOrWhiteSpace(FileSource)) {

				SpawnLogger.Write("StoreProfile with ID [" + ProfileSubtypeId + "] Missing File Source. Cannot Link StoreItems.", SpawnerDebugEnum.Error);
				return;
			
			}

			StoreItemsContainer _storeContainer = ProfileManager.GetStoreItemContainer(FileSource);

			if (_storeContainer == null) {

				SpawnLogger.Write("StoreProfile with ID [" + ProfileSubtypeId + "] using FileSource [" + FileSource + "] returned null StoreItemContainer.", SpawnerDebugEnum.Error);
				return;

			}

			if (_storeContainer.StoreItemLimits != null && _storeContainer.StoreItemLimits.Length > 0)
				_storeLimits = _storeContainer.StoreItemLimits[0];

			SpawnLogger.Write("StoreProfile with ID [" + ProfileSubtypeId + "] Contains Store Item Count: " + _storeContainer.StoreItems.Length, SpawnerDebugEnum.Dev);

			foreach (var item in _storeContainer.StoreItems) {

				SpawnLogger.Write(item.StoreItemId, SpawnerDebugEnum.Dev);

				if (item.ItemType != StoreProfileItemTypes.RandomCraftable && item.ItemType != StoreProfileItemTypes.RandomItem && !EconomyHelper.AllItemIds.Contains(item.GetItemId()))
					if(item.ItemType != StoreProfileItemTypes.Hydrogen && item.ItemType != StoreProfileItemTypes.Oxygen && item.ItemType != StoreProfileItemTypes.Prefab)
						continue;

				if (Offers.Contains(item.StoreItemId))
					OfferItems.Add(item);

				if (Orders.Contains(item.StoreItemId))
					OrderItems.Add(item);

				if (RequiredOffers.Contains(item.StoreItemId))
					RequiredOfferItems.Add(item);

				if (RequiredOrders.Contains(item.StoreItemId))
					RequiredOrderItems.Add(item);

			}
		
		}

		public void InitTags(string customData) {

			if (string.IsNullOrWhiteSpace(customData) == false) {

				var descSplit = customData.Split('\n');

				foreach (var tag in descSplit) {

					EditValue(tag);

				}

			}

			Setup();

		}

		public void EditValue(string receivedValue) {

			var processedTag = TagParse.ProcessTag(receivedValue);

			if (processedTag.Length < 2)
				return;

			Action<string, object> referenceMethod = null;

			if (!EditorReference.TryGetValue(processedTag[0], out referenceMethod))
				//TODO: Notes About Value Not Found
				return;

			referenceMethod?.Invoke(receivedValue, null);

		}

		public void ApplyProfileToBlock(IMyStoreBlock block, bool clearExisting = true) {

			if (block == null) {

				BehaviorLogger.Write(" - Store Block Null", BehaviorDebugEnum.Action);
				return;

			}

			_tempItems.Clear();
			_tempUniqueOffers.Clear();
			_tempUniqueOrders.Clear();
			block.GetStoreItems(_tempItems);

			if (clearExisting) {

				BehaviorLogger.Write(" - Clearing Existing Items In Store", BehaviorDebugEnum.Action);

				foreach (var item in _tempItems)
					block.RemoveStoreItem(item);

				_tempItems.Clear();

			}

			PrepareTempIndexList(_tempOfferIndexes, MathTools.RandomBetween(MinOfferItems, MaxOfferItems + 1), OfferItems.Count);
			PrepareTempIndexList(_tempOrderIndexes, MathTools.RandomBetween(MinOrderItems, MaxOrderItems + 1), OrderItems.Count);

			BehaviorLogger.Write(" - Potential Offers: " + OfferItems.Count, BehaviorDebugEnum.Action);
			BehaviorLogger.Write(" - Potential Orders: " + OrderItems.Count, BehaviorDebugEnum.Action);
			BehaviorLogger.Write(" - Required Offers: " + RequiredOffers.Count + " / " + RequiredOfferItems.Count, BehaviorDebugEnum.Action);
			BehaviorLogger.Write(" - Required Orders: " + RequiredOrders.Count + " / " + RequiredOrderItems.Count, BehaviorDebugEnum.Action);
			BehaviorLogger.Write(" - Temp Offer Index Count: " + _tempOfferIndexes.Count, BehaviorDebugEnum.Action);
			BehaviorLogger.Write(" - Temp Order Index Count: " + _tempOrderIndexes.Count, BehaviorDebugEnum.Action);

			//Required Offers
			foreach (var item in RequiredOfferItems)
				AddItemToStore(block, item, true);

			//Required Orders
			foreach (var item in RequiredOrderItems)
				AddItemToStore(block, item, false);

			//Offers
			foreach (var index in _tempOfferIndexes)
				AddItemToStore(block, OfferItems[index], true);

			//Orders
			foreach (var index in _tempOrderIndexes)
				AddItemToStore(block, OrderItems[index], false);

			//Concat Duplicates
			_tempItems.Clear();
			block.GetStoreItems(_tempItems);

			foreach (var item in _tempItems) {

				if(item.Item.HasValue)
					BehaviorLogger.Write(string.Format("{0} - {1} - {2}", item.Item.Value.ToString(), item.StoreItemType.ToString(), item.Amount), BehaviorDebugEnum.Action);
				else
					BehaviorLogger.Write(string.Format("{0} - {1} - {2}", item.ItemType.ToString(), item.StoreItemType.ToString(), item.Amount), BehaviorDebugEnum.Action);
			}

			for (int i = _tempItems.Count - 1; i >= 0; i--) {

				_tempFirstItem = _tempItems[i];

				for (int j = _tempItems.Count - 1; j >= 0; j--) {
				
					_tempSecondItem = _tempItems[j];

					if (_tempFirstItem.Amount == 0)
						continue;

					if (_tempSecondItem == _tempFirstItem || _tempFirstItem.StoreItemType != _tempSecondItem.StoreItemType)
						continue;

					if (_tempFirstItem.ItemType == ItemTypes.Hydrogen || _tempFirstItem.ItemType == ItemTypes.Oxygen) {

						if (_tempFirstItem.ItemType == _tempSecondItem.ItemType) {

							//BehaviorLogger.Write(" - Gas Concat: Id" + _tempFirstItem.Id, BehaviorDebugEnum.Action);
							_tempFirstItem.Amount += _tempSecondItem.Amount;
							_tempSecondItem.Amount = 0;
							continue;

						}

					}

					if (!_tempFirstItem.Item.HasValue || !_tempSecondItem.Item.HasValue)
						continue;

					if (_tempFirstItem.Item.Value.TypeId != _tempSecondItem.Item.Value.TypeId)
						continue;

					if (_tempFirstItem.Item.Value.SubtypeId != _tempSecondItem.Item.Value.SubtypeId)
						continue;

					_tempFirstItem.Amount += _tempSecondItem.Amount;
					_tempSecondItem.Amount = 0;

				}

			}

			if (EqualizeOffersAndOrders)
				EqualizeItems();

			for (int i = _tempItems.Count - 1; i >= 0; i--) {

				_tempFirstItem = _tempItems[i];

				if ((_tempFirstItem.Amount - _tempFirstItem.RemovedAmount) <= 0)
					block.RemoveStoreItem(_tempFirstItem);

			}

			var vendingMachine = block as IMyVendingMachine;

			if (vendingMachine == null)
				return;

			vendingMachine.SelectNextItem();

		}

		private void AddItemToStore(IMyStoreBlock block, StoreItem item, bool offer) {

			float additionalAdd = 0;
			float oldPrice = 0;

			if (item.ItemType != StoreProfileItemTypes.Prefab && item.ItemType != StoreProfileItemTypes.RandomCraftable && item.ItemType != StoreProfileItemTypes.RandomItem) {

				var id = item.GetItemId();

				for (int i = _tempItems.Count - 1; i >= 0; i--) {

					if (_tempItems[i].Item.HasValue && _tempItems[i].Item.Value.SubtypeId == id.SubtypeName && _tempItems[i].Item.Value.TypeId == id.TypeId) {

						additionalAdd = AddedItemsCombineQuantity ? _tempItems[i].Amount - _tempItems[i].RemovedAmount : 0;
						oldPrice = _tempItems[i].PricePerUnit;
						_tempItems.RemoveAt(i);
						break;

					}
				
				}
			
			}

			var newItem = CreateStoreItem(block, item, offer ? StoreItemTypes.Offer : StoreItemTypes.Order, (int)additionalAdd);
			

			if (newItem.PricePerUnit <= 0) {

				BehaviorLogger.Write(" - Item Cost Zero", BehaviorDebugEnum.Action);
				return;

			}

			//BehaviorLogger.Write(" - Amount: " + (newItem.Amount), BehaviorDebugEnum.Action);
			//BehaviorLogger.Write(" - Removed Amount: " + (newItem.RemovedAmount), BehaviorDebugEnum.Action);

			newItem.IsCustomStoreItem = item.IsCustomItem;
			ApplyItemLimits(newItem);

			//BehaviorLogger.Write(" - Amount After: " + (newItem.Amount), BehaviorDebugEnum.Action);
			//BehaviorLogger.Write(" - Removed Amount: " + (newItem.RemovedAmount), BehaviorDebugEnum.Action);


			if (AddedItemsAveragePrice) {

				newItem.PricePerUnit = (int)MathTools.Average(newItem.PricePerUnit, oldPrice);

			}


			block.InsertStoreItem(newItem);
			BehaviorLogger.Write(" - Item Added", BehaviorDebugEnum.Action);

			if (!offer) {
				
				var faction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(block.OwnerId);

				if (faction != null)
					faction.RequestChangeBalance(newItem.PricePerUnit * (newItem.Amount - newItem.RemovedAmount));
				
				//MyAPIGateway.Players.RequestChangeBalance(block.OwnerId, (newItem.PricePerUnit * (newItem.Amount - newItem.RemovedAmount)));

			}
				

		}

		private IMyStoreItem CreateStoreItem(IMyStoreBlock block, StoreItem item, StoreItemTypes types, int extra) {

			var offer = types == StoreItemTypes.Offer;

			if (item.ItemType == StoreProfileItemTypes.Prefab) {

				return block.CreateStoreItem(item.ItemSubtypeId, 1, (int)item.GetPrice(offer: offer), item.PCU);
			
			}

			if (item.ItemType == StoreProfileItemTypes.Oxygen) {
			
				return block.CreateStoreItem(item.GetAmount(types == StoreItemTypes.Offer) + extra, (int)item.GetPrice(offer: offer), StoreItemTypes.Offer, ItemTypes.Oxygen);

			}

			if (item.ItemType == StoreProfileItemTypes.Hydrogen) {

				return block.CreateStoreItem(item.GetAmount(types == StoreItemTypes.Offer) + extra, (int)item.GetPrice(offer: offer), StoreItemTypes.Offer, ItemTypes.Hydrogen);

			}

			if (item.ItemType == StoreProfileItemTypes.RandomCraftable) {

				var unique = types == StoreItemTypes.Offer ? _tempUniqueOffers : _tempUniqueOrders;
				var id = RandomCraftableItemId(item, unique);

				if (id.HasValue) {

					
					unique.Add(id.Value);
					return block.CreateStoreItem(id.Value, item.GetAmount(types == StoreItemTypes.Offer) + extra, (int)item.GetPrice(offer: offer, overrideId: id), types);

				}
					

			}

			if (item.ItemType == StoreProfileItemTypes.RandomItem) {

				var unique = types == StoreItemTypes.Offer ? _tempUniqueOffers : _tempUniqueOrders;
				var id = RandomItemId(item, unique);

				BehaviorLogger.Write("ID Random: " + id.ToString(), BehaviorDebugEnum.Action);

				if (id.HasValue) {

					unique.Add(id.Value);
					return block.CreateStoreItem(id.Value, item.GetAmount(types == StoreItemTypes.Offer) + extra, (int)item.GetPrice(offer: offer, overrideId: id), types);

				}
					

			}

			if (item.ItemType == StoreProfileItemTypes.None) {

				return null;
			
			}

			return block.CreateStoreItem(item.GetItemId(), item.GetAmount(types == StoreItemTypes.Offer) + extra, (int)item.GetPrice(offer: offer), types);

		}

		private void EqualizeItems() {

			for (int i = _tempItems.Count - 1; i >= 0; i--) {

				_tempFirstItem = _tempItems[i];

				for (int j = _tempItems.Count - 1; j >= 0; j--) {

					_tempSecondItem = _tempItems[j];

					if (_tempSecondItem == _tempFirstItem || _tempFirstItem.StoreItemType == _tempSecondItem.StoreItemType)
						continue;

					if (!_tempFirstItem.Item.HasValue || !_tempSecondItem.Item.HasValue)
						continue;

					if (_tempFirstItem.Item.Value.TypeId != _tempSecondItem.Item.Value.TypeId)
						continue;

					if (_tempFirstItem.Item.Value.SubtypeId != _tempSecondItem.Item.Value.SubtypeId)
						continue;

					var avgPrice = (_tempFirstItem.PricePerUnit + _tempSecondItem.PricePerUnit) / 2;
					_tempFirstItem.PricePerUnit = avgPrice;
					_tempSecondItem.PricePerUnit = avgPrice;

					/*
					if (_tempFirstItem.Amount > _tempSecondItem.Amount) {

						_tempFirstItem.Amount -= _tempSecondItem.Amount;
						_tempSecondItem.Amount = 0;

					} else if (_tempFirstItem.Amount < _tempSecondItem.Amount) {

						_tempSecondItem.Amount -= _tempFirstItem.Amount;
						_tempFirstItem.Amount = 0;

					}
					*/

				}

			}

		}

		private void ApplyItemLimits(IMyStoreItem item) {

			if (_storeLimits == null)
				return;

			if (item.Item.HasValue) {

				if (item.Item.Value.TypeId == typeof(MyObjectBuilder_Ore)) {

					LimitAmountAndValue(item, _storeLimits.MaxOreAmount, _storeLimits.MaxOreValue);
					return;
				
				}

				if (item.Item.Value.TypeId == typeof(MyObjectBuilder_Ingot)) {

					LimitAmountAndValue(item, _storeLimits.MaxIngotAmount, _storeLimits.MaxIngotValue);
					return;

				}

				if (item.Item.Value.TypeId == typeof(MyObjectBuilder_Component)) {

					LimitAmountAndValue(item, _storeLimits.MaxComponentAmount, _storeLimits.MaxComponentValue);
					return;

				}

				if (item.Item.Value.TypeId == typeof(MyObjectBuilder_AmmoMagazine)) {

					LimitAmountAndValue(item, _storeLimits.MaxAmmoAmount, _storeLimits.MaxAmmoValue);
					return;

				}

				if (item.Item.Value.TypeId == typeof(MyObjectBuilder_PhysicalGunObject)) {

					LimitAmountAndValue(item, _storeLimits.MaxToolAmount, _storeLimits.MaxToolValue);
					return;

				}

				if (item.Item.Value.TypeId == typeof(MyObjectBuilder_ConsumableItem)) {

					LimitAmountAndValue(item, _storeLimits.MaxConsumableAmount, _storeLimits.MaxConsumableValue);
					return;

				}

			}

			if (item.ItemType == ItemTypes.Hydrogen || item.ItemType == ItemTypes.Oxygen) {

				LimitAmountAndValue(item, _storeLimits.MaxGasAmount, _storeLimits.MaxGasValue);
				return;

			}

		}

		private void LimitAmountAndValue(IMyStoreItem item, int maxAmount, int maxValue) {

			var actualAmount = item.Amount - item.RemovedAmount;
			var totalValue = (actualAmount * item.PricePerUnit);
			//BehaviorLogger.Write("Max Value: " + maxValue.ToString(), BehaviorDebugEnum.Action);
			//BehaviorLogger.Write("Max Amount: " + maxAmount.ToString(), BehaviorDebugEnum.Action);
			//BehaviorLogger.Write("2Actual Amount: " + actualAmount.ToString(), BehaviorDebugEnum.Action);
			//BehaviorLogger.Write("2Max Amount: " + maxAmount.ToString(), BehaviorDebugEnum.Action);
			if (maxAmount > -1) {

				if (actualAmount > maxAmount) {

					var difference = actualAmount - maxAmount;
					//BehaviorLogger.Write("Amount Difference: " + difference.ToString(), BehaviorDebugEnum.Action);
					item.Amount -= difference; //was += previously. hopefully that was a derp

				}
			
			}

			if (maxValue > -1) {

				if (totalValue > maxValue) {

					var valueDiff = (int)Math.Abs(maxValue - totalValue);
					var amountToRemove = valueDiff / item.PricePerUnit;
					//BehaviorLogger.Write("Amount Value Difference: " + amountToRemove.ToString(), BehaviorDebugEnum.Action);
					item.Amount -= amountToRemove;

				}

			}

		}

		private MyDefinitionId? RandomCraftableItemId(StoreItem item, List<MyDefinitionId> exclusionList) {

			if(item.ItemSubtypeId == "Component" && EconomyHelper.CraftableComponents.Count > 0)
				return CollectionHelper.GetRandomIdFromList(EconomyHelper.CraftableComponents, exclusionList);

			if (item.ItemSubtypeId == "Ammo" && EconomyHelper.CraftableAmmo.Count > 0)
				return CollectionHelper.GetRandomIdFromList(EconomyHelper.CraftableAmmo, exclusionList);

			if (item.ItemSubtypeId == "Tool" && EconomyHelper.CraftableTools.Count > 0)
				return CollectionHelper.GetRandomIdFromList(EconomyHelper.CraftableTools, exclusionList);

			if (EconomyHelper.AssemblerCraftableItems.Count > 0)
				return CollectionHelper.GetRandomIdFromList(EconomyHelper.AssemblerCraftableItems, exclusionList);

			return null;

			
			
			
			

		}

		private MyDefinitionId? RandomItemId(StoreItem item, List<MyDefinitionId> exclusionList) {

			if (item.ItemSubtypeId == "Ingot" && EconomyHelper.PublicIngots.Count > 0)
				return CollectionHelper.GetRandomIdFromList(EconomyHelper.PublicIngots, exclusionList);

			if (item.ItemSubtypeId == "Consumable" && EconomyHelper.PublicConsumables.Count > 0)
				return CollectionHelper.GetRandomIdFromList(EconomyHelper.PublicConsumables, exclusionList);

			if (item.ItemSubtypeId == "Component" && EconomyHelper.PublicComponents.Count > 0)
				return CollectionHelper.GetRandomIdFromList(EconomyHelper.PublicComponents, exclusionList);

			if (item.ItemSubtypeId == "Ammo" && EconomyHelper.PublicAmmos.Count > 0)
				return CollectionHelper.GetRandomIdFromList(EconomyHelper.PublicAmmos, exclusionList);

			if (item.ItemSubtypeId == "Tool" && EconomyHelper.PublicTools.Count > 0)
				return CollectionHelper.GetRandomIdFromList(EconomyHelper.PublicTools, exclusionList);

			if (EconomyHelper.PublicItems.Count > 0)
				return CollectionHelper.GetRandomIdFromList(EconomyHelper.AssemblerCraftableItems, exclusionList);

			return null;

			
			
			
			
			
			

		}

		private void PrepareTempIndexList(List<int> list, int randomAmount, int itemCount) {

			list.Clear();

			for (int i = 0; i < itemCount; i++) 
				list.Add(i);

			int itemCountTracker = itemCount;
			int randomAdded = 0;

			while (itemCountTracker > randomAmount && itemCountTracker > 0) {

				list.RemoveAt(MathTools.RandomBetween(0, list.Count));
				itemCountTracker--;
			
			}

		}

	}

}
