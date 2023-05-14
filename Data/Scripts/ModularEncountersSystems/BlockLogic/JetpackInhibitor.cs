using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Tasks;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.BlockLogic {
	public class JetpackInhibitor : InhibitorLogic, IBlockLogic {

		internal double _dampenerRange;

		internal List<PlayerEntity> _playersInDampenerRange;
		internal List<PlayerEntity> _playersInDisableRange;

		public JetpackInhibitor(BlockEntity block) {

			Setup(block);

		}

		internal override void Setup(BlockEntity block) {

			_fixCheck = true;
			base.Setup(block);
			

			if (!_isServer) {

				_isValid = false;
				return;

			}

			BaseSetup(block);
			_playersInDampenerRange = new List<PlayerEntity>();
			_playersInDisableRange = new List<PlayerEntity>();

			if (_antenna != null) {

				_antenna.Radius = 800;
				_antenna.CustomName = "[Jetpack Inhibitor Field]";
				_antenna.CustomNameChanged += NameChange;

			} else {

				_antennaRange = 800;
				_dampenerRange = 800;
				_disableRange = _dampenerRange / 3;


			}

			_logicType = "Jetpack Inhibitor";
			_inhibitor = InhibitorTypes.Jetpack;
			_useTick1 = true;
			_useTick100 = true;

		}

		internal void NameChange(IMyTerminalBlock block) {
		
			if(_antenna.CustomName != "[Jetpack Inhibitor Field]")
				_antenna.CustomName = "[Jetpack Inhibitor Field]";

		}

		internal override void RunTick1() {

			if (!_isWorking || !Active || !_playersInRange)
				return;

			foreach (var player in _playersInDampenerRange) {

				if (!player.IsPlayerStandingCharacter())
					continue;

				if (player.Player.Character.EnabledDamping)
					player.Player.Character.SwitchDamping();

			}

			foreach (var player in _playersInDisableRange) {

				if (!player.IsPlayerStandingCharacter())
					continue;

				if (player.Player.Character.EnabledThrusts)
					player.Player.Character.SwitchThrusts();



			}

		}

		internal override void RunTick100() {

			if (!_isWorking || !Active)
				return;

			if (_antenna != null && _antenna.Radius != _antennaRange) {

				_antennaRange = _antenna.Radius;
				_dampenerRange = _antenna.Radius;
				_disableRange = _dampenerRange / 3;

			}

			//Check Player Distances and Status
			foreach (var player in PlayerManager.Players) {

				if (!player.ActiveEntity() || (player.JetpackInhibitorNullifier != null && player.JetpackInhibitorNullifier.EffectActive())) {

					RemovePlayer(player);
					continue;

				}

				var characterPos = player.GetCharacterPosition();

				if (!player.IsPlayerStandingCharacter()) {

					RemovePlayer(player);
					continue;

				} else if (characterPos == Vector3D.Zero) {

					characterPos = player.GetPosition();

				}

				var distance = Vector3D.Distance(Entity.GetPosition(), characterPos);

				if (distance > _dampenerRange) {

					RemovePlayer(player);
					continue;

				}

				if (distance <= _dampenerRange) {

					if (!ProcessInhibitorSuitUpgrades(player)) {

						if (distance <= _disableRange) {

							if (!_playersInDisableRange.Contains(player)) {

								MyVisualScriptLogicProvider.ShowNotification("WARNING: Inhibitor Field Has Disabled Jetpack!", 4000, "Red", player.Player.IdentityId);
								_playersInDisableRange.Add(player);
								_playersInDampenerRange.Remove(player);
								player.AddInhibitorToPlayer(_antenna, _inhibitor);

							}

						} else {

							if (!_playersInDampenerRange.Contains(player)) {

								MyVisualScriptLogicProvider.ShowNotification("WARNING: Inhibitor Field Has Disabled Jetpack Dampeners!", 4000, "Red", player.Player.IdentityId);
								_playersInDampenerRange.Add(player);
								_playersInDisableRange.Remove(player);
								player.AddInhibitorToPlayer(_antenna, _inhibitor);

							}

						}

					}

				}
			
			}

			_playersInRange = _playersInDampenerRange.Count > 0 || _playersInDisableRange.Count > 0;

		}

		internal void RemovePlayer(PlayerEntity player) {

			_playersInDampenerRange.Remove(player);
			_playersInDisableRange.Remove(player);
			player.RemoveInhibitorFromPlayer(_antenna, _inhibitor);

		}

		internal override void Unload(IMyEntity entity = null) {

			base.Unload(entity);

			if (_antenna != null) {

				_antenna.CustomNameChanged -= NameChange;

			}
		
		}

	}

}
