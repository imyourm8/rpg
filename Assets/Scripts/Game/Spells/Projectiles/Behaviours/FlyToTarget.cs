using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Spells.Projectiles.Behaviours
{
	public class FlyToTarget : BaseBehaviour
	{
		private Units.Unit target_;

		public override void Prepare ()
		{
			base.Prepare ();

			var targets = Projectile.Spell.GetTargets ();
			if (targets.Count > 0) 
			{
				target_ = targets[0] as Units.Unit;
			}
		}

		public override void Update()
		{
			base.Update ();

			if (target_ != null) 
			{
				var dir = target_.Position - Projectile.Position;
			}
		}
	}
}