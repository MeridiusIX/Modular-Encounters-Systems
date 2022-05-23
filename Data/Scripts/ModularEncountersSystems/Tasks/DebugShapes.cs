using ModularEncountersSystems.Behavior;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Watchers;
using ModularEncountersSystems.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace ModularEncountersSystems.Tasks {
    public class DebugShapes : TaskItem, ITaskItem {

        internal short _maxRuns;

        public DebugShapes() {

            _isValid = true;
            _tickTrigger = 1;
            _maxRuns = 300;

        }

        public override void Run() {

            _maxRuns--;

            if (_maxRuns <= 0) {

                _isValid = false;
                return;
            
            }

        }

    }

}
