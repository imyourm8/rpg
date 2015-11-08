using UnityEngine;
using System.Collections;

using LootQuest;
using LootQuest.Game.Units;

namespace LootQuest.Game.Units
{
	public static class UnitExtensions
	{
		private static Utils.ObjectPool.ObjectPoolGeneric<Enemy> enemies_ = new Utils.ObjectPool.ObjectPoolGeneric<Enemy> ();

		public static void Init()
		{
			enemies_.Init (10, 0, false);
		}

		public static Enemy CreateEnemy(GameData.EnemyEntry entry)
		{
			var enemy = enemies_.Get ();
			enemy.Init (entry);
			return enemy;
		}
		
		public static void Return(this Enemy enemy)
		{
			enemies_.Return (enemy);
			enemy.View.gameObject.ReturnPooled ();
		}
}
}