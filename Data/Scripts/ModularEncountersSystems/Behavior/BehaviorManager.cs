using ModularEncountersSystems.Core;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Tasks;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Behavior {

	public enum BehaviorManagerMode {

		None,
		Parallel,
		ParallelWorking,
		MainThread

	}

	public enum BehaviorManageSubmode {

		None,
		Collision,
		Targeting,
		AutoPilot,
		Weapons,
		Triggers,
		Behavior

	}

	public static class BehaviorManager {

		public static List<IBehavior> Behaviors = new List<IBehavior>();
		public static List<IBehavior> TerminatedBehaviors = new List<IBehavior>();
		public static List<IMyRemoteControl> DormantAiBlocks = new List<IMyRemoteControl>();
		public static List<long> RegisteredRemoteBlocks = new List<long>();

		public static bool AiNeedsReset = false;
		public static List<IMyRemoteControl> ResetAiBlocks = new List<IMyRemoteControl>();

		public static BehaviorManagerMode Mode = BehaviorManagerMode.None;
		public static BehaviorManageSubmode Submode = BehaviorManageSubmode.None;
		public static int CurrentBehaviorIndex = 0;

		public static bool DebugDraw = false;

		private static byte _barrageCounter = 0;
		private static byte _behaviorCounter = 0;

		public static void Setup() {

			TaskProcessor.Tick1.Tasks += ProcessBehaviors;

			//MES_SessionCore.UnloadActions += Unload;
		
		}

		public static void ProcessBehaviors() {

			if (DebugDraw) {

				for (int i = Behaviors.Count - 1; i >= 0; i--) {

					if (Behaviors[i] == null)
						continue;

					if (!Behaviors[i].IsClosed() && Behaviors[i].IsAIReady()) {

						Behaviors[i].DebugDrawWaypoints();

					}

				}

			}

			if (AiNeedsReset) {

				AiNeedsReset = false;

				if (ResetAiBlocks.Count > 0) {

					for (int j = ResetAiBlocks.Count - 1; j >= 0; j--) {

						var behavior = GetBehavior(ResetAiBlocks[j]);

						if (behavior == null)
							continue;

						behavior.BehaviorTerminated = true;

						if (ResetAiBlocks[j].Storage != null && ResetAiBlocks[j].Storage.ContainsKey(new Guid("FF814A67-AEC3-4DF0-ADC4-A9B239FA954F"))) {

							ResetAiBlocks[j].Storage[new Guid("FF814A67-AEC3-4DF0-ADC4-A9B239FA954F")] = "";

						}

						BehaviorLogger.Write("AI ModStorageComponent Wiped", BehaviorDebugEnum.BehaviorSetup);

					}

					MyAPIGateway.Parallel.Start(() => {

						for (int i = ResetAiBlocks.Count - 1; i >= 0; i--) {

							BehaviorLogger.Write("Re-Registering AI", BehaviorDebugEnum.BehaviorSetup);
							RegisterBehaviorFromRemoteControl(ResetAiBlocks[i]);

						}

						ResetAiBlocks.Clear();

					});

				}

			}

			_barrageCounter++;

			if (Mode != BehaviorManagerMode.None) {

				try {

					ProcessParallelMethods();
					ProcessMainThreadMethods();

				} catch (Exception e) {

					BehaviorLogger.Write("Exception in Main Behavior Processing", BehaviorDebugEnum.Error, true);
					BehaviorLogger.Write(e.ToString(), BehaviorDebugEnum.Error, true);

				}

			} else {

				_behaviorCounter++;

			}

			if (_barrageCounter % 10 == 0) {

				ProcessWeaponsBarrage();
				_barrageCounter = 0;

			}

			if (_behaviorCounter == 15) {

				for (int i = Behaviors.Count - 1; i >= 0; i--) {

					if (Behaviors[i] == null || Behaviors[i].IsClosed() || Behaviors[i].BehaviorTerminated) {

						TerminatedBehaviors.Add(Behaviors[i]);
						Behaviors.RemoveAt(i);
						continue;

					}

				}

				//BehaviorLogger.Write("Start Parallel For All Behaviors", BehaviorDebugEnum.General);
				Mode = BehaviorManagerMode.Parallel;
				_behaviorCounter = 0;

			}

		}

		public static IBehavior GetBehavior(long remoteControlId) {

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				var behavior = Behaviors[i];

				if (behavior?.RemoteControl == null || remoteControlId != behavior?.RemoteControl.EntityId || !behavior.IsAIReady())
					continue;

				return behavior;
			
			}

			return null;
		
		}

		public static void RegisterBehaviorFromRemoteControl(IMyRemoteControl remoteControl) {

			try {

				lock (RegisteredRemoteBlocks) {

					if (RegisteredRemoteBlocks.Contains(remoteControl.EntityId))
						return;
					else
						RegisteredRemoteBlocks.Add(remoteControl.EntityId);

				}
					

				BehaviorLogger.Write("Determining Behavior Type of RemoteControl", BehaviorDebugEnum.BehaviorSetup);

				BehaviorSubclass subclass = GetSubclassFromCustomData(remoteControl.CustomData);

				if (subclass != BehaviorSubclass.None) {

					BehaviorLogger.Write("Behavior: " + subclass, BehaviorDebugEnum.BehaviorSetup);
					var MainBehavior = new CoreBehavior();
					MainBehavior.CoreBehaviorSetup(remoteControl, subclass);

					lock (Behaviors)
						Behaviors.Add(MainBehavior);

				} else {
				
					//Subclass couldnt be determined
				
				}

			} catch (Exception exc) {

				BehaviorLogger.Write("Exception Found During Behavior Setup:", BehaviorDebugEnum.Error, true);
				BehaviorLogger.Write(exc.ToString(), BehaviorDebugEnum.Error, true);

			}

		}

		public static BehaviorSubclass GetSubclassFromCustomData(string customData) {

			if (string.IsNullOrWhiteSpace(customData))
				return BehaviorSubclass.None;

			//CoreBehavior
			if (customData.Contains("[BehaviorName:CoreBehavior]")) {

				return BehaviorSubclass.CoreBehavior;

			}

			//CargoShip
			if (customData.Contains("[BehaviorName:CargoShip]")) {

				return BehaviorSubclass.CargoShip;

			}

			//Escort
			if (customData.Contains("[BehaviorName:Escort]")) {

				return BehaviorSubclass.Escort;

			}

			//Fighter
			if (customData.Contains("[BehaviorName:Fighter]")) {

				return BehaviorSubclass.Fighter;

			}

			//HorseFighter
			if (customData.Contains("[BehaviorName:HorseFighter]")) {

				return BehaviorSubclass.HorseFighter;

			}

			//Horsefly
			if (customData.Contains("[BehaviorName:Horsefly]")) {

				return BehaviorSubclass.Horsefly;

			}

			//Hunter
			if (customData.Contains("[BehaviorName:Hunter]")) {

				return BehaviorSubclass.Hunter;

			}

			//Nautical
			if (customData.Contains("[BehaviorName:Nautical]")) {

				return BehaviorSubclass.Nautical;

			}

			//Passive
			if (customData.Contains("[BehaviorName:Passive]")) {

				return BehaviorSubclass.Passive;

			}

			//Patrol
			if (customData.Contains("[BehaviorName:Patrol]")) {

				return BehaviorSubclass.Patrol;

			}

			//Sniper
			if (customData.Contains("[BehaviorName:Sniper]")) {

				return BehaviorSubclass.Sniper;

			}

			//Strike
			if (customData.Contains("[BehaviorName:Strike]")) {

				return BehaviorSubclass.Strike;

			}

			//Vulture
			if (customData.Contains("[BehaviorName:Vulture]")) {

				return BehaviorSubclass.Vulture;

			}

			return BehaviorSubclass.None;

		}

		private static void ProcessParallelMethods() {

			if (Mode != BehaviorManagerMode.Parallel)
				return;

			MyAPIGateway.Parallel.Start(() => {

				try {

					Mode = BehaviorManagerMode.ParallelWorking;
					//BehaviorLogger.Write("Start Parallel Methods", BehaviorDebugEnum.General);
					ProcessCollisionChecksParallel();
					ProcessTargetingParallel();
					ProcessAutoPilotParallel();
					ProcessWeaponsParallel();
					ProcessTriggersParallel();
					Mode = BehaviorManagerMode.MainThread;
					//BehaviorLogger.Write("End Parallel Methods", BehaviorDebugEnum.General);

				} catch (Exception e) {

					Mode = BehaviorManagerMode.Parallel;
					BehaviorLogger.Write("Exception in Parallel Calculations", BehaviorDebugEnum.General);
					BehaviorLogger.Write(e.ToString(), BehaviorDebugEnum.General);

				}


			});

		}

		private static void ProcessMainThreadMethods() {

			if (Mode != BehaviorManagerMode.MainThread)
				return;

			//BehaviorLogger.Write("Start Main Methods", BehaviorDebugEnum.General);
			ProcessAutoPilotMain();
			ProcessWeaponsMain();
			ProcessTriggersMain();
			ProcessDiagnosticsMain();
			ProcessDespawnConditions();
			ProcessMainBehavior();
			Mode = BehaviorManagerMode.None;
			//BehaviorLogger.Write("End Main Methods", BehaviorDebugEnum.General);

		}

		private static void ProcessCollisionChecksParallel() {

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				if (!Behaviors[i].IsAIReady())
					continue;

				Behaviors[i].ProcessCollisionChecks();
				Behaviors[i].Heartbeat.CollisionParallel++;

			}

		}

		private static void ProcessTargetingParallel() {

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				if (!Behaviors[i].IsAIReady())
					continue;

				Behaviors[i].ProcessTargetingChecks();
				Behaviors[i].Heartbeat.TargetingParallel++;

			}

		}

		private static void ProcessAutoPilotParallel() {

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				if (!Behaviors[i].IsAIReady())
					continue;

				Behaviors[i].ProcessAutoPilotChecks();
				Behaviors[i].Heartbeat.AutopilotParallel++;

			}

		}

		private static void ProcessAutoPilotMain() {

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				if (!Behaviors[i].IsAIReady())
					continue;

				Behaviors[i].EngageAutoPilot();
				Behaviors[i].Heartbeat.AutopilotMain++;

			}

		}

		private static void ProcessWeaponsParallel() {

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				if (!Behaviors[i].IsAIReady())
					continue;

				Behaviors[i].ProcessWeaponChecks();
				Behaviors[i].Heartbeat.WeaponsParallel++;

			}

		}

		private static void ProcessWeaponsMain() {

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				if (!Behaviors[i].IsAIReady())
					continue;

				Behaviors[i].SetInitialWeaponReadiness();
				Behaviors[i].FireWeapons();
				Behaviors[i].Heartbeat.WeaponsMain++;

			}

		}

		private static void ProcessWeaponsBarrage() {

			//TODO
			//Mexpex reported single unreproducible crash here
			//Do some testing here just to be sure.

			if (Behaviors == null) {

				BehaviorLogger.Write("Behaviors List in BehaviorManager is NULL", BehaviorDebugEnum.Error);
				return;

			}

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				if (Behaviors[i] == null) {

					BehaviorLogger.Write("Behavior in Active Behaviors is NULL", BehaviorDebugEnum.Error);
					continue;

				}

				if (!Behaviors[i].IsAIReady())
					continue;

				Behaviors[i].FireBarrageWeapons();

			}

		}

		private static void ProcessTriggersParallel() {

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				if (!Behaviors[i].IsAIReady())
					continue;

				Behaviors[i].ProcessTriggerChecks();
				Behaviors[i].Heartbeat.TriggersParallel++;

			}

		}

		private static void ProcessTriggersMain() {

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				if (!Behaviors[i].IsAIReady())
					continue;

				Behaviors[i].ProcessActivatedTriggers();
				Behaviors[i].Heartbeat.TriggersMain++;

			}

		}

		private static void ProcessDiagnosticsMain() {

			if (!BehaviorLogger.ActiveDebug.HasFlag(BehaviorDebugEnum.BehaviorLog))
				return;

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				if (!Behaviors[i].IsAIReady())
					continue;

				Behaviors[i].Diagnostic.UpdateLogFile();

			}

		}

		private static void ProcessDespawnConditions() {

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				if (!Behaviors[i].IsAIReady())
					continue;

				Behaviors[i].CheckDespawnConditions();
				Behaviors[i].Heartbeat.DespawnMain++;

			}

		}

		private static void ProcessMainBehavior() {

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				if (!Behaviors[i].IsAIReady())
					continue;

				Behaviors[i].RunMainBehavior();
				Behaviors[i].Heartbeat.BehaviorMain++;

			}

		}

		public static IBehavior GetBehavior(IMyRemoteControl remoteControl) {

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				if (Behaviors[i].IsAIReady() && Behaviors[i].RemoteControl == remoteControl)
					return Behaviors[i];

			}

			return null;

		}

	}

}
