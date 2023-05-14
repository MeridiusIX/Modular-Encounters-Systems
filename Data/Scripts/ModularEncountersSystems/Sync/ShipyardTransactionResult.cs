using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.Spawning.Profiles;
using ModularEncountersSystems.Tasks;
using ModularEncountersSystems.Terminal;
using ModularEncountersSystems.Watchers;
using ProtoBuf;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRage.ModAPI;

namespace ModularEncountersSystems.Sync {

	public enum ShipyardTransactionResultEnum {
	
		None,
		PlayerIdentityZero,
		PlayerEntityInactive,
		ProjectorNotFound,
		ProjectorDataStorageNotFound,
		TargetGridEntityNotFound,
		TargetGridClassNotFound,
		ProjectorHasNoProfileName,
		ShipyardProfileNotFound,
		ShipyardModeNotSet,
		ProjectorNotProjectingGrid,
		ShipyardRulesNotSatisfied,
		ServerRawCostZero,
		ServerClientPriceMismatch,
		PlayerInsufficientCredits,
		MerchantInsufficientCredits,
		TransactionSuccessful,
	
	}

	[ProtoContract]
	public class ShipyardTransactionResult {

		[ProtoMember(1)] public ShipyardTransactionResultEnum Result;
		[ProtoMember(2)] public string CustomMessage;
		[ProtoMember(3)] public long ServerPrice;
		[ProtoMember(4)] public ShipyardModes Mode;
		[ProtoMember(5)] public long ProjectorId;

		public ShipyardTransactionResult() {

			Result = ShipyardTransactionResultEnum.None;
			CustomMessage = "";
			ServerPrice = 0;

		}

		public ShipyardTransactionResult(ShipyardTransactionResultEnum result, ShipyardModes mode, long projectorId, long serverPrice = 0, string customMessage = null) {

			Result = result;
			Mode = mode;
			ServerPrice = serverPrice;
			CustomMessage = customMessage;
			ProjectorId = projectorId;
		}

		public static void SendResult(ShipyardTransactionResultEnum result, ShipyardModes mode, ulong sender, long projectorId, long serverPrice = 0, string customMessage = null) {

			var results = new ShipyardTransactionResult(result, mode, projectorId, serverPrice, customMessage);
			var data = MyAPIGateway.Utilities.SerializeToBinary<ShipyardTransactionResult>(results);
			var container = new SyncContainer(SyncMode.ShipyardTransactionResult, data);
			SyncManager.SendSyncMesage(container, sender);

		}

		public void ProcessMessage() {

			string screenTitle = Result == ShipyardTransactionResultEnum.TransactionSuccessful ? "Transaction Successful" : "Transaction Unsuccessful";
			string objective = "";

			if (Mode == ShipyardModes.BlueprintBuilding)
				objective = "Blueprint Building";

			if (Mode == ShipyardModes.ScrapPurchasing)
				objective = "Scrap Purchasing";

			if (Mode == ShipyardModes.RepairAndConstruction)
				objective = "Repair and Construction";

			var sb = new StringBuilder();

			if (Result == ShipyardTransactionResultEnum.TransactionSuccessful) {

				if (Mode == ShipyardModes.ScrapPurchasing) {

					sb.Append("Amount Paid To Player: ").Append(ServerPrice);

				}

				if (Mode == ShipyardModes.RepairAndConstruction) {

					sb.Append("Amount Paid To Merchant: ").Append(ServerPrice);

				}

				if (Mode == ShipyardModes.GridTakeover) {

					sb.Append("Amount Paid To Merchant: ").Append(ServerPrice);

				}

			} else {

				//sb.Append("Transaction Not Successful").AppendLine().AppendLine();

				if (Result == ShipyardTransactionResultEnum.PlayerIdentityZero) {

					sb.Append("Provided Player Identity is Zero.");

				}

				if (Result == ShipyardTransactionResultEnum.PlayerEntityInactive) {

					sb.Append("Provided Player Entity Inactive.");

				}

				if (Result == ShipyardTransactionResultEnum.ProjectorNotFound) {

					sb.Append("Projector / Console Table Not Found on Server.");

				}

				if (Result == ShipyardTransactionResultEnum.ProjectorDataStorageNotFound) {

					sb.Append("Projector / Console Table Missing Data Storage.");

				}

				if (Result == ShipyardTransactionResultEnum.TargetGridEntityNotFound) {

					sb.Append("Selected or Projected Grid Not Found.");

				}

				if (Result == ShipyardTransactionResultEnum.TargetGridClassNotFound) {

					sb.Append("Selected or Projected Grid Entity Object Not Found.");

				}

				if (Result == ShipyardTransactionResultEnum.ProjectorHasNoProfileName) {

					sb.Append("Projector / Console Table Missing Shipyard Profile Name.");

				}

				if (Result == ShipyardTransactionResultEnum.ShipyardProfileNotFound) {

					sb.Append("Projector / Console Table Shipyard Profile Could Not Be Found With Provided Name.");

				}

				if (Result == ShipyardTransactionResultEnum.ShipyardModeNotSet) {

					sb.Append("Projector / Console Table Shipyard Mode Not Set.");

				}

				if (Result == ShipyardTransactionResultEnum.ProjectorNotProjectingGrid) {

					sb.Append("Projector / Console Table Not Projecting A Grid.");

				}

				if (Result == ShipyardTransactionResultEnum.ShipyardRulesNotSatisfied) {

					sb.Append("Projector / Console Table Shipyard Rules Not Satisfied For Target Grid.");

				}

				if (Result == ShipyardTransactionResultEnum.ServerRawCostZero) {

					sb.Append("Calculated Value of Transaction is 0 Credits.");

				}

				if (Result == ShipyardTransactionResultEnum.ServerClientPriceMismatch) {

					sb.Append("Calculated Value of Transaction on Server doesn't match Value on Client. To Proceed With Transaction Anyway, Use \"Allow Server Price\" Checkbox and Retry the Order.").AppendLine().AppendLine();
					sb.Append("Server Price: ").Append(ServerPrice);

				}

				if (Result == ShipyardTransactionResultEnum.PlayerInsufficientCredits) {

					sb.Append("You Don't Have Enough Credits For This Transaction.");

				}

				if (Result == ShipyardTransactionResultEnum.MerchantInsufficientCredits) {

					sb.Append("The Merchant Doesn't Have Enough Credits For This Transaction.");

				}

			}

			IMyEntity projectorEntity = null;

			if (MyAPIGateway.Entities.TryGetEntityById(ProjectorId, out projectorEntity)) {

				var projector = projectorEntity as IMyProjector;

				if (projector != null) {

					var task = new TerminalTransactionRefresh(projector, ShipyardControls.GetPriceQuote);
					TaskProcessor.Tasks.Add(task);
					MyAPIGateway.Utilities.ShowMissionScreen(screenTitle, "", objective, sb.ToString(), task.ScreenClose);

				}
				
			}
			
			
		
		}

	}

}
