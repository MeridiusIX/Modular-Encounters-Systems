using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Tasks;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace ModularEncountersSystems.BlockLogic {
	public class PlayerInhibitor : BaseBlockLogic, IBlockLogic {

		internal IMyRadioAntenna _antenna;

		internal double _antennaRange;

		internal bool _playersInRange;
		internal List<PlayerEntity> _playersInBlockRange;

		internal float _damageAtZeroDistance = 25;

		public PlayerInhibitor(BlockEntity block) {

			Setup(block);

		}

		internal override void Setup(BlockEntity block) {

			_tamperCheck = true;
			base.Setup(block);

			if (!_isServer) {

				_isValid = false;
				return;

			}

			_playersInBlockRange = new List<PlayerEntity>();
			_antenna = block.Block as IMyRadioAntenna;
			_antenna.Radius = 1000;
			_logicType = "Player Inhibitor";
			_useTick60 = true;
			_useTick100 = true;
			_antenna.CustomName = "[Player Inhibitor Field]";
			_antenna.CustomNameChanged += NameChange;

		}

		internal void NameChange(IMyTerminalBlock block) {
		
			if(_antenna.CustomName != "[Player Inhibitor Field]")
				_antenna.CustomName = "[Player Inhibitor Field]";

		}

		internal override void RunTick60() {

			if (!_isWorking || !Active || !_playersInRange)
				return;

			foreach (var player in _playersInBlockRange) {

				if (!player.ActiveEntity() || player.IsParentEntitySeat)
					continue;

				float distanceRatio = 1 - (float)(Vector3D.Distance(player.GetPosition(), _antenna.GetPosition()) / _antennaRange);

				player.Player.Character.DoDamage(_damageAtZeroDistance * distanceRatio, MyStringHash.GetOrCompute("Radiation"), true, null, _antenna.EntityId);

			}

		}

		internal override void RunTick100() {

			if (!_isWorking || !Active)
				return;

			if (_antenna.Radius != _antennaRange) {

				_antennaRange = _antenna.Radius;

			}

			//Check Player Distances and Status
			foreach (var player in PlayerManager.Players) {

				if (!player.ActiveEntity() || player.IsParentEntitySeat || (player.PlayerInhibitorNullifier != null && player.PlayerInhibitorNullifier.EffectActive())) {

					RemovePlayer(player);
					continue;

				}

				var distance = player.Distance(_antenna.GetPosition());

				if (distance > _antennaRange) {

					RemovePlayer(player);
					continue;

				}

				if (distance <= _antennaRange) {

					if (!_playersInBlockRange.Contains(player)) {

						MyVisualScriptLogicProvider.ShowNotification("WARNING: Prolonged Exposure To Player Inhibitor Field May Be Fatal!", 5000, "Red", player.Player.IdentityId);
						_playersInBlockRange.Add(player);

					}

				}
			
			}

			_playersInRange = _playersInBlockRange.Count > 0;

		}

		internal void RemovePlayer(PlayerEntity player) {

			_playersInBlockRange.Remove(player);
		
		}

		internal override void Unload(IMyEntity entity = null) {

			base.Unload(entity);

			if (_antenna != null) {

				_antenna.CustomNameChanged -= NameChange;

			}
		
		}

	}

}
