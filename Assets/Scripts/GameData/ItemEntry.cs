using UnityEngine;

using System.Collections;

using LootQuest.Game.Finance;
using LootQuest.Game.Items;

namespace LootQuest.GameData
{
	public class ItemEntry 
	{
        public string id;
        public CurrencyID currency;
        public ItemType itemType;
        public string icon;
        public string view;
        public Utils.Range<int> levelRange = new Utils.Range<int>(); //at which levels pick this item if requested
	}
}