using ModularEncountersSystems.Behavior;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Watchers;
using ModularEncountersSystems.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Tasks {
    public class DelayedCommand : TaskItem, ITaskItem {

        internal Command _command;

        public DelayedCommand(Command command) {

            _isValid = true;
            _command = command;
            _tickTrigger = (short)command.DelayTicks;

        }

        public override void Run() {

            _isValid = false;
            CommandHelper.SendCommand(_command, true);

        }

    }

}
