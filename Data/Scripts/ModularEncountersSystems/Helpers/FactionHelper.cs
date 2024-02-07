using ModularEncountersSystems.Behavior.Subsystems.Trigger;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Sync;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Helpers {
	public static class FactionHelper {

		public static List<IMyFaction> NpcFactions = new List<IMyFaction>();
		public static List<IMyFaction> NpcBuilderFactions = new List<IMyFaction>();
		public static List<IMyFaction> NpcMinerFactions = new List<IMyFaction>();
		public static List<IMyFaction> NpcTraderFactions = new List<IMyFaction>();

		public static List<string> NpcFactionTags = new List<string>();
		public static List<string> EconomyFactionTags = new List<string>();
		public static List<string> EconomyStationTypes = new List<string>();

		internal static List<MyFactionMember> _memberList = new List<MyFactionMember>();

		public static bool IsIdentityNPC(long id) {

			return !IsIdentityPlayer(id);

		}

		public static bool IsIdentityPlayer(long id) {

			return MyAPIGateway.Players.TryGetSteamId(id) > 0;

		}

		public static long GetFactionOwner(IMyFaction faction) {

			if (faction?.Members == null)
				return 0;

			if (faction.Members.Count == 0) {

				lock (_memberList) {

					_memberList.Clear();

					foreach (var member in faction.Members)
						_memberList.Add(member.Value);

					var factionDef = MyDefinitionManager.Static.TryGetFactionDefinition(faction.Tag);

					if (factionDef != null) {

						MyAPIGateway.Session.Factions.AddNewNPCToFaction(faction.FactionId, factionDef?.Founder ?? faction.Name + " Leader");

						foreach (var member in _memberList)
							MyAPIGateway.Session.Factions.PromoteMember(faction.FactionId, member.PlayerId);

					}
					
				}
				
			}

			if (faction.FounderId != 0)
				return faction.FounderId;

			long result = 0;

			foreach (var member in faction.Members.Keys) {

				var factionMember = faction.Members[member];

				if (IsIdentityNPC(factionMember.PlayerId)) {

					result = factionMember.PlayerId;
					break;

				}
			
			}

			return result;

		}

		public static long GetFactionMemberIdFromTag(string tag) {

			foreach (var faction in NpcFactions) {

				if (faction?.Tag == null || tag != faction.Tag)
					continue;

				return GetFactionOwner(faction);
			
			}

			return 0;
		
		}



		public static void PopulateNpcFactionLists() {

			
			EconomyStationTypes.Add("MiningStation");
			EconomyStationTypes.Add("OrbitalStation");
			EconomyStationTypes.Add("Outpost");
			EconomyStationTypes.Add("SpaceStation");

			var identities = new List<IMyIdentity>();
			MyAPIGateway.Players.GetAllIdentites(identities);

			foreach (var id in MyAPIGateway.Session.Factions.Factions.Keys) {

				var faction = MyAPIGateway.Session.Factions.Factions[id];
				bool playerDetected = false;

				foreach (var member in faction.Members.Keys) {

					if (IsIdentityPlayer(faction.Members[member].PlayerId)) {

						var identityBlank = false;

						foreach (var ident in identities) {

							if (ident.IdentityId == faction.Members[member].PlayerId)
								if (string.IsNullOrWhiteSpace(ident.DisplayName)) {

									identityBlank = true;
									break;

								}
						
						}

						if (!identityBlank) {

							playerDetected = true;
							break;

						}
					
					}
				
				}

				if (playerDetected)
					continue;

				NpcFactions.Add(faction);
				NpcFactionTags.Add(faction.Tag);

				foreach (var factionOB in MyAPIGateway.Session.Factions.GetObjectBuilder().Factions) {

					if (faction.Tag != factionOB.Tag) {

						continue;

					}

					if (factionOB.FactionType == MyFactionTypes.Miner) {

						EconomyFactionTags.Add(faction.Tag);
						NpcMinerFactions.Add(faction);

					}

					if (factionOB.FactionType == MyFactionTypes.Trader) {

						EconomyFactionTags.Add(faction.Tag);
						NpcTraderFactions.Add(faction);

					}

					if (factionOB.FactionType == MyFactionTypes.Builder) {

						EconomyFactionTags.Add(faction.Tag);
						NpcBuilderFactions.Add(faction);

					}

				}

			}

		}

		public static void ChangeDamageOwnerReputation(IMyRemoteControl remoteControl, List<string> factions, long attackingEntity, List<int> amounts, bool applyChangeToAttackerFaction, int minRep, int maxRep) {

			if (amounts.Count != factions.Count) {

				BehaviorLogger.Write("Could Not Do Reputation Change. Faction Tag and Rep Amount Counts Do Not Match", BehaviorDebugEnum.Action);
				return;

			}

			var owner = DamageHelper.GetAttackOwnerId(attackingEntity);

			if (owner == 0) {

				BehaviorLogger.Write("No Owner From Provided Id: " + attackingEntity, BehaviorDebugEnum.Action);
				return;

			}

			var ownerList = new List<long>();
			ownerList.Add(owner);
			ChangePlayerReputationWithFactions(remoteControl, amounts, ownerList, factions, applyChangeToAttackerFaction, minRep, maxRep);

		}

		public static void ChangeReputationWithPlayersInRadius(IMyRemoteControl remoteControl, double radius, List<int> amounts, List<string> factions, bool applyReputationChangeToFactionMembers, int minRep, int maxRep, List<string> ReputationPlayerConditionIds = null) {

			if (amounts.Count != factions.Count) {

				BehaviorLogger.Write("Could Not Do Reputation Change. Faction Tag and Rep Amount Counts Do Not Match", BehaviorDebugEnum.Action);
				return;

			}

			var playerList = new List<IMyPlayer>();
			var playerIds = new List<long>();
			MyAPIGateway.Players.GetPlayers(playerList);

			foreach (var player in playerList) {

				if (player.IsBot == true)
					continue;

				if (Vector3D.Distance(player.GetPosition(), remoteControl.GetPosition()) > radius)
					continue;

				if (ReputationPlayerConditionIds != null && ReputationPlayerConditionIds.Count>0)
					if(!PlayerCondition.ArePlayerConditionsMet(ReputationPlayerConditionIds, player.IdentityId))
						continue;

	


				if (player.IdentityId != 0 && !playerIds.Contains(player.IdentityId))
					playerIds.Add(player.IdentityId);


			}

			ChangePlayerReputationWithFactions(remoteControl, amounts, playerIds, factions, applyReputationChangeToFactionMembers, minRep, maxRep);

		}

		public static void ChangePlayerReputationWithFactions(IMyRemoteControl remoteControl, List<int> amounts, List<long> players, List<string> factionTags, bool applyReputationChangeToFactionMembers, int minRep, int maxRep) {

			var allPlayerIds = new List<long>(players.ToList());

			if (applyReputationChangeToFactionMembers) {

				foreach (var owner in players) {

					var ownerFaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(owner);

					if (ownerFaction != null) {

						foreach (var member in ownerFaction.Members.Keys) {

							if (member != owner && member != 0 && !allPlayerIds.Contains(member))
								allPlayerIds.Add(member);

						}

					}

				}

			}

			for (int i = 0; i < factionTags.Count; i++) {

				var tag = factionTags[i];
				var amount = amounts[i];

				if(tag == "{Self}" && remoteControl != null)
				{
					tag = remoteControl.GetOwnerFactionTag();

				}
				var faction = MyAPIGateway.Session.Factions.TryGetFactionByTag(tag);

				if (faction == null)
					continue;

				foreach (var playerId in players) {

					string color = "Red";
					string modifier = "Decreased";
					var oldRep = MyAPIGateway.Session.Factions.GetReputationBetweenPlayerAndFaction(playerId, faction.FactionId);

					if ((oldRep <= -1500 && amount < 0) || (oldRep >= 1500 && amount > 0))
						continue;

					if (amount > 0) {

						color = "Green";
						modifier = "Increased";

					}

					int proposedRep = oldRep + amount;

					int newRep = oldRep;

					if (proposedRep >= minRep && proposedRep <= maxRep)
						newRep = MathHelper.Clamp(oldRep + amount, minRep, maxRep);
					else {
					
						if((proposedRep < minRep && Math.Sign(amount) == 1) || (proposedRep > minRep && Math.Sign(amount) == -1))
							newRep = oldRep + amount;

					}

					//Extra check
					newRep = MathHelper.Clamp(proposedRep, minRep, maxRep);

					RelationManager.SetReputationWithFaction(playerId, faction.FactionId, newRep);
					//MyVisualScriptLogicProvider.ShowNotification(string.Format("Reputation With {0} {1} By: {2}", faction.Tag, modifier, amount.ToString()), 2000, color, playerId);

					var steamId = MyAPIGateway.Players.TryGetSteamId(playerId);

					if (steamId > 0) {

						//BehaviorLogger.Write("Send Rep Sync Message", BehaviorDebugEnum.Owner);
						var message = new ReputationMessage(amount, tag, steamId);
						var syncContainer = new SyncContainer(message);
						SyncManager.SendSyncMesage(syncContainer, steamId);

					}

				}

			}

		}

		public static bool CompareAllowedOwnerTypes(TargetOwnerEnum allowedOwner, TargetOwnerEnum resultOwner) {

			//Owner: Unowned
			if (allowedOwner.HasFlag(TargetOwnerEnum.Unowned) && resultOwner.HasFlag(TargetOwnerEnum.Unowned)) {

				return true;

			}

			//Owner: Owned
			if (allowedOwner.HasFlag(TargetOwnerEnum.Owned) && resultOwner.HasFlag(TargetOwnerEnum.Owned)) {

				return true;

			}

			//Owner: Player
			if (allowedOwner.HasFlag(TargetOwnerEnum.Player) && resultOwner.HasFlag(TargetOwnerEnum.Player)) {

				return true;

			}

			//Owner: NPC
			if (allowedOwner.HasFlag(TargetOwnerEnum.NPC) && resultOwner.HasFlag(TargetOwnerEnum.NPC)) {

				return true;

			}

			return false;

		}

		public static bool CompareAllowedReputation(TargetRelationEnum allowedRelations, TargetRelationEnum resultRelation) {

			if (allowedRelations.HasFlag(TargetRelationEnum.Faction) && resultRelation.HasFlag(TargetRelationEnum.Faction)) {

				return true;

			}

			//Relation: Neutral
			if (allowedRelations.HasFlag(TargetRelationEnum.Neutral) && resultRelation.HasFlag(TargetRelationEnum.Neutral)) {

				return true;

			}

			//Relation: Enemy
			if (allowedRelations.HasFlag(TargetRelationEnum.Enemy) && resultRelation.HasFlag(TargetRelationEnum.Enemy)) {

				return true;

			}

			//Relation: Friend
			if (allowedRelations.HasFlag(TargetRelationEnum.Friend) && resultRelation.HasFlag(TargetRelationEnum.Friend)) {

				return true;

			}

			//Relation: Unowned
			if (allowedRelations.HasFlag(TargetRelationEnum.Unowned) && resultRelation.HasFlag(TargetRelationEnum.Unowned)) {

				return true;

			}

			return false;

		}

	}

}
