using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Watchers;
using ModularEncountersSystems.World;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Tasks {
    public class NewGrid : TaskItem, ITaskItem {

        internal GridEntity _grid;
        internal int _runCount;

        public NewGrid(GridEntity grid) {

            _isValid = true;
            _tickTrigger = 60;
            _grid = grid;

        }

        public override void Run() {

            _isValid = false;

            if (_grid == null || !_grid.ActiveEntity())
                return;

            _grid.RecheckOwnershipMajority = true;
            EntityEvaluator.GetGridOwnerships(_grid);

            if (!_grid.Ownership.HasFlag(GridOwnershipEnum.NpcMajority) && !_grid.Ownership.HasFlag(GridOwnershipEnum.NpcMinority) && _grid.Ownership != GridOwnershipEnum.None)
                return;

            if (_grid.Npc == null) {

                _grid.CheckForNpcData();

                if (_grid.Npc == null) {

                    //Create NPC Data
                    _grid.DebugData.Append(" - Creating Blank NPC Data").AppendLine();
                    _grid.Npc = new NpcData();
                    _grid.Npc.Grid = _grid;
                    _grid.Npc.SpawnType = SpawningType.OtherNPC;
                    
                }

            }

            _grid.Npc.ProcessTertiaryAttributes();

            //Add Grid To Active NPCs
            if (!NpcManager.ActiveNpcs.Contains(_grid)) {

                NpcManager.ActiveNpcs.Add(_grid);
                _grid.OwnershipMajorityChange += NpcManager.OwnershipMajorityChange;

                if (_grid.Npc.Attributes.IsCargoShip)
                    CargoShipWatcher.CargoShips.Add(_grid);

            }

        }

    }

}
