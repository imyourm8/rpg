﻿using UnityEngine;

using System;
using System.Collections;

using LootQuest.Game.Units;

namespace LootQuest.Game.Modes {
	public class TowerFloor : GameController 
	{
		[SerializeField]
		private GameObject heroPrefab;

		[SerializeField]
		private EnemySpawner spawner_;

		[SerializeField]
		private GameObject battlefield_;

		[SerializeField]
		private GameObject landPrefab_;

		private Hero hero_;
        private Tower tower_;

		protected override void UpdateLogic ()
		{
			base.UpdateLogic ();
		}

		protected override void Prepare ()
		{
			base.Prepare ();
			/*
			var left = battlefield_.transform.InverseTransformPoint(gameCamBounds.min);
			var right = battlefield_.transform.InverseTransformPoint(gameCamBounds.max);

			var obj = Instantiate<GameObject> (landPrefab_);
			obj.transform.SetParent(battlefield_.transform, false);

			obj.transform.localPosition = new Vector3 (left.x, 0, 0);
			var renderer = obj.GetComponent<SpriteRenderer> ();
			*/
			LoadTower ();

			var hero = new Units.Hero ();
			hero.Direction = Entity.ToRight;
			hero.Init (GameData.Heroes.Instance.Data);
			Add (hero);
			hero.GroupID = 1;
			hero.Move ();
			hero_ = hero;

			GameCamera.GetComponent<PlayerCameraFollower> ().SetTarget (hero.View);

            Rules = new GameRules.ExpGameRules();
		}

		private void LoadTower()
		{
            var towerLevel = 1;
            var towerEntry = GameData.Towers.Instance.GetTower(towerLevel);

            tower_ = new Tower();
            tower_.Init(towerLevel, towerEntry);

            spawner_.Init(tower_);

			StartCoroutine (StartSpawn ());    
		}

		IEnumerator StartSpawn()
		{
			yield return new WaitForSeconds(1.0f);
            spawner_.Start();
		}

		protected override void OnEntityRemoved (Entity entity)
		{
			base.OnEntityRemoved (entity);

			Enemy enemy = entity as Enemy;
            if (enemy != null && !enemy.IsAlive())
            {
                spawner_.OnEnemyKilled(enemy);
                Rules.OnEnemyKilled(enemy, hero_);
            }
		}
	}
}