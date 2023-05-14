using ModularEncountersSystems.API;
using ModularEncountersSystems.Behavior;
using ModularEncountersSystems.Behavior.Subsystems;
using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Behavior.Subsystems.Trigger;
using ModularEncountersSystems.BlockLogic;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.Watchers;
using ModularEncountersSystems.World;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Behavior {

	[Flags]
	public enum BehaviorProcessState {
	
		None = 0,
		Start = 1 << 0,
		CollisionParallel = 1 << 1,
		TargetingParallel = 1 << 2,
		AutopilotParallel = 1 << 3,
		WeaponsParallel = 1 << 4,
		TriggersParallel = 1 << 5,
		AutopilotMain = 1 << 6,
		WeaponsMain = 1 << 7,
		TriggersMain = 1 << 8,
		DespawnMain = 1 << 9,
		BehaviorMain = 1 << 10,
		Complete = 1 << 11,
	
	}

	public class CoreBehavior : IBehavior {

		public IMyRemoteControl RemoteControl { get { return _remoteControl; } set { _remoteControl = value; } }
		public IMyCubeGrid CubeGrid;

		public List<string> RemoteControlCode;

		//public BaseSystems Systems;

		private bool _behaviorTerminated;
		private BehaviorProcessState _previousProcessingState;
		private BehaviorProcessState _currentProcessingState;

		private bool _registeredRemoteCode;
		private bool _despawnTriggersRegistered;

		private AutoPilotSystem _newAutoPilot;
		private BroadcastSystem _broadcast;
		private DamageSystem _damage;
		private DespawnSystem _despawn;
		private DiagnosticSystem _diagnostic;
		private EscortSystem _escortSystem;
		private GridSystem _extras;
		private HeartbeatSystem _heartbeat;
		private OwnerSystem _owner;
		private StoredSettings _settings;
		private TriggerSystem _trigger;

		//Behavior Subclasses
		public IBehaviorSubClass ActiveBehavior { get { return _activeBehavior; } set { _activeBehavior = value; } }
		private IBehaviorSubClass _activeBehavior;
		private CargoShip _cargoship;
		private Escort _escort;
		private Fighter _fighter;
		private Horsefly _horsefly;
		private HorseFighter _horsefighter;
		private Hunter _hunter;
		private Nautical _nautical;
		private Passive _passive;
		private Patrol _patrol;
		private Scout _scout;
		private Sniper _sniper;
		private Strike _strike;
		private Vulture _vulture;

		private bool _behaviorTriggerA;
		private bool _behaviorTriggerB;
		private bool _behaviorTriggerC;
		private bool _behaviorTriggerD;
		private bool _behaviorTriggerE;
		private bool _behaviorTriggerF;
		private bool _behaviorTriggerG;
		private bool _behaviorTriggerH;

		private bool _behaviorActionA;
		private bool _behaviorActionB;
		private bool _behaviorActionC;
		private bool _behaviorActionD;
		private bool _behaviorActionE;
		private bool _behaviorActionF;
		private bool _behaviorActionG;
		private bool _behaviorActionH;

		private JetpackInhibitor _jetpackInhibitorLogic;
		private DrillInhibitor _drillInhibitorLogic;
		private NanobotInhibitor _nanobotInhibitorLogic;
		private JumpDriveInhibitor _jumpInhibitorLogic;
		private PlayerInhibitor _playerInhibitorLogic;

		private List<IMyCubeGrid> _currentGrids;
		private List<IMyCockpit> _debugCockpits;

		private IMyRemoteControl _remoteControl;

		internal string _debugString;

		public AutoPilotSystem AutoPilot { get { return _newAutoPilot; } set { _newAutoPilot = value; } }
		public BroadcastSystem Broadcast { get { return _broadcast; } set { _broadcast = value; } }
		public DamageSystem Damage { get { return _damage; } set { _damage = value; } }
		public DespawnSystem Despawn { get { return _despawn; } set { _despawn = value; } }
		public DiagnosticSystem Diagnostic { get { return _diagnostic; } set { _diagnostic = value; } }
		public EscortSystem Escort { get { return _escortSystem; } set { _escortSystem = value; } }
		public GridSystem Grid { get { return _extras; } set { _extras = value; } }
		public HeartbeatSystem Heartbeat { get { return _heartbeat; } set { _heartbeat = value; } }
		public OwnerSystem Owner { get { return _owner; } set { _owner = value; } }
		public StoredSettings BehaviorSettings { get { return _settings; } set { _settings = value; } }
		public TriggerSystem Trigger { get { return _trigger; } set { _trigger = value; } }

		public BehaviorMode Mode { 
			
			get {
				
				if(this.BehaviorSettings != null)
					return this.BehaviorSettings.Mode;

				return BehaviorMode.Init;
			
			}
			
			set {

				if (this.BehaviorSettings != null)
					this.BehaviorSettings.Mode = value;

			}
		
		}

		public bool BehaviorTerminated { get { return _behaviorTerminated; } set { _behaviorTerminated = value; } }
		public bool BehaviorTriggerA { get { return _behaviorTriggerA; } set { _behaviorTriggerA = value; } }
		public bool BehaviorTriggerB { get { return _behaviorTriggerB; } set { _behaviorTriggerB = value; } }
		public bool BehaviorTriggerC { get { return _behaviorTriggerC; } set { _behaviorTriggerC = value; } }
		public bool BehaviorTriggerD { get { return _behaviorTriggerD; } set { _behaviorTriggerD = value; } }
		public bool BehaviorTriggerE { get { return _behaviorTriggerE; } set { _behaviorTriggerE = value; } }
		public bool BehaviorTriggerF { get { return _behaviorTriggerF; } set { _behaviorTriggerF = value; } }
		public bool BehaviorTriggerG { get { return _behaviorTriggerG; } set { _behaviorTriggerG = value; } }
		public bool BehaviorTriggerH { get { return _behaviorTriggerH; } set { _behaviorTriggerH = value; } }

		public bool BehaviorActionA { get { return _behaviorActionA; } set { _behaviorActionA = value; } }
		public bool BehaviorActionB { get { return _behaviorActionB; } set { _behaviorActionB = value; } }
		public bool BehaviorActionC { get { return _behaviorActionC; } set { _behaviorActionC = value; } }
		public bool BehaviorActionD { get { return _behaviorActionD; } set { _behaviorActionD = value; } }
		public bool BehaviorActionE { get { return _behaviorActionE; } set { _behaviorActionE = value; } }
		public bool BehaviorActionF { get { return _behaviorActionF; } set { _behaviorActionF = value; } }
		public bool BehaviorActionG { get { return _behaviorActionG; } set { _behaviorActionG = value; } }
		public bool BehaviorActionH { get { return _behaviorActionH; } set { _behaviorActionH = value; } }

		public JetpackInhibitor JetpackInhibitorLogic { get { return _jetpackInhibitorLogic; } set { _jetpackInhibitorLogic = value; } }
		public DrillInhibitor DrillInhibitorLogic { get { return _drillInhibitorLogic; } set { _drillInhibitorLogic = value; } }
		public NanobotInhibitor NanobotInhibitorLogic { get { return _nanobotInhibitorLogic; } set { _nanobotInhibitorLogic = value; } }
		public JumpDriveInhibitor JumpInhibitorLogic { get { return _jumpInhibitorLogic; } set { _jumpInhibitorLogic = value; } }
		public PlayerInhibitor PlayerInhibitorLogic { get { return _playerInhibitorLogic; } set { _playerInhibitorLogic = value; } }

		public BlockEntity RemoteControlBlockEntity { 
			
			get {

				if (CurrentGrid == null)
					return null;

				foreach (var remote in CurrentGrid.RivalAi) {

					if (remote.ActiveEntity() || remote.Block.EntityId == RemoteControl.EntityId)
						return remote;
				
				}

				return null;
			
			} 
		
		}

		public long GridId { get { return RemoteControl?.SlimBlock?.CubeGrid == null ? 0 : RemoteControl.SlimBlock.CubeGrid.EntityId; } }
		public string GridName { get { return RemoteControl?.SlimBlock?.CubeGrid?.CustomName == null ? "N/A" : RemoteControl.SlimBlock.CubeGrid.CustomName; } }

		public List<IMyCubeGrid> CurrentGrids { get { return _currentGrids; } }

		public GridEntity CurrentGrid { get {

				if (_currentGrid == null)
					AssignGridEntity();

				return _currentGrid; 
			
			} set {
				
				_currentGrid = value; 
			
			} 
		
		}

		internal GridEntity _currentGrid;

		internal List<Vector3D> _escortOffsets;

		public List<Vector3D> EscortOffsets { get { return _escortOffsets; } set { _escortOffsets = value; } }

		public List<IMyCockpit> DebugCockpits { get { return _debugCockpits; } }

		public string DebugString { get { return _debugString; } set { _debugString = value; } }

		public BehaviorMode PreviousMode;

		public bool SetupCompleted;
		public bool SetupFailed;
		public bool ConfigCheck;
		

		private DateTime _despawnCheckTimer;
		private DateTime _behaviorRunTimer;

		private int _settingSaveCounter;
		private int _settingSaveCounterTrigger;

		private long _mainBehaviorRunCount;

		private Guid _triggerStorageKey;
		private Guid _settingsStorageKey;

		private bool _readyToSaveSettings;
		private string _settingsDataPending;
		
		public bool IsWorking;
		public bool HasBeenWorking; //block was alive at one point
		public bool PhysicsValid;
		public bool HasHasValidPhysics;
		public bool IsEntityClosed;

		public bool IsParentGridClosed;

		public byte CoreCounter;

		internal bool _firstRun;

		public CoreBehavior() {

			RemoteControl = null;
			CubeGrid = null;

			RemoteControlCode = new List<string>();

			SetupCompleted = false;
			SetupFailed = false;
			ConfigCheck = false;
			BehaviorTerminated = false;

			_despawnCheckTimer = MyAPIGateway.Session.GameDateTime;
			_behaviorRunTimer = MyAPIGateway.Session.GameDateTime;

			_settingSaveCounter = 0;
			_settingSaveCounterTrigger = 5;

			_mainBehaviorRunCount = 0;

			_currentGrids = new List<IMyCubeGrid>();
			_debugCockpits = new List<IMyCockpit>();

			_triggerStorageKey = new Guid("8470FBC9-1B64-4603-AB75-ABB2CD28AA02");
			_settingsStorageKey = new Guid("FF814A67-AEC3-4DF0-ADC4-A9B239FA954F");

			_readyToSaveSettings = false;
			_settingsDataPending = "";

			_escortOffsets = new List<Vector3D>();

			IsWorking = false;
			PhysicsValid = false;

			CoreCounter = 0;
			_firstRun = false;

		}

		//------------------------------------------------------------------------
		//--------------START INTERFACE METHODS-----------------------------------
		//------------------------------------------------------------------------

		public virtual void BehaviorInit(IMyRemoteControl remoteControl) {
		
			
		
		}

		public bool IsAIReady() {

			return (IsWorking && PhysicsValid && Owner.NpcOwned && !BehaviorTerminated && SetupCompleted);

		}

		public void ProcessCollisionChecks() {

			_previousProcessingState = _currentProcessingState;
			_currentProcessingState = BehaviorProcessState.None;
			_currentProcessingState |= BehaviorProcessState.CollisionParallel;

			AutoPilot.Collision.PrepareCollisionChecks();

		}

		public void ProcessTargetingChecks() {

			_currentProcessingState |= BehaviorProcessState.TargetingParallel;
			AutoPilot.Targeting.CheckForTarget();
			AutoPilot.Targeting.PrepareTargetLockOn();

		}

		public void ProcessAutoPilotChecks() {

			_currentProcessingState |= BehaviorProcessState.AutopilotParallel;
			AutoPilot.ThreadedAutoPilotCalculations();
			AutoPilot.PrepareAutopilot();

		}

		public void ProcessWeaponChecks() {

			_currentProcessingState |= BehaviorProcessState.WeaponsParallel;
			AutoPilot.Weapons.PrepareWeapons();

		}

		public void ProcessTriggerChecks() {

			_currentProcessingState |= BehaviorProcessState.TriggersParallel;
			Trigger.ProcessTriggerWatchers();

		}

		public void EngageAutoPilot() {

			_currentProcessingState |= BehaviorProcessState.AutopilotMain;
			AutoPilot.Targeting.ProcessTargetLockOn();
			AutoPilot.EngageAutoPilot();

		}

		public void SetDebugCockpit(IMyCockpit block, bool addMode = false) {

			if(addMode)
				_debugCockpits.Add(block);	
			else
				_debugCockpits.Remove(block);

		}

		public void SetInitialWeaponReadiness() {

			//Attempt Weapon Reloads
			_currentProcessingState |= BehaviorProcessState.WeaponsMain;
			AutoPilot.Weapons.ProcessWeaponReloads();

		}

		public void FireWeapons() {

			AutoPilot.Weapons.FireWeapons();

		}

		public void FireBarrageWeapons() {

			AutoPilot.Weapons.FireBarrageWeapons();

		}

		public void ProcessActivatedTriggers() {

			_currentProcessingState |= BehaviorProcessState.TriggersMain;
			Trigger.ProcessActivatedTriggers();

		}
		
		public void CheckDespawnConditions() {

			_currentProcessingState |= BehaviorProcessState.DespawnMain;
			var timeDifference = MyAPIGateway.Session.GameDateTime - _despawnCheckTimer;

			if (timeDifference.TotalMilliseconds <= 999)
				return;

			_settingSaveCounter++;
			//BehaviorLogger.Write("Checking Despawn Conditions", BehaviorDebugEnum.Dev);
			_despawnCheckTimer = MyAPIGateway.Session.GameDateTime;
			Despawn.ProcessTimers(Mode, AutoPilot.InvalidTarget());
			//MainBehavior();

			if (_settingSaveCounter >= _settingSaveCounterTrigger) {

				SaveData();

			}

		}

		public void RunMainBehavior() {

			_currentProcessingState |= BehaviorProcessState.BehaviorMain;
			if (!_firstRun) {

				_firstRun = true;
				FirstRun();

			}

			var timeDifference = MyAPIGateway.Session.GameDateTime - _behaviorRunTimer;

			if (timeDifference.TotalMilliseconds <= 999)
				return;

			_behaviorRunTimer = MyAPIGateway.Session.GameDateTime; //

			MainBehavior();
			_currentProcessingState |= BehaviorProcessState.Complete;
		}

		public void FirstRun() {

			if (AutoPilot.ThrustProfiles.Count == 0 || AutoPilot.GyroProfiles.Count == 0) {

				AutoPilot.ThrustProfiles.Clear();
				AutoPilot.GyroProfiles.Clear();

				var blockList = new List<IMySlimBlock>();
				GridManager.GetBlocksFromGrid<IMyTerminalBlock>(_remoteControl.SlimBlock.CubeGrid, blockList, true);

				foreach (var block in blockList.Where(item => item.FatBlock as IMyThrust != null)) {

					AutoPilot.ThrustProfiles.Add(new ThrusterProfile(block.FatBlock as IMyThrust, _remoteControl, this, AutoPilot.Data.UseSubgridThrust, AutoPilot.Data.MaxSubgridThrustAngle));

				}

				foreach (var block in blockList.Where(item => item.FatBlock as IMyGyro != null && item.CubeGrid == _remoteControl.SlimBlock.CubeGrid)) {

					AutoPilot.GyroProfiles.Add(new GyroscopeProfile(block.FatBlock as IMyGyro, _remoteControl, this));

				}

			}

			Escort.InitializeEscorts();
		
		}

		public bool IsClosed() {

			return (IsEntityClosed || BehaviorTerminated);
		
		}

		public void DebugDrawWaypoints() {

			AutoPilot.DebugDrawingToWaypoints();
		
		}

		public void ChangeBehavior(string newBehaviorSubtypeID, bool preserveSettings = false, bool preserveTriggers = false, bool preserveTargetData = false) {

			string behaviorString = "";

			if (!ProfileManager.BehaviorTemplates.TryGetValue(newBehaviorSubtypeID, out behaviorString)) {

				BehaviorLogger.Write("Behavior With Following Name Not Found: " + newBehaviorSubtypeID, BehaviorDebugEnum.General);
				return;
			
			}

			this.BehaviorTerminated = true;
			this.RemoteControl.CustomData = behaviorString;

			if (this.RemoteControl.Storage == null) {

				this.RemoteControl.Storage = new MyModStorageComponent();

			}

			if (preserveSettings) {

				BehaviorSettings.State.DataMode = AutoPilotDataMode.Primary;
				BehaviorSettings.State.AutoPilotFlags = NewAutoPilotMode.None;
				BehaviorSettings.Mode = BehaviorMode.Init;
				var newSettings = new StoredSettings(BehaviorSettings, preserveSettings, preserveTriggers, preserveTargetData);
				var tempSettingsBytes = MyAPIGateway.Utilities.SerializeToBinary<StoredSettings>(newSettings);
				var tempSettingsString = Convert.ToBase64String(tempSettingsBytes);

				if (this.RemoteControl.Storage.ContainsKey(_settingsStorageKey)) {

					this.RemoteControl.Storage[_settingsStorageKey] = tempSettingsString;

				} else {

					this.RemoteControl.Storage.Add(_settingsStorageKey, tempSettingsString);

				}

			} else {

				this.RemoteControl.Storage[_settingsStorageKey] = "";

			}

			MyAPIGateway.Parallel.Start(() => {

				BehaviorManager.RegisterBehaviorFromRemoteControl(this.RemoteControl);

			});

		}

		public void ChangeTargetProfile(string newTargetProfile) {

			AutoPilot.Targeting.UseNewTargetProfile = true;
			AutoPilot.Targeting.NewTargetProfileName = newTargetProfile;


		}

		//------------------------------------------------------------------------
		//----------------END INTERFACE METHODS-----------------------------------
		//------------------------------------------------------------------------

		public virtual void MainBehavior() {

			if (!_registeredRemoteCode) {

				_registeredRemoteCode = true;

				if (RemoteControlCode.Count > 0) {

					foreach (var code in RemoteControlCode) {

						if(!string.IsNullOrWhiteSpace(code) && !NpcManager.RemoteControlCodes.ContainsKey(this.RemoteControl))
							NpcManager.RemoteControlCodes.Add(this.RemoteControl, code);

					}

				}
			
			}

			if (!_despawnTriggersRegistered) {

				_despawnTriggersRegistered = true;

				foreach (var trigger in Trigger.Triggers) {

					if (trigger.Type == "DespawnMES") {

						NpcManager.RegisterDespawnWatcher(this.RemoteControl?.SlimBlock?.CubeGrid, Trigger.DespawnFromMES);
						break;
					
					}
								
				}

			}

			if (CurrentGrid != null) {

				if (CurrentGrid.Behavior == null)
					CurrentGrid.Behavior = this;

			}

			if (BehaviorSettings.InitialGridIntegrity == 0)
				BehaviorSettings.InitialGridIntegrity = CurrentGrid?.GetCurrentHealth() ?? 0;


			if (ActiveBehavior != null && ActiveBehavior.SubClass == BehaviorSettings.ActiveBehaviorType) {

				_mainBehaviorRunCount++;
				ActiveBehavior.ProcessBehavior();

			} else {

				if (BehaviorSettings.ActiveBehaviorType == BehaviorSubclass.None)
					BehaviorSettings.ActiveBehaviorType = BehaviorManager.GetSubclassFromCustomData(RemoteControl?.CustomData);

				if (BehaviorSettings.ActiveBehaviorType == BehaviorSubclass.None) {

					BehaviorLogger.Write("Could Not Setup Behavior. Behavior Subclass Could Not Be Determined.", BehaviorDebugEnum.BehaviorSetup, true);
					this.BehaviorTerminated = true;

					if (CurrentGrid?.Npc != null)
						CurrentGrid.Npc.BehaviorTerminationReason = "Behavior Subclass Setup Issue";

					return;

				}

				AssignSubClassBehavior(BehaviorSettings.ActiveBehaviorType);

				if (ActiveBehavior != null) {

					_mainBehaviorRunCount++;
					ActiveBehavior.ProcessBehavior();

				}else {

					BehaviorLogger.Write("NPC Active Behavior Null. Terminating Core Behavior", BehaviorDebugEnum.BehaviorSetup, true);
					this.BehaviorTerminated = true;

					if (CurrentGrid?.Npc != null)
						CurrentGrid.Npc.BehaviorTerminationReason = "Active Behavior Null";

				}

			}

		}

		public void ChangeCoreBehaviorMode(BehaviorMode newMode) {

			BehaviorLogger.Write("Changed Core Mode To: " + newMode.ToString(), BehaviorDebugEnum.BehaviorMode);
			this.Mode = newMode;

		}

		public void CoreBehaviorSetup(IMyRemoteControl remoteControl, BehaviorSubclass subclass) {

			BehaviorLogger.Write("Beginning Core Setup On Remote Control with Behavior: " + subclass, BehaviorDebugEnum.BehaviorSetup);

			if (remoteControl == null) {

				BehaviorLogger.Write("Core Setup Failed on Non-Existing Remote Control", BehaviorDebugEnum.BehaviorSetup);
				SetupFailed = true;
				return;

			}

			if (this.ConfigCheck == false) {

				this.ConfigCheck = true;
				var valA = AddonManager.ConfigInstance.Contains(Encoding.UTF8.GetString(Convert.FromBase64String("MTUyMTkwNTg5MA==")));
				var valB = AddonManager.ConfigInstance.Contains(Encoding.UTF8.GetString(Convert.FromBase64String("MjU0MjU5OTEwMA==")));
				var valC = AddonManager.ConfigInstance.Contains(Encoding.UTF8.GetString(Convert.FromBase64String("NzUwODU1")));

				if (AddonManager.ConfigInstance.Contains(Encoding.UTF8.GetString(Convert.FromBase64String("LnNibQ=="))) && (!valA && !valB && !valC)) {

					this.BehaviorTerminated = true;
					if (CurrentGrid?.Npc != null)
						CurrentGrid.Npc.BehaviorTerminationReason = "Setup/Config Error";
					return;

				}

			}

			BehaviorLogger.Write("Verifying if Remote Control is Functional and Has Physics", BehaviorDebugEnum.BehaviorSetup);
			this.RemoteControl = remoteControl;
			this.CubeGrid = remoteControl.SlimBlock.CubeGrid;
			this.RemoteControl.OnClosing += (e) => { this.IsEntityClosed = true; };

			this.RemoteControl.IsWorkingChanged += RemoteIsWorking;
			RemoteIsWorking(this.RemoteControl);

			this.RemoteControl.OnClosing += RemoteIsClosing;
			
			this.CubeGrid.OnPhysicsChanged += PhysicsValidCheck;
			PhysicsValidCheck(this.CubeGrid);

			this.CubeGrid.OnMarkForClose += GridIsClosing;

			BehaviorLogger.Write("Remote Control Working: " + IsWorking.ToString(), BehaviorDebugEnum.BehaviorSetup);
			BehaviorLogger.Write("Remote Control Has Physics: " + PhysicsValid.ToString(), BehaviorDebugEnum.BehaviorSetup);

			BehaviorLogger.Write("Assigning Behavior To Grid Entity", BehaviorDebugEnum.BehaviorSetup);
			AssignGridEntity();

			BehaviorLogger.Write("Setting Up Subsystems", BehaviorDebugEnum.BehaviorSetup);
			BehaviorSettings = new StoredSettings();
			AutoPilot = new AutoPilotSystem(remoteControl, this);
			Broadcast = new BroadcastSystem(this, remoteControl);
			Damage = new DamageSystem(remoteControl, this);
			Despawn = new DespawnSystem(this, remoteControl);
			Diagnostic = new DiagnosticSystem(this, remoteControl);
			Escort = new EscortSystem(this, remoteControl);
			Grid = new GridSystem(remoteControl);
			Heartbeat = new HeartbeatSystem();
			Owner = new OwnerSystem(remoteControl);
			//Spawning = new SpawningSystem(remoteControl);
			Trigger = new TriggerSystem(remoteControl);

			BehaviorLogger.Write("Setting Up Subsystem References", BehaviorDebugEnum.BehaviorSetup);
			AutoPilot.SetupReferences(this, BehaviorSettings, Trigger);
			Damage.SetupReferences(this.Trigger);
			Damage.IsRemoteWorking += () => { return IsWorking && PhysicsValid;};
			Trigger.SetupReferences(this.AutoPilot, this.Broadcast, this.Despawn, this.Grid, this.Owner, this.BehaviorSettings, this);

			AssignSubClassBehavior(subclass);

			if (ActiveBehavior == null) {

				BehaviorLogger.Write("Could Not Setup Behavior. Behavior Subclass Could Not Be Determined.", BehaviorDebugEnum.BehaviorSetup, true);
				if (CurrentGrid?.Npc != null)
					CurrentGrid.Npc.BehaviorTerminationReason = "Behavior Subclass Setup Issue";
				this.BehaviorTerminated = true;
				return;

			}

			InitCoreTags();
			ActiveBehavior.InitTags();
			SetDefaultTargeting();

			SetupCompleted = true;

		}

		public void AssignSubClassBehavior(BehaviorSubclass subclass) {

			BehaviorLogger.Write("Assigning Subclass Behavior.", BehaviorDebugEnum.BehaviorSetup);

			if (subclass == BehaviorSubclass.CargoShip) {

				if (_cargoship == null)
					_cargoship = new CargoShip(this);

				ActiveBehavior = _cargoship;

				if (BehaviorSettings.ActiveBehaviorType != subclass) {

					Mode = BehaviorMode.Init;

					if (!BehaviorSettings.SubclassBehaviorDefaultsSet) {

						BehaviorSettings.SubclassBehaviorDefaultsSet = true;
						ActiveBehavior.SetDefaultTags();

					}
				
				}

				BehaviorSettings.ActiveBehaviorType = subclass;
				return;

			}

			if (subclass == BehaviorSubclass.Escort) {

				if (_escort == null)
					_escort = new Escort(this);

				ActiveBehavior = _escort;

				if (BehaviorSettings.ActiveBehaviorType != subclass) {

					Mode = BehaviorMode.Init;

					if (!BehaviorSettings.SubclassBehaviorDefaultsSet) {

						BehaviorSettings.SubclassBehaviorDefaultsSet = true;
						ActiveBehavior.SetDefaultTags();

					}

				}

				BehaviorSettings.ActiveBehaviorType = subclass;
				return;

			}

			if (subclass == BehaviorSubclass.Fighter) {

				if (_fighter == null)
					_fighter = new Fighter(this);

				ActiveBehavior = _fighter;

				if (BehaviorSettings.ActiveBehaviorType != subclass) {

					Mode = BehaviorMode.Init;

					if (!BehaviorSettings.SubclassBehaviorDefaultsSet) {

						BehaviorSettings.SubclassBehaviorDefaultsSet = true;
						ActiveBehavior.SetDefaultTags();

					}

				}

				BehaviorSettings.ActiveBehaviorType = subclass;
				return;

			}

			if (subclass == BehaviorSubclass.HorseFighter) {

				if (_horsefighter == null)
					_horsefighter = new HorseFighter(this);

				ActiveBehavior = _horsefighter;

				if (BehaviorSettings.ActiveBehaviorType != subclass) {

					Mode = BehaviorMode.Init;

					if (!BehaviorSettings.SubclassBehaviorDefaultsSet) {

						BehaviorSettings.SubclassBehaviorDefaultsSet = true;
						ActiveBehavior.SetDefaultTags();

					}

				}

				BehaviorSettings.ActiveBehaviorType = subclass;
				return;

			}

			if (subclass == BehaviorSubclass.Horsefly) {

				if (_horsefly == null)
					_horsefly = new Horsefly(this);

				ActiveBehavior = _horsefly;

				if (BehaviorSettings.ActiveBehaviorType != subclass) {

					Mode = BehaviorMode.Init;

					if (!BehaviorSettings.SubclassBehaviorDefaultsSet) {

						BehaviorSettings.SubclassBehaviorDefaultsSet = true;
						ActiveBehavior.SetDefaultTags();

					}

				}

				BehaviorSettings.ActiveBehaviorType = subclass;
				return;

			}

			if (subclass == BehaviorSubclass.Hunter) {

				if (_hunter == null)
					_hunter = new Hunter(this);

				ActiveBehavior = _hunter;

				if (BehaviorSettings.ActiveBehaviorType != subclass) {

					Mode = BehaviorMode.Init;

					if (!BehaviorSettings.SubclassBehaviorDefaultsSet) {

						BehaviorSettings.SubclassBehaviorDefaultsSet = true;
						ActiveBehavior.SetDefaultTags();

					}

				}

				BehaviorSettings.ActiveBehaviorType = subclass;
				return;

			}

			if (subclass == BehaviorSubclass.Nautical) {

				if (_nautical == null)
					_nautical = new Nautical(this);

				ActiveBehavior = _nautical;

				if (BehaviorSettings.ActiveBehaviorType != subclass) {

					Mode = BehaviorMode.Init;

					if (!BehaviorSettings.SubclassBehaviorDefaultsSet) {

						BehaviorSettings.SubclassBehaviorDefaultsSet = true;
						ActiveBehavior.SetDefaultTags();

					}

				}

				BehaviorSettings.ActiveBehaviorType = subclass;
				return;

			}

			if (subclass == BehaviorSubclass.Passive) {

				if (_passive == null)
					_passive = new Passive(this);

				ActiveBehavior = _passive;

				if (BehaviorSettings.ActiveBehaviorType != subclass) {

					Mode = BehaviorMode.Init;

					if (!BehaviorSettings.SubclassBehaviorDefaultsSet) {

						BehaviorSettings.SubclassBehaviorDefaultsSet = true;
						ActiveBehavior.SetDefaultTags();

					}

				}

				BehaviorSettings.ActiveBehaviorType = subclass;
				return;

			}

			if (subclass == BehaviorSubclass.Patrol) {

				if (_patrol == null)
					_patrol = new Patrol(this);

				ActiveBehavior = _patrol;

				if (BehaviorSettings.ActiveBehaviorType != subclass) {

					Mode = BehaviorMode.Init;

					if (!BehaviorSettings.SubclassBehaviorDefaultsSet) {

						BehaviorSettings.SubclassBehaviorDefaultsSet = true;
						ActiveBehavior.SetDefaultTags();

					}

				}

				BehaviorSettings.ActiveBehaviorType = subclass;
				return;

			}

			/*
			if (subclass == BehaviorSubclass.Scout) {

				if (_scout == null)
					_scout = new Scout(this);

				ActiveBehavior = _scout;

				if (Settings.ActiveBehaviorType != subclass) {

					Mode = BehaviorMode.Init;

					if (!Settings.SubclassBehaviorDefaultsSet) {

						Settings.SubclassBehaviorDefaultsSet = true;
						ActiveBehavior.SetDefaultTags();

					}

				}

				Settings.ActiveBehaviorType = subclass;
				return;

			}
			*/

			if (subclass == BehaviorSubclass.Sniper) {

				if (_sniper == null)
					_sniper = new Sniper(this);

				ActiveBehavior = _sniper;

				if (BehaviorSettings.ActiveBehaviorType != subclass) {

					Mode = BehaviorMode.Init;

					if (!BehaviorSettings.SubclassBehaviorDefaultsSet) {

						BehaviorSettings.SubclassBehaviorDefaultsSet = true;
						ActiveBehavior.SetDefaultTags();

					}

				}

				BehaviorSettings.ActiveBehaviorType = subclass;
				return;

			}

			if (subclass == BehaviorSubclass.Strike) {

				if (_strike == null)
					_strike = new Strike(this);

				ActiveBehavior = _strike;

				if (BehaviorSettings.ActiveBehaviorType != subclass) {

					Mode = BehaviorMode.Init;

					if (!BehaviorSettings.SubclassBehaviorDefaultsSet) {

						BehaviorSettings.SubclassBehaviorDefaultsSet = true;
						ActiveBehavior.SetDefaultTags();

					}

				}

				BehaviorSettings.ActiveBehaviorType = subclass;
				return;

			}

			if (subclass == BehaviorSubclass.Vulture) {

				if (_vulture == null)
					_vulture = new Vulture(this);

				ActiveBehavior = _vulture;

				if (BehaviorSettings.ActiveBehaviorType != subclass) {

					Mode = BehaviorMode.Init;

					if (!BehaviorSettings.SubclassBehaviorDefaultsSet) {

						BehaviorSettings.SubclassBehaviorDefaultsSet = true;
						ActiveBehavior.SetDefaultTags();

					}

				}

				BehaviorSettings.ActiveBehaviorType = subclass;
				return;

			}

		}

		public void InitCoreTags() {

			BehaviorLogger.Write("Initing Core Tags", BehaviorDebugEnum.BehaviorSetup);

			CoreTags();
			AutoPilot.InitTags();
			InitWeaponProfile();
			Damage.InitTags();
			Despawn.InitTags();
			Escort.InitTags();
			Owner.InitTags();
			Trigger.InitTags();

			PostTagsSetup();

		}

		internal void CoreTags() {

			if (string.IsNullOrWhiteSpace(this.RemoteControl.CustomData) == false) {

				var descSplit = this.RemoteControl.CustomData.Split('\n');

				foreach (var tag in descSplit) {

					//RemoteControlCode
					if (tag.Contains("[RemoteControlCode:") == true) {

						TagParse.TagStringListCheck(tag, ref RemoteControlCode);

					}

				}

			}

		}

		internal virtual void InitWeaponProfile() {

			if(string.IsNullOrWhiteSpace(BehaviorSettings.WeaponsSystemProfile) || BehaviorSettings.WeaponsSystemProfile == ActiveBehavior.DefaultWeaponProfile)
				AutoPilot.Weapons.InitTags(BehaviorSettings.WeaponsSystemProfile);

		}
		
		internal void PostTagsSetup() {


			if (BehaviorSettings.ActiveBehaviorType != BehaviorSubclass.Passive) {

				BehaviorLogger.Write("Setting Inertia Dampeners: " + (AutoPilot.Data.DisableInertiaDampeners ? "False" : "True"), BehaviorDebugEnum.BehaviorSetup);
				MyAPIGateway.Utilities.InvokeOnGameThread(() => { RemoteControl.DampenersOverride = !AutoPilot.Data.DisableInertiaDampeners; });

			}
	
			BehaviorLogger.Write("Post Tag Setup for " + this.RemoteControl.SlimBlock.CubeGrid.CustomName, BehaviorDebugEnum.BehaviorSetup);

			if (BehaviorLogger.ActiveDebug.HasFlag(BehaviorDebugEnum.BehaviorSetup)) {

				BehaviorLogger.Write("Total Triggers: " + Trigger.Triggers.Count.ToString(), BehaviorDebugEnum.BehaviorSetup);
				BehaviorLogger.Write("Total Damage Triggers: " + Trigger.DamageTriggers.Count.ToString(), BehaviorDebugEnum.BehaviorSetup);
				BehaviorLogger.Write("Total Command Triggers: " + Trigger.CommandTriggers.Count.ToString(), BehaviorDebugEnum.BehaviorSetup);
				BehaviorLogger.Write("Total Compromised Triggers: " + Trigger.CompromisedTriggers.Count.ToString(), BehaviorDebugEnum.BehaviorSetup);

			}

			if (Trigger.DamageTriggers.Count > 0)
				Damage.UseDamageDetection = true;

			BehaviorLogger.Write("Beginning Weapon Setup", BehaviorDebugEnum.BehaviorSetup);
			AutoPilot.Weapons.Setup();

			BehaviorLogger.Write("Beginning Damage Handler Setup", BehaviorDebugEnum.BehaviorSetup);
			Damage.SetupDamageHandler();

			BehaviorLogger.Write("Beginning Stored Settings Init/Retrieval", BehaviorDebugEnum.BehaviorSetup);
			bool foundStoredSettings = false;

			if (this.RemoteControl.Storage != null) {

				string tempSettingsString = "";

				this.RemoteControl.Storage.TryGetValue(_settingsStorageKey, out tempSettingsString);

				try {

					if (!string.IsNullOrWhiteSpace(tempSettingsString)) {

						var tempSettingsBytes = Convert.FromBase64String(tempSettingsString);
						StoredSettings tempSettings = MyAPIGateway.Utilities.SerializeFromBinary<StoredSettings>(tempSettingsBytes);

						if (tempSettings != null) {

							BehaviorSettings = tempSettings;
							foundStoredSettings = true;
							BehaviorLogger.Write("Loaded Stored Settings For " + this.RemoteControl.SlimBlock.CubeGrid.CustomName, BehaviorDebugEnum.BehaviorSetup);
							BehaviorLogger.Write("Stored Settings BehaviorMode: " + BehaviorSettings.Mode.ToString(), BehaviorDebugEnum.BehaviorSetup);

							if (!BehaviorSettings.IgnoreTriggers) {

								Trigger.Triggers = BehaviorSettings.Triggers;
								Trigger.DamageTriggers = BehaviorSettings.DamageTriggers;
								Trigger.CommandTriggers = BehaviorSettings.CommandTriggers;
								Trigger.CompromisedTriggers = BehaviorSettings.CompromisedTriggers;

							} else {

								BehaviorSettings.IgnoreTriggers = false;

							}

						} else {

							BehaviorLogger.Write("Stored Settings Invalid For " + this.RemoteControl.SlimBlock.CubeGrid.CustomName, BehaviorDebugEnum.BehaviorSetup);

						}

					}
	
				} catch (Exception e) {

					BehaviorLogger.Write("Failed to Deserialize Existing Stored Remote Control Data on Grid: " + this.RemoteControl.SlimBlock.CubeGrid.CustomName, BehaviorDebugEnum.BehaviorSetup);
					BehaviorLogger.Write(e.ToString(), BehaviorDebugEnum.BehaviorSetup);

				}

			}

			if (!foundStoredSettings) {

				BehaviorLogger.Write("Stored Settings Not Found For " + this.RemoteControl.SlimBlock.CubeGrid.CustomName, BehaviorDebugEnum.BehaviorSetup);
				BehaviorSettings.Triggers = Trigger.Triggers;
				BehaviorSettings.DamageTriggers = Trigger.DamageTriggers;
				BehaviorSettings.CommandTriggers = Trigger.CommandTriggers;
				BehaviorSettings.CompromisedTriggers = Trigger.CompromisedTriggers;
				BehaviorSettings.InitialWeaponCount = (short)AutoPilot.Weapons.GetActiveWeaponCount();
				BehaviorSettings.InitialTurretCount = (short)AutoPilot.Weapons.GetActiveTurretCount();
				BehaviorSettings.InitialGunCount = (short)AutoPilot.Weapons.GetActiveGunCount();
				BehaviorSettings.InitialGridIntegrity = CurrentGrid?.GetCurrentHealth() ?? 0;

			} else {

				var sb = new StringBuilder();
				sb.Append("Checking and Displaying Existing Stored Booleans and Counters").AppendLine();

				if (BehaviorSettings.StoredCustomBooleans != null && BehaviorSettings.StoredCustomBooleans.Keys.Count > 0) {

					sb.Append("Stored Custom Booleans:").AppendLine();

					foreach (var name in BehaviorSettings.StoredCustomBooleans.Keys) {

						if (string.IsNullOrWhiteSpace(name))
							continue;

						bool result = false;

						if (BehaviorSettings.StoredCustomBooleans.TryGetValue(name, out result)) {

							sb.Append(string.Format(" - [{0}] == [{1}]", name, result)).AppendLine();

						}
					
					}

				}

				if (BehaviorSettings.StoredCustomCounters != null && BehaviorSettings.StoredCustomCounters.Keys.Count > 0) {

					sb.Append("Stored Custom Counters:").AppendLine();

					foreach (var name in BehaviorSettings.StoredCustomCounters.Keys) {

						if (string.IsNullOrWhiteSpace(name))
							continue;

						int result = 0;

						if (BehaviorSettings.StoredCustomCounters.TryGetValue(name, out result)) {

							sb.Append(string.Format(" - [{0}] == [{1}]", name, result)).AppendLine();

						}

					}

				}

				if (BehaviorSettings.CurrentTargetEntityId != 0) {

					AutoPilot.Targeting.ForceTargetEntityId = BehaviorSettings.CurrentTargetEntityId;
					AutoPilot.Targeting.ForceRefresh = true;

				}

				BehaviorLogger.Write(sb.ToString(), BehaviorDebugEnum.BehaviorSetup);

			}

			BehaviorSettings.Behavior = this;

			//TODO: Refactor This Into TriggerSystem

			BehaviorLogger.Write("Beginning Individual Trigger Reference Setup", BehaviorDebugEnum.BehaviorSetup);
			foreach (var trigger in Trigger.Triggers) {

				trigger.Conditions.SetReferences(this.RemoteControl, this);

				if (!string.IsNullOrWhiteSpace(trigger.ActionsDefunct?.ProfileSubtypeId))
					trigger.Actions.Add(trigger.ActionsDefunct);

				if(!foundStoredSettings)
					trigger.ResetTime();

				bool buttonTriggerSet = false;

				if (!buttonTriggerSet && trigger.Type == "ButtonPress") {

					trigger.Behavior = this;
					EventWatcher.ButtonPressed += Trigger.ProcessButtonTriggers;
					buttonTriggerSet = true;

				}

			}


			foreach (var trigger in Trigger.DamageTriggers) {

				trigger.Conditions.SetReferences(this.RemoteControl, this);

				if (!foundStoredSettings)
					trigger.ResetTime();

			}
				

			foreach (var trigger in Trigger.CommandTriggers) {

				trigger.Conditions.SetReferences(this.RemoteControl, this);

				if (!foundStoredSettings)
					trigger.ResetTime();

			}

			foreach (var trigger in Trigger.CompromisedTriggers) {

				trigger.Conditions.SetReferences(this.RemoteControl, this);

				if (!foundStoredSettings)
					trigger.ResetTime();

			}

			BehaviorLogger.Write("Setting Callbacks", BehaviorDebugEnum.BehaviorSetup);
			SetupCallbacks();

			BehaviorLogger.Write("Setting Grid Split Check", BehaviorDebugEnum.BehaviorSetup);
			RemoteControl.SlimBlock.CubeGrid.OnGridSplit += GridSplit;
			_currentGrids = MyAPIGateway.GridGroups.GetGroup(RemoteControl.SlimBlock.CubeGrid, GridLinkTypeEnum.Physical);

			BehaviorLogger.Write("Behavior Mode Set To: " + Mode.ToString(), BehaviorDebugEnum.BehaviorSetup); 
			BehaviorLogger.Write("Core Settings Setup Complete", BehaviorDebugEnum.BehaviorSetup);
			 

		}

		internal void SetDefaultTargeting() {

			var savedTarget = !string.IsNullOrWhiteSpace(BehaviorSettings.CurrentTargetProfile);
			var targetProfileName = !savedTarget ? "RivalAI-GenericTargetProfile-EnemyPlayer" : BehaviorSettings.CurrentTargetProfile;

			if (savedTarget || string.IsNullOrWhiteSpace(AutoPilot.Targeting.NormalData.ProfileSubtypeId)) {

				TargetProfile profile = null;

				if (ProfileManager.TargetProfiles.TryGetValue(targetProfileName, out profile) == true) {

					BehaviorSettings.CurrentTargetProfile = targetProfileName;

				}

			}

			var overrideProfileName = string.IsNullOrWhiteSpace(AutoPilot.Targeting.OverrideData.ProfileSubtypeId) ? "RivalAI-GenericTargetProfile-EnemyOverride" : AutoPilot.Targeting.OverrideData.ProfileSubtypeId;

			if (string.IsNullOrWhiteSpace(AutoPilot.Targeting.OverrideData.ProfileSubtypeId)) {

				TargetProfile profile = null;

				if (ProfileManager.TargetProfiles.TryGetValue(overrideProfileName, out profile) == true) {

					BehaviorSettings.OverrideTargetProfile = overrideProfileName;

				}

			}

		}

		private void GridSplit(IMyCubeGrid a, IMyCubeGrid b) {

			a.OnGridSplit -= GridSplit;
			b.OnGridSplit -= GridSplit;
			_currentGrids.Clear();

			if (RemoteControl == null || RemoteControl.MarkedForClose)
				return;

			RemoteControl.SlimBlock.CubeGrid.OnGridSplit += GridSplit;

			_currentGrids = MyAPIGateway.GridGroups.GetGroup(RemoteControl.SlimBlock.CubeGrid, GridLinkTypeEnum.Physical);
			AssignGridEntity();

			if (RemoteControl.SlimBlock.CubeGrid.Storage == null)
				RemoteControl.SlimBlock.CubeGrid.Storage = new MyModStorageComponent();

			if (!RemoteControl.SlimBlock.CubeGrid.Storage.ContainsKey(StorageTools.NpcDataKey)) {

				var dataSource = RemoteControl.SlimBlock.CubeGrid == b ? a : b;
				string data = "";

				if (dataSource.Storage != null && dataSource.Storage.TryGetValue(StorageTools.NpcDataKey, out data))
					RemoteControl.SlimBlock.CubeGrid.Storage.Add(StorageTools.NpcDataKey, data);


			}



		}

		private void AssignGridEntity() {

			if (RemoteControl?.SlimBlock?.CubeGrid == null)
				return;

			if (_currentGrid == null || _currentGrid.CubeGrid != RemoteControl.SlimBlock.CubeGrid) {

				if (_currentGrid != null)
					_currentGrid.Behavior = null;

				_currentGrid = GridManager.GetGridEntity(RemoteControl.SlimBlock.CubeGrid);

				if (_currentGrid != null)
					_currentGrid.Behavior = this;
			
			}
		
		}

		private void SetupCallbacks() {

			//NewAutoPilot.OnComplete += Trigger.ProcessTriggerWatchers;
			Trigger.OnComplete += CheckDespawnConditions;
			EventWatcher.JumpRequested += Trigger.ProcessJumpRequestTriggers;
			EventWatcher.JumpCompleted += Trigger.ProcessJumpCompletedTriggers;

		}

		public void SaveData() {

			if (!IsAIReady())
				return;

			_settingSaveCounter = 0;

			MyAPIGateway.Parallel.Start(() => {

				try {

					var tempSettingsBytes = MyAPIGateway.Utilities.SerializeToBinary<StoredSettings>(BehaviorSettings);
					var tempSettingsString = Convert.ToBase64String(tempSettingsBytes);
					_settingsDataPending = tempSettingsString;
					_readyToSaveSettings = true;

				} catch (Exception e) {

					BehaviorLogger.Write("Exception Occured While Serializing Settings", BehaviorDebugEnum.General);
					BehaviorLogger.Write(e.ToString(), BehaviorDebugEnum.General);

				}

			}, () => {

				MyAPIGateway.Utilities.InvokeOnGameThread(() => {

					if (!_readyToSaveSettings)
						return;

					if (this.RemoteControl.Storage == null) {

						this.RemoteControl.Storage = new MyModStorageComponent();
						BehaviorLogger.Write("Creating Mod Storage on Remote Control", BehaviorDebugEnum.General);

					}

					if (this.RemoteControl.Storage.ContainsKey(_settingsStorageKey)) {

						this.RemoteControl.Storage[_settingsStorageKey] = _settingsDataPending;

					} else {

						this.RemoteControl.Storage.Add(_settingsStorageKey, _settingsDataPending);

					}

					BehaviorLogger.Write("Saved AI Storage Settings To Remote Control", BehaviorDebugEnum.General);
					_readyToSaveSettings = false;

				});

			});

		}

		public void RemoteIsWorking(IMyCubeBlock cubeBlock) {

			if (this.RemoteControl == null || this.RemoteControl.MarkedForClose) {

				this.IsWorking = false;

				if(Trigger != null)
					Trigger.ProcessCompromisedTriggerWatcher(RemoteCompromiseCheck());

			}

			if(this.RemoteControl.IsWorking && this.RemoteControl.IsFunctional) {

				this.HasBeenWorking = true;
				this.IsWorking = true;
				return;

			}

			this.IsWorking = false;

			if (Trigger != null)
				Trigger.ProcessCompromisedTriggerWatcher(RemoteCompromiseCheck());

		}

		public void RemoteIsClosing(IMyEntity entity) {

			if (Trigger != null)
				Trigger.ProcessCompromisedTriggerWatcher(RemoteCompromiseCheck());

		}

		public void GridIsClosing(IMyEntity entity) {

			IsParentGridClosed = true;

		}

		public bool RemoteCompromiseCheck() {

			return !IsWorking && HasBeenWorking && !IsParentGridClosed && Owner.WasNpcOwned;
		
		}

		public void PhysicsValidCheck(IMyEntity entity) {

			if(this.RemoteControl?.SlimBlock?.CubeGrid?.Physics == null) {

				this.PhysicsValid = false;
				return;

			}

			this.HasHasValidPhysics = true;
			this.PhysicsValid = true;

		}

		public override string ToString() {

			var sb = new StringBuilder();
			var timeDifference = MyAPIGateway.Session.GameDateTime - _behaviorRunTimer;

			//CoreBehavior
			sb.Append("::: NPC Core Behavior :::").AppendLine();
			sb.Append(" - Grid Name:           ").Append(RemoteControl.SlimBlock.CubeGrid.CustomName).AppendLine();
			sb.Append(" - Grid Static:         ").Append(RemoteControl.SlimBlock.CubeGrid.IsStatic).AppendLine();
			sb.Append(" - Block Working:       ").Append(IsWorking).AppendLine();
			sb.Append(" - Physics Valid:       ").Append(PhysicsValid).AppendLine();
			sb.Append(" - NPC Owned:           ").Append(Owner.NpcOwned).AppendLine();
			sb.Append(" - Behavior Setup Pass: ").Append(SetupCompleted).AppendLine();
			sb.Append(" - Behavior Setup Fail: ").Append(SetupFailed).AppendLine();
			sb.Append(" - Marked For Close:    ").Append(RemoteControl.SlimBlock.CubeGrid.MarkedForClose).AppendLine();
			sb.Append(" - Behavior Name:       ").Append(CurrentGrid?.Npc?.BehaviorName != null ? CurrentGrid.Npc.BehaviorName : "(null)").AppendLine(); //SeeWhyThisIsntPopulated
			sb.Append(" - Behavior Subclass:   ").Append(BehaviorSettings.ActiveBehaviorType).AppendLine();
			sb.Append(" - Behavior Mode:       ").Append(Mode).AppendLine();
			sb.Append(" - Active Behavior:     ").Append(ActiveBehavior != null ? ActiveBehavior.SubClass.ToString() : "(null)").AppendLine();
			sb.Append(" - Behavior Run Count:  ").Append(_mainBehaviorRunCount.ToString()).AppendLine();
			sb.Append(" - Previous State:      ").Append(_previousProcessingState.ToString()).AppendLine();
			sb.Append(" - Current State:       ").Append(_currentProcessingState.ToString()).AppendLine();
			sb.Append(" - Terminated:          ").Append(BehaviorTerminated).AppendLine();
			sb.Append(" - Next Run Time:       ").Append(timeDifference.TotalMilliseconds).AppendLine();
			sb.Append(" - Vanilla Autopilot:   ").Append(RemoteControl.IsAutoPilotEnabled).AppendLine();
			sb.Append(" - Cargo Ship Watcher:  ").Append(CargoShipWatcher.CargoShips.Contains(CurrentGrid)).AppendLine();
			sb.Append(" - Legacy Cargo Ship:   ").Append(CargoShipWatcher.LegacyAutopilot.Contains(CurrentGrid)).AppendLine();
			sb.Append(" - Health:              ").Append(CurrentGrid?.GetCurrentHealth() ?? 0).Append(" / ").Append(BehaviorSettings.InitialGridIntegrity).AppendLine();
			sb.Append(" - Weapons:             ").Append(AutoPilot?.Weapons?.GetActiveWeaponCount() ?? 0).Append(" / ").Append(BehaviorSettings.InitialWeaponCount).AppendLine();
			sb.Append(" - Turrets:             ").Append(AutoPilot?.Weapons?.GetActiveTurretCount() ?? 0).Append(" / ").Append(BehaviorSettings.InitialTurretCount).AppendLine();
			sb.Append(" - Guns:                ").Append(AutoPilot?.Weapons?.GetActiveGunCount() ?? 0).Append(" / ").Append(BehaviorSettings.InitialGunCount).AppendLine();

			if (RemoteControl.Storage != null) {

				sb.AppendLine();
				string targetString = null;
				sb.Append("::: 3rd Party Mod Influence :::").AppendLine();
				sb.Append(" - Crew Enabled Disabled AI Block:           ").Append(RemoteControl.Storage.TryGetValue(StorageTools.CrewEnabledDamagedRemoteKey, out targetString)).AppendLine();
				sb.Append(" - Infestation Enabled Disabled AI Block:    ").Append(RemoteControl.Storage.TryGetValue(StorageTools.InfestationEnabledDamagedRemoteKey, out targetString)).AppendLine();

			}

			sb.AppendLine();

			sb.Append("::: Behavior Systems Heartbeat :::").AppendLine();
			sb.Append("Parallel Thread Systems ").AppendLine();
			sb.Append(" - Collision:           ").Append(Heartbeat.CollisionParallel).AppendLine();
			sb.Append(" - Targeting:           ").Append(Heartbeat.TargetingParallel).AppendLine();
			sb.Append(" - Autopilot:           ").Append(Heartbeat.AutopilotParallel).AppendLine();
			sb.Append(" - Weapons:             ").Append(Heartbeat.WeaponsParallel).AppendLine();
			sb.Append(" - Triggers:            ").Append(Heartbeat.TriggersParallel).AppendLine();

			sb.Append("Main Thread Systems").AppendLine();
			sb.Append(" - Autopilot:           ").Append(Heartbeat.AutopilotMain).AppendLine();
			sb.Append(" - Weapons:             ").Append(Heartbeat.WeaponsMain).AppendLine();
			sb.Append(" - Triggers:            ").Append(Heartbeat.TriggersMain).AppendLine();
			sb.Append(" - Despawn:             ").Append(Heartbeat.DespawnMain).AppendLine();
			sb.Append(" - Behavior:            ").Append(Heartbeat.BehaviorMain).AppendLine();


			sb.AppendLine();

			sb.Append("::: Active Profiles and Targeting :::").AppendLine();
			sb.Append(" - Current Autopilot:      ").Append(AutoPilot?.Data?.ProfileSubtypeId ?? "N/A").AppendLine();
			sb.Append(" - Current Weapon Profile: ").Append(AutoPilot?.Weapons?.ProfileSubtypeId ?? "N/A").AppendLine();
			sb.Append(" - Current Target Data:    ").Append(AutoPilot?.Targeting?.Data?.ProfileSubtypeId ?? "N/A").AppendLine();
			sb.Append(" - Current Target:         ").Append((AutoPilot?.Targeting?.HasTarget() ?? false) ? AutoPilot.Targeting.Target.Name() ?? "N/A" : "No Target").AppendLine();

			if (AutoPilot?.Targeting?.Data != null) {

				sb.Append(" - Allowed Targets:     ").Append(AutoPilot.Targeting.Data.Target).AppendLine();
				sb.Append(" - Sort Targets:        ").Append(AutoPilot.Targeting.Data.GetTargetBy).AppendLine();
				sb.Append(" - Sort Targets:        ").Append(AutoPilot.Targeting.Data.MaxDistance).AppendLine();
				sb.Append(" - Match All Targets:   ").Append(AutoPilot.Targeting.AllTargetsString()).AppendLine();
				sb.Append(" - Match Any Targets:   ").Append(AutoPilot.Targeting.AnyTargetsString()).AppendLine();
				sb.Append(" - Match None Targets:  ").Append(AutoPilot.Targeting.NoneTargetsString()).AppendLine();
				sb.Append(" - Target Owners:       ").Append(AutoPilot.Targeting.Data.Owners.ToString()).AppendLine();
				sb.Append(" - Target Relations:    ").Append(AutoPilot.Targeting.Data.Relations.ToString()).AppendLine();

			}

			sb.AppendLine();

			//Subclass Behavior
			var subclassBehaviorString = ActiveBehavior.ToString();

			if (!string.IsNullOrWhiteSpace(subclassBehaviorString)) {

				sb.Append(subclassBehaviorString);

			}

			//Behavior Structure
			sb.Append("::: Behavior Structure :::").AppendLine();
			sb.Append(" - Primary Autopilot:   ").Append(AutoPilot.State.PrimaryAutopilotId ?? "N/A").AppendLine();
			sb.Append(" - Secondary Autopilot: ").Append(AutoPilot.State.SecondaryAutopilotId ?? "N/A").AppendLine();
			sb.Append(" - Tertiary Autopilot:  ").Append(AutoPilot.State.TertiaryAutopilotId ?? "N/A").AppendLine();
			sb.Append(" - Primary Targeting:   ").Append(AutoPilot.Targeting.NormalData?.ProfileSubtypeId ?? "N/A").AppendLine();
			sb.Append(" - Override Targeting:  ").Append(AutoPilot.Targeting.OverrideData?.ProfileSubtypeId ?? "N/A").AppendLine();

			var totalTriggerCount = Trigger.Triggers.Count + Trigger.DamageTriggers.Count + Trigger.CommandTriggers.Count + Trigger.CompromisedTriggers.Count;

			if (totalTriggerCount > 0) {

				sb.Append(" - Triggers:            ").Append(totalTriggerCount).AppendLine();
				BuildTriggerData(sb, Trigger.Triggers);
				BuildTriggerData(sb, Trigger.DamageTriggers);
				BuildTriggerData(sb, Trigger.CommandTriggers);
				BuildTriggerData(sb, Trigger.CompromisedTriggers);

			}

			sb.AppendLine();

			if (BehaviorSettings.ActiveBehaviorType != BehaviorSubclass.Passive) {


				//AutoPilot
				sb.Append("::: AutoPilot :::").AppendLine();
				sb.Append(AutoPilot.GetAutopilotData());
				sb.AppendLine();

				sb.Append("::: AutoPilot Flags :::").AppendLine();
				sb.Append(" - RotateToWaypoint:           ").Append(AutoPilot.State.AutoPilotFlags.HasFlag(NewAutoPilotMode.RotateToWaypoint)).AppendLine();
				sb.Append(" - ThrustForward:              ").Append(AutoPilot.State.AutoPilotFlags.HasFlag(NewAutoPilotMode.ThrustForward)).AppendLine();
				sb.Append(" - Strafe:                     ").Append(AutoPilot.State.AutoPilotFlags.HasFlag(NewAutoPilotMode.Strafe)).AppendLine();
				sb.Append(" - LevelWithGravity:           ").Append(AutoPilot.State.AutoPilotFlags.HasFlag(NewAutoPilotMode.LevelWithGravity)).AppendLine();
				sb.Append(" - ThrustUpward:               ").Append(AutoPilot.State.AutoPilotFlags.HasFlag(NewAutoPilotMode.ThrustUpward)).AppendLine();
				sb.Append(" - BarrelRoll:                 ").Append(AutoPilot.State.AutoPilotFlags.HasFlag(NewAutoPilotMode.BarrelRoll)).AppendLine();
				sb.Append(" - CollisionAvoidance:         ").Append(AutoPilot.State.AutoPilotFlags.HasFlag(NewAutoPilotMode.CollisionAvoidance)).AppendLine();
				sb.Append(" - PlanetaryPathing:           ").Append(AutoPilot.State.AutoPilotFlags.HasFlag(NewAutoPilotMode.PlanetaryPathing)).AppendLine();
				sb.Append(" - WaypointFromTarget:         ").Append(AutoPilot.State.AutoPilotFlags.HasFlag(NewAutoPilotMode.WaypointFromTarget)).AppendLine();
				sb.Append(" - Ram:                        ").Append(AutoPilot.State.AutoPilotFlags.HasFlag(NewAutoPilotMode.Ram)).AppendLine();
				sb.Append(" - OffsetWaypoint:             ").Append(AutoPilot.State.AutoPilotFlags.HasFlag(NewAutoPilotMode.OffsetWaypoint)).AppendLine();
				sb.Append(" - RotateToTarget:             ").Append(AutoPilot.State.AutoPilotFlags.HasFlag(NewAutoPilotMode.RotateToTarget)).AppendLine();
				sb.Append(" - WaterNavigation:            ").Append(AutoPilot.State.AutoPilotFlags.HasFlag(NewAutoPilotMode.WaterNavigation)).AppendLine();
				sb.Append(" - HeavyYaw:                   ").Append(AutoPilot.State.AutoPilotFlags.HasFlag(NewAutoPilotMode.HeavyYaw)).AppendLine();
				sb.Append(" - UseFlyLevelWithGravity:     ").Append(AutoPilot.State.UseFlyLevelWithGravity).AppendLine();
				sb.Append(" - UseFlyLevelWithGravityIdle: ").Append(AutoPilot.State.UseFlyLevelWithGravityIdle).AppendLine();
				sb.AppendLine();

				//WeaponCore Gun Readiness
				if (APIs.WeaponCoreApiLoaded) {

					sb.Append("::: WeaponCore Guns Ready :::").AppendLine();

					foreach (var weapon in AutoPilot.Weapons.AllWeapons) {

						if (weapon?.Block() == null)
							continue;

						sb.Append(weapon.Block().CustomName).Append(" /// ").Append(weapon.Status.ToString()).AppendLine();

					}

					sb.AppendLine();

				}


			}

			//CubeGrid
			sb.Append("::: Grid Debug Data :::").AppendLine();
			sb.Append(CurrentGrid.DebugData.ToString());
			sb.AppendLine();

			//Grid NPC Data
			if (CurrentGrid.Npc != null) {

				sb.Append("::: Grid NPC Data :::").AppendLine();
				sb.Append(CurrentGrid.Npc.ToString());
				sb.AppendLine();

			}

			return sb.ToString();
		
		}

		private void BuildTriggerData(StringBuilder sb, List<TriggerProfile> triggers) {

			foreach (var trigger in triggers) {

				sb.Append("   - Trigger:           ").Append(trigger.ProfileSubtypeId).Append(" :: ").Append(trigger.UseTrigger ? "Enabled" : "Disabled").Append(" :: ").Append(trigger.TriggerCount).Append(" / ").Append(trigger.MaxActions).AppendLine();

				if (!string.IsNullOrWhiteSpace(trigger.Conditions?.ProfileSubtypeId))
					sb.Append("     - Condition:       ").Append(trigger.Conditions.ProfileSubtypeId).AppendLine();

				sb.Append("     - Actions:         ").Append(trigger.Actions.Count).AppendLine();

				foreach (var action in trigger.Actions) {

					sb.Append("       - Action:        ").Append(action.ProfileSubtypeId).AppendLine();
					sb.Append("       - Chats:         ").Append(action.ChatData.Count).AppendLine();

					foreach (var chat in action.ChatData) {

						sb.Append("         - Chat:        ").Append(chat.ProfileSubtypeId).AppendLine();

					}

					sb.Append("       - Spawns:        ").Append(action.Spawner.Count).AppendLine();

					foreach (var spawn in action.Spawner) {

						sb.Append("         - Spawn:       ").Append(spawn.ProfileSubtypeId).AppendLine();

					}

				}

				if (trigger.UseElseActions && trigger.ElseActions.Count > 0) {

					sb.Append("     - ElseActions:         ").Append(trigger.Actions.Count).AppendLine();

					foreach (var action in trigger.Actions) {

						sb.Append("       - Action:        ").Append(action.ProfileSubtypeId).AppendLine();
						sb.Append("       - Chats:         ").Append(action.ChatData.Count).AppendLine();

						foreach (var chat in action.ChatData) {

							sb.Append("         - Chat:        ").Append(chat.ProfileSubtypeId).AppendLine();

						}

						sb.Append("       - Spawns:        ").Append(action.Spawner.Count).AppendLine();

						foreach (var spawn in action.Spawner) {

							sb.Append("         - Spawn:       ").Append(spawn.ProfileSubtypeId).AppendLine();

						}

					}

				}

			}

		}

	}
	
}
