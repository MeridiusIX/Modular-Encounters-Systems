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
	public class ProximityExplosiveModule : BaseBlockLogic, IBlockLogic {

		internal IMyWarhead _warhead;

		internal double _antennaRange;

		internal bool _playersInRange;
		internal List<PlayerEntity> _playersInBlockRange;

		internal float _damageAtZeroDistance = 25;

		public ProximityExplosiveModule(BlockEntity block) {

			Setup(block);

		}

		internal override void Setup(BlockEntity block) {

			base.Setup(block);

			if (!_isServer) {

				_isValid = false;
				return;

			}

			_playersInBlockRange = new List<PlayerEntity>();
			_warhead = block.Block as IMyWarhead;
			_logicType = "Proximity Explosive Module";
			_useTick60 = true;
			_useTick100 = true;
			_warhead.CustomName = "[Proximity Explosive Module]";
			_warhead.CustomNameChanged += NameChange;

		}

		internal void NameChange(IMyTerminalBlock block) {
		
			if(_warhead.CustomName != "[Proximity Explosive Module]")
				_warhead.CustomName = "[Proximity Explosive Module]";

		}

		internal override void RunTick60() {

			if (!_isWorking || !Active || !_playersInRange)
				return;

			foreach (var player in _playersInBlockRange) {

				if (!player.ActiveEntity() || player.IsParentEntitySeat)
					continue;

				float distanceRatio = 1 - (float)(Vector3D.Distance(player.GetPosition(), _warhead.GetPosition()) / _antennaRange);

				player.Player.Character.DoDamage(_damageAtZeroDistance * distanceRatio, MyStringHash.GetOrCompute("Radiation"), true, null, _warhead.EntityId);

			}

		}

		internal override void RunTick100() {

			if (!_isWorking || !Active)
				return;

			//Check Player Distances and Status
			foreach (var player in PlayerManager.Players) {

				if (!player.ActiveEntity() || player.IsParentEntitySeat || (player.PlayerInhibitorNullifier != null && player.PlayerInhibitorNullifier.EffectActive())) {

					RemovePlayer(player);
					continue;

				}

				var distance = player.Distance(_warhead.GetPosition());

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

			if (_warhead != null) {

				_warhead.CustomNameChanged -= NameChange;

			}
		
		}

	}

}
