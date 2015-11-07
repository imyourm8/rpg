using UnityEngine;
using System.Collections;

namespace LootQuest
{
	public class Application : MonoBehaviour 
	{
		void Start () 
		{
			UnitExtensions.InitPool ();

			Game.Spells.Spell.Init ();

			GameData.Heroes.Instance.Load ();
			GameData.Enemies.Instance.Load ();
			GameData.EnemyTables.Instance.Load ();
			GameData.Effects.Instance.Load ();
			GameData.Spells.Instance.Load ();
			GameData.Towers.Instance.Load ();

			Game.Account.Instance.LoadAll ();
		}
	}
}