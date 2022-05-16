using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Behavior.Subsystems.AutoPilot {

    public class ThrusterProfile {

        public IMyThrust Block;
        public IMyCubeGrid Grid;
        private Base6Directions.Direction _realDirection;
        private Base6Directions.Direction _activeDirection;

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

            if (thrust.SlimBlock.CubeGrid != remoteControl.SlimBlock.CubeGrid) {

                if (useSubGrids) {

                    if (VectorHelper.GetAngleBetweenDirections(remoteControl.WorldMatrix.Backward, thrust.WorldMatrix.Forward) <= maxSubgridAngle) {

                        _realDirection = Base6Directions.Direction.Forward;

                    }

                    if (VectorHelper.GetAngleBetweenDirections(remoteControl.WorldMatrix.Forward, thrust.WorldMatrix.Forward) <= maxSubgridAngle) {

                        _realDirection = Base6Directions.Direction.Backward;

                    }

                    if (VectorHelper.GetAngleBetweenDirections(remoteControl.WorldMatrix.Down, thrust.WorldMatrix.Forward) <= maxSubgridAngle) {

                        _realDirection = Base6Directions.Direction.Up;

                    }

                    if (VectorHelper.GetAngleBetweenDirections(remoteControl.WorldMatrix.Up, thrust.WorldMatrix.Forward) <= maxSubgridAngle) {

                        _realDirection = Base6Directions.Direction.Down;

                    }

                    if (VectorHelper.GetAngleBetweenDirections(remoteControl.WorldMatrix.Left, thrust.WorldMatrix.Forward) <= maxSubgridAngle) {

                        _realDirection = Base6Directions.Direction.Right;

                    }

                    if (VectorHelper.GetAngleBetweenDirections(remoteControl.WorldMatrix.Right, thrust.WorldMatrix.Forward) <= maxSubgridAngle) {

                        _realDirection = Base6Directions.Direction.Left;

                    }

                } else {

                    _valid = false;

                }

            } else {

                if (thrust.WorldMatrix.Forward == remoteControl.WorldMatrix.Backward) {

                    _realDirection = Base6Directions.Direction.Forward;

                }

                if (thrust.WorldMatrix.Forward == remoteControl.WorldMatrix.Forward) {

                    _realDirection = Base6Directions.Direction.Backward;

                }

                if (thrust.WorldMatrix.Forward == remoteControl.WorldMatrix.Down) {

                    _realDirection = Base6Directions.Direction.Up;

                }

                if (thrust.WorldMatrix.Forward == remoteControl.WorldMatrix.Up) {

                    _realDirection = Base6Directions.Direction.Down;

                }

                if (thrust.WorldMatrix.Forward == remoteControl.WorldMatrix.Right) {

                    _realDirection = Base6Directions.Direction.Left;

                }

                if (thrust.WorldMatrix.Forward == remoteControl.WorldMatrix.Left) {

                    _realDirection = Base6Directions.Direction.Right;

                }

            }

            _activeDirection = _realDirection;

            Grid = remoteControl.SlimBlock.CubeGrid;
            Block.OnClosing += CloseEntity;
            Block.IsWorkingChanged += WorkingChange;
            WorkingChange(Block);

        }

        //New
        public void SetBaseDirection(MyBlockOrientation orientation) {

            _activeDirection = orientation.TransformDirection(_realDirection);

        }

        public double GetEffectiveThrust(Base6Directions.Direction direction) {

            if (direction != _activeDirection || !_working || !ValidCheck())
                return 0;

            return Block.MaxEffectiveThrust;
        
        }

        //New
        public void ApplyThrust(ThrustAction action) {

            if (!_working)
                return;

            if (_activeDirection == Base6Directions.Direction.Left || _activeDirection == Base6Directions.Direction.Right) {

                bool direction = (_activeDirection == Base6Directions.Direction.Left && action.InvertX) || (_activeDirection == Base6Directions.Direction.Right && !action.InvertX);
                UpdateThrusterBlock(action.ControlX, direction, action.StrengthX);
                return;
            
            }

            if (_activeDirection == Base6Directions.Direction.Up || _activeDirection == Base6Directions.Direction.Down) {

                bool direction = (_activeDirection == Base6Directions.Direction.Down && action.InvertY) || (_activeDirection == Base6Directions.Direction.Up && !action.InvertY);
                UpdateThrusterBlock(action.ControlY, direction, action.StrengthY);
                return;

            }

            if (_activeDirection == Base6Directions.Direction.Forward || _activeDirection == Base6Directions.Direction.Backward) {

                bool direction = (_activeDirection == Base6Directions.Direction.Backward && action.InvertZ) || (_activeDirection == Base6Directions.Direction.Forward && !action.InvertZ);
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

        private void Unload() {

            if (Block == null)
                return;

            Block.OnClosing -= CloseEntity;
            Block.IsWorkingChanged -= WorkingChange;

        }

    }

}