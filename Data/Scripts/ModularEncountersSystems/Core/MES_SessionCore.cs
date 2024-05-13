using ModularEncountersSystems.Admin;
using ModularEncountersSystems.API;
using ModularEncountersSystems.Behavior;
using ModularEncountersSystems.BlockLogic;
using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Events;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Progression;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.Spawning.Procedural;
using ModularEncountersSystems.Sync;
using ModularEncountersSystems.Tasks;
using ModularEncountersSystems.Terminal;
using ModularEncountersSystems.Watchers;
using ModularEncountersSystems.World;
using ModularEncountersSystems.Zones;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.Components;

namespace ModularEncountersSystems.Core {

	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
	public class MES_SessionCore : MySessionComponentBase {

		public static bool ModEnabled = true;
		public static bool OfflineDetected = false;
		public static bool SyncWarning = false;
		public bool FinalSetup = false;

		public static string ModVersion = "2.71.12";
		public static int ModVersionValue = 200710012; //Use above value as reference - 3 digits per part (
		public static MES_SessionCore Instance;

		public static bool IsServer;
		public static bool IsDedicated;
		public static DateTime SessionStartTime;

		public static bool DeveloperMode;
		public static List<ulong> Developers = new List<ulong> { 76561197995523659, 76561198058958866 };

		public static bool AreaTestStart;

		public static Action SaveActions;
		public static Action UnloadActions;

		public static string SaveName;

		public override void LoadData() {

			Instance = this;

			IsServer = MyAPIGateway.Multiplayer.IsServer;
			IsDedicated = MyAPIGateway.Utilities.IsDedicated;
			ModEnabled = CheckSyncRules();

			if (!ModEnabled)
				return;

			//Register Version Checker
			MyAPIGateway.Utilities.RegisterMessageHandler(21521905890, CompareVersions);

			SpawnLogger.Setup();
			BehaviorLogger.Setup();
			TaskProcessor.Setup();
			SyncManager.Setup(); //Register Network and Chat Handlers
			DefinitionHelper.Setup();
			EconomyHelper.Setup();
			Settings.InitSettings("LoadData"); //Get Existing Settings From XML or Create New
			AddonManager.DetectAddons(); //Check Add-on Mods
			ProfileManager.Setup();
			SpawnGroupManager.CreateSpawnLists();
			BotSpawner.Setup();
			APIs.RegisterAPIs(0); //Register Any Applicable APIs

			if (!IsServer)
				return;

			BlockManager.Setup(); //Build Lists of Special Blocks
			PlayerSpawnWatcher.Setup();
			PrefabSpawner.Setup();

		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent) {

			if (!ModEnabled)
				return;

			if (!MyAPIGateway.Multiplayer.IsServer)
				return;

			APIs.TextHud = new HudAPIv2();

		}

		public override void BeforeStart() {

			if (!ModEnabled)
				return;

			MyAPIGateway.Utilities.SendModMessage(21521905890, ModVersionValue);

			Settings.InitSettings("BeforeStart"); //Get Existing Settings From XML or Create New
			BlockLogicManager.Setup();
			ProgressionManager.Setup();
			ProgressionDataManager.Setup();
			ControlManager.Setup();
			EntityWatcher.RegisterWatcher(); //Scan World For Entities and Setup AutoDetect For New Entities
			SetDefaultSettings();
			APIs.RegisterAPIs(2); //Register Any Applicable APIs
			AddonManager.ProcessMesAddons();



			if (!MyAPIGateway.Multiplayer.IsServer)
				return;

			ProgressionManager.ButtonPanelStartupValidation();
			ProgramBlockControls.SpawnProgramBlockForControls();
			LocalApi.SendApiToMods();
			FactionHelper.PopulateNpcFactionLists();
			EventWatcher.Setup();
			NpcManager.Setup();
			CargoShipWatcher.Setup();
			ZoneManager.Setup();
			BehaviorManager.Setup();
			RelationManager.Setup();
			Cleaning.Setup();
			WaveManager.Setup();
			DamageHelper.Setup();
			PrefabManipulation.Setup();
			ProceduralShipManager.Setup();
			CombatPhaseManager.Setup();
			EventManager.Setup();

			SessionStartTime = MyAPIGateway.Session.GameDateTime;
			//AttributeApplication

			SaveName = MyAPIGateway.Session.Name;

		}

		public override void UpdateBeforeSimulation() {

			if (!FinalSetup) {

				FinalSetup = true;
				MyAPIGateway.Utilities.UnregisterMessageHandler(21521905890, CompareVersions);
			
			}

			if (!ModEnabled) {

				if (!MyAPIGateway.Utilities.IsDedicated && OfflineDetected) {

					var sb = new StringBuilder();
					sb.Append("WARNING!!!").AppendLine().AppendLine();
					sb.Append("Modular Encounters System is current not running because this session is using the config option [SelectivePhysicsUpdates] while also using a [SyncDistance] value of less than 10000. This causes most NPCs to behave incorrectly, so as a result the entire mod disables when it detects this configuration.").AppendLine().AppendLine();
					sb.Append("To fix this, either set your Server [SyncDistance] value to 10000 or higher, or disable the [SelectivePhysicsUpdates] option entirely.");
					MyAPIGateway.Utilities.ShowMissionScreen("Modular Encounters Systems", "", "", sb.ToString());

				}

				UnloadActions?.Invoke();
				MyAPIGateway.Utilities.InvokeOnGameThread(() => { this.UpdateOrder = MyUpdateOrder.NoUpdate; });
				return;
			
			}

			TaskProcessor.Process();

		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder() {

			if (!MyAPIGateway.Multiplayer.IsServer)
				return base.GetObjectBuilder();

			MyAPIGateway.Utilities.InvokeOnGameThread(() => {

				SaveActions?.Invoke();

				if(Settings.SavedData != null && Settings.SavedData.DataChanged)
					Settings.SavedData.SaveSettings();

				if (SaveName != MyAPIGateway.Session.Name) {

					SaveName = MyAPIGateway.Session.Name;
					Settings.SaveAll();

				}
					
			
			}); 

			return base.GetObjectBuilder();
		}

		protected override void UnloadData() {

			UnloadActions?.Invoke();
			Settings.SaveAll();

		}

		private static bool CheckSyncRules() {

			if (MES_SessionCore.Instance?.ModContext?.ModId != null && MES_SessionCore.Instance.ModContext.ModId.Contains(".sbm")) {

				foreach (var mod in MyAPIGateway.Session.Mods) {

					var context = mod.GetModContext();

					if (context != null && mod.PublishedFileId == 0 && (context.ModName.Contains("Modular Encounters Systems") || context.ModName.Contains("Modular_Encounters_Systems") || context.ModName.Contains("ModularEncountersSystems"))) {

						if (MyAPIGateway.Utilities.FileExistsInModLocation("ModularEncountersSystemsMod.txt", mod)) {

							SpawnLogger.Write("Detected Offline / Local Version of MES loaded with Workshop Version of MES. Disabling Workshop Version", SpawnerDebugEnum.Startup, true);
							OfflineDetected = true;
							ModEnabled = false;
							return false;

						}

					}

				}

			}

			if (!IsDedicated)
				return true;

			if (MyAPIGateway.Session.SessionSettings.EnableSelectivePhysicsUpdates && MyAPIGateway.Session.SessionSettings.SyncDistance < 10000) {

				//TODO: Log SPU Restriction
				SpawnLogger.Write("WARNING: Selective Physics Updates is Enabled with SyncDistance Less Than 10000. Modular Encounters Systems may not work correctly.", SpawnerDebugEnum.Startup, true);
				SpawnLogger.Write("This could result in NPC grids spawning and getting stuck, or NPC grids not being visible until much closer to the player.", SpawnerDebugEnum.Startup, true);
				SpawnLogger.Write("Consider raising SyncDistance to 10000 or higher while using Selective Physics Updates.", SpawnerDebugEnum.Startup, true);
				SyncWarning = true;
				//return false;

			}

			return true;
		
		}

		private static void CompareVersions(object data) {

			int version = (int)data;

			if (version > ModVersionValue) {

				ModEnabled = false;
				SpawnLogger.Write("Another Version of MES Detected With Higher Version Number: " + version, SpawnerDebugEnum.Startup, true);
				SpawnLogger.Write("Shutting Down Duplicate Instance of MES. Mod With Version " + version + " Will Continue To Run.", SpawnerDebugEnum.Startup, true);

			}

		}

		private static void SetDefaultSettings() {

			if (MyAPIGateway.Session.SessionSettings.CargoShipsEnabled)
				MyAPIGateway.Session.SessionSettings.CargoShipsEnabled = false;

			if (MyAPIGateway.Session.SessionSettings.EnableEncounters)
				MyAPIGateway.Session.SessionSettings.EnableEncounters = false;

		}

	}

}
