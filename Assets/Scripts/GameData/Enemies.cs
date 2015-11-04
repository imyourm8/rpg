﻿using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using LootQuest.Game.Attributes;

namespace LootQuest.GameData
{
	public class Enemies : Utils.Singleton<Enemies>
	{
		public List<GameData.EnemyEntry> Data = new List<GameData.EnemyEntry>();

		public void Load()
		{
			Data.Clear ();
			TextAsset asset = Resources.Load("GameData/enemies.json") as TextAsset;

			if (asset != null) 
			{
				JSONObject obj = new JSONObject (asset.text);

				foreach(JSONObject enemy in obj.list)
				{
					LootQuest.GameData.EnemyEntry model = new LootQuest.GameData.EnemyEntry();

					model.ID = enemy["id"].str;
					model.ai = (LootQuest.Game.Units.AI.Type)enemy["ai"].i;

					if (enemy.HasField("auto_attack"))
						model.autoAttackAbility = enemy["auto_attack"].str; 

					var viewPath = enemy["view"].str;
					viewPath = viewPath.Replace("Assets/Resources/", "");
					viewPath = viewPath.Replace(".prefab", "");
					if (viewPath != null && viewPath.Length > 0)
						model.viewPrefab = Resources.Load<GameObject>(viewPath);

					var stats = enemy["stats"];
					foreach(Game.Attributes.AttributeID statID in Enum.GetValues(typeof(Game.Attributes.AttributeID)))
					{
						var template = model.Stats.Find((AttributeTemplate att)=>
						{
							return att.id == statID;
						});
						if (template == null) continue;
						var statStr = statID.ToString();
						var statData = stats[statStr];
						template.minValue = statData[statStr+"_min"].f;
						template.maxValue = statData[statStr+"_max"].f;
					}

					if (enemy.HasField("spells"))
					{
						var spellEntries = enemy["spells"].list;
						foreach(var entry in spellEntries)
						{
							var spellsEntry = new LootQuest.GameData.EnemySpellsEntry();
							spellsEntry.maxEnemyLevel = (int)entry["level"].i;

							foreach(var sp in entry["spells"].list)
							{
								var spell = new LootQuest.GameData.EnemySpell();
								spell.spellID = sp["id"].str;
								spell.weight = sp["w"].f;
								spellsEntry.spells.Add(spell);
							}

							model.spells.Add(spellsEntry);
						}
					}
					Data.Add(model);
				}
			}
		}

		public void Save()
		{
			var file = File.Create("Assets/Resources/GameData/enemies.json.txt");
			JSONObject obj = new JSONObject (JSONObject.Type.ARRAY);

			foreach (var enemy in Data) 
			{
				JSONObject enemyObj = new JSONObject();
				obj.Add(enemyObj);

				enemyObj.AddField("id", enemy.ID);
				enemyObj.AddField("ai", (int)enemy.ai);
				enemyObj.AddField("auto_attack", enemy.autoAttackAbility);

				if (enemy.viewPrefab != null)
					enemyObj.AddField("view", enemy.viewPrefab.GetPrefabPath());

				JSONObject stats = new JSONObject();
				enemyObj.AddField("stats", stats);

				foreach(var stat in enemy.Stats)
				{
					JSONObject range = new JSONObject();
					var statID = stat.id.ToString();
					range.AddField(statID+"_min", stat.minValue);
					range.AddField(statID+"_max", stat.maxValue);
					stats.AddField(statID, range);
				}

				JSONObject spellsObj = new JSONObject(JSONObject.Type.ARRAY);
				enemyObj.AddField("spells", enemyObj);

				foreach(var spells in enemy.spells)
				{
					JSONObject spellObj = new JSONObject();
					spellObj.AddField("level", spells.maxEnemyLevel);

					JSONObject spellList = new JSONObject(JSONObject.Type.ARRAY);
					spellObj.AddField("spells", spellList);

					foreach(var spell in spells.spells)
					{
						JSONObject sp = new JSONObject();
						spellObj.Add(sp);

						sp.AddField("id", spell.spellID);
						sp.AddField("w", spell.weight);
					}

					spellsObj.Add(spellObj);
				}
			}

			byte[] data = System.Text.Encoding.ASCII.GetBytes (obj.ToString ());
			file.Write (data, 0, data.Length);

			file.Flush ();
			file.Close ();
		}
	}
}