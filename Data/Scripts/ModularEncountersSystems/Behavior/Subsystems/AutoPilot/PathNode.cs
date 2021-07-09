using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Behavior.Subsystems.AutoPilot {

	public class PathNode : IHeapItem<PathNode> {

		public Vector3D WorldPos;
		public Vector3I GridPos;
		public double DistanceFromStart;
		public double DistanceFromEnd;
		public int GridX;
		public int GridY;
		public PathNode Parent;

		public int CostF { get { return CostG - CostH; } }
		public int CostG;
		public int CostH;

		public int heapIndex;

		public PathNode(Vector3D worldPos, int x, int y) {

			WorldPos = worldPos;
			GridPos = new Vector3I(x, y, 0);
			GridX = x;
			GridY = y;

		}

		public int CompareTo(PathNode nodeToCompare) {

			int compare = CostF.CompareTo(nodeToCompare.CostF);

			if (compare == 0) {

				compare = CostH.CompareTo(nodeToCompare.CostH);

			}

			return -compare;

		}

		public int HeapIndex {
			get {
				return heapIndex;
			}
			set {
				heapIndex = value;
			}
		}

	}

}
