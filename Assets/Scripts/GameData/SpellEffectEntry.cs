using UnityEngine;
using System.Collections;

using LootQuest.Utils;
using LootQuest.Game.Spells;
using LootQuest.Game.Spells.SpellEffects;

namespace LootQuest.GameData
{
	public class SpellEffectEntry
	{
		public string titleID = "rename me";
		public int level;

		public LootQuest.Game.Spells.SpellEffects.SpellEffectID handler;
		public LootQuest.Game.Status.EffectID triggeredStatus;

		public Range<float> power = new Range<float>();
		public Range<float> triggerChance = new Range<float>();

		public GameObject view = null;
		public GameObject projectileView = null;
		
		public Range<float> range = new Range<float> ();
		public Range<float> power = new Range<float> ();
		public Range<float> duration = new Range<float>();
		public Range<float> projectileCount = new Range<float> ();
		public Range<float> projectileScale = new Range<float> ();
		public Range<float> maxStacks = new Range<float>();

		public SpellEffectEntry()
		{}
	}
}