using ProtoBuf;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Terminal {

	public enum SuitModificationTypes {

		None,
		JetpackInhibitor,
		DrillInhibitor,
		PersonnelInhibitor,
		EnergyInhibitor,
		Solar,
		DamageReduction,

	}



	public static class SuitModificationControls {

		internal static SuitMods SuitModsAvailable = new SuitMods();
		internal static Dictionary<IMyTerminalBlock, SuitModificationTypes> SelectedSuitMod = new Dictionary<IMyTerminalBlock, SuitModificationTypes>();

		internal static IMyTerminalControlLabel _labelControls;
		internal static IMyTerminalControlButton _infoButton;
		internal static IMyTerminalControlSeparator _separatorA;

		internal static IMyTerminalControlLabel _labelPurchaseMod;
		internal static IMyTerminalControlListbox _purchasableMods;
		internal static IMyTerminalControlSeparator _separatorB;
		internal static IMyTerminalControlButton _confirmPurchase;

	}

	[ProtoContract]
	public class SuitMods {

		[ProtoMember(1)] public long IdentityId;
		[ProtoMember(2)] public ulong SteamId;

		[ProtoMember(3)] public byte JetpackInhibitor;
		[ProtoMember(4)] public byte DrillInhibitor;
		[ProtoMember(5)] public byte PersonnelInhibitor;
		[ProtoMember(6)] public byte EnergyInhibitor;
		[ProtoMember(7)] public byte Solar;
		[ProtoMember(8)] public byte DamageReduction;

		public SuitMods() {

			IdentityId = 0;
			SteamId = 0;
			JetpackInhibitor = 0;
			DrillInhibitor = 0;
			PersonnelInhibitor = 0;
			EnergyInhibitor = 0;
			Solar = 0;
			DamageReduction = 0;

		}

	}

}
