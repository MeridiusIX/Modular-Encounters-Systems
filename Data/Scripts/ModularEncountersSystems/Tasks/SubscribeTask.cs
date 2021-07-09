using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Tasks {
	public class SubscribeTask : TaskItem, ITaskItem{

		public Action Tasks;

		public override void Run() {

			Tasks?.Invoke();
			
		}

		public SubscribeTask(byte counterTrigger) {

			_tickTrigger = counterTrigger;

		}

	}
}
