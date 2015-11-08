using UnityEngine;
using System.Collections;

namespace LootQuest.Game
{
	public class Gravity : MonoBehaviour 
	{
		[SerializeField]
		private float gravity_ = 5.0f;

		[SerializeField]
		private Vector3 startDirection_ = Vector3.one;

		[SerializeField]
		private float friction_ = 5.0f;

		private Units.Entity entity_;
		public Units.Entity entity 
		{
			set { entity_ = value; }
		}

		void FixedUpdate()
		{
			if (entity_ != null) 
			{
				Vector2 pos = entity_.Position;
				pos.y += gravity_ * Time.fixedDeltaTime;
				pos.x += friction_ * Time.fixedDeltaTime;

				entity_.Position = pos;
			}
		}
	}
}