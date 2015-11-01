using UnityEngine;
using System;
using System.Collections;

namespace LootQuest.Game {
	public interface IActionScheduler
	{
		IDisposable DelayAction(Action action, long delay);
		void Execute(Action action);
	}
}