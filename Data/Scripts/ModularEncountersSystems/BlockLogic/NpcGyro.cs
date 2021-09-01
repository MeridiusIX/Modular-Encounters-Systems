using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.World;
using Sandbox.Definitions;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;

namespace ModularEncountersSystems.BlockLogic {
	public class NpcGyro : BaseBlockLogic, IBlockLogic {

		private BlockEntity _block;
		private IMyGyro _gyro;
		private IMyCubeGrid _grid;
		private float _multiplier;

		public NpcGyro(BlockEntity block) {

			Setup(block);

		}

		internal override void Setup(BlockEntity block) {

			base.Setup(block);

			if (block?.Block == null) {

				_isValid = false;
				return;
			
			}

			_block = block;
			_gyro = block.Block as IMyGyro;

			if (_gyro == null) {

				_isValid = false;
				return;

			}

			_grid = _gyro.SlimBlock.CubeGrid;

			if (!OwnershipVerify() || _grid.Storage == null) {

				_isValid = false;
				return;

			}

			string output = "";

			if (!_grid.Storage.TryGetValue(StorageTools.NpcGyroDataKey, out output) || !float.TryParse(output, out _multiplier)) {

				_isValid = false;
				return;

			}

			if (_multiplier == 1) {

				_isValid = false;
				return;

			}

			//Pass
			_grid.OnBlockOwnershipChanged += OwnershipChange;
			_grid.OnGridSplit += GridSplit;
			_gyro.GyroStrengthMultiplier = _multiplier;

		}

		internal void OwnershipChange(IMyCubeGrid cubeGrid) {

			if (!_block.ActiveEntity() || _grid == null || _grid.MarkedForClose || !OwnershipVerify()) {

				Unload();
				return;
			
			}

		}

		internal bool OwnershipVerify() {

			if (_grid?.BigOwners == null || _grid.BigOwners.Count == 0)
				return false;

			for (int i = 0; i < _grid.BigOwners.Count; i++) {

				var owner = _grid.BigOwners[i];
				if (FactionHelper.IsIdentityPlayer(owner))
					return false;

			}

			return true;
		
		}

		internal void GridSplit(IMyCubeGrid a, IMyCubeGrid b) {

			if (!_block.ActiveEntity() || _grid == null || _grid.MarkedForClose) {

				Unload();
				return;
			
			}

			_grid.OnGridSplit -= GridSplit;
			_grid = _gyro.SlimBlock.CubeGrid;

			if (!OwnershipVerify())
				Unload();

			if (_grid.Storage == null)
				_grid.Storage = new MyModStorageComponent();

			if (_grid.Storage.ContainsKey(StorageTools.NpcGyroDataKey)) {

				_grid.Storage[StorageTools.NpcGyroDataKey] = _multiplier.ToString();
			
			} else {

				_grid.Storage.Add(StorageTools.NpcGyroDataKey, _multiplier.ToString());

			}

		}

		internal void Unload() {

			if (_grid != null) {

				_grid.OnBlockOwnershipChanged -= OwnershipChange;
				_grid.OnGridSplit -= GridSplit;

			}

			if (_gyro != null) {

				_gyro.GyroStrengthMultiplier = 1;
			
			}

			_isValid = false;
		
		}

	}

}
