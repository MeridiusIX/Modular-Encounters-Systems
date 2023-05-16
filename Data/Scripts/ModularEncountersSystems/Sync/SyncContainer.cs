using ProtoBuf;
using Sandbox.ModAPI;

namespace ModularEncountersSystems.Sync {

    public enum SyncMode {

        None,
        BehaviorChange,
        Effect,
        ChatCommand,
        ReputationAlert,
        ClipboardRequest,
        ShipyardTransaction,
        ShipyardTransactionResult,
        SuitUpgradePlayerStats,
        SuitUpgradeTransaction,
        SuitUpgradeNewPlayerStats,
        ReputationChangeClient,

    }

    [ProtoContract]
    public class SyncContainer {

        [ProtoMember(1)]
        public SyncMode Mode;

        [ProtoMember(2)]
        public byte[] Data;

        [ProtoMember(3)]
        public string Sender;

        [ProtoMember(4)]
        public long IdentityId;

        [ProtoMember(5)]
        public ulong SteamId;

        public SyncContainer() {

            Mode = SyncMode.None;
            Data = new byte[0];
            Sender = "MES";

        }

        public SyncContainer(SyncMode mode, byte[] data) {

            Mode = mode;
            Data = data;
            Sender = "MES";

        }
        
        public SyncContainer(Effects effect){
            
            this.Mode = SyncMode.Effect;
            this.Data = MyAPIGateway.Utilities.SerializeToBinary<Effects>(effect);
            this.Sender = "MES";

        }

        public SyncContainer(ReputationMessage repAlert) {

            this.Mode = SyncMode.ReputationAlert;
            this.Data = MyAPIGateway.Utilities.SerializeToBinary<ReputationMessage>(repAlert);
            this.Sender = "MES";

        }

    }

}
