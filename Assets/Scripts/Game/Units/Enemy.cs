using UnityEngine;
using System.Collections;

namespace LootQuest.Game.Units 
{
	public enum EnemyType
	{
		Normal,
		Rare,
		Unique,
		Boss
	}

	public class Enemy : Unit 
	{
		public Enemy()
		{

		}

		public void Init(GameData.EnemyEntry entry)
		{
			View = entry.viewPrefab.GetPooled ().GetComponent<View.UnitView>();

			foreach (var stat in entry.Stats) 
			{
				stat.Generate(stats_.Get(stat.id));
			}

			AI = Factory.AIFactory.Instance.Create (entry.ai);

			spells_.Clear ();


			if (entry.autoAttackAbility.Length > 0) 
			{
				autoAttackAbility_ = entry.autoAttackAbility;
				LoadSpell(entry.autoAttackAbility);
			}
		}

		public void ScaleStatsWithTower(GameData.TowerEntry tower)
		{

		}
	}
}