using ModularEncountersSystems.API;
using ModularEncountersSystems.Behavior;
using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.Spawning.Procedural;
using ModularEncountersSystems.Sync;
using ModularEncountersSystems.Tasks;
using ModularEncountersSystems.Terminal;
using ModularEncountersSystems.Watchers;
using ModularEncountersSystems.World;
using ModularEncountersSystems.Zones;
using ModularEncountersSystems.Events;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
using VRageMath;
using ModularEncountersSystems.Spawning.Procedural.Hull;
using System.IO;
using ModularEncountersSystems.Spawning.Procedural.Builder;

namespace ModularEncountersSystems.Logging {

	public struct LoggerQueue {

		public string message;
		public SpawnerDebugEnum type;
		public bool forceSpawn;

		public LoggerQueue(string msg, SpawnerDebugEnum spawntype, bool force = false) {

			message = msg;
			type = spawntype;
			forceSpawn = force;
		
		}

	}

	public static class LoggerTools {

		public static void AppendDateAndTime(StringBuilder sb) {

			DateTime time = DateTime.Now;
			sb.Append(time.ToString("yyyy-MM-dd hh-mm-ss-fff")).Append(": ");

		}

		public static string GetDateAndTime() {

			DateTime time = DateTime.Now;
			return time.ToString("yyyy-MM-dd hh-mm-ss-fff");

		}

		public static string BuildKeyList(string profileType, IEnumerable<string> stringList) {

			var sb = new StringBuilder();
			sb.Append("Detected Profiles: " + profileType).AppendLine();

			foreach (var subtypeName in stringList.OrderBy(x => x)) {

				sb.Append(" - ").Append(subtypeName).AppendLine();

			}

			sb.AppendLine();
			return sb.ToString();

		}

		public static void ChangeBool(ChatMessage msg, string[] msgSplit) {

			if (msgSplit.Length < 5) {

				MyVisualScriptLogicProvider.ShowNotification("Invalid Command Received", 5000, "White", msg.PlayerId);
				return;

			}

			bool newBool = false;
			bool existingBool = false;

			bool boolValid = bool.TryParse(msgSplit[4], out newBool);
			bool existingGot = MyAPIGateway.Utilities.GetVariable(msgSplit[3], out existingBool);

			if (!boolValid) {

				MyVisualScriptLogicProvider.ShowNotification("Could not parse value to modify bool", 5000, "White", msg.PlayerId);
				return;

			}

			MyVisualScriptLogicProvider.ShowNotification("Value for Bool: " + msgSplit[3] + " ::: " + newBool, 5000, "White", msg.PlayerId);

			MyAPIGateway.Utilities.SetVariable(msgSplit[3], newBool);
			return;

		}

		public static void ChangeCounter(ChatMessage msg, string[] msgSplit) {

			if (msgSplit.Length < 5) {

				MyVisualScriptLogicProvider.ShowNotification("Invalid Command Received", 5000, "White", msg.PlayerId);
				return;

			}

			int existingAmount = 0;
			int newAmount = 0;

			bool amountGot = int.TryParse(msgSplit[4], out newAmount);
			bool existingGot = MyAPIGateway.Utilities.GetVariable(msgSplit[3], out existingAmount);

			if (!amountGot) {

				MyVisualScriptLogicProvider.ShowNotification("Could not parse amount to modify counter", 5000, "White", msg.PlayerId);
				return;

			}

			MyVisualScriptLogicProvider.ShowNotification("Value for Counter: " + msgSplit[3] + " ::: " + existingAmount, 5000, "White", msg.PlayerId);

			if (existingAmount + newAmount != existingAmount)
				MyVisualScriptLogicProvider.ShowNotification("New Value for Counter: " + msgSplit[3] + " ::: " + (existingAmount + newAmount), 5000, "White", msg.PlayerId);

			MyAPIGateway.Utilities.SetVariable(msgSplit[3], existingAmount + newAmount);
			return;
		
		}

		public static void ClearAllLogging(string type) {

			if (type == "SpawnDebug") {
			
				
			
			}

			if (type == "BehaviorDebug") {



			}

		}

		public static void ClearAllTimeouts(ChatMessage msg) {

			foreach (var zone in TimeoutManagement.Timeouts) {

				zone.Remove = true;

			}

			MyVisualScriptLogicProvider.ShowNotification("All Timeout Zones at Position Cleared.", 5000, "White", msg.PlayerId);
			return;

		}

		public static void ClearShipInventory(ChatMessage msg) {

			IMyPlayer thisPlayer = null;

			foreach (var player in PlayerManager.Players) {

				if (player.Player.IdentityId == msg.PlayerId) {

					thisPlayer = player.Player;
					break;

				}

			}

			if (thisPlayer == null) {

				MyVisualScriptLogicProvider.ShowNotification("Could Not Clear Inventory, No Player Detected", 5000, "White", msg.PlayerId);
				return;

			}

			var seat = thisPlayer.Controller?.ControlledEntity?.Entity as IMyShipController;

			if (seat == null) {

				MyVisualScriptLogicProvider.ShowNotification("Could Not Clear Inventory, Player Not in Seat", 5000, "White", msg.PlayerId);
				return;

			}

			var gts = MyAPIGateway.TerminalActionsHelper.GetTerminalSystemForGrid(seat.SlimBlock.CubeGrid);
			var blocks = new List<IMyTerminalBlock>();
			gts.GetBlocks(blocks);

			foreach (var block in blocks) {

				if (block.GetInventory() != null)
					block.GetInventory().Clear();

			}

		}

		public static void ClearTimeoutsAtPosition(ChatMessage msg) {

			foreach (var zone in TimeoutManagement.Timeouts) {

				if (zone.InsideRadius(msg.PlayerPosition))
					zone.Remove = true;

			}

			MyVisualScriptLogicProvider.ShowNotification("All Timeout Zones at Position Cleared.", 5000, "White", msg.PlayerId);
			return;

		}

		public static string CreateIdStorage(ChatMessage msg, string[] msgSplit) {

			var sb = new StringBuilder();

			for (int i = 0; i < 20; i++) {

				int a = MathTools.RandomBetween(0, 999999999);
				ulong b = msg.SteamId;
				ulong c = 0;
				int d = MathTools.RandomBetween(0, 999999999);

				if (!ulong.TryParse(msgSplit[3], out c)) {

					continue;

				}

				var list = new List<ulong> { (ulong)a, b, c, (ulong)d };
				sb.Append(SerializationHelper.ConvertClassToString<List<ulong>>(list)).AppendLine();

			}

			return sb.ToString();
		
		}

		public static void CreateKPL(ChatMessage msg, string[] msgSplit) {

			if (msgSplit.Length < 4) {

				MyVisualScriptLogicProvider.ShowNotification("Invalid Command Received", 5000, "White", msg.PlayerId);
				return;

			}

			// /MES.Debug.CreateKPL.Faction.Radius.Duration.Max.Min

			string faction = msgSplit[3];
			double radius = 10000;
			int duration = -1;
			int maxEncounters = -1;
			int minThreat = -1;

			if (msgSplit.Length >= 5)
				double.TryParse(msgSplit[4], out radius);

			if (msgSplit.Length >= 6)
				int.TryParse(msgSplit[5], out duration);

			if (msgSplit.Length >= 7)
				int.TryParse(msgSplit[6], out maxEncounters);

			if (msgSplit.Length >= 8)
				int.TryParse(msgSplit[7], out minThreat);

			KnownPlayerLocationManager.AddKnownPlayerLocation(msg.PlayerPosition, faction, radius, duration, maxEncounters, minThreat);
			return;

		}

		public static void DebugAttachShipyardProfile(ChatMessage msg) {

			var grid = GridManager.GetClosestGridInDirection(MatrixD.CreateWorld(msg.CameraPosition, msg.CameraDirection, VectorHelper.RandomPerpendicular(msg.CameraDirection)), 10000);

			if (grid == null) {

				msg.ReturnMessage = "No Grid in Camera Direction";
				return;

			}

			foreach (var projector in grid.Projectors) {

				if (!projector.ActiveEntity())
					continue;

				if (projector.Block.Storage == null)
					projector.Block.Storage = new MyModStorageComponent();

				projector.Block.Storage.Add(StorageTools.MesShipyardKey, "MES-Shipyard-TestProfile");

			}

			msg.ReturnMessage = "Shipyard Profiles Attached To Eligible Blocks";
			return;

		}

		public static void DebugAttachSuitUpgradeModule(ChatMessage msg) {

			var grid = GridManager.GetClosestGridInDirection(MatrixD.CreateWorld(msg.CameraPosition, msg.CameraDirection, VectorHelper.RandomPerpendicular(msg.CameraDirection)), 10000);

			if (grid == null) {

				msg.ReturnMessage = "No Grid in Camera Direction";
				return;

			}

			foreach (var block in grid.AllTerminalBlocks) {

				if (!block.ActiveEntity() || ControlManager.SuitUpgradeBlockId != block.Block.SlimBlock.BlockDefinition.Id)
					continue;

				if (block.Block.Storage == null)
					block.Block.Storage = new MyModStorageComponent();

				block.Block.Storage.Add(StorageTools.MesSuitModsKey, "1");

			}

			msg.ReturnMessage = "Suit Upgrade Module Attached To Eligible Blocks";
			return;

		}

		public static void DebugAutoPilot(ChatMessage msg) {

			var line = new LineD(msg.CameraPosition, msg.CameraDirection * 10000 + msg.CameraPosition);
			GridEntity thisGrid = null;

			foreach (var grid in GridManager.Grids) {

				if (!grid.ActiveEntity())
					continue;

				if (!grid.CubeGrid.WorldAABB.Intersects(ref line))
					continue;

				if (grid.Behavior == null) {

					foreach (var behavior in BehaviorManager.Behaviors) {

						if (behavior?.RemoteControl?.SlimBlock?.CubeGrid != null && grid.CubeGrid == behavior.RemoteControl.SlimBlock.CubeGrid) {

							grid.Behavior = behavior;
							break;

						}

					}

					if (grid.Behavior == null) {

						foreach (var behavior in BehaviorManager.TerminatedBehaviors) {

							if (behavior?.RemoteControl?.SlimBlock?.CubeGrid != null && grid.CubeGrid == behavior.RemoteControl.SlimBlock.CubeGrid) {

								grid.Behavior = behavior;
								break;

							}

						}

					}

					if (grid.Behavior == null) {

						continue;

					}


					//message.ReturnMessage = string.Format("[{0}] Does Not Have an Active Behavior.", thisGrid.CubeGrid.CustomName);
					//return "";

				}

				thisGrid = grid;
				break;

			}

			if (thisGrid == null) {

				msg.ReturnMessage = "Could Not Locate NPC Grid At Player Camera Position. Point Camera Cursor At Target Within 10KM. Check Clipboard For Results.";
				return;

			}

			msg.ReturnMessage = "Autopilot Debug Activated";
			//thisGrid.Behavior.AutoPilot.Debug = true;
			return;

		}

		public static void DebugBlockPairs(ChatMessage msg) {

			var sbsmall = new StringBuilder();
			sbsmall.Append("::: Small Blocks :::").AppendLine().AppendLine();
			var sblarge = new StringBuilder();
			sblarge.Append("::: Large Blocks :::").AppendLine().AppendLine();

			foreach (var block in DefinitionHelper.AllBlockDefinitions) {

				if (string.IsNullOrWhiteSpace(block.BlockPairName))
					continue;

				StringBuilder sb = null;

				if (block.CubeSize == VRage.Game.MyCubeSize.Small)
					sb = sbsmall;
				else
					sb = sblarge;

				sb.Append("[OldBlock:").Append(block.Id.ToString()).Append("]").AppendLine();
				sb.Append("[NewBlock:").Append(new MyDefinitionId(block.Id.TypeId, block.BlockPairName)).Append("]").AppendLine().AppendLine();

			}

			sbsmall.Append(sblarge.ToString());
			msg.ClipboardPayload = sbsmall.ToString();
			msg.Mode = ChatMsgMode.ReturnMessage;
			msg.ReturnMessage = "Block Pair Data Copied To Clipboard";

		}

		public static void DebugClearThrust(ChatMessage msg) {

			var line = new LineD(msg.CameraPosition, msg.CameraDirection * 10000 + msg.CameraPosition);
			GridEntity thisGrid = null;

			foreach (var grid in GridManager.Grids) {

				if (!grid.ActiveEntity())
					continue;

				if (!grid.CubeGrid.WorldAABB.Intersects(ref line))
					continue;

				if (grid.Behavior == null) {

					foreach (var behavior in BehaviorManager.Behaviors) {

						if (behavior?.RemoteControl?.SlimBlock?.CubeGrid != null && grid.CubeGrid == behavior.RemoteControl.SlimBlock.CubeGrid) {

							grid.Behavior = behavior;
							break;

						}

					}

					if (grid.Behavior == null) {

						foreach (var behavior in BehaviorManager.TerminatedBehaviors) {

							if (behavior?.RemoteControl?.SlimBlock?.CubeGrid != null && grid.CubeGrid == behavior.RemoteControl.SlimBlock.CubeGrid) {

								grid.Behavior = behavior;
								break;

							}

						}

					}

					if (grid.Behavior == null) {

						continue;

					}


					//message.ReturnMessage = string.Format("[{0}] Does Not Have an Active Behavior.", thisGrid.CubeGrid.CustomName);
					//return "";

				}

				thisGrid = grid;
				break;

			}

			if (thisGrid == null) {

				msg.ReturnMessage = "Could Not Locate NPC Grid At Player Camera Position. Point Camera Cursor At Target Within 10KM. Check Clipboard For Results.";
				return;

			}

			msg.ReturnMessage = "Cleared Thrust";
			thisGrid.Behavior.AutoPilot.StopAllThrust();
			return;

		}

		public static void DebugLinkedGrids(ChatMessage msg) {

			var grid = GridManager.GetClosestGridInDirection(MatrixD.CreateWorld(msg.CameraPosition, msg.CameraDirection, VectorHelper.RandomPerpendicular(msg.CameraDirection)), 10000);

			if (grid == null || grid.ActiveEntity() == false) {

				msg.ReturnMessage = "No Grid in Camera Direction";
				return;

			}

			var sb = new StringBuilder();
			sb.Append("Primary Grid: ").Append(grid?.CubeGrid?.CustomName ?? "null").AppendLine().AppendLine();
			var gridGroupList = new List<IMyCubeGrid>();

			//Electrical
			sb.Append("Electrical").AppendLine();
			gridGroupList.Clear();
			MyAPIGateway.GridGroups.GetGroup(grid.CubeGrid, GridLinkTypeEnum.Electrical, gridGroupList);

			foreach (var cg in gridGroupList) {

				sb.Append(" - ").Append(cg?.CustomName ?? "null").AppendLine();

			}

			sb.AppendLine();

			//Logical
			sb.Append("Logical").AppendLine();
			gridGroupList.Clear();
			MyAPIGateway.GridGroups.GetGroup(grid.CubeGrid, GridLinkTypeEnum.Logical, gridGroupList);

			foreach (var cg in gridGroupList) {

				sb.Append(" - ").Append(cg?.CustomName ?? "null").AppendLine();

			}

			sb.AppendLine();

			//Mechanical
			sb.Append("Mechanical").AppendLine();
			gridGroupList.Clear();
			MyAPIGateway.GridGroups.GetGroup(grid.CubeGrid, GridLinkTypeEnum.Mechanical, gridGroupList);

			foreach (var cg in gridGroupList) {

				sb.Append(" - ").Append(cg?.CustomName ?? "null").AppendLine();

			}

			sb.AppendLine();

			//NoContactDamage
			sb.Append("NoContactDamage").AppendLine();
			gridGroupList.Clear();
			MyAPIGateway.GridGroups.GetGroup(grid.CubeGrid, GridLinkTypeEnum.NoContactDamage, gridGroupList);

			foreach (var cg in gridGroupList) {

				sb.Append(" - ").Append(cg?.CustomName ?? "null").AppendLine();

			}

			sb.AppendLine();

			//Physical
			sb.Append("Physical").AppendLine();
			gridGroupList.Clear();
			MyAPIGateway.GridGroups.GetGroup(grid.CubeGrid, GridLinkTypeEnum.Physical, gridGroupList);

			foreach (var cg in gridGroupList) {

				sb.Append(" - ").Append(cg?.CustomName ?? "null").AppendLine();

			}

			sb.AppendLine();

			msg.ReturnMessage = "Processed Linked Grids. Clipboard Updated";
			msg.Mode = ChatMsgMode.ReturnMessage;
			msg.ClipboardPayload = sb.ToString();

		}

		public static void DebugRotateBlockYaw(ChatMessage msg) {

			var matrix = MatrixD.CreateWorld(msg.CameraPosition, msg.CameraDirection, VectorHelper.RandomPerpendicular(msg.CameraDirection));
			var block = GridManager.GetClosestBlockInDirection(matrix, 10000);

			if (block == null) {

				msg.ReturnMessage = "No Grid / Block in Camera Direction";
				return;

			}

			var ob = block.GetObjectBuilder(true);
			var grid = block.CubeGrid;
			grid.RemoveBlock(block);
			var orientation = BuilderTools.RotateYaw(ob.BlockOrientation);
			ob.BlockOrientation = orientation;
			ob.EntityId = 0;
			grid.AddBlock(ob, true);
		
		}

		public static void DebugRotateBlockPitch(ChatMessage msg) {

			var matrix = MatrixD.CreateWorld(msg.CameraPosition, msg.CameraDirection, VectorHelper.RandomPerpendicular(msg.CameraDirection));
			var block = GridManager.GetClosestBlockInDirection(matrix, 10000);

			if (block == null) {

				msg.ReturnMessage = "No Grid / Block in Camera Direction";
				return;

			}


			MyVisualScriptLogicProvider.ShowNotification("F: " + block.Orientation.Forward, 5000, "White", msg.PlayerId);
			MyVisualScriptLogicProvider.ShowNotification("U: " + block.Orientation.Up, 5000, "White", msg.PlayerId);
			MyVisualScriptLogicProvider.ShowNotification("L: " + block.Orientation.Left, 5000, "White", msg.PlayerId);
			var ob = block.GetObjectBuilder(true);
			var grid = block.CubeGrid;
			grid.RemoveBlock(block);
			var orientation = BuilderTools.RotatePitch(ob.BlockOrientation);
			ob.BlockOrientation = orientation;
			ob.EntityId = 0;
			grid.AddBlock(ob, true);

		}

		public static void DebugRotateBlockRoll(ChatMessage msg) {

			var matrix = MatrixD.CreateWorld(msg.CameraPosition, msg.CameraDirection, VectorHelper.RandomPerpendicular(msg.CameraDirection));
			var block = GridManager.GetClosestBlockInDirection(matrix, 10000);

			if (block == null) {

				msg.ReturnMessage = "No Grid / Block in Camera Direction";
				return;

			}


			MyVisualScriptLogicProvider.ShowNotification("F: " + block.Orientation.Forward, 5000, "White", msg.PlayerId);
			MyVisualScriptLogicProvider.ShowNotification("U: " + block.Orientation.Up, 5000, "White", msg.PlayerId);
			MyVisualScriptLogicProvider.ShowNotification("L: " + block.Orientation.Left, 5000, "White", msg.PlayerId);
			var ob = block.GetObjectBuilder(true);
			var grid = block.CubeGrid;
			grid.RemoveBlock(block);
			var orientation = BuilderTools.RotateRoll(ob.BlockOrientation);
			ob.BlockOrientation = orientation;
			ob.EntityId = 0;
			grid.AddBlock(ob, true);

		}

		public static void DebugBlockOrientation(ChatMessage msg) {

			var matrix = MatrixD.CreateWorld(msg.CameraPosition, msg.CameraDirection, VectorHelper.RandomPerpendicular(msg.CameraDirection));
			var block = GridManager.GetClosestBlockInDirection(matrix, 10000);

			if (block == null) {

				msg.ReturnMessage = "No Grid / Block in Camera Direction";
				return;

			}


			MyVisualScriptLogicProvider.ShowNotification(block.BlockDefinition.Id.ToString() + " - " + block.Min.ToString(), 5000, "White", msg.PlayerId);

		}

		public static void DebugSpawnAllSingleBlocks(ChatMessage msg) {

			TaskProcessor.Tasks.Add(new DebugSpawnSingleBlocks(MatrixD.CreateWorld(msg.PlayerPosition, Vector3D.Forward, Vector3D.Up)));
		
		}

		public static void DebugSpawnConstruct(ChatMessage msg) {

			if (!MES_SessionCore.Developers.Contains(msg.SteamId)) {

				msg.ReturnMessage = "Feature Not Available";
				return;
			
			}

			var matrix = MatrixD.CreateWorld((msg.CameraDirection * 400 + msg.CameraPosition), msg.CameraDirection, VectorHelper.RandomPerpendicular(msg.CameraDirection));
			var hull = new HullTypeSomerset(new ShipRules());

			

			for (int i = 0; i < 4; i++) {

				var thisMatrix = MatrixD.CreateWorld((matrix.Right * (100 * i) + matrix.Translation), matrix.Forward, matrix.Up);
				hull.Construct.CubeGrid.PositionAndOrientation = new VRage.MyPositionAndOrientation(new Vector3D((100 * i), 0, 0), Vector3.Forward, Vector3.Up);
				hull.ProcessStep(i);
				string prefabId = "MES-Prefab-ProceduralPrefabDebug-" + i.ToString();
				hull.SpawnCurrentConstruct(thisMatrix, prefabId);

			}

			msg.ClipboardPayload = hull.Construct.Log.ToString();
			return;

		}

		public static void DebugTestSaveGrid(ChatMessage msg) {

			var grid = GridManager.GetClosestGridInDirection(MatrixD.CreateWorld(msg.CameraPosition, msg.CameraDirection, VectorHelper.RandomPerpendicular(msg.CameraDirection)), 10000);

			if (grid == null) {

				msg.ReturnMessage = "No Grid in Camera Direction";
				return;

			}

			var ob = grid.CubeGrid.GetObjectBuilder() as MyObjectBuilder_CubeGrid;
			
			try {

				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("CubeGrid.mes", typeof(MyObjectBuilder_CubeGrid))) {

					writer.Write(MyAPIGateway.Utilities.SerializeToBinary<MyObjectBuilder_CubeGrid>(ob));

				}

			} catch (Exception exc) {

				msg.ReturnMessage = "Saved Grid As File";

			}

			msg.ReturnMessage = "Saved Grid As File";
			//grid.RefreshSubGrids();

		}

		public static void DebugTestSpawnGrid() {

			var grid = new MyObjectBuilder_CubeGrid();

		
		}

		public static void DeleteGrid(ChatMessage message) {

			var line = new LineD(message.CameraPosition, message.CameraDirection * 10000 + message.CameraPosition);
			GridEntity thisGrid = null;

			var sb = new StringBuilder();

			foreach (var grid in GridManager.Grids) {

				if (!grid.ActiveEntity())
					continue;

				if (!grid.CubeGrid.WorldAABB.Intersects(ref line))
					continue;

				thisGrid = grid;
				break;

			}

			if (thisGrid == null) {

				message.ReturnMessage = "Could Not Locate Grid At Player Camera Position. Point Camera Cursor At Target Within 10KM. Check Clipboard For Results.";
				return;

			}

			thisGrid.DespawnSource = "Despawn-Debug";
			Cleaning.RemoveGrid(thisGrid);
			message.ReturnMessage = "Grid Marked for Removal. " + thisGrid.CubeGrid.CustomName ?? "null";
			return;

		}

		public static void ForceSpawnTimer(ChatMessage msg, string[] array) {

			if (array.Length < 4 || string.IsNullOrWhiteSpace(array[3])) {

				msg.ReturnMessage = "Command For Force Spawn Timer Not Entered Correctly";
				return;

			}
				

			var player = PlayerSpawnWatcher.GetWatchedPlayer(msg.PlayerId);

			if (player == null) {

				msg.ReturnMessage = "Command For Force Spawn Timer Could Not Find Associated Player";
				return;

			}

			PlayerSpawnWatcher.Timer = Settings.General.PlayerWatcherTimerTrigger;

			msg.ReturnMessage = "Attempting To Force Spawn Timer For Type: " + array[3];

			if (array[3] == "SpaceCargoShip") {

				
				player.SpaceCargoShipTimer = 0;
				return;
			
			}

			if (array[3] == "PlanetaryCargoShip") {

				player.AtmoCargoShipTimer = 0;
				return;

			}

			if (array[3] == "Creature") {

				player.CreatureCheckTimer = 0;
				return;

			}

			if (array[3] == "RandomEncounter") {

				player.RandomEncounterCheckTimer = 0;
				player.RandomEncounterCoolDownTimer = 0;
				player.RandomEncounterDistanceCoordCheck = Vector3D.Forward * (Settings.RandomEncounters.PlayerTravelDistance * 20) + msg.PlayerPosition;
				return;

			}

			if (array[3] == "PlanetaryInstallation") {

				player.PlanetaryInstallationCheckTimer = 0;
				player.PlanetaryInstallationCooldownTimer = 0;
				var planet = PlanetManager.GetNearestPlanet(msg.PlayerPosition);

				if (planet != null) {

					var up = planet.UpAtPosition(msg.PlayerPosition);
					player.InstallationDistanceCoordCheck = VectorHelper.RandomPerpendicular(up) * (Settings.PlanetaryInstallations.PlayerDistanceSpawnTrigger * 1.5) + msg.PlayerPosition;

				}
				
				return;

			}

			if (array[3] == "BossEncounter") {

				player.BossEncounterCheckTimer = 0;
				player.BossEncounterCooldownTimer = 0;
				player.BossEncounterActive = false;
				return;

			}

			if (array[3] == "DroneEncounter") {

				player.DroneEncounterTimer = 0;
				player.DroneEncounterTimerCooldownTimer = 0;
				return;

			}

			msg.ReturnMessage = "Force Spawn Timer Failed For Unknown Type: " + array[3];

		}

		public static string GetAllProfiles() {

			var sb = new StringBuilder();

			sb.Append(BuildKeyList("SpawnGroup", SpawnGroupManager.SpawnGroupNames));
			sb.Append(BuildKeyList("Manipulation", ProfileManager.ManipulationProfiles.Keys));
			sb.Append(BuildKeyList("Manipulation Group", ProfileManager.ManipulationGroups.Keys));
			sb.Append(BuildKeyList("Block Replacement", ProfileManager.BlockReplacementProfiles.Keys));
			sb.Append(BuildKeyList("Dereliction", ProfileManager.DerelictionProfiles.Keys));
			sb.Append(BuildKeyList("Spawn Condition", ProfileManager.SpawnConditionProfiles.Keys));
			sb.Append(BuildKeyList("Spawn Condition Group", ProfileManager.SpawnConditionGroups.Keys));
			sb.Append(BuildKeyList("Zone Condition", ProfileManager.ZoneConditionsProfiles.Keys));
			sb.Append(BuildKeyList("Replenishment", ProfileManager.ReplenishmentProfiles.Keys));
			sb.Append(BuildKeyList("Zone", ProfileManager.ZoneProfiles.Keys));
			sb.Append(BuildKeyList("Behavior", ProfileManager.BehaviorTemplates.Keys));
			sb.Append(BuildKeyList("Action", ProfileManager.ActionObjectTemplates.Keys));
			sb.Append(BuildKeyList("AutoPilot", ProfileManager.AutoPilotProfiles.Keys));
			sb.Append(BuildKeyList("Chat", ProfileManager.ChatObjectTemplates.Keys));
			sb.Append(BuildKeyList("Command", ProfileManager.CommandProfiles.Keys));
			sb.Append(BuildKeyList("Condition", ProfileManager.ConditionObjectTemplates.Keys));
			sb.Append(BuildKeyList("Datapad", ProfileManager.DatapadTemplates.Keys));
			sb.Append(BuildKeyList("Spawner", ProfileManager.SpawnerObjectTemplates.Keys));
			sb.Append(BuildKeyList("Target", ProfileManager.TargetObjectTemplates.Keys));
			sb.Append(BuildKeyList("TextTemplate", ProfileManager.TextTemplates.Keys));
			sb.Append(BuildKeyList("Trigger", ProfileManager.TriggerObjectTemplates.Keys));
			sb.Append(BuildKeyList("Trigger Group", ProfileManager.TriggerGroupObjectTemplates.Keys));
			sb.Append(BuildKeyList("Waypoint", ProfileManager.WaypointProfiles.Keys));
			sb.Append(BuildKeyList("Loot", ProfileManager.LootProfiles.Keys));
			sb.Append(BuildKeyList("Loot Group", ProfileManager.LootGroups.Keys));
			sb.Append(BuildKeyList("Error", ProfileManager.ErrorProfiles));

			if (ProfileManager.StoreItemContainers.Count > 0) {

				sb.Append("Detected Profiles: Store Item Containers").AppendLine();

				

				foreach (var file in ProfileManager.StoreItemContainers.Keys) {

					sb.Append(" - ").Append(file).AppendLine();
					var container = ProfileManager.StoreItemContainers[file];

					if (container == null || container.StoreItemsList.Count == 0)
						continue;

					foreach (var item in container.StoreItemsList) {

						sb.Append("   - ").Append((item.StoreItemId ?? "null") + " / " + item.ItemType.ToString()).AppendLine();

					}

				}

				sb.AppendLine();

			}

			return sb.ToString();
		
		}

		public static string GetBlockMassData(ChatMessage message) {

			var sb = new StringBuilder();

			foreach (var item in DefinitionHelper.BlockWeightReference.Keys) {

				sb.Append(item.ToString()).AppendLine();
				sb.Append(DefinitionHelper.BlockWeightReference[item]).AppendLine();
				sb.AppendLine();

			}

			return sb.ToString();

		}

		public static string GetColorListFromGrid(ChatMessage msg) {

			var playerEnt = PlayerManager.GetPlayerWithIdentityId(msg.PlayerId);
			var player = playerEnt?.Player;

			var sb = new StringBuilder();

			if (player == null || player?.Character == null) {

				sb.Append("Player Does Not Exist On Server. Congrats, you've reached a state I thought not possible!");
				return sb.ToString();

			}

			if (player?.Controller?.ControlledEntity?.Entity == null) {

				MyVisualScriptLogicProvider.ShowNotification("Player Must Be Seated On Grid To Get Color List.", 5000, "White", player.IdentityId);
				sb.Append(" ");
				return sb.ToString();

			}

			var seat = player.Controller.ControlledEntity.Entity as IMyShipController;

			if (seat == null) {

				MyVisualScriptLogicProvider.ShowNotification("Player Using Invalid Ship Controller / Seat.", 5000, "White", player.IdentityId);
				sb.Append(" ");
				return sb.ToString();

			}

			var blockList = new List<IMySlimBlock>();
			seat.SlimBlock.CubeGrid.GetBlocks(blockList);
			var colorDict = new Dictionary<Vector3, int>();

			foreach (var block in blockList) {

				if (colorDict.ContainsKey(block.ColorMaskHSV) == true) {

					colorDict[block.ColorMaskHSV]++;

				} else {

					colorDict.Add(block.ColorMaskHSV, 1);

				}

			}

			foreach (var color in colorDict.Keys) {

				sb.Append("ColorMaskHSV:      ").Append(color.ToString()).AppendLine();
				sb.Append("Blocks With Color: ").Append(colorDict[color].ToString()).AppendLine().AppendLine();

			}

			MyVisualScriptLogicProvider.ShowNotification("Grid Color List Sent To Clipboard", 5000, "White", player.IdentityId);
			return sb.ToString();

		}

		public static string GetDiagnostics(ChatMessage msg) {

			var sb = new StringBuilder();

			bool modPathError = !MES_SessionCore.Instance.ModContext.ModPath.StartsWith(Path.GetFullPath(MES_SessionCore.Instance.ModContext.ModPath));
			sb.Append("::: MES Diagnostics :::").AppendLine();
			sb.Append(" - Mod Version:    ").Append(MES_SessionCore.ModVersion).AppendLine();
			sb.Append(" - Mod Path:       ").Append(MES_SessionCore.Instance.ModContext?.ModPath ?? "null").AppendLine();
			sb.Append(" - Mod Item Path:  ").Append(MES_SessionCore.Instance.ModContext?.ModItem.GetPath() ?? "null").AppendLine();
			sb.Append(" - Mod Path Error: ").Append(modPathError).AppendLine();

			if(MES_SessionCore.SyncWarning)
				sb.Append(" - Low Sync Distance and Selective Physics Updates: ").Append(MES_SessionCore.SyncWarning).AppendLine();

			if (modPathError) {

				sb.Append(" - WARNING: Server Session using -path parameter incorrectly when starting server. Provided directory contain trailing \\ or / character which can interfere with mods ability to read certain files.").AppendLine();
				SpawnLogger.Write("Server Session using -path parameter incorrectly when starting server. Provided directory contain trailing \\ or / character which can interfere with mods ability to read certain files.", SpawnerDebugEnum.Error, true);

			}
				

			sb.AppendLine();

			//NPC Mods
			if (SpawnGroupManager.UniqueNpcModNames.Keys.Count > 0) {

				sb.Append("::: NPC Content Mods :::").AppendLine();

				foreach (var mod in SpawnGroupManager.UniqueNpcModNames.Keys.OrderBy(x => x)) {

					sb.Append(" - ").Append(SpawnGroupManager.UniqueNpcModNames[mod]).Append(" [").Append(mod).Append("]").AppendLine();

				}

				sb.AppendLine();

			}

			//Settings
			sb.Append("::: Settings :::").AppendLine();
			var settings = MyAPIGateway.Session.SessionSettings;
			sb.Append(" - Dedicated Server:             ").Append(MyAPIGateway.Utilities.IsDedicated).AppendLine();
			sb.Append(" - Sync Distance:                ").Append(settings.SyncDistance).AppendLine();
			sb.Append(" - Selective Physics Updates:    ").Append(settings.EnableSelectivePhysicsUpdates).AppendLine();
			sb.Append(" - Block Limits Enabled:         ").Append(settings.BlockLimitsEnabled).AppendLine();
			sb.Append(" - NPC PCU Limit:                ").Append(settings.PiratePCU).AppendLine();
			sb.Append(" - Cargo Ships Enabled:          ").Append(settings.CargoShipsEnabled).AppendLine();
			sb.Append(" - Random Encounters Enabled:    ").Append(settings.EnableEncounters).AppendLine();
			sb.Append(" - Wolves Enabled:               ").Append(settings.EnableWolfs).AppendLine();
			sb.Append(" - Spiders Enabled:              ").Append(settings.EnableSpiders).AppendLine();
			sb.Append(" - Drones Enabled:               ").Append(settings.EnableDrones).AppendLine();
			sb.Append(" - Max Drones:                   ").Append(settings.MaxDrones).AppendLine();
			sb.Append(" - Economy Enabled:              ").Append(settings.EnableEconomy).AppendLine();
			sb.Append(" - Survival Mode:                ").Append(MyAPIGateway.Session.SurvivalMode).AppendLine();
			sb.Append(" - Ingame Scripting Enabled:     ").Append(settings.EnableIngameScripts).AppendLine();
			sb.Append(" - Unsupported Stations Enabled: ").Append(settings.StationVoxelSupport).AppendLine();

			sb.AppendLine();

			//APIs
			sb.Append("::: Enabled APIs :::").AppendLine();
			sb.Append(" - AI Enabled:                 ").Append(APIs.AiEnabledApiLoaded).AppendLine();
			sb.Append(" - Defense Shields:            ").Append(APIs.ShieldsApiLoaded).AppendLine();
			sb.Append(" - Water Mod:                  ").Append(APIs.WaterModApiLoaded).AppendLine();
			sb.Append(" - Weapon Core / Core Systems: ").Append(APIs.WeaponCoreApiLoaded).AppendLine();
			sb.AppendLine();

			//Presets Used
			sb.Append("::: Settings Preset Mod(s) Previously Used :::").AppendLine();
			bool chaoticUsed = false;
			MyAPIGateway.Utilities.GetVariable<bool>("MES-ChaoticSpawningSettings-UsedInWorld", out chaoticUsed);
			sb.Append(" - Chaotic Spawning Settings:                 ").Append(chaoticUsed).AppendLine();
			sb.AppendLine();

			//Spawners Active
			sb.Append("::: Spawners Active :::").AppendLine();
			sb.Append(" - Space Cargo Ships:          ").Append(Settings.SpaceCargoShips.EnableSpawns).AppendLine();
			sb.Append(" - Random Encounters:          ").Append(Settings.RandomEncounters.EnableSpawns).AppendLine();
			sb.Append(" - Planetary Cargo Ships:      ").Append(Settings.PlanetaryCargoShips.EnableSpawns).AppendLine();
			sb.Append(" - Planetary Installations:    ").Append(Settings.PlanetaryInstallations.EnableSpawns).AppendLine();
			sb.Append(" - Boss Encounters:            ").Append(Settings.BossEncounters.EnableSpawns).AppendLine();
			sb.Append(" - Drone Encounters:           ").Append(Settings.DroneEncounters.EnableSpawns).AppendLine();
			sb.Append(" - Creatures:                  ").Append(Settings.Creatures.EnableSpawns).AppendLine();
			sb.AppendLine();

			//Cleanup Settings
			sb.Append("::: Cleanup Settings Enabled :::").AppendLine();
			sb.Append(" - Space Cargo Ships:          ").Append(Settings.SpaceCargoShips.UseCleanupSettings).AppendLine();
			sb.Append(" - Random Encounters:          ").Append(Settings.RandomEncounters.UseCleanupSettings).AppendLine();
			sb.Append(" - Planetary Cargo Ships:      ").Append(Settings.PlanetaryCargoShips.UseCleanupSettings).AppendLine();
			sb.Append(" - Planetary Installations:    ").Append(Settings.PlanetaryInstallations.UseCleanupSettings).AppendLine();
			sb.Append(" - Boss Encounters:            ").Append(Settings.BossEncounters.UseCleanupSettings).AppendLine();
			sb.Append(" - Drone Encounters:           ").Append(Settings.DroneEncounters.UseCleanupSettings).AppendLine();
			sb.Append(" - Other NPCs:                 ").Append(Settings.OtherNPCs.UseCleanupSettings).AppendLine();
			sb.AppendLine();

			//GESAP
			sb.Append(GetEligibleSpawnsAtPosition(msg));

			//Wave Spawner Data
			sb.Append("::: Space Wave Spawner Data :::").AppendLine();
			sb.Append(WaveManager.Space.ToString()).AppendLine();

			sb.Append("::: Planetary Wave Spawner Data :::").AppendLine();
			sb.Append(WaveManager.Planet.ToString()).AppendLine();

			sb.Append("::: Creature Wave Spawner Data :::").AppendLine();
			sb.Append(WaveManager.Creature.ToString()).AppendLine();

			//Player Data
			sb.Append("::: Player Data :::").AppendLine();
			foreach (var player in PlayerManager.Players) {

				if (player != null)
					player.GetPlayerInfo(sb);

			}

			//Faction Data
			sb.Append("::: Faction Data :::").AppendLine();
			var factions = MyAPIGateway.Session.Factions.Factions;
			var identites = new List<IMyIdentity>();
			MyAPIGateway.Players.GetAllIdentites(identites);

			foreach (var factionId in factions.Keys) {

				var faction = factions[factionId];

				if (faction == null)
					continue;

				if (faction.Tag.Length < 4)
					continue;

				var canUse = FactionHelper.GetFactionMemberIdFromTag(faction.Tag);

				sb.Append(faction.Tag).AppendLine();
				sb.Append(" - Faction ID:          ").Append(faction.FactionId).AppendLine();
				sb.Append(" - Accept Humans:       ").Append(faction.AcceptHumans).AppendLine();
				sb.Append(" - Auto Accept Peace:   ").Append(faction.AutoAcceptPeace).AppendLine();
				sb.Append(" - Auto Accept Members: ").Append(faction.AutoAcceptMember).AppendLine();
				sb.Append(" - Is Everyone Npc:     ").Append(faction.IsEveryoneNpc()).AppendLine();
				sb.Append(" - MES Can Use:         ").Append(canUse > 0).AppendLine();
				sb.Append(" - Members:             ").Append(faction.Members.Count).AppendLine();

				foreach (var memberId in faction.Members.Keys) {

					var member = faction.Members[memberId];
					IMyIdentity identity = null;

					foreach (var id in identites)
						if (id.IdentityId == member.PlayerId) {
							identity = id;
							break;
						}

					sb.Append("   - Member: ").Append(member.PlayerId).AppendLine();
					sb.Append("     - Identity ID:   ").Append(member.PlayerId).AppendLine();
					sb.Append("     - Rank:          ").Append(member.IsFounder ? "Founder" : member.IsLeader ? "Leader" : "Member").AppendLine();

					if (identity == null) {

						sb.AppendLine();
						continue;
					
					}

					sb.Append("     - Name:          ").Append(string.IsNullOrWhiteSpace(identity.DisplayName) ? "(Empty)" : identity.DisplayName).AppendLine();
					sb.Append("     - Steam ID:      ").Append(MyAPIGateway.Players.TryGetSteamId(member.PlayerId)).AppendLine();
					sb.AppendLine();

				}
				

			}

			//All Mods
			if (AddonManager.ModIdNameReferences.Keys.Count > 0) {

				sb.Append("::: All Mods :::").AppendLine();

				foreach (var mod in AddonManager.ModIdNameReferences.Keys.OrderBy(x => x)) {

					sb.Append(" - ").Append(AddonManager.ModIdNameReferences[mod]).Append(" [").Append(mod).Append("]").AppendLine();

				}

				sb.AppendLine();

			}

			//Plugins
			if (MyAPIGateway.Utilities.ConfigDedicated?.Plugins != null && MyAPIGateway.Utilities.ConfigDedicated.Plugins.Count > 0) {

				sb.Append("::: All Server Plugins :::").AppendLine();

				foreach (var plugin in MyAPIGateway.Utilities.ConfigDedicated.Plugins) {

					sb.Append(" - ").Append(plugin).AppendLine();

				}

				sb.AppendLine();

			}

			//Errors
			var errors = SpawnLogger.Error.ToString();

			if (!string.IsNullOrWhiteSpace(errors)) {

				sb.Append("::: Detected Spawner Errors :::").AppendLine();
				sb.Append(errors).AppendLine();
				sb.AppendLine();

			}

			errors = BehaviorLogger.Error.ToString();

			if (!string.IsNullOrWhiteSpace(errors)) {

				sb.Append("::: Detected Behavior Errors :::").AppendLine();
				sb.Append(errors).AppendLine();
				sb.AppendLine();

			}

			return sb.ToString();
		
		}

		public static string GetItemPrices(ChatMessage msg) {

			var sb = new StringBuilder();

			foreach (var item in EconomyHelper.MinimumValuesMaster.Keys/*.OrderBy(x => x)*/) {

				MyPhysicalItemDefinition itemDef = null;

				if (!DefinitionHelper.AllItemDefinitions.TryGetValue(item, out itemDef))
					continue;

				sb.Append("Item Name:   ").Append(itemDef.DisplayNameText != null ? itemDef.DisplayNameText : "(Unnamed)").AppendLine();
				sb.Append("Item Id:     ").Append(item).AppendLine();
				sb.Append("Item Value:  ").Append(EconomyHelper.MinimumValuesMaster[item]).AppendLine();
				sb.Append("Item Source: ").Append(itemDef.Context?.ModName != null ? itemDef.Context.ModName : "None").AppendLine();
				sb.Append("Craftable:   ").Append(EconomyHelper.AssemblerCraftableItems.Contains(item).ToString()).AppendLine();
				sb.AppendLine();

			}

			return sb.ToString();
		
		}

		public static string GetEligibleSpawnsAtPosition(ChatMessage msg) {

			//StringBuilder
			var sb = new StringBuilder();

			//Environment
			var environment = new EnvironmentEvaluation(msg.PlayerPosition);
			var threatLevel = SpawnConditions.GetThreatLevel(5000, false, msg.PlayerPosition);
			var pcuLevel = SpawnConditions.GetPCULevel(5000, msg.PlayerPosition);
			SpawnGroupCollection collection = null;

			sb.Append("::: Spawn Data Near Player :::").AppendLine();
			sb.Append(" - Threat Score: ").Append(threatLevel.ToString()).AppendLine();
			sb.Append(" - PCU Score:    ").Append(pcuLevel.ToString()).AppendLine();
			sb.Append(" - Combat Phase: ").Append(!Settings.Combat.EnableCombatPhaseSystem ? "Disabled In Settings" : (CombatPhaseManager.Active ? "Active" : "Inactive")).AppendLine();

			sb.AppendLine();

			//Environment Data Near Player
			sb.Append("::: Environment Data Near Player :::").AppendLine();
			sb.Append(" - Distance From World Center:      ").Append(environment.DistanceFromWorldCenter.ToString()).AppendLine();
			sb.Append(" - Direction From World Center:     ").Append(environment.DirectionFromWorldCenter.ToString()).AppendLine();
			sb.Append(" - Is On Planet:                    ").Append(environment.IsOnPlanet.ToString()).AppendLine();
			sb.Append(" - Planet Name:                     ").Append(environment.IsOnPlanet ? environment.NearestPlanetName : "N/A").AppendLine();
			sb.Append(" - Planet Entity Id:                ").Append(environment.IsOnPlanet ? environment.NearestPlanet.Planet.EntityId.ToString() : "N/A").AppendLine();
			sb.Append(" - Planet Center Coordinates:       ").Append(environment.IsOnPlanet ? environment.NearestPlanet.Center().ToString() : "N/A").AppendLine();
			sb.Append(" - Planet Surface Coordinates:      ").Append(environment.IsOnPlanet ? environment.SurfaceCoords.ToString() : "N/A").AppendLine();
			sb.Append(" - Planet Diameter:                 ").Append(environment.IsOnPlanet ? environment.PlanetDiameter.ToString() : "N/A").AppendLine();
			sb.Append(" - Oxygen At Position:              ").Append(environment.IsOnPlanet ? environment.OxygenAtPosition.ToString() : "N/A").AppendLine();
			sb.Append(" - Atmosphere At Position:          ").Append(environment.IsOnPlanet ? environment.AtmosphereAtPosition.ToString() : "N/A").AppendLine();
			sb.Append(" - Gravity At Position:             ").Append(environment.IsOnPlanet ? environment.GravityAtPosition.ToString() : "N/A").AppendLine();
			sb.Append(" - Altitude At Position:            ").Append(environment.IsOnPlanet ? environment.AltitudeAtPosition.ToString() : "N/A").AppendLine();
			sb.Append(" - Is Night At Position:            ").Append(environment.IsOnPlanet ? environment.IsNight.ToString() : "N/A").AppendLine();
			sb.Append(" - Weather At Position:             ").Append(environment.IsOnPlanet && !string.IsNullOrWhiteSpace(environment.WeatherAtPosition) ? environment.WeatherAtPosition.ToString() : "N/A").AppendLine();
			sb.Append(" - Common Terrain At Position:      ").Append(environment.IsOnPlanet ? environment.CommonTerrainAtPosition.ToString() : "N/A").AppendLine();
			sb.Append(" - Air Travel Viability Ratio:      ").Append(environment.IsOnPlanet ? (Math.Round(environment.AirTravelViabilityRatio, 3)).ToString() : "N/A").AppendLine().AppendLine();

			sb.Append(" - Water Mod Enabled:               ").Append(AddonManager.WaterMod).AppendLine();
			sb.Append(" - Planet Has Water:                ").Append(environment.IsOnPlanet ? environment.PlanetHasWater.ToString() : "N/A").AppendLine();
			sb.Append(" - Position Underwater:             ").Append(environment.IsOnPlanet ? environment.PositionIsUnderWater.ToString() : "N/A").AppendLine();
			sb.Append(" - Surface Underwater:              ").Append(environment.IsOnPlanet ? environment.SurfaceIsUnderWater.ToString() : "N/A").AppendLine();
			sb.Append(" - Water Coverage Ratio:            ").Append(environment.IsOnPlanet ? (Math.Round(environment.WaterInSurroundingAreaRatio, 3)).ToString() : "N/A").AppendLine().AppendLine();

			sb.Append(" - Nebula Mod Enabled:              ").Append(AddonManager.NebulaMod).AppendLine();
			sb.Append(" - Inside Nebula:                   ").Append(environment.InsideNebula).AppendLine();
			sb.Append(" - Nebula Density:                  ").Append(environment.NebulaDensity).AppendLine();
			sb.Append(" - Nebula Material:                 ").Append(environment.NebulaMaterial).AppendLine();
			sb.Append(" - Nebula Weather:                  ").Append(environment.NebulaWeather).AppendLine().AppendLine();

			sb.Append(" - Space Cargo Ship Eligible:       ").Append(environment.SpaceCargoShipsEligible).AppendLine();
			sb.Append(" - Lunar Cargo Ship Eligible:       ").Append(environment.LunarCargoShipsEligible).AppendLine();
			sb.Append(" - Planetary Cargo Ship Eligible:   ").Append(environment.PlanetaryCargoShipsEligible).AppendLine();
			sb.Append(" - Gravity Cargo Ship Eligible:     ").Append(environment.GravityCargoShipsEligible).AppendLine();
			sb.Append(" - Random Encounter Eligible:       ").Append(environment.RandomEncountersEligible).AppendLine();
			sb.Append(" - Planetary Installation Eligible: ").Append(environment.PlanetaryInstallationEligible).AppendLine();
			sb.Append(" - Water Installation Eligible:     ").Append(environment.WaterInstallationEligible).AppendLine();

			sb.AppendLine();

			SpawnLogger.SpawnGroup.Clear();

			//Space Cargo
			collection = new SpawnGroupCollection();
			SpawnGroupManager.GetSpawnGroups(SpawningType.SpaceCargoShip, "Debug", environment, "", collection);

			if (collection.SpawnGroups.Count > 0) {

				sb.Append("::: Space / Lunar Cargo Ship Eligible Spawns :::").AppendLine();

				foreach (var sgroup in collection.SpawnGroups.Distinct()) {

					sb.Append(" - ").Append(sgroup.SpawnGroupName).AppendLine();

				}

				sb.AppendLine();

			}

			//Random Encounter
			collection = new SpawnGroupCollection();
			SpawnGroupManager.GetSpawnGroups(SpawningType.RandomEncounter, "Debug", environment, "", collection);

			if (collection.SpawnGroups.Count > 0) {

				sb.Append("::: Random Encounter Eligible Spawns :::").AppendLine();

				foreach (var sgroup in collection.SpawnGroups.Distinct()) {

					sb.Append(" - ").Append(sgroup.SpawnGroupName).AppendLine();

				}

				sb.AppendLine();

			}

			//Planetary Cargo
			collection = new SpawnGroupCollection();
			SpawnGroupManager.GetSpawnGroups(SpawningType.PlanetaryCargoShip, "Debug", environment, "", collection);

			if (environment.PlanetaryCargoShipsEligible || environment.GravityCargoShipsEligible) {

				sb.Append("::: Planetary / Gravity Cargo Ship Eligible Spawns :::").AppendLine();

				if (APIs.DragApiLoaded && APIs.Drag.AdvLift && !Settings.Grids.AerodynamicsModAdvLiftOverride) {

					sb.Append(" > Aerodynamics Mod AdvLift Detected. Most Planetary Cargo Ships Will Not Be Compatible.").AppendLine();
					sb.Append(" > To Restore Cargo Ships, Use One Of The Following Options:").AppendLine();
					sb.Append("   > Disable AdvLift in Aerodynamics Mod Config.").AppendLine();
					sb.Append("   > Remove Aerodynamics Mod.").AppendLine();
					sb.Append("   > Enable [AerodynamicsModAdvLiftOverride] in MES Config File Config-Grids.xml (Not Recommended / Ships May Not Behave Properly)").AppendLine();

				}

				if (collection.SpawnGroups.Count > 0) {

					foreach (var sgroup in collection.SpawnGroups.Distinct()) {

						sb.Append(" - ").Append(sgroup.SpawnGroupName).AppendLine();

					}

				}

				sb.AppendLine();

			}

			//Planetary Installation
			collection = new SpawnGroupCollection();
			SpawnGroupManager.GetSpawnGroups(SpawningType.PlanetaryInstallation, "Debug", environment, "", collection);

			if (collection.SpawnGroups.Count > 0) {

				sb.Append("::: Planetary Installation Eligible Spawns :::").AppendLine();

				foreach (var sgroup in collection.SpawnGroups.Distinct()) {

					sb.Append(" - ").Append(sgroup.SpawnGroupName).AppendLine();

				}

				sb.AppendLine();

			}

			//Boss
			collection = new SpawnGroupCollection();
			SpawnGroupManager.GetSpawnGroups(SpawningType.BossEncounter, "Debug", environment, "", collection);

			if (collection.SpawnGroups.Count > 0) {

				sb.Append("::: Boss Encounter Eligible Spawns :::").AppendLine();

				foreach (var sgroup in collection.SpawnGroups.Distinct()) {

					sb.Append(" - ").Append(sgroup.SpawnGroupName).AppendLine();

				}

				sb.AppendLine();

			}

			//Creature
			collection = new SpawnGroupCollection();
			SpawnGroupManager.GetSpawnGroups(SpawningType.Creature, "Debug", environment, "", collection);

			if (collection.SpawnGroups.Count > 0) {

				sb.Append("::: Creature / Bot Eligible Spawns :::").AppendLine();

				foreach (var sgroup in collection.SpawnGroups.Distinct()) {

					sb.Append(" - ").Append(sgroup.SpawnGroupName).AppendLine();

				}

				sb.AppendLine();

			}

			//Drone Encounters
			collection = new SpawnGroupCollection();
			SpawnGroupManager.GetSpawnGroups(SpawningType.DroneEncounter, "Debug", environment, "", collection);

			if (collection.SpawnGroups.Count > 0) {

				sb.Append("::: Drone Encounter Eligible Spawns :::").AppendLine();

				foreach (var sgroup in collection.SpawnGroups.Distinct()) {

					sb.Append(" - ").Append(sgroup.SpawnGroupName).AppendLine();

				}

				sb.AppendLine();

			}

			//StaticEncounters
			if (NpcManager.StaticEncounters != null) {

				if (NpcManager.StaticEncounters.Count > 0) {

					sb.Append("::: Static Encounter Eligible Spawns :::").AppendLine();

					foreach (var enc in NpcManager.StaticEncounters) {

						if (enc == null)
							continue;

						sb.Append(" - ").Append(!string.IsNullOrWhiteSpace(enc?.SpawnGroupName) ? enc.SpawnGroupName : "(null)").AppendLine();
						sb.Append("   - Coodinates: ").Append(enc.ExactLocationCoords.ToString()).AppendLine();
						sb.Append("   - Radius:     ").Append(enc.TriggerRadius.ToString()).AppendLine();

					}

					sb.AppendLine();

				}

			} else {

				sb.Append("::: WARNING: Static Encounter List Null! :::").AppendLine();
				sb.AppendLine();

			}



			//UniqueEncounters
			if (NpcManager.UniqueGroupsSpawned != null) {

				if (NpcManager.UniqueGroupsSpawned.Count > 0) {

					sb.Append("::: Unique Encounters Already Spawned :::").AppendLine();

					foreach (var enc in NpcManager.UniqueGroupsSpawned) {

						sb.Append(" - ").Append(enc).AppendLine();

					}

					sb.AppendLine();

				}

			} else {

				sb.Append("::: WARNING: Unique Encounter List Null! :::").AppendLine();
				sb.AppendLine();

			}



			//Zones
			if (ZoneManager.ActiveZones.Count > 0) {

				sb.Append("::: Active Zones In Range :::").AppendLine();

				foreach (var zone in ZoneManager.ActiveZones) {

					if (zone.PositionInsideZone(environment.Position))
						sb.Append(zone.GetInfo(environment.Position)).AppendLine();
				
				}

			}

			//Timeouts
			if (TimeoutManagement.Timeouts.Count > 0) {

				sb.Append("::: Timeout Zones In Range :::").AppendLine();

				foreach (var timeout in TimeoutManagement.Timeouts) {

					var timeoutRemaining = timeout.TimeoutLength();

					if (timeoutRemaining.X >= timeoutRemaining.Y) {

						timeout.Remove = true;
						continue;
					
					}

					sb.Append(timeout.GetInfo(environment.Position)).AppendLine();

				}

				sb.AppendLine();

			}

			//Planetary Lanes
			if (environment.InsidePlanetaryLanes.Count > 0) {

				sb.Append("::: Planetary Lanes In Position :::").AppendLine();

				foreach (var lane in environment.InsidePlanetaryLanes) {

					sb.Append(" - ").Append(lane.PlanetA.Planet.Generator.Id.SubtypeName).Append(" - ").Append(lane.PlanetB.Planet.Generator.Id.SubtypeName).AppendLine();

				}

				sb.AppendLine();

			}

			return sb.ToString();

		}

		public static string GetBlockData(ChatMessage message) {

			var line = new LineD(message.CameraPosition, message.CameraDirection * 10000 + message.CameraPosition);
			GridEntity thisGrid = null;

			var sb = new StringBuilder();

			foreach (var grid in GridManager.Grids) {

				if (!grid.ActiveEntity())
					continue;

				if (!grid.CubeGrid.WorldAABB.Intersects(ref line))
					continue;

				thisGrid = grid;
				break;

			}

			if (thisGrid == null) {

				message.ReturnMessage = "Could Not Locate Grid At Player Camera Position. Point Camera Cursor At Target Within 10KM. Check Clipboard For Results.";
				return sb.ToString() ?? "No Data";

			}

			var cell = thisGrid.CubeGrid.RayCastBlocks(line.From, line.To);

			if (!cell.HasValue) {

				message.ReturnMessage = "Could Not Locate Block At Player Camera Position. Point Camera Cursor At Target Within 10KM. Check Clipboard For Results.";
				return sb.ToString() ?? "No Data";

			}

			var block = thisGrid.CubeGrid.GetCubeBlock(cell.Value);

			sb.Append("SubtypeID: ").Append(block.BlockDefinition.Id.SubtypeName).AppendLine();
			sb.Append("Min:       ").Append(block.Min).AppendLine();
			message.ReturnMessage = sb.ToString();
			return sb.ToString();

		}

		public static string GetGridBehavior(ChatMessage message) {

			var line = new LineD(message.CameraPosition, message.CameraDirection * 10000 + message.CameraPosition);
			GridEntity thisGrid = null;

			var sb = new StringBuilder();

			foreach (var grid in GridManager.Grids) {

				if (!grid.ActiveEntity())
					continue;

				if (!grid.CubeGrid.WorldAABB.Intersects(ref line))
					continue;

				if (grid.Behavior == null) {

					foreach (var behavior in BehaviorManager.Behaviors) {

						if (behavior?.RemoteControl?.SlimBlock?.CubeGrid != null && grid.CubeGrid == behavior.RemoteControl.SlimBlock.CubeGrid) {

							grid.Behavior = behavior;
							break;

						}
					
					}

					if(grid.Behavior == null) {

						foreach (var behavior in BehaviorManager.TerminatedBehaviors) {

							if (behavior?.RemoteControl?.SlimBlock?.CubeGrid != null && grid.CubeGrid == behavior.RemoteControl.SlimBlock.CubeGrid) {

								grid.Behavior = behavior;
								break;

							}

						}

					}

					if (grid.Behavior == null) {

						sb.Append("No Matching Behavior Found In Master Behavior List Or Terminated Behavior List").AppendLine();
						sb.Append("Grid Does Not Have Registered Behavior In Entity: " + grid.CubeGrid?.CustomName ?? "(null grid name)").AppendLine();

						continue;

					}

					
					//message.ReturnMessage = string.Format("[{0}] Does Not Have an Active Behavior.", thisGrid.CubeGrid.CustomName);
					//return "";

				}

				thisGrid = grid;
				break;

			}

			if (thisGrid == null) {

				message.ReturnMessage = "Could Not Locate NPC Grid At Player Camera Position. Point Camera Cursor At Target Within 10KM. Check Clipboard For Results.";
				return sb.ToString() ?? "No Data";

			}

			message.ReturnMessage = "NPC Behavior Data Sent To Clipboard";
			return thisGrid.Behavior.ToString();

		}

		public static string GetGridData(ChatMessage message) {

			var line = new LineD(message.CameraPosition, message.CameraDirection * 10000 + message.CameraPosition);
			GridEntity thisGrid = null;

			var sb = new StringBuilder();

			foreach (var grid in GridManager.Grids) {

				if (!grid.ActiveEntity())
					continue;

				if (!grid.CubeGrid.WorldAABB.Intersects(ref line))
					continue;

				if (grid.Npc == null) {

					continue;

				}

				thisGrid = grid;
				break;

			}

			if (thisGrid == null) {

				message.ReturnMessage = "Could Not Locate NPC Grid At Player Camera Position. Point Camera Cursor At Target Within 10KM. Check Clipboard For Results.";
				return sb.ToString() ?? "No Data";

			}

			message.ReturnMessage = "NPC Grid Data Sent To Clipboard";
			return thisGrid.Npc.ToString();

		}

		public static string GetGridMatrixInfo(ChatMessage message) {

			var line = new LineD(message.CameraPosition, message.CameraDirection * 400 + message.CameraPosition);
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

				message.ReturnMessage = "Could Not Locate Grid At Player Camera Position.";
				return "";

			}

			var sb = new StringBuilder();

			sb.Append("Grid Name:          ").Append(thisGrid.CubeGrid.CustomName).AppendLine().AppendLine();

			sb.Append("Tags For SpawnGroup:").AppendLine();
			sb.Append("[StaticEncounterCoords:{").Append(thisGrid.CubeGrid.WorldMatrix.Translation).Append("}]").AppendLine();
			sb.Append("[StaticEncounterForward:{").Append(thisGrid.CubeGrid.WorldMatrix.Forward).Append("}]").AppendLine();
			sb.Append("[StaticEncounterUp:{").Append(thisGrid.CubeGrid.WorldMatrix.Up).Append("}]").AppendLine().AppendLine();

			var planet = PlanetManager.GetNearestPlanet(thisGrid.CubeGrid.WorldMatrix.Translation);

			if (planet != null) {

				var up = planet.UpAtPosition(thisGrid.CubeGrid.WorldMatrix.Translation);
				var dist = planet.AltitudeAtPosition(thisGrid.CubeGrid.WorldMatrix.Translation, false);

				sb.Append("Optional Tags For Dynamic Planet Spawning:").AppendLine();
				sb.Append("[StaticEncounterUsePlanetDirectionAndAltitude:").Append("true").Append("]").AppendLine();
				sb.Append("[StaticEncounterPlanet:").Append(planet.Planet.Generator.Id.SubtypeName).Append("]").AppendLine();
				sb.Append("[StaticEncounterPlanetDirection:{").Append(up).Append("}]").AppendLine();
				sb.Append("[StaticEncounterPlanetAltitude:").Append(dist).Append("]").AppendLine();

			}

			message.ReturnMessage = "Registered SpawnGroups Sent To Clipboard.";
			return sb.ToString();

		}

		public static string GetItemMassData(ChatMessage message) {

			var sb = new StringBuilder();

			foreach (var item in DefinitionHelper.ItemWeightReference.Keys) {

				sb.Append(item.ToString()).AppendLine();
				sb.Append(DefinitionHelper.ItemWeightReference[item]).AppendLine();
				sb.AppendLine();

			}

			return sb.ToString();

		}

		public static string GetLogging(string[] command, ref string returnMsg) {

			if (command.Length < 5) {

				returnMsg = "Command Missing Parameters.";
				return "";

			}

			string result = "";

			if (command[3] == "SpawnDebug") {

				if (command[4] == "API")
					result = SpawnLogger.API.ToString();

				if (command[4] == "BlockLogic")
					result = SpawnLogger.BlockLogic.ToString();

				if (command[4] == "CleanUp")
					result = SpawnLogger.CleanUp.ToString();

				if (command[4] == "Dev")
					result = SpawnLogger.Dev.ToString();

				if (command[4] == "Entity")
					result = SpawnLogger.Entity.ToString();

				if (command[4] == "Error")
					result = SpawnLogger.Error.ToString();

				if (command[4] == "Manipulation")
					result = SpawnLogger.Manipulation.ToString();

				if (command[4] == "Pathing")
					result = SpawnLogger.Pathing.ToString();

				if (command[4] == "PostSpawn")
					result = SpawnLogger.PostSpawn.ToString();

				if (command[4] == "Settings")
					result = SpawnLogger.Settings.ToString();

				if (command[4] == "SpawnGroup")
					result = SpawnLogger.SpawnGroup.ToString();

				if (command[4] == "Spawning")
					result = SpawnLogger.Spawning.ToString();

				if (command[4] == "SpawnRecord")
					result = SpawnLogger.SpawnRecord.ToString();

				if (command[4] == "Startup")
					result = SpawnLogger.Startup.ToString();

				if (command[4] == "Zone")
					result = SpawnLogger.Zone.ToString();


			} else if (command[3] == "BehaviorDebug") {

				if (command[4] == "Action")
					result = BehaviorLogger.Action.ToString();

				if (command[4] == "AutoPilot")
					result = BehaviorLogger.AutoPilot.ToString();

				if (command[4] == "BehaviorMode")
					result = BehaviorLogger.BehaviorMode.ToString();

				if (command[4] == "BehaviorSetup")
					result = BehaviorLogger.BehaviorSetup.ToString();

				if (command[4] == "BehaviorSpecific")
					result = BehaviorLogger.BehaviorSpecific.ToString();

				if (command[4] == "Chat")
					result = BehaviorLogger.Chat.ToString();

				if (command[4] == "Command")
					result = BehaviorLogger.Command.ToString();

				if (command[4] == "Condition")
					result = BehaviorLogger.Condition.ToString();

				if (command[4] == "Collision")
					result = BehaviorLogger.Collision.ToString();

				if (command[4] == "Despawn")
					result = BehaviorLogger.Despawn.ToString();

				if (command[4] == "Error")
					result = BehaviorLogger.Error.ToString();

				if (command[4] == "General")
					result = BehaviorLogger.General.ToString();

				if (command[4] == "Owner")
					result = BehaviorLogger.Owner.ToString();

				if (command[4] == "Spawn")
					result = BehaviorLogger.Spawn.ToString();

				if (command[4] == "Startup")
					result = BehaviorLogger.Startup.ToString();

				if (command[4] == "TargetAcquisition")
					result = BehaviorLogger.TargetAcquisition.ToString();

				if (command[4] == "TargetEvaluation")
					result = BehaviorLogger.TargetEvaluation.ToString();

				if (command[4] == "Thrust")
					result = BehaviorLogger.Thrust.ToString();

				if (command[4] == "Trigger")
					result = BehaviorLogger.Trigger.ToString();

				if (command[4] == "Weapon")
					result = BehaviorLogger.Weapon.ToString();

				if (command[4] == "Dev")
					result = BehaviorLogger.Dev.ToString();

			}

			returnMsg = !string.IsNullOrWhiteSpace(result) ? "Logging Info Sent To Clipboard." : "Logging Info Not Found For Provided Type.";
			return result;

		}

		public static void GetThreatScore(ChatMessage msg, string[] array) {

			double defaultDist = 5000;

			if (array.Length >= 4) {

				if (!double.TryParse(array[3], out defaultDist))
					defaultDist = 5000;
			
			}

			var threatLevel = SpawnConditions.GetThreatLevel(5000, false, msg.PlayerPosition);
			MyVisualScriptLogicProvider.ShowNotification("Threat Score At Position With " + defaultDist + " Meters: " + threatLevel, 5000, "White", msg.PlayerId);

		}

		public static void GetZones(ChatMessage msg) {

			var sb = new StringBuilder();
			sb.Append("::::: Zone Data :::::").AppendLine().AppendLine();

			for (int i = ZoneManager.ActiveZones.Count - 1; i >= 0; i--) {

				var zone = ZoneManager.ActiveZones[i];

				if (zone == null || !zone.Active)
					continue;

				sb.Append(zone.GetInfo(msg.PlayerPosition)).AppendLine();

			}

			msg.ClipboardPayload = sb.ToString();
			msg.Mode = ChatMsgMode.ReturnMessage;
			msg.ReturnMessage = "Zone Data Copied To Clipboard";
		
		}

		public static void GetEvents(ChatMessage msg)
		{

			var sb = new StringBuilder();
			var sb2 = new StringBuilder();
			sb.Append("::::: Event Data :::::").AppendLine();

			sb2.Append("::::: Disabled Events :::::").AppendLine();

			for (int i = EventManager.EventsList.Count - 1; i >= 0; i--)
			{
				var Event = EventManager.EventsList[i];

				if (Event == null)
					continue;

				if ((Event.RunCount > 0 && Event.Profile.UniqueEvent) | !Event.EventEnabled)
				{
					sb2.Append(Event.GetShortInfo()).AppendLine();
				}
				else
					sb.Append(Event.GetInfo()).AppendLine();


			}




			sb.Append(sb2);

			msg.ClipboardPayload = sb.ToString();
			msg.Mode = ChatMsgMode.ReturnMessage;
			msg.ReturnMessage = "Event Data Copied To Clipboard";

		}







		public static void ProcessPrefabs(ChatMessage msg, string[] array) {

			if (MyAPIGateway.Session.LocalHumanPlayer == null || MyAPIGateway.Utilities.IsDedicated) {

				msg.Mode = ChatMsgMode.ReturnMessage;
				msg.ReturnMessage = "Command Must Be Run in Offline World";
				return;

			}

			if (array.Length < 4 || string.IsNullOrWhiteSpace(array[3])) {

				msg.Mode = ChatMsgMode.ReturnMessage;
				msg.ReturnMessage = "No Mod Name Found In Chat Command";
				return;

			}

			var prefabProcess = new PrefabDiagnosticsTask(array[3]);
			TaskProcessor.Tasks.Add(prefabProcess);
		
		}

		public static void RemoveAllNpcs(ChatMessage msg) {

			for (int i = NpcManager.ActiveNpcs.Count - 1; i >= 0; i--) {

				var npc = NpcManager.ActiveNpcs[i];

				if (!npc.ActiveEntity() || npc.Npc == null)
					continue;

				if (!Cleaning.BasicCleanupChecks(npc))
					continue;

				if (npc.CubeGrid.IsStatic && Cleaning.IsKeenEconomyStation(npc))
					continue;

				if (npc.Ownership.HasFlag(GridOwnershipEnum.NpcMajority))
					Cleaning.RemoveGrid(npc);

			}

			MyVisualScriptLogicProvider.ShowNotification("Removing All Fully NPC Owned Grids", 5000, "White", msg.PlayerId);

		}


		public static void ResetReputation(ChatMessage msg, string[] msgSplit) {

			if (msgSplit.Length != 4) {

				MyVisualScriptLogicProvider.ShowNotification("Invalid Command Received", 5000, "White", msg.PlayerId);
				return;

			}

			var result = RelationManager.ResetFactionReputation(msgSplit[3]);
			MyVisualScriptLogicProvider.ShowNotification("Faction [" + msgSplit[3] + "] Reputation Reset Result: " + result, 5000, "White", msg.PlayerId);
			return;

		}

		public static void ResetZones(ChatMessage msg) {

			ZoneManager.ResetAllZones();
			MyVisualScriptLogicProvider.ShowNotification("All Zone Data Reset", 5000, "White", msg.PlayerId);
			return;

		}

		public static void RotationData(ChatMessage msg, string[] msgSplit) {

			// /MES.Debug.RotationData.RotationMulti
			if (msgSplit.Length < 4) {

				MyVisualScriptLogicProvider.ShowNotification("Invalid Command Received", 5000, "White", msg.PlayerId);
				return;

			}

			float rotation = 0;

			if (!float.TryParse(msgSplit[3], out rotation)) {

				MyVisualScriptLogicProvider.ShowNotification("Couldn't Parse " + msgSplit[3] + " To A Number.", 5000, "White", msg.PlayerId);
				return;

			}

			var grid = GridManager.GetClosestGridInDirection(MatrixD.CreateWorld(msg.CameraPosition, msg.CameraDirection, VectorHelper.RandomPerpendicular(msg.CameraDirection)), 10000);

			if (grid == null) {

				MyVisualScriptLogicProvider.ShowNotification("No Grid Detected In Direction", 5000, "White", msg.PlayerId);
				return;

			}

			TaskProcessor.Tasks.Add(new DebugRotationData(grid, rotation, msg));

		}

		public static void SetGridOwnership(ChatMessage message, string[] msgSplit) {

			if (msgSplit.Length < 4) {

				MyVisualScriptLogicProvider.ShowNotification("Invalid Command Received", 5000, "White", message.PlayerId);
				return;

			}

			var line = new LineD(message.CameraPosition, message.CameraDirection * 2000 + message.CameraPosition);
			GridEntity thisGrid = null;

			var faction = MyAPIGateway.Session.Factions.TryGetFactionByTag(msgSplit[3]);

			if (faction == null) {

				message.ReturnMessage = "Could Not Locate Faction With ID.";
				return;

			}

			var owner = FactionHelper.GetFactionMemberIdFromTag(faction.Tag);

			for (int i = GridManager.Grids.Count - 1; i >= 0; i--) {

				var grid = GridManager.GetSafeGridFromIndex(i);

				if (!grid.ActiveEntity())
					continue;

				if (!grid.CubeGrid.WorldAABB.Intersects(ref line))
					continue;

				thisGrid = grid;
				break;

			}

			if (thisGrid?.CubeGrid?.CustomName == null) {

				message.ReturnMessage = "Could Not Locate Grid At Player Camera Position.";
				return;

			}

			thisGrid.RefreshSubGrids();
			foreach (var linkedGrid in thisGrid.LinkedGrids) {

				if (linkedGrid == null)
					continue;

				linkedGrid.CubeGrid.ChangeGridOwnership(owner, MyOwnershipShareModeEnum.Faction);
			
			}

			message.ReturnMessage = "Grid [" + thisGrid.CubeGrid.CustomName + "] Ownership Changed To [" + msgSplit[3] + "]";
			return;

		}

		public static void SetReputation(ChatMessage msg, string[] msgSplit) {

			if (msgSplit.Length != 4) {

				MyVisualScriptLogicProvider.ShowNotification("Invalid Command Received", 5000, "White", msg.PlayerId);
				return;

			}

			var result = RelationManager.ResetFactionReputation(msgSplit[3]);
			MyVisualScriptLogicProvider.ShowNotification("Faction [" + msgSplit[3] + "] Reputation Reset Result: " + result, 5000, "White", msg.PlayerId);
			return;

		}

		public static void SpawnAllPrefabs(ChatMessage msg, string[] array) {

			if (MyAPIGateway.Session.LocalHumanPlayer == null || MyAPIGateway.Utilities.IsDedicated) {

				msg.Mode = ChatMsgMode.ReturnMessage;
				msg.ReturnMessage = "Command Must Be Run in Offline World";
				return;

			}

			if (array.Length < 4 || string.IsNullOrWhiteSpace(array[3])) {

				msg.Mode = ChatMsgMode.ReturnMessage;
				msg.ReturnMessage = "No Mod Name Found In Chat Command";
				return;

			}

			var prefabProcess = new SpawnAllPrefabs(array[3]);
			TaskProcessor.Tasks.Add(prefabProcess);

		}

		public static void StoreTest(ChatMessage msg) {

			var matrix = MatrixD.CreateWorld(msg.CameraPosition, msg.CameraDirection, VectorHelper.RandomPerpendicular(msg.CameraDirection));
			var block = GridManager.GetClosestBlockInDirection(matrix, 2000);

			if (block?.FatBlock == null) {

				msg.Mode = ChatMsgMode.ReturnMessage;
				msg.ReturnMessage = "No Block In Camera Direction";
				return;

			}

			var store = block.FatBlock as IMyStoreBlock;

			if (store == null) {

				msg.Mode = ChatMsgMode.ReturnMessage;
				msg.ReturnMessage = "Detected Block Isnt Store Block";
				return;

			}

			var prefab = MyDefinitionManager.Static.GetPrefabDefinition("(NPC-MES) WcHeliosTest");

			if (prefab == null) {

				msg.Mode = ChatMsgMode.ReturnMessage;
				msg.ReturnMessage = "Prefab Null";
				return;

			}

			if (prefab.Icons == null) {

				msg.Mode = ChatMsgMode.ReturnMessage;
				msg.ReturnMessage = "Prefab Icons Null";
				return;

			}

			if (prefab.Icons.Length == 0) {

				msg.Mode = ChatMsgMode.ReturnMessage;
				msg.ReturnMessage = "Prefab Icons Empty";
				return;

			}

			var item = store.CreateStoreItem("(NPC-MES) WcHeliosTest", 1, 1000, 1000);
			item.IsCustomStoreItem = true;
			store.InsertStoreItem(item);
			
			msg.Mode = ChatMsgMode.ReturnMessage;
			msg.ReturnMessage = "Item Added: " + prefab.Icons[0];
			return;

		}

		public static void YeetGrid(ChatMessage msg) {

			var matrix = MatrixD.CreateWorld(msg.CameraPosition, msg.CameraDirection, VectorHelper.RandomPerpendicular(msg.CameraDirection));
			var grid = GridManager.GetClosestGridInDirection(matrix, 10000);

			if (grid?.CubeGrid?.Physics == null) {

				msg.Mode = ChatMsgMode.ReturnMessage;
				msg.ReturnMessage = "No Grid In Camera Direction";
				return;
			
			}

			var linear = msg.CameraDirection * 100;
			var angular = Vector3.One;
			grid.CubeGrid.Physics.SetSpeeds(linear, angular);

		}


	}

}
