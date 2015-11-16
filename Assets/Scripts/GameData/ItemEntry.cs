using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using LootQuest.Game;
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

        public List<GameData.AttributeTemplate> Stats = new List<AttributeTemplate>();

        public int accWeight = 0;

        public ItemEntry()
        {
            Stats.Add(new AttributeTemplate(Game.Attributes.AttributeID.Strength));
            Stats.Add(new AttributeTemplate(Game.Attributes.AttributeID.Defense));
            Stats.Add(new AttributeTemplate(Game.Attributes.AttributeID.CritDamage));
            Stats.Add(new AttributeTemplate(Game.Attributes.AttributeID.Damage));
            Stats.Add(new AttributeTemplate(Game.Attributes.AttributeID.AttackSpeed));
            Stats.Add(new AttributeTemplate(Game.Attributes.AttributeID.Health));
            Stats.Add(new AttributeTemplate(Game.Attributes.AttributeID.HealthRegeneration));
            Stats.Add(new AttributeTemplate(Game.Attributes.AttributeID.SpellDamage));
            Stats.Add(new AttributeTemplate(Game.Attributes.AttributeID.MovementSpeed));
            Stats.Add(new AttributeTemplate(Game.Attributes.AttributeID.CritChance));
        }
	}
}