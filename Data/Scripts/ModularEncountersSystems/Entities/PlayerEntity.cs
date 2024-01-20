using ModularEncountersSystems.BlockLogic;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Progression;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Tasks;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Entities {
	public class PlayerEntity : EntityBase, ITarget {

		public IMyPlayer Player;
		public bool Online;
		public bool IsParentEntityGrid;
		public bool IsParentEntitySeat;
		public bool RemoteControlling;

		public Dictionary<InhibitorTypes, List<MyTuple<IMyRadioAntenna, DateTime>>> InhibitorIdsInRange;

		public bool PlayerEntityChanged;

		public bool ItemConsumeEventRegistered;
		public ConsumableItemTimer JetpackInhibitorNullifier;
		public ConsumableItemTimer DrillInhibitorNullifier;
		public ConsumableItemTimer PlayerInhibitorNullifier;
		public ConsumableItemTimer EnergyInhibitorNullifier;

		public PlayerSolarModule SolarModule;

		public ProgressionContainer Progression {

			get {

				if (_progression == null) {

					if (Player != null) {

						foreach (var progression in ProgressionManager.ProgressionContainers) {

							if (Player.IdentityId == progression.IdentityId && Player.SteamUserId == progression.SteamId) {

								_progression = progression;
								return _progression;

							}

						}

					}

					_progression = new ProgressionContainer();
					_progression.IdentityId = Player?.IdentityId ?? 0;
					_progression.SteamId = Player?.SteamUserId ?? 0;
					ProgressionManager.ProgressionContainers.Add(_progression);

				}

				return _progression;

			}

			set {

				if (value != null)
					_progression = value;
				else if (_progression == null) {

					_progression = new ProgressionContainer();
					_progression.IdentityId = Player?.IdentityId ?? 0;
					_progression.SteamId = Player?.SteamUserId ?? 0;
					ProgressionManager.ProgressionContainers.Add(_progression);

				}
			}
		}

		public ProgressionDataContainer ProgressionData
		{

			get
			{

				if (_progressionData == null)
				{

					if (Player != null)
					{

						foreach (var progression in ProgressionDataManager.ProgressionDataContainersList)
						{

							if (Player.SteamUserId == progression.SteamId)
							{
								progression.IdentityId = Player.IdentityId;
								_progressionData = progression;
								return _progressionData;

							}

						}

					}

					_progressionData = new ProgressionDataContainer();
					_progressionData.IdentityId = Player?.IdentityId ?? 0;
					_progressionData.SteamId = Player?.SteamUserId ?? 0;
					ProgressionDataManager.ProgressionDataContainersList.Add(_progressionData);

				}

				return _progressionData;

			}

			set
			{

				if (value != null)
					_progressionData = value;
				else if (_progressionData == null)
				{
					_progressionData = new ProgressionDataContainer();
					_progressionData.IdentityId = Player?.IdentityId ?? 0;
					_progressionData.SteamId = Player?.SteamUserId ?? 0;
					ProgressionDataManager.ProgressionDataContainersList.Add(_progressionData);

				}


			}
		}


		internal ProgressionContainer _progression;

		internal ProgressionDataContainer _progressionData;

		public List<GridEntity> LinkedGrids;

		public PlayerEntity(IMyPlayer player, IMyEntity entity = null) : base(entity) {

			if (player == null)
				return;

			Type = EntityType.Player;
			IsValidEntity = true;
			Player = player;
			Online = true;

			_progression = null;

			InhibitorIdsInRange = new Dictionary<InhibitorTypes, List<MyTuple<IMyRadioAntenna, DateTime>>>{

				{InhibitorTypes.Drill, new List<MyTuple<IMyRadioAntenna, DateTime>>() },
				{InhibitorTypes.Energy, new List<MyTuple<IMyRadioAntenna, DateTime>>() },
				{InhibitorTypes.Jetpack, new List<MyTuple<IMyRadioAntenna, DateTime>>() },
				{InhibitorTypes.Personnel, new List<MyTuple<IMyRadioAntenna, DateTime>>() },

			};

			LinkedGrids = new List<GridEntity>();

			MyVisualScriptLogicProvider.PlayerDisconnected += PlayerDisconnect;
			MyVisualScriptLogicProvider.PlayerConnected += PlayerConnect;

			MyVisualScriptLogicProvider.PlayerSpawned += PlayerSpawn;
			MyVisualScriptLogicProvider.PlayerDied += PlayerDied;
			MyVisualScriptLogicProvider.PlayerLeftCockpit += PlayerCockpitAction;
			MyVisualScriptLogicProvider.PlayerEnteredCockpit += PlayerCockpitAction;
			MyVisualScriptLogicProvider.RemoteControlChanged += PlayerRemoteAction;
			MyVisualScriptLogicProvider.RespawnShipSpawned += RespawnShipSpawned;

			PlayerConnect(Player.IdentityId);

			RefreshPlayerEntity();
			SpawnLogger.Write("New Player Added To Watcher: " + player.DisplayName, SpawnerDebugEnum.Entity);

		}

		public void AddInhibitorToPlayer(IMyRadioAntenna id, InhibitorTypes type) {

			foreach (var block in InhibitorIdsInRange[type])
				if (block.Item1 == id)
					return;

			InhibitorIdsInRange[type].Add(new MyTuple<IMyRadioAntenna, DateTime>(id, MyAPIGateway.Session.GameDateTime));

		}

		public void RemoveInhibitorFromPlayer(IMyRadioAntenna id, InhibitorTypes type) {

			for (int i = InhibitorIdsInRange[type].Count - 1; i >= 0; i--) {

				var block = InhibitorIdsInRange[type][i];

				if (block.Item1 == id) {

					InhibitorIdsInRange[type].RemoveAt(i);

				}

			}

		}

		public void ItemConsumedEvent(IMyCharacter character, MyDefinitionId id) {

			if (!ActiveEntity() || Player?.Character == null || Player.Character != character)
				return;


			if (id.SubtypeName == "JetpackInhibitorBlocker") {

				if (JetpackInhibitorNullifier == null || !JetpackInhibitorNullifier.EffectActive()) {

					JetpackInhibitorNullifier = new ConsumableItemTimer(30, Player.IdentityId, "Jetpack Inhibitor Nullifier");
					TaskProcessor.Tasks.Add(JetpackInhibitorNullifier);


				} else {

					JetpackInhibitorNullifier.ResetTimer(30);

				}
			
			}

			if (id.SubtypeName == "DrillInhibitorBlocker") {

				if (DrillInhibitorNullifier == null || !DrillInhibitorNullifier.EffectActive()) {

					DrillInhibitorNullifier = new ConsumableItemTimer(30, Player.IdentityId, "Drill Inhibitor Nullifier");
					TaskProcessor.Tasks.Add(DrillInhibitorNullifier);


				} else {

					DrillInhibitorNullifier.ResetTimer(30);

				}

			}

			if (id.SubtypeName == "PlayerInhibitorBlocker") {

				if (PlayerInhibitorNullifier == null || !PlayerInhibitorNullifier.EffectActive()) {

					PlayerInhibitorNullifier = new ConsumableItemTimer(30, Player.IdentityId, "Player Inhibitor Nullifier");
					TaskProcessor.Tasks.Add(PlayerInhibitorNullifier);

				} else {

					PlayerInhibitorNullifier.ResetTimer(30);

				}

			}

			if (id.SubtypeName == "EnergyInhibitorBlocker") {

				if (EnergyInhibitorNullifier == null || !EnergyInhibitorNullifier.EffectActive()) {

					EnergyInhibitorNullifier = new ConsumableItemTimer(30, Player.IdentityId, "Energy Inhibitor Nullifier", true);
					TaskProcessor.Tasks.Add(EnergyInhibitorNullifier);


				} else {

					EnergyInhibitorNullifier.ResetTimer(30);

				}

			}

			if (id.SubtypeName == "SkeletonKey") {
			
				
			
			}

			if (id.SubtypeName == "HackingModule") {



			}

		}

		public Vector3D GetCharacterPosition() {

			return Player?.Character?.WorldAABB.Center ?? Vector3D.Zero;
		
		}

		public void InitSolarModule() {

			if (Progression.SolarChargingSuitUpgradeLevel == 0 || SolarModule != null)
				return;

			SolarModule = new PlayerSolarModule(this);
			TaskProcessor.Tasks.Add(SolarModule);

		}

		public bool IsPlayerStandingCharacter() {

			if (!ActiveEntity())
				return false;

			if (Player?.Character == null || Player.Controller?.ControlledEntity?.Entity == null)
				return false;

			/*
			if (Player.Character.CurrentMovementState.HasFlag(VRage.Game.MyCharacterMovementEnum.Sitting))
				return false;
			*/

			return Player.Character == Player.Controller.ControlledEntity.Entity;
		
		}

		public void PlayerCockpitAction(string entA, long id, string entB) {

			if (id != Player.IdentityId)
				return;

			PlayerEntityChanged = true;

		}

		public void PlayerConnect(long id) {

			if (id != Player.IdentityId)
				return;

			Online = true;
			RegisterConsumeEvent(true);
			SpawnLogger.Write("Player Connected: " + Player.DisplayName, SpawnerDebugEnum.Entity);

		}

		public void PlayerDisconnect(long id) {

			if (id != Player.IdentityId)
				return;

			Online = false;
			RegisterConsumeEvent(false);
			SpawnLogger.Write("Player Disconnected: " + Player.DisplayName, SpawnerDebugEnum.Entity);

		}

		public void RespawnShipSpawned(long shipEntityId, long playerId, string RespawnShipPrefabName)
        {
			if (playerId != Player.IdentityId)
				return;

			ProgressionData.LastRespawnShipName = RespawnShipPrefabName;
        }


		public bool PlayerInPressurizedSeat() {

			if (!IsParentEntitySeat)
				return false;

			var controller = Player?.Controller?.ControlledEntity?.Entity as IMyCockpit;

			if (controller == null)
				return false;

			if (controller.OxygenFilledRatio > 0 && controller.OxygenCapacity > 0)
				return true;

			return false;

		}

		public void PlayerRemoteAction(bool cont, long id, string ent, long endId, string grid, long gridA) {

			if (id != Player.IdentityId)
				return;

			RemoteControlling = cont;
			PlayerEntityChanged = true;

		}

		public void PlayerDied(long id) {

			if (id != Player.IdentityId)
				return;

			JetpackInhibitorNullifier?.ExpireConsumableEffect();
			DrillInhibitorNullifier?.ExpireConsumableEffect();
			PlayerInhibitorNullifier?.ExpireConsumableEffect();
			RegisterConsumeEvent(false);

		}

		public void PlayerSpawn(long id) {

			if (id != Player.IdentityId)
				return;

			PlayerEntityChanged = true;
			RegisterConsumeEvent(true);

		}

		public void RegisterConsumeEvent(bool register) {

			if (register && !ItemConsumeEventRegistered) {

				ItemConsumeEventRegistered = true;
				//TODO += ItemConsumedEvent;

			} else if (!register && ItemConsumeEventRegistered) {

				ItemConsumeEventRegistered = false;
				//TODO -= ItemConsumedEvent;

			}

		}

		public void RefreshPlayerEntity() {

			this.PlayerEntityChanged = false;
			this.IsParentEntityGrid = false;
			this.IsParentEntitySeat = false;

			if (!this.Online)
				return;

			var controlledEntity = Player?.Controller?.ControlledEntity?.Entity;

			if (controlledEntity == null || controlledEntity.Closed || controlledEntity.MarkedForClose) {

				controlledEntity = Player?.Character;

				if (controlledEntity == null || controlledEntity.Closed || controlledEntity.MarkedForClose) {

					Closed = true;
					if (Entity != null && !Entity.Closed && !Entity.MarkedForClose)
						Entity.OnClose -= CloseEntity;
					return;
				
				}
			
			}

			bool reregisterEntityClose = false;

			if (Entity != null && controlledEntity != Entity && !Entity.Closed && !Entity.MarkedForClose) {

				Entity.OnClose -= CloseEntity;
				Entity = null;

			}
			
			var character = controlledEntity as IMyCharacter;

			if (character != null) {

				//RivalAI.Helpers.Logger.Write("Player Character Entity: ", Helpers.BehaviorDebugEnum.BehaviorMode);
				this.Closed = false;
				this.Entity = character;
				this.ParentEntity = character;

			} else {

				var controller = controlledEntity as IMyShipController;

				if (controller != null) {

					//RivalAI.Helpers.Logger.Write("Player Character Entity: ", Helpers.BehaviorDebugEnum.BehaviorMode);
					this.Closed = false;
					this.Entity = controller;
					this.ParentEntity = controller.SlimBlock.CubeGrid;
					this.IsParentEntityGrid = true;
					this.IsParentEntitySeat = (controller as IMyCockpit) != null;

				}

			}

			if (reregisterEntityClose) {

				Entity.OnClose += CloseEntity;

			}

		}

		public void GetPlayerInfo(StringBuilder sb) {

			sb.Append("Player Name:                     ").Append(Player?.DisplayName ?? "null").AppendLine();
			sb.Append("Identity Id:                     ").Append(Player?.IdentityId ?? 0).AppendLine();
			sb.Append("Steam Id:                        ").Append(Player?.SteamUserId ?? 0).AppendLine();
			sb.Append("Online:                          ").Append(Online).AppendLine();
			sb.Append("Closed:                          ").Append(IsClosed()).AppendLine();
			sb.Append("Position:                        ").Append(GetPosition()).AppendLine();

			sb.AppendLine();
			sb.Append("LastRespawnPrefabName:           ").Append(ProgressionData.LastRespawnShipName).AppendLine();
			sb.Append("Tags:                            ").Append(string.Join(",", ProgressionData.Tags)).AppendLine();
			sb.AppendLine();

			foreach (var watchedPlayer in PlayerSpawnWatcher.Players) {

				if (watchedPlayer.Player == this) {

					sb.Append("Space Cargo Ship Timer:          ").Append(watchedPlayer.SpaceCargoShipTimer).AppendLine();
					sb.Append("Planetary Cargo Ship Timer:      ").Append(watchedPlayer.AtmoCargoShipTimer).AppendLine();
					sb.Append("Random Encounter Timer:          ").Append(watchedPlayer.RandomEncounterCheckTimer).AppendLine();
					sb.Append("Random Encounter Cooldown:       ").Append(watchedPlayer.RandomEncounterCoolDownTimer).AppendLine();
					sb.Append("Planetary Installation Timer:    ").Append(watchedPlayer.PlanetaryInstallationCheckTimer).AppendLine();
					sb.Append("Planetary Installation Cooldown: ").Append(watchedPlayer.PlanetaryInstallationCooldownTimer).AppendLine();
					sb.Append("Boss Encounter Timer:            ").Append(watchedPlayer.BossEncounterCheckTimer).AppendLine();
					sb.Append("Boss Encounter Cooldown:         ").Append(watchedPlayer.BossEncounterCooldownTimer).AppendLine();
					sb.Append("Creature Timer:                  ").Append(watchedPlayer.CreatureCheckTimer).AppendLine();

					break;
				
				}
			
			}

			sb.AppendLine();

		}

		//---------------------------------------------------
		//-----------Start Interface Methods-----------------
		//---------------------------------------------------

		public bool ActiveEntity() {

			if (PlayerEntityChanged)
				RefreshPlayerEntity();

			if (IsClosed() || Entity == null) {

				return false;

			}
				

			return true;
		
		}

		public double BroadcastRange(bool onlyAntenna = false) {

			if(IsClosed())
				return 0;

			if (IsParentEntityGrid) {

				var grid = ParentEntity as IMyCubeGrid;

				if (grid == null)
					return 0;

				return EntityEvaluator.GridBroadcastRange(LinkedGrids);

			} else {

				var character = ParentEntity as IMyCharacter;

				if (character == null)
					return 0;

				var controlledEntity = character as Sandbox.Game.Entities.IMyControllableEntity;

				if (controlledEntity == null || !controlledEntity.EnabledBroadcasting)
					return 0;

				return 200; //200 is max range of suit antenna

			}

		}

		public string FactionOwner() {

			var result = "";

			if (!ActiveEntity())
				return result;

			var faction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(Player.IdentityId);

			if (faction == null)
				return result;

			return faction.Tag;

		}

		public double GetCurrentHealth() {

			return MyVisualScriptLogicProvider.GetPlayersHealth(Player?.IdentityId ?? 0);
		
		}

		public override IMyEntity GetEntity() {

			var result = base.GetEntity();

			if ((this.Online && result == null) || PlayerEntityChanged) {

				RefreshPlayerEntity();
				return base.GetEntity();

			}

			return result;
		}

		public override EntityType GetEntityType() {

			return IsParentEntityGrid ? EntityType.Grid : EntityType.Player;

		}

		public List<long> GetOwners(bool onlyGetCurrentEntity = false, bool includeMinorityOwners = false) {

			var result = new List<long>();

			if (!ActiveEntity())
				return result;

			result.Add(Player.IdentityId);
			return result;
		
		}

		public GridOwnershipEnum GetOwnerType() {

			return FactionHelper.IsIdentityPlayer((Player?.IdentityId ?? 0)) ? GridOwnershipEnum.PlayerMajority : GridOwnershipEnum.NpcMajority;
		
		}

		public override bool IsClosed() {

			//If player is not online, they're considered
			//inactive. Otherwise, whatever entity they're
			//in control of is considered instead.

			if (!Online)
				return false;

			return Closed;

		}

		public bool IsPowered() {

			if (!ActiveEntity())
				return false;

			return PowerOutput().Y > 0;

		}

		public bool IsSameGrid(IMyEntity entity) {

			if (!IsParentEntityGrid || !ActiveEntity())
				return false;

			foreach (var grid in LinkedGrids) {

				if (!grid.ActiveEntity())
					continue;

				if (grid.CubeGrid.EntityId == entity.EntityId)
					return true;

			}

			return false;

		}

		public bool IsStatic() {

			return false;
		
		}

		public int MovementScore() {

			if (!ActiveEntity() || IsParentEntityGrid)
				return 0;

			return EntityEvaluator.GridMovementScore(LinkedGrids);

		}

		public string Name() {

			if (!ActiveEntity())
				return "N/A";

			return Player.DisplayName;

		}

		public OwnerTypeEnum OwnerTypes(bool onlyGetCurrentEntity = false, bool includeMinorityOwners = false) {

			return ActiveEntity() ? OwnerTypeEnum.Player : OwnerTypeEnum.Unowned;

		}

		public bool PlayerControlled() {

			if (!ActiveEntity())
				return false;

			return true;

		}

		public Vector2 PowerOutput() {

			if (IsClosed())
				return Vector2.Zero;

			if (IsParentEntityGrid) {

				var grid = ParentEntity as IMyCubeGrid;

				if (grid == null)
					return Vector2.Zero;

				var result = EntityEvaluator.GridPowerOutput(LinkedGrids);

				if (result.Y > 0)
					return result;


				if (!IsParentEntitySeat)
					return Vector2.Zero;

				var controller = Entity as IMyCockpit;

				if(controller?.Pilot == null)
					return Vector2.Zero;

				if(controller.Pilot.SuitEnergyLevel < 0.01)
					return Vector2.Zero;

				return new Vector2(0.009f, 0.009f);

			} else {

				var character = ParentEntity as IMyCharacter;

				if (character == null)
					return Vector2.Zero;

				if(character.SuitEnergyLevel < 0.01)
					return Vector2.Zero;

				return new Vector2(0.009f, 0.009f);

			}

		}

		public RelationTypeEnum RelationTypes(long ownerId, bool onlyGetCurrentEntity = false, bool includeMinorityOwners = false) {

			var owners = GetOwners(onlyGetCurrentEntity, includeMinorityOwners);
			return EntityEvaluator.GetRelationsFromList(ownerId, owners);

		}

		public void RefreshSubGrids() {

			if (IsParentEntityGrid) {

				var grid = ParentEntity as IMyCubeGrid;

				if (grid == null)
					return;

				LinkedGrids = EntityEvaluator.GetAttachedGrids(grid);

			}

		}

		public float TargetValue() {

			if (IsClosed())
				return 0;

			if (IsParentEntityGrid) {

				var grid = ParentEntity as IMyCubeGrid;

				if (grid == null)
					return 0;

				return EntityEvaluator.GridTargetValue(LinkedGrids);

			} else {

				var character = ParentEntity as IMyCharacter;

				if (character == null)
					return 0;

				float threat = 0;

				if (!character.HasInventory)
					return 0;

				var items = new List<VRage.Game.ModAPI.Ingame.MyInventoryItem>();
				character.GetInventory().GetItems(items);

				foreach (var item in items) {

					if (item.Type.TypeId.EndsWith("PhysicalGunObject")) {

						threat += 25;
						continue;

					}

					if (item.Type.TypeId.EndsWith("AmmoMagazine")) {

						threat += 3;
						continue;

					}

					if (item.Type.TypeId.EndsWith("ContainerObject")) {

						threat += 10;
						continue;

					}

				}

				return threat;

			}

		}

		public int WeaponCount() {

			if (IsParentEntityGrid) {

				var grid = ParentEntity as IMyCubeGrid;

				if (grid == null)
					return 0;

				return EntityEvaluator.GridWeaponCount(EntityEvaluator.GetAttachedGrids(grid));

			} else {

				var character = ParentEntity as IMyCharacter;

				if (character == null)
					return 0;

				float count = 0;

				if (!character.HasInventory)
					return 0;

				var items = new List<VRage.Game.ModAPI.Ingame.MyInventoryItem>();
				character.GetInventory().GetItems(items);

				foreach (var item in items) {

					if (item.Type.TypeId.EndsWith("PhysicalGunObject")) {

						count++;
						continue;

					}

				}

			}

			return 0;

		}

		//---------------------------------------------------
		//------------End Interface Methods------------------
		//---------------------------------------------------

		public override void Unload(){

			base.Unload();
			MyVisualScriptLogicProvider.PlayerDisconnected -= PlayerDisconnect;
			MyVisualScriptLogicProvider.PlayerConnected -= PlayerConnect;
			MyVisualScriptLogicProvider.PlayerDied -= PlayerDied;
			MyVisualScriptLogicProvider.PlayerSpawned -= PlayerSpawn;
			MyVisualScriptLogicProvider.PlayerLeftCockpit -= PlayerCockpitAction;
			MyVisualScriptLogicProvider.PlayerEnteredCockpit -= PlayerCockpitAction;
			MyVisualScriptLogicProvider.RemoteControlChanged -= PlayerRemoteAction;
			MyVisualScriptLogicProvider.RespawnShipSpawned -= RespawnShipSpawned;
		}

	}

}
