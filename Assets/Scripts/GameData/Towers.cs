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

		public void Load()
		{
			TextAsset asset = Resources.Load("GameData/towers.json") as TextAsset;
			Data.Clear ();
			if (asset != null) 
			{
				JSONObject obj = new JSONObject (asset.text);
				
				foreach(JSONObject tower in obj.list)
				{
					var model = new TowerEntry();
					model.EnemyDamageMultiplier = tower["enemy_dmg_mult"].f;
					model.EnemyHpMultiplier = tower["enemy_hp_mult"].f;
					model.LevelRange = new LootQuest.Utils.Range<long>(tower["lvl_min"].i, tower["lvl_max"].i);

					Data.Add(model);
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
			}
			
			byte[] data = System.Text.Encoding.ASCII.GetBytes (obj.ToString ());
			file.Write (data, 0, data.Length);
			
			file.Flush ();
			file.Close ();
		}
	}
}