using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Manipulation.Procedural {
	public static class ProceduralGrids {



	}

	public class GridComponent {

		public GridType Grid;
		public GridComponentType Section;
		public List<ComponentAttributes> Attributes;
		public string PrefabName;
		public MyPrefabDefinition Prefab;

		public MountPoint MountingPoint;

		


		public GridComponent() {

			Grid = GridType.None;
			Section = GridComponentType.None;
			Attributes = new List<ComponentAttributes>();
			PrefabName = "";
			Prefab = null;

			MountingPoint = null;


		}

	}

	public class MountPoint {

		public Vector3I MountCoords;

		public Vector3I MinMountSize;
		public Vector3I MaxMountSize;

		public MountPoint() {

			MountCoords = Vector3I.Zero;

			MinMountSize = Vector3I.Zero;
			MaxMountSize = Vector3I.Zero;

		}

	}

	

}
