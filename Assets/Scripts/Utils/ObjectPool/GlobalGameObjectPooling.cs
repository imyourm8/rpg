using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LootQuest.Utils.ObjectPool 
{
	public class GlobalGameObjectPooling : SingletonMonobehaviour<GlobalGameObjectPooling>
	{
		private Dictionary<GameObject, IObjectPool<GameObject>> pools_;
		private Dictionary<GameObject, GameObject> objectsByPrefabs_;
	
		void Start()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(this);
				
				pools_ = new Dictionary<GameObject, IObjectPool<GameObject>>();
				objectsByPrefabs_ = new Dictionary<GameObject, GameObject>();
			} else 
			{
				Destroy(this);
			}
		}
		
		private IObjectPool<GameObject> GetPool(GameObject obj)
		{
			IObjectPool<GameObject> pool; 

			if (!pools_.TryGetValue(obj, out pool))
			{
				pool = new ObjectPoolPrefab(obj);
				pool.Init(1, 0, false);
				pools_.Add(obj, pool);
			}
			
			return pool;
		}
		
		public void Return(GameObject obj)
		{
			if (!objectsByPrefabs_.ContainsKey(obj))
			{
				throw new UnityException("Can't return example object!");
			}
			var prefab = objectsByPrefabs_ [obj];
			GetPool(prefab).Return(obj);
			objectsByPrefabs_.Remove (obj);
		}
		
		public GameObject Get(GameObject obj)
		{
			var pool = GetPool (obj);
			var pooledObj = pool.Get ();
			objectsByPrefabs_.Add (pooledObj, obj);
			return pooledObj;
		}
	}
}

static public class GlobalGameObjectPoolingExtensions
{
	public static GameObject GetPooled(this GameObject obj)
	{
		return LootQuest.Utils.ObjectPool.GlobalGameObjectPooling.Instance.Get(obj);
	}
	
	public static void ReturnPooled(this GameObject obj)
	{
		LootQuest.Utils.ObjectPool.GlobalGameObjectPooling.Instance.Return(obj);
	}
}