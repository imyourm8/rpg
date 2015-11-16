using UnityEngine;

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using LootQuest.GameData;
using LootQuest.Game.Items;

namespace LootQuest.GameData
{
    public class ItemGenerationRules : Utils.Singleton<ItemGenerationRules>
    {
        private Dictionary<Rarity, ItemGenerationRulesEntry> byRarity_ = new Dictionary<Rarity, ItemGenerationRulesEntry>();
        public List<ItemGenerationRulesEntry> Data = new List<ItemGenerationRulesEntry>();

        public void Load()
        { 
            TextAsset asset = Resources.Load("GameData/item_generation_rules.json") as TextAsset;
			Data.Clear ();
            byRarity_.Clear();

            foreach (Rarity r in Enum.GetValues(typeof(Rarity)))
            {
                var entry = new ItemGenerationRulesEntry(r);
                Data.Add(entry);
                byRarity_.Add(r, entry);
            }

			if (asset != null)
            {

            }
        }

        public void Save()
        {
            var path = "Assets/Resources/GameData/item_generation_rules.json.txt";
            var file = File.Create(path);
            JSONObject obj = new JSONObject(JSONObject.Type.ARRAY);

            byte[] data = System.Text.Encoding.ASCII.GetBytes(obj.ToString());
            file.Write(data, 0, data.Length);

            file.Flush();
            file.Close();

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.ImportAsset(path);
#endif
        }
    }
}