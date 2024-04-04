using ModularEncountersSystems.API;
using ModularEncountersSystems.Behavior;
using ModularEncountersSystems.Configuration.Editor;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Create;
using ModularEncountersSystems.Tasks;
using ModularEncountersSystems.Watchers;
using ModularEncountersSystems.World;
using ProtoBuf;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;

namespace ModularEncountersSystems.Sync {

	public enum ChatMsgMode {

		None,
		ServerProcessing,
		ReturnMessage

	}

	[ProtoContract]
	public class ChatMessage {

		[ProtoMember(1)]
		public ChatMsgMode Mode;

		[ProtoMember(2)]
		public string Message;

		[ProtoMember(3)]
		public long PlayerId;

		[ProtoMember(4)]
		public ulong SteamId;

		[ProtoMember(5)]
		public string ReturnMessage;

		[ProtoMember(6)]
		public string ClipboardPayload;

		[ProtoMember(7)]
		public Vector3D PlayerPosition;

		[ProtoMember(8)]
		public long PlayerEntity;

		[ProtoMember(9)]
		public bool IsAdmin;

		[ProtoMember(10)]
		public Vector3D CameraPosition;

		[ProtoMember(11)]
		public Vector3D CameraDirection;

		[ProtoMember(12)]
		public bool UnlockAdminBlocks;

		[ProtoMember(13)]
		public bool DeveloperMode;

		public ChatMessage() {

			Mode = ChatMsgMode.None;
			Message = "";
			PlayerId = 0;
			SteamId = 0;
			ReturnMessage = "";
			ClipboardPayload = "";
			PlayerPosition = Vector3D.Zero;
			PlayerEntity = 0;
			IsAdmin = false;
			CameraPosition = Vector3D.Zero;
			CameraDirection = Vector3D.Zero;
			UnlockAdminBlocks = false;

		}

		public bool ProcessChat() {

			Message = Message.Trim();

			//Catch Shortened Messages
			ExpandShortenedMessages();

			//Determine Where Command Belongs

			SpawnLogger.Write("Chat Command Received: " + Message, SpawnerDebugEnum.Settings);

			//Spawn
			if (Message.StartsWith("/MES.Spawn."))
				return ProcessSpawn();

			//Settings
			if (Message.StartsWith("/MES.Settings."))
				return ProcessSettings();

			//Command
			if (Message.StartsWith("/MES.Command."))
				return ProcessCommand();

			//SpawnDebug
			if (Message.StartsWith("/MES.SpawnDebug."))
				return ProcessSpawnDebug();

			//BehaviorDebug
			if (Message.StartsWith("/MES.BehaviorDebug."))
				return ProcessBehaviorDebug();

			//Info
			if (Message.StartsWith("/MES.Info."))
				return ProcessInfo();

			//Create

			if (Message.StartsWith("/MES.Create."))
				return ProcessCreate();

			

			//Debug
			if (Message.StartsWith("/MES.Debug."))
				return ProcessDebug();



			SpawnLogger.Write("Chat Command Type Isn't Recognized", SpawnerDebugEnum.Settings);
			ReturnMessage = "Chat Command Type Isn't Recognized";
			return false;

		}

		private void ExpandShortenedMessages() {

			if (Message.StartsWith("/MES.SSCS"))
				Message = Message.Replace("/MES.SSCS", "/MES.Spawn.SpaceCargoShip");

			if (Message.StartsWith("/MES.SRE"))
				Message = Message.Replace("/MES.SRE", "/MES.Spawn.RandomEncounter");

			if (Message.StartsWith("/MES.SPCS"))
				Message = Message.Replace("/MES.SPCS", "/MES.Spawn.PlanetaryCargoShip");

			if (Message.StartsWith("/MES.SPI"))
				Message = Message.Replace("/MES.SPI", "/MES.Spawn.PlanetaryInstallation");

			if (Message.StartsWith("/MES.SBE"))
				Message = Message.Replace("/MES.SBE", "/MES.Spawn.BossEncounter");

			if (Message.StartsWith("/MES.SC"))
				Message = Message.Replace("/MES.SC", "/MES.Spawn.Creature");

			if (Message.StartsWith("/MES.SDE"))
				Message = Message.Replace("/MES.SDE", "/MES.Spawn.DroneEncounter");

			if (Message.StartsWith("/MES.SSE"))
				Message = Message.Replace("/MES.SSE", "/MES.Spawn.StaticEncounter");
			
			if (Message.StartsWith("/MES.SP"))
				Message = Message.Replace("/MES.SP", "/MES.Spawn.Prefab");

			if (Message.StartsWith("/MES.SSP"))
				Message = Message.Replace("/MES.SSP", "/MES.Spawn.PrefabStation");

			if (Message.StartsWith("/MES.GTS"))
				Message = Message.Replace("/MES.GTS", "/MES.Info.GetThreatScore");

			if (Message.StartsWith("/MES.GESAP"))
				Message = Message.Replace("/MES.GESAP", "/MES.Info.GetEligibleSpawnsAtPosition");

			if (Message.StartsWith("/MES.IGLSD"))
				Message = Message.Replace("/MES.IGLSD", "/MES.Info.GetLogging.SpawnDebug");

			if (Message.StartsWith("/MES.IGLBD"))
				Message = Message.Replace("/MES.IGLBD", "/MES.Info.GetLogging.BehaviorDebug");

			if (Message.StartsWith("/MES.IGGB"))
				Message = Message.Replace("/MES.IGGB", "/MES.Info.GetGridBehavior");

			if (Message.StartsWith("/MES.IGGD"))
				Message = Message.Replace("/MES.IGGD", "/MES.Info.GetGridData");

			if (Message.StartsWith("/MES.IGD"))
				Message = Message.Replace("/MES.IGD", "/MES.Info.GetDiagnostics");

		}

		public string[] GetArray(int length, int combineLength) {

			var array = Message.Trim().Split('.');

			if (array.Length < length)
				return null;

			if (array.Length > combineLength) {

				string lastElement = "";

				for (int i = length - 1; i < array.Length; i++) {

					lastElement += array[i];

				}

				array[length - 1] = lastElement;

			}

			return array;
		
		}

		private bool ProcessSpawn() {

			var array = GetArray(3, 5);

			if (array == null) {

				SpawnLogger.Write("Chat Array Size Too Small", SpawnerDebugEnum.Settings);
				ReturnMessage = "Chat Array Size Too Small";
				return false;

			}

			SpawnLogger.Write("Get Spawn Type From Chat", SpawnerDebugEnum.Settings);
			SpawningType type = SpawningType.None;

			//MES.Spawn.ForceSpawnTimer
			if (array[2] == "ForceSpawnTimer") {

				LoggerTools.ForceSpawnTimer(this, array);
				return true;

			}

			if (array[2] == "WaveSpawner") {

				if (array.Length < 4) {

					SpawnLogger.Write("Missing WaveSpawner Type", SpawnerDebugEnum.Settings);
					ReturnMessage = "Missing WaveSpawner Type";
					return false;

				}

				if (array[3] == "Space") {

					WaveManager.Space.Timer = WaveManager.Space.TimerTrigger;
					WaveManager.Space.ProcessWaveSpawner(true);
					SpawnLogger.Write("Wave Spawner (Space) Activated", SpawnerDebugEnum.Settings);
					ReturnMessage = "Wave Spawner (Space) Activated";
					return true;

				}

				if (array[3] == "Planet") {

					WaveManager.Planet.Timer = WaveManager.Planet.TimerTrigger;
					WaveManager.Planet.ProcessWaveSpawner(true);
					SpawnLogger.Write("Wave Spawner (Planet) Activated", SpawnerDebugEnum.Settings);
					ReturnMessage = "Wave Spawner (Planet) Activated";
					return true;

				}

				if (array[3] == "Creature") {

					WaveManager.Creature.Timer = WaveManager.Creature.TimerTrigger;
					WaveManager.Creature.ProcessWaveSpawner(true);
					SpawnLogger.Write("Wave Spawner (Creature) Activated", SpawnerDebugEnum.Settings);
					ReturnMessage = "Wave Spawner (Creature) Activated";
					return true;

				}

				SpawnLogger.Write("Provided WaveSpawner Type Not Recognized", SpawnerDebugEnum.Settings);
				ReturnMessage = "Provided WaveSpawner Type Not Recognized";
				return false;

			}

			if (array[2] == "Prefab") {

				PrefabSpawner.PrefabSpawnDebug(this);
				return true;

			}

			if (array[2] == "PrefabStation") {

				PrefabSpawner.PrefabStationSpawnDebug(this);
				return true;

			}

			if (array[2] == "SpaceCargoShip")
				type = SpawningType.SpaceCargoShip;

			if (array[2] == "RandomEncounter")
				type = SpawningType.RandomEncounter;

			if (array[2] == "PlanetaryCargoShip")
				type = SpawningType.PlanetaryCargoShip;

			if (array[2] == "PlanetaryInstallation")
				type = SpawningType.PlanetaryInstallation;

			if (array[2] == "BossEncounter")
				type = SpawningType.BossEncounter;

			if (array[2] == "Creature")
				type = SpawningType.Creature;

			if (array[2] == "DroneEncounter")
				type = SpawningType.DroneEncounter;


			if (type == SpawningType.None) {

				SpawnLogger.Write("No Spawning Type From Chat", SpawnerDebugEnum.Settings);
				ReturnMessage = "No Spawning Type From Chat";
				return false;

			}

			List<string> spawnNames = null;

			if (array.Length >= 4 && !string.IsNullOrWhiteSpace(array[3]))
				spawnNames = new List<string> { array[3] };

			try {

				SpawnLogger.Write("Chat Command Spawn Request Sent", SpawnerDebugEnum.Settings);
				ReturnMessage = "Chat Command Spawn Request Sent";
				SpawnRequest.CalculateSpawn(this.PlayerPosition, "MES-ChatCommand", type, false, true, spawnNames);

			} catch (Exception e) {

				SpawnLogger.Write(e.ToString(), SpawnerDebugEnum.Error, true);
			
			}

			
			return true;
		
		}

		private bool ProcessSettings() {

			ReturnMessage = EditorTools.EditSettings(Message);
			return true;

		}

		private bool ProcessBehaviorDebug() {

			var array = GetArray(4, 4);

			if (array == null) {

				SpawnLogger.Write("Array Size Too Small",  SpawnerDebugEnum.Settings);
				ReturnMessage = "Missing Command Elements";
				return true;

			}

			bool mode = false;
			BehaviorDebugEnum type = BehaviorDebugEnum.None;

			if (!bool.TryParse(array[3], out mode)) {

				ReturnMessage = "BehaviorDebug Set Mode Not Recognized";
				return true;

			}


			if (!Enum.TryParse<BehaviorDebugEnum>(array[2], out type)) {

				ReturnMessage = "BehaviorDebug Type Not Recognized";
				return true;

			}

			var result = BehaviorLogger.SetActiveDebugFlag(type, mode);

			if (result)
				ReturnMessage = "BehaviorDebug Type " + array[2] + " Set To: " + array[3];
			else
				ReturnMessage = "BehaviorDebug Type " + array[2] + " Already Set To: " + array[3];

			return result;

		}

		private bool ProcessSpawnDebug() {

			var array = GetArray(4, 4);

			if (array == null) {

				SpawnLogger.Write("Array Size Too Small", SpawnerDebugEnum.Settings);
				ReturnMessage = "Missing Command Elements";
				return true;

			}

			bool mode = false;
			SpawnerDebugEnum type = SpawnerDebugEnum.None;

			if (!bool.TryParse(array[3], out mode)) {

				ReturnMessage = "SpawnDebug Set Mode Not Recognized";
				return true;

			}


			if (!Enum.TryParse<SpawnerDebugEnum>(array[2], out type)) {

				ReturnMessage = "SpawnDebug Type Not Recognized";
				return true;

			}

			var result = SpawnLogger.SetActiveDebugFlag(type, mode);

			if(result)
				ReturnMessage = "SpawnDebug Type " + array[2] + " Set To: " + array[3];
			else
				ReturnMessage = "SpawnDebug Type " + array[2] + " Already Set To: " + array[3];

			return result;

		}

		private bool ProcessDebug() {

			var array = GetArray(3, 8);

			if (array == null) {

				SpawnLogger.Write("Array Size Too Small", SpawnerDebugEnum.Settings);
				return false;

			}

			//MES.Debug.AttachShipyardProfile
			if (array[2] == "AttachShipyardProfile") {

				LoggerTools.DebugAttachShipyardProfile(this);
				return true;

			}

			//MES.Debug.AttachSuitUpgradeModule
			if (array[2] == "AttachSuitUpgradeModule") {

				LoggerTools.DebugAttachSuitUpgradeModule(this);
				return true;

			}

			//MES.Debug.Autopilot
			if (array[2] == "Autopilot") {

				LoggerTools.DebugAutoPilot(this);
				return true;

			}

			//MES.Debug.ChangeBool
			if (array[2] == "ChangeBool") {

				LoggerTools.ChangeBool(this, array);
				return true;

			}

			//MES.Debug.ChangeCounter
			if (array[2] == "ChangeCounter") {

				LoggerTools.ChangeCounter(this, array);
				return true;

			}

			//MES.Debug.ClearAllTimeouts
			if (array[2] == "ClearAllTimeouts") {

				LoggerTools.ClearAllTimeouts(this);
				return true;

			}

			//MES.Debug.ClearShipInventory
			if (array[2] == "ClearShipInventory") {

				LoggerTools.ClearShipInventory(this);
				return true;

			}

			//MES.Debug.ClearStaticEncounters
			if (array[2] == "ClearStaticEncounters") {

				foreach (var enc in NpcManager.StaticEncounters) {

					enc.IsValid = false;
				
				}

				NpcManager.UpdateStaticEncounters();
				return true;

			}

			if (array[2] == "ClearThrust") {

				LoggerTools.DebugClearThrust(this);
				return true;

			}

			//MES.Debug.ClearTimeoutsAtPosition
			if (array[2] == "ClearTimeoutsAtPosition") {

				LoggerTools.ClearTimeoutsAtPosition(this);
				return true;

			}

			//MES.Debug.ClearUniqueEncounters
			if (array[2] == "ClearUniqueEncounters") {

				NpcManager.UniqueGroupsSpawned.Clear();
				NpcManager.UpdateStaticEncounters();
				return true;

			}

			//MES.Debug.ResetThisStaticEncounters
			if (array[2] == "ResetThisStaticEncounters")
			{
				if (array.Length >= 4 && !string.IsNullOrWhiteSpace(array[3]))
				{
					NpcManager.ResetThisResetThisStaticEncounter(array[3]);
					return true;
				}
			}


			//MES.Debug.CreateIdStorage
			if (array[2] == "CreateIdStorage") {

				ClipboardPayload = LoggerTools.CreateIdStorage(this, array);
				Mode = ChatMsgMode.ReturnMessage;
				ReturnMessage = "Created Id Storage and Saved To Clipboard.";
				return true;

			}

			//MES.Debug.CreateKPL
			if (array[2] == "CreateKPL") {

				LoggerTools.CreateKPL(this, array);
				return true;

			}

			//MES.Debug.CreatePlanet
			if (array[2] == "CreatePlanet") {

				var name = array.Length >= 4 ? array[3] : "EarthLike";
				float size = 120000;

				if (array.Length >= 5) {

					float.TryParse(array[4], out size);

				}

				var pos = this.CameraDirection * (size * 2) + this.CameraPosition;
				var pos2 = (size * 1.89186136208056666) * new Vector3D(-0.577350269189626, -0.577350269189626, -0.577350269189626) + pos;

				MyAPIGateway.Session.VoxelMaps.SpawnPlanet(name, size, MathTools.RandomBetween(1000000, 10000000), pos2);

				return true;

			}

			//MES.Debug.CreatePredeterminedVoxel
			if (array[2] == "CreatePredeterminedVoxel") {

				var name = array.Length >= 4 ? array[3] : "Barths_moon_base";
				float size = 120000;

				var pos = this.CameraDirection * 1000 + this.CameraPosition;

				MyAPIGateway.Session.VoxelMaps.CreatePredefinedVoxelMap(name, null, MatrixD.CreateWorld(pos, MatrixD.Identity.Forward, MatrixD.Identity.Up), false);

				return true;

			}

			//MES.Debug.CreateProceduralVoxelMap
			if (array[2] == "CreateProceduralVoxelMap") {

				float size = 512;

				var pos = this.CameraDirection * 1000 + this.CameraPosition;

				MyAPIGateway.Session.VoxelMaps.CreateProceduralVoxelMap(MathTools.RandomBetween(1000000, 10000000), size, MatrixD.CreateWorld(pos, MatrixD.Identity.Forward, MatrixD.Identity.Up));

				return true;

			}

			//MES.Debug.DeveloperMode
			if (array[2] == "DeveloperMode") {

				Mode = ChatMsgMode.ReturnMessage;
				MES_SessionCore.DeveloperMode = !MES_SessionCore.DeveloperMode;
				this.DeveloperMode = MES_SessionCore.DeveloperMode;
				this.ReturnMessage = "Developer Mode Set: " + MES_SessionCore.DeveloperMode;
				return true;

			}

			//MES.Debug.DeleteGrid
			if (array[2] == "DeleteGrid") {

				LoggerTools.DeleteGrid(this);
				return true;

			}

			//MES.Debug.DrawPaths
			if (array[2] == "DrawPaths") {

				BehaviorManager.DebugDraw = !BehaviorManager.DebugDraw;
				this.ReturnMessage = "Path Drawing For Behaviors Active: " + BehaviorManager.DebugDraw;
				return true;

			}

			//MES.Debug.ForceCombatPhase
			if (array[2] == "ForceCombatPhase") {

				CombatPhaseManager.ForcePhase(true);
				return true;

			}

			//MES.Debug.ForcePeacePhase
			if (array[2] == "ForcePeacePhase") {

				CombatPhaseManager.ForcePhase(false);
				return true;

			}

			//MES.Debug.GetBlockData
			if (array[2] == "GetBlockData") {

				LoggerTools.GetBlockData(this);
				return true;

			}

			//MES.Debug.Lanes
			if (array[2] == "Lanes") {

				this.ReturnMessage = "LaneCount: " + PlanetManager.Lanes.Count;
				return true;

			}

			//MES.Debug.GetBlockPairs
			if (array[2] == "GetBlockPairs") {

				LoggerTools.DebugBlockPairs(this);
				return true;

			}

			//MES.Debug.GetPlanetData
			if (array[2] == "GetPlanetData") {

				var planet = PlanetManager.GetNearestPlanet(this.CameraPosition);

				//MyVisualScriptLogicProvider.ShowNotificationToAll("Checking Planet Null", 3000);
				if (planet != null) {

					var voxel = planet.Planet as IMyVoxelBase;

					if (voxel != null) {

						//MyVisualScriptLogicProvider.ShowNotificationToAll("Getting Details From Planet", 3000);

						var dist = Vector3D.Distance(planet.Center(), voxel.PositionLeftBottomCorner);
						var diameter = planet.Planet.AverageRadius * 2;
						var dir = Vector3D.Normalize(voxel.PositionLeftBottomCorner - planet.Center());

						var sb = new StringBuilder();
						sb.Append("Distance:  ").Append(dist).AppendLine();
						sb.Append("Diameter:  ").Append(diameter).AppendLine();
						sb.Append("Direction: ").Append(dir.ToString()).AppendLine();
						this.ClipboardPayload = sb.ToString();
						this.ReturnMessage = "Planet Details Saved To Clipboard";
						this.Mode = ChatMsgMode.ReturnMessage;

					}
				
				}

				return true;

			}

			//MES.Debug.LinkedGrids
			if (array[2] == "LinkedGrids") {

				LoggerTools.DebugLinkedGrids(this);
				return true;

			}

			//MES.Debug.ProcessPrefabs
			if (array[2] == "ProcessPrefabs") {

				LoggerTools.ProcessPrefabs(this, array);
				return true;

			}

			//MES.Debug.RemoveAllNpcs
			if (array[2] == "RemoveAllNpcs") {

				LoggerTools.RemoveAllNpcs(this);
				return true;

			}

			//MES.Debug.ResetReputation
			if (array[2] == "ResetReputation") {

				LoggerTools.ResetReputation(this, array);
				return true;

			}

			//MES.Debug.ResetZones
			if (array[2] == "ResetZones") {

				LoggerTools.ResetZones(this);
				return true;

			}

			//MES.Debug.RotateBlockYaw
			if (array[2] == "RotateBlockYaw") {

				LoggerTools.DebugRotateBlockYaw(this);
				return true;

			}

			//MES.Debug.RotateBlockPitch
			if (array[2] == "RotateBlockPitch") {

				LoggerTools.DebugRotateBlockPitch(this);
				return true;

			}

			//MES.Debug.RotateBlockRoll
			if (array[2] == "RotateBlockRoll") {

				LoggerTools.DebugRotateBlockRoll(this);
				return true;

			}

			//MES.Debug.BlockOrientation
			if (array[2] == "BlockOrientation") {

				LoggerTools.DebugBlockOrientation(this);
				return true;

			}

			//MES.Debug.RotationData
			if (array[2] == "RotationData") {

				LoggerTools.RotationData(this, array);
				return true;

			}

			//MES.Debug.SaveGridTest
			if (array[2] == "SaveGridTest") {

				LoggerTools.DebugTestSaveGrid(this);
				return true;

			}

			//MES.Debug.SetGridOwnership
			if (array[2] == "SetGridOwnership") {

				LoggerTools.SetGridOwnership(this, array);
				return true;

			}

			//MES.Debug.SpawnSingleBlocks
			if (array[2] == "SpawnSingleBlocks") {

				LoggerTools.DebugSpawnAllSingleBlocks(this);
				return true;

			}

			//MES.Debug.Meteor
			if (array[2] == "Meteor") {

				var definitionId = new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Stone");
				var amount = (MyFixedPoint)10000;
				var content = (MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(definitionId);
				MyObjectBuilder_InventoryItem inventoryItem = new MyObjectBuilder_InventoryItem { Amount = amount, Content = content, PhysicalContent = content };

				var matrix = MatrixD.CreateWorld(CameraPosition, CameraDirection, VectorHelper.RandomPerpendicular(CameraDirection));
				var offset = new Vector3D(0, MathTools.RandomBetween(-600, -100), MathTools.RandomBetween(-600, -100));
				var position = new MyPositionAndOrientation {
					Position = Vector3D.Transform(offset, matrix),
					Forward = (Vector3)matrix.Forward,
					Up = (Vector3)matrix.Up,
				};


				var meteor = new MyObjectBuilder_Meteor();
				meteor.Item = inventoryItem;
				meteor.PersistentFlags = VRage.ObjectBuilders.MyPersistentEntityFlags2.InScene;
				meteor.PositionAndOrientation = position;
				meteor.LinearVelocity = (Vector3)matrix.Forward * 500;
				meteor.Integrity = 350;

				MyAPIGateway.Entities.RemapObjectBuilder(meteor);
				var entity = MyAPIGateway.Entities.CreateFromObjectBuilderAndAdd(meteor);
				MyVisualScriptLogicProvider.ShowNotificationToAll("Spiders!", 2000);

				return true;

			}

			//MES.Debug.SetReputation
			if (array[2] == "SetReputation") {

				//LoggerTools.SetReputation(this, array);
				return true;

			}

			//MES.Debug.SetSyncedReputation
			if (array[2] == "SetSyncedReputation") {

				//LoggerTools.SetSyncedReputation(this, array);
				return true;

			}

			//MES.Debug.SpawnAllPrefabs
			if (array[2] == "SpawnAllPrefabs") {

				LoggerTools.SpawnAllPrefabs(this, array);
				return true;

			}

			//MES.Debug.StoreTest
			if (array[2] == "StoreTest") {

				LoggerTools.StoreTest(this);
				return true;

			}

			//MES.Debug.YeetGrid
			if (array[2] == "YeetGrid") {

				LoggerTools.YeetGrid(this);
				return true;

			}

			//TestAsteroidSpawns
			if (array[2] == "TestAsteroidSpawns") {

				var matrix = MatrixD.CreateWorld(CameraPosition, CameraDirection, VectorHelper.RandomPerpendicular(CameraDirection));

				var oldcoords = Vector3D.Transform(new Vector3D(-300, 0, 800), matrix);
				var newMatrix = MatrixD.CreateWorld(Vector3D.Transform(new Vector3D(300, 0, 800), matrix), Vector3D.Forward, Vector3D.Up);

				var voxelSpawnA = MyAPIGateway.Session.VoxelMaps.CreateVoxelMapFromStorageName("Nearby_Station_7", "Nearby_Station_7", oldcoords);
				var voxelSpawnB = MyAPIGateway.Session.VoxelMaps.CreatePredefinedVoxelMap("Nearby_Station_7", null, newMatrix, true);

				ReturnMessage = "Test Spawn Asteroids:";
				Mode = ChatMsgMode.ReturnMessage;
				return true;

			}

			//MES.Debug.SpawnConstruct
			if (array[2] == "SpawnConstruct") {

				ReturnMessage = "Attempting Spawn Construct";
				LoggerTools.DebugSpawnConstruct(this);
				Mode = ChatMsgMode.ReturnMessage;
				return true;

			}

			//MES.Debug.TestSpawnAPI
			if (array[2] == "TestSpawnAPI") {

				var list = new List<string>();
				list.Add("Reaver-Group-Test-A");
				var matrix = MyAPIGateway.Session.LocalHumanPlayer.Character.WorldMatrix;
				matrix.Translation = matrix.Forward * 1000 + matrix.Translation;
				var result = APIs.MES.CustomSpawnRequest(list, matrix, Vector3.Zero, false, null, "Debug");
				ReturnMessage = "Test Spawn: " + result + " / " + APIs.MES.MESApiReady;
				Mode = ChatMsgMode.ReturnMessage;
				return true;
			
			}

			//MES.Debug.TextTest
			if (array[2] == "TextTest") {

				var textText = ProfileManager.GetTextTemplate("Imber-TextTemplate-DatapadLore.xml");

				if (textText == null) {

					ReturnMessage = "Could Not Find TextTemplate";
					Mode = ChatMsgMode.ReturnMessage;
					return true;
				
				}

				if (textText.DataPadEntries.Length == 0) {

					ReturnMessage = "Datapad Entries Count 0";
					Mode = ChatMsgMode.ReturnMessage;
					return true;

				}

				ReturnMessage = "Found TextTemplate";
				Mode = ChatMsgMode.ReturnMessage;
				ClipboardPayload = textText.DataPadEntries[5].GetBody();
				MyAPIGateway.Utilities.ShowMissionScreen(textText.DataPadEntries[5].GetTitle(), null, null, textText.DataPadEntries[5].GetBody());

				return true;

			}

			/*
			//MES.Debug.ToolParentEntity
			if (array[2] == "ToolParentEntity") {

				var character = PlayerManager.GetPlayerWithIdentityId(PlayerId)?.Player?.Character;

				if (character?.EquippedTool == null) {

					MyVisualScriptLogicProvider.ShowNotification("No Character or Equipped Tool", 4000);
					return true;
				
				}

				if (character.EquippedTool.GetTopMostParent() as IMyCharacter == null) {

					MyVisualScriptLogicProvider.ShowNotification("Tool Parent Is Not Character", 4000);
					MyVisualScriptLogicProvider.ShowNotification("Tool Parent Is Tool: " + (character.EquippedTool.GetTopMostParent() == character.EquippedTool), 4000);

				} else {

					MyVisualScriptLogicProvider.ShowNotification("Tool Parent Is Character", 4000);

				}

				return true;

			}
			*/

			//MES.Debug.UnlockAdminBlocks
			if (array[2] == "UnlockNpcBlocks" || array[2] == "UnlockAdminBlocks") {

				Mode = ChatMsgMode.ReturnMessage;
				ReturnMessage = "NPC-Only Blocks Have Been Unlocked For This Session.";
				UnlockAdminBlocks = true;
				return true;

			}

			return false;

		}

		private bool ProcessInfo() {

			var array = GetArray(3, 5);

			if (array == null) {

				SpawnLogger.Write("Array Size Too Small", SpawnerDebugEnum.Settings);
				return false;

			}

			//GetActiveNpcs
			if (array[2] == "GetActiveNpcs") {

				ClipboardPayload = NpcManager.GetActiveNpcData();
				Mode = ChatMsgMode.ReturnMessage;
				ReturnMessage = "Active Npc Data Sent To Clipboard.";
				return true;

			}

			//GetAllProfiles
			if (array[2] == "GetAllProfiles") {

				ClipboardPayload = LoggerTools.GetAllProfiles();
				Mode = ChatMsgMode.ReturnMessage;
				ReturnMessage = "Loaded Profiles Sent To Clipboard.";

				if (MyAPIGateway.Utilities.IsDedicated)
					SpawnLogger.Write(ClipboardPayload, SpawnerDebugEnum.GameLog, true);

				return true;

			}

			//GetBlockDefinitions
			if (array[2] == "GetBlockDefinitions") {

				ClipboardPayload = DefinitionHelper.GetBlockDefinitionInfo();
				Mode = ChatMsgMode.ReturnMessage;
				ReturnMessage = "Block Definition Info Sent To Clipboard.";
				return true;

			}

			//GetBlockMassData
			if (array[2] == "GetBlockMassData") {

				ClipboardPayload = LoggerTools.GetBlockMassData(this);
				Mode = ChatMsgMode.ReturnMessage;
				ReturnMessage = "Block Mass Data Sent To Clipboard.";
				return true;

			}

			//GetBlockOffset
			/*
			if (newChatData.Message.StartsWith("/RAI.GetBlockOffset")) {

				if (DebugTerminalControls.ReferenceRemoteControl == null || DebugTerminalControls.ReferenceRemoteControl.MarkedForClose)
					return;

				var rc = DebugTerminalControls.ReferenceRemoteControl.WorldMatrix;
				var playerPos = chatData.PlayerPosition - rc.Translation;
				var offset = new Vector3D(Vector3D.Dot(playerPos, rc.Right), Vector3D.Dot(playerPos, rc.Up), Vector3D.Dot(playerPos, rc.Forward));
				var offsetString = offset.ToString();

				if (string.IsNullOrEmpty(offsetString)) {

					return;

				}

				MyVisualScriptLogicProvider.ShowNotification("Offset Position To Reference Block Saved To Clipboard", 5000, "White", chatData.PlayerId);
				VRage.Utils.MyClipboardHelper.SetClipboard(offsetString);
			

			}
			*/

			//GetColorsFromGrid
			if (array[2] == "GetColorsFromGrid") {


				Mode = ChatMsgMode.ReturnMessage;
				ClipboardPayload = LoggerTools.GetColorListFromGrid(this);
				return true;

			}

			//GetDiagnostics
			if (array[2] == "GetDiagnostics") {


				Mode = ChatMsgMode.ReturnMessage;
				ReturnMessage = "Spawner Diagnostics Sent To Clipboard";
				ClipboardPayload = LoggerTools.GetDiagnostics(this);
				return true;

			}

			//GetEligibleSpawnsAtPosition
			if (array[2] == "GetEligibleSpawnsAtPosition") {


				Mode = ChatMsgMode.ReturnMessage;
				ReturnMessage = "Eligible Spawns Sent To Clipboard.";
				ClipboardPayload = LoggerTools.GetEligibleSpawnsAtPosition(this);
				return true;

			}

			//GetGridBehavior
			if (array[2] == "GetGridBehavior") {

				ClipboardPayload = LoggerTools.GetGridBehavior(this);
				Mode = ChatMsgMode.ReturnMessage;
				return true;

			}

			//GetGridData
			if (array[2] == "GetGridData") {

				ClipboardPayload = LoggerTools.GetGridData(this);
				Mode = ChatMsgMode.ReturnMessage;
				return true;

			}

			//GetGridMatrix
			if (array[2] == "GetGridMatrix") {

				ClipboardPayload = LoggerTools.GetGridMatrixInfo(this);
				Mode = ChatMsgMode.ReturnMessage;
				return true;

			}

			//GetItemMassData
			if (array[2] == "GetItemMassData") {

				ClipboardPayload = LoggerTools.GetItemMassData(this);
				Mode = ChatMsgMode.ReturnMessage;
				ReturnMessage = "Item Mass Data Sent To Clipboard.";
				return true;

			}

			//GetItemPrices
			if (array[2] == "GetItemPrices") {

				ClipboardPayload = LoggerTools.GetItemPrices(this);
				Mode = ChatMsgMode.ReturnMessage;
				ReturnMessage = "Item Price Data Sent To Clipboard.";
				return true;

			}

			//GetLogging
			if (array[2] == "GetLogging") {

				ClipboardPayload = LoggerTools.GetLogging(array, ref ReturnMessage);
				Mode = ChatMsgMode.ReturnMessage;

				if (MyAPIGateway.Utilities.IsDedicated)
					SpawnLogger.Write(ClipboardPayload, SpawnerDebugEnum.GameLog, true);

				return string.IsNullOrWhiteSpace(ClipboardPayload);

			}

			//GetPlayers
			if (array[2] == "GetPlayers") {

				var sb = new StringBuilder();

				foreach (var player in PlayerManager.Players) {

					player.GetPlayerInfo(sb);

				}

				ClipboardPayload = sb.ToString();
				Mode = ChatMsgMode.ReturnMessage;
				ReturnMessage = "Current Player Data Sent To Clipboard.";
				return true;

			}

			//GetThreatScore
			if (array[2] == "GetThreatScore") {

				LoggerTools.GetThreatScore(this, array);
				return true;

			}

			//GetVersion
			if (array[2] == "GetVersion") {

				Mode = ChatMsgMode.ReturnMessage;
				ReturnMessage = "Mod Version: " + MES_SessionCore.ModVersion;
				return true;

			}

			//GetZones
			if (array[2] == "GetZones") {

				LoggerTools.GetZones(this);
				return true;

			}


			//GetZones
			if (array[2] == "GetEvents")
			{

				LoggerTools.GetEvents(this);
				return true;

			}

			

			return false;

		}

		public bool ProcessCommand() {

			//MES.Command.Antenna.Dist.Code
			//MES.Command.NoAntenna.Dist.Code
			/*
			var msg = this.Message.Split('.');

			if (msg.Length != 5 || string.IsNullOrWhiteSpace(msg[4])) {

				this.ReturnMessage = "Command Received Could Not Be Read Properly.";
				return false;

			}

			double distance = 0;

			if (!double.TryParse(msg[3], out distance)) {

				this.ReturnMessage = "Command Distance Unreadable.";
				return false;

			}

			var players = new List<IMyPlayer>();
			MyAPIGateway.Players.GetPlayers(players);

			foreach (var player in players) {

				if (player.IdentityId != PlayerId || player?.Controller?.ControlledEntity?.Entity == null)
					continue;

				var command = new Command();
				command.CommandCode = msg[4];
				command.Character = player.Controller.ControlledEntity.Entity;
				command.Radius = distance;
				command.IgnoreAntennaRequirement = msg[2] == "Antenna" ? false : true;
				CommandHelper.CommandTrigger?.Invoke(command);
				this.ReturnMessage = "Command Message Broadcast.";
				return true;

			}

			this.ReturnMessage = "Not Sent From Valid Player or Player Has No Entity.";
			*/

			return false;
		
		}


		private bool ProcessCreate()
        {

			var array = GetArray(3, 5);

			if (array == null)
			{

				SpawnLogger.Write("Array Size Too Small", SpawnerDebugEnum.Settings);
				return false;

			}

			//GetEligibleSpawnsAtPosition
			if (array[2] == "Event")
			{
				if (array.Length < 4)
				{
					ReturnMessage = "Missing Type";
					return false;

				}

				if (array[3] == "Area")
				{

					Mode = ChatMsgMode.ReturnMessage;
					ReturnMessage = "Eligible Spawns Sent To Clipboard.";
					ClipboardPayload = CreateMaker.CreateEventArea(this,array[4]);
					return true;

				}



			}
			return false;
        }

		public bool ProcessDebugMode() {

			// /RAI.Debug.Mode.true
			/*
			var msg = GetArray(Message, 4, 4);
			
			if(msg == null) {

				this.ReturnMessage = "Command Received Could Not Be Read Properly.";
				return false;

			}

			bool result = false;

			if(bool.TryParse(msg[3], out result) == false) {

				this.ReturnMessage = "Debug Mode Value Not Recognized. Accepts true or false.";
				return true;

			}

			if (msg[2] == "SpawnerDebug") {

				this.ReturnMessage = "Spawner Debugging Is Now " ;
				return true;

			}

			if (msg[2] == "BehaviorDebug") {


				return true;

			}

			if (msg[2] == "RemoveAll" && result) {

				this.ReturnMessage = "Debug Type: " + msg[2] + " Set: " + result.ToString();
				Logger.DisableAllOptions();
				Logger.SaveDebugToSandbox();
				
				return true;

			}
			
			this.ReturnMessage = "Debug Command Not Recognized: " + msg[2];
			*/

			return false;

		}

	}

}
