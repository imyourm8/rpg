using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace LootQuest.Game.Spells
{
	public class SpellData
	{
		public int level = 1;
		public string ID;
		public string titleID;
		public List<Game.Spells.SpellEffectData> effects;
		
		public Game.Spells.SlotStype slot;
		public Game.Spells.SpellType type;
		
		public LootQuest.Game.Spells.SpellTargetStrategy targetStrategy;
		public LootQuest.Game.Spells.SpellTargetFilter targetFilter;
		
		public float cooldown;
	}
}