using ModularEncountersSystems.Entities;
using ModularEncountersSystems.World;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.BlockLogic {
	public class NpcTurretController : BaseBlockLogic, IBlockLogic {

		internal bool _firstRun = false;
		internal byte _counter = 0;
		internal IMyTurretControlBlock _controlBlock;
		internal List<Sandbox.ModAPI.Ingame.IMyFunctionalBlock> _toolList;

		public NpcTurretController(BlockEntity block) {

			Setup(block);

		}

		internal override void Setup(BlockEntity block) {

			base.Setup(block);
			RegisterOwnershipWatcher();
			_controlBlock = Block.Block as IMyTurretControlBlock;
			_toolList = new List<Sandbox.ModAPI.Ingame.IMyFunctionalBlock>();
			_useTick100 = true;

		}

		internal override void RunTick100() {

			if (!_firstRun) {

				_firstRun = true;
				SetAttachedWeapons();

			}

			_counter++;

			/*
			if(_controlBlock.IsAimed)
				MyVisualScriptLogicProvider.ShowNotificationToAll("Aimed", 1600, "Green");
			*/

			if (_counter < 5)
				return;

			_counter = 0;

		}

		internal void SetAttachedWeapons() {

			
		
		}

	}

}
