using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Status {
	public class StatusEffectHolder<TStatusID> : IStatusEffectHolder<TStatusID>
	{
		#region IStatusEffectHolder implementation
		void IStatusEffectHolder<TStatusID>.Add (IStatusEffect<TStatusID> effect)
		{
			throw new System.NotImplementedException ();
		}
		IStatusEffectManager<TStatusID> IStatusEffectHolder<TStatusID>.Manager {
			set {
				throw new System.NotImplementedException ();
			}
		}
		#endregion
	}
}