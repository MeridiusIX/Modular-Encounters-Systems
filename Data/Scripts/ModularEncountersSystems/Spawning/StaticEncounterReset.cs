using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning.Profiles;
using ModularEncountersSystems.Sync;
using ModularEncountersSystems.Tasks;
using ModularEncountersSystems.World;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Spawning {

	public class StaticEncounterResetSite {

		public string SpawnGroupName;
		public ImprovedSpawnGroup SpawnGroup;
		public SpawnConditionsProfile Conditions;

		public bool Spawned; //SpawnGroupName is in NpcManager.UniqueGroupsSpawned
		public StaticEncounter Pending; //A valid encounter that is waiting for a player to come in range

		public List<IMyCubeGrid> Grids;
		public List<string> Blockers;
		public string Problem;

		public BoundingBoxD Footprint;
		public bool HasFootprint;
		public Vector3D SpawnCoords;

		public StaticEncounterResetSite(string spawnGroupName) {

			SpawnGroupName = spawnGroupName;
			Grids = new List<IMyCubeGrid>();
			Blockers = new List<string>();
			Problem = null;

		}

		public bool IsSafe { get { return Problem == null && Blockers.Count == 0; } }

		public bool IsTracked { get { return Spawned || Pending != null; } }

	}

	public static class StaticEncounterReset {

		public const string ChatCommand = "/MES.Debug.ResetStaticEncounters";
		public const string ChatCommandInfo = "/MES.Debug.ResetStaticEncounters.Info";

		public const double SpawnClearance = 250;
		public const double FootprintMargin = 25;
		public const int CloseTimeoutTicks = 600;

		public static HashSet<string> InProgress = new HashSet<string>();

		private static List<IMyCubeGrid> _tempGrids = new List<IMyCubeGrid>();
		private static List<IMyPlayer> _tempPlayers = new List<IMyPlayer>();
		private static HashSet<IMyEntity> _tempEntities = new HashSet<IMyEntity>();
		private static HashSet<long> _tempCheckedCharacters = new HashSet<long>();

		public static bool IsChatCommand(string message) {

			return message == ChatCommand || message.StartsWith(ChatCommand + ".");

		}

		public static void ProcessChatCommand(ChatMessage msg) {

			bool infoOnly = false;

			if (msg.Message == ChatCommandInfo) {

				infoOnly = true;

			} else if (msg.Message != ChatCommand) {

				msg.Mode = ChatMsgMode.ReturnMessage;
				msg.ReturnMessage = "Usage: /MES.Debug.ResetStaticEncounters or /MES.Debug.ResetStaticEncounters.Info. To reset a single encounter, use /MES.Debug.ResetThisStaticEncounters.SpawnGroupName";
				return;

			}

			var sb = new StringBuilder();
			sb.Append("::: Reset Static Encounters").Append(infoOnly ? " (Info Only)" : "").Append(" :::").AppendLine().AppendLine();

			var sites = new List<StaticEncounterResetSite>();
			int notSpawned = 0;

			foreach (var spawnGroup in SpawnGroupManager.SpawnGroups) {

				if (string.IsNullOrWhiteSpace(spawnGroup?.SpawnGroupName) || GetStaticConditions(spawnGroup) == null)
					continue;

				var site = EvaluateSite(spawnGroup.SpawnGroupName);

				if (!site.Spawned && site.Grids.Count == 0) {

					notSpawned++;
					continue;

				}

				sites.Add(site);

			}

			int reset = 0;
			int skipped = 0;

			foreach (var site in sites) {

				sb.Append("[").Append(site.SpawnGroupName).Append("]: ");

				if (!site.IsSafe) {

					skipped++;
					sb.Append("SKIPPED");

					if (site.Problem != null)
						sb.Append(" - ").Append(site.Problem);

					foreach (var blocker in site.Blockers)
						sb.AppendLine().Append("   - blocked by ").Append(blocker);

					sb.AppendLine();
					continue;

				}

				if (site.Grids.Count == 0) {

					skipped++;
					sb.Append("NOT FOUND - marked as spawned, but none of its grids were found (destroyed or removed?). ");
					sb.Append("Use /MES.Debug.ResetThisStaticEncounters.").Append(site.SpawnGroupName).Append(" to let it spawn again when a player comes in range.").AppendLine();
					continue;

				}

				var action = DescribeAction(site);

				if (infoOnly) {

					sb.Append("ready: ").Append(action).AppendLine();
					continue;

				}

				Execute(site, msg.PlayerId);
				reset++;
				sb.Append("RESETTING - ").Append(action).AppendLine();

			}

			if (sites.Count == 0)
				sb.Append("No spawned Static Encounters found.").AppendLine();

			sb.AppendLine().Append("Static Encounters not spawned yet (left alone): ").Append(notSpawned).AppendLine();
			sb.AppendLine().Append(infoOnly ? "Nothing was changed. When an encounter is reset, its old grids are removed directly: their Despawn settings and Despawn/Compromised triggers do not run." : "The old grids are removed directly: their Despawn settings and Despawn/Compromised triggers do not run.").AppendLine();

			if (!infoOnly)
				sb.Append("The result for each encounter is shown as a notification and written to /MES.IGLSD.SpawnRecord.").AppendLine();

			SpawnLogger.Write(sb.ToString(), SpawnerDebugEnum.SpawnRecord);

			msg.Mode = ChatMsgMode.ReturnMessage;
			msg.ClipboardPayload = sb.ToString();

			if (infoOnly)
				msg.ReturnMessage = string.Format("Reset Static Encounters Info: {0} ready to reset, {1} skipped. Report Copied To Clipboard.", sites.Count - skipped, skipped);
			else
				msg.ReturnMessage = string.Format("Reset Static Encounters: {0} resetting, {1} skipped. Report Copied To Clipboard.", reset, skipped);

		}

		public static SpawnConditionsProfile GetStaticConditions(ImprovedSpawnGroup spawnGroup) {

			if (spawnGroup?.SpawnConditionsProfiles == null)
				return null;

			foreach (var condition in spawnGroup.SpawnConditionsProfiles) {

				if (condition != null && condition.StaticEncounter)
					return condition;

			}

			return null;

		}

		public static StaticEncounterResetSite EvaluateSite(string spawnGroupName) {

			var site = new StaticEncounterResetSite(spawnGroupName);

			if (InProgress.Contains(spawnGroupName)) {

				site.Problem = "a reset of this encounter is already in progress";
				return site;

			}

			site.SpawnGroup = SpawnGroupManager.GetSpawnGroupByName(spawnGroupName);

			if (site.SpawnGroup == null) {

				site.Problem = "SpawnGroup not found";
				return site;

			}

			site.Conditions = GetStaticConditions(site.SpawnGroup);

			if (site.Conditions == null) {

				site.Problem = "not a Static Encounter (no SpawnConditions profile with StaticEncounter set to true)";
				return site;

			}

			site.Spawned = NpcManager.UniqueGroupsSpawned.Contains(spawnGroupName);

			foreach (var encounter in NpcManager.StaticEncounters) {

				if (encounter != null && encounter.IsValid && encounter.SpawnGroupName == spawnGroupName) {

					site.Pending = encounter;
					break;

				}

			}

			var probe = new StaticEncounter();
			probe.InitStaticEncounter(site.SpawnGroup, site.Conditions);

			if (!probe.IsValid) {

				site.Problem = "could not find the spawn location from the current SpawnGroup definition (is the planet missing?)";
				return site;

			}

			site.SpawnCoords = probe.ExactLocationCoords;

			CollectGrids(site);

			if (!site.IsTracked) {

				site.Problem = "its grids were found, but MES has no record of this encounter spawning, so it cannot be reset";
				return site;

			}

			CheckSafety(site);
			return site;

		}

		private static bool IsStaticSpawnType(NpcData npc) {

			var mask = SpawningType.StaticEncounter | SpawningType.StaticEncounterPlanet | SpawningType.StaticEncounterSpace;
			return (npc.SpawnType & mask) != 0 || (npc.OriginalSpawnType & mask) != 0;

		}

		private static void CollectGrids(StaticEncounterResetSite site) {

			for (int i = GridManager.Grids.Count - 1; i >= 0; i--) {

				var grid = GridManager.GetSafeGridFromIndex(i);

				if (grid?.CubeGrid == null || grid.Closed || grid.CubeGrid.MarkedForClose || grid.Npc == null)
					continue;

				if (grid.Npc.SpawnGroupName != site.SpawnGroupName || !IsStaticSpawnType(grid.Npc))
					continue;

				if (site.Grids.Contains(grid.CubeGrid))
					continue;

				_tempGrids.Clear();
				MyAPIGateway.GridGroups.GetGroup(grid.CubeGrid, GridLinkTypeEnum.Mechanical, _tempGrids);

				if (!_tempGrids.Contains(grid.CubeGrid))
					_tempGrids.Add(grid.CubeGrid);

				foreach (var member in _tempGrids) {

					if (member != null && !site.Grids.Contains(member))
						site.Grids.Add(member);

				}

			}

			foreach (var cubeGrid in site.Grids) {

				if (!site.HasFootprint) {

					site.Footprint = cubeGrid.PositionComp.WorldAABB;
					site.HasFootprint = true;

				} else {

					site.Footprint.Include(cubeGrid.PositionComp.WorldAABB);

				}

			}

			if (site.HasFootprint)
				site.Footprint.Inflate(FootprintMargin);

		}

		private static bool HasPlayerOwner(IMyCubeGrid cubeGrid) {

			foreach (var owner in cubeGrid.BigOwners) {

				if (owner != 0 && !FactionHelper.IsIdentityNPC(owner))
					return true;

			}

			foreach (var owner in cubeGrid.SmallOwners) {

				if (owner != 0 && !FactionHelper.IsIdentityNPC(owner))
					return true;

			}

			return false;

		}

		private static bool IsInsideSite(StaticEncounterResetSite site, Vector3D position) {

			if (site.HasFootprint && site.Footprint.Contains(position) != ContainmentType.Disjoint)
				return true;

			return Vector3D.Distance(position, site.SpawnCoords) < SpawnClearance;

		}

		private static bool IsPlayerCharacter(IMyCharacter character) {

			if (character == null || character.IsDead)
				return false;

			var identity = character.ControllerInfo?.ControllingIdentityId ?? 0;

			if (identity == 0)
				return true;

			return FactionHelper.IsIdentityPlayer(identity);

		}

		private static string DescribeGrid(IMyCubeGrid cubeGrid) {

			var name = cubeGrid.CustomName ?? cubeGrid.EntityId.ToString();

			if (HasPlayerOwner(cubeGrid))
				return "'" + name + "' (player-owned)";

			var gridEntity = GridManager.GetGridEntity(cubeGrid, true);

			if (gridEntity?.Npc != null)
				return "'" + name + "' (NPC, " + (gridEntity.Npc.SpawnGroupName ?? "no SpawnGroup") + ")";

			if (cubeGrid.BigOwners.Count == 0 && cubeGrid.SmallOwners.Count == 0)
				return "'" + name + "' (no owner)";

			return "'" + name + "' (not spawned by MES)";

		}

		private static void CheckSafety(StaticEncounterResetSite site) {

			foreach (var cubeGrid in site.Grids) {

				var name = cubeGrid.CustomName ?? cubeGrid.EntityId.ToString();

				if (HasPlayerOwner(cubeGrid))
					site.Blockers.Add("player-owned blocks on encounter grid '" + name + "'");

				var gridEntity = GridManager.GetGridEntity(cubeGrid, true);

				if (gridEntity?.Npc != null && gridEntity.Npc.SpawnGroupName != site.SpawnGroupName)
					site.Blockers.Add("attached grid " + DescribeGrid(cubeGrid) + " belongs to another encounter");

				if (gridEntity == null)
					continue;

				foreach (var block in gridEntity.AllTerminalBlocks) {

					var controller = block?.Block as IMyShipController;

					if (controller == null || controller.Pilot == null)
						continue;

					if (IsPlayerCharacter(controller.Pilot))
						site.Blockers.Add("player seated in '" + controller.CustomName + "' on '" + name + "'");

				}

			}

			foreach (var cubeGrid in site.Grids) {

				_tempGrids.Clear();
				MyAPIGateway.GridGroups.GetGroup(cubeGrid, GridLinkTypeEnum.Physical, _tempGrids);

				foreach (var attached in _tempGrids) {

					if (attached == null || site.Grids.Contains(attached))
						continue;

					var text = "attached grid " + DescribeGrid(attached);

					if (!site.Blockers.Contains(text))
						site.Blockers.Add(text);

				}

			}

			for (int i = GridManager.Grids.Count - 1; i >= 0; i--) {

				var grid = GridManager.GetSafeGridFromIndex(i);

				if (grid?.CubeGrid == null || !grid.ActiveEntity() || site.Grids.Contains(grid.CubeGrid))
					continue;

				var inFootprint = site.HasFootprint && site.Footprint.Intersects(grid.CubeGrid.PositionComp.WorldAABB);
				var distance = grid.Distance(site.SpawnCoords);

				if (!inFootprint && distance >= SpawnClearance)
					continue;

				var text = "grid " + DescribeGrid(grid.CubeGrid) + (inFootprint ? " inside the encounter's current area" : string.Format(" {0:0}m from the spawn point", distance));

				if (!site.Blockers.Contains(text))
					site.Blockers.Add(text);

			}

			_tempCheckedCharacters.Clear();
			_tempPlayers.Clear();
			MyAPIGateway.Players.GetPlayers(_tempPlayers);

			foreach (var player in _tempPlayers) {

				if (player == null || player.IsBot)
					continue;

				if (player.Character != null)
					_tempCheckedCharacters.Add(player.Character.EntityId);

				if (player.Character == null && player.Controller?.ControlledEntity?.Entity == null)
					continue;

				if (IsInsideSite(site, player.GetPosition()))
					site.Blockers.Add("player " + (player.DisplayName ?? player.SteamUserId.ToString()));

			}

			_tempEntities.Clear();
			MyAPIGateway.Entities.GetEntities(_tempEntities, e => e is IMyCharacter);

			foreach (var entity in _tempEntities) {

				var character = entity as IMyCharacter;

				if (character == null || _tempCheckedCharacters.Contains(character.EntityId))
					continue;

				if (!IsPlayerCharacter(character) || !IsInsideSite(site, character.GetPosition()))
					continue;

				site.Blockers.Add("character '" + (character.DisplayName ?? character.EntityId.ToString()) + "'");

			}

			_tempEntities.Clear();
			_tempPlayers.Clear();
			_tempGrids.Clear();

		}

		private static string DescribeAction(StaticEncounterResetSite site) {

			return string.Format("remove {0} grid(s), then reset and spawn at {1:0},{2:0},{3:0}", site.Grids.Count, site.SpawnCoords.X, site.SpawnCoords.Y, site.SpawnCoords.Z);

		}

		private static void Execute(StaticEncounterResetSite site, long requesterId) {

			InProgress.Add(site.SpawnGroupName);

			foreach (var cubeGrid in site.Grids) {

				if (cubeGrid == null || cubeGrid.MarkedForClose)
					continue;

				SpawnLogger.Write(string.Format("[{0}] Reset: removing grid '{1}' ({2})", site.SpawnGroupName, cubeGrid.CustomName ?? "null", cubeGrid.EntityId), SpawnerDebugEnum.SpawnRecord);

				var myGrid = cubeGrid as MyCubeGrid;

				if (myGrid != null)
					myGrid.DismountAllCockpits();

				cubeGrid.Close();

			}

			TaskProcessor.Tasks.Add(new StaticEncounterResetTask(site, requesterId));

		}

		public static void CompleteReset(StaticEncounterResetSite site, long requesterId) {

			var name = site.SpawnGroupName;

			if (site.Spawned)
				NpcManager.ResetThisStaticEncounter(name);

			StaticEncounter pending = null;

			foreach (var encounter in NpcManager.StaticEncounters) {

				if (encounter != null && encounter.IsValid && encounter.SpawnGroupName == name) {

					pending = encounter;
					break;

				}

			}

			if (pending == null) {

				Report(name, "FAILED: the encounter could not be reset (see /MES.IGLSD.Startup)", requesterId, "Red");
				return;

			}

			bool update = false;
			pending.ProcessEncounter(ref update, true, pending.TriggerCoords);
			NpcManager.UpdateStaticEncounters();

			if (!pending.IsValid && NpcManager.UniqueGroupsSpawned.Contains(name)) {

				Report(name, "Successful Reset, new copy spawned", requesterId, "Green");

			} else if (!pending.IsValid) {

				Report(name, "FAILED: the encounter expired (time limit or max spawn attempts)", requesterId, "Red");

			} else {

				Report(name, string.Format("reset, but it could not spawn right away (see /MES.IGLSD.Spawning); it will spawn when a player comes within {0:0}m", pending.TriggerRadius), requesterId, "Yellow");

			}

		}

		public static void Report(string spawnGroupName, string result, long requesterId, string color) {

			var text = "Reset Static Encounter [" + spawnGroupName + "]: " + result;
			SpawnLogger.Write(text, SpawnerDebugEnum.SpawnRecord);

			if (requesterId != 0)
				MyVisualScriptLogicProvider.ShowNotification(text, 8000, color, requesterId);

		}

	}

}
