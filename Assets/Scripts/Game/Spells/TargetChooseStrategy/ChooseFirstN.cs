using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LootQuest.Game.Spells.TargetChooseStrategy
{
    class ChooseFirstN : TargetStrategyDecorator
    {
        public ChooseFirstN(ITargetStrategy strat)
            : base(strat)
        {
        }

		override public List<Game.Units.Entity> FilterTargets(Game.ITargetProvider provider, ITargetFilter filter)
        {
			List<Game.Units.Entity> targets = base.FilterTargets(provider, filter);
            if (targets.Count > filter.TargetsMaxCount())
            {
                targets.RemoveRange(filter.TargetsMaxCount(), targets.Count - 1);
            }
            return targets;
        }
    }
}
