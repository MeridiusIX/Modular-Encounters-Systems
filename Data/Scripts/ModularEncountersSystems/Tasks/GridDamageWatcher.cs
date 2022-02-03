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
	public class GridDamageWatcher : TaskItem, ITaskItem {

		internal GridEntity _grid;
		internal bool _notUsingShields;
		internal MyEntity _shield;
		internal List<MyTuple<long, float, uint>> _lastAttackers;
		internal bool _firstRun;

		public GridDamageWatcher(GridEntity grid) {

			_tickTrigger = 180;
			_grid = grid;
			_lastAttackers = new List<MyTuple<long, float, uint>>();
			DamageHelper.DamageRelay += DamageHandler;
			TaskProcessor.Tasks.Add(this);

		}

		public override void Run() {

			if (!_firstRun) {

				_firstRun = true;
				FirstRun();
				
			}

			ShieldValidator();

		}

		internal virtual void FirstRun() {
		
		
		
		}

		internal virtual void DamageDetect() {
		
			
		
		}

		public override void Invalidate() {

			base.Invalidate();
			DamageHelper.DamageRelay -= DamageHandler;

		}

		public void ShieldValidator() {

			if (!AddonManager.DefenseShields || !APIs.ShieldsApiLoaded || _notUsingShields)
				return;

			if (!_grid.ActiveEntity()) {

				DamageHelper.DamageRelay -= DamageHandler;
				_isValid = false;
				return;

			}

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

					DamageDetect();
					SavePlayerDamage();
					

					_isValid = false;
					break;

				}
			
			}

		}

		internal void SavePlayerDamage() {

			if (_grid.Npc == null)
				return;

			_grid.Npc.Attributes.ReceivedPlayerDamage = true;
			_grid.Npc.AppliedAttributes.ReceivedPlayerDamage = true;
			MyAPIGateway.Utilities.InvokeOnGameThread(() => _grid.Npc.Update());

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

			DamageDetect();
			SavePlayerDamage();
			DamageHelper.DamageRelay -= DamageHandler;
			_isValid = false;
			return;

		}

	}

}
