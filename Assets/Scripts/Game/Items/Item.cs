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
        private Attributes.AttributeManager stats_ = new Attributes.AttributeManager();

        public Attributes.AttributeManager Stats
        {
            get { return stats_;  }
        }

        public void SetRarity(Rarity r)
        {
            rarity_ = r;
        }

        public void SetType(ItemType t)
        {
            itemType_ = t;
        }

        public void SetCurrency(Finance.CurrencyID c)
        {
            currency_ = c;
        }

        public void SetID(string id)
        {
            id_ = id;
        }

        public void SetCount(int c)
        {
            count_ = c;
        }
	}
}