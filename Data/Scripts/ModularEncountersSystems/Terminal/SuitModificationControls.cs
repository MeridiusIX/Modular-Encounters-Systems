using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Progression;
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
				_purchasableMods.VisibleRowsCount = 4;
				_purchasableMods.ListContent = GetSuitModsList;
				_purchasableMods.ItemSelected = SelectSuitModeFromList;

				_confirmPurchase = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlButton, IMyTerminalBlock>("MES-SuitMods-ConfirmPurchase");
				_confirmPurchase.Enabled = (b) => { return true; };
				_confirmPurchase.Title = MyStringId.GetOrCompute("Send To Store");
				_confirmPurchase.SupportsMultipleBlocks = false;
				_confirmPurchase.Tooltip = MyStringId.GetOrCompute("Purchases currently selected suit modification.");
				_confirmPurchase.Action = ConfirmPurchase;

				_separatorA = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSeparator, IMyTerminalBlock>("MES-SuitMods-SeparatorA");
				_separatorB = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSeparator, IMyTerminalBlock>("MES-SuitMods-SeparatorB");

				if (MyAPIGateway.Session.LocalHumanPlayer != null) {

					_progressionStats = PlayerManager.GetProgressionContainer(MyAPIGateway.Session.LocalHumanPlayer.IdentityId, MyAPIGateway.Session.LocalHumanPlayer.SteamUserId);
				
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

			//Refresh Progression Container

			//TODO: Send To Server
			_lastSelectedBlock = block;

		}

		public static void GetSuitModsList(IMyTerminalBlock block, List<MyTerminalControlListBoxItem> items, List<MyTerminalControlListBoxItem> selected) {

			if (_progressionStats == null)
				return;
		
		}

		public static void SelectSuitModeFromList(IMyTerminalBlock block, List<MyTerminalControlListBoxItem> selected) {

			if (_progressionStats == null)
				return;

		}

		public static void ConfirmPurchase(IMyTerminalBlock block) {

			if (_progressionStats == null)
				return;

		}

		internal static void SuitModsInfo(IMyTerminalBlock block) {

			var sb = new StringBuilder();
			sb.Append("");
			MyAPIGateway.Utilities.ShowMissionScreen("Shipyard System (MES)", "", "Information and Help", sb.ToString());

		}

		public static void CustomInfo(IMyTerminalBlock block, StringBuilder sb) {

			sb.Clear();

			var upgrade = SelectedSuitMod[block];
			var credits = _progressionStats.GetUpgradeCreditCost(upgrade);
			var points = _progressionStats.GetUpgradeResearchCost(upgrade);
			var level = _progressionStats.GetUpgradeLevel(upgrade);

			sb.Append("Selected Upgrade: ").AppendLine().Append(upgrade.ToString() ?? "Error").AppendLine().AppendLine();
			sb.Append("Credit Cost: ").AppendLine().Append(credits > 0 ? credits.ToString("C0") : "N/A").AppendLine();
			sb.Append("Research Points Cost: ").Append(points > 0 ? points.ToString() : "N/A").AppendLine().AppendLine();
			sb.Append("Current Upgrade Level: ").Append(level >= 4 ? "MAX" : level.ToString()).AppendLine();
			sb.Append("Next Upgrade Level: ").Append(level + 1 >= 4 ? "MAX" : (level + 1).ToString()).AppendLine();

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
