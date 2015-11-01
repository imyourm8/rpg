using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LootQuest {
	public class SceneManager : Utils.SingletonMonobehaviour<SceneManager> 
	{
		public enum Scenes
		{
			TOWN,
			BATTLE
		}

		[Serializable]
		public struct ScenePair
		{
			public Scene controller;
			public Scenes id;
		}

		[SerializeField]
		private ScenePair[] scenesToRegister;

		[SerializeField]
		private Scenes startScene;

		private Dictionary<Scenes, Scene> scenes_;
		private Scene currentScene_;

		void Awake()
		{
			if (instance_ != null) 
			{
				Destroy(this);
				return;
			}

			DontDestroyOnLoad (this);
			instance_ = this;

			scenes_ = new Dictionary<Scenes, Scene> ();

			foreach (var pair in scenesToRegister) 
			{
				scenes_.Add(pair.id, pair.controller);
			}

			GoTo (startScene);
		}

		public void GoTo(Scenes sceneID)
		{
			var scene = scenes_ [sceneID];

			if (currentScene_ != null) 
			{
				currentScene_.OnFinish();
			}

			scene.OnStart ();
			currentScene_ = scene;
		}
	}
}