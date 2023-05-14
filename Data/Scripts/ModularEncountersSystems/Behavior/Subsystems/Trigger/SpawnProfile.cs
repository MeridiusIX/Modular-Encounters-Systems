using ProtoBuf;
using ModularEncountersSystems.Helpers;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRageMath;
using ModularEncountersSystems.Logging;

namespace ModularEncountersSystems.Behavior.Subsystems.Trigger {

	[ProtoContract]
	public class SpawnProfile {

		[ProtoMember(1)]
		public bool UseSpawn;

		[ProtoMember(2)]
		public bool StartsReady;

		[ProtoMember(3)]
		public float FirstSpawnTimeMs;

		[ProtoMember(4)]
		public float SpawnMinCooldown;

		[ProtoMember(5)]
		public float SpawnMaxCooldown;

		[ProtoMember(6)]
		public int MaxSpawns;

		[ProtoMember(7)]
		public List<string> SpawnGroups;

		[ProtoMember(8)]
		public int CooldownTime;

		[ProtoMember(9)]
		public int SpawnCount;

		[ProtoMember(10)]
		public DateTime LastSpawnTime;

		[ProtoMember(11)]
		public bool UseRelativeSpawnPosition;

		[ProtoMember(12)]
		public double MinDistance;

		[ProtoMember(13)]
		public double MaxDistance;

		[ProtoMember(14)]
		public double MinAltitude;

		[ProtoMember(15)]
		public double MaxAltitude;

		[ProtoMember(16)]
		public Vector3D RelativeSpawnOffset;

		[ProtoMember(17)]
		public Vector3D RelativeSpawnVelocity;

		[ProtoMember(18)]
		public bool IgnoreSafetyChecks;

		[ProtoMember(19)]
		public bool InheritNpcAltitude;

		[ProtoMember(20)]
		public string ProfileSubtypeId;

		[ProtoMember(21)]
		public bool ForceSameFactionOwnership;

		[ProtoMember(22)]
		public SpawnTypeEnum SpawningType;

		[ProtoMember(23)]
		public Direction CustomRelativeForward;

		[ProtoMember(24)]
		public Direction CustomRelativeUp;

		[ProtoMember(25)]
		public string ParentGridNameRequirement;

		[ProtoMember(26)]
		public short FailedAttemptsToIncreaseCount;

		[ProtoMember(27)]
		public short FailedAttempts;

		[ProtoMember(28)]
		public bool ProcessAsAdminSpawn;

		[ProtoIgnore]
		public MatrixD CurrentPositionMatrix;

		[ProtoIgnore]
		public string CurrentFactionTag;

		[ProtoIgnore]
		public Random Rnd;

		public SpawnProfile() {

			UseSpawn = false;
			StartsReady = false;
			FirstSpawnTimeMs = 0;
			SpawnMinCooldown = 0;
			SpawnMaxCooldown = 1;
			MaxSpawns = -1;
			SpawnGroups = new List<string>();

			CooldownTime = 0;
			SpawnCount = 0;
			LastSpawnTime = MyAPIGateway.Session.GameDateTime;

			UseRelativeSpawnPosition = false;
			MinDistance = 0;
			MaxDistance = 1;
			MinAltitude = 0;
			MaxAltitude = 1;
			RelativeSpawnOffset = Vector3D.Zero;
			RelativeSpawnVelocity = Vector3D.Zero;
			IgnoreSafetyChecks = false;
			InheritNpcAltitude = false;
			ProfileSubtypeId = "";

			ForceSameFactionOwnership = false;

			SpawningType = SpawnTypeEnum.CustomSpawn;

			CustomRelativeForward = Direction.None;
			CustomRelativeUp = Direction.None;

			ParentGridNameRequirement = "";

			FailedAttemptsToIncreaseCount = 5;

			ProcessAsAdminSpawn = false;

			CurrentPositionMatrix = MatrixD.Identity;
			CurrentFactionTag = "";
			Rnd = new Random();

		}

		public void AssignInitialMatrix(MatrixD initialMatrix) {

			var position = initialMatrix.Translation;
			var forward = initialMatrix.Forward;
			var up = initialMatrix.Up;

			if (CustomRelativeForward != Direction.None)
				forward = GetDirectionFromMatrixAndEnum(initialMatrix, CustomRelativeForward);

			if (CustomRelativeUp != Direction.None)
				up = GetDirectionFromMatrixAndEnum(initialMatrix, CustomRelativeUp);

			if (Vector3D.ArePerpendicular(ref forward, ref up))
				CurrentPositionMatrix = MatrixD.CreateWorld(position, forward, up);
			else {

				CurrentPositionMatrix = initialMatrix;
				//Logger.Write(string.Format("Warning: Custom Spawn Directions [{0}] and [{1}] are not perpendicular. Using default directions instead", CustomRelativeForward, CustomRelativeUp), BehaviorDebugEnum.Spawn);

			}
		}

		public Vector3D GetDirectionFromMatrixAndEnum(MatrixD matrix, Direction direction) {

			if (direction == Direction.None)
				return Vector3D.Zero;

			if (direction == Direction.Forward)
				return matrix.Forward;

			if (direction == Direction.Backward)
				return matrix.Backward;

			if (direction == Direction.Left)
				return matrix.Left;

			if (direction == Direction.Right)
				return matrix.Right;

			if (direction == Direction.Up)
				return matrix.Up;

			return matrix.Down;

		}

		public bool IsReadyToSpawn() {

			if (UseSpawn == false)
				return false;

			if (MaxSpawns >= 0 && SpawnCount >= MaxSpawns) {



				BehaviorLogger.Write(ProfileSubtypeId + ": Max Spawns Already Exceeded", BehaviorDebugEnum.Spawn);
				UseSpawn = false;
				return false;

			}

			TimeSpan duration = MyAPIGateway.Session.GameDateTime - LastSpawnTime;

			if (duration.TotalSeconds < CooldownTime) {

				if (StartsReady == true) {

					BehaviorLogger.Write(ProfileSubtypeId + ": Spawn Cooldown Not Finished", BehaviorDebugEnum.Spawn);
					if (SpawnCount > 0)
						return false;

				} else {

					BehaviorLogger.Write(ProfileSubtypeId + ": Spawn Cooldown Not Finished", BehaviorDebugEnum.Spawn);
					return false;

				}

			}

			BehaviorLogger.Write(ProfileSubtypeId + ": Spawn Is Ready to Spawn", BehaviorDebugEnum.Spawn);
			return true;

		}

		public void ProcessSuccessfulSpawn() {

			SpawnCount++;

			if (MaxSpawns >= 0 && SpawnCount >= MaxSpawns) {

				UseSpawn = false;

			}

			TimeSpan duration = MyAPIGateway.Session.GameDateTime - LastSpawnTime;

			if (duration.TotalSeconds < CooldownTime) {

				return;

			}

		}

		public void InitTags(string customData) {

			if (string.IsNullOrWhiteSpace(customData) == false) {

				var descSplit = customData.Split('\n');

				foreach (var tag in descSplit) {

					//UseSpawn
					if (tag.Contains("[UseSpawn:") == true) {

						TagParse.TagBoolCheck(tag, ref UseSpawn);

					}

					//FirstSpawnTimeMs
					if (tag.Contains("[FirstSpawnTimeMs:") == true) {

						TagParse.TagFloatCheck(tag, ref FirstSpawnTimeMs);

					}

					//SpawnMinCooldown
					if (tag.Contains("[SpawnMinCooldown:") == true) {

						TagParse.TagFloatCheck(tag, ref SpawnMinCooldown);

					}

					//SpawnMaxCooldown
					if (tag.Contains("[SpawnMaxCooldown:") == true) {

						TagParse.TagFloatCheck(tag, ref SpawnMaxCooldown);

					}

					//MaxSpawns
					if (tag.Contains("MaxSpawns:") == true) {

						TagParse.TagIntCheck(tag, ref MaxSpawns);

					}

					//SpawnGroups
					if (tag.Contains("SpawnGroups:") == true) {

						TagParse.TagStringListCheck(tag, ref SpawnGroups);

					}

					//UseRelativeSpawnPosition
					if (tag.Contains("[UseRelativeSpawnPosition:") == true) {

						TagParse.TagBoolCheck(tag, ref UseRelativeSpawnPosition);

					}

					//MinDistance
					if (tag.Contains("[MinDistance:") == true) {

						TagParse.TagDoubleCheck(tag, ref MinDistance);

					}

					//MaxDistance
					if (tag.Contains("[MaxDistance:") == true) {

						TagParse.TagDoubleCheck(tag, ref MaxDistance);

					}

					//MinAltitude
					if (tag.Contains("[MinAltitude:") == true) {

						TagParse.TagDoubleCheck(tag, ref MinAltitude);

					}

					//MaxAltitude
					if (tag.Contains("[MaxAltitude:") == true) {

						TagParse.TagDoubleCheck(tag, ref MaxAltitude);

					}

					//RelativeSpawnOffset
					if (tag.Contains("[RelativeSpawnOffset:") == true) {

						TagParse.TagVector3DCheck(tag, ref RelativeSpawnOffset);

					}

					//RelativeSpawnVelocity
					if (tag.Contains("[RelativeSpawnVelocity:") == true) {

						TagParse.TagVector3DCheck(tag, ref RelativeSpawnVelocity);

					}

					//IgnoreSafetyChecks
					if (tag.Contains("[IgnoreSafetyChecks:") == true) {

						TagParse.TagBoolCheck(tag, ref IgnoreSafetyChecks);

					}

					//InheritNpcAltitude
					if (tag.Contains("[InheritNpcAltitude:") == true) {

						TagParse.TagBoolCheck(tag, ref InheritNpcAltitude);

					}

					//ForceSameFactionOwnership
					if (tag.Contains("[ForceSameFactionOwnership:") == true) {

						TagParse.TagBoolCheck(tag, ref ForceSameFactionOwnership);

					}

					//SpawningType
					if (tag.Contains("[SpawningType:") == true) {

						TagParse.TagSpawnTypeEnumCheck(tag, ref SpawningType);

					}

					//CustomRelativeForward
					if (tag.Contains("[CustomRelativeForward:") == true) {

						TagParse.TagDirectionEnumCheck(tag, ref CustomRelativeForward);

					}

					//CustomRelativeUp
					if (tag.Contains("[CustomRelativeUp:") == true) {

						TagParse.TagDirectionEnumCheck(tag, ref CustomRelativeUp);

					}

					//ParentGridNameRequirement
					if (tag.Contains("ParentGridNameRequirement:") == true){

						TagParse.TagStringCheck(tag, ref ParentGridNameRequirement);

					}

					//FailedAttemptsToIncreaseCount
					if (tag.Contains("FailedAttemptsToIncreaseCount:") == true) {

						TagParse.TagShortCheck(tag, ref FailedAttemptsToIncreaseCount);

					}

					//ProcessAsAdminSpawn
					if (tag.Contains("[ProcessAsAdminSpawn:") == true) {

						TagParse.TagBoolCheck(tag, ref ProcessAsAdminSpawn);

					}

				}

			}

			if (SpawnMinCooldown > SpawnMaxCooldown) {

				SpawnMinCooldown = SpawnMaxCooldown;

			}

		}

	}
}
