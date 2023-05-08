using ModularEncountersSystems.Core;
using ModularEncountersSystems.Helpers;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.ObjectBuilders;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Procedural {
	public static class BuilderTools {

		public static Dictionary<MyDefinitionId, MyBlockOrientation> OrientationMasterReference = new Dictionary<MyDefinitionId, MyBlockOrientation>();
		public static Dictionary<MyDefinitionId, Dictionary<MyBlockOrientation, MyBlockOrientation>> SymmetryXReference = new Dictionary<MyDefinitionId, Dictionary<MyBlockOrientation, MyBlockOrientation>>();
		public static Dictionary<MyDefinitionId, Dictionary<MyBlockOrientation, MyBlockOrientation>> SymmetryYReference = new Dictionary<MyDefinitionId, Dictionary<MyBlockOrientation, MyBlockOrientation>>();

		public static MyDefinitionId ArmorBlock = new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorBlock");
		public static MyDefinitionId ArmorSlope = new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorSlope");
		public static MyDefinitionId ArmorCorner = new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorCorner");
		public static MyDefinitionId ArmorInvCorner = new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorCornerInv");

		private static SerializableVector3 _colorMask = new SerializableVector3(0.122222222f, 0.05f, 0.46f);

		private static Dictionary<MyBlockOrientation, MyBlockOrientation> _tempDict = new Dictionary<MyBlockOrientation, MyBlockOrientation>();
		private static MyObjectBuilder_CubeBlock _symmetryXBlock = null;
		private static MyObjectBuilder_CubeBlock _symmetryYBlock = null;

		public static MyBlockOrientation DefaultOrientation;

		public static List<MyDefinitionId> CubeShapedBlocks = new List<MyDefinitionId>();

		public static MyObjectBuilder_CubeBlock AddBlockToGrid(MyObjectBuilder_CubeGrid grid, MyDefinitionId id, Vector3I? pos = null, SerializableBlockOrientation? orientation = null, Vector3? color = null, string skin = null) {
			
			try {

				var ob = MyObjectBuilderSerializer.CreateNewObject(id);
				var block = ob as MyObjectBuilder_CubeBlock;

				if (block == null || grid?.CubeBlocks == null)
					return block;

				if (pos.HasValue)
					block.Min = pos.Value;

				if (orientation.HasValue)
					block.BlockOrientation = orientation.Value;

				if (color.HasValue)
					block.ColorMaskHSV = color.Value;

				if (skin != null)
					block.SkinSubtypeId = skin;

				grid.CubeBlocks.Add(block);
				return block;

			} catch (Exception e) {
			
				
			
			}

			return null;

		}

		public static MyBlockOrientation GetSymmetryOrientation(MyDefinitionId id, MyBlockOrientation orientation, bool xSymmetry, bool ySymmetry) {

			if (!xSymmetry && !ySymmetry)
				return orientation;

			if (xSymmetry && !ySymmetry) {

				return GetSymmetryOrientation(id, orientation, SymmetryXReference);

			}

			if (!xSymmetry && ySymmetry) {

				return GetSymmetryOrientation(id, orientation, SymmetryYReference);

			}

			if (xSymmetry && ySymmetry) {

				var xOrientation = GetSymmetryOrientation(id, orientation, SymmetryXReference);
				return GetSymmetryOrientation(id, xOrientation, SymmetryYReference);

			}

			//TODO: raise error in log
			return orientation;

		}

		private static MyBlockOrientation GetSymmetryOrientation(MyDefinitionId id, MyBlockOrientation orientation, Dictionary<MyDefinitionId, Dictionary<MyBlockOrientation, MyBlockOrientation>> referenceDict) {

			_tempDict = null;

			if (!referenceDict.TryGetValue(id, out _tempDict)) {

				//TODO: raise error in log
				return orientation;

			}

			MyBlockOrientation newOrientation;

			if (_tempDict.TryGetValue(orientation, out newOrientation)) {

				return newOrientation;

			}

			//TODO: raise error in log
			return orientation;

		}

		public static MyObjectBuilder_CubeBlock GetBlockAtMinPosition(Vector3I min, MyObjectBuilder_CubeGrid grid) {

			if (grid.CubeBlocks == null)
				return null;

			foreach (var block in grid.CubeBlocks) {

				if (((Vector3I)(block.Min)) == min)
					return block;
			
			}

			return null;
		
		}

		public static MyBlockOrientation RotateOrientation(MyBlockOrientation baseOrientation, int pitch, int yaw, int roll) {

			var orientation = baseOrientation;

			if (pitch > 0)
				orientation = RotatePitch(orientation, pitch);

			if (yaw > 0)
				orientation = RotateYaw(orientation, yaw);

			if (roll > 0)
				orientation = RotateRoll(orientation, roll);

			return orientation;

		}

		public static MyBlockOrientation RotateYaw(MyBlockOrientation original, int steps = 1) {

			MyBlockOrientation orientation = original;

			for (int i = 0; i < steps; i++) {

				orientation = RotateYaw(orientation);

			}

			return orientation;
		
		}

		private static MyBlockOrientation RotateYaw(MyBlockOrientation original) {

			//Up
			if (original.Up == Base6Directions.Direction.Up) {

				if (original.Forward == Base6Directions.Direction.Forward)
					return new MyBlockOrientation(Base6Directions.Direction.Right, Base6Directions.Direction.Up);

				if (original.Forward == Base6Directions.Direction.Right)
					return new MyBlockOrientation(Base6Directions.Direction.Backward, Base6Directions.Direction.Up);

				if (original.Forward == Base6Directions.Direction.Backward)
					return new MyBlockOrientation(Base6Directions.Direction.Left, Base6Directions.Direction.Up);

				if (original.Forward == Base6Directions.Direction.Left)
					return new MyBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Up);

			}

			//Down
			if (Base6Directions.GetFlippedDirection(original.Up) == Base6Directions.Direction.Up) {

				if (original.Forward == Base6Directions.Direction.Forward)
					return new MyBlockOrientation(Base6Directions.Direction.Right, Base6Directions.Direction.Down);

				if (original.Forward == Base6Directions.Direction.Right)
					return new MyBlockOrientation(Base6Directions.Direction.Backward, Base6Directions.Direction.Down);

				if (original.Forward == Base6Directions.Direction.Backward)
					return new MyBlockOrientation(Base6Directions.Direction.Left, Base6Directions.Direction.Down);

				if (original.Forward == Base6Directions.Direction.Left)
					return new MyBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Down);

			}

			//Left
			if (original.Left == Base6Directions.Direction.Up) {

				if (original.Forward == Base6Directions.Direction.Right)
					return new MyBlockOrientation(Base6Directions.Direction.Backward, Base6Directions.Direction.Left);

				if (original.Forward == Base6Directions.Direction.Backward)
					return new MyBlockOrientation(Base6Directions.Direction.Left, Base6Directions.Direction.Forward);

				if (original.Forward == Base6Directions.Direction.Left)
					return new MyBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Right);

				if (original.Forward == Base6Directions.Direction.Forward)
					return new MyBlockOrientation(Base6Directions.Direction.Right, Base6Directions.Direction.Backward);

			}

			//Right
			if (Base6Directions.GetFlippedDirection(original.Left) == Base6Directions.Direction.Up) {

				if (original.Forward == Base6Directions.Direction.Left)
					return new MyBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Left);

				if (original.Forward == Base6Directions.Direction.Forward)
					return new MyBlockOrientation(Base6Directions.Direction.Right, Base6Directions.Direction.Forward);

				if (original.Forward == Base6Directions.Direction.Right)
					return new MyBlockOrientation(Base6Directions.Direction.Backward, Base6Directions.Direction.Right);

				if (original.Forward == Base6Directions.Direction.Backward)
					return new MyBlockOrientation(Base6Directions.Direction.Left, Base6Directions.Direction.Backward);

			}

			//Forward
			if (original.Forward == Base6Directions.Direction.Up) {

				if (original.Up == Base6Directions.Direction.Forward)
					return new MyBlockOrientation(Base6Directions.Direction.Up, Base6Directions.Direction.Right);

				if (original.Up == Base6Directions.Direction.Right)
					return new MyBlockOrientation(Base6Directions.Direction.Up, Base6Directions.Direction.Backward);

				if (original.Up == Base6Directions.Direction.Backward)
					return new MyBlockOrientation(Base6Directions.Direction.Up, Base6Directions.Direction.Left);

				if (original.Up == Base6Directions.Direction.Left)
					return new MyBlockOrientation(Base6Directions.Direction.Up, Base6Directions.Direction.Forward);

			}

			//Backward
			if (Base6Directions.GetFlippedDirection(original.Forward) == Base6Directions.Direction.Up) {

				if (original.Up == Base6Directions.Direction.Forward)
					return new MyBlockOrientation(Base6Directions.Direction.Down, Base6Directions.Direction.Right);

				if (original.Up == Base6Directions.Direction.Right)
					return new MyBlockOrientation(Base6Directions.Direction.Down, Base6Directions.Direction.Backward);

				if (original.Up == Base6Directions.Direction.Backward)
					return new MyBlockOrientation(Base6Directions.Direction.Down, Base6Directions.Direction.Left);

				if (original.Up == Base6Directions.Direction.Left)
					return new MyBlockOrientation(Base6Directions.Direction.Down, Base6Directions.Direction.Forward);

			}

			return original;

		}

		public static MyBlockOrientation RotatePitch(MyBlockOrientation original, int steps = 1) {

			MyBlockOrientation orientation = original;

			for (int i = 0; i < steps; i++) {

				orientation = RotatePitch(orientation);

			}

			return orientation;

		}

		private static MyBlockOrientation RotatePitch(MyBlockOrientation original) {

			//Left
			if (original.Left == Base6Directions.Direction.Left) {

				if (original.Forward == Base6Directions.Direction.Forward)
					return new MyBlockOrientation(Base6Directions.Direction.Down, Base6Directions.Direction.Forward);

				if (original.Forward == Base6Directions.Direction.Down)
					return new MyBlockOrientation(Base6Directions.Direction.Backward, Base6Directions.Direction.Down);

				if (original.Forward == Base6Directions.Direction.Backward)
					return new MyBlockOrientation(Base6Directions.Direction.Up, Base6Directions.Direction.Backward);

				if (original.Forward == Base6Directions.Direction.Up)
					return new MyBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Up);

			}

			//Right
			if (Base6Directions.GetFlippedDirection(original.Left) == Base6Directions.Direction.Left) {

				if (original.Forward == Base6Directions.Direction.Forward)
					return new MyBlockOrientation(Base6Directions.Direction.Down, Base6Directions.Direction.Backward);

				if (original.Forward == Base6Directions.Direction.Down)
					return new MyBlockOrientation(Base6Directions.Direction.Backward, Base6Directions.Direction.Up);

				if (original.Forward == Base6Directions.Direction.Backward)
					return new MyBlockOrientation(Base6Directions.Direction.Up, Base6Directions.Direction.Forward);

				if (original.Forward == Base6Directions.Direction.Up)
					return new MyBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Down);

			}

			//Up
			if (original.Up == Base6Directions.Direction.Left) {

				if (original.Forward == Base6Directions.Direction.Up)
					return new MyBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Left);

				if (original.Forward == Base6Directions.Direction.Forward)
					return new MyBlockOrientation(Base6Directions.Direction.Down, Base6Directions.Direction.Left);

				if (original.Forward == Base6Directions.Direction.Down)
					return new MyBlockOrientation(Base6Directions.Direction.Backward, Base6Directions.Direction.Left);

				if (original.Forward == Base6Directions.Direction.Backward)
					return new MyBlockOrientation(Base6Directions.Direction.Up, Base6Directions.Direction.Left);

			}

			//Down
			if (Base6Directions.GetFlippedDirection(original.Up) == Base6Directions.Direction.Left) {

				if (original.Forward == Base6Directions.Direction.Up)
					return new MyBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Right);

				if (original.Forward == Base6Directions.Direction.Forward)
					return new MyBlockOrientation(Base6Directions.Direction.Down, Base6Directions.Direction.Right);

				if (original.Forward == Base6Directions.Direction.Down)
					return new MyBlockOrientation(Base6Directions.Direction.Backward, Base6Directions.Direction.Right);

				if (original.Forward == Base6Directions.Direction.Backward)
					return new MyBlockOrientation(Base6Directions.Direction.Up, Base6Directions.Direction.Right);

			}

			//Forward
			if (original.Forward == Base6Directions.Direction.Left) {

				if (original.Up == Base6Directions.Direction.Up)
					return new MyBlockOrientation(Base6Directions.Direction.Left, Base6Directions.Direction.Forward);

				if (original.Up == Base6Directions.Direction.Forward)
					return new MyBlockOrientation(Base6Directions.Direction.Left, Base6Directions.Direction.Down);

				if (original.Up == Base6Directions.Direction.Down)
					return new MyBlockOrientation(Base6Directions.Direction.Left, Base6Directions.Direction.Backward);

				if (original.Up == Base6Directions.Direction.Backward)
					return new MyBlockOrientation(Base6Directions.Direction.Left, Base6Directions.Direction.Up);

			}

			//Backward
			if (Base6Directions.GetFlippedDirection(original.Forward) == Base6Directions.Direction.Left) {

				if (original.Up == Base6Directions.Direction.Up)
					return new MyBlockOrientation(Base6Directions.Direction.Right, Base6Directions.Direction.Forward);

				if (original.Up == Base6Directions.Direction.Forward)
					return new MyBlockOrientation(Base6Directions.Direction.Right, Base6Directions.Direction.Down);

				if (original.Up == Base6Directions.Direction.Down)
					return new MyBlockOrientation(Base6Directions.Direction.Right, Base6Directions.Direction.Backward);

				if (original.Up == Base6Directions.Direction.Backward)
					return new MyBlockOrientation(Base6Directions.Direction.Right, Base6Directions.Direction.Up);

			}

			return original;

		}

		public static MyBlockOrientation RotateRoll(MyBlockOrientation original, int steps = 1) {

			MyBlockOrientation orientation = original;

			for (int i = 0; i < steps; i++) {

				orientation = RotateRoll(orientation);

			}

			return orientation;

		}

		private static MyBlockOrientation RotateRoll(MyBlockOrientation original) {

			//Forward
			if (original.Forward == Base6Directions.Direction.Forward) {

				if (original.Up == Base6Directions.Direction.Up)
					return new MyBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Right);

				if (original.Up == Base6Directions.Direction.Right)
					return new MyBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Down);

				if (original.Up == Base6Directions.Direction.Down)
					return new MyBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Left);

				if (original.Up == Base6Directions.Direction.Left)
					return new MyBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Up);

			}

			//Backward
			if (Base6Directions.GetFlippedDirection(original.Forward) == Base6Directions.Direction.Forward) {

				if (original.Up == Base6Directions.Direction.Up)
					return new MyBlockOrientation(Base6Directions.Direction.Backward, Base6Directions.Direction.Right);

				if (original.Up == Base6Directions.Direction.Right)
					return new MyBlockOrientation(Base6Directions.Direction.Backward, Base6Directions.Direction.Down);

				if (original.Up == Base6Directions.Direction.Down)
					return new MyBlockOrientation(Base6Directions.Direction.Backward, Base6Directions.Direction.Left);

				if (original.Up == Base6Directions.Direction.Left)
					return new MyBlockOrientation(Base6Directions.Direction.Backward, Base6Directions.Direction.Up);

			}

			//Left
			if (original.Left == Base6Directions.Direction.Forward) {

				if (original.Up == Base6Directions.Direction.Up)
					return new MyBlockOrientation(Base6Directions.Direction.Down, Base6Directions.Direction.Right);

				if (original.Up == Base6Directions.Direction.Right)
					return new MyBlockOrientation(Base6Directions.Direction.Left, Base6Directions.Direction.Down);

				if (original.Up == Base6Directions.Direction.Down)
					return new MyBlockOrientation(Base6Directions.Direction.Up, Base6Directions.Direction.Left);

				if (original.Up == Base6Directions.Direction.Left)
					return new MyBlockOrientation(Base6Directions.Direction.Right, Base6Directions.Direction.Up);

			}

			//Right
			if (Base6Directions.GetFlippedDirection(original.Left) == Base6Directions.Direction.Forward) {

				if (original.Up == Base6Directions.Direction.Up)
					return new MyBlockOrientation(Base6Directions.Direction.Up, Base6Directions.Direction.Right);

				if (original.Up == Base6Directions.Direction.Right)
					return new MyBlockOrientation(Base6Directions.Direction.Right, Base6Directions.Direction.Down);

				if (original.Up == Base6Directions.Direction.Down)
					return new MyBlockOrientation(Base6Directions.Direction.Down, Base6Directions.Direction.Left);

				if (original.Up == Base6Directions.Direction.Left)
					return new MyBlockOrientation(Base6Directions.Direction.Left, Base6Directions.Direction.Up);

			}

			//Up
			if (original.Up == Base6Directions.Direction.Forward) {

				if (original.Forward == Base6Directions.Direction.Up)
					return new MyBlockOrientation(Base6Directions.Direction.Right, Base6Directions.Direction.Forward);

				if (original.Forward == Base6Directions.Direction.Right)
					return new MyBlockOrientation(Base6Directions.Direction.Down, Base6Directions.Direction.Forward);

				if (original.Forward == Base6Directions.Direction.Down)
					return new MyBlockOrientation(Base6Directions.Direction.Left, Base6Directions.Direction.Forward);

				if (original.Forward == Base6Directions.Direction.Left)
					return new MyBlockOrientation(Base6Directions.Direction.Up, Base6Directions.Direction.Forward);

			}

			//Down
			if (Base6Directions.GetFlippedDirection(original.Up) == Base6Directions.Direction.Forward) {

				if (original.Forward == Base6Directions.Direction.Up)
					return new MyBlockOrientation(Base6Directions.Direction.Right, Base6Directions.Direction.Backward);

				if (original.Forward == Base6Directions.Direction.Right)
					return new MyBlockOrientation(Base6Directions.Direction.Down, Base6Directions.Direction.Backward);

				if (original.Forward == Base6Directions.Direction.Down)
					return new MyBlockOrientation(Base6Directions.Direction.Left, Base6Directions.Direction.Backward);

				if (original.Forward == Base6Directions.Direction.Left)
					return new MyBlockOrientation(Base6Directions.Direction.Up, Base6Directions.Direction.Backward);

			}

			return original;

		}

		public static void Setup() {

			//Master Orientation Reference

			CubeShapedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorBlock"));
			CubeShapedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeHeavyBlockArmorBlock"));
			CubeShapedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_VirtualMass), "VirtualMassLarge"));
			CubeShapedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_Conveyor), "LargeBlockConveyor"));
			CubeShapedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_Decoy), "LargeDecoy"));
			CubeShapedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_Warhead), "LargeWarhead"));
			CubeShapedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_CargoContainer), "LargeBlockSmallContainer"));
			CubeShapedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_BatteryBlock), "LargeBlockBatteryBlock"));
			CubeShapedBlocks.Add(new MyDefinitionId(typeof(MyObjectBuilder_BatteryBlock), "LargeBlockBatteryBlockWarfare2"));

			DefaultOrientation = new MyBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Up);
			MasterReferenceSetup();
			SymmetryReferenceSetup();


		}

		private static void MasterReferenceSetup() {

			var prefab = MyDefinitionManager.Static.GetPrefabDefinition("MES-Prefab-BlockOrientationReference");

			if (prefab?.CubeGrids == null || prefab.CubeGrids.Length == 0)
				return;

			int count = 0;

			foreach (var block in prefab.CubeGrids[0].CubeBlocks) {

				if (block.ColorMaskHSV != _colorMask)
					continue;

				var id = block.GetId();

				if (OrientationMasterReference.ContainsKey(id))
					continue;

				OrientationMasterReference.Add(id, block.BlockOrientation);
				count++;

			}

		}

		private static void SymmetryReferenceSetup() {

			var prefabList = MyDefinitionManager.Static.GetPrefabDefinitions();

			foreach (var prefab in prefabList) {

				if (prefab.Value?.Context?.ModId == null || prefab.Value.Context.ModId != MES_SessionCore.Instance.ModContext.ModId)
					continue;

				if (!prefab.Value.Id.SubtypeName.StartsWith("Symm-"))
					continue;

				ProcessSymmetryPrefab(prefab.Value.CubeGrids);

			}
		
		}

		private static void ProcessSymmetryPrefab(MyObjectBuilder_CubeGrid[] gridList) {

			if (gridList == null || gridList.Length == 0)
				return;

			int count = 0;
			var grid = gridList[0];
			MyCubeBlockDefinition blockDef = null;

			foreach (var block in grid.CubeBlocks) {

				if (block.ColorMaskHSV != _colorMask)
					continue;

				_symmetryXBlock = null;
				_symmetryYBlock = null;

				var id = block.GetId();

				if (blockDef == null && !DefinitionHelper.AllBlockDefinitionsDictionary.TryGetValue(id, out blockDef)) {

					continue;
				
				}
				
				//Process X
				var mirrorMinX = new Vector3I(-block.Min.X, block.Min.Y, block.Min.Z);

				if (blockDef.Size != Vector3I.One) {

					int newX = block.Min.X - (blockDef.Size.X - 1);
					mirrorMinX.X = newX;

				}

				_symmetryXBlock = GetBlockAtMinPosition(mirrorMinX, grid);

				if (_symmetryXBlock != null && id == _symmetryXBlock.GetId()) {

					AddSymmetryReference(SymmetryXReference, id, block.BlockOrientation, _symmetryXBlock.BlockOrientation);

				}

				//Process Y
				var mirrorMinY = new Vector3I(block.Min.X, -block.Min.Y, block.Min.Z);

				if (blockDef.Size != Vector3I.One) {

					int newY = block.Min.Y - (blockDef.Size.Y - 1);
					mirrorMinY.Y = newY;

				}
				_symmetryYBlock = GetBlockAtMinPosition(mirrorMinY, grid);

				if (_symmetryYBlock != null && id == _symmetryYBlock.GetId()) {

					AddSymmetryReference(SymmetryYReference, id, block.BlockOrientation, _symmetryYBlock.BlockOrientation);

				}

			}

		}

		private static void AddSymmetryReference(Dictionary<MyDefinitionId, Dictionary<MyBlockOrientation, MyBlockOrientation>> reference, MyDefinitionId id, MyBlockOrientation orientation, MyBlockOrientation mirrorOrientation) {

			if (!reference.TryGetValue(id, out _tempDict)) {

				_tempDict = new Dictionary<MyBlockOrientation, MyBlockOrientation>();
				reference.Add(id, _tempDict);

			}

			if (!_tempDict.ContainsKey(orientation))
				_tempDict.Add(orientation, mirrorOrientation);
		
		}

	}

}
