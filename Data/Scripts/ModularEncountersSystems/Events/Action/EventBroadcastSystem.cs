using ModularEncountersSystems.Behavior.Subsystems.Trigger;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.Sync;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Events.Action {

	public struct ChatDetails {

		public bool UseChat;
		public double ChatTriggerDistance;
		public int ChatMinTime;
		public int ChatMaxTime;
		public int ChatChance;
		public int MaxChats;
		public List<string> ChatMessages;
		public List<string> ChatAudio;
		public List<BroadcastType> BroadcastChatType;

	}

	public class EventBroadcastSystem {

		//Configurable
		public bool UseChatSystem;
		public bool UseNotificationSystem;
		public bool DelayChatIfSoundPlaying;
		public bool SingleChatPerTrigger;
		public string ChatAuthor;
		public string ChatAuthorColor;

		//New Classes
		public List<ChatProfile> ChatControlReference;

		//Non-Configurable
		public string LastChatMessageSent;
		public List<long> SpecificPlayerIds;

		public Random Rnd;


		public EventBroadcastSystem() {

			UseChatSystem = false;
			UseNotificationSystem = false;
			DelayChatIfSoundPlaying = true;
			SingleChatPerTrigger = true;
			ChatAuthor = "";
			ChatAuthorColor = "";

			ChatControlReference = new List<ChatProfile>();

			LastChatMessageSent = "";
			SpecificPlayerIds = new List<long>();
			Rnd = new Random();

		}


		public void BroadcastRequest(ChatProfile chat, Command command = null, List<long> specificPlayerIds = null) { 

			string message = "";
			string sound = "";
			string avatar = "";
			float volume = 1;
			var broadcastType = BroadcastType.None;


			if (chat.Chance < 100) {

				var roll = Rnd.Next(0, 101);

				if (roll > chat.Chance) {

					BehaviorLogger.Write(chat.ProfileSubtypeId + ": Chat Chance Roll Failed", BehaviorDebugEnum.Chat);
					return;

				}
					
			
			}

			if(chat.ProcessChat(ref message, ref sound, ref broadcastType, ref avatar, ref volume) == false) {

				BehaviorLogger.Write(chat.ProfileSubtypeId + ": Process Chat Fail", BehaviorDebugEnum.Chat);
				return;

			}

			if((this.LastChatMessageSent == message || string.IsNullOrWhiteSpace(message)) && !chat.AllowDuplicatedMessages) {

				BehaviorLogger.Write(chat.ProfileSubtypeId + ": Last Message Same", BehaviorDebugEnum.Chat);
				return;

			}



			var playerList = new List<IMyPlayer>();
			MyAPIGateway.Players.GetPlayers(playerList);
			BehaviorLogger.Write(chat.ProfileSubtypeId + ": Sending Chat to all Players within distance: ", BehaviorDebugEnum.Chat);

			bool sentToAll = false;

			SpecificPlayerIds.Clear();

			if (command != null && chat.SendToCommandPlayer) {

				SpecificPlayerIds.Add(command.PlayerIdentity);

			}





			if (specificPlayerIds != null) {

				foreach (var id in specificPlayerIds)
					SpecificPlayerIds.Add(id);

			}

			foreach (var player in playerList) {



				var playerId = player.IdentityId;
				var playerName = player.DisplayName;
				var modifiedMsg = message;

				if (modifiedMsg.Contains("{PlayerName}") == true)
				{

					modifiedMsg = modifiedMsg.Replace("{PlayerName}", playerName);

				}

				if (chat.SendToSpecificPlayers && chat.PlayerConditionIds != null)
				{
					if (PlayerCondition.ArePlayerConditionsMet(chat.PlayerConditionIds, playerId))
						SpecificPlayerIds.Add(playerId);
				}

				if (chat.SendToCommandPlayer || chat.SendToSpecificPlayers) {

					if (!SpecificPlayerIds.Contains(player.IdentityId))
						continue;
				}

				/*
				if (modifiedMsg.Contains("{GPS}") == true) {
					
					var modifiedLabel = chat.GPSLabel;
					
					if (modifiedLabel.Contains("{AntennaName}")) 
						modifiedLabel = modifiedLabel.Replace("{AntennaName}", this.HighestAntennaRangeName);
					
					if(this.RemoteControl?.SlimBlock?.CubeGrid?.CustomName != null && modifiedLabel.Contains("{GridName}"))
						modifiedLabel = modifiedLabel.Replace("{GridName}", this.RemoteControl.SlimBlock.CubeGrid.CustomName);

					modifiedMsg = modifiedMsg.Replace("{GPS}", GetGPSString(modifiedLabel));
					SendGPSToPlayer(modifiedLabel, RemoteControl.SlimBlock.CubeGrid.WorldAABB.Center, player.IdentityId);

				} 
				*/

				var authorName = chat.Author;
				var authorColor = chat.Color;
				if (authorColor != "White" && authorColor != "Red" && authorColor != "Green" && authorColor != "Blue" && authorColor != "{PlayerRelation}") {

					authorColor = "White";

				}

				if (!sentToAll) {

					if (broadcastType == BroadcastType.Chat || broadcastType == BroadcastType.Both) {

						MyVisualScriptLogicProvider.SendChatMessage(modifiedMsg, authorName, playerId, authorColor);

					}

					if (broadcastType == BroadcastType.Notify || broadcastType == BroadcastType.Both) {

						if (playerId == 0) {

							MyVisualScriptLogicProvider.ShowNotificationToAll(modifiedMsg, 6000, authorColor);

						} else {

							MyVisualScriptLogicProvider.ShowNotification(modifiedMsg, 6000, authorColor, playerId);

						}

					}

				}

				if(string.IsNullOrWhiteSpace(sound) == false && sound != "None") {

					var effect = new Effects();
					effect.Mode = EffectSyncMode.PlayerSound;
					effect.SoundId = sound;
					effect.AvatarId = avatar;
					effect.SoundVolume = volume;
					//MyVisualScriptLogicProvider.ShowNotificationToAll("Volume: " + volume, 4000);
					var sync = new SyncContainer(effect);
					SyncManager.SendSyncMesage(sync, player.SteamUserId);

				}

				if (playerId == 0)
					sentToAll = true;

			}

		}

		public void ProcessAutoMessages() {

			if(MES_SessionCore.IsServer == false || (this.UseChatSystem == false && this.UseNotificationSystem == false)) {

				//BehaviorLogger.AddMsg("Chat System Inactive", true);
				return;

			}

		}



		
		/*
		private void GetRandomChatAndSoundFromLists(List<string> messages, List<string> sounds, List<BroadcastType> broadcastTypes, List<string> avatars, ref string message, ref string sound, ref BroadcastType broadcastType, ref string avatar){

			if(messages.Count == 0) {

				return;

			}

			var index = Rnd.Next(0, messages.Count);
			message = messages[index];

			if(sounds.Count >= messages.Count) {

				sound = sounds[index];

			}

			if(broadcastTypes.Count >= messages.Count) {

				broadcastType = broadcastTypes[index];

			}
			
			if(avatars.Count >= messages.Count) {

				avatar = avatars[index];

			}

		}
		*/

				/*
				internal string GetGPSString(string name) {

					var coords = RemoteControl.SlimBlock.CubeGrid.WorldAABB.Center;

					StringBuilder stringBuilder = new StringBuilder("GPS:", 256);
					stringBuilder.Append(name);
					stringBuilder.Append(":");
					stringBuilder.Append(Math.Round(coords.X, 2).ToString());
					stringBuilder.Append(":");
					stringBuilder.Append(Math.Round(coords.Y, 2).ToString());
					stringBuilder.Append(":");
					stringBuilder.Append(Math.Round(coords.Z, 2).ToString());
					stringBuilder.Append(":");

					return stringBuilder.ToString();

				}

				internal void SendGPSToPlayer(string gpsName, Vector3D gpsCoords, long playerId) {

					var gps = MyAPIGateway.Session.GPS.Create(gpsName, "", gpsCoords, false);
					MyAPIGateway.Session.GPS.AddGps(playerId, gps);

				}

				*/

			}

		}
