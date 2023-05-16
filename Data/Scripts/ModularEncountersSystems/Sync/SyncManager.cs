using ModularEncountersSystems.Core;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Progression;
using ModularEncountersSystems.Tasks;
using ModularEncountersSystems.Terminal;
using Sandbox.ModAPI;
using System;

namespace ModularEncountersSystems.Sync {
    public static class SyncManager {

        public static ushort NetworkId = 42007;
        public static bool SetupDone = false;

        public static void Setup() {

            if (SetupDone)
                return;

            SetupDone = true;
            MyAPIGateway.Multiplayer.RegisterSecureMessageHandler(NetworkId, NetworkMessageReceiver);
            MyAPIGateway.Utilities.MessageEntered += ChatManager.ChatReceived;
            TaskProcessor.Tick10.Tasks += EffectManager.ProcessPlayerSoundEffect;
            MES_SessionCore.UnloadActions += Close;

        }
        
        public static void SendSyncMesage(SyncContainer syncContainer, ulong userId = 0, bool sendServer = false, bool sendOthers = false){
        
            var byteData = MyAPIGateway.Utilities.SerializeToBinary<SyncContainer>(syncContainer);
            
            if(userId != 0){
            
                MyAPIGateway.Multiplayer.SendMessageTo(NetworkId, byteData, userId);
            
            }
            
            if(sendServer == true){
            
                MyAPIGateway.Multiplayer.SendMessageToServer(NetworkId, byteData);
            
            }
            
            if(sendOthers == true){
            
                MyAPIGateway.Multiplayer.SendMessageToOthers(NetworkId, byteData);
            
            }
        
        }

        public static void NetworkMessageReceiver(ushort handlerId, byte[] initialData, ulong sender, bool fromServer) {

            try {

                var container = MyAPIGateway.Utilities.SerializeFromBinary<SyncContainer>(initialData);

                if(container == null || container.Sender != "MES") {

                    return;

                }

                if(container.Mode == SyncMode.ChatCommand) {

                    var chatData = MyAPIGateway.Utilities.SerializeFromBinary<ChatMessage>(container.Data);

                    if(chatData != null) {

                        ChatManager.ChatReceivedNetwork(chatData);

                    }

                }

                if(container.Mode == SyncMode.Effect) {

                    var effectData = MyAPIGateway.Utilities.SerializeFromBinary<Effects>(container.Data);

                    if(effectData != null) {

                        EffectManager.ClientReceiveEffect(effectData);

                    }

                }

                if (container.Mode == SyncMode.ReputationAlert) {

                    var alertData = MyAPIGateway.Utilities.SerializeFromBinary<ReputationMessage>(container.Data);

                    if (alertData != null) {

                        ReputationAnnounceManager.ProcessMessage(alertData);

                    }

                }

                if (container.Mode == SyncMode.ReputationChangeClient) {

                    var repChange = MyAPIGateway.Utilities.SerializeFromBinary<ReputationChange>(container.Data);

                    if (repChange != null) {

                        repChange.Process();

                    }

                }

                if (container.Mode == SyncMode.ShipyardTransaction) {

                    var shipYardData = MyAPIGateway.Utilities.SerializeFromBinary<ShipyardTransaction>(container.Data);

                    if (shipYardData != null) {

                        shipYardData.ProcessTransaction(sender);

                    }

                }

                if (container.Mode == SyncMode.ShipyardTransactionResult) {

                    var shipYardData = MyAPIGateway.Utilities.SerializeFromBinary<ShipyardTransactionResult>(container.Data);

                    if (shipYardData != null) {

                        shipYardData.ProcessMessage();

                    }

                }

                if (container.Mode == SyncMode.SuitUpgradeNewPlayerStats) {

                    if (MyAPIGateway.Multiplayer.IsServer) {

                        var newContainer = PlayerManager.GetProgressionContainer(container.IdentityId, sender);

                        if (newContainer == null)
                            return;

                        var serializedContainer = MyAPIGateway.Utilities.SerializeToBinary<ProgressionContainer>(newContainer);
                        container.Data = serializedContainer;
                        container.Mode = SyncMode.SuitUpgradePlayerStats;
                        SendSyncMesage(container, sender);

                    }

                }

                if (container.Mode == SyncMode.SuitUpgradePlayerStats) {

                    var suitData = MyAPIGateway.Utilities.SerializeFromBinary<ProgressionContainer>(container.Data);

                    if (suitData == null) {

                        return;

                    }

                    if (MyAPIGateway.Multiplayer.IsServer) {

                        if (MyAPIGateway.Session.LocalHumanPlayer == null || MyAPIGateway.Session.LocalHumanPlayer.SteamUserId != sender) {

                            var newContainer = PlayerManager.GetProgressionContainer(suitData.IdentityId, sender);

                            if (newContainer == null)
                                return;

                            var serializedContainer = MyAPIGateway.Utilities.SerializeToBinary<ProgressionContainer>(newContainer);
                            container.Data = serializedContainer;
                            SendSyncMesage(container, sender);


                        } else {

                            SuitModificationControls.UpdateProgressionContainer(suitData);

                        }
                    
                    } else {

                        SuitModificationControls.UpdateProgressionContainer(suitData);
                    
                    }

                }

                if (container.Mode == SyncMode.SuitUpgradeTransaction) {

                    var suitData = MyAPIGateway.Utilities.SerializeFromBinary<SuitUpgradeTransaction>(container.Data);

                    if (suitData == null) {

                        return;

                    }

                    if (suitData.Result == SuitUpgradeTransactionResult.None) {

                        suitData.ProcessTransaction(sender);

                        if (suitData.Result == SuitUpgradeTransactionResult.None)
                            suitData.Result = SuitUpgradeTransactionResult.NotProcessedOnServer;

                        container.Data = MyAPIGateway.Utilities.SerializeToBinary<SuitUpgradeTransaction>(suitData);
                        SendSyncMesage(container, sender);

                    } else {

                        suitData.ProcessResult();

                    }

                }

            } catch(Exception exc) {

                SpawnLogger.Write("Exception in NetworkMessageReceiver", SpawnerDebugEnum.Error, true);
                SpawnLogger.Write(exc.ToString(), SpawnerDebugEnum.Error, true);

            }

        }

        public static void Close() {

            MyAPIGateway.Utilities.MessageEntered -= ChatManager.ChatReceived;
            MyAPIGateway.Multiplayer.UnregisterSecureMessageHandler(NetworkId, NetworkMessageReceiver);

        }

    }
}
