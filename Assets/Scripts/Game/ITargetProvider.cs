using UnityEngine;
using System.Collections;

namespace LootQuest.Game
{
	public interface ITargetProvider
	{
		void InitTargets();
		bool NextTarget(out Game.Units.Entity target);
	}
}