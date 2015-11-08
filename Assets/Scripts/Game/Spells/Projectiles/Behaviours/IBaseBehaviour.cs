using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Spells.Projectiles.Behaviours
{
	public interface IBaseBehaviour
	{
		void Prepare ();	
		void Update ();
	}
}