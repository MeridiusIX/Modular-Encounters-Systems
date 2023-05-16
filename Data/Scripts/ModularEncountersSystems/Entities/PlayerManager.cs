using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Progression;
using ModularEncountersSystems.Terminal;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Entities {
	public static class PlayerManager {

		public static List<IMyPlayer> ActivePlayers = new List<IMyPlayer>();
		public static List<PlayerEntity> Players = new List<PlayerEntity>();

		public static Action<PlayerEntity> NewPlayerDetected;
		public static Action UnloadEntities;

		

		public static PlayerEntity GetNearestPlayer(Vector3D coords) {

			PlayerEntity result = null;
			double distance = -1;

			foreach (var player in Players) {

				if (!player.ActiveEntity())
					continue;

				var dist = player.Distance(coords);

				if (distance > -1 && dist > distance)
					continue;

				distance = dist;
				result = player;
			
			}

			return result;
		
		}

		public static PlayerEntity GetPlayerUsingTool(IMyEntity entity) {

			foreach (var player in Players) {

				if (!player.ActiveEntity())
					continue;

				if (player.Player.Character?.EquippedTool == entity)
					return player;

			}

			return null;

		}

		public static PlayerEntity GetPlayerWithCharacter(IMyCharacter character) {

			foreach (var player in Players) {

				if (!player.ActiveEntity())
					continue;

				if (player.Player.Character == character)
					return player;

			}

			return null;

		}

		public static PlayerEntity GetPlayerWithIdentityId(long id) {

			foreach (var player in Players) {

				if (!player.ActiveEntity())
					continue;

				if (player.Player.IdentityId == id)
					return player;

			}

			return null;

		}

		public static void ItemConsumedEvent(IMyCharacter character, MyDefinitionId id) {

			foreach (var player in Players) {

				player.ItemConsumedEvent(character, id);
			
			}
		
		}

		public static void PlayerConnectEvent(long playerId) {

			MyAPIGateway.Utilities.InvokeOnGameThread(() => { RefreshAllPlayers(true); });
			RelationManager.RunCount = 0;
			RelationManager.PlayerConnected = true;


		}

		public static void RefreshAllPlayers(bool forceCheck = false) {

			if (!forceCheck)
				return;

			lock (ActivePlayers) {

				ActivePlayers.Clear();
				MyAPIGateway.Players.GetPlayers(ActivePlayers);

				foreach (var player in ActivePlayers) {

					if (string.IsNullOrWhiteSpace(player.DisplayName))
						continue;

					bool foundExisting = false;

					for (int i = Players.Count - 1; i >= 0; i--) {

						var playerEnt = Players[i];

						if (playerEnt.Player == null)
							continue;

						if (playerEnt.Player.SteamUserId == player.SteamUserId) {

							foundExisting = true;

							if (playerEnt.Player.IdentityId != player.IdentityId) {

								playerEnt.Player = player;

							}

						}

					}

					if (foundExisting)
						continue;

					if (!player.IsBot && player.SteamUserId > 0) {

						var playerEntity = new PlayerEntity(player);
						var progression = playerEntity.Progression;
						playerEntity.InitSolarModule();
						UnloadEntities += playerEntity.Unload;
						Players.Add(playerEntity);
						NewPlayerDetected?.Invoke(playerEntity);

					}

				}

			}

		}

		public static ProgressionContainer GetProgressionContainer(long identityId, ulong steamId) {

			lock (Players) {

				foreach (var player in Players) {

					if (player?.Player == null)
						continue;

					var container = player.Progression;

					if (container.IdentityId == identityId && container.SteamId == steamId) {

						return container;

					}

				}
			
			}

			return null;

		}

	}

}
