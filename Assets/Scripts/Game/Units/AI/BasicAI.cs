using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Units.AI
{
	public class BasicAI : BaseAI 
	{
		protected Unit target_;

		public override void Update ()
		{
			base.Update ();

			if (!HasTarget ()) 
			{
				FindTarget();
			}

			if (HasTarget ()) 
			{
				float dist = owner_.Game.Distance(owner_, target_);
				if (dist > owner_.Stats.GetFinValue (LootQuest.Game.Attributes.AttributeID.AttackRange))
				{
					float moveDir = target_.X - owner_.X;
					owner_.Direction = Mathf.Sign(moveDir);
					owner_.Move();
				} else
				{
					owner_.Stop();
					owner_.PerformAutoAttack();
				}
			}
		}

		protected void FindTarget()
		{
			var game = owner_.Game;
			var targets = game.GetUnitsInRadius 
				(99999.0f, owner_);
			//find nearest target on arena
			if (targets.Count > 0) 
			{
				targets.Sort(
				delegate(Units.Unit t1, Units.Unit t2)
                {
					float dist1 = game.Distance(t1, owner_);
					float dist2 = game.Distance(t2, owner_);
					return dist1.CompareTo(dist2);
				});

				target_ = targets[0];
			}
		}

		protected bool HasTarget()
		{
			return target_ != null;
		}
		
		protected void ResetTarget()
		{
			target_ = null;
		}
	}
}