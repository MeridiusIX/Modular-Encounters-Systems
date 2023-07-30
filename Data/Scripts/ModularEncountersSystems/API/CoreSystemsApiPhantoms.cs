using System;
using VRage;
using VRageMath;
using VRage.Game.Entity;

namespace ModularEncountersSystems.API {
    /// <summary>
    /// https://github.com/sstixrud/CoreSystems/blob/master/BaseData/Scripts/CoreSystems/Api/CoreSystemsApiPhantoms.cs
    /// </summary>
    public partial class WcApi {
        public enum TriggerActions {
            TriggerOff,
            TriggerOn,
            TriggerOnce,
        }

        private Func<MyEntity, MyEntity, int, bool, bool, MyTuple<bool, bool, Vector3D?>> _getTargetAssessment;
        //private Action<string, ICollection<MyTuple<MyEntity, long, int, float, uint, long>>> _getPhantomInfo;
        private Action<MyEntity, int> _setTriggerState;
        private Action<MyEntity, int, long> _addMagazines;
        private Action<MyEntity, int, string> _setAmmo;
        private Func<MyEntity, bool> _closePhantom;
        private Func<string, uint, bool, long, string, int, float?, MyEntity, bool, bool, long, bool, MyEntity> _spawnPhantom;
        private Func<MyEntity, MyEntity, int, bool> _setPhantomFocusTarget;

        /// <summary>
        /// Get information about a particular target relative to this phantom
        /// </summary>
        /// <param name="phantom"></param>
        /// <param name="target"></param>
        /// <param name="weapon"></param>
        /// <param name="mustBeInrange"></param>
        /// <param name="checkTargetOrb"></param>
        internal void GetTargetAssessment(MyEntity phantom, MyEntity target, int weapon = 0, bool mustBeInrange = false, bool checkTargetOrb = false) => _getTargetAssessment?.Invoke(phantom, target, weapon, mustBeInrange, checkTargetOrb);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phantomSubtypeId"></param>
        /// <param name="collection"></param>
        //internal void GetPhantomInfo(string phantomSubtypeId, ICollection<MyTuple<MyEntity, long, int, float, uint, long>> collection) => _getPhantomInfo?.Invoke(phantomSubtypeId, collection);

        /// <summary>
        /// Change the phantoms current fire state
        /// </summary>
        /// <param name="phantom"></param>
        /// <param name="trigger"></param>
        internal void SetTriggerState(MyEntity phantom, TriggerActions trigger) => _setTriggerState?.Invoke(phantom, (int)trigger);

        /// <summary>
        /// Add additional magazines
        /// </summary>
        /// <param name="phantom"></param>
        /// <param name="weapon"></param>
        /// <param name="quanity"></param>
        internal void AddMagazines(MyEntity phantom, int weapon, long quanity) => _addMagazines?.Invoke(phantom, weapon, quanity);
        /// <summary>
        /// Set/switch ammo
        /// </summary>
        /// <param name="phantom"></param>
        /// <param name="weapon"></param>
        /// <param name="ammoName"></param>
        internal void SetAmmo(MyEntity phantom, int weapon, string ammoName) => _setAmmo?.Invoke(phantom, weapon, ammoName);
        /// <summary>
        /// Close phantoms, required for phantoms that do not auto close
        /// </summary>
        /// <param name="phantom"></param>
        /// <returns></returns>
        internal bool ClosePhantom(MyEntity phantom) => _closePhantom?.Invoke(phantom) ?? false;

        /// <summary>
        /// string: weapon subtypeId
        /// uint: max age, defaults to never die, you must issue close request!  Max duration is 14400 ticks (4 minutes)
        /// bool: close when phantom runs out of ammo
        /// long: Number of ammo reloads phantom has per default, prior to you adding more, defaults to long.MaxValue
        /// string: name of the ammo you want the phantom to start with, if different than default
        /// TriggerActions: TriggerOff, TriggerOn, TriggerOnce
        /// float?: scales the model if defined in the WeaponDefinition for this subtypeId
        /// MyEntity: Parent's this phantom to another world entity.
        /// StringerBuilder: Assign a name to this phantom
        /// bool: Add this phantom to the world PrunningStructure
        /// bool: Enable shadows for the model. 
        /// </summary>
        /// <param name="phantomType"></param>
        /// <param name="maxAge"></param>
        /// <param name="closeWhenOutOfAmmo"></param>
        /// <param name="defaultReloads"></param>
        /// <param name="ammoOverideName"></param>
        /// <param name="trigger"></param>
        /// <param name="modelScale"></param>
        /// <param name="parnet"></param>
        /// <param name="addToPrunning"></param>
        /// <param name="shadows"></param>
        /// <param name="identityId"></param>
        /// <param name="sync"></param>

        /// <returns></returns>
        internal MyEntity SpawnPhantom(string phantomType, uint maxAge = 0, bool closeWhenOutOfAmmo = false, long defaultReloads = long.MaxValue, string ammoOverideName = null, TriggerActions trigger = TriggerActions.TriggerOff, float? modelScale = null, MyEntity parnet = null, bool addToPrunning = false, bool shadows = false, long identityId = 0, bool sync = false)
            => _spawnPhantom?.Invoke(phantomType, maxAge, closeWhenOutOfAmmo, defaultReloads, ammoOverideName, (int)trigger, modelScale, parnet, addToPrunning, shadows, identityId, sync) ?? null;

        /// <summary>
        /// Set/switch ammo
        /// focusId is a value between 0 and 1, can have two active focus targets.
        /// </summary>
        /// <param name="phantom"></param>
        /// <param name="target"></param>
        /// <param name="focusId"></param>
        internal bool SetPhantomFocusTarget(MyEntity phantom, MyEntity target, int focusId) => _setPhantomFocusTarget?.Invoke(phantom, target, focusId) ?? false;
    }
}