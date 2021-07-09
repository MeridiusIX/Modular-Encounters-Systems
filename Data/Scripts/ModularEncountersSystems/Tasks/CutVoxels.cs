using ModularEncountersSystems.Entities;
using ModularEncountersSystems.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Tasks {
    public class CutVoxels : TaskItem, ITaskItem {

        internal GridEntity _grid;
        internal double _cutSize;

        internal double _cutSizeMin;
        internal double _cutSizeMax;

        internal List<MatrixD> _airtightCellLocations;
        internal List<IMyVoxelBase> _mapList;
        internal int _listIndex;
        internal int _runCount;

        public CutVoxels(GridEntity grid, double cutSize = 2.7) {

            _isValid = true;
            _tickTrigger = 1;

            _grid = grid;
            _airtightCellLocations = new List<MatrixD>();
            _mapList = new List<IMyVoxelBase>();

            var min = _grid.CubeGrid.Min;
            var max = _grid.CubeGrid.Max;

            for (int x = min.X; x <= max.X; x++) {

                for (int y = min.Y; y <= max.Y; y++) {

                    for (int z = min.Z; z <= max.Z; z++) {

                        var checkCell = new Vector3I(x, y, z);

                        if (_grid.CubeGrid.IsRoomAtPositionAirtight(checkCell) == true) {

                            var cellWorldSpace = _grid.CubeGrid.GridIntegerToWorld(checkCell);
                            _airtightCellLocations.Add(MatrixD.CreateWorld(cellWorldSpace, _grid.CubeGrid.WorldMatrix.Forward, _grid.CubeGrid.WorldMatrix.Up));

                        }

                    }

                }

            }

            _listIndex = _airtightCellLocations.Count;

            if (_listIndex == 0)
                _isValid = false;

        }

        public override void Run() {

            try {


                _listIndex--;

                if (_listIndex < 0 || _listIndex >= _airtightCellLocations.Count) {

                    _listIndex = _airtightCellLocations.Count - 1;
                    _runCount++;

                    if (_runCount >= 2 || _listIndex < 0) {

                        _isValid = false;
                        return;

                    }

                }

                var cutMatrix = _airtightCellLocations[_listIndex];
                var sphere = new BoundingSphereD(cutMatrix.Translation, 1000);
                _mapList.Clear();
                MyAPIGateway.Session.VoxelMaps.GetInstances(_mapList);

                for (int i = _mapList.Count - 1; i >= 0; i--) {

                    if (_mapList[i].PositionComp.WorldAABB.Intersects(sphere) == false) {

                        _mapList.RemoveAt(i);

                    }

                }

                var voxelTool = MyAPIGateway.Session.VoxelMaps.GetBoxVoxelHand();
                voxelTool.Boundaries = new BoundingBoxD(new Vector3D(_cutSizeMin, _cutSizeMin, _cutSizeMin), new Vector3D(_cutSizeMax, _cutSizeMax, _cutSizeMax));
                voxelTool.Transform = cutMatrix;

                foreach (var voxel in _mapList) {

                    MyAPIGateway.Session.VoxelMaps.CutOutShape(voxel, voxelTool);

                }


            } catch (Exception e) {

                //TODO Logger: Voxel Cut Operation Failed
                _isValid = false;

            }

        }

    }

}
