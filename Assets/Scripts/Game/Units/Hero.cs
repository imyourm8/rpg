using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LootQuest.Game.Units 
{
	public class Hero : Unit 
	{
		public void Init(GameData.HeroEntry entry)
		{
			View = entry.viewPrefab.GetPooled ().GetComponent<View.UnitView>();
			
			foreach (var stat in entry.Stats) 
			{
				stats_.Get(stat.ID).SetValue(stat.Value);
			}

			stats_.Get (LootQuest.Game.Attributes.AttributeID.AttackRange).SetValue (-1.0f);

			AI = Factory.AIFactory.Instance.Create (LootQuest.Game.Units.AI.Type.Basic);
		}
	}
}