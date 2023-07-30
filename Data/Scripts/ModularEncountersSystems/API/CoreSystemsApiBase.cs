using System;
using System.Collections.Generic;
using Sandbox.ModAPI;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.API{
    /// <summary>
    /// https://github.com/sstixrud/CoreSystems/blob/master/BaseData/Scripts/CoreSystems/Api/CoreSystemsApiBase.cs
    /// </summary>
    public partial class WcApi {
        private bool _apiInit;

        private Action<IList<byte[]>> _getAllWeaponDefinitions;
        private Action<ICollection<MyDefinitionId>> _getCoreWeapons;
        private Action<ICollection<MyDefinitionId>> _getNpcSafeWeapons;
        private Action<IDictionary<MyDefinitionId, List<MyTuple<int, MyTuple<MyDefinitionId, string, string, bool>>>>> _getAllWeaponMagazines;
        private Action<IDictionary<MyDefinitionId, List<MyTuple<int, MyTuple<MyDefinitionId, string, string, bool>>>>> _getAllNpcSafeWeaponMagazines;

        private Action<ICollection<MyDefinitionId>> _getCoreStaticLaunchers;
        private Action<ICollection<MyDefinitionId>> _getCoreTurrets;
        private Action<ICollection<MyDefinitionId>> _getCorePhantoms;
        private Action<ICollection<MyDefinitionId>> _getCoreRifles;
        private Action<IList<byte[]>> _getCoreArmors;

        private Action<long, int, Action<ListReader<MyTuple<ulong, long, int, MyEntity, MyEntity, ListReader<MyTuple<Vector3D, object, float>>>>>> _registerDamageEvent;
        private Func<long, bool, Func<MyEntity, IMyCharacter, long, int, bool>, bool> _targetFocusHandler;
        private Func<long, bool, Func<IMyCharacter, long, int, bool>, bool> _hudHandler;
        private Func<long, bool, Func<Vector3D, Vector3D, int, bool, object, int, int, int, bool>, bool> _shootHandler;
        private Action<MyEntity, int, Action<int, bool>> _monitorEvents;
        private Action<MyEntity, int, Action<int, bool>> _unmonitorEvents;
        private Action<MyEntity, int, Action<long, int, ulong, long, Vector3D, bool>> _addProjectileMonitor;
        private Action<MyEntity, int, Action<long, int, ulong, long, Vector3D, bool>> _removeProjectileMonitor;

        private Func<MyEntity, object, int, double, bool> _shootRequest;

        private Func<ulong, MyTuple<Vector3D, Vector3D, float, float, long, string>> _getProjectileState;
        private Action<ulong, MyTuple<bool, Vector3D, Vector3D, float>> _setProjectileState;

        private Action<MyEntity, ICollection<MyTuple<MyEntity, float>>> _getSortedThreats;
        private Action<MyEntity, ICollection<MyEntity>> _getObstructions;
        private Func<MyEntity, int, MyEntity> _getAiFocus;
        private Func<MyEntity, MyEntity, int, bool> _setAiFocus;
        private Func<MyEntity, long, bool> _releaseAiFocus;
        private Func<MyEntity, bool> _hasAi;
        private Func<MyEntity, bool> _hasCoreWeapon;

        private Func<MyEntity, bool> _toggoleInfiniteResources;
        private Action<MyEntity> _disableRequiredPower;

        private Func<MyEntity, long> _getPlayerController;
        private Func<MyEntity, MyTuple<bool, int, int>> _getProjectilesLockedOn;
        private Action<MyEntity, ICollection<Vector3D>> _getProjectilesLockedOnPos;
        private Func<MyDefinitionId, float> _getMaxPower;

        private Func<MyEntity, float> _getOptimalDps;
        private Func<MyEntity, float> _getConstructEffectiveDps;
        private Func<MyEntity, MyTuple<bool, bool>> _isInRange;

        private Func<MyEntity, int, MyTuple<bool, bool, bool, MyEntity>> _getWeaponTarget;
        private Action<MyEntity, MyEntity, int> _setWeaponTarget;
        private Action<MyEntity, bool, int> _fireWeaponOnce;
        private Action<MyEntity, bool, bool, int> _toggleWeaponFire;
        private Func<MyEntity, int, bool, bool, bool> _isWeaponReadyToFire;
        private Func<MyEntity, int, float> _getMaxWeaponRange;
        private Func<MyEntity, ICollection<string>, int, bool> _getTurretTargetTypes;
        private Action<MyEntity, ICollection<string>, int> _setTurretTargetTypes;
        private Action<MyEntity, float> _setBlockTrackingRange;
        private Func<MyEntity, MyEntity, int, bool> _isTargetAligned;
        private Func<MyEntity, MyEntity, int, MyTuple<bool, Vector3D?>> _isTargetAlignedExtended;
        private Func<MyEntity, MyEntity, int, bool> _canShootTarget;
        private Func<MyEntity, MyEntity, int, Vector3D?> _getPredictedTargetPos;
        private Func<MyEntity, float> _getHeatLevel;
        private Func<MyEntity, float> _currentPowerConsumption;
        private Func<MyEntity, int, string> _getActiveAmmo;
        private Action<MyEntity, int, string> _setActiveAmmo;

        private Func<MyEntity, int, Matrix> _getWeaponAzimuthMatrix;
        private Func<MyEntity, int, Matrix> _getWeaponElevationMatrix;
        private Func<MyEntity, MyEntity, bool, bool, bool> _isTargetValid;
        private Func<MyEntity, int, bool> _isWeaponShooting;
        private Func<MyEntity, int, int> _getShotsFired;
        private Action<MyEntity, int, List<MyTuple<Vector3D, Vector3D, Vector3D, Vector3D, MatrixD, MatrixD>>> _getMuzzleInfo;
        private Func<MyEntity, int, MyTuple<Vector3D, Vector3D>> _getWeaponScope;
        private Func<MyEntity, int, MyTuple<MyDefinitionId, string, string, bool>> _getMagazineMap;
        private Func<MyEntity, int, MyDefinitionId, bool, bool> _setMagazine;
        private Func<MyEntity, int, bool> _forceReload;

        public void SetWeaponTarget(MyEntity weapon, MyEntity target, int weaponId = 0) =>
            _setWeaponTarget?.Invoke(weapon, target, weaponId);

        public void FireWeaponOnce(MyEntity weapon, bool allWeapons = true, int weaponId = 0) =>
            _fireWeaponOnce?.Invoke(weapon, allWeapons, weaponId);

        public void ToggleWeaponFire(MyEntity weapon, bool on, bool allWeapons, int weaponId = 0) =>
            _toggleWeaponFire?.Invoke(weapon, on, allWeapons, weaponId);

        public bool IsWeaponReadyToFire(MyEntity weapon, int weaponId = 0, bool anyWeaponReady = true,
            bool shootReady = false) =>
            _isWeaponReadyToFire?.Invoke(weapon, weaponId, anyWeaponReady, shootReady) ?? false;

        public float GetMaxWeaponRange(MyEntity weapon, int weaponId) =>
            _getMaxWeaponRange?.Invoke(weapon, weaponId) ?? 0f;

        public bool GetTurretTargetTypes(MyEntity weapon, IList<string> collection, int weaponId = 0) =>
            _getTurretTargetTypes?.Invoke(weapon, collection, weaponId) ?? false;

        public void SetTurretTargetTypes(MyEntity weapon, IList<string> collection, int weaponId = 0) =>
            _setTurretTargetTypes?.Invoke(weapon, collection, weaponId);

        public void SetBlockTrackingRange(MyEntity weapon, float range) =>
            _setBlockTrackingRange?.Invoke(weapon, range);

        public bool IsTargetAligned(MyEntity weapon, MyEntity targetEnt, int weaponId) =>
            _isTargetAligned?.Invoke(weapon, targetEnt, weaponId) ?? false;

        public MyTuple<bool, Vector3D?> IsTargetAlignedExtended(MyEntity weapon, MyEntity targetEnt, int weaponId) =>
            _isTargetAlignedExtended?.Invoke(weapon, targetEnt, weaponId) ?? new MyTuple<bool, Vector3D?>();

        public bool CanShootTarget(MyEntity weapon, MyEntity targetEnt, int weaponId) =>
            _canShootTarget?.Invoke(weapon, targetEnt, weaponId) ?? false;

        public Vector3D? GetPredictedTargetPosition(MyEntity weapon, MyEntity targetEnt, int weaponId) =>
            _getPredictedTargetPos?.Invoke(weapon, targetEnt, weaponId) ?? null;

        public float GetHeatLevel(MyEntity weapon) => _getHeatLevel?.Invoke(weapon) ?? 0f;
        public float GetCurrentPower(MyEntity weapon) => _currentPowerConsumption?.Invoke(weapon) ?? 0f;
        public void DisableRequiredPower(MyEntity weapon) => _disableRequiredPower?.Invoke(weapon);
        public bool HasCoreWeapon(MyEntity weapon) => _hasCoreWeapon?.Invoke(weapon) ?? false;

        public string GetActiveAmmo(MyEntity weapon, int weaponId) =>
            _getActiveAmmo?.Invoke(weapon, weaponId) ?? null;

        public void SetActiveAmmo(MyEntity weapon, int weaponId, string ammoType) =>
            _setActiveAmmo?.Invoke(weapon, weaponId, ammoType);

        public long GetPlayerController(MyEntity weapon) => _getPlayerController?.Invoke(weapon) ?? -1;

        public Matrix GetWeaponAzimuthMatrix(MyEntity weapon, int weaponId) =>
            _getWeaponAzimuthMatrix?.Invoke(weapon, weaponId) ?? Matrix.Zero;

        public Matrix GetWeaponElevationMatrix(MyEntity weapon, int weaponId) =>
            _getWeaponElevationMatrix?.Invoke(weapon, weaponId) ?? Matrix.Zero;

        public bool IsTargetValid(MyEntity weapon, MyEntity target, bool onlyThreats, bool checkRelations) =>
            _isTargetValid?.Invoke(weapon, target, onlyThreats, checkRelations) ?? false;

        public void GetAllWeaponDefinitions(IList<byte[]> collection) => _getAllWeaponDefinitions?.Invoke(collection);
        public void GetAllCoreWeapons(ICollection<MyDefinitionId> collection) => _getCoreWeapons?.Invoke(collection);
        public void GetNpcSafeWeapons(ICollection<MyDefinitionId> collection) => _getNpcSafeWeapons?.Invoke(collection);

        public void GetAllCoreStaticLaunchers(ICollection<MyDefinitionId> collection) => _getCoreStaticLaunchers?.Invoke(collection);
        public void GetAllWeaponMagazines(IDictionary<MyDefinitionId, List<MyTuple<int, MyTuple<MyDefinitionId, string, string, bool>>>> collection) => _getAllWeaponMagazines?.Invoke(collection);
        public void GetAllNpcSafeWeaponMagazines(IDictionary<MyDefinitionId, List<MyTuple<int, MyTuple<MyDefinitionId, string, string, bool>>>> collection) => _getAllNpcSafeWeaponMagazines?.Invoke(collection);

        public void GetAllCoreTurrets(ICollection<MyDefinitionId> collection) => _getCoreTurrets?.Invoke(collection);
        public void GetAllCorePhantoms(ICollection<MyDefinitionId> collection) => _getCorePhantoms?.Invoke(collection);
        public void GetAllCoreRifles(ICollection<MyDefinitionId> collection) => _getCoreRifles?.Invoke(collection);
        public void GetAllCoreArmors(IList<byte[]> collection) => _getCoreArmors?.Invoke(collection);

        public MyTuple<bool, int, int> GetProjectilesLockedOn(MyEntity victim) =>
            _getProjectilesLockedOn?.Invoke(victim) ?? new MyTuple<bool, int, int>();
        public void GetProjectilesLockedOnPos(MyEntity victim, ICollection<Vector3D> collection) =>
           _getProjectilesLockedOnPos?.Invoke(victim, collection);
        public void GetSortedThreats(MyEntity shooter, ICollection<MyTuple<MyEntity, float>> collection) =>
            _getSortedThreats?.Invoke(shooter, collection);
        public void GetObstructions(MyEntity shooter, ICollection<MyEntity> collection) =>
            _getObstructions?.Invoke(shooter, collection);
        public MyEntity GetAiFocus(MyEntity shooter, int priority = 0) => _getAiFocus?.Invoke(shooter, priority);
        public bool SetAiFocus(MyEntity shooter, MyEntity target, int priority = 0) =>
            _setAiFocus?.Invoke(shooter, target, priority) ?? false;
        public bool ReleaseAiFocus(MyEntity shooter, long playerId) =>
            _releaseAiFocus?.Invoke(shooter, playerId) ?? false;
        public MyTuple<bool, bool, bool, MyEntity> GetWeaponTarget(MyEntity weapon, int weaponId = 0) =>
            _getWeaponTarget?.Invoke(weapon, weaponId) ?? new MyTuple<bool, bool, bool, MyEntity>();
        public float GetMaxPower(MyDefinitionId weaponDef) => _getMaxPower?.Invoke(weaponDef) ?? 0f;
        public bool HasAi(MyEntity entity) => _hasAi?.Invoke(entity) ?? false;
        public float GetOptimalDps(MyEntity entity) => _getOptimalDps?.Invoke(entity) ?? 0f;
        public MyTuple<Vector3D, Vector3D, float, float, long, string> GetProjectileState(ulong projectileId) =>
            _getProjectileState?.Invoke(projectileId) ?? new MyTuple<Vector3D, Vector3D, float, float, long, string>();

        public float GetConstructEffectiveDps(MyEntity entity) => _getConstructEffectiveDps?.Invoke(entity) ?? 0f;
        public MyTuple<Vector3D, Vector3D> GetWeaponScope(MyEntity weapon, int weaponId) =>
            _getWeaponScope?.Invoke(weapon, weaponId) ?? new MyTuple<Vector3D, Vector3D>();

        public void AddProjectileCallback(MyEntity entity, int weaponId, Action<long, int, ulong, long, Vector3D, bool> action) =>
            _addProjectileMonitor?.Invoke(entity, weaponId, action);

        public void RemoveProjectileCallback(MyEntity entity, int weaponId, Action<long, int, ulong, long, Vector3D, bool> action) =>
            _removeProjectileMonitor?.Invoke(entity, weaponId, action);


        // block/grid/player, Threat, Other 
        public MyTuple<bool, bool> IsInRange(MyEntity entity) =>
            _isInRange?.Invoke(entity) ?? new MyTuple<bool, bool>();

        /// <summary>
        /// Set projectile values *Warning* be sure to pass in Vector3D.MinValue or float.MinValue to NOT set that value.
        /// bool = EndNow
        /// Vector3D Position
        /// Vector3D Additive velocity
        /// float BaseDamagePool
        /// </summary>
        /// <param name="projectileId"></param>
        /// <param name="values"></param>
        public void SetProjectileState(ulong projectileId, MyTuple<bool, Vector3D, Vector3D, float> values) =>
            _setProjectileState?.Invoke(projectileId, values);

        /// <summary>
        /// Gets whether the weapon is shooting, used by Hakerman's Beam Logic
        /// Unexpected behavior may occur when using this method
        /// </summary>
        /// <param name="weaponBlock"></param>
        /// <param name="weaponId"></param>
        /// <returns></returns>
        internal bool IsWeaponShooting(MyEntity weaponBlock, int weaponId) => _isWeaponShooting?.Invoke(weaponBlock, weaponId) ?? false;

        /// <summary>
        /// Gets how many shots the weapon fired, used by Hakerman's Beam Logic
        /// Unexpected behavior may occur when using this method
        /// </summary>
        /// <param name="weaponBlock"></param>
        /// <param name="weaponId"></param>
        /// <returns></returns>
        internal int GetShotsFired(MyEntity weaponBlock, int weaponId) => _getShotsFired?.Invoke(weaponBlock, weaponId) ?? -1;

        /// <summary>
        /// Gets the info of the weapon's all muzzles, used by Hakerman's Beam Logic
        /// returns: A list that contains every muzzle's Position, LocalPosition, Direction, UpDirection, ParentMatrix, DummyMatrix
        /// Unexpected behavior may occur when using this method
        /// </summary>
        /// <param name="weaponBlock"></param>
        /// <param name="weaponId"></param>
        /// <returns></returns>
        internal void GetMuzzleInfo(MyEntity weaponBlock, int weaponId, List<MyTuple<Vector3D, Vector3D, Vector3D, Vector3D, MatrixD, MatrixD>> output) =>
            _getMuzzleInfo?.Invoke(weaponBlock, weaponId, output);

        /// <summary>
        /// Entity can be a weapon or a grid/player (enables on all subgrids as well)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ToggleInfiniteResources(MyEntity entity) =>
            _toggoleInfiniteResources?.Invoke(entity) ?? false;

        /// <summary>
        /// Monitor various kind of events, see WcApiDef.WeaponDefinition.AnimationDef.PartAnimationSetDef.EventTriggers for int mapping, bool is for active/inactive
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="partId"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public void MonitorEvents(MyEntity entity, int partId, Action<int, bool> action) =>
            _monitorEvents?.Invoke(entity, partId, action);

        /// <summary>
        /// Monitor various kind of events, see WcApiDef.WeaponDefinition.AnimationDef.PartAnimationSetDef.EventTriggers for int mapping, bool is for active/inactive
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="partId"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public void UnMonitorEvents(MyEntity entity, int partId, Action<int, bool> action) =>
            _unmonitorEvents?.Invoke(entity, partId, action);

        /// <summary>
        /// Monitor all weaponcore damage
        /// </summary>
        /// <param name="modId"></param>
        /// <param name="type"></param>  0 unregister, 1 register
        /// <param name="callback"></param>  object casts (ulong = projectileId, IMySlimBlock, MyFloatingObject, IMyCharacter, MyVoxelBase, MyPlanet, MyEntity Shield see next line)
        ///                                  You can detect the shield entity in a performant way by creating a hash check ShieldHash = MyStringHash.GetOrCompute("DefenseShield");
        ///                                  then use it by Session.ShieldApiLoaded && Session.ShieldHash == ent.DefinitionId?.SubtypeId && ent.Render.Visible;  Visible means shield online
        public void RegisterDamageEvent(long modId, int type, Action<ListReader<MyTuple<ulong, long, int, MyEntity, MyEntity, ListReader<MyTuple<Vector3D, object, float>>>>> callback) {
            _registerDamageEvent?.Invoke(modId, type, callback);
        }

        /// <summary>
        /// This allows you to determine when and if a player can modify the current target focus on a player/grid/phantonm. Use only on server
        /// </summary>
        /// <param name="handledEntityId"> is the player/grid/phantom you want to control the target focus for, applies to subgrids as well</param>
        /// <param name="unregister"> be sure to unregister when you no longer want to receive callbacks</param>
        public void TargetFocushandler(long handledEntityId, bool unregister) {
            _targetFocusHandler(handledEntityId, unregister, TargetFocusCallback);
        }

        /// <summary>
        /// This callback fires whenever a player attempts to modify the target focus
        /// </summary>
        /// <param name="target"></param>
        /// <param name="requestingCharacter"></param>
        /// <param name="handledEntityId"></param>
        /// <param name="modeCode"></param>
        /// <returns></returns>
        private bool TargetFocusCallback(MyEntity target, IMyCharacter requestingCharacter, long handledEntityId, int modeCode) {
            var mode = (ChangeMode)modeCode;

            return true;
        }

        public enum ChangeMode {
            Add,
            Release,
            Lock,
        }
        /// <summary>
        ///  Enables you to allow/deny hud draw requests.  Do not use this on dedicated server.
        /// </summary>
        /// <param name="handledEntityId"></param>
        /// <param name="unregister"></param>
        public void Hudhandler(long handledEntityId, bool unregister) {
            _hudHandler?.Invoke(handledEntityId, unregister, HudCallback);
        }

        /// <summary>
        /// This callback fires whenever the hud tries to update
        /// </summary>
        /// <param name="requestingCharacter"></param>
        /// <param name="handledEntityId"></param>
        /// <param name="modeCode"></param>
        /// <returns></returns>
        private bool HudCallback(IMyCharacter requestingCharacter, long handledEntityId, int modeCode) {
            var mode = (HudMode)modeCode;

            return true;
        }

        internal enum HudMode {
            Selector,
            Reload,
            TargetInfo,
            Lead,
            Drone,
            PainterMarks,
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="weaponEntity"></param>
        /// <param name="target">MyEntity, ulong (projectile) or Vector3D</param>
        /// <param name="weaponId">Most weapons have only id 0, but some are multi-weapon entities</param>
        /// <param name="additionalDeviateShotAngle"></param>
        /// <returns></returns>
        public bool ShootRequest(MyEntity weaponEntity, object target, int weaponId = 0, double additionalDeviateShotAngle = 0) => _shootRequest?.Invoke(weaponEntity, target, weaponId, additionalDeviateShotAngle) ?? false;

        /// <summary>
        /// Enables you to monitor and approve shoot requests for this weapon/construct/player/grid network
        /// </summary>
        /// <param name="handledEntityId"></param>
        /// <param name="unregister"></param>
        /// <param name="callback"></param>
        public void ShootRequestHandler(long handledEntityId, bool unregister, Func<Vector3D, Vector3D, int, bool, object, int, int, int, bool> callback) {
            _shootHandler?.Invoke(handledEntityId, unregister, callback); // see example callback below
        }

        /// <summary>
        /// This callback fires whenever a shoot request is being evaluated for against a success criteria or is pending some action 
        /// </summary>
        /// <param name="scopePos"></param>
        /// <param name="scopeDirection"></param>
        /// <param name="requestState">This is the state of your request, state 0 means proceeding as requested</param>
        /// <param name="hasLos">This is false if wc thinks the target is occluded, you can choose to allow it to proceed anyway or not</param>
        /// <param name="target"> valid objects to cast too are MyEntity, ulong (projectile ids) and target Vector3Ds</param>
        /// <param name="currentAmmo"></param>
        /// <param name="remainingMags"></param>
        /// <param name="requestStage">The number of times this callback will fire will depend on the relevant firing stages for this weapon/ammo and how far it gets</param>
        /// <returns></returns>
        private bool ShootCallBack(Vector3D scopePos, Vector3D scopeDirection, int requestState, bool hasLos, object target, int currentAmmo, int remainingMags, int requestStage) {
            var stage = (EventTriggers)requestStage;
            var state = (ShootState)requestState;
            var targetAsEntity = target as MyEntity;
            var targetAsProjectileId = target as ulong? ?? 0;
            var targetAsPosition = target as Vector3D? ?? Vector3D.Zero;

            return true;
        }

        public enum ShootState {
            EventStart,
            EventEnd,
            Preceding,
            Canceled,
        }

        public enum EventTriggers {
            Reloading,
            Firing,
            Tracking,
            Overheated,
            TurnOn,
            TurnOff,
            BurstReload,
            NoMagsToLoad,
            PreFire,
            EmptyOnGameLoad,
            StopFiring,
            StopTracking,
            LockDelay,
            Init,
            Homing,
            TargetAligned,
            WhileOn,
            TargetRanged100,
            TargetRanged75,
            TargetRanged50,
            TargetRanged25,
        }

        /// <summary>
        /// Get active ammo Mag map from weapon 
        /// </summary>
        /// <param name="weapon"></param>
        /// <param name="weaponId"></param>
        /// <returns>Mag definitionId, mag name, ammoRound name, weapon must aim (not manual aim) true/false</returns>
        public MyTuple<MyDefinitionId, string, string, bool> GetMagazineMap(MyEntity weapon, int weaponId) {
            return _getMagazineMap?.Invoke(weapon, weaponId) ?? new MyTuple<MyDefinitionId, string, string, bool>();
        }

        /// <summary>
        ///  Set the active ammo type via passing Mag DefinitionId
        /// </summary>
        /// <param name="weapon"></param>
        /// <param name="weaponId"></param>
        /// <param name="id"></param>
        /// <param name="forceReload"></param>
        /// <returns></returns>
        public bool SetMagazine(MyEntity weapon, int weaponId, MyDefinitionId id, bool forceReload) {
            return _setMagazine?.Invoke(weapon, weaponId, id, forceReload) ?? false;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="weapon"></param>
        /// <param name="weaponId"></param>
        /// <returns></returns>
        public bool ForceReload(MyEntity weapon, int weaponId) {
            return _forceReload?.Invoke(weapon, weaponId) ?? false;

        }

        private const long Channel = 67549756549;
        private bool _getWeaponDefinitions;
        private bool _isRegistered;
        private Action _readyCallback;

        /// <summary>
        /// True if CoreSystems replied when <see cref="Load"/> got called.
        /// </summary>
        public bool IsReady { get; private set; }

        /// <summary>
        /// Only filled if giving true to <see cref="Load"/>.
        /// </summary>
        public readonly List<WcApiDef.WeaponDefinition> WeaponDefinitions = new List<WcApiDef.WeaponDefinition>();

        /// <summary>
        /// Ask CoreSystems to send the API methods.
        /// <para>Throws an exception if it gets called more than once per session without <see cref="Unload"/>.</para>
        /// </summary>
        /// <param name="readyCallback">Method to be called when CoreSystems replies.</param>
        /// <param name="getWeaponDefinitions">Set to true to fill <see cref="WeaponDefinitions"/>.</param>
        public void Load(Action readyCallback = null, bool getWeaponDefinitions = false) {
            if (_isRegistered)
                throw new Exception($"{GetType().Name}.Load() should not be called multiple times!");

            _readyCallback = readyCallback;
            _getWeaponDefinitions = getWeaponDefinitions;
            _isRegistered = true;
            MyAPIGateway.Utilities.RegisterMessageHandler(Channel, HandleMessage);
            MyAPIGateway.Utilities.SendModMessage(Channel, "ApiEndpointRequest");
        }

        public void Unload() {
            MyAPIGateway.Utilities.UnregisterMessageHandler(Channel, HandleMessage);

            ApiAssign(null);

            _isRegistered = false;
            _apiInit = false;
            IsReady = false;
        }

        private void HandleMessage(object obj) {
            if (_apiInit || obj is string
            ) // the sent "ApiEndpointRequest" will also be received here, explicitly ignoring that
                return;

            var dict = obj as IReadOnlyDictionary<string, Delegate>;

            if (dict == null)
                return;

            ApiAssign(dict, _getWeaponDefinitions);

            IsReady = true;
            _readyCallback?.Invoke();
        }

        public void ApiAssign(IReadOnlyDictionary<string, Delegate> delegates, bool getWeaponDefinitions = false) {
            _apiInit = (delegates != null);
            /// base methods
            AssignMethod(delegates, "GetAllWeaponDefinitions", ref _getAllWeaponDefinitions);
            AssignMethod(delegates, "GetCoreWeapons", ref _getCoreWeapons);
            AssignMethod(delegates, "GetNpcSafeWeapons", ref _getNpcSafeWeapons);

            AssignMethod(delegates, "GetAllWeaponMagazines", ref _getAllWeaponMagazines);
            AssignMethod(delegates, "GetAllNpcSafeWeaponMagazines", ref _getAllNpcSafeWeaponMagazines);

            AssignMethod(delegates, "GetCoreStaticLaunchers", ref _getCoreStaticLaunchers);
            AssignMethod(delegates, "GetCoreTurrets", ref _getCoreTurrets);
            AssignMethod(delegates, "GetCorePhantoms", ref _getCorePhantoms);
            AssignMethod(delegates, "GetCoreRifles", ref _getCoreRifles);
            AssignMethod(delegates, "GetCoreArmors", ref _getCoreArmors);

            AssignMethod(delegates, "GetBlockWeaponMap", ref _getBlockWeaponMap);
            AssignMethod(delegates, "GetSortedThreatsBase", ref _getSortedThreats);
            AssignMethod(delegates, "GetObstructionsBase", ref _getObstructions);
            AssignMethod(delegates, "GetMaxPower", ref _getMaxPower);
            AssignMethod(delegates, "GetProjectilesLockedOnBase", ref _getProjectilesLockedOn);
            AssignMethod(delegates, "GetProjectilesLockedOnPos", ref _getProjectilesLockedOnPos);
            AssignMethod(delegates, "GetAiFocusBase", ref _getAiFocus);
            AssignMethod(delegates, "SetAiFocusBase", ref _setAiFocus);
            AssignMethod(delegates, "ReleaseAiFocusBase", ref _releaseAiFocus);
            AssignMethod(delegates, "HasGridAiBase", ref _hasAi);
            AssignMethod(delegates, "GetOptimalDpsBase", ref _getOptimalDps);
            AssignMethod(delegates, "GetConstructEffectiveDpsBase", ref _getConstructEffectiveDps);
            AssignMethod(delegates, "IsInRangeBase", ref _isInRange);
            AssignMethod(delegates, "GetProjectileState", ref _getProjectileState);
            AssignMethod(delegates, "SetProjectileState", ref _setProjectileState);

            AssignMethod(delegates, "AddMonitorProjectile", ref _addProjectileMonitor);
            AssignMethod(delegates, "RemoveMonitorProjectile", ref _removeProjectileMonitor);

            AssignMethod(delegates, "TargetFocusHandler", ref _targetFocusHandler);
            AssignMethod(delegates, "HudHandler", ref _hudHandler);
            AssignMethod(delegates, "ShootHandler", ref _shootHandler);
            AssignMethod(delegates, "ShootRequest", ref _shootRequest);

            /// block methods
            AssignMethod(delegates, "GetWeaponTargetBase", ref _getWeaponTarget);
            AssignMethod(delegates, "SetWeaponTargetBase", ref _setWeaponTarget);
            AssignMethod(delegates, "FireWeaponOnceBase", ref _fireWeaponOnce);
            AssignMethod(delegates, "ToggleWeaponFireBase", ref _toggleWeaponFire);
            AssignMethod(delegates, "IsWeaponReadyToFireBase", ref _isWeaponReadyToFire);
            AssignMethod(delegates, "GetMaxWeaponRangeBase", ref _getMaxWeaponRange);
            AssignMethod(delegates, "GetTurretTargetTypesBase", ref _getTurretTargetTypes);
            AssignMethod(delegates, "SetTurretTargetTypesBase", ref _setTurretTargetTypes);
            AssignMethod(delegates, "SetBlockTrackingRangeBase", ref _setBlockTrackingRange);
            AssignMethod(delegates, "IsTargetAlignedBase", ref _isTargetAligned);
            AssignMethod(delegates, "IsTargetAlignedExtendedBase", ref _isTargetAlignedExtended);
            AssignMethod(delegates, "CanShootTargetBase", ref _canShootTarget);
            AssignMethod(delegates, "GetPredictedTargetPositionBase", ref _getPredictedTargetPos);
            AssignMethod(delegates, "GetHeatLevelBase", ref _getHeatLevel);
            AssignMethod(delegates, "GetCurrentPowerBase", ref _currentPowerConsumption);
            AssignMethod(delegates, "DisableRequiredPowerBase", ref _disableRequiredPower);
            AssignMethod(delegates, "HasCoreWeaponBase", ref _hasCoreWeapon);
            AssignMethod(delegates, "GetActiveAmmoBase", ref _getActiveAmmo);
            AssignMethod(delegates, "SetActiveAmmoBase", ref _setActiveAmmo);
            AssignMethod(delegates, "GetPlayerControllerBase", ref _getPlayerController);
            AssignMethod(delegates, "GetWeaponAzimuthMatrixBase", ref _getWeaponAzimuthMatrix);
            AssignMethod(delegates, "GetWeaponElevationMatrixBase", ref _getWeaponElevationMatrix);
            AssignMethod(delegates, "IsTargetValidBase", ref _isTargetValid);
            AssignMethod(delegates, "GetWeaponScopeBase", ref _getWeaponScope);

            //Phantom methods
            AssignMethod(delegates, "GetTargetAssessment", ref _getTargetAssessment);
            //AssignMethod(delegates, "GetPhantomInfo", ref _getPhantomInfo);
            AssignMethod(delegates, "SetTriggerState", ref _setTriggerState);
            AssignMethod(delegates, "AddMagazines", ref _addMagazines);
            AssignMethod(delegates, "SetAmmo", ref _setAmmo);
            AssignMethod(delegates, "ClosePhantom", ref _closePhantom);
            AssignMethod(delegates, "SpawnPhantom", ref _spawnPhantom);
            AssignMethod(delegates, "SetFocusTarget", ref _setPhantomFocusTarget);

            //Hakerman's Beam Logic
            AssignMethod(delegates, "IsWeaponShootingBase", ref _isWeaponShooting);
            AssignMethod(delegates, "GetShotsFiredBase", ref _getShotsFired);
            AssignMethod(delegates, "GetMuzzleInfoBase", ref _getMuzzleInfo);
            AssignMethod(delegates, "ToggleInfiniteAmmoBase", ref _toggoleInfiniteResources);
            AssignMethod(delegates, "RegisterEventMonitor", ref _monitorEvents);
            AssignMethod(delegates, "UnRegisterEventMonitor", ref _unmonitorEvents);
            AssignMethod(delegates, "GetMagazineMap", ref _getMagazineMap);

            AssignMethod(delegates, "SetMagazine", ref _setMagazine);
            AssignMethod(delegates, "ForceReload", ref _forceReload);

            // Damage handler
            AssignMethod(delegates, "DamageHandler", ref _registerDamageEvent);

            if (getWeaponDefinitions) {
                var byteArrays = new List<byte[]>();
                GetAllWeaponDefinitions(byteArrays);
                foreach (var byteArray in byteArrays)
                    WeaponDefinitions.Add(MyAPIGateway.Utilities.SerializeFromBinary<WcApiDef.WeaponDefinition>(byteArray));
            }
        }

        private void AssignMethod<T>(IReadOnlyDictionary<string, Delegate> delegates, string name, ref T field)
            where T : class {
            if (delegates == null) {
                field = null;
                return;
            }

            Delegate del;
            if (!delegates.TryGetValue(name, out del))
                throw new Exception($"{GetType().Name} :: Couldn't find {name} delegate of type {typeof(T)}");

            field = del as T;

            if (field == null)
                throw new Exception(
                    $"{GetType().Name} :: Delegate {name} is not type {typeof(T)}, instead it's: {del.GetType()}");
        }

        public class DamageHandlerHelper {
            public void YourCallBackFunction(List<ProjectileDamageEvent> list) {
                // Your code goes here
                //
                // Once this function completes the data in the list will be deleted... if you need to use the data in this list
                // after this function completes make a copy of it.
                //
                // This is setup to be easy to use.  If you need more performance modify the Default Callback for your purposes and avoid
                // copying callbacks into new lists with ProjectileDamageEvent structs.  Note that the ListReader will remain usable for only 1 tick, then it will be cleared by wc.
                //
            }


            /// Don't touch anything below this line
            public void RegisterForDamage(long modId, EventType type) {
                _wcApi.RegisterDamageEvent(modId, (int)type, DefaultCallBack);
            }

            private void DefaultCallBack(ListReader<MyTuple<ulong, long, int, MyEntity, MyEntity, ListReader<MyTuple<Vector3D, object, float>>>> listReader) {
                YourCallBackFunction(ProcessEvents(listReader));
                CleanUpEvents();
            }

            private readonly List<ProjectileDamageEvent> _convertedObjects = new List<ProjectileDamageEvent>();
            private readonly Stack<List<ProjectileDamageEvent.ProHit>> _hitPool = new Stack<List<ProjectileDamageEvent.ProHit>>(256);

            private List<ProjectileDamageEvent> ProcessEvents(ListReader<MyTuple<ulong, long, int, MyEntity, MyEntity, ListReader<MyTuple<Vector3D, object, float>>>> projectiles) {
                foreach (var p in projectiles) {
                    var hits = _hitPool.Count > 0 ? _hitPool.Pop() : new List<ProjectileDamageEvent.ProHit>();

                    foreach (var hitObj in p.Item6) {
                        hits.Add(new ProjectileDamageEvent.ProHit { HitPosition = hitObj.Item1, ObjectHit = hitObj.Item2, Damage = hitObj.Item3 });
                    }
                    _convertedObjects.Add(new ProjectileDamageEvent { ProId = p.Item1, PlayerId = p.Item2, WeaponId = p.Item3, WeaponEntity = p.Item4, WeaponParent = p.Item5, ObjectsHit = hits });
                }

                return _convertedObjects;
            }

            private void CleanUpEvents() {
                foreach (var p in _convertedObjects) {
                    p.ObjectsHit.Clear();
                    _hitPool.Push(p.ObjectsHit);
                }
                _convertedObjects.Clear();
            }

            public struct ProjectileDamageEvent {
                public ulong ProId;
                public long PlayerId;
                public int WeaponId;
                public MyEntity WeaponEntity;
                public MyEntity WeaponParent;
                public List<ProHit> ObjectsHit;

                public struct ProHit {
                    public Vector3D HitPosition; // To == first hit, From = projectile start position this frame
                    public object ObjectHit; // block, player, etc... 
                    public float Damage;
                }
            }


            private readonly WcApi _wcApi;
            public DamageHandlerHelper(WcApi wcApi) {
                _wcApi = wcApi;
            }

            public enum EventType {
                Unregister,
                SystemWideDamageEvents,
            }
        }

    }

}