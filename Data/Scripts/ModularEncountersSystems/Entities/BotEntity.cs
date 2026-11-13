
using System;
using System.Collections.Generic;
using System.Text;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning.Profiles;
using ModularEncountersSystems.World;
using ProtoBuf;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Entities
{

    [ProtoContract]
    public class BotEntity : EntityBase, ITarget
    {
        [ProtoMember(1)] private long CharacterEntityId;

        [ProtoMember(2)] public string BotSpawnProfileName;

        [ProtoMember(3)] public DateTime SpawnTime;

        [ProtoMember(4)] public List<string> Tags;

        [ProtoMember(5)] public bool SpawnedByMES;

        [ProtoIgnore] public IMyCharacter Bot;

        [ProtoIgnore] public IMyPlayer Player;

        [ProtoIgnore] public bool BotEntityChanged;

        [ProtoIgnore] public bool IsParentEntityGrid;

        [ProtoIgnore] public bool IsParentEntitySeat;

        [ProtoIgnore] public List<GridEntity> LinkedGrids;

        [ProtoIgnore] public BotSpawnProfile Profile;

        public BotEntity() : base(null)
        {
            //VRage.Utils.MyLog.Default.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>> BotEntity Constructor (empty)");

            if (!BotManager.Bots.Contains(this))
                BotManager.Bots.Add(this);
        }

        public BotEntity(IMyCharacter character, BotSpawnProfile profile = null, IMyEntity entity = null) : base(entity)
        {
            //VRage.Utils.MyLog.Default.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>> BotEntity Constructor");

            if (character == null)
                return;

            Type = EntityType.Bot;
            CharacterEntityId = character.EntityId;

            if (profile != null)
            {
                Profile = profile;
                BotSpawnProfileName = Profile.ProfileSubtypeId;
                SpawnedByMES = true;
            }
            else
            {
                SpawnedByMES = false;
            }

            Bot = character;
            SpawnTime = MyAPIGateway.Session.GameDateTime;

            if (Player == null)
            {
                var refEntity = Bot.Parent ?? Bot;
                Player = MyAPIGateway.Players.GetPlayerControllingEntity(refEntity);
            }

            LinkedGrids = new List<GridEntity>();
            Tags = new List<string>();

            if (!BotManager.Bots.Contains(this))
                BotManager.Bots.Add(this);

            RefreshBotEntity();
            SpawnLogger.Write("New Bot Added To Watcher: " + Bot.DisplayName, SpawnerDebugEnum.Entity);
        }

        protected override void RegisterHooks()
        {
			Entity.OnClose += CloseEntity;

            BotManager.UnloadEntities += Unload;

            MyVisualScriptLogicProvider.PlayerSpawned += PlayerSpawn;
            MyVisualScriptLogicProvider.PlayerDied += PlayerDied;
            MyVisualScriptLogicProvider.PlayerLeftCockpit += PlayerCockpitAction;
            MyVisualScriptLogicProvider.PlayerEnteredCockpit += PlayerCockpitAction;
        }

        public void RefreshBotEntity()
        {
            IsParentEntityGrid = false;
            BotEntityChanged = false;

            if (Bot == null)
            {
                IMyEntity bot;
                var entityExists = MyAPIGateway.Entities.TryGetEntityById(CharacterEntityId, out bot);
                Bot = bot as IMyCharacter;

                if (!entityExists || Bot == null)
                {
                    Entity.Close();
                    Unload();
                    return;
                }
            }

            if (Player == null)
            {
                var refEntity = Bot.Parent ?? Bot;
                Player = MyAPIGateway.Players.GetPlayerControllingEntity(refEntity);
            }

            if (SpawnedByMES)
            {
                if (Profile == null && BotSpawnProfileName == null || Profile == null && BotSpawnProfileName != null && !ProfileManager.BotSpawnProfiles.TryGetValue(BotSpawnProfileName, out Profile))
                {
                    EjectFromSeat();
                    Bot?.Close();
                    Unload();
                    return;
                }
            }

            if (Tags == null)
                Tags = new List<string>();


            if (Entity != null && Bot != Entity && !Entity.Closed && !Entity.MarkedForClose)
            {
                Entity.OnClose -= CloseEntity;
                Entity = null;
            }

            if (Bot != null)
            {
                Closed = false;
                Entity = Bot;
                ParentEntity = Bot;
            }
            else
            {
                var controller = Bot as IMyShipController;

                if (controller != null)
                {
                    Closed = false;
                    Entity = controller;
                    ParentEntity = controller.SlimBlock.CubeGrid;
                    IsParentEntityGrid = true;
                    IsParentEntitySeat = (controller as IMyCockpit) != null;
                }
            }

            if (!BotManager.Bots.Contains(this))
                BotManager.Bots.Add(this);
        }

        public override void CloseEntity(IMyEntity entity)
        {
			Closed = true;

            if (BotManager.Bots.Contains(this))
                BotManager.Bots.Remove(this);

            base.CloseEntity(entity);
            Unload();
        }

        public override IMyEntity GetEntity()
        {
            var result = base.GetEntity();
            if (result == null || BotEntityChanged)
            {
                RefreshBotEntity();
                return base.GetEntity();
            }

            return result;
        }

        public override EntityType GetEntityType()
        {
            return IsParentEntityGrid ? EntityType.Grid : EntityType.Bot;
        }

        public void EjectFromSeat()
        {
            var seat = Bot.Parent as IMyCockpit;
            if(seat == null)
                return;

            var ent = (MyEntity)Bot;
            using(ent.Pin())
            {
                seat.RemovePilot();
            }
        }

        public void PlayerDied(long id)
        {
            if (Player != null && id != Player.IdentityId)
                return;
        }

        public void PlayerSpawn(long id)
        {
            if (Player != null && id != Player.IdentityId)
                return;

            BotEntityChanged = true;
        }

        public void PlayerCockpitAction(string entA, long id, string entB)
        {
            if (Player != null && id != Player.IdentityId)
                return;

            BotEntityChanged = true;
        }

        public Vector3D GetBotPosition()
        {
            return Bot?.WorldAABB.Center ?? Vector3D.Zero;
        }

        public bool IsBotStandingCharacter()
        {
            if (!ActiveEntity())
                return false;

            if (Bot == null || Bot.UsingEntity != null)
                return false;

            return true;
        }

        public bool ActiveEntity()
        {
            if (BotEntityChanged)
                RefreshBotEntity();

            if (IsClosed() || Entity == null)
                return false;

            return true;
        }

        public void GetBotInfo(StringBuilder sb)
        {
            sb.Append("  - Bot Name:             ").Append(Bot?.DisplayName ?? "null").AppendLine();
            sb.Append("  - Character Id:         ").Append(Bot?.EntityId ?? 0).AppendLine();
            sb.Append("  - Player Id:            ").Append(Player?.IdentityId ?? 0).AppendLine();
            sb.Append("  - Faction:              ").Append(MyAPIGateway.Session.Factions.TryGetPlayerFaction(Player?.IdentityId ?? 0)?.Tag ?? "null").AppendLine();
            sb.Append("  - SpawnedByMES:         ").Append(SpawnedByMES).AppendLine();
            sb.Append("  - Bot Spawner Profile:  ").Append(Profile?.ProfileSubtypeId ?? "null").AppendLine();
            sb.Append("  - Bot Type:             ").Append(Profile?.BotType ?? "null").AppendLine();
            sb.Append("  - WeaponsContainerId:   ").Append(Profile?.WeaponsContainerId ?? "null").AppendLine();
            sb.Append("  - LootContainerId:      ").Append(Profile?.LootContainerId ?? "null").AppendLine();
            sb.Append("  - ChatMessageSetId:     ").Append(Profile?.ChatMessageSetId ?? "null").AppendLine();
            sb.Append("  - Closed:               ").Append(IsClosed()).AppendLine();
            sb.Append("  - Position:             ").Append(GetPosition()).AppendLine();
            sb.Append("  - Tags:                 ").Append(string.Join(",", Tags)).AppendLine();
            sb.AppendLine();
        }

        public string FactionOwner()
        {
            var result = "";

            if (!ActiveEntity())
                return result;

            var faction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(Player.IdentityId);

            if (faction == null)
                return result;

            return faction.Tag;
        }

        public double GetCurrentHealth()
        {
            return MyVisualScriptLogicProvider.GetPlayersHealth(Player?.IdentityId ?? 0);
        }

        public string Name()
        {
            if (!ActiveEntity())
                return "N/A";

            return Bot.DisplayName;
        }

        public float TargetValue()
        {
            if (IsClosed())
                return 0;

            if (!IsBotStandingCharacter())
            {
                var grid = ParentEntity as IMyCubeGrid;

                if (grid == null)
                    return 0;

                return EntityEvaluator.GridTargetValue(LinkedGrids);
            }
            else
            {
                if (Bot == null)
                    return 0;

                float threat = 0;

                if (!Bot.HasInventory)
                    return 0;

                var items = new List<VRage.Game.ModAPI.Ingame.MyInventoryItem>();
                Bot.GetInventory().GetItems(items);

                foreach (var item in items)
                {
                    if (item.Type.TypeId.EndsWith("PhysicalGunObject"))
                    {
                        threat += 25;
                        continue;
                    }

                    if (item.Type.TypeId.EndsWith("AmmoMagazine"))
                    {
                        threat += 3;
                        continue;
                    }

                    if (item.Type.TypeId.EndsWith("ContainerObject"))
                    {
                        threat += 10;
                        continue;
                    }
                }

                return threat;
            }
        }

        public double BroadcastRange(bool onlyAntenna = false)
        {
            if (IsClosed())
                return 0;

            if (IsParentEntityGrid)
            {
                var grid = ParentEntity as IMyCubeGrid;

                if (grid == null)
                    return 0;

                return EntityEvaluator.GridBroadcastRange(LinkedGrids);
            }
            else
            {
                var character = ParentEntity as IMyCharacter;

                if (character == null)
                    return 0;

                var controlledEntity = character as Sandbox.Game.Entities.IMyControllableEntity;

                if (controlledEntity == null || !controlledEntity.EnabledBroadcasting)
                    return 0;

                return 200; //200 is max range of suit antenna
            }
        }

        public List<long> GetOwners(bool onlyGetCurrentEntity = false, bool includeMinorityOwners = false)
        {
            var result = new List<long>();

            if (!ActiveEntity())
                return result;

            result.Add(Player.IdentityId);
            return result;
        }

        public GridOwnershipEnum GetOwnerType()
        {
            return FactionHelper.IsIdentityPlayer(Player?.IdentityId ?? 0) ? GridOwnershipEnum.PlayerMajority : GridOwnershipEnum.NpcMajority;
        }

        public override bool IsClosed()
        {
            return Closed;
        }

        public bool IsPowered()
        {
            if (!ActiveEntity())
                return false;

            return PowerOutput().Y > 0;
        }

        public bool IsSameGrid(IMyEntity entity)
        {
            if (!IsParentEntityGrid || !ActiveEntity())
                return false;

            foreach (var grid in LinkedGrids)
            {

                if (!grid.ActiveEntity())
                    continue;

                if (grid.CubeGrid.EntityId == entity.EntityId)
                    return true;

            }

            return false;
        }

        public bool IsStatic()
        {
            return false;
        }

        public int MovementScore()
        {
            if (!ActiveEntity() || IsParentEntityGrid)
                return 0;

            return EntityEvaluator.GridMovementScore(LinkedGrids);
        }

        public OwnerTypeEnum OwnerTypes(bool onlyGetCurrentEntity = false, bool includeMinorityOwners = false)
        {
            return ActiveEntity() ? OwnerTypeEnum.NPC : OwnerTypeEnum.Unowned;
        }

        public bool PlayerControlled()
        {
            return false;
        }

        public Vector2 PowerOutput()
        {
            if (IsClosed())
                return Vector2.Zero;

            if (IsParentEntityGrid)
            {
                var grid = ParentEntity as IMyCubeGrid;

                if (grid == null)
                    return Vector2.Zero;

                var result = EntityEvaluator.GridPowerOutput(LinkedGrids);

                if (result.Y > 0)
                    return result;


                if (!IsParentEntitySeat)
                    return Vector2.Zero;

                var controller = Entity as IMyCockpit;

                if (controller?.Pilot == null)
                    return Vector2.Zero;

                if (controller.Pilot.SuitEnergyLevel < 0.01)
                    return Vector2.Zero;

                return new Vector2(0.009f, 0.009f);
            }
            else
            {
                var character = ParentEntity as IMyCharacter;

                if (character == null)
                    return Vector2.Zero;

                if (character.SuitEnergyLevel < 0.01)
                    return Vector2.Zero;

                return new Vector2(0.009f, 0.009f);
            }
        }

        public void RefreshSubGrids()
        {
            if (IsParentEntityGrid)
            {
                var grid = ParentEntity as IMyCubeGrid;

                if (grid == null)
                    return;

                LinkedGrids = EntityEvaluator.GetAttachedGrids(grid);
            }
        }

        public RelationTypeEnum RelationTypes(long ownerId, bool onlyGetCurrentEntity = false, bool includeMinorityOwners = false)
        {
            var owners = GetOwners(onlyGetCurrentEntity, includeMinorityOwners);
            return EntityEvaluator.GetRelationsFromList(ownerId, owners);
        }

        public int WeaponCount()
        {
            if(IsParentEntityGrid)
            {
                var grid = ParentEntity as IMyCubeGrid;

                if(grid == null)
                    return 0;

                return EntityEvaluator.GridWeaponCount(EntityEvaluator.GetAttachedGrids(grid));
            }
            else
            {
                var character = ParentEntity as IMyCharacter;
                if(character == null)
                    return 0;

                var inv = character.GetInventory() as MyInventory;
                if(inv == null)
                    return 0;

                int count = 0;
                foreach(var item in inv.GetItems())
                {
                    if(item.Content is MyObjectBuilder_PhysicalGunObject)
                        count++;
                }

                return count;
            }
        }

        public override void Unload()
        {
            BotManager.UnloadEntities -= Unload;

            MyVisualScriptLogicProvider.PlayerDied -= PlayerDied;
            MyVisualScriptLogicProvider.PlayerSpawned -= PlayerSpawn;
            MyVisualScriptLogicProvider.PlayerLeftCockpit -= PlayerCockpitAction;
            MyVisualScriptLogicProvider.PlayerEnteredCockpit -= PlayerCockpitAction;

            base.Unload();
        }
    }
}