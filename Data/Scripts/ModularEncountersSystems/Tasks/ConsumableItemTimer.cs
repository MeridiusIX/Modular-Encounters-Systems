using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Watchers;
using Sandbox.Game;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Tasks {
	public class ConsumableItemTimer : TaskItem, ITaskItem {

		private int _timer;
		private bool _expired;
		private long _playerId;
		private string _consumableType;
		private bool _warning;
		private bool _noEnergyConsumption;

		public ConsumableItemTimer(int timerSeconds, long playerId, string consumableType = null, bool ignoreEnergy = false) {

			_tickTrigger = 60;
			_playerId = playerId;
			_consumableType = consumableType;
			_noEnergyConsumption = ignoreEnergy;
			ResetTimer(timerSeconds);

		}

		public override void Run() {

			_timer--;

			if (_timer > 0) {

				if (!_warning && _timer <= 6) {

					_warning = true;
					MyVisualScriptLogicProvider.ShowNotification(_consumableType + " Effect About To Expire", 4000, "Red", _playerId);

				}

				if (_warning && _timer > 6) {

					_warning = false;

				}

				return;
			
			}

			ExpireConsumableEffect();

		}

		public bool EffectActive() {

			return _isValid && _timer > 0;
		
		}

		public void ExpireConsumableEffect() {

			_isValid = false;
			_timer = 0;

			if (!_expired && !string.IsNullOrWhiteSpace(_consumableType))
				MyVisualScriptLogicProvider.ShowNotification(_consumableType + " Effect Has Expired", 4000, "Red", _playerId);

			_expired = true;

		}

		public void ResetTimer(int newTimer) {

			_timer = newTimer;
			if (!_expired && !string.IsNullOrWhiteSpace(_consumableType))
				MyVisualScriptLogicProvider.ShowNotification(_consumableType + " Effect Now Active For The Next " + newTimer + " Seconds", 4000, "Green", _playerId);

		}

	}

}
