using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace LootQuest.Game.Spells
{
	public class SpellData
	{
		public GameData.SpellEntry entry;
		public List<Game.Spells.SpellEffectData> effects = new List<SpellEffectData>();
		
		public float cooldown;

		public SpellData()
		{}

		public SpellData(GameData.SpellEntry entry)
		{

		}
	}
}