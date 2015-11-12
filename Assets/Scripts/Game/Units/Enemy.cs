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
        private double expGain_;
        public double ExpGain
        {
            get { return expGain_;  }
        }
		public Enemy()
		{

		}

		public void Init(GameData.EnemyEntry entry)
		{
			View = entry.viewPrefab.GetPooled ().GetComponent<View.UnitView>();
            expGain_ = entry.exp;

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

		public void ScaleStatsWithTower(Tower tower)
		{
            LevelManager.SetLevel(tower.Level);

			var health = stats_.Get (LootQuest.Game.Attributes.AttributeID.Health);
            health.SetMultiplier(tower.Entry.EnemyHpMultiplier);

			var damage = stats_.Get (LootQuest.Game.Attributes.AttributeID.Damage);
			damage.SetMultiplier(tower.Entry.EnemyHpMultiplier);
		}
	}
}