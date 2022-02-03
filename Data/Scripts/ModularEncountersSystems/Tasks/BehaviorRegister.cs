using ModularEncountersSystems.Behavior;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Watchers;
using ModularEncountersSystems.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Tasks {
    public class BehaviorRegister : TaskItem, ITaskItem {

        internal IMyRemoteControl _remote;

        public BehaviorRegister(IMyRemoteControl remote) {

            _isValid = true;
            _tickTrigger = 15;
            _remote = remote;

        }

        public override void Run() {

            _isValid = false;

            if (_remote != null && !_remote.MarkedForClose) {

                MyAPIGateway.Parallel.Start(() => {

                    BehaviorManager.RegisterBehaviorFromRemoteControl(_remote);

                });

            }

        }

    }

}
