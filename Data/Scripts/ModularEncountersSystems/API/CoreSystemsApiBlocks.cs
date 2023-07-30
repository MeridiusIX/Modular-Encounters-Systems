using System;
using System.Collections.Generic;
using Sandbox.ModAPI;
using VRage;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.API {
    /// <summary>
    /// https://github.com/sstixrud/CoreSystems/blob/master/BaseData/Scripts/CoreSystems/Api/CoreSystemsApiBlocks.cs
    /// </summary>
    public partial class WcApi {
        private Func<IMyTerminalBlock, IDictionary<string, int>, bool> _getBlockWeaponMap;

        public bool GetBlockWeaponMap(IMyTerminalBlock weaponBlock, IDictionary<string, int> collection) =>
            _getBlockWeaponMap?.Invoke(weaponBlock, collection) ?? false;
    }
}
