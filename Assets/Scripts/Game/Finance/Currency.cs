using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LootQuest.Game.Finance
{
	public enum CurrencyID
	{
		Gold
	}

	public class Currency
	{
		private Dictionary<CurrencyID, int> values_;

		public static Currency operator +(Currency left, Currency right) 
		{
			int value = 0;
			foreach(CurrencyID cur in Enum.GetValues(typeof(CurrencyID)))
			{
				if (right.values_.TryGetValue(cur, out value))
				{
					left.values_[cur] += value;
				}
			}

			return left;
		}

		public static Currency operator -(Currency left, Currency right) 
		{
			int value = 0;
			foreach(CurrencyID cur in Enum.GetValues(typeof(CurrencyID)))
			{
				if (right.values_.TryGetValue(cur, out value))
				{
					left.values_[cur] -= value;
				}
			}

			return left;
		}

		public Currency()
		{
			values_ = new Dictionary<CurrencyID, int> ();
		}
	}
}