using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Progression;
using ModularEncountersSystems.Sync;
using Sandbox.Game;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.ModAPI;
using VRage.Utils;

namespace ModularEncountersSystems.Terminal {

	public static class SuitModificationControls {

		internal static bool _setupDone = false;
		internal static List<string> _privateControls = new List<string>();
		internal static Dictionary<IMyTerminalBlock, SuitUpgradeTypes> SelectedSuitMod = new Dictionary<IMyTerminalBlock, SuitUpgradeTypes>();
		internal static ProgressionContainer _progressionStats;
		internal static IMyTerminalBlock _lastSelectedBlock;

		internal static IMyTerminalControlLabel _labelControls;
		internal static IMyTerminalControlButton _infoButton;
		internal static IMyTerminalControlSeparator _separatorA;

		internal static IMyTerminalControlLabel _labelPurchaseMod;
		internal static IMyTerminalControlListbox _purchasableMods;
		internal static IMyTerminalControlSeparator _separatorB;
		internal static IMyTerminalControlButton _confirmPurchase;

		internal static IMyTerminalAction _actionA;
		internal static IMyTerminalAction _actionB;
		internal static IMyTerminalAction _actionC;
		internal static IMyTerminalAction _actionD;
		internal static IMyTerminalAction _actionE;

		public static void DisplayControls(IMyTerminalBlock block, List<IMyTerminalControl> controls) {

			if (!_setupDone) {

				_setupDone = true;

				//Hidden Controls Names
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
				_privateControls.Add("AnyoneCanUse");
				_privateControls.Add("Open Toolbar");
				_privateControls.Add("ButtonText");
				_privateControls.Add("ButtonName");

				//Create Controls
				_labelControls = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlLabel, IMyTerminalBlock>("MES-SuitMods-Label");
				_labelControls.Enabled = (b) => { return true; };
				_labelControls.Label = MyStringId.GetOrCompute("Suit Modifications");

				_infoButton = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlButton, IMyTerminalBlock>("MES-SuitMods-Quote");
				_infoButton.Enabled = (b) => { return true; };
				_infoButton.SupportsMultipleBlocks = false;
				_infoButton.Title = MyStringId.GetOrCompute("Help / Info");
				_infoButton.Tooltip = MyStringId.GetOrCompute("Click here for information on how to use the Suit Modifications System.");
				_infoButton.Action = SuitModsInfo;

				_purchasableMods = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlListbox, IMyTerminalBlock>("MES-SuitMods-ModSelect");
				_purchasableMods.Enabled = (b) => { return true; };
				_purchasableMods.Title = MyStringId.GetOrCompute("Suit Mod Selection");
				_purchasableMods.SupportsMultipleBlocks = false;
				_purchasableMods.Multiselect = false;
				_purchasableMods.VisibleRowsCount = 10;
				_purchasableMods.ListContent = GetSuitModsList;
				_purchasableMods.ItemSelected = SelectSuitModeFromList;

				_confirmPurchase = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlButton, IMyTerminalBlock>("MES-SuitMods-ConfirmPurchase");
				_confirmPurchase.Enabled = (b) => { return true; };
				_confirmPurchase.Title = MyStringId.GetOrCompute("Purchase Upgrade");
				_confirmPurchase.SupportsMultipleBlocks = false;
				_confirmPurchase.Tooltip = MyStringId.GetOrCompute("Purchases currently selected suit upgrade.");
				_confirmPurchase.Action = ConfirmPurchase;

				_separatorA = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSeparator, IMyTerminalBlock>("MES-SuitMods-SeparatorA");
				_separatorB = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSeparator, IMyTerminalBlock>("MES-SuitMods-SeparatorB");

				_actionA = MyAPIGateway.TerminalControls.CreateAction<IMyTerminalBlock>("MES-SuitMods-ActionA");
				_actionA.Enabled = (b) => { return true; };
				_actionA.ValidForGroups = false;
				_actionA.Name = new StringBuilder("Up");
				_actionA.Action = BlankAction;

				_actionB = MyAPIGateway.TerminalControls.CreateAction<IMyTerminalBlock>("MES-SuitMods-ActionB");
				_actionB.Enabled = (b) => { return true; };
				_actionB.ValidForGroups = false;
				_actionB.Name = new StringBuilder("Down");
				_actionB.Action = BlankAction;

				_actionC = MyAPIGateway.TerminalControls.CreateAction<IMyTerminalBlock>("MES-SuitMods-ActionC");
				_actionC.Enabled = (b) => { return true; };
				_actionC.ValidForGroups = false;
				_actionC.Name = new StringBuilder("Left");
				_actionC.Action = BlankAction;

				_actionD = MyAPIGateway.TerminalControls.CreateAction<IMyTerminalBlock>("MES-SuitMods-ActionD");
				_actionD.Enabled = (b) => { return true; };
				_actionD.ValidForGroups = false;
				_actionD.Name = new StringBuilder("Right");
				_actionD.Action = BlankAction;

				_actionE = MyAPIGateway.TerminalControls.CreateAction<IMyTerminalBlock>("MES-SuitMods-ActionE");
				_actionE.Enabled = (b) => { return true; };
				_actionE.ValidForGroups = false;
				_actionE.Name = new StringBuilder("Enter");
				_actionE.Action = BlankAction;

				if (MyAPIGateway.Multiplayer.IsServer && MyAPIGateway.Session.LocalHumanPlayer != null) {

					_progressionStats = PlayerManager.GetProgressionContainer(MyAPIGateway.Session.LocalHumanPlayer.IdentityId, MyAPIGateway.Session.LocalHumanPlayer.SteamUserId);

				} else {

					var syncContainer = new SyncContainer(SyncMode.SuitUpgradeNewPlayerStats, null);
					syncContainer.IdentityId = MyAPIGateway.Session.LocalHumanPlayer?.IdentityId ?? 0;
					syncContainer.SteamId = MyAPIGateway.Session.LocalHumanPlayer?.SteamUserId ?? 0;
					SyncManager.SendSyncMesage(syncContainer, 0, true);

				}

			}

			//Remove Private Controls
			if (MyAPIGateway.Session.LocalHumanPlayer == null || MyAPIGateway.Session.LocalHumanPlayer.IdentityId != block.OwnerId) {

				for (int i = controls.Count - 1; i >= 0; i--) {

					if (_privateControls.Contains(controls[i].Id)) {

						controls.RemoveAt(i);

					}

				}

			}

			//Register Controls
			if (!SelectedSuitMod.ContainsKey(block)) {

				SelectedSuitMod.Add(block, SuitUpgradeTypes.None);
				block.AppendingCustomInfo += CustomInfo;

			}

			//Insert Controls
			controls.Insert(0, _labelControls);
			controls.Insert(1, _infoButton);
			controls.Insert(2, _separatorA);
			controls.Insert(3, _purchasableMods);
			controls.Insert(4, _confirmPurchase);

			_lastSelectedBlock = block;

			/*
			if (_progressionStats != null) {

				var serializedProgression = MyAPIGateway.Utilities.SerializeToBinary<ProgressionContainer>(_progressionStats);
				var syncContainer = new SyncContainer(SyncMode.SuitUpgradePlayerStats, serializedProgression);
				SyncManager.SendSyncMesage(syncContainer, 0, true);

			} else {

				//Request New Progression Stats
				var syncContainer = new SyncContainer(SyncMode.SuitUpgradeNewPlayerStats, null);
				SyncManager.SendSyncMesage(syncContainer, 0, true);

			}
			*/
		}

		public static void BlankAction(IMyTerminalBlock block) { }

		public static void GetSuitModsList(IMyTerminalBlock block, List<MyTerminalControlListBoxItem> items, List<MyTerminalControlListBoxItem> selected) {

			if (_progressionStats == null)
				return;

			SuitUpgradeTypes selectedType = SuitUpgradeTypes.None;
			SelectedSuitMod.TryGetValue(block, out selectedType);

			foreach (SuitUpgradeTypes type in Enum.GetValues(typeof(SuitUpgradeTypes))) {

				var name = ProgressionContainer.GetUpgradeName(type);

				if (string.IsNullOrWhiteSpace(name) || !ProgressionContainer.IsUpgradeAllowedInConfig(type))
					continue;

				var item = new MyTerminalControlListBoxItem(MyStringId.GetOrCompute(name), MyStringId.GetOrCompute(ProgressionContainer.GetUpgradeDescriptions(type)), type);
				items.Add(item);

				if (selectedType == type)
					selected.Add(item);

			}

		}

		public static void SelectSuitModeFromList(IMyTerminalBlock block, List<MyTerminalControlListBoxItem> selected) {

			if (_progressionStats == null || selected.Count == 0)
				return;

			SelectedSuitMod[block] = (SuitUpgradeTypes)selected[0].UserData;
			block.RefreshCustomInfo();
			MyAPIGateway.Utilities.InvokeOnGameThread(() => { ControlManager.RefreshMenu(block); });
			
		}

		public static void ConfirmPurchase(IMyTerminalBlock block) {

			if (_progressionStats == null)
				return;

			var upgrade = SelectedSuitMod[block];

			if (upgrade == SuitUpgradeTypes.None)
				return;

			var transaction = new SuitUpgradeTransaction();
			transaction.BlockId = block.EntityId;
			transaction.IdentityId = _progressionStats.IdentityId;
			transaction.Upgrade = upgrade;
			var syncContainer = new SyncContainer(SyncMode.SuitUpgradeTransaction, MyAPIGateway.Utilities.SerializeToBinary<SuitUpgradeTransaction>(transaction));
			SyncManager.SendSyncMesage(syncContainer, 0, true);

		}

		internal static void SuitModsInfo(IMyTerminalBlock block) {

			var sb = new StringBuilder();
			sb.Append("The Modular Encounters Systems (MES) Suit Upgrade System allows you to purchase Permanent Upgrades for your Engineer Character. The upgrades require a combination of Space Credits and Research Points (the latter can be downloaded from Special Terminals on other NPC grids).").AppendLine();

			sb.AppendLine().Append("[About Upgrades]").AppendLine();
			sb.Append("Below is a list of the current upgrades that may be available for purchase at this upgrade station.").AppendLine();
			sb.Append(" - Anti Jetpack Inhibitor: This provides immunity from Jetpack Inhibitors at the expense of suit energy. When this upgrade is at max level, it provides immunity at zero energy cost.").AppendLine();
			sb.Append(" - Anti Hand Drill Inhibitor: This provides immunity from Hand Drill Inhibitors at the expense of suit energy. When this upgrade is at max level, it provides immunity at zero energy cost.").AppendLine();
			sb.Append(" - Anti Hand Drill Inhibitor: This provides immunity from Personnel Inhibitors at the expense of suit energy. When this upgrade is at max level, it provides immunity at zero energy cost.").AppendLine();
			sb.Append(" - Anti Energy Inhibitor: This provides immunity from Energy Inhibitors in the form of reduced suit energy consumption. When this upgrade is at max level, it provides immunity at zero energy cost.").AppendLine();
			sb.Append(" - Solar Charging: This allows you to passively recharge suit energy while in direct sunlight. Higher levels of this upgrade allow faster charging.").AppendLine();
			sb.Append(" - Damage Reduction: This upgrade reduces the amount of damage your character takes from all sources. Higher levels of this upgrade provide further damage reduction.").AppendLine();

			sb.AppendLine().Append("[Purchasing an Upgrade]").AppendLine();
			sb.Append(" - To initiate this transaction, select an item in the [Suit Mod Selection] menu.").AppendLine();
			sb.Append(" - Once you have selected an upgrade, you will receive a price quote in the info pane of the terminal (bottom right). This will indicate the Space Credit and Research Point costs, along with your current upgrade level.").AppendLine();
			sb.Append(" - If the terms are agreeable to you, then press the [Purchase Upgrade] button to complete the transaction.").AppendLine();
			sb.Append(" - The credits will be removed from your player balance, and the upgrade will now be activated for your character.").AppendLine();

			MyAPIGateway.Utilities.ShowMissionScreen("Shipyard System (MES)", "", "Information and Help", sb.ToString());

		}

		public static void CustomInfo(IMyTerminalBlock block, StringBuilder sb) {

			sb.Clear();

			var upgrade = SelectedSuitMod[block];
			var name = ProgressionContainer.GetUpgradeName(upgrade);
			var credits = _progressionStats.GetUpgradeCreditCost(upgrade);
			var points = _progressionStats.GetUpgradeResearchCost(upgrade);
			var level = _progressionStats.GetUpgradeLevel(upgrade);

			sb.Append("Selected Upgrade: ").AppendLine().Append(name ?? "Error").AppendLine().AppendLine();
			sb.Append("Credit Cost: ").AppendLine().Append(credits > 0 ? credits.ToString("C0") : "N/A").AppendLine();
			sb.Append("Research Points Balance / Cost: ").Append(points > 0 ? (_progressionStats.Points.ToString() + " / " + points.ToString()) : "N/A").AppendLine().AppendLine();
			sb.Append("Current Upgrade Level: ").Append(level >= 5 ? "MAX" : level.ToString()).AppendLine();
			sb.Append("Next Upgrade Level: ").Append(level + 1 >= 5 ? "MAX" : (level + 1).ToString()).AppendLine();

		}

		public static void UpdateProgressionContainer(ProgressionContainer container) {

			if (_progressionStats == null || !container.CompareValues(_progressionStats)) {

				_progressionStats = container;

				if (_lastSelectedBlock != null) {

					_lastSelectedBlock.RefreshCustomInfo();
					ControlManager.RefreshMenu(_lastSelectedBlock);

				}
				
			}
		
		}

	}

}
