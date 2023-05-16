using ProtoBuf;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Utils;

namespace ModularEncountersSystems.Sync {

	[ProtoContract]
	public class ReputationChange {

		[ProtoMember(1)] public long Player;
		[ProtoMember(2)] public long Faction;
		[ProtoMember(3)] public int Reputation;
		[ProtoMember(4)] public bool Valid;

		public ReputationChange() {

			Player = 0;
			Faction = 0;
			Reputation = 0;
			Valid = false;

		}

		public ReputationChange(long player, long faction, int rep) {

			Player = player;
			Faction = faction;
			Reputation = rep;
			Valid = true;

		}

		public void Process() {

			if (!Valid)
				return;

			MyAPIGateway.Session.Factions.SetReputationBetweenPlayerAndFaction(Player, Faction, Reputation);
			//MyLog.Default.WriteLineAndConsole("MES Spawner / Client: NPC Faction Rep Change: " + Faction.ToString() + " / " + Reputation.ToString());

		}

	}

}
