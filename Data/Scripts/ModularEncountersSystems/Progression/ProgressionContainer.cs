using ProtoBuf;
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

		public string GetUpgradeName(SuitUpgradeTypes type) {

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

		public string GetUpgradeDescriptions(SuitUpgradeTypes type) {

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
				return DrillInhibitorSuitUpgradeLevel;

			return 0;

		}

		public long GetUpgradeCreditCost(SuitUpgradeTypes type) {

			if (type == SuitUpgradeTypes.None)
				return 0;

			if (type == SuitUpgradeTypes.JetpackInhibitor) {

				if (JetpackInhibitorSuitUpgradeLevel == 0)
					return 100000000;

				if (JetpackInhibitorSuitUpgradeLevel == 1)
					return 200000000;

				if (JetpackInhibitorSuitUpgradeLevel == 2)
					return 300000000;

				if (JetpackInhibitorSuitUpgradeLevel == 3)
					return 400000000;

				if (JetpackInhibitorSuitUpgradeLevel == 4)
					return 500000000;

				if (JetpackInhibitorSuitUpgradeLevel > 4)
					return 0;

			}
				

			if (type == SuitUpgradeTypes.HandDrillInhibitor) {

				if (DrillInhibitorSuitUpgradeLevel == 0)
					return 100000000;

				if (DrillInhibitorSuitUpgradeLevel == 1)
					return 200000000;

				if (DrillInhibitorSuitUpgradeLevel == 2)
					return 300000000;

				if (DrillInhibitorSuitUpgradeLevel == 3)
					return 400000000;

				if (DrillInhibitorSuitUpgradeLevel == 4)
					return 500000000;

				if (DrillInhibitorSuitUpgradeLevel > 4)
					return 0;

			}

			if (type == SuitUpgradeTypes.PersonnelInhibitor) {

				if (PersonnelInhibitorSuitUpgradeLevel == 0)
					return 100000000;

				if (PersonnelInhibitorSuitUpgradeLevel == 1)
					return 200000000;

				if (PersonnelInhibitorSuitUpgradeLevel == 2)
					return 300000000;

				if (PersonnelInhibitorSuitUpgradeLevel == 3)
					return 400000000;

				if (PersonnelInhibitorSuitUpgradeLevel == 4)
					return 500000000;

				if (PersonnelInhibitorSuitUpgradeLevel > 4)
					return 0;

			}

			if (type == SuitUpgradeTypes.EnergyInhibitor) {

				if (EnergyInhibitorSuitUpgradeLevel == 0)
					return 100000000;

				if (EnergyInhibitorSuitUpgradeLevel == 1)
					return 200000000;

				if (EnergyInhibitorSuitUpgradeLevel == 2)
					return 300000000;

				if (EnergyInhibitorSuitUpgradeLevel == 3)
					return 400000000;

				if (EnergyInhibitorSuitUpgradeLevel == 4)
					return 500000000;

				if (EnergyInhibitorSuitUpgradeLevel > 4)
					return 0;

			}

			if (type == SuitUpgradeTypes.SolarCharging) {

				if (SolarChargingSuitUpgradeLevel == 0)
					return 100000000;

				if (SolarChargingSuitUpgradeLevel == 1)
					return 200000000;

				if (SolarChargingSuitUpgradeLevel == 2)
					return 300000000;

				if (SolarChargingSuitUpgradeLevel == 3)
					return 400000000;

				if (SolarChargingSuitUpgradeLevel == 4)
					return 500000000;

				if (SolarChargingSuitUpgradeLevel > 4)
					return 0;

			}

			if (type == SuitUpgradeTypes.DamageReduction) {

				if (DamageReductionSuitUpgradeLevel == 0)
					return 100000000;

				if (DamageReductionSuitUpgradeLevel == 1)
					return 200000000;

				if (DamageReductionSuitUpgradeLevel == 2)
					return 300000000;

				if (DamageReductionSuitUpgradeLevel == 3)
					return 400000000;

				if (DamageReductionSuitUpgradeLevel == 4)
					return 500000000;

				if (DamageReductionSuitUpgradeLevel > 4)
					return 0;

			}

			return 0;

		}

		public byte GetUpgradeResearchCost(SuitUpgradeTypes type) {

			if (type == SuitUpgradeTypes.None)
				return 0;

			if (type == SuitUpgradeTypes.JetpackInhibitor) {

				if (JetpackInhibitorSuitUpgradeLevel == 0)
					return 1;

				if (JetpackInhibitorSuitUpgradeLevel == 1)
					return 1;

				if (JetpackInhibitorSuitUpgradeLevel == 2)
					return 2;

				if (JetpackInhibitorSuitUpgradeLevel == 3)
					return 2;

				if (JetpackInhibitorSuitUpgradeLevel == 4)
					return 3;

				if (JetpackInhibitorSuitUpgradeLevel > 4)
					return 0;

			}


			if (type == SuitUpgradeTypes.HandDrillInhibitor) {

				if (DrillInhibitorSuitUpgradeLevel == 0)
					return 1;

				if (DrillInhibitorSuitUpgradeLevel == 1)
					return 1;

				if (DrillInhibitorSuitUpgradeLevel == 2)
					return 2;

				if (DrillInhibitorSuitUpgradeLevel == 3)
					return 2;

				if (DrillInhibitorSuitUpgradeLevel == 4)
					return 3;

				if (DrillInhibitorSuitUpgradeLevel > 4)
					return 0;

			}

			if (type == SuitUpgradeTypes.PersonnelInhibitor) {

				if (PersonnelInhibitorSuitUpgradeLevel == 0)
					return 1;

				if (PersonnelInhibitorSuitUpgradeLevel == 1)
					return 1;

				if (PersonnelInhibitorSuitUpgradeLevel == 2)
					return 2;

				if (PersonnelInhibitorSuitUpgradeLevel == 3)
					return 2;

				if (PersonnelInhibitorSuitUpgradeLevel == 4)
					return 3;

				if (PersonnelInhibitorSuitUpgradeLevel > 4)
					return 0;

			}

			if (type == SuitUpgradeTypes.EnergyInhibitor) {

				if (EnergyInhibitorSuitUpgradeLevel == 0)
					return 1;

				if (EnergyInhibitorSuitUpgradeLevel == 1)
					return 1;

				if (EnergyInhibitorSuitUpgradeLevel == 2)
					return 2;

				if (EnergyInhibitorSuitUpgradeLevel == 3)
					return 2;

				if (EnergyInhibitorSuitUpgradeLevel == 4)
					return 3;

				if (EnergyInhibitorSuitUpgradeLevel > 4)
					return 0;

			}

			if (type == SuitUpgradeTypes.SolarCharging) {

				if (SolarChargingSuitUpgradeLevel == 0)
					return 1;

				if (SolarChargingSuitUpgradeLevel == 1)
					return 1;

				if (SolarChargingSuitUpgradeLevel == 2)
					return 2;

				if (SolarChargingSuitUpgradeLevel == 3)
					return 2;

				if (SolarChargingSuitUpgradeLevel == 4)
					return 3;

				if (SolarChargingSuitUpgradeLevel > 4)
					return 0;

			}

			if (type == SuitUpgradeTypes.DamageReduction) {

				if (DamageReductionSuitUpgradeLevel == 0)
					return 1;

				if (DamageReductionSuitUpgradeLevel == 1)
					return 1;

				if (DamageReductionSuitUpgradeLevel == 2)
					return 2;

				if (DamageReductionSuitUpgradeLevel == 3)
					return 2;

				if (DamageReductionSuitUpgradeLevel == 4)
					return 3;

				if (DamageReductionSuitUpgradeLevel > 4)
					return 0;

			}

			return 0;

		}

	}
}
