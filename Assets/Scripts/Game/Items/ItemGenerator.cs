using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Items
{
    public class ItemGenerator : Utils.Singleton<ItemGenerator>
    {
        public Item Generate(int requiredLevel, string tableID)
        {
            Item item = null;
            return item;
        }
    }
}