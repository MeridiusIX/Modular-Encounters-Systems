using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Progression;
using ModularEncountersSystems.Spawning.Procedural.Builder;
using ModularEncountersSystems.Sync;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace ModularEncountersSystems.Terminal {

	public static class ProceduralShipControls {

		internal static bool _setupDone = false;
		internal static List<string> _privateControls = new List<string>();

		internal static bool _debug = true;

		internal static IMyTerminalControlListbox _controlDebugBlockList;
		internal static IMyTerminalControlButton _controlDebugAddBlock;
		internal static IMyTerminalControlButton _controlDebugYaw;

		internal static IMyTerminalControlSlider _controlDebugMinX;
		internal static IMyTerminalControlSlider _controlDebugMinY;
		internal static IMyTerminalControlSlider _controlDebugMinZ;

		internal static List<MyCubeBlockDefinition> _debugSingleBlockDefinitions;
		internal static MyDefinitionId _debugSelectedBlock;
		internal static Vector3I _debugMin;

		public static void DisplayControls(IMyTerminalBlock block, List<IMyTerminalControl> controls) {

			if (!_setupDone) {

				_setupDone = true;

				//Debug
				_debugSingleBlockDefinitions = new List<MyCubeBlockDefinition>();
				var size = new Vector3I(1, 1, 1);

				foreach (var def in DefinitionHelper.AllBlockDefinitions) {

					if (!def.Public || def.Size != size)
						continue;

					_debugSingleBlockDefinitions.Add(def);

				}

				_debugSelectedBlock = new MyDefinitionId(typeof(MyObjectBuilder_RemoteControl), "LargeBlockRemoteControl");
				_debugMin = Vector3I.Zero;

				//Hidden Controls Names
				_privateControls.Add("");
				_privateControls.Add("OnOff");
				_privateControls.Add("ShowInTerminal");
				_privateControls.Add("ShowInInventory");
				_privateControls.Add("ShowInToolbarConfig");
				_privateControls.Add("Name");
				_privateControls.Add("ShowOnHUD");
				_privateControls.Add("CustomData");
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
				_controlDebugBlockList = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlListbox, IMyTerminalBlock>("MES-ProcShipControls-DebugBlockList");
				_controlDebugBlockList.Enabled = (b) => { return _debug; };
				_controlDebugBlockList.Visible = (b) => { return _debug; };
				_controlDebugBlockList.Title = MyStringId.GetOrCompute("Select Block To Spawn");
				_controlDebugBlockList.SupportsMultipleBlocks = false;
				_controlDebugBlockList.Multiselect = false;
				_controlDebugBlockList.VisibleRowsCount = 10;
				_controlDebugBlockList.ListContent = DebugGetSingleBlocks;
				_controlDebugBlockList.ItemSelected = DebugSelectSingleBlocks;

				_controlDebugMinX = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSlider, IMyTerminalBlock>("MES-ProcShipControls-DebugMinSliderX");
				_controlDebugMinX.Enabled = (b) => { return _debug; };
				_controlDebugMinX.Title = MyStringId.GetOrCompute("Pos X");
				_controlDebugMinX.SetLimits(-10, 10);
				_controlDebugMinX.Getter = (b) => { return _debugMin.X; };
				_controlDebugMinX.Setter = (b, val) => { _debugMin.X = (int)Math.Round(val); };

				_controlDebugMinY = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSlider, IMyTerminalBlock>("MES-ProcShipControls-DebugMinSliderY");
				_controlDebugMinY.Enabled = (b) => { return _debug; };
				_controlDebugMinY.Title = MyStringId.GetOrCompute("Pos Y");
				_controlDebugMinY.SetLimits(-10, 10);
				_controlDebugMinY.Getter = (b) => { return _debugMin.Y; };
				_controlDebugMinY.Setter = (b, val) => { _debugMin.Y = (int)Math.Round(val); };

				_controlDebugMinZ = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSlider, IMyTerminalBlock>("MES-ProcShipControls-DebugMinSliderZ");
				_controlDebugMinZ.Enabled = (b) => { return _debug; };
				_controlDebugMinZ.Title = MyStringId.GetOrCompute("Pos Z");
				_controlDebugMinZ.SetLimits(-10, 10);
				_controlDebugMinZ.Getter = (b) => { return _debugMin.Z; };
				_controlDebugMinZ.Setter = (b, val) => { _debugMin.Z = (int)Math.Round(val); };

				_controlDebugAddBlock = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlButton, IMyTerminalBlock>("MES-ProcShipControls-DebugAddBlockButton");
				_controlDebugAddBlock.Enabled = (b) => { return _debug; };
				_controlDebugAddBlock.Visible = (b) => { return _debug; };
				_controlDebugAddBlock.Title = MyStringId.GetOrCompute("Add Block");
				_controlDebugAddBlock.Action = DebugAddBlock;

				_controlDebugYaw = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlButton, IMyTerminalBlock>("MES-ProcShipControls-DebugYawButton");
				_controlDebugYaw.Enabled = (b) => { return _debug; };
				_controlDebugYaw.Visible = (b) => { return _debug; };
				_controlDebugYaw.Title = MyStringId.GetOrCompute("Yaw");
				_controlDebugYaw.Action = DebugYaw;

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
			

			//Insert Controls
			//controls.Insert(1, _controlDebugMinX);
			//controls.Insert(2, _controlDebugMinY);
			//controls.Insert(3, _controlDebugMinZ);
			controls.Insert(1, _controlDebugAddBlock);
			controls.Insert(2, _controlDebugYaw);

		}

		private static void DebugGetSingleBlocks(IMyTerminalBlock block, List<MyTerminalControlListBoxItem> items, List<MyTerminalControlListBoxItem> selected) {

			var first = new MyTerminalControlListBoxItem(MyStringId.GetOrCompute("(None)"), MyStringId.GetOrCompute("(None)"), new MyDefinitionId());

			if (_debugSelectedBlock == new MyDefinitionId())
				selected.Add(first);

			foreach (var def in _debugSingleBlockDefinitions) {

				var item = new MyTerminalControlListBoxItem(MyStringId.GetOrCompute(def.DisplayNameText), MyStringId.GetOrCompute(def.DescriptionText), def.Id);
				items.Add(item);

				if (_debugSelectedBlock == def.Id)
					selected.Add(item);

			}
		
		}

		private static void DebugSelectSingleBlocks(IMyTerminalBlock block, List<MyTerminalControlListBoxItem> selected) {

			if (selected.Count == 0)
				return;

			_debugSelectedBlock = (MyDefinitionId)selected[0].UserData;

		}

		private static void DebugAddBlock(IMyTerminalBlock block) {

			var projector = block as IMyProjector;

			if (projector == null || _debugSelectedBlock == new MyDefinitionId())
				return;

			var grid = new MyObjectBuilder_CubeGrid();
			var orientation = new SerializableBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Up);
			//var orientation = new SerializableBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Down);
			BuilderTools.AddBlockToGrid(grid, _debugSelectedBlock, _debugMin, orientation);
			projector.SetProjectedGrid(null);
			projector.SetProjectedGrid(grid);
		
		}

		private static void DebugYaw(IMyTerminalBlock block) {

			var projector = block as IMyProjector;

			if (projector?.CubeGrid == null)
				return;

			var b = projector.CubeGrid.GetCubeBlock(Vector3I.Zero);
			
			if (b == null)
				return;

			MyVisualScriptLogicProvider.ShowNotificationToAll(b.Orientation.Forward.ToString(), 5000);
			MyVisualScriptLogicProvider.ShowNotificationToAll(b.Orientation.Up.ToString(), 5000);

			var orientation = BuilderTools.RotateYaw(b.Orientation);

			var grid = new MyObjectBuilder_CubeGrid();
			BuilderTools.AddBlockToGrid(grid, _debugSelectedBlock, _debugMin, new SerializableBlockOrientation(orientation.Forward, orientation.Up));
			projector.SetProjectedGrid(null);
			projector.SetProjectedGrid(grid);

			MyVisualScriptLogicProvider.ShowNotificationToAll("Yawed", 3000);
			MyVisualScriptLogicProvider.ShowNotificationToAll(orientation.Forward.ToString(), 5000);
			MyVisualScriptLogicProvider.ShowNotificationToAll(orientation.Up.ToString(), 5000);

		}

		public static void CustomInfo(IMyTerminalBlock block, StringBuilder sb) {

			sb.Clear();
			/*
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
			*/
		}

	}

}
