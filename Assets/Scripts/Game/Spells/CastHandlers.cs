using UnityEngine;

using System.Linq;
using System.Collections;

namespace LootQuest.Game.Spells
{
	public partial class Spell
	{
		public void HandleMeleeCast()
		{
			//spawn arc
			float endPoint = caster_.X + caster_.Direction * caster_.Stats.GetFinValue (LootQuest.Game.Attributes.AttributeID.AttackRange);
			float startPoint = caster_.X;

			if (endPoint < startPoint) 
			{
				float t = endPoint;
				endPoint = startPoint;
				startPoint = t;
			}

			targets_.RemoveAll (
			delegate(LootQuest.Game.Units.Entity obj) 
			{
				return startPoint <= obj.X && endPoint >= obj.X;
			});

			ApplyEffectsOnTargets();
		}
	}
}