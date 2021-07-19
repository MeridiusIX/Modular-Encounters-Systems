using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Spawning.Profiles;
using ProtoBuf;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Game.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.World {

	[ProtoContract]
	public class LegacyActiveNPC{

		[ProtoMember(1)]
		public string SpawnGroupName;
		
		[ProtoMember(2)]
		public string Name;
		
		[ProtoMember(3)]
		public string GridName;
		
		[ProtoMember(4)]
		public string InitialFaction;
		
		[ProtoMember(5)]
		public Vector3D StartCoords;
		
		[ProtoMember(6)]
		public Vector3D EndCoords;
		
		[ProtoMember(7)]
		public Vector3D CurrentCoords;
		
		[ProtoMember(8)]
		public float AutoPilotSpeed;
		
		[ProtoMember(9)]
		public bool CleanupIgnore;
		
		[ProtoMember(10)]
		public int CleanupTime;
		
		[ProtoMember(11)]
		public bool KeenBehaviorCheck;

		[ProtoMember(12)]
		public string SpawnType;

		[ProtoMember(13)]
		public bool CargoShipOverride;

		public LegacyActiveNPC(){
			
			SpawnGroupName = "";
			Name = "";
			GridName = "";
			InitialFaction = "";
			
			StartCoords = Vector3D.Zero;
			EndCoords = Vector3D.Zero;
			CurrentCoords = Vector3D.Zero;
			
			AutoPilotSpeed = 0;
			
			SpawnType = "Other";
			CargoShipOverride = false;
			CleanupIgnore = false;
			CleanupTime = 0;
			KeenBehaviorCheck = false;
			
		}

	}
	
}