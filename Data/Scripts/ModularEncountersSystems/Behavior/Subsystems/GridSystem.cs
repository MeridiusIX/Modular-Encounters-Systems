using ModularEncountersSystems.BlockLogic;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Files;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;

namespace ModularEncountersSystems.Behavior.Subsystems {

	public class GridSystem {

		public IMyRemoteControl RemoteControl;
		public Random Rnd;

		public bool ListsBuilt;

		public List<IMyCubeGrid> ConnectedGrids;
		public DateTime LastConnectedGridCheck;
		public bool OverrideConnectedGridCheck;

		public List<IMySlimBlock> AllBlocks;
		public List<IMyTerminalBlock> AllTerminalBlocks;
		public List<IMyFunctionalBlock> AllFunctionalBlocks;

		public List<IMyRadioAntenna> Antennas;
		public List<IMyCameraBlock> Cameras;
		public List<IMyProjector> Projectors;
		public List<IMyCockpit> Seats;
		public List<IMySensorBlock> Sensors;
		public List<IMyTimerBlock> Timers;
		public List<IMyWarhead> Warheads;

		public GridSystem(IMyRemoteControl remoteControl = null) {

			this.RemoteControl = remoteControl;
			Rnd = new Random();

			ListsBuilt = false;

			ConnectedGrids = new List<IMyCubeGrid>();
			LastConnectedGridCheck = MyAPIGateway.Session.GameDateTime;
			OverrideConnectedGridCheck = false;

			AllBlocks = new List<IMySlimBlock>();
			AllTerminalBlocks = new List<IMyTerminalBlock>();
			AllFunctionalBlocks = new List<IMyFunctionalBlock>();

			Antennas = new List<IMyRadioAntenna>();
			Cameras = new List<IMyCameraBlock>();
			Projectors = new List<IMyProjector>();
			Seats = new List<IMyCockpit>();
			Sensors = new List<IMySensorBlock>();
			Timers = new List<IMyTimerBlock>();
			Warheads = new List<IMyWarhead>();
			
			BuildLists();

		}

		public void BuildLists() {

			if (ListsBuilt)
				return;

			ListsBuilt = true;

			if (this.RemoteControl?.SlimBlock?.CubeGrid == null)
				return;

			this.RemoteControl.SlimBlock.CubeGrid.OnGridSplit += GridSplit;
			
			OverrideConnectedGridCheck = true;
			CheckConnectedGrids();

			var tempAllBlocks = BlockCollectionHelper.GetAllBlocks(GridManager.GetGridEntity(this.RemoteControl.SlimBlock.CubeGrid));
			BehaviorLogger.Write("Grid System Total Blocks: " + tempAllBlocks.Count, BehaviorDebugEnum.BehaviorSetup);

			foreach (var block in tempAllBlocks) {

				AddBlock(block, true);

			}

			this.RemoteControl.SlimBlock.CubeGrid.OnBlockAdded += AddBlock;

		}

		public void AddBlock(IMySlimBlock block) {

			AddBlock(block, false);

		}
 
		public void AddBlock(IMySlimBlock block, bool skipTimer = false) {

			if (block == null)
				return;

			if (!skipTimer && !OverrideConnectedGridCheck)
				CheckConnectedGrids();

			if (!ConnectedGrids.Contains(block.CubeGrid))
				return;

			AllBlocks.Add(block);

			if (block.FatBlock == null)
				return;

			if ((block.FatBlock as IMyTerminalBlock) != null)
				AllTerminalBlocks.Add(block.FatBlock as IMyTerminalBlock);

			if ((block.FatBlock as IMyFunctionalBlock) != null)
				AllFunctionalBlocks.Add(block.FatBlock as IMyFunctionalBlock);

			if ((block.FatBlock as IMyRadioAntenna) != null)
				Antennas.Add(block.FatBlock as IMyRadioAntenna);

			if ((block.FatBlock as IMyCameraBlock) != null)
				Cameras.Add(block.FatBlock as IMyCameraBlock);

			if ((block.FatBlock as IMyProjector) != null)
				Projectors.Add(block.FatBlock as IMyProjector);

			if ((block.FatBlock as IMyCockpit) != null)
				Seats.Add(block.FatBlock as IMyCockpit);

			if ((block.FatBlock as IMySensorBlock) != null)
				Sensors.Add(block.FatBlock as IMySensorBlock);

			if ((block.FatBlock as IMyTimerBlock) != null)
				Timers.Add(block.FatBlock as IMyTimerBlock);

			if ((block.FatBlock as IMyWarhead) != null)
				Warheads.Add(block.FatBlock as IMyWarhead);

		}

		public void AddCustomData(List<string> blockNames, List<TextTemplate> files) {

			for (int i = AllTerminalBlocks.Count - 1; i >= 0; i--) {

				var block = AllTerminalBlocks[i];

				if (block?.CustomName == null)
					continue;

				for (int j = 0; j < blockNames.Count && j < files.Count; j++) {

					if (block.CustomName.Contains(blockNames[j]) && files[j]?.CustomData != null) {

						block.CustomData = files[j].CustomData;
						break;

					}

				}

			}

		}

		public void ApplyContainerTypes(List<string> blockNames, List<string> ids) {

			for (int i = AllTerminalBlocks.Count - 1; i >= 0; i--) {

				var block = AllTerminalBlocks[i] as MyEntity;

				if (block?.GetInventory() == null)
					continue;

				for (int j = 0; j < blockNames.Count && j < ids.Count; j++) {

					if (AllTerminalBlocks[i].CustomName == blockNames[j] && ids[j] != null) {

						var containerType = MyDefinitionManager.Static.GetContainerTypeDefinition(ids[j]);

						if (containerType != null) {

							block.GetInventory().GenerateContent(containerType);
							break;

						}
						
					}

				}

			}

		}

		public void ApplyLcdContents(string textTemplate, List<string> blockNames, List<int> indexes) {

			TextTemplate template = ProfileManager.GetTextTemplate(textTemplate);

			if (template == null)
				return;
			for (int i = AllTerminalBlocks.Count - 1; i >= 0; i--) {

				var block = AllTerminalBlocks[i];

				for (int j = 0; j < blockNames.Count && j < indexes.Count; j++) {

					var indexAllowed = template.LcdEntries.Length <= indexes[j] + 1;
					if (block.CustomName == blockNames[j] && indexAllowed) {

						template.LcdEntries[indexes[j]].ApplyLcdContents(block as IMyTextSurfaceProvider);

					}

				}

			}

		}

		public void BuildProjectedBlocks(int maxBlocksToBuild) {

			int builtBlocks = 0;

			if (maxBlocksToBuild <= 0)
				return;

			foreach(var projector in Projectors) {

				if (projector == null || projector.MarkedForClose) 
					continue;

				if (projector.ProjectedGrid == null)
					continue;

				var projectedBlocks = new List<IMySlimBlock>();

				while (builtBlocks < maxBlocksToBuild) {

					if (projector.ProjectedGrid == null)
						break;

					projectedBlocks.Clear();
					projector.ProjectedGrid.GetBlocks(projectedBlocks);

					if (projectedBlocks.Count == 0)
						break;

					bool restartLoop = false;

					while (projectedBlocks.Count > 0 && builtBlocks < maxBlocksToBuild) {

						int randomIndex = 0;

						if (projectedBlocks.Count > 1)
							randomIndex = MathTools.RandomBetween(0, projectedBlocks.Count);

						var projectedBlock = projectedBlocks[randomIndex];
						projectedBlocks.RemoveAt(randomIndex);

						if (projectedBlock == null)
							continue;

						if (projector.CanBuild(projectedBlock, true) != BuildCheckResult.OK)
							continue;

						projector.Build(projectedBlock, RemoteControl.OwnerId, RemoteControl.OwnerId, true);
						builtBlocks++;
						restartLoop = true;
						break;

					}

					if (restartLoop)
						continue;

					break;

				}

			}
		
		}

		public void ChangeHighestRangeAntennas(bool enabled) {

			double highestRange = 0;

			foreach (var antenna in Antennas) {

				if (antenna == null || antenna.MarkedForClose || antenna.Closed)
					continue;

				if (antenna.Radius > highestRange)
					highestRange = antenna.Radius;

			}

			foreach (var antenna in Antennas) {

				if (antenna == null || antenna.MarkedForClose || antenna.Closed)
					continue;

				if (antenna.Radius == highestRange)
					antenna.Enabled = enabled;

			}

		}

		public bool CheckBlockValid(IMyTerminalBlock block) {

			CheckConnectedGrids();

			if (block == null || block.MarkedForClose) {

				return false;

			}

			if (!ConnectedGrids.Contains(block.SlimBlock.CubeGrid)) {

				return false;

			}

			return true;

		}

		public bool CheckBlockValid(IMySlimBlock block) {

			CheckConnectedGrids();

			if (block == null) {

				return false;

			}

			if (!ConnectedGrids.Contains(block.CubeGrid)) {

				return false;

			}

			return true;

		}

		public void CheckConnectedGrids() {

			if (!OverrideConnectedGridCheck) {

				var time = MyAPIGateway.Session.GameDateTime - LastConnectedGridCheck;

				if (time.TotalMilliseconds < 1000)
					return;

			} else {

				OverrideConnectedGridCheck = false;

			}

			ConnectedGrids.Clear();
			ConnectedGrids = MyAPIGateway.GridGroups.GetGroup(this.RemoteControl.SlimBlock.CubeGrid, GridLinkTypeEnum.Physical);
			LastConnectedGridCheck = MyAPIGateway.Session.GameDateTime;

		}

		public void EnableBlocks(List<string> names, List<SwitchEnum> states) {

			if (names.Count != states.Count)
				return;

			for (int i = AllFunctionalBlocks.Count - 1; i >= 0; i--) {

				var block = AllFunctionalBlocks[i];

				if (!CheckBlockValid(block)) {

					AllFunctionalBlocks.RemoveAt(i);
					continue;

				}

				if (string.IsNullOrWhiteSpace(block.CustomName))
					continue;

				IBlockLogic logic = null;
				BlockLogicManager.LogicBlocks.TryGetValue(block.EntityId, out logic);

				for (int j = 0; j < names.Count; j++) {

					if (block.CustomName == names[j]) {

						bool changeTo = block.Enabled;

						if (states[j] == SwitchEnum.Off)
							changeTo = false;

						if (states[j] == SwitchEnum.On)
							changeTo = true;

						if (states[j] == SwitchEnum.Toggle)
							changeTo = changeTo ? false : true;

						if (logic as InhibitorLogic != null) {

							var inhibitor = logic as InhibitorLogic;
							inhibitor.Toggle(changeTo);
							break;

						}

						block.Enabled = changeTo;
						break;

					}
  
				}

			}
		
		}

		public void EnableBlocksInGroup(string groupName, SwitchEnum state) {

			var terminal = MyAPIGateway.TerminalActionsHelper.GetTerminalSystemForGrid(RemoteControl.SlimBlock.CubeGrid);

			if (terminal == null)
				return;

			var group = terminal.GetBlockGroupWithName(groupName);

			if (group == null)
				return;

			var functionalBlocksInGroup = new List<IMyFunctionalBlock>();
			group.GetBlocksOfType<IMyFunctionalBlock>(functionalBlocksInGroup);

			for (int i = functionalBlocksInGroup.Count - 1; i >= 0; i--) {

				var block = functionalBlocksInGroup[i];

				bool changeTo = block.Enabled;

				if (state == SwitchEnum.Off)
					changeTo = false;

				if (state == SwitchEnum.On)
					changeTo = true;

				if (state == SwitchEnum.Toggle)
					changeTo = changeTo ? false : true;

				block.Enabled = changeTo;

			}

		}

		public bool JumpToCoords(Vector3D coords) {

			if (RemoteControl?.SlimBlock?.CubeGrid?.JumpSystem == null)
				return false;

			var jumpSystem = RemoteControl.SlimBlock.CubeGrid.JumpSystem;
			var safeCoords = RemoteControl.SlimBlock.CubeGrid.JumpSystem.FindSuitableJumpLocation(coords);
			var result = false;

			if (safeCoords.HasValue) {

				jumpSystem.Jump(safeCoords.Value, RemoteControl.OwnerId, 10);
				result = jumpSystem.IsJumping;

			}

			if (result)
				return result;

			var offset = new Vector3D(2000, 2000, 2000);
			var box = new BoundingBoxD(coords - offset, coords + offset);

			foreach (var corner in box.GetCorners()) {

				safeCoords = RemoteControl.SlimBlock.CubeGrid.JumpSystem.FindSuitableJumpLocation(corner);

				if (safeCoords.HasValue) {

					jumpSystem.Jump(safeCoords.Value, RemoteControl.OwnerId, 10);
					result = jumpSystem.IsJumping;

					if (result)
						break;

				}

			}

			return result;

		}

		public void ToggleBlocksOfType(List<SerializableDefinitionId> types, List<SwitchEnum> toggles) {

			int maxIndex = types.Count <= toggles.Count ? types.Count : toggles.Count;

			if (maxIndex == 0)
				return;

			for (int i = AllFunctionalBlocks.Count - 1; i >= 0; i--) {

				var block = AllFunctionalBlocks[i];

				if (!CheckBlockValid(block)) {

					AllFunctionalBlocks.RemoveAt(i);
					continue;

				}

				bool actionPerformed = false;

				for (int j = 0; j < maxIndex; j++) {

					var type = (MyDefinitionId)types[j];
					var toggle = toggles[j];

					if (block.SlimBlock.BlockDefinition.Id == type) {

						if (toggle == SwitchEnum.Toggle)
							block.Enabled = !block.Enabled;

						if (toggle == SwitchEnum.Off)
							block.Enabled = false;

						if (toggle == SwitchEnum.On)
							block.Enabled = true;

						actionPerformed = true;

					}

					if (actionPerformed)
						break;

				}

				if (actionPerformed)
					continue;

			}
		
		}

		public IMyRadioAntenna GetActiveAntenna() {

			IMyRadioAntenna resultAntenna = null;
			float range = 0;

			for (int i = Antennas.Count - 1; i >= 0; i--) {

				var antenna = Antennas[i];

				if (!CheckBlockValid(antenna)) {

					Antennas.RemoveAt(i);
					continue;

				}

				if (antenna.IsWorking == false || antenna.IsFunctional == false) {

					continue;

				}

				return antenna;

			}

			return resultAntenna;

		}

		public IMyRadioAntenna GetAntennaWithHighestRange(string antennaName = "") {

			IMyRadioAntenna resultAntenna = null;
			float range = 0;

			for (int i = Antennas.Count - 1; i >= 0; i--) {

				var antenna = Antennas[i];

				if (!CheckBlockValid(antenna)) {

					Antennas.RemoveAt(i);
					continue;

				}

				if (antenna.IsWorking == false || antenna.IsFunctional == false || antenna.IsBroadcasting == false) {

					continue;

				}

				if (!string.IsNullOrWhiteSpace(antennaName) && antennaName != antenna.CustomName)
					continue;

				if (antenna.Radius > range) {

					resultAntenna = antenna;
					range = antenna.Radius;

				}

			}

			return resultAntenna;

		}

		public void GridSplit(IMyCubeGrid gridA, IMyCubeGrid gridB) {

			gridA.OnGridSplit -= GridSplit;
			gridB.OnGridSplit -= GridSplit;

			if (RemoteControl == null || RemoteControl.MarkedForClose || RemoteControl?.SlimBlock?.CubeGrid == null)
				return;

			if(gridA == RemoteControl.SlimBlock.CubeGrid)
				gridA.OnGridSplit -= GridSplit;

			if (gridB == RemoteControl.SlimBlock.CubeGrid)
				gridB.OnGridSplit -= GridSplit;

			OverrideConnectedGridCheck = true;
			CheckConnectedGrids();

		}

		public bool InsertDatapadIntoInventory(IMyTerminalBlock block, string datapadId) {

			if (block == null || !block.HasInventory)
				return false;

			var inventory = block.GetInventory() as MyInventory;

			if (inventory == null)
				return false;

			MyDefinitionBase def = null;

			if (!ProfileManager.DatapadTemplates.TryGetValue(datapadId, out def))
				return false;

			var id = new MyDefinitionId(typeof(MyObjectBuilder_Datapad), "Datapad");
			var datapadOb = MyObjectBuilderSerializer.CreateNewObject(id) as MyObjectBuilder_Datapad;

			if (datapadOb == null) {

				return false;
			
			}

			datapadOb.Name = def.DisplayNameString;
			datapadOb.Data = def.DescriptionString;

			if (!inventory.CanItemsBeAdded(1, id) == true) {

				return false;

			}

			inventory.AddItems(1, datapadOb);
			return true;
		
		}

		public void InsertDatapadsIntoSeats(List<string> datapadIds, int count) {

			if (Seats.Count == 0)
				return;

			var dataPadList = new List<string>(datapadIds.ToList());

			for (int i = 0; i < count; i++) {

				var listCount = dataPadList.Count;

				if (listCount == 0)
					break;

				string id = "";

				if (listCount == 1) {

					id = dataPadList[0];
					dataPadList.RemoveAt(0);

				} else {

					int index = MathTools.RandomBetween(0, listCount);
					id = dataPadList[index];
					dataPadList.RemoveAt(index);

				}

				int seatIndex = 0;

				if (Seats.Count > 1)
					seatIndex = MathTools.RandomBetween(0, Seats.Count);

				if (!InsertDatapadIntoInventory(Seats[seatIndex], id))
					i--;

			}
		
		}

		public bool RaycastGridCheck(Vector3D coords) {

			bool gotHit = false;

			for (int i = Cameras.Count - 1; i >= 0; i--) {

				var camera = Cameras[i];

				if (!CheckBlockValid(camera)) {

					Cameras.RemoveAt(i);
					continue;

				}

				if (!camera.EnableRaycast)
					camera.EnableRaycast = true;

				if (!camera.CanScan(coords))
					continue;

				var result = camera.Raycast(coords);

				if (result.IsEmpty())
					continue;

				if (result.Type.ToString().EndsWith("Grid") || result.Type == Sandbox.ModAPI.Ingame.MyDetectedEntityType.CharacterHuman) {

					gotHit = true;
					break;
				
				}

			}

			return gotHit;

		}

		public void RazeBlocksWithNames(List<string> names) {

			BehaviorLogger.Write("Razing Blocks With Names Count: " + names.Count.ToString(), BehaviorDebugEnum.Action);
			BehaviorLogger.Write("Razing Blocks Total Terminal Blocks: " + AllTerminalBlocks.Count.ToString(), BehaviorDebugEnum.Action);

			for (int i = AllTerminalBlocks.Count - 1; i >= 0; i--) {

				var block = AllTerminalBlocks[i];

				if (!CheckBlockValid(block)) {

					AllTerminalBlocks.RemoveAt(i);
					continue;

				}

				if (names.Contains(block.CustomName))
					block.SlimBlock.CubeGrid.RazeBlock(block.SlimBlock.Min);

			}

		}

		public void RazeBlocksWithTypes(List<MyDefinitionId> types) {

			for (int i = AllBlocks.Count - 1; i >= 0; i--) {

				var block = AllBlocks[i];

				if (!CheckBlockValid(block)) {

					AllBlocks.RemoveAt(i);
					continue;

				}

				if (types.Contains(block.BlockDefinition.Id))
					block.CubeGrid.RazeBlock(block.Min);

			}

		}

		public void RecolorBlocks(IMyCubeGrid grid, List<Vector3D> oldColors, List<Vector3D> newColors, List<string> newSkins) {

			for (int j = AllBlocks.Count - 1; j >= 0; j--) {

				var block = AllBlocks[j];

				if (!CheckBlockValid(block)) {

					AllTerminalBlocks.RemoveAt(j);
					continue;

				}

				if (block.CubeGrid != grid)
					continue;

				for (int i = 0; i < oldColors.Count; i++) {

					if (i >= newColors.Count && i >= newSkins.Count)
						break;

					if (Math.Round(oldColors[i].X, 3) != Math.Round(block.ColorMaskHSV.X, 3))
						continue;

					if (Math.Round(oldColors[i].Y, 3) != Math.Round(block.ColorMaskHSV.Y, 3))
						continue;

					if (Math.Round(oldColors[i].Z, 3) != Math.Round(block.ColorMaskHSV.Z, 3))
						continue;

					if (i < newColors.Count) {

						if (newColors[i] != new Vector3D(-10, -10, -10))
							grid.ColorBlocks(block.Min, block.Min, (Vector3)newColors[i]);

					}

					if (i < newSkins.Count) {

						if (!string.IsNullOrWhiteSpace(newSkins[i]))
							grid.SkinBlocks(block.Min, block.Min, null, newSkins[i]);

					}

				}

			}

		}

		public void RenameBlocks(List<string> oldNames, List<string> newNames, string actionId) {

			if (oldNames.Count != newNames.Count) {

				BehaviorLogger.Write(actionId + ": ChangeBlockNames From and To lists not the same count. Aborting operation", BehaviorDebugEnum.Action);
				return;

			}

			var dictionary = new Dictionary<string, string>();

			for (int i = 0; i < oldNames.Count; i++) {

				if (!dictionary.ContainsKey(oldNames[i]))
					dictionary.Add(oldNames[i], newNames[i]);

			}

			for (int i = AllTerminalBlocks.Count - 1; i >= 0; i--) {

				var block = AllTerminalBlocks[i];

				if (!CheckBlockValid(block)) {

					AllTerminalBlocks.RemoveAt(i);
					continue;

				}

				if (oldNames.Contains(block.CustomName))
					block.CustomName = dictionary[block.CustomName];

			}

		}

		public bool SensorCheck(string sensorName, bool triggeredState = true) {

			bool result = false;
			var entities = new List<Sandbox.ModAPI.Ingame.MyDetectedEntityInfo>();

			for (int i = Sensors.Count - 1; i >= 0; i--) {

				var sensor = Sensors[i];

				if (!CheckBlockValid(sensor)) {

					Sensors.RemoveAt(i);
					continue;

				}

				if (sensor.CustomName != sensorName)
					continue;

				entities.Clear();
				sensor.DetectedEntities(entities);

				if ((entities.Count > 0) == triggeredState) {

					result = true;
					break;
				
				}

			}

			return result;
		
		}

		public void SetGridAntennaRanges(List<string> names, string operation, float amount) {

			bool checkNames = names.Count > 0 ? true : false;

			for (int i = Antennas.Count - 1; i >= 0; i--) {

				var antenna = Antennas[i];

				if (!CheckBlockValid(antenna)) {

					Antennas.RemoveAt(i);
					continue;

				}

				if (operation == "Set") {

					antenna.Radius = amount;
					continue;

				}

				if (operation == "Increase") {

					antenna.Radius += amount;
					continue;

				}

				if (operation == "Decrease") {

					antenna.Radius -= amount;
					continue;

				}

			}

		}

		public void SetGridDestructible(IMyCubeGrid cubeGrid, bool enabled) {

			var grid = cubeGrid as MyCubeGrid;

			if (grid == null)
				return;

			grid.DestructibleBlocks = enabled;

		}

		public void SetGridEditable(IMyCubeGrid cubeGrid, bool enabled) {

			var grid = cubeGrid as MyCubeGrid;

			if (grid == null)
				return;

			grid.Editable = enabled;

		}

		public void Unload() {
		
			
		
		}

	}

}
