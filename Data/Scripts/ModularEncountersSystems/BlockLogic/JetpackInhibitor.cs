using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Tasks;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.ModAPI;

namespace ModularEncountersSystems.BlockLogic {
	public class JetpackInhibitor : BaseBlockLogic, IBlockLogic {

		internal IMyRadioAntenna _antenna;

		internal double _dampenerRange;
		internal double _disableRange;
		internal double _antennaRange;

		internal bool _playersInRange;
		internal List<PlayerEntity> _playersInDampenerRange;
		internal List<PlayerEntity> _playersInDisableRange;

		public JetpackInhibitor(BlockEntity block) {

			Setup(block);

		}

		internal override void Setup(BlockEntity block) {

			_tamperCheck = true;
			base.Setup(block);

			if (!_isServer) {

				_isValid = false;
				return;

			}

			_playersInDampenerRange = new List<PlayerEntity>();
			_playersInDisableRange = new List<PlayerEntity>();
			_antenna = block.Block as IMyRadioAntenna;
			_antenna.Radius = 1000;
			_logicType = "Jetpack Inhibitor";
			_useTick1 = true;
			_useTick100 = true;
			_antenna.CustomName = "[Jetpack Inhibitor Field]";
			_antenna.CustomNameChanged += NameChange;

		}

		internal void NameChange(IMyTerminalBlock block) {
		
			if(_antenna.CustomName != "[Jetpack Inhibitor Field]")
				_antenna.CustomName = "[Jetpack Inhibitor Field]";

		}

		internal override void RunTick1() {

			if (!_isWorking || !Active || !_playersInRange)
				return;

			foreach (var player in _playersInDampenerRange) {

				if (player?.Player?.Character == null || !player.ActiveEntity() || player.IsParentEntitySeat)
					continue;

				if (player.Player.Character.EnabledDamping)
					player.Player.Character.SwitchDamping();

			}

			foreach (var player in _playersInDisableRange) {

				if (player?.Player?.Character == null || !player.ActiveEntity() || player.IsParentEntitySeat)
					continue;

				if (player.Player.Character.EnabledThrusts)
					player.Player.Character.SwitchThrusts();



			}

		}

		internal override void RunTick100() {

			if (!_isWorking || !Active)
				return;

			if (_antenna.Radius != _antennaRange) {

				_antennaRange = _antenna.Radius;
				_dampenerRange = _antenna.Radius;
				_disableRange = _dampenerRange / 3;

			}

			//Check Player Distances and Status
			foreach (var player in PlayerManager.Players) {

				if (!player.ActiveEntity() || player.IsParentEntitySeat || (player.JetpackInhibitorNullifier != null && player.JetpackInhibitorNullifier.EffectActive())) {

					RemovePlayer(player);
					continue;

				}

				var distance = player.Distance(_antenna.GetPosition());

				if (distance > _dampenerRange) {

					RemovePlayer(player);
					continue;

				}

				if (distance <= _dampenerRange) {

					if (distance <= _disableRange) {

						if (!_playersInDisableRange.Contains(player)) {

							MyVisualScriptLogicProvider.ShowNotification("WARNING: Inhibitor Field Has Disabled Jetpack!", 4000, "Red", player.Player.IdentityId);
							_playersInDisableRange.Add(player);
							_playersInDampenerRange.Remove(player);

						}
					
					} else {

						if (!_playersInDampenerRange.Contains(player)) {

							MyVisualScriptLogicProvider.ShowNotification("WARNING: Inhibitor Field Has Disabled Jetpack Dampeners!", 4000, "Red", player.Player.IdentityId);
							_playersInDampenerRange.Add(player);
							_playersInDisableRange.Remove(player);

						}

					}
				
				}
			
			}

			_playersInRange = _playersInDampenerRange.Count > 0 || _playersInDisableRange.Count > 0;

		}

		internal void RemovePlayer(PlayerEntity player) {

			_playersInDampenerRange.Remove(player);
			_playersInDisableRange.Remove(player);
		
		}

		internal override void Unload(IMyEntity entity = null) {

			base.Unload(entity);

			if (_antenna != null) {

				_antenna.CustomNameChanged -= NameChange;

			}
		
		}

	}

}
