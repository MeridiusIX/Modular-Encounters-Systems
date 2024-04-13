using ModularEncountersSystems.Core;
using ModularEncountersSystems.Helpers;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Utils;
using VRageMath;

/*

Modified For Compatibility With Space Engineers ModAPI

Original Script At Link Below:

https://github.com/SebLague/Pathfinding-2D

Original Author License Notice Below:

MIT License

Copyright (c) 2017 Sebastian Lague

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 
*/

namespace ModularEncountersSystems.Behavior.Subsystems.AutoPilot {
	public class RoverPathing {

		public bool EnablePathGeneration;

		private IBehavior _behavior;

		private PathNode _closestNodeToStart;
		private PathNode _closestNodeToEnd;

		private Vector3D _gridStartCoords;
		private Vector3D _gridEndCoords;
		private Vector3D _targetCoords;

		private Vector3D _currentWaterPathWaypoint;

		private List<PathNode> _goodNodes;
		private List<PathNode> _badNodes;
		private List<PathNode> _shorelineNodes;
		private List<Vector3D> _pathWaypoints;
		private bool _endPath;

		private int _worldStepDistance;
		private int _steps;
		private int _minimumAltitudeDifference;

		private DateTime _lastPathRecalculation;
		private bool _firstRun;
		private int _refreshTime;

		public RoverPathing(IBehavior behavior) {

			EnablePathGeneration = true;

			_behavior = behavior;

			_goodNodes = new List<PathNode>();
			_badNodes = new List<PathNode>();
			_pathWaypoints = new List<Vector3D>();
			_shorelineNodes = new List<PathNode>();
			_endPath = false;

			_worldStepDistance = 100;
			_steps = 40;
			_minimumAltitudeDifference = 50;

			_lastPathRecalculation = MyAPIGateway.Session.GameDateTime;
			_firstRun = false;
			_refreshTime = 15;

		}

		public Vector3D GetPathCoords(Vector3D currentPos, double waypointTolerance) {

			var time = MyAPIGateway.Session.GameDateTime - _lastPathRecalculation;

			if (time.TotalSeconds >= _refreshTime || !_firstRun) {

				//Logger.Write("Getting Pathing For Water Navigation...", BehaviorDebugEnum.AutoPilot);
				_firstRun = true;
				_lastPathRecalculation = MyAPIGateway.Session.GameDateTime;
				CalculateAreaAndPath();
				//Logger.Write("Total Path Nodes Calculated: " + _pathWaypoints.Count, BehaviorDebugEnum.AutoPilot);

				if (_pathWaypoints.Count >= 2) {

					foreach (var waypoint in _pathWaypoints) {

						//Logger.Write(" - Waypoint: " + waypoint, BehaviorDebugEnum.AutoPilot);

					}
				
				}
				MyVisualScriptLogicProvider.ShowNotificationToAll("RoverPath", 5000, "Green");
				GetNextWaypoint(currentPos);

			}

			if (_endPath || _currentWaterPathWaypoint == Vector3D.Zero)
				return currentPos;

			if (Vector3D.Distance(currentPos, _currentWaterPathWaypoint) < waypointTolerance)
				GetNextWaypoint(currentPos);

			MyVisualScriptLogicProvider.RemoveGPSForAll("Wp");
			MyVisualScriptLogicProvider.RemoveGPSForAll("Target");

			MyVisualScriptLogicProvider.AddGPSForAll("Wp", "", _currentWaterPathWaypoint, Color.Aqua);
			MyVisualScriptLogicProvider.AddGPSForAll("Target", "", _targetCoords, Color.Red);

			DrawCurrentPath();


			return _currentWaterPathWaypoint == Vector3D.Zero ? currentPos : _currentWaterPathWaypoint;
		
		}

		public void DrawCurrentPath() {

			if (MES_SessionCore.IsDedicated)
				return;

			if (_pathWaypoints.Count < 2)
				return;

			Vector4 colorGreen = ConvertColor(Color.Aqua);

			for (int i = 1; i < _pathWaypoints.Count; i++) {

				MySimpleObjectDraw.DrawLine(_pathWaypoints[i-1], _pathWaypoints[i], MyStringId.GetOrCompute("WeaponLaser"), ref colorGreen, 2.0f);

			}

		}

		internal Vector4 ConvertColor(Color color) {

			return new Vector4(color.X / 10, color.Y / 10, color.Z / 10, 0.1f);

		}

		private void GetNextWaypoint(Vector3D currentPos) {

			if (_pathWaypoints.Count == 0) {

				_endPath = true;
				_currentWaterPathWaypoint = Vector3D.Zero;
				return;

			}

			_currentWaterPathWaypoint = _pathWaypoints[0];
			_pathWaypoints.RemoveAt(0);


		}

		private void CalculateAreaAndPath() {

			//Reset Existing Values
			_closestNodeToStart = null;
			_closestNodeToEnd = null;
			_endPath = false;
			_goodNodes.Clear();
			_badNodes.Clear();
			_pathWaypoints.Clear();
			_shorelineNodes.Clear();

			if (!EnablePathGeneration) {

				_endPath = true;
				return;

			}

			//Create The Grid Of Nodes
			var up = _behavior.AutoPilot.UpDirectionFromPlanet;
			var forward = Vector3D.CalculatePerpendicularVector(up);
			var myPos = _behavior.RemoteControl.GetPosition();
			var myMatrix = MatrixD.CreateWorld(myPos, forward, up);
			double distanceOffsetHalf = (_worldStepDistance * _steps) / 2;
			var startOffset = new Vector3D(-distanceOffsetHalf, 0, -distanceOffsetHalf);
			var startGridCoords = Vector3D.Transform(startOffset, myMatrix);
			_targetCoords = _behavior.AutoPilot.GetPendingWaypoint();

			for (int x = 0; x <= _steps; x++) {

				for (int y = 0; y <= _steps; y++) {

					var gridWorldCellOffset = startOffset;
					gridWorldCellOffset.X += (x * _worldStepDistance);
					gridWorldCellOffset.Z += (y * _worldStepDistance);

					var gridWorldCoords = Vector3D.Transform(gridWorldCellOffset, myMatrix);

					var surfaceCoords = _behavior.AutoPilot.CurrentPlanet.SurfaceCoordsAtPosition(gridWorldCoords);

					if (x == 0 & y == 0)
						_gridStartCoords = surfaceCoords;

					if (x == _steps - 1 & y == _steps - 1)
						_gridEndCoords = surfaceCoords;

					var node = new PathNode(surfaceCoords, x, y);

					var Altdifference = _behavior.AutoPilot.CurrentPlanet.DistanceToCore(surfaceCoords)- _behavior.AutoPilot.CurrentPlanet.DistanceToCore(myPos);

					

					if (Math.Abs(Altdifference) > _minimumAltitudeDifference) {

						_badNodes.Add(node);
						continue;

					}
	

					_goodNodes.Add(node);
					node.DistanceFromStart = Vector3D.Distance(myPos, surfaceCoords);
					node.DistanceFromEnd = Vector3D.Distance(surfaceCoords, _targetCoords);

					if (_closestNodeToStart == null || node.DistanceFromStart < _closestNodeToStart.DistanceFromStart)
						_closestNodeToStart = node;

					if (_closestNodeToEnd == null || node.DistanceFromEnd < _closestNodeToEnd.DistanceFromEnd)
						_closestNodeToEnd = node;

				}

			}

			for (int i = _goodNodes.Count - 1; i >= 0; i--) {

				if (GetNeighbours(_goodNodes[i], true).Count > 0) {

					_shorelineNodes.Add(_goodNodes[i]);
					_goodNodes.RemoveAt(i);

				}

			}

			if (_shorelineNodes.Contains(_closestNodeToStart)) {

				_closestNodeToStart = null;

				foreach (var node in _goodNodes) {

					if (_closestNodeToStart == null || node.DistanceFromStart < _closestNodeToStart.DistanceFromStart)
						_closestNodeToStart = node;

				}
			
			}

			if (_shorelineNodes.Contains(_closestNodeToEnd)) {

				_closestNodeToEnd = null;

				foreach (var node in _goodNodes) {

					if (_closestNodeToEnd == null || node.DistanceFromEnd < _closestNodeToEnd.DistanceFromEnd)
						_closestNodeToEnd = node;

				}

			}


			//Logger.Write("Total Nodes Calculated: " + _goodNodes.Count, BehaviorDebugEnum.AutoPilot);


			//Calculate the Path
			_pathWaypoints = new List<Vector3D>(FindPath());

		}

		private Vector3D[] FindPath() {

			Vector3D[] waypoints = new Vector3D[0];

			if (_closestNodeToStart == null || _closestNodeToEnd == null || _closestNodeToStart == _closestNodeToEnd)
				return waypoints;

			bool pathSuccess = false;
			_closestNodeToStart.Parent = _closestNodeToStart;

			Heap<PathNode> openSet = new Heap<PathNode>(_steps*_steps);
			HashSet<PathNode> closedSet = new HashSet<PathNode>();
			openSet.Add(_closestNodeToStart);

			while (openSet.Count > 0) {

				PathNode currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);

				if (currentNode == _closestNodeToEnd) {

					pathSuccess = true;
					break;

				}

				foreach (var neighbour in GetNeighbours(currentNode)) {

					if (closedSet.Contains(neighbour))
						continue;

					int newMoveCostToNeighbour = currentNode.CostG + GetDistance(currentNode, neighbour);

					if (newMoveCostToNeighbour < neighbour.CostG || !openSet.Contains(neighbour)) {

						neighbour.CostG = newMoveCostToNeighbour;
						neighbour.CostH = GetDistance(neighbour, _closestNodeToEnd);
						neighbour.Parent = currentNode;

						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
						else
							openSet.UpdateItem(neighbour);


					}

				}

			}

			if (pathSuccess) {

				waypoints = RetracePath(_closestNodeToStart, _closestNodeToEnd);

			}

			return waypoints;

		}

		Vector3D[] RetracePath(PathNode startNode, PathNode endNode) {
			List<PathNode> path = new List<PathNode>();
			PathNode currentNode = endNode;

			while (currentNode != startNode) {
				path.Add(currentNode);
				currentNode = currentNode.Parent;
			}

			//Logger.Write("Unsimplified Path Waypoint Count: " + path.Count, BehaviorDebugEnum.AutoPilot);

			Vector3D[] waypoints = SimplifyPath(path);
			Array.Reverse(waypoints);
			return waypoints;

		}

		Vector3D[] SimplifyPath(List<PathNode> path) {
			List<Vector3D> waypoints = new List<Vector3D>();
			Vector3I directionOld = Vector3I.Zero;

			for (int i = 1; i < path.Count; i++) {
				Vector3I directionNew = new Vector3I(path[i - 1].GridX - path[i].GridX, path[i - 1].GridY - path[i].GridY, 0);
				if (directionNew != directionOld) {
					waypoints.Add(path[i].WorldPos);
				}
				directionOld = directionNew;
			}

			//Logger.Write("Simplified Path Waypoint Count: " + waypoints.Count, BehaviorDebugEnum.AutoPilot);
			return waypoints.ToArray();
		}

		private List<PathNode> GetNeighbours(PathNode primaryNode, bool checkBad = false) {

			var result = new List<PathNode>();
			var nodesToCheck = !checkBad ? _goodNodes : _badNodes;

			foreach (var node in nodesToCheck) {

				if (node == primaryNode)
					continue;

				if (node.GridX > primaryNode.GridX + 1 || node.GridX < primaryNode.GridX - 1)
					continue;

				if (node.GridY > primaryNode.GridY + 1 || node.GridY < primaryNode.GridY - 1)
					continue;

				//Logger.Write("Got Neightbour Node", BehaviorDebugEnum.AutoPilot);
				result.Add(node);

			}

			return result;
		
		}

		private int GetDistance(PathNode nodeA, PathNode nodeB) {

			int dstX = Math.Abs(nodeA.GridX - nodeB.GridX);
			int dstY = Math.Abs(nodeA.GridY - nodeB.GridY);

			if (dstX > dstY)
				return 14 * dstY + 10 * (dstX - dstY);

			return 14 * dstX + 10 * (dstY - dstX);

		}

	}

	public class RoverHeap<T> where T : IHeapItem<T> {

		T[] items;
		int currentItemCount;

		public RoverHeap(int maxHeapSize) {
			items = new T[maxHeapSize];
		}

		public void Add(T item) {
			item.HeapIndex = currentItemCount;
			items[currentItemCount] = item;
			SortUp(item);
			currentItemCount++;
		}

		public T RemoveFirst() {
			T firstItem = items[0];
			currentItemCount--;
			items[0] = items[currentItemCount];
			items[0].HeapIndex = 0;
			SortDown(items[0]);
			return firstItem;
		}

		public void UpdateItem(T item) {
			SortUp(item);
		}

		public int Count {
			get {
				return currentItemCount;
			}
		}

		public bool Contains(T item) {
			return Equals(items[item.HeapIndex], item);
		}

		void SortDown(T item) {
			while (true) {
				int childIndexLeft = item.HeapIndex * 2 + 1;
				int childIndexRight = item.HeapIndex * 2 + 2;
				int swapIndex = 0;

				if (childIndexLeft < currentItemCount) {
					swapIndex = childIndexLeft;

					if (childIndexRight < currentItemCount) {
						if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) {
							swapIndex = childIndexRight;
						}
					}

					if (item.CompareTo(items[swapIndex]) < 0) {
						Swap(item, items[swapIndex]);
					} else {
						return;
					}

				} else {
					return;
				}

			}
		}

		void SortUp(T item) {
			int parentIndex = (item.HeapIndex - 1) / 2;

			while (true) {
				T parentItem = items[parentIndex];
				if (item.CompareTo(parentItem) > 0) {
					Swap(item, parentItem);
				} else {
					break;
				}

				parentIndex = (item.HeapIndex - 1) / 2;
			}
		}

		void Swap(T itemA, T itemB) {
			items[itemA.HeapIndex] = itemB;
			items[itemB.HeapIndex] = itemA;
			int itemAIndex = itemA.HeapIndex;
			itemA.HeapIndex = itemB.HeapIndex;
			itemB.HeapIndex = itemAIndex;
		}



	}

	public interface IRoverHeapHeapItem<T> : IComparable<T> {

		int HeapIndex {
			get;
			set;
		}

	}

}
