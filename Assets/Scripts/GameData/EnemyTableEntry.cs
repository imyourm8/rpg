using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace LootQuest.GameData
{
	public class EnemyTableSpawnChanceEntry
	{
		public int weight;
		public LootQuest.Game.Units.EnemyType enemyType;

		public bool expanded = false;
		public int nameIndex = 0;

		public EnemyTableSpawnChanceEntry(LootQuest.Game.Units.EnemyType t)
		{
			enemyType = t;
		}
	}

	public class EnemyTableSpawnEntry
	{
		public float weight;
		public EnemyEntry enemy;
	}

	public class EnemyTableEntry
	{
		public string id = "<id>";
		public int maxTowerLevel;
		public List<EnemyTableSpawnChanceEntry> spawnChances = new List<EnemyTableSpawnChanceEntry> ();
		public Utils.Range<int> groupSize = new LootQuest.Utils.Range<int> ();
		public Utils.Range<int> groupCount = new LootQuest.Utils.Range<int> ();
		public Dictionary<LootQuest.Game.Units.EnemyType, List<EnemyTableSpawnEntry>> enemies = new Dictionary<LootQuest.Game.Units.EnemyType, List<EnemyTableSpawnEntry>>();

		public EnemyTableEntry()
		{
			foreach(LootQuest.Game.Units.EnemyType t in Enum.GetValues(typeof(LootQuest.Game.Units.EnemyType)))
			{
				spawnChances.Add(new EnemyTableSpawnChanceEntry(t));
				enemies.Add(t, new List<EnemyTableSpawnEntry>());
			}
		}
	}
}