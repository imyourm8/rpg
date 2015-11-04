using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Land
{
	public class BaseLand
	{
		private GameObject root_;

		public void Init(GameObject landContainer)
		{
			root_ = landContainer;
		}

		public void Add(Units.Entity entity)
		{
			root_.AddChild (entity.View);
		}

		public void Remove(Units.Entity entity)
		{
			(entity.View as GameObject).Detach ();
		}
	}
}