using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Spawning.Profiles {
	public class ShipyardProfile {

		public string ProfileSubtypeId;

		public string BlockName;

		public string StoreBlockName;
		public double InteractionRadius;
		public int MinReputation;

		public int ReputationNeededForDiscount;
		public bool TransactionsUseNpcFactionBalance;
		
		public bool AllowBlueprintBuilding;
		public int BlueprintBuildingSmallGridBlockLimit;
		public int BlueprintBuildingLargeGridBlockLimit;
		public int BlueprintBuildingCommissionPercentage;
		public int BlueprintBuildingReputationDiscount;

		public bool AllowScrapPurchasing;
		public int ScrapPurchasingSmallGridBlockLimit;
		public int ScrapPurchasingLargeGridBlockLimit;
		public bool ScrapPurchasingIncludeInventory;
		public int ScrapPurchasingMaxPercentageValue;
		public int ScrapPurchasingReputationDiscount;

		public bool AllowRepairAndConstruction;
		public int RepairAndConstructionSmallGridBlockLimit;
		public int RepairAndConstructionLargeGridBlockLimit;
		public int RepairAndConstructionCommissionPercentage;
		public int RepairAndConstructionReputationDiscount;

		public bool AllowGridTakeover;
		public int GridTakeoverSmallGridBlockLimit;
		public int GridTakeoverLargeGridBlockLimit;
		public int GridTakeoverPricePerComputerMultiplier;
		public int GridTakeoverReputationDiscount;

		public Dictionary<string, Action<string, object>> EditorReference;

		public ShipyardProfile() {

			ProfileSubtypeId = "";

			BlockName = "";
			StoreBlockName = "";
			InteractionRadius = 250;
			MinReputation = -500;

			ReputationNeededForDiscount = 501;

			AllowBlueprintBuilding = false;
			BlueprintBuildingSmallGridBlockLimit = 2500;
			BlueprintBuildingLargeGridBlockLimit = 5000;
			BlueprintBuildingCommissionPercentage = 115;
			BlueprintBuildingReputationDiscount = 7;

			AllowScrapPurchasing = false;
			ScrapPurchasingSmallGridBlockLimit = 2500;
			ScrapPurchasingLargeGridBlockLimit = 5000;
			ScrapPurchasingIncludeInventory = false;
			ScrapPurchasingMaxPercentageValue = 75;
			ScrapPurchasingReputationDiscount = 10;

			AllowRepairAndConstruction = false;
			RepairAndConstructionSmallGridBlockLimit = 2500;
			RepairAndConstructionLargeGridBlockLimit = 5000;
			RepairAndConstructionCommissionPercentage = 115;
			RepairAndConstructionReputationDiscount = 7;

			AllowGridTakeover = false;
			GridTakeoverSmallGridBlockLimit = 2500;
			GridTakeoverLargeGridBlockLimit = 5000;
			GridTakeoverPricePerComputerMultiplier = 100;
			GridTakeoverReputationDiscount = 10;

			EditorReference = new Dictionary<string, Action<string, object>> {

				{"BlockName", (s, o) => TagParse.TagStringCheck(s, ref BlockName) },

				{"InteractionRadius", (s, o) => TagParse.TagDoubleCheck(s, ref InteractionRadius) },
				{"MinReputation", (s, o) => TagParse.TagIntCheck(s, ref MinReputation) },

				{"ReputationNeededForDiscount", (s, o) => TagParse.TagIntCheck(s, ref ReputationNeededForDiscount) },

				{"AllowBlueprintBuilding", (s, o) => TagParse.TagBoolCheck(s, ref AllowBlueprintBuilding) },
				{"BlueprintBuildingSmallGridBlockLimit", (s, o) => TagParse.TagIntCheck(s, ref BlueprintBuildingSmallGridBlockLimit) },
				{"BlueprintBuildingLargeGridBlockLimit", (s, o) => TagParse.TagIntCheck(s, ref BlueprintBuildingLargeGridBlockLimit) },
				{"BlueprintBuildingCommissionPercentage", (s, o) => TagParse.TagIntCheck(s, ref BlueprintBuildingCommissionPercentage) },
				{"BlueprintBuildingReputationDiscount", (s, o) => TagParse.TagIntCheck(s, ref BlueprintBuildingReputationDiscount) },

				{"AllowScrapPurchasing", (s, o) => TagParse.TagBoolCheck(s, ref AllowScrapPurchasing) },
				{"ScrapPurchasingSmallGridBlockLimit", (s, o) => TagParse.TagIntCheck(s, ref ScrapPurchasingSmallGridBlockLimit) },
				{"ScrapPurchasingLargeGridBlockLimit", (s, o) => TagParse.TagIntCheck(s, ref ScrapPurchasingLargeGridBlockLimit) },
				{"ScrapPurchasingIncludeInventory", (s, o) => TagParse.TagBoolCheck(s, ref ScrapPurchasingIncludeInventory) },
				{"ScrapPurchasingMaxPercentageValue", (s, o) => TagParse.TagIntCheck(s, ref ScrapPurchasingMaxPercentageValue) },
				{"ScrapPurchasingReputationDiscount", (s, o) => TagParse.TagIntCheck(s, ref ScrapPurchasingReputationDiscount) },

				{"AllowRepairAndConstruction", (s, o) => TagParse.TagBoolCheck(s, ref AllowRepairAndConstruction) },
				{"RepairAndConstructionSmallGridBlockLimit", (s, o) => TagParse.TagIntCheck(s, ref RepairAndConstructionSmallGridBlockLimit) },
				{"RepairAndConstructionLargeGridBlockLimit", (s, o) => TagParse.TagIntCheck(s, ref RepairAndConstructionLargeGridBlockLimit) },
				{"RepairAndConstructionCommissionPercentage", (s, o) => TagParse.TagIntCheck(s, ref RepairAndConstructionCommissionPercentage) },
				{"RepairAndConstructionReputationDiscount", (s, o) => TagParse.TagIntCheck(s, ref RepairAndConstructionReputationDiscount) },

				{"AllowGridTakeover", (s, o) => TagParse.TagBoolCheck(s, ref AllowGridTakeover) },
				{"GridTakeoverSmallGridBlockLimit", (s, o) => TagParse.TagIntCheck(s, ref GridTakeoverSmallGridBlockLimit) },
				{"GridTakeoverLargeGridBlockLimit", (s, o) => TagParse.TagIntCheck(s, ref GridTakeoverLargeGridBlockLimit) },
				{"GridTakeoverPricePerComputerMultiplier", (s, o) => TagParse.TagIntCheck(s, ref GridTakeoverPricePerComputerMultiplier) },
				{"GridTakeoverReputationDiscount", (s, o) => TagParse.TagIntCheck(s, ref GridTakeoverReputationDiscount) },

			};

		}

		public long GetBlueprintPrice(long rawValue, int rep) {

			int percentage = BlueprintBuildingCommissionPercentage - (rep >= ReputationNeededForDiscount ? BlueprintBuildingReputationDiscount : 0);
			float multiplier = ((float)percentage / 100);
			return (long)Math.Floor(rawValue * multiplier);

		}

		public long GetScrapPrice(long rawValue, int rep) {

			int percentage = ScrapPurchasingMaxPercentageValue + (rep >= ReputationNeededForDiscount ? ScrapPurchasingReputationDiscount : 0);
			float multiplier = ((float)percentage / 100);
			return (long)Math.Floor(rawValue * multiplier);

		}

		public long GetRepairPrice(long rawValue, int rep) {

			int percentage = RepairAndConstructionCommissionPercentage - (rep >= ReputationNeededForDiscount ? RepairAndConstructionReputationDiscount : 0);
			float multiplier = ((float)percentage / 100);
			return (long)Math.Floor(rawValue * multiplier);

		}

		public long GetTakeoverPrice(long rawValue, int rep) {

			int percentage = 100 - (rep >= ReputationNeededForDiscount ? GridTakeoverReputationDiscount : 0);
			float multiplier = ((float)percentage / 100);
			return (long)Math.Floor(rawValue * multiplier);

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

		public void InitTags(string customData) {

			if (string.IsNullOrWhiteSpace(customData) == false) {

				var descSplit = customData.Split('\n');

				foreach (var tag in descSplit) {

					EditValue(tag);

				}

			}

		}

	}
}
