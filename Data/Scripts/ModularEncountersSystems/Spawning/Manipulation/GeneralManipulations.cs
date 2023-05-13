using ModularEncountersSystems.BlockLogic;
using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning.Profiles;
using ModularEncountersSystems.World;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;

namespace ModularEncountersSystems.Spawning.Manipulation {
	public static class GeneralManipulations {

		//Process Blocks
		public static void ProcessBlocks(MyObjectBuilder_CubeGrid cubeGrid, ImprovedSpawnGroup spawnGroup, ManipulationProfile profile, NpcData data) {

			//Grid Level Changes

			//Get Config
			var config = Settings.GetConfig(SpawnRequest.GetPrimarySpawningType(data.SpawnType));

			//Damage Modifier Value
			float damageModifier = 100;

			if (profile.BlockDamageModifier <= 0) {

				damageModifier = 0;

			} else {

				damageModifier = (float)profile.BlockDamageModifier / 100;

			}

			//Editable
			cubeGrid.Editable = profile.GridsAreEditable;

			//Destructable
			cubeGrid.DestructibleBlocks = profile.GridsAreDestructable;

			//Thruster Settings
			if (profile.ConfigureSpecialNpcThrusters) {

				var thrustProfile = new ThrustSettings(profile);
				var thrustProfileString = thrustProfile.ConvertToString();
				StorageTools.ApplyCustomEntityStorage(cubeGrid, StorageTools.NpcThrusterDataKey, thrustProfileString);
				data.Attributes.CustomThrustDataUsed = true;

			}

			//Gyro Settings
			if (profile.SetNpcGyroscopeMultiplier) {

				StorageTools.ApplyCustomEntityStorage(cubeGrid, StorageTools.NpcGyroDataKey, profile.NpcGyroscopeMultiplier.ToString());

			}

			if (profile.AttachModStorageComponentToGrid == true) {

				SpawnLogger.Write("Assigning ModStorageComponent", SpawnerDebugEnum.Manipulation);

				StorageTools.ApplyCustomEntityStorage(cubeGrid, profile.StorageKey, profile.StorageValue);

			}

			foreach (var block in cubeGrid.CubeBlocks) {

				//Process Non-Terminal Blocks

				//Damage Modifier
				if (profile.OverrideBlockDamageModifier == true) {

					block.BlockGeneralDamageModifier *= damageModifier;

				}

				//Remove Authorship
				if (profile.ClearAuthorship == true) {

					block.BuiltBy = 0;

				}

				//Clear Grid Inventories
				if (profile.ClearGridInventories) {

					PrefabInventory.RemoveInventoryFromBlock(block);

				}

				//Process Terminal Blocks
				var funcBlock = block as MyObjectBuilder_FunctionalBlock;
				var termBlock = block as MyObjectBuilder_TerminalBlock;

				if (termBlock == null) {

					continue;

				}

				if (profile.EraseIngameScripts == true) {

					var pbBlock = block as MyObjectBuilder_MyProgrammableBlock;

					if (pbBlock != null) {

						pbBlock.Program = null;
						pbBlock.Storage = "";
						pbBlock.DefaultRunArgument = null;

					}

				}

				if (spawnGroup.ReplenishSystems) {

					ReplenishSystems(termBlock);

				}

				if (profile.DisableTimerBlocks == true) {

					var timer = block as MyObjectBuilder_TimerBlock;

					if (timer != null) {

						timer.IsCountingDown = false;

					}

				}

				if (profile.DisableSensorBlocks == true) {

					var sensor = block as MyObjectBuilder_SensorBlock;

					if (sensor != null) {

						sensor.Enabled = false;

					}

				}

				if (profile.DisableWarheads == true) {

					var warhead = block as MyObjectBuilder_Warhead;

					if (warhead != null) {

						warhead.CountdownMs = 10000;
						warhead.IsArmed = false;
						warhead.IsCountingDown = false;

					}

				}

				if (profile.DisableThrustOverride == true) {

					var thrust = block as MyObjectBuilder_Thrust;

					if (thrust != null) {

						thrust.ThrustOverride = 0.0f;

					}

				}

				if (profile.DisableGyroOverride == true) {

					var gyro = block as MyObjectBuilder_Gyro;

					if (gyro != null) {

						gyro.GyroPower = 1f;
						gyro.GyroOverride = false;

					}

				}

				if (profile.SetDoorsAnyoneCanUse) {

					var door = block as MyObjectBuilder_Door;

					if (door != null) {

						door.AnyoneCanUse = true;
					
					}
				
				}

				if (profile.SetStoresAnyoneCanUse) {

					var store = block as MyObjectBuilder_StoreBlock;

					if (store != null) {

						store.AnyoneCanUse = true;

					}

				}

				if (profile.SetConnectorsTradeMode) {

					var connector = block as MyObjectBuilder_ShipConnector;

					if (connector != null) {

						connector.TradingEnabled = true;
						connector.AutoUnlockTime = 600;

					}

				}

				if (!string.IsNullOrWhiteSpace(termBlock.CustomName) && funcBlock != null) {

					//Enable Blocks By Name
					foreach (var blockName in profile.EnableBlocksWithName) {

						if (string.IsNullOrWhiteSpace(blockName) == true) {

							continue;

						}

						if (profile.AllowPartialNames == true) {

							if (termBlock.CustomName.Contains(blockName) == true) {

								funcBlock.Enabled = true;

							}

						} else if (termBlock.CustomName == blockName) {

							funcBlock.Enabled = true;

						}

					}

					//Disable Blocks By Name
					foreach (var blockName in profile.DisableBlocksWithName) {

						if (string.IsNullOrWhiteSpace(blockName)) {

							continue;

						}

						if (profile.AllowPartialNames == true) {

							if (termBlock.CustomName.Contains(blockName) == true) {

								funcBlock.Enabled = false;

							}

						} else if (termBlock.CustomName == blockName) {

							funcBlock.Enabled = false;

						}

					}

					//Block Name Replacer
					if (profile.UseBlockNameReplacer) {

						string name = "";

						if (profile.BlockNameReplacerReference.TryGetValue(termBlock.CustomName, out name)) {

							termBlock.CustomName = name;

						}

					}

				}

				//Disable Blocks From Config
				ApplyBlockDisable(funcBlock, config);

				//AssignContainerTypesToAllCargo
				if (profile.AssignContainerTypesToAllCargo.Count > 0) {

					var container = termBlock as MyObjectBuilder_CargoContainer;

					if (container != null) {

						var dlcLockers = new string[] { "LargeBlockLockerRoom", "LargeBlockLockerRoomCorner", "LargeBlockLockers" };

						if (!dlcLockers.Contains(block.SubtypeName)) {

							container.ContainerType = profile.AssignContainerTypesToAllCargo[MathTools.RandomBetween(0, profile.AssignContainerTypesToAllCargo.Count)];

						}

					}

				}

				//Container Type Assignment
				if (profile.UseContainerTypeAssignment) {

					var container = termBlock as MyObjectBuilder_CargoContainer;

					if (container != null && !string.IsNullOrWhiteSpace(termBlock.CustomName)) {

						string containerType = "";

						if (profile.ContainerTypeAssignmentReference.TryGetValue(termBlock.CustomName, out containerType)) {

							container.ContainerType = containerType;

						}

					}

				}

				//Shipyard Settings
				if (profile.ShipyardSetup) {

					var projector = termBlock as MyObjectBuilder_Projector;

					if (projector != null && !string.IsNullOrWhiteSpace(termBlock.CustomName)) {

						for (int i = 0; i < profile.ShipyardConsoleBlockNames.Count && i < profile.ShipyardProfileNames.Count; i++) {

							if (projector.CustomName == profile.ShipyardConsoleBlockNames[i]) {

								StorageTools.ApplyCustomBlockStorage(projector, StorageTools.MesShipyardKey, profile.ShipyardProfileNames[i]);
							
							}
						
						}

					}

				}

				//Turret Settings
				if (profile.ChangeTurretSettings == true) {

					var turret = block as MyObjectBuilder_TurretBase;

					if (turret != null) {

						var defId = turret.GetId();
						var weaponBlockDef = (MyLargeTurretBaseDefinition)MyDefinitionManager.Static.GetCubeBlockDefinition(defId);

						if (weaponBlockDef != null) {

							if (profile.TurretRange > weaponBlockDef.MaxRangeMeters) {

								turret.Range = weaponBlockDef.MaxRangeMeters;

							} else {

								turret.Range = (float)profile.TurretRange;

							}

							turret.EnableIdleRotation = profile.TurretIdleRotation;
							turret.TargetMeteors = profile.TurretTargetMeteors;
							turret.TargetMissiles = profile.TurretTargetMissiles;
							turret.TargetCharacters = profile.TurretTargetCharacters;
							turret.TargetSmallGrids = profile.TurretTargetSmallGrids;
							turret.TargetLargeGrids = profile.TurretTargetLargeGrids;
							turret.TargetStations = profile.TurretTargetStations;
							turret.TargetNeutrals = profile.TurretTargetNeutrals;

						}

					}

				}

			}

		}

		//ReplenishSystems
		public static void ReplenishSystems(MyObjectBuilder_TerminalBlock block) {

			var tank = block as MyObjectBuilder_GasTank;

			if (tank != null) {

				tank.FilledRatio = 1;

			}

			var battery = block as MyObjectBuilder_BatteryBlock;

			if (battery != null) {

				float maxStored = 0;

				if (DefinitionHelper.BatteryMaxCapacityReference.TryGetValue(battery.GetId(), out maxStored)) {

					battery.CurrentStoredPower = maxStored;
					battery.MaxStoredPower = maxStored;

				}
				
			}

		}

		//Disable Block 
		public static void ApplyBlockDisable(MyObjectBuilder_FunctionalBlock block, ConfigBase config) {

			if (block == null || config == null)
				return;

			var id = block.GetId();

			if (config.DisableBlocksByType != null && config.DisableBlocksByType.Contains<string>(id.TypeId.ToString())) {

				block.Enabled = false;
				return;

			}

			if (config.DisableBlocksByDefinitionId != null && config.DisableBlocksDefinitionList.Contains(id)) {

				block.Enabled = false;
				return;

			}


		}

		//GetGridMass
		public static float GetGridMass(MyObjectBuilder_CubeGrid grid) {

			float result = 0;

			foreach (var block in grid.CubeBlocks) {

				float mass = 0;

				if (DefinitionHelper.BlockWeightReference.TryGetValue(block.GetId(), out mass))
					result += mass;
			
			}

			return result;
		
		}

	}

}
