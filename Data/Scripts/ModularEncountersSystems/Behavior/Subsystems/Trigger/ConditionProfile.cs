using ModularEncountersSystems.Behavior;
using ProtoBuf;
using ModularEncountersSystems.Helpers;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game.ModAPI;
using VRageMath;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.API;
using ModularEncountersSystems.Configuration;

namespace ModularEncountersSystems.Behavior.Subsystems.Trigger {

	public enum CounterCompareEnum {
	
		GreaterOrEqual,
		Greater,
		Equal,
		NotEqual,
		Less,
		LessOrEqual,
	
	}

	[ProtoContract]
	public class ConditionProfile {

		[ProtoMember(1)]
		public bool UseConditions;

		[ProtoMember(2)]
		public bool MatchAnyCondition;

		[ProtoMember(3)]
		public bool CheckAllLoadedModIDs;

		[ProtoMember(4)]
		public List<long> AllModIDsToCheck;

		[ProtoMember(5)]
		public bool CheckAnyLoadedModIDs;

		[ProtoMember(6)]
		public List<long> AnyModIDsToCheck;

		[ProtoMember(7)]
		public bool CheckTrueBooleans;

		[ProtoMember(8)]
		public List<string> TrueBooleans;

		[ProtoMember(9)]
		public bool CheckCustomCounters;

		[ProtoMember(10)]
		public List<string> CustomCounters;

		[ProtoMember(11)]
		public List<int> CustomCountersTargets;

		[ProtoMember(12)]
		public bool CheckGridSpeed;

		[ProtoMember(13)]
		public float MinGridSpeed;

		[ProtoMember(14)]
		public float MaxGridSpeed;

		[ProtoMember(15)]
		public bool CheckMESBlacklistedSpawnGroups;

		[ProtoMember(16)]
		public List<string> SpawnGroupBlacklistContainsAll;

		[ProtoMember(17)]
		public List<string> SpawnGroupBlacklistContainsAny;

		[ProtoMember(18)]
		public string ProfileSubtypeId;

		[ProtoMember(19)]
		public bool UseRequiredFunctionalBlocks;

		[ProtoMember(20)]
		public List<string> RequiredAllFunctionalBlockNames;

		[ProtoMember(21)]
		public List<string> RequiredAnyFunctionalBlockNames;

		[ProtoMember(22)]
		public List<string> RequiredNoneFunctionalBlockNames;

		[ProtoMember(23)]
		public bool UseAccumulatedDamageWatcher;

		[ProtoMember(24)]
		public float MinAccumulatedDamage;

		[ProtoMember(25)]
		public float MaxAccumulatedDamage;

		[ProtoMember(26)]
		public bool CheckTrueSandboxBooleans;

		[ProtoMember(27)]
		public List<string> TrueSandboxBooleans;

		[ProtoMember(28)]
		public bool CheckCustomSandboxCounters;

		[ProtoMember(29)]
		public List<string> CustomSandboxCounters;

		[ProtoMember(30)]
		public List<int> CustomSandboxCountersTargets;

		[ProtoMember(31)]
		public bool CheckTargetAltitudeDifference;

		[ProtoMember(32)]
		public double MinTargetAltitudeDifference;

		[ProtoMember(33)]
		public double MaxTargetAltitudeDifference;

		[ProtoMember(34)]
		public bool CheckTargetDistance;

		[ProtoMember(35)]
		public double MinTargetDistance;

		[ProtoMember(36)]
		public double MaxTargetDistance;

		[ProtoMember(37)]
		public bool CheckTargetAngleFromForward;

		[ProtoMember(38)]
		public double MinTargetAngle;

		[ProtoMember(39)]
		public double MaxTargetAngle;

		[ProtoMember(40)]
		public bool CheckIfTargetIsChasing;

		[ProtoMember(41)]
		public double MinTargetChaseAngle;

		[ProtoMember(42)]
		public double MaxTargetChaseAngle;

		[ProtoMember(43)]
		public List<CounterCompareEnum> CounterCompareTypes;

		[ProtoMember(44)]
		public List<CounterCompareEnum> SandboxCounterCompareTypes;

		[ProtoMember(45)]
		public bool CheckIfGridNameMatches;

		[ProtoMember(46)]
		public bool AllowPartialGridNameMatches;

		[ProtoMember(47)]
		public List<string> GridNamesToCheck;

		[ProtoMember(48)]
		public bool UnderwaterCheck;

		[ProtoMember(49)]
		public bool IsUnderwater;

		[ProtoMember(50)]
		public bool TargetUnderwaterCheck;

		[ProtoMember(51)]
		public bool TargetIsUnderwater;

		[ProtoMember(52)]
		public bool BehaviorModeCheck;

		[ProtoMember(53)]
		public BehaviorMode CurrentBehaviorMode;

		[ProtoMember(54)]
		public double MinDistanceUnderwater;

		[ProtoMember(55)]
		public double MaxDistanceUnderwater;

		[ProtoMember(56)]
		public double MinTargetDistanceUnderwater;

		[ProtoMember(57)]
		public double MaxTargetDistanceUnderwater;

		[ProtoMember(58)]
		public bool AltitudeCheck;

		[ProtoMember(59)]
		public double MinAltitude;

		[ProtoMember(60)]
		public double MaxAltitude;

		[ProtoIgnore]
		private IMyRemoteControl _remoteControl;

		[ProtoIgnore]
		private IBehavior _behavior;

		[ProtoIgnore]
		private StoredSettings _settings;

		[ProtoIgnore]
		private bool _gotWatchedBlocks;

		[ProtoIgnore]
		private List<IMyCubeBlock> _watchedAllBlocks;

		[ProtoIgnore]
		private List<IMyCubeBlock> _watchedAnyBlocks;

		[ProtoIgnore]
		private List<IMyCubeBlock> _watchedNoneBlocks;

		[ProtoIgnore]
		private bool _watchingAllBlocks;

		[ProtoIgnore]
		private bool _watchingAnyBlocks;

		[ProtoIgnore]
		private bool _watchingNoneBlocks;

		[ProtoIgnore]
		private bool _watchedAllBlocksResult;

		[ProtoIgnore]
		private bool _watchedAnyBlocksResult;

		[ProtoIgnore]
		private bool _watchedNoneBlocksResult;

		public ConditionProfile() {

			UseConditions = false;
			MatchAnyCondition = false;

			CheckAllLoadedModIDs = false;
			AllModIDsToCheck = new List<long>();

			CheckAnyLoadedModIDs = false;
			AnyModIDsToCheck = new List<long>();

			CheckTrueBooleans = false;
			TrueBooleans = new List<string>();

			CheckCustomCounters = false;
			CustomCounters = new List<string>();
			CustomCountersTargets = new List<int>();

			CheckTrueSandboxBooleans = false;
			TrueSandboxBooleans = new List<string>();

			CheckCustomSandboxCounters = false;
			CustomSandboxCounters = new List<string>();
			CustomSandboxCountersTargets = new List<int>();

			CheckGridSpeed = false;
			MinGridSpeed = -1;
			MaxGridSpeed = -1;

			CheckMESBlacklistedSpawnGroups = false;
			SpawnGroupBlacklistContainsAll = new List<string>();
			SpawnGroupBlacklistContainsAny = new List<string>();

			UseRequiredFunctionalBlocks = false;
			RequiredAllFunctionalBlockNames = new List<string>();
			RequiredAnyFunctionalBlockNames = new List<string>();
			RequiredNoneFunctionalBlockNames = new List<string>();

			UseAccumulatedDamageWatcher = false;
			MinAccumulatedDamage = -1;
			MaxAccumulatedDamage = -1;

			CheckTargetAltitudeDifference = false;
			MinTargetAltitudeDifference = 0;
			MaxTargetAltitudeDifference = 0;

			CheckTargetDistance = false;
			MinTargetDistance = -1;
			MaxTargetDistance = -1;

			CheckTargetAngleFromForward = false;
			MinTargetAngle = -1;
			MaxTargetAngle = -1;

			CheckIfTargetIsChasing = false;
			MinTargetChaseAngle = -1;
			MaxTargetChaseAngle = -1;

			CounterCompareTypes = new List<CounterCompareEnum>();
			SandboxCounterCompareTypes = new List<CounterCompareEnum>();

			CheckIfGridNameMatches = false;
			AllowPartialGridNameMatches = false;
			GridNamesToCheck = new List<string>();

			UnderwaterCheck = false;
			IsUnderwater = false;
			TargetUnderwaterCheck = false;
			TargetIsUnderwater = false;
			MinDistanceUnderwater = -1;
			MaxDistanceUnderwater = -1;
			MinTargetDistanceUnderwater = -1;
			MaxTargetDistanceUnderwater = -1;

			AltitudeCheck = false;
			MinAltitude = -1;
			MaxAltitude = -1;

			BehaviorModeCheck = false;
			CurrentBehaviorMode = BehaviorMode.Init;

			ProfileSubtypeId = "";

			_remoteControl = null;
			_settings = new StoredSettings();

			_gotWatchedBlocks = false;
			_watchedAllBlocks = new List<IMyCubeBlock>();
			_watchedAnyBlocks = new List<IMyCubeBlock>();
			_watchedNoneBlocks = new List<IMyCubeBlock>();
			_watchedAllBlocksResult = false;
			_watchedAnyBlocksResult = false;
			_watchedNoneBlocksResult = false;

		}

		public void SetReferences(IMyRemoteControl remoteControl, StoredSettings settings) {

			_remoteControl = remoteControl;
			_settings = settings;

		}

		public bool AreConditionsMets() {

			if (!_gotWatchedBlocks)
				SetupWatchedBlocks();

			if (UseConditions == false) {

				return true;

			}

			int usedConditions = 0;
			int satisfiedConditions = 0;

			if (_behavior == null) {

				_behavior = BehaviorManager.GetBehavior(_remoteControl);

				if (_behavior == null)
					return false;

			}

			if (CheckAllLoadedModIDs == true) {

				usedConditions++;
				bool missingMod = false;

				foreach (var mod in AllModIDsToCheck) {

					if (AddonManager.ModIdList.Contains((ulong)mod) == false) {

						BehaviorLogger.Write(ProfileSubtypeId + ": Mod ID Not Present", BehaviorDebugEnum.Condition);
						missingMod = true;
						break;

					}

				}

				if (!missingMod)
					satisfiedConditions++;

			}

			if (CheckAnyLoadedModIDs == true) {

				usedConditions++;

				foreach (var mod in AllModIDsToCheck) {

					if (AddonManager.ModIdList.Contains((ulong)mod)) {

						BehaviorLogger.Write(ProfileSubtypeId + ": A Mod ID was Found: " + mod.ToString(), BehaviorDebugEnum.Condition);
						satisfiedConditions++;
						break;

					}

				}

			}

			if (CheckTrueBooleans == true) {

				usedConditions++;
				bool failedCheck = false;

				foreach (var boolName in TrueBooleans) {

					if (!_settings.GetCustomBoolResult(boolName)) {

						BehaviorLogger.Write(ProfileSubtypeId + ": Boolean Not True: " + boolName, BehaviorDebugEnum.Condition);
						failedCheck = true;
						break;

					}

				}

				if (!failedCheck)
					satisfiedConditions++;

			}

			if (CheckCustomCounters == true) {

				usedConditions++;
				bool failedCheck = false;

				if (CustomCounters.Count == CustomCountersTargets.Count) {

					for (int i = 0; i < CustomCounters.Count; i++) {

						try {

							var compareType = CounterCompareEnum.GreaterOrEqual;

							if (i <= CounterCompareTypes.Count - 1)
								compareType = CounterCompareTypes[i];

							if (_settings.GetCustomCounterResult(CustomCounters[i], CustomCountersTargets[i], compareType) == false) {

								BehaviorLogger.Write(ProfileSubtypeId + ": Counter Amount Condition Not Satisfied: " + CustomCounters[i], BehaviorDebugEnum.Condition);
								failedCheck = true;
								break;

							}

						} catch (Exception e) {

							BehaviorLogger.Write("Exception: ", BehaviorDebugEnum.Condition);
							BehaviorLogger.Write(e.ToString(), BehaviorDebugEnum.Condition);

						}

					}

				} else {

					BehaviorLogger.Write(ProfileSubtypeId + ": Counter Names and Targets List Counts Don't Match. Check Your Condition Profile", BehaviorDebugEnum.Condition);
					failedCheck = true;

				}

				if (!failedCheck)
					satisfiedConditions++;

			}

			if (CheckTrueSandboxBooleans == true) {

				usedConditions++;
				bool failedCheck = false;

				for (int i = 0; i < TrueSandboxBooleans.Count; i++) {

					try {

						bool output = false;
						var result = MyAPIGateway.Utilities.GetVariable(TrueSandboxBooleans[i], out output);

						if (!result || !output) {

							BehaviorLogger.Write(ProfileSubtypeId + ": Sandbox Boolean False: " + TrueSandboxBooleans[i], BehaviorDebugEnum.Condition);
							failedCheck = true;
							break;

						}

					} catch (Exception e) {

						BehaviorLogger.Write("Exception: ", BehaviorDebugEnum.Condition);
						BehaviorLogger.Write(e.ToString(), BehaviorDebugEnum.Condition);

					}

				}

				if (!failedCheck)
					satisfiedConditions++;

			}

			if (CheckCustomSandboxCounters == true) {

				usedConditions++;
				bool failedCheck = false;

				if (CustomSandboxCounters.Count == CustomSandboxCountersTargets.Count) {

					for (int i = 0; i < CustomSandboxCounters.Count; i++) {

						try {

							int counter = 0;
							var result = MyAPIGateway.Utilities.GetVariable(CustomSandboxCounters[i], out counter);

							var compareType = CounterCompareEnum.GreaterOrEqual;

							if (i <= SandboxCounterCompareTypes.Count - 1)
								compareType = SandboxCounterCompareTypes[i];

							bool counterResult = false;

							if (compareType == CounterCompareEnum.GreaterOrEqual)
								counterResult = (counter >= CustomSandboxCountersTargets[i]);

							if (compareType == CounterCompareEnum.Greater)
								counterResult = (counter > CustomSandboxCountersTargets[i]);

							if (compareType == CounterCompareEnum.Equal)
								counterResult = (counter == CustomSandboxCountersTargets[i]);

							if (compareType == CounterCompareEnum.NotEqual)
								counterResult = (counter != CustomSandboxCountersTargets[i]);

							if (compareType == CounterCompareEnum.Less)
								counterResult = (counter < CustomSandboxCountersTargets[i]);

							if (compareType == CounterCompareEnum.LessOrEqual)
								counterResult = (counter <= CustomSandboxCountersTargets[i]);

							if (!result || !counterResult) {

								BehaviorLogger.Write(ProfileSubtypeId + ": Sandbox Counter Amount Condition Not Satisfied: " + CustomSandboxCounters[i], BehaviorDebugEnum.Condition);
								failedCheck = true;
								break;

							}

						} catch (Exception e) {

							BehaviorLogger.Write("Exception: ", BehaviorDebugEnum.Condition);
							BehaviorLogger.Write(e.ToString(), BehaviorDebugEnum.Condition);

						}

					}

				} else {

					BehaviorLogger.Write(ProfileSubtypeId + ": Sandbox Counter Names and Targets List Counts Don't Match. Check Your Condition Profile", BehaviorDebugEnum.Condition);
					failedCheck = true;

				}

				if (!failedCheck)
					satisfiedConditions++;

			}

			if (CheckGridSpeed == true) {

				usedConditions++;
				float speed = (float)_remoteControl.GetShipSpeed();

				if ((MinGridSpeed == -1 || speed >= MinGridSpeed) && (MaxGridSpeed == -1 || speed <= MaxGridSpeed)) {

					BehaviorLogger.Write(ProfileSubtypeId + ": Grid Speed High Enough", BehaviorDebugEnum.Condition);
					satisfiedConditions++;

				} else {

					BehaviorLogger.Write(ProfileSubtypeId + ": Grid Speed Not High Enough", BehaviorDebugEnum.Condition);

				}

			}

			if (CheckMESBlacklistedSpawnGroups) {

				if (SpawnGroupBlacklistContainsAll.Count > 0) {

					usedConditions++;
					bool failedCheck = false;

					foreach (var group in SpawnGroupBlacklistContainsAll) {

						if (Settings.General.NpcSpawnGroupBlacklist.Contains<string>(group) == false) {

							BehaviorLogger.Write(ProfileSubtypeId + ": A Spawngroup was not on MES BlackList: " + group, BehaviorDebugEnum.Condition);
							failedCheck = true;
							break;

						}

					}

					if (!failedCheck)
						satisfiedConditions++;

				}

				if (SpawnGroupBlacklistContainsAny.Count > 0) {

					usedConditions++;
					foreach (var group in SpawnGroupBlacklistContainsAll) {

						if (Settings.General.NpcSpawnGroupBlacklist.Contains<string>(group)) {

							BehaviorLogger.Write(ProfileSubtypeId + ": A Spawngroup was on MES BlackList: " + group, BehaviorDebugEnum.Condition);
							satisfiedConditions++;
							break;

						}

					}

				}

			}

			if (UseAccumulatedDamageWatcher) {

				usedConditions++;
				bool failedCheck = false;
				BehaviorLogger.Write("Damage Accumulated: " + _settings.TotalDamageAccumulated, BehaviorDebugEnum.Condition);

				if (MinAccumulatedDamage >= 0 && _settings.TotalDamageAccumulated < MinAccumulatedDamage)
					failedCheck = true;

				if (MaxAccumulatedDamage >= 0 && _settings.TotalDamageAccumulated > MaxAccumulatedDamage)
					failedCheck = true;

				if (!failedCheck)
					satisfiedConditions++;

			}

			if (UseRequiredFunctionalBlocks) {

				if (_watchingAllBlocks) {

					usedConditions++;

					if (_watchedAllBlocksResult)
						satisfiedConditions++;

				}

				if (_watchingAnyBlocks) {

					usedConditions++;

					if (_watchedAnyBlocksResult)
						satisfiedConditions++;

				}

				if (_watchingNoneBlocks) {

					usedConditions++;

					if (_watchedNoneBlocksResult)
						satisfiedConditions++;

				}

			}

			if (CheckTargetAltitudeDifference) {

				usedConditions++;

				if (_behavior.AutoPilot.Targeting.HasTarget() && _behavior.AutoPilot.InGravity()) {

					var planetPos = _behavior.AutoPilot.CurrentPlanet.Center();
					var targetCoreDist = _behavior.AutoPilot.Targeting.Target.Distance(planetPos);
					var myCoreDist = Vector3D.Distance(planetPos, _remoteControl.GetPosition());
					var difference = targetCoreDist - myCoreDist;

					if (difference >= this.MinTargetAltitudeDifference && difference <= this.MaxTargetAltitudeDifference)
						satisfiedConditions++;

				}
			
			}

			if (CheckTargetDistance) {

				usedConditions++;

				if (_behavior.AutoPilot.Targeting.HasTarget()) {

					var dist = _behavior.AutoPilot.Targeting.Target.Distance(_remoteControl.GetPosition());

					if ((this.MinTargetDistance == -1 || dist >= this.MinTargetDistance) && (this.MaxTargetDistance == -1 || dist <= this.MaxTargetDistance))
						satisfiedConditions++;

				}

			}

			if (CheckTargetAngleFromForward) {

				usedConditions++;

				if (_behavior.AutoPilot.Targeting.HasTarget()) {

					var dirToTarget = Vector3D.Normalize(_behavior.AutoPilot.Targeting.GetTargetCoords() - _remoteControl.GetPosition());
					var myForward = _behavior.AutoPilot.RefBlockMatrixRotation.Forward;
					var angle = VectorHelper.GetAngleBetweenDirections(dirToTarget, myForward);

					if ((this.MinTargetAngle == -1 || angle >= this.MinTargetAngle) && (this.MaxTargetAngle == -1 || angle <= this.MaxTargetAngle))
						satisfiedConditions++;

				}

			}

			if (CheckIfTargetIsChasing) {

				usedConditions++;

				if (_behavior.AutoPilot.Targeting.HasTarget()) {

					var dirFromTarget = Vector3D.Normalize(_remoteControl.GetPosition() - _behavior.AutoPilot.Targeting.GetTargetCoords());
					var targetVelocity = Vector3D.Normalize(_behavior.AutoPilot.Targeting.Target.CurrentVelocity());

					if (targetVelocity.IsValid() && targetVelocity.Length() > 0) {

						var angle = VectorHelper.GetAngleBetweenDirections(dirFromTarget, targetVelocity);

						if ((this.MinTargetChaseAngle == -1 || angle >= this.MinTargetChaseAngle) && (this.MaxTargetChaseAngle == -1 || angle <= this.MaxTargetChaseAngle))
							satisfiedConditions++;
					
					}

				}

			}

			if (CheckIfGridNameMatches) {

				usedConditions++;

				if(!string.IsNullOrWhiteSpace(_remoteControl.SlimBlock.CubeGrid.CustomName)){

					bool pass = false;

					foreach (var name in GridNamesToCheck) {

						if (AllowPartialGridNameMatches) {

							if (_remoteControl.SlimBlock.CubeGrid.CustomName.Contains(name))
								pass = true;

						} else {

							if (_remoteControl.SlimBlock.CubeGrid.CustomName == name)
								pass = true;

						}

					}

					if(pass)
						satisfiedConditions++;

				}

			}

			if (UnderwaterCheck) {

				usedConditions++;

				if(_behavior.AutoPilot.CurrentPlanet != null)
					if (_behavior.AutoPilot.CurrentPlanet.UnderwaterAndDepthCheck(_remoteControl.GetPosition(), IsUnderwater, MinDistanceUnderwater, MaxDistanceUnderwater))
						satisfiedConditions++;

			}

			if (TargetUnderwaterCheck) {

				usedConditions++;

				if(_behavior.AutoPilot.Targeting.HasTarget() && _behavior.AutoPilot.CurrentPlanet != null)
					if (_behavior.AutoPilot.CurrentPlanet.UnderwaterAndDepthCheck(_behavior.AutoPilot.Targeting.TargetLastKnownCoords, TargetIsUnderwater, MinTargetDistanceUnderwater, MaxTargetDistanceUnderwater))
						satisfiedConditions++;

			}

			if (BehaviorModeCheck) {

				usedConditions++;

				if(_behavior.Mode == CurrentBehaviorMode)
					satisfiedConditions++;

			}

			if (AltitudeCheck) {

				usedConditions++;

				if (_behavior.AutoPilot.CurrentPlanet != null)
					if ((MinAltitude == -1 || _behavior.AutoPilot.MyAltitude > MinAltitude) && (MaxAltitude == -1 || _behavior.AutoPilot.MyAltitude < MaxAltitude))
						satisfiedConditions++;

			}

			if (MatchAnyCondition == false) {

				bool result = satisfiedConditions >= usedConditions;
				BehaviorLogger.Write(ProfileSubtypeId + ": All Condition Satisfied: " + result.ToString(), BehaviorDebugEnum.Condition);
				BehaviorLogger.Write(string.Format("Used Conditions: {0} // Satisfied Conditions: {1}", usedConditions, satisfiedConditions), BehaviorDebugEnum.Condition);
				return result;

			} else {

				bool result = satisfiedConditions > 0;
				BehaviorLogger.Write(ProfileSubtypeId + ": Any Condition(s) Satisfied: " + result.ToString(), BehaviorDebugEnum.Condition);
				BehaviorLogger.Write(string.Format("Used Conditions: {0} // Satisfied Conditions: {1}", usedConditions, satisfiedConditions), BehaviorDebugEnum.Condition);
				return result;

			}

		}

		

		private void SetupWatchedBlocks() {

			BehaviorLogger.Write("Setting Up Required Block Watcher", BehaviorDebugEnum.Condition);
			_gotWatchedBlocks = true;
			_watchedAnyBlocks.Clear();
			_watchedAllBlocks.Clear();
			_watchedNoneBlocks.Clear();

			if (!UseRequiredFunctionalBlocks)
				return;

			_remoteControl.SlimBlock.CubeGrid.OnGridSplit += GridSplitHandler;
			var allBlocks = TargetHelper.GetAllBlocks(_remoteControl?.SlimBlock?.CubeGrid).Where(x => x.FatBlock != null);

			foreach (var block in allBlocks) {

				var terminalBlock = block.FatBlock as IMyTerminalBlock;

				if (terminalBlock == null)
					continue;

				BehaviorLogger.Write(" - " + terminalBlock.CustomName.Trim(), BehaviorDebugEnum.Condition);

				if (RequiredAllFunctionalBlockNames.Contains(terminalBlock.CustomName.Trim())) {

					BehaviorLogger.Write("Monitoring Required-All Block: " + terminalBlock.CustomName, BehaviorDebugEnum.Condition);
					_watchedAllBlocks.Add(block.FatBlock);
					block.FatBlock.IsWorkingChanged += CheckAllBlocks;
					_watchingAllBlocks = true;

				}

				if (RequiredAnyFunctionalBlockNames.Contains(terminalBlock.CustomName.Trim())) {

					BehaviorLogger.Write("Monitoring Required-Any Block: " + terminalBlock.CustomName, BehaviorDebugEnum.Condition);
					_watchedAnyBlocks.Add(block.FatBlock);
					block.FatBlock.IsWorkingChanged += CheckAnyBlocks;
					_watchingAnyBlocks = true;

				}

				if (RequiredNoneFunctionalBlockNames.Contains(terminalBlock.CustomName.Trim())) {

					BehaviorLogger.Write("Monitoring Required-None Block: " + terminalBlock.CustomName, BehaviorDebugEnum.Condition);
					_watchedNoneBlocks.Add(block.FatBlock);
					block.FatBlock.IsWorkingChanged += CheckNoneBlocks;
					_watchingNoneBlocks = true;

				}

			}

			CheckAllBlocks();
			CheckAnyBlocks();
			CheckNoneBlocks();

		}

		private void CheckAllBlocks(IMyCubeBlock cubeBlock = null) {

			for (int i = _watchedAllBlocks.Count - 1; i >= 0; i--) {

				var block = _watchedAllBlocks[i];

				if (block == null || block?.SlimBlock?.CubeGrid == null || block.SlimBlock.CubeGrid.MarkedForClose) {

					_watchedAllBlocks.RemoveAt(i);
					continue;

				}

				if (!block.IsWorking || !block.IsFunctional) {

					_watchedAllBlocksResult = false;
					return;

				}

			}

			_watchedAllBlocksResult = true;

		}

		private void CheckAnyBlocks(IMyCubeBlock cubeBlock = null) {

			for (int i = _watchedAnyBlocks.Count - 1; i >= 0; i--) {

				var block = _watchedAnyBlocks[i];

				if (block == null || block?.SlimBlock?.CubeGrid == null || block.SlimBlock.CubeGrid.MarkedForClose) {

					_watchedAnyBlocks.RemoveAt(i);
					continue;

				}

				if (block.IsWorking && block.IsFunctional) {

					_watchedAnyBlocksResult = true;
					return;

				}

			}

			_watchedAnyBlocksResult = false;

		}

		private void CheckNoneBlocks(IMyCubeBlock cubeBlock = null) {

			for (int i = _watchedNoneBlocks.Count - 1; i >= 0; i--) {

				var block = _watchedNoneBlocks[i];

				if (block == null || block?.SlimBlock?.CubeGrid == null || block.SlimBlock.CubeGrid.MarkedForClose) {

					_watchedNoneBlocks.RemoveAt(i);
					continue;

				}

				if (block.IsWorking && block.IsFunctional) {

					_watchedNoneBlocksResult = false;
					return;

				}

			}

			_watchedNoneBlocksResult = true;

		}

		private void GridSplitHandler(IMyCubeGrid gridA, IMyCubeGrid gridB) {

			gridA.OnGridSplit -= GridSplitHandler;
			gridB.OnGridSplit -= GridSplitHandler;

			if (_remoteControl == null || _remoteControl?.SlimBlock?.CubeGrid == null || _remoteControl.SlimBlock.CubeGrid.MarkedForClose)
				return;

			_remoteControl.SlimBlock.CubeGrid.OnGridSplit += GridSplitHandler;

			for (int i = _watchedAllBlocks.Count - 1; i >= 0; i--) {

				var block = _watchedAllBlocks[i];

				if (block == null || !MyAPIGateway.Entities.Exist(block?.SlimBlock?.CubeGrid)) {

					_watchedAllBlocks.RemoveAt(i);
					continue;

				}

				if (!_remoteControl.SlimBlock.CubeGrid.IsSameConstructAs(block.SlimBlock.CubeGrid)) {

					_watchedAllBlocks.RemoveAt(i);
					continue;

				}

			}

			for (int i = _watchedAnyBlocks.Count - 1; i >= 0; i--) {

				var block = _watchedAnyBlocks[i];

				if (block == null || !MyAPIGateway.Entities.Exist(block?.SlimBlock?.CubeGrid)) {

					_watchedAnyBlocks.RemoveAt(i);
					continue;

				}

				if (!_remoteControl.SlimBlock.CubeGrid.IsSameConstructAs(block.SlimBlock.CubeGrid)) {

					_watchedAnyBlocks.RemoveAt(i);
					continue;

				}

			}

			for (int i = _watchedNoneBlocks.Count - 1; i >= 0; i--) {

				var block = _watchedNoneBlocks[i];

				if (block == null || !MyAPIGateway.Entities.Exist(block?.SlimBlock?.CubeGrid)) {

					_watchedNoneBlocks.RemoveAt(i);
					continue;

				}

				if (!_remoteControl.SlimBlock.CubeGrid.IsSameConstructAs(block.SlimBlock.CubeGrid)) {

					_watchedNoneBlocks.RemoveAt(i);
					continue;

				}

			}

			CheckAllBlocks();
			CheckAnyBlocks();
			CheckNoneBlocks();

		}

		public void InitTags(string customData) {

			if (string.IsNullOrWhiteSpace(customData) == false) {

				var descSplit = customData.Split('\n');

				foreach (var tag in descSplit) {

					//UseConditions
					if (tag.Contains("[UseConditions:") == true) {

						TagParse.TagBoolCheck(tag, ref UseConditions);

					}

					//MatchAnyCondition
					if (tag.Contains("[MatchAnyCondition:") == true) {

						TagParse.TagBoolCheck(tag, ref MatchAnyCondition);

					}

					//CheckAllLoadedModIDs
					if (tag.Contains("[CheckAllLoadedModIDs:") == true) {

						TagParse.TagBoolCheck(tag, ref CheckAllLoadedModIDs);

					}

					//AllModIDsToCheck
					if (tag.Contains("[AllModIDsToCheck:") == true) {

						TagParse.TagLongCheck(tag, ref AllModIDsToCheck);

					}

					//CheckAnyLoadedModIDs
					if (tag.Contains("[CheckAnyLoadedModIDs:") == true) {

						TagParse.TagBoolCheck(tag, ref CheckAnyLoadedModIDs);

					}

					//AnyModIDsToCheck
					if (tag.Contains("[AnyModIDsToCheck:") == true) {

						TagParse.TagLongCheck(tag, ref AnyModIDsToCheck);

					}

					//CheckTrueBooleans
					if (tag.Contains("[CheckTrueBooleans:") == true) {

						TagParse.TagBoolCheck(tag, ref CheckTrueBooleans);

					}

					//TrueBooleans
					if (tag.Contains("[TrueBooleans:") == true) {

						TagParse.TagStringListCheck(tag, ref TrueBooleans);

					}

					//CheckCustomCounters
					if (tag.Contains("[CheckCustomCounters:") == true) {

						TagParse.TagBoolCheck(tag, ref CheckCustomCounters);

					}

					//CustomCounters
					if (tag.Contains("[CustomCounters:") == true) {

						TagParse.TagStringListCheck(tag, ref CustomCounters);

					}

					//CustomCountersTargets
					if (tag.Contains("[CustomCountersTargets:") == true) {

						TagParse.TagIntListCheck(tag, ref CustomCountersTargets);

					}

					//CheckTrueSandboxBooleans
					if (tag.Contains("[CheckTrueSandboxBooleans:") == true) {

						TagParse.TagBoolCheck(tag, ref CheckTrueSandboxBooleans);

					}

					//TrueSandboxBooleans
					if (tag.Contains("[TrueSandboxBooleans:") == true) {

						TagParse.TagStringListCheck(tag, ref TrueSandboxBooleans);

					}

					//CheckCustomSandboxCounters
					if (tag.Contains("[CheckCustomSandboxCounters:") == true) {

						TagParse.TagBoolCheck(tag, ref CheckCustomSandboxCounters);

					}

					//CustomSandboxCounters
					if (tag.Contains("[CustomSandboxCounters:") == true) {

						TagParse.TagStringListCheck(tag, ref CustomSandboxCounters);

					}

					//CustomSandboxCountersTargets
					if (tag.Contains("[CustomSandboxCountersTargets:") == true) {

						TagParse.TagIntListCheck(tag, ref CustomSandboxCountersTargets);

					}

					//CheckGridSpeed
					if (tag.Contains("[CheckGridSpeed:") == true) {

						TagParse.TagBoolCheck(tag, ref CheckGridSpeed);

					}

					//MinGridSpeed
					if (tag.Contains("[MinGridSpeed:") == true) {

						TagParse.TagFloatCheck(tag, ref MinGridSpeed);

					}

					//MaxGridSpeed
					if (tag.Contains("[MaxGridSpeed:") == true) {

						TagParse.TagFloatCheck(tag, ref MaxGridSpeed);

					}

					//CheckMESBlacklistedSpawnGroups
					if (tag.Contains("[CheckMESBlacklistedSpawnGroups:") == true) {

						TagParse.TagBoolCheck(tag, ref CheckMESBlacklistedSpawnGroups);

					}

					//SpawnGroupBlacklistContainsAll
					if (tag.Contains("[SpawnGroupBlacklistContainsAll:") == true) {

						TagParse.TagStringListCheck(tag, ref SpawnGroupBlacklistContainsAll);

					}

					//SpawnGroupBlacklistContainsAny
					if (tag.Contains("[SpawnGroupBlacklistContainsAny:") == true) {

						TagParse.TagStringListCheck(tag, ref SpawnGroupBlacklistContainsAny);

					}

					//UseRequiredFunctionalBlocks
					if (tag.Contains("[UseRequiredFunctionalBlocks:") == true) {

						TagParse.TagBoolCheck(tag, ref UseRequiredFunctionalBlocks);

					}

					//RequiredAllFunctionalBlockNames
					if (tag.Contains("[RequiredAllFunctionalBlockNames:") == true) {

						TagParse.TagStringListCheck(tag, ref RequiredAllFunctionalBlockNames);

					}

					//RequiredAnyFunctionalBlockNames
					if (tag.Contains("[RequiredAnyFunctionalBlockNames:") == true) {

						TagParse.TagStringListCheck(tag, ref RequiredAnyFunctionalBlockNames);

					}

					//RequiredNoneFunctionalBlockNames
					if (tag.Contains("[RequiredNoneFunctionalBlockNames:") == true) {

						TagParse.TagStringListCheck(tag, ref RequiredNoneFunctionalBlockNames);

					}

					//UseAccumulatedDamageWatcher
					if (tag.Contains("[UseAccumulatedDamageWatcher:") == true) {

						TagParse.TagBoolCheck(tag, ref UseAccumulatedDamageWatcher);

					}

					//MinAccumulatedDamage
					if (tag.Contains("[MinAccumulatedDamage:") == true) {

						TagParse.TagFloatCheck(tag, ref MinAccumulatedDamage);

					}

					//MaxAccumulatedDamage
					if (tag.Contains("[MaxAccumulatedDamage:") == true) {

						TagParse.TagFloatCheck(tag, ref MaxAccumulatedDamage);

					}

					//CheckTargetAltitudeDifference
					if (tag.Contains("[CheckTargetAltitudeDifference:") == true) {

						TagParse.TagBoolCheck(tag, ref CheckTargetAltitudeDifference);

					}

					//MinTargetAltitudeDifference
					if (tag.Contains("[MinTargetAltitudeDifference:") == true) {

						TagParse.TagDoubleCheck(tag, ref MinTargetAltitudeDifference);

					}

					//MaxTargetAltitudeDifference
					if (tag.Contains("[MaxTargetAltitudeDifference:") == true) {

						TagParse.TagDoubleCheck(tag, ref MaxTargetAltitudeDifference);

					}

					//CheckTargetDistance
					if (tag.Contains("[CheckTargetDistance:") == true) {

						TagParse.TagBoolCheck(tag, ref CheckTargetDistance);

					}

					//MinTargetDistance
					if (tag.Contains("[MinTargetDistance:") == true) {

						TagParse.TagDoubleCheck(tag, ref MinTargetDistance);

					}

					//MaxTargetDistance
					if (tag.Contains("[MaxTargetDistance:") == true) {

						TagParse.TagDoubleCheck(tag, ref MaxTargetDistance);

					}

					//CheckTargetAngleFromForward
					if (tag.Contains("[CheckTargetAngleFromForward:") == true) {

						TagParse.TagBoolCheck(tag, ref CheckTargetAngleFromForward);

					}

					//MinTargetAngle
					if (tag.Contains("[MinTargetAngle:") == true) {

						TagParse.TagDoubleCheck(tag, ref MinTargetAngle);

					}

					//MaxTargetAngle
					if (tag.Contains("[MaxTargetAngle:") == true) {

						TagParse.TagDoubleCheck(tag, ref MaxTargetAngle);

					}

					//CheckIfTargetIsChasing
					if (tag.Contains("[CheckIfTargetIsChasing:") == true) {

						TagParse.TagBoolCheck(tag, ref CheckIfTargetIsChasing);

					}

					//MinTargetChaseAngle
					if (tag.Contains("[MinTargetChaseAngle:") == true) {

						TagParse.TagDoubleCheck(tag, ref MinTargetChaseAngle);

					}

					//MaxTargetChaseAngle
					if (tag.Contains("[MaxTargetChaseAngle:") == true) {

						TagParse.TagDoubleCheck(tag, ref MaxTargetChaseAngle);

					}

					//CounterCompareTypes
					if (tag.Contains("[CounterCompareTypes:") == true) {

						TagParse.TagCounterCompareEnumCheck(tag, ref CounterCompareTypes);

					}

					//SandboxCounterCompareTypes
					if (tag.Contains("[SandboxCounterCompareTypes:") == true) {

						TagParse.TagCounterCompareEnumCheck(tag, ref SandboxCounterCompareTypes);

					}

					//CheckIfGridNameMatches
					if (tag.Contains("[CheckIfGridNameMatches:") == true) {

						TagParse.TagBoolCheck(tag, ref CheckIfGridNameMatches);

					}

					//AllowPartialGridNameMatches
					if (tag.Contains("[AllowPartialGridNameMatches:") == true) {

						TagParse.TagBoolCheck(tag, ref AllowPartialGridNameMatches);

					}

					//GridNamesToCheck
					if (tag.Contains("[GridNamesToCheck:") == true) {

						TagParse.TagStringListCheck(tag, ref GridNamesToCheck);

					}

					//UnderwaterCheck
					if (tag.Contains("[UnderwaterCheck:") == true) {

						TagParse.TagBoolCheck(tag, ref UnderwaterCheck);

					}

					//IsUnderwater
					if (tag.Contains("[IsUnderwater:") == true) {

						TagParse.TagBoolCheck(tag, ref IsUnderwater);

					}

					//TargetUnderwaterCheck
					if (tag.Contains("[TargetUnderwaterCheck:") == true) {

						TagParse.TagBoolCheck(tag, ref TargetUnderwaterCheck);

					}

					//TargetIsUnderwater
					if (tag.Contains("[TargetIsUnderwater:") == true) {

						TagParse.TagBoolCheck(tag, ref TargetIsUnderwater);

					}

					//MinDistanceUnderwater
					if (tag.Contains("[MinDistanceUnderwater:") == true) {

						TagParse.TagDoubleCheck(tag, ref MinDistanceUnderwater);

					}

					//MaxDistanceUnderwater
					if (tag.Contains("[MaxDistanceUnderwater:") == true) {

						TagParse.TagDoubleCheck(tag, ref MaxDistanceUnderwater);

					}

					//MinTargetDistanceUnderwater
					if (tag.Contains("[MinTargetDistanceUnderwater:") == true) {

						TagParse.TagDoubleCheck(tag, ref MinTargetDistanceUnderwater);

					}

					//MaxTargetDistanceUnderwater
					if (tag.Contains("[MaxTargetDistanceUnderwater:") == true) {

						TagParse.TagDoubleCheck(tag, ref MaxTargetDistanceUnderwater);

					}

					//AltitudeCheck
					if (tag.Contains("[AltitudeCheck:") == true) {

						TagParse.TagBoolCheck(tag, ref AltitudeCheck);

					}

					//MinAltitude
					if (tag.Contains("[MinAltitude:") == true) {

						TagParse.TagDoubleCheck(tag, ref MinAltitude);

					}

					//MaxAltitude
					if (tag.Contains("[MaxAltitude:") == true) {

						TagParse.TagDoubleCheck(tag, ref MaxAltitude);

					}

					//BehaviorModeCheck
					if (tag.Contains("[BehaviorModeCheck:") == true) {

						TagParse.TagBoolCheck(tag, ref BehaviorModeCheck);

					}

					//CurrentBehaviorMode
					if (tag.Contains("[CurrentBehaviorMode:") == true) {

						TagParse.TagBehaviorModeEnumCheck(tag, ref CurrentBehaviorMode);

					}

				}

			}

		}

	}

}


