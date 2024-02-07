using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ProtoBuf;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Zones {

	[ProtoContract]
	public class Zone {

		[ProtoMember(1)] public bool Active; //Determines if the zone is enabled
		[ProtoMember(2)] public bool Persistent; //Zone will not expire after timer or max spawns
		[ProtoMember(3)] public bool Strict; //Spawngroups Must Be Allowed to Spawn in this Zone
		[ProtoMember(4)] public bool PlayerKnownLocation; //Zone is treated as KPL
		[ProtoMember(5)] public bool NoSpawnZone; //Zone Does Not Allow Any Spawning At All

		[ProtoMember(6)] public string Name; //Internal Name Used By SpawnGroups
		[ProtoMember(7)] public string PublicName; //Name of Zone That Player Will See
		[ProtoMember(8)] public string ProfileSubtypeId; //SubtypeId of the Zone Profile used to create this zone

		[ProtoMember(9)] public bool UseLimitedFactions; //Determines Whether a SpawnGroup's Current Faction Should be Considered When Spawning
		[ProtoMember(10)] public List<string> Factions; //Factions Associated To Zone, if any.

		[ProtoMember(11)] public Vector3D Coordinates; //Zone Center (Can Be Provided Manually Or Calculated From Planet)
		[ProtoMember(12)] public double Radius; //Zone Radius
		[ProtoMember(13)] public double RadiusSquared; //Zone Radius Squared (For Quicker Distnace Checking)

		[ProtoMember(14)] public bool PlanetaryZone; //Determines if this zone should be dynamically placed on a planet
		[ProtoMember(15)] public string PlanetName; //Planet name that receives the zone
		
		[ProtoMember(16)] public Vector3D Direction; //Zone Direction From Planet Center To Surface (If Not Provided, planet center is used as zone center)
		[ProtoMember(17)] public double HeightOffset; //Height Offset From Surface for Zone
		[ProtoMember(18)] public bool ScaleZoneRadiusWithPlanet;
		[ProtoMember(19)] public double IntendedPlanetSize;

		[ProtoMember(20)] public bool UseZoneTimer; //Determines If Zone Should Expire After Some Time (doesn't apply to Persistent zones)
		[ProtoMember(21)] public DateTime TimeCreated; //When in Game Time the Zone Was Created
		[ProtoMember(22)] public int MinutesToExpiration; //How Long Until Zone Expires
		[ProtoMember(23)] public bool PlayerPresenceResetsTimer; //Determines if Players Being in the Zone will Reset Timer.

		[ProtoMember(24)] public bool UseMaxSpawnedEncounters; //Determines if the Zone can only have a limited amount of spawns
		[ProtoMember(25)] public int SpawnedEncounters; //Number of Spawned Encounters that have occurred inside this Zone
		[ProtoMember(26)] public int MaxSpawnedEncounters; //Maximum number of Spawned Encounters Allowed For Zone

		[ProtoMember(27)] public bool UseAllowedSpawnGroups; //Determines if the Zone can only spawn certain SpawnGroups (Warning: If Used, SpawnGroups Listed Can ONLY spawn in zones they're allowed in)
		[ProtoMember(28)] public List<string> AllowedSpawnGroups; //SpawnGroup IDs for Allowed Spawning

		[ProtoMember(29)] public bool UseRestrictedSpawnGroups; //Determines if the Zone will prevent certain SpawnGroups from spawning
		[ProtoMember(30)] public List<string> RestrictedSpawnGroups; //SpawnGroup IDs for Restricted Spawning

		[ProtoMember(31)] public bool UseAllowedModIDs; //Determines if the Zone can only spawn SpawnGroups from certain Mods (Warning: If Used, Mod IDs Listed Can ONLY spawn in zones they're allowed in)
		[ProtoMember(32)] public List<ulong> AllowedModIDs; //Mod IDs for Allowed Spawning

		[ProtoMember(33)] public bool UseRestrictedModIDs; //Determines if the Zone will prevent certain SpawnGroup from specified ModIDs from spawning
		[ProtoMember(34)] public List<ulong> RestrictedModIDs; //Mod IDs for Restricted Spawning

		[ProtoMember(35)] public bool UseZoneAnnounce; //Determines if Zone Should Announce To Players A Message When They Enter or Leave the Zone
		[ProtoMember(36)] public string ZoneEnterAnnounce; //Message Displayed To Players Entering The Zone
		[ProtoMember(37)] public string ZoneLeaveAnnounce; //Message Displayed To Players Leaving The Zone
		[ProtoMember(38)] public bool FlashZoneRadius; //Determines if the Radius of the Zone should briefly flash on screen when a player enters or leaves zone.

		[ProtoMember(39)] public Dictionary<string, long> CustomCounters; //Custom Counters Associated To The Zone (Assigned Via RivalAI behavior)
		[ProtoMember(40)] public Dictionary<string, bool> CustomBools; //Custom Bools Associated To The Zone (Assigned Via RivalAI behavior)

		[ProtoMember(41)] public List<long> PlayersInZone;

		[ProtoMember(42)] public long PlanetId; //Planet Entity Id (used internally)

		[ProtoMember(43)] public string RequiredSandboxBool; //Sandbox Bool Name That Must Be True To Use Zone. Not Used if Null
		[ProtoMember(44)] public string RequiredFalseSandboxBool; //Sandbox Bool Name That Must Be False To Use Zone. Not Used if Null

		[ProtoIgnore]
		public BoundingSphereD Sphere { 
			get {
				if (_sphere.Radius < 1)
					_sphere = new BoundingSphereD(Coordinates, Radius);
				return _sphere;
			} }
		[ProtoIgnore] private BoundingSphereD _sphere = new BoundingSphereD();

		public Zone() {

			Active = false;
			Persistent = false;
			Strict = false;
			PlayerKnownLocation = false;

			Name = "";
			PublicName = "";
			ProfileSubtypeId = "";

			UseLimitedFactions = false;
			Factions = new List<string>();

			Coordinates = Vector3D.Zero;
			Radius = 0;
			RadiusSquared = 0;

			PlanetaryZone = false;
			PlanetName = "";
			PlanetId = 0;
			Direction = Vector3D.Zero;
			HeightOffset = 0;
			ScaleZoneRadiusWithPlanet = false;
			IntendedPlanetSize = 120000;

			UseZoneTimer = false;
			TimeCreated = MyAPIGateway.Session.GameDateTime;
			MinutesToExpiration = 60;
			PlayerPresenceResetsTimer = false;

			UseMaxSpawnedEncounters = false;
			SpawnedEncounters = 0;
			MaxSpawnedEncounters = 0;

			UseAllowedSpawnGroups = false;
			AllowedSpawnGroups = new List<string>();

			UseRestrictedSpawnGroups = false;
			RestrictedSpawnGroups = new List<string>();

			UseAllowedModIDs = false;
			AllowedModIDs = new List<ulong>();

			UseRestrictedModIDs = false;
			RestrictedModIDs = new List<ulong>();

			UseZoneAnnounce = false;
			ZoneEnterAnnounce = "";
			ZoneLeaveAnnounce = "";
			FlashZoneRadius = false;

			CustomCounters = new Dictionary<string, long>();
			CustomBools = new Dictionary<string, bool>();

			PlayersInZone = new List<long>();

		}

		public void InitAsKnownPlayerLocation(string faction, Vector3D coords, double radius, int duration, int maxEncounters, int unused = 0) {

			Active = true;
			PlayerKnownLocation = true;

			if (!string.IsNullOrWhiteSpace(faction)) {

				UseLimitedFactions = true;
				Factions.Add(faction);

			}
				
			Coordinates = coords;
			Radius = radius;

			if (duration > -1) {

				UseZoneTimer = true;
				PlayerPresenceResetsTimer = true;
				MinutesToExpiration = duration;
				TimeCreated = MyAPIGateway.Session.GameDateTime;

			}

			if (maxEncounters > -1) {

				UseMaxSpawnedEncounters = true;
				MaxSpawnedEncounters = maxEncounters;

			}

		}

		public void MergeExistingKnownPlayerLocation(Zone existingZone) {

			var dirFromCurrentToIntersection = Vector3D.Normalize(existingZone.Coordinates - this.Coordinates);
			var coordsBetweenCenters = dirFromCurrentToIntersection * (Vector3D.Distance(existingZone.Coordinates, this.Coordinates) / 2) + this.Coordinates;
			var radiusToUse = existingZone.Radius == this.Radius ? this.Radius : this.Radius > existingZone.Radius ? this.Radius : existingZone.Radius;
			var outerRimCoords = -dirFromCurrentToIntersection * radiusToUse + this.Coordinates;
			Radius = Vector3D.Distance(outerRimCoords, coordsBetweenCenters);
			Coordinates = coordsBetweenCenters;
			MergeVariablesFromOldLocation(existingZone);

		}

		public void Reset(long planetId = 0) {

			RadiusSquared = Radius * Radius;
			PlanetId = planetId;
			TimeCreated = MyAPIGateway.Session.GameDateTime;
			SpawnedEncounters = 0;
			CustomCounters.Clear();
			CustomBools.Clear();
			PlayersInZone.Clear();

		}

		public void PlanetSetup(PlanetEntity planet) {

			PlanetId = planet.Planet.EntityId;

			if (this.Direction != Vector3D.Zero) {

				Coordinates = planet.SurfaceCoordsAtPosition(planet.Center() + this.Direction);
				Coordinates += this.Direction * this.HeightOffset;
			
			} else {

				Coordinates = planet.Center();

			}
		
		}

		public void MergeVariablesFromOldLocation(Zone oldZone) {

			if (oldZone.CustomBools != null) {

				foreach (var boolean in oldZone.CustomBools.Keys) {

					bool boolresult = false;

					if (CustomBools.TryGetValue(boolean, out boolresult)) {

						if (oldZone.CustomBools[boolean] || boolresult)
							CustomBools[boolean] = true;

					} else {

						CustomBools.Add(boolean, boolresult);

					}

				}

			}

			if (oldZone.CustomCounters != null) {

				foreach (var counter in oldZone.CustomCounters.Keys) {

					long counterValue = 0;

					if (CustomCounters.TryGetValue(counter, out counterValue)) {

						CustomCounters[counter] += counterValue;

					} else {

						CustomCounters.Add(counter, counterValue);

					}

				}

			}
		
		}

		public bool PositionInsideZone(Vector3D coords) {

			return Vector3D.Distance(coords, Coordinates) < Radius;
		
		}

		public string GetInfo(Vector3D coords) {

			var sb = new StringBuilder();
			sb.Append(" - Profile SubtypeId:           ").Append(ProfileSubtypeId).AppendLine();
			sb.Append(" - Public Name:                 ").Append(PublicName).AppendLine();
			sb.Append(" - Active:                      ").Append(Active).AppendLine();
			sb.Append(" - Persistent:                  ").Append(Persistent).AppendLine();
			sb.Append(" - Strict:                      ").Append(Strict).AppendLine();
			sb.Append(" - Player Known Location:       ").Append(PlayerKnownLocation).AppendLine();
			sb.Append(" - No Spawn Zone:               ").Append(NoSpawnZone).AppendLine();
			sb.Append(" - Use Limited Factions:        ").Append(UseLimitedFactions).AppendLine();
			sb.Append(" - Coordinates:                 ").Append(Coordinates).AppendLine();
			sb.Append(" - Radius:                      ").Append(Radius).AppendLine();
			sb.Append(" - Planetary Zone:              ").Append(PlanetaryZone).AppendLine();
			sb.Append(" - Planet Name:                 ").Append(PlanetName).AppendLine();
			sb.Append(" - Planet Id:                   ").Append(PlanetId).AppendLine();
			sb.Append(" - Use Zone Timer:              ").Append(UseZoneTimer).AppendLine();
			sb.Append(" - Use Max Spawned Encounters:  ").Append(UseMaxSpawnedEncounters).AppendLine();
			sb.Append(" - Use Allowed Spawn Groups:    ").Append(UseAllowedSpawnGroups).AppendLine();
			sb.Append(" - Use Restricted Spawn Groups: ").Append(UseRestrictedSpawnGroups).AppendLine();
			sb.Append(" - Use Allowed Mod IDs:         ").Append(UseAllowedModIDs).AppendLine();
			sb.Append(" - Use Restricted Mod IDs:      ").Append(UseRestrictedModIDs).AppendLine();

			if (CustomBools.Count > 0) {

				sb.Append(" - Custom Bools:      ").AppendLine();

				foreach (var custbool in CustomBools.Keys) {

					if (string.IsNullOrWhiteSpace(custbool))
						continue;

					sb.Append("   - ").Append(custbool).Append(" : ").Append(CustomBools[custbool]).AppendLine();

				}

			}

			if (CustomCounters.Count > 0) {

				sb.Append(" - Custom Counters:      ").AppendLine();

				foreach (var custCounter in CustomCounters.Keys) {

					if (string.IsNullOrWhiteSpace(custCounter))
						continue;

					sb.Append("   - ").Append(custCounter).Append(" : ").Append(CustomCounters[custCounter]).AppendLine();

				}

			}

			return sb.ToString();

		}

		public bool SandboxBoolCheck() {

			bool trueResult = true;
			bool falseResult = true;

			if (!string.IsNullOrWhiteSpace(RequiredSandboxBool)) {

				if (!MyAPIGateway.Utilities.GetVariable<bool>(RequiredSandboxBool, out trueResult))
					trueResult = false;

			}

			if (!string.IsNullOrWhiteSpace(RequiredFalseSandboxBool)) {

				if (!MyAPIGateway.Utilities.GetVariable<bool>(RequiredFalseSandboxBool, out falseResult))
					falseResult = false;

			}

			return trueResult && falseResult;
		
		}

		public void InitTags(string data = null) {

			if (string.IsNullOrWhiteSpace(data))
				return;

			var descSplit = data.Split('\n');

			foreach (var tagRaw in descSplit) {

				var tag = tagRaw.Trim();

				//Active
				if (tag.StartsWith("[Active:") == true) {

					TagParse.TagBoolCheck(tag, ref this.Active);

				}

				//Persistent
				if (tag.StartsWith("[Persistent:") == true) {

					TagParse.TagBoolCheck(tag, ref this.Persistent);

				}

				//Strict
				if (tag.StartsWith("[Strict:") == true) {

					TagParse.TagBoolCheck(tag, ref this.Strict);

				}

				//NoSpawnZone
				if (tag.StartsWith("[NoSpawnZone:") == true) {

					TagParse.TagBoolCheck(tag, ref this.NoSpawnZone);

				}

				//PlayerKnownLocation
				if (tag.StartsWith("[PlayerKnownLocation:") == true) {

					TagParse.TagBoolCheck(tag, ref this.PlayerKnownLocation);

				}

				//Name
				if (tag.StartsWith("[Name:") == true) {

					TagParse.TagStringCheck(tag, ref this.Name);

				}

				//PublicName
				if (tag.StartsWith("[PublicName:") == true) {

					TagParse.TagStringCheck(tag, ref this.PublicName);

				}

				//UseLimitedFactions
				if (tag.StartsWith("[UseLimitedFactions:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseLimitedFactions);

				}

				//Factions
				if (tag.StartsWith("[Factions:") == true) {

					TagParse.TagStringListCheck(tag, ref this.Factions);

				}

				//Coordinates
				if (tag.StartsWith("[Coordinates:") == true) {

					TagParse.TagVector3DCheck(tag, ref this.Coordinates);

				}

				//Radius
				if (tag.StartsWith("[Radius:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.Radius);

				}

				//PlanetaryZone
				if (tag.StartsWith("[PlanetaryZone:") == true) {

					TagParse.TagBoolCheck(tag, ref this.PlanetaryZone);

				}

				//PlanetName
				if (tag.StartsWith("[PlanetName:") == true) {

					TagParse.TagStringCheck(tag, ref this.PlanetName);

				}

				//Direction
				if (tag.StartsWith("[Direction:") == true) {

					TagParse.TagVector3DCheck(tag, ref this.Direction);

				}

				//HeightOffset
				if (tag.StartsWith("[HeightOffset:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.HeightOffset);

				}

				//UseZoneTimer
				if (tag.StartsWith("[UseZoneTimer:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseZoneTimer);

				}

				//MinutesToExpiration
				if (tag.StartsWith("[MinutesToExpiration:") == true) {

					TagParse.TagIntCheck(tag, ref this.MinutesToExpiration);

				}

				//PlayerPresenceResetsTimer
				if (tag.StartsWith("[PlayerPresenceResetsTimer:") == true) {

					TagParse.TagBoolCheck(tag, ref this.PlayerPresenceResetsTimer);

				}

				//UseMaxSpawnedEncounters
				if (tag.StartsWith("[UseMaxSpawnedEncounters:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseMaxSpawnedEncounters);

				}

				//MaxSpawnedEncounters
				if (tag.StartsWith("[MaxSpawnedEncounters:") == true) {

					TagParse.TagIntCheck(tag, ref this.MaxSpawnedEncounters);

				}

				//UseAllowedSpawnGroups
				if (tag.StartsWith("[UseAllowedSpawnGroups:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseAllowedSpawnGroups);

				}

				//AllowedSpawnGroups
				if (tag.StartsWith("[AllowedSpawnGroups:") == true) {

					TagParse.TagStringListCheck(tag, ref this.AllowedSpawnGroups);

				}

				//UseRestrictedSpawnGroups
				if (tag.StartsWith("[UseRestrictedSpawnGroups:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseRestrictedSpawnGroups);

				}

				//RestrictedSpawnGroups
				if (tag.StartsWith("[RestrictedSpawnGroups:") == true) {

					TagParse.TagStringListCheck(tag, ref this.RestrictedSpawnGroups);

				}

				//UseAllowedModIDs
				if (tag.StartsWith("[UseAllowedModIDs:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseAllowedModIDs);

				}

				//AllowedModIDs
				if (tag.StartsWith("[AllowedModIDs:") == true) {

					TagParse.TagUlongListCheck(tag, ref this.AllowedModIDs);

				}

				//UseRestrictedModIDs
				if (tag.StartsWith("[UseRestrictedModIDs:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseRestrictedModIDs);

				}

				//RestrictedModIDs
				if (tag.StartsWith("[RestrictedModIDs:") == true) {

					TagParse.TagUlongListCheck(tag, ref this.RestrictedModIDs);

				}

				//UseZoneAnnounce
				if (tag.StartsWith("[UseZoneAnnounce:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseZoneAnnounce);

				}

				//ZoneEnterAnnounce
				if (tag.StartsWith("[ZoneEnterAnnounce:") == true) {

					TagParse.TagStringCheck(tag, ref this.ZoneEnterAnnounce);

				}

				//ZoneLeaveAnnounce
				if (tag.StartsWith("[ZoneLeaveAnnounce:") == true) {

					TagParse.TagStringCheck(tag, ref this.ZoneLeaveAnnounce);

				}

				//FlashZoneRadius
				if (tag.StartsWith("[FlashZoneRadius:") == true) {

					TagParse.TagBoolCheck(tag, ref this.FlashZoneRadius);

				}

				//RequiredSandboxBool
				if (tag.StartsWith("[RequiredSandboxBool:") == true) {

					TagParse.TagStringCheck(tag, ref this.RequiredSandboxBool);

				}

				//RequiredFalseSandboxBool
				if (tag.StartsWith("[RequiredFalseSandboxBool:") == true) {

					TagParse.TagStringCheck(tag, ref this.RequiredFalseSandboxBool);

				}

			}

		}

		public void InitLegacyTags(string data) {

			if (string.IsNullOrWhiteSpace(data))
				return;

			var descSplit = data.Split('\n');

			foreach (var tagRaw in descSplit) {

				var tag = tagRaw.Trim();

				//Type
				if (tag.StartsWith("[Type:") == true) {

					string result = "";
					TagParse.TagStringCheck(tag, ref result);

					if (result == "Planetary") {

						this.PlanetaryZone = true;
					
					}

				}

				//ScaleRadiusWithPlanetSize
				if (tag.StartsWith("[ScaleRadiusWithPlanetSize:") == true) {

					TagParse.TagBoolCheck(tag, ref this.ScaleZoneRadiusWithPlanet);

				}

				//Active
				if (tag.StartsWith("[Active:") == true) {

					TagParse.TagBoolCheck(tag, ref this.Active);

				}

				//Name
				if (tag.StartsWith("[Name:") == true) {

					TagParse.TagStringCheck(tag, ref this.PublicName);

				}

				//Radius
				if (tag.Contains("[Radius:") == true) {

					TagParse.TagDoubleCheck(tag, ref Radius);

				}

				//NoSpawnZone
				if (tag.StartsWith("[NoSpawnZone:") == true) {

					TagParse.TagBoolCheck(tag, ref this.NoSpawnZone);

				}

				//StrictTerritory
				if (tag.StartsWith("[StrictTerritory:") == true) {

					TagParse.TagBoolCheck(tag, ref this.Strict);

				}

				//CoordsX
				if (tag.Contains("[CoordsX") == true) {

					TagParse.TagDoubleCheck(tag, ref Coordinates.X);

				}

				//CoordsY
				if (tag.Contains("[CoordsY") == true) {

					TagParse.TagDoubleCheck(tag, ref Coordinates.Y);

				}

				//CoordsZ
				if (tag.Contains("[CoordsZ") == true) {

					TagParse.TagDoubleCheck(tag, ref Coordinates.Z);

				}

				//AnnounceArriveDepart
				if (tag.StartsWith("[AnnounceArriveDepart:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseZoneAnnounce);

				}

				//CustomArriveMessage
				if (tag.StartsWith("[CustomArriveMessage:") == true) {

					TagParse.TagStringCheck(tag, ref this.ZoneEnterAnnounce);

				}

				//CustomDepartMessage
				if (tag.StartsWith("[CustomDepartMessage:") == true) {

					TagParse.TagStringCheck(tag, ref this.ZoneLeaveAnnounce);

				}

				//PlanetGeneratorName
				if (tag.StartsWith("[PlanetGeneratorName:") == true) {

					TagParse.TagStringCheck(tag, ref this.PlanetName);

				}

			}

		}

		public Zone Clone() {

			var data = MyAPIGateway.Utilities.SerializeToBinary<Zone>(this);
			return MyAPIGateway.Utilities.SerializeFromBinary<Zone>(data);
		
		}

	}

}
