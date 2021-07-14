using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
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
		CustomThrustDataUsed = 1 << 16

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
			InitialFaction = "";
			FirstAttributesCheck = false;
			SecondAttributesCheck = false;
			ThirdAttributesCheck = false;
			DespawnAttempts = 0;
			PrimaryRemoteControlId = 0;

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

			//ForceStatic
			if (Attributes.HasFlag(NpcAttributes.ForceStatic)) {

				Grid.CubeGrid.IsStatic = true;
				AppliedAttributes |= NpcAttributes.ForceStatic;

			}

			//ReleasePrefab
			if (Attributes.HasFlag(NpcAttributes.ReleasePrefab)) {

				foreach (var prefab in PrefabSpawner.Prefabs) {

					if (prefab.PrefabSubtypeId == this.SpawnerPrefabId) {

						prefab.SpawningInProgress = false;
						break;

					}

				}

				AppliedAttributes |= NpcAttributes.ReleasePrefab;

			}

			MyAPIGateway.Utilities.InvokeOnGameThread(() => ProcessSecondaryAttributes());
			if (updateSettings)
				Update();

		}

		public void ProcessSecondaryAttributes() {

			if (Grid == null || !Grid.ActiveEntity())
				return;

			SecondAttributesCheck = true;
			var updateSettings = true;

			//ReplenishSystems
			if (Attributes.HasFlag(NpcAttributes.ReplenishSystems)) {

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
			if (Attributes.HasFlag(NpcAttributes.WeaponRandomizationAdjustments)) {

				WeaponRandomizer.SetWeaponCoreRandomRanges(Grid.CubeGrid);
				AppliedAttributes |= NpcAttributes.WeaponRandomizationAdjustments;

			}

			//ApplyBehavior
			if (Attributes.HasFlag(NpcAttributes.ApplyBehavior)) {

				AppliedAttributes |= NpcAttributes.ApplyBehavior;

				if (string.IsNullOrEmpty(Grid.CubeGrid.Name) == true) {

					MyVisualScriptLogicProvider.SetName(Grid.CubeGrid.EntityId, Grid.CubeGrid.EntityId.ToString());

				}

				MyVisualScriptLogicProvider.SetDroneBehaviourFull(Grid.CubeGrid.EntityId.ToString(), this.BehaviorName, true, false, null, false, null, 10, this.BehaviorTriggerDist);

			}

			//RegisterRemoteControlCode
			if (Attributes.HasFlag(NpcAttributes.RegisterRemoteControlCode)) {

				WeaponRandomizer.SetWeaponCoreRandomRanges(Grid.CubeGrid);
				AppliedAttributes |= NpcAttributes.RegisterRemoteControlCode;

			}

		}

		public void ProcessTertiaryAttributes() {

			if (Grid == null || !Grid.ActiveEntity())
				return;

			ThirdAttributesCheck = true;
			var updateSettings = true;

			//DigAirTightVoxels
			if (Attributes.HasFlag(NpcAttributes.DigAirTightVoxels)) {

				TaskProcessor.Tasks.Add(new CutVoxels(Grid, SpawnGroup?.SpawnConditionsProfiles[ConditionIndex].CutVoxelSize ?? 2.7));
				AppliedAttributes |= NpcAttributes.DigAirTightVoxels;

			}

			//InitEconomyBlocks
			if (Attributes.HasFlag(NpcAttributes.InitEconomyBlocks)) {

				AppliedAttributes |= NpcAttributes.InitEconomyBlocks;
				EconomyHelper.InitNpcStoreBlock(Grid.CubeGrid, SpawnGroup);

			}

			//NonPhysicalAmmo
			if (Attributes.HasFlag(NpcAttributes.NonPhysicalAmmo)) {

				AppliedAttributes |= NpcAttributes.NonPhysicalAmmo;
				InventoryHelper.NonPhysicalAmmoProcessing(Grid.CubeGrid);

			}

			//ShieldActivation
			if (Attributes.HasFlag(NpcAttributes.ShieldActivation)) {

				NPCShieldManager.ActivateShieldsForNPC(Grid.CubeGrid);
				AppliedAttributes |= NpcAttributes.ShieldActivation;

			}

			if (updateSettings)
				Update();

		}

		private void Update() {

			var byteData = MyAPIGateway.Utilities.SerializeToBinary<NpcData>(this);
			var stringData = Convert.ToBase64String(byteData);

			if (Grid != null && !Grid.ActiveEntity()) {

				if (Grid.CubeGrid.Storage == null)
					Grid.CubeGrid.Storage = new MyModStorageComponent();

				if (Grid.CubeGrid.Storage.ContainsKey(StorageTools.NpcDataKey))
					Grid.CubeGrid.Storage[StorageTools.NpcDataKey] = stringData;
				else
					Grid.CubeGrid.Storage.Add(StorageTools.NpcDataKey, stringData);

			}

		}

	}

}
