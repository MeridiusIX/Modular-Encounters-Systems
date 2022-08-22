using Sandbox.Common.ObjectBuilders;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Profiles {

	public enum SafezoneAccessType {
	
		Whitelist,
		Blacklist
	
	}

	[Flags]
	public enum SafezoneAction {

		Damage = 1,
		Shooting = 2,
		Drilling = 4,
		Welding = 8,
		Grinding = 0x10,
		VoxelHand = 0x20,
		Building = 0x40,
		LandingGearLock = 0x80,
		ConvertToStation = 0x100,
		BuildingProjections = 0x200,
		All = 0x3FF,
		AdminIgnore = 0x37E

	}

	public class SafezoneProfile {

		public MySafeZoneShape Shape;

		public Vector3D Coordinates;
		public Vector3D Offset;
		public Vector3D MinOffset;
		public Vector3D MaxOffset;

		public double Radius;

		public bool Enabled;
		public bool IsVisible;
		public Vector3 Color;
		public string Texture;

		public SafezoneAccessType FactionAccess;
		public SafezoneAccessType GridAccess;
		public SafezoneAccessType PlayerAccess;

		public SafezoneAction AllowedActions;

	}

}
