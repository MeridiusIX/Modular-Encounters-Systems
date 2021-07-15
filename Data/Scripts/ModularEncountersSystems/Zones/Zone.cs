using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ProtoBuf;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Zones {

	public class Zone {

		public bool Active; //Determines if the zone is enabled
		public bool Persistent; //Zone will not expire after timer or max spawns
		public bool Strict; //Spawngroups Must Be Allowed to Spawn in this Zone
		public bool PlayerKnownLocation; //Zone is treated as KPL
		public bool NoSpawnZone; //Zone Does Not Allow Any Spawning At All

		public string Name; //Internal Name Used By SpawnGroups
		public string PublicName; //Name of Zone That Player Will See
		public string ProfileSubtypeId; //SubtypeId of the Zone Profile used to create this zone

		public bool UseLimitedFactions; //Determines Whether a SpawnGroup's Current Faction Should be Considered When Spawning
		public List<string> Factions; //Factions Associated To Zone, if any.

		public Vector3D Coordinates; //Zone Center (Can Be Provided Manually Or Calculated From Planet)
		public double Radius; //Zone Radius
		public double RadiusSquared; //Zone Radius Squared (For Quicker Distnace Checking)

		public bool PlanetaryZone; //Determines if this zone should be dynamically placed on a planet
		public string PlanetName; //Planet name that receives the zone
		public long PlanetId; //Planet Entity Id (used internally)
		public Vector3D Direction; //Zone Direction From Planet Center To Surface (If Not Provided, planet center is used as zone center)
		public double HeightOffset; //Height Offset From Surface for Zone
		public bool ScaleZoneRadiusWithPlanet;
		public double IntendedPlanetSize;

		public bool UseZoneTimer; //Determines If Zone Should Expire After Some Time (doesn't apply to Persistent zones)
		public DateTime TimeCreated; //When in Game Time the Zone Was Created
		public int MinutesToExpiration; //How Long Until Zone Expires
		public bool PlayerPresenceResetsTimer; //Determines if Players Being in the Zone will Reset Timer.

		public bool UseMaxSpawnedEncounters; //Determines if the Zone can only have a limited amount of spawns
		public int SpawnedEncounters; //Number of Spawned Encounters that have occurred inside this Zone
		public int MaxSpawnedEncounters; //Maximum number of Spawned Encounters Allowed For Zone

		public bool UseAllowedSpawnGroups; //Determines if the Zone can only spawn certain SpawnGroups (Warning: If Used, SpawnGroups Listed Can ONLY spawn in zones they're allowed in)
		public List<string> AllowedSpawnGroups; //SpawnGroup IDs for Allowed Spawning

		public bool UseRestrictedSpawnGroups; //Determines if the Zone will prevent certain SpawnGroups from spawning
		public List<string> RestrictedSpawnGroups; //SpawnGroup IDs for Restricted Spawning

		public bool UseAllowedModIDs; //Determines if the Zone can only spawn SpawnGroups from certain Mods (Warning: If Used, Mod IDs Listed Can ONLY spawn in zones they're allowed in)
		public List<ulong> AllowedModIDs; //Mod IDs for Allowed Spawning

		public bool UseRestrictedModIDs; //Determines if the Zone will prevent certain SpawnGroup from specified ModIDs from spawning
		public List<ulong> RestrictedModIDs; //Mod IDs for Restricted Spawning

		public bool UseZoneAnnounce; //Determines if Zone Should Announce To Players A Message When They Enter or Leave the Zone
		public string ZoneEnterAnnounce; //Message Displayed To Players Entering The Zone
		public string ZoneLeaveAnnounce; //Message Displayed To Players Leaving The Zone
		public bool FlashZoneRadius; //Determines if the Radius of the Zone should briefly flash on screen when a player enters or leaves zone.

		public Dictionary<string, long> CustomCounters; //Custom Counters Associated To The Zone (Assigned Via RivalAI behavior)
		public Dictionary<string, bool> CustomBools; //Custom Bools Associated To The Zone (Assigned Via RivalAI behavior)

		public List<long> PlayersInZone;

		public BoundingSphereD Sphere { 
			get {
				if (_sphere.Radius < 1)
					_sphere = new BoundingSphereD(Coordinates, Radius);
				return _sphere;
			} }
		private BoundingSphereD _sphere = new BoundingSphereD();

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

			if(!string.IsNullOrWhiteSpace(faction))
				Factions.Add(faction);

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

		public string GetInfo(Vector3D coords) {

			var sb = new StringBuilder();
			sb.Append(" - [Zone Info] ").AppendLine();

			return sb.ToString();

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
				if (tag.Contains("[Radius") == true) {

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
