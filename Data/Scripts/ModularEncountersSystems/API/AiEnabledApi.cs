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

namespace ModularEncountersSystems.API {
    public class RemoteBotAPI {
        //Create Instance of this object in your SessionComponent LoadData() method.
        public RemoteBotAPI() {
            MyAPIGateway.Utilities.RegisterMessageHandler(_botControllerModChannel, ReceiveModMessage);
        }

        /// <summary>
        /// Call this in your Unload to ensure the Message Handler is unregistered properly
        /// </summary>
        public void Close() {
            try {
                MyAPIGateway.Utilities.UnregisterMessageHandler(_botControllerModChannel, ReceiveModMessage);
            } catch (Exception ex) {
                MyLog.Default.WriteLineAndConsole($"Exception in AiEnabled.RemoteBotAPI.Close: {ex.Message}\n{ex.StackTrace}");
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
        public class SpawnData {
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
        /// <returns>The IMyCharacter created for the Bot, or null if unsuccessful, in a callback method</returns>
        public void SpawnBotQueued(string subType, string displayName, MyPositionAndOrientation positionAndOrientation, MyCubeGrid grid = null, string role = null, long? owner = null, Color? color = null, Action<IMyCharacter> callBack = null) => _spawnBotQueued?.Invoke(subType, displayName, positionAndOrientation, grid, role, owner, color, callBack);

        /// <summary>
        /// This method will queue a Bot to be spawned with custom behavior
        /// </summary>
        /// <param name="displayName">The DisplayName of the Bot</param>
        /// <param name="positionAndOrientation">Position and Orientation</param>
        /// <param name="spawnData">The serialized <see cref="SpawnData"/> object</param>
        /// <param name="grid">If supplied, the Bot will start with a Cubegrid Map for pathfinding, otherwise a Voxel Map</param>
        /// <param name="owner">Owner's IdentityId for the Bot (if a HelperBot)</param>
        /// <param name="callback">The callback method to invoke when the bot is spawned</param>
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
        public bool CheckBotRelationTo(long botEntityId, long otherIdentityId, out MyRelationsBetweenPlayerAndBlock relationBetween) {
            relationBetween = MyRelationsBetweenPlayerAndBlock.NoOwnership;
            return _getBotAndRelationTo?.Invoke(botEntityId, otherIdentityId, out relationBetween) ?? false;
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
        public void GetInteriorNodes(MyCubeGrid grid, List<Vector3I> nodeList, int enclosureRating = 5, bool allowAirNodes = true, bool onlyAirtightNodes = false, Action callBack = null) => _getInteriorNodes?.Invoke(grid, nodeList, enclosureRating, allowAirNodes, onlyAirtightNodes, callBack);

        /// <summary>
        /// Attempts to get the closest valid node to a given grid position
        /// </summary>
        /// <param name="grid">The grid the position is on</param>
        /// <param name="startPosition">The position you want to get a nearby node for</param>
        /// <param name="upVec">If supplied, the returned node will be confined to nodes on the same level as the start position</param>
        /// <param name="validWorldPosition">The returned world position</param>
        /// <returns>true if able to find a valid node nearby, otherwise false</returns>
        [Obsolete("Use the overload that takes in a Vector3D for startPosition to avoid subgrid issues")]
        public bool GetClosestValidNode(long botEntityId, MyCubeGrid grid, Vector3I startPosition, Vector3D? upVec, out Vector3D validWorldPosition) {
            validWorldPosition = Vector3D.Zero;
            return _getClosestValidNode?.Invoke(botEntityId, grid, startPosition, upVec, out validWorldPosition) ?? false;
        }

        /// <summary>
        /// Attempts to get the closest valid node to a given grid position
        /// </summary>
        /// <param name="grid">The grid the position is on</param>
        /// <param name="startPosition">The world position you want to get a nearby node for</param>
        /// <param name="upVec">If supplied, the returned node will be confined to nodes on the same level as the start position</param>
        /// <param name="validWorldPosition">The returned world position</param>
        /// <returns>true if able to find a valid node nearby, otherwise false</returns>
        public bool GetClosestValidNode(long botEntityId, MyCubeGrid grid, Vector3D startPosition, Vector3D? upVec, out Vector3D validWorldPosition, bool allowAirNodes) {
            validWorldPosition = Vector3D.Zero;
            return _getClosestValidNodeNew?.Invoke(botEntityId, grid, startPosition, upVec, out validWorldPosition, allowAirNodes) ?? false;
        }

        /// <summary>
        /// Starts processing a grid to be used as a grid map, if it doesn't already exist
        /// </summary>
        /// <param name="grid">The grid to process</param>
        /// <param name="orientation">The matrix to use to determine proper orientation for the grid map</param>
        /// <returns>true if a map exists or if able to create one, otherwise false</returns>
        public bool CreateGridMap(MyCubeGrid grid, MatrixD? orientation = null) => _createGridMap?.Invoke(grid, orientation) ?? false;

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
        public bool SwitchBotRole(long botEntityId, SpawnData spawnData) => _switchBotRole?.Invoke(botEntityId, spawnData) ?? false;

        /// <summary>
        /// Changes the bot's role and potential tooltype
        /// </summary>
        /// <param name="botEntityId">The EntityId of the Bot's Character</param>
        /// <param name="newRole">Bot Role: see <see cref="GetFriendlyBotRoles"/>, <see cref="GetNPCBotRoles"/>, or <see cref="GetNeutralBotRoles"/></param>
        /// <param name="toolSubtypes">A list of SubtypeIds for the weapon or tool you want to give the bot. A random item will be chosen from the list.</param>
        /// <returns>true if the change is successful, otherwise false</returns>
        public bool SwitchBotRole(long botEntityId, string newRole, List<string> toolSubtypes) => _switchBotRoleSlim?.Invoke(botEntityId, newRole, toolSubtypes) ?? false;

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


        /////////////////////////////////////////////
        //API Methods End
        /////////////////////////////////////////////

        delegate bool GetClosestNodeDelegate(long botEntityId, MyCubeGrid grid, Vector3I start, Vector3D? up, out Vector3D result);
        delegate bool GetClosestNodeDelegateNew(long botEntityId, MyCubeGrid grid, Vector3D start, Vector3D? up, out Vector3D result, bool allowAirNodes);
        delegate bool GetBotAndRelationTo(long botEntityId, long otherIdentityId, out MyRelationsBetweenPlayerAndBlock relationBetween);

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
        private Func<long, long, MyRelationsBetweenPlayerAndBlock> _getRelationshipBetween;
        private Func<string, string, bool> _canBotUseTool;
        private Action<string, string, MyPositionAndOrientation, MyCubeGrid, string, long?, Color?, Action<IMyCharacter>> _spawnBotQueued;
        private Action<MyPositionAndOrientation, byte[], MyCubeGrid, long?, Action<IMyCharacter>> _spawnBotCustomQueued;
        private Func<long, SpawnData, bool> _switchBotRole;
        private Func<long, string, List<string>, bool> _switchBotRoleSlim;
        private GetClosestNodeDelegate _getClosestValidNode;
        private GetClosestNodeDelegateNew _getClosestValidNodeNew;
        private GetBotAndRelationTo _getBotAndRelationTo;
        private Func<long, long> _getBotOwnerId;
        private Func<long, string, bool> _switchBotWeapon;
        private Action<Dictionary<long, IMyCharacter>, bool, bool, bool> _getBots;
        private Action<MyCubeGrid, List<Vector3I>, int, bool, bool, Action> _getInteriorNodes;
        private Func<IMyCubeGrid, bool> _isGridValidForPathfinding;

        private void ReceiveModMessage(object payload) {
            if (Valid)
                return;

            var dict = payload as Dictionary<string, Delegate>;

            if (dict == null)
                return;

            try {

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
                _getClosestValidNode = dict["GetClosestValidNode"] as GetClosestNodeDelegate;
                _createGridMap = dict["CreateGridMap"] as Func<MyCubeGrid, MatrixD?, bool>;
                _isGridMapReady = dict["IsGridMapReady"] as Func<MyCubeGrid, bool>;
                _removeBot = dict["RemoveBot"] as Func<long, bool>;
                _perform = dict["Perform"] as Action<long, string>;
                _speak = dict["Speak"] as Action<long, string>;
                _isBot = dict["IsBot"] as Func<long, bool>;
                _getRelationshipBetween = dict["GetRelationshipBetween"] as Func<long, long, MyRelationsBetweenPlayerAndBlock>;
                _getBotAndRelationTo = dict["GetBotAndRelationTo"] as GetBotAndRelationTo;
                _spawnBotQueued = dict["SpawnBotQueued"] as Action<string, string, MyPositionAndOrientation, MyCubeGrid, string, long?, Color?, Action<IMyCharacter>>;
                _spawnBotCustomQueued = dict["SpawnBotCustomQueued"] as Action<MyPositionAndOrientation, byte[], MyCubeGrid, long?, Action<IMyCharacter>>;
                _setBotPatrol = dict["SetBotPatrol"] as Func<long, List<Vector3D>, bool>;
                _canBotUseTool = dict["CanRoleUseTool"] as Func<string, string, bool>;
                _switchBotRole = dict["SwitchBotRole"] as Func<long, SpawnData, bool>;
                _getClosestValidNodeNew = dict["GetClosestValidNodeNew"] as GetClosestNodeDelegateNew;
                _switchBotRoleSlim = dict["SwitchBotRoleSlim"] as Func<long, string, List<string>, bool>;
                _switchBotWeapon = dict["SwitchBotWeapon"] as Func<long, string, bool>;
                _getBotOwnerId = dict["GetBotOwnerId"] as Func<long, long>;
                _getBots = dict["GetBots"] as Action<Dictionary<long, IMyCharacter>, bool, bool, bool>;
                _getInteriorNodes = dict["GetInteriorNodes"] as Action<MyCubeGrid, List<Vector3I>, int, bool, bool, Action>;
                _isGridValidForPathfinding = dict["IsValidForPathfinding"] as Func<IMyCubeGrid, bool>;

            } catch {

                Valid = false;
                return;

            }

            Valid = true;

        }

    }

}
