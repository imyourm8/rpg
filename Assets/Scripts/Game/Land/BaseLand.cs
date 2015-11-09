using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Land
{
	public class BaseLand
	{
		protected Camera camera_;
		protected GameObject root_;

		public void Init(GameObject landContainer, Camera camera)
		{
			root_ = landContainer;
			camera_ = camera;
		}

		public virtual void Prepare()
		{}

		public void Add(Units.Entity entity)
		{
			root_.AddChild (entity.View);
		}

		public void Remove(Units.Entity entity)
		{
			(entity.View as GameObject).Detach ();
		}

		public virtual void Update()
		{

		}
	}
}