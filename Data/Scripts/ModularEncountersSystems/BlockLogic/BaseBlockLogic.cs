using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Tasks;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.BlockLogic {
	public abstract class BaseBlockLogic : TaskItem, ITaskItem {

		public string LogicType { get { return _logicType; } }
		internal string _logicType = "";
		public bool Active { get { return _entityActive && _physicsActive; } }

		internal bool _entityActive;
		internal bool _physicsActive;

		internal bool _fixCheck;
		internal DateTime _createTime;
		internal IMyCubeGrid _parentGrid;
		internal Vector3I _blockCell;

		internal bool _isWorking;
		internal bool _isNpcOwned;
		internal bool _isServer;
		internal bool _isDedicated;

		internal bool _isClientPlayer { get { return !_isServer || _isServer && !_isDedicated; } }

		internal bool _useTick1;
		internal bool _useTick10;
		internal bool _useTick60;
		internal bool _useTick100;

		internal short _blockTicks;

		public BlockEntity Block;
		public IMyEntity Entity;
		public bool FunctionalOverride;

		public IMyCubeBlock CubeBlock { get { return Entity as IMyCubeBlock; } }

		internal bool _registeredOwnershipCheck = false;
		internal bool _invalidOwnership = false;


		internal virtual void Setup(BlockEntity block) {

			_createTime = MyAPIGateway.Session.GameDateTime;

			Block = block;
			Entity = block.Entity;
			block.Entity.OnMarkForClose += Unload;
			_entityActive = true;
			_blockTicks = TaskProcessor.TickIncrement;

			Block.Block.IsWorkingChanged += WorkingChanged;
			WorkingChanged();

			_isServer = MyAPIGateway.Multiplayer.IsServer;
			_isDedicated = MyAPIGateway.Utilities.IsDedicated;
			_parentGrid = block?.Block?.SlimBlock?.CubeGrid;
			_blockCell = block.Block.SlimBlock.Min;

			if (block?.Block?.SlimBlock?.CubeGrid?.Physics == null) {

				block.Block.SlimBlock.CubeGrid.OnPhysicsChanged += PhysicsChanged;

			} else {

				_physicsActive = true;
			
			}

			if(_fixCheck)
				TaskProcessor.Tasks.Add(new FixBlock(this));

			TaskProcessor.Tasks.Add(this);

		}

		internal virtual void RegisterOwnershipWatcher() {

			_registeredOwnershipCheck = true;
			Block.Block.OwnershipChanged += NpcOwnerChanged;
			NpcOwnerChanged(null);


		}

		internal virtual void PhysicsChanged(IMyEntity entity = null) {

			if (Block?.Block?.SlimBlock?.CubeGrid?.Physics != null) {

				_physicsActive = true;
				Block.Block.SlimBlock.CubeGrid.OnPhysicsChanged -= PhysicsChanged;

			}
			
		}

		public virtual void WorkingChanged(IMyCubeBlock block = null) {

			if (FunctionalOverride) {

				_isWorking = true;
				return;

			}

			if (Block?.Block != null)
				_isWorking = Block.Block.IsFunctional && Block.Block.IsWorking;
			else
				return;

			if (_invalidOwnership && Block.FunctionalBlock != null) {

				_isWorking = false;
				Block.FunctionalBlock.Enabled = false;

			}

		}

		internal void NpcOwnerChanged(IMyTerminalBlock block) {

			if (!Block.ActiveEntity()) {

				_invalidOwnership = true;
				return;
			
			}

			_invalidOwnership = FactionHelper.IsIdentityPlayer(Block.Block.OwnerId);
			WorkingChanged();


		}

		public override void Run() {

			_blockTicks++;

			if (_useTick1)
				RunTick1();

			if (_useTick10 && _blockTicks % 10 == 0)
				RunTick10();

			if (_useTick60 && _blockTicks % 60 == 0)
				RunTick60();

			if (_useTick100 && _blockTicks % 100 == 0)
				RunTick100();

			if (_blockTicks >= 300)
				_blockTicks = 0;

		}

		internal virtual void RunTick1() {
		
			
		
		}

		internal virtual void RunTick10() {



		}

		internal virtual void RunTick60() {



		}

		internal virtual void RunTick100() {



		}

		internal virtual void Unload(IMyEntity entity = null) {

			_entityActive = false;
			_physicsActive = false;
			_isValid = false;

			if(TaskProcessor.Tick1?.Tasks != null)
				TaskProcessor.Tick1.Tasks -= RunTick1;

			if (TaskProcessor.Tick10?.Tasks != null)
				TaskProcessor.Tick10.Tasks -= RunTick10;

			if (TaskProcessor.Tick60?.Tasks != null)
				TaskProcessor.Tick60.Tasks -= RunTick60;

			if (TaskProcessor.Tick100?.Tasks != null)
				TaskProcessor.Tick100.Tasks -= RunTick100;

			if (Block?.Block != null) {

				Block.Block.IsWorkingChanged -= WorkingChanged;

				if (_registeredOwnershipCheck)
					Block.Block.OwnershipChanged -= NpcOwnerChanged;

			}
				


		}

	}

}
