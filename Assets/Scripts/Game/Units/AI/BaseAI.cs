using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Units.AI
{
	public enum Type
	{
		Basic
	}

	public class BaseAI
	{
		protected Unit owner_;

		public Unit Owner
		{
			set { owner_ = value; }
		}

		public virtual void Update()
		{}

		public virtual void OnDeath()
		{}
	}
}