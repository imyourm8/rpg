using UnityEngine;

using System.IO;
using System.Collections.Generic;

using LootQuest.Utils;

namespace LootQuest.GameData
{
	public class Effects : Utils.Singleton<Effects> 
	{
		public List<SpellEffectEntry> Data = new List<SpellEffectEntry>();
		private Dictionary<string, SpellEffectEntry> hash_ = new Dictionary<string, SpellEffectEntry>();

		public SpellEffectEntry GetEntry(string id)
		{
			SpellEffectEntry entry;
			hash_.TryGetValue (id, out entry);
			return entry;
		}

		public void Load()
		{
			Data.Clear ();
			hash_.Clear ();

			TextAsset asset = Resources.Load("GameData/spell_effects.json") as TextAsset;
			
			if (asset != null) 
			{
				JSONObject obj = new JSONObject (asset.text);

				foreach(var effect in obj.list)
				{ 
					var model = new SpellEffectEntry();
					RangeUtils.FromJson(model.power, effect["power"]);
					RangeUtils.FromJson(model.triggerChance, effect["chance"]);
					model.handler = (LootQuest.Game.Spells.SpellEffects.SpellEffectID)effect["handler"].i;
					model.level = (int)effect["level"].i;
					model.titleID = effect["title"].str;
					model.triggeredStatus = (LootQuest.Game.Status.EffectID)effect["triggered_status"].i;
					model.id = effect["id"].str;

					Data.Add(model);
					hash_.Add(model.id, model);
				}
			}
		}

		public void Save()
		{
			var file = File.Create("Assets/Resources/GameData/spell_effects.json.txt");
			JSONObject obj = new JSONObject (JSONObject.Type.ARRAY);
			
			foreach (var effect in Data) 
			{
				JSONObject effectData = new JSONObject ();

				effectData.AddField("id", effect.id);
				effectData.AddField("level", effect.level);
				effectData.AddField("title", effect.titleID);
				effectData.AddField("handler", (int)effect.handler);
				effectData.AddField("triggered_status", (int)effect.triggeredStatus);
				effectData.AddField("chance", RangeUtils.ToJson(effect.triggerChance));
				effectData.AddField("power", RangeUtils.ToJson(effect.power));

				obj.Add(effectData);
			}

			byte[] data = System.Text.Encoding.ASCII.GetBytes (obj.ToString ());
			file.Write (data, 0, data.Length);
			
			file.Flush ();
			file.Close ();
		}
	}
}