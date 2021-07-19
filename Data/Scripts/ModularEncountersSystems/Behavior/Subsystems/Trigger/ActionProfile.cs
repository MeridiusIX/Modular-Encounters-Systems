using ProtoBuf;
using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Helpers;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.ObjectBuilders;
using VRageMath;
using ModularEncountersSystems.Logging;

namespace ModularEncountersSystems.Behavior.Subsystems.Trigger {

	[ProtoContract]
	public class ActionProfile {

		[ProtoMember(1)]
		public bool UseChatBroadcast;

		[ProtoMember(2)]
		public ChatProfile ChatDataDefunct;

		[ProtoMember(3)]
		public bool BarrelRoll;

		[ProtoMember(4)]
		public bool Strafe;

		[ProtoMember(5)]
		public bool ChangeAutopilotSpeed;

		[ProtoMember(6)]
		public float NewAutopilotSpeed;

		[ProtoMember(7)]
		public bool SpawnEncounter;

		[ProtoMember(8)]
		public SpawnProfile SpawnerDefunct;

		[ProtoMember(9)]
		public bool SelfDestruct;

		[ProtoMember(10)]
		public bool Retreat;

		[ProtoMember(11)]
		public bool BroadcastCurrentTarget;

		[ProtoMember(12)]
		public bool BroadcastDamagerTarget;

		[ProtoMember(13)]
		public string BroadcastSendCode;

		[ProtoMember(14)]
		public bool SwitchToReceivedTarget;

		[ProtoMember(15)]
		public bool SwitchToBehavior;

		[ProtoMember(16)]
		public string NewBehavior;

		[ProtoMember(17)]
		public bool PreserveSettingsOnBehaviorSwitch;

		[ProtoMember(18)]
		public bool RefreshTarget;

		[ProtoMember(19)]
		public bool SwitchTargetProfile; //Obsolete

		[ProtoMember(20)]
		public string NewTargetProfile; //Obsolete

		[ProtoMember(21)]
		public bool TriggerTimerBlocks;

		[ProtoMember(22)]
		public List<string> TimerBlockNames;

		[ProtoMember(23)]
		public bool ChangeReputationWithPlayers;

		[ProtoMember(24)]
		public double ReputationChangeRadius;

		[ProtoMember(25)]
		public List<int> ReputationChangeAmount;

		[ProtoMember(26)]
		public bool ActivateAssertiveAntennas;

		[ProtoMember(27)]
		public bool ChangeAntennaOwnership;

		[ProtoMember(28)]
		public string AntennaFactionOwner;

		[ProtoMember(29)]
		public bool CreateKnownPlayerArea;

		[ProtoMember(30)]
		public double KnownPlayerAreaRadius;

		[ProtoMember(31)]
		public int KnownPlayerAreaTimer;

		[ProtoMember(32)]
		public int KnownPlayerAreaMaxSpawns;

		[ProtoMember(33)]
		public bool DamageToolAttacker;

		[ProtoMember(34)]
		public float DamageToolAttackerAmount;

		[ProtoMember(35)]
		public string DamageToolAttackerParticle;

		[ProtoMember(36)]
		public string DamageToolAttackerSound;

		[ProtoMember(37)]
		public bool PlayParticleEffectAtRemote;

		[ProtoMember(38)]
		public string ParticleEffectId;

		[ProtoMember(39)]
		public Vector3D ParticleEffectOffset;

		[ProtoMember(40)]
		public float ParticleEffectScale;

		[ProtoMember(41)]
		public float ParticleEffectMaxTime;

		[ProtoMember(42)]
		public Vector3D ParticleEffectColor;

		[ProtoMember(43)]
		public List<string> SetBooleansTrue;

		[ProtoMember(44)]
		public List<string> SetBooleansFalse;

		[ProtoMember(45)]
		public List<string> IncreaseCounters;

		[ProtoMember(46)]
		public List<string> DecreaseCounters;

		[ProtoMember(47)]
		public List<string> ResetCounters;

		[ProtoMember(48)]
		public List<string> SetSandboxBooleansTrue;

		[ProtoMember(49)]
		public List<string> SetSandboxBooleansFalse;

		[ProtoMember(50)]
		public List<string> IncreaseSandboxCounters;

		[ProtoMember(51)]
		public List<string> DecreaseSandboxCounters;

		[ProtoMember(52)]
		public List<string> ResetSandboxCounters;

		[ProtoMember(53)]
		public bool ChangeAttackerReputation;

		[ProtoMember(54)]
		public List<string> ChangeAttackerReputationFaction;

		[ProtoMember(55)]
		public List<int> ChangeAttackerReputationAmount;

		[ProtoMember(56)]
		public List<string> ReputationChangeFactions;

		[ProtoMember(57)]
		public bool ReputationChangesForAllRadiusPlayerFactionMembers;

		[ProtoMember(58)]
		public bool ReputationChangesForAllAttackPlayerFactionMembers;

		[ProtoMember(59)]
		public string ProfileSubtypeId;

		[ProtoMember(60)]
		public bool BroadcastGenericCommand;

		[ProtoMember(61)]
		public bool BehaviorSpecificEventA;

		[ProtoMember(62)]
		public bool BehaviorSpecificEventB;

		[ProtoMember(63)]
		public bool BehaviorSpecificEventC;

		[ProtoMember(64)]
		public bool BehaviorSpecificEventD;

		[ProtoMember(65)]
		public bool BehaviorSpecificEventE;

		[ProtoMember(66)]
		public bool BehaviorSpecificEventF;

		[ProtoMember(67)]
		public bool BehaviorSpecificEventG;

		[ProtoMember(68)]
		public bool BehaviorSpecificEventH;

		[ProtoMember(69)]
		public bool TerminateBehavior;

		[ProtoMember(70)]
		public bool ChangeTargetProfile;

		[ProtoMember(71)]
		public string NewTargetProfileId; //Implement

		[ProtoMember(72)]
		public bool PreserveTriggersOnBehaviorSwitch;

		[ProtoMember(73)]
		public bool PreserveTargetDataOnBehaviorSwitch;

		[ProtoMember(74)]
		public bool ChangeBlockNames;

		[ProtoMember(75)]
		public List<string> ChangeBlockNamesFrom;

		[ProtoMember(76)]
		public List<string> ChangeBlockNamesTo;

		[ProtoMember(77)]
		public bool ChangeAntennaRanges;

		[ProtoMember(78)]
		public List<string> AntennaNamesForRangeChange;

		[ProtoMember(79)]
		public string AntennaRangeChangeType;

		[ProtoMember(80)]
		public float AntennaRangeChangeAmount;

		[ProtoMember(81)]
		public bool ForceDespawn;

		[ProtoMember(82)]
		public bool ResetCooldownTimeOfTriggers;

		[ProtoMember(83)]
		public List<string> ResetTriggerCooldownNames;

		[ProtoMember(84)]
		public bool ChangeInertiaDampeners;

		[ProtoMember(85)]
		public bool InertiaDampenersEnable;

		[ProtoMember(86)]
		public bool CreateWeatherAtPosition;

		[ProtoMember(87)]
		public string WeatherSubtypeId;

		[ProtoMember(88)]
		public double WeatherRadius;

		[ProtoMember(89)]
		public bool StaggerWarheadDetonation;

		[ProtoMember(90)]
		public bool EnableTriggers;

		[ProtoMember(91)]
		public List<string> EnableTriggerNames;

		[ProtoMember(92)]
		public bool DisableTriggers;

		[ProtoMember(93)]
		public List<string> DisableTriggerNames;

		[ProtoMember(94)]
		public bool ChangeRotationDirection;

		[ProtoMember(95)]
		public Direction RotationDirection;

		[ProtoMember(96)]
		public int WeatherDuration;

		[ProtoMember(97)]
		public bool GenerateExplosion;

		[ProtoMember(98)]
		public Vector3D ExplosionOffsetFromRemote;

		[ProtoMember(99)]
		public int ExplosionDamage;

		[ProtoMember(100)]
		public int ExplosionRange;

		[ProtoMember(101)]
		public CheckEnum GridEditable;

		[ProtoMember(102)]
		public CheckEnum SubGridsEditable;

		[ProtoMember(103)]
		public CheckEnum GridDestructible;

		[ProtoMember(104)]
		public CheckEnum SubGridsDestructible;

		[ProtoMember(105)]
		public bool RecolorGrid;

		[ProtoMember(106)]
		public bool RecolorSubGrids;

		[ProtoMember(107)]
		public List<Vector3D> OldBlockColors;

		[ProtoMember(108)]
		public List<Vector3D> NewBlockColors;

		[ProtoMember(109)]
		public List<string> NewBlockSkins;

		[ProtoMember(110)]
		public bool ExplosionIgnoresVoxels;

		[ProtoMember(111)]
		public bool ChangeBlockOwnership;

		[ProtoMember(112)]
		public List<string> OwnershipBlockNames;

		[ProtoMember(113)]
		public List<string> OwnershipBlockFactions;

		[ProtoMember(114)]
		public bool ChangeBlockDamageMultipliers;

		[ProtoMember(115)]
		public List<string> DamageMultiplierBlockNames;

		[ProtoMember(116)]
		public List<int> DamageMultiplierValues;

		[ProtoMember(117)]
		public int KnownPlayerAreaMinThreatForAvoidingAbandonment;

		[ProtoMember(118)]
		public bool RazeBlocksWithNames;

		[ProtoMember(119)]
		public List<string> RazeBlocksNames;

		[ProtoMember(120)]
		public bool ManuallyActivateTrigger;

		[ProtoMember(121)]
		public List<string> ManuallyActivatedTriggerNames;

		[ProtoMember(122)]
		public bool SendCommandWithoutAntenna;

		[ProtoMember(123)]
		public double SendCommandWithoutAntennaRadius;

		[ProtoMember(124)]
		public bool SwitchToDamagerTarget;

		[ProtoMember(125)]
		public List<ChatProfile> ChatData;

		[ProtoMember(126)]
		public List<SpawnProfile> Spawner;

		[ProtoMember(127)]
		public bool RemoveKnownPlayerArea;

		[ProtoMember(128)]
		public bool RemoveAllKnownPlayerAreas;

		[ProtoMember(129)]
		public int Chance;

		[ProtoMember(130)]
		public bool EnableBlocks;

		[ProtoMember(131)]
		public List<string> EnableBlockNames;

		[ProtoMember(132)]
		public List<SwitchEnum> EnableBlockStates;

		[ProtoMember(133)]
		public bool ChangeAutopilotProfile;

		[ProtoMember(134)]
		public AutoPilotDataMode AutopilotProfile;

		[ProtoMember(135)]
		public bool Ramming;

		[ProtoMember(136)]
		public bool CreateRandomLightning;

		[ProtoMember(137)]
		public bool CreateLightningAtAttacker;

		[ProtoMember(138)]
		public int LightningDamage;

		[ProtoMember(139)]
		public int LightningExplosionRadius;

		[ProtoMember(140)]
		public Vector3D LightningColor;

		[ProtoMember(141)]
		public double LightningMinDistance;

		[ProtoMember(142)]
		public double LightningMaxDistance;

		[ProtoMember(143)]
		public bool CreateLightningAtTarget;

		[ProtoMember(144)]
		public int SelfDestructTimerPadding;

		[ProtoMember(145)]
		public int SelfDestructTimeBetweenBlasts;

		[ProtoMember(146)]
		public List<string> SetCounters;

		[ProtoMember(147)]
		public List<string> SetSandboxCounters;

		[ProtoMember(148)]
		public List<int> SetCountersValues;

		[ProtoMember(149)]
		public List<int> SetSandboxCountersValues;

		[ProtoMember(150)]
		public bool InheritLastAttackerFromCommand;

		[ProtoMember(151)]
		public bool ChangePlayerCredits;

		[ProtoMember(152)]
		public long ChangePlayerCreditsAmount;

		[ProtoMember(153)]
		public bool ChangeNpcFactionCredits;

		[ProtoMember(154)]
		public long ChangeNpcFactionCreditsAmount;

		[ProtoMember(155)]
		public string ChangeNpcFactionCreditsTag;

		[ProtoMember(156)]
		public bool BuildProjectedBlocks;

		[ProtoMember(157)]
		public int MaxProjectedBlocksToBuild;

		[ProtoMember(158)]
		public bool ForceManualTriggerActivation;

		[ProtoMember(159)]
		public bool OverwriteAutopilotProfile;

		[ProtoMember(160)]
		public AutoPilotDataMode OverwriteAutopilotMode;

		[ProtoMember(161)]
		public string OverwriteAutopilotId;

		[ProtoMember(162)]
		public bool SwitchTargetToDamager;

		[ProtoMember(163)]
		public bool BroadcastCommandProfiles;

		[ProtoMember(164)]
		public List<string> CommandProfileIds;

		[ProtoMember(165)]
		public bool AddWaypointFromCommand;

		[ProtoMember(166)]
		public bool RecalculateDespawnCoords;

		[ProtoMember(168)]
		public bool AddDatapadsToSeats;

		[ProtoMember(169)]
		public List<string> DatapadNamesToAdd;

		[ProtoMember(170)]
		public int DatapadCountToAdd;

		[ProtoMember(171)]
		public bool ToggleBlocksOfType;

		[ProtoMember(172)]
		public List<SerializableDefinitionId> BlockTypesToToggle;

		[ProtoMember(173)]
		public List<SwitchEnum> BlockTypeToggles;

		[ProtoMember(174)]
		public bool CancelWaitingAtWaypoint;

		[ProtoMember(175)]
		public bool SwitchToNextWaypoint;

		[ProtoMember(176)]
		public bool HeavyYaw;

		[ProtoMember(177)]
		public bool StopAllRotation;

		[ProtoMember(178)]
		public bool StopAllThrust;

		[ProtoMember(179)]
		public bool RandomGyroRotation;

		[ProtoMember(180)]
		public bool RandomThrustDirection;

		[ProtoMember(181)]
		public string ParentGridNameRequirement;



		public ActionProfile() {

			UseChatBroadcast = false;
			ChatData = new List<ChatProfile>();
			ChatDataDefunct = new ChatProfile();

			BarrelRoll = false;
			Strafe = false;

			ChangeAutopilotSpeed = false;
			NewAutopilotSpeed = 0;

			SpawnEncounter = false;
			Spawner = new List<SpawnProfile>();
			SpawnerDefunct = new SpawnProfile();

			SelfDestruct = false;
			StaggerWarheadDetonation = false;

			Retreat = false;

			BroadcastCurrentTarget = false;
			BroadcastDamagerTarget = false;
			BroadcastSendCode = "";
			SwitchToReceivedTarget = false;
			SwitchTargetToDamager = false;

			SwitchToBehavior = false;
			NewBehavior = "";
			PreserveSettingsOnBehaviorSwitch = false;
			PreserveTriggersOnBehaviorSwitch = false;
			PreserveTargetDataOnBehaviorSwitch = false;

			RefreshTarget = false;

			TriggerTimerBlocks = false;
			TimerBlockNames = new List<string>();

			ChangeReputationWithPlayers = false;
			ReputationChangeRadius = 0;
			ReputationChangeFactions = new List<string>();
			ReputationChangeAmount = new List<int>();
			ReputationChangesForAllRadiusPlayerFactionMembers = false;

			ChangeAttackerReputation = false;
			ChangeAttackerReputationFaction = new List<string>();
			ChangeAttackerReputationAmount = new List<int>();
			ReputationChangesForAllAttackPlayerFactionMembers = false;

			ActivateAssertiveAntennas = false;

			ChangeAntennaOwnership = false;
			AntennaFactionOwner = "Nobody";

			CreateKnownPlayerArea = false;
			KnownPlayerAreaRadius = 10000;
			KnownPlayerAreaTimer = 30;
			KnownPlayerAreaMaxSpawns = -1;

			DamageToolAttacker = false;
			DamageToolAttackerAmount = 90;
			DamageToolAttackerParticle = "";
			DamageToolAttackerSound = "";

			PlayParticleEffectAtRemote = false;
			ParticleEffectId = "";
			ParticleEffectOffset = Vector3D.Zero;
			ParticleEffectScale = 1;
			ParticleEffectMaxTime = -1;
			ParticleEffectColor = Vector3D.Zero;

			SetBooleansTrue = new List<string>();
			SetBooleansFalse = new List<string>();
			IncreaseCounters = new List<string>();
			DecreaseCounters = new List<string>();
			ResetCounters = new List<string>();
			SetCounters = new List<string>();
			SetCountersValues = new List<int>();

			SetSandboxBooleansTrue = new List<string>();
			SetSandboxBooleansFalse = new List<string>();
			IncreaseSandboxCounters = new List<string>();
			DecreaseSandboxCounters = new List<string>();
			ResetSandboxCounters = new List<string>();
			SetSandboxCounters = new List<string>();
			SetSandboxCountersValues = new List<int>();

			BroadcastGenericCommand = false;

			BehaviorSpecificEventA = false;
			BehaviorSpecificEventB = false;
			BehaviorSpecificEventC = false;
			BehaviorSpecificEventD = false;
			BehaviorSpecificEventE = false;
			BehaviorSpecificEventF = false;
			BehaviorSpecificEventG = false;
			BehaviorSpecificEventH = false;

			TerminateBehavior = false;

			ChangeTargetProfile = false;
			NewTargetProfileId = "";

			ChangeBlockNames = false;
			ChangeBlockNamesFrom = new List<string>();
			ChangeBlockNamesTo = new List<string>();

			ChangeAntennaRanges = false;
			AntennaNamesForRangeChange = new List<string>();
			AntennaRangeChangeType = "Set";
			AntennaRangeChangeAmount = 0;

			ForceDespawn = false;

			ResetCooldownTimeOfTriggers = false;
			ResetTriggerCooldownNames = new List<string>();

			ChangeInertiaDampeners = false;
			InertiaDampenersEnable = false;

			EnableTriggers = false;
			EnableTriggerNames = new List<string>();

			DisableTriggers = false;
			DisableTriggerNames = new List<string>();

			ChangeRotationDirection = false;
			RotationDirection = Direction.None;

			GenerateExplosion = false;
			ExplosionOffsetFromRemote = Vector3D.Zero;
			ExplosionDamage = 1;
			ExplosionRange = 1;
			ExplosionIgnoresVoxels = false;

			GridEditable = CheckEnum.Ignore;
			SubGridsEditable = CheckEnum.Ignore;
			GridDestructible = CheckEnum.Ignore;
			SubGridsDestructible = CheckEnum.Ignore;

			RecolorGrid = false;
			RecolorSubGrids = false;
			OldBlockColors = new List<Vector3D>();
			NewBlockColors = new List<Vector3D>();
			NewBlockSkins = new List<string>();

			ChangeBlockOwnership = false;
			OwnershipBlockNames = new List<string>();
			OwnershipBlockFactions = new List<string>();

			ChangeBlockDamageMultipliers = false;
			DamageMultiplierBlockNames = new List<string>();
			DamageMultiplierValues = new List<int>();

			KnownPlayerAreaMinThreatForAvoidingAbandonment = -1;

			RazeBlocksWithNames = false;
			RazeBlocksNames = new List<string>();

			ManuallyActivateTrigger = false;
			ManuallyActivatedTriggerNames = new List<string>();

			SendCommandWithoutAntenna = false;
			SendCommandWithoutAntennaRadius = -1;

			SwitchToDamagerTarget = false;

			RemoveKnownPlayerArea = false;
			RemoveAllKnownPlayerAreas = false;

			Chance = 100;

			EnableBlocks = false;
			EnableBlockNames = new List<string>();
			EnableBlockStates = new List<SwitchEnum>();

			ChangeAutopilotProfile = false;
			AutopilotProfile = AutoPilotDataMode.Primary;

			OverwriteAutopilotProfile = false;
			OverwriteAutopilotMode = AutoPilotDataMode.Primary;
			OverwriteAutopilotId = "";

			Ramming = false;

			CreateRandomLightning = false;
			CreateLightningAtAttacker = false;
			LightningDamage = 0;
			LightningExplosionRadius = 1;
			LightningColor = new Vector3D(100, 100, 100);
			LightningMinDistance = 100;
			LightningMaxDistance = 200;
			CreateLightningAtTarget = false;

			SelfDestructTimerPadding = 0;
			SelfDestructTimeBetweenBlasts = 1;

			InheritLastAttackerFromCommand = false;

			ChangePlayerCredits = false;
			ChangePlayerCreditsAmount = 0;

			ChangeNpcFactionCredits = false;
			ChangeNpcFactionCreditsAmount = 0;
			ChangeNpcFactionCreditsTag = "";

			BuildProjectedBlocks = false;
			MaxProjectedBlocksToBuild = -1;

			ForceManualTriggerActivation = false;

			BroadcastCommandProfiles = false;
			CommandProfileIds = new List<string>();

			AddWaypointFromCommand = false;

			RecalculateDespawnCoords = false;

			AddDatapadsToSeats = false;
			DatapadNamesToAdd = new List<string>();
			DatapadCountToAdd = 1;

			ToggleBlocksOfType = false;
			BlockTypesToToggle = new List<SerializableDefinitionId>();
			BlockTypeToggles = new List<SwitchEnum>();

			CancelWaitingAtWaypoint = false;
			SwitchToNextWaypoint = false;

			HeavyYaw = false;

			StopAllRotation = false;
			StopAllThrust = false;
			RandomGyroRotation = false;
			RandomThrustDirection = false;

			ParentGridNameRequirement = "";

			ProfileSubtypeId = "";

		}

		public void InitTags(string customData) {

			if (string.IsNullOrWhiteSpace(customData) == false) {

				var descSplit = customData.Split('\n');

				foreach (var tag in descSplit) {

					//UseChatBroadcast
					if (tag.Contains("[UseChatBroadcast:") == true) {

						TagParse.TagBoolCheck(tag, ref UseChatBroadcast);

					}

					//ChatData
					if (tag.Contains("[ChatData:") == true) {

						string tempValue = "";
						TagParse.TagStringCheck(tag, ref tempValue);
						bool gotChat = false;

						if (string.IsNullOrWhiteSpace(tempValue) == false) {

							byte[] byteData = { };

							if (ProfileManager.ChatObjectTemplates.TryGetValue(tempValue, out byteData) == true) {

								try {

									var profile = MyAPIGateway.Utilities.SerializeFromBinary<ChatProfile>(byteData);

									if (profile != null) {

										ChatData.Add(profile);
										gotChat = true;

									} else {

										BehaviorLogger.Write("Deserialized Chat Profile was Null", BehaviorDebugEnum.Error, true);

									}

								} catch (Exception e) {

									BehaviorLogger.Write("Caught Exception While Attaching to Action Profile:", BehaviorDebugEnum.Error, true);
									BehaviorLogger.Write(e.ToString(), BehaviorDebugEnum.Error, true);

								}

							} else {

								ProfileManager.ReportProfileError(tempValue, "Chat Profile Not Registered in Profile Manager");

							}

						}

						if (!gotChat)
							ProfileManager.ReportProfileError(tempValue, "Provided Chat Profile Could Not Be Loaded in Trigger: " + ProfileSubtypeId);

					}

					//BarrelRoll
					if (tag.Contains("[BarrelRoll:") == true) {

						 TagParse.TagBoolCheck(tag, ref BarrelRoll);

					}

					//Strafe
					if (tag.Contains("[Strafe:") == true) {

						 TagParse.TagBoolCheck(tag, ref Strafe);

					}

					//ChangeAutopilotSpeed
					if (tag.Contains("[ChangeAutopilotSpeed:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeAutopilotSpeed);

					}

					//NewAutopilotSpeed
					if (tag.Contains("[NewAutopilotSpeed:") == true) {

						 TagParse.TagFloatCheck(tag, ref NewAutopilotSpeed);

					}

					//SpawnEncounter
					if (tag.Contains("[SpawnEncounter:") == true) {

						 TagParse.TagBoolCheck(tag, ref SpawnEncounter);

					}

					//Spawner
					if (tag.Contains("[Spawner:") == true) {

						string tempValue = "";
						TagParse.TagStringCheck(tag, ref tempValue);
						bool gotSpawn = false;

						if (string.IsNullOrWhiteSpace(tempValue) == false) {

							byte[] byteData = { };

							if (ProfileManager.SpawnerObjectTemplates.TryGetValue(tempValue, out byteData) == true) {

								try {

									var profile = MyAPIGateway.Utilities.SerializeFromBinary<SpawnProfile>(byteData);

									if (profile != null) {

										Spawner.Add(profile);
										gotSpawn = true;

									}

								} catch (Exception) {



								}

							}

						}

						if (!gotSpawn)
							ProfileManager.ReportProfileError(tempValue, "Provided Spawn Profile Could Not Be Loaded In Profile: " + ProfileSubtypeId);


					}

					//SelfDestruct
					if (tag.Contains("[SelfDestruct:") == true) {

						 TagParse.TagBoolCheck(tag, ref SelfDestruct);

					}

					//Retreat
					if (tag.Contains("[Retreat:") == true) {

						 TagParse.TagBoolCheck(tag, ref Retreat);

					}

					//TerminateBehavior
					if (tag.Contains("[TerminateBehavior:") == true) {

						 TagParse.TagBoolCheck(tag, ref TerminateBehavior);

					}

					//BroadcastCurrentTarget
					if (tag.Contains("[BroadcastCurrentTarget:") == true) {

						 TagParse.TagBoolCheck(tag, ref BroadcastCurrentTarget);

					}

					//SwitchToReceivedTarget
					if (tag.Contains("[SwitchToReceivedTarget:") == true) {

						 TagParse.TagBoolCheck(tag, ref SwitchToReceivedTarget);

					}

					//SwitchTargetToDamager
					if (tag.Contains("[SwitchTargetToDamager:") == true) {

						 TagParse.TagBoolCheck(tag, ref SwitchTargetToDamager);

					}

					//BroadcastDamagerTarget
					if (tag.Contains("[BroadcastDamagerTarget:") == true) {

						 TagParse.TagBoolCheck(tag, ref BroadcastDamagerTarget);

					}

					//BroadcastSendCode
					if (tag.Contains("[BroadcastSendCode:") == true) {

						 TagParse.TagStringCheck(tag, ref BroadcastSendCode);

					}

					//SwitchToBehavior
					if (tag.Contains("[SwitchToBehavior:") == true) {

						 TagParse.TagBoolCheck(tag, ref SwitchToBehavior);

					}

					//NewBehavior
					if (tag.Contains("[NewBehavior:") == true) {

						 TagParse.TagStringCheck(tag, ref NewBehavior);

					}

					//PreserveSettingsOnBehaviorSwitch
					if (tag.Contains("[PreserveSettingsOnBehaviorSwitch:") == true) {

						 TagParse.TagBoolCheck(tag, ref PreserveSettingsOnBehaviorSwitch);

					}

					//PreserveTriggersOnBehaviorSwitch
					if (tag.Contains("[PreserveTriggersOnBehaviorSwitch:") == true) {

						 TagParse.TagBoolCheck(tag, ref PreserveTriggersOnBehaviorSwitch);

					}

					//PreserveTargetDataOnBehaviorSwitch
					if (tag.Contains("[PreserveTargetDataOnBehaviorSwitch:") == true) {

						 TagParse.TagBoolCheck(tag, ref PreserveTargetDataOnBehaviorSwitch);

					}

					//RefreshTarget
					if (tag.Contains("[RefreshTarget:") == true) {

						 TagParse.TagBoolCheck(tag, ref RefreshTarget);

					}

					//SwitchTargetProfile
					if (tag.Contains("[SwitchTargetProfile:") == true) {

						 TagParse.TagBoolCheck(tag, ref SwitchTargetProfile);

					}

					//NewTargetProfile
					if (tag.Contains("[NewTargetProfile:") == true) {

						 TagParse.TagStringCheck(tag, ref NewTargetProfile);

					}

					//TriggerTimerBlocks
					if (tag.Contains("[TriggerTimerBlocks:") == true) {

						 TagParse.TagBoolCheck(tag, ref TriggerTimerBlocks);

					}

					//TimerBlockNames
					if (tag.Contains("[TimerBlockNames:") == true) {

						TagParse.TagStringListCheck(tag, ref TimerBlockNames);

					}

					//ChangeReputationWithPlayers
					if (tag.Contains("[ChangeReputationWithPlayers:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeReputationWithPlayers);

					}

					//ReputationChangeRadius
					if (tag.Contains("[ReputationChangeRadius:") == true) {

						 TagParse.TagDoubleCheck(tag, ref ReputationChangeRadius);

					}

					//ReputationChangeFactions
					if (tag.Contains("[ReputationChangeFactions:") == true) {

						TagParse.TagStringListCheck(tag, ref ReputationChangeFactions);

					}

					//ReputationChangeAmount
					if (tag.Contains("[ReputationChangeAmount:") == true) {

						TagParse.TagIntListCheck(tag, ref ReputationChangeAmount);

					}

					//ActivateAssertiveAntennas
					if (tag.Contains("[ActivateAssertiveAntennas:") == true) {

						 TagParse.TagBoolCheck(tag, ref ActivateAssertiveAntennas);

					}

					//ChangeAntennaOwnership
					if (tag.Contains("[ChangeAntennaOwnership:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeAntennaOwnership);

					}

					//AntennaFactionOwner
					if (tag.Contains("[AntennaFactionOwner:") == true) {

						 TagParse.TagStringCheck(tag, ref AntennaFactionOwner);

					}

					//CreateKnownPlayerArea
					if (tag.Contains("[CreateKnownPlayerArea:") == true) {

						 TagParse.TagBoolCheck(tag, ref CreateKnownPlayerArea);

					}

					//KnownPlayerAreaRadius
					if (tag.Contains("[KnownPlayerAreaRadius:") == true) {

						 TagParse.TagDoubleCheck(tag, ref KnownPlayerAreaRadius);

					}

					//KnownPlayerAreaTimer
					if (tag.Contains("[KnownPlayerAreaTimer:") == true) {

						 TagParse.TagIntCheck(tag, ref KnownPlayerAreaTimer);

					}

					//KnownPlayerAreaMaxSpawns
					if (tag.Contains("[KnownPlayerAreaMaxSpawns:") == true) {

						 TagParse.TagIntCheck(tag, ref KnownPlayerAreaMaxSpawns);

					}

					//KnownPlayerAreaMinThreatForAvoidingAbandonment
					if (tag.Contains("[KnownPlayerAreaMinThreatForAvoidingAbandonment:") == true) {

						 TagParse.TagIntCheck(tag, ref KnownPlayerAreaMinThreatForAvoidingAbandonment);

					}

					//DamageToolAttacker
					if (tag.Contains("[DamageToolAttacker:") == true) {

						 TagParse.TagBoolCheck(tag, ref DamageToolAttacker);

					}

					//DamageToolAttackerAmount
					if (tag.Contains("[DamageToolAttackerAmount:") == true) {

						 TagParse.TagFloatCheck(tag, ref DamageToolAttackerAmount);

					}

					//DamageToolAttackerParticle
					if (tag.Contains("[DamageToolAttackerParticle:") == true) {

						 TagParse.TagStringCheck(tag, ref DamageToolAttackerParticle);

					}

					//DamageToolAttackerSound
					if (tag.Contains("[DamageToolAttackerSound:") == true) {

						 TagParse.TagStringCheck(tag, ref DamageToolAttackerSound);

					}

					//PlayParticleEffectAtRemote
					if (tag.Contains("[PlayParticleEffectAtRemote:") == true) {

						 TagParse.TagBoolCheck(tag, ref PlayParticleEffectAtRemote);

					}

					//ParticleEffectId
					if (tag.Contains("[ParticleEffectId:") == true) {

						 TagParse.TagStringCheck(tag, ref ParticleEffectId);

					}

					//ParticleEffectOffset
					if (tag.Contains("[ParticleEffectOffset:") == true) {

						 TagParse.TagVector3DCheck(tag, ref ParticleEffectOffset);

					}

					//ParticleEffectScale
					if (tag.Contains("[ParticleEffectScale:") == true) {

						 TagParse.TagFloatCheck(tag, ref ParticleEffectScale);

					}

					//ParticleEffectMaxTime
					if (tag.Contains("[ParticleEffectMaxTime:") == true) {

						 TagParse.TagFloatCheck(tag, ref ParticleEffectMaxTime);

					}

					//ParticleEffectColor
					if (tag.Contains("[ParticleEffectColor:") == true) {

						 TagParse.TagVector3DCheck(tag, ref ParticleEffectColor);

					}

					//SetBooleansTrue
					if (tag.Contains("[SetBooleansTrue:") == true) {

						TagParse.TagStringListCheck(tag, ref SetBooleansTrue);

					}

					//SetBooleansFalse
					if (tag.Contains("[SetBooleansFalse:") == true) {

						TagParse.TagStringListCheck(tag, ref SetBooleansFalse);

					}

					//IncreaseCounters
					if (tag.Contains("[IncreaseCounters:") == true) {

						TagParse.TagStringListCheck(tag, ref IncreaseCounters);

					}

					//DecreaseCounters
					if (tag.Contains("[DecreaseCounters:") == true) {

						TagParse.TagStringListCheck(tag, ref DecreaseCounters);

					}

					//ResetCounters
					if (tag.Contains("[ResetCounters:") == true) {

						TagParse.TagStringListCheck(tag, ref ResetCounters);

					}

					//SetSandboxBooleansTrue
					if (tag.Contains("[SetSandboxBooleansTrue:") == true) {

						TagParse.TagStringListCheck(tag, ref SetSandboxBooleansTrue);

					}

					//SetSandboxBooleansFalse
					if (tag.Contains("[SetSandboxBooleansFalse:") == true) {

						TagParse.TagStringListCheck(tag, ref SetSandboxBooleansFalse);

					}

					//IncreaseSandboxCounters
					if (tag.Contains("[IncreaseSandboxCounters:") == true) {

						TagParse.TagStringListCheck(tag, ref IncreaseSandboxCounters);

					}

					//DecreaseSandboxCounters
					if (tag.Contains("[DecreaseSandboxCounters:") == true) {

						TagParse.TagStringListCheck(tag, ref DecreaseSandboxCounters);

					}

					//ResetSandboxCounters
					if (tag.Contains("[ResetSandboxCounters:") == true) {

						TagParse.TagStringListCheck(tag, ref ResetSandboxCounters);

					}

					//ChangeAttackerReputation
					if (tag.Contains("[ChangeAttackerReputation:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeAttackerReputation);

					}

					//ChangeAttackerReputationFaction
					if (tag.Contains("[ChangeAttackerReputationFaction:") == true) {

						TagParse.TagStringListCheck(tag, ref ChangeAttackerReputationFaction);

					}

					//ChangeAttackerReputationAmount
					if (tag.Contains("[ChangeAttackerReputationAmount:") == true) {

						TagParse.TagIntListCheck(tag, ref ChangeAttackerReputationAmount);

					}

					//ReputationChangesForAllAttackPlayerFactionMembers
					if (tag.Contains("[ReputationChangesForAllAttackPlayerFactionMembers:") == true) {

						 TagParse.TagBoolCheck(tag, ref ReputationChangesForAllAttackPlayerFactionMembers);

					}

					//ChangeTargetProfile
					if (tag.Contains("[ChangeTargetProfile:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeTargetProfile);

					}

					//NewTargetProfileId
					if (tag.Contains("[NewTargetProfileId:") == true) {

						 TagParse.TagStringCheck(tag, ref NewTargetProfileId);

					}

					//ChangeBlockNames
					if (tag.Contains("[ChangeBlockNames:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeBlockNames);

					}

					//ChangeBlockNamesFrom
					if (tag.Contains("[ChangeBlockNamesFrom:") == true) {

						TagParse.TagStringListCheck(tag, ref ChangeBlockNamesFrom);

					}

					//ChangeBlockNamesTo
					if (tag.Contains("[ChangeBlockNamesTo:") == true) {

						TagParse.TagStringListCheck(tag, ref ChangeBlockNamesTo);

					}

					//ChangeAntennaRanges
					if (tag.Contains("[ChangeAntennaRanges:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeAntennaRanges);

					}

					//AntennaNamesForRangeChange
					if (tag.Contains("[AntennaNamesForRangeChange:") == true) {

						TagParse.TagStringListCheck(tag, ref AntennaNamesForRangeChange);

					}

					//AntennaRangeChangeType
					if (tag.Contains("[AntennaRangeChangeType:") == true) {

						 TagParse.TagStringCheck(tag, ref AntennaRangeChangeType);

					}

					//AntennaRangeChangeAmount
					if (tag.Contains("[AntennaRangeChangeAmount:") == true) {

						 TagParse.TagFloatCheck(tag, ref AntennaRangeChangeAmount);

					}

					//ForceDespawn
					if (tag.Contains("[ForceDespawn:") == true) {

						 TagParse.TagBoolCheck(tag, ref ForceDespawn);

					}

					//ResetCooldownTimeOfTriggers
					if (tag.Contains("[ResetCooldownTimeOfTriggers:") == true) {

						 TagParse.TagBoolCheck(tag, ref ResetCooldownTimeOfTriggers);

					}

					//ResetTriggerCooldownNames
					if (tag.Contains("[ResetTriggerCooldownNames:") == true) {

						TagParse.TagStringListCheck(tag, ref ResetTriggerCooldownNames);

					}

					//BroadcastGenericCommand
					if (tag.Contains("[BroadcastGenericCommand:") == true) {

						 TagParse.TagBoolCheck(tag, ref BroadcastGenericCommand);

					}

					//BehaviorSpecificEventA
					if (tag.Contains("[BehaviorSpecificEventA:") == true) {

						 TagParse.TagBoolCheck(tag, ref BehaviorSpecificEventA);

					}

					//ChangeInertiaDampeners
					if (tag.Contains("[ChangeInertiaDampeners:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeInertiaDampeners);

					}

					//InertiaDampenersEnable
					if (tag.Contains("[InertiaDampenersEnable:") == true) {

						 TagParse.TagBoolCheck(tag, ref InertiaDampenersEnable);

					}

					//EnableTriggers
					if (tag.Contains("[EnableTriggers:") == true) {

						 TagParse.TagBoolCheck(tag, ref EnableTriggers);

					}

					//EnableTriggerNames
					if (tag.Contains("[EnableTriggerNames:") == true) {

						TagParse.TagStringListCheck(tag, ref EnableTriggerNames);

					}

					//DisableTriggers
					if (tag.Contains("[DisableTriggers:") == true) {

						 TagParse.TagBoolCheck(tag, ref DisableTriggers);

					}

					//DisableTriggerNames
					if (tag.Contains("[DisableTriggerNames:") == true) {

						TagParse.TagStringListCheck(tag, ref DisableTriggerNames);

					}

					//StaggerWarheadDetonation
					if (tag.Contains("[StaggerWarheadDetonation:") == true) {

						 TagParse.TagBoolCheck(tag, ref StaggerWarheadDetonation);

					}

					//ChangeRotationDirection
					if (tag.Contains("[ChangeRotationDirection:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeRotationDirection);

					}

					//RotationDirection
					if (tag.Contains("[RotationDirection:") == true) {

						 TagParse.TagDirectionEnumCheck(tag, ref RotationDirection);

					}

					//GenerateExplosion
					if (tag.Contains("[GenerateExplosion:") == true) {

						 TagParse.TagBoolCheck(tag, ref GenerateExplosion);

					}

					//ExplosionOffsetFromRemote
					if (tag.Contains("[ExplosionOffsetFromRemote:") == true) {

						 TagParse.TagVector3DCheck(tag, ref ExplosionOffsetFromRemote);

					}

					//ExplosionRange
					if (tag.Contains("[ExplosionRange:") == true) {

						 TagParse.TagIntCheck(tag, ref ExplosionRange);

					}

					//ExplosionDamage
					if (tag.Contains("[ExplosionDamage:") == true) {

						 TagParse.TagIntCheck(tag, ref ExplosionDamage);

					}

					//ExplosionIgnoresVoxels
					if (tag.Contains("[ExplosionIgnoresVoxels:") == true) {

						 TagParse.TagBoolCheck(tag, ref ExplosionIgnoresVoxels);

					}

					//GridEditable
					if (tag.Contains("[GridEditable:") == true) {

						 TagParse.TagCheckEnumCheck(tag, ref GridEditable);

					}

					//SubGridsEditable
					if (tag.Contains("[SubGridsEditable:") == true) {

						 TagParse.TagCheckEnumCheck(tag, ref SubGridsEditable);

					}

					//GridDestructible
					if (tag.Contains("[GridDestructible:") == true) {

						 TagParse.TagCheckEnumCheck(tag, ref GridDestructible);

					}

					//SubGridsDestructible
					if (tag.Contains("[SubGridsDestructible:") == true) {

						 TagParse.TagCheckEnumCheck(tag, ref SubGridsDestructible);

					}

					//RecolorGrid
					if (tag.Contains("[RecolorGrid:") == true) {

						 TagParse.TagBoolCheck(tag, ref RecolorGrid);

					}

					//RecolorSubGrids
					if (tag.Contains("[RecolorSubGrids:") == true) {

						 TagParse.TagBoolCheck(tag, ref RecolorSubGrids);

					}

					//OldBlockColors
					if (tag.Contains("[OldBlockColors:") == true) {

						TagParse.TagVector3DListCheck(tag, ref OldBlockColors);

					}

					//NewBlockColors
					if (tag.Contains("[NewBlockColors:") == true) {

						TagParse.TagVector3DListCheck(tag, ref NewBlockColors);

					}

					//NewBlockSkins
					if (tag.Contains("[NewBlockSkins:") == true) {

						TagParse.TagStringListCheck(tag, ref NewBlockSkins);

					}

					//ChangeBlockOwnership
					if (tag.Contains("[ChangeBlockOwnership:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeBlockOwnership);

					}

					//OwnershipBlockNames
					if (tag.Contains("[OwnershipBlockNames:") == true) {

						TagParse.TagStringListCheck(tag, ref OwnershipBlockNames);

					}

					//OwnershipBlockFactions
					if (tag.Contains("[OwnershipBlockFactions:") == true) {

						TagParse.TagStringListCheck(tag, ref OwnershipBlockFactions);

					}

					//ChangeBlockDamageMultipliers
					if (tag.Contains("[ChangeBlockDamageMultipliers:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeBlockDamageMultipliers);

					}

					//DamageMultiplierBlockNames
					if (tag.Contains("[DamageMultiplierBlockNames:") == true) {

						TagParse.TagStringListCheck(tag, ref DamageMultiplierBlockNames);

					}

					//DamageMultiplierValues
					if (tag.Contains("[DamageMultiplierValues:") == true) {

						TagParse.TagIntListCheck(tag, ref DamageMultiplierValues);

					}

					//RazeBlocksWithNames
					if (tag.Contains("[RazeBlocksWithNames:") == true) {

						 TagParse.TagBoolCheck(tag, ref RazeBlocksWithNames);

					}

					//RazeBlocksNames
					if (tag.Contains("[RazeBlocksNames:") == true) {

						TagParse.TagStringListCheck(tag, ref RazeBlocksNames);

					}

					//ManuallyActivateTrigger
					if (tag.Contains("[ManuallyActivateTrigger:") == true) {

						 TagParse.TagBoolCheck(tag, ref ManuallyActivateTrigger);

					}

					//ManuallyActivatedTriggerNames
					if (tag.Contains("[ManuallyActivatedTriggerNames:") == true) {

						TagParse.TagStringListCheck(tag, ref ManuallyActivatedTriggerNames);

					}

					//SendCommandWithoutAntenna
					if (tag.Contains("[SendCommandWithoutAntenna:") == true) {

						 TagParse.TagBoolCheck(tag, ref SendCommandWithoutAntenna);

					}

					//SendCommandWithoutAntennaRadius
					if (tag.Contains("[SendCommandWithoutAntennaRadius:") == true) {

						 TagParse.TagDoubleCheck(tag, ref SendCommandWithoutAntennaRadius);

					}

					//RemoveKnownPlayerArea
					if (tag.Contains("[RemoveKnownPlayerArea:") == true) {

						 TagParse.TagBoolCheck(tag, ref RemoveKnownPlayerArea);

					}

					//RemoveAllKnownPlayerAreas
					if (tag.Contains("[RemoveAllKnownPlayerAreas:") == true) {

						 TagParse.TagBoolCheck(tag, ref RemoveAllKnownPlayerAreas);

					}

					//Chance
					if (tag.Contains("[Chance:") == true) {

						 TagParse.TagIntCheck(tag, ref Chance);

					}

					//EnableBlocks
					if (tag.Contains("[EnableBlocks:") == true) {

						 TagParse.TagBoolCheck(tag, ref EnableBlocks);

					}

					//EnableBlockNames
					if (tag.Contains("[EnableBlockNames:") == true) {

						TagParse.TagStringListCheck(tag, ref EnableBlockNames);

					}

					//EnableBlockStates
					if (tag.Contains("[EnableBlockStates:") == true) {

						TagParse.TagSwitchEnumCheck(tag, ref EnableBlockStates);

					}

					//ChangeAutopilotProfile
					if (tag.Contains("[ChangeAutopilotProfile:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeAutopilotProfile);

					}

					//AutopilotProfile
					if (tag.Contains("[AutopilotProfile:") == true) {

						 TagParse.TagAutoPilotProfileModeCheck(tag, ref AutopilotProfile);

					}

					//Ramming
					if (tag.Contains("[Ramming:") == true) {

						 TagParse.TagBoolCheck(tag, ref Ramming);

					}

					//CreateRandomLightning
					if (tag.Contains("[CreateRandomLightning:") == true) {

						 TagParse.TagBoolCheck(tag, ref CreateRandomLightning);

					}

					//CreateLightningAtAttacker
					if (tag.Contains("[CreateLightningAtAttacker:") == true) {

						 TagParse.TagBoolCheck(tag, ref CreateLightningAtAttacker);

					}

					//LightningDamage
					if (tag.Contains("[LightningDamage:") == true) {

						 TagParse.TagIntCheck(tag, ref LightningDamage);

					}

					//LightningExplosionRadius
					if (tag.Contains("[LightningExplosionRadius:") == true) {

						 TagParse.TagIntCheck(tag, ref LightningExplosionRadius);

					}

					//LightningColor
					if (tag.Contains("[LightningColor:") == true) {

						 TagParse.TagVector3DCheck(tag, ref LightningColor);

					}

					//LightningMinDistance
					if (tag.Contains("[LightningMinDistance:") == true) {

						 TagParse.TagDoubleCheck(tag, ref LightningMinDistance);

					}

					//LightningMaxDistance
					if (tag.Contains("[LightningMaxDistance:") == true) {

						 TagParse.TagDoubleCheck(tag, ref LightningMaxDistance);

					}

					//CreateLightningAtTarget
					if (tag.Contains("[CreateLightningAtTarget:") == true) {

						 TagParse.TagBoolCheck(tag, ref CreateLightningAtTarget);

					}

					//SelfDestructTimerPadding
					if (tag.Contains("[SelfDestructTimerPadding:") == true) {

						 TagParse.TagIntCheck(tag, ref SelfDestructTimerPadding);

					}

					//SelfDestructTimeBetweenBlasts
					if (tag.Contains("[SelfDestructTimeBetweenBlasts:") == true) {

						 TagParse.TagIntCheck(tag, ref SelfDestructTimeBetweenBlasts);

					}

					//SetCounters
					if (tag.Contains("[SetCounters:") == true) {

						TagParse.TagStringListCheck(tag, ref SetCounters);

					}

					//SetSandboxCounters
					if (tag.Contains("[SetSandboxCounters:") == true) {

						TagParse.TagStringListCheck(tag, ref SetSandboxCounters);

					}

					//SetCountersValues
					if (tag.Contains("[SetCountersValues:") == true) {

						TagParse.TagIntListCheck(tag, ref SetCountersValues);

					}

					//SetSandboxCountersValues
					if (tag.Contains("[SetSandboxCountersValues:") == true) {

						TagParse.TagIntListCheck(tag, ref SetSandboxCountersValues);

					}

					//InheritLastAttackerFromCommand
					if (tag.Contains("[InheritLastAttackerFromCommand:") == true) {

						 TagParse.TagBoolCheck(tag, ref InheritLastAttackerFromCommand);

					}

					//ChangePlayerCredits
					if (tag.Contains("[ChangePlayerCredits:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangePlayerCredits);

					}

					//ChangePlayerCreditsAmount
					if (tag.Contains("[ChangePlayerCreditsAmount:") == true) {

						 TagParse.TagLongCheck(tag, ref ChangePlayerCreditsAmount);

					}

					//ChangeNpcFactionCredits
					if (tag.Contains("[ChangeNpcFactionCredits:") == true) {

						 TagParse.TagBoolCheck(tag, ref ChangeNpcFactionCredits);

					}

					//ChangeNpcFactionCreditsAmount
					if (tag.Contains("[ChangeNpcFactionCreditsAmount:") == true) {

						 TagParse.TagLongCheck(tag, ref ChangeNpcFactionCreditsAmount);

					}

					//ChangeNpcFactionCreditsTag
					if (tag.Contains("[ChangeNpcFactionCreditsTag:") == true) {

						 TagParse.TagStringCheck(tag, ref ChangeNpcFactionCreditsTag);

					}

					//BuildProjectedBlocks
					if (tag.Contains("[BuildProjectedBlocks:") == true) {

						 TagParse.TagBoolCheck(tag, ref BuildProjectedBlocks);

					}

					//MaxProjectedBlocksToBuild
					if (tag.Contains("[MaxProjectedBlocksToBuild:") == true) {

						 TagParse.TagIntCheck(tag, ref MaxProjectedBlocksToBuild);

					}

					//ForceManualTriggerActivation
					if (tag.Contains("[ForceManualTriggerActivation:") == true) {

						 TagParse.TagBoolCheck(tag, ref ForceManualTriggerActivation);

					}

					//OverwriteAutopilotProfile
					if (tag.Contains("[OverwriteAutopilotProfile:") == true) {

						 TagParse.TagBoolCheck(tag, ref OverwriteAutopilotProfile);

					}

					//OverwriteAutopilotMode
					if (tag.Contains("[OverwriteAutopilotMode:") == true) {

						 TagParse.TagAutoPilotProfileModeCheck(tag, ref OverwriteAutopilotMode);

					}

					//OverwriteAutopilotId
					if (tag.Contains("[OverwriteAutopilotId:") == true) {

						 TagParse.TagStringCheck(tag, ref OverwriteAutopilotId);

					}

					//BroadcastCommandProfiles
					if (tag.Contains("[BroadcastCommandProfiles:") == true) {

						 TagParse.TagBoolCheck(tag, ref BroadcastCommandProfiles);

					}

					//CommandProfileIds
					if (tag.Contains("[CommandProfileIds:") == true) {

						TagParse.TagStringListCheck(tag, ref CommandProfileIds);

					}

					//AddWaypointFromCommand
					if (tag.Contains("[AddWaypointFromCommand:") == true) {

						 TagParse.TagBoolCheck(tag, ref AddWaypointFromCommand);

					}

					//RecalculateDespawnCoords
					if (tag.Contains("[RecalculateDespawnCoords:") == true) {

						 TagParse.TagBoolCheck(tag, ref RecalculateDespawnCoords);

					}

					//AddDatapadsToSeats
					if (tag.Contains("[AddDatapadsToSeats:") == true) {

						 TagParse.TagBoolCheck(tag, ref AddDatapadsToSeats);

					}

					//DatapadNamesToAdd
					if (tag.Contains("[DatapadNamesToAdd:") == true) {

						TagParse.TagStringListCheck(tag, ref DatapadNamesToAdd);

					}

					//DatapadCountToAdd
					if (tag.Contains("[DatapadCountToAdd:") == true) {

						 TagParse.TagIntCheck(tag, ref DatapadCountToAdd);

					}

					//ToggleBlocksOfType
					if (tag.Contains("[ToggleBlocksOfType:") == true) {

						 TagParse.TagBoolCheck(tag, ref ToggleBlocksOfType);

					}

					//BlockTypesToToggle
					if (tag.Contains("[BlockTypesToToggle:") == true) {

						TagParse.TagMyDefIdCheck(tag, ref BlockTypesToToggle);

					}

					//BlockTypeToggles
					if (tag.Contains("[BlockTypeToggles:") == true) {

						TagParse.TagSwitchEnumCheck(tag, ref BlockTypeToggles);

					}

					//CancelWaitingAtWaypoint
					if (tag.Contains("[CancelWaitingAtWaypoint:") == true) {

						 TagParse.TagBoolCheck(tag, ref CancelWaitingAtWaypoint);

					}

					//SwitchToNextWaypoint
					if (tag.Contains("[SwitchToNextWaypoint:") == true) {

						 TagParse.TagBoolCheck(tag, ref SwitchToNextWaypoint);

					}

					//HeavyYaw
					if (tag.Contains("[HeavyYaw:") == true) {

						 TagParse.TagBoolCheck(tag, ref HeavyYaw);

					}

					//StopAllRotation
					if (tag.Contains("[StopAllRotation:") == true) {

						 TagParse.TagBoolCheck(tag, ref StopAllRotation);

					}

					//StopAllThrust
					if (tag.Contains("[StopAllThrust:") == true) {

						 TagParse.TagBoolCheck(tag, ref StopAllThrust);

					}

					//RandomGyroRotation
					if (tag.Contains("[RandomGyroRotation:") == true) {

						 TagParse.TagBoolCheck(tag, ref RandomGyroRotation);

					}

					//RandomThrustDirection
					if (tag.Contains("[RandomThrustDirection:") == true) {

						 TagParse.TagBoolCheck(tag, ref RandomThrustDirection);

					}

					//ParentGridNameRequirement
					if (tag.Contains("ParentGridNameRequirement:") == true) {

						TagParse.TagStringCheck(tag, ref ParentGridNameRequirement);

					}

				}

			}

		}

	}

}
