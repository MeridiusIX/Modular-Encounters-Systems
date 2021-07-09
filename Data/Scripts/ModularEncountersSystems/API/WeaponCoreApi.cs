using System;
using System.Collections.Generic;
using ProtoBuf;
using Sandbox.ModAPI;
using VRage;
using VRage.Game;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.API {

	/// <summary>
	/// https://github.com/sstixrud/WeaponCore/blob/master/Data/Scripts/WeaponCore/Api/WeaponCoreApi.cs
	/// </summary>
	/// 

	public class WcApi {
		private bool _apiInit;

		private Action<IList<byte[]>> _getAllWeaponDefinitions;
		private Action<ICollection<MyDefinitionId>> _getCoreWeapons;
		private Action<ICollection<MyDefinitionId>> _getCoreStaticLaunchers;
		private Action<ICollection<MyDefinitionId>> _getCoreTurrets;
		private Func<IMyTerminalBlock, IDictionary<string, int>, bool> _getBlockWeaponMap;
		private Func<IMyEntity, MyTuple<bool, int, int>> _getProjectilesLockedOn;
		private Action<IMyEntity, ICollection<MyTuple<IMyEntity, float>>> _getSortedThreats;
		private Func<IMyEntity, int, IMyEntity> _getAiFocus;
		private Func<IMyEntity, IMyEntity, int, bool> _setAiFocus;
		private Func<IMyTerminalBlock, int, MyTuple<bool, bool, bool, IMyEntity>> _getWeaponTarget;
		private Action<IMyTerminalBlock, IMyEntity, int> _setWeaponTarget;
		private Action<IMyTerminalBlock, bool, int> _fireWeaponOnce;
		private Action<IMyTerminalBlock, bool, bool, int> _toggleWeaponFire;
		private Func<IMyTerminalBlock, int, bool, bool, bool> _isWeaponReadyToFire;
		private Func<IMyTerminalBlock, int, float> _getMaxWeaponRange;
		private Func<IMyTerminalBlock, ICollection<string>, int, bool> _getTurretTargetTypes;
		private Action<IMyTerminalBlock, ICollection<string>, int> _setTurretTargetTypes;
		private Action<IMyTerminalBlock, float> _setBlockTrackingRange;
		private Func<IMyTerminalBlock, IMyEntity, int, bool> _isTargetAligned;
		private Func<IMyTerminalBlock, IMyEntity, int, MyTuple<bool, Vector3D?>> _isTargetAlignedExtended;
		private Func<IMyTerminalBlock, IMyEntity, int, bool> _canShootTarget;
		private Func<IMyTerminalBlock, IMyEntity, int, Vector3D?> _getPredictedTargetPos;
		private Func<IMyTerminalBlock, float> _getHeatLevel;
		private Func<IMyTerminalBlock, float> _currentPowerConsumption;
		private Func<MyDefinitionId, float> _getMaxPower;
		private Action<IMyTerminalBlock> _disableRequiredPower;
		private Func<IMyEntity, bool> _hasGridAi;
		private Func<IMyTerminalBlock, bool> _hasCoreWeapon;
		private Func<IMyEntity, float> _getOptimalDps;
		private Func<IMyTerminalBlock, int, string> _getActiveAmmo;
		private Action<IMyTerminalBlock, int, string> _setActiveAmmo;
		private Action<IMyTerminalBlock, int, Action<long, int, ulong, long, Vector3D, bool>> _monitorProjectile;
		private Action<IMyTerminalBlock, int, Action<long, int, ulong, long, Vector3D, bool>> _unMonitorProjectile;
		private Func<ulong, MyTuple<Vector3D, Vector3D, float, float, long, string>> _getProjectileState;
		private Func<IMyEntity, float> _getConstructEffectiveDps;
		private Func<IMyTerminalBlock, long> _getPlayerController;
		private Func<IMyTerminalBlock, int, Matrix> _getWeaponAzimuthMatrix;
		private Func<IMyTerminalBlock, int, Matrix> _getWeaponElevationMatrix;
		private Func<IMyTerminalBlock, IMyEntity, bool, bool, bool> _isTargetValid;
		private Func<IMyTerminalBlock, int, MyTuple<Vector3D, Vector3D>> _getWeaponScope;
		private Func<IMyEntity, MyTuple<bool, bool>> _isInRange;
		private const long Channel = 67549756549;
		private bool _getWeaponDefinitions;
		private bool _isRegistered;
		private Action _readyCallback;

		/// <summary>
		/// True if the WeaponCore replied when <see cref="Load"/> got called.
		/// </summary>
		public bool IsReady { get; private set; }

		/// <summary>
		/// Only filled if giving true to <see cref="Load"/>.
		/// </summary>
		public readonly List<WcApiDef.WeaponDefinition> WeaponDefinitions = new List<WcApiDef.WeaponDefinition>();

		/// <summary>
		/// Ask WeaponCore to send the API methods.
		/// <para>Throws an exception if it gets called more than once per session without <see cref="Unload"/>.</para>
		/// </summary>
		/// <param name="readyCallback">Method to be called when WeaponCore replies.</param>
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

			ApiAssign(null, false);

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
			_apiInit = delegates != null;

			AssignMethod(delegates, "GetAllWeaponDefinitions", ref _getAllWeaponDefinitions);
			AssignMethod(delegates, "GetCoreWeapons", ref _getCoreWeapons);
			AssignMethod(delegates, "GetCoreStaticLaunchers", ref _getCoreStaticLaunchers);
			AssignMethod(delegates, "GetCoreTurrets", ref _getCoreTurrets);
			AssignMethod(delegates, "GetBlockWeaponMap", ref _getBlockWeaponMap);
			AssignMethod(delegates, "GetProjectilesLockedOn", ref _getProjectilesLockedOn);
			AssignMethod(delegates, "GetSortedThreats", ref _getSortedThreats);
			AssignMethod(delegates, "GetAiFocus", ref _getAiFocus);
			AssignMethod(delegates, "SetAiFocus", ref _setAiFocus);
			AssignMethod(delegates, "GetWeaponTarget", ref _getWeaponTarget);
			AssignMethod(delegates, "SetWeaponTarget", ref _setWeaponTarget);
			AssignMethod(delegates, "FireWeaponOnce", ref _fireWeaponOnce);
			AssignMethod(delegates, "ToggleWeaponFire", ref _toggleWeaponFire);
			AssignMethod(delegates, "IsWeaponReadyToFire", ref _isWeaponReadyToFire);
			AssignMethod(delegates, "GetMaxWeaponRange", ref _getMaxWeaponRange);
			AssignMethod(delegates, "GetTurretTargetTypes", ref _getTurretTargetTypes);
			AssignMethod(delegates, "SetTurretTargetTypes", ref _setTurretTargetTypes);
			AssignMethod(delegates, "SetBlockTrackingRange", ref _setBlockTrackingRange);
			AssignMethod(delegates, "IsTargetAligned", ref _isTargetAligned);
			AssignMethod(delegates, "IsTargetAlignedExtended", ref _isTargetAlignedExtended);
			AssignMethod(delegates, "CanShootTarget", ref _canShootTarget);
			AssignMethod(delegates, "GetPredictedTargetPosition", ref _getPredictedTargetPos);
			AssignMethod(delegates, "GetHeatLevel", ref _getHeatLevel);
			AssignMethod(delegates, "GetCurrentPower", ref _currentPowerConsumption);
			AssignMethod(delegates, "GetMaxPower", ref _getMaxPower);
			AssignMethod(delegates, "DisableRequiredPower", ref _disableRequiredPower);
			AssignMethod(delegates, "HasGridAi", ref _hasGridAi);
			AssignMethod(delegates, "HasCoreWeapon", ref _hasCoreWeapon);
			AssignMethod(delegates, "GetOptimalDps", ref _getOptimalDps);
			AssignMethod(delegates, "GetActiveAmmo", ref _getActiveAmmo);
			AssignMethod(delegates, "SetActiveAmmo", ref _setActiveAmmo);
			AssignMethod(delegates, "MonitorProjectile", ref _monitorProjectile);
			AssignMethod(delegates, "UnMonitorProjectile", ref _unMonitorProjectile);
			AssignMethod(delegates, "GetProjectileState", ref _getProjectileState);
			AssignMethod(delegates, "GetConstructEffectiveDps", ref _getConstructEffectiveDps);
			AssignMethod(delegates, "GetPlayerController", ref _getPlayerController);
			AssignMethod(delegates, "GetWeaponAzimuthMatrix", ref _getWeaponAzimuthMatrix);
			AssignMethod(delegates, "GetWeaponElevationMatrix", ref _getWeaponElevationMatrix);
			AssignMethod(delegates, "IsTargetValid", ref _isTargetValid);
			AssignMethod(delegates, "GetWeaponScope", ref _getWeaponScope);
			AssignMethod(delegates, "IsInRange", ref _isInRange);

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

		public void GetAllWeaponDefinitions(IList<byte[]> collection) => _getAllWeaponDefinitions?.Invoke(collection);
		public void GetAllCoreWeapons(ICollection<MyDefinitionId> collection) => _getCoreWeapons?.Invoke(collection);

		public void GetAllCoreStaticLaunchers(ICollection<MyDefinitionId> collection) =>
			_getCoreStaticLaunchers?.Invoke(collection);

		public void GetAllCoreTurrets(ICollection<MyDefinitionId> collection) => _getCoreTurrets?.Invoke(collection);

		public bool GetBlockWeaponMap(IMyTerminalBlock weaponBlock, IDictionary<string, int> collection) =>
			_getBlockWeaponMap?.Invoke(weaponBlock, collection) ?? false;

		public MyTuple<bool, int, int> GetProjectilesLockedOn(IMyEntity victim) =>
			_getProjectilesLockedOn?.Invoke(victim) ?? new MyTuple<bool, int, int>();

		public void GetSortedThreats(IMyEntity shooter, ICollection<MyTuple<IMyEntity, float>> collection) =>
			_getSortedThreats?.Invoke(shooter, collection);

		public IMyEntity GetAiFocus(IMyEntity shooter, int priority = 0) => _getAiFocus?.Invoke(shooter, priority);

		public bool SetAiFocus(IMyEntity shooter, IMyEntity target, int priority = 0) =>
			_setAiFocus?.Invoke(shooter, target, priority) ?? false;

		public MyTuple<bool, bool, bool, IMyEntity> GetWeaponTarget(IMyTerminalBlock weapon, int weaponId = 0) =>
			_getWeaponTarget?.Invoke(weapon, weaponId) ?? new MyTuple<bool, bool, bool, IMyEntity>();

		public void SetWeaponTarget(IMyTerminalBlock weapon, IMyEntity target, int weaponId = 0) =>
			_setWeaponTarget?.Invoke(weapon, target, weaponId);

		public void FireWeaponOnce(IMyTerminalBlock weapon, bool allWeapons = true, int weaponId = 0) =>
			_fireWeaponOnce?.Invoke(weapon, allWeapons, weaponId);

		public void ToggleWeaponFire(IMyTerminalBlock weapon, bool on, bool allWeapons, int weaponId = 0) =>
			_toggleWeaponFire?.Invoke(weapon, on, allWeapons, weaponId);

		public bool IsWeaponReadyToFire(IMyTerminalBlock weapon, int weaponId = 0, bool anyWeaponReady = true,
			bool shootReady = false) =>
			_isWeaponReadyToFire?.Invoke(weapon, weaponId, anyWeaponReady, shootReady) ?? false;

		public float GetMaxWeaponRange(IMyTerminalBlock weapon, int weaponId) =>
			_getMaxWeaponRange?.Invoke(weapon, weaponId) ?? 0f;

		public bool GetTurretTargetTypes(IMyTerminalBlock weapon, IList<string> collection, int weaponId = 0) =>
			_getTurretTargetTypes?.Invoke(weapon, collection, weaponId) ?? false;

		public void SetTurretTargetTypes(IMyTerminalBlock weapon, IList<string> collection, int weaponId = 0) =>
			_setTurretTargetTypes?.Invoke(weapon, collection, weaponId);

		public void SetBlockTrackingRange(IMyTerminalBlock weapon, float range) =>
			_setBlockTrackingRange?.Invoke(weapon, range);

		public bool IsTargetAligned(IMyTerminalBlock weapon, IMyEntity targetEnt, int weaponId) =>
			_isTargetAligned?.Invoke(weapon, targetEnt, weaponId) ?? false;

		public MyTuple<bool, Vector3D?> IsTargetAlignedExtended(IMyTerminalBlock weapon, IMyEntity targetEnt, int weaponId) =>
			_isTargetAlignedExtended?.Invoke(weapon, targetEnt, weaponId) ?? new MyTuple<bool, Vector3D?>();

		public bool CanShootTarget(IMyTerminalBlock weapon, IMyEntity targetEnt, int weaponId) =>
			_canShootTarget?.Invoke(weapon, targetEnt, weaponId) ?? false;

		public Vector3D? GetPredictedTargetPosition(IMyTerminalBlock weapon, IMyEntity targetEnt, int weaponId) =>
			_getPredictedTargetPos?.Invoke(weapon, targetEnt, weaponId) ?? null;

		public float GetHeatLevel(IMyTerminalBlock weapon) => _getHeatLevel?.Invoke(weapon) ?? 0f;
		public float GetCurrentPower(IMyTerminalBlock weapon) => _currentPowerConsumption?.Invoke(weapon) ?? 0f;
		public float GetMaxPower(MyDefinitionId weaponDef) => _getMaxPower?.Invoke(weaponDef) ?? 0f;
		public void DisableRequiredPower(IMyTerminalBlock weapon) => _disableRequiredPower?.Invoke(weapon);
		public bool HasGridAi(IMyEntity entity) => _hasGridAi?.Invoke(entity) ?? false;
		public bool HasCoreWeapon(IMyTerminalBlock weapon) => _hasCoreWeapon?.Invoke(weapon) ?? false;
		public float GetOptimalDps(IMyEntity entity) => _getOptimalDps?.Invoke(entity) ?? 0f;

		public string GetActiveAmmo(IMyTerminalBlock weapon, int weaponId) =>
			_getActiveAmmo?.Invoke(weapon, weaponId) ?? null;

		public void SetActiveAmmo(IMyTerminalBlock weapon, int weaponId, string ammoType) =>
			_setActiveAmmo?.Invoke(weapon, weaponId, ammoType);

		public void MonitorProjectileCallback(IMyTerminalBlock weapon, int weaponId, Action<long, int, ulong, long, Vector3D, bool> action) =>
			_monitorProjectile?.Invoke(weapon, weaponId, action);

		public void UnMonitorProjectileCallback(IMyTerminalBlock weapon, int weaponId, Action<long, int, ulong, long, Vector3D, bool> action) =>
			_unMonitorProjectile?.Invoke(weapon, weaponId, action);

		public MyTuple<Vector3D, Vector3D, float, float, long, string> GetProjectileState(ulong projectileId) =>
			_getProjectileState?.Invoke(projectileId) ?? new MyTuple<Vector3D, Vector3D, float, float, long, string>();

		public float GetConstructEffectiveDps(IMyEntity entity) => _getConstructEffectiveDps?.Invoke(entity) ?? 0f;

		public long GetPlayerController(IMyTerminalBlock weapon) => _getPlayerController?.Invoke(weapon) ?? -1;

		public Matrix GetWeaponAzimuthMatrix(IMyTerminalBlock weapon, int weaponId) =>
			_getWeaponAzimuthMatrix?.Invoke(weapon, weaponId) ?? Matrix.Zero;

		public Matrix GetWeaponElevationMatrix(IMyTerminalBlock weapon, int weaponId) =>
			_getWeaponElevationMatrix?.Invoke(weapon, weaponId) ?? Matrix.Zero;

		public bool IsTargetValid(IMyTerminalBlock weapon, IMyEntity target, bool onlyThreats, bool checkRelations) =>
			_isTargetValid?.Invoke(weapon, target, onlyThreats, checkRelations) ?? false;

		public MyTuple<Vector3D, Vector3D> GetWeaponScope(IMyTerminalBlock weapon, int weaponId) =>
			_getWeaponScope?.Invoke(weapon, weaponId) ?? new MyTuple<Vector3D, Vector3D>();

		// block/grid, Threat, Other 
		public MyTuple<bool, bool> IsInRange(IMyEntity entity) =>
			_isInRange?.Invoke(entity) ?? new MyTuple<bool, bool>();
	}

	public static class WcApiDef {

		[ProtoContract]
		public struct WeaponDefinition {
			[ProtoMember(1)] internal ModelAssignmentsDef Assignments;
			[ProtoMember(2)] internal TargetingDef Targeting;
			[ProtoMember(3)] internal AnimationDef Animations;
			[ProtoMember(4)] internal HardPointDef HardPoint;
			[ProtoMember(5)] internal AmmoDef[] Ammos;
			[ProtoMember(6)] internal string ModPath;
			[ProtoMember(7)] internal Dictionary<string, UpgradeValues[]> Upgrades;

			[ProtoContract]
			public struct ModelAssignmentsDef {
				[ProtoMember(1)] internal MountPointDef[] MountPoints;
				[ProtoMember(2)] internal string[] Barrels;
				[ProtoMember(3)] internal string Ejector;
				[ProtoMember(4)] internal string Scope;

				[ProtoContract]
				public struct MountPointDef {
					[ProtoMember(1)] internal string SubtypeId;
					[ProtoMember(2)] internal string AimPartId;
					[ProtoMember(3)] internal string MuzzlePartId;
					[ProtoMember(4)] internal string AzimuthPartId;
					[ProtoMember(5)] internal string ElevationPartId;
					[ProtoMember(6)] internal float DurabilityMod;
					[ProtoMember(7)] internal string IconName;
				}
			}

			[ProtoContract]
			public struct TargetingDef {
				public enum Threat {
					Projectiles,
					Characters,
					Grids,
					Neutrals,
					Meteors,
					Other
				}

				public enum BlockTypes {
					Any,
					Offense,
					Utility,
					Power,
					Production,
					Thrust,
					Jumping,
					Steering
				}

				[ProtoMember(1)] internal int TopTargets;
				[ProtoMember(2)] internal int TopBlocks;
				[ProtoMember(3)] internal double StopTrackingSpeed;
				[ProtoMember(4)] internal float MinimumDiameter;
				[ProtoMember(5)] internal float MaximumDiameter;
				[ProtoMember(6)] internal bool ClosestFirst;
				[ProtoMember(7)] internal BlockTypes[] SubSystems;
				[ProtoMember(8)] internal Threat[] Threats;
				[ProtoMember(9)] internal float MaxTargetDistance;
				[ProtoMember(10)] internal float MinTargetDistance;
				[ProtoMember(11)] internal bool IgnoreDumbProjectiles;
				[ProtoMember(12)] internal bool LockedSmartOnly;
			}


			[ProtoContract]
			public struct AnimationDef {
				[ProtoMember(1)] internal PartAnimationSetDef[] WeaponAnimationSets;
				[ProtoMember(2)] internal WeaponEmissive[] Emissives;
				[ProtoMember(3)] internal string[] HeatingEmissiveParts;
				[ProtoMember(4)] internal Dictionary<PartAnimationSetDef.EventTriggers, EventParticle[]> EventParticles;

				[ProtoContract(IgnoreListHandling = true)]
				public struct PartAnimationSetDef {
					public enum EventTriggers {
						Reloading,
						Firing,
						Tracking,
						Overheated,
						TurnOn,
						TurnOff,
						BurstReload,
						OutOfAmmo,
						PreFire,
						EmptyOnGameLoad,
						StopFiring,
						StopTracking
					}


					[ProtoMember(1)] internal string[] SubpartId;
					[ProtoMember(2)] internal string BarrelId;
					[ProtoMember(3)] internal uint StartupFireDelay;
					[ProtoMember(4)] internal Dictionary<EventTriggers, uint> AnimationDelays;
					[ProtoMember(5)] internal EventTriggers[] Reverse;
					[ProtoMember(6)] internal EventTriggers[] Loop;
					[ProtoMember(7)] internal Dictionary<EventTriggers, RelMove[]> EventMoveSets;
					[ProtoMember(8)] internal EventTriggers[] TriggerOnce;
					[ProtoMember(9)] internal EventTriggers[] ResetEmissives;

				}

				[ProtoContract]
				public struct WeaponEmissive {
					[ProtoMember(1)] internal string EmissiveName;
					[ProtoMember(2)] internal string[] EmissivePartNames;
					[ProtoMember(3)] internal bool CycleEmissivesParts;
					[ProtoMember(4)] internal bool LeavePreviousOn;
					[ProtoMember(5)] internal Vector4[] Colors;
					[ProtoMember(6)] internal float[] IntensityRange;
				}

				[ProtoContract]
				public struct EventParticle {
					[ProtoMember(1)] internal string[] EmptyNames;
					[ProtoMember(2)] internal string[] MuzzleNames;
					[ProtoMember(3)] internal ParticleDef Particle;
					[ProtoMember(4)] internal uint StartDelay;
					[ProtoMember(5)] internal uint LoopDelay;
					[ProtoMember(6)] internal bool ForceStop;
				}

				[ProtoContract]
				internal struct RelMove {
					public enum MoveType {
						Linear,
						ExpoDecay,
						ExpoGrowth,
						Delay,
						Show, //instant or fade
						Hide, //instant or fade
					}

					[ProtoMember(1)] internal MoveType MovementType;
					[ProtoMember(2)] internal XYZ[] LinearPoints;
					[ProtoMember(3)] internal XYZ Rotation;
					[ProtoMember(4)] internal XYZ RotAroundCenter;
					[ProtoMember(5)] internal uint TicksToMove;
					[ProtoMember(6)] internal string CenterEmpty;
					[ProtoMember(7)] internal bool Fade;
					[ProtoMember(8)] internal string EmissiveName;

					[ProtoContract]
					internal struct XYZ {
						[ProtoMember(1)] internal double x;
						[ProtoMember(2)] internal double y;
						[ProtoMember(3)] internal double z;
					}
				}
			}

			[ProtoContract]
			public struct UpgradeValues {
				[ProtoMember(1)] internal string[] Ammo;
				[ProtoMember(2)] internal int RateOfFireMod;
				[ProtoMember(3)] internal int BarrelsPerShotMod;
				[ProtoMember(4)] internal int ReloadMod;
				[ProtoMember(5)] internal int MaxHeatMod;
				[ProtoMember(6)] internal int HeatSinkRateMod;
				[ProtoMember(7)] internal int ShotsInBurstMod;
				[ProtoMember(8)] internal int DelayAfterBurstMod;
				[ProtoMember(9)] internal int AmmoPriority;
			}

			[ProtoContract]
			public struct HardPointDef {
				public enum Prediction {
					Off,
					Basic,
					Accurate,
					Advanced,
				}

				[ProtoMember(1)] internal string WeaponName;
				[ProtoMember(2)] internal int DelayCeaseFire;
				[ProtoMember(3)] internal float DeviateShotAngle;
				[ProtoMember(4)] internal double AimingTolerance;
				[ProtoMember(5)] internal Prediction AimLeadingPrediction;
				[ProtoMember(6)] internal LoadingDef Loading;
				[ProtoMember(7)] internal AiDef Ai;
				[ProtoMember(8)] internal HardwareDef HardWare;
				[ProtoMember(9)] internal UiDef Ui;
				[ProtoMember(10)] internal HardPointAudioDef Audio;
				[ProtoMember(11)] internal HardPointParticleDef Graphics;
				[ProtoMember(12)] internal OtherDef Other;
				[ProtoMember(13)] internal bool AddToleranceToTracking;
				[ProtoMember(14)] internal bool CanShootSubmerged;

				[ProtoContract]
				public struct UpgradeValues {
					[ProtoMember(1)] internal string[] Ammo;
					[ProtoMember(2)] internal int RateOfFireMod;
					[ProtoMember(3)] internal int BarrelsPerShotMod;
					[ProtoMember(4)] internal int ReloadMod;
					[ProtoMember(5)] internal int MaxHeatMod;
					[ProtoMember(6)] internal int HeatSinkRateMod;
					[ProtoMember(7)] internal int ShotsInBurstMod;
					[ProtoMember(8)] internal int DelayAfterBurstMod;
					[ProtoMember(9)] internal int AmmoPriority;
				}

				[ProtoContract]
				public struct LoadingDef {
					[ProtoMember(1)] internal int ReloadTime;
					[ProtoMember(2)] internal int RateOfFire;
					[ProtoMember(3)] internal int BarrelsPerShot;
					[ProtoMember(4)] internal int SkipBarrels;
					[ProtoMember(5)] internal int TrajectilesPerBarrel;
					[ProtoMember(6)] internal int HeatPerShot;
					[ProtoMember(7)] internal int MaxHeat;
					[ProtoMember(8)] internal int HeatSinkRate;
					[ProtoMember(9)] internal float Cooldown;
					[ProtoMember(10)] internal int DelayUntilFire;
					[ProtoMember(11)] internal int ShotsInBurst;
					[ProtoMember(12)] internal int DelayAfterBurst;
					[ProtoMember(13)] internal bool DegradeRof;
					[ProtoMember(14)] internal int BarrelSpinRate;
					[ProtoMember(15)] internal bool FireFullBurst;
					[ProtoMember(16)] internal bool GiveUpAfterBurst;
					[ProtoMember(17)] internal bool DeterministicSpin;
				}


				[ProtoContract]
				public struct UiDef {
					[ProtoMember(1)] internal bool RateOfFire;
					[ProtoMember(2)] internal bool DamageModifier;
					[ProtoMember(3)] internal bool ToggleGuidance;
					[ProtoMember(4)] internal bool EnableOverload;
				}


				[ProtoContract]
				public struct AiDef {
					[ProtoMember(1)] internal bool TrackTargets;
					[ProtoMember(2)] internal bool TurretAttached;
					[ProtoMember(3)] internal bool TurretController;
					[ProtoMember(4)] internal bool PrimaryTracking;
					[ProtoMember(5)] internal bool LockOnFocus;
					[ProtoMember(6)] internal bool SuppressFire;
					[ProtoMember(7)] internal bool OverrideLeads;
				}

				[ProtoContract]
				public struct HardwareDef {
					public enum ArmorState {
						IsWeapon,
						Passive,
						Active,
					}

					[ProtoMember(1)] internal float RotateRate;
					[ProtoMember(2)] internal float ElevateRate;
					[ProtoMember(3)] internal Vector3D Offset;
					[ProtoMember(4)] internal bool FixedOffset;
					[ProtoMember(5)] internal int MaxAzimuth;
					[ProtoMember(6)] internal int MinAzimuth;
					[ProtoMember(7)] internal int MaxElevation;
					[ProtoMember(8)] internal int MinElevation;
					[ProtoMember(9)] internal float InventorySize;
					[ProtoMember(10)] internal ArmorState Armor;
				}

				[ProtoContract]
				public struct HardPointAudioDef {
					[ProtoMember(1)] internal string ReloadSound;
					[ProtoMember(2)] internal string NoAmmoSound;
					[ProtoMember(3)] internal string HardPointRotationSound;
					[ProtoMember(4)] internal string BarrelRotationSound;
					[ProtoMember(5)] internal string FiringSound;
					[ProtoMember(6)] internal bool FiringSoundPerShot;
					[ProtoMember(7)] internal string PreFiringSound;
					[ProtoMember(8)] internal uint FireSoundEndDelay;
				}

				[ProtoContract]
				public struct OtherDef {
					[ProtoMember(1)] internal int GridWeaponCap;
					[ProtoMember(2)] internal int EnergyPriority;
					[ProtoMember(3)] internal int RotateBarrelAxis;
					[ProtoMember(4)] internal bool MuzzleCheck;
					[ProtoMember(5)] internal bool Debug;
					[ProtoMember(6)] internal double RestrictionRadius;
					[ProtoMember(7)] internal bool CheckInflatedBox;
					[ProtoMember(8)] internal bool CheckForAnyWeapon;
				}

				[ProtoContract]
				public struct HardPointParticleDef {
					[ProtoMember(1)] internal ParticleDef Barrel1;
					[ProtoMember(2)] internal ParticleDef Barrel2;
				}
			}

			[ProtoContract]
			public class AmmoDef {
				[ProtoMember(1)] internal string AmmoMagazine;
				[ProtoMember(2)] internal string AmmoRound;
				[ProtoMember(3)] internal bool HybridRound;
				[ProtoMember(4)] internal float EnergyCost;
				[ProtoMember(5)] internal float BaseDamage;
				[ProtoMember(6)] internal float Mass;
				[ProtoMember(7)] internal float Health;
				[ProtoMember(8)] internal float BackKickForce;
				[ProtoMember(9)] internal DamageScaleDef DamageScales;
				[ProtoMember(10)] internal ShapeDef Shape;
				[ProtoMember(11)] internal ObjectsHitDef ObjectsHit;
				[ProtoMember(12)] internal TrajectoryDef Trajectory;
				[ProtoMember(13)] internal AreaDamageDef AreaEffect;
				[ProtoMember(14)] internal BeamDef Beams;
				[ProtoMember(15)] internal ShrapnelDef Shrapnel;
				[ProtoMember(16)] internal GraphicDef AmmoGraphics;
				[ProtoMember(17)] internal AmmoAudioDef AmmoAudio;
				[ProtoMember(18)] internal bool HardPointUsable;
				[ProtoMember(19)] internal AmmoPatternDef Pattern;
				[ProtoMember(20)] internal int EnergyMagazineSize;
				[ProtoMember(21)] internal float DecayPerShot;
				[ProtoMember(22)] internal AmmoEjectionDef Ejection;
				[ProtoMember(23)] internal bool IgnoreWater;

				[ProtoContract]
				public struct DamageScaleDef {

					[ProtoMember(1)] internal float MaxIntegrity;
					[ProtoMember(2)] internal bool DamageVoxels;
					[ProtoMember(3)] internal float Characters;
					[ProtoMember(4)] internal bool SelfDamage;
					[ProtoMember(5)] internal GridSizeDef Grids;
					[ProtoMember(6)] internal ArmorDef Armor;
					[ProtoMember(7)] internal CustomScalesDef Custom;
					[ProtoMember(8)] internal ShieldDef Shields;
					[ProtoMember(9)] internal FallOffDef FallOff;
					[ProtoMember(10)] internal double HealthHitModifier;
					[ProtoMember(11)] internal double VoxelHitModifier;

					[ProtoContract]
					public struct FallOffDef {
						[ProtoMember(1)] internal float Distance;
						[ProtoMember(2)] internal float MinMultipler;
					}

					[ProtoContract]
					public struct GridSizeDef {
						[ProtoMember(1)] internal float Large;
						[ProtoMember(2)] internal float Small;
					}

					[ProtoContract]
					public struct ArmorDef {
						[ProtoMember(1)] internal float Armor;
						[ProtoMember(2)] internal float Heavy;
						[ProtoMember(3)] internal float Light;
						[ProtoMember(4)] internal float NonArmor;
					}

					[ProtoContract]
					public struct CustomScalesDef {
						[ProtoMember(1)] internal CustomBlocksDef[] Types;
						[ProtoMember(2)] internal bool IgnoreAllOthers;
					}

					[ProtoContract]
					public struct ShieldDef {
						internal enum ShieldType {
							Heal,
							Bypass,
							Emp,
							Energy,
							Kinetic
						}

						[ProtoMember(1)] internal float Modifier;
						[ProtoMember(2)] internal ShieldType Type;
						[ProtoMember(3)] internal float BypassModifier;
					}
				}

				[ProtoContract]
				public struct ShapeDef {
					public enum Shapes {
						LineShape,
						SphereShape,
					}

					[ProtoMember(1)] internal Shapes Shape;
					[ProtoMember(2)] internal double Diameter;
				}

				[ProtoContract]
				public struct ObjectsHitDef {
					[ProtoMember(1)] internal int MaxObjectsHit;
					[ProtoMember(2)] internal bool CountBlocks;
				}


				[ProtoContract]
				public struct CustomBlocksDef {
					[ProtoMember(1)] internal string SubTypeId;
					[ProtoMember(2)] internal float Modifier;
				}

				[ProtoContract]
				public struct GraphicDef {
					[ProtoMember(1)] internal bool ShieldHitDraw;
					[ProtoMember(2)] internal float VisualProbability;
					[ProtoMember(3)] internal string ModelName;
					[ProtoMember(4)] internal AmmoParticleDef Particles;
					[ProtoMember(5)] internal LineDef Lines;

					[ProtoContract]
					public struct AmmoParticleDef {
						[ProtoMember(1)] internal ParticleDef Ammo;
						[ProtoMember(2)] internal ParticleDef Hit;
						[ProtoMember(3)] internal ParticleDef Eject;
					}

					[ProtoContract]
					public struct LineDef {
						internal enum Texture {
							Normal,
							Cycle,
							Chaos,
							Wave,
						}

						[ProtoMember(1)] internal TracerBaseDef Tracer;
						[ProtoMember(2)] internal string TracerMaterial;
						[ProtoMember(3)] internal Randomize ColorVariance;
						[ProtoMember(4)] internal Randomize WidthVariance;
						[ProtoMember(5)] internal TrailDef Trail;
						[ProtoMember(6)] internal OffsetEffectDef OffsetEffect;

						[ProtoContract]
						public struct OffsetEffectDef {
							[ProtoMember(1)] internal double MaxOffset;
							[ProtoMember(2)] internal double MinLength;
							[ProtoMember(3)] internal double MaxLength;
						}

						[ProtoContract]
						public struct TracerBaseDef {
							[ProtoMember(1)] internal bool Enable;
							[ProtoMember(2)] internal float Length;
							[ProtoMember(3)] internal float Width;
							[ProtoMember(4)] internal Vector4 Color;
							[ProtoMember(5)] internal uint VisualFadeStart;
							[ProtoMember(6)] internal uint VisualFadeEnd;
							[ProtoMember(7)] internal SegmentDef Segmentation;
							[ProtoMember(8)] internal string[] Textures;
							[ProtoMember(9)] internal Texture TextureMode;

							[ProtoContract]
							public struct SegmentDef {
								[ProtoMember(1)] internal string Material; //retired
								[ProtoMember(2)] internal double SegmentLength;
								[ProtoMember(3)] internal double SegmentGap;
								[ProtoMember(4)] internal double Speed;
								[ProtoMember(5)] internal Vector4 Color;
								[ProtoMember(6)] internal double WidthMultiplier;
								[ProtoMember(7)] internal bool Reverse;
								[ProtoMember(8)] internal bool UseLineVariance;
								[ProtoMember(9)] internal Randomize ColorVariance;
								[ProtoMember(10)] internal Randomize WidthVariance;
								[ProtoMember(11)] internal string[] Textures;
								[ProtoMember(12)] internal bool Enable;
							}
						}

						[ProtoContract]
						public struct TrailDef {
							[ProtoMember(1)] internal bool Enable;
							[ProtoMember(2)] internal string Material;
							[ProtoMember(3)] internal int DecayTime;
							[ProtoMember(4)] internal Vector4 Color;
							[ProtoMember(5)] internal bool Back;
							[ProtoMember(6)] internal float CustomWidth;
							[ProtoMember(7)] internal bool UseWidthVariance;
							[ProtoMember(8)] internal bool UseColorFade;
							[ProtoMember(9)] internal string[] Textures;
							[ProtoMember(10)] internal Texture TextureMode;

						}
					}
				}

				[ProtoContract]
				public struct BeamDef {
					[ProtoMember(1)] internal bool Enable;
					[ProtoMember(2)] internal bool ConvergeBeams;
					[ProtoMember(3)] internal bool VirtualBeams;
					[ProtoMember(4)] internal bool RotateRealBeam;
					[ProtoMember(5)] internal bool OneParticle;
				}

				[ProtoContract]
				public struct ShrapnelDef {
					[ProtoMember(1)] internal string AmmoRound;
					[ProtoMember(2)] internal int Fragments;
					[ProtoMember(3)] internal float ForwardDegrees;
					[ProtoMember(4)] internal float BackwardDegrees;
					[ProtoMember(5)] internal float Degrees;
					[ProtoMember(6)] internal bool Reverse;
					[ProtoMember(7)] internal bool RandomizeDir;
				}

				[ProtoContract]
				public struct AmmoPatternDef {
					[ProtoMember(1)] internal string[] Ammos;
					[ProtoMember(2)] internal bool Enable;
					[ProtoMember(3)] internal float TriggerChance;
					[ProtoMember(4)] internal bool SkipParent;
					[ProtoMember(5)] internal bool Random;
					[ProtoMember(6)] internal int RandomMin;
					[ProtoMember(7)] internal int RandomMax;
					[ProtoMember(8)] internal int PatternSteps;
				}

				[ProtoContract]
				public struct AmmoEjectionDef {
					public enum SpawnType {
						Item,
						Particle,
					}
					[ProtoMember(1)] internal float Speed;
					[ProtoMember(2)] internal float SpawnChance;
					[ProtoMember(3)] internal SpawnType Type;
					[ProtoMember(4)] internal ComponentDef CompDef;

					[ProtoContract]
					public struct ComponentDef {
						[ProtoMember(1)] internal string ItemName;
						[ProtoMember(2)] internal int ItemLifeTime;
						[ProtoMember(3)] internal int Delay;
					}
				}

				[ProtoContract]
				public struct AreaDamageDef {
					public enum AreaEffectType {
						Disabled,
						Explosive,
						Radiant,
						AntiSmart,
						JumpNullField,
						EnergySinkField,
						AnchorField,
						EmpField,
						OffenseField,
						NavField,
						DotField,
						PushField,
						PullField,
					}

					[ProtoMember(1)] internal double AreaEffectRadius;
					[ProtoMember(2)] internal float AreaEffectDamage;
					[ProtoMember(3)] internal AreaEffectType AreaEffect;
					[ProtoMember(4)] internal PulseDef Pulse;
					[ProtoMember(5)] internal DetonateDef Detonation;
					[ProtoMember(6)] internal ExplosionDef Explosions;
					[ProtoMember(7)] internal EwarFieldsDef EwarFields;
					[ProtoMember(8)] internal AreaInfluence Base;

					[ProtoContract]
					public struct AreaInfluence {
						[ProtoMember(1)] internal double Radius;
						[ProtoMember(2)] internal float EffectStrength;
					}


					[ProtoContract]
					public struct PulseDef {
						[ProtoMember(1)] internal int Interval;
						[ProtoMember(2)] internal int PulseChance;
						[ProtoMember(3)] internal int GrowTime;
						[ProtoMember(4)] internal bool HideModel;
						[ProtoMember(5)] internal bool ShowParticle;
						[ProtoMember(6)] internal ParticleDef Particle;
					}

					[ProtoContract]
					public struct EwarFieldsDef {
						[ProtoMember(1)] internal int Duration;
						[ProtoMember(2)] internal bool StackDuration;
						[ProtoMember(3)] internal bool Depletable;
						[ProtoMember(4)] internal double TriggerRange;
						[ProtoMember(5)] internal int MaxStacks;
						[ProtoMember(6)] internal PushPullDef Force;
						[ProtoMember(7)] internal bool DisableParticleEffect;

						[ProtoContract]
						public struct PushPullDef {
							public enum Force {
								ProjectileLastPosition,
								ProjectileOrigin,
								HitPosition,
								TargetCenter,
								TargetCenterOfMass,
							}

							[ProtoMember(1)] internal Force ForceFrom;
							[ProtoMember(2)] internal Force ForceTo;
							[ProtoMember(3)] internal Force Position;
						}
					}

					[ProtoContract]
					public struct DetonateDef {
						[ProtoMember(1)] internal bool DetonateOnEnd;
						[ProtoMember(2)] internal bool ArmOnlyOnHit;
						[ProtoMember(3)] internal float DetonationRadius;
						[ProtoMember(4)] internal float DetonationDamage;
						[ProtoMember(5)] internal int MinArmingTime;
					}

					[ProtoContract]
					public struct ExplosionDef {
						[ProtoMember(1)] internal bool NoVisuals;
						[ProtoMember(2)] internal bool NoSound;
						[ProtoMember(3)] internal float Scale;
						[ProtoMember(4)] internal string CustomParticle;
						[ProtoMember(5)] internal string CustomSound;
						[ProtoMember(6)] internal bool NoShrapnel;
						[ProtoMember(7)] internal bool NoDeformation;
					}
				}

				[ProtoContract]
				public struct AmmoAudioDef {
					[ProtoMember(1)] internal string TravelSound;
					[ProtoMember(2)] internal string HitSound;
					[ProtoMember(3)] internal float HitPlayChance;
					[ProtoMember(4)] internal bool HitPlayShield;
					[ProtoMember(5)] internal string VoxelHitSound;
					[ProtoMember(6)] internal string PlayerHitSound;
					[ProtoMember(7)] internal string FloatingHitSound;
					[ProtoMember(8)] internal string ShieldHitSound;
				}

				[ProtoContract]
				public struct TrajectoryDef {
					internal enum GuidanceType {
						None,
						Remote,
						TravelTo,
						Smart,
						DetectTravelTo,
						DetectSmart,
						DetectFixed,
					}

					[ProtoMember(1)] internal float MaxTrajectory;
					[ProtoMember(2)] internal float AccelPerSec;
					[ProtoMember(3)] internal float DesiredSpeed;
					[ProtoMember(4)] internal float TargetLossDegree;
					[ProtoMember(5)] internal int TargetLossTime;
					[ProtoMember(6)] internal int MaxLifeTime;
					[ProtoMember(7)] internal int FieldTime;
					[ProtoMember(8)] internal Randomize SpeedVariance;
					[ProtoMember(9)] internal Randomize RangeVariance;
					[ProtoMember(10)] internal GuidanceType Guidance;
					[ProtoMember(11)] internal SmartsDef Smarts;
					[ProtoMember(12)] internal MinesDef Mines;
					[ProtoMember(13)] internal float GravityMultiplier;
					[ProtoMember(14)] internal uint MaxTrajectoryTime;

					[ProtoContract]
					public struct SmartsDef {
						[ProtoMember(1)] internal double Inaccuracy;
						[ProtoMember(2)] internal double Aggressiveness;
						[ProtoMember(3)] internal double MaxLateralThrust;
						[ProtoMember(4)] internal double TrackingDelay;
						[ProtoMember(5)] internal int MaxChaseTime;
						[ProtoMember(6)] internal bool OverideTarget;
						[ProtoMember(7)] internal int MaxTargets;
						[ProtoMember(8)] internal bool NoTargetExpire;
						[ProtoMember(9)] internal bool Roam;
						[ProtoMember(10)] internal bool KeepAliveAfterTargetLoss;
						[ProtoMember(11)] internal float OffsetRatio;
						[ProtoMember(12)] internal int OffsetTime;
					}

					[ProtoContract]
					public struct MinesDef {
						[ProtoMember(1)] internal double DetectRadius;
						[ProtoMember(2)] internal double DeCloakRadius;
						[ProtoMember(3)] internal int FieldTime;
						[ProtoMember(4)] internal bool Cloak;
						[ProtoMember(5)] internal bool Persist;
					}
				}

				[ProtoContract]
				public struct Randomize {
					[ProtoMember(1)] internal float Start;
					[ProtoMember(2)] internal float End;
				}
			}

			[ProtoContract]
			public struct ParticleOptionDef {
				[ProtoMember(1)] internal float Scale;
				[ProtoMember(2)] internal float MaxDistance;
				[ProtoMember(3)] internal float MaxDuration;
				[ProtoMember(4)] internal bool Loop;
				[ProtoMember(5)] internal bool Restart;
				[ProtoMember(6)] internal float HitPlayChance;
			}


			[ProtoContract]
			public struct ParticleDef {
				[ProtoMember(1)] internal string Name;
				[ProtoMember(2)] internal Vector4 Color;
				[ProtoMember(3)] internal Vector3D Offset;
				[ProtoMember(4)] internal ParticleOptionDef Extras;
				[ProtoMember(5)] internal bool ApplyToShield;
				[ProtoMember(6)] internal bool ShrinkByDistance;
			}
		}
	}
}