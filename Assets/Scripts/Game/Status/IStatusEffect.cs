using UnityEngine;
using System.Collections;
using Retlang.Fibers;

namespace LootQuest.Game.Status 
{
	public interface IStatusEffect<TID>
	{
		TID ID { get; }
	}
}