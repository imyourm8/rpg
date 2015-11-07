using UnityEngine;
using System.Collections;

using LootQuest.Game.Units;

public static class UnitExtensions
{
	private static LootQuest.Utils.ObjectPool.IObjectPool<Enemy> pool_ = new LootQuest.Utils.ObjectPool.ObjectPoolGeneric<Enemy>();

	public static void InitPool()
	{
		pool_.Init (10, 0, false);
	}

	public static Enemy CreateEnemy()
	{
		return pool_.Get();
	}
	
	public static void Return(this Enemy enemy)
	{
		pool_.Return (enemy);
		enemy.View.gameObject.ReturnPooled ();
	}
}
