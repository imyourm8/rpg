using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Status {
	public enum UnitStatus
	{
		NONE
	}

	public class UnitStatusEffect : IStatusEffect<UnitStatus>
	{
		public UnitStatusEffect()
		{
			
		}

		#region IStatusEffect implementation

		UnitStatus IStatusEffect<UnitStatus>.ID {
			get {
				throw new System.NotImplementedException ();
			}
		}

		#endregion
	}
}