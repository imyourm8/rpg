using UnityEngine;
using System.Collections;

namespace LootQuest.Game {
	public interface IEntityControl
	{
		void Add(Units.Entity entity);
		void Remove(Units.Entity entity);
	}
}