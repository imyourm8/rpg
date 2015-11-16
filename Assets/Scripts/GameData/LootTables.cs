using UnityEngine;

using System.IO;
using System.Collections;
using System.Collections.Generic;

using LootQuest.Game.Items;

namespace LootQuest.GameData
{
    public class LootTables : Utils.Singleton<LootTables>
    {
        public List<LootTableEntry> Data = new List<LootTableEntry>();
        private Dictionary<string, LootTableEntry> hash_ = new Dictionary<string, LootTableEntry>();

        public LootTableEntry GetEntry(string id, int level)
        {
            LootTableEntry table = null;
            hash_.TryGetValue(id, out table);
            return table;
        }

        public void Save()
        {
            var path = "Assets/Resources/GameData/loot_tables.json.txt";
            var file = File.Create(path);
            JSONObject obj = new JSONObject(JSONObject.Type.ARRAY);

            foreach (var table in Data)
            {
                JSONObject tableData = new JSONObject();
                obj.Add(tableData);

                tableData.AddField("id", table.id);
                tableData.AddField("group", table.group);
                tableData.AddField("level", Utils.RangeUtils.ToJson(table.level));

                JSONObject itemData = new JSONObject(JSONObject.Type.ARRAY);
                tableData.AddField("drop", itemData);

                foreach (var drop in table.items)
                {
                    JSONObject dropData = new JSONObject();
                    itemData.Add(dropData);

                    dropData.AddField("w", drop.weight);
                    dropData.AddField("t", (int)drop.item);
                    dropData.AddField("table", drop.table);
                    dropData.AddField("level", drop.level);
                }
            }

            byte[] data = System.Text.Encoding.ASCII.GetBytes(obj.ToString());
            file.Write(data, 0, data.Length);

            file.Flush();
            file.Close();

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.ImportAsset(path);
#endif
        }

        public void Load()
        {
            Items.Instance.Load();

            TextAsset asset = Resources.Load("GameData/loot_tables.json") as TextAsset;
            hash_.Clear();
            Data.Clear();

            if (asset == null) return;

            JSONObject tables = new JSONObject(asset.text);

            foreach (var tableData in tables.list)
            {
                var table = new LootTableEntry();
                table.id = tableData["id"].str;
                table.group = tableData["group"].str;
                Utils.RangeUtils.FromJson(table.level, tableData["level"]);

                foreach (var dropData in tableData["drop"].list)
                {
                    var drop = new LootTableItemDrop();
                    drop.weight = (int)dropData["w"].i;
                    drop.item = (ItemType)dropData["t"].i;
                    drop.table = dropData["table"].str;
                    drop.level = (int)dropData["level"].i;
                
                    table.items.Add(drop);
                }

                Data.Add(table);
                hash_.Add(table.id, table);
            }

            GenerateItemDropWeights();
        }

        private void GenerateItemDropWeights()
        {
            foreach (var table in Data)
            {
                int totalWeight = 0;
                foreach (var drop in table.items)
                {
                    totalWeight += drop.weight;
                    drop.totalWeight = totalWeight;
                }
            }
        }
    }
}