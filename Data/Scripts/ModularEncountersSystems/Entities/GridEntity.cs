using ModularEncountersSystems.Behavior;
using ModularEncountersSystems.BlockLogic;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.Tasks;
using ModularEncountersSystems.World;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
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

	public class GridEntity : EntityBase, ITarget{

		public IMyCubeGrid CubeGrid;
		public bool HasPhysics;

		public List<GridEntity> LinkedGrids;

		public List<BlockEntity> AllTerminalBlocks;
		public List<BlockEntity> Antennas;
		public List<BlockEntity> Beacons;
		public List<BlockEntity> Containers;
		public List<BlockEntity> Controllers;
		public List<BlockEntity> Gravity;
		public List<BlockEntity> Guns;
		public List<BlockEntity> JumpDrives;
		public List<BlockEntity> Mechanical;
		public List<BlockEntity> Medical;
		public List<BlockEntity> NanoBots;
		public List<BlockEntity> Production;
		public List<BlockEntity> Power;
		public List<BlockEntity> RivalAi;
		public List<BlockEntity> Shields;
		public List<BlockEntity> Thrusters;
		public List<BlockEntity> Tools;
		public List<BlockEntity> Turrets;

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

		public GridEntity(IMyEntity entity) : base(entity) {

			DebugData = new StringBuilder();

			Type = EntityType.Grid;
			CubeGrid = entity as IMyCubeGrid;

			LinkedGrids = new List<GridEntity>();

			AllTerminalBlocks = new List<BlockEntity>();
			Antennas = new List<BlockEntity>();
			Beacons = new List<BlockEntity>();
			Containers = new List<BlockEntity>();
			Controllers = new List<BlockEntity>();
			Gravity = new List<BlockEntity>();
			Guns = new List<BlockEntity>();
			JumpDrives = new List<BlockEntity>();
			Mechanical = new List<BlockEntity>();
			Medical = new List<BlockEntity>();
			NanoBots = new List<BlockEntity>();
			Production = new List<BlockEntity>();
			Power = new List<BlockEntity>();
			RivalAi = new List<BlockEntity>();
			Shields = new List<BlockEntity>();
			Thrusters = new List<BlockEntity>();
			Tools = new List<BlockEntity>();
			Turrets = new List<BlockEntity>();

			AllBlocks = new List<IMySlimBlock>();

			BlockListReference = new Dictionary<BlockTypeEnum, List<BlockEntity>>();
			BlockListReference.Add(BlockTypeEnum.All, AllTerminalBlocks);
			BlockListReference.Add(BlockTypeEnum.Antennas, Antennas);
			BlockListReference.Add(BlockTypeEnum.Beacons, Beacons);
			BlockListReference.Add(BlockTypeEnum.Containers, Containers);
			BlockListReference.Add(BlockTypeEnum.Controllers, Controllers);
			BlockListReference.Add(BlockTypeEnum.Gravity, Gravity);
			BlockListReference.Add(BlockTypeEnum.Guns, Guns);
			BlockListReference.Add(BlockTypeEnum.JumpDrives, JumpDrives);
			BlockListReference.Add(BlockTypeEnum.Mechanical, Mechanical);
			BlockListReference.Add(BlockTypeEnum.Medical, Medical);
			BlockListReference.Add(BlockTypeEnum.NanoBots, NanoBots);
			BlockListReference.Add(BlockTypeEnum.Production, Production);
			BlockListReference.Add(BlockTypeEnum.Power, Power);
			BlockListReference.Add(BlockTypeEnum.RivalAi, RivalAi);
			BlockListReference.Add(BlockTypeEnum.Shields, Shields);
			BlockListReference.Add(BlockTypeEnum.Thrusters, Thrusters);
			BlockListReference.Add(BlockTypeEnum.Tools, Tools);
			BlockListReference.Add(BlockTypeEnum.Turrets, Turrets);

			LastThreatCalculationTime = DateTime.MinValue;

			LastPcuCalculationTime = DateTime.MinValue;

			if (CubeGrid.Physics == null) {

				DebugData.Append(" - Grid Has No Physics On Entity Load. Registering Watcher.").AppendLine();
				CubeGrid.OnPhysicsChanged += PhysicsCheck;

			} else {

				DebugData.Append(" - Grid Has Physics On Entity Load.").AppendLine();
				HasPhysics = true;
			
			}

			if (string.IsNullOrWhiteSpace(MyVisualScriptLogicProvider.GetEntityName(CubeGrid.EntityId)))
				MyVisualScriptLogicProvider.SetName(CubeGrid.EntityId, CubeGrid.EntityId.ToString());

			CubeGrid.GetBlocks(AllBlocks);

			foreach (var block in AllBlocks) {

				NewBlockAdded(block);

			}

			CubeGrid.OnBlockAdded += NewBlockAdded;
			CubeGrid.OnBlockRemoved += BlockRemoved;
			CubeGrid.OnGridSplit += GridSplit;
			CubeGrid.OnBlockOwnershipChanged += OwnershipChange;

			CheckForNpcData();

			if (Npc != null) {

				Npc.Grid = this;
				Npc.ProcessPrimaryAttributes();

			}

			TaskProcessor.Tasks.Add(new NewGrid(this));

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

				DebugData.Append(" - Retrieved Following NPC Data: ").AppendLine();
				DebugData.Append(data.ToString()).AppendLine();

			}

			DebugData.Append(" - Checking if NPC Data Conditions Resolve as Null.").AppendLine();
			Npc = data.Conditions != null ? data : null;
			DebugData.Append(" - NPC Data Conditions: ").Append(Npc == null ? "Null" : "OK").AppendLine();

		}

		private void NewBlockAdded(IMySlimBlock block) {

			if (!AllBlocks.Contains(block))
				AllBlocks.Add(block);

			if (!GridManager.ProcessBlock(block))
				return;

			if (block.FatBlock == null || block.FatBlock as IMyTerminalBlock == null)
				return;

			var terminalBlock = block.FatBlock as IMyTerminalBlock;
			bool assignedBlock = false;

			//All Terminal Blocks
			if (terminalBlock as IMyTerminalBlock != null) {

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

			//Production
			if (terminalBlock as IMyProductionBlock != null) {

				assignedBlock = AddBlock(terminalBlock, Production);

			}

			//Power
			if (terminalBlock as IMyPowerProducer != null) {

				assignedBlock = AddBlock(terminalBlock, Power);

			}

			//Thrusters
			if (terminalBlock as IMyThrust != null) {

				assignedBlock = AddBlock(terminalBlock, Thrusters);

			}

			//Tools
			if (terminalBlock as IMyShipToolBase != null) {

				assignedBlock = AddBlock(terminalBlock, Tools);

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

		private void BlockRemoved(IMySlimBlock block) {

			AllBlocks.Remove(block);
		
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

		public void CleanBlockLists() {

			lock (AllBlocks) {

				for (int i = AllBlocks.Count - 1; i >= 0; i--) {

					if (AllBlocks[i]?.CubeGrid == null || AllBlocks[i].CubeGrid.EntityId != ParentEntity.EntityId)
						AllBlocks.RemoveAt(i);

				}
			
			}

			try {

				CleanBlockList(AllTerminalBlocks);
				CleanBlockList(Antennas);
				CleanBlockList(Beacons);
				CleanBlockList(Containers);
				CleanBlockList(Controllers);
				CleanBlockList(Gravity);
				CleanBlockList(Guns);
				CleanBlockList(JumpDrives);
				CleanBlockList(Mechanical);
				CleanBlockList(Medical);
				CleanBlockList(NanoBots);
				CleanBlockList(Production);
				CleanBlockList(Power);
				CleanBlockList(Shields);
				CleanBlockList(Thrusters);
				CleanBlockList(Tools);
				CleanBlockList(Turrets);

			} catch (Exception e) {

				SpawnLogger.Write("Caught Error While Cleaning Grid Block Lists", SpawnerDebugEnum.Error, true);

			}

		}

		public void OwnershipChange(IMyCubeGrid cubeGrid) {

			RecheckOwnershipMajority = true;
			RefreshSubGrids();
			EntityEvaluator.GetGridOwnerships(LinkedGrids, true);

		}

		public void PhysicsCheck(IMyEntity entity) {

			HasPhysics = entity.Physics != null ? true : false;

		}

		public void RefreshSubGrids() {

			LinkedGrids = EntityEvaluator.GetAttachedGrids(CubeGrid);

		}

		public override void Unload() {

			base.Unload();
			CubeGrid.OnGridSplit -= GridSplit;
			CubeGrid.OnBlockAdded -= NewBlockAdded;
			CubeGrid.OnBlockRemoved -= BlockRemoved;
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
