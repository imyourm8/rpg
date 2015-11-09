using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Spells.Projectiles.Behaviours
{
	public class BehaviorDecorator : IBaseBehaviour 
	{
		private IBaseBehaviour behaviour_;

		public IBaseBehaviour Behaviour
		{
			set { behaviour_ = value; }
			protected get { return behaviour_; }
		}

		private Projectile projectile_;
		public Projectile Projectile
		{
			set 
			{ 
				projectile_ = value; 
				if (behaviour_ != null)
					behaviour_.Projectile = value;
			}
			protected get { return projectile_; }
		}

		public virtual void Prepare()
		{
			if (behaviour_ != null)
				behaviour_.Prepare ();
		}
		
		public virtual void Update()
		{
			if (behaviour_ != null)
				behaviour_.Update ();
		}
	}
}