using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Sync;
using ModularEncountersSystems.Watchers;
using Sandbox.Game;
using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Tasks {

	public class AfNotify : TaskItem, ITaskItem {

		internal int _secondsToTrigger;
		internal int _elapsedTime;
		internal Effects _effect;

		public AfNotify() {

			_secondsToTrigger = MathTools.RandomBetween(600, 1800);
			_effect = new Effects();
			_effect.Mode = EffectSyncMode.PlayerSound;
			_effect.SoundId = "MES-Audio-AfDNotify";
			_effect.SoundVolume = 1;
			_tickTrigger = 60;
			DateCheck();

		}

		public override void Run() {

			_elapsedTime++;

			if (_elapsedTime < _secondsToTrigger)
				return;

			_secondsToTrigger = MathTools.RandomBetween(600, 1800);
			_elapsedTime = 0;
			EffectManager.ClientReceiveEffect(_effect);
			DateCheck();

		}

		public void DateCheck() {

			var time = DateTime.Now;

			if (time.Month != 4 || time.Day != 1) {

				_isValid = false;

			}
			
		}

	}

}
