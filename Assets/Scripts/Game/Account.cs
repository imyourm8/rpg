using UnityEngine;
using System.Collections;

namespace LootQuest.Game 
{
	public class Account : Utils.Singleton<Account>
	{
		private Finance.Wallet wallet_;
		private Units.Hero hero_;

		public void LoadAll()
		{
			wallet_ = new LootQuest.Game.Finance.Wallet ();

			hero_ = new LootQuest.Game.Units.Hero ();
		}

		public void SaveWallet()
		{}
	}
}