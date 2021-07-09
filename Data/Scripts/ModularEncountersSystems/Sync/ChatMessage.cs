using ModularEncountersSystems.API;
using ModularEncountersSystems.Configuration.Editor;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Watchers;
using ModularEncountersSystems.World;
using ProtoBuf;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game.ModAPI;
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

			if (Message.StartsWith("/MES.SSE"))
				Message = Message.Replace("/MES.SSE", "/MES.Spawn.StaticEncounter");

			if (Message.StartsWith("/MES.SP"))
				Message = Message.Replace("/MES.SP", "/MES.Spawn.Prefab");

			if (Message.StartsWith("/MES.SSP"))
				Message = Message.Replace("/MES.SSP", "/MES.Spawn.StationPrefab");

			if (Message.StartsWith("/MES.GTS"))
				Message = Message.Replace("/MES.GTS", "/MES.Info.GetThreatScore");

			if (Message.StartsWith("/MES.GESAP"))
				Message = Message.Replace("/MES.GESAP", "/MES.Info.GetEligibleSpawnsAtPosition");

		}

		private string[] GetArray(int length, int combineLength) {

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

			var array = GetArray(3, 4);

			if (array == null) {

				SpawnLogger.Write("Chat Array Size Too Small", SpawnerDebugEnum.Settings);
				ReturnMessage = "Chat Array Size Too Small";
				return false;

			}

			SpawnLogger.Write("Get Spawn Type From Chat", SpawnerDebugEnum.Settings);
			SpawningType type = SpawningType.None;

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

			var array = GetArray(3, 4);

			if (array == null) {

				SpawnLogger.Write("Array Size Too Small", SpawnerDebugEnum.Settings);
				return false;

			}

			//MES.Debug.ClearStaticEncounters
			if (array[2] == "ClearStaticEncounters") {

				foreach (var enc in NpcManager.StaticEncounters) {

					enc.IsValid = false;
				
				}

				NpcManager.UpdateStaticEncounters();
				Mode = ChatMsgMode.ReturnMessage;
				ReturnMessage = "Cleared All Active Static Encounters. Please Reload The World To Regenerate Static Encounters.";
				return true;

			}

			//MES.Debug.ClearUniqueEncounters
			if (array[2] == "ClearUniqueEncounters") {

				NpcManager.UniqueGroupsSpawned.Clear();
				NpcManager.UpdateStaticEncounters();
				Mode = ChatMsgMode.ReturnMessage;
				ReturnMessage = "Cleared All Previously Spawned Unique SpawnGroups.";
				return true;

			}

			//MES.Debug.UnlockAdminBlocks
			if (array[2] == "UnlockNpcBlocks") {

				Mode = ChatMsgMode.ReturnMessage;
				ReturnMessage = "NPC-Only Blocks Have Been Unlocked For This Session.";
				UnlockAdminBlocks = true;
				return true;

			}

			return false;

		}

		private bool ProcessInfo() {

			var array = GetArray(3, 4);

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

			//GetProfiles
			if (array[2] == "GetProfiles") {

				ClipboardPayload = ProfileManager.GetProfileData();
				Mode = ChatMsgMode.ReturnMessage;
				ReturnMessage = "Loaded Profiles Sent To Clipboard.";
				return true;

			}

			//GetRegisteredSpawnGroups
			if (array[2] == "GetRegisteredSpawnGroups") {

				ClipboardPayload = LoggerTools.BuildKeyList("SpawnGroup", SpawnGroupManager.SpawnGroupNames);
				Mode = ChatMsgMode.ReturnMessage;
				ReturnMessage = "Registered SpawnGroups Sent To Clipboard.";
				return true;

			}

			//GetGridMatrix
			if (array[2] == "GetGridMatrix") {

				var line = new LineD(this.CameraPosition, this.CameraDirection * 400 + this.CameraPosition);
				GridEntity thisGrid = null;

				foreach (var grid in GridManager.Grids) {

					if (!grid.ActiveEntity())
						continue;

					if (!grid.CubeGrid.WorldAABB.Intersects(ref line))
						continue;

					thisGrid = grid;
					break;

				}

				if (thisGrid == null) {

					ReturnMessage = "Could Not Locate Grid At Player Camera Position.";
					return false;

				}

				var sb = new StringBuilder();

				sb.Append("Grid Name:          ").Append(thisGrid.CubeGrid.CustomName).AppendLine();
				sb.Append("Matrix Position:    ").Append(thisGrid.CubeGrid.WorldMatrix.Translation).AppendLine();
				sb.Append("Matrix Forward:     ").Append(thisGrid.CubeGrid.WorldMatrix.Forward).AppendLine();
				sb.Append("Matrix Up:          ").Append(thisGrid.CubeGrid.WorldMatrix.Up).AppendLine().AppendLine();

				sb.Append("Tags For SpawnGroup:").AppendLine();
				sb.Append("[StaticEncounterCoords:{").Append(thisGrid.CubeGrid.WorldMatrix.Translation).Append("}]").AppendLine();
				sb.Append("[StaticEncounterForward:{").Append(thisGrid.CubeGrid.WorldMatrix.Forward).Append("}]").AppendLine();
				sb.Append("[StaticEncounterUp:{").Append(thisGrid.CubeGrid.WorldMatrix.Up).Append("}]");

				ClipboardPayload = sb.ToString();
				Mode = ChatMsgMode.ReturnMessage;
				ReturnMessage = "Grid Position Data Sent To Clipboard.";
				return true;

			}

			//GetPlayers
			if (array[2] == "GetPlayers") {

				var sb = new StringBuilder();

				foreach (var player in PlayerManager.Players) {

					player.ToString(sb);

				}

				ClipboardPayload = sb.ToString();
				Mode = ChatMsgMode.ReturnMessage;
				ReturnMessage = "Registered SpawnGroups Sent To Clipboard.";
				return true;

			}

			//GetEligibleSpawnsAtPosition
			if (array[2] == "GetEligibleSpawnsAtPosition") {

				//StringBuilder
				var sb = new StringBuilder();

				//Environment
				var environment = new EnvironmentEvaluation(this.PlayerPosition);
				var threatLevel = SpawnConditions.GetThreatLevel(5000, false, this.PlayerPosition);
				var pcuLevel = SpawnConditions.GetPCULevel(5000, this.PlayerPosition);
				SpawnGroupCollection collection = null;

				sb.Append("::: Spawn Data Near Player :::").AppendLine();
				sb.Append(" - Threat Score: ").Append(threatLevel.ToString()).AppendLine();
				sb.Append(" - PCU Score:    ").Append(pcuLevel.ToString()).AppendLine();

				sb.AppendLine();

				//Environment Data Near Player
				sb.Append("::: Environment Data Near Player :::").AppendLine();
				sb.Append(" - Distance From World Center:  ").Append(environment.DistanceFromWorldCenter.ToString()).AppendLine();
				sb.Append(" - Direction From World Center: ").Append(environment.DirectionFromWorldCenter.ToString()).AppendLine();
				sb.Append(" - Is On Planet:                ").Append(environment.IsOnPlanet.ToString()).AppendLine();
				sb.Append(" - Planet Name:                 ").Append(environment.IsOnPlanet ? environment.NearestPlanetName : "N/A").AppendLine();
				sb.Append(" - Planet Diameter:             ").Append(environment.IsOnPlanet ? environment.PlanetDiameter.ToString() : "N/A").AppendLine();
				sb.Append(" - Oxygen At Position:          ").Append(environment.IsOnPlanet ? environment.OxygenAtPosition.ToString() : "N/A").AppendLine();
				sb.Append(" - Atmosphere At Position:      ").Append(environment.IsOnPlanet ? environment.AtmosphereAtPosition.ToString() : "N/A").AppendLine();
				sb.Append(" - Gravity At Position:         ").Append(environment.IsOnPlanet ? environment.GravityAtPosition.ToString() : "N/A").AppendLine();
				sb.Append(" - Altitude At Position:        ").Append(environment.IsOnPlanet ? environment.AltitudeAtPosition.ToString() : "N/A").AppendLine();
				sb.Append(" - Is Night At Position:        ").Append(environment.IsOnPlanet ? environment.IsNight.ToString() : "N/A").AppendLine();
				sb.Append(" - Weather At Position:         ").Append(environment.IsOnPlanet && !string.IsNullOrWhiteSpace(environment.WeatherAtPosition) ? environment.WeatherAtPosition.ToString() : "N/A").AppendLine();
				sb.Append(" - Common Terrain At Position:  ").Append(environment.IsOnPlanet ? environment.CommonTerrainAtPosition.ToString() : "N/A").AppendLine();
				sb.Append(" - Water Mod Enabled:           ").Append(AddonManager.WaterMod).AppendLine();
				sb.Append(" - Planet Has Water:            ").Append(environment.IsOnPlanet ? environment.PlanetHasWater.ToString() : "N/A").AppendLine();
				sb.Append(" - Position Underwater:         ").Append(environment.IsOnPlanet ? environment.PositionIsUnderWater.ToString() : "N/A").AppendLine();
				sb.Append(" - Surface Underwater:          ").Append(environment.IsOnPlanet ? environment.SurfaceIsUnderWater.ToString() : "N/A").AppendLine();
				sb.Append(" - Air Travel Viability Ratio:  ").Append(environment.IsOnPlanet ? (Math.Round(environment.AirTravelViabilityRatio, 3)).ToString() : "N/A").AppendLine();
				sb.Append(" - Water Coverage Ratio:        ").Append(environment.IsOnPlanet ? (Math.Round(environment.WaterInSurroundingAreaRatio, 3)).ToString() : "N/A").AppendLine();

				sb.AppendLine();

				//Space Cargo
				collection = new SpawnGroupCollection();
				SpawnGroupManager.GetSpawnGroups(SpawningType.SpaceCargoShip, environment, "", collection);

				if (collection.SpawnGroups.Count > 0) {

					sb.Append("::: Space / Lunar Cargo Ship Eligible Spawns :::").AppendLine();

					foreach (var sgroup in collection.SpawnGroups.Distinct()) {

						sb.Append(" - ").Append(sgroup.SpawnGroupName).AppendLine();

					}

					sb.AppendLine();

				}

				//Random Encounter
				collection = new SpawnGroupCollection();
				SpawnGroupManager.GetSpawnGroups(SpawningType.RandomEncounter, environment, "", collection);

				if (collection.SpawnGroups.Count > 0) {

					sb.Append("::: Random Encounter Eligible Spawns :::").AppendLine();

					foreach (var sgroup in collection.SpawnGroups.Distinct()) {

						sb.Append(" - ").Append(sgroup.SpawnGroupName).AppendLine();

					}

					sb.AppendLine();

				}

				//Planetary Cargo
				collection = new SpawnGroupCollection();
				SpawnGroupManager.GetSpawnGroups(SpawningType.PlanetaryCargoShip, environment, "", collection);

				if (collection.SpawnGroups.Count > 0) {

					sb.Append("::: Planetary / Gravity Cargo Ship Eligible Spawns :::").AppendLine();

					foreach (var sgroup in collection.SpawnGroups.Distinct()) {

						sb.Append(" - ").Append(sgroup.SpawnGroupName).AppendLine();

					}

					sb.AppendLine();

				}

				//Planetary Installation
				collection = new SpawnGroupCollection();
				SpawnGroupManager.GetSpawnGroups(SpawningType.PlanetaryInstallation, environment, "", collection);

				if (collection.SpawnGroups.Count > 0) {

					sb.Append("::: Planetary Installation Eligible Spawns :::").AppendLine();

					foreach (var sgroup in collection.SpawnGroups.Distinct()) {

						sb.Append(" - ").Append(sgroup.SpawnGroupName).AppendLine();

					}

					sb.AppendLine();

				}

				//Boss
				collection = new SpawnGroupCollection();
				SpawnGroupManager.GetSpawnGroups(SpawningType.BossEncounter, environment, "", collection);

				if (collection.SpawnGroups.Count > 0) {

					sb.Append("::: Boss Encounter Eligible Spawns :::").AppendLine();

					foreach (var sgroup in collection.SpawnGroups.Distinct()) {

						sb.Append(" - ").Append(sgroup.SpawnGroupName).AppendLine();

					}

					sb.AppendLine();

				}

				//Creature
				collection = new SpawnGroupCollection();
				SpawnGroupManager.GetSpawnGroups(SpawningType.Creature, environment, "", collection);

				if (collection.SpawnGroups.Count > 0) {

					sb.Append("::: Creature / Bot Eligible Spawns :::").AppendLine();

					foreach (var sgroup in collection.SpawnGroups.Distinct()) {

						sb.Append(" - ").Append(sgroup.SpawnGroupName).AppendLine();

					}

					sb.AppendLine();

				}

				//StaticEncounters
				if (NpcManager.StaticEncounters.Count > 0) {

					sb.Append("::: Static Encounter Eligible Spawns :::").AppendLine();

					foreach (var enc in NpcManager.StaticEncounters) {

						sb.Append(" - ").Append(!string.IsNullOrWhiteSpace(enc?.ProfileSubtypeId) ? enc.ProfileSubtypeId : "(null)").AppendLine();

					}

					sb.AppendLine();

				}

				//UniqueEncounters
				if (NpcManager.UniqueGroupsSpawned.Count > 0) {

					sb.Append("::: Unique Encounters Already Spawned :::").AppendLine();

					foreach (var enc in NpcManager.UniqueGroupsSpawned) {

						sb.Append(" - ").Append(enc).AppendLine();

					}

					sb.AppendLine();

				}

				//Zones

				//Timeouts

				Mode = ChatMsgMode.ReturnMessage;
				ReturnMessage = "Eligible Spawns Sent To Clipboard.";
				ClipboardPayload = sb.ToString();
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
