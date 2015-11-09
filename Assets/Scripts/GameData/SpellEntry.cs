using UnityEngine;
using System.Collections.Generic;

using LootQuest.Utils;

namespace LootQuest.GameData
{
	public class ProjectileEntry
	{
		public Game.Spells.Projectiles.Behaviours.BehaviourID behaviour = Game.Spells.Projectiles.Behaviours.BehaviourID.Dummy;
		public GameObject view;
		public float speed = 100.0f;
		public bool pierce = false;
	}

	public class SpellEntry 
	{
		public int level = 1;
		public string ID = "unique_id";
		public string titleID = "rename me";
		public List<string> effects;

		public Game.Spells.SlotStype slot;
		public Game.Spells.SpellType type;

		public LootQuest.Game.Spells.SpellTargetStrategy targetStrategy;
		public LootQuest.Game.Spells.SpellTargetFilter targetFilter;

		public Range<float> cooldown = new Range<float>();

		public GameObject view;
		public ProjectileEntry projectile = new ProjectileEntry();

		public SpellEntry()
		{
			effects = new List<string> ();
		}

		public SpellEntry Copy()
		{
			var entry = (SpellEntry)this.MemberwiseClone ();
			entry.effects = new List<string> ();
			entry.cooldown = cooldown.Copy ();
			foreach (var eff in effects) 
			{
				entry.effects.Add(eff);
			}
			return entry;
		}
	}
}