using ModularEncountersSystems.Configuration;
using ProtoBuf;
using Sandbox.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Progression {

	public enum SuitUpgradeTypes {
	
		None,
		JetpackInhibitor,
		HandDrillInhibitor,
		PersonnelInhibitor,
		EnergyInhibitor,
		SolarCharging,
		DamageReduction,
		PassiveIncome,
	
	}

	[ProtoContract]
	public class ProgressionContainer {

		//Identity Info
		[ProtoMember(1)] public long IdentityId;
		[ProtoMember(2)] public ulong SteamId;

		//Points
		[ProtoMember(3)] public byte Points;

		//Suit Upgrades
		[ProtoMember(4)] public byte JetpackInhibitorSuitUpgradeLevel;
		[ProtoMember(5)] public byte DrillInhibitorSuitUpgradeLevel;
		[ProtoMember(6)] public byte PersonnelInhibitorSuitUpgradeLevel;
		[ProtoMember(7)] public byte EnergyInhibitorSuitUpgradeLevel;
		[ProtoMember(8)] public byte SolarChargingSuitUpgradeLevel;
		[ProtoMember(9)] public byte DamageReductionSuitUpgradeLevel;


		//Research

		public ProgressionContainer() {

			IdentityId = 0;
			SteamId = 0;

			Points = 0;

			JetpackInhibitorSuitUpgradeLevel = 0;
			DrillInhibitorSuitUpgradeLevel = 0;
			PersonnelInhibitorSuitUpgradeLevel = 0;
			EnergyInhibitorSuitUpgradeLevel = 0;
			SolarChargingSuitUpgradeLevel = 0;
			DamageReductionSuitUpgradeLevel = 0;


		}

		public bool CompareValues(ProgressionContainer otherContainer) {

			if (this.IdentityId != otherContainer.IdentityId)
				return false;

			if (this.SteamId != otherContainer.SteamId)
				return false;

			if (this.Points != otherContainer.Points)
				return false;

			if (this.JetpackInhibitorSuitUpgradeLevel != otherContainer.JetpackInhibitorSuitUpgradeLevel)
				return false;

			if (this.DrillInhibitorSuitUpgradeLevel != otherContainer.DrillInhibitorSuitUpgradeLevel)
				return false;

			if (this.PersonnelInhibitorSuitUpgradeLevel != otherContainer.PersonnelInhibitorSuitUpgradeLevel)
				return false;

			if (this.EnergyInhibitorSuitUpgradeLevel != otherContainer.EnergyInhibitorSuitUpgradeLevel)
				return false;

			if (this.SolarChargingSuitUpgradeLevel != otherContainer.SolarChargingSuitUpgradeLevel)
				return false;

			if (this.DamageReductionSuitUpgradeLevel != otherContainer.DamageReductionSuitUpgradeLevel)
				return false;

			return true;

		}

		public void ApplyUpgrade(SuitUpgradeTypes type) {

			if (type == SuitUpgradeTypes.None) {

				//MyVisualScriptLogicProvider.ShowNotificationToAll("Fails with None", 4000);
				return;

			}
				
			if (type == SuitUpgradeTypes.JetpackInhibitor)
				JetpackInhibitorSuitUpgradeLevel++;

			if (type == SuitUpgradeTypes.HandDrillInhibitor)
				DrillInhibitorSuitUpgradeLevel++;

			if (type == SuitUpgradeTypes.PersonnelInhibitor)
				PersonnelInhibitorSuitUpgradeLevel++;

			if (type == SuitUpgradeTypes.EnergyInhibitor)
				EnergyInhibitorSuitUpgradeLevel++;

			if (type == SuitUpgradeTypes.SolarCharging)
				SolarChargingSuitUpgradeLevel++;

			if (type == SuitUpgradeTypes.DamageReduction) 
				DamageReductionSuitUpgradeLevel++;




		}

		public static string GetUpgradeName(SuitUpgradeTypes type) {

			if (type == SuitUpgradeTypes.None)
				return "None";

			if (type == SuitUpgradeTypes.JetpackInhibitor)
				return "Anti Jetpack Inhibitor";

			if (type == SuitUpgradeTypes.HandDrillInhibitor)
				return "Anti Hand Drill Inhibitor";

			if (type == SuitUpgradeTypes.PersonnelInhibitor)
				return "Anti Personnel Inhibitor";

			if (type == SuitUpgradeTypes.EnergyInhibitor)
				return "Anti Energy Inhibitor";

			if (type == SuitUpgradeTypes.SolarCharging)
				return "Solar Charging";

			if (type == SuitUpgradeTypes.DamageReduction)
				return "Damage Reduction";

			return "";

		}

		public static string GetUpgradeDescriptions(SuitUpgradeTypes type) {

			if (type == SuitUpgradeTypes.None)
				return "No upgrade selected";

			if (type == SuitUpgradeTypes.JetpackInhibitor)
				return "Protects against jetpack inhibitor effect in exchange for suit energy. Higher levels of this upgrade use less energy.";

			if (type == SuitUpgradeTypes.HandDrillInhibitor)
				return "Protects against hand drill inhibitor effect in exchange for suit energy. Higher levels of this upgrade use less energy.";

			if (type == SuitUpgradeTypes.PersonnelInhibitor)
				return "Protects against personnel inhibitor effect in exchange for suit energy. Higher levels of this upgrade use less energy.";

			if (type == SuitUpgradeTypes.EnergyInhibitor)
				return "Protects against energy inhibitor effect in exchange for suit energy. Higher levels of this upgrade use less energy.";

			if (type == SuitUpgradeTypes.SolarCharging)
				return "Allows player to regain suit energy while in direct sunlight. Higher levels of this upgrade increase accumulated energy.";

			if (type == SuitUpgradeTypes.DamageReduction)
				return "Reduces damage taken from all sources. Higher levels of this upgrade reduce damage even further.";

			return "";

		}

		public byte GetUpgradeLevel(SuitUpgradeTypes type) {

			if (type == SuitUpgradeTypes.None)
				return 0;

			if (type == SuitUpgradeTypes.JetpackInhibitor)
				return JetpackInhibitorSuitUpgradeLevel;

			if (type == SuitUpgradeTypes.HandDrillInhibitor)
				return DrillInhibitorSuitUpgradeLevel;

			if (type == SuitUpgradeTypes.PersonnelInhibitor)
				return PersonnelInhibitorSuitUpgradeLevel;

			if (type == SuitUpgradeTypes.EnergyInhibitor)
				return EnergyInhibitorSuitUpgradeLevel;

			if (type == SuitUpgradeTypes.SolarCharging)
				return SolarChargingSuitUpgradeLevel;

			if (type == SuitUpgradeTypes.DamageReduction)
				return DamageReductionSuitUpgradeLevel;

			return 0;

		}

		public long GetUpgradeCreditCost(SuitUpgradeTypes type) {

			if (type == SuitUpgradeTypes.None)
				return 0;

			long nextLevel = GetUpgradeLevel(type) + 1;

			if (nextLevel > 5)
				return 0;

			return 100000000 * nextLevel;

		}

		public byte GetUpgradeResearchCost(SuitUpgradeTypes type) {

			if (type == SuitUpgradeTypes.None)
				return 0;

			byte nextLevel = GetUpgradeLevel(type);
			nextLevel++;

			if (nextLevel > 5)
				return 0;

			return nextLevel;

		}

		public float GetSolarEnergyIncrease() {

			if (SolarChargingSuitUpgradeLevel == 0)
				return 0;

			if (SolarChargingSuitUpgradeLevel == 1)
				return 0.0015f;

			if (SolarChargingSuitUpgradeLevel == 2)
				return 0.0035f;

			if (SolarChargingSuitUpgradeLevel == 3)
				return 0.0055f;

			if (SolarChargingSuitUpgradeLevel == 4)
				return 0.075f;

			if (SolarChargingSuitUpgradeLevel >= 5)
				return 0.01f;

			return 0;

		}

		public static bool IsUpgradeAllowedInConfig(SuitUpgradeTypes upgrade) {

			if (upgrade == SuitUpgradeTypes.None)
				return false;

			if (upgrade == SuitUpgradeTypes.JetpackInhibitor && !Settings.Progression.AllowAntiJetpackInhibitorSuitUpgrade)
				return false;

			if (upgrade == SuitUpgradeTypes.HandDrillInhibitor && !Settings.Progression.AllowAntiHandDrillInhibitorSuitUpgrade)
				return false;

			if (upgrade == SuitUpgradeTypes.PersonnelInhibitor && !Settings.Progression.AllowAntiPersonnelInhibitorSuitUpgrade)
				return false;

			if (upgrade == SuitUpgradeTypes.EnergyInhibitor && !Settings.Progression.AllowAntiEnergyInhibitorSuitUpgrade)
				return false;

			if (upgrade == SuitUpgradeTypes.SolarCharging && !Settings.Progression.AllowSolarChargingSuitUpgrade)
				return false;

			if (upgrade == SuitUpgradeTypes.DamageReduction && !Settings.Progression.AllowDamageReductionSuitUpgrade)
				return false;

			return true;

		}

	}
}
