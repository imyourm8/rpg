using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LootQuest.Utils.ObjectPool 
{
	public class ObjectPoolPrefab : ObjectPoolGeneric<GameObject>
	{
		[SerializeField]
		private GameObject prefab_ = null;
		
		public ObjectPoolPrefab()
		{}
		
		public ObjectPoolPrefab(GameObject prefab)
		{
			prefab_ = prefab;
		}
		
		protected override void OnObjectGetPoped(GameObject obj)
		{
			obj.SetActive(true);
		}
		
		protected override void OnObjectGetReturned(GameObject obj)
		{
			obj.SetActive(false);
		}
		
		protected override GameObject CreateObject()
        {
			var obj = GameObject.Instantiate (prefab_);
			obj.SetActive (false);
			return obj;
        }
    }
}