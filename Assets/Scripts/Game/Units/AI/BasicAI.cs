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
					owner_.Direction = target_.Position - owner_.Position;
					owner_.Move();

					ResetTarget();
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
			var targets = game.GetUnitsInRadius(99999.0f, owner_);
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

				foreach(var target in targets)
				{
					if (owner_.CanAttack(target))
					{
						target_ = target;
						break;
					}
				}
			}
		}

		protected bool HasTarget()
		{
			return target_ != null && target_.IsAlive();
		}
		
		protected void ResetTarget()
		{
			target_ = null;
		}
	}
}