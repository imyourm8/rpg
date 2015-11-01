using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Spells
{
	public class SpellEffectData
	{
		public string titleID;
		public int level;
		
		public LootQuest.Game.Spells.SpellEffects.SpellEffectID handler;
		public LootQuest.Game.Status.EffectID triggeredStatus;
		
		public float effectPower;
		public float triggerChance;
	}
}