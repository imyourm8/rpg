using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LootQuest.GameData;

namespace LootQuest.Game.Items
{
    public class ItemGenerator : Utils.Singleton<ItemGenerator>
    {
        private List<ItemEntry> templates_ = new List<ItemEntry>();

        public Item Generate(int requiredLevel, string tableID)
        {
            Item item = null;
            var dropTable = LootTables.Instance.GetEntry(tableID, requiredLevel);

            if (dropTable != null)
            {
                item = Generate(dropTable, requiredLevel, true);
            }

            return item;
        }

        private Item Generate(LootTableEntry table, int level, bool noDropCheck)
        {
            int noDrop = table.noDropChance;
            int totalItemWeight = table.totalItemWeight;

            if (Random.Range(1, totalItemWeight + noDrop) <= noDrop)
            { 
                //no drop
                return null;
            }

            int itemEntryRoll = Random.Range(1, totalItemWeight);
            
            int totalItems = table.items.Count;
            int itemIdx = 0;
            while (itemIdx < totalItems && table.items[itemIdx].accWeight < itemEntryRoll)
            {
                itemIdx++;
            }

            LootTableItemDrop dropEntry = table.items[itemIdx];
            if (dropEntry.table != "")
            {
                //no item, another table
                var anotherTable = LootTables.Instance.GetEntry(dropEntry.table, level);
                if (anotherTable != null)
                    return Generate(anotherTable, level, false);
            }

            //now, roll rarity
            int rarityRoll = Random.Range(1, table.totalRarityWeight);
            int totalRarities = table.rarity.Count;
            int rarityIdx = 0;
            while (rarityIdx < totalRarities && table.rarity[rarityIdx].accWeight < rarityRoll)
            {
                rarityIdx++;
            }

            var rarityEntry = table.rarity[rarityIdx];
            //create item and generate attributes

            Item item = new Item();
            item.SetType(dropEntry.item);

            if (dropEntry.item == ItemType.Currency)
            {
                //drop gold 
                item.SetCurrency(Finance.CurrencyID.Gold);
            } else
            {
                //pick item template by type and level
                var itemsByType = GameData.Items.Instance.GetItemsByType(dropEntry.item);
                if (itemsByType != null)
                {
                    //filter items by level
                    templates_.Clear();
                    int count = itemsByType.Count;
                    for (int i = 0; i < count; ++i)
                    {
                        var entry = itemsByType[i];
                        if (entry.levelRange.Min >= level && level <= entry.levelRange.Max)
                        {
                            templates_.Add(entry);
                        }
                    }

                    if (templates_.Count > 0)
                    {
                        //pick item
                        int totalTemplates = templates_.Count;
                        int totalWeight = 0;
                        for (int j = 0; j < count; ++j)
                        {
                            totalWeight++;
                            templates_[j].accWeight = totalWeight;
                        }

                        int templateRoll = Random.Range(1, totalWeight);
                        int templateIdx = 0;
                        while (templateIdx < totalTemplates && templates_[templateIdx].accWeight < templateRoll)
                        {
                            templateIdx++;
                        }

                        var template = templates_[templateIdx];
                        
                    }
                }
            }

            return item;
        }
    }
}