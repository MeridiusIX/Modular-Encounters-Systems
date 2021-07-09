using ModularEncountersSystems.Logging;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
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

		public bool PlayerEntityChanged;

		public bool JetpackInhibitorNullifierActive;
		public DateTime LastJetpackInhibitorNullifierTime;

		public bool DrillInhibitorNullifierActive;
		public DateTime LastDrillInhibitorNullifierTime;

		public bool PlayerInhibitorNullifierActive;
		public DateTime LastPlayerInhibitorNullifierTime;

		public List<GridEntity> LinkedGrids;

		public PlayerEntity(IMyPlayer player, IMyEntity entity = null) : base(entity) {

			if (player == null) 
				return;

			Type = EntityType.Player;
			IsValidEntity = true;
			Player = player;
			Online = true;

			LinkedGrids = new List<GridEntity>();

			MyVisualScriptLogicProvider.PlayerDisconnected += PlayerDisconnect;
			MyVisualScriptLogicProvider.PlayerConnected += PlayerConnect;

			MyVisualScriptLogicProvider.PlayerSpawned += PlayerSpawn;
			MyVisualScriptLogicProvider.PlayerLeftCockpit += PlayerCockpitAction;
			MyVisualScriptLogicProvider.PlayerEnteredCockpit += PlayerCockpitAction;
			MyVisualScriptLogicProvider.RemoteControlChanged += PlayerRemoteAction;

			RefreshPlayerEntity();
			SpawnLogger.Write("New Player Added To Watcher: " + player.DisplayName, SpawnerDebugEnum.Entity);

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
			SpawnLogger.Write("Player Connected: " + Player.DisplayName, SpawnerDebugEnum.Entity);

		}

		public void PlayerDisconnect(long id) {

			if (id != Player.IdentityId)
				return;

			Online = false;
			SpawnLogger.Write("Player Disconnected: " + Player.DisplayName, SpawnerDebugEnum.Entity);

		}

		public void PlayerRemoteAction(bool cont, long id, string ent, long endId, string grid, long gridA) {

			if (id != Player.IdentityId)
				return;

			PlayerEntityChanged = true;

		}

		public void PlayerSpawn(long id) {

			if (id != Player.IdentityId)
				return;

			PlayerEntityChanged = true;

		}

		public void RefreshPlayerEntity() {

			this.PlayerEntityChanged = false;
			this.IsParentEntityGrid = false;
			this.IsParentEntitySeat = false;

			if (!this.Online)
				return;

			if (Player?.Controller?.ControlledEntity?.Entity == null)
				return;

			if (Player.Controller.ControlledEntity.Entity.Closed || Player.Controller.ControlledEntity.Entity.MarkedForClose)
				return;

			if (Entity != null && !Entity.Closed && !Entity.MarkedForClose)
				Entity.OnClose -= (e) => { Closed = true; };

			var character = Player.Controller.ControlledEntity.Entity as IMyCharacter;

			if (character != null) {

				//RivalAI.Helpers.Logger.Write("Player Character Entity: ", Helpers.BehaviorDebugEnum.BehaviorMode);
				this.Closed = false;
				this.Entity = character;
				this.ParentEntity = character;
				return;

			}

			var controller = Player.Controller.ControlledEntity.Entity as IMyShipController;

			if (controller != null) {

				//RivalAI.Helpers.Logger.Write("Player Character Entity: ", Helpers.BehaviorDebugEnum.BehaviorMode);
				this.Closed = false;
				this.Entity = controller;
				this.ParentEntity = controller.SlimBlock.CubeGrid;
				this.IsParentEntityGrid = true;
				this.IsParentEntitySeat = (controller as IMyCockpit) != null;
				return;

			}

		}

		public void ToString(StringBuilder sb) {

			sb.Append("Player Name: ").Append(Player.DisplayName).AppendLine();
			sb.Append("Identity Id: ").Append(Player.IdentityId).AppendLine();
			sb.Append("Steam Id:    ").Append(Player.SteamUserId).AppendLine();
			sb.Append("Online:      ").Append(Online).AppendLine();
			sb.Append("Closed:      ").Append(IsClosed()).AppendLine();
			sb.AppendLine();

		}

		//---------------------------------------------------
		//-----------Start Interface Methods-----------------
		//---------------------------------------------------

		public bool ActiveEntity() {

			if (PlayerEntityChanged)
				RefreshPlayerEntity();

			if (IsClosed() || !Online) {

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
			MyVisualScriptLogicProvider.PlayerSpawned -= PlayerSpawn;
			MyVisualScriptLogicProvider.PlayerLeftCockpit -= PlayerCockpitAction;
			MyVisualScriptLogicProvider.PlayerEnteredCockpit -= PlayerCockpitAction;
			MyVisualScriptLogicProvider.RemoteControlChanged -= PlayerRemoteAction;

		}

	}

}
