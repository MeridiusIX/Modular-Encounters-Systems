using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.Spawning.Profiles;
using ModularEncountersSystems.Terminal;
using ModularEncountersSystems.Watchers;
using ProtoBuf;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRage.ModAPI;

namespace ModularEncountersSystems.Sync {

	[ProtoContract]
	public class ShipyardTransaction {

		[ProtoMember(1)] public long ProjectorId;
		[ProtoMember(2)] public long ClientPrice;
		[ProtoMember(3)] public long TargetGridId;
		[ProtoMember(4)] public ShipyardModes Mode;
		[ProtoMember(5)] public bool ConstructBlocks;
		[ProtoMember(6)] public bool RepairBlocks;
		[ProtoMember(7)] public bool UseServerPrice;
		[ProtoMember(8)] public int ReputationPenalty;

		public ShipyardTransaction() {

			ProjectorId = 0;
			ClientPrice = 0;
			TargetGridId = 0;
			Mode = ShipyardModes.None;
			ConstructBlocks = false;
			RepairBlocks = false;
			UseServerPrice = false;
			ReputationPenalty = 0;

		}

		public void ProcessTransaction(ulong sender) {

			if (!MyAPIGateway.Multiplayer.IsServer)
				return;

			//Get Player
			var identityId = MyAPIGateway.Players.TryGetIdentityId(sender);

			if (identityId <= 0) {

				ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.PlayerIdentityZero, Mode, sender, ProjectorId);
				return;
			
			}

			var player = PlayerManager.GetPlayerWithIdentityId(identityId);

			if (player == null || !player.ActiveEntity()) {

				ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.PlayerEntityInactive, Mode, sender, ProjectorId);
				return;

			}

			//Get Projector
			IMyEntity projectorEntity = null;

			if (!MyAPIGateway.Entities.TryGetEntityById(ProjectorId, out projectorEntity)) {

				ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.ProjectorNotFound, Mode, sender, ProjectorId);
				return;
			
			}

			var projector = projectorEntity as IMyProjector;

			if (projector?.Storage == null) {

				ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.ProjectorDataStorageNotFound, Mode, sender, ProjectorId);
				return;
			
			}

			//Get Target Grid
			IMyEntity gridEntity = null;

			if (!MyAPIGateway.Entities.TryGetEntityById(TargetGridId, out gridEntity)) {

				ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.TargetGridEntityNotFound, Mode, sender, ProjectorId);
				return;

			}

			var grid = GridManager.GetGridEntity(gridEntity as IMyCubeGrid);

			if (grid == null) {

				ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.TargetGridClassNotFound, Mode, sender, ProjectorId);
				return;

			}

			//TODO: Distance Check

			//Get Shipyard Profile
			ShipyardProfile shipyardProfile = null;
			string shipyardProfileName = null;

			if (!projector.Storage.TryGetValue(StorageTools.MesShipyardKey, out shipyardProfileName)) {

				ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.ProjectorHasNoProfileName, Mode, sender, ProjectorId);
				return;
			
			}

			if (!ProfileManager.ShipyardProfiles.TryGetValue(shipyardProfileName, out shipyardProfile)) {

				ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.ShipyardProfileNotFound, Mode, sender, ProjectorId);
				return;
			
			}

			//Reputation
			var faction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(projector.OwnerId);
			var rep = MyAPIGateway.Session.Factions.GetReputationBetweenPlayerAndFaction(identityId, faction?.FactionId ?? 0);

			if (Mode == ShipyardModes.BlueprintBuilding)
				ProcessBlueprintBuilding(player, grid, projector, shipyardProfile, rep, sender);

			if (Mode == ShipyardModes.ScrapPurchasing)
				ProcessScrapPurchasing(player, grid, projector, shipyardProfile, rep, sender);

			if (Mode == ShipyardModes.RepairAndConstruction)
				ProcessRepairAndConstruct(player, grid, projector, shipyardProfile, rep, sender);

			if (Mode == ShipyardModes.GridTakeover)
				ProcessGridTakeover(player, grid, projector, shipyardProfile, rep, sender);

		}

		internal void ProcessBlueprintBuilding(PlayerEntity player, GridEntity grid, IMyProjector projector, ShipyardProfile profile, int reputation, ulong sender) {

			//Calculate Work Eligiblity
			if (!projector.IsProjecting || projector.ProjectedGrid == null || projector.ProjectedGrid != grid.CubeGrid) {

				ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.ProjectorNotProjectingGrid, Mode, sender, ProjectorId);
				return;
			
			}

			if (!ShipyardControls.GridSelectionRules(projector, grid, ShipyardModes.BlueprintBuilding, profile)) {

				ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.ShipyardRulesNotSatisfied, Mode, sender, ProjectorId);
				return;

			}

			//Calculate Cost Of Work
			var serverRawCost = grid.CreditValueRegular(false, true);

			if (serverRawCost <= 0) {

				ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.ServerRawCostZero, Mode, sender, ProjectorId);
				return;
			
			}

			var serverFinalCost = profile.GetBlueprintPrice(serverRawCost, reputation);

			//Compare To ClientPrice
			if (serverFinalCost != ClientPrice && !UseServerPrice) {

				ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.ServerClientPriceMismatch, Mode, sender, ProjectorId, serverFinalCost);
				return;
			
			}

			//Check Merchant Credits

			//Build Item

			//Register Prefab

			//Remove Existing Prefab Item From Store, If Exists

			//Send To Store

			//Register Store Entity Id and Item Id Pair

		}

		internal void ProcessScrapPurchasing(PlayerEntity player, GridEntity grid, IMyProjector projector, ShipyardProfile profile, int reputation, ulong sender) {

			//Calculate Work Eligiblity
			if (!ShipyardControls.GridSelectionRules(projector, grid, ShipyardModes.ScrapPurchasing, profile)) {

				ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.ShipyardRulesNotSatisfied, Mode, sender, ProjectorId);
				return;

			}

			//Calculate Cost Of Work
			var serverRawCost = grid.CreditValueRegular(profile.ScrapPurchasingIncludeInventory);

			if (serverRawCost <= 0) {

				ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.ServerRawCostZero, Mode, sender, ProjectorId);
				return;

			}

			var serverFinalCost = profile.GetScrapPrice(serverRawCost, reputation);

			//Compare To ClientPrice
			if (serverFinalCost != ClientPrice && !UseServerPrice) {

				ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.ServerClientPriceMismatch, Mode, sender, ProjectorId, serverFinalCost);
				return;

			}

			var faction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(projector.OwnerId);

			//Merchant Credits
			if (profile.TransactionsUseNpcFactionBalance) {

				if (faction == null) {

					ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.MerchantInsufficientCredits, Mode, sender, ProjectorId);
					return;

				}

				long balance = 0;
				faction.TryGetBalanceInfo(out balance);

				if(balance < serverFinalCost) {

					ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.MerchantInsufficientCredits, Mode, sender, ProjectorId);
					return;

				}

			}

			//Connected To Station

			//Remove Ship
			MyVisualScriptLogicProvider.PlaySingleSoundAtPosition("MES-ShipyardSell", grid.GetPosition());
			grid.DisconnectSubgrids();
			((MyCubeGrid)grid.CubeGrid).DismountAllCockpits();
			Cleaning.ForceRemoveGrid(grid, "Despawn-SoldToShipyard");

			//Credit Player
			player.Player.RequestChangeBalance(serverFinalCost);

			//Charge Merchant
			if (profile.TransactionsUseNpcFactionBalance) {

				if (faction != null) {

					faction.RequestChangeBalance(-serverFinalCost);
				
				}
			
			}

			ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.TransactionSuccessful, Mode, sender, ProjectorId, serverFinalCost);

		}

		internal void ProcessRepairAndConstruct(PlayerEntity player, GridEntity grid, IMyProjector projector, ShipyardProfile profile, int reputation, ulong sender) {

			//Calculate Work Eligiblity
			int additionaBlocks = 0;
			long constructionCost = 0;
			long repairCost = 0;

			if (ConstructBlocks) {

				constructionCost = grid.CreditValueProjectedBlocksBuildable(out additionaBlocks);

			}

			if (!ShipyardControls.GridSelectionRules(projector, grid, ShipyardModes.ScrapPurchasing, profile, -1, additionaBlocks)) {

				ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.ShipyardRulesNotSatisfied, Mode, sender, ProjectorId);
				return;

			}

			//Calculate Cost Of Work
			if (RepairBlocks) {

				repairCost = grid.CreditValueRepair();

			}

			var serverRawCost = repairCost + constructionCost;

			if (serverRawCost <= 0) {

				ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.ServerRawCostZero, Mode, sender, ProjectorId);
				return;

			}

			var serverFinalCost = profile.GetRepairPrice(serverRawCost, reputation);

			if (serverFinalCost <= 0) {

				ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.ServerRawCostZero, Mode, sender, ProjectorId);
				return;

			}

			//Compare To ClientPrice
			if (serverFinalCost != ClientPrice && !UseServerPrice) {

				ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.ServerClientPriceMismatch, Mode, sender, ProjectorId, serverFinalCost);
				return;

			}

			//Check Player Credits
			long playerBalance = 0;
			player.Player.TryGetBalanceInfo(out playerBalance);

			if (playerBalance < serverFinalCost) {

				ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.PlayerInsufficientCredits, Mode, sender, ProjectorId);
				return;
			
			}

			//Remove Credits From Player
			player.Player.RequestChangeBalance(-serverFinalCost);

			//Credit Merchant
			if (profile.TransactionsUseNpcFactionBalance) {

				var faction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(projector.OwnerId);

				if (faction != null) {

					faction.RequestChangeBalance(serverFinalCost);

				}

			}

			//Disable Safezones
			var safezones = new List<SafeZoneEntity>();

			foreach (var safezone in SafeZoneManager.SafeZones) {

				//MyVisualScriptLogicProvider.ShowNotificationToAll("Active Entity:" + safezone.ActiveEntity(), 10000);
				//MyVisualScriptLogicProvider.ShowNotificationToAll("Null:" + (safezone.SafeZone == null), 10000);
				//MyVisualScriptLogicProvider.ShowNotificationToAll("Enabled:" + safezone.SafeZone.Enabled, 10000);
				//MyVisualScriptLogicProvider.ShowNotificationToAll("In Zone:" + safezone.InZone(projector.GetPosition()), 10000);

				if (!safezone.ActiveEntity() || safezone.SafeZone == null || !safezone.SafeZone.Enabled || !safezone.InZone(projector.GetPosition()))
					continue;

				safezone.SafeZone.Enabled = false;
				safezones.Add(safezone);

			}

			int repairResult = 0;
			int projResult = 0;

			//Apply Repairs
			if (RepairBlocks) {

				repairResult = grid.AutoRepairBlocks();
			
			}

			//Construct Blocks
			if (ConstructBlocks) {

				projResult = grid.AutoConstuctProjectedBlocks(repairResult > 0);
			
			}

			//Enable Safezones
			foreach (var safezone in safezones) {

				if (!safezone.ActiveEntity() || safezone.SafeZone == null || safezone.SafeZone.Enabled)
					continue;

				safezone.SafeZone.Enabled = true;

			}

			ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.TransactionSuccessful, Mode, sender, ProjectorId, serverFinalCost);

		}

		internal void ProcessGridTakeover(PlayerEntity player, GridEntity grid, IMyProjector projector, ShipyardProfile profile, int reputation, ulong sender) {

			//Calculate Work Eligiblity
			if (!ShipyardControls.GridSelectionRules(projector, grid, ShipyardModes.GridTakeover, profile)) {

				ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.ShipyardRulesNotSatisfied, Mode, sender, ProjectorId);
				return;

			}

			//Calculate Cost Of Work
			var serverRawCost = EconomyHelper.GridTakeoverCost(grid, player.Player.IdentityId, profile.GridTakeoverPricePerComputerMultiplier); //grid.CreditValueRegular(profile.ScrapPurchasingIncludeInventory);

			if (serverRawCost <= 0) {

				ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.ServerRawCostZero, Mode, sender, ProjectorId);
				return;

			}

			var serverFinalCost = profile.GetTakeoverPrice(serverRawCost, reputation);

			//Compare To ClientPrice
			if (serverFinalCost != ClientPrice && !UseServerPrice) {

				ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.ServerClientPriceMismatch, Mode, sender, ProjectorId, serverFinalCost);
				return;

			}

			var faction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(projector.OwnerId);

			//Check Player Credits
			long playerBalance = 0;
			player.Player.TryGetBalanceInfo(out playerBalance);

			if (playerBalance < serverFinalCost) {

				ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.PlayerInsufficientCredits, Mode, sender, ProjectorId);
				return;

			}

			//Remove Credits From Player
			player.Player.RequestChangeBalance(-serverFinalCost);

			//Switch Ownership
			MyVisualScriptLogicProvider.PlaySingleSoundAtPosition("MES-ShipyardSell", grid.GetPosition());
			grid.CubeGrid.ChangeGridOwnership(player.Player.IdentityId, VRage.Game.MyOwnershipShareModeEnum.Faction);

			//Credit Merchant
			if (profile.TransactionsUseNpcFactionBalance) {

				var npcfaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(projector.OwnerId);

				if (npcfaction != null) {

					npcfaction.RequestChangeBalance(serverFinalCost);

				}

			}

			ShipyardTransactionResult.SendResult(ShipyardTransactionResultEnum.TransactionSuccessful, Mode, sender, ProjectorId, serverFinalCost);

		}

	}

}
