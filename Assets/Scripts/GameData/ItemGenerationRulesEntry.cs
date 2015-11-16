using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using LootQuest;
using LootQuest.Game.Items;

namespace LootQuest.GameData
{
    public class ItemGenerationAttEntry
    {
        public LootQuest.Game.Attributes.AttributeID id;

        public int weight = 0;
        public int accWeight = 0;
    }

    public class ItemGenerationRulesEntry
    {
        public Rarity rarity;
        public List<ItemGenerationAttEntry> attributes = new List<ItemGenerationAttEntry>();

        public int attributesTotalWeight = 0;

        public ItemGenerationRulesEntry(Rarity r)
        {
            rarity = r;
        }

        public void CalculateWeights()
        {
            int totalWeight = 0;
            foreach (var att in attributes)
            {
                totalWeight += att.weight;
                att.accWeight = totalWeight;
            }
            attributesTotalWeight = totalWeight;
        }
    }
}