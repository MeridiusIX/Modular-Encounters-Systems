using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Tasks;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRage.ModAPI;

namespace ModularEncountersSystems.BlockLogic {
	public abstract class BaseBlockLogic : TaskItem, ITaskItem {

		public string LogicType { get { return _logicType; } }
		internal string _logicType = "";
		public bool Active { get { return _entityActive && _physicsActive; } }

		internal bool _entityActive;
		internal bool _physicsActive;

		internal bool _isWorking;
		internal bool _isNpcOwned;
		internal bool _isServer;
		internal bool _isDedicated;

		internal bool _useTick1;
		internal bool _useTick10;
		internal bool _useTick60;
		internal bool _useTick100;

		internal short _blockTicks;

		public BlockEntity Block;
		public IMyEntity Entity;

		internal virtual void Setup(BlockEntity block) {

			Block = block;
			Entity = block.Entity;
			block.Entity.OnMarkForClose += Unload;
			_entityActive = true;
			_blockTicks = TaskProcessor.TickIncrement;

			Block.Block.IsWorkingChanged += WorkingChanged;
			WorkingChanged();

			_isServer = MyAPIGateway.Multiplayer.IsServer;
			_isDedicated = MyAPIGateway.Utilities.IsDedicated;

			if (block?.Block?.SlimBlock?.CubeGrid?.Physics == null) {

				block.Block.SlimBlock.CubeGrid.OnPhysicsChanged += PhysicsChanged;

			} else {

				_physicsActive = true;
			
			}

			TaskProcessor.Tasks.Add(this);

		}

		internal virtual void PhysicsChanged(IMyEntity entity = null) {

			if (Block?.Block?.SlimBlock?.CubeGrid?.Physics != null) {

				_physicsActive = true;
				Block.Block.SlimBlock.CubeGrid.OnPhysicsChanged -= PhysicsChanged;

			}
			
		}

		internal virtual void WorkingChanged(IMyCubeBlock block = null) {

			if (Block.Block != null)
				_isWorking = Block.Block.IsFunctional && Block.Block.IsWorking;

		}

		internal void OwnerChanged() {
		
			
		
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

			TaskProcessor.Tick1.Tasks -= RunTick1;
			TaskProcessor.Tick10.Tasks -= RunTick10;
			TaskProcessor.Tick60.Tasks -= RunTick60;
			TaskProcessor.Tick100.Tasks -= RunTick100;

			if (Block.Block != null)
				Block.Block.IsWorkingChanged -= WorkingChanged;

		}

	}

}
