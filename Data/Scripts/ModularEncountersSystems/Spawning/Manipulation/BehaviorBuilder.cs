using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning.Profiles;
using Sandbox.Common.ObjectBuilders;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;

namespace ModularEncountersSystems.Spawning.Manipulation {
	public static class BehaviorBuilder {

		public static bool RivalAiInitialize(MyObjectBuilder_CubeGrid cubeGrid, ManipulationProfile profile, string behaviorName, bool primaryBehaviorAlreadySet, bool rivalAIOverride) {

			SpawnLogger.Write("Attempting To Add RivalAI Block / Behavior", SpawnerDebugEnum.Manipulation);

			MyObjectBuilder_RemoteControl primaryRemote = null;
			MyObjectBuilder_RemoteControl rivalAiRemote = null;

			foreach (var block in cubeGrid.CubeBlocks) {

				var thisRemote = block as MyObjectBuilder_RemoteControl;

				if (thisRemote == null) {

					continue;

				} else {

					if (profile.ConvertAllRemoteControlBlocks && (thisRemote.SubtypeName == "RemoteControlLarge" || thisRemote.SubtypeName == "RemoteControlSmall")) {

						if (cubeGrid.GridSizeEnum == MyCubeSize.Large) {

							thisRemote.SubtypeName = "RivalAIRemoteControlLarge";
							thisRemote.CustomName = "AI Control Module";

						} else {

							thisRemote.SubtypeName = "RivalAIRemoteControlSmall";
							thisRemote.CustomName = "AI Control Module";

						}

					}

					if (!string.IsNullOrWhiteSpace(profile.ApplyBehaviorToNamedBlock)) {

						var termBlock = thisRemote as MyObjectBuilder_TerminalBlock;

						if (termBlock == null)
							continue;

						if (string.IsNullOrWhiteSpace(termBlock.CustomName) || termBlock.CustomName != profile.ApplyBehaviorToNamedBlock)
							continue;

					}

					if (primaryRemote == null) {

						primaryRemote = thisRemote;

					}

					if (thisRemote.IsMainRemoteControl == true) {

						primaryRemote = thisRemote;

					}

				}

			}

			if (primaryRemote != null && rivalAiRemote == null && (profile.RivalAiReplaceRemoteControl || rivalAIOverride)) {

				if (!DefinitionHelper.RivalAiControlModules.Contains(primaryRemote.SubtypeName)) {

					if (cubeGrid.GridSizeEnum == MyCubeSize.Large) {

						primaryRemote.SubtypeName = "RivalAIRemoteControlLarge";

					} else {

						primaryRemote.SubtypeName = "RivalAIRemoteControlSmall";

					}

				}

				rivalAiRemote = primaryRemote;

			}

			if (primaryBehaviorAlreadySet) {

				SpawnLogger.Write("RivalAI Behavior Already Set On This Prefab", SpawnerDebugEnum.Manipulation);
				return false;

			}

			if (rivalAiRemote != null && string.IsNullOrWhiteSpace(behaviorName) == false) {

				string fullBehavior = "";

				if (ProfileManager.BehaviorTemplates.TryGetValue(behaviorName, out fullBehavior) == false) {

					SpawnLogger.Write("RivalAI Profile Does Not Exist For: " + behaviorName, SpawnerDebugEnum.Manipulation);
					return false;

				}

				StorageTools.ApplyCustomBlockStorage(rivalAiRemote, StorageTools.CustomDataKey, fullBehavior);

				SpawnLogger.Write("Applied Following RivalAI Behavior To Prefab: " + behaviorName, SpawnerDebugEnum.Manipulation);
				return true;

			}

			SpawnLogger.Write("Following Behavior Could Not Be Applied To Prefab: " + behaviorName, SpawnerDebugEnum.Manipulation);
			return false;

		}

	}
}
