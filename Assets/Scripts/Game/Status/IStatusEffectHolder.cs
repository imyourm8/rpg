using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Status {
	public interface IStatusEffectHolder<TStatusID>
	{
		IStatusEffectManager<TStatusID> Manager
		{ set; }
		
		void Add(IStatusEffect<TStatusID> effect);
	}
}