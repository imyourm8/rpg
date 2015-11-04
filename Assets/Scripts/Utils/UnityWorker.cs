using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace LootQuest.Utils {
	public class UnityWorker : SingletonMonobehaviour<UnityWorker>
	{
		private List<Action> works_;
		private List<Action> nextWorks_;
		private object lock_;

		void Start()
		{
			if (Instance != null)
			{
				Instance = this;
				DontDestroyOnLoad(this);

				works_ = new List<Action>();
				nextWorks_ = new List<Action>();
				lock_ = new object();
			} else 
			{
				Destroy(this);
			}
		}

		public void Add(Action work)
		{
			lock (lock_) 
			{
				nextWorks_.Add(work);
			}
		}

		void Update()
		{
			lock (lock_) 
			{
				var temp = nextWorks_;
				nextWorks_ = works_;
				works_ = temp;
			}

			while(works_.Count > 0)
			{
				//TODO use deque for this purpose
			}
		}
	}
}