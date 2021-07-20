using ModularEncountersSystems.API;
using ModularEncountersSystems.Behavior;
using ModularEncountersSystems.Behavior.Subsystems;
using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Behavior.Subsystems.Trigger;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.World;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRage.ModAPI;

namespace ModularEncountersSystems.Behavior {

	public class CoreBehavior : IBehavior {

		public IMyRemoteControl RemoteControl { get { return _remoteControl; } set { _remoteControl = value; } }
		public IMyCubeGrid CubeGrid;

		public List<string> RemoteControlCode;

		//public BaseSystems Systems;

		private bool _behaviorTerminated;

		private bool _registeredRemoteCode;
		private bool _despawnTriggersRegistered;

		private AutoPilotSystem _newAutoPilot;
		private BroadcastSystem _broadcast;
		private DamageSystem _damage;
		private DespawnSystem _despawn;
		private GridSystem _extras;
		private OwnerSystem _owner;
		private StoredSettings _settings;
		private TriggerSystem _trigger;

		//Behavior Subclasses
		private IBehaviorSubClass ActiveBehavior;
		private Fighter _fighter;
		private Horsefly _horsefly;
		private HorseFighter _horsefighter;
		private Hunter _hunter;
		private Strike _strike;
		private Passive _passive;
		private CargoShip _cargoship;
		private Scout _scout;
		private Sniper _sniper;
		private Nautical _nautical;

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



		private List<IMyCubeGrid> _currentGrids;
		private List<IMyCockpit> _debugCockpits;

		private IMyRemoteControl _remoteControl;

		public AutoPilotSystem AutoPilot { get { return _newAutoPilot; } set { _newAutoPilot = value; } }
		public BroadcastSystem Broadcast { get { return _broadcast; } set { _broadcast = value; } }
		public DamageSystem Damage { get { return _damage; } set { _damage = value; } }
		public DespawnSystem Despawn { get { return _despawn; } set { _despawn = value; } }
		public GridSystem Grid { get { return _extras; } set { _extras = value; } }
		public OwnerSystem Owner { get { return _owner; } set { _owner = value; } }
		public StoredSettings Settings { get { return _settings; } set { _settings = value; } }
		public TriggerSystem Trigger { get { return _trigger; } set { _trigger = value; } }

		public BehaviorMode Mode { 
			
			get {
				
				if(this.Settings != null)
					return this.Settings.Mode;

				return BehaviorMode.Init;
			
			}
			
			set {

				if (this.Settings != null)
					this.Settings.Mode = value;

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

		public long GridId { get { return RemoteControl?.SlimBlock?.CubeGrid == null ? 0 : RemoteControl.SlimBlock.CubeGrid.EntityId; } }
		public string GridName { get { return RemoteControl?.SlimBlock?.CubeGrid?.CustomName == null ? "N/A" : RemoteControl.SlimBlock.CubeGrid.CustomName; } }

		public List<IMyCubeGrid> CurrentGrids { get { return _currentGrids; } }

		public GridEntity CurrentGrid { get {

				if (_currentGrid == null)
					AssignGridEntity();

				return _currentGrid; 
			
			} set {
				
				_currentGrid = value; 
			
			} }
		internal GridEntity _currentGrid;

		public List<IMyCockpit> DebugCockpits { get { return _debugCockpits; } }

		public BehaviorMode PreviousMode;

		public bool SetupCompleted;
		public bool SetupFailed;
		public bool ConfigCheck;
		

		private DateTime _despawnCheckTimer;
		private DateTime _behaviorRunTimer;

		private int _settingSaveCounter;
		private int _settingSaveCounterTrigger;

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

			_currentGrids = new List<IMyCubeGrid>();
			_debugCockpits = new List<IMyCockpit>();

			_triggerStorageKey = new Guid("8470FBC9-1B64-4603-AB75-ABB2CD28AA02");
			_settingsStorageKey = new Guid("FF814A67-AEC3-4DF0-ADC4-A9B239FA954F");

			_readyToSaveSettings = false;
			_settingsDataPending = "";

			IsWorking = false;
			PhysicsValid = false;

			CoreCounter = 0;

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

			AutoPilot.Collision.PrepareCollisionChecks();

		}

		public void ProcessTargetingChecks() {

			AutoPilot.Targeting.CheckForTarget();

		}

		public void ProcessAutoPilotChecks() {

			AutoPilot.ThreadedAutoPilotCalculations();
			AutoPilot.PrepareAutopilot();

		}

		public void ProcessWeaponChecks() {

			AutoPilot.Weapons.PrepareWeapons();

		}

		public void ProcessTriggerChecks() {

			Trigger.ProcessTriggerWatchers();

		}

		public void EngageAutoPilot() {
		
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
			AutoPilot.Weapons.ProcessWeaponReloads();

		}

		public void FireWeapons() {

			AutoPilot.Weapons.FireWeapons();

		}

		public void FireBarrageWeapons() {

			AutoPilot.Weapons.FireBarrageWeapons();

		}

		public void ProcessActivatedTriggers() {

			Trigger.ProcessActivatedTriggers();

		}
		
		public void CheckDespawnConditions() {

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

			var timeDifference = MyAPIGateway.Session.GameDateTime - _behaviorRunTimer;

			if (timeDifference.TotalMilliseconds <= 999)
				return;

			_behaviorRunTimer = MyAPIGateway.Session.GameDateTime;
			MainBehavior();

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

				Settings.State.DataMode = AutoPilotDataMode.Primary;
				Settings.State.AutoPilotFlags = NewAutoPilotMode.None;
				Settings.Mode = BehaviorMode.Init;
				var newSettings = new StoredSettings(Settings, preserveSettings, preserveTriggers, preserveTargetData);
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

			if (ActiveBehavior != null && ActiveBehavior.SubClass == Settings.ActiveBehaviorType)
				ActiveBehavior.ProcessBehavior();
			else {

				if (Settings.ActiveBehaviorType == BehaviorSubclass.None)
					Settings.ActiveBehaviorType = BehaviorManager.GetSubclassFromCustomData(RemoteControl?.CustomData);

				if (Settings.ActiveBehaviorType == BehaviorSubclass.None) {

					BehaviorLogger.Write("Could Not Setup Behavior. Behavior Subclass Could Not Be Determined.", BehaviorDebugEnum.BehaviorSetup, true);
					this.BehaviorTerminated = true;
					return;

				}

				AssignSubClassBehavior(Settings.ActiveBehaviorType);

				if (ActiveBehavior != null)
					ActiveBehavior.ProcessBehavior();

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
			Settings = new StoredSettings();
			AutoPilot = new AutoPilotSystem(remoteControl, this);
			Broadcast = new BroadcastSystem(remoteControl);
			Damage = new DamageSystem(remoteControl);
			Despawn = new DespawnSystem(this, remoteControl);
			Grid = new GridSystem(remoteControl);
			Owner = new OwnerSystem(remoteControl);
			//Spawning = new SpawningSystem(remoteControl);
			Trigger = new TriggerSystem(remoteControl);

			BehaviorLogger.Write("Setting Up Subsystem References", BehaviorDebugEnum.BehaviorSetup);
			AutoPilot.SetupReferences(this, Settings, Trigger);
			Damage.SetupReferences(this.Trigger);
			Damage.IsRemoteWorking += () => { return IsWorking && PhysicsValid;};
			Trigger.SetupReferences(this.AutoPilot, this.Broadcast, this.Despawn, this.Grid, this.Owner, this.Settings, this);

			AssignSubClassBehavior(subclass);

			if (ActiveBehavior == null) {

				BehaviorLogger.Write("Could Not Setup Behavior. Behavior Subclass Could Not Be Determined.", BehaviorDebugEnum.BehaviorSetup, true);
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

			if (subclass == BehaviorSubclass.Fighter) {

				if (_fighter == null)
					_fighter = new Fighter(this);

				ActiveBehavior = _fighter;

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

			if (subclass == BehaviorSubclass.HorseFighter) {

				if (_horsefighter == null)
					_horsefighter = new HorseFighter(this);

				ActiveBehavior = _horsefighter;

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

			if (subclass == BehaviorSubclass.Horsefly) {

				if (_horsefly == null)
					_horsefly = new Horsefly(this);

				ActiveBehavior = _horsefly;

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

			if (subclass == BehaviorSubclass.Hunter) {

				if (_hunter == null)
					_hunter = new Hunter(this);

				ActiveBehavior = _hunter;

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

			if (subclass == BehaviorSubclass.Nautical) {

				if (_nautical == null)
					_nautical = new Nautical(this);

				ActiveBehavior = _nautical;

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

			if (subclass == BehaviorSubclass.Passive) {

				if (_passive == null)
					_passive = new Passive(this);

				ActiveBehavior = _passive;

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

			if (subclass == BehaviorSubclass.Strike) {

				if (_strike == null)
					_strike = new Strike(this);

				ActiveBehavior = _strike;

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

		}

		public void InitCoreTags() {

			BehaviorLogger.Write("Initing Core Tags", BehaviorDebugEnum.BehaviorSetup);

			CoreTags();
			AutoPilot.InitTags();
			AutoPilot.Weapons.InitTags();
			Damage.InitTags();
			Despawn.InitTags();
			Owner.InitTags();
			Trigger.InitTags();

			PostTagsSetup();


		}

		public void CoreTags() {

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
		
		
		public void PostTagsSetup() {


			if (Settings.ActiveBehaviorType != BehaviorSubclass.Passive) {

				BehaviorLogger.Write("Setting Inertia Dampeners: " + (AutoPilot.Data.DisableInertiaDampeners ? "False" : "True"), BehaviorDebugEnum.BehaviorSetup);
				RemoteControl.DampenersOverride = !AutoPilot.Data.DisableInertiaDampeners;

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

							Settings = tempSettings;
							foundStoredSettings = true;
							BehaviorLogger.Write("Loaded Stored Settings For " + this.RemoteControl.SlimBlock.CubeGrid.CustomName, BehaviorDebugEnum.BehaviorSetup);
							BehaviorLogger.Write("Stored Settings BehaviorMode: " + Settings.Mode.ToString(), BehaviorDebugEnum.BehaviorSetup);

							if (!Settings.IgnoreTriggers) {

								Trigger.Triggers = Settings.Triggers;
								Trigger.DamageTriggers = Settings.DamageTriggers;
								Trigger.CommandTriggers = Settings.CommandTriggers;
								Trigger.CompromisedTriggers = Settings.CompromisedTriggers;

							} else {

								Settings.IgnoreTriggers = false;

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
				Settings.Triggers = Trigger.Triggers;
				Settings.DamageTriggers = Trigger.DamageTriggers;
				Settings.CommandTriggers = Trigger.CommandTriggers;
				Settings.CompromisedTriggers = Trigger.CompromisedTriggers;

			} else {

				var sb = new StringBuilder();
				sb.Append("Checking and Displaying Existing Stored Booleans and Counters").AppendLine();

				if (Settings.StoredCustomBooleans != null && Settings.StoredCustomBooleans.Keys.Count > 0) {

					sb.Append("Stored Custom Booleans:").AppendLine();

					foreach (var name in Settings.StoredCustomBooleans.Keys) {

						if (string.IsNullOrWhiteSpace(name))
							continue;

						bool result = false;

						if (Settings.StoredCustomBooleans.TryGetValue(name, out result)) {

							sb.Append(string.Format(" - [{0}] == [{1}]", name, result)).AppendLine();

						}
					
					}

				}

				if (Settings.StoredCustomCounters != null && Settings.StoredCustomCounters.Keys.Count > 0) {

					sb.Append("Stored Custom Counters:").AppendLine();

					foreach (var name in Settings.StoredCustomCounters.Keys) {

						if (string.IsNullOrWhiteSpace(name))
							continue;

						int result = 0;

						if (Settings.StoredCustomCounters.TryGetValue(name, out result)) {

							sb.Append(string.Format(" - [{0}] == [{1}]", name, result)).AppendLine();

						}

					}

				}

				if (Settings.CurrentTargetEntityId != 0) {

					AutoPilot.Targeting.ForceTargetEntityId = Settings.CurrentTargetEntityId;
					AutoPilot.Targeting.ForceRefresh = true;

				}

				BehaviorLogger.Write(sb.ToString(), BehaviorDebugEnum.BehaviorSetup);

			}

			//TODO: Refactor This Into TriggerSystem

			BehaviorLogger.Write("Beginning Individual Trigger Reference Setup", BehaviorDebugEnum.BehaviorSetup);
			foreach (var trigger in Trigger.Triggers) {

				trigger.Conditions.SetReferences(this.RemoteControl, Settings);

				if (!string.IsNullOrWhiteSpace(trigger.ActionsDefunct?.ProfileSubtypeId))
					trigger.Actions.Add(trigger.ActionsDefunct);

				if(!foundStoredSettings)
					trigger.ResetTime();

			}


			foreach (var trigger in Trigger.DamageTriggers) {

				trigger.Conditions.SetReferences(this.RemoteControl, Settings);

				if (!foundStoredSettings)
					trigger.ResetTime();

			}
				

			foreach (var trigger in Trigger.CommandTriggers) {

				trigger.Conditions.SetReferences(this.RemoteControl, Settings);

				if (!foundStoredSettings)
					trigger.ResetTime();

			}

			foreach (var trigger in Trigger.CompromisedTriggers) {

				trigger.Conditions.SetReferences(this.RemoteControl, Settings);

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

			var savedTarget = !string.IsNullOrWhiteSpace(Settings.CustomTargetProfile);
			var targetProfileName = !savedTarget ? "RivalAI-GenericTargetProfile-EnemyPlayer" : Settings.CustomTargetProfile;

			if (savedTarget || string.IsNullOrWhiteSpace(AutoPilot.Targeting.NormalData.ProfileSubtypeId)) {

				byte[] byteData = { };

				if (ProfileManager.TargetObjectTemplates.TryGetValue(targetProfileName, out byteData) == true) {

					try {

						var profile = MyAPIGateway.Utilities.SerializeFromBinary<TargetProfile>(byteData);

						if (profile != null) {

							AutoPilot.Targeting.NormalData = profile;

						}

					} catch (Exception) {



					}

				}

			}

			if (string.IsNullOrWhiteSpace(AutoPilot.Targeting.OverrideData.ProfileSubtypeId)) {

				byte[] byteData = { };

				if (ProfileManager.TargetObjectTemplates.TryGetValue("RivalAI-GenericTargetProfile-EnemyOverride", out byteData) == true) {

					try {

						var profile = MyAPIGateway.Utilities.SerializeFromBinary<TargetProfile>(byteData);

						if (profile != null) {

							AutoPilot.Targeting.OverrideData = profile;

						}

					} catch (Exception) {



					}

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

		
		}

		public void SaveData() {

			if (!IsAIReady())
				return;

			_settingSaveCounter = 0;

			MyAPIGateway.Parallel.Start(() => {

				try {

					var tempSettingsBytes = MyAPIGateway.Utilities.SerializeToBinary<StoredSettings>(Settings);
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

		

	}
	
}
