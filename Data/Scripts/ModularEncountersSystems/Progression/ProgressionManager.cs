using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.Watchers;
using Sandbox.Game;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Progression {
	public static class ProgressionManager {

		public static List<ProgressionContainer> ProgressionContainers = new List<ProgressionContainer>();
		public static List<int> ActiveResearchPointIds = new List<int>();

		public static void Setup() {

			if (!MyAPIGateway.Multiplayer.IsServer)
				return;

			MES_SessionCore.SaveActions += SaveProgression;
			MES_SessionCore.UnloadActions += Unload;
			EventWatcher.ButtonPressed += ResearchPointButtonPress;

			if (string.IsNullOrWhiteSpace(Settings.SavedData?.PlayerProgressionData)) {

				SpawnLogger.Write("No Saved Progression Data For Players", SpawnerDebugEnum.Startup, true);
				return;

			}

			var serialized = Convert.FromBase64String(Settings.SavedData.PlayerProgressionData);

			if (serialized == null) {

				SpawnLogger.Write("Couldn't Convert Progression Data From String To Byte Array", SpawnerDebugEnum.Startup, true);
				return;

			}

			var data = MyAPIGateway.Utilities.SerializeFromBinary<List<ProgressionContainer>>(serialized);

			if (data == null) {

				SpawnLogger.Write("Couldn't Convert Progression Byte Array To Object", SpawnerDebugEnum.Startup, true);
				return;

			}

			ProgressionManager.ProgressionContainers = data;

		}

		public static int GetResearchPoint() {

			int pointId = 0;

			while (true) {

				pointId = MathTools.RandomBetween(0, 9999999);

				if (ActiveResearchPointIds.Contains(pointId))
					continue;

				break;

			}

			ActiveResearchPointIds.Add(pointId);
			return pointId;

		}

		public static void ButtonPanelStartupValidation() {

			List<int> buttonData = new List<int>();

			for (int i = GridManager.Grids.Count - 1; i >= 0; i--) {

				var grid = GridManager.GetSafeGridFromIndex(i);

				if (grid == null)
					continue;

				foreach (var buttonPanel in grid.Buttons) {

					if (!buttonPanel.ActiveEntity() || buttonPanel.Block?.Storage == null)
						continue;

					string data = "";

					if (!buttonPanel.Block.Storage.TryGetValue(StorageTools.MesButtonPointsKey, out data))
						continue;

					int result = 0;

					if (int.TryParse(data, out result))
						buttonData.Add(result);
				
				}
			
			}

			for (int i = ActiveResearchPointIds.Count - 1; i >= 0; i--) {

				var id = ActiveResearchPointIds[i];

				if (buttonData.Contains(id))
					continue;

				ActiveResearchPointIds.RemoveAt(i);

			}
		
		}

		public static void ResearchPointButtonPress(IMyButtonPanel panel, int index, long identityId) {

			if (panel?.Storage == null)
				return;

			string data = "";

			if (!panel.Storage.TryGetValue(StorageTools.MesButtonPointsKey, out data))
				return;

			int result = 0;

			if (int.TryParse(data, out result))
				return;

			if (!ActiveResearchPointIds.Contains(result))
				return;

			var player = PlayerManager.GetPlayerWithIdentityId(identityId);

			if (player == null)
				return;

			player.Progression.Points++;
			ActiveResearchPointIds.Remove(result);
			MyVisualScriptLogicProvider.ShowNotification("Acquired (1) Research Point", 4000, "Green", identityId);
			var block = panel as IMyFunctionalBlock;

			if (block != null)
				block.Enabled = false;

		}

		public static void SaveProgression() {

			var serialized = MyAPIGateway.Utilities.SerializeToBinary<List<ProgressionContainer>>(ProgressionManager.ProgressionContainers);
			Settings.SavedData.UpdateData(Convert.ToBase64String(serialized), ref Settings.SavedData.PlayerProgressionData);

		}

		public static void Unload() {

			MES_SessionCore.SaveActions -= ProgressionManager.SaveProgression;
			EventWatcher.ButtonPressed -= ResearchPointButtonPress;

		}

	}
}
