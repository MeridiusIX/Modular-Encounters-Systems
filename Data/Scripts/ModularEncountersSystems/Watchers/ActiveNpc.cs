using ModularEncountersSystems.Entities;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Watchers {

    [ProtoContract]
    public class ActiveNpc {

        public long ParentEntityId;

        private GridEntity _parentEntity;


        public ActiveNpc() {
        
            
        
        }

    }

}
