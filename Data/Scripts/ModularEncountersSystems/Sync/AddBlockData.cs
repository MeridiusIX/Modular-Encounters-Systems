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
using ModularEncountersSystems.Spawning.Procedural;
using ModularEncountersSystems.Tasks;
using VRage.ModAPI;

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

            IMyEntity gridEntity = null;

            if (MyAPIGateway.Entities.TryGetEntityById(CubeGrid_EntityId, out gridEntity))
            {

                var grid = gridEntity as IMyCubeGrid;

                if (grid != null)
                {

                    var task = new AddBlock(NewBlock_ob, grid);
                    TaskProcessor.Tasks.Add(task);


                }
                else
                {
                    MyAPIGateway.Utilities.ShowMessage("MES", "Send id is not cubegrid?");
                }

            }
            else
            {
                MyAPIGateway.Utilities.ShowMessage("MES","could not find Cubegrid via ID");
            }

        }
    }

}