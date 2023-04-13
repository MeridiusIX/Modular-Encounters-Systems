using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning.Profiles;
using ModularEncountersSystems.Watchers;
using ModularEncountersSystems.World;
using ProtoBuf;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Spawning {

	[ProtoContract]
	public class StaticEncounter {

		//Serializable

		[ProtoMember(1)]
		public bool CheckSpawnConditions;

		[ProtoMember(2)]
		public string SpawnGroupName;

		[ProtoMember(3)]
		public int ConditionIndex;

		[ProtoMember(4)]
		public string Faction;

		[ProtoMember(5)]
		public Vector3D TriggerCoords;

		[ProtoMember(6)]
		public double TriggerRadius;

		[ProtoMember(7)]
		public bool UseSpecificPlayers;

		[ProtoMember(8)]
		public List<long> SpecificPlayers;

		[ProtoMember(9)]
		public Dictionary<long, int> GpsHashData;

		[ProtoMember(10)]
		public bool UseTimeLimit;

		[ProtoMember(11)]
		public DateTime StartTime;

		[ProtoMember(12)]
		public int TimeLimit;

		[ProtoMember(13)]
		public bool UseExactSpawnLocation;

		[ProtoMember(14)]
		public Vector3D ExactLocationCoords;

		[ProtoMember(15)]
		public Vector3D ExactLocationForward;

		[ProtoMember(16)]
		public Vector3D ExactLocationUp;

		[ProtoMember(17)]
		public int SpawnAttempts;

		[ProtoMember(18)]
		public DateTime LastSpawnAttempt;

		[ProtoMember(19)]
		public int TimeBetweenSpawnAttempts;

		[ProtoMember(20)]
		public bool UseMaxSpawnAttempts;

		[ProtoMember(21)]
		public int MaxSpawnAttempts;

		[ProtoMember(22)]
		public SpawningType SpawnType;

		[ProtoMember(23)]
		public bool IsValid;

		[ProtoMember(24)]
		public string ProfileSubtypeId;

		[ProtoMember(25)]
		public SpawningType SpecificType;

		[ProtoMember(26)]
		public long PlanetEntityId;

		[ProtoMember(27)]
		public bool IsBoss;

		[ProtoMember(28)]
		public bool PreviouslySpawnedEncounter;

		[ProtoMember(29)]
		public List<string> PreviouslySpawnedPrefabs;

		//Non-Serialized

		[ProtoIgnore]
		private ImprovedSpawnGroup _spawnGroup;

		[ProtoIgnore]
		public ImprovedSpawnGroup SpawnGroup {
			get {

				if (_spawnGroup != null)
					return _spawnGroup;

				_spawnGroup = SpawnGroupManager.GetSpawnGroupByName(SpawnGroupName);

				if (_spawnGroup != null)
					return _spawnGroup;

				return null;

			}
		}

		[ProtoIgnore]
		private SpawnConditionsProfile _condition;

		[ProtoIgnore]
		public SpawnConditionsProfile Condition {

			get {

				if (_condition != null)
					return _condition;

				_condition = SpawnGroup?.SpawnConditionsProfiles[ConditionIndex];

				if (_condition != null)
					return _condition;

				return null;

			}

		}

		public StaticEncounter() {

			ProfileSubtypeId = "";

			CheckSpawnConditions = false;

			SpawnGroupName = "";
			ConditionIndex = 0;

			Faction = "";

			TriggerCoords = Vector3D.Zero;
			TriggerRadius = 0;

			UseSpecificPlayers = false;
			SpecificPlayers = new List<long>();
			GpsHashData = new Dictionary<long, int>();

			UseTimeLimit = false;
			StartTime = MyAPIGateway.Session.GameDateTime;
			TimeLimit = 0;

			UseExactSpawnLocation = false;
			ExactLocationCoords = Vector3D.Zero;
			ExactLocationForward = Vector3D.Zero;
			ExactLocationUp = Vector3D.Zero;

			SpawnAttempts = 0;
			LastSpawnAttempt = MyAPIGateway.Session.GameDateTime;
			TimeBetweenSpawnAttempts = 5;
			UseMaxSpawnAttempts = false;
			MaxSpawnAttempts = 0;

			SpawnType = SpawningType.None;
			SpecificType = SpawningType.None;

			PreviouslySpawnedPrefabs = new List<string>();

		}

		public void InitBossEncounter(string spawnGroupName, int condition, Vector3D coords, string faction, SpawningType type) {

			IsValid = true;
			IsBoss = true;
			SpawnGroupName = spawnGroupName;
			ConditionIndex = condition;
			TriggerCoords = coords;
			Faction = faction;

			TriggerRadius = Settings.BossEncounters.TriggerDistance;
			UseSpecificPlayers = true;

			var gps = MyAPIGateway.Session.GPS.Create(Condition.BossCustomGPSLabel, "", coords, true);
			gps.GPSColor = new Color((Vector3)Condition.BossCustomGPSColor);

			//SpecificPlayers
			for (int i = PlayerManager.Players.Count - 1; i >= 0; i--) {

				var player = PlayerManager.Players[i];

				if (!player.ActiveEntity()) {

					SpawnLogger.Write("Player is not Active Entity", SpawnerDebugEnum.Spawning);
					continue;

				}

				bool alreadyHasBoss = false;

				foreach (var encounter in NpcManager.StaticEncounters) {

					if (!encounter.IsValid || !encounter.IsBoss)
						continue;

					if (!encounter.SpecificPlayers.Contains(player.Player.IdentityId))
						continue;

					alreadyHasBoss = true;
					break;
				
				}

				if (alreadyHasBoss)
					continue;

				if (player.Distance(coords) > Settings.BossEncounters.PlayersWithinDistance) {

					SpawnLogger.Write("Player is outside Boss Creation Range", SpawnerDebugEnum.Spawning);
					continue;

				}
					
				SpecificPlayers.Add(player.Player.IdentityId);
				MyAPIGateway.Session.GPS.AddGps(player.Player.IdentityId, gps);

				if (!GpsHashData.ContainsKey(player.Player.IdentityId))
					GpsHashData.Add(player.Player.IdentityId, gps.Hash);

				//Custom Chat Message
				if (Condition.BossCustomAnnounceEnable) {

					MyVisualScriptLogicProvider.SendChatMessage(Condition.BossCustomAnnounceMessage, Condition.BossCustomAnnounceAuthor, player.Player.IdentityId, "Red");
				
				}

			}

			if (SpecificPlayers.Count == 0) {

				IsValid = false;

			}

			UseTimeLimit = true;
			TimeLimit = Settings.BossEncounters.SignalActiveTimer;

			TimeBetweenSpawnAttempts = 1;
			UseMaxSpawnAttempts = true;
			MaxSpawnAttempts = 5;

			SpawnType = SpawningType.BossEncounter;
			SpecificType = type;

		}

		public void InitStaticEncounter(ImprovedSpawnGroup spawnGroup, SpawnConditionsProfile profile) {

			SpawnGroupName = spawnGroup.SpawnGroupName;
			Faction = profile.FactionOwner;

			TriggerCoords = profile.TriggerCoords;
			TriggerRadius = profile.TriggerRadius;

			UseExactSpawnLocation = true;
			ExactLocationForward = profile.StaticEncounterForward;
			ExactLocationUp = profile.StaticEncounterUp;
			ExactLocationCoords = profile.StaticEncounterCoords;

			if (!profile.StaticEncounterUsePlanetDirectionAndAltitude) {

				ExactLocationCoords = profile.StaticEncounterCoords;

			} else {

				var planet = PlanetManager.GetPlanetWithName(profile.StaticEncounterPlanet);

				if (planet == null)
					return;

				PlanetEntityId = planet.Planet.EntityId;
				var surfaceCoords = planet.SurfaceCoordsAtPosition(planet.Center() + (profile.StaticEncounterPlanetDirection * 10000));
				ExactLocationCoords = profile.StaticEncounterPlanetDirection * profile.StaticEncounterPlanetAltitude + surfaceCoords;
				TriggerCoords = ExactLocationCoords;

			}

			SpawnType = SpawningType.StaticEncounter;
			IsValid = true;

		}

		public void InitPreviouslySpawnedEncounter() {
		
			
		
		}

		public void ProcessEncounter(ref bool update) {

			if (!IsValid)
				return;

			//Check Conditions Before Checking Players
			if (UseMaxSpawnAttempts) {

				if (SpawnAttempts >= MaxSpawnAttempts) {

					InvalidateEncounter();
					return;

				}
			
			}

			if (UseTimeLimit) {

				var timeSpan = MyAPIGateway.Session.GameDateTime - StartTime;

				if (timeSpan.TotalSeconds >= TimeLimit) {

					InvalidateEncounter();
					return;
				
				}

			}

			if (TimeBetweenSpawnAttempts > 1) {

				var timeSpan = MyAPIGateway.Session.GameDateTime - LastSpawnAttempt;

				if (timeSpan.TotalSeconds < TimeBetweenSpawnAttempts)
					return;

			}

			for (int i = PlayerManager.Players.Count - 1; i >= 0; i--) {

				var player = PlayerManager.Players[i];

				if (!player.ActiveEntity())
					continue;

				if (UseSpecificPlayers && !SpecificPlayers.Contains(player.Player.IdentityId)) {

					continue;
				
				}

				if(player.Distance(TriggerCoords) > TriggerRadius) {

					continue;

				}

				SpawnLogger.Write(player.Player.DisplayName + " is within range of static/boss encounter. Attempting spawn.", SpawnerDebugEnum.Spawning);

				//Attempt Spawn
				if (SpawnRequest.CalculateStaticSpawn(this, player, SpawnType, SpecificType)) {

					//Invalidate because it was successfully spawned.
					InvalidateEncounter();

				} else {

					SpawnAttempts++;
					LastSpawnAttempt = MyAPIGateway.Session.GameDateTime;
					update = true;
					
				}

				break;

			}

		}

		public void InvalidateEncounter() {

			IsValid = false;
			RemoveGpsFromPlayers();
		
		}

		public void RemoveGpsFromPlayers() {

			foreach (var playerId in GpsHashData.Keys) {

				MyAPIGateway.Session.GPS.RemoveGps(playerId, GpsHashData[playerId]);
			
			}
		
		}

		public void InitTags(string data = null) {

			if (string.IsNullOrWhiteSpace(data))
				return;

			var descSplit = data.Split('\n');

			bool setDampeners = false;
			bool setAtmoRequired = false;
			bool setForceStatic = false;

			foreach (var tagRaw in descSplit) {

				var tag = tagRaw.Trim();

				//CheckSpawnConditions
				if (tag.StartsWith("[CheckSpawnConditions:") == true) {

					TagParse.TagBoolCheck(tag, ref this.CheckSpawnConditions);

				}

				//SpawnGroupName
				if (tag.StartsWith("[SpawnGroupName:") == true) {

					TagParse.TagStringCheck(tag, ref this.SpawnGroupName);

				}

				//Faction
				if (tag.StartsWith("[Faction:") == true) {

					TagParse.TagStringCheck(tag, ref this.Faction);

				}

				//TriggerCoords
				if (tag.StartsWith("[TriggerCoords:") == true) {

					TagParse.TagVector3DCheck(tag, ref this.TriggerCoords);

				}

				//TriggerRadius
				if (tag.StartsWith("[TriggerRadius:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.TriggerRadius);

				}

				//UseTimeLimit
				if (tag.StartsWith("[UseTimeLimit:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseTimeLimit);

				}

				//TimeLimit
				if (tag.StartsWith("[TimeLimit:") == true) {

					TagParse.TagIntCheck(tag, ref this.TimeLimit);

				}

				//UseExactSpawnLocation
				if (tag.StartsWith("[UseExactSpawnLocation:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseExactSpawnLocation);

				}

				//ExactLocationCoords
				if (tag.StartsWith("[ExactLocationCoords:") == true) {

					TagParse.TagVector3DCheck(tag, ref this.ExactLocationCoords);

				}

				//ExactLocationForward
				if (tag.StartsWith("[ExactLocationForward:") == true) {

					TagParse.TagVector3DCheck(tag, ref this.ExactLocationForward);

				}

				//ExactLocationUp
				if (tag.StartsWith("[ExactLocationUp:") == true) {

					TagParse.TagVector3DCheck(tag, ref this.ExactLocationUp);

				}

				//TimeBetweenSpawnAttempts
				if (tag.StartsWith("[TimeBetweenSpawnAttempts:") == true) {

					TagParse.TagIntCheck(tag, ref this.TimeBetweenSpawnAttempts);

				}

				//UseMaxSpawnAttempts
				if (tag.StartsWith("[UseMaxSpawnAttempts:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseMaxSpawnAttempts);

				}

				//MaxSpawnAttempts
				if (tag.StartsWith("[MaxSpawnAttempts:") == true) {

					TagParse.TagIntCheck(tag, ref this.MaxSpawnAttempts);

				}

			}

		}

	}

}
