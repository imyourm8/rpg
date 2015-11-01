using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LootQuest.Game.Spells.TargetChooseStrategy
{
    class TargetStrategyDecorator : ITargetStrategy
    {
        private ITargetStrategy strategy_;
        public TargetStrategyDecorator(ITargetStrategy strat)
        {
            strategy_ = strat;
        }

        virtual public List<Game.Units.Entity> FilterTargets(Game.ITargetProvider provider, ITargetFilter filter)
        {
            return strategy_.FilterTargets(provider, filter);
        }
    }
}
