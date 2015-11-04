using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LootQuest.Game.Spells.TargetChooseStrategy
{
    interface ITargetFilter
    {
		bool Filter(Game.Units.Entity target);
		float DistanceTo(Game.Units.Entity target);
        int TargetsMaxCount();
    }

    interface ITargetStrategy
    {
		List<Game.Units.Entity> FilterTargets(Game.ITargetProvider provider, ITargetFilter filter);
    }
}
