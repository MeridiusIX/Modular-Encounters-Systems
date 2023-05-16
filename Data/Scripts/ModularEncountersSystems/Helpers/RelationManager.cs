using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Sync;
using ModularEncountersSystems.Tasks;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.ModAPI;

namespace ModularEncountersSystems.Helpers {

	public static class RelationManager {

		public static bool RunAgain = false;
		public static bool SetupDone = false;
		public static bool PlayerConnected = true;

		public static List<long> NeutralNpcFactions = new List<long>();
		public static List<long> FriendsNpcFactions = new List<long>();
		public static Dictionary<long, long> SetNeutralRelations = new Dictionary<long, long>();
		public static Dictionary<long, long> SetFriendsRelations = new Dictionary<long, long>();
		public static List<string> PreviouslySetRelations = new List<string>();

		public static byte RunCount = 10;

		public static void Setup() {

			var factionList = MyDefinitionManager.Static.GetDefaultFactions();

			foreach (var factionDef in factionList) {

				var faction = MyAPIGateway.Session.Factions.TryGetFactionByTag(factionDef.Tag);

				if (faction == null) {

					continue;

				}

				if (factionDef.Context != null) {

					if (string.IsNullOrWhiteSpace(factionDef.Context.ModId) == false) {

						//EFM-Wico
						if (factionDef.Context.ModId.Contains("1301917772") == true) {

							continue;

						}

						//EEM
						if (factionDef.Context.ModId.Contains("531659576") == true) {

							continue;

						}

						//EEM-Unstable
						if (factionDef.Context.ModId.Contains("1508213460") == true) {

							continue;

						}

					}

				}

				if (factionDef.DefaultRelation == MyRelationsBetweenFactions.Enemies || factionDef.DefaultRelationToPlayers == MyRelationsBetweenFactions.Enemies) {

					continue;

				}

				if (factionDef.DefaultRelation == MyRelationsBetweenFactions.Neutral || factionDef.DefaultRelationToPlayers == MyRelationsBetweenFactions.Neutral) {

					NeutralNpcFactions.Add(faction.FactionId);

				}

				if (factionDef.DefaultRelation == MyRelationsBetweenFactions.Friends && factionDef.DefaultRelationToPlayers == MyRelationsBetweenFactions.Friends) {

					FriendsNpcFactions.Add(faction.FactionId);

				}

			}

			TaskProcessor.Tick60.Tasks += ScheduledRun;
			SetupDone = true;

		}

		public static bool ResetFactionReputation(string factionTag) {

			if (SetupDone == false) {

				SetupDone = true;
				Setup();

			}

			var faction = MyAPIGateway.Session.Factions.TryGetFactionByTag(factionTag);

			if (faction == null)
				return false;

			lock (PreviouslySetRelations) {

				PreviouslySetRelations.Clear();
				string previousRelationsArray = "";

				if (MyAPIGateway.Utilities.GetVariable("MES-FixedDefaultNpcRelations", out previousRelationsArray) == true) {

					var bytes = Convert.FromBase64String(previousRelationsArray);
					PreviouslySetRelations = MyAPIGateway.Utilities.SerializeFromBinary<List<string>>(bytes);


				}

				for (int i = PreviouslySetRelations.Count - 1; i >= 0; i--) {

					var id = PreviouslySetRelations[i];

					if (id.Contains(faction.FactionId.ToString()))
						PreviouslySetRelations.RemoveAt(i);

				}

				int defaultRep = -1000;

				if (NeutralNpcFactions.Contains(faction.FactionId))
					defaultRep = 0;

				if (FriendsNpcFactions.Contains(faction.FactionId))
					defaultRep = 1000;

				var identities = new List<IMyIdentity>();
				MyAPIGateway.Players.GetAllIdentites(identities);

				foreach (var identity in identities) {

					ulong steamId = MyAPIGateway.Players.TryGetSteamId(identity.IdentityId);

					if (steamId > 0)
						SetReputationWithFaction(identity.IdentityId, faction.FactionId, defaultRep);

				}

			}

			return true;

		}

		public static void SetReputation(long playerId, string factionTag) {
		
			
		
		}

		public static void ScheduledRun() {

			if (!PlayerConnected)
				return;

			RunCount++;

			if (RunCount >= 10) {

				PlayerConnected = false;
				InitialReputationFixer();
				RunCount = 0;
			
			}

		}

		public static void InitialReputationFixer() {

			if (SetupDone == false) {

				SetupDone = true;
				Setup();

			}

			lock (PreviouslySetRelations) {

				PreviouslySetRelations.Clear();
				string previousRelationsArray = "";

				if (MyAPIGateway.Utilities.GetVariable("MES-FixedDefaultNpcRelations", out previousRelationsArray) == true) {

					var bytes = Convert.FromBase64String(previousRelationsArray);
					PreviouslySetRelations = MyAPIGateway.Utilities.SerializeFromBinary<List<string>>(bytes);

				}

				for (int i = PlayerManager.Players.Count - 1; i >= 0; i--) {

					if (!PlayerManager.Players[i].ActiveEntity())
						continue;

					var player = PlayerManager.Players[i].Player;

					if (player.IsBot == true || player.Character == null) {

						continue;

					}

					foreach (var neutral in NeutralNpcFactions) {

						string identityString = player.IdentityId.ToString() + "-" + neutral.ToString();

						if (PreviouslySetRelations.Contains(identityString) == true) {

							continue;

						}

						if (SetNeutralRelations.ContainsKey(player.IdentityId) == false) {

							SetNeutralRelations.Add(player.IdentityId, neutral);
							PreviouslySetRelations.Add(identityString);
							RunAgain = true;
							break;

						}

					}

					foreach (var friends in FriendsNpcFactions) {

						string identityString = player.IdentityId.ToString() + "-" + friends.ToString();

						if (PreviouslySetRelations.Contains(identityString) == true) {

							continue;

						}

						if (SetFriendsRelations.ContainsKey(player.IdentityId) == false) {

							SetFriendsRelations.Add(player.IdentityId, friends);
							PreviouslySetRelations.Add(identityString);
							RunAgain = true;
							break;

						}

					}

				}

				var newbytes = MyAPIGateway.Utilities.SerializeToBinary(PreviouslySetRelations);
				string storage = Convert.ToBase64String(newbytes);
				MyAPIGateway.Utilities.SetVariable("MES-FixedDefaultNpcRelations", storage);

			}

			foreach (var player in SetNeutralRelations.Keys) {

				if (MyAPIGateway.Session.Factions.GetReputationBetweenPlayerAndFaction(player, SetNeutralRelations[player]) < -499) {

					SetReputationWithFaction(player, SetNeutralRelations[player], 0);

				}

			}

			foreach (var player in SetFriendsRelations.Keys) {

				if (MyAPIGateway.Session.Factions.GetReputationBetweenPlayerAndFaction(player, SetFriendsRelations[player]) < -499) {

					SetReputationWithFaction(player, SetFriendsRelations[player], 500);

				}

			}

			SetNeutralRelations.Clear();
			SetFriendsRelations.Clear();

			if (RunAgain == true) {

				MyAPIGateway.Utilities.InvokeOnGameThread(() => {

					RunAgain = false;
					InitialReputationFixer();

				});
				
			}

		}

		public static void SetReputationWithFaction(long player, long faction, int reputation) {

			MyAPIGateway.Session.Factions.SetReputationBetweenPlayerAndFaction(player, faction, reputation);
			var local = MyAPIGateway.Session.LocalHumanPlayer?.IdentityId ?? 0;

			if (local != player) {

				var repChange = new ReputationChange(player, faction, reputation);
				var data = MyAPIGateway.Utilities.SerializeToBinary<ReputationChange>(repChange);
				var syncContainer = new SyncContainer(SyncMode.ReputationChangeClient, data);
				SyncManager.SendSyncMesage(syncContainer, MyAPIGateway.Players.TryGetSteamId(player));
			
			}


		}

	}

}