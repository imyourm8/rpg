using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LootQuest.GameData
{
	public class TowerEntry
	{
		public TowerEntry()
		{
			LevelRange = new LootQuest.Utils.Range<int> (0, 0);
		}

		public Utils.Range<int> LevelRange
		{ get; set; }

		public float EnemyHpMultiplier
		{ get; set; }

		public float EnemyDamageMultiplier
		{ get; set; } 

		public List<EnemyTableEntry> EnemyTables = new List<EnemyTableEntry>();
		public Utils.Range<int> groupCount = new LootQuest.Utils.Range<int>();
	}
}