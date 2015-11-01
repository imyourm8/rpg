using UnityEngine;
using System.Collections;

namespace LootQuest.Utils.ObjectPool {
	public interface IObjectPool<T>
	{
		T Get();
		void Return(T obj);
		void Init(int initialSize, int maxSize, bool isFixed);
	}
}