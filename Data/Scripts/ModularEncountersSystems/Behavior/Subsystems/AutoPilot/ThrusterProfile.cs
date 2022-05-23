using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Behavior.Subsystems.AutoPilot {

    public class ThrusterProfile {

        public IMyThrust Block;
        public IMyCubeGrid Grid;
        public Base6Directions.Direction RealDirection;
        public Base6Directions.Direction ActiveDirection;

        public double MaxThrustForceInAtmo;
        public double MaxThrustForceInGravity;

        public bool AxisEnabled;
        public bool DirectionEnabled;
        public float CurrentOverride;

        public IBehavior Behavior;
        //private bool _gridSplitCheck;
        private bool _valid;
        private bool _working;

        public ThrusterProfile(IMyThrust thrust, IMyRemoteControl remoteControl, IBehavior behavior, bool useSubGrids = false, double maxSubgridAngle = 35) {

            _valid = true;
            Block = thrust;

            AxisEnabled = false;

            CurrentOverride = 0;
            DirectionEnabled = false;
            Behavior = behavior;

            MaxThrustForceInAtmo = thrustForce(thrust.SlimBlock.BlockDefinition as MyThrustDefinition, 0.7f);
            MaxThrustForceInGravity = thrustForce(thrust.SlimBlock.BlockDefinition as MyThrustDefinition, 0);

            if (thrust.SlimBlock.CubeGrid != remoteControl.SlimBlock.CubeGrid) {

                if (useSubGrids) {

                    if (VectorHelper.GetAngleBetweenDirections(remoteControl.WorldMatrix.Backward, thrust.WorldMatrix.Forward) <= maxSubgridAngle) {

                        RealDirection = Base6Directions.Direction.Forward;

                    }

                    if (VectorHelper.GetAngleBetweenDirections(remoteControl.WorldMatrix.Forward, thrust.WorldMatrix.Forward) <= maxSubgridAngle) {

                        RealDirection = Base6Directions.Direction.Backward;

                    }

                    if (VectorHelper.GetAngleBetweenDirections(remoteControl.WorldMatrix.Down, thrust.WorldMatrix.Forward) <= maxSubgridAngle) {

                        RealDirection = Base6Directions.Direction.Up;

                    }

                    if (VectorHelper.GetAngleBetweenDirections(remoteControl.WorldMatrix.Up, thrust.WorldMatrix.Forward) <= maxSubgridAngle) {

                        RealDirection = Base6Directions.Direction.Down;

                    }

                    if (VectorHelper.GetAngleBetweenDirections(remoteControl.WorldMatrix.Left, thrust.WorldMatrix.Forward) <= maxSubgridAngle) {

                        RealDirection = Base6Directions.Direction.Right;

                    }

                    if (VectorHelper.GetAngleBetweenDirections(remoteControl.WorldMatrix.Right, thrust.WorldMatrix.Forward) <= maxSubgridAngle) {

                        RealDirection = Base6Directions.Direction.Left;

                    }

                } else {

                    _valid = false;

                }

            } else {

                if (thrust.WorldMatrix.Forward == remoteControl.WorldMatrix.Backward) {

                    RealDirection = Base6Directions.Direction.Forward;

                }

                if (thrust.WorldMatrix.Forward == remoteControl.WorldMatrix.Forward) {

                    RealDirection = Base6Directions.Direction.Backward;

                }

                if (thrust.WorldMatrix.Forward == remoteControl.WorldMatrix.Down) {

                    RealDirection = Base6Directions.Direction.Up;

                }

                if (thrust.WorldMatrix.Forward == remoteControl.WorldMatrix.Up) {

                    RealDirection = Base6Directions.Direction.Down;

                }

                if (thrust.WorldMatrix.Forward == remoteControl.WorldMatrix.Right) {

                    RealDirection = Base6Directions.Direction.Left;

                }

                if (thrust.WorldMatrix.Forward == remoteControl.WorldMatrix.Left) {

                    RealDirection = Base6Directions.Direction.Right;

                }

            }

            ActiveDirection = RealDirection;

            Grid = remoteControl.SlimBlock.CubeGrid;
            Block.OnClosing += CloseEntity;
            Block.IsWorkingChanged += WorkingChange;
            WorkingChange(Block);

        }

        //New
        public void SetBaseDirection(MyBlockOrientation orientation) {

            ActiveDirection = orientation.TransformDirection(RealDirection);

        }

        public double GetEffectiveThrust(Base6Directions.Direction direction) {

            if (direction != ActiveDirection || !_working || !ValidCheck())
                return 0;

            return Block.MaxEffectiveThrust;
        
        }

        //New
        public void ApplyThrust(ThrustAction action) {

            if (!_working)
                return;

            if (ActiveDirection == Base6Directions.Direction.Left || ActiveDirection == Base6Directions.Direction.Right) {

                bool direction = (ActiveDirection == Base6Directions.Direction.Left && action.InvertX) || (ActiveDirection == Base6Directions.Direction.Right && !action.InvertX);
                UpdateThrusterBlock(action.ControlX, direction, action.StrengthX);
                return;
            
            }

            if (ActiveDirection == Base6Directions.Direction.Up || ActiveDirection == Base6Directions.Direction.Down) {

                bool direction = (ActiveDirection == Base6Directions.Direction.Down && action.InvertY) || (ActiveDirection == Base6Directions.Direction.Up && !action.InvertY);
                UpdateThrusterBlock(action.ControlY, direction, action.StrengthY);
                return;

            }

            if (ActiveDirection == Base6Directions.Direction.Forward || ActiveDirection == Base6Directions.Direction.Backward) {

                bool direction = (ActiveDirection == Base6Directions.Direction.Backward && action.InvertZ) || (ActiveDirection == Base6Directions.Direction.Forward && !action.InvertZ);
                UpdateThrusterBlock(action.ControlZ, direction, action.StrengthZ);
                return;

            }

        }
        
        //New
        public void UpdateThrusterBlock(bool axisEnabled, bool directionEnabled, float overrideAmount) {

            if (!ValidCheck())
                return;

            if (AxisEnabled == axisEnabled && DirectionEnabled == directionEnabled && CurrentOverride == overrideAmount)
                return;

            if (!_working) {

                return;
            
            }

            AxisEnabled = axisEnabled;
            DirectionEnabled = directionEnabled;
            CurrentOverride = overrideAmount;

            if (AxisEnabled) {

                if (DirectionEnabled) {

                    Block.ThrustOverridePercentage = CurrentOverride;

                } else {

                    Block.ThrustOverridePercentage = 0.0001f;

                }

            } else {

                Block.ThrustOverridePercentage = 0;

            }

        }

        public bool ValidCheck() {

            if (!_valid)
                return false;

            if (Block == null || Block.MarkedForClose) {

                BehaviorLogger.Write("Removed Thrust - Block Null or Closed", BehaviorDebugEnum.Thrust);
                _valid = false;
                return false;

            }

            if (Grid == null || Grid.MarkedForClose || Grid != Block.SlimBlock.CubeGrid) {

                _valid = false;
                return false;

            }

            return true;
        
        }

        private void WorkingChange(IMyCubeBlock cubeBlock) {

            _working = Block.IsWorking && Block.IsFunctional;

        }

        private void CloseEntity(IMyEntity entity) {

            BehaviorLogger.Write("Removed Thrust - Block Closed", BehaviorDebugEnum.Thrust);
            _valid = false;
            Unload();

        }

        internal double thrustForce(MyThrustDefinition def, float airDensity = 0.7f) {

            double minPlanetInfluence = def.MinPlanetaryInfluence;
            double maxPlanetInfluence = def.MaxPlanetaryInfluence;
            double effectiveAtMin = def.EffectivenessAtMinInfluence;
            double effectiveAtMax = def.EffectivenessAtMaxInfluence;

            var InvDiffMinMaxPlanetaryInfluence = 1f / (maxPlanetInfluence - minPlanetInfluence);

            double value = (airDensity - minPlanetInfluence) * InvDiffMinMaxPlanetaryInfluence;
            var result = MathHelper.Lerp(effectiveAtMin, effectiveAtMax, MathHelper.Clamp(value, 0f, 1f));
            return result;

        }

        private void Unload() {

            if (Block == null)
                return;

            Block.OnClosing -= CloseEntity;
            Block.IsWorkingChanged -= WorkingChange;

        }

    }

}