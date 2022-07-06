using ModularEncountersSystems.Helpers;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;

namespace ModularEncountersSystems.Behavior.Subsystems {

	public enum BehaviorHackingEnumA {

		LightShow,
		Lcds,
		ProductionStop,
		AutomationFreeze,
		ItemDisassembly

	}

	public enum BehaviorHackingEnumB {

		ShieldDrop,
		GyroSpin,
		ThrustOverride,
		WeaponDisarm,
		TurretConfusion

	}

	public enum BehaviorHackingEnumC {

		HudSpam,
		TerminalCorruption,
		JoyRide,
		BlockSubjugation,
		ClangAttacks,
		ConnectorVomit

	}

	public enum BehaviorHackingResult {

		UnknownFail,
		TargetNotFound,
		MyAntennaNotFound,
		MyAntennaNotWorking,
		TargetOutOfMyAntennaRange,
		TargetNotBroadcasting,
		TargetBroadcastNotInRange,
		DecoyInterference,
		Success

	}

	public class HackingSystem {

		public static BehaviorHackingResult AttemptHacking(IMyCubeGrid targetGrid, string hackingType, IMyRadioAntenna myAntenna, int interferenceLimit = 3) {

			if(targetGrid == null || MyAPIGateway.Entities.Exist(targetGrid) == false) {

				return BehaviorHackingResult.TargetNotFound;

			}

			if(myAntenna == null || MyAPIGateway.Entities.Exist(myAntenna?.SlimBlock?.CubeGrid) == false) {

				return BehaviorHackingResult.MyAntennaNotFound;

			}

			if(myAntenna.IsFunctional == false || myAntenna.IsWorking == false || myAntenna.EnableBroadcasting == false) {

				return BehaviorHackingResult.MyAntennaNotWorking;

			}



			var blockList = BlockCollectionHelper.GetBlocksOfType<IMyTerminalBlock>(targetGrid);

			return BehaviorHackingResult.UnknownFail;

		}

	}

}