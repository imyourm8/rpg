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
			float endPoint = caster_.X + caster_.Direction.x * caster_.Stats.GetFinValue (LootQuest.Game.Attributes.AttributeID.AttackRange);
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
				return !(startPoint <= obj.X && endPoint >= obj.X);
			});

			ApplyEffectsOnTargets();
		}

		public void HandleRangeCast()
		{
			//create projectile object
			var projectile = Projectiles.ProjectileExtensions.Create (data_);
			projectile.Spell = this;
			projectile.X = caster_.X;

			caster_.Game.Add (projectile);
		}
	}
}