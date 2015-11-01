using UnityEngine;
using System.Collections;

namespace LootQuest.GameData
{
	public class TowerEntry
	{
		public TowerEntry()
		{
			LevelRange = new LootQuest.Utils.Range<long> (0, 0);
		}

		public Utils.Range<long> LevelRange
		{ get; set; }

		public float EnemyHpMultiplier
		{ get; set; }

		public float EnemyDamageMultiplier
		{ get; set; } 
	}
}