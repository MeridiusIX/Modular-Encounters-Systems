using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Progression;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Tasks {
	public class PlayerSolarModule : TaskItem, ITaskItem {

		internal PlayerEntity _player;
		internal PlanetEntity _currentPlanet;
		internal List<IHitInfo> _rayHits;
		internal byte _ticks;

		public PlayerSolarModule(PlayerEntity player) {

			_player = player;
			_rayHits = new List<IHitInfo>();
			_tickTrigger = 100;
			_tickCounter = TaskProcessor.TickIncrement;

			if (_tickCounter > 200)
				_tickCounter -= 200;
			else if(_tickCounter > 100)
				_tickCounter -= 100;

		}

		public override void Run() {

			if (!_player.ActiveEntity() || _player.Player?.Character == null || _player.Player.Character.IsDead || !_player.IsPlayerStandingCharacter())
				return;

			if (_player.Progression.SolarChargingSuitUpgradeLevel == 0 || !ProgressionContainer.IsUpgradeAllowedInConfig(SuitUpgradeTypes.SolarCharging))
				return;

			/*
			if (_player.Player.Character.OxygenLevel > 0) {

				MyVisualScriptLogicProvider.ShowNotificationToAll("In Pressurized Area", 1000, "Red");
				return;
			
			}
			*/

			if (PlanetManager.InGravity(_player.GetPosition())) {

				_currentPlanet = PlanetManager.GetNearestPlanet(_player.GetPosition());

				if (_currentPlanet?.Planet != null && !_currentPlanet.Planet.MarkedForClose) {

					if (MyVisualScriptLogicProvider.IsOnDarkSide(_currentPlanet.Planet, _player.GetPosition())) {

						return;
					
					}
				
				}
			
			}

			var sunDir = MyVisualScriptLogicProvider.GetSunDirection();
			_rayHits.Clear();
			MyAPIGateway.Physics.CastRay(_player.GetCharacterPosition(), sunDir * 800 + _player.GetCharacterPosition(), _rayHits);

			bool clear = true;

			foreach (var hit in _rayHits) {

				if (hit.HitEntity == _player.Player.Character || hit.HitEntity == _player.Player.Character.EquippedTool)
					continue;

				clear = false;
				break;
			
			}

			if (!clear)
				return;

			var newEnergy = MyVisualScriptLogicProvider.GetPlayersEnergyLevel(_player.Player.IdentityId) + _player.Progression.GetSolarEnergyIncrease();

			if (newEnergy > 1)
				newEnergy = 1;

			MyVisualScriptLogicProvider.SetPlayersEnergyLevel(_player.Player.IdentityId, newEnergy);

		}

	}
}
