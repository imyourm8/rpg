using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using LootQuest.Utils;
using LootQuest.GameData;
using LootQuest.Game.Units;

namespace LootQuest.Game 
{
	public class EnemySpawner : MonoBehaviour 
	{
		[SerializeField]
		private Camera gameCam_;

		[SerializeField]
		private GameController game_;

		[SerializeField]
		[Range(0.0f, 1.0f)]
		private float spawnOffset_ = 0.1f;

		[SerializeField]
		private float spawnPointFluctuation_ = 10.0f;

		[SerializeField]
		private float direction_ = -1.0f;

		private int enemiesSpawned_ = 0;
		private EnemyTableEntry table_;
		private int groupsSpawned_ = 0;
		private int groupsToBeSpawned_ = 0;
		private Tower tower_;

		public void Init(Tower tower)
		{
			tower_ = tower;

            //pick enemy table for spawning by level
            //if no table that lower by level not found - pick the highest lvl table
            EnemyTableEntry maxLvlTable = null;
            int highestTableLvl = 0;
            int level = tower.Level;
            foreach (var table in tower.Entry.EnemyTables)
            {
                if (table.maxTowerLevel <= level)
                {
                    table_ = table;
                    break;
                }

                if (highestTableLvl <= level)
                {
                    maxLvlTable = table;
                    highestTableLvl = level;
                }
            }

            if (maxLvlTable != null)
            {
                table_ = maxLvlTable;
            }

			groupsToBeSpawned_ = table_.groupCount.Randomize ();
			groupsSpawned_ = 0;
		}

        public void Start()
        {
            SpawnNextGroup();
        }

		public void OnEnemyKilled(Enemy enemy)
		{
			enemy.Return ();
			enemiesSpawned_--;
			if (enemiesSpawned_ == 0) 
			{
				SpawnNextGroup();
			}
		}

		private void SpawnNextGroup()
		{
			if (groupsSpawned_ == groupsToBeSpawned_)
				return;

			if (enemiesSpawned_ > 0)
				return;
		
			var camBounds = Utils.CameraUtils.OrthographicBounds (gameCam_);

			enemiesSpawned_ = table_.groupSize.Randomize ();
			for (int i = 0; i < enemiesSpawned_; ++i) 
			{
				//pick enemy type first
				var typeRoll = Random.Range(0, table_.TotalSpawnChanceWeight);
				EnemyType enemyType = EnemyType.Normal;
				int j = 0;
				while (j < table_.spawnChances.Count && 
				       (table_.spawnChances[j].realWeight == 0 ||
				       table_.spawnChances[j].realWeight < typeRoll))
				{
					j++;
				}
				enemyType = table_.spawnChances[j].enemyType;

				//now pick enemy from enemy list
				j = 0;
				var enemyList = table_.enemies[enemyType].enemies;
				float enemyRoll = Random.Range(0, table_.enemies[enemyType].totalWeight);
				EnemyEntry enemyEntry = null;
				while (j < enemyList.Count && 
				       (enemyList[j].realWeight.Equals(0.0f) || 
				       enemyList[j].realWeight < enemyRoll))
				{
					j++;
				}

				enemyEntry = enemyList[j].enemy;

				if (enemyEntry != null)
				{
					var enemy = UnitExtensions.CreateEnemy(enemyEntry);
					enemy.Direction = Entity.ToRight.Scale(direction_);

					float spawnPosition = Random.Range(0.0f, spawnPointFluctuation_);
					enemy.X = camBounds.max.x + camBounds.size.x*spawnOffset_ + spawnPosition;

					enemy.ScaleStatsWithTower(tower_);

					game_.Add (enemy);
				}
			}
		}
	}
}