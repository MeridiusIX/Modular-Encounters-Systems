using ModularEncountersSystems.Logging;

using ProtoBuf;

using Sandbox.ModAPI;

using System;

using VRage.Game.ModAPI;

namespace ModularEncountersSystems.Sync
{
	[ProtoContract]
    public class BalanceChangeMessage
    {
		[ProtoMember(1)]
        public int ChangeAmount;

		[ProtoMember(2)]
		public string BalanceChangeFactionTarget;

		[ProtoMember(3)]
		public ulong SteamId;

		[ProtoIgnore]
		public string Color;

		[ProtoIgnore]
		public IMyHudNotification Notification;

		[ProtoIgnore]
		public DateTime LastAliveTime;

		public BalanceChangeMessage(){ 

			ChangeAmount = 0;
			BalanceChangeFactionTarget = string.Empty;
			Color = "Green";
			Notification = MyAPIGateway.Utilities.CreateNotification("", 2000, Color);
			LastAliveTime = MyAPIGateway.Session.GameDateTime;

		}

		public BalanceChangeMessage(ulong steamId, string balanceChangeFactionTarget, int changeAmount): base(){ 

			ChangeAmount = changeAmount;
			BalanceChangeFactionTarget = balanceChangeFactionTarget;
			SteamId = steamId;

		}
		
		public void DisplayMessage() {

			var time = MyAPIGateway.Session.GameDateTime - LastAliveTime;

			Notification.Hide();
			Notification.Font = ChangeAmount < 0 ? "Red" : "Green";
			Notification.Text = string.Format("Faction Balance Modified By: {0}", ChangeAmount);
			Notification.ResetAliveTime();
			Notification.Show();
			LastAliveTime = MyAPIGateway.Session.GameDateTime;

		}

    }

}