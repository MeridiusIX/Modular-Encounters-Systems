using ModularEncountersSystems.Helpers;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.ObjectBuilders;
using VRageMath;

namespace ModularEncountersSystems.Tasks {
	public class DebugSpawnSingleBlocks : TaskItem, ITaskItem {

		internal int _index;
		internal List<MyCubeBlockDefinition> _blocks;
		internal MatrixD _initialSpawn;

		internal double _xPos;
		internal double _zPos;

		public DebugSpawnSingleBlocks(MatrixD coords) {

			_index = 0;
			_initialSpawn = coords;
			_tickTrigger = 1;
			_blocks = new List<MyCubeBlockDefinition>();

			_xPos = -25;
			_zPos = 10;

			//TODO: Get Blocks
			foreach (var block in DefinitionHelper.AllBlockDefinitions) {

				if (block.Size.X == 1 && block.Size.Y == 1 && block.Size.Z == 1)
					_blocks.Add(block);

			}

		}

		public override void Run() {

			if (_index >= _blocks.Count) {

				_isValid = false;
				return;
			
			}

			var spawnCoords = Vector3D.Transform(new Vector3D(_xPos, 0, _zPos), _initialSpawn);
			var newMatrix = MatrixD.CreateWorld(spawnCoords, _initialSpawn.Forward, _initialSpawn.Up);
			if (!SpawnBlock(_blocks[_index], newMatrix)) {

				_index++;
				return;

			}

			_index++;

			_xPos += 5;

			if (_xPos > 25) {

				_xPos = -25;
				_zPos += 5;
			
			}

		}

		internal bool SpawnBlock(MyCubeBlockDefinition block, MatrixD coords) {

			try {

				var grid = new MyObjectBuilder_CubeGrid();
				grid.GridSizeEnum = MyCubeSize.Large;
				grid.PersistentFlags = MyPersistentEntityFlags2.InScene | MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.CastShadows;
				grid.PositionAndOrientation = new MyPositionAndOrientation(ref coords);
				grid.LinearVelocity = Vector3.Zero;
				grid.AngularVelocity = Vector3.Zero;
				grid.DisplayName = block.Id.ToString();

				var obBlock = MyObjectBuilderSerializer.CreateNewObject(block.Id) as MyObjectBuilder_CubeBlock;

				if (obBlock == null)
					return false;

				obBlock.Min = Vector3I.Zero;

				grid.CubeBlocks.Add(obBlock);

				MyAPIGateway.Entities.RemapObjectBuilder(grid);
				MyAPIGateway.Entities.CreateFromObjectBuilderAndAdd(grid);

			} catch (Exception e) {

				return false;
			
			}

			return true;

		}

	}

}
