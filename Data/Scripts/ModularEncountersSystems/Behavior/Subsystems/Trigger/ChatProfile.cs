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
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;

namespace ModularEncountersSystems.Behavior.Subsystems.Trigger {

	[ProtoContract]
	public class ChatProfile {

		[ProtoMember(1)]
		public bool UseChat;

		[ProtoMember(2)]
		public int MinTime;

		[ProtoMember(3)]
		public int MaxTime;

		[ProtoMember(4)]
		public bool StartsReady;

		[ProtoMember(5)]
		public int Chance;

		[ProtoMember(6)]
		public int MaxChats;

		[ProtoMember(7)]
		public bool BroadcastRandomly;

		[ProtoMember(8)]
		public List<string> ChatMessages;

		[ProtoMember(9)]
		public List<string> ChatAudio;

		[ProtoMember(10)]
		public List<BroadcastType> BroadcastChatType;

		[ProtoMember(11)]
		public int SecondsUntilChat;

		[ProtoMember(12)]
		public int ChatSentCount;

		[ProtoMember(13)]
		public DateTime LastChatTime;

		[ProtoMember(14)]
		public int MessageIndex;

		[ProtoMember(15)]
		public string Author;

		[ProtoMember(16)]
		public string Color;

		[ProtoMember(17)]
		public string ProfileSubtypeId;

		[ProtoMember(18)]
		public bool IgnoreAntennaRequirement;

		[ProtoMember(19)]
		public double IgnoredAntennaRangeOverride;

		[ProtoMember(20)]
		public bool UseRandomNameGeneratorFromMES;

		[ProtoMember(21)]
		public List<string> ChatAvatar;

		[ProtoMember(22)]
		public bool SendToAllOnlinePlayers;

		[ProtoMember(23)]
		public string GPSLabel;

		[ProtoMember(24)]
		public List<float> ChatVolumeMultiplier;

		[ProtoIgnore]
		public Random Rnd;

		public ChatProfile() {

			UseChat = false;
			MinTime = 0;
			MaxTime = 1;
			StartsReady = false;
			Chance = 100;
			MaxChats = -1;
			BroadcastRandomly = false;
			ChatMessages = new List<string>();
			ChatAudio = new List<string>();
			BroadcastChatType = new List<BroadcastType>();
			Author = "";
			Color = "";
			ProfileSubtypeId = "";
			IgnoreAntennaRequirement = false;
			IgnoredAntennaRangeOverride = 0;
			UseRandomNameGeneratorFromMES = false;
			ChatAvatar = new List<string>();
			SendToAllOnlinePlayers = false;
			GPSLabel = "";
			ChatVolumeMultiplier = new List<float>();

			SecondsUntilChat = 0;
			ChatSentCount = 0;
			LastChatTime = MyAPIGateway.Session.GameDateTime;
			MessageIndex = 0;

			Rnd = new Random();

		}

		public bool ProcessChat(ref string msg, ref string audio, ref BroadcastType type, ref string avatar, ref float volume) {

			if (UseChat == false) {

				BehaviorLogger.Write(" - UseChat False", BehaviorDebugEnum.Chat);
				return false;

			}

			if (MaxChats >= 0 && ChatSentCount >= MaxChats) {

				BehaviorLogger.Write(" - Max Chats Sent", BehaviorDebugEnum.Chat);
				UseChat = false;
				return false;

			}

			TimeSpan duration = MyAPIGateway.Session.GameDateTime - LastChatTime;

			if (duration.TotalSeconds < SecondsUntilChat) {

				BehaviorLogger.Write(" - Chat Timer Not Ready", BehaviorDebugEnum.Chat);
				return false;

			}

			string thisMsg = "";
			string thisSound = "";
			string thisAvatar = "";
			float thisVolume = 1;
			BroadcastType thisType = BroadcastType.None;

			GetChatAndSoundFromLists(ref thisMsg, ref thisSound, ref thisType, ref thisAvatar, ref thisVolume);

			if (string.IsNullOrWhiteSpace(thisMsg) == true || thisType == BroadcastType.None) {

				BehaviorLogger.Write(" - Message Null or Broadcast None", BehaviorDebugEnum.Chat);
				return false;

			}

			LastChatTime = MyAPIGateway.Session.GameDateTime;
			SecondsUntilChat = Rnd.Next(MinTime, MaxTime);
			ChatSentCount++;

			msg = thisMsg;
			audio = thisSound;
			type = thisType;
			avatar = thisAvatar;
			volume = thisVolume;
			return true;

		}

		public void InitTags(string customData) {

			if (string.IsNullOrWhiteSpace(customData) == false) {

				var descSplit = customData.Split('\n');

				foreach (var tag in descSplit) {

					//UseChat
					if (tag.Contains("[UseChat:") == true) {

						TagParse.TagBoolCheck(tag, ref UseChat);

					}

					//ChatMinTime
					if (tag.Contains("[MinTime:") == true) {

						TagParse.TagIntCheck(tag, ref MinTime);

					}

					//ChatMaxTime
					if (tag.Contains("[MaxTime:") == true) {

						TagParse.TagIntCheck(tag, ref MaxTime);

					}

					//ChatStartsReady
					if (tag.Contains("[StartsReady:") == true) {

						TagParse.TagBoolCheck(tag, ref StartsReady);

					}

					//ChatChance
					if (tag.Contains("[Chance:") == true) {

						TagParse.TagIntCheck(tag, ref Chance);

					}

					//MaxChats
					if (tag.Contains("[MaxChats:") == true) {

						TagParse.TagIntCheck(tag, ref MaxChats);

					}

					//BroadcastRandomly
					if (tag.Contains("[BroadcastRandomly:") == true) {

						TagParse.TagBoolCheck(tag, ref BroadcastRandomly);

					}

					//ChatMessages
					if (tag.Contains("[ChatMessages:") == true) {

						TagParse.TagStringListCheck(tag, ref ChatMessages);

					}

					//ChatAudio
					if (tag.Contains("[ChatAudio:") == true) {

						TagParse.TagStringListCheck(tag, ref ChatAudio);

					}

					//ChatAvatar
					if (tag.Contains("[ChatAvatar:") == true) {

						TagParse.TagStringListCheck(tag, ref ChatAvatar);

					}

					//SendToAllOnlinePlayers
					if (tag.Contains("[SendToAllOnlinePlayers:") == true) {

						TagParse.TagBoolCheck(tag, ref SendToAllOnlinePlayers);

					}

					//GPSLabel
					if (tag.Contains("[GPSLabel:")) {

						TagParse.TagStringCheck(tag, ref GPSLabel);

					}

					//BroadcastChatType
					if (tag.Contains("[BroadcastChatType:") == true) {

						TagParse.TagBroadcastTypeEnumCheck(tag, ref BroadcastChatType);

					}

					//Author
					if (tag.Contains("[Author:") == true) {

						TagParse.TagStringCheck(tag, ref Author);

					}

					//Color
					if (tag.Contains("[Color:") == true) {

						TagParse.TagStringCheck(tag, ref Color);

					}

					//IgnoreAntennaRequirement
					if (tag.Contains("[IgnoreAntennaRequirement:") == true) {

						TagParse.TagBoolCheck(tag, ref IgnoreAntennaRequirement);

					}

					//IgnoredAntennaRangeOverride
					if (tag.Contains("[IgnoredAntennaRangeOverride:") == true) {

						TagParse.TagDoubleCheck(tag, ref IgnoredAntennaRangeOverride);

					}

					//UseRandomNameGeneratorFromMES
					if (tag.Contains("[UseRandomNameGeneratorFromMES:") == true) {

						TagParse.TagBoolCheck(tag, ref UseRandomNameGeneratorFromMES);

					}

					//ChatVolumeMultiplier
					if (tag.Contains("[ChatVolumeMultiplier:") == true) {

						TagParse.TagFloatCheck(tag, ref ChatVolumeMultiplier);

					}

				}

			}

			if (MinTime > MaxTime) {

				MinTime = MaxTime;

			}

			if (StartsReady == true) {

				SecondsUntilChat = 0;

			} else {

				SecondsUntilChat = Rnd.Next(MinTime, MaxTime);

			}


		}

		private void GetChatAndSoundFromLists(ref string message, ref string sound, ref BroadcastType type, ref string avatar, ref float volume) {

			if (ChatMessages.Count == 0) {

				return;

			}

			if (BroadcastRandomly == true) {

				var index = Rnd.Next(0, ChatMessages.Count);
				message = ChatMessages[index];

				if (ChatAudio.Count >= ChatMessages.Count) {

					sound = ChatAudio[index];

				}

				if (BroadcastChatType.Count >= ChatMessages.Count) {

					type = BroadcastChatType[index];

				}

				if (ChatAvatar.Count >= ChatMessages.Count) {

					avatar = ChatAvatar[index];

				}

				if (ChatVolumeMultiplier.Count >= ChatMessages.Count) {

					volume = ChatVolumeMultiplier[index];

				}

			} else {

				if (MessageIndex >= ChatMessages.Count) {

					MessageIndex = 0;

				}

				message = ChatMessages[MessageIndex];

				if (ChatAudio.Count >= ChatMessages.Count) {

					sound = ChatAudio[MessageIndex];

				}

				if (BroadcastChatType.Count >= ChatMessages.Count) {

					type = BroadcastChatType[MessageIndex];

				}

				if (ChatAvatar.Count >= ChatMessages.Count) {

					avatar = ChatAvatar[MessageIndex];

				}

				if (ChatVolumeMultiplier.Count >= ChatMessages.Count) {

					volume = ChatVolumeMultiplier[MessageIndex];

				}

				MessageIndex++;

			}


		}

	}
}
