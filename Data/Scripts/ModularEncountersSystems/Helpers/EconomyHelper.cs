using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning.Profiles;
using ProtoBuf;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;

namespace ModularEncountersSystems.Helpers {

	public static class EconomyHelper {

		public static Dictionary<MyDefinitionId, int> OreMinimumValues = new Dictionary<MyDefinitionId, int>();
		public static Dictionary<MyDefinitionId, int> IngotMinimumValues = new Dictionary<MyDefinitionId, int>();
		public static Dictionary<MyDefinitionId, int> ComponentMinimumValues = new Dictionary<MyDefinitionId, int>();
		public static Dictionary<MyDefinitionId, int> OtherItemMinimumValues = new Dictionary<MyDefinitionId, int>();
		public static Dictionary<MyDefinitionId, int> BlockMinimumValues = new Dictionary<MyDefinitionId, int>();

		public static Dictionary<MyDefinitionId, int> MinimumValuesMaster = new Dictionary<MyDefinitionId, int>();

		public static List<MyDefinitionId> ItemsWithBadValue = new List<MyDefinitionId>();
		public static List<MyDefinitionId> ForbiddenItems = new List<MyDefinitionId>();

		public static Dictionary<MyDefinitionId, MyBlueprintDefinitionBase> ComponentBlueprints = new Dictionary<MyDefinitionId, MyBlueprintDefinitionBase>();


		public static void Setup() {

			ForbiddenItems.Add(new MyDefinitionId(typeof(MyObjectBuilder_Ingot), "ShieldPoint"));

			//Build Lists of Ore, Ingot, Component Values
			var allItems = MyDefinitionManager.Static.GetPhysicalItemDefinitions();
			List<MyBlueprintDefinitionBase> usedBlueprints = new List<MyBlueprintDefinitionBase>();

			foreach (var item in allItems) {

				if (ForbiddenItems.Contains(item.Id) == true) {

					continue;

				}

				//Logger.Write("EH - " + item.Id.ToString(), true);

				usedBlueprints.Clear();

				if (item.Id.TypeId == typeof(MyObjectBuilder_Ore)) {

					int itemPrice = 0;

					if (OreMinimumValues.ContainsKey(item.Id) == false) {

						itemPrice = CalculateItemMinimalPrice(item.Id, 1f, usedBlueprints);
						OreMinimumValues.Add(item.Id, itemPrice);
						AddToMasterReference(item.Id, itemPrice);

					}

				}

				if (item.Id.TypeId == typeof(MyObjectBuilder_Ingot)) {

					int itemPrice = 0;

					if (IngotMinimumValues.ContainsKey(item.Id) == false) {

						itemPrice = CalculateItemMinimalPrice(item.Id, 1f, usedBlueprints);
						IngotMinimumValues.Add(item.Id, itemPrice);
						AddToMasterReference(item.Id, itemPrice);

					}

				}

				if (item.Id.TypeId == typeof(MyObjectBuilder_Component)) {

					int itemPrice = 0;

					if (ComponentMinimumValues.ContainsKey(item.Id) == false) {

						itemPrice = CalculateItemMinimalPrice(item.Id, 1f, usedBlueprints);
						ComponentMinimumValues.Add(item.Id, itemPrice);
						AddToMasterReference(item.Id, itemPrice);

					}

				}

				//TODO: Add Tools, Weapon, Bottles, Health/Energy, Zonechips?, Datapads?
				if (item.Id.TypeId == typeof(MyObjectBuilder_PhysicalGunObject)) {

					int itemPrice = 0;

					if (MinimumValuesMaster.ContainsKey(item.Id) == false) {

						itemPrice = CalculateItemMinimalPrice(item.Id, 1f, usedBlueprints);
						OtherItemMinimumValues.Add(item.Id, itemPrice);
						AddToMasterReference(item.Id, itemPrice);

					}

				}

				if (item.Id.TypeId == typeof(MyObjectBuilder_GasContainerObject)) {

					int itemPrice = 0;

					if (MinimumValuesMaster.ContainsKey(item.Id) == false) {

						itemPrice = CalculateItemMinimalPrice(item.Id, 1f, usedBlueprints);
						OtherItemMinimumValues.Add(item.Id, itemPrice);
						AddToMasterReference(item.Id, itemPrice);

					}

				}

				if (item.Id.TypeId == typeof(MyObjectBuilder_OxygenContainerObject)) {

					int itemPrice = 0;

					if (MinimumValuesMaster.ContainsKey(item.Id) == false) {

						itemPrice = CalculateItemMinimalPrice(item.Id, 1f, usedBlueprints);
						OtherItemMinimumValues.Add(item.Id, itemPrice);
						AddToMasterReference(item.Id, itemPrice);

					}

				}

				if (item.Id.TypeId == typeof(MyObjectBuilder_ConsumableItem)) {

					int itemPrice = 0;

					if (MinimumValuesMaster.ContainsKey(item.Id) == false) {

						itemPrice = CalculateItemMinimalPrice(item.Id, 1f, usedBlueprints);
						OtherItemMinimumValues.Add(item.Id, itemPrice);
						AddToMasterReference(item.Id, itemPrice);

					}

				}

				if (item.Id.TypeId == typeof(MyObjectBuilder_Datapad)) {

					int itemPrice = 0;

					if (MinimumValuesMaster.ContainsKey(item.Id) == false) {

						itemPrice = CalculateItemMinimalPrice(item.Id, 1f, usedBlueprints);
						OtherItemMinimumValues.Add(item.Id, itemPrice);
						AddToMasterReference(item.Id, itemPrice);

					}

				}

			}

			//Build List of Block Values
			var allDefinitions = MyDefinitionManager.Static.GetAllDefinitions();
			var definitions = new List<MyDefinitionBase>();

			foreach (var definition in allDefinitions.ToList().Where(x => x as MyCubeBlockDefinition != null)) {

				if (BlockMinimumValues.ContainsKey(definition.Id)) {

					continue;

				}

				var blockDefinition = definition as MyCubeBlockDefinition;
				int totalCost = 0;
				bool badItemValue = false;

				foreach (var component in blockDefinition.Components) {

					int componentValue = 0;

					if (ComponentMinimumValues.TryGetValue(component.Definition.Id, out componentValue) == true) {

						totalCost += componentValue * component.Count;

					}

					if (componentValue == 0 || ItemsWithBadValue.Contains(component.Definition.Id)) {

						badItemValue = true;

					}

				}

				BlockMinimumValues.Add(blockDefinition.Id, totalCost);

				if (badItemValue == true) {

					ItemsWithBadValue.Add(blockDefinition.Id);

				}

			}

		}

		public static void AddToMasterReference(MyDefinitionId id, int amount) {

			if (MinimumValuesMaster.ContainsKey(id) == false) {

				MinimumValuesMaster.Add(id, amount);

			}

		}

		public static int CalculateItemMinimalPrice(MyDefinitionId itemId, float baseCostProductionSpeedMultiplier, List<MyBlueprintDefinitionBase> usedBlueprints) {

			try {

				int minimalPrice = -1;

				//First, try to see if item definition has a minimum price already (for ore,ingot,component)
				MyPhysicalItemDefinition definition = null;

				if (MyDefinitionManager.Static.TryGetDefinition(itemId, out definition) && definition.MinimalPricePerUnit != -1) {

					//Has minimal price, so just use that.
					minimalPrice += definition.MinimalPricePerUnit;
					return minimalPrice;

				}

				MyBlueprintDefinitionBase definition2 = null;

				//Try to get a blueprint where the result is the item we are checking
				if (!MyDefinitionManager.Static.TryGetBlueprintDefinitionByResultId(itemId, out definition2)) {

					if (ItemsWithBadValue.Contains(itemId) == false) {

						ItemsWithBadValue.Add(itemId);

					}

					return minimalPrice;

				}

				if (usedBlueprints.Contains(definition2) == true) {

					//Logger.Write("Cannot Create Economy Value For Item: " + itemId.ToString() + " - Blueprint Already Used Or Gets Stuck In Loop.");
					return minimalPrice;

				}

				usedBlueprints.Add(definition2);
				float num = definition.IsIngot ? 1f : MyAPIGateway.Session.AssemblerEfficiencyMultiplier;
				int num2 = 0;
				MyBlueprintDefinitionBase.Item[] prerequisites = definition2.Prerequisites;
				bool hasBadValue = false;

				foreach (MyBlueprintDefinitionBase.Item item in prerequisites) {

					int minimalPrice2 = CalculateItemMinimalPrice(item.Id, baseCostProductionSpeedMultiplier, usedBlueprints);
					float num3 = (float)item.Amount / num;
					num2 += (int)(minimalPrice2 * num3);

					if (minimalPrice2 <= 0) {

						hasBadValue = true;

						if (ItemsWithBadValue.Contains(item.Id) == false) {

							ItemsWithBadValue.Add(item.Id);

						}

					}

				}

				if (hasBadValue == true) {

					if (ItemsWithBadValue.Contains(itemId) == false) {

						ItemsWithBadValue.Add(itemId);

					}

				}

				float num4 = definition.IsIngot ? MyAPIGateway.Session.RefinerySpeedMultiplier : MyAPIGateway.Session.AssemblerSpeedMultiplier;
				int num5 = 0;
				MyBlueprintDefinitionBase.Item item2;

				while (true) {

					if (num5 < definition2.Results.Length) {

						item2 = definition2.Results[num5];

						if (item2.Id == itemId) {

							break;

						}

						num5++;
						continue;

					}

					return minimalPrice;

				}

				float num6 = (float)item2.Amount;
				float num7 = 1f + (float)Math.Log(definition2.BaseProductionTimeInSeconds + 1f) * baseCostProductionSpeedMultiplier / num4;
				minimalPrice += (int)(num2 * (1f / num6) * num7);
				return minimalPrice;

			} catch (Exception e) {

				SpawnLogger.Write("Caught Exception While Processing Economy Price For Item ID " + itemId.ToString(), SpawnerDebugEnum.Error, true);
				SpawnLogger.Write(e.ToString(), SpawnerDebugEnum.Error, true);

			}

			return -1;

		}

		public static void InitNpcStoreBlock(IMyCubeGrid cubeGrid, ImprovedSpawnGroup spawnGroup) {

			var errorLog = new StringBuilder();
			errorLog.Append("Starting Store Block Init For Grid").AppendLine();

			try {

				errorLog.Append(" - Check if Grid Exists").AppendLine();
				if (cubeGrid == null || MyAPIGateway.Entities.Exist(cubeGrid) == false) {

					return;

				}

				errorLog.Append(" - Get Blocks From Grid: " + cubeGrid.CustomName).AppendLine();
				var blockList = new List<IMySlimBlock>();
				var storeBlockList = new List<IMyStoreBlock>();
				var containerList = new List<IMyCargoContainer>();
				var gastankList = new List<IMyGasTank>();
				cubeGrid.GetBlocks(blockList);

				foreach (var block in blockList.Where(x => x.FatBlock != null)) {

					var storeBlock = block.FatBlock as IMyStoreBlock;
					var cargo = block.FatBlock as IMyCargoContainer;
					var gasTank = block.FatBlock as IMyGasTank;

					if (storeBlock != null) {

						errorLog.Append("   - Found Store Block " + storeBlock.EntityId.ToString()).AppendLine();
						storeBlockList.Add(storeBlock);

					}

					if (cargo != null) {

						errorLog.Append("   - Found Container Block " + cargo.EntityId.ToString()).AppendLine();
						containerList.Add(cargo);

					}

				}

				var usedCargoContainers = new List<IMyCargoContainer>();
				//var usedTanks = new List<IMyGasTank>();

				errorLog.Append(" - Processing Store Blocks").AppendLine();

				//Process Stores
				foreach (var store in storeBlockList) {

					errorLog.Append("   - Check If Store Inventory Exists").AppendLine();
					var storeInv = store.GetInventory();

					if (storeInv == null)
						continue;

					try {

						errorLog.Append("   - Check Store For Existing Items And Remove " + store.EntityId.ToString()).AppendLine();
						var ob = (MyObjectBuilder_StoreBlock)store.SlimBlock.GetObjectBuilder();

						errorLog.Append("      - Check If ObjectBuilder Null" + store.EntityId.ToString()).AppendLine();
						if (ob != null) {

							//var existingIdList = new List<long>();

							if (ob.PlayerItems != null) {

								foreach (var item in ob.PlayerItems.ToList()) {

									store.CancelStoreItem(item.Id);

								}

							}

						}

					} catch (Exception exc) {

						errorLog.Append("      - Exception Encountered, Probably One Of Those Troublesome ATMs" + store.EntityId.ToString()).AppendLine();

					}

					var itemsAvailable = new Dictionary<MyDefinitionId, int>();
					var gasAvailable = new Dictionary<MyDefinitionId, int>();
					var storeitemList = storeInv.GetItems();

					errorLog.Append("   - Check Items In Store Inventory To Sell").AppendLine();

					foreach (var item in storeitemList) {

						var itemDefId = new MyDefinitionId(item.Content.TypeId, item.Content.SubtypeId);
						//Logger.Write("Item: " + itemDefId.ToString(), true);

						if (itemDefId.SubtypeName == "SpaceCredit") {

							continue;

						}

						var amount = (float)item.Amount;
						int amountRounded = (int)Math.Floor(amount);

						if (itemsAvailable.ContainsKey(itemDefId) == false) {

							itemsAvailable.Add(itemDefId, amountRounded);

						} else {

							itemsAvailable[itemDefId] += amountRounded;

						}

					}

					//Get Gas
					/*
					double hydrogen = 100000;
					double oxygen = 100000;

					foreach(var tank in gastankList) {

						if(usedTanks.Contains(tank) == true) {

							continue;

						}

						usedTanks.Add(tank);

						if(tank.IsWorking == false || tank.IsFunctional == false) {

							continue;

						}

						var tankDef = (MyGasTankDefinition)tank.SlimBlock.BlockDefinition;
						var tankInv = tank.GetInventory();

						if(tankInv.IsConnectedTo(storeInv) == false || tank.IsSameConstructAs(store) == false) {

							continue;

						}

						if(tankDef.StoredGasId.SubtypeName == "Hydrogen") {

							hydrogen += tank.Capacity * tank.FilledRatio;

						}

						if(tankDef.StoredGasId.SubtypeName == "Oxygen") {

							oxygen += tank.Capacity * tank.FilledRatio;

						}

					}

					hydrogen = Math.Floor(hydrogen / 1000);
					oxygen = Math.Floor(oxygen / 1000);
					*/

					errorLog.Append("   - Check Items in Attached Cargo Containers To Sell").AppendLine();

					//Get Items For Offers
					foreach (var cargo in containerList) {

						//Logger.Write("Checking Cargo Container On: " + cargo.CubeGrid.CustomName);

						if (usedCargoContainers.Contains(cargo)) {

							continue;

						}

						var cargoInv = cargo.GetInventory();

						if (cargoInv.IsConnectedTo(storeInv) == false || cargo.IsSameConstructAs(store) == false) {

							continue;

						}

						usedCargoContainers.Add(cargo);

						var itemList = cargoInv.GetItems();

						foreach (var item in itemList) {

							var itemDefId = new MyDefinitionId(item.Content.TypeId, item.Content.SubtypeId);
							//Logger.Write("Item: " + itemDefId.ToString(), true);

							if (itemDefId.SubtypeName == "SpaceCredit") {

								continue;

							}

							var amount = (float)item.Amount;
							int amountRounded = (int)Math.Floor(amount);

							if (itemsAvailable.ContainsKey(itemDefId) == false) {

								itemsAvailable.Add(itemDefId, amountRounded);

							} else {

								itemsAvailable[itemDefId] += amountRounded;

							}

						}

					}

					errorLog.Append("   - Add Each Item To The Store Block").AppendLine();
					foreach (var item in itemsAvailable.Keys) {

						errorLog.Append("     - Checking Item: " + item.ToString()).AppendLine();
						if (ItemsWithBadValue.Contains(item) || MinimumValuesMaster.ContainsKey(item) == false) {

							errorLog.Append("     - Item Has Bad Value According To Economy Helper").AppendLine();
							//Logger.Write(item.ToString() + " has invalid economy value or was not in master reference", true);
							continue;

						}


						double markup = 1.2; //TODO: Figure out how reputation affects this
						int itemValue = (int)Math.Floor(MinimumValuesMaster[item] * markup);
						MyStoreItemData orderData = new MyStoreItemData(item, itemsAvailable[item], itemValue, OnSaleComplete, null);
						long orderResult = 0;
						var storeAddResult = store.InsertOffer(orderData, out orderResult);
						errorLog.Append("     - Added Item To Store With Result: " + storeAddResult.ToString()).AppendLine();
						//Logger.Write(item.ToString() + " Add To Store Result: " + storeAddResult.ToString(), true);

					}

					//Populate Store With Orders

					if (spawnGroup != null) {

						if (spawnGroup.ContainerTypesForStoreOrders.Count > 0) {

							var containerString = spawnGroup.ContainerTypesForStoreOrders[MathTools.RandomBetween(0, spawnGroup.ContainerTypesForStoreOrders.Count)];
							var containerDef = MyDefinitionManager.Static.GetContainerTypeDefinition(containerString);

							if (containerDef != null) {

								foreach (var item in containerDef.Items) {

									if (ItemsWithBadValue.Contains(item.DefinitionId) || MinimumValuesMaster.ContainsKey(item.DefinitionId) == false) {

										continue;

									}

									double markup = 1.2;
									int itemCount = MathTools.RandomBetween((int)item.AmountMin, (int)item.AmountMax);
									int itemValue = (int)Math.Floor(MinimumValuesMaster[item.DefinitionId] * markup);
									MyStoreItemData orderData = new MyStoreItemData(item.DefinitionId, itemCount, itemValue, null, null);
									long orderResult = 0;
									var storeAddResult = store.InsertOrder(orderData, out orderResult);

									if (storeAddResult == Sandbox.ModAPI.Ingame.MyStoreInsertResults.Success && FactionHelper.IsIdentityNPC(store.OwnerId) == true) {

										MyAPIGateway.Players.RequestChangeBalance(store.OwnerId, itemCount * itemValue);

									}

									errorLog.Append("     - Added Item To Store With Result: " + storeAddResult.ToString()).AppendLine();
									//Logger.Write(item.ToString() + " Add To Store Result: " + storeAddResult.ToString(), true);

								}

							}

						}

					}

				}

			} catch (Exception e) {

				SpawnLogger.Write("Init Store Blocks for NPC Grid Failed. Please Provide The Log File To The Modular Encounters Spawner Mod Author:", SpawnerDebugEnum.Error, true);
				SpawnLogger.Write(errorLog.ToString(), SpawnerDebugEnum.Error, true);

			}

		}

		public static void OnSaleComplete(int amtSold, int amtRemain, long cost, long storeOwner, long customer) {

			if (customer == 0) {

				//Logger.Write("Buyer is Nobody", true);
				return;

			}

			var faction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(storeOwner);

			if (faction != null) {

				faction.RequestChangeBalance(cost);

			}

			if (Settings.General.UseEconomyBuyingReputationIncrease == false || customer == 0) {

				//Logger.Write("Economy Buy Rep Increaser Disabled or Buyer is Nobody", true);
				return;

			}

			var playerSales = new SaleTracker();

			string saleDataString = "";

			if (MyAPIGateway.Utilities.GetVariable("MES-SaleTracker-" + customer.ToString(), out saleDataString) == true) {

				try {

					var saleByteData = Convert.FromBase64String(saleDataString);
					var saleData = MyAPIGateway.Utilities.SerializeFromBinary<SaleTracker>(saleByteData);

					if (saleData != null) {

						//Logger.Write("Got Existing Player Sale Tracker", true);
						playerSales = saleData;

					}

				} catch (Exception e) {



				}

			}

			long existingAmt = 0;

			if (playerSales.Transactions.TryGetValue(storeOwner, out existingAmt) == false) {

				//Logger.Write("First Time Purchase From Faction", true);
				playerSales.Transactions.Add(storeOwner, 0);

			}

			existingAmt += cost;
			double dividedAmount = Math.Floor((double)existingAmt / Settings.General.EconomyBuyingReputationCostAmount);
			//Logger.Write("Total Added To Cost: " + existingAmt.ToString(), true);

			if (dividedAmount > 0) {

				var npcFaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(storeOwner);

				if (npcFaction != null) {

					//Logger.Write("Change Rep", true);
					var rep = MyAPIGateway.Session.Factions.GetReputationBetweenPlayerAndFaction(customer, npcFaction.FactionId);
					MyAPIGateway.Session.Factions.SetReputationBetweenPlayerAndFaction(customer, npcFaction.FactionId, rep + (int)dividedAmount);
					existingAmt -= (int)dividedAmount * Settings.General.EconomyBuyingReputationCostAmount;

				}

			}

			playerSales.Transactions[storeOwner] = existingAmt;
			var newByteData = MyAPIGateway.Utilities.SerializeToBinary(playerSales);
			var newStringData = Convert.ToBase64String(newByteData);
			MyAPIGateway.Utilities.SetVariable("MES-SaleTracker-" + customer.ToString(), newStringData);

		}

	}

	[ProtoContract]
	public class SaleTracker {

		[ProtoMember(1)]
		public Dictionary<long, long> Transactions;

		public SaleTracker() {

			Transactions = new Dictionary<long, long>();

		}

		public override string ToString() {

			try {

				var byteData = MyAPIGateway.Utilities.SerializeToBinary<SaleTracker>(this);
				var stringData = Convert.ToBase64String(byteData);
				return stringData;

			} catch (Exception exc) {

				SpawnLogger.Write("Failed To Save SaleTracker Data to String", SpawnerDebugEnum.Error, true);

			}

			return "";

		}

	}

}