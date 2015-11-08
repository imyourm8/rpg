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
		}

		public virtual void Prepare()
		{
			behaviour_.Prepare ();
		}
		
		public virtual void Update()
		{
			behaviour_.Update ();
		}
	}
}