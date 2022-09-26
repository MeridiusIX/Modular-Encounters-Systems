using ModularEncountersSystems.Behavior;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Terminal;
using ModularEncountersSystems.Watchers;
using ModularEncountersSystems.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;

namespace ModularEncountersSystems.Tasks {
    public class TerminalTransactionRefresh : TaskItem, ITaskItem {

        internal bool _screenClosed;
        internal byte _tickCounts;
        internal IMyTerminalBlock _block;
        internal Action<IMyTerminalBlock> _additionalAction;


        public TerminalTransactionRefresh(IMyTerminalBlock block, Action<IMyTerminalBlock> additionalAction = null) {

            _isValid = true;
            _tickTrigger = 1;
            _block = block;
            _additionalAction = additionalAction;

        }

        public override void Run() {

            if (_screenClosed)
                _tickCounts++;

            if (_tickCounts >= 20)
                Refresh();

        }

        public void ScreenClose(ResultEnum result) {

            _additionalAction?.Invoke(_block);
            _block.RefreshCustomInfo();
            ControlManager.RefreshMenu(_block);
            _screenClosed = true;

        }

        public void Refresh() {

            _additionalAction?.Invoke(_block);
            _block.RefreshCustomInfo();
            ControlManager.RefreshMenu(_block);
            _isValid = false;

        }

    }

}
