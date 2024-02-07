using ModularEncountersSystems.Configuration;
using ProtoBuf;
using Sandbox.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Progression
{
	//Not Synced, unlike ProgressionContainer
	[ProtoContract]
	public class ProgressionDataContainer
	{
		//Identity Info
		[ProtoMember(1)] public ulong SteamId;
		[ProtoMember(2)] public long IdentityId;
		[ProtoMember(3)] public string LastRespawnShipName;
		[ProtoMember(4)] public List<string> Tags;


		public ProgressionDataContainer()
		{
			IdentityId = 0;
			SteamId = 0;
			LastRespawnShipName = "";
			Tags = new List<string>();
		}



	}
}