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
using ModularEncountersSystems.Entities;
using Sandbox.Game;
using ModularEncountersSystems.Spawning;

namespace ModularEncountersSystems.Behavior.Subsystems.Trigger {

	[ProtoContract]
	public class ConditionProfile {

		[ProtoMember(18)]
		public string ProfileSubtypeId;

		[ProtoIgnore]
		public ConditionReferenceProfile ConditionReference {

			get {

				if (_conditionReference == null) {

					ProfileManager.ConditionReferenceProfiles.TryGetValue(ProfileSubtypeId, out _conditionReference);

				}

				return _conditionReference;

			}

		}

		[ProtoIgnore]
		private ConditionReferenceProfile _conditionReference;
		
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

		public void SetReferences(IMyRemoteControl remoteControl, IBehavior behavior) {

			_remoteControl = remoteControl;
			_behavior = behavior;
			_settings = _behavior?.BehaviorSettings;

		}

		public bool AreConditionsMets(Command command = null) {

			if (ConditionReference == null) {

				BehaviorLogger.Write(ProfileSubtypeId + ": Condition Reference Null", BehaviorDebugEnum.Condition);
				return false;

			}

			if (ConditionReference.UseConditions == false) {

				BehaviorLogger.Write(ProfileSubtypeId + ": Condition Not In Use", BehaviorDebugEnum.Condition);
				return true;

			}

			if (_behavior == null) {

				_behavior = BehaviorManager.GetBehavior(_remoteControl);

				if (_behavior == null) {

					BehaviorLogger.Write(ProfileSubtypeId + ": Behavior Is Null, Cannot Continue With Conditions", BehaviorDebugEnum.Condition);
					return false;

				}
					
			}

			if (!_gotWatchedBlocks)
				SetupWatchedBlocks();

			int usedConditions = 0;
			int satisfiedConditions = 0;

			if (ConditionReference.CheckAllLoadedModIDs == true) {

				usedConditions++;
				bool missingMod = false;

				foreach (var mod in ConditionReference.AllModIDsToCheck) {

					if (AddonManager.ModIdList.Contains((ulong)mod) == false) {

						BehaviorLogger.Write(ProfileSubtypeId + ": Mod ID Not Present", BehaviorDebugEnum.Condition);
						missingMod = true;
						break;

					}

				}

				if (!missingMod)
					satisfiedConditions++;

			}

			if (ConditionReference.CheckAnyLoadedModIDs == true) {

				usedConditions++;

				foreach (var mod in ConditionReference.AllModIDsToCheck) {

					if (AddonManager.ModIdList.Contains((ulong)mod)) {

						BehaviorLogger.Write(ProfileSubtypeId + ": A Mod ID was Found: " + mod.ToString(), BehaviorDebugEnum.Condition);
						satisfiedConditions++;
						break;

					}

				}

			}

			if (ConditionReference.CheckTrueBooleans == true) {

				usedConditions++;
				bool failedCheck = false;

				foreach (var boolName in ConditionReference.TrueBooleans) {

					if (!_settings.GetCustomBoolResult(boolName)) {

						BehaviorLogger.Write(ProfileSubtypeId + ": Boolean Not True: " + boolName, BehaviorDebugEnum.Condition);
						failedCheck = true;

						if (!ConditionReference.AllowAnyTrueBoolean) {

							failedCheck = true;
							break;

						}
						
					} else if (ConditionReference.AllowAnyTrueBoolean) {

						failedCheck = false;
						break;
					
					}

				}

				if (!failedCheck)
					satisfiedConditions++;

			}


			//Bool False
			if (ConditionReference.CheckFalseBooleans == true)
			{
				usedConditions++;
				bool failedCheck = false;

				for (int i = 0; i < ConditionReference.FalseBooleans.Count; i++)
				{

					var boolName = ConditionReference.FalseBooleans[i];

					try
					{

						//bool output = false;
						var output = _settings.GetCustomBoolResult(boolName);

						if (output)
						{
							//BehaviorLogger.Write(ProfileSubtypeId + ":  Boolean False: " + boolName, BehaviorDebugEnum.Condition);
							failedCheck = true;
							continue;

						}
						else if (ConditionReference.AllowAnyFalseBoolean)
						{
							failedCheck = false;
							break;

						}

					}
					catch (Exception e)
					{

						//BehaviorLogger.Write("Exception: ", BehaviorDebugEnum.Condition);
						//BehaviorLogger.Write(e.ToString(), BehaviorDebugEnum.Condition);

					}

				}

				if (!failedCheck)
					satisfiedConditions++;
			}



			if (ConditionReference.CheckCustomCounters == true) {

				usedConditions++;
				bool failedCheck = false;

				if (ConditionReference.CustomCounters.Count == ConditionReference.CustomCountersTargets.Count) {

					for (int i = 0; i < ConditionReference.CustomCounters.Count; i++) {

						try {

							var compareType = CounterCompareEnum.GreaterOrEqual;

							if (i <= ConditionReference.CounterCompareTypes.Count - 1)
								compareType = ConditionReference.CounterCompareTypes[i];

							if (_settings.GetCustomCounterResult(ConditionReference.CustomCounters[i], ConditionReference.CustomCountersTargets[i], compareType) == false) {

								BehaviorLogger.Write(ProfileSubtypeId + ": Counter Amount Condition Not Satisfied: " + ConditionReference.CustomCounters[i], BehaviorDebugEnum.Condition);
								failedCheck = true;
								break;

							} else if (ConditionReference.AllowAnyValidCounter) {

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

			if (ConditionReference.CheckTrueSandboxBooleans == true) {

				usedConditions++;
				bool failedCheck = false;

				for (int i = 0; i < ConditionReference.TrueSandboxBooleans.Count; i++) {

					var boolName = ConditionReference.TrueSandboxBooleans[i];
					if (boolName.Contains("{SpawnGroupName}") && _behavior.CurrentGrid?.Npc.SpawnGroupName != null)
					{
						boolName = boolName.Replace("{SpawnGroupName}", _behavior.CurrentGrid?.Npc.SpawnGroupName);
					}

					if (boolName.Contains("{Faction}") && _behavior.Owner?.Faction.Tag != null)
					{
						boolName = boolName.Replace("{Faction}", _behavior.Owner?.Faction.Tag);
					}

					try {

						bool output = false;
						var result = MyAPIGateway.Utilities.GetVariable(boolName, out output);

						if (!result || !output) {

							BehaviorLogger.Write(ProfileSubtypeId + ": Sandbox Boolean False: " + boolName, BehaviorDebugEnum.Condition);
							failedCheck = true;

							if (!ConditionReference.AllowAnyTrueSandboxBoolean)
								break;
							else
								continue;

						} else if (ConditionReference.AllowAnyTrueSandboxBoolean && output) {

							failedCheck = false;
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

			if (ConditionReference.CheckCustomSandboxCounters == true) {

				usedConditions++;
				bool failedCheck = false;

				if (ConditionReference.CustomSandboxCounters.Count == ConditionReference.CustomSandboxCountersTargets.Count) {

					for (int i = 0; i < ConditionReference.CustomSandboxCounters.Count; i++) {

						try {

							int counter = 0;
							var result = MyAPIGateway.Utilities.GetVariable(ConditionReference.CustomSandboxCounters[i], out counter);

							var compareType = CounterCompareEnum.GreaterOrEqual;

							if (i <= ConditionReference.SandboxCounterCompareTypes.Count - 1)
								compareType = ConditionReference.SandboxCounterCompareTypes[i];

							bool counterResult = false;

							if (compareType == CounterCompareEnum.GreaterOrEqual)
								counterResult = (counter >= ConditionReference.CustomSandboxCountersTargets[i]);

							if (compareType == CounterCompareEnum.Greater)
								counterResult = (counter > ConditionReference.CustomSandboxCountersTargets[i]);

							if (compareType == CounterCompareEnum.Equal)
								counterResult = (counter == ConditionReference.CustomSandboxCountersTargets[i]);

							if (compareType == CounterCompareEnum.NotEqual)
								counterResult = (counter != ConditionReference.CustomSandboxCountersTargets[i]);

							if (compareType == CounterCompareEnum.Less)
								counterResult = (counter < ConditionReference.CustomSandboxCountersTargets[i]);

							if (compareType == CounterCompareEnum.LessOrEqual)
								counterResult = (counter <= ConditionReference.CustomSandboxCountersTargets[i]);

							if (!result || !counterResult) {

								BehaviorLogger.Write(ProfileSubtypeId + ": Sandbox Counter Amount Condition Not Satisfied: " + ConditionReference.CustomSandboxCounters[i], BehaviorDebugEnum.Condition);
								failedCheck = true;

								if (!ConditionReference.AllowAnyValidSandboxCounter)
									break;
								else
									continue;

							} else if (ConditionReference.AllowAnyValidSandboxCounter && counterResult) {

								failedCheck = false;
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


			//Bool False
			if (ConditionReference.CheckFalseSandboxBooleans == true)
			{
				usedConditions++;
				bool failedCheck = false;

				for (int i = 0; i < ConditionReference.FalseSandboxBooleans.Count; i++)
				{

					var boolName = ConditionReference.FalseSandboxBooleans[i];

					try
					{

						bool output = false;
						var result = MyAPIGateway.Utilities.GetVariable(boolName, out output);

						if (output)
						{
							//BehaviorLogger.Write(ProfileSubtypeId + ":  Boolean False: " + boolName, BehaviorDebugEnum.Condition);
							failedCheck = true;
							continue;

						}
						else if (ConditionReference.AllowAnyFalseSandboxBoolean)
						{
							failedCheck = false;
							break;

						}

					}
					catch (Exception e)
					{

						//BehaviorLogger.Write("Exception: ", BehaviorDebugEnum.Condition);
						//BehaviorLogger.Write(e.ToString(), BehaviorDebugEnum.Condition);

					}

				}

				if (!failedCheck)
					satisfiedConditions++;
			}




			if (ConditionReference.CheckGridSpeed == true) {

				usedConditions++;
				float speed = (float)_remoteControl.GetShipSpeed();

				if ((ConditionReference.MinGridSpeed == -1 || speed >= ConditionReference.MinGridSpeed) && (ConditionReference.MaxGridSpeed == -1 || speed <= ConditionReference.MaxGridSpeed)) {

					BehaviorLogger.Write(ProfileSubtypeId + ": Grid Speed High Enough", BehaviorDebugEnum.Condition);
					satisfiedConditions++;

				} else {

					BehaviorLogger.Write(ProfileSubtypeId + ": Grid Speed Not High Enough", BehaviorDebugEnum.Condition);

				}

			}

			if (ConditionReference.CheckMESBlacklistedSpawnGroups) {

				if (ConditionReference.SpawnGroupBlacklistContainsAll.Count > 0) {

					usedConditions++;
					bool failedCheck = false;

					foreach (var group in ConditionReference.SpawnGroupBlacklistContainsAll) {

						if (Settings.General.NpcSpawnGroupBlacklist.Contains<string>(group) == false) {

							BehaviorLogger.Write(ProfileSubtypeId + ": A Spawngroup was not on MES BlackList: " + group, BehaviorDebugEnum.Condition);
							failedCheck = true;
							break;

						}

					}

					if (!failedCheck)
						satisfiedConditions++;

				}

				if (ConditionReference.SpawnGroupBlacklistContainsAny.Count > 0) {

					usedConditions++;
					foreach (var group in ConditionReference.SpawnGroupBlacklistContainsAll) {

						if (Settings.General.NpcSpawnGroupBlacklist.Contains<string>(group)) {

							BehaviorLogger.Write(ProfileSubtypeId + ": A Spawngroup was on MES BlackList: " + group, BehaviorDebugEnum.Condition);
							satisfiedConditions++;
							break;

						}

					}

				}

			}

			if (ConditionReference.UseAccumulatedDamageWatcher) {

				usedConditions++;
				bool failedCheck = false;
				BehaviorLogger.Write("Damage Accumulated: " + _settings.TotalDamageAccumulated, BehaviorDebugEnum.Condition);

				if (ConditionReference.MinAccumulatedDamage >= 0 && _settings.TotalDamageAccumulated < ConditionReference.MinAccumulatedDamage)
					failedCheck = true;

				if (ConditionReference.MaxAccumulatedDamage >= 0 && _settings.TotalDamageAccumulated > ConditionReference.MaxAccumulatedDamage)
					failedCheck = true;

				if (!failedCheck)
					satisfiedConditions++;

			}

			if (ConditionReference.UseRequiredFunctionalBlocks) {

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

			if (ConditionReference.CheckTargetAltitudeDifference) {

				usedConditions++;

				if (_behavior.AutoPilot.Targeting.HasTarget() && _behavior.AutoPilot.InGravity()) {

					var planetPos = _behavior.AutoPilot.CurrentPlanet.Center();
					var targetCoreDist = _behavior.AutoPilot.Targeting.Target.Distance(planetPos);
					var myCoreDist = Vector3D.Distance(planetPos, _remoteControl.GetPosition());
					var difference = targetCoreDist - myCoreDist;

					if (difference >= ConditionReference.MinTargetAltitudeDifference && difference <= ConditionReference.MaxTargetAltitudeDifference)
						satisfiedConditions++;

				}
			
			}

			if (ConditionReference.CheckTargetDistance) {

				usedConditions++;

				if (_behavior.AutoPilot.Targeting.HasTarget()) {

					var dist = _behavior.AutoPilot.Targeting.Target.Distance(_remoteControl.GetPosition());

					if ((ConditionReference.MinTargetDistance == -1 || dist >= ConditionReference.MinTargetDistance) && (ConditionReference.MaxTargetDistance == -1 || dist <= ConditionReference.MaxTargetDistance))
						satisfiedConditions++;

				}

			}

			if (ConditionReference.CheckTargetSpeed)
			{

				usedConditions++;

				if (_behavior.AutoPilot.Targeting.HasTarget())
				{

					var speed = _behavior.AutoPilot.Targeting.Target.CurrentSpeed();

					if ((ConditionReference.MinTargetSpeed == -1 || speed >= ConditionReference.MinTargetSpeed) && (ConditionReference.MaxTargetSpeed == -1 || speed <= ConditionReference.MaxTargetSpeed))
						satisfiedConditions++;

				}

			}


			if (ConditionReference.CheckTargetAngleFromForward) {

				usedConditions++;

				if (_behavior.AutoPilot.Targeting.HasTarget()) {

					var dirToTarget = Vector3D.Normalize(_behavior.AutoPilot.Targeting.GetTargetCoords() - _remoteControl.GetPosition());
					var myForward = _behavior.AutoPilot.RefBlockMatrixRotation.Forward;
					var angle = VectorHelper.GetAngleBetweenDirections(dirToTarget, myForward);

					if ((ConditionReference.MinTargetAngle == -1 || angle >= ConditionReference.MinTargetAngle) && (ConditionReference.MaxTargetAngle == -1 || angle <= ConditionReference.MaxTargetAngle))
						satisfiedConditions++;

				}

			}

			if (ConditionReference.CheckIfTargetIsChasing) {

				usedConditions++;

				if (_behavior.AutoPilot.Targeting.HasTarget()) {

					var dirFromTarget = Vector3D.Normalize(_remoteControl.GetPosition() - _behavior.AutoPilot.Targeting.GetTargetCoords());
					var targetVelocity = Vector3D.Normalize(_behavior.AutoPilot.Targeting.Target.CurrentVelocity());
					
					if (targetVelocity.IsValid() && targetVelocity.Length() > 0) {

						var angle = VectorHelper.GetAngleBetweenDirections(dirFromTarget, targetVelocity);

						if ((ConditionReference.MinTargetChaseAngle == -1 || angle >= ConditionReference.MinTargetChaseAngle) && (ConditionReference.MaxTargetChaseAngle == -1 || angle <= ConditionReference.MaxTargetChaseAngle))
							satisfiedConditions++;
					
					}

				}

			}

			if (ConditionReference.CheckIfGridNameMatches) {

				usedConditions++;

				if(!string.IsNullOrWhiteSpace(_remoteControl.SlimBlock.CubeGrid.CustomName)){

					bool pass = false;

					foreach (var name in ConditionReference.GridNamesToCheck) {

						if (ConditionReference.AllowPartialGridNameMatches) {

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

			if (ConditionReference.UnderwaterCheck) {

				usedConditions++;

				if(_behavior.AutoPilot.CurrentPlanet != null)
					if (_behavior.AutoPilot.CurrentPlanet.UnderwaterAndDepthCheck(_remoteControl.GetPosition(), ConditionReference.IsUnderwater, ConditionReference.MinDistanceUnderwater, ConditionReference.MaxDistanceUnderwater))
						satisfiedConditions++;

			}

			if (ConditionReference.TargetUnderwaterCheck) {

				usedConditions++;

				if(_behavior.AutoPilot.Targeting.HasTarget() && _behavior.AutoPilot.CurrentPlanet != null)
					if (_behavior.AutoPilot.CurrentPlanet.UnderwaterAndDepthCheck(_behavior.AutoPilot.Targeting.TargetLastKnownCoords, ConditionReference.TargetIsUnderwater, ConditionReference.MinTargetDistanceUnderwater, ConditionReference.MaxTargetDistanceUnderwater))
						satisfiedConditions++;

			}

			if (ConditionReference.BehaviorSubclassCheck) {

				usedConditions++;

				if (ConditionReference.BehaviorSubclass.Contains(_behavior.ActiveBehavior.SubClass))
					satisfiedConditions++;

			}

			if (ConditionReference.BehaviorModeCheck) {

				usedConditions++;

				if(ConditionReference.CurrentBehaviorMode.Contains(_behavior.Mode))
					satisfiedConditions++;

			}

			if (ConditionReference.GravityCheck) {

				usedConditions++;
				var grav = PlanetManager.GetTotalNaturalGravity(_behavior.RemoteControl.GetPosition()).Length();

				if ((ConditionReference.MinGravity == -1000 || grav > ConditionReference.MinGravity) && (ConditionReference.MaxGravity == -1000 || grav < ConditionReference.MaxGravity)) {

					satisfiedConditions++;

				} else {

					BehaviorLogger.Write("Gravity Check Failed. Current Gravity: " + grav, BehaviorDebugEnum.Condition);

				}

			}

			if (ConditionReference.AltitudeCheck) {

				usedConditions++;

				if (_behavior.AutoPilot.CurrentPlanet != null) {

					if ((ConditionReference.MinAltitude == -1 || _behavior.AutoPilot.MyAltitude > ConditionReference.MinAltitude) && (ConditionReference.MaxAltitude == -1 || _behavior.AutoPilot.MyAltitude < ConditionReference.MaxAltitude)) {

						satisfiedConditions++;

					} else {

						BehaviorLogger.Write("Altitude Check Failed. Current Altitude: " + _behavior.AutoPilot.MyAltitude, BehaviorDebugEnum.Condition);

					}

				} else {

					BehaviorLogger.Write("Altitude Check Failed, Not On Planet", BehaviorDebugEnum.Condition);

				}
				
			}

			if (ConditionReference.TargetAltitudeCheck)
			{

				usedConditions++;

				if (_behavior.AutoPilot.Targeting.HasTarget() && _behavior.AutoPilot.CurrentPlanet != null)
				{
					var altitude = _behavior.AutoPilot.CurrentPlanet.AltitudeAtPosition(_behavior.AutoPilot.Targeting.TargetLastKnownCoords);


					if ((ConditionReference.MinTargetAltitude == -1 || altitude > ConditionReference.MinAltitude) && (ConditionReference.MaxAltitude == -1 || altitude < ConditionReference.MaxTargetAltitude))
					{

						satisfiedConditions++;

					}
					else
					{

						BehaviorLogger.Write("Altitude Check Failed. Current Altitude: " + _behavior.AutoPilot.MyAltitude, BehaviorDebugEnum.Condition);

					}

				}
				else
				{

					BehaviorLogger.Write("Altitude Check Failed, Not On Planet", BehaviorDebugEnum.Condition);

				}

			}








			if (ConditionReference.CheckHorizonAngle) {

				usedConditions++;

				if (_behavior.AutoPilot.InGravity() && _behavior.RemoteControl != null) {

					var result = Math.Abs(VectorHelper.GetAngleBetweenDirections(_behavior.AutoPilot.UpDirectionFromPlanet, _behavior.RemoteControl.WorldMatrix.Forward) - 90);

					if ((ConditionReference.MinHorizonAngle == -1 || _behavior.AutoPilot.MyAltitude > ConditionReference.MinHorizonAngle) && (ConditionReference.MaxHorizonAngle == -1 || _behavior.AutoPilot.MyAltitude < ConditionReference.MaxHorizonAngle))
						satisfiedConditions++;
				
				}

			}

			if (ConditionReference.CheckIfDamagerIsPlayer) {

				usedConditions++;

				if (_behavior.BehaviorSettings.LastDamagerEntity != 0) {

					if(FactionHelper.IsIdentityPlayer(DamageHelper.GetAttackOwnerId(_behavior.BehaviorSettings.LastDamagerEntity)))
						satisfiedConditions++;

				}
			
			}

			if (ConditionReference.CheckIfDamagerIsNpc) {

				usedConditions++;

				if (_behavior.BehaviorSettings.LastDamagerEntity != 0) {

					if (FactionHelper.IsIdentityNPC(DamageHelper.GetAttackOwnerId(_behavior.BehaviorSettings.LastDamagerEntity)))
						satisfiedConditions++;

				}

			}

			if (ConditionReference.CheckIfTargetIsPlayerOwned) {

				usedConditions++;

				if (_behavior.AutoPilot.Targeting.HasTarget()) {

					if(_behavior.AutoPilot.Targeting.Target.GetOwnerType().HasFlag(GridOwnershipEnum.PlayerMajority))
						satisfiedConditions++;

				}

			}

			if (ConditionReference.CheckIfTargetIsNpcOwned) {

				usedConditions++;

				if (_behavior.AutoPilot.Targeting.HasTarget()) {

					if (_behavior.AutoPilot.Targeting.Target.GetOwnerType().HasFlag(GridOwnershipEnum.NpcMajority))
						satisfiedConditions++;

				}

			}

			if (ConditionReference.CheckPlayerReputation) {

				usedConditions++;

				List<IMyPlayer> ListOfPlayersinRange = null;
				List<long> ListOfPlayerIds = new List<long>();

				int amountofplayersmatch = 0;

				if(command != null) {

					//If trigger is buttonpress for example, then use only that player to check wether the condition is satisfied
					if (command.PlayerIdentity != 0) {

						ListOfPlayerIds.Add(command.PlayerIdentity);

					}


					//If trigger is buttonpress for example, then use only that player to check wether the condition is satisfied
					if (command != null && command.PlayerIdentity != 0) {



					} else {

						var gridcoords = _behavior.RemoteControl.GetPosition();
						ListOfPlayersinRange = TargetHelper.GetPlayersWithinDistance(gridcoords, ConditionReference.MaxPlayerReputationDistanceCheck);
						foreach (IMyPlayer Player in ListOfPlayersinRange) {
							
							ListOfPlayerIds.Add(Player.IdentityId);

						}

					}
					
				} else {

					//If not, then check for all the players in range
					var gridcoords = _behavior.RemoteControl.GetPosition();
					ListOfPlayersinRange = TargetHelper.GetPlayersWithinDistance(gridcoords, ConditionReference.MaxPlayerReputationDistanceCheck);
					foreach (IMyPlayer Player in ListOfPlayersinRange){

						ListOfPlayerIds.Add(Player.IdentityId);

					}

				}

				int amountofplayers = ListOfPlayerIds.Count;

				if (ConditionReference.CheckReputationwithFaction.Count == ConditionReference.MaxPlayerReputation.Count && ConditionReference.MaxPlayerReputation.Count == ConditionReference.MinPlayerReputation.Count){
					
						foreach (long PlayerId in ListOfPlayerIds) {

						int TotalFactions = ConditionReference.CheckReputationwithFaction.Count;
						int SatisfiedFactions = 0;

						for (int i = 0; i < ConditionReference.CheckReputationwithFaction.Count; i++) {

							long FactionId;

							if (ConditionReference.CheckReputationwithFaction[i] == "{self}") {

								FactionId = _behavior.Owner.FactionId;

							} else {

								var customfaction = MyAPIGateway.Session.Factions.TryGetFactionByTag(ConditionReference.CheckReputationwithFaction[i]);
								FactionId = customfaction.FactionId;

							}


							if (FactionId != 0)	{

								var rep = MyAPIGateway.Session.Factions.GetReputationBetweenPlayerAndFaction(PlayerId, FactionId);

								if (rep >= ConditionReference.MinPlayerReputation[i] && rep <= ConditionReference.MaxPlayerReputation[i])
									SatisfiedFactions++;

							}

						}

						if (SatisfiedFactions == TotalFactions)
							amountofplayersmatch++;


					}

					if (ConditionReference.AllPlayersReputationMustMatch == true && amountofplayers == amountofplayersmatch){

						satisfiedConditions++;

					}

					if (ConditionReference.AllPlayersReputationMustMatch == false && amountofplayersmatch > 0){

						satisfiedConditions++;

					}

				}else{

					BehaviorLogger.Write("CheckReputationwithFaction, MaxPlayerReputation, and MinPlayerReputation do not match in count. Condition Failed", BehaviorDebugEnum.Condition);

				}

			}

			if (ConditionReference.IsTargetPlayer) {

				usedConditions++;

				if (_behavior.AutoPilot.Targeting.HasTarget() && _behavior.AutoPilot.Targeting.Target.GetEntityType() == EntityType.Player) {

					satisfiedConditions++;

				}

			}

			if (ConditionReference.IsTargetGrid) {

				usedConditions++;

				if (_behavior.AutoPilot.Targeting.HasTarget() && _behavior.AutoPilot.Targeting.Target.GetEntityType() != EntityType.Player) {

					satisfiedConditions++;

				}

			}



			if (ConditionReference.IsTargetStatic)
			{

				usedConditions++;

				if (_behavior.AutoPilot.Targeting.HasTarget() && _behavior.AutoPilot.Targeting.Target.IsStatic())
				{

					satisfiedConditions++;

				}

			}

			if (ConditionReference.HasTarget)
			{

				usedConditions++;

				if (_behavior.AutoPilot.Targeting.HasTarget())
				{

					satisfiedConditions++;

				}

			}

			if (ConditionReference.NoTarget)
			{

				usedConditions++;

				if (!_behavior.AutoPilot.Targeting.HasTarget())
				{

					satisfiedConditions++;

				}

			}




			if (ConditionReference.IsAttackerHostile) {

				usedConditions++;

				var rep = MyAPIGateway.Session.Factions.GetReputationBetweenPlayerAndFaction(DamageHelper.GetAttackOwnerId(_behavior.BehaviorSettings.LastDamagerEntity), MyAPIGateway.Session.Factions.TryGetPlayerFaction(_remoteControl.OwnerId)?.FactionId ?? 0);

				if(rep <= -501)
					satisfiedConditions++;

			}

			if (ConditionReference.IsAttackerNeutral) {

				usedConditions++;

				var rep = MyAPIGateway.Session.Factions.GetReputationBetweenPlayerAndFaction(DamageHelper.GetAttackOwnerId(_behavior.BehaviorSettings.LastDamagerEntity), MyAPIGateway.Session.Factions.TryGetPlayerFaction(_remoteControl.OwnerId)?.FactionId ?? 0);

				if (rep >= -500 && rep <= 500)
					satisfiedConditions++;

			}

			if (ConditionReference.IsAttackerFriendly) {

				usedConditions++;//

				var rep = MyAPIGateway.Session.Factions.GetReputationBetweenPlayerAndFaction(DamageHelper.GetAttackOwnerId(_behavior.BehaviorSettings.LastDamagerEntity), MyAPIGateway.Session.Factions.TryGetPlayerFaction(_remoteControl.OwnerId)?.FactionId ?? 0);

				if (rep >= 501)
					satisfiedConditions++;

			}

			if (ConditionReference.CheckCommandGridValue) {

				usedConditions++;

				if (command != null) {
				
					if(MathTools.CompareValues(command.GridValueScore, ConditionReference.CommandGridValue, ConditionReference.CheckCommandGridValueCompare))
						satisfiedConditions++;

				}
			
			}

			if (ConditionReference.CompareCommandGridValue) {

				usedConditions++;

				if (command != null) {

					var myScore = (_behavior.CurrentGrid?.TargetValue() ?? 0) * ConditionReference.CompareCommandGridValueSelfMultiplier;
					BehaviorLogger.Write(string.Format("Command Grid Value Compare: Self={0} // Other={1}", myScore, command.GridValueScore), BehaviorDebugEnum.Condition);

					if (MathTools.CompareValues(command.GridValueScore, myScore, ConditionReference.CompareCommandGridValueMode))
						satisfiedConditions++;

				} else {

					BehaviorLogger.Write("Command Was Null For CompareCommandGridValue", BehaviorDebugEnum.Condition);

				}

			}


			//Check
			if (ConditionReference.CheckThreatScore){

				usedConditions++;

				Vector3D Position = _behavior.CurrentGrid.GetPosition();

				if (ConditionReference.CheckThreatScoreFromTargetPosition && _behavior.AutoPilot.Targeting.HasTarget())
				{
					Position = _behavior.AutoPilot.Targeting.Target.GetPosition();
				}

				var ThreatScore = SpawnConditions.GetThreatLevel(ConditionReference.CheckThreatScoreRadius, ConditionReference.CheckThreatScoreIncludeOtherNpcOwners, Position, ConditionReference.CheckThreatScoreGridConfiguration, _behavior.RemoteControl.GetOwnerFactionTag());

				if (ThreatScore < (float)ConditionReference.CheckThreatScoreMinimum && (float)ConditionReference.CheckThreatScoreMinimum > 0 && ThreatScore > (float)ConditionReference.CheckThreatScoreMaximum && (float)ConditionReference.CheckThreatScoreMaximum > 0)
					satisfiedConditions++;
			}

			//CompareThreatScore
			if (ConditionReference.CompareThreatScore){

				usedConditions++;

				float ThreatScoreCompare = ConditionReference.CompareThreatScoreValue;
				Vector3D Position = _behavior.CurrentGrid.GetPosition();

				if (ConditionReference.CompareThreatScoreUseSelfValue)
                {
					ThreatScoreCompare = _behavior.CurrentGrid.ThreatScore * ConditionReference.CompareThreatScoreSelfValueMultiplier;
				}

                if (ConditionReference.CompareThreatScoreFromTargetPosition && _behavior.AutoPilot.Targeting.HasTarget())
                {
					Position = _behavior.AutoPilot.Targeting.Target.GetPosition();
                }

				var ThreatScore = SpawnConditions.GetThreatLevel(ConditionReference.CompareThreatScoreRadius, ConditionReference.CompareThreatScoreIncludeOtherNpcOwners, Position, ConditionReference.CompareThreatScoreGridConfiguration, _behavior.RemoteControl.GetOwnerFactionTag());


				if (MathTools.CompareValues(ThreatScore, ThreatScoreCompare, ConditionReference.CompareThreatScoreMode))
					satisfiedConditions++;
			}





			if (ConditionReference.CommandGravityCheck) {

				usedConditions++;

				if (command != null) {

					var match = PlanetManager.InGravity(command.Position) == PlanetManager.InGravity(_behavior.RemoteControl.GetPosition());

					if(match == ConditionReference.CommandGravityMatches)
						satisfiedConditions++;

				}

			}

			if (ConditionReference.PlayerIdentityMatches) {

				usedConditions++;

				if (command != null) {

					if (command.PlayerIdentity == _behavior.BehaviorSettings.SavedPlayerIdentityId)
						satisfiedConditions++;

				}

			}

			if (ConditionReference.CheckPlayerIdentitySandboxList)
			{

				usedConditions++;

				if (command != null)
				{
					var playerId = command.PlayerIdentity;
					List<long> PlayerIdentitySandboxList = new List<long>();

					//Get variable
					MyAPIGateway.Utilities.GetVariable<List<long>>(ConditionReference.PlayerIdentitySandboxListId, out PlayerIdentitySandboxList);

					if (!(ConditionReference.PlayerIdentityMatches ^ PlayerIdentitySandboxList.Contains(playerId)))
					{
						satisfiedConditions++;
					}
				}

			}

			if (ConditionReference.CheckForBlocksOfType && _behavior?.CurrentGrid?.AllTerminalBlocks != null) {

				usedConditions++;

				foreach (var block in _behavior.CurrentGrid.AllTerminalBlocks) {

					if (block.Working && ConditionReference.BlocksOfType.Contains(block.BlockType)) {

						satisfiedConditions++;
						break;
					
					}
				
				}
			
			}

			if (ConditionReference.CheckForSpawnConditions) {

				usedConditions++;
				var thisCondition = _behavior?.CurrentGrid?.Npc?.Conditions?.ProfileSubtypeId;

				if (thisCondition != null) {

					foreach (var spawnCondition in ConditionReference.RequiredSpawnConditions) {

						if (spawnCondition == thisCondition) {

							satisfiedConditions++;
							break;
						
						}

					}

				}

			}

			if (ConditionReference.CheckForPlanetaryLane) {

				usedConditions++;
				var laneResult = PlanetManager.IsPositionInsideLane(_behavior.RemoteControl.GetPosition()) == ConditionReference.PlanetaryLanePassValue;

				if (laneResult)
					satisfiedConditions++;

			}

			if (ConditionReference.CheckSufficientUpwardThrust) {

				usedConditions++;

				if (_behavior.AutoPilot.InGravity()) {

					//MyVisualScriptLogicProvider.ShowNotificationToAll("Max Grav For Thrust: " + _behavior.AutoPilot.CalculateMaxGravity().ToString(), 6000);

					if(_behavior.AutoPilot.CalculateMaxGravity() > PlanetManager.GetTotalNaturalGravity(_behavior.RemoteControl.GetPosition()).Length())
						satisfiedConditions++;

				} else {

					//MyVisualScriptLogicProvider.ShowNotificationToAll("Not In Grav??: " + _behavior.AutoPilot.CalculateMaxGravity().ToString(), 6000);
					satisfiedConditions++;

				}
				
			}

			if (ConditionReference.MatchAnyCondition == false) {

				bool result = satisfiedConditions >= usedConditions;
				BehaviorLogger.Write(ProfileSubtypeId + ": All Condition Satisfied: " + result.ToString(), BehaviorDebugEnum.Condition);
				BehaviorLogger.Write(string.Format("Used Conditions: {0} // Satisfied Conditions: {1}", usedConditions, satisfiedConditions), BehaviorDebugEnum.Condition);
				return !ConditionReference.UseFailCondition ? result : !result;

			} else {

				bool result = satisfiedConditions > 0;
				BehaviorLogger.Write(ProfileSubtypeId + ": Any Condition(s) Satisfied: " + result.ToString(), BehaviorDebugEnum.Condition);
				BehaviorLogger.Write(string.Format("Used Conditions: {0} // Satisfied Conditions: {1}", usedConditions, satisfiedConditions), BehaviorDebugEnum.Condition);
				return !ConditionReference.UseFailCondition ? result : !result;

			}

		}
		
		private void SetupWatchedBlocks() {

			BehaviorLogger.Write("Setting Up Required Block Watcher", BehaviorDebugEnum.Condition);
			_gotWatchedBlocks = true;
			_watchedAnyBlocks.Clear();
			_watchedAllBlocks.Clear();
			_watchedNoneBlocks.Clear();

			if (!ConditionReference.UseRequiredFunctionalBlocks) {

				BehaviorLogger.Write("Condition Not Using Required Functional Blocks", BehaviorDebugEnum.Condition);
				return;

			}

			_remoteControl.SlimBlock.CubeGrid.OnGridSplit += GridSplitHandler;

			var allBlocks = BlockCollectionHelper.GetBlocksOfType<IMyTerminalBlock>(_behavior.CurrentGrid); ;

			BehaviorLogger.Write("Monitoring Blocks Pre-Filtered Count: " + allBlocks.Count, BehaviorDebugEnum.Condition);

			foreach (var block in allBlocks) {

				if (block == null)
					continue;

				if (ConditionReference.RequiredAllFunctionalBlockNames.Contains(block.CustomName.Trim())) {

					BehaviorLogger.Write("Monitoring Required-All Block: " + block.CustomName, BehaviorDebugEnum.Condition);
					_watchedAllBlocks.Add(block);
					block.IsWorkingChanged += CheckAllBlocks;
					_watchingAllBlocks = true;

				}

				if (ConditionReference.RequiredAnyFunctionalBlockNames.Contains(block.CustomName.Trim())) {

					BehaviorLogger.Write("Monitoring Required-Any Block: " + block.CustomName, BehaviorDebugEnum.Condition);
					_watchedAnyBlocks.Add(block);
					block.IsWorkingChanged += CheckAnyBlocks;
					_watchingAnyBlocks = true;

				}

				if (ConditionReference.RequiredNoneFunctionalBlockNames.Contains(block.CustomName.Trim())) {

					BehaviorLogger.Write("Monitoring Required-None Block: " + block.CustomName, BehaviorDebugEnum.Condition);
					_watchedNoneBlocks.Add(block);
					block.IsWorkingChanged += CheckNoneBlocks;
					_watchingNoneBlocks = true;

				}

			}

			BehaviorLogger.Write("Watch All Blocks Count:  " + _watchedAllBlocks.Count, BehaviorDebugEnum.Condition);
			BehaviorLogger.Write("Watch Any Blocks Count:  " + _watchedAnyBlocks.Count, BehaviorDebugEnum.Condition);
			BehaviorLogger.Write("Watch None Blocks Count: " + _watchedNoneBlocks.Count, BehaviorDebugEnum.Condition);

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



		}

	}

}


