using ModularEncountersSystems.Behavior.Subsystems;
using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Behavior.Subsystems.Trigger;
using ModularEncountersSystems.BlockLogic;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Behavior {
	public interface IBehavior {

		IBehaviorSubClass ActiveBehavior { get; }
		AutoPilotSystem AutoPilot { get; }
		BroadcastSystem Broadcast { get; }
		DamageSystem Damage { get; }
		DespawnSystem Despawn { get; }
		DiagnosticSystem Diagnostic { get; }
		EscortSystem Escort { get; }
		GridSystem Grid { get; }
		HeartbeatSystem Heartbeat { get; }
		OwnerSystem Owner { get; }
		StoredSettings BehaviorSettings { get; }
		TriggerSystem Trigger { get; }
		BehaviorMode Mode { get; }
		bool BehaviorTerminated { get; set; }
		bool BehaviorTriggerA { get; set; }
		bool BehaviorTriggerB { get; set; }
		bool BehaviorTriggerC { get; set; }
		bool BehaviorTriggerD { get; set; }
		bool BehaviorTriggerE { get; set; }
		bool BehaviorTriggerF { get; set; }
		bool BehaviorTriggerG { get; set; }
		bool BehaviorTriggerH { get; set; }
		bool BehaviorActionA { get; set; }
		bool BehaviorActionB { get; set; }
		bool BehaviorActionC { get; set; }
		bool BehaviorActionD { get; set; }
		bool BehaviorActionE { get; set; }
		bool BehaviorActionF { get; set; }
		bool BehaviorActionG { get; set; }
		bool BehaviorActionH { get; set; }

		JetpackInhibitor JetpackInhibitorLogic { get; set; }
		DrillInhibitor DrillInhibitorLogic { get; set; }
		NanobotInhibitor NanobotInhibitorLogic { get; set; }
		JumpDriveInhibitor JumpInhibitorLogic { get; set; }
		PlayerInhibitor PlayerInhibitorLogic { get; set; }

		void BehaviorInit(IMyRemoteControl remoteControl);
		//List<IMyCubeGrid> CurrentGrids { get; }
		GridEntity CurrentGrid { get; }
		BlockEntity RemoteControlBlockEntity { get; }
		List<IMyCockpit> DebugCockpits { get; }
		long GridId { get; }
		string GridName { get; }
		List<Vector3D> EscortOffsets { get; }
		IMyRemoteControl RemoteControl { get; set; }
		void ChangeCoreBehaviorMode(BehaviorMode behaviorMode);
		bool IsAIReady();
		void ProcessCollisionChecks();
		void ProcessTargetingChecks();
		void ProcessAutoPilotChecks();
		void ProcessWeaponChecks();
		void ProcessTriggerChecks();
		void EngageAutoPilot();
		void SetDebugCockpit(IMyCockpit block, bool addMode);
		void SetInitialWeaponReadiness();
		void FireWeapons();
		void FireBarrageWeapons();
		void ProcessActivatedTriggers();
		void CheckDespawnConditions();
		void RunMainBehavior();
		bool IsClosed();
		void DebugDrawWaypoints();
		void ChangeTargetProfile(string newTargetProfile);
		void ChangeBehavior(string newBehavior, bool preserveSettings, bool preserveTriggers, bool preserveTargetData);
		void AssignSubClassBehavior(BehaviorSubclass subclass);

	}
}
