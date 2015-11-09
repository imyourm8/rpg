using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Units.View
{
	public class UnitView : MonoBehaviour
	{
		[SerializeField]
		private string[] attackAnimations;

		[SerializeField]
		private string moveAnimation;

		public static implicit operator GameObject(UnitView view)
		{
			return view.gameObject;
		}

		protected virtual void OnAttack()
		{}

		protected virtual void OnDeath()
		{}

		public virtual void Reset()
		{}

		public virtual Vector2 GetProjectileSpawnPoint()
		{
			return Vector2.zero;
		}

		public virtual Vector2 GetCenter()
		{
			return Vector2.zero;
		}
	}
}