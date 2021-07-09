using ModularEncountersSystems.Behavior.Subsystems.Trigger;
using System.Collections.Generic;
using VRage.Game;

namespace ModularEncountersSystems.Behavior.Subsystems.Profiles {

	public enum HackingType {

		None,
		ChangeLcdTextures,
		ConnectorThrowing,
		DetachMechanicalBlocks,
		DisableAutomation,
		DisableProduction,
		DisableShields,
		DisableWeapons,
		DisassembleItems,
		FireAllWeapons,
		GyroOverride,
		HudSpam,
		ScrambleLightColors,
		TakeOwnership,
		TerminalScrambler,
		ThrustOverride,

	}
	public class HackerProfile {

		public bool EnableHacking;

		public int PreHackingTime;
		public int HackingCooldownTime;
		public int MaxHackingAttacks;
		public int MaxFailedHackingAttacks;
		public List<HackingType> HackingTypes;

		public bool UseBlockInterference;
		public List<MyDefinitionId> InterferenceBlockIDs;
		public int InterferenceBlockCountRequired;
		public bool InterferenceBlocksReduceSuccess;

		public ChatProfile HackingChat; //Change Chat Profile To Use Modifiers

		public HackerProfile() {

			EnableHacking = false;
			PreHackingTime = 10;
			HackingCooldownTime = 30;
			MaxHackingAttacks = 3;
			MaxFailedHackingAttacks = 3;
			HackingTypes = new List<HackingType>();

			UseBlockInterference = false;
			InterferenceBlockIDs = new List<MyDefinitionId>();
			InterferenceBlockCountRequired = 3;
			InterferenceBlocksReduceSuccess = true;

			HackingChat = new ChatProfile();

		}

	}
}
