using UnityEngine;
using System.Collections;

namespace LootQuest {
	public class Scene : MonoBehaviour 
	{
		public virtual void OnFinish()
		{
			gameObject.SetActive (false);
		}

		public virtual void OnStart()
		{
			gameObject.SetActive (true);
		}
	}
}