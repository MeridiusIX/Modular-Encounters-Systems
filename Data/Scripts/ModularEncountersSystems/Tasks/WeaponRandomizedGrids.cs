using ModularEncountersSystems.API;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Watchers;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Tasks {
	public class WeaponRandomizedGrids : TaskItem, ITaskItem {

		private GridEntity _grid;
		private IMyEntity _shield;

		public WeaponRandomizedGrids(GridEntity grid) {

			_tickTrigger = 180;
			_grid = grid;
		
		}

		public override void Run() {

			

		}

		public void ShieldValidator() {

			if (!AddonManager.DefenseShields)
				return;


		
		}

		public void DamageHandler(object target, MyDamageInformation info) {
		
			
		
		}

	}

}
