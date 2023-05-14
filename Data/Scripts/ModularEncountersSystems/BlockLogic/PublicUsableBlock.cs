using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.Terminal;
using ProtoBuf;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage;
using VRage.Game.ModAPI;
using VRage.ModAPI;

namespace ModularEncountersSystems.BlockLogic {
	public class PublicUsableBlock : BaseBlockLogic, IBlockLogic {

		private BlockEntity _block;
		private IMyCubeGrid _grid;

		public IMyTerminalBlock TerminalBlock;

		public PublicUsableBlock(BlockEntity block) {

			Setup(block);

		}

		internal override void Setup(BlockEntity block) {

			base.Setup(block);

			if (block?.Block == null || !_isServer) {

				_isValid = false;
				return;
			
			}

			_block = block;
			TerminalBlock = block.Block;

			//Pass
			_grid = TerminalBlock.SlimBlock.CubeGrid;
			_grid.OnBlockOwnershipChanged += OwnershipChange;
			_grid.OnGridSplit += GridSplit;
			OwnershipChange(_grid);

		}

		internal void OwnershipChange(IMyCubeGrid cubeGrid) {

			if (!_block.ActiveEntity() || _grid == null || _grid.MarkedForClose) {

				Unload();
				return;
			
			}

			var cubeBlock = _block.Block as MyCubeBlock;

			if (cubeBlock?.IDModule == null)
				return;

			if (cubeBlock.IDModule.ShareMode != VRage.Game.MyOwnershipShareModeEnum.All)
				cubeBlock.ChangeBlockOwnerRequest(Block.Block.OwnerId, VRage.Game.MyOwnershipShareModeEnum.All);

		}

		internal void GridSplit(IMyCubeGrid a, IMyCubeGrid b) {

			if (!_block.ActiveEntity() || _grid == null || _grid.MarkedForClose) {

				Unload();
				return;
			
			}

			_grid.OnGridSplit -= GridSplit;
			_grid.OnBlockOwnershipChanged -= OwnershipChange;

			_grid = TerminalBlock.SlimBlock.CubeGrid;

			_grid.OnGridSplit += GridSplit;
			_grid.OnBlockOwnershipChanged += OwnershipChange;


		}

		internal void Unload() {

			if (_grid != null) {

				_grid.OnBlockOwnershipChanged -= OwnershipChange;
				_grid.OnGridSplit -= GridSplit;

			}

			_isValid = false;
		
		}

	}

}
