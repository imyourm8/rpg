using UnityEngine;
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

					entries_.Add(entry.ID, entry);
					Data.Add(entry);
				}
			}
		}

		public void Save()
		{
			var file = File.Create("Assets/Resources/GameData/spells.json.txt");
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

				JSONObject effects = new JSONObject(JSONObject.Type.ARRAY);
				spellData.AddField("effects", effects);

				foreach(var eff in spell.effects)
				{
					effects.Add(eff);
				}
			}

			byte[] data = System.Text.Encoding.ASCII.GetBytes (obj.ToString ());
			file.Write (data, 0, data.Length);
			
			file.Flush ();
			file.Close ();
		}
	}
}