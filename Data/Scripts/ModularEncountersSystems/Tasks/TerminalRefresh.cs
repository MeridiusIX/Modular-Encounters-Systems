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
    public class TerminalRefresh : TaskItem, ITaskItem {

        internal bool _screenClosed;
        internal byte _tickCounts;
        internal IMyTerminalBlock _block;


        public TerminalRefresh(IMyTerminalBlock block) {

            _isValid = true;
            _tickTrigger = 1;
            _block = block;

        }

        public override void Run() {

            if (_screenClosed)
                _tickCounts++;

            if (_tickCounts >= 20)
                Refresh();

        }

        public void ScreenClose(ResultEnum result) {

            ShipyardControls.GetPriceQuote(_block);
            _block.RefreshCustomInfo();
            ShipyardControls.RefreshMenu(_block);

        }

        public void Refresh() {

            ShipyardControls.GetPriceQuote(_block);
            _block.RefreshCustomInfo();
            ShipyardControls.RefreshMenu(_block);
            _isValid = false;

        }

    }

}
