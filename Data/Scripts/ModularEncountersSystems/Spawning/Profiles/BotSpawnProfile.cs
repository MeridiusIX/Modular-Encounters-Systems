using ModularEncountersSystems.Helpers;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;
using static ModularEncountersSystems.API.RemoteBotAPI;

namespace ModularEncountersSystems.Spawning.Profiles {
	public class BotSpawnProfile {

		public string ProfileSubtypeId;
		public bool UseAiEnabled;
		public SpawnData Data;
		public byte[] SerializedData;

		public string BotType;
		public string BotBehavior;
		public string BotDisplayName;
		public Vector3I Color;

		public bool CanUseAirNodes;
		public bool CanUseSpaceNodes;
		public bool UseGroundNodesFirst;
		public bool CanUseWaterNodes;
		public bool WaterNodesOnly;
		public bool CanUseLadders;
		public bool CanUseSeats;

		public uint DespawnTicks;

		public string DeathSound;
		public List<string> AttackSounds;
		public List<string> PainSounds;
		public List<string> IdleSounds;

		public List<string> EmoteActions;

		public float ShotDeviationAngle;
		public bool LeadTargets;
		public string ToolSubtypeId;

		public string LootContainerSubtypeId;

		public BotSpawnProfile() {

			ProfileSubtypeId = "";
			UseAiEnabled = true;
			Data = new SpawnData();

			BotType = "";
			BotBehavior = "Default";
			BotDisplayName = "";
			Color = new Vector3I(300, 300, 300);

			CanUseAirNodes = false;
			CanUseSpaceNodes = false;
			UseGroundNodesFirst = false;
			CanUseWaterNodes = false;
			WaterNodesOnly = false;
			CanUseLadders = false;
			CanUseSeats = false;

			DespawnTicks = 0;

			DeathSound = null;
			AttackSounds = new List<string>();
			PainSounds = new List<string>();
			IdleSounds = new List<string>();

			EmoteActions = new List<string>();

			ShotDeviationAngle = 1.5f;
			LeadTargets = false;
			ToolSubtypeId = null;

			LootContainerSubtypeId = "";

		}

		public void InitTags(string data) {
		
			if (string.IsNullOrWhiteSpace(data))
				return;

			var descSplit = data.Split('\n');

			foreach (var tag in descSplit) {

				if (tag.Contains("[BotType:")) {

					TagParse.TagStringCheck(tag, ref BotType);
					continue;

				}

				if (tag.Contains("[UseAiEnabled:")) {

					TagParse.TagBoolCheck(tag, ref UseAiEnabled);
					continue;

				}

				if (tag.Contains("[BotBehavior:")) {

					TagParse.TagStringCheck(tag, ref BotBehavior);
					continue;

				}

				if (tag.Contains("[BotDisplayName:")) {

					TagParse.TagStringCheck(tag, ref BotDisplayName);
					continue;

				}

				if (tag.Contains("[Color:")) {

					TagParse.TagVector3ICheck(tag, ref Color);
					continue;

				}

				if (tag.Contains("[CanUseAirNodes:")) {

					TagParse.TagBoolCheck(tag, ref CanUseAirNodes);
					continue;

				}

				if (tag.Contains("[CanUseSpaceNodes:")) {

					TagParse.TagBoolCheck(tag, ref CanUseSpaceNodes);
					continue;

				}

				if (tag.Contains("[UseGroundNodesFirst:")) {

					TagParse.TagBoolCheck(tag, ref UseGroundNodesFirst);
					continue;

				}

				if (tag.Contains("[CanUseWaterNodes:")) {

					TagParse.TagBoolCheck(tag, ref CanUseWaterNodes);
					continue;

				}

				if (tag.Contains("[WaterNodesOnly:")) {

					TagParse.TagBoolCheck(tag, ref WaterNodesOnly);
					continue;

				}

				if (tag.Contains("[CanUseLadders:")) {

					TagParse.TagBoolCheck(tag, ref CanUseLadders);
					continue;

				}

				if (tag.Contains("[CanUseSeats:")) {

					TagParse.TagBoolCheck(tag, ref CanUseSeats);
					continue;

				}

				if (tag.Contains("[DespawnTicks:")) {

					TagParse.TagUintCheck(tag, ref DespawnTicks);
					continue;

				}

				if (tag.Contains("[DeathSound:")) {

					TagParse.TagStringCheck(tag, ref DeathSound);
					continue;

				}

				if (tag.Contains("[AttackSounds:")) {

					TagParse.TagStringListCheck(tag, ref AttackSounds);
					continue;

				}

				if (tag.Contains("[PainSounds:")) {

					TagParse.TagStringListCheck(tag, ref PainSounds);
					continue;

				}

				if (tag.Contains("[IdleSounds:") || tag.Contains("[Taunts:")) {

					TagParse.TagStringListCheck(tag, ref IdleSounds);
					continue;

				}

				if (tag.Contains("[EmoteActions:")) {

					TagParse.TagStringListCheck(tag, ref EmoteActions);
					continue;

				}

				if (tag.Contains("[ShotDeviationAngle:")) {

					TagParse.TagFloatCheck(tag, ref ShotDeviationAngle);
					continue;

				}

				if (tag.Contains("[LeadTargets:")) {

					TagParse.TagBoolCheck(tag, ref LeadTargets);
					continue;

				}

				if (tag.Contains("[ToolSubtypeId:")) {

					TagParse.TagStringCheck(tag, ref ToolSubtypeId);
					continue;

				}

				
				if (tag.Contains("[LootContainerSubtypeId:")) {

					TagParse.TagStringCheck(tag, ref LootContainerSubtypeId);
					continue;

				}

			}

			Data.BotSubtype = this.BotType;
			Data.BotRole = this.BotBehavior;
			Data.DisplayName = this.BotDisplayName;
			var tempColor = new Color(this.Color.X, this.Color.Y, this.Color.Z);
			if (this.Color.X != 300)
				Data.Color = tempColor;

			Data.CanUseAirNodes = this.CanUseAirNodes;
			Data.CanUseSpaceNodes = this.CanUseSpaceNodes;
			Data.UseGroundNodesFirst = this.UseGroundNodesFirst;
			Data.CanUseWaterNodes = this.CanUseWaterNodes;
			Data.WaterNodesOnly = this.WaterNodesOnly;
			Data.CanUseLadders = this.CanUseLadders;
			Data.CanUseSeats = this.CanUseSeats;
			Data.DespawnTicks = this.DespawnTicks;
			Data.DeathSound = this.DeathSound;
			Data.AttackSounds = this.AttackSounds;
			Data.PainSounds = this.PainSounds;
			Data.IdleSounds = this.IdleSounds;
			Data.Actions = this.EmoteActions;
			Data.ShotDeviationAngle = this.ShotDeviationAngle;
			Data.LeadTargets = this.LeadTargets;
			Data.ToolSubtypeId = this.ToolSubtypeId;
			Data.LootContainerSubtypeId = this.LootContainerSubtypeId;
			SerializedData = MyAPIGateway.Utilities.SerializeToBinary<SpawnData>(Data);

		}

	}

}
