using ModularEncountersSystems.API;
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

        // Base game
		public string BotType;
		public string BotBehavior;
		public string BotDisplayName;
        public string BotFactionTag;
		public List<string> Tags;
        public string WeaponsContainerId;
        public string LootContainerId;
        public string ChatMessageSetId;
		public List<string> SpawnBlocks;
        public bool SpawnSeated;
		public Vector3I Color;
        public List<SkinSetEnum> AssetOverrideSets;

        // AiEnabled
		public bool UseAiEnabled;
		public SpawnData Data;
		public byte[] SerializedData;

		public bool CanUseAirNodes;
		public bool CanUseSpaceNodes;
		public bool UseGroundNodesFirst;
		public bool CanUseWaterNodes;
		public bool WaterNodesOnly;
		public bool CanUseLadders;
		public bool CanUseSeats;
		public bool CanDamageGrids;

		public uint DespawnTicks;

		public string DeathSound;
		public List<string> AttackSounds;
		public List<string> PainSounds;
		public List<string> IdleSounds;
		public List<string> TauntSounds;
		public List<string> TargetPriorities;
		public List<string> RepairPriorities;
		public List<string> EmoteActions;

		public float ShotDeviationAngle;
		public bool LeadTargets;
		public string ToolSubtypeId;

		public string LootContainerSubtypeId;

		public BotSpawnProfile() {

			ProfileSubtypeId = "";
			UseAiEnabled = false;

			BotType = "";
			BotBehavior = "Default";
			BotDisplayName = "";
			Tags = new List<string>();
            BotFactionTag = "";
            WeaponsContainerId = "";
            LootContainerId = "";
            ChatMessageSetId = "";
			SpawnBlocks = new List<string>();
            SpawnSeated = true;
			Color = new Vector3I(300, 300, 300);
			AssetOverrideSets = new List<SkinSetEnum>();

			CanUseAirNodes = false;
			CanUseSpaceNodes = false;
			UseGroundNodesFirst = false;
			CanUseWaterNodes = false;
			WaterNodesOnly = false;
			CanUseLadders = false;
			CanUseSeats = false;
			CanDamageGrids = false;

			DespawnTicks = 0;

			DeathSound = null;
			AttackSounds = new List<string>();
			PainSounds = new List<string>();
			IdleSounds = new List<string>();
			TauntSounds = new List<string>();
			TargetPriorities = new List<string>();
			RepairPriorities = new List<string>();
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

				if (tag.Contains("[Tags:")) {

					TagParse.TagStringListCheck(tag, ref Tags);
					continue;

				}

				if (tag.Contains("[WeaponsContainerId:")) {

					TagParse.TagStringCheck(tag, ref WeaponsContainerId);
					continue;

				}

				if (tag.Contains("[LootContainerId:")) {

					TagParse.TagStringCheck(tag, ref LootContainerId);
					continue;

				}

				if (tag.Contains("[ChatMessageSetId:")) {

					TagParse.TagStringCheck(tag, ref ChatMessageSetId);
					continue;

				}

				if (tag.Contains("[BotFactionTag:")) {

					TagParse.TagStringCheck(tag, ref BotFactionTag);
					continue;

				}

				if (tag.Contains("[SpawnBlocks:")) {

					TagParse.TagStringListCheck(tag, ref SpawnBlocks);
					continue;

				}

				if (tag.Contains("[AssetOverrideSets:")) {

					TagParse.TagSkinSetEnumCheck(tag, ref AssetOverrideSets);
					continue;

				}

				if (tag.Contains("[SpawnSeated:")) {

					TagParse.TagBoolCheck(tag, ref SpawnSeated);
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

				if (tag.Contains("[CanDamageGrids:")) {

					TagParse.TagBoolCheck(tag, ref CanDamageGrids);
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

				if (tag.Contains("[IdleSounds:")) {

					TagParse.TagStringListCheck(tag, ref IdleSounds);
					continue;

				}

				if (tag.Contains("[TauntSounds:") || tag.Contains("[Taunts:")) {

					TagParse.TagStringListCheck(tag, ref TauntSounds);
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


				if (tag.Contains("[TargetPriorities:")) {

					TagParse.TagStringListCheck(tag, ref TargetPriorities);
					continue;

				}


        if (tag.Contains("[RepairPriorities:")) {

          TagParse.TagStringListCheck(tag, ref RepairPriorities);
          continue;

        }

      }

			if (TargetPriorities.Count == 0)
				TargetPriorities = GetDefaultTargetPriorities();

			if (RepairPriorities.Count == 0)
				RepairPriorities = GetDefaultRepairPriorities();

      Data = new SpawnData
      {
        BotSubtype = this.BotType,
        BotRole = this.BotBehavior,
        DisplayName = this.BotDisplayName,
        CanUseAirNodes = this.CanUseAirNodes,
        CanUseSpaceNodes = this.CanUseSpaceNodes,
        UseGroundNodesFirst = this.UseGroundNodesFirst,
        CanUseWaterNodes = this.CanUseWaterNodes,
        WaterNodesOnly = this.WaterNodesOnly,
        CanUseLadders = this.CanUseLadders,
        CanUseSeats = this.CanUseSeats,
        CanDamageGrids = this.CanDamageGrids,
        DespawnTicks = this.DespawnTicks,
        DeathSound = this.DeathSound,
        AttackSounds = this.AttackSounds,
        PainSounds = this.PainSounds,
        IdleSounds = this.IdleSounds,
        TauntSounds = this.TauntSounds,
        Actions = this.EmoteActions,
        ShotDeviationAngle = this.ShotDeviationAngle,
        LeadTargets = this.LeadTargets,
        ToolSubtypeId = this.ToolSubtypeId,
        LootContainerSubtypeId = this.LootContainerSubtypeId,
        TargetPriorities = this.TargetPriorities,
        RepairPriorities = this.RepairPriorities
      };

      if (this.Color.X != 300)
      {
        var tempColor = new Color(this.Color.X, this.Color.Y, this.Color.Z);
        Data.Color = tempColor;
      }

      SerializedData = MyAPIGateway.Utilities.SerializeToBinary<SpawnData>(Data);

		}

	}

}
