using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LootQuest.Utils.ObjectPool 
{
	public class GlobalGameObjectPooling : SingletonMonobehaviour<GlobalGameObjectPooling>
	{
		private Dictionary<GameObject, IObjectPool<GameObject>> pools_;
	
		void Start()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(this);
				
				pools_ = new Dictionary<GameObject, IObjectPool<GameObject>>();
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
			if (pools_.ContainsKey(obj))
			{
				throw new UnityException("Can't return example object!");
			}
			
			GetPool(obj).Return(obj);
		}
		
		public GameObject Get(GameObject obj)
		{
			return GetPool(obj).Get();
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