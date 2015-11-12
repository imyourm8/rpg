using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Items
{
	public enum ItemType
	{
		Loot,
		Currency
	}

	public class Item 
	{
		private int count_ = 1;
		private string id_;
		private Finance.CurrencyID currency_;
		private ItemType itemType_;
	}
}