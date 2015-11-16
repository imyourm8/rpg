using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using LootQuest.Game.Items;

namespace LootQuest.GameData
{
	public class Items : Utils.Singleton<Items> 
	{
        public List<ItemEntry> Data = new List<ItemEntry>();
        private Dictionary<string, ItemEntry> hash_ = new Dictionary<string, ItemEntry>();
        private Dictionary<ItemType, List<ItemEntry>> itemsByType_ = new Dictionary<ItemType, List<ItemEntry>>();

        public ItemEntry GetItem(string id)
        {
            ItemEntry item = null;
            hash_.TryGetValue(id, out item);
            return item;
        }

        public List<ItemEntry> GetItemsByType(ItemType type)
        {
            List<ItemEntry> byType = null;
            itemsByType_.TryGetValue(type, out byType);
            return byType;
        }

		public void Load()
		{
            TextAsset asset = Resources.Load("GameData/items.json") as TextAsset;
            hash_.Clear();
            Data.Clear();

            if (asset == null) return;

            JSONObject items = new JSONObject(asset.text);
            foreach (var itemData in items.list)
            {
                var item = new ItemEntry();
                item.id = itemData["id"].str;
                item.currency = (Game.Finance.CurrencyID)itemData["currency"].i;
                item.icon = itemData["icon"].str;
                if (itemData.HasField("view"))
                    item.view = itemData["view"].str;
                item.itemType = (Game.Items.ItemType)itemData["type"].i;

                Data.Add(item);
                hash_.Add(item.id, item);

                if (!itemsByType_.ContainsKey(item.itemType))
                {
                    itemsByType_.Add(item.itemType, new List<ItemEntry>());
                }

                itemsByType_[item.itemType].Add(item);
            }
        }

		public void Save()
		{
            var path = "Assets/Resources/GameData/items.json.txt";
            var file = File.Create(path);
            JSONObject obj = new JSONObject(JSONObject.Type.ARRAY);

            foreach (var item in Data)
            {
                var itemObj = new JSONObject();
                obj.Add(itemObj);

                itemObj.AddField("id", item.id);
                itemObj.AddField("type", (int)item.itemType);
                itemObj.AddField("icon", item.icon);
                itemObj.AddField("currency", (int)item.currency);
            }

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