using ModularEncountersSystems.Behavior;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Sync;
using ModularEncountersSystems.Terminal;
using ModularEncountersSystems.Watchers;
using ModularEncountersSystems.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender.Messages;

namespace ModularEncountersSystems.Tasks {
    public class AddBlock : TaskItem, ITaskItem {

        internal bool _screenClosed;
        internal MyObjectBuilder_CubeBlock _block;
        internal IMyCubeGrid _parentGrid;

        
        public AddBlock(MyObjectBuilder_CubeBlock blockToAdd, IMyCubeGrid parentGrid) {

            _isValid = true;
            _block = blockToAdd;
            _parentGrid = parentGrid;

        }

        public override void Run() {

            _block.EntityId = 0;

            var spawned_block = _parentGrid.AddBlock(_block, false);
            //MyLog.Default.WriteLineAndConsole($"newblock:{spawned_block != null}");
            //MyVisualScriptLogicProvider.SendChatMessage($"newblock:{spawned_block != null}");
            if (spawned_block == null)
            {
                //Try again!
                return;
            }

            // gently remind the client that a new block has been spawned
            MyAPIGateway.Multiplayer.SendMessageToOthers(SyncManager.NetworkId, MyAPIGateway.Utilities.SerializeToBinary<MyObjectBuilder_CubeBlock>(spawned_block.GetObjectBuilder(true)));

            // gently remind the client that it belongs to a grid, this could be particularly problematic when it was the only block in the grid

            MyAPIGateway.Multiplayer.SendMessageToOthers(SyncManager.NetworkId, MyAPIGateway.Utilities.SerializeToBinary<MyObjectBuilder_EntityBase>(_parentGrid.GetObjectBuilder(true)));


            _isValid =false;
        }





    }

}
