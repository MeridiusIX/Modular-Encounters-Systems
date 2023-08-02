using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.Spawning.Profiles;
using ModularEncountersSystems.Sync;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.ModAPI;
using VRage.Utils;

namespace ModularEncountersSystems.Terminal {

	public enum ShipyardModes {
	
		None,
		BlueprintBuilding,
		ScrapPurchasing,
		RepairAndConstruction,
		GridTakeover,

	}

	public class ShipyardControlValues {

		public ShipyardProfile Profile;

		public ShipyardModes Mode;
		public GridEntity SelectedGridItem;

		public bool ConstructNewBlocks;
		public bool RepairBlocks;
		public bool UseServerPrice;

		public StringBuilder SmallGridLimit;
		public StringBuilder LargeGridLimit;
		public StringBuilder QuotedPrice;

		public long QuotedPriceValue;

		public ShipyardControlValues(IMyTerminalBlock block) {

			Mode = ShipyardModes.None;
			SelectedGridItem = null;

			ConstructNewBlocks = true;
			RepairBlocks = true;

			SmallGridLimit = new StringBuilder();
			SmallGridLimit.Append("");
			LargeGridLimit = new StringBuilder();
			LargeGridLimit.Append("");
			QuotedPrice = new StringBuilder();
			QuotedPrice.Append("");

			QuotedPriceValue = 0;

			block.AppendingCustomInfo += CustomInfo;

			string profileName = null;

			if (block.Storage != null)
				block.Storage.TryGetValue(StorageTools.MesShipyardKey, out profileName);

			if (profileName != null) 
				ProfileManager.ShipyardProfiles.TryGetValue(profileName, out Profile);

			if (Profile == null)
				Profile = new ShipyardProfile();

		}

		public void CustomInfo(IMyTerminalBlock block, StringBuilder sb) {

			sb.Clear();
			
			//Selected Grid
			sb.Append("Selected Grid: ").AppendLine().Append(SelectedGridItem?.CubeGrid?.CustomName ?? "[No Grid Selected]").AppendLine().AppendLine();
			//Transaction Cost
			sb.Append("Transaction Amount: ").AppendLine().Append(QuotedPriceValue > 0 ? QuotedPriceValue.ToString("C0") : "N/A").AppendLine().AppendLine();
			//Mode
			sb.Append("Mode: ").Append(Mode.ToString()).AppendLine();
			//Small Limit
			sb.Append("Small Grid Limit: ").Append(SmallGridLimit.ToString()).AppendLine();
			//Large Limit
			sb.Append("Large Grid Limit: ").Append(LargeGridLimit.ToString()).AppendLine();
			

		}

	}

	public static class ShipyardControls {

		internal static bool _setupDone = false;
		internal static Dictionary<long, ShipyardControlValues> _controlValues = new Dictionary<long, ShipyardControlValues>();
		internal static List<IMyProjector> _shipyardProjectors = new List<IMyProjector>();
		internal static List<string> _privateControls = new List<string>();
		internal static List<GridEntity> _grids = new List<GridEntity>();

		//Common
		internal static IMyTerminalControlLabel _label;
		internal static IMyTerminalControlLabel _labelBlueprint;
		internal static IMyTerminalControlLabel _labelScrap;
		internal static IMyTerminalControlLabel _labelRepair;
		internal static IMyTerminalControlLabel _labelTakeover;
		internal static IMyTerminalControlLabel _labelLimits;
		internal static IMyTerminalControlLabel _labelQuote;
		internal static IMyTerminalControlLabel _labelConfirmation;
		internal static IMyTerminalControlSeparator _separatorA;
		internal static IMyTerminalControlSeparator _separatorB;
		internal static IMyTerminalControlSeparator _separatorC;
		internal static IMyTerminalControlSeparator _separatorD;
		internal static IMyTerminalControlSeparator _separatorE;
		internal static IMyTerminalControlListbox _modeSelect;
		internal static IMyTerminalControlButton _infoButton;
		internal static IMyTerminalControlListbox _gridSelect;
		internal static IMyTerminalControlCheckbox _constructBlocks;
		internal static IMyTerminalControlCheckbox _repairBlocks;
		internal static IMyTerminalControlTextbox _smallGridLimit;
		internal static IMyTerminalControlTextbox _largeGridLimit;
		internal static IMyTerminalControlTextbox _quotedPrice;
		internal static IMyTerminalControlButton _quoteButton;
		internal static IMyTerminalControlButton _confirmButtonBlueprint;
		internal static IMyTerminalControlButton _confirmButtonScrap;
		internal static IMyTerminalControlButton _confirmButtonConstruct;
		internal static IMyTerminalControlButton _confirmButtonTakeover;
		internal static IMyTerminalControlCheckbox _useServerPrice;

		public static void DisplayControls(IMyTerminalBlock block, List<IMyTerminalControl> controls) {

			if (!_setupDone) {

				_setupDone = true;

				EntityWatcher.GridAdded += NewGrid;

				//Hidden Controls
				_privateControls.Add("");
				_privateControls.Add("OnOff");
				_privateControls.Add("ShowInTerminal");
				_privateControls.Add("ShowInInventory");
				_privateControls.Add("ShowInToolbarConfig");
				_privateControls.Add("Name");
				_privateControls.Add("ShowOnHUD");
				_privateControls.Add("CustomData");
				_privateControls.Add("PanelList");
				_privateControls.Add("Content");
				_privateControls.Add("Script");
				_privateControls.Add("ScriptForegroundColor");
				_privateControls.Add("ScriptBackgroundColor");
				_privateControls.Add("ShowTextPanel");
				_privateControls.Add("Font");
				_privateControls.Add("FontSize");
				_privateControls.Add("FontColor");
				_privateControls.Add("alignment");
				_privateControls.Add("TextPaddingSlider");
				_privateControls.Add("BackgroundColor");
				_privateControls.Add("ImageList");
				_privateControls.Add("SelectTextures");
				_privateControls.Add("ChangeIntervalSlider");
				_privateControls.Add("SelectedImageList");
				_privateControls.Add("RemoveSelectedTextures");
				_privateControls.Add("PreserveAspectRatio");
				_privateControls.Add("Label");
				_privateControls.Add("SpawnProjection");
				_privateControls.Add("InstantBuilding");
				_privateControls.Add("GetOwnership");
				_privateControls.Add("NumberOfProjections");
				_privateControls.Add("NumberOfBlocks");

				//Create Controls

				_label = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlLabel, IMyTerminalBlock>("MES-Shipyard-Label");
				_label.Enabled = (b) => { return true; };
				_label.Label = MyStringId.GetOrCompute("Shipyard Controls");

				_labelBlueprint = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlLabel, IMyTerminalBlock>("MES-Shipyard-LabelBlueprint");
				_labelBlueprint.Enabled = (b) => { return true; };
				_labelBlueprint.Label = MyStringId.GetOrCompute("Blueprint Building");

				_labelScrap = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlLabel, IMyTerminalBlock>("MES-Shipyard-LabelScrap");
				_labelScrap.Enabled = (b) => { return true; };
				_labelScrap.Label = MyStringId.GetOrCompute("Scrap Purchasing");

				_labelRepair = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlLabel, IMyTerminalBlock>("MES-Shipyard-LabelRepair");
				_labelRepair.Enabled = (b) => { return true; };
				_labelRepair.Label = MyStringId.GetOrCompute("Repair And Construction");

				_labelTakeover = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlLabel, IMyTerminalBlock>("MES-Shipyard-LabelTakeover");
				_labelTakeover.Enabled = (b) => { return true; };
				_labelTakeover.Label = MyStringId.GetOrCompute("Grid Takeover");

				_labelLimits = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlLabel, IMyTerminalBlock>("MES-Shipyard-LabelLimits");
				_labelLimits.Enabled = (b) => { return true; };
				_labelLimits.Label = MyStringId.GetOrCompute("Block / Grid Limits");

				_labelQuote = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlLabel, IMyTerminalBlock>("MES-Shipyard-LabelQuote");
				_labelQuote.Enabled = (b) => { return true; };
				_labelQuote.Label = MyStringId.GetOrCompute("Price Estimate / Quote");

				_labelConfirmation = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlLabel, IMyTerminalBlock>("MES-Shipyard-LabelConfirmation");
				_labelConfirmation.Enabled = (b) => { return true; };
				_labelConfirmation.Label = MyStringId.GetOrCompute("Confirmation");

				_modeSelect = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlListbox, IMyTerminalBlock>("MES-Shipyard-ModeSelect");
				_modeSelect.Enabled = (b) => { return true; };
				_modeSelect.Title = MyStringId.GetOrCompute("Mode Select");
				_modeSelect.Multiselect = false;
				_modeSelect.VisibleRowsCount = 4;
				_modeSelect.ListContent = ModeListContent;
				_modeSelect.ItemSelected = ModeListSelected;

				_infoButton = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlButton, IMyTerminalBlock>("MES-Shipyard-Quote");
				_infoButton.Enabled = (b) => { return true; };
				_infoButton.Title = MyStringId.GetOrCompute("Help / Info");
				_infoButton.Tooltip = MyStringId.GetOrCompute("Click here for information on how to use the Shipyard System.");
				_infoButton.Action = ShipyardInfo;

				_gridSelect = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlListbox, IMyTerminalBlock>("MES-Shipyard-GridSelect");
				_gridSelect.Enabled = (b) => { return true; };
				_gridSelect.Title = MyStringId.GetOrCompute("Grid Select");
				_gridSelect.Multiselect = false;
				_gridSelect.VisibleRowsCount = 4;
				_gridSelect.ListContent = GridSelectionListContent;
				_gridSelect.ItemSelected = GridSelection;

				_constructBlocks = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlCheckbox, IMyTerminalBlock>("MES-Shipyard-ConstructBlocks");
				_constructBlocks.Enabled = (b) => { return true; };
				_constructBlocks.Title = MyStringId.GetOrCompute("Construct New Blocks");
				_constructBlocks.Getter = (b) => {

					var ctrls = GetControlValues(b);
					return ctrls.ConstructNewBlocks;

				};
				_constructBlocks.Setter = (b, val) => {

					var ctrls = GetControlValues(b);
					ctrls.ConstructNewBlocks = val;
					GetPriceQuote(b);
					b.RefreshCustomInfo();
					ControlManager.RefreshMenu(b);

				};

				_repairBlocks = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlCheckbox, IMyTerminalBlock>("MES-Shipyard-RepairBlocks");
				_repairBlocks.Enabled = (b) => { return true; };
				_repairBlocks.Title = MyStringId.GetOrCompute("Repair Blocks");
				_repairBlocks.Getter = (b) => {

					var ctrls = GetControlValues(b);
					return ctrls.RepairBlocks;

				};
				_repairBlocks.Setter = (b, val) => {

					var ctrls = GetControlValues(b);
					ctrls.RepairBlocks = val;
					GetPriceQuote(b);
					b.RefreshCustomInfo();
					ControlManager.RefreshMenu(b);

				};

				_smallGridLimit = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlTextbox, IMyTerminalBlock>("MES-Shipyard-SmallGridLimit");
				_smallGridLimit.Enabled = (b) => { return false; };
				_smallGridLimit.Title = MyStringId.GetOrCompute("Small Grid Block Limit");
				_smallGridLimit.Getter = (b) => {

					var ctrls = GetControlValues(b);
					return ctrls.SmallGridLimit;

				};
				_smallGridLimit.Setter = (b, sb) => {};

				_largeGridLimit = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlTextbox, IMyTerminalBlock>("MES-Shipyard-LargeGridLimit");
				_largeGridLimit.Enabled = (b) => { return false; };
				_largeGridLimit.Title = MyStringId.GetOrCompute("Large Grid Block Limit");
				_largeGridLimit.Getter = (b) => {

					var ctrls = GetControlValues(b);
					return ctrls.LargeGridLimit;

				};
				_largeGridLimit.Setter = (b, sb) => { };

				_quoteButton = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlButton, IMyTerminalBlock>("MES-Shipyard-Quote");
				_quoteButton.Enabled = (b) => { return true; };
				_quoteButton.Title = MyStringId.GetOrCompute("Generate Quote");
				_quoteButton.Tooltip = MyStringId.GetOrCompute("Generates a quote with total costs of the requested work.");
				_quoteButton.Action = GetPriceQuote;

				_quotedPrice = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlTextbox, IMyTerminalBlock>("MES-Shipyard-QuotedPrice");
				_quotedPrice.Enabled = (b) => { return false; };
				_quotedPrice.Title = MyStringId.GetOrCompute("Quoted Price");
				_quotedPrice.Getter = (b) => {

					var ctrls = GetControlValues(b);
					return ctrls.QuotedPrice;

				};
				_quotedPrice.Setter = (b, sb) => {

					var ctrls = GetControlValues(b);
					ctrls.QuotedPrice.Clear();
					ctrls.QuotedPrice.Append(sb.ToString());

				};

				_confirmButtonBlueprint = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlButton, IMyTerminalBlock>("MES-Shipyard-ConfirmBlueprint");
				_confirmButtonBlueprint.Enabled = (b) => { return true; };
				_confirmButtonBlueprint.Title = MyStringId.GetOrCompute("Send To Store");
				_confirmButtonBlueprint.Tooltip = MyStringId.GetOrCompute("Sends currently displayed projection to store block as a purchasable item.");
				_confirmButtonBlueprint.Action = ConfirmOrder;

				_confirmButtonScrap = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlButton, IMyTerminalBlock>("MES-Shipyard-ConfirmScrap");
				_confirmButtonScrap.Enabled = (b) => { return true; };
				_confirmButtonScrap.Title = MyStringId.GetOrCompute("Sell Grid as Scrap");
				_confirmButtonScrap.Tooltip = MyStringId.GetOrCompute("Despawns the currently selected grid and adds the quoted value to your credit balance.");
				_confirmButtonScrap.Action = ConfirmOrder;

				_confirmButtonConstruct = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlButton, IMyTerminalBlock>("MES-Shipyard-ConfirmConstruct");
				_confirmButtonConstruct.Enabled = (b) => { return true; };
				_confirmButtonConstruct.Title = MyStringId.GetOrCompute("Construct / Repair Blocks");
				_confirmButtonConstruct.Tooltip = MyStringId.GetOrCompute("Constructs and/or Repairs all eligible blocks for the quoted price.");
				_confirmButtonConstruct.Action = ConfirmOrder;

				_confirmButtonTakeover = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlButton, IMyTerminalBlock>("MES-Shipyard-ConfirmTakeover");
				_confirmButtonTakeover.Enabled = (b) => { return true; };
				_confirmButtonTakeover.Title = MyStringId.GetOrCompute("Take Ownership of Grid");
				_confirmButtonTakeover.Tooltip = MyStringId.GetOrCompute("Grants you full ownership of the grid for the quoted price.");
				_confirmButtonTakeover.Action = ConfirmOrder;

				_useServerPrice = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlCheckbox, IMyTerminalBlock>("MES-Shipyard-UseServerPrice");
				_useServerPrice.Enabled = (b) => { return !MyAPIGateway.Multiplayer.IsServer; };
				_useServerPrice.Visible = (b) => { return !MyAPIGateway.Multiplayer.IsServer; };
				_useServerPrice.Title = MyStringId.GetOrCompute("Use Server Price");
				_useServerPrice.Getter = (b) => {

					var ctrls = GetControlValues(b);
					return ctrls.UseServerPrice;

				};
				_useServerPrice.Setter = (b, val) => {

					var ctrls = GetControlValues(b);
					ctrls.UseServerPrice = val;

				};

				_separatorA = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSeparator, IMyTerminalBlock>("MES-Shipyard-SeparatorA");
				_separatorB = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSeparator, IMyTerminalBlock>("MES-Shipyard-SeparatorB");
				_separatorC = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSeparator, IMyTerminalBlock>("MES-Shipyard-SeparatorC");
				_separatorD = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSeparator, IMyTerminalBlock>("MES-Shipyard-SeparatorD");
				_separatorE = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSeparator, IMyTerminalBlock>("MES-Shipyard-SeparatorE");

			}

			//Remove Private Controls
			if (MyAPIGateway.Session.LocalHumanPlayer == null || MyAPIGateway.Session.LocalHumanPlayer.IdentityId != block.OwnerId) {

				for (int i = controls.Count - 1; i >= 0; i--) {

					if (_privateControls.Contains(controls[i].Id)) {

						controls.RemoveAt(i);

					}
				
				}
			
			}

			//Register Shipyard Projector
			if (!_shipyardProjectors.Contains(block as IMyProjector))
				_shipyardProjectors.Add(block as IMyProjector);

			//Get Control Values
			ShipyardControlValues controlValues = GetControlValues(block);

			//Add Controls
			if (controlValues.Mode != ShipyardModes.BlueprintBuilding) {

				controls.Clear();

			}

			controls.Insert(0, _label);
			controls.Insert(1, _infoButton);
			controls.Insert(2, _modeSelect);
			controls.Insert(3, _separatorA);

			if (controlValues.Mode == ShipyardModes.BlueprintBuilding) {

				controls.Insert(3, _labelBlueprint);
				controls.Add(_separatorB);
				controls.Add(_labelConfirmation);
				controls.Add(_confirmButtonBlueprint);
				controls.Add(_useServerPrice);

			}

			if (controlValues.Mode == ShipyardModes.ScrapPurchasing) {

				controls.Add(_labelScrap);
				controls.Add(_gridSelect);
				controls.Add(_separatorB);
				controls.Add(_labelConfirmation);
				controls.Add(_confirmButtonScrap);
				controls.Add(_useServerPrice);

			}

			if (controlValues.Mode == ShipyardModes.RepairAndConstruction) {

				controls.Add(_labelRepair);
				controls.Add(_gridSelect);
				controls.Add(_separatorB);
				controls.Add(_constructBlocks);
				controls.Add(_repairBlocks);
				controls.Add(_separatorC);
				controls.Add(_labelConfirmation);
				controls.Add(_confirmButtonConstruct);
				controls.Add(_useServerPrice);

			}

			if (controlValues.Mode == ShipyardModes.GridTakeover) {

				controls.Add(_labelTakeover);
				controls.Add(_gridSelect);
				controls.Add(_separatorB);
				controls.Add(_labelConfirmation);
				controls.Add(_confirmButtonTakeover);
				controls.Add(_useServerPrice);

			}

		}

		public static void GetPriceQuote(IMyTerminalBlock block) {

			var controls = GetControlValues(block);
			controls.QuotedPriceValue = -1;

			var faction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(block.OwnerId);
			var rep = MyAPIGateway.Session.Factions.GetReputationBetweenPlayerAndFaction(MyAPIGateway.Session.LocalHumanPlayer?.IdentityId ?? 0, faction?.FactionId ?? 0);

			if (controls.Mode == ShipyardModes.BlueprintBuilding) {

				var projector = block as IMyProjector;

				if (projector != null && projector.IsProjecting && projector.ProjectedGrid != null) {

					var gridEntity = GridManager.GetGridEntity(projector.ProjectedGrid, true);

					if (gridEntity != null) {

						controls.SelectedGridItem = gridEntity;
						controls.QuotedPriceValue = gridEntity.CreditValueRegular(false, true);

						if (controls.QuotedPriceValue >= 0) {

							controls.QuotedPriceValue = controls.Profile.GetBlueprintPrice(controls.QuotedPriceValue, rep);

						}

					}

				}

			}

			if (controls.Mode == ShipyardModes.ScrapPurchasing) {

				if (controls.SelectedGridItem == null || !controls.SelectedGridItem.ActiveEntity()) {

					//Nothing

				} else {

					controls.QuotedPriceValue = controls.SelectedGridItem.CreditValueRegular(controls.Profile.ScrapPurchasingIncludeInventory);

					if (controls.QuotedPriceValue >= 0) {

						controls.QuotedPriceValue = controls.Profile.GetScrapPrice(controls.QuotedPriceValue, rep);

					}

					if (controls.QuotedPriceValue == 1)
						controls.QuotedPriceValue = 0;
				}

			}

			if (controls.Mode == ShipyardModes.RepairAndConstruction) {

				if (controls.SelectedGridItem == null || !controls.SelectedGridItem.ActiveEntity()) {

					//Nothing

				} else {

					if(controls.RepairBlocks)
						controls.QuotedPriceValue += controls.SelectedGridItem.CreditValueRepair();

					int blockCount = 0;

					if(controls.ConstructNewBlocks)
						controls.QuotedPriceValue += controls.SelectedGridItem.CreditValueProjectedBlocksBuildable(out blockCount);

					if (controls.QuotedPriceValue >= 0) {

						controls.QuotedPriceValue++;
						controls.QuotedPriceValue = controls.Profile.GetRepairPrice(controls.QuotedPriceValue, rep);

					}

					if (controls.QuotedPriceValue == 1)
						controls.QuotedPriceValue = 0;


				}

			}

			if (controls.Mode == ShipyardModes.GridTakeover) {

				if (controls.SelectedGridItem == null || !controls.SelectedGridItem.ActiveEntity()) {

					//Nothing

				} else {

					controls.QuotedPriceValue = EconomyHelper.GridTakeoverCost(controls.SelectedGridItem, MyAPIGateway.Session.LocalHumanPlayer?.IdentityId ?? 0, controls.Profile.GridTakeoverPricePerComputerMultiplier);

					if (controls.QuotedPriceValue >= 0) {

						controls.QuotedPriceValue = controls.Profile.GetTakeoverPrice(controls.QuotedPriceValue, rep);

					}

					if (controls.QuotedPriceValue == 1)
						controls.QuotedPriceValue = 0;


				}

			}

			controls.QuotedPrice.Clear();
			controls.QuotedPrice.Append(controls.QuotedPriceValue > 0 ? controls.QuotedPriceValue.ToString() : "N/A");

			_quotedPrice.UpdateVisual();

		}


		public static void ConfirmOrder(IMyTerminalBlock block) {

			var controls = GetControlValues(block);
			var transaction = new ShipyardTransaction();
			transaction.ProjectorId = block.EntityId;
			transaction.ClientPrice = controls.QuotedPriceValue;
			transaction.TargetGridId = controls.SelectedGridItem?.CubeGrid?.EntityId ?? 0;
			transaction.Mode = controls.Mode;
			transaction.ConstructBlocks = controls.ConstructNewBlocks;
			transaction.RepairBlocks = controls.RepairBlocks;
			transaction.UseServerPrice = controls.UseServerPrice;
			var data = MyAPIGateway.Utilities.SerializeToBinary<ShipyardTransaction>(transaction);
			var container = new SyncContainer(SyncMode.ShipyardTransaction, data);
			SyncManager.SendSyncMesage(container, 0, true);

		}

		internal static ShipyardControlValues GetControlValues(IMyTerminalBlock block) {

			ShipyardControlValues controlValues = null;

			if (!_controlValues.TryGetValue(block.EntityId, out controlValues)) {

				controlValues = new ShipyardControlValues(block);
				_controlValues.Add(block.EntityId, controlValues);

			}

			return controlValues;

		}

		internal static void ShipyardInfo(IMyTerminalBlock block) {

			var sb = new StringBuilder();
			sb.Append("The Modular Encounters Systems (MES) Shipyard System allows you to complete grid level transactions, depending on what the merchant allows. Below are the different modes available, and how to use them:").AppendLine();

			sb.AppendLine().Append("[Scrap Purchasing]").AppendLine();
			sb.Append("This allows the Merchant Shipyard to purchase one of your grids as scrap, and providing a percentage of the grid's estimated value to the player that initiates the transaction. Some merchants may also include the value of the cargo in the grid as well.").AppendLine();
			sb.Append(" - To initiate this transaction, choose [Scrap Purchasing] from the mode select.").AppendLine();
			sb.Append(" - A terminal control will appear that will allow you to select a grid. Only grids that are a short distance from the merchant, and are also majority-owned by the player requesting the transaction will appear in this list.").AppendLine();
			sb.Append(" - Once you've selected a grid, you will get a price quote in the info pane of the terminal (bottom right).").AppendLine();
			sb.Append(" - If the price is agreeable to you, then press the [Sell Grid as Scrap] button to complete the transaction.").AppendLine();
			sb.Append(" - The credits will be deposited into your player balance and the grid will be removed from the world.").AppendLine();

			sb.AppendLine().Append("[Repair and Construction]").AppendLine();
			sb.Append("This allows you to use the Merchant Shipyard for repairing damaged / incomplete blocks, or constructing new block from a projection on one of your near-by grids. When requesting this work, the Shipyard will attempt to repair/construct as many blocks as it currently can in one transaction.").AppendLine();
			sb.Append(" - To initiate this transaction, choose [Repair and Construction] from the mode select").AppendLine();
			sb.Append(" - A terminal control will appear that will allow you to select a grid. Only grids that are a short distance from the merchant, and are also majority-owned by the player requesting the transaction will appear in this list.").AppendLine();
			sb.Append(" - Once you've chosen a grid, select the types of work you want done on your ship by using the [Construct New Blocks] and [Repair Blocks] checkboxes.").AppendLine();
			sb.Append(" - Once you've made your selections, you will get a price quote in the info pane of the terminal (bottom right).").AppendLine();
			sb.Append(" - If the price is agreeable to you, then press the [Construct / Repair Blocks] button to complete the transaction.").AppendLine();
			sb.Append(" - The credits will be withdrawn from your player account, and the requested work will be performed on your selected grid.").AppendLine();

			sb.AppendLine().Append("[Grid Takeover]").AppendLine();
			sb.Append("This allows you to use the Merchant Shipyard to take complete ownership of another grid. The grid being taken over must have at least 1 block owned by an identity other than yourself (and is also not part of your faction). The cost of the takeover operation is based on the amount of computer components present in the blocks you do not already control.").AppendLine();
			sb.Append(" - To initiate this transaction, choose [Grid Takeover] from the mode select.").AppendLine();
			sb.Append(" - A terminal control will appear that will allow you to select a nearby grid with mixed ownership.").AppendLine();
			sb.Append(" - Once you have selected a grid, you will receive a price quite in the info pane of the terminal (bottom right).").AppendLine();
			sb.Append(" - If the terms are agreeable to you, then press the [Take Ownership of Grid] button to complete the transaction.").AppendLine();
			sb.Append(" - The credits will be removed from your player balance, and the grid will now be fully under your ownership.").AppendLine();
			MyAPIGateway.Utilities.ShowMissionScreen("Shipyard System (MES)", "", "Information and Help", sb.ToString());

		}

		internal static void ModeListContent(IMyTerminalBlock block, List<MyTerminalControlListBoxItem> items, List<MyTerminalControlListBoxItem> selected) {

			var controls = GetControlValues(block);

			var noneItem = new MyTerminalControlListBoxItem(MyStringId.GetOrCompute("None"), MyStringId.GetOrCompute("None"), ShipyardModes.None);
			items.Add(noneItem);

			if (controls.Mode == ShipyardModes.None)
				selected.Add(noneItem);

			if (controls.Profile != null) {

				//MyVisualScriptLogicProvider.ShowNotificationToAll("Got Profile", 5000);

				//AllowBlueprintBuilding
				if (controls.Profile.AllowBlueprintBuilding) {

					var item = new MyTerminalControlListBoxItem(MyStringId.GetOrCompute("Blueprint Buildng"), MyStringId.GetOrCompute("Blueprint Buildng"), ShipyardModes.BlueprintBuilding);
					items.Add(item);

					controls.SmallGridLimit.Clear();
					controls.SmallGridLimit.Append(controls.Profile.BlueprintBuildingSmallGridBlockLimit.ToString());
					controls.LargeGridLimit.Clear();
					controls.LargeGridLimit.Append(controls.Profile.BlueprintBuildingLargeGridBlockLimit.ToString());

					if (controls.Mode == ShipyardModes.BlueprintBuilding)
						selected.Add(item);

				}

				//AllowScrapPurchasing
				if (controls.Profile.AllowScrapPurchasing) {

					var item = new MyTerminalControlListBoxItem(MyStringId.GetOrCompute("Scrap Purchasing"), MyStringId.GetOrCompute("Scrap Purchasing"), ShipyardModes.ScrapPurchasing);
					items.Add(item);

					controls.SmallGridLimit.Clear();
					controls.SmallGridLimit.Append(controls.Profile.ScrapPurchasingSmallGridBlockLimit.ToString());
					controls.LargeGridLimit.Clear();
					controls.LargeGridLimit.Append(controls.Profile.ScrapPurchasingLargeGridBlockLimit.ToString());

					if (controls.Mode == ShipyardModes.ScrapPurchasing)
						selected.Add(item);

				}

				//AllowRepairAndConstruction
				if (controls.Profile.AllowRepairAndConstruction) {

					var item = new MyTerminalControlListBoxItem(MyStringId.GetOrCompute("Repair and Construction"), MyStringId.GetOrCompute("Repair and Construction"), ShipyardModes.RepairAndConstruction);
					items.Add(item);

					controls.SmallGridLimit.Clear();
					controls.SmallGridLimit.Append(controls.Profile.RepairAndConstructionSmallGridBlockLimit.ToString());
					controls.LargeGridLimit.Clear();
					controls.LargeGridLimit.Append(controls.Profile.RepairAndConstructionLargeGridBlockLimit.ToString());

					if (controls.Mode == ShipyardModes.RepairAndConstruction)
						selected.Add(item);

				}

				//AllowGridTakeover
				if (controls.Profile.AllowGridTakeover) {

					var item = new MyTerminalControlListBoxItem(MyStringId.GetOrCompute("Grid Takeover"), MyStringId.GetOrCompute("Grid Takeover"), ShipyardModes.GridTakeover);
					items.Add(item);

					controls.SmallGridLimit.Clear();
					controls.SmallGridLimit.Append(controls.Profile.GridTakeoverSmallGridBlockLimit.ToString());
					controls.LargeGridLimit.Clear();
					controls.LargeGridLimit.Append(controls.Profile.GridTakeoverLargeGridBlockLimit.ToString());

					if (controls.Mode == ShipyardModes.GridTakeover)
						selected.Add(item);

				}

			}

		}

		internal static void ModeListSelected(IMyTerminalBlock block, List<MyTerminalControlListBoxItem> selected) {

			var controls = GetControlValues(block);

			if (selected.Count == 0)
				return;

			controls.Mode = (ShipyardModes)selected[0].UserData;

			controls.SelectedGridItem = null;
			GetPriceQuote(block);
			block.RefreshCustomInfo();
			ControlManager.RefreshMenu(block);

		}

		internal static void GridSelectionListContent(IMyTerminalBlock block, List<MyTerminalControlListBoxItem> items, List<MyTerminalControlListBoxItem> selected) {

			var controls = GetControlValues(block);

			var noneItem = new MyTerminalControlListBoxItem(MyStringId.GetOrCompute("None"), MyStringId.GetOrCompute("None"), null);
			items.Add(noneItem);

			if (controls.SelectedGridItem == null || !controls.SelectedGridItem.ActiveEntity())
				selected.Add(noneItem);

			_grids.Clear();
			GridManager.GetGridsWithinDistance(block.GetPosition(), controls.Profile.InteractionRadius, _grids);
			var playerid = MyAPIGateway.Session?.LocalHumanPlayer?.IdentityId ?? 0;
			var playerFaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(playerid);

			for (int i = _grids.Count - 1; i >= 0; i--) {

				var grid = _grids[i];

				if (block.SlimBlock.CubeGrid == grid.CubeGrid || grid?.CubeGrid?.BigOwners == null || !GridSelectionRules(block, grid, controls.Mode, controls.Profile)) {

					_grids.RemoveAt(i);
					continue;

				}

				if (controls.Mode != ShipyardModes.GridTakeover) {

					if (!grid.CubeGrid.BigOwners.Contains(playerid) ) {

						_grids.RemoveAt(i);
						continue;

					}

				} else {

					bool nonFactionOwnership = false;
					bool hasPlayerOwnershipSomewhere = false;

					if (grid.CubeGrid.BigOwners.Contains(playerid))
						hasPlayerOwnershipSomewhere = true;

					foreach (var owner in grid.CubeGrid.BigOwners) {

						if (owner == playerid || owner == 0)
							continue;

						if (playerFaction == null || !playerFaction.IsMember(owner)) {

							nonFactionOwnership = true;
							break;
						
						}
					
					}

					if (grid.CubeGrid.SmallOwners != null) {

						if (grid.CubeGrid.SmallOwners.Contains(playerid))
							hasPlayerOwnershipSomewhere = true;

						foreach (var owner in grid.CubeGrid.SmallOwners) {

							if (owner == playerid || owner == 0)
								continue;

							if (playerFaction == null || !playerFaction.IsMember(owner)) {

								nonFactionOwnership = true;
								break;

							}

						}

					}

					if (!nonFactionOwnership || !hasPlayerOwnershipSomewhere) {

						_grids.RemoveAt(i);
						continue;

					}
				
				}
				
			
			}

			foreach (var grid in _grids) {

				var gridName = !string.IsNullOrWhiteSpace(grid?.CubeGrid?.CustomName) ? grid.CubeGrid.CustomName : "[Unnamed Grid]";
				var item = new MyTerminalControlListBoxItem(MyStringId.GetOrCompute(gridName), MyStringId.GetOrCompute(gridName), grid);
				items.Add(item);

				if (grid == controls.SelectedGridItem)
					selected.Add(item);

			}

		}

		public static bool GridSelectionRules(IMyTerminalBlock block, GridEntity grid, ShipyardModes mode, ShipyardProfile profile, int blockCount = -1, int additionalBlocks = 0) {

			var count = blockCount > -1 ? blockCount : grid.AllBlocks.Count;
			count -= additionalBlocks;
			var size = grid.CubeGrid.GridSizeEnum;

			if (mode == ShipyardModes.BlueprintBuilding) {

				if (size == MyCubeSize.Small && count > profile.BlueprintBuildingSmallGridBlockLimit)
					return false;

				if (size == MyCubeSize.Large && count > profile.BlueprintBuildingLargeGridBlockLimit)
					return false;

			}

			if (mode == ShipyardModes.ScrapPurchasing) {

				if (size == MyCubeSize.Small && count > profile.ScrapPurchasingSmallGridBlockLimit)
					return false;

				if (size == MyCubeSize.Large && count > profile.ScrapPurchasingLargeGridBlockLimit)
					return false;

			}

			if (mode == ShipyardModes.RepairAndConstruction) {

				if (size == MyCubeSize.Small && count > profile.RepairAndConstructionSmallGridBlockLimit)
					return false;

				if (size == MyCubeSize.Large && count > profile.RepairAndConstructionLargeGridBlockLimit)
					return false;

			}

			if (mode == ShipyardModes.GridTakeover) {

				if (size == MyCubeSize.Small && count > profile.GridTakeoverSmallGridBlockLimit)
					return false;

				if (size == MyCubeSize.Large && count > profile.GridTakeoverLargeGridBlockLimit)
					return false;

			}

			return true;

		}

		internal static void GridSelection(IMyTerminalBlock block, List<MyTerminalControlListBoxItem> selected) {

			var controls = GetControlValues(block);

			if (selected.Count == 0)
				return;

			controls.SelectedGridItem = (GridEntity)selected[0].UserData;
			GetPriceQuote(block);
			block.RefreshCustomInfo();
			ControlManager.RefreshMenu(block);

		}

		public static void NewGrid(GridEntity grid) {

			lock (_shipyardProjectors) {

				MyAPIGateway.Utilities.InvokeOnGameThread(() => {

					foreach (var projector in _shipyardProjectors) {

						if (grid.CubeGrid?.Physics == null && grid.Distance(projector.GetPosition()) < 100) {

							var controls = GetControlValues(projector);

							if (controls != null) {

								controls.SelectedGridItem = grid;
								projector.RefreshCustomInfo();
								ControlManager.RefreshMenu(projector);
								break;

							}
							
						}

					}

				});

			}
		
		}

		


	}

}
