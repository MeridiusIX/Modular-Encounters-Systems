using ModularEncountersSystems.Behavior;
using ModularEncountersSystems.BlockLogic;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.Tasks;
using ModularEncountersSystems.Watchers;
using ModularEncountersSystems.World;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Entities {

	[Flags]
	public enum GridOwnershipEnum {
	
		None = 0,
		NpcMajority = 1,
		NpcMinority = 1 << 1,
		PlayerMajority = 1 << 2,
		PlayerMinority = 1 << 3

	}

	public enum GridConfigurationEnum
	{

		All,
		Static,
		Dynamic

	}


	public class GridEntity : EntityBase, ITarget{

		public IMyCubeGrid CubeGrid;
		public bool HasPhysics;

		public IMyGridGroupData GridGroupData;
		public bool RefreshLinkedGrids;
		public List<GridEntity> LinkedGrids;
		public List<IMyCubeGrid> PhysicalLinkedGrids;

		public double LastHealthReading;
		public bool HealthUpdated;

		public List<BlockEntity> AllTerminalBlocks;
		public List<BlockEntity> Antennas;
		public List<BlockEntity> Beacons;
		public List<BlockEntity> Buttons;
		public List<BlockEntity> Containers;
		public List<BlockEntity> Controllers;
		public List<BlockEntity> Gravity;
		public List<BlockEntity> Guns;
		public List<BlockEntity> Gyros;
		public List<BlockEntity> Inhibitors;
		public List<BlockEntity> JumpDrives;
		public List<BlockEntity> Mechanical;
		public List<BlockEntity> Medical;
		public List<BlockEntity> NanoBots;
		public List<BlockEntity> Parachutes;
		public List<BlockEntity> Production;
		public List<BlockEntity> Projectors;
		public List<BlockEntity> Power;
		public List<BlockEntity> RivalAi;
		public List<BlockEntity> Seats;
		public List<BlockEntity> Shields;
		public List<BlockEntity> Stores;
		public List<BlockEntity> Thrusters;
		public List<BlockEntity> Tools;
		public List<BlockEntity> Turrets;
		public List<BlockEntity> TurretControllers;

		public List<IMySlimBlock> AllBlocks;

		public Dictionary<BlockTypeEnum, List<BlockEntity>> BlockListReference;

		public float ThreatScore;
		public DateTime LastThreatCalculationTime;

		public int PcuScore;
		public DateTime LastPcuCalculationTime;

		public bool RecheckOwnershipMajority;
		public GridOwnershipEnum Ownership;

		public NpcData Npc;
		public IBehavior Behavior;

		public Action<GridEntity> OwnershipMajorityChange;
		public Action UnloadEntities;

		public StringBuilder DebugData;

		public string DespawnSource;

		public bool ForceRemove;

		public string GridName;

		internal Dictionary<string, int> _missingComponents;
		internal List<IMySlimBlock> _projectedBlocks;

		public object NpcWatcher { get; private set; }

		public GridEntity(IMyEntity entity) : base(entity) {

			DebugData = new StringBuilder();

			Type = EntityType.Grid;
			CubeGrid = entity as IMyCubeGrid;

			GridGroupData = MyAPIGateway.GridGroups.GetGridGroup(GridLinkTypeEnum.Physical, CubeGrid);
			RefreshLinkedGrids = true;
			LinkedGrids = new List<GridEntity>();
			PhysicalLinkedGrids = new List<IMyCubeGrid>();

			AllTerminalBlocks = new List<BlockEntity>();
			Antennas = new List<BlockEntity>();
			Beacons = new List<BlockEntity>();
			Buttons = new List<BlockEntity>();
			Containers = new List<BlockEntity>();
			Controllers = new List<BlockEntity>();
			Gravity = new List<BlockEntity>();
			Guns = new List<BlockEntity>();
			Gyros = new List<BlockEntity>();
			Inhibitors = new List<BlockEntity>();
			JumpDrives = new List<BlockEntity>();
			Mechanical = new List<BlockEntity>();
			Medical = new List<BlockEntity>();
			NanoBots = new List<BlockEntity>();
			Parachutes = new List<BlockEntity>();
			Production = new List<BlockEntity>();
			Projectors = new List<BlockEntity>();
			Power = new List<BlockEntity>();
			RivalAi = new List<BlockEntity>();
			Seats = new List<BlockEntity>();
			Shields = new List<BlockEntity>();
			Stores = new List<BlockEntity>();
			Thrusters = new List<BlockEntity>();
			Tools = new List<BlockEntity>();
			Turrets = new List<BlockEntity>();
			TurretControllers = new List<BlockEntity>();

			AllBlocks = new List<IMySlimBlock>();
			_missingComponents = new Dictionary<string, int>();
			_projectedBlocks = new List<IMySlimBlock>();

			BlockListReference = new Dictionary<BlockTypeEnum, List<BlockEntity>>();
			BlockListReference.Add(BlockTypeEnum.All, AllTerminalBlocks);
			BlockListReference.Add(BlockTypeEnum.Antennas, Antennas);
			BlockListReference.Add(BlockTypeEnum.Beacons, Beacons);
			BlockListReference.Add(BlockTypeEnum.Buttons, Buttons);
			BlockListReference.Add(BlockTypeEnum.Containers, Containers);
			BlockListReference.Add(BlockTypeEnum.Controllers, Controllers);
			BlockListReference.Add(BlockTypeEnum.Gravity, Gravity);
			BlockListReference.Add(BlockTypeEnum.Guns, Guns);
			BlockListReference.Add(BlockTypeEnum.Gyros, Gyros);
			BlockListReference.Add(BlockTypeEnum.Inhibitors, Inhibitors);
			BlockListReference.Add(BlockTypeEnum.JumpDrives, JumpDrives);
			BlockListReference.Add(BlockTypeEnum.Mechanical, Mechanical);
			BlockListReference.Add(BlockTypeEnum.Medical, Medical);
			BlockListReference.Add(BlockTypeEnum.NanoBots, NanoBots);
			BlockListReference.Add(BlockTypeEnum.Parachutes, Parachutes);
			BlockListReference.Add(BlockTypeEnum.Production, Production);
			BlockListReference.Add(BlockTypeEnum.Projectors, Projectors);
			BlockListReference.Add(BlockTypeEnum.Power, Power);
			BlockListReference.Add(BlockTypeEnum.RivalAi, RivalAi);
			BlockListReference.Add(BlockTypeEnum.Seats, Seats);
			BlockListReference.Add(BlockTypeEnum.Shields, Shields);
			BlockListReference.Add(BlockTypeEnum.Stores, Stores);
			BlockListReference.Add(BlockTypeEnum.Thrusters, Thrusters);
			BlockListReference.Add(BlockTypeEnum.Tools, Tools);
			BlockListReference.Add(BlockTypeEnum.Turrets, Turrets);
			BlockListReference.Add(BlockTypeEnum.TurretControllers, TurretControllers);

			LastThreatCalculationTime = DateTime.MinValue;

			LastPcuCalculationTime = DateTime.MinValue;

			if (CubeGrid.Physics == null) {

				DebugData.Append(" - Grid Has No Physics On Entity Load. Registering Watcher.").AppendLine();
				CubeGrid.OnPhysicsChanged += PhysicsCheck;

			} else {

				DebugData.Append(" - Grid Has Physics On Entity Load.").AppendLine();
				EntityEvaluator.GetAttachedGrids(this);
				HasPhysics = true;
			
			}

			if (string.IsNullOrWhiteSpace(MyVisualScriptLogicProvider.GetEntityName(CubeGrid.EntityId)))
				MyVisualScriptLogicProvider.SetName(CubeGrid.EntityId, CubeGrid.EntityId.ToString());

			CubeGrid.GetBlocks(AllBlocks);

			foreach (var block in AllBlocks) {

				NewBlockAdded(block);

			}

			HealthUpdated = true;
			CubeGrid.OnBlockAdded += NewBlockAdded;
			CubeGrid.OnBlockRemoved += BlockRemoved;
			CubeGrid.OnGridSplit += GridSplit;
			CubeGrid.OnBlockOwnershipChanged += OwnershipChange;
			DamageHelper.DamageRelay += DamageHandler;

			if (GridGroupData != null) {

				GridGroupData.OnGridAdded += OnSubgridChange;
				GridGroupData.OnGridRemoved += OnSubgridChange;

			}

			CheckForNpcData();

			if (Npc != null) {

				Npc.Grid = this;
				Npc.ProcessPrimaryAttributes();

			}

			ForceRemove = false;
			DespawnSource = "";
			GridName = CubeGrid?.CustomName ?? "null";

			TaskProcessor.Tasks.Add(new NewGrid(this));

		}

		public void AppendDebug(string msg) {

			DebugData.Append(msg).AppendLine();

		}

		public void CheckForNpcData() {

			if (CubeGrid?.Storage == null) {

				DebugData.Append(" - Cube Grid Storage Null. Cannot Check For NPC Data").AppendLine();
				return;

			}
				
			string stringData = null;
			NpcData data = SerializationHelper.GetDataFromEntity<NpcData>(CubeGrid, StorageTools.NpcDataKey);

			if (data == null) {

				DebugData.Append(" - Couldn't Find NpcData. Attempting To Find Legacy NPC Data...").AppendLine();

				var legacyData = SerializationHelper.GetDataFromEntity<LegacyActiveNPC>(CubeGrid, StorageTools.LegacyNpcDataKey);

				if (legacyData == null) {

					DebugData.Append(" - Couldn't Find Any NpcData.").AppendLine();
					return;

				}

				DebugData.Append(" - Legacy NPC Data Found.").AppendLine();
				data = new NpcData(legacyData);

			} else {

				//DebugData.Append(" - Retrieved Following NPC Data: ").AppendLine();
				//DebugData.Append(data.ToString()).AppendLine();

			}

			DebugData.Append(" - Checking if NPC Data Conditions Resolve as Null.").AppendLine();
			Npc = data.Conditions != null ? data : null;
			DebugData.Append(" - NPC Data Conditions: ").Append(Npc == null ? "Null" : "OK").AppendLine();

			if (!EntityWatcher.GridsOnLoad.Contains(CubeGrid)) {

				bool gotData = false;

				lock (NpcManager.SpawnedNpcData) {

					for (int i = NpcManager.SpawnedNpcData.Count - 1; i >= 0; i--) {

						var spawnedData = NpcManager.SpawnedNpcData[i];

						if (spawnedData.UniqueSpawnIdentifier == data.UniqueSpawnIdentifier) {

							gotData = true;
							Npc = spawnedData;
							break;
						
						}

					}

				}

				if (!gotData)
					Npc = null;
			
			}

		}

		public void DamageHandler(object hit, MyDamageInformation info) {

			if ((hit as IMySlimBlock)?.CubeGrid == CubeGrid)
				HealthUpdated = true;
		
		}

		//Disconnects Subgrids Bi-Directionally
		public void DisconnectSubgrids() {

			//SpawnLogger.Write(" - " + GridName + " Disconnecting Subgrids Before Cleanup", SpawnerDebugEnum.CleanUp, true);

			var gridList = new List<IMyCubeGrid>();
			MyAPIGateway.GridGroups.GetGroup(CubeGrid, GridLinkTypeEnum.Physical, gridList);

			//SpawnLogger.Write(CubeGrid.CustomName + " Linked Grid Count: " + gridList.Count, SpawnerDebugEnum.CleanUp, true);

			IMyShipConnector connector = null;
			IMyLandingGear gear = null;
			IMyMotorStator rotor = null;
			IMyPistonBase piston = null;

			lock (AllTerminalBlocks) {
				
				for(int i = AllTerminalBlocks.Count - 1; i >= 0; i--) {

					if (i >= AllTerminalBlocks.Count)
						continue;

					var block = AllTerminalBlocks[i];

					if (!block.ActiveEntity())
						continue;

					connector = block?.Block as IMyShipConnector;
					gear = block?.Block as IMyLandingGear;
					rotor = block?.Block as IMyMotorStator;
					piston = block?.Block as IMyPistonBase;

					if (connector != null)
						connector.Disconnect();

					if (gear != null && gear.IsLocked) {

						//SpawnLogger.Write("Found Gear.", SpawnerDebugEnum.CleanUp, true);

						if (gear.IsLocked) {

							//SpawnLogger.Write("Unlocking Landing Gear.", SpawnerDebugEnum.CleanUp, true);
							gear.AutoLock = false;
							gear.Unlock();

						}
						
					}

					if (rotor != null && rotor.TopGrid != null && rotor as IMyMotorSuspension == null) {

						rotor.Detach();
					
					}

					if (piston != null && piston.TopGrid != null) {

						piston.Detach();

					}

				}
			
			}

			foreach (var physGrid in gridList) {

				var linkedGrid = GridManager.GetGridEntity(physGrid);

				if (linkedGrid == null || !linkedGrid.ActiveEntity() || linkedGrid == this)
					continue;

				//SpawnLogger.Write(" - Disconnecting Parent From Other Subgrids Before Cleanup.", SpawnerDebugEnum.CleanUp, true);

				lock (linkedGrid.AllTerminalBlocks) {

					for (int i = AllTerminalBlocks.Count - 1; i >= 0; i--) {

						if (i >= AllTerminalBlocks.Count)
							continue;

						var block = AllTerminalBlocks[i];

						gear = block?.Block as IMyLandingGear;
						rotor = block?.Block as IMyMotorStator;
						piston = block?.Block as IMyPistonBase;

						if (gear != null && gear.IsLocked && gear.GetAttachedEntity() != null && gear.GetAttachedEntity().EntityId == CubeGrid.EntityId) {

							//SpawnLogger.Write("Unlocking Landing Gear of Other Grid.", SpawnerDebugEnum.CleanUp, true);
							gear.AutoLock = false;
							gear.Unlock();

						}

						if (rotor != null && rotor.TopGrid != null && rotor.TopGrid == CubeGrid && rotor as IMyMotorSuspension == null) {

							rotor.Detach();

						}

						if (piston != null && piston.TopGrid != null && piston.TopGrid == CubeGrid) {

							piston.Detach();

						}

					}

				}
			
			}

			RefreshSubGrids();
		
		}

		private void NewBlockAdded(IMySlimBlock block) {

			lock (AllBlocks) {

				if (!AllBlocks.Contains(block))
					AllBlocks.Add(block);

			}
			
			HealthUpdated = true;

			if (!GridManager.ProcessBlock(block))
				return;

			if (block.FatBlock == null || block.FatBlock as IMyTerminalBlock == null)
				return;

			var terminalBlock = block.FatBlock as IMyTerminalBlock;
			bool assignedBlock = false;

			//All Terminal Blocks
			if (terminalBlock != null) {

				AddBlock(terminalBlock, AllTerminalBlocks);

			}

			//Antenna
			if (terminalBlock as IMyRadioAntenna != null) {

				assignedBlock = AddBlock(terminalBlock, Antennas);

			}

			//Beacon
			if (terminalBlock as IMyBeacon != null) {

				assignedBlock = AddBlock(terminalBlock, Beacons);

			}

			//Button
			if (terminalBlock as IMyButtonPanel != null) {

				assignedBlock = AddBlock(terminalBlock, Buttons);

				if (!EventWatcher.ButtonPanels.ContainsKey(terminalBlock.EntityId))
					EventWatcher.ButtonPanels.Add(terminalBlock.EntityId, terminalBlock as IMyButtonPanel);

			}

			//Container
			if (terminalBlock as IMyCargoContainer != null) {

				assignedBlock = AddBlock(terminalBlock, Containers);

			}

			//Controller
			if (terminalBlock as IMyShipController != null) {

				if((terminalBlock as IMyShipController).CanControlShip)
					assignedBlock = AddBlock(terminalBlock, Controllers);

			}

			//Gravity
			if (terminalBlock as IMyGravityGeneratorBase != null || terminalBlock as IMyVirtualMass != null) {

				assignedBlock = AddBlock(terminalBlock, Gravity);

			}

			//Gyros
			if (terminalBlock as IMyGyro != null) {

				assignedBlock = AddBlock(terminalBlock, Gyros);

			}

			//JumpDrive
			if (terminalBlock as IMyJumpDrive != null) {

				assignedBlock = AddBlock(terminalBlock, JumpDrives);

			}

			//Mechanical
			if (terminalBlock as IMyMechanicalConnectionBlock != null) {

				assignedBlock = AddBlock(terminalBlock, Mechanical);

			}

			//Medical
			if (terminalBlock as IMyMedicalRoom != null || terminalBlock.SlimBlock.BlockDefinition.Id.TypeId == typeof(MyObjectBuilder_SurvivalKit)) {

				assignedBlock = AddBlock(terminalBlock, Medical);

			}

			//Parachutes
			if (terminalBlock as IMyParachute != null) {

				assignedBlock = AddBlock(terminalBlock, Parachutes);

			}

			//Production
			if (terminalBlock as IMyProductionBlock != null) {

				assignedBlock = AddBlock(terminalBlock, Production);

			}

			//Projectors
			if (terminalBlock as IMyProjector != null) {

				assignedBlock = AddBlock(terminalBlock, Projectors);

			}

			//Power
			if (terminalBlock as IMyPowerProducer != null) {

				assignedBlock = AddBlock(terminalBlock, Power);

			}

			//Seats
			if (terminalBlock as IMyCockpit != null) {

				assignedBlock = AddBlock(terminalBlock, Seats);

			}

			//Stores
			if (terminalBlock as IMyStoreBlock != null) {

				assignedBlock = AddBlock(terminalBlock, Stores);

			}

			//Thrusters
			if (terminalBlock as IMyThrust != null) {

				assignedBlock = AddBlock(terminalBlock, Thrusters);

			}

			//Tools
			if (terminalBlock as IMyShipToolBase != null) {

				assignedBlock = AddBlock(terminalBlock, Tools);

			}

			//TurretControllers
			if (terminalBlock as IMyTurretControlBlock != null) {

				assignedBlock = AddBlock(terminalBlock, TurretControllers);

			}

			//Weapon Sections
			if (BlockManager.AllWeaponCoreBlocks.Contains(block.BlockDefinition.Id)) {

				if (BlockManager.AllWeaponCoreGuns.Contains(block.BlockDefinition.Id)) {

					assignedBlock = AddBlock(terminalBlock, Guns);

				}

				if (BlockManager.AllWeaponCoreTurrets.Contains(block.BlockDefinition.Id)) {

					assignedBlock = AddBlock(terminalBlock, Turrets);

				}

			} else {

				if (terminalBlock as IMyLargeTurretBase != null) {

					assignedBlock = AddBlock(terminalBlock, Turrets);

				}else if (terminalBlock as IMyUserControllableGun != null) {

					assignedBlock = AddBlock(terminalBlock, Guns);

				}

			}

			//Inhibitors
			if (ArmorModuleReplacement.SmallModules.Contains(block.BlockDefinition.Id) || ArmorModuleReplacement.LargeModules.Contains(block.BlockDefinition.Id)) {

				assignedBlock = AddBlock(terminalBlock, Inhibitors);

			}

			//Nanobots
			if (BlockManager.NanobotBlockIds.Contains(block.BlockDefinition.Id)) {

				assignedBlock = AddBlock(terminalBlock, NanoBots);

			}

			//RivalAI
			if (BlockManager.RivalAiBlockIds.Contains(block.BlockDefinition.Id)) {

				assignedBlock = AddBlock(terminalBlock, RivalAi);

			}

			//Shields
			if (BlockManager.ShieldBlockIds.Contains(block.BlockDefinition.Id)) {

				assignedBlock = AddBlock(terminalBlock, Shields);

			}

			//Other
			if (!assignedBlock) {
			
				//TODO: Add To 'Other'
			
			}

		}

		public int AutoConstuctProjectedBlocks(bool skipSound = false) {

			if (!ActiveEntity())
				return 0;

			long owner = 0;
			int affectedBlocks = 0;

			if (CubeGrid?.BigOwners != null && CubeGrid.BigOwners.Count > 0)
				owner = CubeGrid.BigOwners[0];

			int particlesDisplayed = 0;
			bool playedSound = skipSound;

			lock (Projectors) {

				foreach (var block in Projectors) {

					if (!block.ActiveEntity())
						continue;

					var projector = block.Block as IMyProjector;

					if (!projector.IsProjecting || projector.ProjectedGrid == null)
						continue;

					_projectedBlocks.Clear();
					projector.ProjectedGrid.GetBlocks(_projectedBlocks);

					for (int i = _projectedBlocks.Count - 1; i >= 0; i--) {

						var proBlock = _projectedBlocks[i];

						if (projector.CanBuild(proBlock, true) != BuildCheckResult.OK) {

							_projectedBlocks.RemoveAt(i);

						}

					}

					foreach (var proBlock in _projectedBlocks) {

						if (particlesDisplayed < 6) {

							if (MathTools.RandomChance(4)) {

								particlesDisplayed++;
								Vector3D worldCenter = Vector3D.Zero;
								proBlock.ComputeWorldCenter(out worldCenter);
								MyVisualScriptLogicProvider.CreateParticleEffectAtPosition("OxyLeakLarge", worldCenter);

							}

						}

						if (!playedSound) {

							playedSound = true;
							MyVisualScriptLogicProvider.PlaySingleSoundAtPosition("MES-ShipyardConstruct", GetPosition());

						}

						projector.Build(proBlock, owner, owner, true, owner);
						affectedBlocks++;

					}

				}

			}

			return affectedBlocks;

		}

		public int AutoRepairBlocks(bool skipSound = false) {

			if (!ActiveEntity())
				return 0;

			long owner = 0;
			int affectedBlocks = 0;

			if (CubeGrid?.BigOwners != null && CubeGrid.BigOwners.Count > 0)
				owner = CubeGrid.BigOwners[0];

			int particlesDisplayed = 0;
			bool playedSound = skipSound;

			lock (AllBlocks) {

				for (int i = AllBlocks.Count - 1; i >= 0; i--) {

					var block = AllBlocks[i];

					if (block == null)
						continue;

					if (!block.IsFullIntegrity || block.CurrentDamage > 0)
						block.IncreaseMountLevel(float.MaxValue, owner);

					block.FixBones(99999, 99999);

					affectedBlocks++;

					if (particlesDisplayed < 6) {

						if (MathTools.RandomChance(4)) {

							particlesDisplayed++;
							Vector3D worldCenter = Vector3D.Zero;
							block.ComputeWorldCenter(out worldCenter);
							MyVisualScriptLogicProvider.CreateParticleEffectAtPosition("OxyLeakLarge", worldCenter);

						}

					}

					if (!playedSound) {

						playedSound = true;
						MyVisualScriptLogicProvider.PlaySingleSoundAtPosition("MES-ShipyardConstruct", GetPosition());

					}

				}

			}

			return affectedBlocks;

		}

		private void BlockRemoved(IMySlimBlock block) {

			CleanBlockLists(block);

		}

		private bool AddBlock(IMyTerminalBlock block, List<BlockEntity> collection) {

			var blockEntity = new BlockEntity(block, CubeGrid);

			//TODO: Add Some Validation To BlockEntity ctor for a fail case

			lock (collection) {

				collection.Add(blockEntity);

			}

			BlockManager.BlockAdded?.Invoke(blockEntity);
			BlockLogicManager.RegisterBlockWithLogic(blockEntity);

			return true;
		
		}

		private void CleanBlockList(List<BlockEntity> collection) {

			if (collection == null)
				return;

			lock (collection) {

				for (int i = collection.Count - 1; i >= 0; i--) {

					var block = collection[i];

					if (block == null || block.IsClosed() || block.ParentEntity != this.ParentEntity)
						collection.RemoveAt(i);

				}

			}

			
		
		}

		public override void CloseEntity(IMyEntity entity) {

			base.CloseEntity(entity);
			Unload();

		}

		public int ComputerCount(long ownership = -1, bool allowNonActiveEntity = false) {

			int result = 0;

			if (!ActiveEntity()) {

				if (!allowNonActiveEntity) {

					return result;

				}

			}

			lock (AllBlocks) {

				IMySlimBlock slimBlock = null;
				IMyTerminalBlock termBlock = null;
				MyCubeBlockDefinition def = null;
				Dictionary<string, int> dict = new Dictionary<string, int>();

				for (int i = AllBlocks.Count - 1; i >= 0; i--) {

					slimBlock = AllBlocks[i];
					def = slimBlock?.BlockDefinition as MyCubeBlockDefinition;
					termBlock = slimBlock?.FatBlock as IMyTerminalBlock;

					if (def == null || termBlock == null)
						continue;

					if (ownership != -1 && termBlock.OwnerId != ownership)
						continue;

					foreach (var comp in def.Components) {

						if (comp.Definition.Id.SubtypeName != "Computer")
							continue;

						result += comp.Count;

					}

					dict.Clear();
					slimBlock.GetMissingComponents(dict);

					foreach (var comp in dict.Keys) {

						if (comp != "Computer")
							continue;

						result -= dict[comp];

					}

				}

			}

			return result;

		}

		public long CreditValueRegular(bool countInventoryItems = false, bool allowNonActiveEntity = false) {

			long result = 0;

			if (!ActiveEntity()) {

				if (!allowNonActiveEntity || Closed) {

					return result;
				
				}
			
			}
				

			lock (AllBlocks) {

				for (int i = AllBlocks.Count - 1; i >= 0; i--) {

					result += EconomyHelper.GetBlockRegularValue(AllBlocks[i], _missingComponents, countInventoryItems);

				}

			}

			return result;

		}

		public long CreditValueProjectedBlocksBuildable(out int buildableBlocks) {

			long result = 0;
			buildableBlocks = 0;

			if (!ActiveEntity())
				return 0;

			lock (Projectors) {

				foreach (var block in Projectors) {

					if (!block.ActiveEntity())
						continue;

					var projector = block.Block as IMyProjector;

					if (!projector.IsProjecting || projector.ProjectedGrid == null)
						continue;

					_projectedBlocks.Clear();
					projector.ProjectedGrid.GetBlocks(_projectedBlocks);

					foreach (var proBlock in _projectedBlocks) {

						if (projector.CanBuild(proBlock, true) == BuildCheckResult.OK) {

							buildableBlocks++;
							result += EconomyHelper.GetBlockRegularValue(proBlock, null, false);

						}
					
					}

				}
			
			}

			return result;

		}

		public long CreditValueRepair() {

			long result = 0;

			if (!ActiveEntity())
				return 0;

			lock (AllBlocks) {

				for (int i = AllBlocks.Count - 1; i >= 0; i--) {

					result += EconomyHelper.GetBlockRepairValue(AllBlocks[i], _missingComponents);

				}

			}

			return result;

		}

		public void GetAllFatBlocks(List<IMySlimBlock> blocks, bool clearList = false) {

			if (clearList)
				blocks.Clear();

			for (int i = LinkedGrids.Count - 1; i >= 0; i--) {

				var blockList = LinkedGrids[i].AllBlocks;

				for (int j = blockList.Count - 1; j >= 0; j--) {

					if(blockList[j].FatBlock != null)
						blocks.Add(blockList[j]);

				}

			}

		}

		public void GetBlocks(List<ITarget> targetList, List<BlockTypeEnum> types) {

			if (types.Contains(BlockTypeEnum.All)) {

				foreach (var block in AllTerminalBlocks) {

					if (!block.ActiveEntity())
						continue;

					targetList.Add(block);

				}

				return;

			}

			foreach (var blockType in types) {

				if (blockType == BlockTypeEnum.None)
					continue;

				foreach (var block in BlockListReference[blockType]) {

					if (!block.ActiveEntity())
						continue;

					targetList.Add(block);

				}
			
			}
		
		}

		public void GetBlocksOfType<T>(List<BlockEntity> blocks, bool clearList = true) where T : class {

			RefreshSubGrids();

			if(clearList)
				blocks.Clear();

			for (int i = LinkedGrids.Count - 1; i >= 0; i--) {

				var blockList = LinkedGrids[i].AllTerminalBlocks;

				for (int j = blockList.Count - 1; j >= 0; j--) {

					if (blockList[j]?.Block as T != null && blockList[j].ActiveEntity()) {

						blocks.Add(blockList[j]);

					}
					
				}
			
			}
		
		}

		public SpawningType GetSpawningTypeFromLinkedGrids() {

			var result = SpawningType.None;

			for (int i = LinkedGrids.Count - 1; i >= 0; i--) {

				var grid = LinkedGrids[i];

				if (grid.Npc == null)
					continue;

				if (result == SpawningType.None && grid.Npc.SpawnType != SpawningType.None) {

					result = grid.Npc.SpawnType;
					continue;

				}

				if (result == SpawningType.OtherNPC && grid.Npc.SpawnType != SpawningType.OtherNPC && grid.Npc.SpawnType != SpawningType.None) {

					result = grid.Npc.SpawnType;
					continue;

				}

			}

			return result;

		}

		public void GridSplit(IMyCubeGrid gridA, IMyCubeGrid gridB) {

			CleanBlockLists();

		}

		public bool HasNpcOwnership() {

			return Ownership.HasFlag(GridOwnershipEnum.NpcMajority) || Ownership.HasFlag(GridOwnershipEnum.NpcMinority);

		}

		public bool HasPlayerOwnership() {

			return Ownership.HasFlag(GridOwnershipEnum.PlayerMajority) || Ownership.HasFlag(GridOwnershipEnum.PlayerMinority);

		}

		public void CleanBlockLists(IMySlimBlock block = null) {

			lock (AllBlocks) {

				if (block == null) {

					for (int i = AllBlocks.Count - 1; i >= 0; i--) {

						if (AllBlocks[i]?.CubeGrid == null || AllBlocks[i].CubeGrid.EntityId != ParentEntity.EntityId)
							AllBlocks.RemoveAt(i);

					}

				} else {

					AllBlocks.Remove(block);

				}
				
			}

			HealthUpdated = true;

			if (block != null && block.FatBlock as IMyTerminalBlock == null)
				return;

			try {

				CleanBlockList(AllTerminalBlocks);
				CleanBlockList(Antennas);
				CleanBlockList(Beacons);
				CleanBlockList(Buttons);
				CleanBlockList(Containers);
				CleanBlockList(Controllers);
				CleanBlockList(Gravity);
				CleanBlockList(Guns);
				CleanBlockList(Gyros);
				CleanBlockList(Inhibitors);
				CleanBlockList(JumpDrives);
				CleanBlockList(Mechanical);
				CleanBlockList(Medical);
				CleanBlockList(NanoBots);
				CleanBlockList(Parachutes);
				CleanBlockList(Production);
				CleanBlockList(Projectors);
				CleanBlockList(Power);
				CleanBlockList(Seats);
				CleanBlockList(Shields);
				CleanBlockList(Stores);
				CleanBlockList(Thrusters);
				CleanBlockList(Tools);
				CleanBlockList(Turrets);
				CleanBlockList(TurretControllers);

			} catch (Exception e) {

				SpawnLogger.Write("Caught Error While Cleaning Grid Block Lists", SpawnerDebugEnum.Error, true);

			}

		}

		public bool LineIntersection(LineD line, RayD ray, ref Vector3D hitPosition, ref IMyDestroyableObject hitBlock) {

			if (!ActiveEntity())
				return false;

			double minDist = 0;
			double maxDist = 0;
			bool boxCheckResult = CubeGrid.PositionComp.WorldAABB.Intersect(ref ray, out minDist, out maxDist);

			Vector3D startBox = boxCheckResult ? (minDist - 5) * line.Direction + line.From : line.From;
			Vector3D endBox = boxCheckResult ? (maxDist + 5) * line.Direction + line.From : line.To;

			var blockPos = CubeGrid.RayCastBlocks(startBox, endBox);

			if (!blockPos.HasValue) {

				return false;

			}

			IMySlimBlock slimBlock = CubeGrid.GetCubeBlock(blockPos.Value);

			if (slimBlock == null) {

				return false;

			}

			slimBlock.ComputeWorldCenter(out hitPosition);
			hitBlock = slimBlock;
			return true;

		}

		public void OnSubgridChange(IMyGridGroupData dataA, IMyCubeGrid grid, IMyGridGroupData dataB) {

			//MyVisualScriptLogicProvider.ShowNotificationToAll("Subgrid Change", 4000);
			RefreshLinkedGrids = true;
		
		}

		public void OwnershipChange(IMyCubeGrid cubeGrid) {

			RecheckOwnershipMajority = true;
			RefreshSubGrids();
			EntityEvaluator.GetGridOwnerships(LinkedGrids, true);

		}

		public bool ParachutesDeployed() {

			bool result = false;

			lock (Parachutes) {

				for (int i = Parachutes.Count - 1; i >= 0; i--) {

					var parachute = Parachutes[i];
					var parachuteBlock = parachute?.Block as IMyParachute;

					if (parachuteBlock == null || !parachute.ActiveEntity())
						continue;

					if (parachuteBlock.Status == Sandbox.ModAPI.Ingame.DoorStatus.Open) {

						result = true;
						break;

					}
		
				}
			
			}

			return result;
		
		}

		public void PhysicsCheck(IMyEntity entity) {

			HasPhysics = entity.Physics != null ? true : false;

			if(HasPhysics)
				EntityEvaluator.GetAttachedGrids(this);

		}

		public BlockEntity RandomTerminalBlock() {

			if (AllTerminalBlocks.Count == 0)
				return null;

			if (AllTerminalBlocks.Count == 1)
				return AllTerminalBlocks[0];

			return AllTerminalBlocks[MathTools.RandomBetween(0, AllTerminalBlocks.Count)];
		}

		public void RefreshSubGrids() {

			if (LinkedGrids == null) {

				SpawnLogger.Write("Warning: LinkedGrids collection null", SpawnerDebugEnum.Error, true);
				return;
			
			}

			for (int i = LinkedGrids.Count - 1; i >= 0; i--) {

				try {

					if (i >= LinkedGrids.Count)
						continue;

					if (LinkedGrids[i] != null && LinkedGrids[i].RefreshLinkedGrids) {

						EntityEvaluator.GetAttachedGrids(this);
						break;

					}

				} catch (Exception e) {

					SpawnLogger.Write("Warning: LinkedGrids List Index Issue", SpawnerDebugEnum.Error, true);
					SpawnLogger.Write(e.ToString(), SpawnerDebugEnum.Error, true);

				}

			}
			
		}

		public void SetAutomatedWeaponRanges(bool useMax = false) {

			WeaponRandomizer.SetWeaponCoreRandomRanges(this, useMax);

			if (this.Npc != null && useMax)
				this.Npc.AppliedAttributes.WeaponRandomizationAggression = true;

			foreach (var grid in this.LinkedGrids) {

				foreach (var turret in this.Turrets) {

					if (!turret.ActiveEntity())
						continue;

					var turretBlock = turret.Block as IMyLargeTurretBase;

					if (turretBlock == null)
						continue;

					var def = turretBlock?.SlimBlock?.BlockDefinition as MyLargeTurretBaseDefinition;

					if (def == null)
						continue;

					if (!useMax) {

						if (def.MaxRangeMeters > 800)
							turretBlock.Range = 800;
						else
							turretBlock.Range = def.MaxRangeMeters;

					} else {

						turretBlock.Range = def.MaxRangeMeters;

					}

				}

			}

			if (this?.Behavior?.BehaviorSettings != null) {

				this.Behavior.BehaviorSettings.HomingWeaponRangeOverride = useMax ? -1 : 800;
			
			}

		}

		public override void Unload() {

			base.Unload();
			CubeGrid.OnGridSplit -= GridSplit;
			CubeGrid.OnBlockAdded -= NewBlockAdded;
			CubeGrid.OnBlockRemoved -= BlockRemoved;
			DamageHelper.DamageRelay -= DamageHandler;

			if (GridGroupData != null) {

				GridGroupData.OnGridAdded -= OnSubgridChange;
				GridGroupData.OnGridRemoved -= OnSubgridChange;

			}
			
			UnloadEntities?.Invoke();

		}

		//---------------------------------------------------
		//-----------Start Interface Methods-----------------
		//---------------------------------------------------

		public bool ActiveEntity() {

			if (Closed || !HasPhysics)
				return false;

			return true;

		}
		public double BroadcastRange(bool onlyAntenna = false) {

			if (!ActiveEntity())
				return 0;

			return EntityEvaluator.GridBroadcastRange(LinkedGrids);

		}

		public string FactionOwner() {

			//TODO: Build Method
			var result = "";

			if (CubeGrid?.BigOwners != null) {

				if (CubeGrid.BigOwners.Count > 0) {

					var faction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(CubeGrid.BigOwners[0]);

					if (faction != null) {

						return faction.Tag;
					
					}
				
				}
			
			}

			return result;
		
		}

		public double GetCurrentHealth() {

			double result = 0;

			if (!HealthUpdated && LastHealthReading > 0)
				return LastHealthReading;

			lock (AllBlocks) {

				for (int i = AllBlocks.Count - 1; i >= 0; i--) {

					var block = AllBlocks[i];

					if (block == null)
						continue;

					result += Math.Round(block.BuildIntegrity - block.CurrentDamage, 3);

				}

			}

			LastHealthReading = result;
			HealthUpdated = false;
			return result;

		}

		public List<long> GetOwners(bool onlyGetCurrentEntity = false, bool includeMinorityOwners = false) {

			var result = new List<long>();

			foreach (var grid in LinkedGrids) {

				if (!grid.ActiveEntity())
					continue;

				if (onlyGetCurrentEntity && grid.CubeGrid != CubeGrid)
					continue;

				if (grid.CubeGrid?.BigOwners != null) {

					foreach (var owner in grid.CubeGrid.BigOwners) {

						if (!result.Contains(owner))
							result.Add(owner);

					}
				
				}

				if (!includeMinorityOwners)
					continue;

				if (grid.CubeGrid?.SmallOwners != null) {

					foreach (var owner in grid.CubeGrid.SmallOwners) {

						if (!result.Contains(owner))
							result.Add(owner);

					}

				}

			}

			return result;
		
		}

		public GridOwnershipEnum GetOwnerType() {

			return Ownership;

		}

		public bool IsPowered() {

			if (!ActiveEntity())
				return false;

			return PowerOutput().Y > 0;

		}

		public bool IsSameGrid(IMyEntity entity) {

			if (!ActiveEntity())
				return false;

			if (LinkedGrids.Count == 0)
				this.RefreshSubGrids();

			foreach (var grid in LinkedGrids) {

				if (!grid.ActiveEntity())
					continue;

				if (grid.CubeGrid.EntityId == entity.EntityId)
					return true;
			
			}

			return false;

		}

		public bool IsStatic() {

			if (!ActiveEntity())
				return false;

			return CubeGrid.IsStatic;
		
		}

		public int MovementScore() {

			if (!ActiveEntity())
				return 0;

			return EntityEvaluator.GridMovementScore(LinkedGrids);

		}

		public string Name() {

			if (!ActiveEntity())
				return "N/A";

			return !string.IsNullOrWhiteSpace(CubeGrid.CustomName) ? CubeGrid.CustomName : "N/A";

		}

		public OwnerTypeEnum OwnerTypes(bool onlyGetCurrentEntity = false, bool includeMinorityOwners = false) {

			var owners = GetOwners(onlyGetCurrentEntity, includeMinorityOwners);
			//BehaviorLogger.Write("Grid Owner Count: " + owners.Count, BehaviorDebugEnum.Dev);
			return EntityEvaluator.GetOwnersFromList(owners);
		
		}

		public bool PlayerControlled() {

			foreach (var grid in LinkedGrids) {

				if (!grid.ActiveEntity())
					continue;

				if (EntityEvaluator.IsPlayerControlled(grid))
					return true;

			}

			return false;

		}

		public Vector2 PowerOutput() {

			if (!ActiveEntity())
				return Vector2.Zero;

			return EntityEvaluator.GridPowerOutput(LinkedGrids);

		}

		public RelationTypeEnum RelationTypes(long ownerId, bool onlyGetCurrentEntity = false, bool includeMinorityOwners = false) {

			var owners = GetOwners(onlyGetCurrentEntity, includeMinorityOwners);
			return EntityEvaluator.GetRelationsFromList(ownerId, owners);

		}

		public float TargetValue() {

			if (!ActiveEntity())
				return 0;

			return EntityEvaluator.GridTargetValue(LinkedGrids);

		}

		public int WeaponCount() {

			if (!ActiveEntity())
				return 0;

			return EntityEvaluator.GridWeaponCount(LinkedGrids);

		}

		//---------------------------------------------------
		//------------End Interface Methods------------------
		//---------------------------------------------------

	}

}
