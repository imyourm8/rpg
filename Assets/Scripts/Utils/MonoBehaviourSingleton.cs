﻿using UnityEngine;
using System.Collections;

namespace LootQuest.Utils {
	public class SingletonMonobehaviour<T> : MonoBehaviour 
	{
		protected static T instance_;
	
		public static T Instance {
			get { return instance_; }
			protected set { instance_ = value; }
		}
	}
}