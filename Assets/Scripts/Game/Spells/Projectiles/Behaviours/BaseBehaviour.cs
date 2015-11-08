using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Spells.Projectiles.Behaviours
{
	public class BaseBehaviour : BehaviorDecorator
	{
		private Projectile projectile_;
		public Projectile Projectile
		{
			set { projectile_ = value; }
			protected get { return projectile_; }
		}
	}
}