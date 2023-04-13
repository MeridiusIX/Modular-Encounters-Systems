using ModularEncountersSystems.Helpers;
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

		None = 0,
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

		public string ProfileSubtypeId;

		public MySafeZoneShape Shape;
		public bool UseDiamondBoxOrientation;

		public Vector3D Coordinates;
		public Vector3D Offset;
		public Vector3D Size;
		
		public double Radius;
		public bool RadiusFromParentEntity;
		public double ParentEntityRadiusMultiplier;

		public bool Enabled;
		public bool LinkToParentEntity;
		public bool IsVisible;
		public Vector3 Color;
		public string Texture;

		public SafeZoneAccessType FactionAccess;
		public SafeZoneAccessType GridAccess;
		public SafeZoneAccessType PlayerAccess;

		public SafeZoneAction AllowedActions;

		public Dictionary<string, Action<string, object>> EditorReference;

		public SafeZoneProfile(string data) {

			ProfileSubtypeId = "";

			Shape = MySafeZoneShape.Sphere;
			UseDiamondBoxOrientation = false;

			Coordinates = Vector3D.Zero;
			Offset = Vector3D.Zero;
			Size = Vector3D.Zero;

			Radius = 0;
			RadiusFromParentEntity = false;
			ParentEntityRadiusMultiplier = 2.5;

			Enabled = false;
			LinkToParentEntity = false;
			IsVisible = true;
			Color = new Vector3(0, 0, 0);
			Texture = "Default";

			FactionAccess = SafeZoneAccessType.Blacklist;
			GridAccess = SafeZoneAccessType.Blacklist;
			PlayerAccess = SafeZoneAccessType.Blacklist;

			AllowedActions = SafeZoneAction.None;

			EditorReference = new Dictionary<string, Action<string, object>> {

				{"Shape", (s, o) => TagParse.TagSafeZoneShapeEnumCheck(s, ref Shape) },
				{"UseDiamondBoxOrientation", (s, o) => TagParse.TagBoolCheck(s, ref UseDiamondBoxOrientation) },
				{"Coordinates", (s, o) => TagParse.TagVector3DCheck(s, ref Coordinates) },
				{"Offset", (s, o) => TagParse.TagVector3DCheck(s, ref Offset) },
				{"Size", (s, o) => TagParse.TagVector3DCheck(s, ref Size) },
				{"Radius", (s, o) => TagParse.TagDoubleCheck(s, ref Radius) },
				{"RadiusFromParentEntity", (s, o) => TagParse.TagBoolCheck(s, ref RadiusFromParentEntity) },
				{"ParentEntityRadiusMultiplier", (s, o) => TagParse.TagDoubleCheck(s, ref ParentEntityRadiusMultiplier) },
				{"Enabled", (s, o) => TagParse.TagBoolCheck(s, ref Enabled) },
				{"LinkToParentEntity", (s, o) => TagParse.TagBoolCheck(s, ref LinkToParentEntity) },
				{"IsVisible", (s, o) => TagParse.TagBoolCheck(s, ref IsVisible) },
				{"Color", (s, o) => TagParse.TagVector3Check(s, ref Color) },
				{"Texture", (s, o) => TagParse.TagStringCheck(s, ref Texture) },
				{"FactionAccess", (s, o) => TagParse.TagSafeZoneAccessTypeEnumCheck(s, ref FactionAccess) },
				{"GridAccess", (s, o) => TagParse.TagSafeZoneAccessTypeEnumCheck(s, ref GridAccess) },
				{"PlayerAccess", (s, o) => TagParse.TagSafeZoneAccessTypeEnumCheck(s, ref PlayerAccess) },
				{"AllowedActions", (s, o) => TagParse.TagSafeZoneActionEnumCheck(s, ref AllowedActions) },

			};

		}

		public void EditValue(string receivedValue) {

			var processedTag = TagParse.ProcessTag(receivedValue);

			if (processedTag.Length < 2)
				return;

			Action<string, object> referenceMethod = null;

			if (!EditorReference.TryGetValue(processedTag[0], out referenceMethod))
				//TODO: Notes About Value Not Found
				return;

			referenceMethod?.Invoke(receivedValue, null);

		}

		public void InitTags(string customData) {

			if (string.IsNullOrWhiteSpace(customData) == false) {

				var descSplit = customData.Split('\n');

				foreach (var tag in descSplit) {

					EditValue(tag);

				}

			}

		}

	}

}
