using ModularEncountersSystems.Core;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using Sandbox.Game;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Sync {
    public static class ChatManager {

        public static void ChatReceived(string messageText, ref bool sendToOthers) {

            var thisPlayer = MyAPIGateway.Session.LocalHumanPlayer;

            if (MyAPIGateway.Session.LocalHumanPlayer == null) {

                SpawnLogger.Write("Player Null, Cannot Send Chat", SpawnerDebugEnum.Settings);
                return;

            }
             
            bool isAdmin = false;

            if (messageText.StartsWith("/MES.")) {

                sendToOthers = false;
                

                if (thisPlayer.PromoteLevel == MyPromoteLevel.Admin || thisPlayer.PromoteLevel == MyPromoteLevel.Owner) {

                    isAdmin = true;

                }

                if (!isAdmin)
                    MyVisualScriptLogicProvider.ShowNotification("Access Denied. Modular Encounters Systems Chat Commands Only Available To Admin Players.", 5000, "Red", thisPlayer.IdentityId);

            }

            var chatData = new ChatMessage();
            chatData.Mode = ChatMsgMode.ServerProcessing;
            chatData.Message = messageText;
            chatData.PlayerId = thisPlayer.IdentityId;
            chatData.SteamId = thisPlayer.SteamUserId;
            chatData.PlayerPosition = thisPlayer.GetPosition();
            chatData.PlayerEntity = thisPlayer.Controller?.ControlledEntity?.Entity != null ? thisPlayer.Controller.ControlledEntity.Entity.EntityId : 0;
            chatData.IsAdmin = isAdmin;
            chatData.CameraPosition = MyAPIGateway.Session.Camera.WorldMatrix.Translation;
            chatData.CameraDirection = MyAPIGateway.Session.Camera.WorldMatrix.Forward;
            SendChatDataOverNetwork(chatData, true);

        }

        public static void ChatFromApi(string msg, MatrixD playerPos, long owner, ulong steamId) {

            var chat = new ChatMessage();
            chat.Mode = ChatMsgMode.ServerProcessing;
            chat.Message = msg;
            chat.PlayerId = owner;
            chat.SteamId = steamId;
            chat.PlayerPosition = playerPos.Translation;
            chat.CameraDirection = playerPos.Forward;
            chat.CameraPosition = playerPos.Translation;
            chat.IsAdmin = true;
            ChatReceivedNetwork(chat);

        }

        public static void ChatReceivedNetwork(ChatMessage chatData) {

            if(chatData.Mode == ChatMsgMode.ServerProcessing) {

                ProcessServerChat(chatData);

            }

            if(chatData.Mode == ChatMsgMode.ReturnMessage) {

                ProcessReturnChat(chatData);

            }

        }

        public static void ProcessServerChat(ChatMessage chatData) {

            var newChatData = chatData;
            
            if (newChatData.IsAdmin) {

                SpawnLogger.Write("Processing Chat Command", SpawnerDebugEnum.Settings);

                if (newChatData.Message != "/noprocessing" && newChatData.ProcessChat()) {

                    if (newChatData.Mode == ChatMsgMode.ReturnMessage) {

                        SendChatDataOverNetwork(newChatData, false);

                    } else if (!string.IsNullOrWhiteSpace(newChatData.ReturnMessage)) {

                        MyVisualScriptLogicProvider.ShowNotification(newChatData.ReturnMessage, 5000, "White", newChatData.PlayerId);

                    }

                    return;

                } else {

                    if (!string.IsNullOrWhiteSpace(newChatData.ReturnMessage)) {

                        MyVisualScriptLogicProvider.ShowNotification(newChatData.ReturnMessage, 5000, "White", newChatData.PlayerId);

                    }

                    if (newChatData.Message == "/noprocessing") {

                        SendChatDataOverNetwork(newChatData, false);

                    }

                    return;

                }
                    

            }

            SpawnLogger.Write("Send Chat as RAI Command", SpawnerDebugEnum.Settings);

            IMyEntity playerEntity = null;

            if (!MyAPIGateway.Entities.TryGetEntityById(newChatData.PlayerEntity, out playerEntity))
                return;

            var command = new Command();
            command.CommandCode = newChatData.Message;
            command.Type = CommandType.PlayerChat;
            command.Character = playerEntity;
            command.UseTriggerTargetDistance = true;
            command.Position = newChatData.PlayerPosition;
            command.PlayerIdentity = newChatData.PlayerId;
            CommandHelper.CommandTrigger?.Invoke(command);

        }

        public static void ProcessReturnChat(ChatMessage chatData) {

            if (string.IsNullOrWhiteSpace(chatData.ClipboardPayload) == false) {

                VRage.Utils.MyClipboardHelper.SetClipboard(chatData.ClipboardPayload);

            }

            if (chatData.UnlockAdminBlocks) {

                DefinitionHelper.UnlockNpcBlocks();

            }

            if (chatData.ReturnMessage.StartsWith("Developer Mode Set")) {

                MES_SessionCore.DeveloperMode = chatData.DeveloperMode;
            
            }

        }

        public static void SendChatDataOverNetwork(ChatMessage chatData, bool sendToServer) {

            if(string.IsNullOrWhiteSpace(chatData.ReturnMessage) == false && sendToServer == false) {

                MyVisualScriptLogicProvider.ShowNotification(chatData.ReturnMessage, 5000, "White", chatData.PlayerId);

            }

            var byteChatData = MyAPIGateway.Utilities.SerializeToBinary<ChatMessage>(chatData);
            var syncData = new SyncContainer(SyncMode.ChatCommand, byteChatData);
            var byteSyncData = MyAPIGateway.Utilities.SerializeToBinary<SyncContainer>(syncData);

            if(sendToServer == true) {

                MyAPIGateway.Multiplayer.SendMessageToServer(SyncManager.NetworkId, byteSyncData);

            } else {

                MyAPIGateway.Multiplayer.SendMessageTo(SyncManager.NetworkId, byteSyncData, chatData.SteamId);

            }
 
        }

    }
}
