using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Finance
{
	public class Wallet
	{
		private Currency currency_;

		public Wallet()
		{
			currency_ = new Currency ();
		}

		public void Withdraw()
		{}

		public void Store()
		{}
	}
}