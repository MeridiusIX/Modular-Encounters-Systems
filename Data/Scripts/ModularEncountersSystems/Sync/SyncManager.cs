using ModularEncountersSystems.Core;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Tasks;
using Sandbox.ModAPI;
using System;

namespace ModularEncountersSystems.Sync {
    public static class SyncManager {

        public static ushort NetworkId = 42007;

        public static void Setup() {

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
