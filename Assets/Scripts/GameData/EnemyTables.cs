using UnityEngine;
using System.Collections.Generic;

namespace LootQuest.GameData {
	public class EnemyTables : LootQuest.Utils.Singleton<EnemyTables>
	{
		public List<EnemyTableEntry> Data = new List<EnemyTableEntry>();

		public void Save()
		{}

		public void Load()
		{}
	}
}