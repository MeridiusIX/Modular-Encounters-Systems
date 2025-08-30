using ModularEncountersSystems.Logging;

using ProtoBuf;

using Sandbox.ModAPI;

using System;

using VRage.Game.ModAPI;
using Sandbox.Game;
using VRage.Game;
using VRage.Utils;
using VRage.ObjectBuilders;
using ModularEncountersSystems.Entities;

namespace ModularEncountersSystems.Sync
{
	[ProtoContract]
    public class AddBlockData
    {

        [ProtoMember(1)]
        public MyObjectBuilder_CubeBlock NewBlock_ob;

        [ProtoMember(2)]
        private long CubeGrid_EntityId;


        public AddBlockData() { }

        public AddBlockData(MyObjectBuilder_CubeBlock _NewBlock_ob, long _CubeGrid_EntityId)
        {
            NewBlock_ob = _NewBlock_ob;
            CubeGrid_EntityId = _CubeGrid_EntityId;
        }

        public void Received()
        {

            var Grid = GridManager.GetGridEntity(CubeGrid_EntityId);

            if (Grid != null)
            {
                Grid.CubeGrid.AddBlock(NewBlock_ob,false); 
            }
        }
    }

}