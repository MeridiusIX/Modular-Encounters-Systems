using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Weapons;
using SpaceEngineers.Game.ModAPI;
using ProtoBuf;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Utils;
using VRageMath;
using ModularEncountersSystems;
using ModularEncountersSystems.Behavior;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Sync;
using ModularEncountersSystems.Behavior.Subsystems.Trigger;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.Core;

namespace ModularEncountersSystems.Behavior.Subsystems {

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

	public class BroadcastSystem {

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
		public IMyRemoteControl RemoteControl;
		private IBehavior _behavior;
		public string LastChatMessageSent;
		public List<IMyRadioAntenna> AntennaList;
		public double HighestRadius;
		public string HighestAntennaRangeName;
		public Vector3D AntennaCoords;
		public Random Rnd;


		public BroadcastSystem(IBehavior behavior, IMyRemoteControl remoteControl = null) {

			UseChatSystem = false;
			UseNotificationSystem = false;
			DelayChatIfSoundPlaying = true;
			SingleChatPerTrigger = true;
			ChatAuthor = "";
			ChatAuthorColor = "";

			ChatControlReference = new List<ChatProfile>();

			RemoteControl = null;
			_behavior = behavior;
			LastChatMessageSent = "";
			AntennaList = new List<IMyRadioAntenna>();
			HighestRadius = 0;
			HighestAntennaRangeName = "";
			AntennaCoords = Vector3D.Zero;
			Rnd = new Random();

			Setup(remoteControl);


		}

		private void Setup(IMyRemoteControl remoteControl) {

			if(remoteControl == null || MyAPIGateway.Entities.Exist(remoteControl?.SlimBlock?.CubeGrid) == false) {

				return;

			}

			this.RemoteControl = remoteControl;
			RefreshAntennaList();

		}

		public void BroadcastRequest(ChatProfile chat) {

			string message = "";
			string sound = "";
			string avatar = "";
			float volume = 1;
			var broadcastType = BroadcastType.None;

			if (!chat.CheckForCustomNames) {

				chat.CheckForCustomNames = true;

				if (_behavior?.CurrentGrid?.Npc?.ChatAuthorName != null)
					chat.Author = _behavior.CurrentGrid.Npc.ChatAuthorName;


			}

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

			if (chat.IgnoreAntennaRequirement || chat.SendToAllOnlinePlayers) {

				this.HighestRadius = chat.IgnoredAntennaRangeOverride;
				this.HighestAntennaRangeName = "";
				this.AntennaCoords = this.RemoteControl.GetPosition();

			} else {

				GetHighestAntennaRange();

				if (this.HighestRadius == 0) {

					BehaviorLogger.Write(chat.ProfileSubtypeId + ": No Valid Antenna", BehaviorDebugEnum.Chat);
					return;

				}

			}

			

			var playerList = new List<IMyPlayer>();
			MyAPIGateway.Players.GetPlayers(playerList);
			BehaviorLogger.Write(chat.ProfileSubtypeId + ": Sending Chat to all Players within distance: " + this.HighestRadius.ToString(), BehaviorDebugEnum.Chat);

			if (message.Contains("{AntennaName}"))
				message = message.Replace("{AntennaName}", this.HighestAntennaRangeName);

			if(this.RemoteControl?.SlimBlock?.CubeGrid?.CustomName != null && message.Contains("{GridName}"))
				message = message.Replace("{GridName}", this.RemoteControl.SlimBlock.CubeGrid.CustomName);
			
			if(chat.UseRandomNameGeneratorFromMES)
				message = RandomNameGenerator.CreateRandomNameFromPattern(message);
			
			var authorName = chat.Author;

			if (authorName.Contains("{AntennaName}"))
				authorName = authorName.Replace("{AntennaName}", this.HighestAntennaRangeName);

			if (this.RemoteControl?.SlimBlock?.CubeGrid?.CustomName != null && authorName.Contains("{GridName}"))
				authorName = authorName.Replace("{GridName}", this.RemoteControl.SlimBlock.CubeGrid.CustomName);

			bool sentToAll = false;

			foreach (var player in playerList) {

				var playerId = chat.SendToAllOnlinePlayers ? 0 : player.IdentityId;
				var playerName = chat.SendToAllOnlinePlayers ? "Player" : player.DisplayName;

				if (!chat.SendToAllOnlinePlayers && (player.IsBot == true || player.Character == null)) {

					continue;

				}

				if(!chat.SendToAllOnlinePlayers && (Vector3D.Distance(player.GetPosition(), this.AntennaCoords) > this.HighestRadius)) {

					continue; //player too far

				}

				var modifiedMsg = message;

				if(modifiedMsg.Contains("{PlayerName}") == true) {

					modifiedMsg = modifiedMsg.Replace("{PlayerName}", playerName);

				}

				if(modifiedMsg.Contains("{PlayerFactionName}") == true) {

					var playerFaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(player.IdentityId);

					if(playerFaction != null) {

						modifiedMsg = modifiedMsg.Replace("{PlayerFactionName}", playerFaction.Name);

					} else {

						modifiedMsg = modifiedMsg.Replace("{PlayerFactionName}", "Unaffiliated");

					}

				}

				if (modifiedMsg.Contains("{GPS}") == true) {
					
					var modifiedLabel = chat.GPSLabel;
					
					if (modifiedLabel.Contains("{AntennaName}")) 
						modifiedLabel = modifiedLabel.Replace("{AntennaName}", this.HighestAntennaRangeName);
					
					if(this.RemoteControl?.SlimBlock?.CubeGrid?.CustomName != null && modifiedLabel.Contains("{GridName}"))
						modifiedLabel = modifiedLabel.Replace("{GridName}", this.RemoteControl.SlimBlock.CubeGrid.CustomName);

					modifiedMsg = modifiedMsg.Replace("{GPS}", GetGPSString(modifiedLabel));
					SendGPSToPlayer(modifiedLabel, RemoteControl.SlimBlock.CubeGrid.WorldAABB.Center, player.IdentityId);

				}

				var authorColor = chat.Color;

				if(authorColor != "White" && authorColor != "Red" && authorColor != "Green" && authorColor != "Blue") {

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

		private void GetHighestAntennaRange() {

			this.HighestRadius = 0;
			this.AntennaCoords = Vector3D.Zero;
			this.HighestAntennaRangeName = "";

			foreach(var antenna in this.AntennaList) {

				if(antenna?.SlimBlock == null) {

					continue;

				}

				if(antenna.IsWorking == false || antenna.IsFunctional == false || antenna.IsBroadcasting == false) {

					continue;

				}

				if(antenna.Radius > this.HighestRadius) {

					this.HighestRadius = antenna.Radius;
					this.AntennaCoords = antenna.GetPosition();

					if (!string.IsNullOrWhiteSpace(antenna.HudText)) {

						this.HighestAntennaRangeName = antenna.HudText;

					} else if (antenna.ShowShipName && !string.IsNullOrWhiteSpace(antenna?.SlimBlock?.CubeGrid?.CustomName)) {

						this.HighestAntennaRangeName = antenna.SlimBlock.CubeGrid.CustomName;

					} else {

						this.HighestAntennaRangeName = antenna.CustomName;

					}
					

				}

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
		public void RefreshAntennaList() {

			if(this.RemoteControl == null || MyAPIGateway.Entities.Exist(this.RemoteControl?.SlimBlock?.CubeGrid) == false) {

				return;

			}

			this.AntennaList.Clear();
			var blockList = BlockCollectionHelper.GetGridAntennas(this.RemoteControl.SlimBlock.CubeGrid);

			foreach(var block in blockList) {

				if(block != null) {

					this.AntennaList.Add(block);

				}

			}

		}

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

	}

}
