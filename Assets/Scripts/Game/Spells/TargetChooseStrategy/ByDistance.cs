using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LootQuest.Game.Spells.TargetChooseStrategy
{
    class ByDistance : TargetStrategyDecorator
    {
        public ByDistance(ITargetStrategy strat)
            : base(strat)
        {
        }

		override public List<Game.Units.Entity> FilterTargets(Game.ITargetProvider provider, ITargetFilter filter)
        {
			List<Game.Units.Entity> targets = base.FilterTargets(provider, filter);
			targets.Sort((Game.Units.Entity t1, Game.Units.Entity t2) => 
            {
                float t1Dist = filter.DistanceTo(t1);
                float t2Dist = filter.DistanceTo(t2);
                return t1Dist.CompareTo(t2Dist);
            });
            return targets;
        }
    }
}
