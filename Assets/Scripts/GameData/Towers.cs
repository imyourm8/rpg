using UnityEngine;

using System;
using System.IO;
using System.Collections.Generic;

namespace LootQuest.GameData
{
	public class Towers : Utils.Singleton<Towers> 
	{
		private static string Path = "Assets/Resources/GameData/towers.json.txt";
		public List<TowerEntry> Data = new List<TowerEntry>();
		private List<TowerEntry> towersByLevel_ = new List<TowerEntry> ();

		public TowerEntry GetTower(int level)
		{
			TowerEntry entry;
			level = Math.Min (towersByLevel_.Count, level);
			return towersByLevel_[level];
		}

		public void Load()
		{
			TextAsset asset = Resources.Load("GameData/towers.json") as TextAsset;
			Data.Clear ();
			towersByLevel_.Clear ();
			if (asset != null) 
			{
				JSONObject obj = new JSONObject (asset.text);
				
				foreach(JSONObject tower in obj.list)
				{
					var model = new TowerEntry();
					model.EnemyDamageMultiplier = tower["enemy_dmg_mult"].f;
					model.EnemyHpMultiplier = tower["enemy_hp_mult"].f;
					model.LevelRange = new LootQuest.Utils.Range<int>((int)tower["lvl_min"].i, (int)tower["lvl_max"].i);

					var tables = tower["tables"];
					if (tables != null)
					{
						foreach(var table in tables.list)
						{
							model.EnemyTables.Add(EnemyTables.Instance.GetTable(table["id"].str));
						}

					}

					Data.Add(model);
				}

				Data.Sort((TowerEntry left, TowerEntry right)=>
				{
					var leftLvl = left.LevelRange;
					var rightLvl = right.LevelRange;

					var minComp = leftLvl.Min.CompareTo(rightLvl.Min);
					var maxComp = leftLvl.Max.CompareTo(rightLvl.Max);

					if (maxComp != 0)
					{
						return maxComp;
					} else
					{
						return minComp;
					}
				});

				towersByLevel_.Resize(Data[Data.Count-1].LevelRange.Max+1);

				foreach (var model in Data)
				{
					for(int i = model.LevelRange.Min; i < model.LevelRange.Max; ++i)
					{
						towersByLevel_[i] = model;
					}
				}
			}
		}

		public void Save()
		{
			var file = File.Create(Path);
			JSONObject obj = new JSONObject (JSONObject.Type.ARRAY);
			
			foreach (var tower in Data) 
			{
				JSONObject towerObj = new JSONObject();
				obj.Add(towerObj);

				towerObj.AddField("enemy_dmg_mult", tower.EnemyDamageMultiplier);
				towerObj.AddField("enemy_hp_mult", tower.EnemyHpMultiplier);
				towerObj.AddField("lvl_min", tower.LevelRange.Min);
				towerObj.AddField("lvl_max", tower.LevelRange.Max);

				JSONObject enemyTables = new JSONObject(JSONObject.Type.ARRAY);
				towerObj.AddField("tables", enemyTables);
				foreach(var table in tower.EnemyTables)
				{
					JSONObject tableObj = new JSONObject();
					enemyTables.Add(tableObj);

					tableObj.AddField("id", table.id);
				}
			}
			
			byte[] data = System.Text.Encoding.ASCII.GetBytes (obj.ToString ());
			file.Write (data, 0, data.Length);
			
			file.Flush ();
			file.Close ();

			#if UNITY_EDITOR
			UnityEditor.AssetDatabase.ImportAsset(Path);
			#endif
		}
	}
}