using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Status {
	public interface IStatusEffectManager<TStatusID>
	{
		void Add(IStatusEffectHolder<TStatusID> holder);
	}
}