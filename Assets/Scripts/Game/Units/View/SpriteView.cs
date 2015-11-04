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
	}
}