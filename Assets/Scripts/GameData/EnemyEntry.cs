using UnityEngine;
using System.Collections.Generic;
using LootQuest.Game;
using LootQuest.Game.Attributes;

namespace LootQuest.GameData
{
	public class EnemySpell
	{
		public float weight;
		public string spellID;

		public int selectedSpell;
	}

	public class EnemySpellsEntry
	{
		public List<EnemySpell> spells = new List<EnemySpell>();
		public int maxEnemyLevel;

		public bool expanded = false;
	}

	public class EnemyEntry
	{
		public List<AttributeTemplate> Stats;
		public string ID = "";
		public GameObject viewPrefab;
		public LootQuest.Game.Units.AI.Type ai;
		public string autoAttackAbility;
		public List<EnemySpellsEntry> spells;
		public LootQuest.Game.Units.EnemyType type;
		public int minionCount;
        public double exp;
        public string drop_table;

		public EnemyEntry()
		{
			Stats = new List<AttributeTemplate> ();
			spells = new List<EnemySpellsEntry> ();

			Stats.Add (new AttributeTemplate(LootQuest.Game.Attributes.AttributeID.AttackRange));
			Stats.Add (new AttributeTemplate(LootQuest.Game.Attributes.AttributeID.Health));
			Stats.Add (new AttributeTemplate(LootQuest.Game.Attributes.AttributeID.DamageMin));
			Stats.Add (new AttributeTemplate(LootQuest.Game.Attributes.AttributeID.DamageMax));
			Stats.Add (new AttributeTemplate(LootQuest.Game.Attributes.AttributeID.CritDamage));
			Stats.Add (new AttributeTemplate(LootQuest.Game.Attributes.AttributeID.CritChance));
			Stats.Add (new AttributeTemplate(LootQuest.Game.Attributes.AttributeID.AttackSpeed));
			Stats.Add (new AttributeTemplate(LootQuest.Game.Attributes.AttributeID.HealthRegeneration));
			Stats.Add (new AttributeTemplate(LootQuest.Game.Attributes.AttributeID.MovementSpeed));
		}
	}
}