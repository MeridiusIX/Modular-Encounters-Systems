using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Files;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Missions;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Definitions;

namespace ModularEncountersSystems.Spawning.Profiles {

	public class ContractBlockProfile {

		public string ProfileSubtypeId;
		public bool SetupComplete;


		public int MinContracts;
		public int MaxContracts;

		public string StoreProfileId;

		public List<string> MissionIds;


		public Dictionary<string, Action<string, object>> EditorReference;

		public ContractBlockProfile() {

			ProfileSubtypeId = "";
			SetupComplete = false;


			StoreProfileId = "MES-StoreProfile-Example";
			MinContracts = 10;
			MaxContracts = 10;


			MissionIds = new List<string>();


			EditorReference = new Dictionary<string, Action<string, object>> {

				{"MinContracts", (s, o) => TagParse.TagIntCheck(s, ref MinContracts) },
				{"MaxContracts", (s, o) => TagParse.TagIntCheck(s, ref MaxContracts) },
				{"StoreProfileId", (s, o) => TagParse.TagStringCheck(s, ref StoreProfileId) },

				{"MissionIds", (s, o) => TagParse.TagStringListCheck(s, ref MissionIds) },



			};

		}


		public void InitTags(string customData) {

			if (string.IsNullOrWhiteSpace(customData) == false) {

				var descSplit = customData.Split('\n');

				foreach (var tag in descSplit) {

					EditValue(tag);

				}

			}

		}

		public void EditValue(string receivedValue) {

			var processedTag = TagParse.ProcessTag(receivedValue);

			if (processedTag.Length < 2)
				return;

			Action<string, object> referenceMethod = null;

			if (!EditorReference.TryGetValue(processedTag[0], out referenceMethod))
				//TODO: Notes About Value Not Found
				return;

			referenceMethod?.Invoke(receivedValue, null);

		}

		public void ApplyProfileToBlock(BlockEntity block,string spawnGroupName, bool clearExisting = true) {

			if (block == null) {

				BehaviorLogger.Write(" - Contract Block Null", BehaviorDebugEnum.Action);
				return;

			}


            if (clearExisting)
            {
				InGameContractManager.ClearBlockContracts(block.Entity.EntityId);

			}


			for (int i = 0; i < MissionIds.Count; i++)
			{
				MissionProfile missionprofile = null;

				if (!ProfileManager.MissionProfiles.TryGetValue(MissionIds[i], out missionprofile))
				{

					BehaviorLogger.Write(ProfileSubtypeId + ": Couldn't find Mission Profile With Name: " + MissionIds[i], BehaviorDebugEnum.Action);
					continue;

				}


				var mission = new Mission(MissionIds[i], this.StoreProfileId, spawnGroupName);
				if (!mission.Init(block))
                {
					MyVisualScriptLogicProvider.ShowNotificationToAll($"Failed  {MissionIds[i]}", 5000);
				}

			}



		}


	}

}
