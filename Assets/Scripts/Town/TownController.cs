using UnityEngine;
using System.Collections;

namespace LootQuest.Town {
	public class TownController : Scene 
	{
		[SerializeField]
		private Game.GameController game_;

		public void GoToFight()
		{
			SceneManager.Instance.GoTo (SceneManager.Scenes.BATTLE);
		}
	}
}