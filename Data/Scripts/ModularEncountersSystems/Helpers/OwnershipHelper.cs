using ModularEncountersSystems.Logging;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;

namespace ModularEncountersSystems.Helpers {

	public class OwnershipHelper {

		public static void ChangeAntennaBlockOwnership(List<IMyRadioAntenna> blocks, string factionTag){

			var faction = MyAPIGateway.Session.Factions.TryGetFactionByTag(factionTag);
			long owner = 0;

			if (faction != null)
				owner = FactionHelper.GetFactionOwner(faction);
			else {

				BehaviorLogger.Write("   - Antenna Ownership Change Fail. Faction Not Found", BehaviorDebugEnum.Action);
				return;

			}
				

			foreach (var block in blocks) {

				if (block == null)
					continue;

				var cubeBlock = block as MyCubeBlock;
				//var cubeGrid = block.SlimBlock.CubeGrid as MyCubeGrid;
				BehaviorLogger.Write(" - Antenna Ownership Changed From/To: " + block.OwnerId + " / " + owner, BehaviorDebugEnum.Action);
				//cubeGrid.ChangeOwnerRequest(cubeGrid, cubeBlock, owner, MyOwnershipShareModeEnum.Faction);

				//var cubeBlock = block as MyCubeBlock;
				cubeBlock.ChangeOwner(owner, MyOwnershipShareModeEnum.Faction);
				cubeBlock.ChangeBlockOwnerRequest(owner, MyOwnershipShareModeEnum.Faction);

			}

		}
		
		public static bool DoesGridHaveHostileOwnership(IMyCubeGrid targetGrid, long myIdentity, bool includeNpcOwnership = false) {

			var gridGroup = MyAPIGateway.GridGroups.GetGroup(targetGrid, GridLinkTypeEnum.Logical);
			var ownerList = new List<long>();

			foreach(var grid in gridGroup) {

				var tempList = new List<long>(grid.BigOwners.ToList());
				tempList = tempList.Concat(grid.SmallOwners).ToList();
				var resultList = tempList.Except(ownerList).ToList();
				ownerList = ownerList.Concat(resultList).ToList();

			}

			foreach(var owner in ownerList) {

				if(owner == 0) {

					continue;

				}

				if(includeNpcOwnership == false && IsNPC(owner) == true) {

					continue;

				}

				if(GetReputation(myIdentity, owner) < -500) {

					return true;

				}

			}

			return false;

		}

		public long GetOwnershipFromGrid(IMyTerminalBlock block) {

			if (block?.SlimBlock?.CubeGrid?.BigOwners != null) {

				if (block.SlimBlock.CubeGrid.BigOwners.Count > 0) {

					foreach (var owner in block.SlimBlock.CubeGrid.BigOwners) {

						if (owner != 0)
							return owner;
					
					}
				
				}
			
			}

			return 0;
		
		}

		public static TargetOwnerEnum GetOwnershipTypes(IMyCubeGrid cubeGrid, bool includeSmallOwners) {

			if(cubeGrid.BigOwners.Count == 0) {

				return GetOwnershipTypes(new List<long> { 0 });

			}

			var ownerList = new List<long>(cubeGrid.BigOwners.ToList());

			if(includeSmallOwners == true) {

				ownerList = ownerList.Concat(cubeGrid.SmallOwners.ToList()).ToList();

			}

			return GetOwnershipTypes(ownerList);

		}

		public static TargetOwnerEnum GetOwnershipTypes(IMyTerminalBlock block) {

			return GetOwnershipTypes(new List<long> { block.OwnerId });

		}

		private static TargetOwnerEnum GetOwnershipTypes(List<long> identities) {

			TargetOwnerEnum result = 0;

			foreach(var identity in identities) {

				if(identity == 0 && result.HasFlag(TargetOwnerEnum.Unowned) == false) {

					result |= TargetOwnerEnum.Unowned;
					continue;

				}

				if(IsNPC(identity) == true && result.HasFlag(TargetOwnerEnum.NPC) == false) {

					result |= TargetOwnerEnum.NPC;
					continue;

				}

				if(IsNPC(identity) == false && result.HasFlag(TargetOwnerEnum.Player) == false) {

					result |= TargetOwnerEnum.Player;
					continue;

				}

			}

			return result;

		}

		public static TargetRelationEnum GetTargetReputation(long myIdentity, IMyTerminalBlock block) {

			var ownerList = new List<long>();

			if (block.OwnerId == 0) {

				return GetTargetReputation(myIdentity, new List<long> { 0 });

			} else {

				ownerList.Add(block.OwnerId);

			}

			return GetTargetReputation(myIdentity, ownerList);

		}

		public static TargetRelationEnum GetTargetReputation(long myIdentity, IMyCubeGrid cubeGrid, bool includeSmallOwners = false) {

			if(cubeGrid.BigOwners.Count == 0) {

				return GetTargetReputation(myIdentity, new List<long> { 0 });

			}

			var ownerList = new List<long>(cubeGrid.BigOwners.ToList());

			if(includeSmallOwners == true) {

				ownerList = ownerList.Concat(cubeGrid.SmallOwners.ToList()).ToList();

			}

			return GetTargetReputation(myIdentity, ownerList);

		}

		public static TargetRelationEnum GetTargetReputation(long myIdentity, List<long> identities) {

			var myFaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(myIdentity);

			if(myFaction == null) {

				return TargetRelationEnum.None;

			}

			var result = TargetRelationEnum.None;

			if(identities.Count == 0) {

				result |= TargetRelationEnum.Unowned;

			}

			foreach(var identity in identities) {

				if(myFaction.IsMember(identity) == true && result.HasFlag(TargetRelationEnum.Faction) == false) {

					result |= TargetRelationEnum.Faction;
					continue;

				}

				if(identity == 0) {

					result |= TargetRelationEnum.Unowned;
					continue;

				}

				var repScore = MyAPIGateway.Session.Factions.GetReputationBetweenPlayerAndFaction(identity, myFaction.FactionId);

				if(repScore < -500 && result.HasFlag(TargetRelationEnum.Enemy) == false) {

					result |= TargetRelationEnum.Enemy;
					continue;

				}

				if(repScore >= -500 && repScore <= 500 && result.HasFlag(TargetRelationEnum.Neutral) == false) {

					result |= TargetRelationEnum.Neutral;
					continue;

				}

				if(repScore > 500 && result.HasFlag(TargetRelationEnum.Friend) == false) {

					result |= TargetRelationEnum.Friend;
					continue;

				}

			}

			return result;

		}

		public static int GetReputation(long myIdentity, long theirIdentity) {

			var myFaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(myIdentity);

			if(myFaction == null) {

				return -1500;

			}

			if(myFaction.IsMember(myIdentity) == true || theirIdentity == 0) {

				return 0;

			}

			return MyAPIGateway.Session.Factions.GetReputationBetweenPlayerAndFaction(theirIdentity, myFaction.FactionId);

		}

		public static bool IsFactionMember(long myIdentity, long theirIdentity) {

			var myFaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(myIdentity);

			if(myFaction == null) {

				return false;

			}

			return myFaction.IsMember(theirIdentity);

		}

		public static bool IsNPC(long identity, bool unownedAllowed = false) {

			if(MyAPIGateway.Players.TryGetSteamId(identity) > 0 || identity == 0) {

				if(!unownedAllowed)
					return false;

			}

			return true;

		}

	}

}
