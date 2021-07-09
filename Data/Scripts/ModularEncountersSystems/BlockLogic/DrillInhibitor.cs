using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Tasks;
using Sandbox.Game;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Weapons;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.ModAPI;

namespace ModularEncountersSystems.BlockLogic {
	public class DrillInhibitor : BaseBlockLogic, IBlockLogic {

		internal IMyRadioAntenna _antenna;

		internal double _disableRange;
		internal double _antennaRange;

		internal bool _playersInRange;
		internal Dictionary<PlayerEntity, DateTime> _playersInDisableRange;
		internal Dictionary<PlayerEntity, int> _toolbarIndexes;

		public DrillInhibitor(BlockEntity block) {

			Setup(block);

		}

		internal override void Setup(BlockEntity block) {

			base.Setup(block);

			if (!_isServer) {

				_isValid = false;
				return;
			
			}

			_playersInDisableRange = new Dictionary<PlayerEntity, DateTime>();
			_toolbarIndexes = new Dictionary<PlayerEntity, int>();
			_antenna = block.Block as IMyRadioAntenna;
			_antenna.Radius = 500;
			_logicType = "HandDrill Inhibitor";
			_useTick10 = true;
			_useTick100 = true;
			_antenna.CustomName = "[HandDrill Inhibitor Field]";
			_antenna.CustomNameChanged += NameChange;
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

			if (_antenna.Radius != _antennaRange) {

				_antennaRange = _antenna.Radius;
				_disableRange = _antenna.Radius;

			}

			//Check Player Distances and Status
			foreach (var player in PlayerManager.Players) {

				if (!player.ActiveEntity() || player.IsParentEntitySeat || player.JetpackInhibitorNullifierActive) {

					RemovePlayer(player);
					continue;

				}

				var distance = player.Distance(_antenna.GetPosition());

				if (distance > _disableRange) {

					RemovePlayer(player);
					continue;

				}

				if (distance <= _disableRange) {

					if (!_playersInDisableRange.ContainsKey(player)) {

						MyVisualScriptLogicProvider.ShowNotification("WARNING: Inhibitor Field Has Disabled Hand Drills!", 4000, "Red", player.Player.IdentityId);
						_playersInDisableRange.Add(player, MyAPIGateway.Session.GameDateTime);
						_toolbarIndexes.Add(player, 0);

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

				if (!player.ActiveEntity() || player.IsParentEntitySeat)
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

					MyVisualScriptLogicProvider.SetPlayersHealth(MyAPIGateway.Session.LocalHumanPlayer.IdentityId, MyVisualScriptLogicProvider.GetPlayersHealth(MyAPIGateway.Session.LocalHumanPlayer.IdentityId) - 6);

				}

				_toolbarIndexes[player]++;

				if (_toolbarIndexes[player] > 8)
					_toolbarIndexes[player] = 0;

				MyVisualScriptLogicProvider.SwitchToolbarToSlot(_toolbarIndexes[player], MyAPIGateway.Session.LocalHumanPlayer.IdentityId);

			}

		}

		internal void RemovePlayer(PlayerEntity player) {

			_playersInDisableRange.Remove(player);
			_toolbarIndexes.Remove(player);
		
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
