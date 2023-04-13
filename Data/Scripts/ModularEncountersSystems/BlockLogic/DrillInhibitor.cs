using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Tasks;
using Sandbox.Game;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Weapons;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.BlockLogic {
	public class DrillInhibitor : InhibitorLogic, IBlockLogic {

		internal Dictionary<PlayerEntity, DateTime> _playersInDisableRange;
		internal Dictionary<PlayerEntity, int> _toolbarIndexes;

		public DrillInhibitor(BlockEntity block) {

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

			_playersInDisableRange = new Dictionary<PlayerEntity, DateTime>();
			_toolbarIndexes = new Dictionary<PlayerEntity, int>();
			

			if (_antenna != null) {

				_antenna.Radius = 500;
				_antenna.CustomName = "[HandDrill Inhibitor Field]";
				_antenna.CustomNameChanged += NameChange;

			} else {

				_disableRange = 500;
				_antennaRange = 500;

			}

			_logicType = "HandDrill Inhibitor";
			_inhibitor = InhibitorTypes.Drill;

			_useTick10 = true;
			_useTick100 = true;
			
			MyVisualScriptLogicProvider.ToolbarItemChanged += ToolbarItemChanged;

		}

		internal void NameChange(IMyTerminalBlock block) {
		
			if(_antenna.CustomName != "[Hand Drill Inhibitor Field]")
				_antenna.CustomName = "[Hand Drill Inhibitor Field]";

		}

		internal override void RunTick10() {

			if (!_isWorking || !Active || !_playersInRange)
				return;

			ToolEquipped();

		}

		internal override void RunTick100() {

			if (!_isWorking || !Active)
				return;

			if (_antenna != null && _antenna.Radius != _antennaRange) {

				_antennaRange = _antenna.Radius;
				_disableRange = _antenna.Radius;

			}

			//Check Player Distances and Status
			foreach (var player in PlayerManager.Players) {

				if (!player.ActiveEntity() || (player.DrillInhibitorNullifier != null && player.DrillInhibitorNullifier.EffectActive())) {

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

				if (distance > _disableRange) {

					RemovePlayer(player);
					continue;

				}

				if (distance <= _disableRange) {

					if (!ProcessInhibitorSuitUpgrades(player)) {

						if (!_playersInDisableRange.ContainsKey(player)) {

							MyVisualScriptLogicProvider.ShowNotification("WARNING: Inhibitor Field Has Disabled Hand Drills!", 4000, "Red", player.Player.IdentityId);
							_playersInDisableRange.Add(player, MyAPIGateway.Session.GameDateTime);
							player.AddInhibitorToPlayer(_antenna, _inhibitor);
							_toolbarIndexes.Add(player, 0);

						}

					}

				}
			
			}

			_playersInRange = _playersInDisableRange.Count > 0;

		}

		void ToolbarItemChanged(long entityId, string typeId, string subtypeId, int page, int slot) {

			foreach (var player in _playersInDisableRange.Keys) {

				if (!player.ActiveEntity() || player.IsParentEntitySeat)
					continue;


			
			}

		}

		public void ToolEquipped() {

			foreach (var player in _playersInDisableRange.Keys) {

				if (!player.ActiveEntity() || !player.IsPlayerStandingCharacter())
					continue;

				if (player.Player?.Character?.EquippedTool == null) {

					continue;
				
				}

				var timeSpan = MyAPIGateway.Session.GameDateTime - _playersInDisableRange[player];

				if (timeSpan.TotalMilliseconds < 250) {

					//MyVisualScriptLogicProvider.ShowNotificationLocal("Toolbar Change Timer", 4000, "White");
					continue;

				}

				var drill = player.Player.Character.EquippedTool as IMyHandDrill;

				if (drill == null) {

					continue;

				}

				if (drill.IsShooting) {

					MyVisualScriptLogicProvider.SetPlayersHealth(player.Player.IdentityId, MyVisualScriptLogicProvider.GetPlayersHealth(player.Player.IdentityId) - 6);

				}

				_toolbarIndexes[player]++;

				if (_toolbarIndexes[player] > 8)
					_toolbarIndexes[player] = 0;

				MyVisualScriptLogicProvider.SwitchToolbarToSlot(_toolbarIndexes[player], player.Player.IdentityId);

			}

		}

		internal void RemovePlayer(PlayerEntity player) {

			_playersInDisableRange.Remove(player);
			_toolbarIndexes.Remove(player);
			player.RemoveInhibitorFromPlayer(_antenna, _inhibitor);

		}

		internal override void Unload(IMyEntity entity = null) {

			base.Unload(entity);

			MyVisualScriptLogicProvider.ToolbarItemChanged -= ToolbarItemChanged;

			if (_antenna != null) {

				_antenna.CustomNameChanged -= NameChange;

			}
		
		}

	}

}
