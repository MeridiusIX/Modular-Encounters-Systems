using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Progression;
using ModularEncountersSystems.Tasks;
using ProtoBuf;
using Sandbox.Game;
using Sandbox.ModAPI;
using System.Text;
using VRage.ModAPI;

namespace ModularEncountersSystems.Sync {

	public enum SuitUpgradeTransactionResult {
	
		None,
		NotProcessedOnServer,
		ProgressionNotFound,
		UpgradeMaxLevel,
		PlayerNotFound,
		InsufficientCredits,
		InsufficientResearchPoints,
		Success,
	
	}

	[ProtoContract]
	public class SuitUpgradeTransaction {

		[ProtoMember(1)] public long BlockId;
		[ProtoMember(2)] public long ClientPrice;
		[ProtoMember(3)] public SuitUpgradeTypes Upgrade;
		[ProtoMember(4)] public long IdentityId;
		[ProtoMember(5)] public SuitUpgradeTransactionResult Result;
		[ProtoMember(6)] public ProgressionContainer ResultProgression;
		[ProtoMember(7)] public long CreditsPaid;
		[ProtoMember(8)] public long PointsPaid;

		public SuitUpgradeTransaction() {

			BlockId = 0;
			ClientPrice = 0;
			Upgrade = SuitUpgradeTypes.None;
			IdentityId = 0;
			Result = SuitUpgradeTransactionResult.None;
			ResultProgression = null;
			CreditsPaid = 0;
			PointsPaid = 0;

		}

		public void ProcessTransaction(ulong sender) {

			if (!MyAPIGateway.Multiplayer.IsServer) {

				SendResult(SuitUpgradeTransactionResult.NotProcessedOnServer);
				return;

			}

			var progression = PlayerManager.GetProgressionContainer(IdentityId, sender);

			if (progression == null) {

				SendResult(SuitUpgradeTransactionResult.ProgressionNotFound);
				return;
			
			}

			var currentLevel = progression.GetUpgradeLevel(Upgrade);

			if (currentLevel >= 5) {

				SendResult(SuitUpgradeTransactionResult.UpgradeMaxLevel);
				return;

			}

			var player = PlayerManager.GetPlayerWithIdentityId(IdentityId);

			if (player?.Player == null) {

				SendResult(SuitUpgradeTransactionResult.PlayerNotFound);
				return;

			}

			long balance = 0;
			long upgradeCost = progression.GetUpgradeCreditCost(Upgrade);
			player.Player.TryGetBalanceInfo(out balance);

			if (balance < upgradeCost) {

				SendResult(SuitUpgradeTransactionResult.InsufficientCredits);
				return;

			}

			var researchCost = progression.GetUpgradeResearchCost(Upgrade);

			if (progression.Points < researchCost) {

				SendResult(SuitUpgradeTransactionResult.InsufficientResearchPoints);
				return;

			}

			//All Checks Passed, Complete Transaction
			player.Player.RequestChangeBalance(-upgradeCost);
			progression.Points -= researchCost;
			progression.ApplyUpgrade(Upgrade);
			ResultProgression = progression;
			//MyVisualScriptLogicProvider.ShowNotificationToAll(progression.DamageReductionSuitUpgradeLevel.ToString(), 4000);
			CreditsPaid = upgradeCost;
			PointsPaid = researchCost;

			player.InitSolarModule();

			SendResult(SuitUpgradeTransactionResult.Success);

		}

		internal void SendResult(SuitUpgradeTransactionResult result) {

			Result = result;

		}

		public void ProcessResult() {

			string screenTitle = Result == SuitUpgradeTransactionResult.Success ? "Transaction Successful" : "Transaction Unsuccessful";
			string objective = "";

			var sb = new StringBuilder();

			if (Result == SuitUpgradeTransactionResult.Success) {

				sb.Append("Upgrade Successfully Purchased!").AppendLine().AppendLine();
				sb.Append("Type: ").Append(ProgressionContainer.GetUpgradeName(Upgrade)).AppendLine();
				sb.Append("Upgrade Level: ").Append(ResultProgression.GetUpgradeLevel(Upgrade)).AppendLine();
				sb.Append("Credits Spent: ").Append(CreditsPaid).AppendLine();
				sb.Append("Research Points Spend: ").Append(PointsPaid).AppendLine();

			} else {

				//sb.Append("Transaction Not Successful").AppendLine().AppendLine();

				if (Result == SuitUpgradeTransactionResult.NotProcessedOnServer) {

					sb.Append("Transaction Not Processed On Server.");

				}

				if (Result == SuitUpgradeTransactionResult.ProgressionNotFound) {

					sb.Append("Player Progression Tracker Not Found.");

				}

				if (Result == SuitUpgradeTransactionResult.UpgradeMaxLevel) {

					sb.Append("Suit Upgrade Already Maxed Out.");

				}

				if (Result == SuitUpgradeTransactionResult.PlayerNotFound) {

					sb.Append("Player Entity Not Found On Server.");

				}

				if (Result == SuitUpgradeTransactionResult.InsufficientCredits) {

					sb.Append("Insufficient Player Credit Balance.");

				}

				if (Result == SuitUpgradeTransactionResult.InsufficientResearchPoints) {

					sb.Append("Insufficient Research Points Balance.");

				}

			}

			IMyEntity projectorEntity = null;

			if (MyAPIGateway.Entities.TryGetEntityById(BlockId, out projectorEntity)) {

				var block = projectorEntity as IMyTerminalBlock;

				if (block != null) {

					var task = new TerminalTransactionRefresh(block, null);
					TaskProcessor.Tasks.Add(task);
					MyAPIGateway.Utilities.ShowMissionScreen(screenTitle, "", objective, sb.ToString(), task.ScreenClose);

				}

			}

		}

	}

}
