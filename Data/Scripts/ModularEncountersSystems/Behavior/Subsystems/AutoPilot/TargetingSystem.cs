using ModularEncountersSystems.API;
using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Tasks;
using ModularEncountersSystems.Zones;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Behavior.Subsystems {
	public class TargetingSystem {

		public IMyRemoteControl RemoteControl;
		private IBehavior _behavior;

		private IMyCubeGrid _nullGrid;

		public ITarget Target { 
			
			get { 

				if (OverrideTarget != null && OverrideTarget.ActiveEntity()) {

					return OverrideTarget;
				
				}

				return NormalTarget;
			}

			set {
			
				if (OverrideTarget != null && OverrideTarget.ActiveEntity()) {

					OverrideTarget = value;
					_behavior.BehaviorSettings.CurrentTargetEntityId = OverrideTarget.GetEntityId();
					return;
				
				}

				NormalTarget = value;
				_behavior.BehaviorSettings.CurrentTargetEntityId = NormalTarget?.GetEntityId() ?? 0;

			}
		}

		public TargetProfile Data { get { 

				if (OverrideTarget != null && OverrideTarget.ActiveEntity()) {

					//BehaviorLogger.Write("Data - Use Override", BehaviorDebugEnum.Target);
					return OverrideData;
				
				}

				//BehaviorLogger.Write("Data - Use Normal", BehaviorDebugEnum.Target);
				return NormalData;
			} 

		}

		public TargetProfile NormalData { get {

				if (_normalData?.ProfileSubtypeId == null || _normalData.ProfileSubtypeId != _behavior.BehaviorSettings.CurrentTargetProfile) {

					bool gotData = false;

					if (!string.IsNullOrWhiteSpace(_behavior.BehaviorSettings.CurrentTargetProfile)) {

						if (ProfileManager.TargetProfiles.TryGetValue(_behavior.BehaviorSettings.CurrentTargetProfile, out _normalData))
							gotData = true;

					}

					if (!gotData) {
					
						//TODO: Default a blank profile and raise an error in logger
					
					}

				}

				return _normalData;

			}

		}

		public TargetProfile OverrideData {	get {

				if (_overrideData?.ProfileSubtypeId == null || _overrideData.ProfileSubtypeId != _behavior.BehaviorSettings.OverrideTargetProfile) {

					bool gotData = false;

					if (!string.IsNullOrWhiteSpace(_behavior.BehaviorSettings.OverrideTargetProfile)) {

						if (ProfileManager.TargetProfiles.TryGetValue(_behavior.BehaviorSettings.OverrideTargetProfile, out _overrideData))
							gotData = true;

					}

					if (!gotData) {

						//TODO: Default a blank profile and raise an error in logger

					}

				}

				return _overrideData;

			}

		}

		public List<TargetFilterEnum> AllUniqueFilters {
			get {

				if (string.IsNullOrWhiteSpace(Data?.ProfileSubtypeId)) {

					_allUniqueFilters.Clear();
					_currentTargetDataName = "";
					return _allUniqueFilters;

				}

				if (Data.ProfileSubtypeId != _currentTargetDataName) {

					foreach (var filter in Data.MatchAllFilters) {

						if (!_allUniqueFilters.Contains(filter))
							_allUniqueFilters.Add(filter);

					}

					foreach (var filter in Data.MatchAnyFilters) {

						if (!_allUniqueFilters.Contains(filter))
							_allUniqueFilters.Add(filter);

					}

					foreach (var filter in Data.MatchNoneFilters) {

						if (!_allUniqueFilters.Contains(filter))
							_allUniqueFilters.Add(filter);

					}

				}

				return _allUniqueFilters;

			}
		
		}

		private string _currentTargetDataName;
		private List<TargetFilterEnum> _allUniqueFilters;

		public ITarget NormalTarget;
		internal TargetProfile _normalData;

		public ITarget OverrideTarget;
		internal TargetProfile _overrideData;

		public bool PreviousTargetCheckResult;
		public long PreviousTargetEntityId;

		public bool CurrentTargetCheckResult;
		public long CurrentTargetEntityId;

		public bool TargetAcquired;
		public bool TargetLost;
		public bool TargetSwitched;
		public bool TargetChanged;

		public DateTime LastAcquisitionTime;
		public DateTime LastRefreshTime;
		public DateTime LastEvaluationTime;
		public DateTime LastTargetLockOnTime;

		public Vector3D TargetLastKnownCoords;

		public bool ForceRefresh;
		public long ForceTargetEntityId;
		public IMyEntity ForceTargetEntity;
		public bool TargetAlreadyEvaluated;
		public bool UseNewTargetProfile;
		public bool ResetTimerOnProfileChange;
		public string NewTargetProfileName;

		public List<ITarget> PotentialLockOnTargets;
		public ITarget LockOnTarget;
		public bool LockOnActive;
		public bool LockOnOverride;
		public bool ManualLockOnRequest;
		public bool ApplyLockOn;
		public bool ClearLockOn;
		public GridDamageWatcher DamageWatcher;

		public TargetingSystem(IMyRemoteControl remoteControl = null) {

			RemoteControl = remoteControl;

			NormalTarget = null;
			_normalData = new TargetProfile();

			OverrideTarget = null;
			_overrideData = new TargetProfile();

			LastAcquisitionTime = MyAPIGateway.Session.GameDateTime;
			LastRefreshTime = MyAPIGateway.Session.GameDateTime;
			LastEvaluationTime = MyAPIGateway.Session.GameDateTime;
			LastTargetLockOnTime = MyAPIGateway.Session.GameDateTime;

			TargetLastKnownCoords = Vector3D.Zero;

			ForceRefresh = false;
			ForceTargetEntityId = 0;
			TargetAlreadyEvaluated = false;
			UseNewTargetProfile = false;
			ResetTimerOnProfileChange = false;
			NewTargetProfileName = "";

			PotentialLockOnTargets = new List<ITarget>();
			LockOnTarget = null;
			LockOnActive = false;
			LockOnOverride = false;
			ManualLockOnRequest = false;
			ApplyLockOn = false;
			ClearLockOn = false;

			_currentTargetDataName = "";
			_allUniqueFilters = new List<TargetFilterEnum>();

		}

		public void AcquireNewTarget() {

			
			bool skipTimerCheck = false;

			if (UseNewTargetProfile) {

				SetTargetProfile();
				LastAcquisitionTime = MyAPIGateway.Session.GameDateTime;

				if (!ResetTimerOnProfileChange) {

					skipTimerCheck = true;

				}

			}

			if (!Data.UseCustomTargeting)
				return;

			if (!skipTimerCheck && !ForceRefresh) {

				var timespan = MyAPIGateway.Session.GameDateTime - LastAcquisitionTime;

				if (timespan.TotalSeconds < Data.TimeUntilTargetAcquisition)
					return;

			}

			OverrideTarget = null;
			LastAcquisitionTime = MyAPIGateway.Session.GameDateTime;
			ForceRefresh = false;

			var data = ForceTargetEntityId == 0 ? NormalData : OverrideData;

			BehaviorLogger.Write(string.Format("Acquiring New Target From Profile: {0}", data.ProfileSubtypeId), BehaviorDebugEnum.TargetAcquisition);

			//Get Target Below
			var targetList = new List<ITarget>();

			bool targetIsOverride = false;

			if (ForceTargetEntityId == 0) {

				if (Data.Target == TargetTypeEnum.Player || Data.Target == TargetTypeEnum.PlayerAndBlock || Data.Target == TargetTypeEnum.PlayerAndGrid) {

					BehaviorLogger.Write(" - Acquiring Player Target", BehaviorDebugEnum.TargetAcquisition);
					AcquirePlayerTarget(targetList);

				}

				if (Data.Target == TargetTypeEnum.Block || Data.Target == TargetTypeEnum.PlayerAndBlock) {

					BehaviorLogger.Write(" - Acquiring Block Target", BehaviorDebugEnum.TargetAcquisition);
					AcquireBlockTarget(targetList);

				}

				if (Data.Target == TargetTypeEnum.Grid || Data.Target == TargetTypeEnum.PlayerAndGrid) {

					BehaviorLogger.Write(" - Acquiring Grid Target", BehaviorDebugEnum.TargetAcquisition);
					AcquireGridTarget(targetList);

				}

			} else {

				BehaviorLogger.Write(" - Acquiring Custom Target From EntityId: " + ForceTargetEntityId, BehaviorDebugEnum.TargetAcquisition);

				if (ForceTargetEntity != null) {

					var target = EntityEvaluator.GetTargetFromEntity(ForceTargetEntity);

					if (target != null) {

						targetIsOverride = true;
						targetList.Add(target);

					} else {

						BehaviorLogger.Write(" - Failed To Get Valid Target Entity From: " + ForceTargetEntityId, BehaviorDebugEnum.TargetAcquisition);

					}

				} else {

					BehaviorLogger.Write(" - Failed To Parse Entity From EntityId: " + ForceTargetEntityId, BehaviorDebugEnum.TargetAcquisition);

				}

				ForceTargetEntityId = 0;
				ForceTargetEntity = null;

			}

			//Run Filters On Target List
			BehaviorLogger.Write(string.Format(" - Running Evaluation On {0} Potential Targets", targetList.Count), BehaviorDebugEnum.TargetAcquisition);
			for (int i = targetList.Count - 1; i >= 0; i--) {

				targetList[i].RefreshSubGrids();

				if (!EvaluateTarget(targetList[i], data, true))
					targetList.RemoveAt(i);
			
			}

			//Filter Out Factions, if Applicable
			if (data.PrioritizeSpecifiedFactions && (data.MatchAllFilters.Contains(TargetFilterEnum.Faction) || data.MatchAnyFilters.Contains(TargetFilterEnum.Faction))) {

				BehaviorLogger.Write(" - Filtering Potential Preferred Faction Targets", BehaviorDebugEnum.TargetAcquisition);
				var factionPreferred = new List<ITarget>();

				for (int i = targetList.Count - 1; i >= 0; i--) {

					if (data.FactionTargets.Contains(targetList[i].FactionOwner()))
						factionPreferred.Add(targetList[i]);

				}

				if (factionPreferred.Count > 0) {

					targetList = factionPreferred;

				}

			}

			//Filter Out PlayerControlled Grids, if Applicable
			if (data.PrioritizePlayerControlled && (data.MatchAllFilters.Contains(TargetFilterEnum.PlayerControlled) || data.MatchAnyFilters.Contains(TargetFilterEnum.PlayerControlled))) {

				BehaviorLogger.Write(" - Filtering Potential Player Controlled Targets", BehaviorDebugEnum.TargetAcquisition);
				var playerControlled = new List<ITarget>();

				for (int i = targetList.Count - 1; i >= 0; i--) {

					if (targetList[i].PlayerControlled())
						playerControlled.Add(targetList[i]);

				}

				if (playerControlled.Count > 0) {

					targetList = playerControlled;

				}

			}

			BehaviorLogger.Write(string.Format(" - Getting Target From List Of {0} Based On {1} Sorting Rules", targetList.Count, data.GetTargetBy), BehaviorDebugEnum.TargetAcquisition);
			if (targetList.Count > 0) {

				var tempTarget = GetTargetFromSorting(targetList, data);

				if (tempTarget != null) {

					BehaviorLogger.Write(string.Format(" - Target Acquired: {0}", tempTarget.Name()), BehaviorDebugEnum.TargetAcquisition);

					if (targetIsOverride) {

						this.OverrideTarget = tempTarget;

					} else {

						this.NormalTarget = tempTarget;

					}

					
					this.LastRefreshTime = MyAPIGateway.Session.GameDateTime;
					this.LastEvaluationTime = MyAPIGateway.Session.GameDateTime;

				} else {

					BehaviorLogger.Write(string.Format(" - No Valid Target Could be Acquired."), BehaviorDebugEnum.TargetAcquisition);

				}
				

			}

			this.LastAcquisitionTime = MyAPIGateway.Session.GameDateTime;

		}

		public void AcquireBlockTarget(List<ITarget> result) {

			foreach (var grid in GridManager.Grids) {

				if (!grid.ActiveEntity())
					continue;

				if (grid.IsSameGrid(RemoteControl.SlimBlock.CubeGrid)) {

					continue;

				}

				if (grid.Distance(RemoteControl.GetPosition()) > Data.MaxDistance)
					continue;

				grid.GetBlocks(result, Data.BlockTargets);

			}

		}

		public void AcquireGridTarget(List<ITarget> result) {

			foreach (var grid in GridManager.Grids) {

				if (!grid.ActiveEntity())
					continue;

				if (grid.IsSameGrid(RemoteControl.SlimBlock.CubeGrid)) {

					continue;

				}

				if (grid.Distance(RemoteControl.GetPosition()) > Data.MaxDistance)
					continue;

				result.Add(grid);

			}

		}

		public void AcquirePlayerTarget(List<ITarget> result) {

			PlayerManager.RefreshAllPlayers(true);
			
			foreach (var player in PlayerManager.Players) {

				if (!player.ActiveEntity())
					continue;

				if (player.IsSameGrid(RemoteControl.SlimBlock.CubeGrid)) {

					continue;

				}

				if (player.Distance(RemoteControl.GetPosition()) > Data.MaxDistance)
					continue;

				result.Add(player);

			}

		}

		public void CheckForTarget() {

			//BehaviorLogger.Write("CheckForTarget - Start", BehaviorDebugEnum.Target);

			PreviousTargetCheckResult = CurrentTargetCheckResult;
			PreviousTargetEntityId = CurrentTargetEntityId;

			if (HasTarget() && Data.UseTargetRefresh) {

				var refreshDuration = MyAPIGateway.Session.GameDateTime - this.LastRefreshTime;

				if (refreshDuration.TotalSeconds > Data.TimeUntilNextRefresh) {

					ForceRefresh = true;
					this.LastRefreshTime = MyAPIGateway.Session.GameDateTime;

				}

			}

			bool evaluationDone = false;

			//BehaviorLogger.Write("CheckForTarget - Check For New Acquire", BehaviorDebugEnum.Target);

			if (!HasTarget() || ForceRefresh) {

				AcquireNewTarget();
				evaluationDone = true;

			}

			//BehaviorLogger.Write("CheckForTarget - Check For Evaluation", BehaviorDebugEnum.Target);

			if (!evaluationDone) {
			
				var evaluationDuration = MyAPIGateway.Session.GameDateTime - this.LastEvaluationTime;

				if (evaluationDuration.TotalSeconds > Data.TimeUntilNextEvaluation) {

					if (OverrideTarget != null) {

						var evalResult = EvaluateTarget(this.Target, this.Data);
						this.LastEvaluationTime = MyAPIGateway.Session.GameDateTime;

						if (!evalResult)
							this.Target = null;

					}

					if (OverrideTarget == null) {
					
						var evalResult = EvaluateTarget(this.Target, this.Data);
						this.LastEvaluationTime = MyAPIGateway.Session.GameDateTime;

						if (!evalResult)
							this.Target = null;

					}

				}

			}

			CurrentTargetCheckResult = HasTarget();
			CurrentTargetEntityId = Target != null ? Target.GetEntityId() : 0;

			if (CurrentTargetEntityId != PreviousTargetEntityId && !PreviousTargetCheckResult)
				TargetAcquired = true;

			if (PreviousTargetCheckResult && !CurrentTargetCheckResult)
				TargetLost = true;

			if (CurrentTargetEntityId != PreviousTargetEntityId && PreviousTargetCheckResult)
				TargetSwitched = true;

			if (CurrentTargetEntityId != PreviousTargetEntityId)
				TargetChanged = true;

			//BehaviorLogger.Write("CheckForTarget - Complete: " + RemoteControl.SlimBlock.CubeGrid.CustomName, BehaviorDebugEnum.Target);

		}

		public bool EvaluateTarget(ITarget target, TargetProfile data, bool skipExpensiveChecks = false) {

			if (target == null) {

				BehaviorLogger.Write("Target Is Null, Cannot Evaluate", BehaviorDebugEnum.TargetEvaluation);
				return false;

			}

			if (!target.ActiveEntity()) {

				BehaviorLogger.Write("Target Invalid, Cannot Evaluate", BehaviorDebugEnum.TargetEvaluation);
				return false;

			}

			BehaviorLogger.Write(string.Format(" - Evaluating Target: {0} using profile {1}", target.Name(), data.ProfileSubtypeId), BehaviorDebugEnum.TargetEvaluation);

			List<TargetFilterEnum> FilterHits = new List<TargetFilterEnum>();

			//Distance
			var distance = target.Distance(RemoteControl.GetPosition());
			var maxDistance = data.MaxDistance;

			if (data.MaxExistingTargetDistance > -1 && target == Target)
				maxDistance = data.MaxExistingTargetDistance;

			if (distance > maxDistance) {

				return false;
			
			}

			//Stealth
			if (AddonManager.StealthMod) {

				if (target.GetParentEntity() != null && ((uint)target.GetParentEntity().Flags & 0x20000000) > 0) {

					if (!AllUniqueFilters.Contains(TargetFilterEnum.IgnoreStealthDrive)) {

						if (distance > data.StealthDriveMinDistance) {

							var range = target.BroadcastRange(data.BroadcastOnlyAntenna);

							if (range < distance) {

								BehaviorLogger.Write(string.Format(" - Evaluated Stealth Detection: {0}", false), BehaviorDebugEnum.TargetEvaluation);
								return false;

							}

						}

					}

				}

			}

			//Altitude
			if (AllUniqueFilters.Contains(TargetFilterEnum.Altitude)) {

				var altitude = target.CurrentAltitude();

				if (altitude == -1000000 || (altitude >= data.MinAltitude && altitude <= data.MaxAltitude))
					FilterHits.Add(TargetFilterEnum.Altitude);

				BehaviorLogger.Write(string.Format(" - Evaluated Altitude: {0}", altitude), BehaviorDebugEnum.TargetEvaluation);

			}

			//Broadcasting
			if (AllUniqueFilters.Contains(TargetFilterEnum.Broadcasting)) {

				var range = target.BroadcastRange(data.BroadcastOnlyAntenna);
				
				if (range > distance || distance < data.NonBroadcastVisualRange)
					FilterHits.Add(TargetFilterEnum.Broadcasting);

				BehaviorLogger.Write(string.Format(" - Evaluated Broadcast Range vs Distance: {0} / {1}", range, distance), BehaviorDebugEnum.TargetEvaluation);

			}

			//Faction
			if (AllUniqueFilters.Contains(TargetFilterEnum.Faction)) {

				var faction = target.FactionOwner() ?? "";

				if (data.PrioritizeSpecifiedFactions || data.FactionTargets.Contains(faction))
					FilterHits.Add(TargetFilterEnum.Faction);

				BehaviorLogger.Write(string.Format(" - Evaluated Faction: {0}", faction), BehaviorDebugEnum.TargetEvaluation);

			}

			//Gravity
			if (AllUniqueFilters.Contains(TargetFilterEnum.Gravity)) {

				var gravity = target.CurrentGravity();

				if (gravity >= data.MinGravity && gravity <= data.MaxGravity)
					FilterHits.Add(TargetFilterEnum.Gravity);

				BehaviorLogger.Write(string.Format(" - Evaluated Gravity: {0}", gravity), BehaviorDebugEnum.TargetEvaluation);

			}

			//LineOfSight
			if (!skipExpensiveChecks && AllUniqueFilters.Contains(TargetFilterEnum.LineOfSight) && _behavior.AutoPilot.Collision.TargetResult.HasTarget()) {
				
				bool targetMatch = (target.GetParentEntity().EntityId == _behavior.AutoPilot.Collision.TargetResult.GetCollisionEntity().EntityId);

				if (targetMatch)
					FilterHits.Add(TargetFilterEnum.MovementScore);

			}

			//MovementScore
			if (AllUniqueFilters.Contains(TargetFilterEnum.MovementScore)) {

				if (distance < data.MaxMovementDetectableDistance || data.MaxMovementDetectableDistance < 0) {

					var score = target.MovementScore();

					if ((data.MinMovementScore == -1 || score >= data.MinMovementScore) && (data.MaxMovementScore == -1 || score <= data.MaxMovementScore))
						FilterHits.Add(TargetFilterEnum.MovementScore);

				}

			}

			//Name
			if (AllUniqueFilters.Contains(TargetFilterEnum.Name)) {

				var name = target.Name();
				string successName = "N/A";

				foreach (var allowedName in data.Names) {

					if (string.IsNullOrWhiteSpace(allowedName))
						continue;

					if (data.UsePartialNameMatching) {

						if (name.Contains(allowedName)) {

							successName = allowedName;
							break;

						}

					} else {

						if (name == allowedName) {

							successName = allowedName;
							break;

						}
					
					}
				
				}

				if(successName != "N/A")
					FilterHits.Add(TargetFilterEnum.Name);

				BehaviorLogger.Write(string.Format(" - Evaluated Name: {0} // {1}", name, successName), BehaviorDebugEnum.TargetEvaluation);

			}

			//OutsideOfSafezone
			if (AllUniqueFilters.Contains(TargetFilterEnum.OutsideOfSafezone)) {

				bool inZone = target.InSafeZone();

				if (!inZone)
					FilterHits.Add(TargetFilterEnum.OutsideOfSafezone);

				BehaviorLogger.Write(string.Format(" - Evaluated Outside Safezone: {0}", !inZone), BehaviorDebugEnum.TargetEvaluation);

			}

			//Owner
			if (AllUniqueFilters.Contains(TargetFilterEnum.Owner)) {

				var owners = target.OwnerTypes(data.OnlyGetFromEntityOwner, data.GetFromMinorityGridOwners);
				bool gotRelation = false;

				var values = Enum.GetValues(typeof(OwnerTypeEnum)).Cast<OwnerTypeEnum>();
				
				foreach (var ownerType in values) {

					if (ownerType == OwnerTypeEnum.None)
						continue;

					BehaviorLogger.Write("Comparing: " + ownerType, BehaviorDebugEnum.Dev);
					BehaviorLogger.Write("Owners: " + owners.HasFlag(ownerType) + " // Data: " + data.Owners.HasFlag(ownerType), BehaviorDebugEnum.Dev);
					if (owners.HasFlag(ownerType) && data.Owners.HasFlag(ownerType)) {

						gotRelation = true;
						break;

					}

				}

				if (gotRelation)
					FilterHits.Add(TargetFilterEnum.Owner);

				BehaviorLogger.Write(string.Format(" - Evaluated Owners: Required: {0}", data.Owners.ToString()), BehaviorDebugEnum.TargetEvaluation);
				BehaviorLogger.Write(string.Format(" - Evaluated Owners: Found: {0}", owners.ToString()), BehaviorDebugEnum.TargetEvaluation);
				BehaviorLogger.Write(string.Format(" - Evaluated Target Owners: {0} / Passed: {1}", owners.ToString(), gotRelation), BehaviorDebugEnum.TargetEvaluation);
				
			}

			//PlayerControlled
			if (AllUniqueFilters.Contains(TargetFilterEnum.PlayerControlled)) {

				var controlled = target.PlayerControlled();

				if (data.PrioritizePlayerControlled || controlled)
					FilterHits.Add(TargetFilterEnum.PlayerControlled);

				BehaviorLogger.Write(string.Format(" - Evaluated Player Controlled: {0}", controlled), BehaviorDebugEnum.TargetEvaluation);

			}

			//PlayerKnownLocation
			if (AllUniqueFilters.Contains(TargetFilterEnum.PlayerKnownLocation)) {

				bool inKnownLocation = false;

				if (KnownPlayerLocationManager.IsPositionInKnownPlayerLocation(target.GetPosition(), true, string.IsNullOrWhiteSpace(data.PlayerKnownLocationFactionOverride) ? _behavior.Owner.Faction?.Tag : data.PlayerKnownLocationFactionOverride)) {

					FilterHits.Add(TargetFilterEnum.PlayerKnownLocation);
					inKnownLocation = true;

				}

				BehaviorLogger.Write(string.Format(" - Evaluated Player Known Location: {0}", inKnownLocation), BehaviorDebugEnum.TargetEvaluation);

			}

			//Powered
			if (AllUniqueFilters.Contains(TargetFilterEnum.Powered)) {

				bool powered = target.IsPowered();

				if (powered)
					FilterHits.Add(TargetFilterEnum.Powered);

				BehaviorLogger.Write(string.Format(" - Evaluated Power: {0}", powered), BehaviorDebugEnum.TargetEvaluation);

			}

			//Relation
			if (AllUniqueFilters.Contains(TargetFilterEnum.Relation)) {

				var relations = target.RelationTypes(RemoteControl.OwnerId, data.OnlyGetFromEntityOwner, data.GetFromMinorityGridOwners);
				bool gotRelation = false;

				var values = Enum.GetValues(typeof(RelationTypeEnum)).Cast<RelationTypeEnum>();

				foreach (var relationType in values) {

					if (relationType == RelationTypeEnum.None)
						continue;

					if (relations.HasFlag(relationType) && data.Relations.HasFlag(relationType)) {

						gotRelation = true;
						break;

					}

				}

				if (gotRelation)
					FilterHits.Add(TargetFilterEnum.Relation);

				BehaviorLogger.Write(string.Format(" - Evaluated Relations: Required: {0}", data.Relations.ToString()), BehaviorDebugEnum.TargetEvaluation);
				BehaviorLogger.Write(string.Format(" - Evaluated Relations: Found: {0}", relations.ToString()), BehaviorDebugEnum.TargetEvaluation);
				BehaviorLogger.Write(string.Format(" - Evaluated Relations: {0} / Passed: {1}", relations.ToString(), gotRelation), BehaviorDebugEnum.TargetEvaluation);

			}

			//Shielded
			if (AllUniqueFilters.Contains(TargetFilterEnum.Shielded)) {

				bool shielded = target.ProtectedByShields();

				if (shielded)
					FilterHits.Add(TargetFilterEnum.Shielded);

				BehaviorLogger.Write(string.Format(" - Evaluated Shields: {0}", shielded), BehaviorDebugEnum.TargetEvaluation);

			}

			//Speed
			if (AllUniqueFilters.Contains(TargetFilterEnum.Speed)) {

				var speed = target.CurrentSpeed();

				if ((data.MinSpeed < 0 || speed >= data.MinSpeed) && (data.MaxSpeed < 0 || speed <= data.MaxSpeed))
					FilterHits.Add(TargetFilterEnum.Speed);

				BehaviorLogger.Write(string.Format(" - Evaluated Speed: {0}", speed), BehaviorDebugEnum.TargetEvaluation);

			}

			//Static
			if (data.IsStatic != CheckEnum.Ignore && AllUniqueFilters.Contains(TargetFilterEnum.Static)) {

				var staticGrid = target.IsStatic();

				if ((staticGrid && data.IsStatic == CheckEnum.Yes) || (!staticGrid && data.IsStatic == CheckEnum.No))
					FilterHits.Add(TargetFilterEnum.Static);

				BehaviorLogger.Write(string.Format(" - Evaluated Static Grid: {0}", staticGrid), BehaviorDebugEnum.TargetEvaluation);

			}

			//TargetValue
			if (AllUniqueFilters.Contains(TargetFilterEnum.TargetValue)) {

				var targetValue = target.TargetValue();

				if ((data.MinTargetValue == -1 || targetValue >= data.MinTargetValue) && (data.MaxTargetValue == -1 || targetValue <= data.MaxTargetValue))
					FilterHits.Add(TargetFilterEnum.TargetValue);

				BehaviorLogger.Write(string.Format(" - Evaluated Target Value: {0}", targetValue), BehaviorDebugEnum.TargetEvaluation);

			}

			//Underwater
			if (AllUniqueFilters.Contains(TargetFilterEnum.Underwater)) {

				bool result = false;

				if (_behavior.AutoPilot.CurrentPlanet != null && !_behavior.AutoPilot.CurrentPlanet.Planet.Closed)
					result = _behavior.AutoPilot.CurrentPlanet.UnderwaterAndDepthCheck(target.GetPosition(), true, Data.MinUnderWaterDepth, Data.MaxUnderWaterDepth);

				if (result)
					FilterHits.Add(TargetFilterEnum.Underwater);

				BehaviorLogger.Write(string.Format(" - Evaluated Underwater: {0}", result), BehaviorDebugEnum.TargetEvaluation);

			}

			//AirDensity
			if (AllUniqueFilters.Contains(TargetFilterEnum.AirDensity)) {

				bool result = false;

				if (_behavior.AutoPilot.CurrentPlanet != null && !_behavior.AutoPilot.CurrentPlanet.Planet.Closed) {

					if ((Data.MinAirDensity > -1 || _behavior.AutoPilot.AirDensity >= Data.MinAirDensity) && (Data.MaxAirDensity > -1 || _behavior.AutoPilot.AirDensity <= Data.MaxAirDensity))
						result = true;
				
				} else
					result = Data.MinAirDensity <= 0 && Data.MaxAirDensity <= 0;

				if (result)
					FilterHits.Add(TargetFilterEnum.Underwater);

				BehaviorLogger.Write(string.Format(" - Evaluated AirDensity: {0}", result), BehaviorDebugEnum.TargetEvaluation);

			}

			//GravityThrust
			if (AllUniqueFilters.Contains(TargetFilterEnum.GravityThrust)) {

				bool result = false;

				if (PlanetManager.InGravity(target.GetPosition())) {

					double gravityAtPosition = PlanetManager.GetTotalNaturalGravity(target.GetPosition()).Length();
					double gridGravity = 0;

					if (PlanetManager.AirDensityAtPosition(target.GetPosition()) >= 0.7f) {

						gridGravity = _behavior.AutoPilot.CalculateMaxGravity();

					} else {

						gridGravity = _behavior.AutoPilot.CalculateMaxGravity(true);

					}

					if (gridGravity >= gravityAtPosition)
						result = true;

				} else {

					result = true;
				
				}

				if (result)
					FilterHits.Add(TargetFilterEnum.Underwater);

				BehaviorLogger.Write(string.Format(" - Evaluated GravityThrust: {0}", result), BehaviorDebugEnum.TargetEvaluation);

			}

			//Any Conditions Check
			bool anyConditionPassed = false;

			if (data.MatchAnyFilters.Count > 0) {

				foreach (var filter in data.MatchAnyFilters) {

					if (FilterHits.Contains(filter)) {

						anyConditionPassed = true;
						break;

					}
				
				}

			} else {

				anyConditionPassed = true;

			}

			if (!anyConditionPassed) {

				BehaviorLogger.Write(" - Evaluation Condition -Any- Failed", BehaviorDebugEnum.TargetEvaluation);
				return false;

			}
				

			//All Condition Checks
			foreach (var filter in data.MatchAllFilters) {

				if (!FilterHits.Contains(filter)) {

					BehaviorLogger.Write(" - Evaluation Condition -All- Failed", BehaviorDebugEnum.TargetEvaluation);
					return false;

				}

			}

			//None Condition Checks
			foreach (var filter in data.MatchNoneFilters) {

				if (FilterHits.Contains(filter)) {

					BehaviorLogger.Write(" - Evaluation Condition -None- Failed", BehaviorDebugEnum.TargetEvaluation);
					return false;

				}

			}

			BehaviorLogger.Write(" - Evaluation Passed", BehaviorDebugEnum.TargetEvaluation);
			TargetLastKnownCoords = target.GetPosition();
			return true;
		
		}

		public Vector3D GetTargetCoords() {

			if (HasTarget()) {

				TargetLastKnownCoords = Target.GetPosition();
				return TargetLastKnownCoords;

			}

			if (Data.UseTargetLastKnownPosition) {

				return TargetLastKnownCoords;

			}

			return Vector3D.Zero;

		}

		public ITarget GetTargetFromSorting(List<ITarget> targets, TargetProfile data) {

			//List Empty, null soup for you!
			if (targets.Count == 0)
				return null;

			//Only 1 thing in list, therefore you get the 1 thing
			if (targets.Count == 1)
				return targets[0];

			//Random - may RNGesus be generous
			if (data.GetTargetBy == TargetSortEnum.Random) {

				return targets[MathTools.RandomBetween(0, targets.Count)];
			
			}

			//Closest - You can probably smell them from where you are
			if (data.GetTargetBy == TargetSortEnum.ClosestDistance) {

				int index = -1;
				double dist = -1;

				for (int i = 0; i < targets.Count; i++) {

					var thisDist = targets[i].Distance(RemoteControl.GetPosition());

					if (index == -1 || thisDist < dist) {

						dist = thisDist;
						index = i;

					}
				
				}

				return targets[index];

			}

			//Furthest - You might need to squint
			if (data.GetTargetBy == TargetSortEnum.FurthestDistance) {

				int index = -1;
				double dist = -1;

				for (int i = 0; i < targets.Count; i++) {

					var thisDist = targets[i].Distance(RemoteControl.GetPosition());

					if (index == -1 || thisDist > dist) {

						dist = thisDist;
						index = i;

					}

				}

				return targets[index];

			}

			//Toughest - Make an example out of the big guy
			if (data.GetTargetBy == TargetSortEnum.HighestTargetValue) {

				int index = -1;
				float targetValue = -1;

				for (int i = 0; i < targets.Count; i++) {

					var thisValue = targets[i].TargetValue();

					if (index == -1 || thisValue > targetValue) {

						targetValue = thisValue;
						index = i;

					}

				}

				return targets[index];

			}

			//Weakest - A bully is you.
			if (data.GetTargetBy == TargetSortEnum.LowestTargetValue) {

				int index = -1;
				float targetValue = -1;

				for (int i = 0; i < targets.Count; i++) {

					var thisValue = targets[i].TargetValue();

					if (index == -1 || thisValue < targetValue) {

						targetValue = thisValue;
						index = i;

					}

				}

				return targets[index];

			}

			return null;
		
		}

		public void PrepareTargetLockOn() {

			if (!Data.UseVanillaTargetLocking && !ManualLockOnRequest) {

				if (LockOnActive) {

					ClearLockOn = true;
				
				}

				return;
			
			}

			if (Data.ActivateTargetLockAfterPlayerDamage) {

				if (_behavior?.CurrentGrid?.Npc == null)
					return;

				if (!_behavior.CurrentGrid.Npc.AppliedAttributes.ReceivedPlayerDamage) {

					if (DamageWatcher == null || !DamageWatcher.Valid())
						DamageWatcher = new GridDamageWatcher(_behavior.CurrentGrid);

					return;
				
				}
			
			}

			var time = MyAPIGateway.Session.GameDateTime - LastTargetLockOnTime;

			if (time.TotalSeconds < Data.TimeUntilTargetLockingRefresh && !ManualLockOnRequest)
				return;

			LastTargetLockOnTime = MyAPIGateway.Session.GameDateTime;
			PotentialLockOnTargets.Clear();

			if (LockOnOverride) {

				if (TargetEligibleForLockOn(LockOnTarget)) {

					return;

				} else {

					LockOnOverride = false;
					LockOnTarget = null;

				}

			}

			for (int i = GridManager.Grids.Count - 1; i >= 0; i--) {

				var grid = GridManager.GetSafeGridFromIndex(i);

				if (grid == null)
					continue;

				if (!TargetEligibleForLockOn(grid))
					continue;

				if (PotentialLockOnTargets.Contains(grid))
					continue;

				PotentialLockOnTargets.Add(grid);
			}

			if (PotentialLockOnTargets.Count == 0) {

				LockOnTarget = null;
				ClearLockOn = true;
				return;
			
			}

			ApplyLockOn = true;

			//Select Target
			if (Data.GetTargetLockBy == TargetSortEnum.Random) {

				LockOnTarget = PotentialLockOnTargets[MathTools.RandomBetween(0, PotentialLockOnTargets.Count)];

			}

			if (Data.GetTargetLockBy == TargetSortEnum.ClosestDistance) {

				double closestDist = -1;

				foreach (var target in PotentialLockOnTargets) {

					var dist = target.Distance(RemoteControl.GetPosition());

					if (closestDist == -1 || dist < closestDist) {

						closestDist = dist;
						LockOnTarget = target;
					
					}
				
				}

			}

			if (Data.GetTargetLockBy == TargetSortEnum.FurthestDistance) {

				double closestDist = -1;

				foreach (var target in PotentialLockOnTargets) {

					var dist = target.Distance(RemoteControl.GetPosition());

					if (closestDist == -1 || dist > closestDist) {

						closestDist = dist;
						LockOnTarget = target;

					}

				}

			}

			if (Data.GetTargetLockBy == TargetSortEnum.HighestTargetValue) {

				float bestThreat = -1;

				foreach (var target in PotentialLockOnTargets) {

					var threat = target.TargetValue();

					if (bestThreat == -1 || threat > bestThreat) {

						bestThreat = threat;
						LockOnTarget = target;

					}

				}

			}

			if (Data.GetTargetLockBy == TargetSortEnum.LowestTargetValue) {

				float bestThreat = -1;

				foreach (var target in PotentialLockOnTargets) {

					var threat = target.TargetValue();

					if (bestThreat == -1 || threat < bestThreat) {

						bestThreat = threat;
						LockOnTarget = target;

					}

				}

			}

			//TODO: Prepare Weapons in WeaponSystem - Probably set some values and have WeaponSystem prepare during its regular run
			_behavior.AutoPilot.Weapons.PendingTargetLockOn = true;

		}

		public void ProcessTargetLockOn() {

			if (ClearLockOn) {

				RemoteControl.SetLockedTarget(_nullGrid);
				ClearLockOn = false;

			}

			if (ApplyLockOn) {

				if (LockOnTarget.ActiveEntity()) {

					RemoteControl.SetLockedTarget(LockOnTarget.GetParentEntity() as IMyCubeGrid);

				} else {

					LockOnTarget = null;

				}

				ApplyLockOn = false;

			}
		
		}

		public bool TargetEligibleForLockOn(ITarget target, double overrideDistance = -1) {

			if (target == null || !target.ActiveEntity() || target.GetEntityType() != EntityType.Grid)
				return false;

			if (target.Distance(RemoteControl.GetPosition()) > (overrideDistance > -1 ? overrideDistance : Data.MaxTargetLockingDistance))
				return false;

			if (!target.RelationTypes(RemoteControl.OwnerId, true).HasFlag(RelationTypeEnum.Enemy))
				return true;

			return true;

		}

		public bool HasTarget() {

			if (Target == null)
				return false;

			return Data.UseCustomTargeting && Target.ActiveEntity();

		}

		public void InitTags() {

			if (!string.IsNullOrWhiteSpace(this.RemoteControl.CustomData)) {

				var descSplit = this.RemoteControl.CustomData.Split('\n');

				foreach (var tag in descSplit) {

					//TargetData
					if (tag.Contains("[TargetData:") == true) {

						var tempValue = "";

						if (!string.IsNullOrWhiteSpace(_behavior.BehaviorSettings.CurrentTargetProfile))
							tempValue = _behavior.BehaviorSettings.CurrentTargetProfile;
						else
							TagParse.TagStringCheck(tag, ref tempValue);

						if (!string.IsNullOrWhiteSpace(tempValue)) {

							TargetProfile profile = null;

							if (ProfileManager.TargetProfiles.TryGetValue(tempValue, out profile) == true) {

								_normalData = profile;
								_behavior.BehaviorSettings.CurrentTargetProfile = tempValue;

							} else {

								ProfileManager.ReportProfileError(tempValue, "Target Profile Not Found in Profile Manager");

							}

						} else {

							ProfileManager.ReportProfileError(tempValue, "Target Data Provided Was Null or Blank");

						}

					}

					//OverrideTargetData
					if (tag.Contains("[OverrideTargetData:") == true) {

						var tempValue = "";

						if (!string.IsNullOrWhiteSpace(_behavior.BehaviorSettings.OverrideTargetProfile))
							tempValue = _behavior.BehaviorSettings.OverrideTargetProfile;
						else
							TagParse.TagStringCheck(tag, ref tempValue);

						if (string.IsNullOrWhiteSpace(tempValue) == false) {

							TargetProfile profile = null;

							if (ProfileManager.TargetProfiles.TryGetValue(tempValue, out profile) == true) {

								_overrideData = profile;
								_behavior.BehaviorSettings.OverrideTargetProfile = tempValue;
								BehaviorLogger.Write(profile.ProfileSubtypeId + " Override Target Profile Loaded", BehaviorDebugEnum.BehaviorSetup);

							} 

						} else {

							ProfileManager.ReportProfileError(tempValue, "Override Target Data Provided Was Null or Blank");

						}

					}

				}

			}

		}

		public void SetTargetProfile(bool setOverride = false) {

			UseNewTargetProfile = false;

			if (!ProfileManager.TargetProfiles.ContainsKey(NewTargetProfileName))
				return;

			if (setOverride) {

				_behavior.BehaviorSettings.OverrideTargetProfile = NewTargetProfileName;

			} else {

				_behavior.BehaviorSettings.CurrentTargetProfile = NewTargetProfileName;
			
			}

		}

		public void SetupReferences(IBehavior behavior) {

			_behavior = behavior;
		
		}

		public string AllTargetsString() {

			var sb = new StringBuilder();

			foreach (var filter in Data.MatchAllFilters) {

				sb.Append(filter.ToString()).Append(", ");
			
			}

			return sb.ToString();
		
		}

		public string AnyTargetsString() {

			var sb = new StringBuilder();

			foreach (var filter in Data.MatchAnyFilters) {

				sb.Append(filter.ToString()).Append(", ");

			}

			return sb.ToString();

		}

		public string NoneTargetsString() {

			var sb = new StringBuilder();

			foreach (var filter in Data.MatchNoneFilters) {

				sb.Append(filter.ToString()).Append(", ");

			}

			return sb.ToString();

		}

	}

}
