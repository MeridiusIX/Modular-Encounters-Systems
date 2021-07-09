using ModularEncountersSystems.Logging;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game.ModAPI;

namespace ModularEncountersSystems.API {
    public class AiEnabledApi {

        public bool Valid; //Can Check This To Ensure API Loaded Properly

        private const long _botControllerModChannel = 2408831996; //This is the channel this object will receive API methods at. Sender should also use this.
        private Func<string, string, MyPositionAndOrientation, MyCubeGrid, string, long?, IMyCharacter> _spawnBot;
        private Func<string[]> _getFriendlyBotRoles, _getNPCBotRoles, _getBotSubtypes;
        private Func<bool> _canSpawn;

        //Create Instance of this object in your SessionComponent LoadData() method.
        public AiEnabledApi() {

            MyAPIGateway.Utilities.RegisterMessageHandler(_botControllerModChannel, ReceiveModMessage);
        }

        public void Close() {
            try {
                MyAPIGateway.Utilities.UnregisterMessageHandler(_botControllerModChannel, ReceiveModMessage);
            } finally { }
        }

        /////////////////////////////////////////////
        //API Methods Start:
        /////////////////////////////////////////////

        /// <summary>
        /// This method will spawn a Bot with Custom Behavior
        /// </summary>
        /// <param name="subType">The SubtypeId of the Bot you want to Spawn (<see cref="GetBotSubtypes"/>)</param>
        /// <param name="displayName">The DisplayName of the Bot</param>
        /// <param name="role">Bot Role: <see cref="GetFriendlyBotRoles"/> or <see cref="GetNPCBotRoles"/>. If not supplied, it will be determined by the subType's default usage</param>
        /// <param name="positionAndOrientation">Position and Orientation</param>
        /// <param name="grid">If supplied, the Bot will start with a Cubegrid Map for pathfinding, otherwise a Voxel Map</param>
        /// <param name="owner">Owner's IdentityId for the Bot (if a HelperBot)</param>
        /// <returns></returns>
        public IMyCharacter SpawnBot(string subType, string displayName, MyPositionAndOrientation positionAndOrientation, MyCubeGrid grid = null, string role = null, long? owner = null) => _spawnBot?.Invoke(subType, displayName, positionAndOrientation, grid, role, owner) ?? null;

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
        /// Retrieves the current set of available non-friendly bot roles (includes neutral)
        /// </summary>
        public string[] GetNPCBotRoles() => _getNPCBotRoles?.Invoke() ?? null;

        /////////////////////////////////////////////
        //API Methods End
        /////////////////////////////////////////////

        private void ReceiveModMessage(object payload) {
            if (Valid)
                return;

            var dict = payload as Dictionary<string, Delegate>;

            if (dict == null)
                return;

            try {

                _spawnBot = dict["SpawnBot"] as Func<string, string, MyPositionAndOrientation, MyCubeGrid, string, long?, IMyCharacter>;
                _getFriendlyBotRoles = dict["GetFriendlyRoles"] as Func<string[]>;
                _getNPCBotRoles = dict["GetNPCRoles"] as Func<string[]>;
                _getBotSubtypes = dict["GetBotSubtypes"] as Func<string[]>;
                _canSpawn = dict["CanSpawn"] as Func<bool>;

            } catch (Exception e) {
                Valid = false;
                return;

            }

            Valid = true;

        }

    }

}
