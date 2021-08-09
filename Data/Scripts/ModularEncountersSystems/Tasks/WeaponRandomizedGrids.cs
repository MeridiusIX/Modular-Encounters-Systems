using ModularEncountersSystems.API;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.Watchers;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Tasks {
	public class WeaponRandomizedGrids : TaskItem, ITaskItem {

		private GridEntity _grid;
		private bool _notUsingShields;
		private MyEntity _shield;
		private List<MyTuple<long, float, uint>> _lastAttackers;
		private bool _initialNerf;

		public WeaponRandomizedGrids(GridEntity grid) {

			_tickTrigger = 180;
			_grid = grid;
			_lastAttackers = new List<MyTuple<long, float, uint>>();
			DamageHelper.DamageRelay += DamageHandler;

		}

		public override void Run() {

			if (!_initialNerf) {

				SpawnLogger.Write("Setting 800m Initial Range For Weapon Randomized Grid" + _grid.CubeGrid.CustomName, SpawnerDebugEnum.PostSpawn);
				_initialNerf = true;
				_grid.SetAutomatedWeaponRanges(false);

			}

			ShieldValidator();

		}

		public void ShieldValidator() {

			if (!AddonManager.DefenseShields || !APIs.ShieldsApiLoaded || _notUsingShields)
				return;

			if (_shield == null || _shield.MarkedForClose) {

				_shield = APIs.Shields.GetClosestShield(_grid.GetPosition()) as MyEntity;

				if (_shield == null || _shield.MarkedForClose) {

					_notUsingShields = true;
					return;

				}

			}

			APIs.Shields.GetLastAttackers(_shield, _lastAttackers);

			for (int i = 0; i > _lastAttackers.Count; i++) {

				var data = _lastAttackers[i];

				if (FactionHelper.IsIdentityPlayer(data.Item1)) {

					ApplyMaxRange();
					SpawnLogger.Write("Player Damage Detected On Weapon Randomized Shielded Grid" + _grid.CubeGrid.CustomName, SpawnerDebugEnum.PostSpawn);
					_isValid = false;
					break;

				}
			
			}

		}

		public void DamageHandler(object target, MyDamageInformation info) {

			if (info.Amount == 0)
				return;

			var block = target as IMySlimBlock;

			if (block == null)
				return;

			if (!_grid.ActiveEntity()) {

				DamageHelper.DamageRelay -= DamageHandler;
				_isValid = false;
				return;
			
			}

			if (!_grid.CubeGrid.IsSameConstructAs(block.CubeGrid))
				return;

			var id = DamageHelper.GetAttackOwnerId(info.AttackerId);

			if (id == 0)
				return;

			if (!FactionHelper.IsIdentityPlayer(id))
				return;

			ApplyMaxRange();
			DamageHelper.DamageRelay -= DamageHandler;
			_isValid = false;
			SpawnLogger.Write("Player Damage Detected On Weapon Randomized Grid" + _grid.CubeGrid.CustomName, SpawnerDebugEnum.PostSpawn);
			return;

		}

		private void ApplyMaxRange() {

			_grid.SetAutomatedWeaponRanges(true);
			
			if (_grid.Npc != null) {

				_grid.Npc.AppliedAttributes |= World.NpcAttributes.WeaponRandomizationAggression;
				_grid.Npc.Update();

			}
			
		}

	}

}
