﻿using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using LootQuest.Game.Items;

namespace LootQuest.GameData
{
    public class LootTableItemDrop
    {
        public int weight = 0;
        public ItemType item; //item type to drop
        public int level; //which item lvl needs to drop
        public string table = ""; //if not empty load drop from another table

        public int totalWeight = 0;
    }

    public class LootTableItemTypeDrop
    {
        public int weight = 0;
        public Rarity rarity;

        public LootTableItemTypeDrop(Rarity r)
        {
            rarity = r;
        }
    }

    public class LootTableEntry
    {
        public string id = "<id>";
        public List<LootTableItemDrop> items = new List<LootTableItemDrop>();
        public Utils.Range<int> level = new Utils.Range<int>();
        public List<LootTableItemTypeDrop> rarity = new List<LootTableItemTypeDrop>();

        public int selectedItem = 0;

        public LootTableEntry()
        {
            foreach (Rarity iType in Enum.GetValues(typeof(Rarity)))
            {
                rarity.Add(new LootTableItemTypeDrop(iType));
            }
        }
    }
}