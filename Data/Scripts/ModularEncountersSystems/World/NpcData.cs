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
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
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
	public class NpcData {

		//Serialized Data
		[ProtoMember(1)]
		public NpcAttributes Attributes;

		[ProtoMember(2)]
		public NpcAttributes AppliedAttributes;

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
		public string DespawnSource;

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
			if (legacyActiveNPC.CleanupIgnore && !Attributes.HasFlag(NpcAttributes.IgnoreCleanup))
				Attributes |= NpcAttributes.IgnoreCleanup;

		}

		private void SetDefaults() {

			Attributes = NpcAttributes.None;
			Attributes |= NpcAttributes.FixSubparts;
			AppliedAttributes = NpcAttributes.None;
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

			_spawnGroup = null;
			SecondsSinceSpawn = 0;
			SessionTime = MyAPIGateway.Session.GameDateTime;
			SecondsSinceSessionLive = 0;
			Grid = null;
			KeenEconomyStation = BoolEnum.None;

		}

		public void AssignAttributes(ImprovedSpawnGroup spawnGroup, SpawningType type) {
			
			if(SpawnRequest.IsCargoShip(type))
				Attributes |= NpcAttributes.IsCargoShip;

			if (spawnGroup.SpawnConditionsProfiles[ConditionIndex].CutVoxelsAtAirtightCells)
				Attributes |= NpcAttributes.DigAirTightVoxels;

			if (spawnGroup.ReplenishSystems)
				Attributes |= NpcAttributes.ReplenishSystems;

			if (spawnGroup.IgnoreCleanupRules)
				Attributes |= NpcAttributes.IgnoreCleanup;

			if(spawnGroup.SpawnConditionsProfiles[ConditionIndex].ForceStaticGrid)
				Attributes |= NpcAttributes.ForceStatic;

			if (spawnGroup.InitializeStoreBlocks)
				Attributes |= NpcAttributes.InitEconomyBlocks;

			if (spawnGroup.UseNonPhysicalAmmo || Settings.Grids.UseNonPhysicalAmmoForNPCs)
				Attributes |= NpcAttributes.NonPhysicalAmmo;

			if (Settings.Grids.RemoveContainerInventoryFromNPCs)
				Attributes |= NpcAttributes.ClearInventory;

			Attributes |= NpcAttributes.ReleasePrefab;

		}

		public void ProcessPrimaryAttributes() {

			if (Grid == null || !Grid.ActiveEntity())
				return;

			var updateSettings  = true;
			FirstAttributesCheck = true;

			SpawnLogger.Write("Processing Primary Attributes For Grid: " + Grid.CubeGrid.CustomName, SpawnerDebugEnum.PostSpawn);

			//ForceStatic
			if (AttributeCheck(NpcAttributes.ForceStatic)) {

				Grid.CubeGrid.IsStatic = true;
				SpawnLogger.Write(Grid.CubeGrid.CustomName + " Forced To Static Grid After Spawn.", SpawnerDebugEnum.Spawning);
				AppliedAttributes |= NpcAttributes.ForceStatic;

			}

			//ReleasePrefab
			if (AttributeCheck(NpcAttributes.ReleasePrefab)) {

				foreach (var prefab in PrefabSpawner.Prefabs) {

					if (prefab.PrefabSubtypeId == this.SpawnerPrefabId) {

						prefab.SpawningInProgress = false;
						break;

					}

				}

				AppliedAttributes |= NpcAttributes.ReleasePrefab;

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

			if (AttributeCheck(NpcAttributes.ForceStatic, true)) {

				Grid.CubeGrid.IsStatic = true;
				SpawnLogger.Write(Grid.CubeGrid.CustomName + " Forced To Static Grid After Spawn (Secondary Check).", SpawnerDebugEnum.Spawning);
				AppliedAttributes |= NpcAttributes.ForceStatic;

			}

			//SetMatrixPostSpawn
			if (AttributeCheck(NpcAttributes.SetMatrixPostSpawn)) {
				/*
				SpawnLogger.Write("Setting Matrix Post Spawn", SpawnerDebugEnum.Spawning);
				SpawnLogger.Write("Start Matrix Translation:    " + Grid.CubeGrid.WorldMatrix.Translation, SpawnerDebugEnum.Spawning);
				SpawnLogger.Write("Start Matrix Forward:        " + Grid.CubeGrid.WorldMatrix.Forward, SpawnerDebugEnum.Spawning);
				SpawnLogger.Write("Start Matrix Up:             " + Grid.CubeGrid.WorldMatrix.Up, SpawnerDebugEnum.Spawning);
				*/

				var newMatrix = MatrixD.CreateWorld(StartCoords, Forward, Up);
				Grid.CubeGrid.IsStatic = false;
				Grid.CubeGrid.PositionComp.SetWorldMatrix(ref newMatrix);
				Grid.CubeGrid.IsStatic = true;
				//MyVisualScriptLogicProvider.ShowNotificationToAll("Fix MAtrix", 1000);

				/*
				SpawnLogger.Write("Provided Matrix Translation: " + newMatrix.Translation, SpawnerDebugEnum.Spawning);
				SpawnLogger.Write("Provided Matrix Forward:     " + newMatrix.Forward, SpawnerDebugEnum.Spawning);
				SpawnLogger.Write("Provided Matrix Up:          " + newMatrix.Up, SpawnerDebugEnum.Spawning);
				SpawnLogger.Write("Final Matrix Translation:    " + Grid.CubeGrid.WorldMatrix.Translation, SpawnerDebugEnum.Spawning);
				SpawnLogger.Write("Final Matrix Forward:        " + Grid.CubeGrid.WorldMatrix.Forward, SpawnerDebugEnum.Spawning);
				SpawnLogger.Write("Final Matrix Up:             " + Grid.CubeGrid.WorldMatrix.Up, SpawnerDebugEnum.Spawning);
				*/
				AppliedAttributes |= NpcAttributes.SetMatrixPostSpawn;

			}

			//ReplenishSystems
			if (AttributeCheck(NpcAttributes.ReplenishSystems)) {

				InventoryHelper.ReplenishGridSystems(Grid.CubeGrid, SpawnGroup);
				AppliedAttributes |= NpcAttributes.ReplenishSystems;

			}

			//IgnoreCleanup
			if (!Attributes.HasFlag(NpcAttributes.IgnoreCleanup) && !string.IsNullOrWhiteSpace(Grid.CubeGrid.CustomName) && Grid.CubeGrid.CustomName.Contains("[NPC-IGNORE]"))
				Attributes |= NpcAttributes.IgnoreCleanup;

			//FixSubparts
			if (Attributes.HasFlag(NpcAttributes.FixSubparts)) {

				AppliedAttributes |= NpcAttributes.FixSubparts;

				foreach (var block in Grid.AllTerminalBlocks) {

					if (block == null || !block.ActiveEntity()) {

						if (block.Block as IMyDoor != null || block.Block as IMyUserControllableGun != null || block.Block as IMyConveyorSorter != null) {

							var color = block.Block.SlimBlock.ColorMaskHSV;
							Grid.CubeGrid.ColorBlocks(block.Block.Min, block.Block.Min, new Vector3(42, 41, 40));
							block.Block.SlimBlock.UpdateVisual();
							Grid.CubeGrid.ColorBlocks(block.Block.Min, block.Block.Min, color);
							block.Block.SlimBlock.UpdateVisual();

						}

					}

				}

			}

			//WeaponRandomizationAdjustments
			if (AttributeCheck(NpcAttributes.WeaponRandomizationAdjustments, true) && !AppliedAttributes.HasFlag(NpcAttributes.WeaponRandomizationAggression)) {

				TaskProcessor.Tasks.Add(new WeaponRandomizedGrids(Grid));
				AppliedAttributes |= NpcAttributes.WeaponRandomizationAdjustments;

			}

			//ApplyBehavior
			if (AttributeCheck(NpcAttributes.ApplyBehavior)) {

				AppliedAttributes |= NpcAttributes.ApplyBehavior;

				if (string.IsNullOrEmpty(Grid.CubeGrid.Name) == true) {

					MyVisualScriptLogicProvider.SetName(Grid.CubeGrid.EntityId, Grid.CubeGrid.EntityId.ToString());

				}

				BehaviorLogger.Write("Keen Behavior Initialized", BehaviorDebugEnum.BehaviorSetup);
				MyVisualScriptLogicProvider.SetDroneBehaviourFull(Grid.CubeGrid.EntityId.ToString(), this.BehaviorName, true, false, null, false, null, 10, this.BehaviorTriggerDist);

			}

			if (updateSettings)
				Update();

		}

		public void ProcessTertiaryAttributes() {

			if (Grid == null || !Grid.ActiveEntity())
				return;

			ThirdAttributesCheck = true;
			var updateSettings = true;

			SpawnLogger.Write("Processing Tertiary Attributes For Grid: " + Grid.CubeGrid.CustomName, SpawnerDebugEnum.PostSpawn);

			//DigAirTightVoxels
			if (AttributeCheck(NpcAttributes.DigAirTightVoxels)) {

				TaskProcessor.Tasks.Add(new CutVoxels(Grid, SpawnGroup?.SpawnConditionsProfiles[ConditionIndex].CutVoxelSize ?? 2.7));
				AppliedAttributes |= NpcAttributes.DigAirTightVoxels;

			}

			//InitEconomyBlocks
			if (AttributeCheck(NpcAttributes.InitEconomyBlocks)) {

				AppliedAttributes |= NpcAttributes.InitEconomyBlocks;
				EconomyHelper.InitNpcStoreBlock(Grid.CubeGrid, SpawnGroup);

			}

			//NonPhysicalAmmo
			if (AttributeCheck(NpcAttributes.NonPhysicalAmmo)) {

				AppliedAttributes |= NpcAttributes.NonPhysicalAmmo;
				InventoryHelper.NonPhysicalAmmoProcessing(Grid.CubeGrid);

			}

			//ShieldActivation
			if (AttributeCheck(NpcAttributes.ShieldActivation)) {

				NPCShieldManager.ActivateShieldsForNPC(Grid.CubeGrid);
				AppliedAttributes |= NpcAttributes.ShieldActivation;

			}

			if (updateSettings)
				Update();

		}

		private bool AttributeCheck(NpcAttributes attribute, bool force = false) {

			return Attributes.HasFlag(attribute) && (!AppliedAttributes.HasFlag(attribute) || force);

		}

		public void Update() {

			if (Grid != null && Grid.ActiveEntity() && Grid.Npc == this) {

				//ThisIsBreakingShit;
				LastChangeToData = MyAPIGateway.Session.GameDateTime;
				SerializationHelper.SaveDataToEntity<NpcData>(Grid.CubeGrid, this, StorageTools.NpcDataKey);

			}

		}

		public override string ToString() {

			var sb = new StringBuilder();

			sb.Append(" - Attributes:          ").Append(Attributes.ToString()).AppendLine();
			sb.Append(" - AppliedAttributes:   ").Append(AppliedAttributes.ToString()).AppendLine();
			sb.Append(" - SpawnType:           ").Append(SpawnType.ToString()).AppendLine();
			sb.Append(" - SpawnGroupName:      ").Append(!string.IsNullOrWhiteSpace(SpawnGroupName) ? SpawnGroupName : "N/A").AppendLine();
			sb.Append(" - ConditionIndex:      ").Append(ConditionIndex.ToString()).AppendLine();
			sb.Append(" - ZoneIndex:           ").Append(ZoneIndex.ToString()).AppendLine();
			sb.Append(" - OriginalPrefabId:    ").Append(!string.IsNullOrWhiteSpace(OriginalPrefabId) ? OriginalPrefabId : "N/A").AppendLine();
			sb.Append(" - SpawnerPrefabId:     ").Append(!string.IsNullOrWhiteSpace(SpawnerPrefabId) ? SpawnerPrefabId : "N/A").AppendLine();
			sb.Append(" - BehaviorName:        ").Append(!string.IsNullOrWhiteSpace(BehaviorName) ? BehaviorName : "N/A").AppendLine();
			sb.Append(" - BehaviorTriggerDist: ").Append(BehaviorTriggerDist.ToString()).AppendLine();
			sb.Append(" - StartCoords:         ").Append(StartCoords.ToString()).AppendLine();
			sb.Append(" - EndCoords:           ").Append(EndCoords.ToString()).AppendLine();
			sb.Append(" - SpawnTime:           ").Append(SpawnTime != null ? SpawnTime.ToString() : "N/A").AppendLine();
			sb.Append(" - LastChangeToData:    ").Append(LastChangeToData != null ? LastChangeToData.ToString() : "N/A").AppendLine();
			sb.Append(" - InitialFaction:      ").Append(!string.IsNullOrWhiteSpace(InitialFaction) ? InitialFaction : "N/A").AppendLine();
			sb.Append(" - DespawnAttempts:     ").Append(DespawnAttempts.ToString()).AppendLine();
			sb.Append(" - PrefabSpeed:         ").Append(PrefabSpeed.ToString()).AppendLine();
			sb.Append(" - DespawnSource:       ").Append(!string.IsNullOrWhiteSpace(DespawnSource) ? DespawnSource : "N/A").AppendLine();
			sb.Append(" - SpawnedByMES:        ").Append(SpawnedByMES.ToString()).AppendLine();

			return sb.ToString();
			
		}

	}

}
