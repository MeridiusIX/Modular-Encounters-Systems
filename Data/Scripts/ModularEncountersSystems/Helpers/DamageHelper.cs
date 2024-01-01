using ModularEncountersSystems.Behavior.Subsystems.Profiles;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Progression;
using ModularEncountersSystems.Sync;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Weapons;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace ModularEncountersSystems.Helpers {

	public static class DamageHelper {

		public static List<IMyCubeGrid> MonitoredGrids = new List<IMyCubeGrid>();
		public static Dictionary<IMyCubeGrid, Action<object, MyDamageInformation>> RegisteredDamageHandlers = new Dictionary<IMyCubeGrid, Action<object, MyDamageInformation>>();

		public static Action<object, MyDamageInformation> DamageRelay;

		public static List<MissileProfile> CurrentMissiles = new List<MissileProfile>();

		public static void Setup() {

			MyAPIGateway.Session.DamageSystem.RegisterBeforeDamageHandler(75, BeforeDamageHandler);
			MyAPIGateway.Session.DamageSystem.RegisterAfterDamageHandler(75, DamageHelper.DamageHandler);

		}

		public static void BeforeDamageHandler(object target, ref MyDamageInformation info) {

			if (target as IMyCharacter == null)
				return;

			var player = PlayerManager.GetPlayerWithCharacter(target as IMyCharacter);

			if (player == null || player.Progression.DamageReductionSuitUpgradeLevel == 0 || !ProgressionContainer.IsUpgradeAllowedInConfig(SuitUpgradeTypes.DamageReduction))
				return;

			float multiplier = (float)player.Progression.DamageReductionSuitUpgradeLevel * 0.10f;
			float reduction = info.Amount * multiplier;
			info.Amount -= reduction;

		}

		public static void DamageHandler(object target, MyDamageInformation info) {

			var block = target as IMySlimBlock;

			if (block == null) {

				return;

			}

			var grid = block.CubeGrid;

			var newInfo = info;

			if (info.Type.ToString() == "Explosion") {

				try {

					//Logger.AddMsg("Missile List Count: " + CurrentMissiles.Count.ToString(), true);

					for (int i = CurrentMissiles.Count - 1; i >= 0; i--) {

						if (!CurrentMissiles[i].Removed) {

							continue;

						}

						var duration = MyAPIGateway.Session.GameDateTime - CurrentMissiles[i].RemovalTime;

						//Logger.AddMsg("TimeDiff: " + duration, true);

						if (duration.TotalMilliseconds > 1000) {

							//Logger.AddMsg("Removing Dead Missile", true);
							CurrentMissiles.RemoveAt(i);
							continue;

						}

						var dist = Vector3D.Distance(CurrentMissiles[i].RemovalCoords, grid.GridIntegerToWorld(block.Min));

						if (dist > CurrentMissiles[i].ExplodeRadius * 2) {

							//Logger.AddMsg("Distance Fail: " + dist + " vs " + CurrentMissiles[i].ExplodeRadius.ToString(), true);
							continue;

						}

						if (CurrentMissiles[i].HitObjects.Contains(target)) {

							//Logger.AddMsg("Object Already Hit", true);
							continue;

						}

						//Logger.AddMsg("AttackerId Set: " + CurrentMissiles[i].LauncherId, true);
						newInfo.AttackerId = CurrentMissiles[i].LauncherId;
						CurrentMissiles[i].HitObjects.Add(target);

					}

				} catch (Exception) {

					//Logger.AddMsg("Got Crash in Damage Handler For Missiles", true);

				}

			}

			DamageRelay?.Invoke(target, newInfo);

		}

		public static void ApplyDamageToTarget(long entityId, float amount, string particleEffect, string soundEffect) {

			if (entityId == 0)
				return;

			IMyEntity entity = null;

			if (MyAPIGateway.Entities.TryGetEntityById(entityId, out entity) == false)
				return;

			if (entity as IMyCubeGrid != null)
				return;

			var tool = entity as IMyEngineerToolBase;
			var block = entity as IMyShipToolBase;
			bool didDamage = false;
			MatrixD damagePositionMatrix = MatrixD.Identity;
			Vector3 damagerVelocity = Vector3.Zero;

			if (tool != null) {

				IMyEntity characterEntity = null;

				if (MyAPIGateway.Entities.TryGetEntityById(tool.OwnerId, out characterEntity)) {

					var character = characterEntity as IMyCharacter;

					if (character != null) {

						character.DoDamage(amount, MyStringHash.GetOrCompute("Electrocution"), true);
						damagePositionMatrix = character.WorldMatrix;

						if (character.Physics != null)
							damagerVelocity = character.Physics.LinearVelocity;

						didDamage = true;

					}

				}

			}

			if (block != null) {

				block.SlimBlock.DoDamage(amount, MyStringHash.GetOrCompute("Electrocution"), true);
				damagePositionMatrix = block.WorldMatrix;

				if (block?.SlimBlock?.CubeGrid?.Physics != null)
					damagerVelocity = block.SlimBlock.CubeGrid.Physics.LinearVelocity;

				didDamage = true;

			}

			if (didDamage == false)
				return;

			if (!string.IsNullOrWhiteSpace(soundEffect)) {

				var effect = new Effects();
				effect.Mode = EffectSyncMode.PositionSound;
				effect.SoundId = soundEffect;
				effect.Coords = damagePositionMatrix.Translation;
				var syncContainer = new SyncContainer(effect);
				SyncManager.SendSyncMesage(syncContainer, 0, true, true);

			}

			if (!string.IsNullOrWhiteSpace(particleEffect)) {

				var effect = new Effects();
				effect.Mode = EffectSyncMode.Particle;
				effect.ParticleId = particleEffect;
				effect.Coords = damagePositionMatrix.Translation;
				effect.ParticleForwardDir = damagePositionMatrix.Forward;
				effect.ParticleUpDir = damagePositionMatrix.Up;
				var syncContainer = new SyncContainer(effect);
				SyncManager.SendSyncMesage(syncContainer, 0, true, true);

			}

		}

		public static void CreateExplosion(Vector3D coords, int radius, int damage, IMyEntity ownerEntity, bool damageIgnoreVoxels) {

			MyExplosionTypeEnum myExplosionTypeEnum = MyExplosionTypeEnum.WARHEAD_EXPLOSION_02;
			myExplosionTypeEnum = ((radius <= 6.0) ? MyExplosionTypeEnum.WARHEAD_EXPLOSION_02 : ((radius <= 20.0) ? MyExplosionTypeEnum.WARHEAD_EXPLOSION_15 : ((!(radius <= 40.0)) ? MyExplosionTypeEnum.WARHEAD_EXPLOSION_50 : MyExplosionTypeEnum.WARHEAD_EXPLOSION_30)));
			MyExplosionInfo myExplosionInfo = default(MyExplosionInfo);
			myExplosionInfo.PlayerDamage = damage;
			myExplosionInfo.Damage = damage;
			myExplosionInfo.ExplosionType = myExplosionTypeEnum;
			myExplosionInfo.ExplosionSphere = new BoundingSphereD(coords, radius);
			myExplosionInfo.LifespanMiliseconds = 700;
			myExplosionInfo.HitEntity = ownerEntity as MyEntity;
			myExplosionInfo.ParticleScale = 1f;
			myExplosionInfo.OwnerEntity = ownerEntity as MyEntity;
			myExplosionInfo.Direction = Vector3.Forward;
			myExplosionInfo.VoxelExplosionCenter = coords;

			var fakeExplosionInfoFlags = (FakeExplosionFlags.CREATE_DEBRIS | FakeExplosionFlags.APPLY_FORCE_AND_DAMAGE | FakeExplosionFlags.CREATE_DECALS | FakeExplosionFlags.CREATE_PARTICLE_EFFECT | FakeExplosionFlags.CREATE_SHRAPNELS | FakeExplosionFlags.APPLY_DEFORMATION);
			//myExplosionInfo.ExplosionFlags = (MyExplosionFlags.CREATE_DEBRIS | MyExplosionFlags.APPLY_FORCE_AND_DAMAGE | MyExplosionFlags.CREATE_DECALS | MyExplosionFlags.CREATE_PARTICLE_EFFECT | MyExplosionFlags.CREATE_SHRAPNELS | MyExplosionFlags.APPLY_DEFORMATION);


			if (!damageIgnoreVoxels) {

				fakeExplosionInfoFlags |= FakeExplosionFlags.AFFECT_VOXELS;
				myExplosionInfo.VoxelCutoutScale = 1f;

			}

			Enum.TryParse((fakeExplosionInfoFlags.ToString()), out myExplosionInfo.ExplosionFlags);

			myExplosionInfo.PlaySound = true;
			myExplosionInfo.ApplyForceAndDamage = true;
			myExplosionInfo.ObjectsRemoveDelayInMiliseconds = 40;
			MyExplosionInfo explosionInfo = myExplosionInfo;
			
			MyExplosions.AddExplosion(ref explosionInfo);

		}

		public static void CreateLightning(Vector3D coords, int damage, int radius, Vector3D color) {

			var lightning = new MyObjectBuilder_WeatherLightning();
			lightning.Damage = damage;
			lightning.ExplosionRadius = radius;
			lightning.Color = new Vector4((float)color.X, (float)color.Y, (float)color.Z, 1000);
			lightning.Position = coords;
			MyAPIGateway.Session.WeatherEffects.CreateLightning(coords, lightning, true);

		}

		public static long GetAttackOwnerId(long attackingEntity) {

			IMyEntity entity = null;

			if (!MyAPIGateway.Entities.TryGetEntityById(attackingEntity, out entity))
				return 0;

			var handGun = entity as IMyGunBaseUser;
			var handTool = entity as IMyEngineerToolBase;

			if (handGun != null) {

				return handGun.OwnerId;

			}

			if (handTool != null) {

				return handTool.OwnerIdentityId;

			}

			var cubeGrid = entity as IMyCubeGrid;
			var block = entity as IMyCubeBlock;

			if (block != null) {

				cubeGrid = block.SlimBlock.CubeGrid;

			}

			if (cubeGrid == null)
				return 0;

			var shipControllers = BlockCollectionHelper.GetGridControllers(cubeGrid);

			IMyPlayer controlPlayer = null;

			foreach (var controller in shipControllers) {

				var player = MyAPIGateway.Players.GetPlayerControllingEntity(controller);

				if (player == null)
					continue;

				controlPlayer = player;

				if (controller.IsMainCockpit || (controller.CanControlShip && controller.IsUnderControl))
					break;

			}

			long owner = 0;

			if (controlPlayer != null) {

				owner = controlPlayer.IdentityId;

			} else {

				if (cubeGrid.BigOwners.Count > 0)
					owner = cubeGrid.BigOwners[0];

			}

			if (owner == 0) {

				var identityList = new List<IMyIdentity>();
				MyAPIGateway.Players.GetAllIdentites(identityList, x => x.IdentityId == attackingEntity);
				if (identityList.Count > 0)
					owner = attackingEntity;

			}

			return owner;

		}

		public static void NewEntityDetected(IMyEntity entity) {

			var ob = new MyObjectBuilder_Missile();

			try {

				ob = entity.GetObjectBuilder() as MyObjectBuilder_Missile;

			} catch (Exception) {

				return;

			}

			if (ob == null) {

				return;

			}

			CurrentMissiles.Add(new MissileProfile(entity, ob));

		}

		public static void EntityRemoved(IMyEntity entity) {

			try {

				for (int i = 0; i < CurrentMissiles.Count; i++) {

					if (CurrentMissiles[i].EntityId == entity.EntityId) {

						CurrentMissiles[i].Removed = true;
						CurrentMissiles[i].RemovalCoords = entity.GetPosition();
						CurrentMissiles[i].RemovalTime = MyAPIGateway.Session.GameDateTime;
						//Logger.AddMsg("Missile Removed: " + CurrentMissiles[i].EntityId.ToString(), true);
						//Logger.AddMsg("Missile Removed Time: " + CurrentMissiles[i].RemovalTime, true);
						break;

					}

				}

			} catch (Exception) {



			}

		}

		public static void RegisterEntityWatchers() {

			MyAPIGateway.Entities.OnEntityAdd += NewEntityDetected;
			MyAPIGateway.Entities.OnEntityRemove += EntityRemoved;

		}

		public static void UnregisterEntityWatchers() {

			MyAPIGateway.Entities.OnEntityAdd -= NewEntityDetected;
			MyAPIGateway.Entities.OnEntityRemove -= EntityRemoved;

		}

	}

}
