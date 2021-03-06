﻿using UnityEngine;
using System.IO;
using System.Collections.Generic;

using LootQuest.Utils;

namespace LootQuest.GameData
{
	public class Spells : Utils.Singleton<Spells> 
	{
		public List<SpellEntry> Data = new List<SpellEntry>();
		private Dictionary<string, SpellEntry> entries_ = new Dictionary<string, SpellEntry>();

		public SpellEntry GetEntry(string id)
		{
			SpellEntry entry;
			entries_.TryGetValue (id, out entry);
			return entry;
		}

		public void Load()
		{
			TextAsset asset = Resources.Load("GameData/spells.json") as TextAsset;
			entries_.Clear ();
			Data.Clear ();
			if (asset != null) 
			{
				JSONObject obj = new JSONObject (asset.text);

				foreach(var spell in obj.list)
				{
					var entry = new SpellEntry();

					entry.ID = spell["id"].str;
					entry.slot = (LootQuest.Game.Spells.SlotStype)spell["slot"].i;
					entry.type = (LootQuest.Game.Spells.SpellType)spell["type"].i;
					entry.targetStrategy = (LootQuest.Game.Spells.SpellTargetStrategy)spell["strategy"].i;
					entry.targetFilter = (LootQuest.Game.Spells.SpellTargetFilter)spell["filter"].i;
					entry.titleID = spell["title"].str;
					if (spell.HasField("level"))
						entry.level = (int)spell["level"].i;
					RangeUtils.FromJson(entry.cooldown, spell["cooldown"]);

					foreach(var eff in spell["effects"].list)
					{
						entry.effects.Add(eff.str);
					}

					if (spell.HasField("view"))
					{
						var viewPath = spell["view"].str;
						viewPath = viewPath.Replace("Assets/Resources/", "");
						viewPath = viewPath.Replace(".prefab", "");
						if (viewPath != null && viewPath.Length > 0)
							entry.view = Resources.Load<GameObject>(viewPath);
					}

					if (spell.HasField("projectile"))
					{
						var projectileData = spell["projectile"];
						entry.projectile.behaviour = (LootQuest.Game.Spells.Projectiles.Behaviours.BehaviourID)projectileData["behaviour"].i;
						entry.projectile.speed = projectileData["speed"].f;

						if (projectileData.HasField("view"))
					    {
							var viewPath = projectileData["view"].str;
							viewPath = viewPath.Replace("Assets/Resources/", "");
							viewPath = viewPath.Replace(".prefab", "");
							if (viewPath != null && viewPath.Length > 0)
								entry.projectile.view = Resources.Load<GameObject>(viewPath);
						}

						if (projectileData.HasField("pirce"))
							entry.projectile.pierce = projectileData["pierce"].b;
					}

					entries_.Add(entry.ID, entry);
					Data.Add(entry);
				}
			}
		}

		public void Save()
		{
			var path = "Assets/Resources/GameData/spells.json.txt";
			var file = File.Create(path);
			JSONObject obj = new JSONObject (JSONObject.Type.ARRAY);

			foreach (var spell in Data) 
			{
				JSONObject spellData = new JSONObject();
				obj.Add(spellData);

				spellData.AddField("id", spell.ID);
				spellData.AddField("slot", (int)spell.slot);
				spellData.AddField("type", (int)spell.type);
				spellData.AddField("filter", (int)spell.targetFilter);
				spellData.AddField("strategy", (int)spell.targetStrategy);
				spellData.AddField("title", spell.titleID);
				spellData.AddField("cooldown", RangeUtils.ToJson(spell.cooldown));
				spellData.AddField("level", spell.level);

				if (spell.view != null)
					spellData.AddField("view", spell.view.GetPrefabPath());

				JSONObject effects = new JSONObject(JSONObject.Type.ARRAY);
				spellData.AddField("effects", effects);

				foreach(var eff in spell.effects)
				{
					effects.Add(eff);
				}

				JSONObject projectile = new JSONObject();
				spellData.AddField("projectile", projectile);

				projectile.AddField("pierce", spell.projectile.pierce);
				projectile.AddField("behaviour", (int)spell.projectile.behaviour);
				projectile.AddField("speed", spell.projectile.speed);
				if (spell.projectile.view != null)
					projectile.AddField("view", spell.projectile.view.GetPrefabPath());
			}

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