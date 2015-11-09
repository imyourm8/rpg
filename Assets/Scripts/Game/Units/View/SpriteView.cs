using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Units.View
{
	public class SpriteView : UnitView 
	{
		[SerializeField]
		private SpriteRenderer sprite_;

		[SerializeField]
		private Animator animator_;

		[SerializeField]
		private GameObject projectileSpawnPoint_;

		[SerializeField]
		private GameObject centerPoint_;

		public override Vector2 GetProjectileSpawnPoint ()
		{
			var pos = projectileSpawnPoint_.transform.localPosition;
			pos.Scale (gameObject.transform.localScale);
			return new Vector2(pos.x, pos.y);
		}

		public override Vector2 GetCenter ()
		{
			var pos = centerPoint_.transform.localPosition;
			pos.Scale (gameObject.transform.localScale);
			return new Vector2(pos.x, pos.y);
		}
	}
}