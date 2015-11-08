using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Spells.Projectiles.Behaviours
{
	public class StraightFly : BaseBehaviour
	{
		public override void Prepare ()
		{
			base.Prepare ();

			//take direction from projectile's owner
			Projectile.Direction = Projectile.Spell.Caster.Direction;
		}

		public override void Update()
		{
			base.Update ();
		}
	}
}