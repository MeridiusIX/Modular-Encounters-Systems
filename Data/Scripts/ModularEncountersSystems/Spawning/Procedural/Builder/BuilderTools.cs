using ModularEncountersSystems.Core;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.ObjectBuilders;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Procedural.Builder {

	public enum BlockCategory {

		None,
		Armor,


	}

	public static class BuilderTools {

		public static Dictionary<BlockCategory, MyObjectBuilder_CubeGrid> BlockCategoryPrefabReference = new Dictionary<BlockCategory, MyObjectBuilder_CubeGrid>();
		public static Dictionary<BlockCategory, Dictionary<MyObjectBuilder_CubeBlock, MyObjectBuilder_CubeBlock>> SymmetryXReference = new Dictionary<BlockCategory, Dictionary<MyObjectBuilder_CubeBlock, MyObjectBuilder_CubeBlock>>();
		public static Dictionary<BlockCategory, Dictionary<MyObjectBuilder_CubeBlock, MyObjectBuilder_CubeBlock>> SymmetryYReference = new Dictionary<BlockCategory, Dictionary<MyObjectBuilder_CubeBlock, MyObjectBuilder_CubeBlock>>();

		private static SerializableVector3 _colorMask = new SerializableVector3(0.122222222f, 0.05f, 0.46f);

		private static Dictionary<MyObjectBuilder_CubeBlock, MyObjectBuilder_CubeBlock> _tempDict = new Dictionary<MyObjectBuilder_CubeBlock, MyObjectBuilder_CubeBlock>();
		private static MyObjectBuilder_CubeBlock _symmetryXBlock = null;
		private static MyObjectBuilder_CubeBlock _symmetryYBlock = null;

		public static Vector3I IncrementX1 = new Vector3I(1, 0, 0);
		public static Vector3I IncrementZ1 = new Vector3I(0, 0, 1);
		public static Vector3I IncrementX1Z1 = new Vector3I(1, 0, 1);
		public static Vector3I IncrementnX1Z1 = new Vector3I(-1, 0, 1);

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

		public static MyObjectBuilder_CubeBlock GetSymmetryOrientation(ShipConstruct construct, BlockCategory category, MyObjectBuilder_CubeBlock orientation, bool xSymmetry, bool ySymmetry) {

			if (!xSymmetry && !ySymmetry)
				return orientation;

			if (xSymmetry && !ySymmetry) {

				return GetSymmetryOrientation(construct, category, orientation, SymmetryXReference);

			}

			if (!xSymmetry && ySymmetry) {

				return GetSymmetryOrientation(construct, category, orientation, SymmetryYReference);

			}

			if (xSymmetry && ySymmetry) {

				var xOrientation = GetSymmetryOrientation(construct, category, orientation, SymmetryXReference);
				return GetSymmetryOrientation(construct, category, xOrientation, SymmetryYReference);

			}

			construct.Log.Append("Couldn't get any symmetry. Using Default orientation").Append(" - ").Append(category.ToString()).Append(" - ").AppendLine();
			return orientation;

		}

		private static MyObjectBuilder_CubeBlock GetSymmetryOrientation(ShipConstruct construct, BlockCategory category, MyObjectBuilder_CubeBlock orientation, Dictionary<BlockCategory, Dictionary<MyObjectBuilder_CubeBlock, MyObjectBuilder_CubeBlock>> referenceDict) {

			_tempDict = null;

			if (!referenceDict.TryGetValue(category, out _tempDict)) {

				construct.Log.Append("Block Doesn't Have Symmetry Reference. Using Default orientation").Append(" - ").Append(category.ToString()).Append(" - ").AppendLine();
				return orientation;

			}

			MyObjectBuilder_CubeBlock newOrientation;

			if (_tempDict.TryGetValue(orientation, out newOrientation)) {

				return newOrientation;

			}

			construct.Log.Append("Symmetry Not Found In Current Reference. Using Default orientation").Append(" - ").Append(category.ToString()).Append(" - ").Append(orientation.ToString()).AppendLine();
			return orientation;

		}

		public static MyObjectBuilder_CubeBlock GetBlockAtMinPosition(Vector3I min, MyObjectBuilder_CubeGrid grid) {

			if (grid?.CubeBlocks == null)
				return null;

			foreach (var block in grid.CubeBlocks) {

				if ((Vector3I)block.Min == min)
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
			SymmetryReferenceSetup();
			LineBuilder.Setup();

		}

		/*
		private static void MasterReferenceSetup() {

			var prefab = MyDefinitionManager.Static.GetPrefabDefinition("MES-Prefab-BlockOrientationReference");

			if (prefab?.CubeGrids == null || prefab.CubeGrids.Length == 0) {

				//SpawnLogger.Write("MES-Prefab-BlockOrientationReference Not Found", SpawnerDebugEnum.Dev, true);
				return;

			}
				
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

			//SpawnLogger.Write("Total Blocks Registered From MES-Prefab-BlockOrientationReference: " + count.ToString(), SpawnerDebugEnum.Dev, true);

		}
		*/

		private static void SymmetryReferenceSetup() {

			var armorPrefab = MyDefinitionManager.Static.GetPrefabDefinition("MES-Prefab-Symmetry-Armor");
			ProcessSymmetryPrefab(armorPrefab?.CubeGrids, BlockCategory.Armor);


		}

		private static void ProcessSymmetryPrefab(MyObjectBuilder_CubeGrid[] gridList, BlockCategory category) {

			if (gridList == null || gridList.Length == 0 || gridList[0] == null)
				return;

			int count = 0;
			var grid = gridList[0];
			MyCubeBlockDefinition blockDef = null;

			BlockCategoryPrefabReference.Add(category, grid);

			foreach (var block in grid.CubeBlocks) {

				if (block.ColorMaskHSV != _colorMask)
					continue;

				_symmetryXBlock = null;
				_symmetryYBlock = null;

				var id = block.GetId();

				if (!DefinitionHelper.AllBlockDefinitionsDictionary.TryGetValue(id, out blockDef) || blockDef == null) {

					continue;

				}

				//Process X
				var mirrorMinX = new Vector3I(-block.Min.X, block.Min.Y, block.Min.Z);

				if (blockDef.Size != Vector3I.One) {

					int newX = block.Min.X - (blockDef.Size.X - 1);
					mirrorMinX.X = newX;

				}

				_symmetryXBlock = GetBlockAtMinPosition(mirrorMinX, grid);

				if (_symmetryXBlock != null) {

					AddSymmetryReference(true, SymmetryXReference, category, block, _symmetryXBlock);

				}

				//Process Y
				var mirrorMinY = new Vector3I(block.Min.X, -block.Min.Y, block.Min.Z);

				if (blockDef.Size != Vector3I.One) {

					int newY = block.Min.Y - (blockDef.Size.Y - 1);
					mirrorMinY.Y = newY;

				}
				_symmetryYBlock = GetBlockAtMinPosition(mirrorMinY, grid);

				if (_symmetryYBlock != null) {

					AddSymmetryReference(false, SymmetryYReference, category, block, _symmetryYBlock);

				}

			}

		}

		private static void AddSymmetryReference(bool x, Dictionary<BlockCategory, Dictionary<MyObjectBuilder_CubeBlock, MyObjectBuilder_CubeBlock>> reference, BlockCategory category, MyObjectBuilder_CubeBlock orientation, MyObjectBuilder_CubeBlock mirrorOrientation) {

			if (!reference.TryGetValue(category, out _tempDict)) {

				_tempDict = new Dictionary<MyObjectBuilder_CubeBlock, MyObjectBuilder_CubeBlock>();
				reference.Add(category, _tempDict);

			}

			if (!_tempDict.ContainsKey(orientation)) {

				//SpawnLogger.Write((x ? "X" : "Y") + " Symmetry Added For: " + id.ToString() + " - " + orientation.ToString() + " - " + mirrorOrientation.ToString(), SpawnerDebugEnum.Dev);
				_tempDict.Add(orientation, mirrorOrientation);

			}

		}

	}

}
