
using Sandbox.Game.Entities;
using Sandbox.ModAPI;

using System;
using System.Collections.Generic;

using VRage;
using VRage.Game.ModAPI;

using VRageMath;

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
			} finally { }
		}

		/////////////////////////////////////////////
		//API Methods Start:
		/////////////////////////////////////////////

		/// <summary>
		/// Check this to ensure API loaded properly
		/// </summary>
		public bool Valid;

		/// <summary>
		/// This method will spawn a Bot with Custom Behavior
		/// </summary>
		/// <param name="subType">The SubtypeId of the Bot you want to Spawn (<see cref="GetBotSubtypes"/>)</param>
		/// <param name="displayName">The DisplayName of the Bot</param>
		/// <param name="role">Bot Role: <see cref="GetFriendlyBotRoles"/>, <see cref="GetNPCBotRoles"/>, or <see cref="GetNeutralBotRoles"/>. If not supplied, it will be determined by the subType's default usage</param>
		/// <param name="positionAndOrientation">Position and Orientation</param>
		/// <param name="grid">If supplied, the Bot will start with a Cubegrid Map for pathfinding, otherwise a Voxel Map</param>
		/// <param name="owner">Owner's IdentityId for the Bot (if a HelperBot)</param>
		/// <returns></returns>
		public IMyCharacter SpawnBot(string subType, string displayName, MyPositionAndOrientation positionAndOrientation, MyCubeGrid grid = null, string role = null, long? owner = null, Color? color = null) => _spawnBot?.Invoke(subType, displayName, positionAndOrientation, grid, role, owner, color) ?? null;

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

		/////////////////////////////////////////////
		//API Methods End
		/////////////////////////////////////////////

		private const long _botControllerModChannel = 2408831996; //This is the channel this object will receive API methods at. Sender should also use this.
		private Func<string, string, MyPositionAndOrientation, MyCubeGrid, string, long?, Color?, IMyCharacter> _spawnBot;
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

		private void ReceiveModMessage(object payload) {
			if (Valid)
				return;

			var dict = payload as Dictionary<string, Delegate>;

			if (dict == null)
				return;

			try {

				_spawnBot = dict["SpawnBot"] as Func<string, string, MyPositionAndOrientation, MyCubeGrid, string, long?, Color?, IMyCharacter>;
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

			} catch (Exception e) {

				Valid = false;
				return;

			}

			Valid = true;

		}

	}

}
