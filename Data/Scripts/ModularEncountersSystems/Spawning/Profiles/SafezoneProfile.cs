using Sandbox.Common.ObjectBuilders;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Profiles {

	public enum SafeZoneAccessType {
	
		Whitelist,
		Blacklist
	
	}

	[Flags]
	public enum SafeZoneAction {

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

	public class SafeZoneProfile {

		public MySafeZoneShape Shape;

		public Vector3D Coordinates;
		public Vector3D Offset;
		public Vector3D Size;

		public double Radius;

		public bool Enabled;
		public bool IsVisible;
		public Vector3 Color;
		public string Texture;

		public SafeZoneAccessType FactionAccess;
		public SafeZoneAccessType GridAccess;
		public SafeZoneAccessType PlayerAccess;

		public SafeZoneAction AllowedActions;

		public SafeZoneProfile() {

			Shape = MySafeZoneShape.Sphere;
			Coordinates = Vector3D.Zero;
			Offset = Vector3D.Zero;
			Size = Vector3D.Zero;

			Radius = 0;

			Enabled = false;
			IsVisible = true;
			Color = new Vector3(0, 0, 0);
			Texture = "Default";

			FactionAccess = SafeZoneAccessType.Blacklist;
			GridAccess = SafeZoneAccessType.Blacklist;
			PlayerAccess = SafeZoneAccessType.Blacklist;

		}

	}

}
