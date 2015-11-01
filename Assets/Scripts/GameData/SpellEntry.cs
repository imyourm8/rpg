using UnityEngine;
using System.Collections.Generic;

using LootQuest.Utils;

namespace LootQuest.GameData
{
	public class SpellEntry 
	{
		public int level = 1;
		public string ID = "unique_id";
		public string titleID = "rename me";
		public List<Game.Spells.SpellEffects.SpellEffectID> effects;

		public Game.Spells.SlotStype slot;
		public Game.Spells.SpellType type;

		public LootQuest.Game.Spells.SpellTargetStrategy targetStrategy;
		public LootQuest.Game.Spells.SpellTargetFilter targetFilter;

		public Range<float> cooldown = new Range<float>();

		public SpellEntry()
		{
			effects = new List<LootQuest.Game.Spells.SpellEffects.SpellEffectID> ();
		}

		public SpellEntry Copy()
		{
			var entry = (SpellEntry)this.MemberwiseClone ();
			entry.effects = new List<LootQuest.Game.Spells.SpellEffects.SpellEffectID> ();
			foreach (var eff in effects) 
			{
				entry.effects.Add(eff);
			}
			return entry;
		}
	}
}