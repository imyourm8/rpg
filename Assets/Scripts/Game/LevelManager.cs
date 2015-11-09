using UnityEngine;

using System;
using System.Collections;

namespace LootQuest.Game
{
	public class LevelManager
	{
		private double exp_ = 0.0;
		private int level_ = 1;

		public static double GetExpForLevel(int level)
		{
			return Math.Pow(75.0 * 1.0901, (double)level) + 100.0;
		}
		
		public static double GetMobLevel(int heroLevel, int mobLevel)
		{
			double expNeedForMobLevel = GetExpForLevel (mobLevel);
			return expNeedForMobLevel / (10.0 + (double)heroLevel * 3.0);
		}

		public int Level
		{
			get { return level_; }
		}

		public void SetLevel(int level)
		{
			level_ = level;
		}

		public void AddExp(double exp)
		{
			double expForLevel = 0;
			exp_ += exp;

			do {
				expForLevel = GetExpForLevel (level_ + 1);

				if (exp_ > expForLevel) {
					level_ ++;
					exp_ -= expForLevel;
				}
			} while (exp_ > expForLevel);
		}
	}
}