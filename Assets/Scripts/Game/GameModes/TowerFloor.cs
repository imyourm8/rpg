using UnityEngine;
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

		protected override void UpdateLogic ()
		{
			base.UpdateLogic ();
		}

		protected override void Prepare ()
		{
			base.Prepare ();

			var gameCamBounds = Utils.CameraUtils.OrthographicBounds(GameCamera);
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
			hero.Direction = 1;
			hero.Init (GameData.Heroes.Instance.Data);
			Add (hero);
			hero.GroupID = 1;

			GameCamera.GetComponent<PlayerCameraFollower> ().SetTarget (hero.View);

			var enemyEntry = GameData.Enemies.Instance.Data [0];
			var enemy = UnitExtensions.CreateEnemy ();
			enemy.Direction = -1;
			enemy.Init (enemyEntry);
			Add (enemy);

			enemy.X += 300;
		}

		private void LoadTower()
		{

		}
	}
}