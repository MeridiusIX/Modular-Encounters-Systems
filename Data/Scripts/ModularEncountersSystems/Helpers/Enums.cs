using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Helpers {

    public enum AutoPilotMode {

        None, //No Movement Applied
        BarrelRoll,
        LegacyAutoPilotTarget, //Uses Vanilla Remote Control Autopilot.
        LegacyAutoPilotWaypoint, //Uses Vanilla Remote Control Autopilot.
        FlyToWaypoint,
        FlyToTarget,
        RotateToTarget, //Applies Gyro Rotation To Face Target. No Thrust.
        RotateToWaypoint, //Applies Gyro Rotation To Face Waypoint. No Thrust.
        RotateToTargetAndStrafe, //Applies Gyro Rotation To Face Target. Random Thruster Strafing Included.
        Strafe

    }

    public enum BehaviorMode {

        Init,
        ApproachTarget,
        ApproachWaypoint,
        BarrelRoll,
        CustomModeA,
        CustomModeB,
        CustomModeC,
        EngageTarget,
        EvadeCollision,
        Idle,
        KamikazeCollision,
        Retreat,
        WaitAtWaypoint,
        WaitingForTarget,

    }

    public enum BehaviorSubclass {
    
        None,
        CoreBehavior,
        Fighter,
        Horsefly,
        HorseFighter,
        Strike,
        Passive,
        Hunter,
        Scout,
        Sniper,
        Nautical,
        CargoShip,
        Escort,
        Patrol,
        Tunneller,
        Vulture,

    }

    [Flags]
    public enum BlockTargetTypes {

        None = 0,
        All = 1,
        Containers = 2,
        Decoys = 4,
        GravityBlocks = 8,
        Guns = 16,
        JumpDrive = 32,
        Power = 64,
        Production = 128,
        Propulsion = 256,
        Shields = 512,
        ShipControllers = 1024,
        Tools = 2048,
        Turrets = 4096,
        Communications = 8192

    }

    [Flags]
    public enum BroadcastType {

        None = 0,
        Chat = 1,
        Notify = 2,
        Both = 4

    }

    public enum ChatType {

        None,
        Greeting,
        Taunt,
        Retreat,
        Damage,
        Grind,
        TurretTarget,
        Spawning,
        SafeZone,

    }

    public enum CollisionDetectType {

        None,
        Voxel,
        Grid,
        SafeZone,
        DefenseShield

    }

    public enum Axis {

        X,
        Y,
        Z

    }

    public enum Direction {
        
        None,
        Forward,
        Backward,
        Up,
        Down,
        Left,
        Right
    
    }

    public enum FakeExplosionFlags {
        CREATE_DEBRIS = 1,
        AFFECT_VOXELS = 2,
        APPLY_FORCE_AND_DAMAGE = 4,
        CREATE_DECALS = 8,
        FORCE_DEBRIS = 0x10,
        CREATE_PARTICLE_EFFECT = 0x20,
        CREATE_SHRAPNELS = 0x40,
        APPLY_DEFORMATION = 0x80,
        CREATE_PARTICLE_DEBRIS = 0x100,
        FORCE_CUSTOM_END_OF_LIFE_EFFECT = 0x200
    }

    public enum TargetTypeEnum {

        None,
        Coords,
        Player,
        Entity,
        Grid,
        Block,
        Override,
        PlayerAndGrid,
        PlayerAndBlock,

    }

    public enum BlockSizeEnum {
    
        None,
        Small,
        Large
    
    }

    public enum BoolEnum {
    
        None,
        False,
        True,
    
    }

    public enum CheckEnum {

        Ignore,
        No,
        Yes,        

    }

    public enum CounterCompareEnum {

        GreaterOrEqual,
        Greater,
        Equal,
        NotEqual,
        Less,
        LessOrEqual,

    }

    public enum ModifierEnum {
    
        None,
        Set,
        Add,
        Subtract,
        Multiply,
        Divide
    
    }

    public enum RotationEnum {
    
        Pitch,
        Yaw,
        Roll,
    
    }

    public enum SwitchEnum {
    
        Off,
        On,
        Toggle,
    
    }

    public enum SpawnTypeEnum {
    
        CustomSpawn,
        SpaceCargoShip,
        RandomEncounter,
        PlanetaryCargoShip,
        PlanetaryInstallation,
        BossEncounter,
        Creature
    
    }

    public enum TargetSortEnum {

        Random,
        ClosestDistance,
        FurthestDistance,
        HighestTargetValue,
        LowestTargetValue,

    }

    public enum TargetFilterEnum {

        None,
        AirDensity,
        Altitude,
        Broadcasting,
        Faction,
        Gravity,
        LineOfSight,
        MovementScore,
        Name,
        OutsideOfSafezone,
        Owner,
        PlayerControlled,
        PlayerKnownLocation,
        Powered,
        Relation,
        Shielded,
        Speed,
        Static,
        TargetValue,
        Underwater,
        GravityThrust,
        IgnoreStealthDrive,

    }

    [Flags]
    public enum TargetObstructionEnum {

        None = 0,
        Voxel = 1 << 0,
        Safezone = 1 << 1,
        DefenseShield = 1 << 2,
        OtherGrid = 1 << 3,

    }

    [Flags]
    public enum TargetRelationEnum {

        None = 0,
        Faction = 1,
        Neutral = 2,
        Enemy = 4,
        Friend = 8,
        Unowned = 16

    }

    [Flags]
    public enum TargetOwnerEnum {

        None = 0,
        Unowned = 1,
        Owned = 2,
        Player = 4,
        NPC = 8,
        All = 16

    }

    public enum Tolerence {
    
        None,
        Lower,
        Within,
        Higher
    
    }

    [Flags]
    public enum TriggerAction {

        None = 0,
        BarrelRoll = 1 << 0,
        ChatBroadcast = 1 << 1,
        Retreat = 1 << 2,
        SelfDestruct = 1 << 3,
        SpawnReinforcements = 1 << 4,
        Strafe = 1 << 5,
        SwitchToTarget = 1 << 6,
        SwitchToBehavior = 1 << 7,
        TriggerTimerBlock = 1 << 8,
        FindNewTarget = 1 << 9,
        ActivateAssertiveAntennas = 1 << 10,

    }

    [Flags]
    public enum TriggerType {

        None = 0,
        Damage = 1 << 0,
        PlayerNear = 1 << 1, 
        TurretTarget = 1 << 2,
        NoWeapon = 1 << 3,
        TargetInSafezone = 1 << 4,
        Grounded = 1 << 5,
        CommandReceive = 1 << 6,
        Timer = 1 << 7,

    }

    [Flags]
    public enum WaypointModificationEnum {
    
        None = 0,
        Collision = 1 << 0,
        Offset = 1 << 1,
        PlanetPathing = 1 << 2,
        CollisionLeading = 1 << 3,
        WeaponLeading = 1 << 4,
        TargetIsInitialWaypoint = 1 << 5,
        PlanetPathingAscend = 1 << 6,
        TargetPadding = 1 << 7,
        WaterPathing = 1 << 8,
        EscortPathing = 1 << 9,
        CircleTarget = 1 << 10,
        RoverPathing = 1 << 11


    }

    public enum WaypointOffsetType {
    
        None,
        DistanceFromTarget,
        RandomOffsetFixed,
        RandomOffsetRelativeEntity
    
    }

}
