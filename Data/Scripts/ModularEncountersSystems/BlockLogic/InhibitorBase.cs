using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Progression;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.BlockLogic {

	public enum InhibitorTypes {
	
		None,
		Jetpack,
		Personnel,
		Energy,
		Drill,
		JumpDrive,
		Nanobots,
	
	}

	public abstract class InhibitorBase : BaseBlockLogic {

		internal InhibitorTypes _inhibitor;

		internal double _disableRange;
		internal double _antennaRange;

		internal IMyRadioAntenna _antenna;
		internal IMyTerminalBlock _block;

		internal bool _playersInRange;
		
		internal void BaseSetup(BlockEntity block) {

			
			_antenna = block.Block as IMyRadioAntenna;
			_block = block.Block as IMyTerminalBlock;

		}

		internal bool HasInhibitorNullifierEffect(PlayerEntity player) {

			if (_inhibitor == InhibitorTypes.Jetpack && (player.JetpackInhibitorNullifier?.EffectActive() ?? false))
				return true;

			if (_inhibitor == InhibitorTypes.Drill && (player.DrillInhibitorNullifier?.EffectActive() ?? false))
				return true;

			if (_inhibitor == InhibitorTypes.Personnel && (player.PlayerInhibitorNullifier?.EffectActive() ?? false))
				return true;

			if (_inhibitor == InhibitorTypes.Energy && (player.EnergyInhibitorNullifier?.EffectActive() ?? false))
				return true;

			return false;
		}

		internal bool ProcessInhibitorSuitUpgrades(PlayerEntity player) {

			var energy = MyVisualScriptLogicProvider.GetPlayersEnergyLevel(player.Player.IdentityId);

			if (energy < 0.5f)
				return false;

			if (_inhibitor == InhibitorTypes.Jetpack && (player.Progression?.JetpackInhibitorSuitUpgradeLevel ?? 0) > 0 && ProgressionContainer.IsUpgradeAllowedInConfig(SuitUpgradeTypes.JetpackInhibitor)) {

				ApplyInhibitorSuitUpgradeEffect(player, energy, player.Progression.JetpackInhibitorSuitUpgradeLevel);
				return true;

			}
				
			if (_inhibitor == InhibitorTypes.Drill && (player.Progression?.DrillInhibitorSuitUpgradeLevel ?? 0) > 0 && ProgressionContainer.IsUpgradeAllowedInConfig(SuitUpgradeTypes.HandDrillInhibitor)) {

				ApplyInhibitorSuitUpgradeEffect(player, energy, player.Progression.DrillInhibitorSuitUpgradeLevel);
				return true;

			}

			if (_inhibitor == InhibitorTypes.Energy && (player.Progression?.EnergyInhibitorSuitUpgradeLevel ?? 0) > 0 && ProgressionContainer.IsUpgradeAllowedInConfig(SuitUpgradeTypes.PersonnelInhibitor)) {

				ApplyInhibitorSuitUpgradeEffect(player, energy, player.Progression.EnergyInhibitorSuitUpgradeLevel);
				return true;

			}

			if (_inhibitor == InhibitorTypes.Personnel && (player.Progression?.PersonnelInhibitorSuitUpgradeLevel ?? 0) > 0 && ProgressionContainer.IsUpgradeAllowedInConfig(SuitUpgradeTypes.EnergyInhibitor)) {

				ApplyInhibitorSuitUpgradeEffect(player, energy, player.Progression.PersonnelInhibitorSuitUpgradeLevel);
				return true;

			}

			return false;

		}

		internal void ApplyInhibitorSuitUpgradeEffect(PlayerEntity player, float energy, byte level) {

			var suitEnergy = energy;
			var reduction = 0.2f * (float)level;
			var amount = (1 - reduction);
			suitEnergy -= (amount / 100);

			if (suitEnergy > 0)
				MyVisualScriptLogicProvider.SetPlayersEnergyLevel(player.Player.IdentityId, suitEnergy);

		}

		internal void SetInhibitorRange() {
		
			
		
		}

	}

}
