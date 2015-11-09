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
				var distToTarget = Projectile.Game.Distance(Projectile, target_);
				var nextStep = Projectile.Stats.GetFinValue(LootQuest.Game.Attributes.AttributeID.MovementSpeed) * Time.deltaTime;

				if (nextStep < distToTarget)
				{
					var targetPosition = target_.Center();
					var dir = targetPosition - Projectile.Position;
					dir.Normalize();
					Projectile.Direction = dir;
				} else 
				{
					Projectile.Position = target_.Center();
				}
			}
		}
	}
}