using System.Collections.Generic;
using ProtoBuf;
using VRageMath;

namespace ModularEncountersSystems.API {
    public static class WcApiDef {
        [ProtoContract]
        public class ContainerDefinition {
            [ProtoMember(1)] internal WeaponDefinition[] WeaponDefs;
            [ProtoMember(2)] internal ArmorDefinition[] ArmorDefs;
            [ProtoMember(3)] internal UpgradeDefinition[] UpgradeDefs;
            [ProtoMember(4)] internal SupportDefinition[] SupportDefs;
        }

        [ProtoContract]
        public class ConsumeableDef {
            [ProtoMember(1)] internal string ItemName;
            [ProtoMember(2)] internal string InventoryItem;
            [ProtoMember(3)] internal int ItemsNeeded;
            [ProtoMember(4)] internal bool Hybrid;
            [ProtoMember(5)] internal float EnergyCost;
            [ProtoMember(6)] internal float Strength;
        }

        [ProtoContract]
        public class UpgradeDefinition {
            [ProtoMember(1)] internal ModelAssignmentsDef Assignments;
            [ProtoMember(2)] internal HardPointDef HardPoint;
            [ProtoMember(3)] internal WeaponDefinition.AnimationDef Animations;
            [ProtoMember(4)] internal string ModPath;
            [ProtoMember(5)] internal ConsumeableDef[] Consumable;

            [ProtoContract]
            public struct ModelAssignmentsDef {
                [ProtoMember(1)] internal MountPointDef[] MountPoints;

                [ProtoContract]
                public struct MountPointDef {
                    [ProtoMember(1)] internal string SubtypeId;
                    [ProtoMember(2)] internal float DurabilityMod;
                    [ProtoMember(3)] internal string IconName;
                }
            }

            [ProtoContract]
            public struct HardPointDef {
                [ProtoMember(1)] internal string PartName;
                [ProtoMember(2)] internal HardwareDef HardWare;
                [ProtoMember(3)] internal UiDef Ui;
                [ProtoMember(4)] internal OtherDef Other;

                [ProtoContract]
                public struct UiDef {
                    [ProtoMember(1)] internal bool StrengthModifier;
                }

                [ProtoContract]
                public struct HardwareDef {
                    public enum HardwareType {
                        Default,
                    }

                    [ProtoMember(1)] internal float InventorySize;
                    [ProtoMember(2)] internal HardwareType Type;
                    [ProtoMember(3)] internal int BlockDistance;

                }

                [ProtoContract]
                public struct OtherDef {
                    [ProtoMember(1)] internal int ConstructPartCap;
                    [ProtoMember(2)] internal int EnergyPriority;
                    [ProtoMember(3)] internal bool Debug;
                    [ProtoMember(4)] internal double RestrictionRadius;
                    [ProtoMember(5)] internal bool CheckInflatedBox;
                    [ProtoMember(6)] internal bool CheckForAnySupport;
                    [ProtoMember(7)] internal bool StayCharged;
                }
            }
        }

        [ProtoContract]
        public class SupportDefinition {
            [ProtoMember(1)] internal ModelAssignmentsDef Assignments;
            [ProtoMember(2)] internal HardPointDef HardPoint;
            [ProtoMember(3)] internal WeaponDefinition.AnimationDef Animations;
            [ProtoMember(4)] internal string ModPath;
            [ProtoMember(5)] internal ConsumeableDef[] Consumable;
            [ProtoMember(6)] internal SupportEffect Effect;

            [ProtoContract]
            public struct ModelAssignmentsDef {
                [ProtoMember(1)] internal MountPointDef[] MountPoints;

                [ProtoContract]
                public struct MountPointDef {
                    [ProtoMember(1)] internal string SubtypeId;
                    [ProtoMember(2)] internal float DurabilityMod;
                    [ProtoMember(3)] internal string IconName;
                }
            }
            [ProtoContract]
            public struct HardPointDef {
                [ProtoMember(1)] internal string PartName;
                [ProtoMember(2)] internal HardwareDef HardWare;
                [ProtoMember(3)] internal UiDef Ui;
                [ProtoMember(4)] internal OtherDef Other;

                [ProtoContract]
                public struct UiDef {
                    [ProtoMember(1)] internal bool ProtectionControl;
                }

                [ProtoContract]
                public struct HardwareDef {
                    [ProtoMember(1)] internal float InventorySize;
                }

                [ProtoContract]
                public struct OtherDef {
                    [ProtoMember(1)] internal int ConstructPartCap;
                    [ProtoMember(2)] internal int EnergyPriority;
                    [ProtoMember(3)] internal bool Debug;
                    [ProtoMember(4)] internal double RestrictionRadius;
                    [ProtoMember(5)] internal bool CheckInflatedBox;
                    [ProtoMember(6)] internal bool CheckForAnySupport;
                    [ProtoMember(7)] internal bool StayCharged;
                }
            }

            [ProtoContract]
            public struct SupportEffect {
                public enum AffectedBlocks {
                    Armor,
                    ArmorPlus,
                    PlusFunctional,
                    All,
                }

                public enum Protections {
                    KineticProt,
                    EnergeticProt,
                    GenericProt,
                    Regenerate,
                    Structural,
                }

                [ProtoMember(1)] internal Protections Protection;
                [ProtoMember(2)] internal AffectedBlocks Affected;
                [ProtoMember(3)] internal int BlockRange;
                [ProtoMember(4)] internal int MaxPoints;
                [ProtoMember(5)] internal int PointsPerCharge;
                [ProtoMember(6)] internal int UsablePerSecond;
                [ProtoMember(7)] internal int UsablePerMinute;
                [ProtoMember(8)] internal float Overflow;
                [ProtoMember(9)] internal float Effectiveness;
                [ProtoMember(10)] internal float ProtectionMin;
                [ProtoMember(11)] internal float ProtectionMax;
            }
        }

        [ProtoContract]
        public class ArmorDefinition {
            internal enum ArmorType {
                Light,
                Heavy,
                NonArmor,
            }

            [ProtoMember(1)] internal string[] SubtypeIds;
            [ProtoMember(2)] internal ArmorType Kind;
            [ProtoMember(3)] internal double KineticResistance;
            [ProtoMember(4)] internal double EnergeticResistance;
        }

        [ProtoContract]
        public class WeaponDefinition {
            [ProtoMember(1)] internal ModelAssignmentsDef Assignments;
            [ProtoMember(2)] internal TargetingDef Targeting;
            [ProtoMember(3)] internal AnimationDef Animations;
            [ProtoMember(4)] internal HardPointDef HardPoint;
            [ProtoMember(5)] internal AmmoDef[] Ammos;
            [ProtoMember(6)] internal string ModPath;
            [ProtoMember(7)] internal Dictionary<string, UpgradeValues[]> Upgrades;

            [ProtoContract]
            public struct ModelAssignmentsDef {
                [ProtoMember(1)] internal MountPointDef[] MountPoints;
                [ProtoMember(2)] internal string[] Muzzles;
                [ProtoMember(3)] internal string Ejector;
                [ProtoMember(4)] internal string Scope;

                [ProtoContract]
                public struct MountPointDef {
                    [ProtoMember(1)] internal string SubtypeId;
                    [ProtoMember(2)] internal string SpinPartId;
                    [ProtoMember(3)] internal string MuzzlePartId;
                    [ProtoMember(4)] internal string AzimuthPartId;
                    [ProtoMember(5)] internal string ElevationPartId;
                    [ProtoMember(6)] internal float DurabilityMod;
                    [ProtoMember(7)] internal string IconName;
                }
            }

            [ProtoContract]
            public struct TargetingDef {
                public enum Threat {
                    Projectiles,
                    Characters,
                    Grids,
                    Neutrals,
                    Meteors,
                    Other,
                    ScanNeutralGrid,
                    ScanFriendlyGrid,
                    ScanFriendlyCharacter,
                    ScanRoid,
                    ScanPlanet,
                    ScanEnemyCharacter,
                    ScanEnemyGrid,
                    ScanNeutralCharacter,
                    ScanUnOwnedGrid,
                    ScanOwnersGrid
                }

                public enum BlockTypes {
                    Any,
                    Offense,
                    Utility,
                    Power,
                    Production,
                    Thrust,
                    Jumping,
                    Steering
                }

                [ProtoMember(1)] internal int TopTargets;
                [ProtoMember(2)] internal int TopBlocks;
                [ProtoMember(3)] internal double StopTrackingSpeed;
                [ProtoMember(4)] internal float MinimumDiameter;
                [ProtoMember(5)] internal float MaximumDiameter;
                [ProtoMember(6)] internal bool ClosestFirst;
                [ProtoMember(7)] internal BlockTypes[] SubSystems;
                [ProtoMember(8)] internal Threat[] Threats;
                [ProtoMember(9)] internal float MaxTargetDistance;
                [ProtoMember(10)] internal float MinTargetDistance;
                [ProtoMember(11)] internal bool IgnoreDumbProjectiles;
                [ProtoMember(12)] internal bool LockedSmartOnly;
                [ProtoMember(13)] internal bool UniqueTargetPerWeapon;
                [ProtoMember(14)] internal int MaxTrackingTime;
                [ProtoMember(15)] internal bool ShootBlanks;
                [ProtoMember(19)] internal CommunicationDef Communications;
                [ProtoMember(20)] internal bool FocusOnly;
                [ProtoMember(21)] internal bool EvictUniqueTargets;
                [ProtoMember(22)] internal int CycleTargets;
                [ProtoMember(23)] internal int CycleBlocks;

                [ProtoContract]
                public struct CommunicationDef {
                    public enum Comms {
                        NoComms,
                        BroadCast,
                        Relay,
                        Jamming,
                        RelayAndBroadCast,
                    }

                    public enum SecurityMode {
                        Public,
                        Private,
                        Secure,
                    }

                    [ProtoMember(1)] internal bool StoreTargets;
                    [ProtoMember(2)] internal int StorageLimit;
                    [ProtoMember(3)] internal string StorageLocation;
                    [ProtoMember(4)] internal Comms Mode;
                    [ProtoMember(5)] internal SecurityMode Security;
                    [ProtoMember(6)] internal string BroadCastChannel;
                    [ProtoMember(7)] internal double BroadCastRange;
                    [ProtoMember(8)] internal double JammingStrength;
                    [ProtoMember(9)] internal string RelayChannel;
                    [ProtoMember(10)] internal double RelayRange;
                    [ProtoMember(11)] internal bool TargetPersists;
                    [ProtoMember(12)] internal bool StoreLimitPerBlock;
                    [ProtoMember(13)] internal int MaxConnections;
                }
            }


            [ProtoContract]
            public struct AnimationDef {
                [ProtoMember(1)] internal PartAnimationSetDef[] AnimationSets;
                [ProtoMember(2)] internal PartEmissive[] Emissives;
                [ProtoMember(3)] internal string[] HeatingEmissiveParts;
                [ProtoMember(4)] internal Dictionary<PartAnimationSetDef.EventTriggers, EventParticle[]> EventParticles;

                [ProtoContract(IgnoreListHandling = true)]
                public struct PartAnimationSetDef {
                    public enum EventTriggers {
                        Reloading,
                        Firing,
                        Tracking,
                        Overheated,
                        TurnOn,
                        TurnOff,
                        BurstReload,
                        NoMagsToLoad,
                        PreFire,
                        EmptyOnGameLoad,
                        StopFiring,
                        StopTracking,
                        LockDelay,
                    }

                    public enum ResetConditions {
                        None,
                        Home,
                        Off,
                        On,
                        Reloaded
                    }

                    [ProtoMember(1)] internal string[] SubpartId;
                    [ProtoMember(2)] internal string BarrelId;
                    [ProtoMember(3)] internal uint StartupFireDelay;
                    [ProtoMember(4)] internal Dictionary<EventTriggers, uint> AnimationDelays;
                    [ProtoMember(5)] internal EventTriggers[] Reverse;
                    [ProtoMember(6)] internal EventTriggers[] Loop;
                    [ProtoMember(7)] internal Dictionary<EventTriggers, RelMove[]> EventMoveSets;
                    [ProtoMember(8)] internal EventTriggers[] TriggerOnce;
                    [ProtoMember(9)] internal EventTriggers[] ResetEmissives;
                    [ProtoMember(10)] internal ResetConditions Resets;

                }

                [ProtoContract]
                public struct PartEmissive {
                    [ProtoMember(1)] internal string EmissiveName;
                    [ProtoMember(2)] internal string[] EmissivePartNames;
                    [ProtoMember(3)] internal bool CycleEmissivesParts;
                    [ProtoMember(4)] internal bool LeavePreviousOn;
                    [ProtoMember(5)] internal Vector4[] Colors;
                    [ProtoMember(6)] internal float[] IntensityRange;
                }
                [ProtoContract]
                public struct EventParticle {
                    [ProtoMember(1)] internal string[] EmptyNames;
                    [ProtoMember(2)] internal string[] MuzzleNames;
                    [ProtoMember(3)] internal ParticleDef Particle;
                    [ProtoMember(4)] internal uint StartDelay;
                    [ProtoMember(5)] internal uint LoopDelay;
                    [ProtoMember(6)] internal bool ForceStop;
                }
                [ProtoContract]
                internal struct RelMove {
                    public enum MoveType {
                        Linear,
                        ExpoDecay,
                        ExpoGrowth,
                        Delay,
                        Show, //instant or fade
                        Hide, //instant or fade
                    }

                    [ProtoMember(1)] internal MoveType MovementType;
                    [ProtoMember(2)] internal XYZ[] LinearPoints;
                    [ProtoMember(3)] internal XYZ Rotation;
                    [ProtoMember(4)] internal XYZ RotAroundCenter;
                    [ProtoMember(5)] internal uint TicksToMove;
                    [ProtoMember(6)] internal string CenterEmpty;
                    [ProtoMember(7)] internal bool Fade;
                    [ProtoMember(8)] internal string EmissiveName;

                    [ProtoContract]
                    internal struct XYZ {
                        [ProtoMember(1)] internal double x;
                        [ProtoMember(2)] internal double y;
                        [ProtoMember(3)] internal double z;
                    }
                }
            }

            [ProtoContract]
            public struct UpgradeValues {
                [ProtoMember(1)] internal string[] Ammo;
                [ProtoMember(2)] internal Dependency[] Dependencies;
                [ProtoMember(3)] internal int RateOfFireMod;
                [ProtoMember(4)] internal int BarrelsPerShotMod;
                [ProtoMember(5)] internal int ReloadMod;
                [ProtoMember(6)] internal int MaxHeatMod;
                [ProtoMember(7)] internal int HeatSinkRateMod;
                [ProtoMember(8)] internal int ShotsInBurstMod;
                [ProtoMember(9)] internal int DelayAfterBurstMod;
                [ProtoMember(10)] internal int AmmoPriority;

                [ProtoContract]
                public struct Dependency {
                    internal string SubtypeId;
                    internal int Quanity;
                }
            }

            [ProtoContract]
            public struct HardPointDef {
                public enum Prediction {
                    Off,
                    Basic,
                    Accurate,
                    Advanced,
                }

                [ProtoMember(1)] internal string PartName;
                [ProtoMember(2)] internal int DelayCeaseFire;
                [ProtoMember(3)] internal float DeviateShotAngle;
                [ProtoMember(4)] internal double AimingTolerance;
                [ProtoMember(5)] internal Prediction AimLeadingPrediction;
                [ProtoMember(6)] internal LoadingDef Loading;
                [ProtoMember(7)] internal AiDef Ai;
                [ProtoMember(8)] internal HardwareDef HardWare;
                [ProtoMember(9)] internal UiDef Ui;
                [ProtoMember(10)] internal HardPointAudioDef Audio;
                [ProtoMember(11)] internal HardPointParticleDef Graphics;
                [ProtoMember(12)] internal OtherDef Other;
                [ProtoMember(13)] internal bool AddToleranceToTracking;
                [ProtoMember(14)] internal bool CanShootSubmerged;
                [ProtoMember(15)] internal bool NpcSafe;
                [ProtoMember(16)] internal bool ScanTrackOnly;

                [ProtoContract]
                public struct LoadingDef {
                    [ProtoMember(1)] internal int ReloadTime;
                    [ProtoMember(2)] internal int RateOfFire;
                    [ProtoMember(3)] internal int BarrelsPerShot;
                    [ProtoMember(4)] internal int SkipBarrels;
                    [ProtoMember(5)] internal int TrajectilesPerBarrel;
                    [ProtoMember(6)] internal int HeatPerShot;
                    [ProtoMember(7)] internal int MaxHeat;
                    [ProtoMember(8)] internal int HeatSinkRate;
                    [ProtoMember(9)] internal float Cooldown;
                    [ProtoMember(10)] internal int DelayUntilFire;
                    [ProtoMember(11)] internal int ShotsInBurst;
                    [ProtoMember(12)] internal int DelayAfterBurst;
                    [ProtoMember(13)] internal bool DegradeRof;
                    [ProtoMember(14)] internal int BarrelSpinRate;
                    [ProtoMember(15)] internal bool FireFull;
                    [ProtoMember(16)] internal bool GiveUpAfter;
                    [ProtoMember(17)] internal bool DeterministicSpin;
                    [ProtoMember(18)] internal bool SpinFree;
                    [ProtoMember(19)] internal bool StayCharged;
                    [ProtoMember(20)] internal int MagsToLoad;
                    [ProtoMember(21)] internal int MaxActiveProjectiles;
                    [ProtoMember(22)] internal int MaxReloads;
                    [ProtoMember(23)] internal bool GoHomeToReload;
                    [ProtoMember(24)] internal bool DropTargetUntilLoaded;
                }


                [ProtoContract]
                public struct UiDef {
                    [ProtoMember(1)] internal bool RateOfFire;
                    [ProtoMember(2)] internal bool DamageModifier;
                    [ProtoMember(3)] internal bool ToggleGuidance;
                    [ProtoMember(4)] internal bool EnableOverload;
                    [ProtoMember(5)] internal bool AlternateUi;
                    [ProtoMember(6)] internal bool DisableStatus;
                }


                [ProtoContract]
                public struct AiDef {
                    [ProtoMember(1)] internal bool TrackTargets;
                    [ProtoMember(2)] internal bool TurretAttached;
                    [ProtoMember(3)] internal bool TurretController;
                    [ProtoMember(4)] internal bool PrimaryTracking;
                    [ProtoMember(5)] internal bool LockOnFocus;
                    [ProtoMember(6)] internal bool SuppressFire;
                    [ProtoMember(7)] internal bool OverrideLeads;
                    [ProtoMember(8)] internal int DefaultLeadGroup;
                    [ProtoMember(9)] internal bool TargetGridCenter;
                }

                [ProtoContract]
                public struct HardwareDef {
                    public enum HardwareType {
                        BlockWeapon = 0,
                        HandWeapon = 1,
                        Phantom = 6,
                    }

                    [ProtoMember(1)] internal float RotateRate;
                    [ProtoMember(2)] internal float ElevateRate;
                    [ProtoMember(3)] internal Vector3D Offset;
                    [ProtoMember(4)] internal bool FixedOffset;
                    [ProtoMember(5)] internal int MaxAzimuth;
                    [ProtoMember(6)] internal int MinAzimuth;
                    [ProtoMember(7)] internal int MaxElevation;
                    [ProtoMember(8)] internal int MinElevation;
                    [ProtoMember(9)] internal float InventorySize;
                    [ProtoMember(10)] internal HardwareType Type;
                    [ProtoMember(11)] internal int HomeAzimuth;
                    [ProtoMember(12)] internal int HomeElevation;
                    [ProtoMember(13)] internal CriticalDef CriticalReaction;
                    [ProtoMember(14)] internal float IdlePower;

                    [ProtoContract]
                    public struct CriticalDef {
                        [ProtoMember(1)] internal bool Enable;
                        [ProtoMember(2)] internal int DefaultArmedTimer;
                        [ProtoMember(3)] internal bool PreArmed;
                        [ProtoMember(4)] internal bool TerminalControls;
                        [ProtoMember(5)] internal string AmmoRound;
                    }
                }

                [ProtoContract]
                public struct HardPointAudioDef {
                    [ProtoMember(1)] internal string ReloadSound;
                    [ProtoMember(2)] internal string NoAmmoSound;
                    [ProtoMember(3)] internal string HardPointRotationSound;
                    [ProtoMember(4)] internal string BarrelRotationSound;
                    [ProtoMember(5)] internal string FiringSound;
                    [ProtoMember(6)] internal bool FiringSoundPerShot;
                    [ProtoMember(7)] internal string PreFiringSound;
                    [ProtoMember(8)] internal uint FireSoundEndDelay;
                    [ProtoMember(9)] internal bool FireSoundNoBurst;
                }

                [ProtoContract]
                public struct OtherDef {
                    [ProtoMember(1)] internal int ConstructPartCap;
                    [ProtoMember(2)] internal int EnergyPriority;
                    [ProtoMember(3)] internal int RotateBarrelAxis;
                    [ProtoMember(4)] internal bool MuzzleCheck;
                    [ProtoMember(5)] internal bool Debug;
                    [ProtoMember(6)] internal double RestrictionRadius;
                    [ProtoMember(7)] internal bool CheckInflatedBox;
                    [ProtoMember(8)] internal bool CheckForAnyWeapon;
                    [ProtoMember(9)] internal bool DisableLosCheck;
                    [ProtoMember(10)] internal bool NoVoxelLosCheck;
                }

                [ProtoContract]
                public struct HardPointParticleDef {
                    [ProtoMember(1)] internal ParticleDef Effect1;
                    [ProtoMember(2)] internal ParticleDef Effect2;
                }
            }

            [ProtoContract]
            public class AmmoDef {
                [ProtoMember(1)] internal string AmmoMagazine;
                [ProtoMember(2)] internal string AmmoRound;
                [ProtoMember(3)] internal bool HybridRound;
                [ProtoMember(4)] internal float EnergyCost;
                [ProtoMember(5)] internal float BaseDamage;
                [ProtoMember(6)] internal float Mass;
                [ProtoMember(7)] internal float Health;
                [ProtoMember(8)] internal float BackKickForce;
                [ProtoMember(9)] internal DamageScaleDef DamageScales;
                [ProtoMember(10)] internal ShapeDef Shape;
                [ProtoMember(11)] internal ObjectsHitDef ObjectsHit;
                [ProtoMember(12)] internal TrajectoryDef Trajectory;
                [ProtoMember(13)] internal AreaDamageDef AreaEffect;
                [ProtoMember(14)] internal BeamDef Beams;
                [ProtoMember(15)] internal FragmentDef Fragment;
                [ProtoMember(16)] internal GraphicDef AmmoGraphics;
                [ProtoMember(17)] internal AmmoAudioDef AmmoAudio;
                [ProtoMember(18)] internal bool HardPointUsable;
                [ProtoMember(19)] internal PatternDef Pattern;
                [ProtoMember(20)] internal int EnergyMagazineSize;
                [ProtoMember(21)] internal float DecayPerShot;
                [ProtoMember(22)] internal EjectionDef Ejection;
                [ProtoMember(23)] internal bool IgnoreWater;
                [ProtoMember(24)] internal AreaOfDamageDef AreaOfDamage;
                [ProtoMember(25)] internal EwarDef Ewar;
                [ProtoMember(26)] internal bool IgnoreVoxels;
                [ProtoMember(27)] internal bool Synchronize;
                [ProtoMember(28)] internal double HeatModifier;
                [ProtoMember(29)] internal bool NpcSafe;
                [ProtoMember(30)] internal SynchronizeDef Sync;
                [ProtoMember(31)] internal bool NoGridOrArmorScaling;

                [ProtoContract]
                public struct SynchronizeDef {
                    [ProtoMember(1)] internal bool Full;
                    [ProtoMember(2)] internal bool PointDefense;
                    [ProtoMember(3)] internal bool OnHitDeath;
                }

                [ProtoContract]
                public struct DamageScaleDef {

                    [ProtoMember(1)] internal float MaxIntegrity;
                    [ProtoMember(2)] internal bool DamageVoxels;
                    [ProtoMember(3)] internal float Characters;
                    [ProtoMember(4)] internal bool SelfDamage;
                    [ProtoMember(5)] internal GridSizeDef Grids;
                    [ProtoMember(6)] internal ArmorDef Armor;
                    [ProtoMember(7)] internal CustomScalesDef Custom;
                    [ProtoMember(8)] internal ShieldDef Shields;
                    [ProtoMember(9)] internal FallOffDef FallOff;
                    [ProtoMember(10)] internal double HealthHitModifier;
                    [ProtoMember(11)] internal double VoxelHitModifier;
                    [ProtoMember(12)] internal DamageTypes DamageType;
                    [ProtoMember(13)] internal DeformDef Deform;

                    [ProtoContract]
                    public struct FallOffDef {
                        [ProtoMember(1)] internal float Distance;
                        [ProtoMember(2)] internal float MinMultipler;
                    }

                    [ProtoContract]
                    public struct GridSizeDef {
                        [ProtoMember(1)] internal float Large;
                        [ProtoMember(2)] internal float Small;
                    }

                    [ProtoContract]
                    public struct ArmorDef {
                        [ProtoMember(1)] internal float Armor;
                        [ProtoMember(2)] internal float Heavy;
                        [ProtoMember(3)] internal float Light;
                        [ProtoMember(4)] internal float NonArmor;
                    }

                    [ProtoContract]
                    public struct CustomScalesDef {
                        internal enum SkipMode {
                            NoSkip,
                            Inclusive,
                            Exclusive,
                        }

                        [ProtoMember(1)] internal CustomBlocksDef[] Types;
                        [ProtoMember(2)] internal bool IgnoreAllOthers;
                        [ProtoMember(3)] internal SkipMode SkipOthers;
                    }

                    [ProtoContract]
                    public struct DamageTypes {
                        internal enum Damage {
                            Energy,
                            Kinetic,
                        }

                        [ProtoMember(1)] internal Damage Base;
                        [ProtoMember(2)] internal Damage AreaEffect;
                        [ProtoMember(3)] internal Damage Detonation;
                        [ProtoMember(4)] internal Damage Shield;
                    }

                    [ProtoContract]
                    public struct ShieldDef {
                        internal enum ShieldType {
                            Default,
                            Heal,
                            Bypass,
                            EmpRetired,
                        }

                        [ProtoMember(1)] internal float Modifier;
                        [ProtoMember(2)] internal ShieldType Type;
                        [ProtoMember(3)] internal float BypassModifier;
                        [ProtoMember(4)] internal double HeatModifier;
                    }

                    [ProtoContract]
                    public struct DeformDef {
                        internal enum DeformTypes {
                            HitBlock,
                            AllDamagedBlocks,
                            NoDeform,
                        }

                        [ProtoMember(1)] internal DeformTypes DeformType;
                        [ProtoMember(2)] internal int DeformDelay;
                    }
                }

                [ProtoContract]
                public struct ShapeDef {
                    public enum Shapes {
                        LineShape,
                        SphereShape,
                    }

                    [ProtoMember(1)] internal Shapes Shape;
                    [ProtoMember(2)] internal double Diameter;
                }

                [ProtoContract]
                public struct ObjectsHitDef {
                    [ProtoMember(1)] internal int MaxObjectsHit;
                    [ProtoMember(2)] internal bool CountBlocks;
                }


                [ProtoContract]
                public struct CustomBlocksDef {
                    [ProtoMember(1)] internal string SubTypeId;
                    [ProtoMember(2)] internal float Modifier;
                }

                [ProtoContract]
                public struct GraphicDef {
                    [ProtoMember(1)] internal bool ShieldHitDraw;
                    [ProtoMember(2)] internal float VisualProbability;
                    [ProtoMember(3)] internal string ModelName;
                    [ProtoMember(4)] internal AmmoParticleDef Particles;
                    [ProtoMember(5)] internal LineDef Lines;
                    [ProtoMember(6)] internal DecalDef Decals;

                    [ProtoContract]
                    public struct AmmoParticleDef {
                        [ProtoMember(1)] internal ParticleDef Ammo;
                        [ProtoMember(2)] internal ParticleDef Hit;
                        [ProtoMember(3)] internal ParticleDef Eject;
                    }

                    [ProtoContract]
                    public struct LineDef {
                        internal enum Texture {
                            Normal,
                            Cycle,
                            Chaos,
                            Wave,
                        }
                        public enum FactionColor {
                            DontUse,
                            Foreground,
                            Background,
                        }

                        [ProtoMember(1)] internal TracerBaseDef Tracer;
                        [ProtoMember(2)] internal string TracerMaterial;
                        [ProtoMember(3)] internal Randomize ColorVariance;
                        [ProtoMember(4)] internal Randomize WidthVariance;
                        [ProtoMember(5)] internal TrailDef Trail;
                        [ProtoMember(6)] internal OffsetEffectDef OffsetEffect;
                        [ProtoMember(7)] internal bool DropParentVelocity;

                        [ProtoContract]
                        public struct OffsetEffectDef {
                            [ProtoMember(1)] internal double MaxOffset;
                            [ProtoMember(2)] internal double MinLength;
                            [ProtoMember(3)] internal double MaxLength;
                        }

                        [ProtoContract]
                        public struct TracerBaseDef {
                            [ProtoMember(1)] internal bool Enable;
                            [ProtoMember(2)] internal float Length;
                            [ProtoMember(3)] internal float Width;
                            [ProtoMember(4)] internal Vector4 Color;
                            [ProtoMember(5)] internal uint VisualFadeStart;
                            [ProtoMember(6)] internal uint VisualFadeEnd;
                            [ProtoMember(7)] internal SegmentDef Segmentation;
                            [ProtoMember(8)] internal string[] Textures;
                            [ProtoMember(9)] internal Texture TextureMode;
                            [ProtoMember(10)] internal bool AlwaysDraw;
                            [ProtoMember(11)] internal FactionColor FactionColor;

                            [ProtoContract]
                            public struct SegmentDef {
                                [ProtoMember(1)] internal string Material; //retired
                                [ProtoMember(2)] internal double SegmentLength;
                                [ProtoMember(3)] internal double SegmentGap;
                                [ProtoMember(4)] internal double Speed;
                                [ProtoMember(5)] internal Vector4 Color;
                                [ProtoMember(6)] internal double WidthMultiplier;
                                [ProtoMember(7)] internal bool Reverse;
                                [ProtoMember(8)] internal bool UseLineVariance;
                                [ProtoMember(9)] internal Randomize ColorVariance;
                                [ProtoMember(10)] internal Randomize WidthVariance;
                                [ProtoMember(11)] internal string[] Textures;
                                [ProtoMember(12)] internal bool Enable;
                                [ProtoMember(13)] internal FactionColor FactionColor;
                            }
                        }

                        [ProtoContract]
                        public struct TrailDef {
                            [ProtoMember(1)] internal bool Enable;
                            [ProtoMember(2)] internal string Material;
                            [ProtoMember(3)] internal int DecayTime;
                            [ProtoMember(4)] internal Vector4 Color;
                            [ProtoMember(5)] internal bool Back;
                            [ProtoMember(6)] internal float CustomWidth;
                            [ProtoMember(7)] internal bool UseWidthVariance;
                            [ProtoMember(8)] internal bool UseColorFade;
                            [ProtoMember(9)] internal string[] Textures;
                            [ProtoMember(10)] internal Texture TextureMode;
                            [ProtoMember(11)] internal bool AlwaysDraw;
                            [ProtoMember(12)] internal FactionColor FactionColor;
                        }
                    }

                    [ProtoContract]
                    public struct DecalDef {

                        [ProtoMember(1)] internal int MaxAge;
                        [ProtoMember(2)] internal TextureMapDef[] Map;

                        [ProtoContract]
                        public struct TextureMapDef {
                            [ProtoMember(1)] internal string HitMaterial;
                            [ProtoMember(2)] internal string DecalMaterial;
                        }
                    }
                }

                [ProtoContract]
                public struct BeamDef {
                    [ProtoMember(1)] internal bool Enable;
                    [ProtoMember(2)] internal bool ConvergeBeams;
                    [ProtoMember(3)] internal bool VirtualBeams;
                    [ProtoMember(4)] internal bool RotateRealBeam;
                    [ProtoMember(5)] internal bool OneParticle;
                    [ProtoMember(6)] internal bool FakeVoxelHits;
                }

                [ProtoContract]
                public struct FragmentDef {
                    [ProtoMember(1)] internal string AmmoRound;
                    [ProtoMember(2)] internal int Fragments;
                    [ProtoMember(3)] internal float Radial;
                    [ProtoMember(4)] internal float BackwardDegrees;
                    [ProtoMember(5)] internal float Degrees;
                    [ProtoMember(6)] internal bool Reverse;
                    [ProtoMember(7)] internal bool IgnoreArming;
                    [ProtoMember(8)] internal bool DropVelocity;
                    [ProtoMember(9)] internal float Offset;
                    [ProtoMember(10)] internal int MaxChildren;
                    [ProtoMember(11)] internal TimedSpawnDef TimedSpawns;
                    [ProtoMember(12)] internal bool FireSound;
                    [ProtoMember(13)] internal Vector3D AdvOffset;
                    [ProtoMember(14)] internal bool ArmWhenHit;

                    [ProtoContract]
                    public struct TimedSpawnDef {
                        public enum PointTypes {
                            Direct,
                            Lead,
                            Predict,
                        }

                        [ProtoMember(1)] internal bool Enable;
                        [ProtoMember(2)] internal int Interval;
                        [ProtoMember(3)] internal int StartTime;
                        [ProtoMember(4)] internal int MaxSpawns;
                        [ProtoMember(5)] internal double Proximity;
                        [ProtoMember(6)] internal bool ParentDies;
                        [ProtoMember(7)] internal bool PointAtTarget;
                        [ProtoMember(8)] internal int GroupSize;
                        [ProtoMember(9)] internal int GroupDelay;
                        [ProtoMember(10)] internal PointTypes PointType;
                    }
                }

                [ProtoContract]
                public struct PatternDef {
                    public enum PatternModes {
                        Never,
                        Weapon,
                        Fragment,
                        Both,
                    }


                    [ProtoMember(1)] internal string[] Patterns;
                    [ProtoMember(2)] internal bool Enable;
                    [ProtoMember(3)] internal float TriggerChance;
                    [ProtoMember(4)] internal bool SkipParent;
                    [ProtoMember(5)] internal bool Random;
                    [ProtoMember(6)] internal int RandomMin;
                    [ProtoMember(7)] internal int RandomMax;
                    [ProtoMember(8)] internal int PatternSteps;
                    [ProtoMember(9)] internal PatternModes Mode;
                }

                [ProtoContract]
                public struct EjectionDef {
                    public enum SpawnType {
                        Item,
                        Particle,
                    }
                    [ProtoMember(1)] internal float Speed;
                    [ProtoMember(2)] internal float SpawnChance;
                    [ProtoMember(3)] internal SpawnType Type;
                    [ProtoMember(4)] internal ComponentDef CompDef;

                    [ProtoContract]
                    public struct ComponentDef {
                        [ProtoMember(1)] internal string ItemName;
                        [ProtoMember(2)] internal int ItemLifeTime;
                        [ProtoMember(3)] internal int Delay;
                    }
                }

                [ProtoContract]
                public struct AreaOfDamageDef {
                    public enum Falloff {
                        Legacy,
                        NoFalloff,
                        Linear,
                        Curve,
                        InvCurve,
                        Squeeze,
                        Pooled,
                        Exponential,
                    }

                    public enum AoeShape {
                        Round,
                        Diamond,
                    }

                    [ProtoMember(1)] internal ByBlockHitDef ByBlockHit;
                    [ProtoMember(2)] internal EndOfLifeDef EndOfLife;

                    [ProtoContract]
                    public struct ByBlockHitDef {
                        [ProtoMember(1)] internal bool Enable;
                        [ProtoMember(2)] internal double Radius;
                        [ProtoMember(3)] internal float Damage;
                        [ProtoMember(4)] internal float Depth;
                        [ProtoMember(5)] internal float MaxAbsorb;
                        [ProtoMember(6)] internal Falloff Falloff;
                        [ProtoMember(7)] internal AoeShape Shape;
                    }

                    [ProtoContract]
                    public struct EndOfLifeDef {
                        [ProtoMember(1)] internal bool Enable;
                        [ProtoMember(2)] internal double Radius;
                        [ProtoMember(3)] internal float Damage;
                        [ProtoMember(4)] internal float Depth;
                        [ProtoMember(5)] internal float MaxAbsorb;
                        [ProtoMember(6)] internal Falloff Falloff;
                        [ProtoMember(7)] internal bool ArmOnlyOnHit;
                        [ProtoMember(8)] internal int MinArmingTime;
                        [ProtoMember(9)] internal bool NoVisuals;
                        [ProtoMember(10)] internal bool NoSound;
                        [ProtoMember(11)] internal float ParticleScale;
                        [ProtoMember(12)] internal string CustomParticle;
                        [ProtoMember(13)] internal string CustomSound;
                        [ProtoMember(14)] internal AoeShape Shape;
                    }
                }

                [ProtoContract]
                public struct EwarDef {
                    public enum EwarType {
                        AntiSmart,
                        JumpNull,
                        EnergySink,
                        Anchor,
                        Emp,
                        Offense,
                        Nav,
                        Dot,
                        Push,
                        Pull,
                        Tractor,
                    }

                    public enum EwarMode {
                        Effect,
                        Field,
                    }

                    [ProtoMember(1)] internal bool Enable;
                    [ProtoMember(2)] internal EwarType Type;
                    [ProtoMember(3)] internal EwarMode Mode;
                    [ProtoMember(4)] internal float Strength;
                    [ProtoMember(5)] internal double Radius;
                    [ProtoMember(6)] internal int Duration;
                    [ProtoMember(7)] internal bool StackDuration;
                    [ProtoMember(8)] internal bool Depletable;
                    [ProtoMember(9)] internal int MaxStacks;
                    [ProtoMember(10)] internal bool NoHitParticle;
                    [ProtoMember(11)] internal PushPullDef Force;
                    [ProtoMember(12)] internal FieldDef Field;


                    [ProtoContract]
                    public struct FieldDef {
                        [ProtoMember(1)] internal int Interval;
                        [ProtoMember(2)] internal int PulseChance;
                        [ProtoMember(3)] internal int GrowTime;
                        [ProtoMember(4)] internal bool HideModel;
                        [ProtoMember(5)] internal bool ShowParticle;
                        [ProtoMember(6)] internal double TriggerRange;
                        [ProtoMember(7)] internal ParticleDef Particle;
                    }

                    [ProtoContract]
                    public struct PushPullDef {
                        public enum Force {
                            ProjectileLastPosition,
                            ProjectileOrigin,
                            HitPosition,
                            TargetCenter,
                            TargetCenterOfMass,
                        }

                        [ProtoMember(1)] internal Force ForceFrom;
                        [ProtoMember(2)] internal Force ForceTo;
                        [ProtoMember(3)] internal Force Position;
                        [ProtoMember(4)] internal bool DisableRelativeMass;
                        [ProtoMember(5)] internal double TractorRange;
                        [ProtoMember(6)] internal bool ShooterFeelsForce;
                    }
                }


                [ProtoContract]
                public struct AreaDamageDef {
                    public enum AreaEffectType {
                        Disabled,
                        Explosive,
                        Radiant,
                        AntiSmart,
                        JumpNullField,
                        EnergySinkField,
                        AnchorField,
                        EmpField,
                        OffenseField,
                        NavField,
                        DotField,
                        PushField,
                        PullField,
                        TractorField,
                    }

                    [ProtoMember(1)] internal double AreaEffectRadius;
                    [ProtoMember(2)] internal float AreaEffectDamage;
                    [ProtoMember(3)] internal AreaEffectType AreaEffect;
                    [ProtoMember(4)] internal PulseDef Pulse;
                    [ProtoMember(5)] internal DetonateDef Detonation;
                    [ProtoMember(6)] internal ExplosionDef Explosions;
                    [ProtoMember(7)] internal EwarFieldsDef EwarFields;
                    [ProtoMember(8)] internal AreaInfluence Base;

                    [ProtoContract]
                    public struct AreaInfluence {
                        [ProtoMember(1)] internal double Radius;
                        [ProtoMember(2)] internal float EffectStrength;
                    }


                    [ProtoContract]
                    public struct PulseDef {
                        [ProtoMember(1)] internal int Interval;
                        [ProtoMember(2)] internal int PulseChance;
                        [ProtoMember(3)] internal int GrowTime;
                        [ProtoMember(4)] internal bool HideModel;
                        [ProtoMember(5)] internal bool ShowParticle;
                        [ProtoMember(6)] internal ParticleDef Particle;
                    }

                    [ProtoContract]
                    public struct EwarFieldsDef {
                        [ProtoMember(1)] internal int Duration;
                        [ProtoMember(2)] internal bool StackDuration;
                        [ProtoMember(3)] internal bool Depletable;
                        [ProtoMember(4)] internal double TriggerRange;
                        [ProtoMember(5)] internal int MaxStacks;
                        [ProtoMember(6)] internal PushPullDef Force;
                        [ProtoMember(7)] internal bool DisableParticleEffect;

                        [ProtoContract]
                        public struct PushPullDef {
                            public enum Force {
                                ProjectileLastPosition,
                                ProjectileOrigin,
                                HitPosition,
                                TargetCenter,
                                TargetCenterOfMass,
                            }

                            [ProtoMember(1)] internal Force ForceFrom;
                            [ProtoMember(2)] internal Force ForceTo;
                            [ProtoMember(3)] internal Force Position;
                            [ProtoMember(4)] internal bool DisableRelativeMass;
                            [ProtoMember(5)] internal double TractorRange;
                            [ProtoMember(6)] internal bool ShooterFeelsForce;
                        }
                    }

                    [ProtoContract]
                    public struct DetonateDef {
                        [ProtoMember(1)] internal bool DetonateOnEnd;
                        [ProtoMember(2)] internal bool ArmOnlyOnHit;
                        [ProtoMember(3)] internal float DetonationRadius;
                        [ProtoMember(4)] internal float DetonationDamage;
                        [ProtoMember(5)] internal int MinArmingTime;
                    }

                    [ProtoContract]
                    public struct ExplosionDef {
                        [ProtoMember(1)] internal bool NoVisuals;
                        [ProtoMember(2)] internal bool NoSound;
                        [ProtoMember(3)] internal float Scale;
                        [ProtoMember(4)] internal string CustomParticle;
                        [ProtoMember(5)] internal string CustomSound;
                        [ProtoMember(6)] internal bool NoShrapnel;
                        [ProtoMember(7)] internal bool NoDeformation;
                    }
                }

                [ProtoContract]
                public struct AmmoAudioDef {
                    [ProtoMember(1)] internal string TravelSound;
                    [ProtoMember(2)] internal string HitSound;
                    [ProtoMember(3)] internal float HitPlayChance;
                    [ProtoMember(4)] internal bool HitPlayShield;
                    [ProtoMember(5)] internal string VoxelHitSound;
                    [ProtoMember(6)] internal string PlayerHitSound;
                    [ProtoMember(7)] internal string FloatingHitSound;
                    [ProtoMember(8)] internal string ShieldHitSound;
                    [ProtoMember(9)] internal string ShotSound;
                }

                [ProtoContract]
                public struct TrajectoryDef {
                    internal enum GuidanceType {
                        None,
                        Remote,
                        TravelTo,
                        Smart,
                        DetectTravelTo,
                        DetectSmart,
                        DetectFixed,
                        DroneAdvanced,
                    }

                    [ProtoMember(1)] internal float MaxTrajectory;
                    [ProtoMember(2)] internal float AccelPerSec;
                    [ProtoMember(3)] internal float DesiredSpeed;
                    [ProtoMember(4)] internal float TargetLossDegree;
                    [ProtoMember(5)] internal int TargetLossTime;
                    [ProtoMember(6)] internal int MaxLifeTime;
                    [ProtoMember(7)] internal int DeaccelTime;
                    [ProtoMember(8)] internal Randomize SpeedVariance;
                    [ProtoMember(9)] internal Randomize RangeVariance;
                    [ProtoMember(10)] internal GuidanceType Guidance;
                    [ProtoMember(11)] internal SmartsDef Smarts;
                    [ProtoMember(12)] internal MinesDef Mines;
                    [ProtoMember(13)] internal float GravityMultiplier;
                    [ProtoMember(14)] internal uint MaxTrajectoryTime;
                    [ProtoMember(15)] internal ApproachDef[] Approaches;
                    [ProtoMember(16)] internal double TotalAcceleration;

                    [ProtoContract]
                    public struct SmartsDef {
                        [ProtoMember(1)] internal double Inaccuracy;
                        [ProtoMember(2)] internal double Aggressiveness;
                        [ProtoMember(3)] internal double MaxLateralThrust;
                        [ProtoMember(4)] internal double TrackingDelay;
                        [ProtoMember(5)] internal int MaxChaseTime;
                        [ProtoMember(6)] internal bool OverideTarget;
                        [ProtoMember(7)] internal int MaxTargets;
                        [ProtoMember(8)] internal bool NoTargetExpire;
                        [ProtoMember(9)] internal bool Roam;
                        [ProtoMember(10)] internal bool KeepAliveAfterTargetLoss;
                        [ProtoMember(11)] internal float OffsetRatio;
                        [ProtoMember(12)] internal int OffsetTime;
                        [ProtoMember(13)] internal bool CheckFutureIntersection;
                        [ProtoMember(14)] internal double NavAcceleration;
                        [ProtoMember(15)] internal bool AccelClearance;
                        [ProtoMember(16)] internal double SteeringLimit;
                        [ProtoMember(17)] internal bool FocusOnly;
                        [ProtoMember(18)] internal double OffsetMinRange;
                        [ProtoMember(19)] internal bool FocusEviction;
                        [ProtoMember(20)] internal double ScanRange;
                        [ProtoMember(21)] internal bool NoSteering;
                        [ProtoMember(22)] internal double FutureIntersectionRange;
                        [ProtoMember(23)] internal double MinTurnSpeed;
                        [ProtoMember(24)] internal bool NoTargetApproach;
                        [ProtoMember(25)] internal bool AltNavigation;
                    }

                    [ProtoContract]
                    public struct ApproachDef {
                        public enum ReInitCondition {
                            Wait,
                            MoveToPrevious,
                            MoveToNext,
                            ForceRestart,
                        }

                        public enum Conditions {
                            Ignore,
                            Spawn,
                            DistanceFromPositionC,
                            Lifetime,
                            DesiredElevation,
                            MinTravelRequired,
                            MaxTravelRequired,
                            Deadtime,
                            DistanceToPositionC,
                            NextTimedSpawn,
                            RelativeLifetime,
                            RelativeDeadtime,
                            SinceTimedSpawn,
                            RelativeSpawns,
                            EnemyTargetLoss,
                            RelativeHealthLost,
                            HealthRemaining,
                            DistanceFromPositionB,
                            DistanceToPositionB,
                            DistanceFromTarget,
                            DistanceToTarget,
                            DistanceFromEndTrajectory,
                            DistanceToEndTrajectory,
                        }

                        public enum UpRelativeTo {
                            UpRelativeToBlock,
                            UpRelativeToGravity,
                            UpTargetDirection,
                            UpTargetVelocity,
                            UpStoredStartDontUse,
                            UpStoredEndDontUse,
                            UpStoredStartPosition,
                            UpStoredEndPosition,
                            UpStoredStartLocalPosition,
                            UpStoredEndLocalPosition,
                            UpRelativeToShooter,
                            UpOriginDirection,
                            UpElevationDirection,
                        }

                        public enum FwdRelativeTo {
                            ForwardElevationDirection,
                            ForwardRelativeToBlock,
                            ForwardRelativeToGravity,
                            ForwardTargetDirection,
                            ForwardTargetVelocity,
                            ForwardStoredStartDontUse,
                            ForwardStoredEndDontUse,
                            ForwardStoredStartPosition,
                            ForwardStoredEndPosition,
                            ForwardStoredStartLocalPosition,
                            ForwardStoredEndLocalPosition,
                            ForwardRelativeToShooter,
                            ForwardOriginDirection,
                        }

                        public enum RelativeTo {
                            Origin,
                            Shooter,
                            Target,
                            Surface,
                            MidPoint,
                            PositionA,
                            Nothing,
                            StoredStartDontUse,
                            StoredEndDontUse,
                            StoredStartPosition,
                            StoredEndPosition,
                            StoredStartLocalPosition,
                            StoredEndLocalPosition,
                        }

                        public enum ConditionOperators {
                            StartEnd_And,
                            StartEnd_Or,
                            StartAnd_EndOr,
                            StartOr_EndAnd,
                        }

                        public enum StageEvents {
                            DoNothing,
                            EndProjectile,
                            EndProjectileOnRestart,
                            StoreDontUse,
                            StorePositionDontUse,
                            Refund,
                            StorePositionA,
                            StorePositionB,
                            StorePositionC,
                        }

                        [ProtoContract]
                        public struct WeightedIdListDef {

                            [ProtoMember(1)] public int ApproachId;
                            [ProtoMember(2)] public Randomize Weight;
                            [ProtoMember(3)] public double End1WeightMod;
                            [ProtoMember(4)] public double End2WeightMod;
                            [ProtoMember(5)] public int MaxRuns;
                            [ProtoMember(6)] public double End3WeightMod;
                        }

                        [ProtoMember(1)] internal ReInitCondition RestartCondition;
                        [ProtoMember(2)] internal Conditions StartCondition1;
                        [ProtoMember(3)] internal Conditions EndCondition1;
                        [ProtoMember(4)] internal UpRelativeTo Up;
                        [ProtoMember(5)] internal RelativeTo PositionB;
                        [ProtoMember(6)] internal double AngleOffset;
                        [ProtoMember(7)] internal double Start1Value;
                        [ProtoMember(8)] internal double End1Value;
                        [ProtoMember(9)] internal double LeadDistance;
                        [ProtoMember(10)] internal double DesiredElevation;
                        [ProtoMember(11)] internal double AccelMulti;
                        [ProtoMember(12)] internal double SpeedCapMulti;
                        [ProtoMember(13)] internal bool AdjustPositionC;
                        [ProtoMember(14)] internal bool CanExpireOnceStarted;
                        [ProtoMember(15)] internal ParticleDef AlternateParticle;
                        [ProtoMember(16)] internal string AlternateSound;
                        [ProtoMember(17)] internal string AlternateModel;
                        [ProtoMember(18)] internal int OnRestartRevertTo;
                        [ProtoMember(19)] internal ParticleDef StartParticle;
                        [ProtoMember(20)] internal bool AdjustPositionB;
                        [ProtoMember(21)] internal bool AdjustUp;
                        [ProtoMember(22)] internal bool PushLeadByTravelDistance;
                        [ProtoMember(23)] internal double TrackingDistance;
                        [ProtoMember(24)] internal Conditions StartCondition2;
                        [ProtoMember(25)] internal double Start2Value;
                        [ProtoMember(26)] internal Conditions EndCondition2;
                        [ProtoMember(27)] internal double End2Value;
                        [ProtoMember(28)] internal RelativeTo Elevation;
                        [ProtoMember(29)] internal double ElevationTolerance;
                        [ProtoMember(30)] internal ConditionOperators Operators;
                        [ProtoMember(31)] internal StageEvents StartEvent;
                        [ProtoMember(32)] internal StageEvents EndEvent;
                        [ProtoMember(33)] internal double TotalAccelMulti;
                        [ProtoMember(34)] internal double DeAccelMulti;
                        [ProtoMember(35)] internal bool Orbit;
                        [ProtoMember(36)] internal double OrbitRadius;
                        [ProtoMember(37)] internal int OffsetTime;
                        [ProtoMember(38)] internal double OffsetMinRadius;
                        [ProtoMember(39)] internal bool NoTimedSpawns;
                        [ProtoMember(40)] internal double OffsetMaxRadius;
                        [ProtoMember(41)] internal bool ForceRestart;
                        [ProtoMember(42)] internal RelativeTo PositionC;
                        [ProtoMember(43)] internal bool DisableAvoidance;
                        [ProtoMember(44)] internal int StoredStartId;
                        [ProtoMember(45)] internal int StoredEndId;
                        [ProtoMember(46)] internal WeightedIdListDef[] RestartList;
                        [ProtoMember(47)] internal RelativeTo StoredStartType;
                        [ProtoMember(48)] internal RelativeTo StoredEndType;
                        [ProtoMember(49)] internal bool LeadRotateElevatePositionB;
                        [ProtoMember(50)] internal bool LeadRotateElevatePositionC;
                        [ProtoMember(51)] internal bool NoElevationLead;
                        [ProtoMember(52)] internal bool IgnoreAntiSmart;
                        [ProtoMember(53)] internal double HeatRefund;
                        [ProtoMember(54)] internal Randomize AngleVariance;
                        [ProtoMember(55)] internal bool ReloadRefund;
                        [ProtoMember(56)] internal int ModelRotateTime;
                        [ProtoMember(57)] internal FwdRelativeTo Forward;
                        [ProtoMember(58)] internal bool AdjustForward;
                        [ProtoMember(59)] internal bool ToggleIngoreVoxels;
                        [ProtoMember(60)] internal bool SelfAvoidance;
                        [ProtoMember(61)] internal bool TargetAvoidance;
                        [ProtoMember(62)] internal bool SelfPhasing;
                        [ProtoMember(63)] internal bool TrajectoryRelativeToB;
                        [ProtoMember(64)] internal Conditions EndCondition3;
                        [ProtoMember(65)] internal double End3Value;
                        [ProtoMember(66)] internal bool SwapNavigationType;
                        [ProtoMember(67)] internal bool ElevationRelativeToC;
                    }

                    [ProtoContract]
                    public struct MinesDef {
                        [ProtoMember(1)] internal double DetectRadius;
                        [ProtoMember(2)] internal double DeCloakRadius;
                        [ProtoMember(3)] internal int FieldTime;
                        [ProtoMember(4)] internal bool Cloak;
                        [ProtoMember(5)] internal bool Persist;
                    }
                }

                [ProtoContract]
                public struct Randomize {
                    [ProtoMember(1)] internal float Start;
                    [ProtoMember(2)] internal float End;
                }
            }

            [ProtoContract]
            public struct ParticleOptionDef {
                [ProtoMember(1)] internal float Scale;
                [ProtoMember(2)] internal float MaxDistance;
                [ProtoMember(3)] internal float MaxDuration;
                [ProtoMember(4)] internal bool Loop;
                [ProtoMember(5)] internal bool Restart;
                [ProtoMember(6)] internal float HitPlayChance;
            }


            [ProtoContract]
            public struct ParticleDef {
                [ProtoMember(1)] internal string Name;
                [ProtoMember(2)] internal Vector4 Color;
                [ProtoMember(3)] internal Vector3D Offset;
                [ProtoMember(4)] internal ParticleOptionDef Extras;
                [ProtoMember(5)] internal bool ApplyToShield;
                [ProtoMember(6)] internal bool DisableCameraCulling;
            }
        }
    }

}