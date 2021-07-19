using ProtoBuf;
using ModularEncountersSystems.Helpers;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRageMath;
using ModularEncountersSystems.Logging;

namespace ModularEncountersSystems.Behavior.Subsystems.Trigger {

	public enum ActionExecutionEnum {
	
		All,
		Sequential,
		Random
	
	}

	[ProtoContract]
	public class TriggerProfile {

		[ProtoMember(1)]
		public string Type;

		[ProtoMember(2)]
		public bool UseTrigger;

		[ProtoMember(3)]
		public double TargetDistance;

		[ProtoMember(4)]
		public bool InsideAntenna;

		[ProtoMember(5)]
		public float MinCooldownMs;

		[ProtoMember(6)]
		public float MaxCooldownMs;

		[ProtoMember(7)]
		public bool StartsReady;

		[ProtoMember(8)]
		public int MaxActions;

		[ProtoMember(9)]
		public ActionProfile ActionsDefunct;

		[ProtoMember(10)]
		public List<string> DamageTypes;

		[ProtoMember(11)]
		public bool Triggered;

		[ProtoMember(12)]
		public int CooldownTime;

		[ProtoMember(13)]
		public int TriggerCount;

		[ProtoMember(14)]
		public DateTime LastTriggerTime;

		[ProtoMember(15)]
		public int MinPlayerReputation;

		[ProtoMember(16)]
		public int MaxPlayerReputation;

		[ProtoMember(17)]
		public ConditionProfile Conditions;

		[ProtoMember(18)]
		public bool ConditionCheckResetsTimer;

		[ProtoMember(19)]
		public long DetectedEntityId;

		[ProtoMember(20)]
		public string CommandReceiveCode;

		[ProtoMember(21)]
		public string ProfileSubtypeId;

		[ProtoMember(22)]
		public Vector3D PlayerNearPositionOffset;

		[ProtoMember(23)]
		public bool AllPlayersMustMatchReputation;

		[ProtoMember(24)]
		public double CustomReputationRangeCheck;

		[ProtoMember(25)]
		public string InventoryBlockName;

		[ProtoMember(26)]
		public string InventoryItemDefinitionId;

		[ProtoMember(27)]
		public float InventoryItemMin;

		[ProtoMember(28)]
		public float InventoryItemMax;

		[ProtoMember(29)]
		public string InsideAntennaName;

		[ProtoMember(30)]
		public List<string> ExcludedDamageTypes;

		[ProtoMember(31)]
		public List<ActionProfile> Actions;

		[ProtoMember(32)]
		public int NextActionIndex;

		[ProtoMember(33)]
		public ActionExecutionEnum ActionExecution;

		[ProtoMember(34)]
		public CommandType CommandCodeType;

		[ProtoMember(35)]
		public bool AllowCommandCodePartialMatch;

		[ProtoMember(36)]
		public string SensorName;

		[ProtoMember(37)]
		public string DespawnTypeFromSpawner;

		[ProtoMember(38)]
		public List<string> WeatherTypes;

		[ProtoIgnore]
		public Random Rnd;

		public TriggerProfile() {

			Type = "";

			UseTrigger = false;
			TargetDistance = 3000;
			InsideAntenna = false;
			InsideAntennaName = "";
			PlayerNearPositionOffset = Vector3D.Zero;
			MinCooldownMs = 0;
			MaxCooldownMs = 1;
			StartsReady = false;
			MaxActions = -1;
			Actions = new List<ActionProfile>();
			DamageTypes = new List<string>();
			ExcludedDamageTypes = new List<string>();
			Conditions = new ConditionProfile();

			Triggered = false;
			CooldownTime = 0;
			TriggerCount = 0;
			LastTriggerTime = MyAPIGateway.Session.GameDateTime;
			DetectedEntityId = 0;

			Conditions = new ConditionProfile();
			ConditionCheckResetsTimer = false;

			MinPlayerReputation = -1501;
			MaxPlayerReputation = 1501;
			AllPlayersMustMatchReputation = false;
			CustomReputationRangeCheck = 5000;

			InventoryBlockName = "";
			InventoryItemDefinitionId = "";
			InventoryItemMin = -1;
			InventoryItemMax = -1;

			CommandReceiveCode = "";
			CommandCodeType = CommandType.DroneAntenna;
			AllowCommandCodePartialMatch = false;

			NextActionIndex = 0;
			ActionExecution = ActionExecutionEnum.All;

			SensorName = "";

			DespawnTypeFromSpawner = "";

			WeatherTypes = new List<string>();

			ProfileSubtypeId = "";


			Rnd = new Random();

		}

		public void ActivateTrigger(Func<TriggerProfile, bool> mainTriggerCheck = null) {

			if (MaxActions >= 0 && TriggerCount >= MaxActions) {

				BehaviorLogger.Write(ProfileSubtypeId + ": Max Successful Actions Reached. Trigger Disabled: " + Type, BehaviorDebugEnum.Trigger);
				UseTrigger = false;
				return;

			}

			if (CooldownTime > 0) {

				TimeSpan duration = MyAPIGateway.Session.GameDateTime - LastTriggerTime;

				if ((this.StartsReady && this.TriggerCount == 0) || duration.TotalMilliseconds >= CooldownTime) {

					if (mainTriggerCheck != null && !InvokeTriggerTypeCondition(mainTriggerCheck)) {

						return;

					}

					if (Conditions.UseConditions == true) {

						if (Conditions.AreConditionsMets()) {

							BehaviorLogger.Write(ProfileSubtypeId + ": Trigger Cooldown & Conditions Satisfied. Trigger Activated: " + Type, BehaviorDebugEnum.Trigger);
							Triggered = true;

						} else if (ConditionCheckResetsTimer) {

							LastTriggerTime = MyAPIGateway.Session.GameDateTime;
							CooldownTime = Rnd.Next((int)MinCooldownMs, (int)MaxCooldownMs);

						}

					} else {

						BehaviorLogger.Write(ProfileSubtypeId + ": Trigger Cooldown Satisfied. Trigger Activated: " + Type, BehaviorDebugEnum.Trigger);
						Triggered = true;

					}

				}

			} else {

				if (mainTriggerCheck != null && !InvokeTriggerTypeCondition(mainTriggerCheck)) {

					return;

				}

				if (Conditions.UseConditions == true) {

					if (Conditions.AreConditionsMets()) {

						BehaviorLogger.Write(ProfileSubtypeId + ": Trigger Conditions Satisfied. Trigger Activated: " + Type, BehaviorDebugEnum.Trigger);
						Triggered = true;

					}

				} else {

					BehaviorLogger.Write(ProfileSubtypeId + ": No Trigger Cooldown Needed. Trigger Activated: " + Type, BehaviorDebugEnum.Trigger);
					Triggered = true;

				}

			}

		}

		public bool InvokeTriggerTypeCondition(Func<TriggerProfile, bool> mainTriggerCheck) {

			var result = mainTriggerCheck?.Invoke(this);

			if (result == null || !result.HasValue) {

				BehaviorLogger.Write(ProfileSubtypeId + ": Trigger Encountered An Error And Is Now Disabled: " + Type, BehaviorDebugEnum.Trigger);
				UseTrigger = false;
				return false;

			} else {

				if (!result.Value) {

					BehaviorLogger.Write(ProfileSubtypeId + ": Trigger Type Condition Not Met: " + Type, BehaviorDebugEnum.Trigger);
					return false;

				}

			}

			return true;

		}

		public void ResetTime() {

			LastTriggerTime = MyAPIGateway.Session.GameDateTime;

			foreach (var actions in Actions) {

				if (actions?.SpawnerDefunct != null) {

					actions.SpawnerDefunct.LastSpawnTime = MyAPIGateway.Session.GameDateTime;

				}


				if (actions?.ChatDataDefunct != null) {

					actions.ChatDataDefunct.LastChatTime = MyAPIGateway.Session.GameDateTime;

				}

			}

		}

		public void InitRandomTimes() {

			this.CooldownTime = (int)MathTools.RandomBetween(this.MinCooldownMs, this.MaxCooldownMs);

			foreach (var actionProfile in this.Actions) {

				foreach (var spawner in actionProfile.Spawner) {
				
					spawner.CooldownTime = (int)MathTools.RandomBetween(spawner.SpawnMinCooldown, spawner.SpawnMaxCooldown);

				}

				foreach (var chat in actionProfile.ChatData) {
				
					chat.SecondsUntilChat = (int)MathTools.RandomBetween(chat.MinTime, chat.MaxTime);

				}
			
			}

		}

		public void InitTags(string customData) {

			if (string.IsNullOrWhiteSpace(customData) == false) {

				var descSplit = customData.Split('\n');

				foreach (var tag in descSplit) {

					//Type
					if (tag.Contains("[Type:") == true) {

						TagParse.TagStringCheck(tag, ref Type);

					}

					//UseTrigger
					if (tag.Contains("[UseTrigger:") == true) {

						TagParse.TagBoolCheck(tag, ref UseTrigger);

					}

					//InsideAntenna
					if (tag.Contains("[InsideAntenna:") == true) {

						TagParse.TagBoolCheck(tag, ref InsideAntenna);

					}

					//InsideAntennaName
					if (tag.Contains("[InsideAntennaName:") == true) {

						TagParse.TagStringCheck(tag, ref InsideAntennaName);

					}

					//TargetDistance
					if (tag.Contains("[TargetDistance:") == true) {

						TagParse.TagDoubleCheck(tag, ref TargetDistance);

					}

					//MinCooldown
					if (tag.Contains("[MinCooldownMs:") == true) {

						TagParse.TagFloatCheck(tag, ref MinCooldownMs);

					}

					//MaxCooldown
					if (tag.Contains("[MaxCooldownMs:") == true) {

						TagParse.TagFloatCheck(tag, ref MaxCooldownMs);

					}

					//StartsReady
					if (tag.Contains("[StartsReady:") == true) {

						TagParse.TagBoolCheck(tag, ref StartsReady);

					}

					//MaxActions
					if (tag.Contains("[MaxActions:") == true) {

						TagParse.TagIntCheck(tag, ref MaxActions);

					}

					//Actions
					if (tag.Contains("[Actions:") == true) {

						string tempValue = "";
						TagParse.TagStringCheck(tag, ref tempValue);
						bool gotAction = false;

						if (string.IsNullOrWhiteSpace(tempValue) == false) {

							byte[] byteData = { };

							if (ProfileManager.ActionObjectTemplates.TryGetValue(tempValue, out byteData) == true) {

								try {

									var profile = MyAPIGateway.Utilities.SerializeFromBinary<ActionProfile>(byteData);

									if (profile != null) {

										Actions.Add(profile);
										gotAction = true;

									}

								} catch (Exception) {



								}

							}

						}

						if (!gotAction)
							ProfileManager.ReportProfileError(tempValue, "Could Not Load Action Profile From Trigger: " + ProfileSubtypeId);


					}

					//DamageTypes
					if (tag.Contains("[DamageTypes:") == true) {

						TagParse.TagStringListCheck(tag, ref DamageTypes);

					}

					//ExcludedDamageTypes
					if (tag.Contains("[ExcludedDamageTypes:") == true) {

						TagParse.TagStringListCheck(tag, ref ExcludedDamageTypes);

					}

					//MinPlayerReputation
					if (tag.Contains("[MinPlayerReputation:") == true) {

						TagParse.TagIntCheck(tag, ref MinPlayerReputation);

					}

					//MaxPlayerReputation
					if (tag.Contains("[MaxPlayerReputation:") == true) {

						TagParse.TagIntCheck(tag, ref MaxPlayerReputation);

					}

					//AllPlayersMustMatchReputation
					if (tag.Contains("[AllPlayersMustMatchReputation:") == true) {

						TagParse.TagBoolCheck(tag, ref AllPlayersMustMatchReputation);

					}

					//CustomReputationRangeCheck
					if (tag.Contains("[CustomReputationRangeCheck:") == true) {

						TagParse.TagDoubleCheck(tag, ref CustomReputationRangeCheck);

					}

					//Conditions
					if (tag.Contains("[Conditions:") == true) {

						string tempValue = "";
						TagParse.TagStringCheck(tag, ref tempValue);
						bool gotCondition = false;

						if (string.IsNullOrWhiteSpace(tempValue) == false) {

							byte[] byteData = { };

							if (ProfileManager.ConditionObjectTemplates.TryGetValue(tempValue, out byteData) == true) {

								try {

									var profile = MyAPIGateway.Utilities.SerializeFromBinary<ConditionProfile>(byteData);

									if (profile != null) {

										Conditions = profile;
										gotCondition = true;

									}

								} catch (Exception) {



								}

							}

						}

						if (!gotCondition)
							ProfileManager.ReportProfileError(tempValue, "Could Not Load Condition Profile From Trigger: " + ProfileSubtypeId);


					}

					//ConditionCheckResetsTimer
					if (tag.Contains("[ConditionCheckResetsTimer:") == true) {

						TagParse.TagBoolCheck(tag, ref ConditionCheckResetsTimer);

					}

					//CommandReceiveCode
					if (tag.Contains("[CommandReceiveCode:") == true) {

						TagParse.TagStringCheck(tag, ref CommandReceiveCode);

					}

					//PlayerNearPositionOffset
					if (tag.Contains("[PlayerNearPositionOffset:") == true) {

						TagParse.TagVector3DCheck(tag, ref PlayerNearPositionOffset);

					}

					//ActionExecution
					if (tag.Contains("[ActionExecution:") == true) {

						TagParse.TagActionExecutionCheck(tag, ref ActionExecution);

					}

					//SensorName
					if (tag.Contains("[SensorName:") == true) {

						TagParse.TagStringCheck(tag, ref SensorName);

					}

					//DespawnTypeFromSpawner
					if (tag.Contains("[DespawnTypeFromSpawner:") == true) {

						TagParse.TagStringCheck(tag, ref DespawnTypeFromSpawner);

					}

					//WeatherTypes
					if (tag.Contains("[WeatherTypes:") == true) {

						TagParse.TagStringListCheck(tag, ref WeatherTypes);

					}

				}

			}

			if (MinCooldownMs > MaxCooldownMs) {

				MinCooldownMs = MaxCooldownMs;

			}

			if (StartsReady == true) {

				CooldownTime = 0;

			} else {

				CooldownTime = Rnd.Next((int)MinCooldownMs, (int)MaxCooldownMs);

			}


		}

	}
}
