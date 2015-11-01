using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Status 
{
	public class StatusEffectManager<TStatusID> : IStatusEffectManager<TStatusID>
	{
		#region IStatusEffectManager implementation
		void IStatusEffectManager<TStatusID>.Add (IStatusEffectHolder<TStatusID> holder)
		{
			throw new System.NotImplementedException ();
		}
		#endregion
	}
}