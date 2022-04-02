using ModularEncountersSystems.Behavior;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Watchers;
using ModularEncountersSystems.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Tasks {
    public class PrefabDiagnosticsTask : TaskItem, ITaskItem {

        public PrefabDiagnosticsTask(IMyRemoteControl remote) {

            _isValid = true;
            _tickTrigger = 15;
            

        }

        public override void Run() {



        }

    }

    public struct PrefabDiagnostics {

        public int TotalGrids;
        public int AttachedGrids;

        public bool HasMainCockpit;
        public bool HasRemoteControl;

        public int ThrustCount;
        public bool ThrustInAllDirections;
        public bool ThrustDamageSameGrid;
        public bool ThrustDamageAttachedGrid;
        public bool ThrustDamageOtherGrid;
        public bool AtmoCapable;
        public float MaxGravity;

        public int GyroCount;

        public int GravityGenerators;

        public int Reactors;
        public int Batteries;
        public int Generators;
        public int Renewables;

        public int Wheels;

        public int LightArmor;
        public int HeavyArmor;

        public int ProgrammableBlocksWithScripts;
        public int Sensors;
        public int Timers;
        public int Lcds;

        public int ContainersWithCargo;

        public int Antennas;
        public int Beacons;
        public bool ActiveSignals;

        public PrefabDiagnostics(bool dummy = false) {

            TotalGrids = 0;
            AttachedGrids = 0;

            HasMainCockpit = false;
            HasRemoteControl = false;

            ThrustCount = 0;
            ThrustInAllDirections = false;
            ThrustDamageSameGrid = false;
            ThrustDamageAttachedGrid = false;
            ThrustDamageOtherGrid = false;
            AtmoCapable = false;
            MaxGravity = 0f;

            GyroCount = 0;

            Wheels = 0;

            GravityGenerators = 0;

            Reactors = 0;
            Batteries = 0;
            Generators = 0;
            Renewables = 0;

            LightArmor = 0;
            HeavyArmor = 0;

            ProgrammableBlocksWithScripts = 0;
            Sensors = 0;
            Lcds = 0;
            Timers = 0;

            ContainersWithCargo = 0;

            Antennas = 0;
            Beacons = 0;
            ActiveSignals = false;

        }

    }

}
