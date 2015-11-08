using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Spells
{
	public static class SpellExtensions
	{
		private static Utils.ObjectPool.ObjectPoolGeneric<Spell> spells_ = new Utils.ObjectPool.ObjectPoolGeneric<Spell> ();

		public static void Init()
		{
			spells_.Init (10, 0, false);
		}

		public static Spell Create()
		{
			return spells_.Get();
		}

		public static void Return(this Spell spell)
		{
			spells_.Return (spell);
		}
	}
}