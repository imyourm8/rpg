using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LootQuest.Game.Units.AI;

namespace LootQuest.Factory
{
	public class AIFactory : Utils.Singleton<AIFactory> 
	{
		private Utils.ObjectPool.IObjectPool<BasicAI> basicPool_;

		public AIFactory()
		{
			basicPool_ = new LootQuest.Utils.ObjectPool.ObjectPoolGeneric<BasicAI> ();
			basicPool_.Init (10, 0, false);
		}

		public Game.Units.AI.BaseAI Create(Game.Units.AI.Type type)
		{
			BaseAI ai = null;
			switch (type) 
			{
			case Type.Basic:
				ai = new BasicAI();
				break;
			}
			return ai;
		}
	}
}