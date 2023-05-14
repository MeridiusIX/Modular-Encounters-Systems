using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Spawning.Procedural {

	public enum ThrustType {
	
		None,
		Ion,
		Atmo,
		Hydrogen
	
	}

	public class ShipRules {

		//Hull Properties

		public int MinX;
		public int MaxX;

		public int MinY;
		public int MaxY;

		public int MinZ;
		public int MaxZ;

		public int MaxOverageTolerance;

		public bool SymmetricalX;
		public bool SymmetricalY;

		public int[] HullTypes;

		//Nacelle Properties

		public bool HasNacelles;

		public int NacelleMinX;
		public int NacelleMaxX;

		public int NacelleMinY;
		public int NacelleMaxY;

		public int NacelleMinZ;
		public int NacelleMaxZ;

		//Interior Prefab Parts

		public bool UseInteriorPrefab;
		public List<string> InteriorPrefabs;

		//Greeble Prefab Parts
		public bool UseGreeblePrefabs;
		public List<string> GreeblePrefabs;

		//Systems

		public ThrustType Thrust;

		//Paint and Skin



	}

}
