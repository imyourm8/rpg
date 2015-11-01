using UnityEngine;

using System.IO;
using System.Collections.Generic;

using LootQuest.Utils;

namespace LootQuest.GameData
{
	public class Modifiers : Utils.Singleton<Modifiers> 
	{
		public List<ModifierEntry> Data = new List<ModifierEntry>();

		public void Load()
		{
			TextAsset asset = Resources.Load("GameData/spell_mods.json") as TextAsset;
			Data.Clear ();
			if (asset != null) 
			{
				JSONObject obj = new JSONObject(asset.text);
				foreach(var modData in obj.list)
				{
					var mod = new ModifierEntry();

					mod.id = modData["id"].str;
					mod.titleID = modData["title"].str;

					if (modData.HasField("projectile_view"))
					{
						var projViewPath = modData["projectile_view"].str;
						projViewPath = projViewPath.Replace("Assets/Resources/", "");
						projViewPath = projViewPath.Replace(".prefab", "");
						if (projViewPath != null && projViewPath.Length > 0)
							mod.projectileView = Resources.Load<GameObject>(projViewPath);
					}

					if (modData.HasField("view"))
					{
						var viewPath = modData["view"].str;
						viewPath = viewPath.Replace("Assets/Resources/", "");
						viewPath = viewPath.Replace(".prefab", "");
						if (viewPath != null && viewPath.Length > 0)
							mod.view = Resources.Load<GameObject>(viewPath);
					}
					  
					if (modData.HasField("projectile_scale"))
						RangeUtils.FromJson(mod.projectileScale, modData["projectile_scale"]);

					RangeUtils.FromJson(mod.duration, modData["duration"]);
					RangeUtils.FromJson(mod.projectileCount, modData["projectile_count"]);
					RangeUtils.FromJson(mod.maxStacks, modData["max_stacks"]);
					RangeUtils.FromJson(mod.power, modData["power"]);
					RangeUtils.FromJson(mod.range, modData["range"]);

					Data.Add(mod);
				}
			}
		}

		public void Save()
		{
			var file = File.Create("Assets/Resources/GameData/spell_mods.json.txt");
			JSONObject obj = new JSONObject (JSONObject.Type.ARRAY);
			
			foreach (var mod in Data) 
			{
				var modData = new JSONObject();
				obj.Add(modData);

				modData.AddField("id", mod.id);
				modData.AddField("title", mod.titleID);

				if (mod.projectileView != null)
					modData.AddField("projectile_view", mod.projectileView.GetPrefabPath());

				if (mod.view != null)
					modData.AddField("view", mod.view.GetPrefabPath());

				modData.AddField("projectile_scale", RangeUtils.ToJson(mod.projectileScale));
				modData.AddField("projectile_count", RangeUtils.ToJson(mod.projectileCount));
				modData.AddField("max_stacks", RangeUtils.ToJson(mod.maxStacks));
				modData.AddField("duration", RangeUtils.ToJson(mod.duration));
				modData.AddField("power", RangeUtils.ToJson(mod.power));
				modData.AddField("range", RangeUtils.ToJson(mod.range));
			}

			byte[] data = System.Text.Encoding.ASCII.GetBytes (obj.ToString ());
			file.Write (data, 0, data.Length);
			
			file.Flush ();
			file.Close ();
		}
	}
}