using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Sync {
	public static class ReputationAnnounceManager {

		public static List<ReputationMessage> MessageList = new List<ReputationMessage>();
		private static Dictionary<string, ReputationMessage> _messages = new Dictionary<string, ReputationMessage>();

		public static void ProcessMessage(ReputationMessage receivedMessage) {

			//Logger.Write("Process Rep Sync Msg", BehaviorDebugEnum.Owner);
			ReputationMessage existingMsg = null;

			if (_messages.TryGetValue(receivedMessage.ReputationFactionTarget, out existingMsg)) {

				//Logger.Write("Process Existing Rep Sync Msg", BehaviorDebugEnum.Owner);
				existingMsg.DisplayMessage(receivedMessage.ReputationChangeAmount);
				return;

			}

			//Logger.Write("Process New Rep Sync Msg", BehaviorDebugEnum.Owner);
			receivedMessage.DisplayMessage(receivedMessage.ReputationChangeAmount);
			_messages.Add(receivedMessage.ReputationFactionTarget, receivedMessage);

		}

	}

}
