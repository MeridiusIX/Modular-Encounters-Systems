using ModularEncountersSystems.Core;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.World;
using Sandbox.Definitions;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.BlockLogic
{
    public class InfiniteTank : BaseBlockLogic, IBlockLogic
    {

        private IMyGasTank _gasTank;
        private MyResourceSourceComponent _source;
        private MyResourceSinkComponent _sink;
        private MyGasTankDefinition _definition;

        private bool _firstRun;
        private bool _isActive;

        public InfiniteTank(BlockEntity block)
        {
            Setup(block);
        }

        internal override void Setup(BlockEntity block)
        {
            base.Setup(block);
            _gasTank = block.Block as IMyGasTank;
            _useTick100 = true;
        }

        internal override void RunTick100()
        {
            if (!_firstRun)
            {
                if (!_physicsActive || !MES_SessionCore.IsServer)
                    return;

                _firstRun = true;

                if (_gasTank == null)
                {
                    _isValid = false;
                    return;
                }

                Block.RefreshSubGrids();

                for (int i = 0; i < Block.LinkedGrids.Count; i++)
                {
                    var grid = Block.LinkedGrids[i];

                    if (grid.ActiveEntity() && grid.Npc != null)
                    {
                        _isActive = true;
                        break;
                    }
                }

                if (!_isActive)
                {
                    _isValid = false;
                    return;
                }

                var disable = true;
                for (int i = 0; i < Block.LinkedGrids.Count; i++)
                {
                    var grid = Block.LinkedGrids[i];

                    if (!grid.ActiveEntity() || grid.Npc == null)
                        continue;

                    if (grid.Npc.Attributes.ReplenishSystems)
                    {
                        disable = false;
                        break;
                    }
                }

                if (disable)
                {
                    _isValid = false;
                    return;
                }

            }

            if (!FactionHelper.IsIdentityNPC(_gasTank.OwnerId))
            {
                _isValid = false;
                return;
            }

            if (_gasTank.FilledRatio <= .25f)
            {
                _gasTank.ChangeFilledRatio(1, true);
            }

        }

    }

}
