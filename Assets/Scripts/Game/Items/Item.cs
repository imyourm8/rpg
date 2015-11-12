using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Items
{
	public enum ItemType
	{
		Armor,
        Helmet,
        Weapon,
		Currency
	}

    public enum Rarity
    {
        Common,
        Magic,
        Rare,
        Unique,
        Mythic
    }

	public class Item 
	{
		private int count_ = 1;
		private string id_;
		private Finance.CurrencyID currency_;
		private ItemType itemType_;
        private Rarity rarity_;
	}
}