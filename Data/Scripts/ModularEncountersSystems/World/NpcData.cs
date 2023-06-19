using ModularEncountersSystems.API;
using ModularEncountersSystems.BlockLogic;
using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.Spawning.Profiles;
using ModularEncountersSystems.Tasks;
using ProtoBuf;
using Sandbox.Game;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Weapons;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.World {

	[Flags]
	public enum NpcAttributes {

		None = 0,
		DigAirTightVoxels = 1,
		ReplenishSystems = 1 << 1,
		IgnoreCleanup = 1 << 2,
		ForceStatic = 1 << 3,
		IsCargoShip = 1 << 4,
		FixSubparts = 1 << 5,
		InitEconomyBlocks = 1 << 6,
		NonPhysicalAmmo = 1 << 7,
		ClearInventory = 1 << 8,
		ShieldActivation = 1 << 9,
		WeaponRandomizationAdjustments = 1 << 10,
		ApplyBehavior = 1 << 11,
		ReleasePrefab = 1 << 12,
		WeaponsRandomized = 1 << 13,
		RivalAiBehaviorSet = 1 << 14,
		RegisterRemoteControlCode = 1 << 15,
		CustomThrustDataUsed = 1 << 16,
		SetMatrixPostSpawn = 1 << 17,
		WeaponRandomizationAggression = 1 << 18,

	}

	[ProtoContract]
	public class NewNpcAttributes {

		[ProtoMember(1)] public bool DigAirTightVoxels;
		[ProtoMember(2)] public bool ReplenishSystems;
		[ProtoMember(3)] public bool IgnoreCleanup;
		[ProtoMember(4)] public bool ForceStatic;
		[ProtoMember(5)] public bool IsCargoShip;
		[ProtoMember(6)] public bool FixSubparts;
		[ProtoMember(7)] public bool InitEconomyBlocks;
		[ProtoMember(8)] public bool NonPhysicalAmmo;
		[ProtoMember(9)] public bool ClearInventory;
		[ProtoMember(10)] public bool ShieldActivation;
		[ProtoMember(11)] public bool WeaponRandomizationAdjustments;
		[ProtoMember(12)] public bool ApplyBehavior;
		[ProtoMember(13)] public bool ReleasePrefab;
		[ProtoMember(14)] public bool WeaponsRandomized;
		[ProtoMember(15)] public bool RivalAiBehaviorSet;
		[ProtoMember(16)] public bool RegisterRemoteControlCode;
		[ProtoMember(17)] public bool CustomThrustDataUsed;
		[ProtoMember(18)] public bool SetMatrixPostSpawn;
		[ProtoMember(19)] public bool WeaponRandomizationAggression;
		[ProtoMember(20)] public bool OldFlagsProcessed;
		[ProtoMember(21)] public bool UseJetpackDisable;
		[ProtoMember(22)] public bool UseDrillDisable;
		[ProtoMember(23)] public bool UsePlayerDisable;
		[ProtoMember(24)] public bool UseNanobotDisable;
		[ProtoMember(25)] public bool UseJumpDisable;
		[ProtoMember(26)] public bool ConfigureTurretControllers;
		[ProtoMember(27)] public bool ReceivedPlayerDamage;
		[ProtoMember(28)] public bool AnnounceSpawnInApi;
		[ProtoMember(29)] public bool ApplyContainerTypes;
		[ProtoMember(30)] public bool UseEnergyDisable;
		[ProtoMember(31)] public bool OwnershipValidation;

		public NewNpcAttributes() {

			DigAirTightVoxels = false;
			ReplenishSystems = false;
			IgnoreCleanup = false;
			ForceStatic = false;
			IsCargoShip = false;
			FixSubparts = false;
			InitEconomyBlocks = false;
			NonPhysicalAmmo = false;
			ClearInventory = false;
			ShieldActivation = false;
			WeaponRandomizationAdjustments = false;
			ApplyBehavior = false;
			ReleasePrefab = false;
			WeaponsRandomized = false;
			RivalAiBehaviorSet = false;
			RegisterRemoteControlCode = false;
			CustomThrustDataUsed = false;
			SetMatrixPostSpawn = false;
			WeaponRandomizationAggression = false;
			OldFlagsProcessed = false;
			UseJetpackDisable = false;
			UseDrillDisable = false;
			UsePlayerDisable = false;
			UseNanobotDisable = false;
			UseJumpDisable = false;
			ConfigureTurretControllers = false;
			ReceivedPlayerDamage = false;
			AnnounceSpawnInApi = false;
			ApplyContainerTypes = false;
			UseEnergyDisable = false;
			OwnershipValidation = false;

		}

		public override string ToString() {

			var sb = new StringBuilder();

			if (DigAirTightVoxels)
				sb.Append("DigAirTightVoxels").Append(", ");

			if (ReplenishSystems)
				sb.Append("ReplenishSystems").Append(", ");

			if (IgnoreCleanup)
				sb.Append("IgnoreCleanup").Append(", ");

			if (ForceStatic)
				sb.Append("ForceStatic").Append(", ");

			if (IsCargoShip)
				sb.Append("IsCargoShip").Append(", ");

			if (FixSubparts)
				sb.Append("FixSubparts").Append(", ");

			if (InitEconomyBlocks)
				sb.Append("InitEconomyBlocks").Append(", ");

			if (NonPhysicalAmmo)
				sb.Append("NonPhysicalAmmo").Append(", ");

			if (ClearInventory)
				sb.Append("ClearInventory").Append(", ");

			if (ShieldActivation)
				sb.Append("ShieldActivation").Append(", ");

			if (WeaponRandomizationAdjustments)
				sb.Append("WeaponRandomizationAdjustments").Append(", ");

			if (ApplyBehavior)
				sb.Append("ApplyBehavior").Append(", ");

			if (ReleasePrefab)
				sb.Append("ReleasePrefab").Append(", ");

			if (WeaponsRandomized)
				sb.Append("WeaponsRandomized").Append(", ");

			if (RivalAiBehaviorSet)
				sb.Append("RivalAiBehaviorSet").Append(", ");

			if (RegisterRemoteControlCode)
				sb.Append("RegisterRemoteControlCode").Append(", ");

			if (CustomThrustDataUsed)
				sb.Append("CustomThrustDataUsed").Append(", ");

			if (SetMatrixPostSpawn)
				sb.Append("SetMatrixPostSpawn").Append(", ");

			if (WeaponRandomizationAggression)
				sb.Append("WeaponRandomizationAggression").Append(", ");

			if(UseJetpackDisable)
				sb.Append("UseJetpackDisable").Append(", ");

			if (UseDrillDisable)
				sb.Append("UseDrillDisable").Append(", ");

			if (UsePlayerDisable)
				sb.Append("UsePlayerDisable").Append(", ");

			if (UseNanobotDisable)
				sb.Append("UseNanobotDisable").Append(", ");

			if (UseJumpDisable)
				sb.Append("UseJumpDisable").Append(", ");

			if (ConfigureTurretControllers)
				sb.Append("ConfigureTurretControllers").Append(", ");

			if(AnnounceSpawnInApi)
				sb.Append("AnnounceSpawnInApi").Append(", ");

			if (ApplyContainerTypes)
				sb.Append("ApplyContainerTypes").Append(", ");

			if (UseEnergyDisable)
				sb.Append("UseEnergyDisable").Append(", ");

			if (OwnershipValidation)
				sb.Append("OwnershipValidation").Append(", ");

			if (OldFlagsProcessed)
				sb.Append("OldFlagsProcessed").Append(", ");

			return sb.ToString();

		}

		public void ApplyAttributesFromFlags(NpcAttributes flags) {

			if (flags.HasFlag(NpcAttributes.DigAirTightVoxels))
				DigAirTightVoxels = true;

			if (flags.HasFlag(NpcAttributes.ReplenishSystems))
				ReplenishSystems = true;

			if (flags.HasFlag(NpcAttributes.IgnoreCleanup))
				IgnoreCleanup = true;

			if (flags.HasFlag(NpcAttributes.ForceStatic))
				ForceStatic = true;

			if (flags.HasFlag(NpcAttributes.IsCargoShip))
				IsCargoShip = true;

			if (flags.HasFlag(NpcAttributes.FixSubparts))
				FixSubparts = true;

			if (flags.HasFlag(NpcAttributes.NonPhysicalAmmo))
				NonPhysicalAmmo = true;

			if (flags.HasFlag(NpcAttributes.ClearInventory))
				ClearInventory = true;

			if (flags.HasFlag(NpcAttributes.ShieldActivation))
				ShieldActivation = true;

			if (flags.HasFlag(NpcAttributes.WeaponRandomizationAdjustments))
				WeaponRandomizationAdjustments = true;

			if (flags.HasFlag(NpcAttributes.ApplyBehavior))
				ApplyBehavior = true;

			if (flags.HasFlag(NpcAttributes.ReleasePrefab))
				ReleasePrefab = true;

			if (flags.HasFlag(NpcAttributes.WeaponsRandomized))
				WeaponsRandomized = true;

			if (flags.HasFlag(NpcAttributes.RivalAiBehaviorSet))
				RivalAiBehaviorSet = true;

			if (flags.HasFlag(NpcAttributes.RegisterRemoteControlCode))
				RegisterRemoteControlCode = true;

			if (flags.HasFlag(NpcAttributes.CustomThrustDataUsed))
				CustomThrustDataUsed = true;

			if (flags.HasFlag(NpcAttributes.SetMatrixPostSpawn))
				SetMatrixPostSpawn = true;

			if (flags.HasFlag(NpcAttributes.WeaponRandomizationAggression))
				WeaponRandomizationAggression = true;

		}

	}

	[ProtoContract]
	public class NpcData {

		//Serialized Data
		[ProtoMember(1)]
		public NpcAttributes OldAttributes;

		[ProtoMember(2)]
		public NpcAttributes OldAppliedAttributes;

		[ProtoMember(3)]
		public SpawningType SpawnType;

		[ProtoMember(4)]
		public string SpawnGroupName;

		[ProtoMember(5)]
		public int ConditionIndex;

		[ProtoMember(6)]
		public int ZoneIndex;

		[ProtoMember(7)]
		public string OriginalPrefabId;

		[ProtoMember(8)]
		public string SpawnerPrefabId;

		[ProtoMember(9)]
		public string BehaviorName;

		[ProtoMember(10)]
		public float BehaviorTriggerDist;

		[ProtoMember(11)]
		public Vector3D StartCoords;

		[ProtoMember(12)]
		public Vector3D EndCoords;

		[ProtoMember(13)]
		public DateTime SpawnTime;

		[ProtoMember(14)]
		public string InitialFaction;

		[ProtoMember(15)]
		public bool FirstAttributesCheck;

		[ProtoMember(16)]
		public bool SecondAttributesCheck;

		[ProtoMember(17)]
		public bool ThirdAttributesCheck;

		[ProtoMember(18)]
		public int DespawnAttempts;

		[ProtoMember(19)]
		public double PrefabSpeed;

		[ProtoMember(20)]
		private string DespawnSource;

		[ProtoMember(21)]
		public bool SpawnedByMES;

		[ProtoMember(22)]
		public long PrimaryRemoteControlId;

		[ProtoMember(23)]
		public Vector3D Forward;

		[ProtoMember(24)]
		public Vector3D Up;

		[ProtoMember(25)]
		public DateTime LastChangeToData;

		[ProtoMember(26)]
		public NewNpcAttributes Attributes;

		[ProtoMember(27)]
		public NewNpcAttributes AppliedAttributes;

		[ProtoMember(28)]
		public string UniqueSpawnIdentifier;

		[ProtoMember(29)]
		public string FriendlyName;

		[ProtoMember(30)]
		public string ChatAuthorName;

		[ProtoMember(31)]
		public List<string> CustomTags;

		[ProtoMember(32)]
		public SpawningType OriginalSpawnType;

		[ProtoMember(33)]
		public string BehaviorTerminationReason;

		[ProtoMember(34)]
		public string OriginalOwnerFaction;

		[ProtoMember(35)]
		public long OriginalOwnerId;

		[ProtoMember(36)]
		public DateTime SpawnGroupTimeStamp;

		[ProtoMember(37)]
		public List<int> ManipulationsUsed;

		//Non-Serialized Data

		[ProtoIgnore]
		public ImprovedSpawnGroup SpawnGroup {

			get {

				if (_spawnGroup == null) {

					_spawnGroup = SpawnGroupManager.GetSpawnGroupByName(SpawnGroupName);
				
				}

				return _spawnGroup;

			}

		}

		[ProtoIgnore]
		private ImprovedSpawnGroup _spawnGroup;

		[ProtoIgnore]
		public SpawnConditionsProfile Conditions {

			get {

				if (_condition == null) {

					if (SpawnGroup != null && ConditionIndex < SpawnGroup.SpawnConditionsProfiles.Count)
						_condition = SpawnGroup.SpawnConditionsProfiles[ConditionIndex];

				}

				return _condition;

			}

		}

		[ProtoIgnore]
		private SpawnConditionsProfile _condition;

		[ProtoIgnore]
		public ZoneConditionsProfile ZoneConditions {

			get {

				if (_zoneCondition == null) {

					if (_condition != null) {

						if (ZoneIndex >= 0 && ZoneIndex < _condition.ZoneConditions.Count)
							_zoneCondition = _condition.ZoneConditions[ZoneIndex];

					}

				}

				return _zoneCondition;

			}

		}

		[ProtoIgnore]
		private ZoneConditionsProfile _zoneCondition;

		[ProtoIgnore]
		public double SecondsSinceSpawn;

		[ProtoIgnore]
		public DateTime SessionTime;

		[ProtoIgnore]
		public double SecondsSinceSessionLive;

		[ProtoIgnore]
		public GridEntity Grid;

		[ProtoIgnore]
		public List<Action<IMyCubeGrid, string>> DespawnActions;

		[ProtoIgnore]
		public bool CargoShipDriftCheck;

		[ProtoIgnore]
		public Vector3 CargoShipDriftVelocity;

		[ProtoIgnore]
		public BoolEnum KeenEconomyStation;

		[ProtoIgnore]
		public IMyRemoteControl PrimaryRemoteControl;

		public NpcData() {

			SetDefaults();

		}

		public NpcData(LegacyActiveNPC legacyActiveNPC) {

			SetDefaults();

			this.SpawnGroupName = legacyActiveNPC.SpawnGroupName;
			this.InitialFaction = legacyActiveNPC.InitialFaction;
			this.StartCoords = legacyActiveNPC.StartCoords;
			this.EndCoords = legacyActiveNPC.EndCoords;
			this.PrefabSpeed = legacyActiveNPC.AutoPilotSpeed;

			if (Conditions == null)
				return;

			if (legacyActiveNPC.SpawnType == "SpaceCargoShip")
				this.SpawnType = SpawningType.SpaceCargoShip;

			if (legacyActiveNPC.SpawnType == "RandomEncounter")
				this.SpawnType = SpawningType.RandomEncounter;

			if (legacyActiveNPC.SpawnType == "PlanetaryCargoShip")
				this.SpawnType = SpawningType.PlanetaryCargoShip;

			if (legacyActiveNPC.SpawnType == "PlanetaryInstallation")
				this.SpawnType = SpawningType.PlanetaryInstallation;

			if (legacyActiveNPC.SpawnType == "BossEncounter")
				this.SpawnType = SpawningType.BossEncounter;

			if (legacyActiveNPC.SpawnType == "Other" || legacyActiveNPC.SpawnType == "OtherNPC")
				this.SpawnType = SpawningType.OtherNPC;

			AssignAttributes(SpawnGroup, SpawnType);

			//CleanUp
			if (legacyActiveNPC.CleanupIgnore && !Attributes.IgnoreCleanup)
				Attributes.IgnoreCleanup = true;

		}

		private void SetDefaults() {

			Attributes = new NewNpcAttributes();
			AppliedAttributes = new NewNpcAttributes();

			Attributes.FixSubparts = true;
			SpawnType = SpawningType.None;
			SpawnGroupName = "";
			OriginalPrefabId = "";
			SpawnerPrefabId = "";
			BehaviorName = "";
			BehaviorTriggerDist = 0;
			StartCoords = Vector3D.Zero;
			EndCoords = Vector3D.Zero;
			SpawnTime = MyAPIGateway.Session.GameDateTime;
			LastChangeToData = MyAPIGateway.Session.GameDateTime;
			InitialFaction = "";
			FirstAttributesCheck = false;
			SecondAttributesCheck = false;
			ThirdAttributesCheck = false;
			DespawnAttempts = 0;
			PrimaryRemoteControlId = 0;
			Forward = Vector3D.Forward;
			Up = Vector3D.Up;
			UniqueSpawnIdentifier = "";
			CustomTags = new List<string>();
			OriginalSpawnType = SpawningType.None;
			BehaviorTerminationReason = "";
			OriginalOwnerFaction = "";
			OriginalOwnerId = 0;
			SpawnGroupTimeStamp = DateTime.MinValue;
			ManipulationsUsed = new List<int>();

			_spawnGroup = null;
			SecondsSinceSpawn = 0;
			SessionTime = MyAPIGateway.Session.GameDateTime;
			SecondsSinceSessionLive = 0;
			Grid = null;
			KeenEconomyStation = BoolEnum.None;

		}

		public void AssignAttributes(ImprovedSpawnGroup spawnGroup, SpawningType type) {
			
			if(SpawnRequest.IsCargoShip(type))
				Attributes.IsCargoShip = true;

			if (spawnGroup.SpawnConditionsProfiles[ConditionIndex].CutVoxelsAtAirtightCells)
				Attributes.DigAirTightVoxels = true;

			if (spawnGroup.ReplenishSystems)
				Attributes.ReplenishSystems = true;

			if (spawnGroup.IgnoreCleanupRules)
				Attributes.IgnoreCleanup = true;

			if (spawnGroup.SpawnConditionsProfiles[ConditionIndex].ForceStaticGrid)
				Attributes.ForceStatic = true;

			if (spawnGroup.InitializeStoreBlocks)
				Attributes.InitEconomyBlocks = true;

			if (spawnGroup.UseNonPhysicalAmmo || Settings.Grids.UseNonPhysicalAmmoForNPCs)
				Attributes.NonPhysicalAmmo = true;

			if (Settings.Grids.RemoveContainerInventoryFromNPCs)
				Attributes.ClearInventory = true;

			Attributes.ReleasePrefab = true;

		}

		public void ProcessPrimaryAttributes() {

			if (Grid == null || !Grid.ActiveEntity())
				return;

			var updateSettings = true;
			FirstAttributesCheck = true;

			if (!AppliedAttributes.OldFlagsProcessed) {

				AppliedAttributes.OldFlagsProcessed = true;
				Attributes.ApplyAttributesFromFlags(OldAttributes);
				AppliedAttributes.ApplyAttributesFromFlags(OldAppliedAttributes);
			
			}

			SpawnLogger.Write("Processing Primary Attributes For Grid: " + Grid.CubeGrid.CustomName, SpawnerDebugEnum.PostSpawn);

			//ForceStatic
			if (AttributeCheck(Attributes.ForceStatic, AppliedAttributes.ForceStatic)) {

				Grid.CubeGrid.IsStatic = true;
				SpawnLogger.Write(Grid.CubeGrid.CustomName + " Forced To Static Grid After Spawn.", SpawnerDebugEnum.Spawning);
				AppliedAttributes.ForceStatic = true;

			}

			//ReleasePrefab
			if (AttributeCheck(Attributes.ReleasePrefab, AppliedAttributes.ReleasePrefab)) {

				foreach (var prefab in PrefabSpawner.Prefabs) {

					if (prefab.PrefabSubtypeId == this.SpawnerPrefabId) {

						prefab.SpawningInProgress = false;
						break;

					}

				}

				AppliedAttributes.ReleasePrefab = true;

			}

			if (updateSettings)
				Update();

			MyAPIGateway.Utilities.InvokeOnGameThread(() => ProcessSecondaryAttributes());

			

		}

		public void ProcessSecondaryAttributes() {

			if (Grid == null || !Grid.ActiveEntity())
				return;

			SecondAttributesCheck = true;
			var updateSettings = true;

			SpawnLogger.Write("Processing Secondary Attributes For Grid: " + Grid.CubeGrid.CustomName, SpawnerDebugEnum.PostSpawn);

			if (AttributeCheck(Attributes.ForceStatic, AppliedAttributes.ForceStatic, true)) {

				Grid.CubeGrid.IsStatic = true;
				SpawnLogger.Write(Grid.CubeGrid.CustomName + " Forced To Static Grid After Spawn (Secondary Check).", SpawnerDebugEnum.Spawning);
				AppliedAttributes.ForceStatic = true;

			}

			//SetMatrixPostSpawn
			if (AttributeCheck(Attributes.SetMatrixPostSpawn, AppliedAttributes.SetMatrixPostSpawn)) {
				/*
				SpawnLogger.Write("Setting Matrix Post Spawn", SpawnerDebugEnum.Spawning);
				SpawnLogger.Write("Start Matrix Translation:    " + Grid.CubeGrid.WorldMatrix.Translation, SpawnerDebugEnum.Spawning);
				SpawnLogger.Write("Start Matrix Forward:        " + Grid.CubeGrid.WorldMatrix.Forward, SpawnerDebugEnum.Spawning);
				SpawnLogger.Write("Start Matrix Up:             " + Grid.CubeGrid.WorldMatrix.Up, SpawnerDebugEnum.Spawning);
				

				var newMatrix = MatrixD.CreateWorld(StartCoords, Forward, Up);
				Grid.CubeGrid.IsStatic = false;
				Grid.CubeGrid.PositionComp.SetWorldMatrix(ref newMatrix);
				Grid.CubeGrid.IsStatic = true;
				//MyVisualScriptLogicProvider.ShowNotificationToAll("Fix MAtrix", 1000);

				
				SpawnLogger.Write("Provided Matrix Translation: " + newMatrix.Translation, SpawnerDebugEnum.Spawning);
				SpawnLogger.Write("Provided Matrix Forward:     " + newMatrix.Forward, SpawnerDebugEnum.Spawning);
				SpawnLogger.Write("Provided Matrix Up:          " + newMatrix.Up, SpawnerDebugEnum.Spawning);
				SpawnLogger.Write("Final Matrix Translation:    " + Grid.CubeGrid.WorldMatrix.Translation, SpawnerDebugEnum.Spawning);
				SpawnLogger.Write("Final Matrix Forward:        " + Grid.CubeGrid.WorldMatrix.Forward, SpawnerDebugEnum.Spawning);
				SpawnLogger.Write("Final Matrix Up:             " + Grid.CubeGrid.WorldMatrix.Up, SpawnerDebugEnum.Spawning);
				*/
				AppliedAttributes.SetMatrixPostSpawn = true;

			}

			//ReplenishSystems
			if (AttributeCheck(Attributes.ReplenishSystems, AppliedAttributes.ReplenishSystems)) {

				InventoryHelper.ReplenishGridSystems(Grid.CubeGrid, SpawnGroup);
				AppliedAttributes.ReplenishSystems = true;

			}

			//IgnoreCleanup
			if (!Attributes.IgnoreCleanup && !string.IsNullOrWhiteSpace(Grid.CubeGrid.CustomName) && Grid.CubeGrid.CustomName.Contains("[NPC-IGNORE]"))
				Attributes.IgnoreCleanup = true;

			//FixSubparts
			if (Attributes.FixSubparts) {

				AppliedAttributes.FixSubparts = true;

				foreach (var block in Grid.AllTerminalBlocks) {

					if (block == null || !block.ActiveEntity()) {

						if (block.Block as IMyDoor != null || block.Block as IMyUserControllableGun != null || block.Block as IMyConveyorSorter != null) {

							var color = block.Block.SlimBlock.ColorMaskHSV;
							Grid.CubeGrid.ColorBlocks(block.Block.Min, block.Block.Min, new Vector3(42, 41, 40));
							SafeVisualUpdate(block.Block.SlimBlock);
							Grid.CubeGrid.ColorBlocks(block.Block.Min, block.Block.Min, color);
							SafeVisualUpdate(block.Block.SlimBlock);

							if (MyAPIGateway.Utilities.IsDedicated && block.Block as IMyLargeTurretBase != null && !BlockManager.AllWeaponCoreBlocks.Contains(block.Block.SlimBlock.BlockDefinition.Id)) {

								DebugHelper.FireTurretInSafeDirection(block.Block as IMyLargeTurretBase);

							}

						}

					}

				}

			}

			//WeaponRandomizationAdjustments
			if (AttributeCheck(Attributes.WeaponRandomizationAdjustments, AppliedAttributes.WeaponRandomizationAdjustments, true) && !AppliedAttributes.WeaponRandomizationAggression) {

				TaskProcessor.Tasks.Add(new WeaponRandomizedGrids(Grid));
				AppliedAttributes.WeaponRandomizationAdjustments = true;

			}

			//ApplyBehavior
			if (AttributeCheck(Attributes.ApplyBehavior, AppliedAttributes.ApplyBehavior)) {

				AppliedAttributes.ApplyBehavior = true;

				if (string.IsNullOrEmpty(Grid.CubeGrid.Name) == true) {

					MyVisualScriptLogicProvider.SetName(Grid.CubeGrid.EntityId, Grid.CubeGrid.EntityId.ToString());

				}

				BehaviorLogger.Write("Keen Behavior Initialized", BehaviorDebugEnum.BehaviorSetup);
				MyVisualScriptLogicProvider.SetDroneBehaviourFull(Grid.CubeGrid.EntityId.ToString(), this.BehaviorName, true, false, null, false, null, 10, this.BehaviorTriggerDist);

			}

			//AnnounceSpawnInApi
			if (AttributeCheck(true, AppliedAttributes.AnnounceSpawnInApi)) {

				AppliedAttributes.AnnounceSpawnInApi = true;
				LocalApi.SuccessfulSpawnEvent?.Invoke(Grid?.CubeGrid);

			}

			if (updateSettings)
				Update();

		}

		private void SafeVisualUpdate(IMySlimBlock block) {

			if (block == null)
				return;

			try {

				block.UpdateVisual();
			
			} catch (Exception e) {

				SpawnLogger.Write("Block Failed To Update Visuals using IMySlimBlock.UpdateVisual(): " + block?.BlockDefinition?.Id.ToString() ?? "null definition", SpawnerDebugEnum.Error, true);
				SpawnLogger.Write(e.ToString(), SpawnerDebugEnum.Error, true);

			}
		
		}

		public void ProcessTertiaryAttributes() {

			if (Grid == null || !Grid.ActiveEntity())
				return;

			ThirdAttributesCheck = true;
			var updateSettings = true;

			SpawnLogger.Write("Processing Tertiary Attributes For Grid: " + Grid.CubeGrid.CustomName, SpawnerDebugEnum.PostSpawn);

			//DigAirTightVoxels
			if (AttributeCheck(Attributes.DigAirTightVoxels, AppliedAttributes.DigAirTightVoxels)) {

				TaskProcessor.Tasks.Add(new CutVoxels(Grid, SpawnGroup?.SpawnConditionsProfiles[ConditionIndex].CutVoxelSize ?? 2.7));
				AppliedAttributes.DigAirTightVoxels = true;

			}

			//NonPhysicalAmmo
			if (AttributeCheck(Attributes.NonPhysicalAmmo, AppliedAttributes.NonPhysicalAmmo)) {

				AppliedAttributes.NonPhysicalAmmo = true;
				InventoryHelper.NonPhysicalAmmoProcessing(Grid.CubeGrid);

			}

			//ShieldActivation
			if (AttributeCheck(Attributes.ShieldActivation, AppliedAttributes.ShieldActivation)) {

				NPCShieldManager.ActivateShieldsForNPC(Grid.CubeGrid, true);
				//AppliedAttributes |= NpcAttributes.ShieldActivation;

			}

			//ApplyContainerTypes
			if (AttributeCheck(Attributes.ApplyContainerTypes, AppliedAttributes.ApplyContainerTypes)) {

				AppliedAttributes.ApplyContainerTypes = true;
				InventoryHelper.ApplyContainerTypes(Grid);

			}

			//ConfigureTurretControllers
			if (AttributeCheck(Attributes.ConfigureTurretControllers, AppliedAttributes.ConfigureTurretControllers)) {

				AppliedAttributes.ConfigureTurretControllers = true;
				SpawnLogger.Write("Starting Turret Controller Config For Vanilla Weapons", SpawnerDebugEnum.PostSpawn);
				var turretControllers = new List<BlockEntity>();
				var tools = new List<BlockEntity>();
				Grid.GetBlocksOfType<IMyTurretControlBlock>(turretControllers, false);
				Grid.GetBlocksOfType<IMyUserControllableGun>(tools, false);
				Grid.GetBlocksOfType<IMyShipToolBase>(tools, false);
				SpawnLogger.Write(string.Format("[Controllers : {0}] [Weapons/Tools : {1}]", turretControllers.Count, tools.Count), SpawnerDebugEnum.PostSpawn);

				foreach (var controller in turretControllers) {

					string key = "";

					if (controller?.Block?.Storage == null || !controller.Block.Storage.TryGetValue(StorageTools.MesTurretControllerKey, out key)) {

						//SpawnLogger.Write("Controller Key Not Found", SpawnerDebugEnum.PostSpawn);
						continue;

					}
						
					var controllerBlock = controller.Block as IMyTurretControlBlock;

					if (controllerBlock == null) {

						//SpawnLogger.Write("Controller Block Null", SpawnerDebugEnum.PostSpawn);
						continue;

					}
						
					for (int i = tools.Count - 1; i >= 0; i--) {

						string blockKey = "";

						if (tools[i]?.Block?.Storage == null || !tools[i].Block.Storage.TryGetValue(StorageTools.MesTurretControllerKey, out blockKey)) {

							//SpawnLogger.Write("Block Key Not Found", SpawnerDebugEnum.PostSpawn);
							continue;

						}

						if (blockKey == key) {

							SpawnLogger.Write(string.Format("[Controller : {0}] [Linked Tool : {1}]", controllerBlock.CustomName, tools[i].Block.CustomName), SpawnerDebugEnum.PostSpawn);
							controllerBlock.AddTool(tools[i].FunctionalBlock);
							tools.RemoveAt(i);

						} else {

							//SpawnLogger.Write(string.Format("Keys Not Matched: [{0}] [{1}]", controllerBlock.CustomName, tools[i].Block.CustomName), SpawnerDebugEnum.PostSpawn);

						}
					
					}

				}

			}

			Grid.RefreshSubGrids();

			//OwnershipValidation
			if (Attributes.OwnershipValidation && !AppliedAttributes.OwnershipValidation && SpawnGroup.MesSpawnGroup) {

				AppliedAttributes.OwnershipValidation = true;

				if (Grid.CubeGrid.BigOwners.Count == 0 && OriginalOwnerId != 0) {

					var originalFactionTag = MyAPIGateway.Session.Factions.TryGetPlayerFaction(OriginalOwnerId)?.Tag ?? "Nobody";
					var currentFactionTag = "Nobody";

					SpawnLogger.Write(string.Format("Ship From " + SpawnGroupName + " Spawned With Wrong Ownership. Expected [{0}] ; Got [{1}]. Attempting Correction.", originalFactionTag, currentFactionTag), SpawnerDebugEnum.Error, true);

					var gridList = new List<IMyCubeGrid>();
					MyAPIGateway.GridGroups.GetGroup(Grid.CubeGrid, GridLinkTypeEnum.Physical, gridList);

					foreach (var grid in gridList) {

						grid.ChangeGridOwnership(OriginalOwnerId, VRage.Game.MyOwnershipShareModeEnum.Faction);

					}

				} else if (Grid.CubeGrid.BigOwners.Count > 0 && Grid.CubeGrid.BigOwners[0] != OriginalOwnerId) {

					var originalFactionTag = MyAPIGateway.Session.Factions.TryGetPlayerFaction(OriginalOwnerId)?.Tag ?? "Nobody";
					var currentFactionTag = MyAPIGateway.Session.Factions.TryGetPlayerFaction((Grid.CubeGrid.BigOwners.Count > 0) ? Grid.CubeGrid.BigOwners[0] : 0)?.Tag ?? "Nobody";
					SpawnLogger.Write(string.Format("Ship From " + SpawnGroupName + " Spawned With Wrong Ownership. Expected [{0}] ; Got [{1}].", originalFactionTag, currentFactionTag), SpawnerDebugEnum.Error, true);

				}


			}

			//InitEconomyBlocks
			if (AttributeCheck(Attributes.InitEconomyBlocks, AppliedAttributes.InitEconomyBlocks)) {

				AppliedAttributes.InitEconomyBlocks = true;
				EconomyHelper.InitNpcStoreBlock(Grid.CubeGrid, SpawnGroup);

			}

			//InhibitorActivation
			if (Attributes.UseJetpackDisable && !AppliedAttributes.UseJetpackDisable && !HasInhibitor("Jetpack")) {

				var randBlock = Grid.RandomTerminalBlock();

				if (randBlock.ActiveEntity()) {

					var inhibitorLogic = new JetpackInhibitor(randBlock);
					Grid.Inhibitors.Add(randBlock);

				}
				
			} else {

				AppliedAttributes.UseJetpackDisable = true;

			}

			if (Attributes.UseDrillDisable && !AppliedAttributes.UseDrillDisable && !HasInhibitor("Drill")) {

				var randBlock = Grid.RandomTerminalBlock();

				if (randBlock.ActiveEntity()) {

					var inhibitorLogic = new DrillInhibitor(randBlock);
					Grid.Inhibitors.Add(randBlock);

				}

			} else {

				AppliedAttributes.UseDrillDisable = true;

			}

			if (Attributes.UseJumpDisable && !AppliedAttributes.UseJumpDisable && !HasInhibitor("JumpDrive")) {

				var randBlock = Grid.RandomTerminalBlock();

				if (randBlock.ActiveEntity()) {

					var inhibitorLogic = new JumpDriveInhibitor(randBlock);
					Grid.Inhibitors.Add(randBlock);

				}

			} else {

				AppliedAttributes.UseJumpDisable = true;

			}

			if (Attributes.UseNanobotDisable && !AppliedAttributes.UseNanobotDisable && !HasInhibitor("Nanobot")) {

				var randBlock = Grid.RandomTerminalBlock();

				if (randBlock.ActiveEntity()) {

					var inhibitorLogic = new NanobotInhibitor(randBlock);
					Grid.Inhibitors.Add(randBlock);

				}

			} else {

				AppliedAttributes.UseNanobotDisable = true;

			}

			if (Attributes.UsePlayerDisable && !AppliedAttributes.UsePlayerDisable && !HasInhibitor("Player")) {

				var randBlock = Grid.RandomTerminalBlock();

				if (randBlock.ActiveEntity()) {

					var inhibitorLogic = new PlayerInhibitor(randBlock);
					Grid.Inhibitors.Add(randBlock);

				}

			} else {

				AppliedAttributes.UsePlayerDisable = true;

			}

			if (updateSettings)
				Update();

		}

		public bool HasInhibitor(string type) {

			for (int i = Grid.LinkedGrids.Count - 1; i >= 0; i--) {

				if (!Grid.LinkedGrids[i].ActiveEntity())
					continue;

				for (int j = Grid.LinkedGrids[i].Inhibitors.Count - 1; j >= 0; j--) {

					var block = Grid.LinkedGrids[i].Inhibitors[j];

					if (block == null || !block.ActiveEntity())
						continue;

					if (!block.Block.SlimBlock.BlockDefinition.Id.SubtypeName.Contains(type))
						continue;

					var functional = block.Block as IMyFunctionalBlock;

					if (functional != null)
						functional.Enabled = true;

					return true;

				}

			}

			return false;

		}

		private bool AttributeCheck(bool attribute, bool appliedAttribute, bool force = false) {

			return attribute && (!appliedAttribute || force);

		}

		public void Update() {

			if (Grid != null && MyAPIGateway.Session != null && Grid.ActiveEntity() && Grid.Npc == this) {

				LastChangeToData = MyAPIGateway.Session.GameDateTime;
				SerializationHelper.SaveDataToEntity<NpcData>(Grid?.CubeGrid, this, StorageTools.NpcDataKey);

			}

		}

		public override string ToString() {

			var sb = new StringBuilder();

			sb.Append(" - Attributes:          ").Append(Attributes.ToString()).AppendLine();
			sb.Append(" - AppliedAttributes:   ").Append(AppliedAttributes.ToString()).AppendLine();
			sb.Append(" - SpawnType:           ").Append(SpawnType.ToString()).AppendLine();
			sb.Append(" - OriginalSpawnType:   ").Append(OriginalSpawnType.ToString()).AppendLine();
			sb.Append(" - SpawnGroupName:      ").Append(!string.IsNullOrWhiteSpace(SpawnGroupName) ? SpawnGroupName : "N/A").AppendLine();
			sb.Append(" - Mod Name:            ").Append(SpawnGroup?.SpawnGroup?.Context?.ModName ?? "Unknown or Vanilla").AppendLine();
			sb.Append(" - Mod ID:              ").Append(SpawnGroup?.SpawnGroup?.Context?.ModId ?? "0").AppendLine();
			sb.Append(" - ConditionIndex:      ").Append(ConditionIndex.ToString()).AppendLine();
			sb.Append(" - ZoneIndex:           ").Append(ZoneIndex.ToString()).AppendLine();
			sb.Append(" - ZoneCondition:       ").Append(ZoneIndex >= 0 ? (!string.IsNullOrWhiteSpace(ZoneConditions?.ProfileSubtypeId) ? ZoneConditions.ProfileSubtypeId : "Profile Id Null or Empty") : "N/A").AppendLine();
			sb.Append(" - OriginalPrefabId:    ").Append(!string.IsNullOrWhiteSpace(OriginalPrefabId) ? OriginalPrefabId : "N/A").AppendLine();
			sb.Append(" - SpawnerPrefabId:     ").Append(!string.IsNullOrWhiteSpace(SpawnerPrefabId) ? SpawnerPrefabId : "N/A").AppendLine();
			sb.Append(" - BehaviorName:        ").Append(!string.IsNullOrWhiteSpace(BehaviorName) ? BehaviorName : "N/A").AppendLine();
			sb.Append(" - BehaviorTriggerDist: ").Append(BehaviorTriggerDist.ToString()).AppendLine();
			sb.Append(" - StartCoords:         ").Append(StartCoords.ToString()).AppendLine();
			sb.Append(" - Start Coords Dist:   ").Append(Vector3D.Distance(StartCoords, Grid.GetPosition())).AppendLine();
			var startEndDir = Vector3D.Normalize(EndCoords - StartCoords);
			var startPosDir = Vector3D.Normalize(Grid.GetPosition() - StartCoords);
			sb.Append(" - Start/End Angle:     ").Append(VectorHelper.GetAngleBetweenDirections(startEndDir, startPosDir)).AppendLine();
			sb.Append(" - EndCoords:           ").Append(EndCoords.ToString()).AppendLine();
			sb.Append(" - End Coords Dist      ").Append(Vector3D.Distance(EndCoords, Grid.GetPosition())).AppendLine();
			var endStartDir = Vector3D.Normalize(StartCoords - EndCoords);
			var endPosDir = Vector3D.Normalize(Grid.GetPosition() - EndCoords);
			sb.Append(" - Start/End Angle:     ").Append(VectorHelper.GetAngleBetweenDirections(endStartDir, endPosDir)).AppendLine();
			sb.Append(" - SpawnTime:           ").Append(SpawnTime != null ? SpawnTime.ToString() : "N/A").AppendLine();
			sb.Append(" - LastChangeToData:    ").Append(LastChangeToData != null ? LastChangeToData.ToString() : "N/A").AppendLine();
			sb.Append(" - InitialFaction:      ").Append(!string.IsNullOrWhiteSpace(InitialFaction) ? InitialFaction : "N/A").AppendLine();
			sb.Append(" - DespawnAttempts:     ").Append(DespawnAttempts.ToString()).AppendLine();
			sb.Append(" - PrefabSpeed:         ").Append(PrefabSpeed.ToString()).AppendLine();
			sb.Append(" - DespawnSource:       ").Append(!string.IsNullOrWhiteSpace(DespawnSource) ? DespawnSource : "N/A").AppendLine();
			sb.Append(" - SpawnedByMES:        ").Append(SpawnedByMES.ToString()).AppendLine();
			sb.Append(" - OriginalFaction:     ").Append(OriginalOwnerFaction).AppendLine();
			sb.Append(" - OriginalOwnerId:     ").Append(OriginalOwnerId).AppendLine();

			return sb.ToString();
			
		}

	}

}
