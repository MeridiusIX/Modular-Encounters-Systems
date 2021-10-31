using ModularEncountersSystems.BlockLogic;
using Sandbox.Game;
using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Tasks {
	public class FixBlock : TaskItem, ITaskItem {

		BaseBlockLogic Block;

		public FixBlock(BaseBlockLogic block) {

			Block = block;
			_tickTrigger = 5;

		}

		public override void Run() {

			_isValid = false;
			if (Block != null && (!Block.Block.Block.IsFunctional && Block.Block.Block.SlimBlock.Integrity == 0)) {

				Block.FunctionalOverride = true;
				Block.WorkingChanged(null);

			}

		}

	}

}
