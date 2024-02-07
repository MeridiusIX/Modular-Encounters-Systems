using ModularEncountersSystems.Core;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Tasks;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRageMath;

namespace ModularEncountersSystems.Zones {

	public enum ZoneEvaluation {
	
		None,
		ZoneName,
		Strict,
		AllowedSpawnGroup,
		FactionRestricted,
	
	}

	public static class ZoneManager {

		public static List<Zone> ActiveZones = new List<Zone>();
		private static bool _updateZones = false;

		private static byte _secondsForTimerCheck = 0;

		public static void Setup() {

			//Get Stored Zones
			string persString = "";

			if (MyAPIGateway.Utilities.GetVariable<string>("MES-ZoneData", out persString)) {

				var persData = Convert.FromBase64String(persString);

				if (persData != null) {

					ActiveZones = MyAPIGateway.Utilities.SerializeFromBinary<List<Zone>>(persData);

					if (ActiveZones == null) {

						SpawnLogger.Write("No Zones Stored In World At Startup. List Null.", SpawnerDebugEnum.Startup, true);
						ActiveZones = new List<Zone>();

					} else if (ActiveZones.Count == 0) {

						SpawnLogger.Write("No Zones Stored In World At Startup. List Empty.", SpawnerDebugEnum.Startup, true);

					} else {

						SpawnLogger.Write("Zones Loaded At Startup: " + ActiveZones.Count, SpawnerDebugEnum.Startup, true);

					}
						
				}
			
			}

			//Validate Existing Persistent Zones Against Profiles
			for (int i = ActiveZones.Count - 1; i >= 0; i--) {

				var zone = ActiveZones[i];

				if (string.IsNullOrWhiteSpace(zone?.ProfileSubtypeId)) {

					SpawnLogger.Write("Removing Zone With No ProfileSubtypeId: " + zone.Name ?? "null", SpawnerDebugEnum.Startup);
					ActiveZones.RemoveAt(i);
					continue;

				}

				if (!ProfileManager.ZoneProfiles.ContainsKey(zone.ProfileSubtypeId)) {

					SpawnLogger.Write("Removing Zone That Wasn't Registered In Profile Manager: " + zone.Name ?? "null", SpawnerDebugEnum.Startup);
					ActiveZones.RemoveAt(i);
					continue;

				}

				if (zone.PlanetaryZone) {

					bool planetExists = false;

					foreach (var planet in PlanetManager.Planets) {

						var id = planet?.Planet?.EntityId ?? -1;

						if (zone.PlanetId == id) {

							planetExists = true;
							break;
						
						}
					
					}

					if (!planetExists) {

						SpawnLogger.Write("Removing Zone That No Longer Has Associated Planet: " + zone.Name ?? "null", SpawnerDebugEnum.Startup);
						ActiveZones.RemoveAt(i);
						continue;

					}
				
				}
			
			}

			MES_SessionCore.SaveActions += UpdateZoneStorage;
			AddNewZones();

			TaskProcessor.Tick60.Tasks += AnnounceDepartMessages;
			TaskProcessor.Tick60.Tasks += TimerChecks;
			MES_SessionCore.UnloadActions += Unload;
		
		}

		public static void AddNewZones() {

			//Add New Zones
			bool updateZones = false;

			foreach (var zoneId in ProfileManager.ZoneProfiles.Keys) {

				var zone = ProfileManager.ZoneProfiles[zoneId];
				bool skip = false;

				if (!zone.Persistent)
					continue;

				if (!zone.PlanetaryZone) {

					foreach (var perZone in ActiveZones) {

						if (perZone.ProfileSubtypeId == zone.ProfileSubtypeId) {

							skip = true;
							break;

						}

					}

					if (skip)
						continue;

					var newZone = zone.Clone();
					newZone.Reset();
					ActiveZones.Add(newZone);
					updateZones = true;


				} else {

					List<PlanetEntity> planetsNeeded = null;

					foreach (var planet in PlanetManager.Planets) {

						var id = planet?.Planet?.Generator?.Id.SubtypeName ?? "";

						if (string.IsNullOrWhiteSpace(id)) {

							SpawnLogger.Write("Planet Id Null: " + zone.PublicName ?? "null", SpawnerDebugEnum.Startup);
							continue;

						}

						if (id != zone.PlanetName) {

							SpawnLogger.Write("Planet Id doesn't Match PlanetName in Zone: " + zone.PublicName ?? "null", SpawnerDebugEnum.Startup);
							SpawnLogger.Write(" - Planet Id: " + id + " // Zone Planet: " + zone.PlanetName ?? "null", SpawnerDebugEnum.Startup);
							continue;

						}

						bool skipPlanet = false;

						foreach (var perZone in ActiveZones) {

							if (id == perZone.PlanetName && planet.Planet.EntityId == perZone.PlanetId) {

								if (perZone.ProfileSubtypeId == zone.ProfileSubtypeId) {

									skipPlanet = true;
									break;

								}

							}

						}

						if (skipPlanet)
							continue;

						if (planetsNeeded == null)
							planetsNeeded = new List<PlanetEntity>();

						planetsNeeded.Add(planet);

					}

					if (planetsNeeded != null) {

						foreach (var planet in planetsNeeded) {

							var newZone = zone.Clone();
							newZone.Reset(planet.Planet.EntityId);
							newZone.PlanetSetup(planet);
							ActiveZones.Add(newZone);
							updateZones = true;

						}

					}

				}

			}

			SpawnLogger.Write("Zones After Newly Detected: " + ActiveZones.Count, SpawnerDebugEnum.Startup);

			if (updateZones) {

				FlagUpdateZoneStorage();

			}


		}

		public static void RemoveZone(Zone zone) {

			if (ActiveZones.Contains(zone)) {

				ActiveZones.Remove(zone);
				FlagUpdateZoneStorage();

			}

		}

		public static void FlagUpdateZoneStorage() {

			_updateZones = true;
		
		}

		public static void UpdateZoneStorage() {

			if (!_updateZones)
				return;

			var zoneData = MyAPIGateway.Utilities.SerializeToBinary<List<Zone>>(ActiveZones);
			var zoneString = Convert.ToBase64String(zoneData);
			MyAPIGateway.Utilities.SetVariable<string>("MES-ZoneData", zoneString);
			_updateZones = false;

		}

		public static void GetAllowedSpawns(Vector3D coords, SpawnGroupCollection collection) {

			foreach (var zone in ActiveZones) {

				if (!zone.Active)
					continue;

				if (zone.UseAllowedSpawnGroups) {

					foreach (var spawn in zone.AllowedSpawnGroups) {

						if (!string.IsNullOrWhiteSpace(spawn) && !collection.OnlyAllowedZoneSpawns.Contains(spawn))
							collection.OnlyAllowedZoneSpawns.Add(spawn);

					}

				}

				if (Vector3D.Distance(zone.Coordinates, coords) > zone.Radius)
					continue;

				if (zone.Persistent && zone.AllowedSpawnGroups.Count > 0) {

					foreach (var spawn in zone.AllowedSpawnGroups) {

						if(!string.IsNullOrWhiteSpace(spawn) && !collection.AllowedZoneSpawns.Contains(spawn))
							collection.AllowedZoneSpawns.Add(spawn);

					}
				
				}

				if ((zone.Persistent || zone.PlayerKnownLocation) && zone.UseLimitedFactions && zone.Factions.Count > 0) {

					foreach (var faction in zone.Factions) {

						if (!string.IsNullOrWhiteSpace(faction) && !collection.AllowedZoneFactions.Contains(faction))
							collection.AllowedZoneFactions.Add(faction);

					}

				}

				if (zone.Persistent && zone.UseRestrictedSpawnGroups && zone.RestrictedSpawnGroups.Count > 0) {

					foreach (var spawn in zone.RestrictedSpawnGroups) {

						if (!string.IsNullOrWhiteSpace(spawn) && !collection.RestrictedZoneSpawnGroups.Contains(spawn))
							collection.RestrictedZoneSpawnGroups.Add(spawn);

					}

				}

			}

		}

		public static bool InsideNoSpawnZone(Vector3D coords) {

			foreach (var zone in ActiveZones) {

				if (!zone.Active || Vector3D.Distance(zone.Coordinates, coords) > zone.Radius)
					continue;

				if (zone.NoSpawnZone) {

					return true;

				}

			}

			return false;

		}

		public static bool PositionInsideStrictZone(Vector3D coords) {

			foreach (var zone in ActiveZones) {

				if (!zone.Active || Vector3D.Distance(zone.Coordinates, coords) > zone.Radius)
					continue;

				if (zone.Persistent && zone.Strict)
					return true;
			
			}

			return false;
		
		}

		public static void AnnounceDepartMessages() {

			bool updateZones = false;

			for (int i = 0; i < ActiveZones.Count; i++) {

				var zone = ActiveZones[i];

				if (!zone.Persistent || !zone.Active || !zone.UseZoneAnnounce)
					continue;

				for (int j = 0; j < PlayerManager.Players.Count; j++) {

					var player = PlayerManager.Players[j];

					if (!player.ActiveEntity())
						continue;

					var distFromCenter = player.Distance(zone.Coordinates);

					if (zone.PlayersInZone.Contains(player.Player.IdentityId) && distFromCenter > zone.Radius && !string.IsNullOrWhiteSpace(zone.ZoneLeaveAnnounce)) {

						//Leave Zone
						updateZones = true;
						zone.PlayersInZone.Remove(player.Player.IdentityId);
						MyVisualScriptLogicProvider.ShowNotification(zone.ZoneLeaveAnnounce, 5000, "White", player.Player.IdentityId);

					} else if (!zone.PlayersInZone.Contains(player.Player.IdentityId) && distFromCenter < zone.Radius && !string.IsNullOrWhiteSpace(zone.ZoneEnterAnnounce)) {

						//Enter Zone
						updateZones = true;
						zone.PlayersInZone.Add(player.Player.IdentityId);
						MyVisualScriptLogicProvider.ShowNotification(zone.ZoneEnterAnnounce, 5000, "White", player.Player.IdentityId);

					}

				}
			
			}

			if (updateZones)
				FlagUpdateZoneStorage();


		}

		public static void ChangeKPLBools(Vector3D coords, string faction, List<string> counterNames, List<bool> counterValues) {

			bool updateZones = false;

			for (int i = 0; i < ActiveZones.Count; i++) {

				var zone = ActiveZones[i];

				if (!zone.PlayerKnownLocation || !zone.Factions.Contains(faction))
					continue;

				if (zone.PositionInsideZone(coords))
					continue;

				CustomValueHelper.ChangeCustomBools(zone.CustomBools, counterNames, counterValues);
				updateZones = true;

			}

			if (updateZones)
				FlagUpdateZoneStorage();

		}


		public static void ChangeKPLCounters(Vector3D coords, string faction, List<string> counterNames, List<long> counterValues, List<ModifierEnum> counterModifiers) {

			bool updateZones = false;

			for (int i = 0; i < ActiveZones.Count; i++) {

				var zone = ActiveZones[i];

				if (!zone.PlayerKnownLocation || !zone.Factions.Contains(faction))
					continue;

				if (zone.PositionInsideZone(coords))
					continue;

				CustomValueHelper.ChangeCustomCounters(zone.CustomCounters, counterNames, counterValues, counterModifiers);
				updateZones = true;

			}

			if (updateZones)
				FlagUpdateZoneStorage();

		}


		public static void ChangeZoneRadius(Vector3D coords, string name, double radiusChange, ModifierEnum modifier) {

			bool updateZones = false;

			for (int i = 0; i < ActiveZones.Count; i++) {

				var zone = ActiveZones[i];

				if (!zone.Persistent || zone.Name != name)
					continue;

				if (zone.PositionInsideZone(coords))
					continue;

				MathTools.ApplyModifier(radiusChange, modifier, ref zone.Radius);
				updateZones = true;

			}

			if (updateZones)
				FlagUpdateZoneStorage();

		}

		public static void ChangeZoneCounters(Vector3D coords, string name, List<string> counterNames, List<long> counterValues, List<ModifierEnum> counterModifiers) {

			bool updateZones = false;

			for (int i = 0; i < ActiveZones.Count; i++) {

				var zone = ActiveZones[i];

				if (!zone.Persistent || zone.PublicName != name)
					continue;

				if (zone.PositionInsideZone(coords))
					continue;

				CustomValueHelper.ChangeCustomCounters(zone.CustomCounters, counterNames, counterValues, counterModifiers);
				updateZones = true;

			}

			if (updateZones)
				FlagUpdateZoneStorage();

		}

		public static void ChangeZoneBools(Vector3D coords, string name, List<string> counterNames, List<bool> counterValues) {

			bool updateZones = false;

			for (int i = 0; i < ActiveZones.Count; i++) {

				var zone = ActiveZones[i];

				if (!zone.Persistent || zone.PublicName != name)
					continue;

				if (zone.PositionInsideZone(coords))
					continue;

				CustomValueHelper.ChangeCustomBools(zone.CustomBools, counterNames, counterValues);
				updateZones = true;

			}

			if (updateZones)
				FlagUpdateZoneStorage();

		}

		public static bool InsideZoneWithName(Vector3D coords, string zoneName) {

			bool result = false;

			for (int i = ActiveZones.Count - 1; i >= 0; i--) {

				var zone = ActiveZones[i];

				if (zone.PublicName != zoneName)
					continue;

				if (!zone.PositionInsideZone(coords))
					continue;

				result = true;
				break;

			}

			return result;
		
		}

		public static void RemoveAllZones() {

			MyAPIGateway.Utilities.RemoveVariable("MES-ZoneData");
			ActiveZones.Clear();

		}

		public static void ResetAllZones() {

			MyAPIGateway.Utilities.RemoveVariable("MES-ZoneData");
			ActiveZones.Clear();
			Setup();
		
		}

		public static void TimerChecks() {

			_secondsForTimerCheck++;

			if (_secondsForTimerCheck < 10)
				return;

			_secondsForTimerCheck = 0;

			bool updateZones = false;

			for (int i = ActiveZones.Count - 1; i >= 0; i--) {

				var zone = ActiveZones[i];

				if (!zone.Active || !zone.UseZoneTimer)
					continue;

				var mins = MyAPIGateway.Session.GameDateTime - zone.TimeCreated;

				if (mins.TotalMinutes >= zone.MinutesToExpiration) {

					ActiveZones.RemoveAt(i);
					zone.Active = false;
					updateZones = true;
					continue;

				}

				for (int j = 0; j < PlayerManager.Players.Count; j++) {

					var player = PlayerManager.Players[j];

					if (!player.ActiveEntity())
						continue;

					var distFromCenter = player.Distance(zone.Coordinates);

					if (distFromCenter < zone.Radius) {

						//Reset Timer
						zone.TimeCreated = MyAPIGateway.Session.GameDateTime;
						updateZones = true;
						break;

					}

				}

			}

			if (updateZones)
				FlagUpdateZoneStorage();

		}

		public static void ToggleZonesAtPosition(Vector3D coords, string zoneName = null, bool mode = false) {

			bool updateZones = false;

			for (int i = ActiveZones.Count - 1; i >= 0; i--) {

				var zone = ActiveZones[i];

				if (zoneName != null && zone.PublicName != zoneName)
					continue;

				if (!zone.PositionInsideZone(coords))
					continue;

				zone.Active = mode;

				updateZones = true;

			}

			if (updateZones)
				FlagUpdateZoneStorage();

		}

		public static void ToggleZones(string zoneName = null, bool mode = false, Vector3D? coords = null) {

			bool updateZones = false;

			for (int i = ActiveZones.Count - 1; i >= 0; i--) {

				var zone = ActiveZones[i];

				if (zoneName != null && zone.PublicName != zoneName)
					continue;

				if (coords != null && !zone.PositionInsideZone(coords.Value))
					continue;

				zone.Active = mode;

				updateZones = true;

			}

			if (updateZones)
				FlagUpdateZoneStorage();

		}

		public static void Unload() {

			MES_SessionCore.SaveActions -= UpdateZoneStorage;
			TaskProcessor.Tick60.Tasks -= AnnounceDepartMessages;
			TaskProcessor.Tick60.Tasks -= TimerChecks;

		}

	}

}
