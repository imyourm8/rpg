using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LootQuest.Game.Spells
{
    public class SpellManager
    {
        private Game.Units.Unit caster_;
		private Dictionary<SlotStype, Spell> spells_;

		public SpellManager(Game.Units.Unit caster)
        {
            caster_ = caster;
			spells_ = new Dictionary<SlotStype, Spell> ();
        }

		public void Add(Spell spell)
		{
			spells_.Add (spell.Data.entry.slot, spell);
		}

		public Spell GetSpellInSlot(SlotStype slot)
		{
			Spell spell = null;
			spells_.TryGetValue(slot, out spell);
			return spell;
		}

		public void Clear()
		{
			spells_.Clear ();
		}
    }
}
