using UnityEngine;
using System.Collections;

namespace LootQuest.GameData
{
	public class HeroEntry
	{
		public GameObject viewPrefab;
		public Game.Attributes.AttributeManager Stats;

		public HeroEntry()
		{
			Stats = new LootQuest.Game.Attributes.AttributeManager ();

			Stats.Add (new LootQuest.Game.Attributes.Attribute().Init(Game.Attributes.AttributeID.AttackSpeed));
			Stats.Add (new LootQuest.Game.Attributes.Attribute().Init(Game.Attributes.AttributeID.CritChance));
			Stats.Add (new LootQuest.Game.Attributes.Attribute().Init(Game.Attributes.AttributeID.CritDamage));
			Stats.Add (new LootQuest.Game.Attributes.Attribute().Init(Game.Attributes.AttributeID.DamageMax));
			Stats.Add (new LootQuest.Game.Attributes.Attribute().Init(Game.Attributes.AttributeID.DamageMin));
			Stats.Add (new LootQuest.Game.Attributes.Attribute().Init(Game.Attributes.AttributeID.Dexterity));
			Stats.Add (new LootQuest.Game.Attributes.Attribute().Init(Game.Attributes.AttributeID.Health));
			Stats.Add (new LootQuest.Game.Attributes.Attribute().Init(Game.Attributes.AttributeID.HealthRegeneration));
			Stats.Add (new LootQuest.Game.Attributes.Attribute().Init(Game.Attributes.AttributeID.MovementSpeed));
			Stats.Add (new LootQuest.Game.Attributes.Attribute().Init(Game.Attributes.AttributeID.Strength));
		}
	}
}