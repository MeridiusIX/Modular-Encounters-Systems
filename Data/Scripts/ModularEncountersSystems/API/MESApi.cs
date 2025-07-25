using System;
using System.Collections.Generic;
using ProtoBuf;
using Sandbox.ModAPI;
using VRage;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

//Change namespace to your mod's namespace
namespace ModularEncountersSystems.API {
	public class MESApi {

		public bool MESApiReady;

		private const long _mesModId = 1521905890;
		private Action<Vector3D, string, double, int, int, int> _addKnownPlayerLocation;
		private Func<Vector3D, string, string, bool, bool, List<string>, string, MatrixD, Vector3D, bool, long, bool> _apiSpawnRequest;
		private Action<bool, Action<IMyRemoteControl, string, string, IMyEntity, Vector3D>> _behaviorTriggerActivationWatcher; //TODO: Complete This
		private Action<string, MatrixD, long, ulong> _chatCommand;
		private Action<Vector3D, string, double, bool> _changeKnownPlayerLocationSize;
		private Func<string, string> _convertRandomNamePatterns;
		private Func<List<string>, MatrixD, Vector3, bool, string, string, bool> _customSpawnRequest;
		private Func<Dictionary<string, object>, bool> _customSpawnRequest2;
		private Func<IMyCubeGrid, Vector3D> _getDespawnCoords;
		private Func<List<string>> _getSpawnGroupBlackList;
		private Action<string, List<string>> _getSpawnGroupsByType;
		private Action<long, string, List<MyTuple<IMyRadioAntenna, DateTime>>> _getPlayerInhibitorData;
		private Func<List<string>> _getNpcNameBlackList;
		private Func<Vector3D, bool, string, bool> _isPositionInKnownPlayerLocation;
		private Func<IMyCubeGrid, Vector3D> _getNpcStartCoordinates;
		private Func<IMyCubeGrid, Vector3D> _getNpcEndCoordinates;
		private Action<bool, string, Func<IMyRemoteControl, string, IMyEntity, Vector3D, bool>> _registerBehaviorCustomTrigger;
		private Action<bool, Action<IMyRemoteControl, IMyCubeGrid>> _registerCompromisedRemoteWatcher;
		private Action<bool, string, Func<string, string, string, Vector3D, bool>> _registerCustomSpawnCondition;
		private Func<IMyCubeGrid, Action<IMyCubeGrid, string>, bool> _registerDespawnWatcher;
		private Action<IMyRemoteControl, string> _registerRemoteControlCode;
		private Action<Action<IMyCubeGrid>, bool> _registerSuccessfulSpawnAction;
		private Action<Vector3D, string, bool> _removeKnownPlayerLocation;
		private Action<IMyCubeGrid, bool> _setCargoShipOverride;
		private Action<bool> _setCombatPhase;
		private Func<IMyCubeGrid, bool, bool> _setSpawnerIgnoreForDespawn;
		private Action<string, bool, Vector3D?> _setZoneEnabled;
		private Func<Vector3D, List<string>, bool> _spawnBossEncounter;
		private Func<Vector3D, List<string>, bool> _spawnPlanetaryCargoShip;
		private Func<Vector3D, List<string>, bool> _spawnPlanetaryInstallation;
		private Func<Vector3D, List<string>, bool> _spawnRandomEncounter;
		private Func<Vector3D, List<string>, bool> _spawnSpaceCargoShip;
		private Action<Vector3D> _processStaticEncountersAtLocation;
		private Action<string, bool> _toggleSpawnGroupEnabled;
		private Action<bool, string, Action<object[]>> _registerCustomAction;
		private Action<string, List<string>, List<string>> _insertInstanceEventGroup;
		private Action<List<string>, Vector3D, string, double, long> _sendBehaviorCommand;
		private Action<bool, string, Func<string, string, List<string>, Vector3D, Dictionary<string, string>>> _registerCustomMissionMapping;
		//Create this object in your SessionComponent LoadData() Method
		public MESApi() {

			MyAPIGateway.Utilities.RegisterMessageHandler(_mesModId, APIListener);

		}

		/// <summary>
		/// Used to Create a Known Player Location that SpawnGroups can use as a Spawn Condition
		/// If a KPL already exists within the radius of the newly created location, its timer will be reset.
		/// </summary>
		/// <param name="coords"></param>
		/// <param name="faction"></param>
		/// <param name="radius"></param>
		/// <param name="expirationMinutes"></param>
		/// <param name="maxSpawns"></param>
		/// <param name="minThreatForAvoidingAbandonment"></param>
		public void AddKnownPlayerLocation(Vector3D coords, string faction, double radius, int expirationMinutes, int maxSpawns, int minThreatForAvoidingAbandonment) => _addKnownPlayerLocation?.Invoke(coords, faction, radius, expirationMinutes, maxSpawns, minThreatForAvoidingAbandonment);

		/// <summary>
		/// This method allows you to spawn an encounter directly from the MES SpawnRequest() method. It is the most powerful and flexible way to spawn an encounter.
		/// </summary>
		/// <param name="coords"></param>
		/// <param name="source"></param>
		/// <param name="spawningTypeString">This needs to be one of the SpawningType enums (as a string) found in the Spawning\SpawnRequest.cs file</param>
		/// <param name="forceSpawn"></param>
		/// <param name="adminSpawn"></param>
		/// <param name="eligibleNames"></param>
		/// <param name="factionOverride"></param>
		/// <param name="spawnMatrix"></param>
		/// <param name="customVelocity"></param>
		/// <param name="ignoreSafetyChecks"></param>
		/// <param name="ownerOverride"></param>
		/// <returns></returns>
		public bool ApiSpawnRequest(Vector3D coords, string source, string spawningTypeString, bool forceSpawn = false, bool adminSpawn = false, List<string> eligibleNames = null, string factionOverride = null, MatrixD spawnMatrix = new MatrixD(), Vector3D customVelocity = new Vector3D(), bool ignoreSafetyChecks = false, long ownerOverride = -1) => _apiSpawnRequest?.Invoke(coords, source, spawningTypeString, forceSpawn, adminSpawn, eligibleNames, factionOverride, spawnMatrix, customVelocity, ignoreSafetyChecks, ownerOverride) ?? false;

		/// <summary>
		/// This method allows you to register a method that will be invoked each time a RivalAI Behavior Action is Successfully Triggered.
		/// </summary>
		/// <param name="register">If true, the method provided will be registered. If false, the provided method will be deregistered.</param>
		/// <param name="action">The method you want invoked when an Action is triggered.</param>
		/*
			Action Parameters:
			IMyRemoteControl:  The remote control the behavior is attached to
			string:            The Trigger Profile SubtypeId that activated the Action Profile
			string:            The Action Profile SubtypeId that was activated by the Trigger
			IMyEntity:         The current Targeted Entity the behavior has. Null if no current target.
			Vector3D:          The current waypoint the behavior is using.
		*/
		public void BehaviorTriggerActivationWatcher(bool register, Action<IMyRemoteControl, string, string, IMyEntity, Vector3D> action) => _behaviorTriggerActivationWatcher?.Invoke(register, action);

		/// <summary>
		/// Allows you to submit a chat command via the API.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="playerPosition"></param>
		/// <param name="identityId"></param>
		/// <param name="steamUserId"></param>
		public void ChatCommand(string message, MatrixD playerPosition, long identityId, ulong steamUserId) => _chatCommand?.Invoke(message, playerPosition, identityId, steamUserId);

        /// <summary>
        /// Used To Spawn A Random SpawnGroup From A Provided List At A Provided Location. The Spawn Will Not Be Categorized As A CargoShip/RandomEncounter/Etc. The spawngroup/conditions must also use the [RivalAiSpawn:true] tag to be able to spawn with this command.
        /// </summary>
        /// <param name="spawnGroups">List of SpawnGroups you want to attempt spawning from</param>
        /// <param name="coords">The coordinates the Spawn will use</param>
        /// <param name="forwardDir">Forward Direction vector for the spawn</param>
        /// <param name="upDir">Up Direction Vector for the spawn</param>
        /// <param name="velocity">Velocity vector</param>
        /// <param name="factionOverride">Faction tag you want spawngroup to use, regardless of its settings</param>
        /// <param name="spawnProfileId">Identifier for your mod so MES can properly log where the spawn request originated from</param>
        public bool CustomSpawnRequest(List<string> spawnGroups, MatrixD spawningMatrix, Vector3 velocity, bool ignoreSafetyCheck, string factionOverride, string spawnProfileId) => _customSpawnRequest?.Invoke(spawnGroups, spawningMatrix, velocity, ignoreSafetyCheck, factionOverride, spawnProfileId) ?? false;

        /// <summary>
        /// Used To Spawn A Random SpawnGroup From A Provided List At A Provided Location. The Spawn Will Not Be Categorized As A CargoShip/RandomEncounter/Etc. The spawngroup/conditions must also use the [RivalAiSpawn:true] tag to be able to spawn with this command
        /// </summary>
        public bool CustomSpawnRequest(CustomSpawnRequestArgs args) => _customSpawnRequest2?.Invoke(args.ToDictionary()) ?? false;
		
		/// <summary>
		/// Gets the Despawn Coords that are generated from a ship spawned as either Space or Planet CargoShip.
		/// Returns Vector3D.Zero if no Coords can be found.
		/// </summary>
		/// <param name="cubeGrid">The cubegrid of the NPC you want to check Despawn Coords For</param>
		/// <returns></returns>
		public Vector3D GetDespawnCoords(IMyCubeGrid cubeGrid) => _getDespawnCoords?.Invoke(cubeGrid) ?? Vector3D.Zero;

		/// <summary>
		/// Gets all active inhibitors in range of a provided player identity id
		/// </summary>
		/// <param name="playerIdentityId">The identity id of the player you want to check against</param>
		/// <param name="inhibitorType">The type of inhibitor you want to check for. "Drill", "Energy", "Jetpack", "Personnel"</param>
		/// <param name="inhibitorData">The collection the data will be sent to.</param>
		public void GetPlayerInhibitorData(long playerIdentityId, string inhibitorType, List<MyTuple<IMyRadioAntenna, DateTime>> inhibitorData) => _getPlayerInhibitorData(playerIdentityId, inhibitorType, inhibitorData);

		/// <summary>
		/// Get a String List of all Current SpawnGroup SubtypeNames Currently in the MES Blacklist
		/// </summary>
		/// <returns>List of SpawnGroup SubtypeNames</returns>
		public List<string> GetSpawnGroupBlackList() => _getSpawnGroupBlackList?.Invoke() ?? new List<string>();

		/// <summary>
		/// Uses the name of a SpawningType to collect a list of SpawnGroup names and adds them to a provided string list.
		/// </summary>
		/// <param name="spawningType">This needs to be one of the SpawningType enums (as a string) found in the Spawning\SpawnRequest.cs file</param>
		/// <param name="spawnGroupNames"></param>
		public void GetSpawnGroupsByType(string spawningType, List<string> spawnGroupNames) => _getSpawnGroupsByType?.Invoke(spawningType, spawnGroupNames);

		/// <summary>
		/// Get a String List of all Current SpawnGroup SubtypeNames Currently in the MES Blacklist
		/// </summary>
		/// <returns>List of NPC Grid Names</returns>
		public List<string> GetNpcNameBlackList() => _getNpcNameBlackList?.Invoke() ?? new List<string>();

		/// <summary>
		/// Indicates whether a set of coordinates is within a Known Player Location.
		/// Accepts additional parameters to narrow search to faction specific Locations.
		/// </summary>
		/// <param name="coords">The coordinates you want to check</param>
		/// <param name="mustMatchFaction">Indicates if the faction match checks should be used</param>
		/// <param name="faction">Faction Tag to check against if mustMatchFaction is true</param>
		/// <returns>true if position is in a valid Known Player Location</returns>
		public bool IsPositionInKnownPlayerLocation(Vector3D coords, bool mustMatchFaction = false, string faction = "") => _isPositionInKnownPlayerLocation?.Invoke(coords, mustMatchFaction, faction) ?? false;

		/// <summary>
		/// Allows you to provide a string that will be processed by the Random Name Generator
		/// </summary>
		/// <param name="text">The string you want to process</param>
		/// <returns>A string with all Random Name Patterns processed</returns>
		public string ConvertRandomNamePatterns(string text) => _convertRandomNamePatterns?.Invoke(text) ?? text;

		/// <summary>
		/// Allows you to get the coordinates the NPC spawned at if it was spawned via MES
		/// </summary>
		/// <param name="cubeGrid">The cubegrid of the NPC you want to check</param>
		/// <returns>Coordinates of Start Position. Returns Vector3D.Zero if not found</returns>
		public Vector3D GetNpcStartCoordinates(IMyCubeGrid cubeGrid) => _getNpcStartCoordinates?.Invoke(cubeGrid) ?? Vector3D.Zero;

		/// <summary>
		/// Allows you to get the coordinates the NPC will despawn at if it was spawned via MES as a Cargo Ship
		/// </summary>
		/// <param name="cubeGrid">The cubegrid of the NPC you want to check</param>
		/// <returns>Coordinates of End Position. Returns Vector3D.Zero if not found</returns>
		public Vector3D GetNpcEndCoordinates(IMyCubeGrid cubeGrid) => _getNpcEndCoordinates?.Invoke(cubeGrid) ?? Vector3D.Zero;

		/// <summary>
		/// Allows you to register a method that is invoked when a Behavior Trigger is checked. The Trigger will pass or fail depending on the bool output of your provided method.
		/// </summary>
		/// <param name="register">If true, the method provided will be registered. If false, the provided method will be deregistered.</param>
		/// <param name="methodIdentifier">A unique name that is used to link your method to a Trigger Profile</param>
		/// <param name="func">The method you want invoked when Trigger is activated.</param>
		/*
			Func Parameters:
			IMyRemoteControl:  The remote control the behavior is attached to
			string:            The Trigger Profile SubtypeId that is being checked
			IMyEntity:         The current Targeted Entity the behavior has. Null if no current target.
			Vector3D:          The current waypoint the behavior is using.
		*/
		public void RegisterBehaviorCustomTrigger(bool register, string methodIdentifier, Func<IMyRemoteControl, string, IMyEntity, Vector3D, bool> func) => _registerBehaviorCustomTrigger?.Invoke(register, methodIdentifier, func);

		/// <summary>
		/// Allows you to register a method that is triggered whenever a RivalAI Remote Control Block is Compromised
		/// </summary>
		/// <param name="register"></param>
		/// <param name="action"></param>
		public void RegisterCompromisedRemoteWatcher(bool register, Action<IMyRemoteControl, IMyCubeGrid> action) => _registerCompromisedRemoteWatcher?.Invoke(register, action);

		/// <summary>
		/// Allows you to register a method that is invoked when a SpawnGroup's Conditions are being evaluated. The SpawnGroup eligiblity will pass or fail depending on the bool output of your provided method.
		/// </summary>
		/// <param name="register">If true, the method provided will be registered. If false, the provided method will be deregistered.</param>
		/// <param name="methodIdentifier">A unique name that is used to link your method to a SpawnCondition</param>
		/// <param name="func">The method you want invoked when Spawning is requested.</param>
		/*
			Func Parameters:
			string:            The SpawnGroup SubtypeID
			string:            The SpawnConditions Profile SubtypeID
			string:            The type of spawn requested.
			Vector3D:          The location of the entity where spawning will take place (eg: player, drone, etc).
		*/
		public void RegisterCustomSpawnCondition(bool register, string methodIdentifier, Func<string, string, string, Vector3D, bool> func) => _registerCustomSpawnCondition?.Invoke(register, methodIdentifier, func);

		/// <summary>
		/// Allows you to provide an action that will be invoked when the spawner despawns a grid.
		/// The action provided has parameters for the targeted cubegrid and a string that identifies what sort of despawn occured (CleanUp or EndPath)
		/// </summary>
		/// <param name="cubeGrid">The cubegrid you want to register.</param>
		/// <param name="action">The action you want to invoke on despawn.</param>
		/// <returns>Whether or not the handler was registered successfully.</returns>
		public bool RegisterDespawnWatcher(IMyCubeGrid cubeGrid, Action<IMyCubeGrid, string> action) => _registerDespawnWatcher?.Invoke(cubeGrid, action) ?? false;

		/// <summary>
		/// Registers a Remote Control and a string code with the Spawner so other Duplicate Spawn Distances can be Controlled
		/// </summary>
		/// <param name="remoteControl">Remote Control that is referenced</param>
		/// <param name="code">Code associated with Remote Control (this is used in the spawngroup)</param>
		public void RegisterRemoteControlCode(IMyRemoteControl remoteControl, string code) => _registerRemoteControlCode?.Invoke(remoteControl, code);

		public void RegisterSuccessfulSpawnAction(Action<IMyCubeGrid> action, bool register) => _registerSuccessfulSpawnAction.Invoke(action, register);

		/// <summary>
		/// Allows you to remove a Known Player Location at a set of coordinates
		/// </summary>
		/// <param name="coords">The coordinates to check for KPLs</param>
		/// <param name="faction">Remove only a specific faction via their Tag</param>
		/// <param name="removeAll">If true, removes all KPLs at the coords</param>
		public void RemoveKnownPlayerLocation(Vector3D coords, string faction = "", bool removeAll = false) => _removeKnownPlayerLocation?.Invoke(coords, faction, removeAll);

		public void SetCargoShipOverride(IMyCubeGrid cubeGrid, bool enabled) => _setCargoShipOverride(cubeGrid, enabled);

		/// <summary>
		/// Allows you to enable or disable Combat Phase in the world.
		/// </summary>
		/// <param name="enabled">false disables combat phase, true enables combat phase</param>
		public void SetCombatPhase(bool enabled) => _setCombatPhase(enabled);

		/// <summary>
		/// Allows you to enable or disable an MES Zone by name, and optionally at a set of coords in case there are multiple zones with same name
		/// </summary>
		/// <param name="zoneName">Zone Name</param>
		/// <param name="enabled">Whether the Zone should be enabled or not</param>
		/// <param name="zoneAtCoords">Optional position / coords that are checked against</param>
		public void SetZoneEnabled(string zoneName, bool enabled, Vector3D? zoneAtCoords = null) => _setZoneEnabled(zoneName, enabled, zoneAtCoords);

		/// <summary>
		/// Allows you to set a grid to be ignored or considered by the MES Cleanup Processes
		/// </summary>
		/// <param name="cubeGrid">The cubegrid of the NPC you want to set</param>
		/// <param name="ignoreSetting">Whether or not the grid should be ignored by cleanup</param>
		/// <returns>Returns a bool indicating if the change was successful or not</returns>
		public bool SetSpawnerIgnoreForDespawn(IMyCubeGrid cubeGrid, bool ignoreSetting) => _setSpawnerIgnoreForDespawn?.Invoke(cubeGrid, ignoreSetting) ?? false;

		/// <summary>
		/// Allows you to request a Boss Encounter Spawn at a position and with a selection of spawnGroups
		/// </summary>
		/// <param name="coords">The coordinates where a player would normally be (used as the origin to calculate the spawn from)</param>
		/// <param name="spawnGroups">The spawnGroups you want to potentially spawn</param>
		/// <returns>true or false depending on if the spawn was successful</returns>
		public bool SpawnBossEncounter(Vector3D coords, List<string> spawnGroups) => _spawnBossEncounter?.Invoke(coords, spawnGroups) ?? false;

		/// <summary>
		/// Allows you to request a Boss Encounter Spawn at a position and with a selection of spawnGroups
		/// </summary>
		/// <param name="coords">The coordinates where a player would normally be (used as the origin to calculate the spawn from)</param>
		/// <param name="spawnGroups">The spawnGroups you want to potentially spawn</param>
		/// <returns>true or false depending on if the spawn was successful</returns>
		public bool SpawnPlanetaryCargoShip(Vector3D coords, List<string> spawnGroups) => _spawnPlanetaryCargoShip?.Invoke(coords, spawnGroups) ?? false;

		/// <summary>
		/// Allows you to request a Boss Encounter Spawn at a position and with a selection of spawnGroups
		/// </summary>
		/// <param name="coords">The coordinates where a player would normally be (used as the origin to calculate the spawn from)</param>
		/// <param name="spawnGroups">The spawnGroups you want to potentially spawn</param>
		/// <returns>true or false depending on if the spawn was successful</returns>
		public bool SpawnPlanetaryInstallation(Vector3D coords, List<string> spawnGroups) => _spawnPlanetaryInstallation?.Invoke(coords, spawnGroups) ?? false;

		/// <summary>
		/// Allows you to request a Boss Encounter Spawn at a position and with a selection of spawnGroups
		/// </summary>
		/// <param name="coords">The coordinates where a player would normally be (used as the origin to calculate the spawn from)</param>
		/// <param name="spawnGroups">The spawnGroups you want to potentially spawn</param>
		/// <returns>true or false depending on if the spawn was successful</returns>
		public bool SpawnRandomEncounter(Vector3D coords, List<string> spawnGroups) => _spawnRandomEncounter?.Invoke(coords, spawnGroups) ?? false;

		/// <summary>
		/// Allows you to request a Boss Encounter Spawn at a position and with a selection of spawnGroups
		/// </summary>
		/// <param name="coords">The coordinates where a player would normally be (used as the origin to calculate the spawn from)</param>
		/// <param name="spawnGroups">The spawnGroups you want to potentially spawn</param>
		/// <returns>true or false depending on if the spawn was successful</returns>
		public bool SpawnSpaceCargoShip(Vector3D coords, List<string> spawnGroups) => _spawnSpaceCargoShip?.Invoke(coords, spawnGroups) ?? false;

		/// <summary>
		/// Allows you to Enable or Disable a SpawnGroup, determining whether it can be spawned at all.
		/// </summary>
		/// <param name="spawnGroupName">The name (SubtypeId) of the Spawngroup you want to toggle</param>
		/// <param name="toggle">true for enabled, false for disabled</param>
		public void ToggleSpawnGroupEnabled(string spawnGroupName, bool toggle) => _toggleSpawnGroupEnabled?.Invoke(spawnGroupName, toggle);


		/// <summary>
		/// Processes static encounters at a specified location.
		/// </summary>
		/// <param name="position">position</param>
		public void ProcessStaticEncountersAtLocation(Vector3D position) => _processStaticEncountersAtLocation?.Invoke(position);

		/// <summary>
		/// Allows you to register a method that is invoked when a SpawnGroup's Conditions are being evaluated. The SpawnGroup eligiblity will pass or fail depending on the bool output of your provided method.
		/// </summary>
		/// <param name="register">If true, the method provided will be registered. If false, the provided method will be deregistered.</param>
		/// <param name="methodIdentifier">A unique name that is used to link your method to a SpawnCondition</param>
		/// <param name="action">The method you want invoked when Spawning is requested.</param>
		/*
			action Parameters:

		*/
		public void RegisterCustomAction(bool register, string methodIdentifier, Action<object[]> action) => _registerCustomAction?.Invoke(register, methodIdentifier, action);

		/// <summary>
		/// (...)
		/// </summary>
		/// <param name="register">If true, the method provided will be registered. If false, the provided method will be deregistered.</param>
		/// <param name="methodIdentifier">A unique name that is used to link your method to a SpawnCondition</param>
		/// <param name="func">The method you want invoked when Spawning is requested.</param>
		/*
			Func Parameters:
			string:            The Mission SubtypeId
			string:            The SpawnGroup SubtypeID
			List<string>:      The Tags of the mission profile
			Vector3D:          The location of the storeblock on contract accept.
		*/
		public void RegisterCustomMissionMapping(bool register, string methodIdentifier, Func<string, string, List<string>, Vector3D, Dictionary<string, string>> func) => _registerCustomMissionMapping?.Invoke(register, methodIdentifier, func);

        /// <summary>
        /// Allows you to send a Behavior Command..
        /// </summary>
        /// <param name="commandProfileIds">The names (SubtypeIds) of the Commandprofiles  you want to send</param>
        /// <param name="originCoords">Vector3D from where the commandprofile is send</param>
		///  <param name="overrideCommandCode">string</param>
        public void SendBehaviorCommand(List<string> commandProfileIds, Vector3D originCoords, string overrideCommandCode = "", double overrideRadius = -1, long commandOwnerId = 0) => _sendBehaviorCommand?.Invoke(commandProfileIds, originCoords, overrideCommandCode, overrideRadius, commandOwnerId);

        public void InsertInstanceEventGroup(string ProfileSubTypeID, List<string> replacekeys, List<string> replacevalues) => _insertInstanceEventGroup?.Invoke(ProfileSubTypeID, replacekeys, replacevalues);


		//Run This Method in your SessionComponent UnloadData() Method
		public void UnregisterListener() {

			MyAPIGateway.Utilities.UnregisterMessageHandler(_mesModId, APIListener);

		}

		public void APIListener(object data) {

			try {

				var dict = data as Dictionary<string, Delegate>;

				if (dict == null) {

					return;

				}

				MESApiReady = true;
				_addKnownPlayerLocation = (Action<Vector3D, string, double, int, int, int>)dict["AddKnownPlayerLocation"];
				_apiSpawnRequest = (Func<Vector3D, string, string, bool, bool, List<string>, string, MatrixD, Vector3D, bool, long, bool>)dict["ApiSpawnRequest"];
				_behaviorTriggerActivationWatcher = (Action<bool, Action<IMyRemoteControl, string, string, IMyEntity, Vector3D>>)dict["BehaviorTriggerActivationWatcher"];
				_chatCommand = (Action<string, MatrixD, long, ulong>)dict["ChatCommand"];
				_customSpawnRequest = (Func<List<string>, MatrixD, Vector3, bool, string, string, bool>)dict["CustomSpawnRequest"];
				_customSpawnRequest2 = (Func<Dictionary<string, object>, bool>)dict["CustomSpawnRequest2"];
				_getDespawnCoords = (Func<IMyCubeGrid, Vector3D>)dict["GetDespawnCoords"];
				_getSpawnGroupBlackList = (Func<List<string>>)dict["GetSpawnGroupBlackList"];
				_getSpawnGroupsByType = (Action<string, List<string>>)dict["GetSpawnGroupsByType"];
				_getNpcNameBlackList = (Func<List<string>>)dict["GetNpcNameBlackList"];
				_isPositionInKnownPlayerLocation = (Func<Vector3D, bool, string, bool>)dict["IsPositionInKnownPlayerLocation"];
				_convertRandomNamePatterns = (Func<string, string>)dict["ConvertRandomNamePatterns"];
				_getNpcStartCoordinates = (Func<IMyCubeGrid, Vector3D>)dict["GetNpcStartCoordinates"];
				_getNpcEndCoordinates = (Func<IMyCubeGrid, Vector3D>)dict["GetNpcEndCoordinates"];
				_registerCompromisedRemoteWatcher = (Action<bool, Action<IMyRemoteControl, IMyCubeGrid>>)dict["RegisterCompromisedRemoteWatcher"];
				_registerBehaviorCustomTrigger = (Action<bool, string, Func<IMyRemoteControl, string, IMyEntity, Vector3D, bool>>)dict["RegisterBehaviorCustomTrigger"];
				_registerCustomSpawnCondition = (Action<bool, string, Func<string, string, string, Vector3D, bool>>)dict["RegisterCustomSpawnCondition"];
				_registerDespawnWatcher = (Func<IMyCubeGrid, Action<IMyCubeGrid, string>, bool>)dict["RegisterDespawnWatcher"];
				_registerRemoteControlCode = (Action<IMyRemoteControl, string>)dict["RegisterRemoteControlCode"];
				_registerSuccessfulSpawnAction = (Action<Action<IMyCubeGrid>, bool>)dict["RegisterSuccessfulSpawnAction"];
				_removeKnownPlayerLocation = (Action<Vector3D, string, bool>)dict["RemoveKnownPlayerLocation"];
				_setCombatPhase = (Action<bool>)dict["SetCombatPhase"];
				_setSpawnerIgnoreForDespawn = (Func<IMyCubeGrid, bool, bool>)dict["SetSpawnerIgnoreForDespawn"];
				_setZoneEnabled = (Action<string, bool, Vector3D?>)dict["SetZoneEnabled"];
				_spawnBossEncounter = (Func<Vector3D, List<string>, bool>)dict["SpawnBossEncounter"];
				_spawnPlanetaryCargoShip = (Func<Vector3D, List<string>, bool>)dict["SpawnPlanetaryCargoShip"];
				_spawnPlanetaryInstallation = (Func<Vector3D, List<string>, bool>)dict["SpawnPlanetaryInstallation"];
				_spawnRandomEncounter = (Func<Vector3D, List<string>, bool>)dict["SpawnRandomEncounter"];
				_spawnSpaceCargoShip = (Func<Vector3D, List<string>, bool>)dict["SpawnSpaceCargoShip"];
				_processStaticEncountersAtLocation = (Action<Vector3D>)dict["ProcessStaticEncountersAtLocation"];
				_toggleSpawnGroupEnabled = (Action<string, bool>)dict["ToggleSpawnGroupEnabled"];
				_registerCustomAction = (Action<bool, string, Action<object[]>>)dict["RegisterCustomAction"];
				_insertInstanceEventGroup = (Action<string, List<string>, List<string>>)dict["InsertInstanceEventGroup"];
				_sendBehaviorCommand = (Action<List<string>, Vector3D, string,double,long>)dict["SendBehaviorCommand"];
				_registerCustomMissionMapping = (Action< bool, string, Func<string, string, List<string>, Vector3D, Dictionary<string, string>>>)dict["RegisterCustomMissionMapping"];

			} catch (Exception e) {

				MyLog.Default.WriteLineAndConsole("MES API Failed To Load For Client: " + MyAPIGateway.Utilities.GamePaths.ModScopeName);

			}


		}

		public sealed class CustomSpawnRequestArgs
		{
			/// <summary>List of SpawnGroups you want to attempt spawning from</summary>
			public List<string> SpawnGroups;

			/// <summary>The coordinates the Spawn will use</summary>
			public MatrixD SpawningMatrix;
			
			/// <summary>Velocity vector</summary>
			public Vector3 Velocity;
			
			public bool IgnoreSafetyCheck;
			
			/// <summary>Faction tag you want spawngroup to use, regardless of its settings</summary>
			public string FactionOverride;
			
			/// <summary>Identifier for your mod so MES can properly log where the spawn request originated from</summary>
			public string SpawnProfileId;
			
			/// <summary>Arbitrary user data to be inserted to NpcData</summary>
			public string Context;

			public Dictionary<string, object> ToDictionary()
			{
				return new Dictionary<string, object>
				{
					{ nameof(SpawnGroups), SpawnGroups },
					{ nameof(SpawningMatrix), SpawningMatrix },
					{ nameof(Velocity), Velocity },
					{ nameof(IgnoreSafetyCheck), IgnoreSafetyCheck },
					{ nameof(FactionOverride), FactionOverride },
					{ nameof(SpawnProfileId), SpawnProfileId },
					{ nameof(Context), Context },
				};
			}

			public static CustomSpawnRequestArgs FromDictionary(Dictionary<string, object> dictionary)
			{
				return new CustomSpawnRequestArgs
				{
					SpawnGroups = (List<string>)dictionary[nameof(SpawnGroups)],
					SpawningMatrix = (MatrixD)dictionary[nameof(SpawningMatrix)],
					Velocity = (Vector3)dictionary[nameof(Velocity)],
					IgnoreSafetyCheck = (bool)dictionary[nameof(IgnoreSafetyCheck)],
					FactionOverride = (string)dictionary[nameof(FactionOverride)],
					SpawnProfileId = (string)dictionary[nameof(SpawnProfileId)],
					Context = (string)dictionary[nameof(Context)],
				};
			}
		}

	}

}
