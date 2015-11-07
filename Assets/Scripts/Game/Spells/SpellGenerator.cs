using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Spells
{
	public class SpellGenerator : Utils.Singleton<SpellGenerator>
	{
		public SpellData Generate(string spell)
		{
			SpellData data = new SpellData ();
			var entry = GameData.Spells.Instance.GetEntry (spell);

			data.cooldown = Random.Range (entry.cooldown.Min, entry.cooldown.Max);
			data.entry = entry;

			foreach (var effID in entry.effects) 
			{
				var effEntry = GameData.Effects.Instance.GetEntry(effID);
				if (effEntry == null) continue;

				var effData = new SpellEffectData();
				effData.entry = effEntry;
				effData.effectPower = Random.Range (effEntry.power.Min, effEntry.power.Max);
				effData.triggerChance = Random.Range (effEntry.triggerChance.Min, effEntry.triggerChance.Max);

				data.effects.Add (effData);
			}

			return data;
		}
	}
}