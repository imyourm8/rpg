using UnityEngine;
using System;
using System.Collections;

namespace LootQuest.Game {
	public class EnemySpawner : MonoBehaviour 
	{
		[System.Serializable]
		public class SpawnEntry
		{
			public float weight;
			public GameObject prefab;
		}
		
		[SerializeField]
		private SpawnEntry[] spawnList_;
	
		void Start () 
		{
		    StartCoroutine(Spawn(spawnList_[0].prefab));
		}
		
		void Update () 
		{
			
		}
        
        IEnumerator Spawn(GameObject prefab)
        {
            while (true)
            {
                yield return new WaitForSeconds(2.0f);
            }
        }
	}
}