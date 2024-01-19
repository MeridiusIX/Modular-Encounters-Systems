using Sandbox.Game.Entities;
using Sandbox.ModAPI;

using System;
using System.Collections.Generic;

using VRage;
using VRage.Game;
using VRage.Game.ModAPI;

using VRageMath;
using ProtoBuf;
using VRage.Utils;

namespace ModularEncountersSystems.API 
{
  public class RemoteBotAPI
  {
    //Create Instance of this object in your SessionComponent LoadData() method.
    public RemoteBotAPI()
    {
      MyAPIGateway.Utilities.RegisterMessageHandler(_botControllerModChannel, ReceiveModMessage);
    }

    /// <summary>
    /// Call this in your Unload to ensure the Message Handler is unregistered properly
    /// </summary>
    public void Close()
    {
      try
      {
        MyAPIGateway.Utilities.UnregisterMessageHandler(_botControllerModChannel, ReceiveModMessage);
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLineAndConsole($"Exception in AiEnabled.RemoteBotAPI.Close: {ex}");
      }
    }

    /// <summary>
    /// This class is used to provide user customized settings for a spawned bot.
    /// 
    /// How to use: 
    /// Create and cache ONE instance of the class.
    /// Before spawning your bot(s), update the values as desired.
    /// Serialize to binary using: byte[] data = MyAPIGateway.Utilities.SerializeToBinary(yourObject) (you can cache and reuse this as well!).
    /// Pass the serialized data into the spawn method and I'll take it from there!
    /// </summary>
    [ProtoContract]
    public class SpawnData
    {
      /// <summary>
      /// Whether or not the bot is allowed to fly
      /// </summary>
      [ProtoMember(1)] public bool CanUseAirNodes = true;

      /// <summary>
      /// Whether or not the bot is allowed to fly in Space
      /// </summary>
      [ProtoMember(2)] public bool CanUseSpaceNodes = true;

      /// <summary>
      /// Whether or not the bot should prefer to walk when possible
      /// </summary>
      [ProtoMember(3)] public bool UseGroundNodesFirst = true;

      /// <summary>
      /// Whether or not the bot is allowed to swim
      /// </summary>
      [ProtoMember(4)] public bool CanUseWaterNodes = true;

      /// <summary>
      /// If true, the bot will ONLY be able to swim (ie fish / water creature)
      /// </summary>
      [ProtoMember(5)] public bool WaterNodesOnly = false;

      /// <summary>
      /// Whether or not the bot is allowed to climb ladders
      /// </summary>
      [ProtoMember(6)] public bool CanUseLadders = true;

      /// <summary>
      /// Whether or not the bot is allowed to be seated
      /// </summary>
      [ProtoMember(7)] public bool CanUseSeats = true;

      /// <summary>
      /// The number of Ticks before the bot auto-despawns. Set to zero to disable auto-despawn.
      /// Friendly bots do not auto-despawn.
      /// </summary>
      [ProtoMember(8)] public uint DespawnTicks = 15000;

      /// <summary>
      /// The DisplayName to give the bot.
      /// </summary>
      [ProtoMember(9)] public string DisplayName;

      /// <summary>
      /// The SubtypeId of the Bot you want to Spawn. See <see cref="GetBotSubtypes"/> for valid types
      /// </summary>
      [ProtoMember(10)] public string BotSubtype;

      /// <summary>
      /// Which role to give the bot. See <see cref="GetFriendlyBotRoles"/>, <see cref="GetNPCBotRoles"/>, or <see cref="GetNeutralBotRoles"/> for valid roles. If not supplied, it will be determined by the <see cref="BotSubtype"/>'s default usage
      /// </summary>
      [ProtoMember(11)] public string BotRole;

      /// <summary>
      /// The color for the bot in RGB format
      /// </summary>
      [ProtoMember(12)] public Color? Color;

      /// <summary>
      /// A custom sound to use when the bot dies
      /// </summary>
      [ProtoMember(13)] public string DeathSound;

      /// <summary>
      /// Custom sounds to use when the bot punches its target (not used in rifle attacks)
      /// </summary>
      [ProtoMember(14)] public List<string> AttackSounds;

      /// <summary>
      /// Custom sounds to use when the bot takes damage
      /// </summary>
      [ProtoMember(15)] public List<string> PainSounds;

      /// <summary>
      /// These sounds are played randomly when the bot does NOT have a target
      /// </summary>
      [ProtoMember(16)] public List<string> IdleSounds;

      /// <summary>
      /// These actions (emotes) are performed randomly by the Nomad bot
      /// </summary>
      [ProtoMember(17)] public List<string> Actions;

      /// <summary>
      /// This is the angle (in degrees) that the bot's projectiles can vary from its target
      /// </summary>
      [ProtoMember(18)] public float ShotDeviationAngle = 1.5f;

      /// <summary>
      /// If true, bots will lead targets when shooting
      /// </summary>
      [ProtoMember(19)] public bool LeadTargets;

      /// <summary>
      /// This is the SubtypeId for the weapon or tool you want to give the bot. 
      /// If the bot is unable to use the specified type, the default type will be used instead.
      /// </summary>
      [ProtoMember(20)] public string ToolSubtypeId;

      /// <summary>
      /// Bots will receive the loot from the specified loot container, provided the subtype is valid.
      /// If not, the bot will receive its default loot.
      /// This is an SBC definition, so you can create your own custom loot containers and use those.
      /// </summary>
      [ProtoMember(21)] public string LootContainerSubtypeId;

      /// <summary>
      /// This is a list of SubtypeIds for the weapon or tool you want to give the bot.
      /// A random subtype will be chosen from among the valid subtypes for the bot.
      /// If the bot is unable to use any of the specified types, the default type will be used instead.
      /// </summary>
      [ProtoMember(22)] public List<string> ToolSubtypeIdList;

      /// <summary>
      /// These sounds are played randomly when the bot is pursuing a target
      /// </summary>
      [ProtoMember(23)] public List<string> TauntSounds;

      /// <summary>
      /// If true, bots will be able to attack doors and weapon systems
      /// </summary>
      [ProtoMember(24)] public bool CanDamageGrids;

      /// <summary>
      /// If true, the bot will be killed if it's found outside of its starting map area
      /// </summary>
      [ProtoMember(25)] public bool ConfineToMap;

      /// <summary>
      /// If true, the bot's helmet visor will open / close automatically depending on oxygen level
      /// </summary>
      [ProtoMember(26)] public bool AllowHelmetVisorChanges = true;

      /// <summary>
      /// If true, the bot will be able to roam freely. Also requires server setting to be true.
      /// </summary>
      [ProtoMember(27)] public bool AllowMapTransitions = true;

      /// <summary>
      /// If true, the bot will wander around its map area when idle
      /// </summary>
      [ProtoMember(28)] public bool AllowIdleMovement = true;

      /// <summary>
      /// A list of priorities for repair bots to use when deciding which block to weld. Bots will use the default list not supplied.
      /// See <see cref="GetDefaultRepairPriorities"/> for allowable entries.
      /// Remove entries you want bots to ignore completely.
      /// </summary>
      [ProtoMember(29)] public List<string> RepairPriorities;

      /// <summary>
      /// A list of priorities for bots to use when deciding what to attack. Bots will use the default list if this is not supplied.
      /// See <see cref="GetDefaultTargetPriorities"/> for allowable entries.
      /// Remove entries you want bots to ignore completely.
      /// </summary>
      [ProtoMember(30)] public List<string> TargetPriorities;
    }

    [ProtoContract]
    public abstract class Priorities
    {
      [ProtoMember(1)] public List<KeyValuePair<string, bool>> PriorityTypes;

      public Priorities() { }

      internal void AssignDefaults()
      {
        PriorityTypes = new List<KeyValuePair<string, bool>>()
        {
          new KeyValuePair<string, bool>("IMyUserControllableGun", true),
          new KeyValuePair<string, bool>("IMyShipController", true),
          new KeyValuePair<string, bool>("IMyPowerProducer", true),
          new KeyValuePair<string, bool>("IMyThrust", true),
          new KeyValuePair<string, bool>("IMyGyro", true),
          new KeyValuePair<string, bool>("IMyProductionBlock", true),
          new KeyValuePair<string, bool>("IMyDoor", true),
          new KeyValuePair<string, bool>("IMyProgrammableBlock", true),
          new KeyValuePair<string, bool>("IMyProjector", true),
          new KeyValuePair<string, bool>("IMyConveyor", true),
          new KeyValuePair<string, bool>("IMyCargoContainer", true),
          new KeyValuePair<string, bool>("IMyFunctionalBlock", true),
          new KeyValuePair<string, bool>("IMyTerminalBlock", true),
          new KeyValuePair<string, bool>("IMyCubeBlock", true),
          new KeyValuePair<string, bool>("IMySlimBlock", true),
        };

        if (this is TargetPriorities)
          PriorityTypes.Insert(0, new KeyValuePair<string, bool>("IMyCharacter", true));
      }

      internal int GetBlockPriority(object item)
      {
        for (int i = 0; i < PriorityTypes.Count; i++)
        {
          var pri = PriorityTypes[i];
          var priName = GetPriorityName(pri.Key);

          if (CheckTypeFromString(item, priName))
            return pri.Value ? i : -1;
        }

        return -1;
      }

      internal int IndexOf(string priority)
      {
        var pri = GetPriorityName(priority);

        for (int i = 0; i < PriorityTypes.Count; i++)
        {
          if (PriorityTypes[i].Key?.Equals(pri, StringComparison.OrdinalIgnoreCase) == true)
            return i;
        }

        return -1;
      }

      internal void UpdatePriority(int oldIndex, int newIndex)
      {
        PriorityTypes.Move(oldIndex, newIndex);
      }

      internal bool ContainsPriority(string priority)
      {
        if (string.IsNullOrEmpty(priority))
          return false;

        var idx = priority.IndexOf("]");

        if (idx >= 0)
          priority = priority.Substring(idx + 1);

        priority = priority.Trim();

        for (int i = 0; i < PriorityTypes.Count; i++)
        {
          if (PriorityTypes[i].Key.Equals(priority, StringComparison.OrdinalIgnoreCase))
            return true;
        }

        return false;
      }

      internal void AddPriority(string priority, bool enabled)
      {
        if (!ContainsPriority(priority))
        {
          PriorityTypes.Add(new KeyValuePair<string, bool>(priority.Trim(), enabled));
        }
      }

      internal bool GetEnabled(string priority)
      {
        var idx = priority.IndexOf("]");

        if (idx >= 0)
          priority = priority.Substring(idx + 1);

        priority = priority.Trim();

        foreach (var pri in PriorityTypes)
        {
          if (pri.Key.Equals(priority, StringComparison.OrdinalIgnoreCase))
            return pri.Value;
        }

        return false;
      }

      internal string GetPriorityName(string priority)
      {
        var idx = priority.IndexOf("]");

        if (idx >= 0)
          priority = priority.Substring(idx + 1);

        return priority.Trim();
      }

      bool CheckTypeFromString(object item, string priType)
      {
        var block = item as IMySlimBlock;
        var fatBlock = block?.FatBlock;

        switch (priType)
        {
          case "IMyCharacter":
            return item is IMyCharacter;
          case "IMyUserControllableGun":
            return fatBlock is IMyUserControllableGun;
          case "IMyShipController":
            return fatBlock is IMyShipController;
          case "IMyPowerProducer":
            return fatBlock is IMyPowerProducer;
          case "IMyThrust":
            return fatBlock is IMyThrust;
          case "IMyGyro":
            return fatBlock is IMyGyro;
          case "IMyProductionBlock":
            return fatBlock is IMyProductionBlock;
          case "IMyDoor":
            return fatBlock is IMyDoor;
          case "IMyProjector":
            return fatBlock is IMyProjector;
          case "IMyProgrammableBlock":
            return fatBlock is IMyProgrammableBlock;
          case "IMyConveyor":
            return fatBlock is IMyConveyor || fatBlock is IMyConveyorSorter || fatBlock is IMyConveyorTube;
          case "IMyCargoContainer":
            return fatBlock is IMyCargoContainer;
          case "IMyFunctionalBlock":
            return fatBlock is IMyFunctionalBlock;
          case "IMyTerminalBlock":
            return fatBlock is IMyTerminalBlock;
          case "IMyCubeBlock":
            return fatBlock != null;
          default:
            return block != null;
        }
      }
    }

    [ProtoContract]
    public class RepairPriorities : Priorities
    {
      [ProtoMember(1)] public bool WeldBeforeGrind = true;

      public RepairPriorities()
      {
        AssignDefaults();
      }

      public RepairPriorities(List<KeyValuePair<string, bool>> pris)
      {
        if (pris?.Count > 0)
        {
          PriorityTypes = new List<KeyValuePair<string, bool>>(pris);
        }
        else
        {
          AssignDefaults();
        }
      }

      public RepairPriorities(List<string> pris)
      {
        if (pris?.Count > 0)
        {
          var defaultPris = GetDefaultRepairPriorities();

          PriorityTypes = new List<KeyValuePair<string, bool>>();

          foreach (var p in pris)
          {
            var idx = p.IndexOf("]");
            if (idx >= 0)
            {
              var enabled = p.Trim().StartsWith("[X]");
              var name = GetPriorityName(p);

              PriorityTypes.Add(new KeyValuePair<string, bool>(name, enabled));
            }
            else
            {
              PriorityTypes.Add(new KeyValuePair<string, bool>(p.Trim(), true));
            }
          }

          foreach (var p in defaultPris)
          {
            if (!ContainsPriority(p))
              PriorityTypes.Add(new KeyValuePair<string, bool>(p, false));
          }
        }
        else
        {
          AssignDefaults();
        }
      }
    }

    [ProtoContract]
    public class TargetPriorities : Priorities
    {
      [ProtoMember(1)] public bool DamageToDisable = true;

      public TargetPriorities()
      {
        AssignDefaults();
      }

      public TargetPriorities(List<KeyValuePair<string, bool>> pris)
      {
        if (pris?.Count > 0)
        {
          PriorityTypes = new List<KeyValuePair<string, bool>>(pris);
        }
        else
        {
          AssignDefaults();
        }
      }

      public TargetPriorities(List<string> pris)
      {
        if (pris?.Count > 0)
        {
          var defaultPris = GetDefaultTargetPriorities();

          PriorityTypes = new List<KeyValuePair<string, bool>>();

          foreach (var p in pris)
          {
            var idx = p.IndexOf("]");
            if (idx >= 0)
            {
              var enabled = p.Trim().StartsWith("[X]");
              var name = GetPriorityName(p);

              PriorityTypes.Add(new KeyValuePair<string, bool>(name, enabled));
            }
            else
            {
              PriorityTypes.Add(new KeyValuePair<string, bool>(p.Trim(), true));
            }
          }

          foreach (var p in defaultPris)
          {
            if (!ContainsPriority(p))
              PriorityTypes.Add(new KeyValuePair<string, bool>(p, false));
          }
        }
        else
        {
          AssignDefaults();
        }
      }
    }

    /////////////////////////////////////////////
    //API Methods Start:
    /////////////////////////////////////////////

    /// <summary>
    /// Check this to ensure API loaded properly
    /// </summary>
    public bool Valid;

    /// <summary>
    /// This method will spawn a Bot with custom behavior
    /// </summary>
    /// <param name="subType">The SubtypeId of the Bot you want to Spawn (<see cref="GetBotSubtypes"/>)</param>
    /// <param name="displayName">The DisplayName of the Bot</param>
    /// <param name="role">Bot Role: <see cref="GetFriendlyBotRoles"/>, <see cref="GetNPCBotRoles"/>, or <see cref="GetNeutralBotRoles"/>. If not supplied, it will be determined by the subType's default usage</param>
    /// <param name="positionAndOrientation">Position and Orientation</param>
    /// <param name="grid">If supplied, the Bot will start with a Cubegrid Map for pathfinding, otherwise a Voxel Map</param>
    /// <param name="owner">Owner's IdentityId for the Bot</param>
    /// <param name="color">The color for the bot in RGB format</param>
    /// <returns>The IMyCharacter created for the Bot, or null if unsuccessful</returns>
    [Obsolete("This method can cause lag, use SpawnBotQueued instead.")]
    public IMyCharacter SpawnBot(string subType, string displayName, MyPositionAndOrientation positionAndOrientation, MyCubeGrid grid = null, string role = null, long? owner = null, Color? color = null) => _spawnBot?.Invoke(subType, displayName, positionAndOrientation, grid, role, owner, color) ?? null;

    /// <summary>
    /// This method will spawn a Bot with custom behavior
    /// </summary>
    /// <param name="displayName">The DisplayName of the Bot</param>
    /// <param name="positionAndOrientation">Position and Orientation</param>
    /// <param name="spawnData">The serialized <see cref="SpawnData"/> object</param>
    /// <param name="grid">If supplied, the Bot will start with a Cubegrid Map for pathfinding, otherwise a Voxel Map</param>
    /// <param name="owner">Owner's IdentityId for the Bot</param>
    /// <returns>The IMyCharacter created for the Bot, or null if unsuccessful</returns>
    [Obsolete("This method can cause lag, use SpawnBotQueued instead.")]
    public IMyCharacter SpawnBot(MyPositionAndOrientation positionAndOrientation, byte[] spawnData, MyCubeGrid grid = null, long? owner = null) => _spawnBotCustom?.Invoke(positionAndOrientation, spawnData, grid, owner) ?? null;

    /// <summary>
    /// This method will queue a Bot to be spawned with custom behavior
    /// </summary>
    /// <param name="subType">The SubtypeId of the Bot you want to Spawn (see <see cref="GetBotSubtypes"/>)</param>
    /// <param name="displayName">The DisplayName of the Bot</param>
    /// <param name="role">Bot Role: see <see cref="GetFriendlyBotRoles"/>, <see cref="GetNPCBotRoles"/>, or <see cref="GetNeutralBotRoles"/>. If not supplied, it will be determined by the subType's default usage</param>
    /// <param name="positionAndOrientation">Position and Orientation</param>
    /// <param name="grid">If supplied, the Bot will start with a Cubegrid Map for pathfinding, otherwise a Voxel Map</param>
    /// <param name="owner">Owner / Identity of the Bot</param>
    /// <param name="color">The color for the bot in RGB format</param>
    /// <param name="callBack">The callback method to invoke when the bot is spawned</param>
    /// <returns>The spawn id associated with the request, or -1 if invalid, 
    /// and the IMyCharacter created for the Bot, or null if unsuccessful, in a callback method</returns>
    public long SpawnBotQueuedWithId(string subType, string displayName, MyPositionAndOrientation positionAndOrientation, MyCubeGrid grid = null, string role = null, long? owner = null, Color? color = null, Action<IMyCharacter, long> callBack = null) => _spawnBotQueuedWithId?.Invoke(subType, displayName, positionAndOrientation, grid, role, owner, color, callBack) ?? -1;

    /// <summary>
    /// This method will queue a Bot to be spawned with custom behavior
    /// </summary>
    /// <param name="positionAndOrientation">Position and Orientation</param>
    /// <param name="spawnData">The serialized <see cref="SpawnData"/> object</param>
    /// <param name="grid">If supplied, the Bot will start with a Cubegrid Map for pathfinding, otherwise a Voxel Map</param>
    /// <param name="owner">Owner's IdentityId for the Bot (if a HelperBot)</param>
    /// <param name="callBack">The callback method to invoke when the bot is spawned</param>
    /// <returns>The spawn id associated with the request, or -1 if invalid, 
    /// and the IMyCharacter created for the Bot, or null if unsuccessful, in a callback method</returns>
    public long SpawnBotQueuedWithId(MyPositionAndOrientation positionAndOrientation, byte[] spawnData, MyCubeGrid grid = null, long? owner = null, Action<IMyCharacter, long> callBack = null) => _spawnBotCustomQueuedWithId?.Invoke(positionAndOrientation, spawnData, grid, owner, callBack) ?? -1;

    /// <summary>
    /// This method will queue a Bot to be spawned with custom behavior
    /// </summary>
    /// <param name="subType">The SubtypeId of the Bot you want to Spawn (see <see cref="GetBotSubtypes"/>)</param>
    /// <param name="displayName">The DisplayName of the Bot</param>
    /// <param name="role">Bot Role: see <see cref="GetFriendlyBotRoles"/>, <see cref="GetNPCBotRoles"/>, or <see cref="GetNeutralBotRoles"/>. If not supplied, it will be determined by the subType's default usage</param>
    /// <param name="positionAndOrientation">Position and Orientation</param>
    /// <param name="grid">If supplied, the Bot will start with a Cubegrid Map for pathfinding, otherwise a Voxel Map</param>
    /// <param name="owner">Owner / Identity of the Bot</param>
    /// <param name="color">The color for the bot in RGB format</param>
    /// <param name="callBack">The callback method to invoke when the bot is spawned</param>
    /// <returns>The IMyCharacter created for the Bot, or null if unsuccessful, in a callback method</returns>
    public void SpawnBotQueued(string subType, string displayName, MyPositionAndOrientation positionAndOrientation, MyCubeGrid grid = null, string role = null, long? owner = null, Color? color = null, Action<IMyCharacter> callBack = null) => _spawnBotQueued?.Invoke(subType, displayName, positionAndOrientation, grid, role, owner, color, callBack);

    /// <summary>
    /// This method will queue a Bot to be spawned with custom behavior
    /// </summary>
    /// <param name="positionAndOrientation">Position and Orientation</param>
    /// <param name="spawnData">The serialized <see cref="SpawnData"/> object</param>
    /// <param name="grid">If supplied, the Bot will start with a Cubegrid Map for pathfinding, otherwise a Voxel Map</param>
    /// <param name="owner">Owner's IdentityId for the Bot (if a HelperBot)</param>
    /// <param name="callBack">The callback method to invoke when the bot is spawned</param>
    /// <returns>The IMyCharacter created for the Bot, or null if unsuccessful, in a callback method</returns>
    public void SpawnBotQueued(MyPositionAndOrientation positionAndOrientation, byte[] spawnData, MyCubeGrid grid = null, long? owner = null, Action<IMyCharacter> callBack = null) => _spawnBotCustomQueued?.Invoke(positionAndOrientation, spawnData, grid, owner, callBack);

    /// <summary>
    /// Check this BEFORE attempting to spawn a bot to ensure the mod is ready
    /// </summary>
    public bool CanSpawn => _canSpawn?.Invoke() ?? false;

    /// <summary>
    /// Retrieves the current set of available bot subtypes the mod will recognize
    /// </summary>
    public string[] GetBotSubtypes() => _getBotSubtypes?.Invoke() ?? null;

    /// <summary>
    /// Retrieves the current set of available friendly bot roles
    /// </summary>
    public string[] GetFriendlyBotRoles() => _getFriendlyBotRoles?.Invoke() ?? null;

    /// <summary>
    /// Retrieves the current set of available non-friendly bot roles
    /// </summary>
    public string[] GetNPCBotRoles() => _getNPCBotRoles?.Invoke() ?? null;

    /// <summary>
    /// Retrieves the current set of available neutral bot roles
    /// </summary>
    public string[] GetNeutralBotRoles() => _getNeutralBotRoles?.Invoke() ?? null;

    /// <summary>
    /// Determines if the entity or player id belongs to an AiEnabled bot
    /// </summary>
    /// <param name="id">The EntityId or PlayerId of the Character</param>
    /// <returns>true if the Id belongs to a bot, otherwise false</returns>
    public bool IsBot(long id) => _isBot?.Invoke(id) ?? false;

    /// <summary>
    /// Retrieves the relationship between an AiEnabled bot and an identity
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    /// <param name="otherIdentityId">The IdentityId to check against</param>
    /// <returns>MyRelationsBetweenPlayerAndBlock, default is NoOwnership</returns>
    public MyRelationsBetweenPlayerAndBlock GetRelationshipBetween(long botEntityId, long otherIdentityId) => _getRelationshipBetween?.Invoke(botEntityId, otherIdentityId) ?? MyRelationsBetweenPlayerAndBlock.NoOwnership;

    /// <summary>
    /// Determines if an entity id belongs to an AiEnabled bot, and retrieves its relationship to an identity if so
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    /// <param name="otherIdentityId">The IdentityId to check against</param>
    /// <param name="relationBetween">The relationship between the bot and the identity, defaults to NoOwnership</param>
    /// <returns>true if the EntityId is an AiEnabled bot, otherwise false</returns>
    public bool CheckBotRelationTo(long botEntityId, long otherIdentityId, out MyRelationsBetweenPlayerAndBlock relationBetween)
    {
      var result = _getBotAndRelationTo?.Invoke(botEntityId, otherIdentityId);

      if (result.HasValue)
      {
        relationBetween = result.Value;
        return true;
      }

      relationBetween = MyRelationsBetweenPlayerAndBlock.NoOwnership;
      return false;
    }

    /// <summary>
    /// Gets the current Overridden GoTo position for the Bot
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    public Vector3D? GetBotGoto(long botEntityId) => _getBotOverride?.Invoke(botEntityId) ?? null;

    /// <summary>
    /// Overrides the GoTo position for the Bot. The Bot will attempt to pathfind to the given coordinate.
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    /// <param name="goTo">The World Position the Bot should path to</param>
    /// <returns>true if the override is set successfully, otherwise false</returns>
    public bool SetBotGoto(long botEntityId, Vector3D goTo) => _setBotOverride?.Invoke(botEntityId, goTo) ?? false;

    /// <summary>
    /// Sets the Override Complete Action associated with a given Bot. 
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    /// <param name="action">The Action to perform when the current Overridden GoTo is nullified</param>
    /// <returns>true if the action is set successfully, otherwise false</returns>
    public bool SetGotoRemovedAction(long botEntityId, Action<long, bool> action) => _setOverrideAction?.Invoke(botEntityId, action) ?? false;

    /// <summary>
    /// Sets the Bots target and forces the Bot to use only API-provided targets. 
    /// You must call <see cref="ResetBotTargeting(long)"/> for it to resume autonomous targeting
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    /// <param name="target">The target can be any player, character, block, or grid. 
    /// DO NOT use a VECTOR as the target. To override the GoTo, use <see cref="SetBotOverride(long, Vector3D)"/></param>
    /// <returns>true if the target is set successfully, otherwise false</returns>
    public bool SetBotTarget(long botEntityId, object target) => _setBotTarget?.Invoke(botEntityId, target) ?? false;

    /// <summary>
    /// Assigns a patrol route to the Bot. In patrol mode, the bot will attack any enemies that come near its route, but will not hunt outside of its current map.
    /// You must call <see cref="ResetBotTargeting(long)"/> for it to resume normal functions
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    /// <param name="waypoints">A list of world coordinates for the bot to patrol</param>
    /// <returns>true if the route is assigned successfully, otherwise false</returns>
    public bool SetBotPatrol(long botEntityId, List<Vector3D> waypoints) => _setBotPatrol?.Invoke(botEntityId, waypoints) ?? false;

    /// <summary>
    /// Assigns a patrol route to the Bot. In patrol mode, the bot will attack any enemies that come near its route, but will not hunt outside of its current map.
    /// You must call <see cref="ResetBotTargeting(long)"/> for it to resume normal functions
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    /// <param name="waypoints">A list of grid-local coordinates for the bot to patrol</param>
    /// <returns>true if the route is assigned successfully, otherwise false</returns>
    public bool SetBotPatrol(long botEntityId, List<Vector3I> waypoints) => _setBotPatrolLocal?.Invoke(botEntityId, waypoints) ?? false;

    /// <summary>
    /// Clears the Bot's current target and re-enables autonomous targeting
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    /// <returns>true if targeting is successfully reset, otherwise false</returns>
    public bool ResetBotTargeting(long botEntityId) => _resetBotTargeting?.Invoke(botEntityId) ?? false;

    /// <summary>
    /// Sets the Action to perform when the Bot's API target is removed. 
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    /// <param name="action">The Action to perform when the current API target is nullified</param>
    /// <returns>true if the action is set successfully, otherwise false</returns>
    public bool SetTargetRemovedAction(long botEntityId, Action<long> action) => _setTargetAction?.Invoke(botEntityId, action) ?? false;

    /// <summary>
    /// Attempts to place the Bot in the given Seat
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    /// <param name="seat">The Seat to place the Bot in</param>
    /// <returns>true if able to seat the Bot, otherwise false</returns>
    public bool TrySeatBot(long botEntityId, IMyCockpit seat) => _trySeatBot?.Invoke(botEntityId, seat) ?? false;

    /// <summary>
    /// Attempts to place the Bot in the first open seat on the Grid
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    /// <param name="grid">The IMyCubeGrid to find a seat on</param>
    /// <returns>true if able to seat the Bot, otherwise false</returns>
    public bool TrySeatBotOnGrid(long botEntityId, IMyCubeGrid grid) => _trySeatBotOnGrid?.Invoke(botEntityId, grid) ?? false;

    /// <summary>
    /// Attempts to remove the Bot from its seat
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    /// <returns>true if able to remove the Bot from its seat, otherwise false</returns>
    public bool TryRemoveBotFromSeat(long botEntityId) => _tryRemoveBotFromSeat?.Invoke(botEntityId) ?? false;

    /// <summary>
    /// Determines whether or not the supplied grid can be walked on by AiEnabled bots (ie it has at least one large grid connected to it)
    /// </summary>
    /// <param name="gridToCheck">the initial grid to check</param>
    /// <returns>true if the grid can be used by AiEnabled bots, otherwise false</returns>
    public bool IsValidForPathfinding(IMyCubeGrid gridToCheck) => _isGridValidForPathfinding?.Invoke(gridToCheck) ?? false;


    /// <summary>
    /// Attempts to find valid grid nodes to spawn NPCs at
    /// </summary>
    /// <param name="grid">The grid to spawn on</param>
    /// <param name="numberOfNodesNeeded">The number of bots you want to spawn</param>
    /// <param name="nodeList">The list to fill with World Positions. This will only be valid for one frame unless grid doesn't move!</param>
    /// <param name="upVector">The normalized Up direction for the grid, if known</param>
    /// <param name="onlyAirtightNodes">If only pressurized areas should be considered</param>
    /// <returns></returns>
    public void GetAvailableGridNodes(MyCubeGrid grid, int numberOfNodesNeeded, List<Vector3D> nodeList, Vector3D? upVector = null, bool onlyAirtightNodes = false) => _getAvailableGridNodes?.Invoke(grid, numberOfNodesNeeded, nodeList, upVector, onlyAirtightNodes);

    /// <summary>
    /// Attempts to find interior nodes based on how many airtight blocks are found in all directions of a given point
    /// </summary>
    /// <param name="grid">The main grid. All connected grids will also be considered.</param>
    /// <param name="nodeList">The list to fill with local points. Convert to world using mainGrid.GridIntegerToWorld(point).</param>
    /// <param name="enclosureRating">How many sides need to be found to consider a point inside. Default is 5.</param>
    /// <param name="allowAirNodes">Whether or not to consider air nodes for inside-ness.</param>
    /// <param name="onlyAirtightNodes">Whether or not to only accept airtight nodes.</param>
    /// <param name="callBack">The Action to be invoked when the thread finishes</param>
    public void GetInteriorNodes(MyCubeGrid grid, List<Vector3I> nodeList, int enclosureRating = 5, bool allowAirNodes = true, bool onlyAirtightNodes = false, Action<IMyCubeGrid, List<Vector3I>> callBack = null) => _getInteriorNodes?.Invoke(grid, nodeList, enclosureRating, allowAirNodes, onlyAirtightNodes, callBack);

    /// <summary>
    /// Attempts to get the closest valid node to a given grid position
    /// </summary>
    /// <param name="grid">The grid the position is on</param>
    /// <param name="startPosition">The position you want to get a nearby node for</param>
    /// <param name="upVec">If supplied, the returned node will be confined to nodes on the same level as the start position</param>
    /// <param name="validWorldPosition">The returned world position</param>
    /// <returns>true if able to find a valid node nearby, otherwise false</returns>
    [Obsolete("Use the overload that takes in a Vector3D for startPosition to avoid subgrid issues")]
    public bool GetClosestValidNode(long botEntityId, MyCubeGrid grid, Vector3I startPosition, Vector3D? upVec, out Vector3D validWorldPosition)
    {
      var result = _getClosestValidNode?.Invoke(botEntityId, grid, startPosition, upVec);

      if (result.HasValue)
      {
        validWorldPosition = result.Value;
        return true;
      }

      validWorldPosition = Vector3D.Zero;
      return false;
    }

    /// <summary>
    /// Attempts to get the closest valid node to a given grid position
    /// </summary>
    /// <param name="grid">The grid the position is on</param>
    /// <param name="startPosition">The world position you want to get a nearby node for</param>
    /// <param name="upVec">If supplied, the returned node will be confined to nodes on the same level as the start position</param>
    /// <param name="validWorldPosition">The returned world position</param>
    /// <returns>true if able to find a valid node nearby, otherwise false</returns>
    public bool GetClosestValidNode(long botEntityId, MyCubeGrid grid, Vector3D startPosition, Vector3D? upVec, out Vector3D validWorldPosition, bool allowAirNodes)
    {
      var result = _getClosestValidNodeNew?.Invoke(botEntityId, grid, startPosition, upVec, allowAirNodes);
      
      if (result.HasValue)
      {
        validWorldPosition = result.Value;
        return true;
      }

      validWorldPosition = startPosition;
      return false;
    }

    /// <summary>
    /// Attempts to find the closest surface point to the provided world position by checking up and down from that point. 
    /// Does nothing if the position is not near a voxel body (planet or asteroid).
    /// </summary>
    /// <param name="startPosition">the position to check from</param>
    /// <param name="voxel">a planet or asteroid, if null one will be looked for</param>\
    /// <param name="validWorldPosition">the surface point if it exists, otherwise the start position</param>
    /// <returns>true if able to find a valid surface position, otherwise false</returns>
    public bool GetClosestSurfacePoint(Vector3D startPosition, out Vector3D validWorldPosition, MyVoxelBase voxel = null)
    {
      var result = _getClosestSurfacePoint?.Invoke(startPosition, voxel);

      if (result.HasValue)
      {
        validWorldPosition = result.Value;
        return true;
      }

      validWorldPosition = startPosition;
      return false;
    }

    /// <summary>
    /// Starts processing a grid to be used as a grid map, if it doesn't already exist
    /// </summary>
    /// <param name="grid">The grid to process</param>
    /// <param name="orientation">The matrix to use to determine proper orientation for the grid map</param>
    /// <returns>true if a map exists or if able to create one, otherwise false</returns>
    public bool CreateGridMap(MyCubeGrid grid, MatrixD? orientation = null) => _createGridMap?.Invoke(grid, orientation) ?? false;

    /// <summary>
    /// Attempts to retrieve the world orientation for the main grid that would be used for pathing. This may not be the same grid that is passed into the method.
    /// HINT: Use this as the orientation for bots spawned on this grid!
    /// </summary>
    /// <param name="grid">the grid that you want to get the map orientation for</param>
    /// <param name="checkForMainGrid">if true, check for other grids that may be more suitable as the map's main grid</param>
    /// <returns>The world orientation the grid's map will use, or null if no suitable main grid is found</returns>
    public bool GetGridMapMatrix(MyCubeGrid grid, bool checkForMainGrid, out MatrixD mapMatrix)
    {
      var result = _getGridMapMatrix?.Invoke(grid, checkForMainGrid);

      if (result.HasValue)
      {
        mapMatrix = result.Value;
        return true;
      }

      mapMatrix = MatrixD.Identity;
      return false;
    }

    /// <summary>
    /// Determines if a grid map is ready to be used. Note that when blocks are added or removed, the grid is reprocessed!
    /// </summary>
    /// <param name="grid">The grid to check</param>
    /// <returns>true if a grid is ready to use, otherwise false</returns>
    public bool IsGridMapReady(MyCubeGrid grid) => _isGridMapReady?.Invoke(grid) ?? false;

    /// <summary>
    /// Removed a bot from the world. Use this to ensure everything is cleaned up properly!
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    /// <returns>true if the bot is successfully removed, otherwise false</returns>
    public bool RemoveBot(long botEntityId) => _removeBot?.Invoke(botEntityId) ?? false;

    /// <summary>
    /// Attempts to have a bot perform some action
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    /// <param name="action">The name of the action to perform (leave null to use a random action)</param>
    public void Perform(long botEntityId, string action = null) => _perform?.Invoke(botEntityId, action);

    /// <summary>
    /// Attempts to have a bot speak some phrase
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    /// <param name="phrase">The name of the phrase (leave null to use a random phrase)</param>
    public void Speak(long botEntityId, string phrase = null) => _speak?.Invoke(botEntityId, phrase);

    /// <summary>
    /// Determins if the bot role is allowed to use a given tool type
    /// </summary>
    /// <param name="botRole">the BotRole to check (ie SoldierBot, GrinderBot, etc)</param>
    /// <param name="toolSubtype">the SubtypeId of the weapon or tool</param>
    /// <returns>true if the role is allowed to use the item, otherwise false</returns>
    public bool CanRoleUseTool(string botRole, string toolSubtype) => _canBotUseTool?.Invoke(botRole, toolSubtype) ?? false;

    /// <summary>
    /// Changes the bot's role and any other associated data from <see cref="SpawnData"/>
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    /// <param name="spawnData">The serialized <see cref="SpawnData"/> object</param>
    /// <returns>true if the change is successful, otherwise false</returns>
    public bool SwitchBotRole(long botEntityId, byte[] spawnData) => _switchBotRole?.Invoke(botEntityId, spawnData) ?? false;

    /// <summary>
    /// Changes the bot's role and potential tooltype
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    /// <param name="newRole">Bot Role: see <see cref="GetFriendlyBotRoles"/>, <see cref="GetNPCBotRoles"/>, or <see cref="GetNeutralBotRoles"/></param>
    /// <param name="toolSubtypes">A list of SubtypeIds for the weapon or tool you want to give the bot. A random item will be chosen from the list.</param>
    /// <returns>true if the change is successful, otherwise false</returns>
    public bool SwitchBotRole(long botEntityId, string newRole, List<string> toolSubtypes) => _switchBotRoleSlim?.Invoke(botEntityId, newRole, toolSubtypes) ?? false;

    /// <summary>
    /// Updates the bot's data associated with <see cref="SpawnData"/>. 
    /// Does NOT change data associated with the character itself (color, subtype, etc).
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    /// <param name="spawnData">The serialized <see cref="SpawnData"/> object</param>
    /// <returns>true if the change is successful, otherwise false</returns>
    public bool UpdateBotSpawnData(long botEntityId, byte[] spawnData) => _updateBotSpawnData?.Invoke(botEntityId, spawnData) ?? false;

    /// <summary>
    /// Attempts to assign ownership of the bot to the player
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    /// <param name="playerIdentityId">The IdentityId of the Player to take ownership</param>
    /// <returns>true if the change is successful, otherwise false</returns>
    public bool AssignToPlayer(long botEntityId, long playerIdentityId) => _assignToPlayer?.Invoke(botEntityId, playerIdentityId) ?? false;

    /// <summary>
    /// Attempts to have the bot follow the player
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    /// <param name="playerIdentityId">The IdentityId of the Player to follow</param>
    /// <returns>true if the change is successful, otherwise false</returns>
    public bool FollowPlayer(long botEntityId, long playerIdentityId) => _followPlayer?.Invoke(botEntityId, playerIdentityId) ?? false;

    /// <summary>
    /// Attempts to switch a bot's weapon. The weapon will be added if not found in the bot's inventory. Ammo is NOT included.
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    /// <param name="toolSubtypeId">The SubtypeId for the weapon or tool you want the bot to use</param>
    /// <returns>true if the switch is successful, otherwise false</returns>
    public bool SwitchBotWeapon(long botEntityId, string toolSubtypeId) => _switchBotWeapon?.Invoke(botEntityId, toolSubtypeId) ?? false;

    /// <summary>
    /// Gets the bot's owner's IdentityId (Player Id)
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    /// <returns>the IdentityId of the bot's owner if found, otherwise 0</returns>
    public long GetBotOwnerId(long botEntityId) => _getBotOwnerId?.Invoke(botEntityId) ?? 0L;

    /// <summary>
    /// Adds AiEnabled bots to a user-supplied dictionary
    /// </summary>
    /// <param name="botDict">the dictionary to add bots to. Key is the bot's IdentityId. The method will create and/or clear the dictionary before use</param>
    /// <param name="includeFriendly">whether or not to include all player-owned bots</param>
    /// <param name="includeEnemy">whether or not to include all enemy bots</param>
    /// <param name="includeNeutral">whether or not to include Nomad bots</param>
    public void GetBots(Dictionary<long, IMyCharacter> botDict, bool includeFriendly = true, bool includeEnemy = true, bool includeNeutral = true) => _getBots?.Invoke(botDict, includeFriendly, includeEnemy, includeNeutral);

    /// <summary>
    /// Used to generate new characters in order to relieve the invisible bot issue. USE ONLY IN MULTIPLAYER!
    /// </summary>
    /// <param name="gridEntityId">the EntityId of the grid to work on</param>
    /// <param name="callBackAction">this method will be called at some point in the future, after all bots are created and seated. The list will contain all new bot characters.</param>
    public void ReSyncBotCharacters(long gridEntityId, Action<List<IMyCharacter>> callBackAction) => _reSyncBotCharacters?.Invoke(gridEntityId, callBackAction);

    /// <summary>
    /// Attempts to throw a grenade at the bot's current target. Does nothing if the target is not an IMyEntity or is friendly.
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    public void ThrowGrenade(long botEntityId) => _throwGrenade?.Invoke(botEntityId);

    /// <summary>
    /// Attempts to transform world to local using the proper grid for a given Grid Map. Check null!
    /// </summary>
    /// <param name="gridEntityId">the EntityId for any grid in a Grid Map</param>
    /// <param name="worldPosition">the World Position to transform</param>
    /// <returns>the local position if everything is valid, otherwise null</returns>
    public Vector3I? GetLocalPositionForGrid(long gridEntityId, Vector3D worldPosition) => _getLocalPositionForGrid?.Invoke(gridEntityId, worldPosition);

    /// <summary>
    /// Attempts to retrieve the main grid used for position transformations for a given grid map. Check null!
    /// </summary>
    /// <param name="gridEntityId">the EntityId for any grid in a Grid Map</param>
    /// <returns>the IMyCubeGrid to use for a map's position transformations, if a map exists, otherwise null</returns>
    public IMyCubeGrid GetMainMapGrid(long gridEntityId) => _getMainGrid?.Invoke(gridEntityId);

    /// <summary>
    /// Changes a Bot's ability to equip character tools. If disabled, the bot will unequip its current tool.
    /// </summary>
    /// <param name="botEntityId">The EntityId of the Bot's Character</param>
    /// <param name="enable">Whether or not to allow the Bot to use tools and weapons</param>
    /// <returns>true if the change is successful, otherwise false</returns>
    public bool SetToolsEnabled(long botEntityId, bool enable) => _setToolsEnabled?.Invoke(botEntityId, enable) ?? false;

    /// <summary>
    /// Provides the default list of repair priorities. Allocates a new list when called!
    /// </summary>
    /// <returns>a list of strings representing priority types</returns>
    public static List<string> GetDefaultRepairPriorities() => new List<string>()
    {
      "IMyUserControllableGun",
      "IMyShipController",
      "IMyPowerProducer",
      "IMyThrust",
      "IMyGyro",
      "IMyProductionBlock",
      "IMyDoor",
      "IMyProgrammableBlock",
      "IMyProjector",
      "IMyConveyor",
      "IMyCargoContainer",
      "IMyFunctionalBlock",
      "IMyTerminalBlock",
      "IMyCubeBlock",
      "IMySlimBlock",
   };

    /// <summary>
    /// Provides the default list of target priorities. Allocates a new list when called!
    /// </summary>
    /// <returns>a list of strings representing priority types</returns>
    public static List<string> GetDefaultTargetPriorities() => new List<string>()
    {
      "IMyCharacter",
      "IMyUserControllableGun",
      "IMyShipController",
      "IMyPowerProducer",
      "IMyThrust",
      "IMyGyro",
      "IMyProductionBlock",
      "IMyDoor",
      "IMyProgrammableBlock",
      "IMyProjector",
      "IMyConveyor",
      "IMyCargoContainer",
      "IMyFunctionalBlock",
      "IMyTerminalBlock",
      "IMyCubeBlock",
      "IMySlimBlock",
    };

    /////////////////////////////////////////////
    //API Methods End
    /////////////////////////////////////////////

    private const long _botControllerModChannel = 2408831996; //This is the channel this object will receive API methods at. Sender should also use this.
    private Func<string, string, MyPositionAndOrientation, MyCubeGrid, string, long?, Color?, IMyCharacter> _spawnBot;
    private Func<MyPositionAndOrientation, byte[], MyCubeGrid, long?, IMyCharacter> _spawnBotCustom;
    private Func<string[]> _getFriendlyBotRoles, _getNPCBotRoles, _getNeutralBotRoles, _getBotSubtypes;
    private Func<bool> _canSpawn;
    private Func<long, Vector3D?> _getBotOverride;
    private Func<long, Vector3D, bool> _setBotOverride;
    private Func<long, Action<long, bool>, bool> _setOverrideAction;
    private Func<long, object, bool> _setBotTarget;
    private Func<long, bool> _resetBotTargeting;
    private Func<long, Action<long>, bool> _setTargetAction;
    private Func<long, bool> _tryRemoveBotFromSeat;
    private Func<long, IMyCubeGrid, bool> _trySeatBotOnGrid;
    private Func<long, IMyCockpit, bool> _trySeatBot;
    private Action<MyCubeGrid, int, List<Vector3D>, Vector3D?, bool> _getAvailableGridNodes;
    private Func<MyCubeGrid, MatrixD?, bool> _createGridMap;
    private Func<MyCubeGrid, bool> _isGridMapReady;
    private Func<long, bool> _removeBot;
    private Action<long, string> _perform;
    private Action<long, string> _speak;
    private Func<long, bool> _isBot;
    private Func<long, List<Vector3D>, bool> _setBotPatrol;
    private Func<long, List<Vector3I>, bool> _setBotPatrolLocal;
    private Func<long, long, MyRelationsBetweenPlayerAndBlock> _getRelationshipBetween;
    private Func<string, string, bool> _canBotUseTool;
    private Func<string, string, MyPositionAndOrientation, MyCubeGrid, string, long?, Color?, Action<IMyCharacter, long>, long> _spawnBotQueuedWithId;
    private Func<MyPositionAndOrientation, byte[], MyCubeGrid, long?, Action<IMyCharacter, long>, long> _spawnBotCustomQueuedWithId;
    private Action<string, string, MyPositionAndOrientation, MyCubeGrid, string, long?, Color?, Action<IMyCharacter>> _spawnBotQueued;
    private Action<MyPositionAndOrientation, byte[], MyCubeGrid, long?, Action<IMyCharacter>> _spawnBotCustomQueued;
    private Func<long, byte[], bool> _switchBotRole;
    private Func<long, string, List<string>, bool> _switchBotRoleSlim;
    private Func<long, MyCubeGrid, Vector3I, Vector3D?, Vector3D?> _getClosestValidNode;
    private Func<long, MyCubeGrid, Vector3D, Vector3D?, bool, Vector3D?> _getClosestValidNodeNew;
    private Func<long, long, MyRelationsBetweenPlayerAndBlock?> _getBotAndRelationTo;
    private Func<long, long> _getBotOwnerId;
    private Func<long, string, bool> _switchBotWeapon;
    private Action<Dictionary<long, IMyCharacter>, bool, bool, bool> _getBots;
    private Action<MyCubeGrid, List<Vector3I>, int, bool, bool, Action<IMyCubeGrid, List<Vector3I>>> _getInteriorNodes;
    private Func<IMyCubeGrid, bool> _isGridValidForPathfinding;
    private Action<long, Action<List<IMyCharacter>>> _reSyncBotCharacters;
    private Action<long> _throwGrenade;
    private Func<long, Vector3D, Vector3I?> _getLocalPositionForGrid;
    private Func<long, IMyCubeGrid> _getMainGrid;
    private Func<long, bool, bool> _setToolsEnabled;
    private Func<Vector3D, MyVoxelBase, Vector3D?> _getClosestSurfacePoint;
    private Func<long, byte[], bool> _updateBotSpawnData;
    private Func<MyCubeGrid, bool, MatrixD?> _getGridMapMatrix;
    private Func<long, long, bool> _assignToPlayer;
    private Func<long, long, bool> _followPlayer;

    private void ReceiveModMessage(object payload)
    {
      if (Valid)
        return;

      var dict = payload as Dictionary<string, Delegate>;

      if (dict == null)
        return;

      try
      {

        _spawnBot = dict["SpawnBot"] as Func<string, string, MyPositionAndOrientation, MyCubeGrid, string, long?, Color?, IMyCharacter>;
        _spawnBotCustom = dict["SpawnBotCustom"] as Func<MyPositionAndOrientation, byte[], MyCubeGrid, long?, IMyCharacter>;
        _getFriendlyBotRoles = dict["GetFriendlyRoles"] as Func<string[]>;
        _getNPCBotRoles = dict["GetNPCRoles"] as Func<string[]>;
        _getNeutralBotRoles = dict["GetNeutralRoles"] as Func<string[]>;
        _getBotSubtypes = dict["GetBotSubtypes"] as Func<string[]>;
        _canSpawn = dict["CanSpawn"] as Func<bool>;
        _getBotOverride = dict["GetBotOverride"] as Func<long, Vector3D?>;
        _setBotOverride = dict["SetBotOverride"] as Func<long, Vector3D, bool>;
        _setOverrideAction = dict["SetOverrideAction"] as Func<long, Action<long, bool>, bool>;
        _setBotTarget = dict["SetBotTarget"] as Func<long, object, bool>;
        _resetBotTargeting = dict["ResetBotTargeting"] as Func<long, bool>;
        _setTargetAction = dict["SetTargetAction"] as Func<long, Action<long>, bool>;
        _tryRemoveBotFromSeat = dict["TryRemoveBotFromSeat"] as Func<long, bool>;
        _trySeatBot = dict["TrySeatBot"] as Func<long, IMyCockpit, bool>;
        _trySeatBotOnGrid = dict["TrySeatBotOnGrid"] as Func<long, IMyCubeGrid, bool>;
        _getAvailableGridNodes = dict["GetAvailableGridNodes"] as Action<MyCubeGrid, int, List<Vector3D>, Vector3D?, bool>;
        _getClosestValidNode = dict["GetClosestValidNode"] as Func<long, MyCubeGrid, Vector3I, Vector3D?, Vector3D?>;
        _getClosestValidNodeNew = dict["GetClosestValidNodeNew"] as Func<long, MyCubeGrid, Vector3D, Vector3D?, bool, Vector3D?>;
        _createGridMap = dict["CreateGridMap"] as Func<MyCubeGrid, MatrixD?, bool>;
        _isGridMapReady = dict["IsGridMapReady"] as Func<MyCubeGrid, bool>;
        _removeBot = dict["RemoveBot"] as Func<long, bool>;
        _perform = dict["Perform"] as Action<long, string>;
        _speak = dict["Speak"] as Action<long, string>;
        _isBot = dict["IsBot"] as Func<long, bool>;
        _getRelationshipBetween = dict["GetRelationshipBetween"] as Func<long, long, MyRelationsBetweenPlayerAndBlock>;
        _getBotAndRelationTo = dict["GetBotAndRelationTo"] as Func<long, long, MyRelationsBetweenPlayerAndBlock?>; 
        _spawnBotQueuedWithId = dict["SpawnBotQueuedWithId"] as Func<string, string, MyPositionAndOrientation, MyCubeGrid, string, long?, Color?, Action<IMyCharacter, long>, long>;
        _spawnBotCustomQueuedWithId = dict["SpawnBotCustomQueuedWithId"] as Func<MyPositionAndOrientation, byte[], MyCubeGrid, long?, Action<IMyCharacter, long>, long>;
        _spawnBotQueued = dict["SpawnBotQueued"] as Action<string, string, MyPositionAndOrientation, MyCubeGrid, string, long?, Color?, Action<IMyCharacter>>;
        _spawnBotCustomQueued = dict["SpawnBotCustomQueued"] as Action<MyPositionAndOrientation, byte[], MyCubeGrid, long?, Action<IMyCharacter>>;
        _setBotPatrol = dict["SetBotPatrol"] as Func<long, List<Vector3D>, bool>;
        _setBotPatrolLocal = dict["SetBotPatrolLocal"] as Func<long, List<Vector3I>, bool>;
        _canBotUseTool = dict["CanRoleUseTool"] as Func<string, string, bool>;
        _switchBotRole = dict["SwitchBotRole"] as Func<long, byte[], bool>;
        _switchBotRoleSlim = dict["SwitchBotRoleSlim"] as Func<long, string, List<string>, bool>;
        _switchBotWeapon = dict["SwitchBotWeapon"] as Func<long, string, bool>;
        _getBotOwnerId = dict["GetBotOwnerId"] as Func<long, long>;
        _getBots = dict["GetBots"] as Action<Dictionary<long, IMyCharacter>, bool, bool, bool>;
        _getInteriorNodes = dict["GetInteriorNodes"] as Action<MyCubeGrid, List<Vector3I>, int, bool, bool, Action<IMyCubeGrid, List<Vector3I>>>;
        _isGridValidForPathfinding = dict["IsValidForPathfinding"] as Func<IMyCubeGrid, bool>;
        _reSyncBotCharacters = dict["ReSyncBotCharacters"] as Action<long, Action<List<IMyCharacter>>>;
        _throwGrenade = dict["ThrowGrenade"] as Action<long>;
        _getLocalPositionForGrid = dict["GetLocalPositionForGrid"] as Func<long, Vector3D, Vector3I?>;
        _getMainGrid = dict["GetMainMapGrid"] as Func<long, IMyCubeGrid>;
        _setToolsEnabled = dict["SetToolsEnabled"] as Func<long, bool, bool>;
        _getClosestSurfacePoint = dict["GetClosestSurfacePoint"] as Func<Vector3D, MyVoxelBase, Vector3D?>;
        _updateBotSpawnData = dict["UpdateBotSpawnData"] as Func<long, byte[], bool>;
        _getGridMapMatrix = dict["GetGridMapMatrix"] as Func<MyCubeGrid, bool, MatrixD?>;
        _assignToPlayer = dict["AssignToPlayer"] as Func<long, long, bool>;
        _followPlayer = dict["FollowPlayer"] as Func<long, long, bool>;

      }
      catch
      {

        Valid = false;
        return;

      }

      Valid = true;
    }
  }
}
