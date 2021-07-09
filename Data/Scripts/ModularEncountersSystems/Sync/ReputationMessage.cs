using ProtoBuf;
using ModularEncountersSystems.Helpers;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;

namespace ModularEncountersSystems.Sync {

	[ProtoContract]
	public class ReputationMessage {

		[ProtoMember(1)]
		public int ReputationChangeAmount;

		[ProtoMember(2)]
		public string ReputationFactionTarget;

		[ProtoMember(3)]
		public ulong SteamId;

		[ProtoIgnore]
		public int CurrentValue;

		[ProtoIgnore]
		public bool Increase;

		[ProtoIgnore]
		public string Color;

		[ProtoIgnore]
		public IMyHudNotification Notification;

		[ProtoIgnore]
		public DateTime LastAliveTime;

		public ReputationMessage() {

			ReputationChangeAmount = 0;
			ReputationFactionTarget = "";
			CurrentValue = 0;
			Increase = false;
			Color = "Red";
			Notification = MyAPIGateway.Utilities.CreateNotification("", 2000, Color);
			LastAliveTime = MyAPIGateway.Session.GameDateTime;

		}

		public ReputationMessage(int amount, string faction, ulong steamId) : base() {

			ReputationChangeAmount = amount;
			ReputationFactionTarget = faction;
			SteamId = steamId;

		}

		public void DisplayMessage(int amount) {

			var time = MyAPIGateway.Session.GameDateTime - LastAliveTime;

			if (time.TotalSeconds > Notification.AliveTime) {

				CurrentValue = 0;

			}

			CurrentValue += amount;
			Notification.Hide();
			Notification.Font = CurrentValue < 0 ? "Red" : "Green";
			Notification.Text = string.Format("Reputation With {0} Changed By: {1}", ReputationFactionTarget, CurrentValue);
			Notification.ResetAliveTime();
			Notification.Show();
			LastAliveTime = MyAPIGateway.Session.GameDateTime;
			//Logger.Write(string.Format("Rep Message Displayed. Current Value {0} - ReceivedAmount: {1}", CurrentValue, amount), BehaviorDebugEnum.Owner);

		}

	}

	/*
	[ProtoContract]
	public class ReputationMessageContainer {

		[ProtoMember(1)]
		public List<ReputationMessage> Messages;

		public ReputationMessageContainer() {

			Messages = new List<ReputationMessage>();

		}

	}
	*/
}
