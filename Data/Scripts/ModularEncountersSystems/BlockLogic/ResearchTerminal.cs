using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Entities;
using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using System;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.ObjectBuilders;


namespace ModularEncountersSystems.BlockLogic {

	public class ResearchTerminal : BaseBlockLogic, IBlockLogic{

		public IMyButtonPanel ButtonPanel;

		public ResearchTerminal(BlockEntity block) {

			Setup(block);

		}

		internal override void Setup(BlockEntity block) {

			ButtonPanel = block.Block as IMyButtonPanel;

			if (ButtonPanel == null) {

				_isValid = false;
				return;

			}

			_isValid = false;

		}

	}
	
}