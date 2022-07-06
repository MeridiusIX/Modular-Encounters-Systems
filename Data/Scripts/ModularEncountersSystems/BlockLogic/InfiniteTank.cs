using ModularEncountersSystems.Entities;
using ModularEncountersSystems.World;
using Sandbox.Definitions;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.BlockLogic {
	public class InfiniteTank : BaseBlockLogic, IBlockLogic {

		private IMyGasTank _gasTank;
		private MyResourceSourceComponent _source;
		private MyResourceSinkComponent _sink;
		private MyGasTankDefinition _definition;

		private bool _firstRun;
		private bool _isCargoShip;

		public InfiniteTank(BlockEntity block) {

			Setup(block);

		}

		internal override void Setup(BlockEntity block) {

			base.Setup(block);
			_gasTank = block.Block as IMyGasTank;
			_useTick100 = true;

		}

		internal override void RunTick100() {

			if (!_firstRun) {

				if (!_physicsActive)
					return;

				_firstRun = true;

				if (_gasTank == null) {

					_isValid = false;
					return;
				
				}

				var _source = _gasTank.Components.Get<MyResourceSourceComponent>();
				var _sink = _gasTank.Components.Get<MyResourceSinkComponent>();
				var _definition = _gasTank.SlimBlock.BlockDefinition as MyGasTankDefinition;

				if (_source == null || _sink == null || _definition == null) {

					_isValid = false;
					return;

				}

				Block.RefreshSubGrids();

				for (int i = 0; i < Block.LinkedGrids.Count; i++) {

					var grid = Block.LinkedGrids[i];

					if (!grid.ActiveEntity() || grid.Npc == null) {

						continue;
					
					}

					if (grid.Npc.Attributes.IsCargoShip) {

						_isCargoShip = true;
						break;

					}
				
				}

				if (!_isCargoShip) {

					_isValid = false;
					return;

				}

			}

			if (!_isNpcOwned) {

				_isValid = false;
				return;

			}
				
			if (_source.RemainingCapacityByType(_definition.StoredGasId) < _definition.Capacity / 4) {

				_source.SetRemainingCapacityByType(_definition.StoredGasId, _definition.Capacity);
				_sink.Update();

			}

		}

	}

}
