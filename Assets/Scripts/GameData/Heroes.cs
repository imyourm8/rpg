﻿using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;

namespace LootQuest.GameData
{
	public class Heroes : Utils.Singleton<Heroes> 
	{
		public HeroEntry Data = new HeroEntry();

		public void Load()
		{
			TextAsset asset = Resources.Load("GameData/heroes.json") as TextAsset;

			if (asset != null) 
			{
				JSONObject hero = new JSONObject (asset.text);

				var viewPath = hero["view"].str;
				viewPath = viewPath.Replace("Assets/Resources/", "");
				viewPath = viewPath.Replace(".prefab", "");

				if (viewPath != null && viewPath.Length > 0)
					Data.viewPrefab = Resources.Load<GameObject>(viewPath);

				var stats = hero["stats"];
				foreach(var stat in Data.Stats)
				{
					var statID = stat.ID.ToString();
					if (stats.HasField(statID))
					{
						stat.SetValue(stats[statID].f);
					}
				}

				if (hero.HasField("auto_attack"))
				{
					Data.autoAttackAbility = hero["auto_attack"].str;
				}
			}
		}

		public void Save()
		{
			var path = "Assets/Resources/GameData/heroes.json.txt";
			var file = File.Create(path);
			JSONObject obj = new JSONObject ();

			if (Data.viewPrefab != null)
				obj.AddField ("view", Data.viewPrefab.GetPrefabPath ());

			var stats = new JSONObject ();
			obj.AddField ("stats", stats);

			foreach (var stat in Data.Stats) 
			{
				stats.AddField(stat.ID.ToString(), stat.Value);
			}

			obj.AddField ("auto_attack", Data.autoAttackAbility);

			byte[] data = System.Text.Encoding.ASCII.GetBytes (obj.ToString ());
			file.Write (data, 0, data.Length);
			
			file.Flush ();
			file.Close ();

			#if UNITY_EDITOR
			UnityEditor.AssetDatabase.ImportAsset(path);
			#endif
		}
	}
}