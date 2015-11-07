using UnityEngine;

using System;
using System.IO;
using System.Collections.Generic;

namespace LootQuest.GameData {
	public class EnemyTables : LootQuest.Utils.Singleton<EnemyTables>
	{
		public List<EnemyTableEntry> Data = new List<EnemyTableEntry>();
		private Dictionary<string, EnemyTableEntry> hash_ = new Dictionary<string, EnemyTableEntry>();

		public EnemyTableEntry GetTable(string id)
		{
			EnemyTableEntry entry;
			hash_.TryGetValue (id, out entry);
			return entry;
		}

		public void Load()
		{
			Data.Clear ();
			hash_.Clear ();
			TextAsset asset = Resources.Load("GameData/enemy_tables.json") as TextAsset;

			if (asset != null) 
			{
				JSONObject data = new JSONObject(asset.text);
				if (data.list == null) return;
				foreach(var tableData in data.list)
				{
					var table = new EnemyTableEntry();

					table.id = tableData["id"].str;
					Utils.RangeUtils.FromJson(table.groupCount, tableData["group_count"]);
					Utils.RangeUtils.FromJson(table.groupSize, tableData["group_size"]);
					table.maxTowerLevel = (int)tableData["level"].i;

					foreach(var spawnChanceData in tableData["spawn_chances"].list)
					{
						var enemyType = (LootQuest.Game.Units.EnemyType)spawnChanceData["enemy_type"].i;
						var spawnChance = table.spawnChances.Find(x=>x.enemyType==enemyType);

						spawnChance.weight = (int)spawnChanceData["weight"].i;

						var enemies = table.enemies[spawnChance.enemyType];
						foreach(var enemyData in spawnChanceData["enemies"].list)
						{
							var enemyEntry = Enemies.Instance.GetEnemy(enemyData["id"].str);
							if (enemyEntry == null) continue;
							var enemy = new EnemyTableSpawnEntry();
							enemies.enemies.Add(enemy);

							enemy.weight = (int)enemyData["weight"].i;
							enemy.enemy = enemyEntry;
						}
					}

					Data.Add(table);
					hash_.Add(table.id, table);

					table.OrderEnemiesByWeight();
					table.OrderSpawnChancesByWeight();
				}
			}
		}

		public void Save()
		{
			var file = File.Create("Assets/Resources/GameData/enemy_tables.json.txt");
			JSONObject obj = new JSONObject (JSONObject.Type.ARRAY);

			foreach (var table in Data) 
			{
				JSONObject tableData = new JSONObject();
				obj.Add(tableData);

				tableData.AddField("group_count", Utils.RangeUtils.ToJson(table.groupCount));
				tableData.AddField("group_size", Utils.RangeUtils.ToJson(table.groupSize));
				tableData.AddField("id", table.id);
				tableData.AddField("level", table.maxTowerLevel);

				JSONObject spawnChancesData = new JSONObject(JSONObject.Type.ARRAY);
				tableData.AddField("spawn_chances", spawnChancesData);
				foreach(var spawnChance in table.spawnChances)
				{
					JSONObject spawnChanceData = new JSONObject();
					spawnChancesData.Add(spawnChanceData);

					spawnChanceData.AddField("weight", spawnChance.weight);
					spawnChanceData.AddField("enemy_type", (int)spawnChance.enemyType);

					JSONObject enemiesData = new JSONObject(JSONObject.Type.ARRAY);
					spawnChanceData.AddField("enemies", enemiesData);

					foreach(var enemy in table.enemies[spawnChance.enemyType].enemies)
					{
						JSONObject enemyData = new JSONObject();
						enemiesData.Add(enemyData);

						enemyData.AddField("id", enemy.enemy.ID);
						enemyData.AddField("weight", enemy.weight);
					}
				}
			}

			byte[] data = System.Text.Encoding.ASCII.GetBytes (obj.ToString ());
			file.Write (data, 0, data.Length);
			
			file.Flush ();
			file.Close ();
		}
	}
}