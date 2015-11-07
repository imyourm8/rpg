using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace LootQuest.GameData
{
	public class EnemyTableSpawnChanceEntry
	{
		public int weight;
		public int realWeight;
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
		public float realWeight;
		public EnemyEntry enemy;
	}

	public class EnemyTableSpawnList
	{
		public float totalWeight = 0.0f;
		public List<EnemyTableSpawnEntry> enemies = new List<EnemyTableSpawnEntry>();
	}

	public class EnemyTableEntry
	{
		public string id = "<id>";
		public int maxTowerLevel;
		public List<EnemyTableSpawnChanceEntry> spawnChances = new List<EnemyTableSpawnChanceEntry> ();
		public Utils.Range<int> groupSize = new LootQuest.Utils.Range<int> ();
		public Utils.Range<int> groupCount = new LootQuest.Utils.Range<int> ();
		public Dictionary<LootQuest.Game.Units.EnemyType, EnemyTableSpawnList> enemies = new Dictionary<LootQuest.Game.Units.EnemyType, EnemyTableSpawnList>();

		public int TotalSpawnChanceWeight = 0;

		public EnemyTableEntry()
		{
			foreach(LootQuest.Game.Units.EnemyType t in Enum.GetValues(typeof(LootQuest.Game.Units.EnemyType)))
			{
				spawnChances.Add(new EnemyTableSpawnChanceEntry(t));
				enemies.Add(t, new EnemyTableSpawnList());
			}
		}

		public void OrderSpawnChancesByWeight()
		{
			spawnChances.Sort ((EnemyTableSpawnChanceEntry s1, EnemyTableSpawnChanceEntry s2)=>{ return s1.weight.CompareTo(s2.weight); });

			foreach (var chance in spawnChances) 
			{
				if (chance.weight < 1) continue;
				TotalSpawnChanceWeight += chance.weight;
				chance.realWeight = TotalSpawnChanceWeight;
			}
		}
		
		public void OrderEnemiesByWeight()
		{
			foreach (var table in enemies) 
			{
				table.Value.enemies.Sort((EnemyTableSpawnEntry e1, EnemyTableSpawnEntry e2)=>{ return e1.weight.CompareTo(e2); });

				foreach(var entry in table.Value.enemies)
				{
					if (entry.weight <= 0.0f) continue;
					table.Value.totalWeight += entry.weight;
					entry.realWeight = table.Value.totalWeight;
				}
			}
		}
	}
}