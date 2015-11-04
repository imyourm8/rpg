using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LootQuest.Game.Spells.TargetChooseStrategy
{
    class TargetStrategy : ITargetStrategy
    {
        public TargetStrategy()
        {
        }

		public List<Game.Units.Entity> FilterTargets(Game.ITargetProvider provider, ITargetFilter filter)
        {
			List<Game.Units.Entity> targets = new List<Game.Units.Entity>();
            provider.InitTargets();
			Game.Units.Entity target;
            while (provider.NextTarget(out target))
            {
                if (filter.Filter(target))
                {
                    targets.Add(target);
                }
            }
            return targets;
        }
    }
}
