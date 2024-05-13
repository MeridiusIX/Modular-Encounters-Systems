using ModularEncountersSystems.API;
using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Files;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Watchers;
using Sandbox.Game;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Game.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Behavior.Subsystems.Trigger {

	public partial class TriggerSystem {

		public IMyRemoteControl RemoteControl;
		public List<IMyRadioAntenna> AntennaList = new List<IMyRadioAntenna>();
		public List<IMyLargeTurretBase> TurretList = new List<IMyLargeTurretBase>();

		private IBehavior _behavior;

		private AutoPilotSystem _autopilot;
		private BroadcastSystem _broadcast;
		private DespawnSystem _despawn;
		private GridSystem _extras;
		private OwnerSystem _owner;
		private StoredSettings _settings;

		public List<TriggerProfile> Triggers;
		public List<TriggerProfile> DamageTriggers;
		public List<TriggerProfile> CommandTriggers;
		public List<TriggerProfile> CompromisedTriggers;
		public List<string> ExistingTriggers;

		public List<DialogueBank> _dialogueBanks;

		public bool RemoteControlCompromised;

		public bool TimedTriggersProcessed;

		public bool CommandListenerRegistered;
		public bool DamageHandlerRegistered;
		public MyDamageInformation DamageInfo;
		public bool PendingDamage;

		public bool PaymentSuccessTriggered;
		public bool PaymentFailureTriggered;

		public DateTime LastTriggerRun;

		public Action OnComplete;
		public Action<IMyCubeGrid, string> DespawnFromMES;

		public TriggerSystem(IMyRemoteControl remoteControl) {

			RemoteControl = null;
			AntennaList = new List<IMyRadioAntenna>();
			TurretList = new List<IMyLargeTurretBase>();

			Triggers = new List<TriggerProfile>();
			DamageTriggers = new List<TriggerProfile>();
			CommandTriggers = new List<TriggerProfile>();
			CompromisedTriggers = new List<TriggerProfile>();
			ExistingTriggers = new List<string>();

			_dialogueBanks = new List<DialogueBank>();
			RemoteControlCompromised = false;

			TimedTriggersProcessed = false;

			CommandListenerRegistered = false;

			LastTriggerRun = MyAPIGateway.Session.GameDateTime;

			Setup(remoteControl);

		}

		public void ProcessTriggerWatchers() {

			var timeDifference = MyAPIGateway.Session.GameDateTime - LastTriggerRun;

			if (timeDifference.TotalMilliseconds < 500) {

				//BehaviorLogger.Write("Triggers Not Ready (total ms elapsed: "+ timeDifference.TotalMilliseconds.ToString() + "), Handing Off to Next Action", BehaviorDebugEnum.Dev);
				//OnComplete?.Invoke();
				return;

			}

			//BehaviorLogger.Write("Checking Triggers", BehaviorDebugEnum.Dev);
			LastTriggerRun = MyAPIGateway.Session.GameDateTime;

			for (int i = 0; i < Triggers.Count; i++) {

				var trigger = Triggers[i];

				if (!trigger.UseTrigger)
					continue;

				//Timer
				if (trigger.Type == "Timer") {

					trigger.ActivateTrigger();
					continue;

				}

				//PlayerNear
				if (trigger.Type == "PlayerNear") {

					if (trigger.UsePlayerFilterProfile)
						trigger.ActivateTrigger(CheckPlayerNearPlayerCondition);
					else
						trigger.ActivateTrigger(CheckPlayerNear);

					continue;

				}

				//PlayerFar
				if (trigger.Type == "PlayerFar") {

					if (trigger.UsePlayerFilterProfile) 
						trigger.ActivateTrigger(CheckPlayerFarPlayerCondition);
					else
						trigger.ActivateTrigger(CheckPlayerFar);

					continue;

				}

				//TargetNear
				if (trigger.Type == "TargetNear") {

					trigger.ActivateTrigger(CheckTargetNear);
					continue;

				}

				//TargetFar
				if (trigger.Type == "TargetFar") {

					trigger.ActivateTrigger(CheckTargetFar);
					continue;

				}

				//DespawnNear
				if (trigger.Type == "DespawnNear") {

					trigger.ActivateTrigger(CheckDespawnNear);
					continue;

				}

				//DespawnFar
				if (trigger.Type == "DespawnFar") {

					//BehaviorLogger.Write("Checking DespawnFar Trigger: " + trigger.ProfileSubtypeId, BehaviorDebugEnum.Trigger);
					if (trigger.UseTrigger == true) {

						if (_behavior.BehaviorSettings.DespawnCoords != Vector3D.Zero && Vector3D.Distance(RemoteControl.GetPosition(), _behavior.BehaviorSettings.DespawnCoords) > trigger.TargetDistance) {

							trigger.ActivateTrigger(CheckDespawnFar);

						}

					}

					continue;

				}

				//TurretTarget
				if (trigger.Type == "TurretTarget") {

					trigger.ActivateTrigger(CheckTurretTarget);
					continue;

				}

				//NoWeapon
				if (trigger.Type == "NoWeapon") {

					trigger.ActivateTrigger(CheckNoWeapon);
					continue;

				}

				//NoTarget
				if (trigger.Type == "NoTarget") {

					trigger.ActivateTrigger(CheckNoTarget);
					continue;

				}

				//HasTarget
				if (trigger.Type == "HasTarget") {

					trigger.ActivateTrigger(CheckHasTarget);
					continue;

				}

				//AcquiredTarget
				if (trigger.Type == "AcquiredTarget") {

					trigger.ActivateTrigger(CheckAcquiredTarget);
					continue;

				}

				//LostTarget
				if (trigger.Type == "LostTarget") {

					trigger.ActivateTrigger(CheckLostTarget);
					continue;

				}

				//SwitchedTarget
				if (trigger.Type == "SwitchedTarget") {

					trigger.ActivateTrigger(CheckSwitchedTarget);
					continue;

				}

				//ChangedTarget
				if (trigger.Type == "ChangedTarget") {

					trigger.ActivateTrigger(CheckChangedTarget);
					continue;

				}

				//TargetInSafezone
				if (trigger.Type == "TargetInSafezone") {

					trigger.ActivateTrigger(CheckTargetInSafezone);
					continue;

				}

				//BehaviorTriggerA
				if (trigger.Type == "BehaviorTriggerA") {

					trigger.ActivateTrigger(CheckBehaviorTriggerA);
					continue;

				}

				//BehaviorTriggerB
				if (trigger.Type == "BehaviorTriggerB") {

					trigger.ActivateTrigger(CheckBehaviorTriggerB);
					continue;

				}

				//BehaviorTriggerC
				if (trigger.Type == "BehaviorTriggerC") {

					trigger.ActivateTrigger(CheckBehaviorTriggerC);
					continue;

				}

				//BehaviorTriggerD
				if (trigger.Type == "BehaviorTriggerD") {

					trigger.ActivateTrigger(CheckBehaviorTriggerD);
					continue;

				}

				//BehaviorTriggerE
				if (trigger.Type == "BehaviorTriggerE") {

					trigger.ActivateTrigger(CheckBehaviorTriggerE);
					continue;

				}

				//BehaviorTriggerF
				if (trigger.Type == "BehaviorTriggerF") {

					trigger.ActivateTrigger(CheckBehaviorTriggerF);
					continue;

				}

				//BehaviorTriggerG
				if (trigger.Type == "BehaviorTriggerG") {

					trigger.ActivateTrigger(CheckBehaviorTriggerG);
					continue;

				}

				//PaymentSuccess
				if (trigger.Type == "PaymentSuccess") {

					trigger.ActivateTrigger(CheckPaymentSuccess);
					continue;

				}

				//PaymentFailure
				if (trigger.Type == "PaymentFailure") {

					trigger.ActivateTrigger(CheckPaymentFailure);
					continue;

				}

				//PlayerKnownLocation
				if (trigger.Type == "PlayerKnownLocation") {

					trigger.ActivateTrigger(CheckPlayerKnownLocation);
					continue;

				}

				//SensorActive
				if (trigger.Type == "SensorActive") {

					trigger.ActivateTrigger(CheckSensorActive);
					continue;

				}

				//SensorIdle
				if (trigger.Type == "SensorIdle") {

					trigger.ActivateTrigger(CheckSensorIdle);
					continue;

				}

				//Weather
				if (trigger.Type == "Weather") {

					trigger.ActivateTrigger(CheckWeather);
					continue;

				}

				//JumpRequested
				if (trigger.Type == "JumpRequested") {

					trigger.ActivateTrigger(JumpRequested);
					continue;

				}

				//JumpCompleted
				if (trigger.Type == "JumpCompleted") {

					trigger.ActivateTrigger(JumpCompleted);
					continue;

				}

				//InsideZone
				if (trigger.Type == "InsideZone") {

					trigger.ActivateTrigger(InsideZone);
					continue;

				}

				//OutsideZone
				if (trigger.Type == "OutsideZone") {

					trigger.ActivateTrigger(OutsideZone);
					continue;

				}

				//Session
				if (trigger.Type == "Session") {

					trigger.ActivateTrigger(CheckSession);
					continue;

				}

				//ActiveWeaponsPercentage
				if (trigger.Type == "ActiveWeaponsPercentage") {

					trigger.ActivateTrigger(CheckActiveWeaponsPercentage);
					continue;

				}

				//ActiveTurretsPercentage
				if (trigger.Type == "ActiveTurretsPercentage") {

					trigger.ActivateTrigger(CheckActiveTurretsPercentage);
					continue;

				}

				//ActiveGunsPercentage
				if (trigger.Type == "ActiveGunsPercentage") {

					trigger.ActivateTrigger(CheckActiveGunsPercentage);
					continue;

				}

				//HealthPercentage
				if (trigger.Type == "HealthPercentage") {

					trigger.ActivateTrigger(CheckHealthPercentage);
					continue;

				}

			}

			_behavior.AutoPilot.Targeting.TargetAcquired = false;
			_behavior.AutoPilot.Targeting.TargetLost = false;
			_behavior.AutoPilot.Targeting.TargetSwitched = false;
			_behavior.AutoPilot.Targeting.TargetChanged = false;
			_behavior.BehaviorTriggerA = false;
			_behavior.BehaviorTriggerB = false;
			_behavior.BehaviorTriggerC = false;
			_behavior.BehaviorTriggerD = false;
			_behavior.BehaviorTriggerE = false;
			_behavior.BehaviorTriggerF = false;
			_behavior.BehaviorTriggerG = false;
			PaymentSuccessTriggered = false;
			PaymentFailureTriggered = false;
			TimedTriggersProcessed = true;

		}

		public void ProcessActivatedTriggers() {

			if (!TimedTriggersProcessed)
				return;

			TimedTriggersProcessed = false;

			for (int i = 0; i < Triggers.Count; i++) {

				ProcessTrigger(Triggers[i]);

			}

			//BehaviorLogger.Write("Trigger Actions Complete", BehaviorDebugEnum.Actions);
			//this.OnComplete?.Invoke();

		}

		public void ProcessDamageTriggerWatchers(object target, MyDamageInformation info) {

			if (!_behavior.IsAIReady())
				return;

			//BehaviorLogger.Write("Damage Trigger Count: " + this.DamageTriggers.Count.ToString(), BehaviorDebugEnum.Trigger);
			if (info.Amount <= 0)
				return;

			_settings.LastDamageTakenTime = MyAPIGateway.Session.GameDateTime;
			_settings.TotalDamageAccumulated += info.Amount;
			_settings.LastDamagerEntity = info.AttackerId;

			for (int i = 0; i < DamageTriggers.Count; i++) {

				//BehaviorLogger.AddMsg("Got Trigger Profile", true);

				var trigger = DamageTriggers[i];

				var damageType = info.Type.ToString();

				if ((trigger.DamageTypes.Contains(damageType) || trigger.DamageTypes.Contains("Any")) && !trigger.ExcludedDamageTypes.Contains(damageType)) {

					if (trigger.UseTrigger == true) {

						trigger.ActivateTrigger();

						if (trigger.Triggered == true) {

							BehaviorLogger.Write("Process Damage Actions", BehaviorDebugEnum.Trigger);

							Command newCommand = null;

							var idOwner = DamageHelper.GetAttackOwnerId(info.AttackerId);

							if (FactionHelper.IsIdentityPlayer(idOwner)) 
							{
								newCommand = Command.PlayerRelatedCommand(idOwner);

							}

							ProcessTrigger(trigger, info.AttackerId, newCommand);

						}

					}

				}

			}

		}

		public void ProcessCommandReceiveTriggerWatcher(Command receivedCommand) {

			if (_behavior == null || !_behavior.IsAIReady()) {

				BehaviorLogger.Write("Behavior AI That Received Command Not Active. It Will Be Unregistered.", BehaviorDebugEnum.Command);
				UnregisterCommandListener();
				return;

			}

			if (receivedCommand == null) {

				BehaviorLogger.Write("Command Null", BehaviorDebugEnum.Command);
				return;

			}

			if (string.IsNullOrWhiteSpace(receivedCommand.CommandCode)) {

				BehaviorLogger.Write("Command Code Null or Blank", BehaviorDebugEnum.Command);
				return;

			}

			if (CommandTriggers == null) {

				BehaviorLogger.Write("No Eligible Command Triggers", BehaviorDebugEnum.Command);
				return;

			}


			if (!receivedCommand.FromEvent && (receivedCommand.SenderEntity?.PositionComp == null || RemoteControl?.SlimBlock?.CubeGrid == null)) {

				BehaviorLogger.Write("Sender Remote CubeGrid Null or Receiver Remote CubeGrid Null", BehaviorDebugEnum.Command);
				return;

			}

			if (receivedCommand.SingleRecipient && receivedCommand.Recipient > 0 && receivedCommand.Recipient != RemoteControl.EntityId) {

				BehaviorLogger.Write("Code Is Single Recipient and Already Processed By Another Entity", BehaviorDebugEnum.Command);
				return;

			}

			var dist = Vector3D.Distance(RemoteControl.GetPosition(), receivedCommand.SenderEntity?.GetPosition() ?? receivedCommand.Position);

			if (!receivedCommand.UseTriggerTargetDistance) {

				if (!receivedCommand.IgnoreAntennaRequirement && !receivedCommand.IgnoreReceiverAntennaRequirement) {

					var antenna = _behavior.Grid.GetActiveAntenna();

					if (antenna == null) {

						BehaviorLogger.Write("Receiver Has No Antenna", BehaviorDebugEnum.Command);
						return;

					}

				}

				if (dist > receivedCommand.Radius) {

					BehaviorLogger.Write("Receiver Out Of Code Broadcast Range. Distance: " + dist + " // Command Radius: " + receivedCommand.Radius, BehaviorDebugEnum.Command);
					return;

				}

			}

			if (receivedCommand.MatchSenderReceiverOwners) {

				if (receivedCommand.CommandOwnerId != RemoteControl.OwnerId) {

					BehaviorLogger.Write("Receiver Owner Doesn't Match Sender Owner", BehaviorDebugEnum.Command);
					return;

				}
			
			}

			if (receivedCommand.CheckRelationSenderReceiver)
            {
				var relation = EntityEvaluator.GetRelationBetweenIdentities(receivedCommand.CommandOwnerId, RemoteControl.OwnerId);

                if (receivedCommand.Relation != relation)
				{
					BehaviorLogger.Write("Receiver Owner Doesn't Match relation Sender Owner", BehaviorDebugEnum.Command);
					return;
				}


			}


			bool processed = false;

			for (int i = 0; i < CommandTriggers.Count; i++) {

				var trigger = CommandTriggers[i];

				if (trigger.CommandCodeType != receivedCommand.Type)
					continue;

				if (receivedCommand.UseTriggerTargetDistance && dist > trigger.TargetDistance)
					continue;

				if (string.IsNullOrWhiteSpace(receivedCommand.CommandCode))
					continue;

				var commandreceivecode = IdsReplacer.ReplaceId(_behavior?.CurrentGrid?.Npc ?? null, trigger.CommandReceiveCode);

				bool commandCodePass = !trigger.AllowCommandCodePartialMatch ? (receivedCommand.CommandCode.ToLower() == commandreceivecode.ToLower()) : (receivedCommand.CommandCode.ToLower().Contains(commandreceivecode.ToLower()));

				if (trigger.UseTrigger == true && commandCodePass) {

					trigger.ActivateTrigger(null, receivedCommand);

					if (trigger.Triggered) {

						processed = true;
						ProcessTrigger(trigger, 0, receivedCommand);

					}

				}

			}

			if (receivedCommand.SingleRecipient && processed)
				receivedCommand.Recipient = RemoteControl.EntityId;

		}

		public void ProcessCompromisedTriggerWatcher(bool validToProcess) {

			if (!MyAPIGateway.Multiplayer.IsServer || !validToProcess || RemoteControlCompromised)
				return;

			RemoteControlCompromised = true;

			if (Settings.Grids.StopCompromisedAiMovement) {

				_behavior.AutoPilot.StopAllRotation();
				_behavior.AutoPilot.StopAllThrust();

			}
			LocalApi.CompromisedRemoteEvent?.Invoke(RemoteControl, _behavior.CurrentGrid?.CubeGrid);

			for (int i = 0; i < CompromisedTriggers.Count; i++) {

				var trigger = CompromisedTriggers[i];

				if (trigger.UseTrigger == true) {

					trigger.ActivateTrigger();

					if (trigger.Triggered == true) {

						ProcessTrigger(trigger);

					}

				}

			}

		}

		public void ProcessRetreatTriggers() {

			for (int i = 0; i < Triggers.Count; i++) {

				var trigger = Triggers[i];

				if (trigger.UseTrigger == true && trigger.Type == "Retreat") {

					trigger.ActivateTrigger();

					if (trigger.Triggered == true) {

						ProcessTrigger(trigger);

					}

				}

			}

		}

		public void ProcessJumpRequestTriggers(GridEntity grid, Vector3D coords) {

			for (int i = 0; i < Triggers.Count; i++) {

				var trigger = Triggers[i];

				if (trigger.UseTrigger == true && trigger.Type == "JumpRequested") {

					if (RemoteControl?.SlimBlock?.CubeGrid == null)
						continue;

					bool isSelfGrid = grid.CubeGrid == RemoteControl.SlimBlock.CubeGrid;

					if (!trigger.DetectSelfAsJumpedGrid && isSelfGrid)
						continue;

					if (trigger.DetectSelfAsJumpedGrid && !isSelfGrid)
						continue;

					if (Vector3D.Distance(RemoteControl.GetPosition(), coords) > trigger.JumpedGridActivationDistance)
						continue;

					if (!trigger.DetectSelfAsJumpedGrid && !trigger.JumpedGridsCanBeNonHostile && !grid.RelationTypes(RemoteControl.OwnerId).HasFlag(RelationTypeEnum.Enemy))
						continue;

					trigger.JumpedGrid = grid;
					trigger.JumpStart = coords;
					trigger.JumpEnd = coords;
					trigger.ActivateTrigger();

					if (trigger.Triggered == true) {

						ProcessTrigger(trigger);

					}

				}

			}

		}

		public void ProcessJumpCompletedTriggers(GridEntity grid, Vector3D startCoords, Vector3D endCoords) {

			for (int i = 0; i < Triggers.Count; i++) {

				var trigger = Triggers[i];

				if (trigger.UseTrigger == true && trigger.Type == "JumpCompleted") {

					if (RemoteControl?.SlimBlock?.CubeGrid == null) {

						//MyVisualScriptLogicProvider.ShowNotificationToAll("Self Grid Null", 3000);
						continue;

					}
						

					bool isSelfGrid = grid.CubeGrid == RemoteControl.SlimBlock.CubeGrid;

					if (!trigger.DetectSelfAsJumpedGrid && isSelfGrid) {

						//MyVisualScriptLogicProvider.ShowNotificationToAll("Is Self Grid", 3000);
						continue;

					}
						

					if (trigger.DetectSelfAsJumpedGrid && !isSelfGrid) {

						//MyVisualScriptLogicProvider.ShowNotificationToAll("Not Self Grid", 3000);
						continue;

					}
						

					if (!trigger.DetectSelfAsJumpedGrid && Vector3D.Distance(RemoteControl.GetPosition(), startCoords) > trigger.JumpedGridActivationDistance) {

						//MyVisualScriptLogicProvider.ShowNotificationToAll("Jump Distance Fail", 3000);
						continue;

					}
						

					if (!trigger.DetectSelfAsJumpedGrid && !trigger.JumpedGridsCanBeNonHostile && !grid.RelationTypes(RemoteControl.OwnerId).HasFlag(RelationTypeEnum.Enemy)) {

						//MyVisualScriptLogicProvider.ShowNotificationToAll("Jump Relation Fail", 3000);
						continue;

					}
						

					trigger.JumpedGrid = grid;
					trigger.JumpStart = startCoords;
					trigger.JumpEnd = endCoords;
					trigger.ActivateTrigger();

					if (trigger.Triggered == true) {

						ProcessTrigger(trigger);

					}

				}

			}

		}

		public void ProcessButtonTriggers(IMyButtonPanel panel, int index, long playerId) {

			for (int i = 0; i < Triggers.Count; i++) {

				var trigger = Triggers[i];

				if (trigger.UseTrigger == true && trigger.Type == "ButtonPress") {

					if (_behavior?.RemoteControl == null || !_behavior.IsAIReady()) {

						EventWatcher.ButtonPressed -= ProcessButtonTriggers;
						continue;

					}

					if (index != trigger.ButtonPanelIndex)
						continue;

					if (panel.CustomName == null || panel.CustomName != trigger.ButtonPanelName)
						continue;

					if (panel.SlimBlock.CubeGrid != _behavior.RemoteControl.SlimBlock.CubeGrid && panel.SlimBlock.CubeGrid.IsInSameLogicalGroupAs(_behavior.RemoteControl.SlimBlock.CubeGrid))
						continue;

					if (trigger.MinPlayerReputation >= -1500 || trigger.MaxPlayerReputation <= 1500) {

						var rep = MyAPIGateway.Session.Factions.GetReputationBetweenPlayerAndFaction(playerId, _behavior.Owner.FactionId);

						if ((trigger.MinPlayerReputation >= -1500 && rep < trigger.MinPlayerReputation) || (trigger.MaxPlayerReputation <= 1500 && rep > trigger.MaxPlayerReputation))
							continue;
					
					}


                    if (trigger.UsePlayerFilterProfile)
                    {
						if (!PlayerCondition.ArePlayerConditionsMet(trigger.PlayerFilterProfileIds, playerId))
							continue;
                    }

					trigger.ActivateTrigger(command: Command.PlayerRelatedCommand(playerId));

					if (trigger.Triggered == true) {

						ProcessTrigger(trigger, 0, Command.PlayerRelatedCommand(playerId));

					}

				}

			}

		}

		public void ProcessDespawnTriggers() {

			for (int i = 0; i < Triggers.Count; i++) {

				var trigger = Triggers[i];

				if (trigger.UseTrigger == true && trigger.Type == "Despawn") {

					trigger.ActivateTrigger();

					if (trigger.Triggered == true) {

						ProcessTrigger(trigger);

					}

				}

			}

		}

		public void ProcessMESDespawnTriggers(IMyCubeGrid cubeGrid, string despawnType) {

			for (int i = 0; i < Triggers.Count; i++) {

				var trigger = Triggers[i];

				if (trigger.UseTrigger == true && trigger.Type == "DespawnMES") {

					bool despawnMatch = false;

					if (trigger.DespawnTypeFromSpawner == "Any")
						despawnMatch = true;
					else if(trigger.DespawnTypeFromSpawner == "CleanUp" && despawnType.StartsWith("CleanUp"))
						despawnMatch = true;
					else if (trigger.DespawnTypeFromSpawner == "PathEnd" && despawnType.StartsWith("Cargo Ship Reached End"))
						despawnMatch = true;

					if (trigger.DespawnTypeFromSpawner == "Any" || despawnType == trigger.DespawnTypeFromSpawner) {

						trigger.ActivateTrigger();

						if (trigger.Triggered == true) {

							ProcessTrigger(trigger);

						}

					}

				}

			}

		}

		public void ProcessManualTrigger(TriggerProfile trigger, bool forceActivation = false) {

			BehaviorLogger.Write("Attempting To Manually Trigger Profile " + trigger.ProfileSubtypeId, BehaviorDebugEnum.Trigger);

			if (trigger.UseTrigger) {

				trigger.ActivateTrigger();

				if (!trigger.Triggered && forceActivation)
					trigger.Triggered = true;

				if (trigger.Triggered) {

					ProcessTrigger(trigger);

				}

			}

		}

		public void ProcessTrigger(TriggerProfile trigger, long attackerEntityId = 0, Command command = null) {

			if (RemoteControl?.SlimBlock?.CubeGrid == null)
				return;

			if (!trigger.Triggered)
				return;

			var actionList = trigger.Actions;

			if (trigger.LastRunFailed) {

				actionList = trigger.ElseActions;
				trigger.LastRunFailed = false;

			}

			long detectedEntity = attackerEntityId;

			if (trigger.DetectedEntityId != 0 && detectedEntity == 0) {

				detectedEntity = trigger.DetectedEntityId;

			}

			trigger.DetectedEntityId = 0;
			trigger.Triggered = false;
			trigger.SessionTriggerActivated = true;
			trigger.CooldownTime = trigger.Rnd.Next((int)trigger.MinCooldownMs, (int)trigger.MaxCooldownMs);
			trigger.LastTriggerTime = MyAPIGateway.Session.GameDateTime;
			trigger.TriggerCount++;

			if (!string.IsNullOrWhiteSpace(trigger.ToggleWithTriggerProfile)) {

				trigger.UseTrigger = false;
				ToggleTriggers(trigger.ToggleWithTriggerProfile, CheckEnum.Yes, trigger.ToggledProfileResetsCooldown ? CheckEnum.Yes : CheckEnum.Ignore);
			
			}

			if (!string.IsNullOrWhiteSpace(trigger.EnableNamedTriggerOnSuccess)) {

				ToggleTriggers(trigger.EnableNamedTriggerOnSuccess, CheckEnum.Yes, CheckEnum.Ignore);

			}

			if (!string.IsNullOrWhiteSpace(trigger.DisableNamedTriggerOnSuccess)) {

				ToggleTriggers(trigger.DisableNamedTriggerOnSuccess, CheckEnum.No, CheckEnum.Ignore);

			}

			if (actionList == null || actionList.Count == 0) {

				//Trigger doesn't have actions. Shouldn't be possible to get here with that, but we'll play it safe anyway
				return;
			
			}

			//Action Execution
			if (trigger.ActionExecution == ActionExecutionEnum.All) {

				foreach (var actions in actionList) {

					ProcessAction(trigger, actions, attackerEntityId, detectedEntity, command != null ? command : trigger.TempCommand);

				}

			}

			if (trigger.ActionExecution == ActionExecutionEnum.Sequential) {

				if (trigger.NextActionIndex >= actionList.Count)
					trigger.NextActionIndex = 0;

				ProcessAction(trigger, actionList[trigger.NextActionIndex], attackerEntityId, detectedEntity, command != null ? command : trigger.TempCommand);
				trigger.NextActionIndex++;

			}

			if (trigger.ActionExecution == ActionExecutionEnum.Random) {

				if (actionList.Count == 1) {

					ProcessAction(trigger, actionList[0], attackerEntityId, detectedEntity, command != null ? command : trigger.TempCommand);

				} else {

					ProcessAction(trigger, actionList[MathTools.RandomBetween(0, actionList.Count)], attackerEntityId, detectedEntity, command != null ? command : trigger.TempCommand);

				}

			}

		}

		public void ToggleTriggers(List<string> names, CheckEnum toggle, CheckEnum reset) {

			if (names == null)
				return;

			foreach (var name in names) {

				ToggleTriggers(name, toggle, reset);

			}
		
		}

		public void ToggleTriggers(string triggerName = "", CheckEnum toggleMode = CheckEnum.Ignore, CheckEnum resetCooldown = CheckEnum.Ignore) {

			ToggleTriggers(Triggers, triggerName, toggleMode, resetCooldown);
			ToggleTriggers(DamageTriggers, triggerName, toggleMode, resetCooldown);
			ToggleTriggers(CommandTriggers, triggerName, toggleMode, resetCooldown);
			ToggleTriggers(CompromisedTriggers, triggerName, toggleMode, resetCooldown);
			
		}

		public void ToggleTriggers(List<TriggerProfile> triggers, string triggerName = "", CheckEnum toggleMode = CheckEnum.Ignore, CheckEnum resetCooldown = CheckEnum.Ignore) {

			foreach (var resetTrigger in triggers) {

				if (triggerName == resetTrigger.ProfileSubtypeId) {

					if (toggleMode != CheckEnum.Ignore)
						resetTrigger.UseTrigger = (toggleMode == CheckEnum.Yes);

					if (resetCooldown == CheckEnum.Yes)
						resetTrigger.LastTriggerTime = MyAPIGateway.Session.GameDateTime;

				}
			}
		}

		//Tags
		public void ToggleTagTriggers(List<string> tags, CheckEnum toggle, CheckEnum reset)
		{

			if (tags == null)
				return;

			foreach (var tag in tags)
			{

				ToggleTagTriggers(tag, toggle, reset);

			}

		}

		public void ToggleTagTriggers(string tagName = "", CheckEnum toggleMode = CheckEnum.Ignore, CheckEnum resetCooldown = CheckEnum.Ignore)
		{

			ToggleTagTriggers(Triggers, tagName, toggleMode, resetCooldown);
			ToggleTagTriggers(DamageTriggers, tagName, toggleMode, resetCooldown);
			ToggleTagTriggers(CommandTriggers, tagName, toggleMode, resetCooldown);
			ToggleTagTriggers(CompromisedTriggers, tagName, toggleMode, resetCooldown);

		}

		public void ToggleTagTriggers(List<TriggerProfile> triggers, string tagName = "", CheckEnum toggleMode = CheckEnum.Ignore, CheckEnum resetCooldown = CheckEnum.Ignore)
		{

			foreach (var resetTrigger in triggers)
			{

				if (resetTrigger.Tags.Contains(tagName))
				{

					if (toggleMode != CheckEnum.Ignore)
						resetTrigger.UseTrigger = (toggleMode == CheckEnum.Yes);

					if (resetCooldown == CheckEnum.Yes)
						resetTrigger.LastTriggerTime = MyAPIGateway.Session.GameDateTime;

				}
			}
		}




		public void SetSandboxBool(string boolName, bool mode) {

			if (string.IsNullOrWhiteSpace(boolName))
				return;

			if (boolName.Contains("{SpawnGroupName}") && _behavior?.CurrentGrid?.Npc?.SpawnGroupName != null) {

				boolName = boolName.Replace("{SpawnGroupName}", _behavior.CurrentGrid.Npc.SpawnGroupName);

			}

			if (boolName.Contains("{Faction}") && _behavior?.Owner?.Faction?.Tag != null) {

				boolName = boolName.Replace("{Faction}", _behavior.Owner.Faction.Tag);

			}

			MyAPIGateway.Utilities.SetVariable(boolName, mode);

		}

		public void SetSandboxCounter(string counterName, int amount, bool hardSet = false) {

			if (counterName.Contains("{Faction}") && _behavior?.Owner?.Faction?.Tag != null) {

				counterName = counterName.Replace("{Faction}", _behavior.Owner.Faction.Tag);

			}

			if (hardSet) {

				MyAPIGateway.Utilities.SetVariable(counterName, amount);
				return;
			}

			int existingCounter = 0;

			MyAPIGateway.Utilities.GetVariable(counterName, out existingCounter);

			//This is for ResetSandboxCounters
			if (amount == 0) {

				MyAPIGateway.Utilities.SetVariable(counterName, 0);
				return;

			} else {

				existingCounter += amount;
				MyAPIGateway.Utilities.SetVariable(counterName, existingCounter);
				return;

			}

		}

		public bool IsPlayerNearby(TriggerProfile control, bool playerOutsideDistance = false) {

			PlayerEntity player = null;

			var remotePosition = Vector3D.Transform(control.PlayerNearPositionOffset, RemoteControl.WorldMatrix);

			if (control.MinPlayerReputation != -1501 || control.MaxPlayerReputation != 1501) {

				var customfaction = MyAPIGateway.Session.Factions.TryGetFactionByTag(control.FactionTag);

				if (control.UseCustomFactionTag == true && customfaction != null){

					player = TargetHelper.GetClosestPlayerWithReputation(remotePosition, customfaction.FactionId, control);

				}else{

					player = TargetHelper.GetClosestPlayerWithReputation(remotePosition, _owner.FactionId, control);

				}
			} 

			else {

				player = PlayerManager.GetNearestPlayer(remotePosition);

			}

			if (player == null || !player.ActiveEntity()) {

				//BehaviorLogger.Write(control.ProfileSubtypeId + ": No Eligible Player for PlayerNear Check", BehaviorDebugEnum.Trigger);
				return false;

			}

			var playerDist = Vector3D.Distance(player.GetPosition(), remotePosition);

			if (playerOutsideDistance) {

				if (playerDist < control.TargetDistance) {

					return false;

				}

			} else {

				if (playerDist > control.TargetDistance) {

					return false;

				}

			}

			

			if (control.InsideAntenna == true) {

				var antenna = _behavior.Grid.GetAntennaWithHighestRange(control.InsideAntennaName);

				if (antenna != null) {

					playerDist = Vector3D.Distance(player.GetPosition(), antenna.GetPosition());
					if (playerDist > antenna.Radius) {

						return false;

					}

				} else {

					return false;

				}

			}

			if (control.TempCommand == null)
				control.TempCommand = new Command();

			control.TempCommand.PlayerIdentity = player.Player.IdentityId;

			return true;

		}

		public bool IsPlayerNearbyWithPlayerConditon(TriggerProfile control, bool playerOutsideDistance = false)
		{

			var remotePosition = Vector3D.Transform(control.PlayerNearPositionOffset, RemoteControl.WorldMatrix);


			PlayerEntity result = null;
			double distance = -1;

			foreach (var player in PlayerManager.Players)
			{

				if (player == null || !player.ActiveEntity())
					continue;

				var dist = player.Distance(remotePosition);

				if (distance > -1 && dist > distance)
					continue;

				if (!PlayerCondition.ArePlayerConditionsMet(control.PlayerFilterProfileIds, player.Player.IdentityId))
					continue;

				distance = dist;
				result = player;
			}

			if (result == null || !result.ActiveEntity())
			{

				//BehaviorLogger.Write(control.ProfileSubtypeId + ": No Eligible Player for PlayerNear Check", BehaviorDebugEnum.Trigger);
				return false;

			}

			var playerDist = Vector3D.Distance(result.GetPosition(), remotePosition);



			if (playerOutsideDistance)
            {
				if (playerDist < control.TargetDistance)
					return false;
			}
			else
				if (playerDist > control.TargetDistance)
						return false;

			if (control.TempCommand == null)
				control.TempCommand = new Command();

			control.TempCommand.PlayerIdentity = result.Player.IdentityId;
			return true;

		}



		public void Setup(IMyRemoteControl remoteControl) {

			if (remoteControl?.SlimBlock == null) {

				return;

			}

			RemoteControl = remoteControl;
			DespawnFromMES += ProcessMESDespawnTriggers;
			AntennaList = BlockCollectionHelper.GetGridAntennas(RemoteControl.SlimBlock.CubeGrid);

		}

		public void SetupReferences(AutoPilotSystem autopilot, BroadcastSystem broadcast, DespawnSystem despawn, GridSystem extras, OwnerSystem owners, StoredSettings settings, IBehavior behavior) {

			_autopilot = autopilot;
			_broadcast = broadcast;
			_despawn = despawn;
			_extras = extras;
			_owner = owners;
			_settings = settings;
			_behavior = behavior;

		}

		public void RegisterCommandListener() {

			if (CommandListenerRegistered)
				return;

			CommandListenerRegistered = true;
			CommandHelper.CommandTrigger += ProcessCommandReceiveTriggerWatcher;

		}

		public void UnregisterCommandListener() {

			if (!CommandListenerRegistered)
				return;

			CommandListenerRegistered = false;
			CommandHelper.CommandTrigger -= ProcessCommandReceiveTriggerWatcher;

		}

		public bool AddTrigger(TriggerProfile trigger) {

			if (ExistingTriggers.Contains(trigger.ProfileSubtypeId)) {

				BehaviorLogger.Write("Trigger Already Added: " + trigger.ProfileSubtypeId, BehaviorDebugEnum.BehaviorSetup);
				return false;

			}


			ExistingTriggers.Add(trigger.ProfileSubtypeId);

			trigger.InitRandomTimes();
			if (trigger.Type == "Damage") {

				DamageTriggers.Add(trigger);
				return true;

			}

			if (trigger.Type == "CommandReceived") {

				CommandTriggers.Add(trigger);
				RegisterCommandListener();
				return true;

			}

			if (trigger.Type == "Compromised") {

				CompromisedTriggers.Add(trigger);
				return true;

			}

			Triggers.Add(trigger);
			return true;

		}

		public void InitTags() {

			//TODO: Try To Get Triggers From Block Storage At Start

			//Start With This Class
			if (string.IsNullOrWhiteSpace(RemoteControl.CustomData) == true) {

				return;

			}

			var descSplit = RemoteControl.CustomData.Split('\n');

			foreach (var tag in descSplit) {

				//Triggers
				if (tag.Contains("[Triggers:") == true) {

					bool gotTrigger = false;
					string tempValue = "";
					TagParse.TagStringCheck(tag, ref tempValue);

					if (string.IsNullOrWhiteSpace(tempValue) == false) {

						byte[] byteData = { };

						if (ProfileManager.TriggerObjectTemplates.TryGetValue(tempValue, out byteData) == true) {

							try {

								var profile = MyAPIGateway.Utilities.SerializeFromBinary<TriggerProfile>(byteData);

								if (profile != null) {

									gotTrigger = AddTrigger(profile);

								}

							} catch (Exception e) {

								BehaviorLogger.Write("Exception In Trigger Setup for Tag: " + tag, BehaviorDebugEnum.BehaviorSetup);
								BehaviorLogger.Write(e.ToString(), BehaviorDebugEnum.BehaviorSetup);

							}

						}

					}

					if (!gotTrigger)
						ProfileManager.ReportProfileError(tempValue, "Could Not Add Trigger Profile To Behavior");

				}



				//TriggerGroups
				if (tag.Contains("[TriggerGroups:") == true) {

					bool gotTrigger = false;
					string tempValue = "";
					TagParse.TagStringCheck(tag, ref tempValue);

					if (string.IsNullOrWhiteSpace(tempValue) == false) {

						byte[] byteData = { };

						if (ProfileManager.TriggerGroupObjectTemplates.TryGetValue(tempValue, out byteData) == true) {

							try {

								var profile = MyAPIGateway.Utilities.SerializeFromBinary<TriggerGroupProfile>(byteData);

								if (profile != null) {

									gotTrigger = true;
									foreach (var trigger in profile.Triggers) {

										AddTrigger(trigger);

									}

								}

							} catch (Exception) {



							}

						}

					}

					if (!gotTrigger)
						ProfileManager.ReportProfileError(tempValue, "Could Not Add Trigger Group Profile To Behavior");

				}

				//Triggers
				if (tag.Contains("[DialogueBanks:") == true)
				{
					bool gotTrigger = false;
					string FileSource = "";
					TagParse.TagStringCheck(tag, ref FileSource);

					if (string.IsNullOrWhiteSpace(FileSource) == false)
					{

						DialogueBank dialogueBank = ProfileManager.GetDialogueBank(FileSource);

						if (dialogueBank != null)
						{
							gotTrigger = true;
							_dialogueBanks.Add(dialogueBank);
						}
					}

					if (!gotTrigger)
						ProfileManager.ReportProfileError(FileSource, "Could Not Add DialogueBank To Behavior");



				}



			}

		}

	}

}
